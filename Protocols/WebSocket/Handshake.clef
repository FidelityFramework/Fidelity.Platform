/// WebSocket Handshake Implementation
/// RFC 6455 compliant HTTP upgrade handshake with pure F# SHA-1 and Base64
namespace Fidelity.Platform.WebSocket

open Fidelity.Platform.Bindings

/// Pure F# SHA-1 implementation (FIPS 180-4)
module SHA1 =
    /// Left rotate 32-bit word
    let inline private rotl (n: int) (x: uint32) = (x <<< n) ||| (x >>> (32 - n))

    /// Initial hash values (FIPS 180-4)
    let private h0Init = 0x67452301u
    let private h1Init = 0xEFCDAB89u
    let private h2Init = 0x98BADCFEu
    let private h3Init = 0x10325476u
    let private h4Init = 0xC3D2E1F0u

    /// Round constants
    let private k0 = 0x5A827999u  // rounds 0-19
    let private k1 = 0x6ED9EBA1u  // rounds 20-39
    let private k2 = 0x8F1BBCDCu  // rounds 40-59
    let private k3 = 0xCA62C1D6u  // rounds 60-79

    /// Process a single 512-bit (64-byte) block
    let private processBlock (block: byte[]) (blockOffset: int) (h: uint32[]) =
        // Message schedule array
        let w = Array.zeroCreate<uint32> 80

        // Copy block into first 16 words (big-endian)
        for i in 0..15 do
            let j = blockOffset + i * 4
            w.[i] <- (uint32 block.[j] <<< 24) |||
                     (uint32 block.[j+1] <<< 16) |||
                     (uint32 block.[j+2] <<< 8) |||
                     (uint32 block.[j+3])

        // Extend to 80 words
        for i in 16..79 do
            w.[i] <- rotl 1 (w.[i-3] ^^^ w.[i-8] ^^^ w.[i-14] ^^^ w.[i-16])

        // Working variables
        let mutable a = h.[0]
        let mutable b = h.[1]
        let mutable c = h.[2]
        let mutable d = h.[3]
        let mutable e = h.[4]

        // 80 rounds
        for i in 0..79 do
            let f, k =
                if i < 20 then
                    ((b &&& c) ||| ((~~~b) &&& d), k0)
                elif i < 40 then
                    (b ^^^ c ^^^ d, k1)
                elif i < 60 then
                    ((b &&& c) ||| (b &&& d) ||| (c &&& d), k2)
                else
                    (b ^^^ c ^^^ d, k3)

            let temp = (rotl 5 a) + f + e + k + w.[i]
            e <- d
            d <- c
            c <- rotl 30 b
            b <- a
            a <- temp

        // Add to hash
        h.[0] <- h.[0] + a
        h.[1] <- h.[1] + b
        h.[2] <- h.[2] + c
        h.[3] <- h.[3] + d
        h.[4] <- h.[4] + e

    /// Compute SHA-1 hash of byte array, returns 20-byte hash
    let hash (data: byte[]) : byte[] =
        let h = [| h0Init; h1Init; h2Init; h3Init; h4Init |]
        let dataLen = data.Length

        // Pad message: append 1 bit, zeros, then 64-bit length (big-endian)
        // Total padded length must be multiple of 64 bytes
        let padLen =
            let rem = (dataLen + 9) % 64
            if rem = 0 then dataLen + 9 else dataLen + 9 + (64 - rem)

        let padded = Array.zeroCreate<byte> padLen
        Array.blit data 0 padded 0 dataLen
        padded.[dataLen] <- 0x80uy  // append bit '1'

        // Append original length in bits (big-endian, 64-bit)
        let bitLen = uint64 dataLen * 8UL
        padded.[padLen - 8] <- byte (bitLen >>> 56)
        padded.[padLen - 7] <- byte (bitLen >>> 48)
        padded.[padLen - 6] <- byte (bitLen >>> 40)
        padded.[padLen - 5] <- byte (bitLen >>> 32)
        padded.[padLen - 4] <- byte (bitLen >>> 24)
        padded.[padLen - 3] <- byte (bitLen >>> 16)
        padded.[padLen - 2] <- byte (bitLen >>> 8)
        padded.[padLen - 1] <- byte bitLen

        // Process each 64-byte block
        for i in 0 .. (padLen / 64 - 1) do
            processBlock padded (i * 64) h

        // Output hash (big-endian)
        let result = Array.zeroCreate<byte> 20
        for i in 0..4 do
            result.[i*4]   <- byte (h.[i] >>> 24)
            result.[i*4+1] <- byte (h.[i] >>> 16)
            result.[i*4+2] <- byte (h.[i] >>> 8)
            result.[i*4+3] <- byte h.[i]
        result

