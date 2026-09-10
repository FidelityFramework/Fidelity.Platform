# Protocols

This branch owns reusable communication formats and sequencing requirements.
Concrete silicon, product and virtual-machine packages supply instances and
resources; execution bindings supply the mechanisms used to access them.

The generic MMIO region/mapping/grant vocabulary remains in
[Contracts](../Contracts/DeviceAccess.clef). [Virtio](Virtio/README.md) is reserved
here for shared queue/device semantics and separate transport implementations.
