# Radio protocols

Documentation scaffold, 2026-09-13. `Radio` groups wireless communication
families without requiring one common connection API. There are no selectable
drivers or protocol implementations in this directory yet.

| Family | Responsibilities to outline | Separate dependency |
| --- | --- | --- |
| Bluetooth | Selected LE/Classic capability; controller HCI transport; GAP discovery/connection roles; L2CAP, ATT/GATT and security sequencing as required | Actual controller and firmware; a GATT service's application meaning; key custody and durable bond storage |
| Wi-Fi | Selected station/AP roles, scan/join/leave, authentication, link events and bounded frame transfer | Radio/backend; IP addressing, UDP/TCP and application protocols above the link |
| LoRa | Selected PHY parameters and bounded packet send/receive behavior | Concrete transceiver, antenna and channel plan; LoRaWAN only when that network protocol is selected |

The future subdirectory names `Bluetooth`, `WiFi` and `LoRa` can be introduced
as work acquires sources and a consumer. They are organizational names, not
frozen Clef namespaces. LoRaWAN must remain explicit within any LoRa protocol
work: LoRa is the physical modulation layer, while LoRaWAN adds network
protocol and system behavior. See [Semtech's description](https://www.semtech.com/lora/lorawan-standard).

[Hardware/Silicon/Radio](../../Hardware/Silicon/Radio/README.md) owns the radio
inventory and future discrete parts. Existing MCU packages retain integrated
radio facts. Products own physical instances and routes; environments own
host/foreign bindings; profiles select resources and budgets. Device-specific
command protocols stay with their device backend, while reusable wire formats
and state transitions belong here. General IP networking is shared with wired
links and should not become Wi-Fi-specific code.

The [radio model](../../docs/RADIO_MODEL.md) records how the supplied
BluetoothModule example can inform commands, events and bounded payloads.
BAREWire can describe the actual wire layout and owned buffer views; it does
not replace Bluetooth, Wi-Fi or LoRaWAN framing with a new private format.

This scaffold selects neither a C binding nor a native stack. No profile gains
radio access merely because the physical MCU contains a radio.
