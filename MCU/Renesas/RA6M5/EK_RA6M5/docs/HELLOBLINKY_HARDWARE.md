# HelloBlinky hardware contract

Reviewed September 9, 2026. Source IDs refer to [SOURCE_MANIFEST.md](SOURCE_MANIFEST.md).
“Verified” in the register tables means checked against the named document; the acceptance section separately records board execution.
The application owns behavior; this package owns reusable board and MCU facts.

## Board controls

| Control | Pin | Trace-cut jumper | External interrupt / event | Source |
| --- | --- | --- | --- | --- |
| LED1 blue | P006 | E27 | — | BOARD-1.01 §5.5.1, Table 18, pp.24–25 |
| LED2 green | P007 | E26 | — | Same |
| LED3 red | P008 | E28 | — | Same |
| S1 | P005 | E31 | IRQ10-DS / PORT_IRQ10 `0x00B` | BOARD-1.01 §5.5.2 Table 19; HW-1.50 §13.3.2 Table 13.4 |
| S2 | P004 | E32 | IRQ9-DS / PORT_IRQ9 `0x00A` | Same |

These are three separate LEDs. An open trace-cut jumper disconnects its control.
The user has confirmed active-high LED operation and active-low button behavior on the connected board through HelloBlinky. Schematic acquisition and the board PCB revision remain provenance work. HW table 19.5 gives these LED pins no GPT alternate function; dimming uses software PWM.

J10 is the debug USB connector; the onboard S124 provides J-Link over SWD
(BOARD-1.01 §5.2). USB enumeration as `1366:0101` identifies the probe transport,
not the attached MCU variant or its security configuration. BOARD-1.01 names
R7FA6M5BH3CFC; the installed reference launch file names R7FA6M5AH. The live PNR registers resolve that difference: `R7FA6M5BH3CFC`, MCUVER 1. The probe transport alone was not used to choose the flash device.

## Initial register subset

Absolute addresses below are derived from the manual's base-plus-offset formulas.
Access width is a device property, independent of the source integer's inferred range.

| Register | Address / derivation | Access used | Source / rule |
| --- | --- | --- | --- |
| P004PFS, S2 | `0x40080810` | 32 bits | HW-1.50 §19.2.5; PFS base `0x40080800`, offset `0x40*m + 4*n` |
| P005PFS, S1 | `0x40080814` | 32 bits | Same |
| P006PFS, LED1 | `0x40080818` | 32 bits | Same |
| P007PFS, LED2 | `0x4008081C` | 32 bits | Same |
| P008PFS, LED3 | `0x40080820` | 32 bits | Same |
| PWPR | `0x40080D03` | 8 bits | §19.2.6; Non-secure-attributed pin protection |
| PWPRS | `0x40080D05` | 8 bits | §19.2.8; Secure-attributed pin protection; additional PRCR protection applies |
| IRQCR9, S2 | `0x40006009` | 8 bits | §13.2.10 |
| IRQCR10, S1 | `0x4000600A` | 8 bits | §13.2.10 |
| IELSRn | `0x40006300 + 4*n`, n=0…95 | 32 bits | §13.2.15 permits halfword/word access; byte access is unsuitable |
| P0 PODR / PIDR | `0x40080000` / `0x40080006` | 16 bits | §19.2.1–2; dedicated halfword views |
| P0SAR | `0x40080D10` | 16 bits | §19.2.9; pin attribution, PRCR.PRC4 protected |
| PRCR | `0x4001E3FE` | 16 bits | §12.2.1; keyed access; readback after PRC4 change |
| ICUSARG | `0x40008070` | 32 bits | §13.2.7; CPSCU base `0x40008000`, not ICU base `0x40006000`; PRCR.PRC4 protected |
| SCKDIVCR | `0x4001E020` | 32 bits | §8.2.2; reset `0x22022222`, selected `0x20022222` |
| SCKSCR | `0x4001E026` | 8 bits, inspect | §8.2.3; reset CKSEL=1 selects MOCO |
| OFS0 | `0x0100A100` | inspect option word | §6.2.1; bit 1 IWDTSTRT and bit 17 WDTSTRT use 0 for auto-start |

