# Source Manifest: EK-RA6M5

Reconciled September 9, 2026. This is the source inventory for the platform package,
not a claim that all documented endpoints are implemented. The package is intended
to cover the board comprehensively, as ArtyA7 does. HelloBlinky consumes the first
small verified subset. [IO_MAP.md](IO_MAP.md) and [CONNECTOR_MAP.md](CONNECTOR_MAP.md) now cover the full package and board wiring.

The register review uses **HW-1.50**; the board wiring review uses **BOARD-1.01**.
The package already held revision 1.40 MCU PDFs. The later MCU and board PDFs remain
in the credential source pack and are linked directly, avoiding another undocumented
copy. Their exact bytes are pinned below. The entire 1.40 → 1.50 change history has
not been reconciled; only explicitly cited facts have been checked in this pass.

## Documents

| Source ID | Revision | Local artifact | Use / scope |
| --- | --- | --- | --- |
| HW-1.50 | R01UH0891EJ0150, Rev.1.50 | [r01uh0891ej0150-ra6m5.pdf](../../../../../../post-quantum-credential/hardware/ek-ra6m5/docs/r01uh0891ej0150-ra6m5.pdf) | Working reference for the register review; sections cited in HELLOBLINKY_HARDWARE.md |
| BOARD-1.01 | R20UT4829EG0101, Rev.1.01 | [r20ut4829eg0101-ek-ra6m5-v1-um_mp.pdf](../../../../../../post-quantum-credential/hardware/ek-ra6m5/docs/r20ut4829eg0101-ek-ra6m5-v1-um_mp.pdf) | Device, control wiring and trace-cut jumpers; §§5.2, 5.5 |
| DS-1.50 | R01DS0366EJ0150, Rev.1.50 | [r01ds0366ej0150-ra6m5.pdf](../../../../../../post-quantum-credential/hardware/ek-ra6m5/docs/r01ds0366ej0150-ra6m5.pdf) | Device/package and electrical reference; full electrical reconciliation remains open |
| HW-1.40 | R01UH0891EJ0140, Rev.1.40 | [r01uh0891ej0140-ra6m5.pdf](r01uh0891ej0140-ra6m5.pdf) | Previously staged revision retained; do not silently substitute its section numbering for 1.50 |
| DS-1.40 | R01DS0366EJ0140, Rev.1.40 | [r01ds0366ej0140-ra6m5.pdf](r01ds0366ej0140-ra6m5.pdf) | Previously staged datasheet retained |
| EXAMPLES-1.53 | R20AN0619EU0153, Rev.1.53 | [r20an0619eu0153-ek-ra6m5-exampleprojects.pdf](r20an0619eu0153-ek-ra6m5-exampleprojects.pdf) | Vendor example guide; runtime/HAL code is reference material |
| QSG-1.01 | R20QS0021EG0101, Rev.1.01 | [r20qs0021eg0101-ek-ra6m5-qsg.pdf](../../../../../../post-quantum-credential/hardware/ek-ra6m5/docs/r20qs0021eg0101-ek-ra6m5-qsg.pdf) | Stock example and board setup reference |
| SCACHE-1.40 | R11AN0538EU0140, Rev.1.40 | [r11an0538eu0140-ra-s-cache.pdf](../../../../../../post-quantum-credential/hardware/ek-ra6m5/docs/r11an0538eu0140-ra-s-cache.pdf) | Supplementary S-cache application note; hardware contract uses HW-1.50 §14.8 |

### Artifact hashes

```text
8ed710f70cd6ec8316c676e7858346b9a81ee3b457b40a154ffd028ecfbd591d  HW-1.50
0e7a87184cd9b0291df0b4b7df910c33ff66da47ce7444e47850d43b7bd1d291  BOARD-1.01
91bb2b87dfa537ef051d49459bb85e10551672a2ca10f679708ccc5f667cd7bd  DS-1.50
7948ab8b5abf39131759beff6c0a58cd09c5732f9440411b635407643320da75  HW-1.40
f0a5ac821d87ae915a11191957b7374012144378d9ab4e3b1f2c0d5114bd2f72  DS-1.40
e71dd532fd50c90dc5ceb97af98949a30543ffc671ba27b7153a74076f75b6bb  EXAMPLES-1.53
b31285a0b36818a35ec9810f1a7795a864924ce23370a1b0396e54cb47721510  QSG-1.01
cecdf253b68fbc0811b5fb3a85c739b369e8b326e00095cd2c04b384aaad5e43  SCACHE-1.40
```

