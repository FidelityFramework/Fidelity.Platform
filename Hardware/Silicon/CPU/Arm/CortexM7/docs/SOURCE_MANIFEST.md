# Cortex-M7 core source manifest

## 2026-09-12 source-closure update

PM0253 is now staged as
`pm0253-stm32f7-series-and-stm32h7-series-cortexm7-processor-programming-manual-stmicroelectronics.pdf`,
Rev6 May2026, SHA-256
`9dd51a3a0a9f8a4ed282e41c74fba8d2d84baa55374a3048849f716e00b6be62`.
Table14 p31 explicitly establishes STM32H74x/75x M7 single/double FPU,16 MPU
regions and cache/TCM sizes. Earlier statements below that PM0253 is missing,
that16 MPU regions have no local authority, or that `__FPU_PRESENT` alone proves
double precision are superseded by this document. F7/M7 optional features must
not be generalized from the H7 implementation.

The exported numeric/MPU configuration is scoped to STM32H74x/75x. IEEE32/64 now
use exact signed finite bounds and the platform `Boundary.Exact` tag, which is
not a mathematical exactness guarantee. See BAREWire Platform/Description.fs
(least/greatest finite endpoint) and Tags.fs (IEEE boundary semantics).
Exported bindings remain `repIeee32`/`repIeee64`, but their representation names
are `float32`/`float64`, matching the compiler's named-seal lookup in
`clef/src/Compiler/NativeTypedTree/NativeTypes.fs` (the float/double/single aliases).

Original-source CCS checks now accept the product, environment and legacy
HelloH7 dependency closures with zero admitted errors/warnings;93 raw diagnostics
remain Unreachable in existing BAREWire/Contracts sources. No hardware operation
or generated-image validation was performed by this package change.

**Audit update, 2026-09-12:** PM0253 Rev 6 is now present in this directory.
The [board source audit](../../../../../Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md)
records its hash and implementation tables. The missing-PM0253 statements
below describe the earlier draft. PM0253 distinguishes single/double precision
and MPU configurations across F7/H7 parts; those options must not be
generalized to every Cortex-M7. The shared numeric declarations also need
their IEEE boundary/range metadata corrected before acceptance. DDI0489
remains absent from the audited paths.

Every fact in this package is transcribed from one of the sources below.
Retrieved 2026-09-12. Nothing from any source has been checked by CCS or run
on a board.

| ID | File | Document | Version | Use |
| --- | --- | --- | --- | --- |
| ARM-DDI0403E | `~/repos/MCU/ST/STM32H747I-DISCO/reference/datasheets/armv7m-arm-ddi0403e.pdf` | Armv7-M Architecture Reference Manual | DDI 0403E.e, ID021621 (2021-02-16) | SCS layout (B3.1, B3.2), SCB/SysTick/NVIC/MPU register summaries (Tables B3-4, B3-6, B3-7, B3-8, B3-11), cache-maintenance operations (Table B2-1), cache ID registers (Table B4-1), VTOR alignment (B3.2.5), vector table entries and reserved slots (B1.5.2), FP extension (A2.5, Table B3-5) |
| CMSIS-CM7 | `~/repos/STM32CubeL5/Drivers/CMSIS/Core/Include/core_cm7.h` | CMSIS-Core(M) `core_cm7.h` | CMSIS Core(M) 5.3 (`cmsis_version.h`), STM32CubeL5 @ `0ae29ef5` (2025-12-08) | Cross-check of every SCB struct offset; the only on-disk source for the six Cortex-M7 implementation registers (`ITCMCR`..`ABFSR`, SCB offsets `0x290`..`0x2A8`) and for `__SCB_DCACHE_LINE_SIZE = 32` |
| HDR-H7 | `~/repos/MCU/ST/STM32H747I-DISCO/reference/stm32cubeh7/cmsis-device-h7/Include/stm32h747xx.h` | STM32H747xx CMSIS device header | cmsis-device-h7 @ `81db1ec6` (2026-08-28) | Applicability evidence only: `__CM7_REV 0x0101` (r1p1), `__FPU_PRESENT`, `__ICACHE_PRESENT`, `__DCACHE_PRESENT`, `__MPU_PRESENT`, `__NVIC_PRIO_BITS 4`, last IRQ `WAKEUP_PIN_IRQn = 149` |

