# Virtio

Reserved for shared feature/status rules, queue layouts, device protocols and
transport implementations. No virtio implementation is included in this migration.

Planned ownership:

- Common and virtqueue declarations: protocol semantics and BAREWire layouts.
- `Transports/MMIO`: register offsets, transaction requirements and sequencing.
- `Transports/PCI`: PCI discovery/capability transport requirements.
- Device protocols such as `Net` and `Block`: device-specific configuration and
  queue operations over the selected transport.

Machine packages supply actual register windows, interrupts and discovery
contracts. A driver additionally needs device-visible memory, publication
barriers, buffer ownership and completion handling. Volatile MMIO alone does
not implement those requirements. The
[OASIS virtio specification](https://docs.oasis-open.org/virtio/virtio/v1.3/virtio-v1.3.html)
is the protocol authority; a chosen implementation must pin its supported
version and features.
