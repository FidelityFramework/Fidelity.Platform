# Layer 2 Marshaling Report: Fidelity.Wayland.Bridge

## Generated

### Protocol Dispatch (226 requests)
- Constructors: 23
- Destructors: 24
- Void requests: 179

### Callback Wrappers
- Registration wrappers: 3
- Listener struct builders: 22

## Unmapped — Developer Review Required

### Unpaired Constructors
These interfaces have constructors but no explicit destroy request.
The developer must determine lifecycle management:
- `wl_callback`
- `wl_registry`
- `wl_shell_surface`

## Notes
- Protocol dispatch uses Fidelity.Libc.Memory for argument arrays (malloc/free)
- Interface globals resolved via Fidelity.Libc.DynamicLink.dlsym
- NativeInterop.NativePtr used for argument array writes
