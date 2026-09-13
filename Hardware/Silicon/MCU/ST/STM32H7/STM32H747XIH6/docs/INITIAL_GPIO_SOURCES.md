# Initial STM32H747XIH6 GPIO package

Dated 2026-09-12. This package restores a concrete dependency closure for the
four LEDs and joystick on STM32H747I-DISCO. It provides **20 register declarations**,
not a complete H7 register map or hardware acceptance.

## Sources

The [product source audit](../../../../../../Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md)
records complete document hashes and BOM applicability. The source IDs below
refer to files staged under
`/home/hhh/repos/MCU/ST/STM32H747I-DISCO/reference/stm32cubeh7/`.

| Source | Pinned revision | Facts used |
| --- | --- | --- |
| `cmsis-device-h7/Include/stm32h747xx.h` | repository `81db1ec63cdc191fae1565b772da3ea5aa29a683`; SHA-256 `78d84d7042b66f7ee07dc86a6e794b2a0774e464ae945961bed19573dd46ea07` | Memory/peripheral bases; GPIO/RCC/PWR/DBGMCU member offsets; RCC clock-enable bits; final IRQ149 |
| `stm32h747i-disco-bsp/stm32h747i_discovery.{h,c}` | `c61d8f01d3fa9a03b81c21ce83c0d334150ea3d4` | LED pin/color/polarity; joystick pins, pull-ups and active-low state |
| DS12930 Rev3, January2026 | family `docs/stm32h747ag.pdf` | §3.3 RAM capacities, flash banks; identified TFBGA240+25 part |
| PM0253 Rev6, May2026 | core `CortexM7/docs/` | Table14 M7 configuration, vector/MPU/cache/SysTick architecture |
| UM2411 Rev7, September2025 | product `docs/` | §§6.11–6.12 GPIO roles; board supply and external-memory context |
| Interim CM7 SVD | family `docs/svd/STM32H7x7_CM7.svd`; SHA-256 in audit | GPIOI/GPIOK inherit GPIOC access fields; independent offset/permission cross-check |

RM0399, ES0445 and the vendor SVD archive remain absent from the audited local
source pack. The header is an address oracle and the BSP is a driver reference;
they do not settle all register transitions or errata. This slice narrows access
to the necessary GPIO operations and read-only diagnostics. No flash-control,
power-supply write, interrupt routing, cache-maintenance or display operation is
implemented here.

## Memory and vector declarations

Eleven internal memory envelopes preserve the legacy HelloH7 descriptor's source
names. Bases follow the part header; capacities follow DS12930 §3.3. Flash bank
ownership, bank swapping, actual clock state and CPU2 state remain image admission
facts. A memory envelope does not initialize RAM clocks, configure cache policy,
or grant a register transaction. System memory is read/execute inventory only.

The vector storage has 166 U32 slots (664bytes): 16 core plus 150 external positions.
HDR-H7's final external IRQ is149; holes are not callable interrupts. Placement
alignment is1024. The architectural alignment requirement is separate from the
natural four-byte field alignment. Nominal HSI is64MHz per the vendor system
template; `resetSysclkHz` is not an observation of a running board.

## Driver-facing subset

| Region | Base | Declared offsets / permissions |
| --- | --- | --- |
| RCC | `0x58024400` | CR0x00 read; CFGR0x10 read; D1CFGR0x18 read; AHB4ENR0xE0 read/write |
| GPIOI | `0x58022000` | MODER0x00, OTYPER0x04, OSPEEDR0x08, PUPDR0x0C read/write; IDR0x10 read; ODR0x14 read/write; BSRR0x18 write |
| GPIOK | `0x58022800` | Same selected GPIO fields and permissions |
| DBGMCU | `0x5C001000` | IDCODE0x00 read |
| PWR | `0x58024800` | CR3 0x0C read |

GPIOI is RCC_AHB4ENR bit8; GPIOK is bit10. Enabling both uses mask0x500 and must
preserve unrelated clock bits. Clock-gate readback/barriers and actual pin ownership
belong to the driver. Use BSRR for LED changes: low16bits set outputs high,
high16bits reset them low. The LEDs are active low. Never request set and reset
of the same pin together. Preserve unrelated GPIO configuration fields.

Each DeviceRegion directly references its original `Description` MemorySpace.
Mappings and grants belong to the profile and must keep that identity rather
than reconstructing a structurally similar space.

## Product GPIO wiring

`Hardware.Products.ST.STM32H747I_DISCO.Description` declares GPIOI PI12 green,
PI13 orange, PI14 red and PI15 blue, all active low. These are four independent
single-color LEDs. The joystick uses GPIOK PK2 select, PK3 down, PK4 left, PK5
right and PK6 up. The pinned BSP enables pull-ups and considers a low input
pressed. Debouncing, timing and application actions are not wiring facts.

The product also preserves `sdram` and `qspiStore` as uninitialized envelopes
for legacy descriptor closure. The source audit records the SDRAM bank discrepancy
and external-memory requirements. The initial GPIO package does not claim a
working display, touch, audio, SDRAM, DMA or dual-core implementation.

## Validation

The original-source CCS ProjectChecker accepted the product dependency closure,
the M7 environment, and the legacy HelloH7 profile: zero admitted errors and zero
warnings for each. Each run still reports93 raw diagnostics, all classified
Unreachable in existing BAREWire/Contracts sources; no raw diagnostic names the new
H7 or M7 package files. This is the checker's reachability-filtered result, not a
claim of a globally diagnostic-free library.

A separate source-oracle check compared all20 register offsets with the pinned
part-header struct members and all14 GPIO access declarations with the inherited
SVD entries; they agree. Exact signed finite endpoints of IEEE32/64 were checked
using integer significand/exponent construction. No image was generated or
downloaded by this package change, and no device register was accessed.
