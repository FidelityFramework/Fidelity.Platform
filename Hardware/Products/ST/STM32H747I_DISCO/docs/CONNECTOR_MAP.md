# STM32H747I-DISCO manual connector contact map

Transcribed from UM2411 Rev 7 (source ID **UM-7**) §7 Tables 6–21 and Appendix A Table 25. MB1248 schematic and manufacturing connectivity files are now staged and hashed in the source audit. This table remains a manual transcription, not a completed reconciliation against that design database; it does not short across resistors, solder bridges or transceivers and it does not enumerate mounting or shield contacts the manual omits. See the [source audit](SOURCE_AUDIT.md) for staged paths, provenance, selected device routes and open conflicts. The earlier planned `IO_MAP.md` and `SOURCE_MANIFEST.md` were never supplied; a complete MCU-side sharing matrix remains pending.

All **249 contacts the manual documents on 19 connector references** are listed, plus CN10 whose contacts the manual does not document. `NC` is the manual's no-connect marker; separate NC contacts are not a common electrical net. `-` reproduces an empty or dash cell in the manual. `MCU pin` is the port/pin the manual prints; `_C` names are the STM32H747 direct-ADC balls, which are distinct package balls from the same-numbered GPIO (PA0_C is not PA0) [verify: DS12930]. `Shared with` lists every other connector the manual routes the same port/pin to; on-board devices that also own a pin (codec, ST-LINK, PHY, SDRAM) require a separate MCU-side connectivity audit; selected routes are recorded in [SOURCE_AUDIT.md](SOURCE_AUDIT.md). Evidence is `documented UM-7 Table n`; anything the manual leaves blank or unreadable is marked so.

## CN1 — USB OTG HS Micro-AB

MCU side is the ULPI bus to the on-board USB3320 PHY (MCU-side connectivity reconciliation remains pending); the connector carries no MCU pin directly.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | VBUS | - | USB bus supply | - | documented UM-7 Table 6 |
| 2 | DM | - | USB D- (via PHY) | - | documented UM-7 Table 6 |
| 3 | DP | - | USB D+ (via PHY) | - | documented UM-7 Table 6 |
| 4 | ID | - | OTG ID | - | documented UM-7 Table 6 |
| 5 | GND | - | Ground | - | documented UM-7 Table 6 |

## CN2 — STLINK-V3E USB Micro-B

Connects the embedded STLINK-V3E to the host; the target sees it as SWD/SWO (PA13/PA14/PB3) and the VCP on USART1 PA9/PA10 (UM-7 §6.10), all through the ST-LINK MCU, not directly on these contacts.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | VBUS (power) | - | Board supply when JP6 selects STLK | - | documented UM-7 Table 7 |
| 2 | DM | - | USB D- (ST-LINK) | - | documented UM-7 Table 7 |
| 3 | DP | - | USB D+ (ST-LINK) | - | documented UM-7 Table 7 |
| 4 | NC | - | - | - | documented UM-7 Table 7 |
| 5 | GND | - | Ground | - | documented UM-7 Table 7 |

## CN3 — SPDIF input RCA

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | SPDIF_RX0 | PD7 | SPDIFRX input 0 | - | documented UM-7 Table 8 |
| 2 | GND | - | Ground | - | documented UM-7 Table 8 |
| 3 | GND | - | Ground | - | documented UM-7 Table 8 |
| 4 | GND | - | Ground | - | documented UM-7 Table 8 |

## CN4 — STLINK-V3E MCU programming header (not populated)

Reserved for programming an external target with the on-board STLINK-V3E; the manual says it is not populated by default. The PA13/PA14 names are the SWD pair of the *external* target's expected pinout as printed by the manual.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | 3V3 | - | Supply | - | documented UM-7 Table 9 |
| 2 | SWCLK | PA14 | SWD clock | CN13-6, CN16-4 | documented UM-7 Table 9 |
| 3 | GND | - | Ground | - | documented UM-7 Table 9 |
| 4 | SWDIO | PA13 | SWD data | CN13-4, CN16-2 | documented UM-7 Table 9 |

## CN5 — Arduino Uno V3 digital D8–D15

Table 10 numbers CN5 from 1 (D8) to 10 (D15). The manual's caution applies to all four Arduino headers: MCU I/Os are 3.3 V, not 5 V; JP6 must be set per §6.2.1 before a shield is fitted.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | D8 | PJ5 | - | - | documented UM-7 Table 10 |
| 2 | D9 | PJ6 | TIM8_CH2 | - | documented UM-7 Table 10 |
| 3 | D10 | PK1 | TIM1_CH1, SPI5_NSS | - | documented UM-7 Table 10 |
| 4 | D11 | PJ10 | TIM1_CH2N, SPI5_MOSI | - | documented UM-7 Table 10 |
| 5 | D12 | PJ11 | SPI5_MISO | - | documented UM-7 Table 10 |
| 6 | D13 | PK0 | SPI5_SCK | - | documented UM-7 Table 10 |
| 7 | GND | - | Ground | - | documented UM-7 Table 10 |
| 8 | AREF | - | AVDD | - | documented UM-7 Table 10 |
| 9 | D14 | PD13 | I2C4_SDA | CN9-5 (SB7 option), CN15-40, P1-27, P2-10 | documented UM-7 Table 10 |
| 10 | D15 | PD12 | I2C4_SCL | CN9-6 (SB24 option), CN15-44, P1-28, P2-7 | documented UM-7 Table 10 |

