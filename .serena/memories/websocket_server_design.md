# WebSocket Server Design

## Context

Fidelity.Platform includes a native WebSocket server implementation for WRENStack IPC.

## Decision

Implement WebSocket protocol natively without external dependencies:

```
Fidelity.Platform/Linux_x86_64/WebSocket/
├── Types.fs      # Frame types, opcodes, close codes
├── Handshake.fs  # HTTP upgrade handshake
├── Frame.fs      # Frame encoding/decoding
└── Server.fs     # Server loop, connection handling
```

## Protocol Implementation

### Handshake (RFC 6455)

```fsharp
// Client sends HTTP upgrade request
// Server responds with:
// HTTP/1.1 101 Switching Protocols
// Upgrade: websocket
// Connection: Upgrade
// Sec-WebSocket-Accept: <base64(SHA1(key + magic))>
```

### Frame Format

```
 0                   1                   2                   3
 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
+-+-+-+-+-------+-+-------------+-------------------------------+
|F|R|R|R| opcode|M| Payload len |    Extended payload length    |
|I|S|S|S|  (4)  |A|     (7)     |             (16/64)           |
|N|V|V|V|       |S|             |   (if payload len==126/127)   |
| |1|2|3|       |K|             |                               |
+-+-+-+-+-------+-+-------------+ - - - - - - - - - - - - - - - +
|     Extended payload length continued, if payload len == 127  |
+ - - - - - - - - - - - - - - - +-------------------------------+
|                               |Masking-key, if MASK set to 1  |
+-------------------------------+-------------------------------+
| Masking-key (continued)       |          Payload Data         |
+-------------------------------- - - - - - - - - - - - - - - - +
:                     Payload Data continued ...                :
+ - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
|                     Payload Data continued ...                |
+---------------------------------------------------------------+
```

### Opcodes

| Opcode | Meaning |
|--------|---------|
| 0x0 | Continuation |
| 0x1 | Text |
| 0x2 | Binary |
| 0x8 | Close |
| 0x9 | Ping |
| 0xA | Pong |

## Server Loop

```fsharp
let serve (port: int) (handler: Frame -> Frame option) =
    let sock = Sys.socket AF_INET SOCK_STREAM 0
    Sys.bind sock addr port
    Sys.listen sock 5
    while true do
        let client = Sys.accept sock
        if WebSocket.handshake client then
            while true do
                let frame = Frame.read client
                match handler frame with
                | Some response -> Frame.write client response
                | None -> ()
```

## Related

- `platform_role` memory
- `/home/hhh/repos/BAREWire/.serena/memories/wren_stack_integration.md`
