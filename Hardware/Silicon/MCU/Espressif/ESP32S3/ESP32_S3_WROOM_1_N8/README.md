# ESP32-S3-WROOM-1-N8

Espressif module: an ESP32-S3 die (dual Xtensa LX7, 512 KB SRAM, 384 KB mask
ROM) with 8 MB in-module quad SPI NOR flash and **no PSRAM**.

[Description.clef](Description.clef) declares the memory banks, bus views and
Xtensa vector layout. [Registers.clef](Registers.clef) declares the MMIO
inventory, generated from Espressif's SVD by
[tools/svd_to_clef.py](tools/svd_to_clef.py).
[docs/SOURCE_MANIFEST.md](docs/SOURCE_MANIFEST.md) records provenance.

## Status

**Declarations only. Nothing here has been checked by CCS or built into an
image.** 111 registers over 12 peripherals are declared — the subset a display
and LED bring-up needs, not the part's full 2053-register inventory.

## The -N8 suffix is load-bearing

This variant has no PSRAM, which is why the badge can break out IO35/IO36/IO37.
On an octal-PSRAM module (N8R8, N16R8V) those pins carry
`SPIIO6`/`SPIIO7`/`SPIDQS` and are unusable. Declaring the part rather than the
family keeps that distinction.

It also means **all working memory is the 512 KB of internal SRAM**. That is
comfortable for this board — a 160 × 128 16 bpp framebuffer is 40,960 bytes —
and it enables the shortest bring-up path: an image that lives entirely in SRAM
needs no flash cache or MMU configuration.

## Memory: three banks, two buses, one bank on both

The 512 KB of SRAM is three physical banks, and one of them is visible from both
CPU buses at two different addresses:

| Bank | Size | Instruction bus | Data bus |
| --- | --- | --- | --- |
| SRAM0 | 32 KB | `0x4037_0000..0x4037_7FFF` | — |
| SRAM1 | 416 KB | `0x4037_8000..0x403D_FFFF` | `0x3FC8_8000..0x3FCE_FFFF` |
| SRAM2 | 64 KB | — | `0x3FCF_0000..0x3FCF_FFFF` |

SRAM1's two windows are the same silicon (TRM Table 15.3-1 lists SRAM Block2 at
both addresses). `Description.clef` therefore declares the bank's capacity
**once**, on the instruction-bus declaration, and carries the data-bus window as
a plain base, `sram1DataBusBase`, that the image descriptor takes as
`Sram1DataBase`. A second MemorySpace for the alias would either double-count
the silicon — adding the published window sizes together claims 928 KB on a
512 KB part — or fail the platform checker with zero capacity.

Two more facts about this memory are the ROM's, not the silicon's, and the
description states them because an image cannot discover them by reading the
part: the mask ROM still owns everything from `romHandoverDataLimit`
(`0x3FCD_7E00`) up when it jumps to an image — its shared buffers, its two CPU
stacks, its `.bss` and `.data` — and its flash-boot path leaves a 32 KB data
cache enabled over SRAM2 Block10 (`0x3FCF_8000..`), memory the CPU then cannot
address at all. An image's data window and stack end at the limit.

The two windows also carry different access tags — `rx` for the instruction
view, `rw` for the data view — because the buses genuinely differ. Code is
fetched through one and data is stored through the other.

## Registers

Regenerate after editing the generator's `SELECTION` list:

```sh
python3 tools/svd_to_clef.py docs/svd/esp32s3.svd --out Registers.clef
```

Two facts simplify every binding on this part:

- **Every peripheral register is 32 bits and word-addressed.** There are no
  byte or halfword peripheral transactions, so every binding is `Mmio.bind32`.
  The RA6M5, by contrast, mixes 8-, 16- and 32-bit registers.
- **Every peripheral occupies exactly 4 KB** in `0x6000_0000..0x600D_0FFF`.

One limitation to keep in view: the SVD leaves register-level `access` unset on
2048 of its 2053 registers, so the generator emits `"rw"` as a transcription
default. That records what the source document says, not a checked hardware
permission — read the TRM before relying on a write.

## Xtensa vectors are code, not addresses

A Cortex-M vector table is an array of handler addresses. An Xtensa vector block
is 1024 bytes of **code** at fixed offsets from a relocatable `VECBASE`, each
entry a few instructions that jump onward. `Description.clef` declares the
offsets; the two that have no Cortex-M analogue at all are the mandatory window
overflow/underflow entries at offset `0x000` and the absence of any level-1
interrupt vector.

## Toolchain

There is **no `esp32s3` CPU model in upstream LLVM** — upstream's Xtensa backend
defines `esp32`, `esp8266` and `esp32s2` only. Targeting this part requires
Espressif's LLVM fork (`esp-clang`).
