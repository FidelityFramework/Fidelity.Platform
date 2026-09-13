# HelloDISCO native static display selection

This profile selects the STM32H747 Cortex-M7, internal AXI SRAM, and a bounded
native Clef DSI/LTDC implementation. On 2026-09-12, the connected NT35510 panel
displayed the 640 × 360 Clef banner. The user confirmed its appearance and that
it returned after disconnecting and reconnecting CN2 with OpenOCD disconnected.
The programmed image passed ELF load-segment readback, exact framebuffer/asset
comparison, and reset telemetry: stage `20`, controller `128`, and DSI/LTDC
error registers zero. The earlier 416 × 416 orange glyph remains a separate
working fallback.

## Image and execution premises

| Property | Selected premise |
| --- | --- |
| Core and boot | Cortex-M7 owns startup and display; hardware reset establishes the initial state. The startup core-ID guard parks an unintended M4 entry. Boot options are preserved. |
| CPU clock | Reset HSI at 64 MHz; HSI divider, SYSCLK selection, D1CPRE, and HPRE remain unchanged. SysTick supplies a polled 1 ms timebase with reload `63999`; no display interrupts are required. |
| Display clocks | External 25 MHz HSE oscillator in bypass mode; DSI PLL `100/5/1` gives 500 MHz and a 62.5 MHz lane-byte clock. PLL3 `M=5, N=132, R=24` supplies 27.5 MHz LTDC pixels. PLL1/2/3 must initially be off before their shared source selection changes. |
| Flash | Bank 1, `0x08000000`–`0x080FFFFF`, 1 MiB; no second-bank image or option-byte modification. |
| RAM | Original part-owned AXI SRAM space, `0x24000000`–`0x2407FFFF`, 512 KiB. Startup initializes the selected memory before generated code runs. |
| Stack | Linker-reserved 8 KiB, `0x2407E000`–`0x2407FFFF`; initial SP `0x24080000`. Reservation is not an MPU or stack-overflow guard. |
| Banner pixels | One 640 × 360 RGB565 framebuffer, 460,800 bytes, address `0x24000040`, exclusive end `0x24070840`, 1,280-byte row pitch. It is centered at `(80, 60)` in an 800 × 480 stream. All static RAM ends at `0x2407084C`, below the stack. |
| Glyph fallback pixels | A separate image uses one 416 × 416 RGB565 framebuffer, 346,112 bytes, address `0x24000040`, exclusive end `0x24054840`, 832-byte pitch, centered at `(192, 32)`. The two frames are not allocated together. |
| Publication | Bounded halfword stores initialize one frame before a DSB publication barrier and scanout enable. The banner reads its pinned ROM asset through a bounded halfword adapter. The application never changes the published frame; there is no concurrent painter. |
| Cache policy | Instruction and data caches remain disabled from reset. This experiment does not establish a cache-maintenance protocol for later shared buffers. |
| Native access | `DisplayRegisters.clef` contributes 80 register declarations; both builds record 225 MMIO sites across 83 addresses, including three reused core SysTick registers. The source-backed transport uses no vendor C linkage. |

The profile retains the original `MemorySpace` instances from the part, core,
and display-register packages. It selects RCC, GPIOG, GPIOJ, DSI, LTDC and PPB
alongside flash and AXI SRAM. The image's grants and MMIO handles narrow access
within those envelopes; the profile itself declares no general surface, buffer,
or transport abstraction. Audio, touch, SDRAM, animation, and general DMA/cache
ownership remain subsequent work.

## Accepted timing and evidence

The working NT35510 timing tuple is **horizontal sync/back/front `2/34/34`,
vertical sync/back/front `120/150/150`**, with active size 800 × 480. These are
the constants actually used by the pinned discovery BSP. The component header's
separately named landscape constants swap the axes; using those values produced
a backlit blank display in this experiment. DSI horizontal periods are computed
from the actual 27.5 MHz pixel clock and 62.5 MHz byte clock. Both LTDC and DSI
must use the same selected timing. See the
[source and offset audit](../../Hardware/Silicon/MCU/ST/STM32H7/STM32H747XIH6/docs/display/SOURCES.md).

The board workspace is `/home/hhh/repos/MCU/ST/STM32H747I-DISCO`. The accepted
banner record is retained under `recovery/2026-09-12-banner-display-eerblu_w/`:

| Evidence | Recorded value |
| --- | --- |
| Binary | `HelloDISCO.Banner.bin`, 475,936 bytes; SHA256 `20ebed5458ecce038093f1b335b983609d0e980d2cfcda5ddea7383d0279ad60` |
| ELF SHA256 | `7832595a932d127bd5f8efb2efe51d1a717251cd9736aea87e639a2bf28f06af` |
| Flash readback | `readback-checks.json`: both ELF load segments match exactly, at `0x08000000` (664 bytes) and `0x080002A0` (475,264 bytes). |
| Framebuffer | All 460,800 bytes match the pinned banner asset; SHA256 `993c729068e047981ee1d3dcb6c264172997cec0e290eeea21a809979a340244`. |
| Reset observation | `reset-acceptance.json`: status `20/128/0x24000040/460800`, no recorded fault, DSI ISR0/ISR1 and LTDC ISR zero. |
| Visual and power cycle | User confirmed the banner and its return after CN2 disconnect/reconnect with OpenOCD disconnected. |

The complete readback binary differs from the objcopy binary only in eight
unallocated gap bytes at offsets `0x298..0x29F`: ELF programming left `0xFF`
where objcopy emitted `0x00`. The accepted claim is exact **ELF load-segment**
verification, not whole-BIN equality. This gap has no allocated ELF payload.

The corrected glyph fallback and its deployment record remain under
`recovery/2026-09-12-static-display-bsp-timings-w7nhibgb/`:

| Evidence | Recorded value |
| --- | --- |
| Binary | `HelloDISCO.Display.bin`, 17,370 bytes |
| Binary and exact flash-readback SHA256 | `ee72a3a2ea8cdc687d58388e2b075ef8cc31539650af3ae6e82b96d860b18c12` |
| ELF SHA256 | `b14c9c03271accd03696ab119c800c27ce290a426bb597052ee2e32f2e4c5ef3` |
| Deployment | `openocd-operation.json`: sector backup, program, verify, reset |
| Reset observation | `reset-acceptance.json`: status `20/128/0x24000040/346112`, no recorded fault, DSI ISR0/ISR1 zero |
| Pixel capture before timing correction | Halted OpenOCD comparison validated every framebuffer byte: 133,712 background pixels and 39,344 orange pixels; SHA256 `dd274b3a95974c8f9e4cfe714fd858f412c5c79ddf43c8e714a967cf904dfa4a` |

The framebuffer capture is retained in
`recovery/2026-09-12-static-display-busypark-iy6m9pan/frame-openocd.bin`.
The visual and power-cycle observations, offset comparisons, declared-access checks,
flash verification and reset telemetry establish different facts. They do not
establish complete MCU coverage, all startup conditions, or real-time audio/UI
behavior. RM0399/ES0445 semantic and errata acceptance remains open.

The banner and glyph selections remain separate under
`HelloDISCO/experiments/banner-display/` and `static-display/`. Follow the board
workspace's `HelloDISCO/STATUS.md` for subsequent deployments and interactivity.
