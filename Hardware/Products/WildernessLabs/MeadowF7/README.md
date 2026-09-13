# Wilderness Labs Meadow F7 product scaffold

Wilderness Labs owns the product identity; the selected MCU family references
the [ST STM32F7 scaffold](../../../Silicon/MCU/ST/STM32F7/README.md).
[Platform.clef](Platform.clef) retains empty endpoint collections and no core
description. This package is an inventory scaffold, not a working freestanding
target or an accepted board bring-up.

Hardware revision applicability remains unconfirmed. The staged vendor page
describes F7v2, but that document identity does not establish the physical
board revision or populated MCU. No revision directory is invented. The
[source manifest](docs/SOURCE_MANIFEST.md) distinguishes available evidence
from the missing routing and startup information.

## Future composite radio and power target

The user has approximately five Meadow Feather boards intended for projects
later in 2026. This is a future consumer of the
[Radio model](../../../../docs/RADIO_MODEL.md), alongside the integrated radio
in the HelloESP badge and optional radio expansion on DISCO.

The [vendor hardware reference](https://developer.wildernesslabs.co/Hardware/Reference/Meadow_Hardware/Meadow_F7/)
describes an STM32F7 application processor and an ESP32 coprocessor. The
interprocessor transport and coprocessor firmware are explicit boundaries for
shared Bluetooth/Wi-Fi behavior. Keep each processor's part identity, memory,
reset and execution responsibilities distinct. A composite product need not
select two Clef compilation targets if the radio uses retained vendor firmware.

Battery charging uses separate board circuitry. System sleep/wake must account
for processor and radio state, power rails and retained RTC state; it is a
product lifecycle concern alongside Radio. The official
[V2.B Feather schematic](https://github.com/WildernessLabs/Meadow_Hardware_Designs/blob/main/Meadow_F7v2/Feather_Dev_Module/PCA_Meadow_F7_Micro_SCH_V2b.pdf)
identifies an STM32F777IIKx, ESP32-PICO-D4 and MCP73831T-2A charger. This online
reference has not been added as a hashed local artifact and does not identify
the user's boards. The staged STM32F750 datasheet remains an unconfirmed
candidate, not an established Meadow part.

Before a future implementation, identify one unit's revision and population,
reconcile its schematic and bus/reset/power routes, record the selected ESP32
firmware and actual supported roles, then choose one bounded consumer. The
[Meadow Bluetooth API](https://developer.wildernesslabs.co/Meadow/Meadow.OS/Bluetooth/)
documents a BLE server surface; it is not a Fidelity implementation or evidence
that every Bluetooth capability of the underlying chip is exposed.
