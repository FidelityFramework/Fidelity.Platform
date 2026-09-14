# Protocols

This branch owns reusable communication formats and sequencing requirements.
Concrete silicon, product and virtual-machine packages supply instances and
resources; execution bindings supply the mechanisms used to access them.

The generic MMIO region/mapping/grant vocabulary remains in
[Contracts](../Contracts/DeviceAccess.clef). [Virtio](Virtio/README.md) is reserved
here for shared queue/device semantics and separate transport implementations.

[WebSocket](WebSocket/README.md) holds RFC 6455 frame, handshake and type source
shared by every native environment; the socket, TLS and run loop are environment
bindings, the first being the Linux x86_64 server under `Environments/`.

[Radio](Radio/README.md) is a documentation scaffold for Bluetooth, Wi-Fi and
future LoRa/LoRaWAN work. It separates reusable protocol behavior from radio
hardware, host/foreign bindings and the application's model and messages.
