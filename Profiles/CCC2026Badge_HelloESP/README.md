# CCC2026Badge_HelloESP

The selected image for [HelloESP](../../../MCU/Espressif/CCC2026Badge/HelloESP)
on the Carolina Code Conference 2026 badge: an all-SRAM Clef unikernel loaded by
the ESP32-S3 mask ROM from flash offset 0.

## Status

The selected HelloESP application compiled through Composer and CCS and ran
on the badge on 2026-09-12, including display, LEDs, buttons and a 1 kHz
interrupt. See [the hardware acceptance record](../../docs/ESP32S3_BRINGUP.md#10-helloesp-runs-2026-09-12).
This establishes that workload, not every declared peripheral or future driver.

## What it selects

| Layer | Package |
| --- | --- |
| Silicon | [ESP32-S3-WROOM-1-N8](../../Hardware/Silicon/MCU/Espressif/ESP32S3/ESP32_S3_WROOM_1_N8) — memory banks, vector layout, 111 registers |
| Product | [CCC2026Badge](../../Hardware/Products/CircuitBoardMedics/CCC2026Badge) — panel, LEDs, buttons, pads |
| Environment | [Freestanding xtensa_esp32s3](../../Environments/Freestanding/xtensa_esp32s3) — ABI, representations, bring-up obligations |

## The number worth reading

`sram1InstructionBytes = 131072`.

SRAM1 is one 416 KB bank visible from *both* CPU buses at different addresses.
This profile gives 128 KB of it to the instruction side, which leaves:

- **instruction window** — 160 KB (SRAM0's 32 KB + 128 KB of SRAM1)
- **data window** — 352 KB (288 KB of SRAM1 + SRAM2's 64 KB)

352 KB of data comfortably holds the 40,960-byte framebuffer, the 1,352-byte
logo mask and a 16 KB stack. The split can move later without touching any
other declaration — an image that links LVGL grows on the data side.

## Evidence behind the image header

The `esp_image_header_t` fields are not preferences. The mask ROM configures the
flash bus from them before any code runs, so a wrong value is a silent boot
failure. They were read out of **this board's own stock bootloader**, captured in
the [2026-09-11 recovery image](../../../MCU/Espressif/CCC2026Badge/recovery/2026-09-11-stock):

| Field | Value | Source |
| --- | --- | --- |
| `chip_id` | 9 (ESP32-S3) | stock header |
| `spi_mode` | 2 (DIO) | stock header — the eFuse reports the part quad-capable, but the shipped image boots DIO |
| `spi_speed` | 15 (80 MHz) | stock header |
| `spi_size` | 3 (8 MB) | stock header, and confirmed by `esptool flash-id` on the part |

## Not in this image

No RTOS, no ESP-IDF, no second-stage bootloader, no partition table, no
DMA, no WiFi, no BLE, no flash cache or MMU configuration — and **no LVGL**.
HelloESP draws its own framebuffer so the hardware path can be proven before a
foreign archive joins the link.

The application does use image-owned allocation for its arrays; the
[display model](../../docs/DISPLAY_MODEL.md#representation-and-lifetime-determine-the-implementation)
records actual storage widths and allocation behavior. The
[Radio scaffold](../../docs/RADIO_MODEL.md) inventories available wireless
hardware without adding it to this workload.