## CN6 — Arduino Uno V3 digital D0–D7

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | D0 | PJ9 | UART8_RX | - | documented UM-7 Table 10 |
| 2 | D1 | PJ8 | UART8_TX | - | documented UM-7 Table 10 |
| 3 | D2 | PJ3 | - | P3-8 (RESET) | documented UM-7 Table 10 |
| 4 | D3 | PF8 | TIM13_CH1 | P2-14 (PWM) | documented UM-7 Table 10 |
| 5 | D4 | PJ4 | - | - | documented UM-7 Table 10 |
| 6 | D5 | PA6 | TIM3_CH1 | P1-15 (DCMI_PIXCK) | documented UM-7 Table 10 |
| 7 | D6 | PJ7 | TIM8_CH2N | - | documented UM-7 Table 10 |
| 8 | D7 | PJ0 | - | - | documented UM-7 Table 10 |

## CN8 — Arduino Uno V3 power

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | NC | - | - | - | documented UM-7 Table 10 |
| 2 | IOREF | - | 3.3 V reference | - | documented UM-7 Table 10 |
| 3 | RESET | NRST | Target reset | CN13-12, CN16-10, P1-26 (RESET#) | documented UM-7 Table 10 |
| 4 | +3V3 | - | 3.3 V input/output; footnote 1: not a power input on this board | - | documented UM-7 Table 10 note 1 |
| 5 | +5V | - | 5 V output | - | documented UM-7 Table 10 |
| 6 | GND | - | Ground | - | documented UM-7 Table 10 |
| 7 | GND | - | Ground | - | documented UM-7 Table 10 |
| 8 | VIN | - | Power input; footnote 2: 6–9 V at 25 °C, regulator U19 overheats above | - | documented UM-7 Table 10 note 2 |

## CN9 — Arduino Uno V3 analog

Footnote 3: A4/A5 default to the ADC balls PC2_C/PC3_C (SB6 and SB23 closed, SB7 and SB24 open). To put I2C4 on A4/A5 instead, open SB6/SB23 and close SB7/SB24.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | A0 | PA4 | ADC12_INP18 | P1-17 (DCMI_HSYNC), P2-13 (ADC) | documented UM-7 Table 10 |
| 2 | A1 | PF10 | ADC3_INP6 | - | documented UM-7 Table 10 |
| 3 | A2 | PA0_C | ADC12_INP0 | - (PA0 on P2-1/P3-1 is a different ball) | documented UM-7 Table 10 |
| 4 | A3 | PA1_C | ADC12_INP1 | - | documented UM-7 Table 10 |
| 5 | A4 | PC2_C (SB6/SB23, default) or PD13 (SB7/SB24) | ADC3_INP0 or I2C4_SDA | PD13: CN5-9, CN15-40, P1-27, P2-10 (PC2 on P2-3/P3-3 is a different ball) | documented UM-7 Table 10 note 3 |
| 6 | A5 | PC3_C (SB6/SB23, default) or PD12 (SB7/SB24) | ADC3_INP1 or I2C4_SCL | PD12: CN5-10, CN15-44, P1-28, P2-7 (PC3 on CN17-7/P2-2/P3-2 is a different ball) | documented UM-7 Table 10 note 3 |

## CN7 — Ethernet RJ45

The manual's Table 11 is titled "USB Micro-B connector CN2" — a title erratum; its contents are the RJ45 (UM-7 §7.6, Figure 10). Yellow LED = link, green LED = traffic. The complete RMII pin audit remains pending; the contacts are PHY-side magnetics, not MCU pins. Figure 10 numbers the contacts 12 … 1 left to right in front view.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | TX+ | - | Transmit pair | - | documented UM-7 Table 11 |
| 2 | TX- | - | Transmit pair | - | documented UM-7 Table 11 |
| 3 | RX+ | - | Receive pair | - | documented UM-7 Table 11 |
| 4 | CT | - | Centre tap | - | documented UM-7 Table 11 |
| 5 | CT | - | Centre tap | - | documented UM-7 Table 11 |
| 6 | RX- | - | Receive pair | - | documented UM-7 Table 11 |
| 7 | CT | - | Centre tap | - | documented UM-7 Table 11 |
| 8 | CT | - | Centre tap | - | documented UM-7 Table 11 |
| 9 | K, yellow LED | - | Link LED cathode | - | documented UM-7 Table 11 |
| 10 | A, yellow LED | - | Link LED anode | - | documented UM-7 Table 11 |
| 11 | K, green LED | - | Traffic LED cathode | - | documented UM-7 Table 11 |
| 12 | A, green LED | - | Traffic LED anode | - | documented UM-7 Table 11 |

## CN10 — Audio blue jack (line in)

UM-7 §7.7 describes CN10 as the 3.5 mm stereo line-in to the WM8994 codec but gives no contact table. Contacts remain unverified pending connectivity extraction from the staged MB1248 schematic. The [source audit](SOURCE_AUDIT.md) identifies the codec/SAI input chain, without claiming a complete jack-contact map.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| - | - | - | stereo line input to codec | - | unknown — no table in UM-7; verify against schematic |

## CN11 — Audio green jack (line out / headphone)

Codec-driven; no MCU pin on the connector. Third column of Table 12 is the stereo-headset pinning.

| Contact | Signal | MCU pin | Function | Stereo headset pinning | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | NC | - | - | NA | documented UM-7 Table 12 |
| 2 | NC | - | - | NA | documented UM-7 Table 12 |
| 3 | GND | - | Ground | GND | documented UM-7 Table 12 |
| 4 | OUT_Right | - | Codec HP right | SPK_R (33 ohm typ.) | documented UM-7 Table 12 |
| 5 | NC | - | - | NA | documented UM-7 Table 12 |
| 6 | OUT_Left | - | Codec HP left | SPK_L (33 ohm typ.) | documented UM-7 Table 12 |

## CN12 — microSD card

The manual labels the signals `SDIO1_*`; the STM32H747 peripheral is SDMMC1. Card detect PI8 reads 0 with a card inserted, 1 otherwise (UM-7 §7.9). 3.3 V only.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | SDIO1_D2 | PC10 | SDMMC1_D2 | - | documented UM-7 Table 13 |
| 2 | SDIO1_D3 | PC11 | SDMMC1_D3 | P1-8 (DCMI_D4) | documented UM-7 Table 13 |
| 3 | SDIO1_CMD | PD2 | SDMMC1_CMD | - | documented UM-7 Table 13 |
| 4 | +3V3 | - | Supply | - | documented UM-7 Table 13 |
| 5 | SDIO1_CK | PC12 | SDMMC1_CK | - | documented UM-7 Table 13 |
| 6 | GND | - | Ground | - | documented UM-7 Table 13 |
| 7 | SDIO1_D0 | PC8 | SDMMC1_D0 | - | documented UM-7 Table 13 |
| 8 | SDIO1_D1 | PC9 | SDMMC1_D1 | P1-7 (DCMI_D3) | documented UM-7 Table 13 |
| 9 | GND | - | Ground | - | documented UM-7 Table 13 |
| 10 | µSD_DETECT | PI8 | Card detect GPIO | - | documented UM-7 Table 13 |

## CN13 — STDC14 debug

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | - | - | - | - | documented UM-7 Table 14 |
| 2 | - | - | - | - | documented UM-7 Table 14 |
| 3 | VDD | - | Target reference supply | - | documented UM-7 Table 14 |
| 4 | SWDIO/TMS | PA13 | SWD data / JTAG TMS | CN4-4, CN16-2 | documented UM-7 Table 14 |
| 5 | GND | - | Ground | - | documented UM-7 Table 14 |
| 6 | SWDCLK/TCK | PA14 | SWD clock / JTAG TCK | CN4-2, CN16-4 | documented UM-7 Table 14 |
| 7 | GND | - | Ground | - | documented UM-7 Table 14 |
| 8 | SWO/TDO | PB3 | Trace SWO / JTAG TDO | CN16-6 | documented UM-7 Table 14 |
| 9 | KEY | - | Keying position | - | documented UM-7 Table 14 |
| 10 | TDI | PA15 | JTAG TDI | CN16-8 | documented UM-7 Table 14 |
| 11 | GND | - | Ground | - | documented UM-7 Table 14 |
| 12 | RESET# | NRST | Target reset | CN8-3, CN16-10, P1-26 | documented UM-7 Table 14 |
| 13 | VCP_RX | PA10 | USART1_RX (VCP) | - (also ST-LINK internal, see source audit) | documented UM-7 Table 14 |
| 14 | VCP_TX | PA9 | USART1_TX (VCP) | - (also ST-LINK internal, see source audit) | documented UM-7 Table 14 |

## CN14 — External 5 V USB Micro-B

Power only; selected by JP6 (UM-7 §6.2).

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | VBUS (power) | - | 5 V input | - | documented UM-7 Table 15 |
| 2 | NC | - | - | - | documented UM-7 Table 15 |
| 3 | NC | - | - | - | documented UM-7 Table 15 |
| 4 | NC | - | - | - | documented UM-7 Table 15 |
| 5 | GND | - | Ground | - | documented UM-7 Table 15 |

## CN15 — DSI LCD module (MB1166)

Table 16 is laid out odd contacts left, even contacts right; each is one row here. The DSI lane contacts carry `-` in the manual's "Pin connection" column because they are the dedicated DSI PHY balls (DSI_CKP/CKN, D0P/D0N, D1P/D1N), not GPIO [verify: DS12930]. Contacts 35/37/39 are the SAI1 Block A pins that also drive the on-board WM8994 (see source audit). Contacts 8, 10, 14 and 16 print `GND` in "Pin connection" and `RFU` (reserved for future use, footnote 1) in "Function", reproduced as printed.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | GND | - | Ground | - | documented UM-7 Table 16 |
| 2 | - | - | - | - | documented UM-7 Table 16 |
| 3 | DSI_CK_P | - (DSI PHY) | DSI clock lane + | - | documented UM-7 Table 16 |
| 4 | DSI_INT | PK7 | Touch/LCD interrupt | - | documented UM-7 Table 16 |
| 5 | DSI_CK_N | - (DSI PHY) | DSI clock lane - | - | documented UM-7 Table 16 |
| 6 | GND | - | Ground | - | documented UM-7 Table 16 |
| 7 | GND | - | Ground | - | documented UM-7 Table 16 |
| 8 | RFU | GND (as printed) | Reserved for future use | - | documented UM-7 Table 16 note 1 |
| 9 | DSI_D0_P | - (DSI PHY) | DSI data lane 0 + | - | documented UM-7 Table 16 |
| 10 | RFU | GND (as printed) | Reserved for future use | - | documented UM-7 Table 16 note 1 |
| 11 | DSI_D0_N | - (DSI PHY) | DSI data lane 0 - | - | documented UM-7 Table 16 |
| 12 | GND | - | Ground | - | documented UM-7 Table 16 |
| 13 | GND | - | Ground | - | documented UM-7 Table 16 |
| 14 | RFU | GND (as printed) | Reserved for future use | - | documented UM-7 Table 16 note 1 |
| 15 | DSI_D1_P | - (DSI PHY) | DSI data lane 1 + | - | documented UM-7 Table 16 |
| 16 | RFU | GND (as printed) | Reserved for future use | - | documented UM-7 Table 16 note 1 |
| 17 | DSI_D1_N | - (DSI PHY) | DSI data lane 1 - | - | documented UM-7 Table 16 |
| 18 | GND | - | Ground | - | documented UM-7 Table 16 |
| 19 | GND | - | Ground | - | documented UM-7 Table 16 |
| 20 | - | - | - | - | documented UM-7 Table 16 |
| 21 | BLVDD (5V) | - | Backlight supply | - | documented UM-7 Table 16 |
| 22 | - | - | - | - | documented UM-7 Table 16 |
| 23 | BLVDD (5V) | - | Backlight supply | - | documented UM-7 Table 16 |
| 24 | - | - | - | - | documented UM-7 Table 16 |
| 25 | - | - | - | - | documented UM-7 Table 16 |
| 26 | - | - | - | - | documented UM-7 Table 16 |
| 27 | BLGND | - | Backlight ground | - | documented UM-7 Table 16 |
| 28 | - | - | - | - | documented UM-7 Table 16 |
| 29 | BLGND | - | Backlight ground | - | documented UM-7 Table 16 |
| 30 | - | - | - | - | documented UM-7 Table 16 |
| 31 | - | - | - | - | documented UM-7 Table 16 |
| 32 | - | - | - | - | documented UM-7 Table 16 |
| 33 | - | - | - | - | documented UM-7 Table 16 |
| 34 | - | - | - | - | documented UM-7 Table 16 |
| 35 | SCLK/MCLK | PE5 | SAI1_SCK_A (on-board codec bit clock) | - (codec, see source audit) | documented UM-7 Table 16 |
| 36 | 3.3 V | - | Supply | - | documented UM-7 Table 16 |
| 37 | LRCLK | PE4 | SAI1_FS_A (on-board codec frame sync) | - (codec, see source audit) | documented UM-7 Table 16 |
| 38 | - | - | - | - | documented UM-7 Table 16 |
| 39 | I2S_DATA | PE6 | SAI1_SD_A (on-board codec data out) | - (codec, see source audit) | documented UM-7 Table 16 |
| 40 | I2C_SDA | PD13 | I2C4_SDA (touch controller) | CN5-9, CN9-5 (SB7 option), P1-27, P2-10 | documented UM-7 Table 16 |
| 41 | - | - | - | - | documented UM-7 Table 16 |
| 42 | - | - | - | - | documented UM-7 Table 16 |
| 43 | - | - | - | - | documented UM-7 Table 16 |
| 44 | I2C_SCL | PD12 | I2C4_SCL (touch controller) | CN5-10, CN9-6 (SB24 option), P1-28, P2-7 | documented UM-7 Table 16 |
| 45 | CEC_CLK | PA8 | HDMI-CEC clock | - | documented UM-7 Table 16 |
| 46 | - | - | - | - | documented UM-7 Table 16 |
| 47 | CEC | PB6 | HDMI-CEC data | - | documented UM-7 Table 16 |
| 48 | - | - | - | - | documented UM-7 Table 16 |
| 49 | DSI_TE | PJ2 | Tearing-effect input | - | documented UM-7 Table 16 |
| 50 | - | - | - | - | documented UM-7 Table 16 |
| 51 | - | (cell blank in UM-7) | - | - | documented UM-7 Table 16 |
| 52 | - | - | - | - | documented UM-7 Table 16 |
| 53 | DSI_BL_CTRL | PJ12 | Backlight enable | - | documented UM-7 Table 16 |
| 54 | - | - | - | - | documented UM-7 Table 16 |
| 55 | - | - | - | - | documented UM-7 Table 16 |
| 56 | - | - | - | - | documented UM-7 Table 16 |
| 57 | DSI_RST | PG3 | LCD reset | - | documented UM-7 Table 16 |
| 58 | - | - | - | - | documented UM-7 Table 16 |
| 59 | - | - | - | - | documented UM-7 Table 16 |
| 60 | 1.8 V | - | Supply | - | documented UM-7 Table 16 |

## CN16 — TAG connector (footprint)

Tag-Connect footprint for an external probe (UM-7 §7.13, Figure 16: 1–5 lower row, 6–10 upper row).

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | VDD | - | Target reference supply | - | documented UM-7 Table 17 |
| 2 | SWDIO/TMS | PA13 | SWD data / JTAG TMS | CN4-4, CN13-4 | documented UM-7 Table 17 |
| 3 | GND | - | Ground | - | documented UM-7 Table 17 |
| 4 | SWDCLK/TCK | PA14 | SWD clock / JTAG TCK | CN4-2, CN13-6 | documented UM-7 Table 17 |
| 5 | GND | - | Ground | - | documented UM-7 Table 17 |
| 6 | SWO/TDO | PB3 | Trace SWO / JTAG TDO | CN13-8 | documented UM-7 Table 17 |
| 7 | NC | - | - | - | documented UM-7 Table 17 |
| 8 | TDI | PA15 | JTAG TDI | CN13-10 | documented UM-7 Table 17 |
| 9 | TRST | PB4 | JTAG TRST | - | documented UM-7 Table 17 |
| 10 | RESET# | NRST | Target reset | CN8-3, CN13-12, P1-26 | documented UM-7 Table 17 |

## CN17 — Audio MEMS (DFSDM) 2x10 header

2x10 male 1.27 mm header for an audio MEMS daughterboard on the DFSDM interface (UM-7 §7.14). DFSDM_CKOUT is brought out on both contact 3 and contact 4.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | GND | - | Ground | - | documented UM-7 Table 18 |
| 2 | +3V3 | - | Supply | - | documented UM-7 Table 18 |
| 3 | DFSDM_CKOUT | PD3 | DFSDM clock out | CN17-4, P1-9 (DCMI_D5), P2-18 | documented UM-7 Table 18 |
| 4 | DFSDM_CKOUT | PD3 | DFSDM clock out | CN17-3, P1-9 (DCMI_D5), P2-18 | documented UM-7 Table 18 |
| 5 | DFSDM_DATIN3 | PC7 | DFSDM data in 3 | P1-5 (DCMI_D1), P2-17 | documented UM-7 Table 18 |
| 6 | DFSDM_DATIN7 | PB9 | DFSDM data in 7 | P1-11 (DCMI_D7), P2-19 | documented UM-7 Table 18 |
| 7 | DFSDM_DATIN1 | PC3 | DFSDM data in 1 | P2-2 (SB34 option), P3-2 (PC3_C on CN9-6 is a different ball) | documented UM-7 Table 18 |
| 8 | DFSDM_DATIN2 | PB14 | DFSDM data in 2 | P2-9 (SPI2_MISOs) | documented UM-7 Table 18 |
| 9 | NC | - | - | - | documented UM-7 Table 18 |
| 10 | DETECTn | PC6 | Daughterboard detect | P1-4 (DCMI_D0), P2-11 (INT), P3-7 (INT) | documented UM-7 Table 18 |
| 11 | NC | - | - | - | documented UM-7 Table 18 |
| 12 | MEMS_LED | PJ13 | Daughterboard LED | P2-12 (RESET) | documented UM-7 Table 18 |
| 13 | NC | - | - | - | documented UM-7 Table 18 |
| 14 | NC | - | - | - | documented UM-7 Table 18 |
| 15 | NC | - | - | - | documented UM-7 Table 18 |
| 16 | NC | - | - | - | documented UM-7 Table 18 |
| 17 | NC | - | - | - | documented UM-7 Table 18 |
| 18 | NC | - | - | - | documented UM-7 Table 18 |
| 19 | +3V3 | - | Supply | - | documented UM-7 Table 18 |
| 20 | GND | - | Ground | - | documented UM-7 Table 18 |

## P1 — Camera module (30-pin ZIF, STM32F4DIS-CAM)

8-bit DCMI. Camera I2C addresses 0x61/0x60 (8-bit transfer addresses; canonical 7-bit address 0x30) on the shared I2C4 bus. UM-7 §7.15 states the sharing explicitly: DCMI_SDA/SCL with Pmod/STMod+/Arduino/DSI LCD; PA4, PC6, PC7, PB8, PB9, PD3 with Pmod (the manual says "Pmod", Table 25 shows them on STMod+ P2); PC9 and PC11 with SDIO; PA6 with Arduino.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | GND | - | Ground | - | documented UM-7 Table 19 |
| 2 | NC | - | - | - | documented UM-7 Table 19 |
| 3 | NC | - | - | - | documented UM-7 Table 19 |
| 4 | DCMI_D0 | PC6 | DCMI data 0 | CN17-10 (DETECTn), P2-11 (INT), P3-7 (INT) | documented UM-7 Table 19 |
| 5 | DCMI_D1 | PC7 | DCMI data 1 | CN17-5 (DFSDM_DATIN3), P2-17 | documented UM-7 Table 19 |
| 6 | DCMI_D2 | PG10 | DCMI data 2 | - | documented UM-7 Table 19 |
| 7 | DCMI_D3 | PC9 | DCMI data 3 | CN12-8 (SDMMC1_D1) | documented UM-7 Table 19 |
| 8 | DCMI_D4 | PC11 | DCMI data 4 | CN12-2 (SDMMC1_D3) | documented UM-7 Table 19 |
| 9 | DCMI_D5 | PD3 | DCMI data 5 | CN17-3/4 (DFSDM_CKOUT), P2-18 | documented UM-7 Table 19 |
| 10 | DCMI_D6 | PB8 | DCMI data 6 | P2-20 (DFSDM-CK7) | documented UM-7 Table 19 |
| 11 | DCMI_D7 | PB9 | DCMI data 7 | CN17-6 (DFSDM_DATIN7), P2-19 | documented UM-7 Table 19 |
| 12 | NC | - | - | - | documented UM-7 Table 19 |
| 13 | NC | - | - | - | documented UM-7 Table 19 |
| 14 | GND | - | Ground | - | documented UM-7 Table 19 |
| 15 | DCMI_PIXCK | PA6 | DCMI pixel clock | CN6-6 (D5) | documented UM-7 Table 19 |
| 16 | GND | - | Ground | - | documented UM-7 Table 19 |
| 17 | DCMI_HSYNC | PA4 | DCMI HSYNC | CN9-1 (A0), P2-13 (ADC) | documented UM-7 Table 19 |
| 18 | NC | - | - | - | documented UM-7 Table 19 |
| 19 | DCMI_VSYNC | PB7 | DCMI VSYNC | - | documented UM-7 Table 19 |
| 20 | 3V3 | - | Supply | - | documented UM-7 Table 19 |
| 21 | Camera_CLK | - (OSC_24M) | 24 MHz from X1 (UM-7 §6.3) | - | documented UM-7 Table 19 |
| 22 | NC | - | - | - | documented UM-7 Table 19 |
| 23 | GND | - | Ground | - | documented UM-7 Table 19 |
| 24 | NC | - | - | - | documented UM-7 Table 19 |
| 25 | DCMI_PWR_EN | PJ14 | Camera power enable | - | documented UM-7 Table 19 |
| 26 | RESET# | NRST | Target reset | CN8-3, CN13-12, CN16-10 | documented UM-7 Table 19 |
| 27 | DCMI_SDA | PD13 | I2C4_SDA | CN5-9, CN9-5 (SB7 option), CN15-40, P2-10 | documented UM-7 Table 19 |
| 28 | DCMI_SCL | PD12 | I2C4_SCL | CN5-10, CN9-6 (SB24 option), CN15-44, P2-7 | documented UM-7 Table 19 |
| 29 | GND | - | Ground | - | documented UM-7 Table 19 |
| 30 | 3V3 | - | Supply | - | documented UM-7 Table 19 |

## P2 — STMod+ (20-pin)

Contacts 1–4 carry either the SPI2 set or the USART2 set, chosen by solder bridges SB31–SB38 (Table 25); Table 20 prints both candidates as `SPI2_x/USART2_y (Pa/Pb)`. The MB1280 fan-out board supplied with the kit plugs in here (UM-7 §7.16). Figure 18: contacts 1–10 upper row right-to-left, 11–20 lower row.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | SPI2_NSS/USART2_CTS | PA11 (SB32) / PA0 (SB31) | NSS2 or CTSS2 | P3-1 | documented UM-7 Table 20, Table 25 |
| 2 | SPI2_MOSI/USART2_TX | PC3 (SB34) / PD5 (SB33) | MOSI2 or TXS2 | P3-2; PC3: CN17-7 (DFSDM_DATIN1) | documented UM-7 Table 20, Table 25 |
| 3 | SPI2_MISO/USART2_RX | PC2 (SB36) / PD6 (SB35) | MISO2 or RXS2 | P3-3 | documented UM-7 Table 20, Table 25 |
| 4 | SPI2_SCK/USART2_RTS | PA12 (SB38) / PD4 (SB37) | SCK2 or RTSS2 | P3-4 | documented UM-7 Table 20, Table 25 |
| 5 | GND | - | Ground | - | documented UM-7 Table 20 |
| 6 | +5 V | - | Supply | - | documented UM-7 Table 20 |
| 7 | I2C4_SCL | PD12 | SCL4 | CN5-10, CN9-6 (SB24 option), CN15-44, P1-28 | documented UM-7 Table 20 |
| 8 | SPI2_MOSIs | PB15 | MOSI2 (secondary) | - | documented UM-7 Table 20 |
| 9 | SPI2_MISOs | PB14 | MISO2 (secondary) | CN17-8 (DFSDM_DATIN2) | documented UM-7 Table 20 |
| 10 | I2C4_SDA | PD13 | SDA4 | CN5-9, CN9-5 (SB7 option), CN15-40, P1-27 | documented UM-7 Table 20 |
| 11 | INT | PC6 | Module interrupt | CN17-10 (DETECTn), P1-4 (DCMI_D0), P3-7 | documented UM-7 Table 20 |
| 12 | RESET | PJ13 | Module reset (GPIO) | CN17-12 (MEMS_LED) | documented UM-7 Table 20 |
| 13 | ADC | PA4 | ADC12_IN4 | CN9-1 (A0), P1-17 (DCMI_HSYNC) | documented UM-7 Table 20 |
| 14 | PWM | PF8 | TIM13_CH1 | CN6-4 (D3) | documented UM-7 Table 20 |
| 15 | +5 V | - | Supply | - | documented UM-7 Table 20 |
| 16 | GND | - | Ground | - | documented UM-7 Table 20 |
| 17 | DFSDM-DATA3 | PC7 | DFSDM data in 3 | CN17-5, P1-5 (DCMI_D1) | documented UM-7 Table 20 |
| 18 | DFSDM-CKOUT | PD3 | DFSDM clock out | CN17-3/4, P1-9 (DCMI_D5) | documented UM-7 Table 20 |
| 19 | DFSDM-DATA7 | PB9 | DFSDM data in 7 | CN17-6, P1-11 (DCMI_D7) | documented UM-7 Table 20 |
| 20 | DFSDM-CK7 | PB8 | DFSDM clock in 7 | P1-10 (DCMI_D6) | documented UM-7 Table 20 |

### P2 sharing and multiplexing (UM-7 Table 25)

Table 25 restates the STMod+ contacts with the solder bridge per candidate, the other alternate functions of each port, and which other connector (Arduino, Pmod, DFSDM header, camera) shares the pin. Footnote 1: the I2C bus on contacts 19/20 (PB9/PB8 = I2C1_SDA/SCL) may be shared with built-in target devices — check addresses before adding a device. PD3 and PB8 carry an asterisk in Table 25 that the manual does not explain [verify: schematic].

| Contact | Port | SB | STMod+ basic | Other alternate functions (Table 25) | Pmod | Arduino | DFSDM | DCMI |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | PA0 | SB31 | CTSS2 | ADC123_IN0, TIM5_CH1 | CTSS2 | - | - | - |
| 1 | PA11 | SB32 | NSS2 | CAN1_RX, USART1_CTS | CSN2 | - | - | - |
| 2 | PD5 | SB33 | TXS2 | CAN1_TXFD, USART2_RX | TXS3 (as printed) | - | - | - |
| 2 | PC3 | SB34 | MOSI2 | ADC123_IN13 | MOSI2 | - | DATA1 | - |
| 3 | PD6 | SB35 | RXS2 | USART2_TX | RXS3 (as printed) | - | DATA1 | - |
| 3 | PC2 | SB36 | MISO2 | ADC123_IN12 | MISO2 | - | CKOUT | - |
| 4 | PD4 | SB37 | RTSS2 | CAN1_RXFD | RTSS3 (as printed) | - | - | - |
| 4 | PA12 | SB38 | SCK2 | TIM1_ETR, CAN1_TX, USART1_RTS | SCK2 | - | - | - |
| 5 | GND | - | GND | - | - | - | - | - |
| 6 | +5V | - | +5V | - | - | - | - | - |
| 7 | PD12 | - | SCL4 | TIM4_CH1 | - | SCL4 | - | - |
| 8 | PB15 | - | MOSI2 | TIM1_CH3N, TIM8_CH3N, TIM12_CH2, USART1_RX | - | - | CK2 | - |
| 9 | PB14 | - | MISO2 | TIM1_CH2N, TIM8_CH2N, TIM12_CH1, USART1_TX | - | - | DATA2 | - |
| 10 | PD13 | - | SDA4 | TIM4_CH2 | - | SDA4 | - | - |
| 11 | PC6 | - | INT | TIM3_CH1, TIM8_CH1, USART6_TX | INT | - | CK3 | D0 |
| 12 | PJ13 | - | RST | - | RST (as printed; Table 21 puts Pmod RESET on PJ3) | - | - | - |
| 13 | PA4 | - | ADC | ADC12_IN4, TIM5_ETR, DAC1_OUT | - | A0 | - | HSYNC |
| 14 | PF8 | - | PWM | ADC3_IN6, TIM13_CH1 | - | D3 | - | - |
| 15 | +5V | - | +5V | - | - | - | - | - |
| 16 | GND | - | GND | - | - | - | - | - |
| 17 | PC7 | - | GPIO | TIM3_CH2, TIM8_CH2, USART6_RX | - | - | - | D1 |
| 18 | PD3* | - | GPIO | - | - | - | CKOUT | D5 |
| 19 | PB9 | - | GPIO | TIM4_CH4, TIM17_CH1, CAN1_TX, I2C1_SDA | - | - | DATA7 | D7 |
| 20 | PB8* | - | GPIO | TIM4_CH3, TIM16_CH1, CAN1_RX, I2C1_SCL | - | - | CK7 | D6 |

Table 25's DFSDM column for PC2 reads `CKOUT` and for PC3/PD6 `DATA1`; Table 18 assigns DFSDM_CKOUT to PD3 and DATIN1 to PC3. Both are legal alternate functions; the CN17 header wiring is Table 18's [verify: schematic].

## P3 — Pmod (12-pin, type 2A/4A)

Contacts 1–4 are the same SPI2/USART2 candidate nets as P2 contacts 1–4 and follow the same SB31–SB38 selection (Table 25). Figure 19: contacts 1–6 upper row right-to-left, 7–12 lower row.

| Contact | Signal | MCU pin | Function | Shared with | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | SPI2_NSS/USART2_CTS | PA11 (SB32) / PA0 (SB31) | NSS2 or CTSS2 | P2-1 | documented UM-7 Table 21, Table 25 |
| 2 | SPI2_MOSI/USART2_TX | PC3 (SB34) / PD5 (SB33) | MOSI2 or TXS2 | P2-2; PC3: CN17-7 (DFSDM_DATIN1) | documented UM-7 Table 21, Table 25 |
| 3 | SPI2_MISO/USART2_RX | PC2 (SB36) / PD6 (SB35) | MISO2 or RXS2 | P2-3 | documented UM-7 Table 21, Table 25 |
| 4 | SPI2_SCK/USART2_RTS | PA12 (SB38) / PD4 (SB37) | SCK2 or RTSS2 | P2-4 | documented UM-7 Table 21, Table 25 |
| 5 | GND | - | Ground | - | documented UM-7 Table 21 |
| 6 | +3V3 | - | Supply | - | documented UM-7 Table 21 |
| 7 | INT | PC6 | Module interrupt | CN17-10 (DETECTn), P1-4 (DCMI_D0), P2-11 | documented UM-7 Table 21 |
| 8 | RESET | PJ3 | Module reset (GPIO) | CN6-3 (D2); Table 25 shows Pmod RST on PJ13 instead — see below | documented UM-7 Table 21 |
| 9 | NA | - | - | - | documented UM-7 Table 21 |
| 10 | NA | - | - | - | documented UM-7 Table 21 |
| 11 | GND | - | Ground | - | documented UM-7 Table 21 |
| 12 | +3V3 | - | Supply | - | documented UM-7 Table 21 |

## Cross-connector sharing by port/pin

Every port/pin the manual places on more than one connector. A pin listed once per connector but with an SB-selected alternative is shown with the bridge. A complete board-internal sharing matrix for codec, PHY, ST-LINK, SDRAM, QSPI and LEDs remains pending; [SOURCE_AUDIT.md](SOURCE_AUDIT.md) records the selected routes and known conflicts.

| Port/pin | Connectors | Note |
| --- | --- | --- |
| NRST / RESET# | CN8-3, CN13-12, CN16-10, P1-26 | reset line; also B1 button |
| PA13 | CN4-4, CN13-4, CN16-2 | SWDIO/TMS; CN4 not fitted |
| PA14 | CN4-2, CN13-6, CN16-4 | SWDCLK/TCK; CN4 not fitted |
| PB3 | CN13-8, CN16-6 | SWO/TDO |
| PA15 | CN13-10, CN16-8 | TDI |
| PD12 | CN5-10, CN9-6 (SB24), CN15-44, P1-28, P2-7 | I2C4_SCL bus: touch, camera, Arduino, STMod+, codec (see source audit) |
| PD13 | CN5-9, CN9-5 (SB7), CN15-40, P1-27, P2-10 | I2C4_SDA bus |
| PA4 | CN9-1, P1-17, P2-13 | A0 / DCMI_HSYNC / STMod+ ADC |
| PA6 | CN6-6, P1-15 | D5 / DCMI_PIXCK |
| PF8 | CN6-4, P2-14 | D3 / STMod+ PWM, TIM13_CH1 |
| PJ3 | CN6-3, P3-8 | D2 / Pmod RESET |
| PJ13 | CN17-12, P2-12 | MEMS_LED / STMod+ RESET |
| PC6 | CN17-10, P1-4, P2-11, P3-7 | DETECTn / DCMI_D0 / INT |
| PC7 | CN17-5, P1-5, P2-17 | DFSDM_DATIN3 / DCMI_D1 |
| PD3 | CN17-3, CN17-4, P1-9, P2-18 | DFSDM_CKOUT / DCMI_D5 |
| PB9 | CN17-6, P1-11, P2-19 | DFSDM_DATIN7 / DCMI_D7 / I2C1_SDA |
| PB8 | P1-10, P2-20 | DCMI_D6 / DFSDM-CK7 / I2C1_SCL |
| PB14 | CN17-8, P2-9 | DFSDM_DATIN2 / SPI2_MISOs |
| PC3 | CN17-7, P2-2 (SB34), P3-2 (SB34) | DFSDM_DATIN1 / SPI2_MOSI; PC3_C on CN9-6 is a separate ball |
| PC2 | P2-3 (SB36), P3-3 (SB36) | SPI2_MISO; PC2_C on CN9-5 is a separate ball |
| PA0 | P2-1 (SB31), P3-1 (SB31) | USART2_CTS; PA0_C on CN9-3 is a separate ball |
| PA11 | P2-1 (SB32), P3-1 (SB32) | SPI2_NSS |
| PD5 | P2-2 (SB33), P3-2 (SB33) | USART2_TX |
| PD6 | P2-3 (SB35), P3-3 (SB35) | USART2_RX |
| PA12 | P2-4 (SB38), P3-4 (SB38) | SPI2_SCK |
| PD4 | P2-4 (SB37), P3-4 (SB37) | USART2_RTS |
| PC9 | CN12-8, P1-7 | SDMMC1_D1 / DCMI_D3 |
| PC11 | CN12-2, P1-8 | SDMMC1_D3 / DCMI_D4 |

Pins on exactly one connector: PD7 (CN3); PJ5, PJ6, PK1, PJ10, PJ11, PK0 (CN5); PJ9, PJ8, PJ4, PJ7, PJ0 (CN6); PF10, PA0_C, PA1_C, PC2_C, PC3_C (CN9); PC10, PD2, PC12, PC8, PI8 (CN12); PA10, PA9 (CN13, also ST-LINK VCP); PK7, PE5, PE4, PE6, PA8, PB6, PJ2, PJ12, PG3 (CN15; PE4/PE5/PE6 also the on-board codec); PB4 (CN16); PG10, PB7, PJ14 (P1); PB15 (P2).

## Corrections and open items

- **Table 11 title.** Printed as "USB Micro-B connector CN2"; the contents are the CN7 RJ45 (§7.6).
- **`SDIO1_*` naming (Table 13).** The STM32H747 peripheral is SDMMC1; the signal names are kept as printed, the function column uses the peripheral name.
- **Pmod RESET pin.** Table 21 puts P3 contact 8 RESET on PJ3; Table 25's Pmod column puts RST on PJ13 (the STMod+ RESET). One of the two is wrong or the Pmod RESET is bridged — [verify: schematic].
- **Pmod signal suffixes in Table 25.** `TXS3`, `RXS3`, `RTSS3` in the Pmod column against `TXS2`, `RXS2`, `RTSS2` in the STMod+ column for the same nets; presumably typos for the `2` forms.
- **DFSDM columns in Table 25** (CKOUT on PC2, DATA1 on PD6) describe alternate-function capability, not the CN17 wiring in Table 18.
- **Asterisks on PD3 and PB8 in Table 25** are unexplained in the manual.
- **CN10 contacts** are not documented anywhere in UM-7 — [verify: schematic].
- **CN1/CN2/CN7/CN11/CN14** carry no MCU pin directly; a complete MCU-side route audit for ULPI, ST-LINK SWD/VCP, RMII, codec and power remains pending; see [SOURCE_AUDIT.md](SOURCE_AUDIT.md) for current evidence.
- **Mounting and shield contacts** are not enumerated by the manual and are absent here; the EK-RA6M5 map lists them because it was built from a netlist.
