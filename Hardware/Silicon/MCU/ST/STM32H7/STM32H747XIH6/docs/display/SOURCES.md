# DisplayRegisters sources and reviewed behavior

Recorded 2026-09-12 for `STM32H747XIH6/DisplayRegisters.clef` and the HelloDISCO
416 × 416 static glyph experiment. This is a bounded RCC/GPIO/DSI/LTDC slice:
80 register offsets, with core SysTick declarations reused from the core package.
All offsets match compiled `offsetof` values from the pinned device header.
The connected NT35510 panel subsequently displayed color bars, the glyph, and
the separate 640 × 360 banner using the discovery BSP's actual timing constants.
The user confirmed the banner returned after CN2 disconnect/reconnect with
OpenOCD disconnected. The
[profile record](../../../../../../../../Profiles/STM32H747I_DISCO_HelloDISCO_Display/README.md)
separates those observations from build and reset evidence.

## Pinned authorities

The immutable [SOURCE_MANIFEST.json](SOURCE_MANIFEST.json) records full upstream
commit IDs, exact URLs, local paths, byte counts and SHA256 values for each file,
plus glyph-checkpoint hashes of the Clef registers/access/transport and this
oracle. Those hashes remain frozen; banner deployment evidence is recorded
separately below.
The local reference root is
`/home/hhh/repos/MCU/ST/STM32H747I-DISCO/reference/stm32cubeh7/`.

