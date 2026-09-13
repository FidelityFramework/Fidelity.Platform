# CCC2026Badge

Carolina Code Conference 2026 conference badge, designed by
[Circuit Board Medics](https://circuitboardmedics.com). An ESP32-S3-WROOM-1-N8
module with a 160 × 128 ST7735S panel, five WS2812 LEDs, three buttons, native
USB, and unused GPIOs on solder pads.

[docs/BADGE_HARDWARE.md](docs/BADGE_HARDWARE.md) is the wiring reference and the
evidence record. [Description.clef](Description.clef) declares the signal
assignments.

## Status

HelloESP compiled through Composer and CCS and ran on this badge on 2026-09-12,
including its display, five LEDs, three buttons and 1 kHz interrupt. See the
[hardware acceptance record](../../../../docs/ESP32S3_BRINGUP.md#10-helloesp-runs-2026-09-12).
That establishes the selected workload; unused peripheral declarations remain
outside its acceptance.

| Piece | State |
| --- | --- |
| Wiring declarations | Selected HelloESP routes exercised; documentation, silkscreen and CircuitPython sources remain the provenance |
| [Silicon memory/register declarations](../../../Silicon/MCU/Espressif/ESP32S3/ESP32_S3_WROOM_1_N8) | Written; 111 registers generated from Espressif's SVD |
| Execution environment and profile | [HelloESP selection](../../../../Profiles/CCC2026Badge_HelloESP/README.md), consumed by the compiled workload |
| Composer image backend and Xtensa codegen | Executed for HelloESP; see the dated acceptance record for the actual toolchain and image |

## Radio capability

The ESP32-S3-WROOM-1-N8 contains 2.4 GHz Wi-Fi 802.11b/g/n and Bluetooth 5 LE
with a PCB antenna. It has no Bluetooth Classic or LoRa transceiver. HelloESP
does not initialize or exercise Wi-Fi/BLE. The
[radio inventory](../../../Silicon/Radio/README.md) records primary sources,
shared RF use and the distinction between available hardware and a Fidelity
driver. The [radio model](../../../../docs/RADIO_MODEL.md) leaves backend/API
implementation until its language and resource requirements are ready.

## Initial toolchain investigation (historical)

The following investigation predates the accepted HelloESP workload. Its
options and machine-tool observations describe that initial state; §10 of the
linked bring-up record establishes the later execution result.

This part is a dual-core **Xtensa LX7**, and Xtensa is the obstacle.

The system LLVM (22.1.8) registers no Xtensa target at all. Upstream LLVM does
carry an Xtensa backend, but its `XtensaProcessors.td` defines only `esp32`,
`esp8266` and `esp32s2` — **there is no `esp32s3` CPU model upstream**. The
`esp32s3` processor definition, with its `FeatureESP32S3Ops`, exists in
Espressif's fork (`espressif/llvm-project`, released as `esp-clang`).

That reads like a hard dependency on Espressif's LLVM, but `esp32s3` is a named
bundle of features rather than a backend, and **26 of its 29 features are
already upstream**. Only `ESP32S3Ops` (S3-specific DSP/PIE instructions, which a
UI unikernel does not emit) and two interrupt/timer parameterizations are
missing. `llc` accepts `-mattr`, so the part can be described by its features.

The likely answer is therefore to build upstream LLVM 22.1.8 with
`LLVM_EXPERIMENTAL_TARGETS_TO_BUILD=Xtensa` and select the S3 by flags — which
keeps the lowering path pure LLVM and version-matched to the installed MLIR.
`esp-clang` still has its role: building LVGL's static archive, which is where
[toolchain sovereignty](../../../../../Farscape/docs/roadmap/05_toolchain-sovereignty-and-native-assets.md)
confines clang anyway. See [the bring-up plan](../../../../docs/ESP32S3_BRINGUP.md) §1.

## Why the wiring is declared without a netlist

The EK-RA6M5 package declares literal copper nets because Renesas publishes a
design package. Nothing equivalent is published for this badge, so this package
declares *signal assignments* and records their provenance instead of claiming
net membership. `docs/BADGE_HARDWARE.md` states which facts are documented,
which are read off the silkscreen, and which are unknown.

## What the board gives a bring-up

- **JTAG over the shipped USB cable.** With no eFuses burned, the default JTAG
  source is the USB Serial/JTAG controller. The four JTAG pins are *also* on
  adjacent pads (IO39–42) for an external probe.
- **512 KB of SRAM and no PSRAM**, which is enough for a 40 KB framebuffer
  several times over. An image that fits in SRAM needs no flash cache or MMU
  setup at all — the shortest path to a first running image.
- **The panel on SPI2's native IO MUX pins**, so no GPIO matrix route is needed.
- **A hardware UART on pads IO17/IO18** if a serial console is wanted before
  USB is standing up.