/// Pure F# Base64 implementation
module Base64 =
    let private alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"

    /// Encode byte array to Base64 string
    let encode (data: byte[]) : string =
        let len = data.Length
        if len = 0 then "" else

        let outLen = ((len + 2) / 3) * 4
        let result = Array.zeroCreate<char> outLen
        let mutable ri = 0
        let mutable i = 0

        while i + 2 < len do
            let b0 = int data.[i]
            let b1 = int data.[i+1]
            let b2 = int data.[i+2]
            result.[ri]   <- alphabet.[b0 >>> 2]
            result.[ri+1] <- alphabet.[((b0 &&& 0x03) <<< 4) ||| (b1 >>> 4)]
            result.[ri+2] <- alphabet.[((b1 &&& 0x0F) <<< 2) ||| (b2 >>> 6)]
            result.[ri+3] <- alphabet.[b2 &&& 0x3F]
            ri <- ri + 4
            i <- i + 3

        // Handle remaining bytes
        let rem = len - i
        if rem = 1 then
            let b0 = int data.[i]
            result.[ri]   <- alphabet.[b0 >>> 2]
            result.[ri+1] <- alphabet.[(b0 &&& 0x03) <<< 4]
            result.[ri+2] <- '='
            result.[ri+3] <- '='
        elif rem = 2 then
            let b0 = int data.[i]
            let b1 = int data.[i+1]
            result.[ri]   <- alphabet.[b0 >>> 2]
            result.[ri+1] <- alphabet.[((b0 &&& 0x03) <<< 4) ||| (b1 >>> 4)]
            result.[ri+2] <- alphabet.[(b1 &&& 0x0F) <<< 2]
            result.[ri+3] <- '='

        System.String(result)

/// WebSocket handshake implementation
module Handshake =
    /// RFC 6455 magic GUID for Sec-WebSocket-Accept computation
    let private magicGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"

    /// Compute Sec-WebSocket-Accept value from client key
    let computeAcceptKey (clientKey: string) : string =
        let combined = clientKey + magicGuid
        let combinedBytes = System.Text.Encoding.ASCII.GetBytes(combined)
        let hash = SHA1.hash combinedBytes
        Base64.encode hash

    /// HTTP response template for successful WebSocket upgrade
    let private responseTemplate acceptKey =
        "HTTP/1.1 101 Switching Protocols\r\n" +
        "Upgrade: websocket\r\n" +
        "Connection: Upgrade\r\n" +
        "Sec-WebSocket-Accept: " + acceptKey + "\r\n" +
        "\r\n"

    /// Parse Sec-WebSocket-Key from HTTP request headers
    /// Returns None if not found or invalid
    let parseWebSocketKey (request: string) : string option =
        // Simple line-by-line parsing
        let lines = request.Split([| "\r\n"; "\n" |], System.StringSplitOptions.RemoveEmptyEntries)
        lines
        |> Array.tryFind (fun line ->
            line.StartsWith("Sec-WebSocket-Key:", System.StringComparison.OrdinalIgnoreCase))
        |> Option.map (fun line ->
            line.Substring(18).Trim())

    /// Generate HTTP 101 response for WebSocket upgrade
    let generateUpgradeResponse (clientKey: string) : byte[] =
        let acceptKey = computeAcceptKey clientKey
        let response = responseTemplate acceptKey
        System.Text.Encoding.ASCII.GetBytes(response)

    /// Validate WebSocket upgrade request
    /// Returns Some(key) if valid, None if invalid
    let validateUpgradeRequest (request: string) : string option =
        // Must be GET request
        if not (request.StartsWith("GET ")) then None
        // Must have Upgrade: websocket header
        elif not (request.Contains("Upgrade: websocket") || request.Contains("Upgrade:websocket")) then None
        // Must have Connection: Upgrade header
        elif not (request.Contains("Connection: Upgrade") || request.Contains("Connection:Upgrade") ||
                  request.Contains("connection: upgrade") || request.Contains("Connection: upgrade")) then None
        else
            parseWebSocketKey request
