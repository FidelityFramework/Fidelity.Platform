# EK-RA6M5 complete connector contact map

Derived from the pinned MP-release Cadence netlist, checked against schematic sheet 11 for the native headers and sheet 6 for Pmod/Grove/Qwiic. See [source provenance](SOURCE_MANIFEST.md) and [I/O usage and conflicts](IO_MAP.md).

All **355 contacts on 32 connector references** are listed, including mounting/shield contacts and configuration headers. J32 is not present in this design. `NC` is a no-connect marker; separate NC contacts are not a common electrical net. Names beginning `UNNAMED_` are preserved vendor net names and require following the components in [BoardNets.fs](../BoardNets.fs). They are not unspecified connections. Trace links, resistors, filters and transceivers separate nets; the table does not short across them.

## J1 — Native header

| Odd contact | Net | Even contact | Net |
| --- | --- | --- | --- |
| 1 | 3.3 V | 2 | +3V3_MCU |
| 3 | P400 | 4 | P401 |
| 5 | P402 | 6 | P403 |
| 7 | P404 | 8 | P405 |
| 9 | P406 | 10 | P700 |
| 11 | P701 | 12 | P702 |
| 13 | P703 | 14 | P704 |
| 15 | P705 | 16 | P706 |
| 17 | P707 | 18 | PB00 |
| 19 | PB01 | 20 | VBATT |
| 21 | NC — no connection | 22 | P213 |
| 23 | P212 | 24 | NC — no connection |
| 25 | NC — no connection | 26 | NC — no connection |
| 27 | NC — no connection | 28 | NC — no connection |
| 29 | NC — no connection | 30 | P708 |
| 31 | P415 | 32 | P414 |
| 33 | P413 | 34 | P412 |
| 35 | P411 | 36 | P410 |
| 37 | P409 | 38 | P408 |
| 39 | P407 | 40 | GND |

## J2 — Native header

| Odd contact | Net | Even contact | Net |
| --- | --- | --- | --- |
| 1 | 3.3 V | 2 | +3V3_USB |
| 3 | P207 | 4 | P206 |
| 5 | P205 | 6 | P204 |
| 7 | P203 | 8 | P202 |
| 9 | P313 | 10 | P314 |
| 11 | P315 | 12 | P900 |
| 13 | P901 | 14 | NC — no connection |
| 15 | P214 | 16 | P211 |
| 17 | P210 | 18 | P209 |
| 19 | P208 | 20 | RESET# |
| 21 | P201/MD | 22 | P200/NMI |
| 23 | P908 | 24 | P907 |
| 25 | P906 | 26 | P905 |
| 27 | P312 | 28 | P311 |
| 29 | P310 | 30 | P309 |
| 31 | P308 | 32 | P307 |
| 33 | P306 | 34 | P305 |
| 35 | P304 | 36 | P303 |
| 37 | P302 | 38 | P301 |
| 39 | P300 | 40 | GND |

## J3 — Native header

| Odd contact | Net | Even contact | Net |
| --- | --- | --- | --- |
| 1 | 3.3 V | 2 | +3V3_MCU |
| 3 | P108 | 4 | P109 |
| 5 | P110 | 6 | P111 |
| 7 | P112 | 8 | P113 |
| 9 | P114 | 10 | P115 |
| 11 | P608 | 12 | P609 |
| 13 | P610 | 14 | P611 |
| 15 | P612 | 16 | P613 |
| 17 | P614 | 18 | P615 |
| 19 | PA08 | 20 | PA09 |
| 21 | PA10 | 22 | PA01 |
| 23 | PA00 | 24 | P607 |
| 25 | P606 | 26 | P605 |
| 27 | P604 | 28 | P603 |
| 29 | P602 | 30 | P601 |
| 31 | P600 | 32 | P107 |
| 33 | P106 | 34 | P105 |
| 35 | P104 | 36 | P103 |
| 37 | P102 | 38 | P101 |
| 39 | P100 | 40 | GND |

## J4 — Native header

| Odd contact | Net | Even contact | Net |
| --- | --- | --- | --- |
| 1 | +3V3_MCU | 2 | P800 |
| 3 | P801 | 4 | P802 |
| 5 | P803 | 6 | P804 |
| 7 | P500 | 8 | P501 |
| 9 | P502 | 10 | P503 |
| 11 | P504 | 12 | P505 |
| 13 | P506 | 14 | P507 |
| 15 | P508 | 16 | P015 |
| 17 | P014 | 18 | VREFL |
| 19 | VREFH | 20 | AVCC0 |
| 21 | AVSS0 | 22 | VREFL0 |
| 23 | VREFH0 | 24 | P010 |
| 25 | P009 | 26 | P008 |
| 27 | P007 | 28 | P006 |
| 29 | P005 | 30 | P004 |
| 31 | P003 | 32 | P002 |
| 33 | P001 | 34 | P000 |
| 35 | P806 | 36 | P805 |
| 37 | P513 | 38 | P512 |
| 39 | P511 | 40 | GND |

