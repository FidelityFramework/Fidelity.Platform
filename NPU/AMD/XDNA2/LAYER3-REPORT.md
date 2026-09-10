# Layer 2 Marshaling Report: Fidelity.XRT.Bridge

Historical generator inventory, retained for regeneration comparison. The
[bridge manifest](Fidelity.XRT.Bridge.fidproj) selects legacy callback wrappers
with raw-address types. This report is not current-Clef compatibility, device
execution or NPU-kernel acceptance. The generic NativePtr/dlsym notes below do
not establish which operations these particular wrappers perform.

## Generated

### Callback Wrappers
- Registration wrappers: 1
- Listener struct builders: 0

## Notes
- Interface globals resolved via Fidelity.Libc.DynamicLink.dlsym
- NativeInterop.NativePtr used for argument array writes
