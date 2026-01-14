/// WebSocket Types for WREN Stack
/// RFC 6455 compliant WebSocket protocol types
namespace Fidelity.Platform.WebSocket

/// WebSocket frame opcode (RFC 6455 Section 5.2)
type Opcode =
    | Continuation = 0x0
    | Text = 0x1
    | Binary = 0x2
    | Close = 0x8
    | Ping = 0x9
    | Pong = 0xA

/// WebSocket close status codes (RFC 6455 Section 7.4)
module CloseCode =
    [<Literal>]
    let NormalClosure = 1000us
    [<Literal>]
    let GoingAway = 1001us
    [<Literal>]
    let ProtocolError = 1002us
    [<Literal>]
    let UnsupportedData = 1003us
    [<Literal>]
    let InvalidPayload = 1007us
    [<Literal>]
    let PolicyViolation = 1008us
    [<Literal>]
    let MessageTooBig = 1009us
    [<Literal>]
    let InternalError = 1011us

/// Parsed WebSocket frame header
[<Struct>]
type FrameHeader = {
    Fin: bool
    Opcode: Opcode
    Masked: bool
    PayloadLength: uint64
    MaskKey: uint32
    HeaderSize: int  // Total header size in bytes (2-14)
}

/// WebSocket connection state
type ConnectionState =
    | Connecting
    | Open
    | Closing
    | Closed

/// Message received from Face (WebView)
type HostMessage =
    | CloseRequest of code: uint16 * reason: string
    | TextMessage of text: string
    | BinaryMessage of data: nativeptr<byte> * length: int
    | PingMessage of data: nativeptr<byte> * length: int
    | PongMessage of data: nativeptr<byte> * length: int
    | Error of message: string

/// WebSocket server state
[<Struct>]
type WebSocketServer = {
    ListenFd: int
    ClientFd: int
    Port: uint16
    State: ConnectionState
}

/// Buffer for accumulating frame data
[<Struct>]
type FrameBuffer = {
    Data: nativeptr<byte>
    Capacity: int
    Length: int
}

/// Socket address structure for IPv4 (sockaddr_in)
/// Layout: family (2) + port (2) + addr (4) + zero (8) = 16 bytes
module SockAddrIn =
    [<Literal>]
    let Size = 16

    [<Literal>]
    let AF_INET = 2us

    /// Build sockaddr_in in a stack buffer
    /// port should be in network byte order (use htons)
    /// addr 0 means INADDR_ANY (bind to all interfaces)
    let inline offsetFamily = 0
    let inline offsetPort = 2
    let inline offsetAddr = 4