## J5 — Ethernet RJ45

| Contact | Net |
| --- | --- |
| 1 | ETX_P |
| 2 | ETX_N |
| 3 | ERX_P |
| 4 | UNNAMED_12_CAP_I85_B |
| 5 | UNNAMED_12_CAP_I86_B |
| 6 | ERX_N |
| 7 | NC — no connection |
| 8 | GND |
| 9 | UNNAMED_12_RES_I87_B |
| 10 | ENET_LEDG_N |
| 11 | ENET_LEDY_N |
| 12 | UNNAMED_12_RES_I89_B |
| 13 | GND |
| 14 | GND |

## J6 — J-Link mode

| Contact | Net |
| --- | --- |
| 1 | P201/MD |
| 2 | UNNAMED_16_DOUBLEROW10_I42_4 |

## J7 — USB HS power mode

| Contact | Net |
| --- | --- |
| 1 | USBHS_VBUS |
| 2 | UNNAMED_17_MICROUSBAB_I45_VBUS |
| 3 | UNNAMED_17_RES_I48_B |

## J8 — Reset routing

| Contact | Net |
| --- | --- |
| 1 | UNNAMED_16_DOUBLEROW10_I42_10 |
| 2 | RESET# |
| 3 | GND |

## J9 — Debug MCU reset

| Contact | Net |
| --- | --- |
| 1 | JLOB_RESET# |
| 2 | GND |

## J10 — Debug USB (S124)

| Contact | Net |
| --- | --- |
| 1 | +5V_USB_DBG |
| 2 | UNNAMED_16_MICROUSBB9PIN_I12_DM |
| 3 | UNNAMED_16_MICROUSBB9PIN_I12_DP |
| 4 | NC — no connection |
| 5 | GND |
| 6 | GND |
| 7 | GND |
| 8 | GND |
| 9 | GND |

## J11 — Target USB FS

| Contact | Net |
| --- | --- |
| 1 | USBFS_VBUS |
| 2 | USBF_N |
| 3 | USBF_P |
| 4 | NC — no connection |
| 5 | GND |
| 6 | GND |
| 7 | GND |
| 8 | GND |
| 9 | GND |
| 10 | GND |
| 11 | GND |

## J12 — USB FS power mode

| Contact | Net |
| --- | --- |
| 1 | +5V_H_USBFS |
| 2 | USBFS_VBUS |
| 3 | USBFS_VBUS_IN |

## J13 — 10-pin target debug

| Contact | Net |
| --- | --- |
| 1 | 3.3 V |
| 2 | UNNAMED_16_DOUBLEROW10_I42_2 |
| 3 | GND |
| 4 | UNNAMED_16_DOUBLEROW10_I42_4 |
| 5 | GND |
| 6 | UNNAMED_16_DOUBLEROW10_I42_6 |
| 7 | NC — no connection |
| 8 | UNNAMED_16_DOUBLEROW10_I42_8 |
| 9 | UNNAMED_16_DOUBLEROW10_I42_9 |
| 10 | UNNAMED_16_DOUBLEROW10_I42_10 |

## J14 — Debug MCU needle-adapter footprint (not fitted)

| Contact | Net |
| --- | --- |
| 1 | +3V3JLOB |
| 2 | JLOB_SWDIO |
| 3 | JLOB_RESET# |
| 4 | JLOB_SWCLK |
| 5 | GND |
| 6 | NC — no connection |

## J15 — USB FS supply link

| Contact | Net |
| --- | --- |
| 1 | USBFS_VBUS |
| 2 | +5V_USBFS |

## J16 — Target boot mode

| Contact | Net |
| --- | --- |
| 1 | UNNAMED_7_RES_I54_A |
| 2 | P201/MD |

## J17 — USB HS supply link

| Contact | Net |
| --- | --- |
| 1 | UNNAMED_17_MICROUSBAB_I45_VBUS |
| 2 | +5V_USBHS |

## J18 — Arduino power

| Contact | Net |
| --- | --- |
| 1 | NC — no connection |
| 2 | 3.3 V |
| 3 | P303 |
| 4 | 3.3 V |
| 5 | 5 V |
| 6 | GND |
| 7 | GND |
| 8 | NC — no connection |

## J19 — Arduino analog

| Contact | Net |
| --- | --- |
| 1 | P000 |
| 2 | P001 |
| 3 | P002 |
| 4 | P003 |
| 5 | P014 |
| 6 | P015 |

## J20 — 20-pin target debug and trace