## Software reference provenance

- Installed e² studio reference project: `~/e2_studio/workspace/card_flat`, using
  FSP 6.5.0 and Arm GNU 13.2.Rel1. Generated `ra_gen/pin_data.c` corroborates the
  selected pins; `ra/fsp/src/bsp/cmsis/Device/RENESAS/Source/startup.c` demonstrates
  `Reset_Handler → SystemInit → main`. This project includes FreeRTOS.
- Separate `~/repos/fsp` checkout: `a409855a` at review. Do not conflate its source
  line numbers or configuration with the installed generated project.
- Composer baseline: `cf48225a`; Clef baseline: `534429798`. The subsequent working-tree changes implement the MMIO boundary, early MCU layout and first board execution; see [hardware acceptance](HELLOBLINKY_HARDWARE.md).

## Local design package acquired

The user supplied the complete design package beside HelloBlinky, outside that
repository. It remains a local reference; application builds do not read it.

- SCHEMATIC-3.0: [ek-ra6m5-v1-schematic.pdf](../../../../../../MCU/Renesas/EK-RA6M5/ek-ra6m5-v1-designpackage/ek-ra6m5-v1-schematic.pdf),
  D017572_04 issue 3.0, MP release January 22, 2021.
  SHA-256 `fbbe683e5d95ba30072c6f63ffd660000417e7f4fbb9b76c8e687ea913b57e6e`.
- NETLIST-3.0: `Design Files - Cadence/ek_ra6m5_archive/worklib/ek_ra6m5/packaged/pstxnet.dat`
  under the same package; Packager-XL January 22, 2021.
  SHA-256 `cb8bfea9f92a1faaa1ff5fe9420cdc8e6fd356bd39d907e5c85dcf1d077922aa`.
- BOM-3.0: [ek-ra6m5-v1-BOM.csv](../../../../../../MCU/Renesas/EK-RA6M5/ek-ra6m5-v1-designpackage/ek-ra6m5-v1-BOM.csv).
  SHA-256 `164bfb4024d8f7083f923529b1f113a6dfb932d1f1439699a6e1839373abc1a8`.
- The PCB neutral report identifies `D017572_06_V0300_ek_ra6m5.brd`, February 19, 2021.
  SHA-256 `dafcea6c2c00bf0545b13113fe0f4b199fa1139ed430941d807d94dffb934560`.

The earlier HTTP 403 acquisition failure is resolved by these local assets.
Schematic sheets 2, 6 and 11 now establish control circuitry and connector routing.
The live part-number read resolved the AH/BH debugger-project ambiguity as
`R7FA6M5BH3CFC`. The physical PCB artwork revision and actual strap/component
population still need observation when bringing up interfaces beyond HelloBlinky.

## Coverage and acceptance

[HELLOBLINKY_HARDWARE.md](HELLOBLINKY_HARDWARE.md) is the maintained initial fact table
and implementation gate list. Facts are marked as manual-verified, derived, or
requiring device confirmation. Compiler/driver support is separate from each fact's
source verification.

| Family | Source review | Implemented board endpoints |
| --- | --- | --- |
| Core, reset, memory | Initial scope reconciled; device configuration open | Core descriptor exists; startup/layout gates open |
| User LEDs/buttons | Pins and IRQ events verified; polarity/circuit open | Not yet populated |
| Interrupts | Initial routing/width/security rules verified | Not yet populated |
| Clocks/timers | Reset clock and SysTick plan; full clock tree later | Not yet populated |
| GPIO/alternate functions | Initial PFS subset | Not yet populated |
| UART, I2C, SPI, PWM | Source material staged; complete endpoint review pending | Not yet populated |
| ADC/DAC, ELC, DMAC/DTC | Earlier entropy catalog; corrections and open gates recorded there | Not yet populated |
| USB/debug/connectors | J10 debug connection documented; other interfaces pending | Not yet populated |
| Cache, MPU, security | Important distinctions reconciled; provisioning policy separate | Not yet populated |

When adding an endpoint, record source ID plus section/table, device/package variant,
access width and effects, applicable permissions, and acceptance evidence. Keep a
previous revision's file/hash when adopting a new manual. A new PDF's presence does
not mean its hardware claims or corresponding compiler support have been verified.
