# Indexed palette supplement

Recorded 2026-09-13. This adds one concrete register, `layer1Clutwr`, to the
original 80-register display slice. The original [source checkpoint](SOURCE_MANIFEST.json),
[oracle](offset-oracle.c) and [results](offsets.txt) remain frozen. This
supplement establishes source and host offset evidence and separately records
the final bounded interactive image's cold-start/control acceptance below.

## Register and source evidence

`layer1Clutwr` is a 32-bit, little-endian, volatile, write-only transaction at
LTDC offset `196` (`0xC4`), physical address `0x500010C4`. Each write selects
one of 256 palette entries with bits 31:24 and supplies RGB888 in bits 23:0.
It is an entry port, so readback cannot verify palette contents.

The pinned [STM32H747 header](https://github.com/STMicroelectronics/cmsis-device-h7/blob/81db1ec63cdc191fae1565b772da3ea5aa29a683/Include/stm32h747xx.h)
defines `LTDC_Layer_TypeDef.CLUTWR`, the layer base and component masks.
Its member comment says `0x144`, which is the **layer 2** global offset.
The actual member offset is `0x40`; adding layer 1's `0x84` gives `0xC4`.
The [standalone supplement oracle](clut-offset-oracle.c) compiles that extracted
structure and compares the result with the new descriptor literal. Its
[output](clut-offsets.txt) is `binding compiled-offset declared-offset`.

The pinned [HAL LTDC implementation](https://github.com/STMicroelectronics/stm32h7xx-hal-driver/blob/e3c518ebda3e00c8e52b1625ca44fba65da3b63e/Src/stm32h7xx_hal_ltdc.c)
provides the relevant procedures: `HAL_LTDC_ConfigCLUT` writes the entry port
without a shadow reload; `HAL_LTDC_EnableCLUT_NoReload` stages `CR.CLUTEN`;
`HAL_LTDC_Reload` requests the chosen reload. `LTDC_SetConfig` handles L8 as
one byte per pixel and uses line bytes plus seven in `CFBLR`.
Its pinned [public header](https://github.com/STMicroelectronics/stm32h7xx-hal-driver/blob/e3c518ebda3e00c8e52b1625ca44fba65da3b63e/Inc/stm32h7xx_hal_ltdc.h)
defines `LTDC_PIXEL_FORMAT_L8` as `0x00000005U`.

[AN4861 Rev 7](https://www.st.com/resource/en/application_note/an4861-introduction-to-lcdtft-display-controller-ltdc-on-stm32-mcus-stmicroelectronics.pdf)
explicitly covers STM32H747. Section 5.7.3, page 46, excludes CLUT from ordinary
layer changes on the fly and describes the background when layers are disabled.
Section 4.3 describes the indexed formats and color conversion.

For the detailed update restriction, [RM0433 Rev 8](https://www.st.com/resource/en/reference_manual/rm0433-stm32h742-stm32h743-753-and-stm32h750-value-line-advanced-armbased-32bit-mcus-stmicroelectronics.pdf)
section 32.7.25, page 1227, limits CLUT writes to blanking or a disabled layer.
Section 32.7.6, page 1216, excludes CLUTWR from shadowing, states that layer
reads return active values, and makes hardware clear `SRCR.VBR` after reload
at the first line following the active display area. This is corroborating
documentation for related H7 LTDC hardware, **not the exact H747 manual**.

The exact [RM0399](https://www.st.com/resource/en/reference_manual/rm0399-stm32h745755-and-stm32h747757-advanced-armbased-32bit-mcus-stmicroelectronics.pdf)
and ES0445 remain absent from the checked local family/Downloads inventory.
The indexed RM0399 Rev 4 contents identify corresponding sections 33.7.6
(page 1249) and 33.7.25 (page 1260), but their body text was not accepted in
this review: full PDF retrieval failed. Exact-part semantic and errata review
therefore remains open. The supplementary manifest distinguishes immutable
local source hashes from remote documentation observations.

### Corrected clock prerequisite

A subsequent startup review identified an omitted prerequisite in the initial
palette sequence. [ES0445 Rev 6, September 2025](https://www.st.com/resource/en/errata_sheet/es0445-stm32h745xig-stm32h755xi-stm32h747xig-stm32h757xi-device-errata-stmicroelectronics.pdf),
section 2.13.1, page 26, directly covers the STM32H747: LTDC accesses can stall
the device when its pixel clock is disabled. The workaround starts `pll3_r_ck`
before setting `RCC_APB3ENR.LTDCEN`. The document lists silicon U/V with
`REV_ID=0x2003`, matching the connected part's recorded revision. This clause
has now been reviewed from ST's complete remote PDF; it does not constitute
a review of the entire errata sheet. Local PDF retrieval still timed out; see
the [family source status](../../../docs/ES0445_SOURCE_STATUS.md).

Related-H7 RM0433 tables 270–271, page 1203, place both `GCR` and `CLUTWR`
in the pixel clock domain and describe the APB stall during synchronized
access. Consequently, even the initialization guard's `GCR` **read** needs
the clock. A bounded software poll cannot recover from a single stalled bus
transaction. The corrected host starts and awaits PLL3R before enabling the
LTDC interface; later video configuration reuses that already running clock.
This preserves HSI64 and the existing CPU/AHB dividers.

## Chosen HelloDISCO update policy

This is application policy derived from those constraints, rather than a
claim that 256 palette writes form an atomic hardware transaction.

1. Fill the immutable L8 index frame, then execute the framebuffer publication
   barrier (`DSB`); its address has not yet been given to LTDC. Initialize the
   host, starting and awaiting PLL3R before enabling the LTDC register
   interface, then initialize the panel. The revised startup policy configures
   the L8 layer with `CLUTEN=1`, `LEN=0` and starts the background stream.
   Palette initialization enters the same incremental batch state used below,
   with selected palette/index/time origin zero and the tick counter started
   from zero. Steps 4–5 load the palette and publish first visibility; startup
   does not expose the layer or use a separate early CLUT-write loop.
2. For a palette change, first require that no previous reload is pending.
   Stage layer `LEN=0`, preserving `CLUTEN`, and request vertical blank reload
   with `SRCR=2`.
3. Poll `(SRCR & 2)==0` and confirm the active layer has `LEN=0` before the
   first palette write. A timeout terminates the update without writing CLUT.
4. While the layer remains disabled, write 32 entries per foreground
   millisecond for eight ticks. Freeze the requested palette for this batch;
   newer input can choose the next batch. Continue sampling GPIO each tick.
5. Stage `LEN|CLUTEN=0x11`, request another vertical blank reload and wait for
   completion before marking the new palette visible. Bound the entire
   transaction to 100 ms of observed elapsed time. Keep the LTDC and DSI
   stream running throughout.

A background-only interval is expected; this policy does not promise a
seamless palette swap. Clearing only `CLUTEN` while the layer remains visible
would display luminance values instead. Palette writes need no independent
reload; the two reloads control when the entire layer is visible. Neither
the index frame nor its address changes during runtime updates.

Failure before the disable reload completes must prevent any CLUT writes.
Failure during a batch leaves the layer disabled. After the final enable
request, a timeout can leave the complete new palette visible or pending;
it cannot make the already completed batch partial. The foreground stops
processing on failure, so successful completion must not be reported then.

For the 640 × 360 image, L8 takes 230,400 bytes, `PFCR=5`,
`CFBLR=(640 << 16)|647` and `CFBLNR=360`. Window and DSI RGB565 transport
settings remain those of the accepted banner. A palette formed by replicating
the source RGB565 components into RGB888 can round-trip through the RGB565
output without quantization loss. Asset verification must establish that
every index and expanded entry reproduces the accepted source pixel.

### Startup palette observation and accepted correction

The intermediate clock-corrected image ran successfully after reset: the user confirmed
its appearance, the complete index frame remained unchanged across 33 palette
updates, and observed DSI/LTDC error status was zero. A later cold-start check
showed correct geometry with corrupted initial colors; an Up/Down update
restored the intended image. Its earlier initial CLUT loop ran with `GCR`
disabled and before L8 configuration, while successful runtime updates used
the running, configured controller with only the layer disabled.

The revised startup policy above removes that sequencing difference. The pinned
HAL also places CLUT configuration after `HAL_LTDC_Init`, which enables `GCR`,
and layer configuration. This is supporting sequence evidence, not proof of
the hardware cause of the observed corruption.

The final 250190-byte image passed the user's debugger-disconnected test: CN2
unplug/reconnect produced the correct banner without stripes before any joystick
input; Left/Right/Center LED controls were confirmed, and Up/Down palette
operation was also observed. The final
[readback record](../../../../../../../../../MCU/ST/STM32H747I-DISCO/HelloDISCO/evidence/hardware/2026-09-13-interactive-initial-palette/readback-checks.json)
confirms both ELF load segments and the complete 230400-byte index frame.
ELF SHA256 is `1a1f452b134cea1b816f79f877a439af9d1a45bef7c83959321ef5d18a536960`;
BIN SHA256 is `c3e81169a5f16a63bf10c4a15ef707d035f748aa72c33ecf5a04f5760e4d6030`.
Eight unallocated binary padding bytes differ after ELF programming, as recorded;
all allocated load-segment bytes match. The write-only CLUT requires visible
color acceptance, which the user supplied. This closes the bounded demo
checkpoint, not the whole errata review, a general renderer proof or M4 startup.

## Reproduce the host observation

From this directory:

```sh
cc -std=c11 -Wall -Wextra -Werror clut-offset-oracle.c -o /tmp/hellodisco-clut-offset-oracle
/tmp/hellodisco-clut-offset-oracle > /tmp/hellodisco-clut-offsets.txt
diff -u clut-offsets.txt /tmp/hellodisco-clut-offsets.txt
```

The extracted structure retains ST's Apache-2.0 attribution and
[license](LICENSE.CMSIS.md). This host program does not access hardware and is
not linked into the Clef firmware. The [supplementary manifest](CLUT_SOURCE_MANIFEST.json)
captures source hashes, the new descriptor checkpoint and oracle results;
future descriptor changes require a fresh comparison.
