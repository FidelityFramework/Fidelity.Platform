# STM32H747I-DISCO source and hardware audit

Audit date: **2026-09-12**. This is evidence for design and scaffolding, not a
selectable target, accepted driver, compiled proof, or hardware test result.
PDF contents, vendor source, BOM rows, and selected schematic text were inspected;
no target probing, reset, flashing, or option-byte operation was performed.
The inventory includes files ignored by Git and was rechecked immediately before
writing. Absence means absent from the audited source locations, not absent from
every directory on the host.

## Path keys and scope

- `F/` means `/home/hhh/repos/Fidelity.Platform/`.
- `B/` means `/home/hhh/repos/MCU/ST/STM32H747I-DISCO/`.
- `BSP` means `B/reference/stm32cubeh7/stm32h747i-disco-bsp/`.
- `HDR` means `B/reference/stm32cubeh7/cmsis-device-h7/Include/stm32h747xx.h`.

These prefixes and suffixes identify exact local paths; `B/` sources are a sibling
checkout and are not packaged by a Fidelity.Platform manifest. No applicable
`AGENTS.md` was found in the inspected ancestors or hardware subtrees.
Datasheet dates, document revisions, PCB revisions, and silicon revisions are
distinct identities.

The session's separate online check identified **UM2411 Rev 8, July 2026** at
[ST's UM2411 URL](https://www.st.com/resource/en/user_manual/um2411-discovery-kit-with-stm32h747xi-mcu-stmicroelectronics.pdf).
That is a separate source revision from the hashed **local Rev 7** inspected here;
the local PDF was preserved. Reconcile the revision delta before using updated
table/page references. An independent fetch during this audit timed out.

## Document inventory

Hashes identify the bytes inspected, not correctness or vendor authenticity.

| Exact path using keys above | Document / revision | Bytes | SHA-256 |
| --- | --- | ---: | --- |
| `F/Hardware/Products/ST/STM32H747I_DISCO/docs/open_platform_license_agreement.pdf` | ST Open Platform License Agreement; no numbered revision read | 43549 | `0809c990f7a3b971bc9517e20d93c3da880b46d30505409c47f61b371703d2a0` |
| `F/Hardware/Products/ST/STM32H747I_DISCO/docs/stm32h747i-disco.pdf` | DB3608 Rev 3, August 2025; product data brief | 2527504 | `357f579efabc5fcaff506060ba62ed9c8eef461704a5f9e7c741cab42cc1aaad` |
| `F/Hardware/Products/ST/STM32H747I_DISCO/docs/tn1238-stmod-interface-specification-stmicroelectronics.pdf` | TN1238 Rev 3, October 2021 | 3741598 | `164059c708e39aefdd3b6f362e974072edc7ce0c9482af0b5fec3a04ae40c1b1` |
| `F/Hardware/Products/ST/STM32H747I_DISCO/docs/um2411-discovery-kit-with-stm32h747xi-mcu-stmicroelectronics.pdf` | UM2411 Rev 7, September 2025 | 1524258 | `25e8ab66fb10bca5bab91f751fb5a9e9aaef21ced761f4397cf52b5bc3d2440a` |
| `F/Hardware/Silicon/MCU/ST/STM32H7/STM32H747XIH6/docs/stm32h747i-disco.pdf` | DB3608 Rev 4, May 2026; product data brief, filed under silicon part | 2530486 | `dc8261a0ed079bbcee86c485ddf617e5d7b9e8bcd3d823e8596019d41b1c0c90` |
| `F/Hardware/Silicon/MCU/ST/STM32H7/docs/stm32h747ag.pdf` | DS12930 Rev 3, January 2026; STM32H747xI/G silicon datasheet | 5085716 | `edc4b20830a15c82da5824e8c8d69f3c950aae179cae4db680df0e590f54e909` |
| `F/Hardware/Silicon/MCU/ST/STM32H7/docs/svd/STM32H7x7_CM4.svd` | CMSIS-SVD schema 1.1, device version 1.0; community mirror provenance below | 4457344 | `9394ef86c8eae15d78df1ef38d17392044d727187ff7ab54f95fd70c468609c2` |
| `F/Hardware/Silicon/MCU/ST/STM32H7/docs/svd/STM32H7x7_CM7.svd` | CMSIS-SVD schema 1.1, device version 1.0; community mirror provenance below | 4468471 | `39f2cb32c6fef206096eaec234b8d44a4e500a42d0221765751c0430b1d372cb` |
| `F/Hardware/Silicon/CPU/Arm/CortexM7/docs/pm0253-stm32f7-series-and-stm32h7-series-cortexm7-processor-programming-manual-stmicroelectronics.pdf` | PM0253 Rev 6, May 2026 | 2757322 | `9dd51a3a0a9f8a4ed282e41c74fba8d2d84baa55374a3048849f716e00b6be62` |
| `B/reference/datasheets/armv7m-arm-ddi0403e.pdf` | Arm DDI 0403E.e, ID021621 | 5957561 | `76500176d20f897eaf05eeadb5a6202cef641e332073b107905c8898e0ee0747` |
| `B/reference/datasheets/ft6x06.pdf` | FocalTech D-FT6x06-1207-V01 v0.1, 2012-07-23 | 1214674 | `f8b7d1dca2367dad460f3b98675e4d067822c95246ea92d03b42e64a4b7e6bef` |
| `B/reference/datasheets/ft6x06_app_note.pdf` | FT6x06 CTPM application note v0.1, 2012-07-26 | 451632 | `b5aeb91d0f2e704d2bc759334742859e7699fb56ea609d6134e97d98050b8b99` |
| `B/reference/datasheets/lan8742a.pdf` | Microchip DS00001989A | 1055830 | `6bba9e47b1297f53069e8a9336cf8a719f3ed0a2a1f6a00eba83fcb006e07554` |
| `B/reference/datasheets/usb3320.pdf` | Microchip DS00001792E | 1011525 | `3f0404ef4097969756ad0abfc800c8731c3602e7acf274c0223c88a939cfc0db` |
| `B/reference/datasheets/wm8994.pdf` | Cirrus WM8994 Rev 4.6, July 2018 | 5379652 | `74a9351a183ae04f2811e8f5b88eaff39fb222c51700b0d0d54137062102ffbe` |
| `B/mb1166-designpackage/mb1166-bom/MB1166-Default-A08_BOM.xlsx` | BOM revision named in filename; workbook read | 25774 | `005a4179b6223ebe6f10b009b78cab72e61f9c4d5f76d94f5423b1065fa108e3` |
| `B/mb1166-designpackage/mb1166-bom/MB1166-Default-A09_BOM.xlsx` | BOM revision named in filename; workbook read | 34605 | `73ded1d1467054dc0aeef9779bd0d82520d2a239fe8b2c64f9d4540dc67541da` |
| `B/mb1166-designpackage/mb1166-bom/MB1166-Default-A10_BOM.xlsx` | BOM revision named in filename; workbook read | 27897 | `9c7ae27a2f727d6c90965189e1b31486e5f9e6c05086b1bbc001a7b7ee659b3b` |
| `B/mb1166-designpackage/mb1166-default-a10-schematic.pdf` | MB1166 schematic, A-10 | 321281 | `78ca0e810cf154321d80d4a2af812f6337324113fcd5232113ef841800d95277` |
| `B/mb1280-designpackage/mb1280-3v3-c01-schematic.pdf` | MB1280 3V3 schematic, C-01 | 1505736 | `af638cc64a7e37f75d0cef0262c039ee4336c4d4e901279bc1c6fdf70141e0fb` |
| `B/mb1280-designpackage/mb1280-bom/MB1280-3V3-C01_BOM.xlsx` | BOM revision named in filename; workbook read | 24510 | `dfe56284c93e3b28a01a55464913827420ffcb13159d3c324c8576c5fd726c06` |
| `B/mb1280-designpackage/mb1280-bom/MB1280-A0x_BOM.xls` | Older BOM, inventoried only; workbook not decoded in this audit | 67072 | `583a98710eb537b80ac82f4e27499eb7d93415039ec1cedc966298dd7ad479b0` |
| `B/mb1280-designpackage/mb1280-bom/MB1280-B01_BOM.xlsx` | BOM revision named in filename; workbook read | 43169 | `5f5f0fdba9e9c5e478f6635d4fa577f11c572f75a1274da8f8774dd47e42d91b` |
| `B/mb1248-designpackage/mb1248_bom/MB1248-H747I-D02_BOM.xls` | Older BOM, inventoried only; workbook not decoded in this audit | 140800 | `e62b50d6397f06ea9df233d9799d1389f65a8d68eb97e932756e4ab64df8b225` |
| `B/mb1248-designpackage/mb1248_bom/MB1248-H747I-D03_BOM.xlsx` | BOM revision named in filename; workbook read | 40233 | `c176c69c8e4e0fa7e752e7466c935ed5da3fa369c26ba00ca613133281e8251e` |
| `B/mb1248-designpackage/mb1248_bom/MB1248-H747I-D04_BOM.xlsx` | BOM revision named in filename; workbook read | 36900 | `4ef1042e3ca5afeb88313f6e369858ae0ed2e5be54dd4efef2d19112a3e93029` |
| `B/mb1248-designpackage/MB1248D/Altium_Designer/Memory.SchDoc` | MB1248D Altium schematic source; not a rendered PDF | 183296 | `ed273265ed4bec44aa0dcd12e49c03aeb5f5b771ab74567a2b9138ed7dc01223` |
| `B/mb1248-designpackage/MB1248D/Altium_Designer/Audio.SchDoc` | MB1248D Altium schematic source; not a rendered PDF | 388096 | `eb675423aceb695933494ef2d63fd3e01ea5a85a5960b84fbb80c233013b023e` |
| `B/mb1248-designpackage/mb1248_manufacturing/MB1248_D/Gerber/MB1248D_180117_FAB.ipc` | IPC-D-356A manufacturing connectivity, header dated 2018-01-17 | 284212 | `f2af4c9b2e0fe2cbbc144b75d93b08278f999d749643bb6cdbe9fe36c94105de` |
| `B/mb1248-designpackage/mb1248_manufacturing/MB1248_D/ODB++/MB1248D_180813_FAB_odbjob.tgz` | ODB++ manufacturing archive; inventoried, not connectivity-verified | 2594203 | `36d5bd21eb6ecc340ca4c33b97d81307e7fc83236b8346d34e5ee9e2c8a7126f` |

MB1248 also contains the remaining Altium sheets, Allegro PCB, placement and
manufacturing files. The table hashes the audio/memory sheets and two connectivity
artifacts used or identified during this bounded audit; it is not a complete
manufacturing archive manifest. The supplied MB1166 and MB1280 circuit PDFs are
present. MB1248 has editable circuit schematics, but no rendered circuit PDF was
found; its mechanical, assembly, and fabrication PDFs are different documents.

### Requested sources still absent from the audited locations

| Source | Location searched | Consequence |
| --- | --- | --- |
| RM0399 reference manual | `F/Hardware/Silicon/MCU/ST/STM32H7/docs/` | Register semantics, bus-master access matrix, reset/power/clock sequencing and dual-core behavior need this authority before acceptance |
| ES0445 errata | Same family docs directory | No revision-specific workaround set can yet be accepted |
| AN4891, AN4839, AN4938, AN5286, AN5557 | Same family docs directory | Requested local PDFs absent; limited official online DMA/cache evidence was read below |
| AN4860, AN4861 (optional) | Same family docs directory | Additional display guidance remains unstaged |
| ST `stm32h7-svd.zip` | Same family docs directory | Current SVDs are interim files, not the requested ST archive |
| Cortex-M7 TRM DDI0489 | `F/Hardware/Silicon/CPU/Arm/CortexM7/docs/` and `B/reference/datasheets/` | Implementation-specific details beyond PM0253 remain to reconcile |
| IS42S32800J, MT25QL512 and IMP34DT05 component datasheets | `B/reference/datasheets/` | BOM identities and vendor drivers are present, but component timing/electrical authorities are incomplete |
| Panel-module/controller documentation for fitted MB1166 assembly | MB1166 design pack and reference datasheets | BOM module names and two vendor LCD drivers exist; exact fitted panel and touch firmware behavior remain unknown |

**DS12930 is already present:** `stm32h747ag.pdf` is the STM32H747xI/G
datasheet, despite its filename and family-level location. The part-directory
`stm32h747i-disco.pdf` is a newer **DB3608 board data brief**, not DS12930.
**PM0253 is already present**, and its Table 14 supplies H7-specific M7 facts.
The older CortexM7 missing-PM0253 claim was stale; the source manifest has now
been corrected during the initial GPIO package work.

## Pinned vendor implementation evidence

The local Git HEAD and worktree state were read directly. The repositories are
useful sequence and register oracles; this audit did not compile or link them.

| Local repository | Commit | Worktree at audit |
| --- | --- | --- |
| `B/reference/stm32cubeh7/cmsis-device-h7/` | `81db1ec63cdc191fae1565b772da3ea5aa29a683` | Clean |
| `B/reference/stm32cubeh7/stm32-ft6x06/` | `d4d40ad52b495b650222addb4549257c0b9c0059` | Clean |
| `B/reference/stm32cubeh7/stm32-is42s32800j/` | `d8069315a8ecdd218358d9ae67ca6c4312cf9ed5` | Clean |
| `B/reference/stm32cubeh7/stm32-lan8742/` | `1ce7c19488f0f6d44a2c1dc7033a23e88cd0bbc8` | Clean |
| `B/reference/stm32cubeh7/stm32-mt25tl01g/` | `dba8219e15cbdf5191d168ea4cfa730542bb56bf` | Clean |
| `B/reference/stm32cubeh7/stm32-nt35510/` | `0d3008be195d1b0750a6d33cc7e944eea61e2074` | Clean |
| `B/reference/stm32cubeh7/stm32-otm8009a/` | `c0229f087eb3b87a99b392a08b74d2a8933f4aa4` | Clean |
| `B/reference/stm32cubeh7/stm32-wm8994/` | `6681406de006714bb4c082cc2e9e31b563d4962e` | Clean |
| `B/reference/stm32cubeh7/stm32h747i-disco-bsp/` | `c61d8f01d3fa9a03b81c21ce83c0d334150ea3d4` | Clean |

Upstream identity is recorded in `B/reference/MANIFEST.md` as the correspondingly
named STMicroelectronics repositories. The concrete entry points used here are
`BSP/stm32h747i_discovery_audio.{h,c}`,
`stm32h747i_discovery_lcd.{h,c}`,
`stm32h747i_discovery_ts.{h,c}`,
`stm32h747i_discovery_bus.h`,
`stm32h747i_discovery_sdram.{h,c}`,
`stm32h747i_discovery_qspi.{h,c}`, and
`stm32h747i_discovery_sd.{h,c}`.

### SVD provenance and limits

`B/reference/MANIFEST.md` records the two family SVDs as downloaded from
`cmsis-svd/cmsis-svd-data`, `data/STMicro/`, branch `main`; it does not record a
source commit. The complete local hashes above now pin the inspected bytes.
The CM7 file declares CPU revision `r0p1`, while the part-specific `HDR` declares
`__CM7_REV = 0x0101` (`r1p1`). Thus even metadata needs reconciliation.
Neither an SVD device name nor a header-offset comparison establishes access
side effects, read-to-clear behavior, allowed widths, reserved-bit handling,
package applicability, or power-domain availability.

Compare the eventual ST archive against these files and the part header before
generating accepted descriptors. Pin the archive hash, member paths, source
version, generator version, and reviewed differences. A verifier described merely
as “Table 22 balls” needs its source ID: in the staged **DS12930 Rev 3, Table 22 is
run-mode current consumption**, not a ball assignment table. Figure 10 is the
TFBGA240+25 ballout. Preserve document-specific table references rather than
reusing numbering across sources.

## Physical identity and BOM applicability

UM2411 Rev 7 Table 1 identifies the DISCO product as **MB1248 mainboard + MB1166
LCD module + MB1280 STMod+ fan-out**. DISC1 omits the LCD module. The MCU is
STM32H747XIH6, with Cortex-M7 and Cortex-M4 and a TFBGA240+25 package. A product
order code alone does not identify the mainboard assembly or replacement panel.

The following are **BOM populations**, not observations of the connected board:

| Assembly evidence | Component | What the supplied BOM says |
| --- | --- | --- |
| MB1248 H747I D03 and D04 | U7 SDRAM | IS42S32800J-6BLI, 256 Mbit, 8M × 32 = 32 MiB |
| Same | U3 and U14 QSPI | Two MT25QL512ABB8ESF-0SIT devices, 512 Mbit each = 128 MiB combined |
| Same | U12 codec | WM8994ECS/R |
| Same | U21 digital microphone | One IMP34DT05TR |
| MB1166 A08 | LCD module | Frida FRD397B25009-D-CTK |
| MB1166 A09 | LCD module | Frida FRD400B25025-A-CTK |
| MB1166 A10 | LCD module | Frida FRD400B25021-B-CTQ |

D03 names MCU part STM32H747XIH6U; D04's manufacturer-part field names
STM32H747XIH6 while its description still contains the U-suffixed variant.
Record the physical component marking and assembly label before resolving that
difference. The A10 panel schematic lists several supported module part numbers;
the BOM selects a population. Do not infer a one-to-one controller mapping from
BOM module names alone. The BSP probes **NT35510 first, OTM8009A second**.

Still unobserved in this audit: board order-code sticker, MB1248 assembly revision,
MB1166 revision/module label, actual MCU device/revision ID, option bytes, live
power topology, solder bridges, panel/controller IDs, and touch I2C response.
The older reference manifest's ST-LINK/FAIL.TXT report is historical evidence;
it was not revalidated here.

### First-use power and display interpretation

UM2411 Rev 7 §§6.2–6.2.2, pp15–16, and Table 3, p18, distinguish these inputs:

| Connector / label | Role | JP6 selection when powering through it |
| --- | --- | --- |
| CN2, STLINK, Micro-B | Debugger/virtual serial USB and selectable board power | `STlk` for host enumeration (default); `CHgr` for a supply without enumeration |
| CN14, external 5 V, Micro-B | Board power | `U5V` |
| CN1, USB OTG_HS, Micro-AB | Target USB peripheral/host and selectable board power | `HS` when used as the power input |
| CN13, STDC14 | Debug header | Not a USB power connector |

For external CN14 power plus CN2 debugging, the manual specifies: select JP6
`U5V`, connect CN14 power, verify green **LD8**, then connect CN2 to the host.
Disconnect all supplies before moving a power-selection jumper as a handling
precaution; this phrasing is audit guidance rather than a quotation from Table 3.
The silkscreen labels determine the setting, not how the board is rotated.

LD8 indicates the board's +5 V supply. **LD10** on the underside is STLINK COM;
a blink or color there does not establish target power or application execution.
**LD9** is the STLINK overcurrent alarm. **LD5** is USB HS overcurrent, while
**LD7** indicates power supplied to an OTG peripheral (Figures 5/6; §§6.2.1, 6.6.2).
Identify the LED reference before interpreting a report of “red” or “green.”

Dark LCD is not a complete diagnosis. Table 23, p47, explicitly says product
identifications **DK32H747I$AT2 / $AT3 have no preloaded demonstration software**.
The table lists AT4 separately without that statement. Board power, live target
access, application contents, panel reset/backlight and panel initialization are
separate observations. A missing demo alone does not explain a failed SWD probe.

Section 6.2.3, p17, states the default core supply is **Direct SMPS** and warns
that a firmware SMPS/LDO configuration mismatching the populated hardware can
prevent debugger reconnection. Its BOOT0/R192 recovery procedure includes flash
erase. That is a recovery procedure for a diagnosed mismatch, not a necessary
first-use step inferred from a dark screen.

## Synth-relevant signal and memory chains

| Function | Documented chain and pins | Evidence and design consequence |
| --- | --- | --- |
| Synth playback | CPU-owned PCM buffer → DMA2 Stream1, request SAI1_A → SAI1 Block A → WM8994 → CN11 headphones; JP2/JP5 speaker outputs | BSP audio header around lines 191–238; UM2411 §§6.5, 7.8. SAI1 AF6: PG7 MCLK, PE5 SCK, PE4 FS, PE6 data. DMA selection is the BSP's assignment, not a uniquely hardwired stream |
| Codec control | I2C4, PD12 SCL / PD13 SDA, AF4 → WM8994 | BSP audio header line 141 and bus header lines 75–81; HAL address `0x34` corresponds to canonical 7-bit `0x1A`. Codec register addressing is 16-bit in BSP bus calls |
| Audio input via codec | CN10 line input → WM8994 → SAI1 Block B on PE3 AF6 → DMA2 Stream4, request SAI1_B | BSP audio header lines 243–261 and UM2411 §§6.5, 7.7. Exact jack-contact routing still needs schematic connectivity extraction |
| Onboard PDM microphone | IMP34DT05TR → PC1 data / PE2 clock, AF10 SAI4 → SAI4 Block A → BDMA Channel1 | BSP audio header lines 270–298; D03/D04 BOM; UM2411 §6.5.1/Table 4. Use D3-accessible capture buffers and model jumper-dependent codec routing separately |
| Framebuffer and scanout | SDRAM → LTDC → DSI host/wrapper → two DSI data lanes → MB1166 controller/panel | BSP LCD init and `MX_DSIHOST_DSI_Init`; UM2411 Table 16. Landscape default 800 × 480. DMA2D is an optional memory compositor; it is not the DSI transport |
| Display sideband | PG3 reset; PJ12 backlight; PJ2 tearing-effect signal; dedicated DSI PHY balls | BSP LCD header lines 77–94; UM2411 Table 16. Model reset/backlight and panel DCS initialization in addition to LTDC register layout |
| Touch/UI input | MB1166 touch controller → I2C4 PD12/PD13; PK7 interrupt → EXTI7 / EXTI9_5 | BSP TS header lines 51–91 and `FT6X06_Probe`. Probe addresses are HAL `0x54` then `0x70`, canonical 7-bit `0x2A` then `0x38`; physical response is unobserved |
| External working memory | FMC SDRAM, 32-bit data, 12 row bits, 9 column bits, four internal SDRAM banks, 32 MiB | BSP SDRAM init. BSP selects FMC SDRAM bank 2 at `0xD0000000`; resolve manual discrepancy below before accepting the profile |
| Persistent assets | QUADSPI → two 512-Mbit MT25QL512 devices; BSP uses dual-flash operation and MT25TL01G component driver | D03/D04 BOM; BSP QSPI init; component header `MT25TL01G_FLASH_SIZE = 0x08000000`. Driver name does not change physical BOM population |
| Removable assets | SDMMC1 → CN12 microSD: PC8–PC12 data/clock and PD2 command; PI8 card detect, active low | UM2411 §7.9/Table 13; BSP SD header. PC9/PC11 are shared with the camera connector |

The TDM codec configuration can activate separate headphone and speaker slot
pairs: BSP uses slots 0/2 for headphone and 1/3 for speaker; all four permit
separate stereo streams. PCM channel order, frame length, slot masks, DMA transfer
width, and codec routing must agree. The audio header lists 16- and 32-bit formats
and rate constants; their presence is not validation of every rate/route pairing.
Synth output does not require the microphone path.

The BSP uses PLL2 for SAI1 and SAI4 audio clocks, and PLL3 for LTDC pixel clock.
Treat PLL outputs as shared system resources. Timing constants and sample rates
must be derived from the selected oscillator, divider configuration and device
limits, then measured. The SDRAM BSP timings explicitly assume a **100 MHz SDRAM
clock**; they are not generic constants for any core frequency.

Touch BSP exposes up to two contacts plus weight, area and event fields.
These fields do not establish calibrated force, pressure, velocity, or reliable
musical aftertouch. Coordinate transforms, contact tracking, latency and any use
of weight as expression need tests against the fitted module. Ordinary on-screen
knobs and keys can be designed without assuming force measurement.

The native implementation boundary can remain small: BAREWire memory/register
views, interrupt/DMA ownership, codec I2C transactions, panel DCS transactions,
touch decoding, and framebuffer primitives. The BSP's microphone PCM conversion
calls external `PDM_Filter*` APIs; that optional PDM-decimation decision is separate
from oscillator/filter/envelope DSP for the synthesizer. C source can serve as an
auditable sequence reference without creating a runtime C dependency.

## LCD power follow-up

MB1166 A10 schematic sheet 2 supplies the daughterboard from the mainboard
through CN15: pins 21/23 carry +5 V to the STLD40DPUR backlight driver, pin 36
carries 3.3 V for display/touch logic, and pin 57/PG3 resets display and touch.
The A10 population routes backlight enable from the panel's CABC output through
fitted R1 (0 ohms). R4, the alternative route from MCU BL_CTRL/PJ12 at CN15
pin 53, is not fitted; R5 pull-up is also not fitted. Therefore the BSP's PJ12
write alone cannot be treated as the A10 backlight-enablement protocol. Panel
initialization is required. Verify the physical module's revision/population
before selecting this wiring fact. No separate USB cable powers the LCD.

## Memory and coherency constraints

DS12930 Rev 3 §3.3.2, p23 supplies the part's RAM breakdown: 64 KiB ITCM and
128 KiB DTCM; D1 AXI SRAM 512 KiB; D2 SRAM1 128 KiB, SRAM2 128 KiB, SRAM3 32 KiB;
D3 SRAM4 64 KiB; backup SRAM 4 KiB. Its TCM access description identifies Cortex-M7
and MDMA. PM0253 Rev 6 Table 14, p31 explicitly supplies the STM32H74x/75x M7
configuration: 16 MPU regions, single/double-precision FPU, 16 KiB instruction
and data caches, and the TCM sizes above. These now have local authority rather
than only header assertions.

ST's technical article explicitly applying to STM32H74x/H75x confirms that
DMA1/2 cannot reach ITCM or DTCM, and BDMA reaches only D3 slaves. Thus audio
DMA2 buffers must not be allocated in DTCM, and SAI4/BDMA capture buffers need
D3-accessible memory such as SRAM4. This is a bus reachability property, separate
from bounds and CPU MPU permissions. [ST DMA/BDMA interconnect explanation](https://community.st.com/stm32-mcus-60/the-most-probable-reason-to-have-an-issue-with-dma-or-bdma-transfers-on-stm32h7-142147)

CPU caches and DMA do not automatically present identical data. Explicit cache
maintenance or suitable noncacheable memory attributes are required when sharing
buffers. [ST AN4839 cache coherency discussion](https://www.st.com/resource/en/application_note/an4839-level-1-cache-on-stm32f7-series-and-stm32h7-series-stmicroelectronics.pdf)

Design implication: memory contracts must include the master, reachable domain,
CPU cache policy, transfer direction, ownership interval, alignment, and completion
event. A transmit handoff must make CPU writes visible before DMA starts.
A receive handoff must prevent dirty CPU lines from overwriting device results
and make completed writes visible before CPU consumption. The complete sequence
requires the chosen cache API, cache-line ownership, barriers, and target checks.
A bounds proof alone is insufficient.

For display planning, one 800 × 480 RGB565 framebuffer is 768,000 bytes; two need
1,536,000 bytes. Two 32-bit framebuffers need 3,072,000 bytes. These are calculated
storage requirements, excluding metadata and alignment. The external 32 MiB is
suitable capacity for this design candidate, but LTDC/DMA2D/CPU contention and
audio deadline behavior still need measurement. Framebuffer publication and
DMA completion belong in the proof boundary.

## Conflicts and stale claims to resolve

1. **FMC SDRAM bank:** UM2411 Rev 7 §6.8 says “Bank1.” The pinned BSP selects
   `FMC_SDRAM_BANK2`, `FMC_SDRAM_CMD_TARGET_BANK2`, and address `0xD0000000`.
   The supplied Altium `Memory.SchDoc` contains `SDNE1` and `SDCKE1` net labels,
   which support the BSP's second-bank interpretation. This audit extracted
   labels, not complete connectivity. Prefer the BSP bank/address as a candidate;
   reconcile the netlist and selected MCU balls, then memory-test before acceptance.
2. **Microphone identity:** the BSP comment names MP34DT01TR, while both supplied
   D03/D04 BOMs name **IMP34DT05TR**. Retain population variants until the board
   assembly is identified; do not promote a code comment over the BOM.
3. **Ethernet/mic conflict:** UM2411 §6.7/Table 5 says the default PC1 routing
   serves microphone data (SB8 open, SB21 closed), preventing Ethernet MDC.
   PE2 microphone clock also has an Ethernet interrupt routing option. A board
   profile must encode bridge choices rather than claim all functions coexist.
4. **Shared I2C4:** codec, LCD-module touch, camera and expansion connectors share
   PD12/PD13 (UM2411 §6.5 and connector tables). One bus owner/arbitration policy
   and canonical 7-bit addresses prevent colliding initialization and double shifts.
   The panel's DSI DCS path is distinct from touch I2C control.
5. **Stale source inventory:** the initial audit found CortexM7's source manifest
   incorrectly listing PM0253 as missing; that entry has now been corrected.
   The connector map also incorrectly claimed no MB1248 schematic or netlist
   was on disk and linked absent `IO_MAP.md`/`SOURCE_MANIFEST.md` files. Its
   provenance and references now point to this audit and explicitly mark the
   remaining MCU-side connectivity work. This audit does not replace a
   completed net map.
6. **Unproven panel-revision mapping:** older reference notes associate particular
   MB1166 revisions with OTM8009A/NT35510. The present BOMs supply module part
   numbers, and BSP supplies a two-controller probe. Their association with the
   physical unit remains an observation to record.
7. **Proof scope:** source hashes, offsets, pin maps and BAREWire layouts are
   different evidence from startup execution, DMA coherency, interrupt behavior,
   real-time deadlines, and floating-point error bounds. Keep each acceptance
   claim separate; none was hardware-tested in this audit.

## Next evidence gates

Record physical board/panel identities; stage and hash RM0399 and ES0445; reconcile
the part/header/SVD register semantics and interrupt inventory; derive the board
net and solder-bridge graph from the supplied design files; and establish a
minimal clock/power/boot configuration. Then accept small chains in order:
core identity and internal memory, serial diagnostic, codec control and audible
PCM, SDRAM, panel scanout, touch, and concurrent audio/UI operation. Optional
microphone, removable media, external flash and second-core work can be accepted
independently after their required chains. No earlier blanket “complete profile”
claim should bypass these gates.


## Appended native display milestone — 2026-09-12

The source inventory and open gates above remain the original audit. Subsequent
native bring-up established a bounded display chain on the connected board:
NT35510 ID `0x80`, an internal AXI RGB565 framebuffer, DSI color bars, and a
visible orange Clef glyph confirmed by the user after a live timing correction.
The accepted NT timing tuple is H sync/back/front `2/34/34`, V `120/150/150`,
matching the actual discovery BSP. The component header's landscape aliases
swap the axes and produced a backlit blank screen in the first image.

The corrected 17,370-byte glyph image was then programmed with OpenOCD verify,
read back exactly, and reset. The retained observation records stage 20,
controller 128, frame `0x24000040`, byte count 346112, no recorded fault and
DSI ISR0/ISR1 zero. This image remains a separate fallback. Evidence resides in the board
workspace's `recovery/2026-09-12-static-display-bsp-timings-w7nhibgb/`.
An earlier halted OpenOCD framebuffer capture independently matched every
pixel; hot-plug `st-flash` reads had produced unreliable zero/truncated data.

The separate 640 × 360 banner was subsequently programmed as a 475,936-byte
image. Both ELF load segments match flash exactly; the complete 460,800-byte
AXI framebuffer matches the pinned banner asset SHA256
`993c729068e047981ee1d3dcb6c264172997cec0e290eeea21a809979a340244`.
Its frame occupies `0x24000040..0x24070840`; all static storage remains below
the 8 KiB stack. Reset telemetry records stage 20, NT35510 ID 128 and zero
DSI/LTDC errors. The user confirmed its appearance and that the banner returned
after CN2 disconnect/reconnect with OpenOCD disconnected.
`recovery/2026-09-12-banner-display-eerblu_w/readback-checks.json` and
`reset-acceptance.json` retain the machine observations. Whole-BIN equality is
not claimed: only the eight unallocated gap bytes at offsets `0x298..0x29F`
differ (`0xFF` after ELF programming, `0x00` in objcopy output).

See the [static display profile](../../../../../Profiles/STM32H747I_DISCO_HelloDISCO_Display/README.md) for exact clock, memory,
stack, frame ownership and artifact hashes, and the
[display source audit](../../../../Silicon/MCU/ST/STM32H7/STM32H747XIH6/docs/display/SOURCES.md) for pinned CMSIS/HAL/BSP sources and the
preserved 80-register offset oracle. A broad DSI dump can consume `GPDR` and
create a Generic Payload Read Error; targeted passive snapshots exclude it.

This milestone identifies the observed controller, not the physical panel
assembly revision. It does not accept touch, SDRAM, audio, concurrent painting,
cache-enabled sharing, or the full MCU catalog. RM0399/ES0445 and the remaining
source/semantic gates above remain open.
