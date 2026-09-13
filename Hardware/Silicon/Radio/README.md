# Radio hardware

Documentation scaffold, 2026-09-13. This branch reserves an owner for discrete
radio silicon and indexes radio blocks already owned by MCU packages. It has
no executable package, register declarations or accepted radio driver.

Future standalone parts belong under `Radio/<vendor>/<family>/<part>/` when a
concrete device is selected. Integrated radios retain their existing MCU owner:
the ESP32-S3 must not become a second physical device or acquire a second copy
of its shared memory budget here. Modules, antennas, wiring and population
belong to products; protocol sequencing belongs to
[Protocols/Radio](../../../Protocols/Radio/README.md).
A programmable radio companion such as Meadow's ESP32 also retains an MCU
part owner; its use as a radio does not change its silicon identity.

## Boards reviewed

| Product | Physical radio capability | Fidelity workload status |
| --- | --- | --- |
| [CCC2026Badge](../../Products/CircuitBoardMedics/CCC2026Badge/README.md), using [ESP32-S3-WROOM-1-N8](../MCU/Espressif/ESP32S3/ESP32_S3_WROOM_1_N8/README.md) | 2.4 GHz Wi-Fi 802.11b/g/n and Bluetooth 5 LE, with the WROOM-1 PCB antenna. No Bluetooth Classic/BR/EDR or LoRa transceiver. | HelloESP does not initialize or exercise Wi-Fi/BLE. Hardware availability does not establish native driver support. |
| [STM32H747I-DISCO](../../Products/ST/STM32H747I_DISCO/README.md), MB1248 | No onboard Wi-Fi, Bluetooth or LoRa radio. STMod+/Pmod/Arduino connections offer expansion routes. | HelloDISCO uses no radio. Any wireless capability requires an additional selected device and a new resource/driver profile. |
| [Meadow F7 Feather](../../Products/WildernessLabs/MeadowF7/README.md) | STM32F7 application processor plus an ESP32 radio coprocessor for Wi-Fi/Bluetooth; exact owned board revisions and available firmware roles remain to confirm. | Product scaffold only. Approximately five user-owned boards are future consumers later in 2026. |

The badge's [silicon readback](../../Products/CircuitBoardMedics/CCC2026Badge/docs/BADGE_HARDWARE.md#read-from-this-boards-silicon-2026-09-11)
identifies Wi-Fi and BT 5 LE. Espressif's
[WROOM datasheet](https://www.espressif.com/sites/default/files/documentation/esp32-s3-wroom-1_wroom-1u_datasheet_en.pdf)
§1.1–1.2 confirms the radio types and module antenna. The S3
[controller documentation](https://docs.espressif.com/projects/esp-idf/en/stable/esp32s3/api-reference/bluetooth/controller_vhci.html)
excludes Classic and dual-mode Bluetooth. Its
[coexistence guide](https://docs.espressif.com/projects/esp-idf/en/stable/esp32s3/api-guides/coexist.html)
describes shared RF use by Wi-Fi and BLE; they are not two independently
available radios. A profile must budget their combined activity.

The DISCO inventory comes from the retained
[UM2411 Rev 7](../../Products/ST/STM32H747I_DISCO/docs/um2411-discovery-kit-with-stm32h747xi-mcu-stmicroelectronics.pdf),
its features/kit inventory and STMod+ section, and the design packs indexed by
the [product source audit](../../Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md).
MB1280 is the supplied STMod+ fan-out board. Its C-01 schematic labels CN4 an
**ESP-01 connector**, but its matching BOM contains no radio module. A socket
is an expansion provision, not installed Wi-Fi. Check the selected module,
power budget, bus routing and shared pins before defining an expansion profile.
The live [ST manual](https://www.st.com/resource/en/user_manual/um2411-discovery-kit-with-stm32h747xi-mcu-stmicroelectronics.pdf)
is Rev 8 at this review; the retained Rev 7 bytes and hashes have not changed.

Wilderness Labs' [hardware reference](https://developer.wildernesslabs.co/Hardware/Reference/Meadow_Hardware/Meadow_F7/)
documents Meadow's composite architecture. Its
[F7v2 reference](https://developer.wildernesslabs.co/Hardware/Reference/Meadow_Hardware/Meadow_F7/F7v2/)
names an ESP32-PICO-D4, a different part from the badge's ESP32-S3. Those vendor
references do not identify the user's individual units. Keep part capability,
coprocessor firmware/API support and an eventual Fidelity backend separate.

## What to record when selecting a radio

Record the exact part/module and antenna, supported bands and PHYs, host
interface, reset/power/clock dependencies, shared resources and firmware
requirements. Distinguish hardware capability, available vendor software,
selected implementation and observed behavior. A family name or Bluetooth
version alone does not select roles, profiles or every optional feature.

LoRa is a future radio category. A selected transceiver and its packet interface
would live in the hardware inventory; LoRaWAN, if wanted, is an additional
protocol selection. Neither current board supplies a LoRa transceiver.

See the [radio model](../../../docs/RADIO_MODEL.md) for backend choices,
BAREWire boundaries and the language-readiness handoff.