PFS transfer widths are not interchangeable pointer casts. The manual defines
`PmnPFS_HA` at full-register offset **+2**, accessing logical bits 15:0, and
`PmnPFS_BY` at **+3**, accessing logical bits 7:0 (§19.2.5). These special peripheral
views must use their declared addresses; do not infer them from ordinary RAM byte
layout. The initial configuration path can use full 32-bit PFS accesses.

PFS fields needed here are PODR bit 0, PIDR bit 1 (read-only), PDR bit 2, PCR bit 4,
ISEL bit 14, ASEL bit 15, PMR bit 16, and PSEL bits 28:24. Reserved bits must follow
their documented write values. Configure only the intended GPIO/IRQ functions.
For an attributed pin's applicable protection register, clear B0WI before setting
PFSWE. Close PFSWE and then B0WI after configuration. CPU Secure execution does not
by itself determine which pin-protection register applies; inspect PmSAR.

## Interrupt routing and acknowledgement

The board's IRQ9/IRQ10 names identify pin interrupt sources. They are not the
chosen NVIC slots. Select supported IELSR slots, retain their vector entries, and
bind event `0x00B` for S1 and `0x00A` for S2. Table 13.4 admits these events across
the listed vector groups.

Configure IRQCR while its target IELSR is zero (§13.2.10). IRQMD=0 selects falling
edge, 1 rising edge, 2 both edges, and 3 low level. Choose the detection policy
after establishing switch polarity. The optional digital filter is not a complete
mechanical debounce policy.

IELSR.IR is bit 16: clear by writing **zero**; writing one is prohibited. Preserve
the selected event with an explicit legal value when acknowledging, with DTCE=0
for this CPU-interrupt application. A generic write-one-to-clear helper is wrong.
For level-sensitive operation, use the additional source-deassert/read/wait sequence
in §13.2.15. Do not copy a DTC acknowledgement/rearm sequence into this path.

ICU event-slot security and NVIC ITNS must agree (§13.2.7–9); their documented reset
defaults differ. Establish a deliberate interrupt security policy before enabling
the route. Keep handlers and the timer bounded, with an explicit mainline/interrupt
state-sharing protocol. Volatile MMIO alone does not synchronize ordinary RAM.

## Reset, memory, and timer

The selected BH3CFC device has a 2 MB code-flash range beginning at `0x00000000`
and 512 KB SRAM beginning at `0x20000000` (HW-1.50 §4). Confirm the actual device and
usable security-attributed regions before fixing a linker map. Data flash begins
at `0x08000000`; it is not the application code region.