| Contact | Net |
| --- | --- |
| 1 | 3.3 V |
| 2 | UNNAMED_16_DOUBLEROW10_I42_2 |
| 3 | GND |
| 4 | UNNAMED_16_DOUBLEROW10_I42_4 |
| 5 | GND |
| 6 | UNNAMED_16_DOUBLEROW10_I42_6 |
| 7 | NC — no connection |
| 8 | UNNAMED_16_DOUBLEROW10_I42_8 |
| 9 | UNNAMED_16_DOUBLEROW10_I42_9 |
| 10 | UNNAMED_16_DOUBLEROW10_I42_10 |
| 11 | GND |
| 12 | P214 |
| 13 | GND |
| 14 | P211 |
| 15 | GND |
| 16 | P210 |
| 17 | GND |
| 18 | P209 |
| 19 | GND |
| 20 | P208 |

## J21 — mikroBUS left

| Contact | Net |
| --- | --- |
| 1 | P000 |
| 2 | P303 |
| 3 | P205 |
| 4 | P204 |
| 5 | P202 |
| 6 | P203 |
| 7 | 3.3 V |
| 8 | GND |

## J22 — mikroBUS right

| Contact | Net |
| --- | --- |
| 1 | P111 |
| 2 | P409 |
| 3 | P614 |
| 4 | P613 |
| 5 | P512 |
| 6 | P511 |
| 7 | 5 V |
| 8 | GND |

## J23 — Arduino digital 0–7

| Contact | Net |
| --- | --- |
| 1 | P614 |
| 2 | P613 |
| 3 | P409 |
| 4 | P111 |
| 5 | P112 |
| 6 | P113 |
| 7 | P114 |
| 8 | P608 |

## J24 — Arduino digital 8–13 and I2C

| Contact | Net |
| --- | --- |
| 1 | P207 |
| 2 | P115 |
| 3 | P205 |
| 4 | P203 |
| 5 | P202 |
| 6 | P204 |
| 7 | GND |
| 8 | 3.3 V |
| 9 | P511 |
| 10 | P512 |

## J25 — Pmod 2

| Contact | Net |
| --- | --- |
| 1 | P413 |
| 2 | P411 |
| 3 | P410 |
| 4 | P412 |
| 5 | GND |
| 6 | 3.3 V |
| 7 | P400 |
| 8 | P404 |
| 9 | P708 |
| 10 | P408 |
| 11 | GND |
| 12 | 3.3 V |

## J26 — Pmod 1

| Contact | Net |
| --- | --- |
| 1 | P206 |
| 2 | P203 |
| 3 | UNNAMED_6_DOUBLEROW6PMODIF_I125 |
| 4 | UNNAMED_6_DOUBLEROW6PMODIF_I1_1 |
| 5 | GND |
| 6 | UNNAMED_6_CAP_I127_A |
| 7 | P905 |
| 8 | P311 |
| 9 | P301 |
| 10 | P302 |
| 11 | GND |
| 12 | UNNAMED_6_CAP_I127_A |

## J27 — Grove 1

| Contact | Net |
| --- | --- |
| 1 | P415 |
| 2 | P414 |
| 3 | 3.3 V |
| 4 | GND |

## J28 — Grove 2

| Contact | Net |
| --- | --- |
| 1 | P505 |
| 2 | P506 |
| 3 | 3.3 V |
| 4 | GND |

## J29 — On-board debugger isolation

| Contact | Net |
| --- | --- |
| 1 | P110 |
| 2 | UNNAMED_16_DOUBLEROW10_I42_8 |
| 3 | P109 |
| 4 | UNNAMED_16_DOUBLEROW10_I42_6 |
| 5 | P108 |
| 6 | UNNAMED_16_DOUBLEROW10_I42_2 |
| 7 | P300 |
| 8 | UNNAMED_16_DOUBLEROW10_I42_4 |

## J30 — Qwiic

| Contact | Net |
| --- | --- |
| 1 | GND |
| 2 | 3.3 V |
| 3 | P414 |
| 4 | P415 |

## J31 — Target USB HS

| Contact | Net |
| --- | --- |
| 1 | UNNAMED_17_MICROUSBAB_I45_VBUS |
| 2 | UNNAMED_17_MICROUSBAB_I45_DM |
| 3 | UNNAMED_17_MICROUSBAB_I45_DP |
| 4 | NC — no connection |
| 5 | GND |
| 6 | GND |
| 7 | GND |
| 8 | GND |
| 9 | GND |
| 10 | GND |
| 11 | GND |

## J33 — CAN bus

| Contact | Net |
| --- | --- |
| 1 | UNNAMED_18_RES_I31_A |
| 2 | UNNAMED_18_RES_I32_A |
| 3 | GND |


