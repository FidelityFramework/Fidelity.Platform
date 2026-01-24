# Platform Role

## Context

Fidelity.Platform provides low-level OS abstractions and platform-specific bindings for Fidelity applications.

## Decision

Fidelity.Platform is the **foundation layer** that all other Fidelity libraries build upon:

```
Fidelity.Desktop / Fidelity.WebView / Fidelity.Signal
                        │
                        ▼
               Fidelity.Platform
                        │
                        ▼
              OS Syscalls / FFI
```

## Responsibilities

1. **Syscall wrappers**: `Sys.write`, `Sys.read`, `Sys.socket`, etc.
2. **Platform detection**: OS family, architecture
3. **GTK bindings**: Window, widget, event loop via quotations
4. **WebKitGTK bindings**: WebView widget integration
5. **WebSocket server**: Full WebSocket protocol implementation
6. **Threading primitives**: Thread spawn, join, mutex (future)

## Structure

```
Fidelity.Platform/
├── Linux_x86_64/
│   ├── Types.fs        # Platform-specific type sizes
│   ├── Syscalls.fs     # Linux syscall numbers
│   ├── Platform.fs     # Platform initialization
│   ├── Console.fs      # Console I/O
│   ├── Bindings/
│   │   ├── GTKConduits.fs    # GTK quotation bindings
│   │   ├── WebViewAPI.fs     # WebView abstraction
│   │   └── Sockets.fs        # Socket operations
│   └── WebSocket/
│       ├── Server.fs         # WebSocket server
│       ├── Frame.fs          # Frame encoding
│       └── Handshake.fs      # HTTP upgrade
├── Windows_x86_64/     # Future
└── MacOS_x86_64/       # Future
```

## Related

- `quotation_binding_pattern` memory
- `gtk_webkit_integration` memory
- `/home/hhh/repos/Firefly/docs/Platform_Binding_Model.md`