Architectural reset on Cortex-M33 with Security Extension enters Secure state.
IDAU/SAU attribution and a later Non-secure bootloader handoff are separate facts.
See [Arm Cortex-M33 Generic User Guide](https://documentation-service.arm.com/static/5e7cd7b67158f500bd5c4f0c?token=),
§2.3.4. Use the vector and SysTick bank appropriate to the actual application entry.

The 16 core entries and 96 ICU-routed entries occupy 448 bytes; use 512-byte
alignment for that vector table. Retain default fault/unassigned handlers. The
manual also describes debug CTI connections to IRQ96/97 (§2.4); do not enable an
additional debug interrupt without accounting for its vector. Initialize `.data`
and `.bss` before calling compiled code that can depend on them. A DTC table is
unneeded for HelloBlinky and is not intrinsically outside `.bss`.

The documented reset clock is MOCO at nominal 8 MHz with ICLK divided by four,
giving nominal 2 MHz (§8.2.2–3 and the MOCO description). The initial image verified that plan with reload 1999. The user's requested 10% dimming now uses ICLK /1, nominal 8 MHz (`SCKDIVCR=0x20022222`), and SysTick reload 799 for nominal 10 kHz slots. Ten slots form a 1 kHz software-PWM cycle, with blink/debounce work once per cycle. Other clock divisors remain /4. PRCR.PRC0 gates the change; three volatile readbacks exceed the required 250 ns wait after lowering the divisor (§8.2.2). Zero flash wait cycles support up to 50 MHz (§50.4.3).

The live oscillator runs approximately 4.3% faster than nominal in the retained trace. Timing is not crystal-calibrated. The image checks reset clock state before changing the divider; a stock-application handoff with PLL clocks is rejected.

`AIRCR.SYSRESETREQ` performs a software reset (§5.3.7). Table 5.3 specifies retention
per register and reset source, including debugger qualifications for attribution
registers. Calling initialization again or jumping from a bootloader has a different
entry contract. Test both debugger-assisted execution and standalone reset.

## Cache and protection boundaries

HW-1.50 §14.8 describes Renesas C-cache and S-cache. S-cache can cover internal SRAM;
§14.8.4.2 requires software coherency for CPU/DMAC shared memory when applicable.
The lack of a CMSIS core D-cache macro does not remove this obligation. The first
blinky has no DMA; later ring/descriptor users must establish their cache policy.

Single ownership also does not override hardware write protection. GPT GTWP resets
with write protection disabled (§21.2.1); a path relying on that reset state must
establish it. If protection is active, follow the keyed update requirements. This
does not require adding GPT to the SysTick-based first milestone.

## Acceptance status

| Gate | Evidence / remaining work |
| --- | --- |
| Board | PNR part and MCUVER read over SWD; color/button behavior user-confirmed; MP schematic/netlist pinned; physical PCB revision still to inspect |
| Entry/security | Reset-secure image runs; erased watchdog options preserved; selected ICU slots and NVIC target Secure, pins configured via PWPR |
| Target/MMIO | Early 32-bit layout; all six volatile accessors plus negative-source and optimization tests pass |
| Image/layout | BAREWire-checked spaces/vectors generate linker input; ELF linked without unintended runtime dependencies |
| Execution | SysTick and both external adapters observed; user confirmed color/pace/hold behavior and 10% dimming |
| Recovery | Original 2 MiB code double-read-verified; download readback verified; app running after unplug/demo/reconnect with exact image readback; restore command unexercised |

Application details and exact artifacts: [HelloBlinky acceptance](../../../../../../MCU/Renesas/EK-RA6M5/HelloBlinky/docs/ACCEPTANCE.md).

The earlier entropy catalog retains ADC/ELC/DTC timing and analog-front-end work.
Its unresolved hardware assumptions must not be counted as solved by this subset.

## Review evidence

The installed FSP device header `R7FA6M5BH.h`, version 1.10.08, independently agrees
with the addresses and widths listed above for the five PFS registers, the P006
halfword/byte views, PWPR/PWPRS, IRQCR9/10, IELSR0/95, and SCKDIVCR/SCKSCR. A
temporary C translation unit checked base-plus-`offsetof` and `sizeof` for those
15 entries using 30 `_Static_assert` declarations. Arm GNU 13.2.Rel1 accepted it
with `-mcpu=cortex-m33 -mthumb -mfloat-abi=soft -std=c11 -Wall -Werror -fsyntax-only`.
That particular check linked no code. It validates header/layout agreement,
not side effects, permissions, pin polarity, or the correctness of a future driver.

Header SHA-256:
`85b6dbb81bb1589a33293c4ab754137419ebf9b66e220aa098db9d0e9c7f9352`.
The temporary check source is `/tmp/ra6m5-header-grounding.c`; the authoritative
inputs remain the versioned manual and installed header pinned here.

All eight document paths/hashes in SOURCE_MANIFEST.md were checked after editing.
Local links among the source manifest, hardware contract, grounding record, and
active build guide resolve. The subsequent compiler-to-board result is recorded above; the earlier 15-register header check is historical evidence, not a substitute for that execution.

The initial independent FSP/CMSIS check covered all 27 register declarations with
54 address/width assertions. Its result remains in HelloBlinky
`docs/example_artifacts/register-check.txt`; the temporary checker was removed
when build and deploy moved entirely into Composer. No C or Python participates
in the application toolchain. Composer now checks BAREWire layout, entry ABI,
ELF and vectors directly; see the application README.

The MP schematic and complete netlist are now pinned in SOURCE_MANIFEST.md.
[IO_MAP.md](IO_MAP.md) records all board interfaces, manual errata and sharing
constraints; [CONNECTOR_MAP.md](CONNECTOR_MAP.md) includes all J1–J4 contacts.
