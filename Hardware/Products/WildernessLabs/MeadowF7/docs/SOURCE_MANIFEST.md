# Source Manifest: Meadow F7

Inventory audited 2026-09-10. Revisions below were read from the staged files;
no full electrical, register or board-routing reconciliation is claimed.

| Artifact | Kind | Version/Revision | Local Path | Used For | Notes |
|---|---|---|---|---|---|
| STM32F750x8 | MCU datasheet | DS12535 Rev 2, August 2025 | [stm32f750n8.pdf](../../../../Silicon/MCU/ST/STM32F7/docs/stm32f750n8.pdf) | Candidate MCU/package reference | Board part and revision still to reconcile |
| ESP32 Series | Companion-device datasheet | Version 5.2 | [esp32_datasheet_en.pdf](esp32_datasheet_en.pdf) | Radio/companion reference | Not the MCU register manual |
| Meadow F7v2 | Saved vendor web page | Snapshot date not recorded | [Meadow F7v2](<Meadow F7v2 _ Wilderness Labs Developer Portal.html>) and adjacent `_files/` | Board reference material | Saved origin: developer.wildernesslabs.co/Hardware/Reference/Meadow_Hardware/Meadow_F7/F7v2/ |

SHA-256 fingerprints (the HTML hash covers the page, not its entire asset folder):

```text
9e604a901e41aa48926b14dd64f76bc0893b5dec5f647e9f6bbaa569fd0e7f39  stm32f750n8.pdf
6fdff42cce00775643335e0ccb1dc1024070bb86208a2c734e9c09675ca3894a  esp32_datasheet_en.pdf
2aec639d6eb9200842826f0c6d6fa3112077b18387647f43d07af894a34738a6  Meadow F7v2 _ Wilderness Labs Developer Portal.html
```

Not present as separately identified sources: MCU register reference manual,
board schematic and a reconciled pin/alternate-function table. These gaps remain
open; placeholder paths are not source artifacts.

## Binding Coverage Checklist

- [ ] GPIO groups
- [ ] UART endpoints
- [ ] I2C endpoints
- [ ] SPI endpoints
- [ ] PWM/timer channels
- [ ] ADC/DAC channels
- [ ] USB and debug/programming channels
