/// WebSocket Frame Implementation
/// RFC 6455 compliant frame parsing and building
namespace Fidelity.Platform.WebSocket

open Fidelity.Platform.Bindings

/// WebSocket frame operations
module Frame =
    // ===================================================================
    // Frame Header Parsing (RFC 6455 Section 5.2)
    // ===================================================================

    /// Parse frame header from buffer
    /// Returns None if insufficient data
    let parseHeader (buffer: nativeptr<byte>) (length: int) : FrameHeader option =
        if length < 2 then None
        else
            let b0 = NativePtr.read buffer
            let b1 = NativePtr.get buffer 1

            let fin = (b0 &&& 0x80uy) <> 0uy
            let opcode = enum<Opcode>(int (b0 &&& 0x0Fuy))
            let masked = (b1 &&& 0x80uy) <> 0uy
            let lenByte = b1 &&& 0x7Fuy

            // Determine payload length and header size
            let payloadLen, headerSize =
                if lenByte < 126uy then
                    uint64 lenByte, 2
                elif lenByte = 126uy then
                    if length < 4 then
                        0UL, -1  // Insufficient data marker
                    else
                        // 16-bit length (big-endian)
                        let len = (uint64 (NativePtr.get buffer 2) <<< 8) |||
                                  uint64 (NativePtr.get buffer 3)
                        len, 4
                else  // lenByte = 127
                    if length < 10 then
                        0UL, -1  // Insufficient data marker
                    else
                        // 64-bit length (big-endian)
                        let len = (uint64 (NativePtr.get buffer 2) <<< 56) |||
                                  (uint64 (NativePtr.get buffer 3) <<< 48) |||
                                  (uint64 (NativePtr.get buffer 4) <<< 40) |||
                                  (uint64 (NativePtr.get buffer 5) <<< 32) |||
                                  (uint64 (NativePtr.get buffer 6) <<< 24) |||
                                  (uint64 (NativePtr.get buffer 7) <<< 16) |||
                                  (uint64 (NativePtr.get buffer 8) <<< 8) |||
                                  uint64 (NativePtr.get buffer 9)
                        len, 10

            if headerSize < 0 then None
            else
                // Add 4 bytes for mask if present
                let totalHeaderSize = if masked then headerSize + 4 else headerSize

                if length < totalHeaderSize then None
                else
                    // Extract mask key if present (big-endian)
                    let maskKey =
                        if masked then
                            let maskOffset = headerSize
                            (uint32 (NativePtr.get buffer maskOffset) <<< 24) |||
                            (uint32 (NativePtr.get buffer (maskOffset + 1)) <<< 16) |||
                            (uint32 (NativePtr.get buffer (maskOffset + 2)) <<< 8) |||
                            uint32 (NativePtr.get buffer (maskOffset + 3))
                        else
                            0u

                    Some {
                        Fin = fin
                        Opcode = opcode
                        Masked = masked
                        PayloadLength = payloadLen
                        MaskKey = maskKey
                        HeaderSize = totalHeaderSize
                    }

    // ===================================================================
    // Masking Operations (RFC 6455 Section 5.3)
    // ===================================================================

    /// Apply XOR mask to payload data (in-place)
    /// Used for both masking and unmasking (XOR is symmetric)
    let applyMask (data: nativeptr<byte>) (length: int) (maskKey: uint32) : unit =
        let mask0 = byte ((maskKey >>> 24) &&& 0xFFu)
        let mask1 = byte ((maskKey >>> 16) &&& 0xFFu)
        let mask2 = byte ((maskKey >>> 8) &&& 0xFFu)
        let mask3 = byte (maskKey &&& 0xFFu)

        let mutable i = 0
        while i < length do
            let maskByte =
                match i % 4 with
                | 0 -> mask0
                | 1 -> mask1
                | 2 -> mask2
                | _ -> mask3
            let current = NativePtr.get data i
            NativePtr.set data i (current ^^^ maskByte)
            i <- i + 1

    /// Unmask payload data (alias for applyMask - XOR is symmetric)
    let unmask (data: nativeptr<byte>) (length: int) (maskKey: uint32) : unit =
        applyMask data length maskKey

    // ===================================================================
    // Frame Building
    // ===================================================================

    /// Calculate required header size for a payload
    let headerSizeForPayload (payloadLength: uint64) (masked: bool) : int =
        let baseSize =
            if payloadLength < 126UL then 2
            elif payloadLength <= 65535UL then 4
            else 10
        if masked then baseSize + 4 else baseSize

    /// Build frame header into buffer
    /// Returns number of bytes written
    let buildHeader (buffer: nativeptr<byte>) (fin: bool) (opcode: Opcode)
                    (payloadLength: uint64) (maskKey: uint32 option) : int =
        // First byte: FIN + opcode
        let b0 = (if fin then 0x80uy else 0uy) ||| byte (int opcode)
        NativePtr.write buffer b0

        // Determine if masked
        let masked = Option.isSome maskKey
        let maskBit = if masked then 0x80uy else 0uy

        // Second byte + extended length
        let mutable offset = 2
        if payloadLength < 126UL then
            NativePtr.set buffer 1 (maskBit ||| byte payloadLength)
        elif payloadLength <= 65535UL then
            NativePtr.set buffer 1 (maskBit ||| 126uy)
            NativePtr.set buffer 2 (byte (payloadLength >>> 8))
            NativePtr.set buffer 3 (byte payloadLength)
            offset <- 4
        else
            NativePtr.set buffer 1 (maskBit ||| 127uy)
            NativePtr.set buffer 2 (byte (payloadLength >>> 56))
            NativePtr.set buffer 3 (byte (payloadLength >>> 48))
            NativePtr.set buffer 4 (byte (payloadLength >>> 40))
            NativePtr.set buffer 5 (byte (payloadLength >>> 32))
            NativePtr.set buffer 6 (byte (payloadLength >>> 24))
            NativePtr.set buffer 7 (byte (payloadLength >>> 16))
            NativePtr.set buffer 8 (byte (payloadLength >>> 8))
            NativePtr.set buffer 9 (byte payloadLength)
            offset <- 10

        // Write mask key if present
        match maskKey with
        | Some key ->
            NativePtr.set buffer offset (byte (key >>> 24))
            NativePtr.set buffer (offset + 1) (byte (key >>> 16))
            NativePtr.set buffer (offset + 2) (byte (key >>> 8))
            NativePtr.set buffer (offset + 3) (byte key)
            offset + 4
        | None ->
            offset

    // ===================================================================
    // Frame Utilities
    // ===================================================================

    /// Check if opcode is a control frame (Close, Ping, Pong)
    let isControlFrame (opcode: Opcode) : bool =
        match opcode with
        | Opcode.Close | Opcode.Ping | Opcode.Pong -> true
        | _ -> false

    /// Check if opcode is a data frame (Text, Binary, Continuation)
    let isDataFrame (opcode: Opcode) : bool =
        match opcode with
        | Opcode.Text | Opcode.Binary | Opcode.Continuation -> true
        | _ -> false

    /// Parse close frame payload for status code and reason
    /// Returns (statusCode, reasonBytes, reasonLength)
    let parseClosePayload (payload: nativeptr<byte>) (length: int) : uint16 * nativeptr<byte> * int =
        if length < 2 then
            (CloseCode.NormalClosure, payload, 0)
        else
            let code = (uint16 (NativePtr.read payload) <<< 8) |||
                       uint16 (NativePtr.get payload 1)
            let reasonPtr = NativePtr.add payload 2
            let reasonLen = length - 2
            (code, reasonPtr, reasonLen)

    /// Build close frame payload
    /// Returns number of bytes written
    let buildClosePayload (buffer: nativeptr<byte>) (code: uint16)
                          (reason: nativeptr<byte>) (reasonLength: int) : int =
        // Write status code (big-endian)
        NativePtr.write buffer (byte (code >>> 8))
        NativePtr.set buffer 1 (byte code)

        // Copy reason if provided
        if reasonLength > 0 && not (NativePtr.isNullPtr reason) then
            let mutable i = 0
            while i < reasonLength do
                NativePtr.set buffer (2 + i) (NativePtr.get reason i)
                i <- i + 1
            2 + reasonLength
        else
            2

    /// Build a complete text frame (unmasked, for server-to-client)
    /// Caller must ensure buffer has space for header + payload
    /// Returns total frame size
    let buildTextFrame (buffer: nativeptr<byte>) (text: nativeptr<byte>) (textLength: int) : int =
        let payloadLen = uint64 textLength
        let headerSize = buildHeader buffer true Opcode.Text payloadLen None

        // Copy text payload
        let mutable i = 0
        while i < textLength do
            NativePtr.set buffer (headerSize + i) (NativePtr.get text i)
            i <- i + 1

        headerSize + textLength

    /// Build a complete binary frame (unmasked, for server-to-client)
    let buildBinaryFrame (buffer: nativeptr<byte>) (data: nativeptr<byte>) (dataLength: int) : int =
        let payloadLen = uint64 dataLength
        let headerSize = buildHeader buffer true Opcode.Binary payloadLen None

        // Copy binary payload
        let mutable i = 0
        while i < dataLength do
            NativePtr.set buffer (headerSize + i) (NativePtr.get data i)
            i <- i + 1

        headerSize + dataLength

    /// Build a pong frame (response to ping)
    let buildPongFrame (buffer: nativeptr<byte>) (pingData: nativeptr<byte>) (dataLength: int) : int =
        let payloadLen = uint64 dataLength
        let headerSize = buildHeader buffer true Opcode.Pong payloadLen None

        // Copy ping data as pong payload (RFC 6455 requirement)
        let mutable i = 0
        while i < dataLength do
            NativePtr.set buffer (headerSize + i) (NativePtr.get pingData i)
            i <- i + 1

        headerSize + dataLength

    /// Build a close frame
    let buildCloseFrame (buffer: nativeptr<byte>) (code: uint16) : int =
        // Close frame with just status code (no reason)
        let headerSize = buildHeader buffer true Opcode.Close 2UL None
        NativePtr.set buffer headerSize (byte (code >>> 8))
        NativePtr.set buffer (headerSize + 1) (byte code)
        headerSize + 2

