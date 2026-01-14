/// WebSocket Server Implementation
/// RFC 6455 compliant WebSocket server for WREN Stack
namespace Fidelity.Platform.WebSocket

open Fidelity.Platform.Bindings
open Fidelity.Platform.Types

/// WebSocket server operations
module Server =
    // ===================================================================
    // Constants
    // ===================================================================

    /// AF_INET for IPv4
    [<Literal>]
    let private AF_INET = 2

    /// SOCK_STREAM for TCP
    [<Literal>]
    let private SOCK_STREAM = 1

    /// SOL_SOCKET for socket-level options
    [<Literal>]
    let private SOL_SOCKET = 1

    /// SO_REUSEADDR option
    [<Literal>]
    let private SO_REUSEADDR = 2

    /// Maximum HTTP request size for handshake
    [<Literal>]
    let private MaxHandshakeSize = 4096

    /// Maximum WebSocket frame size we'll handle
    [<Literal>]
    let private MaxFrameSize = 65536

    /// Receive buffer size
    [<Literal>]
    let private RecvBufferSize = 8192

    // ===================================================================
    // sockaddr_in Building
    // ===================================================================

    /// Build sockaddr_in structure for binding
    /// Layout: family (2) + port (2) + addr (4) + zero (8) = 16 bytes
    let private buildSockAddrIn (buffer: nativeptr<byte>) (port: uint16) : unit =
        // Family: AF_INET (2) - little-endian on x86
        NativePtr.write buffer (byte AF_INET)
        NativePtr.set buffer 1 0uy

        // Port: network byte order (big-endian)
        NativePtr.set buffer 2 (byte (port >>> 8))
        NativePtr.set buffer 3 (byte port)

        // Address: INADDR_ANY (0.0.0.0)
        NativePtr.set buffer 4 0uy
        NativePtr.set buffer 5 0uy
        NativePtr.set buffer 6 0uy
        NativePtr.set buffer 7 0uy

        // Zero padding (8 bytes)
        let mutable i = 8
        while i < 16 do
            NativePtr.set buffer i 0uy
            i <- i + 1

    // ===================================================================
    // Server Lifecycle
    // ===================================================================

    /// Create and start a WebSocket server on the given port
    /// Returns server state with listening socket
    let start (port: uint16) : WebSocketServer =
        // Create TCP socket
        let listenFd = Sockets.socket AF_INET SOCK_STREAM 0
        if listenFd < 0 then
            { ListenFd = -1; ClientFd = -1; Port = port; State = Closed }
        else
            // Set SO_REUSEADDR to allow quick restart
            let stackBuffer = NativePtr.stackalloc<byte> 4
            NativePtr.write stackBuffer 1uy  // optval = 1
            NativePtr.set stackBuffer 1 0uy
            NativePtr.set stackBuffer 2 0uy
            NativePtr.set stackBuffer 3 0uy
            let _ = Sockets.setsockopt listenFd SOL_SOCKET SO_REUSEADDR stackBuffer 4

            // Build sockaddr_in and bind
            let addrBuffer = NativePtr.stackalloc<byte> 16
            buildSockAddrIn addrBuffer port

            let bindResult = Sockets.bind listenFd addrBuffer 16
            if bindResult < 0 then
                let _ = Sockets.close listenFd
                { ListenFd = -1; ClientFd = -1; Port = port; State = Closed }
            else
                // Start listening with backlog of 1 (single client for now)
                let listenResult = Sockets.listen listenFd 1
                if listenResult < 0 then
                    let _ = Sockets.close listenFd
                    { ListenFd = -1; ClientFd = -1; Port = port; State = Closed }
                else
                    { ListenFd = listenFd; ClientFd = -1; Port = port; State = Connecting }

    /// Accept a client connection and perform WebSocket handshake
    /// Blocks until a client connects
    let acceptClient (server: WebSocketServer) : WebSocketServer =
        if server.ListenFd < 0 then
            { server with State = Closed }
        else
            // Accept connection (blocking)
            let clientFd = Sockets.accept server.ListenFd (NativePtr.nullPtr<byte>()) (NativePtr.nullPtr<int>())
            if clientFd < 0 then
                { server with State = Closed }
            else
                // Read HTTP upgrade request
                let requestBuffer = NativePtr.stackalloc<byte> MaxHandshakeSize
                let bytesRead = Sockets.recv clientFd requestBuffer MaxHandshakeSize 0

                if bytesRead <= 0 then
                    let _ = Sockets.close clientFd
                    { server with State = Closed }
                else
                    // Convert to string for handshake parsing
                    // Note: This is a simplified string conversion - assumes ASCII
                    let requestChars = Array.zeroCreate<char> bytesRead
                    let mutable i = 0
                    while i < bytesRead do
                        requestChars.[i] <- char (NativePtr.get requestBuffer i)
                        i <- i + 1
                    let request = System.String(requestChars)

                    // Validate upgrade request and get client key
                    match Handshake.validateUpgradeRequest request with
                    | None ->
                        // Invalid request - close connection
                        let _ = Sockets.close clientFd
                        { server with State = Closed }
                    | Some clientKey ->
                        // Generate and send upgrade response
                        let responseBytes = Handshake.generateUpgradeResponse clientKey
                        let responseLen = responseBytes.Length

                        // Copy response to native buffer
                        let responseBuffer = NativePtr.stackalloc<byte> responseLen
                        let mutable j = 0
                        while j < responseLen do
                            NativePtr.set responseBuffer j responseBytes.[j]
                            j <- j + 1

                        let bytesSent = Sockets.send clientFd responseBuffer responseLen 0
                        if bytesSent < 0 then
                            let _ = Sockets.close clientFd
                            { server with State = Closed }
                        else
                            { server with ClientFd = clientFd; State = Open }

    /// Close the WebSocket server
    let close (server: WebSocketServer) : WebSocketServer =
        if server.ClientFd >= 0 then
            // Send close frame
            let closeBuffer = NativePtr.stackalloc<byte> 4
            let frameLen = Frame.buildCloseFrame closeBuffer CloseCode.NormalClosure
            let _ = Sockets.send server.ClientFd closeBuffer frameLen 0
            let _ = Sockets.shutdown server.ClientFd 2  // SHUT_RDWR
            let _ = Sockets.close server.ClientFd
            ()

        if server.ListenFd >= 0 then
            let _ = Sockets.close server.ListenFd
            ()

        { server with ListenFd = -1; ClientFd = -1; State = Closed }

    // ===================================================================
    // Message Receiving
    // ===================================================================

    /// Receive and parse next WebSocket message
    /// Returns None on error or connection close
    let receiveMessage (server: WebSocketServer) : HostMessage option =
        if server.ClientFd < 0 || server.State <> Open then
            None
        else
            // Receive data
            let recvBuffer = NativePtr.stackalloc<byte> RecvBufferSize
            let bytesRead = Sockets.recv server.ClientFd recvBuffer RecvBufferSize 0

            if bytesRead <= 0 then
                // Connection closed or error
                Some (Error "Connection closed")
            else
                // Parse frame header
                match Frame.parseHeader recvBuffer bytesRead with
                | None ->
                    Some (Error "Invalid frame header")
                | Some header ->
                    let totalFrameSize = header.HeaderSize + int header.PayloadLength
                    if totalFrameSize > bytesRead then
                        // TODO: Handle fragmented receives
                        Some (Error "Incomplete frame received")
                    else
                        // Get payload pointer
                        let payloadPtr = NativePtr.add recvBuffer header.HeaderSize
                        let payloadLen = int header.PayloadLength

                        // Unmask payload if masked (client-to-server frames MUST be masked)
                        if header.Masked then
                            Frame.unmask payloadPtr payloadLen header.MaskKey

                        // Handle based on opcode
                        match header.Opcode with
                        | Opcode.Text ->
                            // Convert payload to string
                            let textChars = Array.zeroCreate<char> payloadLen
                            let mutable i = 0
                            while i < payloadLen do
                                textChars.[i] <- char (NativePtr.get payloadPtr i)
                                i <- i + 1
                            Some (TextMessage (System.String(textChars)))

                        | Opcode.Binary ->
                            Some (BinaryMessage (payloadPtr, payloadLen))

                        | Opcode.Close ->
                            let code, reasonPtr, reasonLen = Frame.parseClosePayload payloadPtr payloadLen
                            // Convert reason to string
                            let reasonChars = Array.zeroCreate<char> reasonLen
                            let mutable i = 0
                            while i < reasonLen do
                                reasonChars.[i] <- char (NativePtr.get reasonPtr i)
                                i <- i + 1
                            Some (CloseRequest (code, System.String(reasonChars)))

                        | Opcode.Ping ->
                            // Auto-respond with Pong
                            let pongBuffer = NativePtr.stackalloc<byte> (14 + payloadLen)
                            let pongLen = Frame.buildPongFrame pongBuffer payloadPtr payloadLen
                            let _ = Sockets.send server.ClientFd pongBuffer pongLen 0
                            Some (PingMessage (payloadPtr, payloadLen))

                        | Opcode.Pong ->
                            Some (PongMessage (payloadPtr, payloadLen))

                        | Opcode.Continuation ->
                            // TODO: Handle fragmented messages
                            Some (Error "Fragmented messages not yet supported")

                        | _ ->
                            Some (Error "Unknown opcode")

    // ===================================================================
    // Message Sending
    // ===================================================================

    /// Send a text message to the client
    let sendText (server: WebSocketServer) (text: string) : bool =
        if server.ClientFd < 0 || server.State <> Open then
            false
        else
            let textLen = text.Length
            let frameBuffer = NativePtr.stackalloc<byte> (14 + textLen)

            // Copy string to native buffer
            let textBuffer = NativePtr.stackalloc<byte> textLen
            let mutable i = 0
            while i < textLen do
                NativePtr.set textBuffer i (byte text.[i])
                i <- i + 1

            let frameLen = Frame.buildTextFrame frameBuffer textBuffer textLen
            let bytesSent = Sockets.send server.ClientFd frameBuffer frameLen 0
            bytesSent > 0

    /// Send a binary message to the client
    let sendBinary (server: WebSocketServer) (data: nativeptr<byte>) (length: int) : bool =
        if server.ClientFd < 0 || server.State <> Open then
            false
        else
            let frameBuffer = NativePtr.stackalloc<byte> (14 + length)
            let frameLen = Frame.buildBinaryFrame frameBuffer data length
            let bytesSent = Sockets.send server.ClientFd frameBuffer frameLen 0
            bytesSent > 0

    /// Send a close frame and initiate graceful shutdown
    let sendClose (server: WebSocketServer) (code: uint16) : bool =
        if server.ClientFd < 0 then
            false
        else
            let closeBuffer = NativePtr.stackalloc<byte> 4
            let frameLen = Frame.buildCloseFrame closeBuffer code
            let bytesSent = Sockets.send server.ClientFd closeBuffer frameLen 0
            bytesSent > 0

    // ===================================================================
    // Server Loop (for threading)
    // ===================================================================

    /// Simple server loop that receives messages and calls a handler
    /// This is designed to run on a dedicated thread
    /// messageHandler should use g_idle_add to dispatch to GTK main loop
    let runLoop (server: WebSocketServer) (messageHandler: HostMessage -> unit) : unit =
        let mutable running = true
        let mutable currentServer = server

        while running && currentServer.State = Open do
            match receiveMessage currentServer with
            | None ->
                running <- false
            | Some msg ->
                messageHandler msg
                match msg with
                | CloseRequest _ | Error _ ->
                    running <- false
                | _ -> ()