| Source | Commit | Used for |
| --- | --- | --- |
| [CMSIS device H7](https://github.com/STMicroelectronics/cmsis-device-h7/tree/81db1ec63cdc191fae1565b772da3ea5aa29a683) | `81db1ec63cdc191fae1565b772da3ea5aa29a683` | `Include/stm32h747xx.h`: register structures, base addresses and bit definitions. Header SHA256 `78d84d7042b66f7ee07dc86a6e794b2a0774e464ae945961bed19573dd46ea07`. |
| [STM32H7 HAL](https://github.com/STMicroelectronics/stm32h7xx-hal-driver/tree/e3c518ebda3e00c8e52b1625ca44fba65da3b63e) | `e3c518ebda3e00c8e52b1625ca44fba65da3b63e` | DSI regulator/PLL/PHY, packet/FIFO and video procedures; LTDC layer placement and stride; RCC PLL3 configuration. A partial source-only snapshot and its license/manifest are under `hal-display-e3c518e/`. |
| [STM32H747I discovery BSP](https://github.com/STMicroelectronics/stm32h747i-disco-bsp/tree/c61d8f01d3fa9a03b81c21ce83c0d334150ea3d4) | `c61d8f01d3fa9a03b81c21ce83c0d334150ea3d4` | `stm32h747i_discovery_lcd.c/.h`: PG3 reset, optional PJ12 backlight signal, DSI/LTDC clocks, actual panel timings and DCS transaction choices. LCD C source SHA256 `e9db4bdbbdc61278402a8345979165ef438e57fa60e957d30bd6c9749d79dfa7`. |
| [NT35510 component](https://github.com/STMicroelectronics/stm32-nt35510/tree/0d3008be195d1b0750a6d33cc7e944eea61e2074) | `0d3008be195d1b0750a6d33cc7e944eea61e2074` | ID `0x80`, native panel initialization transcript, RGB565/MADCTL/CABC values, timing-header comparison and readback commands. |
| [OTM8009A component](https://github.com/STMicroelectronics/stm32-otm8009a/tree/c0229f087eb3b87a99b392a08b74d2a8933f4aa4) | `c0229f087eb3b87a99b392a08b74d2a8933f4aa4` | Alternate ID `0x40` and initialization transcript. This physical run accepted NT35510 only. |

The native transport preserves the HAL BSD-3-Clause attribution. The panel port
has separate component attribution in the board workspace's
`HelloDISCO/experiments/static-display/LICENSE.ST-PANEL.md`. Vendor source and
the C oracle are reference/host evidence, with no C binding or firmware linkage.
The extracted CMSIS structures retain their Apache-2.0 license in
[LICENSE.CMSIS.md](LICENSE.CMSIS.md).

Local DS12930 Rev 3 establishes the memory domains and DSI/LTDC capabilities;
MB1166 A10's schematic establishes the panel supplies and backlight routing.
Their exact paths and hashes are in the product
[SOURCE_AUDIT.md](../../../../../../../Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md).
RM0399 and ES0445 still need local semantic/errata acceptance. Header structure
qualifiers alone do not prove FIFO side effects, reserved-bit policy, timing,
or DMA/cache correctness.

## Preserved offset oracle

[offset-oracle.c](offset-oracle.c) is a standalone generated host program. It
extracts the pinned `RCC_TypeDef`, `GPIO_TypeDef`, `DSI_TypeDef`, `LTDC_TypeDef`,
and `LTDC_Layer_TypeDef`; defines their register qualifiers locally; and compares
compiled member offsets to the 80 literal descriptor offsets. Layer offsets add
`0x84` for layer 1 and `0x104` for layer 2. No device memory is mapped or touched.

[offsets.txt](offsets.txt) contains `binding compiled-offset declared-offset`
in decimal bytes. All 80 rows agree. To reproduce only this host observation
from this directory:

```sh
cc -std=c11 -Wall -Wextra offset-oracle.c -o /tmp/hellodisco-display-offset-oracle
/tmp/hellodisco-display-offset-oracle > /tmp/hellodisco-display-offsets.txt
diff -u offsets.txt /tmp/hellodisco-display-offsets.txt
```

This frozen oracle uses the captured descriptor literals, whose source hash is
in the manifest. Changes to `DisplayRegisters.clef` require regenerating the
comparison before citing the result for a new revision. This is preserved
evidence, not a new firmware C test framework or full register-semantics proof.

## Reviewed pitfalls

- **NT35510 porches:** `MX_DSIHOST_DSI_Init` and `MX_LTDC_Init` actually select
  horizontal sync/back/front `2/34/34`, vertical `120/150/150`, including an
  800 × 480 landscape stream. The component header's `800X480` aliases instead
  swap those axes. The swapped version yielded a backlit blank panel here;
  the BSP tuple produced color bars and the glyph. Use matching values on DSI
  and LTDC. Derive byte periods from the actual 27.5 MHz pixel clock rather
  than copying the BSP comment/divisor `27429`.
- **Layer window and line length:** HAL `LTDC_SetConfig` adds accumulated
  back porch plus one to each start and accumulated back porch to each
  exclusive stop. For the glyph, coordinates are `[192,608) × [32,448)`;
  width and line count are 416. H7 `CFBLR` is `(832 << 16) | 839`: pitch is
  832 bytes, and hardware line length is line bytes **plus 7**, not plus 3.
  The banner uses `[80,720) × [60,420)`, 360 lines, and
  `CFBLR=(1280 << 16) | 1287`. Its frame occupies
  `0x24000040..0x24070840`, below the reserved stack. Buffer addresses are
  8-byte aligned and constrained to fit AXI SRAM.
- **RGB565 and wrapper:** HAL defines DSI RGB565 as color code zero, so
  `LCOLCR.COLC=0` and `WCFGR.COLMUX=0` are intentional. LTDC layer `PFCR=2`
  selects RGB565. `WCR.DSIEN` is bit 3. `WCR.LTDCEN` bit 2 is used by adapted
  command refresh; adding it is not the missing video-mode enable. LTDC
  starts before the host/wrapper's final video enable.
- **Debug reads have effects:** DSI `GPDR` at `0x50000070` consumes receive
  FIFO data. A broad register dump read this empty port and was followed by
  ISR1 `0x800`, Generic Payload Read Error. Skip GPDR in passive snapshots.
  ISR0 is correctly at `0x500000BC`, ISR1 at `0x500000C0`, IER0/1 at
  `0x500000C4/C8`. A post-reset targeted ISR0/1 read recorded zero errors.
- **Clock and PHY startup:** the bypass oscillator is enabled with HSE off
  before HSEON, and PLL source changes require initially disabled PLL1/2/3.
  DSI regulator ready, a minimum 1 ms delay after PLL enable, PLL lock and
  clock/data-lane stop states are observed with bounded polls. HSI64 and
  core/AHB divider assumptions remain unchanged for the polled timebase.
- **Panel versus touch/backlight:** PG3 resets the panel; PJ12 is an optional
  motherboard backlight route. The supplied A10 schematic populates the
  panel CABC route to the boost enable instead. The native panel transcript
  retains brightness and CABC setup. A responding DSI ID does not initialize
  the separate I2C touch controller. Native command-mode panel setup followed
  by video switching worked for the observed NT35510 in this experiment;
  the BSP's different initialization order is not a remaining demonstrated fault.
- **Static ownership:** one linker-owned AXI frame is filled before publication
  and left unchanged during scanout, with caches disabled. Register-offset and
  bounds evidence does not establish safe concurrent repaint, cache-enabled
  DMA sharing, audio deadlines, or arbitrary panel variants.

## Banner hardware checkpoint

The separate 475,936-byte banner image is retained in the board workspace's
`recovery/2026-09-12-banner-display-eerblu_w/`. `reset-acceptance.json` records
stage 20, NT35510 ID 128, frame address `0x24000040`, 460,800 bytes and zero
DSI/LTDC error status. `readback-checks.json` confirms both ELF load segments
match flash and the complete framebuffer matches the pinned asset SHA256
`993c729068e047981ee1d3dcb6c264172997cec0e290eeea21a809979a340244`.
The user confirmed the displayed banner and its return after a CN2 USB power
cycle with OpenOCD disconnected. The original glyph remains a separate fallback.

Whole-BIN equality is not claimed for this deployment: the eight unallocated
bytes at offsets `0x298..0x29F` are `0xFF` after ELF programming and `0x00` in
objcopy's binary. All allocated ELF load-segment bytes match. This distinction
is recorded independently of the earlier glyph's whole-binary equality.