Not on disk, and the only authority for what they cover:

| ID | Document | Would settle |
| --- | --- | --- |
| DDI0489 | Arm Cortex-M7 Processor Technical Reference Manual | The implementation registers' fields and reset values, the implemented MPU region count as configured for the STM32H7, cache geometry, TCM/AHBP behaviour. Everything cited `[verify: DDI0489]` |
| PM0253 | STM32F7 and STM32H7 Cortex-M7 programming manual | ST's transcription of the same registers with the STM32-specific implementation choices filled in |

### Artifact hashes

| File | SHA-256 |
| --- | --- |
| `armv7m-arm-ddi0403e.pdf` | `76500176d20f897eaf05eeadb5a6202cef641e332073b107905c8898e0ee0747` |
| `core_cm7.h` | `34d91327a5f1b251c54fa6bb532d9f5ec0423bf3b7f2b679c8f7b3df95af0fc8` |
| `stm32h747xx.h` | `78d84d7042b66f7ee07dc86a6e794b2a0774e464ae945961bed19573dd46ea07` |

## Provenance notes

**The ARM ARM is the authority for every ARMv7-M offset; CMSIS is the
cross-check.** `core_cm7.h` is Arm's own header and agrees with the ARM ARM
on every register both name. Where the two spell a thing differently (CMSIS
`SHPR[12]` as bytes versus the ARM ARM's three `SHPRn` words; CMSIS's
`ID_MFR[4]` for the ARM ARM's `ID_MMFR0-3`) the address agrees and only the
grouping differs.

**The six implementation registers have one on-disk source.** Table B3-6 of
the ARM ARM marks `0xE000EF90`-`0xE000EFCC` IMPLEMENTATION DEFINED and says
nothing more. `core_cm7.h` places `ITCMCR`, `DTCMCR`, `AHBPCR`, `CACR`,
`AHBSCR` and `ABFSR` at SCB offsets `0x290`, `0x294`, `0x298`, `0x29C`,
`0x2A0`, `0x2A8` (`0x2A4` is a reserved word), all `__IOM`. That is a
transcription of the Cortex-M7 TRM by Arm's own CMSIS team, and it is
sufficient for an inventory. Their bit fields, reset values and the
H7-specific facts (TCMs enabled at reset) are not transcribed here beyond a
doc comment and stay `[verify: DDI0489]` / `[verify: RM0399]`.

**Access permissions are the ARM ARM's, not a transcription default.** Each
table read has a Type column (RO/RW/WO) and every `Access` value in
`Registers.clef` is that column: `"r"`, `"rw"` or `"w"`. The ten
cache-maintenance operations are write-only (B2.2.7: "32-bit write-only
operations"), and `SYST_CALIB`, `NVIC_IABRn`, `CPUID`, `CLIDR`, `CTR`,
`CCSIDR` and `MPU_TYPE` are read-only. The implementation registers take
CMSIS's `__IOM` as RW.

**NVIC register count is a part fact declared at the core.** The ARM ARM
defines up to 16 words per NVIC bank and 124 IPR words; the number
implemented follows `ICTR.INTLINESNUM` in steps of 32 lines. Five words per
bank (160 lines) and 40 IPR words are declared, because the STM32H747's last
IRQ is 149 (`HDR-H7`). A part with a shorter table grants fewer.

**`mpuRegions = 16` is asserted, not read.** The Cortex-M7 MPU is an
implementer's option of 0, 8 or 16 regions. `__MPU_PRESENT 1` in `HDR-H7`
says only that one exists; 16 is the STM32H7's documented configuration but
the document that says so (RM0399 / DDI0489) is not on disk. `MPU_TYPE.DREGION`
reads the truth at rung 1 and this scalar is corrected if it disagrees.

**`cacheLineBytes = 32` comes from CMSIS.** `__SCB_DCACHE_LINE_SIZE 32U`
with the comment "Cortex-M7 cache line size is fixed to 32 bytes (8 words)".
`CCSIDR.LineSize` confirms it at run time.

**Double precision is declared Native on the strength of the STM32 headers.**
`__FPU_PRESENT 1` for the CM7 in `HDR-H7`, and CMSIS's `core_cm7.h` handling
of `__FPU_DP`, establish FPv5-D16 with double precision on the H747. The ARM
ARM's Table B3-5 gives `MVFR0 = 0x10110221` "if double-precision is
implemented"; reading `MVFR0` at `0xE000EF40` at rung 1 is the confirmation.

## Offset verification, 2026-09-12

All 117 declared offsets were checked by hand against the ARM ARM's
register-summary tables and against `core_cm7.h` SCB/SysTick/NVIC/MPU struct
offsets (SCB base `0xE000ED00`, SysTick `0xE000E010`, NVIC `0xE000E100`, MPU
`0xE000ED90`). **All agree.** The representative rows, one per block, with
the access column:

| Register | Declared (from `0xE000E000`) | ARM ARM address | ARM ARM table | Type | CMSIS offset |
| --- | --- | --- | --- | --- | --- |
| `systCsr` | `0x010` | `0xE000E010` | B3-7 | RW | SysTick `0x000` |
| `systCalib` | `0x01C` | `0xE000E01C` | B3-7 | **RO** | SysTick `0x00C` |
| `nvicIser0` | `0x100` | `0xE000E100` | B3-8 | RW | NVIC `0x000` |
| `nvicIabr0` | `0x300` | `0xE000E300` | B3-8 | **RO** | NVIC `0x200` |
| `nvicIpr0` | `0x400` | `0xE000E400` | B3-8 | RW | NVIC `0x300` |
| `nvicIpr39` | `0x49C` | `0xE000E49C` | B3-8 (IPR0-123 range) | RW | NVIC `0x39C` |
| `scbCpuid` | `0xD00` | `0xE000ED00` | B3-4 | **RO** | SCB `0x000` |
| `scbVtor` | `0xD08` | `0xE000ED08` | B3-4 | RW | SCB `0x008` |
| `scbShpr3` | `0xD20` | `0xE000ED20` | B3-4 | RW | SCB `0x018+8` |
| `scbCfsr` | `0xD28` | `0xE000ED28` | B3-4 | RW | SCB `0x028` |
| `scbAfsr` | `0xD3C` | `0xE000ED3C` | B3-4 | RW | SCB `0x03C` |
| `scbClidr` | `0xD78` | `0xE000ED78` | B4-1 | **RO** | SCB `0x078` |
| `scbCsselr` | `0xD84` | `0xE000ED84` | B4-1 | RW (B4.1 text) | SCB `0x084` |
| `scbCpacr` | `0xD88` | `0xE000ED88` | B3-4 | RW | SCB `0x088` |
| `mpuType` | `0xD90` | `0xE000ED90` | B3-11 | **RO** | MPU `0x000` |
| `mpuRasrA3` | `0xDB8` | `0xE000EDB8` | B3-11 | RW | MPU `0x028` |
| `scbIciallu` | `0xF50` | `0xE000EF50` | B2-1 | **WO** | SCB `0x250` |
| `scbBpiall` | `0xF78` | `0xE000EF78` | B2-1 | **WO** | (not in CMSIS SCB_Type) |
| `scbItcmcr` | `0xF90` | IMPLEMENTATION DEFINED (B3-6) | — | — | SCB `0x290` `__IOM` |
| `scbAbfsr` | `0xFA8` | IMPLEMENTATION DEFINED (B3-6) | — | — | SCB `0x2A8` `__IOM` |

One thing the check turned up: Table B4-1 lists `CSSELR` under a heading
whose Type column reads RO for the whole table, but B4.1's text and CMSIS
both make `CSSELR` writable (it selects which cache `CCSIDR` reports), and
the ARM ARM's own register description (B4-667) is RW. `"rw"` is declared.
`BPIALL` exists in the ARM ARM's Table B2-1 but CMSIS's `SCB_Type` stops at
`DCCISW`; the ARM ARM is the authority and the register is declared.
