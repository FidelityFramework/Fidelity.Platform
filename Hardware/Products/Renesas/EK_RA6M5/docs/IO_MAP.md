# EK-RA6M5 I/O map and credential bring-up

This map describes the **R7FA6M5BH3CFC, LQFP176**, with the EK-RA6M5 v1 design's
**D017572_04 issue 3.0 MP schematic**. The user's physical silicon identity has
been read; the PCB artwork revision has not been independently inspected. Wiring
below is the pinned design's wiring. HelloBlinky has exercised GPIO, the two
button IRQ routes, SysTick and the SWD programming path. The other interfaces are
mapped, not implemented or electrically qualified by this work.

## Maintained declarations

- [PackagePins.clef](../../../../Silicon/MCU/Renesas/RA6M5/R7FA6M5BH3CFC/PackagePins.clef): all 176 physical package pins, GPIO names,
  system/debug functions, external bus, interrupts, serial interfaces, timers,
  analog channels and touch functions from DS-1.50 Table 1.16. Alternative
  functions are capabilities, not simultaneous assignments. Select the PSEL,
  PMR, ASEL/ISEL and electrical settings from HW-1.50 Tables 19.5–19.16 for the
  chosen function; a function name alone does not configure it.
- [BoardNets.clef](../BoardNets.clef): all 1,149 terminal-to-net facts in the MP
  netlist, plus the 38 trace-link defaults and the ordinary not-fitted footprints.
  This includes the MCU, connectors, passives, switches, LEDs, external memories,
  PHY, CAN transceiver and debug MCU. It preserves separate nets across components.
- [CONNECTOR_MAP.md](CONNECTOR_MAP.md): every one of the 355 connector contacts,
  including all 160 J1–J4 native-header contacts and explicit no-connects.
- [Registers.clef](../../../../Silicon/MCU/Renesas/RA6M5/R7FA6M5BH3CFC/Registers.clef): the 27 initial register transaction
  requirements; application grants select the checked handles. The larger physical map does not claim that every peripheral register
  or driver is implemented.

These source declarations are compiled with the platform. They are not emitted
as live application tables when unused. There is no script-generated build step
and no dependency on the local design package for application compilation.

## Board interfaces

| Interface | MCU connection | Configuration and shared ownership |
| --- | --- | --- |
| User LEDs | Blue P006, green P007, red P008 | Active high; E27/E26/E28 closed by default. Sheet 2 shows R32 = 110 Ω, R34/R33 = 2.37 kΩ. Equal PWM duty is not equal perceived brightness. These pins are also AN006/007/008 and have no GPT alternate. |
| User buttons | S1 P005 / IRQ10-DS; S2 P004 / IRQ9-DS | E31/E32 closed; external R39/R40 10 kΩ pull-ups; press grounds input. Also AN005/004, so they cannot be treated as unconnected analog inputs. S3 drives RESET#. |
| Grove 1 / Qwiic | P415 SCL2, P414 SDA2 | J27 and J30 share IIC2 and fitted 1.5 kΩ pull-ups R16/R30. Pmod1 can be rerouted onto these same wires. |
| Grove 2 | J28-1 P505 SCL6/AN121; J28-2 P506 SDA6/AN122 | SCI6 simple-I2C, not a fourth dedicated IIC controller. R11/R79 1.5 kΩ pull-up footprints are **DNF** in the BOM. Fit for I2C as required; leave absent for analog. Actual population must match the intended experiment. |
| Arduino analog / mikroBUS AN | A0 P000, A1 P001, A2 P002, A3 P003, A4 P014, A5 P015 | J19 AN000/001/002/003/012/013. P000 also reaches mikroBUS J21-1. ADC0/ADC1 alternatives on P000–P002 are listed in PackagePins; do not infer routing from channel numbers. |
| Arduino / mikroBUS I2C | P511 SDA1, P512 SCL1 | J24-9/10 and J22-6/5 share IIC1. |
| Arduino / mikroBUS UART | P614 RXD7, P613 TXD7 | J23-1/2 and J22-3/4 share SCI7. |
| Arduino / mikroBUS SPI | P205 SSLA0, P204 RSPCKA, P202 MISOA, P203 MOSIA | J24-3..6 and J21-3..6 share SPI0. Pmod1 also shares the data/clock pads. |
| Arduino / mikroBUS GPIO | P303 reset, P409 IRQ6, P111 GTIOC3A/IRQ4 | P303 is a shield reset GPIO, **not the MCU RESET# net**. Other Arduino pins: D4 P112, D5 P113, D6 P114, D7 P608, D8 P207, D9 P115. |
| Pmod1 | J26: P206 SS/CTS, P203 TX/MOSI, P202 RX/MISO, P204 SCK; P905 IRQ8, **P311 reset**, P301/P302 extra selects | SPI0 or SCI9 alternatives. E23/E24 closed; open them before closing E37/E38 to move contacts 3/4 to IIC2. E25 supplies 3.3 V; E36 is the alternative 5 V path. Never close competing source links together. |
| Pmod2 | J25: P413 SS/CTS, P411 TX/MOSI, P410 RX/MISO, P412 SCK; P400 IRQ0, P404 reset, P708/P408 GPIO | SPI1 or SCI0; 3.3 V supply. A possible future sensor interface without taking the current LED/button pads; sensor voltage and protocol still determine suitability. |
| USB FS target | J11; dedicated USB_DM/DP, P407 VBUS sense | Device default: J12 2–3, J15 closed. Host: J12 1–2, J15 open. Sheet 3 has ESD/filter/series parts and divided VBUS sensing. J10 is the separate S124 debugger's USB, not this controller. |
| USB HS target | J31; dedicated USBHS_DM/DP, PB01 VBUS sense | Device default: J7 2–3, J17 closed. Host: J7 1–2, J17 open. Sheet 13 also routes PB00 overcurrent and P707 VBUS enable. Both USB ports need proper clock and controller initialization. |
| Ethernet | P401 MDC, P402 MDIO, P403 reset, P405 TX_EN, P406 TXD1, P700 TXD0, P701 REF50CK, P702 RXD0, P703 RXD1, P704 RX_ER, P705 CRS_DV, P706 IRQ7 | KSZ8091RNB PHY U4 / J5, 25 MHz PHY crystal. E8–E10/E14–E22 closed. Opening only one bridge does not isolate the whole PHY. Buffer ownership and RA S-cache coherency precede DMA use. |
| QSPI memory | P306 CS, P305 CLK, P307..P310 DQ0..DQ3 | U2 MX25L25645G, 32 MiB. These pads also appear on native headers; the memory is still attached. Mapping the flash does not initialize XIP or establish secure credential storage. |
| OSPI memory | P602 CS, P615 reset, P100 CLK, P104 DQS; DQ0..7 = P106/P102/P601/P107/P600/P105/P103/P101 | U3 MX25LM51245GM, 64 MiB. External memory, not secret storage merely because it is on-board. |
| CAN | P611 standby, P610 receive, P609 transmit | TJA1042T/3 U10; E33/E34/E35 **open** by default. J33: 1 CANH, 2 CANL, 3 GND. Sheet 15 shows split 60.4 Ω termination resistors. |
| Debug and trace | P108 SWDIO/TMS, P300 SWCLK/TCK, P109 SWO/TDO, P110 TDI; P214 TCLK, P211/P210/P209/P208 trace data | J13/J20 and J29 routes. These pads remain reserved while their debug functions are in use. J14 is a DNF debug-MCU adapter footprint. |
| Clocks / boot / references | P212 EXTAL, P213 XTAL, P201 MD, P200 NMI, XCIN/XCOUT, analog rails | E7/E11 connect the 24 MHz crystal; E12/E13 header alternatives are open by default. J16 open selects normal single-chip boot. Analog rail/reference bridges E1–E6 are closed by default. Native headers do not make these independent floating inputs. |

## Corrections to older assumptions

1. BOARD-1.01 Table 13 duplicates P905 for J26-7/8. Schematic sheet 6 and the
   netlist agree that J26-8 is **P311**; only J26-7 is P905.
2. BOARD-1.01 Table 11 duplicates AN121. Schematic sheet 6, DS-1.50 Table 1.16
   and HW-1.50 Table 19.10 agree on **P506 = AN122**.
3. The native header contacts P212/P213 are on the far side of normally open
   E13/E12. A port name printed by a header does not imply an active connection.
4. Trace-cut bridges appear as DNF entries in the component BOM because they are
   copper features. Their electrical defaults come from the trace-link design,
   not a rule that treats every DNF entry as an open circuit.

## Credential work this enables

Start a FIDO transport experiment at **J11 USB FS**, retaining J10 for debugging.
The current buttons and LEDs cover physical confirmation and status. The next
implementation work is USB device enumeration/HID, bounded CTAPHID/CBOR parsing,
credential operations, and PIN-based verification. A fingerprint sensor is an
optional later peripheral; reserve an interface only once its voltage, host
protocol and matching/enrollment boundary are chosen.

For analog entropy experiments, Grove 2 provides P505/P506 on distinct ADC1
channels with the documented pull-up population caveat. The map does not validate
the analog front end, sample independence, health tests or entropy estimate.
ADC/ELC/DTC, ownership of sample buffers, clock accuracy and cache treatment remain
separate bring-up work. GPIO switch/LED pins are already electrically loaded.

For persistent credentials, first define the erase/program granularity, power-loss
behavior, integrity, rollback policy and key-protection boundary. The MCU's
TrustZone/SCE capabilities and external flash capacity do not by themselves prove
non-exportability or a FIDO security level. No debug lock, option memory, lifecycle
or external-memory programming was performed by this mapping work.

## Validation

From the Composer checkout, run the F# test against the untracked local design package:

```sh
dotnet run --project tests/IOMap/IOMap.Tests.fsproj -- "/path/to/ek-ra6m5-v1-designpackage/Design Files - Cadence/ek_ra6m5_archive/worklib/ek_ra6m5/packaged/pstxnet.dat"
```

It loads HelloBlinky through CCS and projects the checked `.clef` declaration arrays. Production platform sources are not compiled as F# by the test runner. It checks the netlist hash and every terminal, unique package coverage and U1 pin
labels, all four complete 40-contact headers, and all 38 trace links. This check
does not build firmware or program the board. The MP source artifacts remain local;
their hashes and relative locations are in [SOURCE_MANIFEST.md](SOURCE_MANIFEST.md).
