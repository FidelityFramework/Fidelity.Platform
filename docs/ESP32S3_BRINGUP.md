# ESP32-S3 bring-up: what the target needs that Cortex-M did not

Planning document for running a Clef unikernel with statically bound LVGL on the
Carolina Code Conference 2026 badge (ESP32-S3-WROOM-1-N8). Written 2026-09-11.

The accepted MCU path in this repository is
[EK_RA6M5_HelloBlinky](../Profiles/EK_RA6M5_HelloBlinky): a Cortex-M33 image with
checked MMIO, an owned vector table and J-Link download. This document records
what carries over to Xtensa, what does not, and in what order to attack it.

**Superseded in part: an image has now been built and run.** On 2026-09-11 a
Composer-built ESP image was loaded into the badge's RAM and executed, and it
drove the panel backlight. What that validated, and what it did not, is recorded
in [§9 First light](#9-first-light).

The Clef declarations added alongside this document
([silicon](../Hardware/Silicon/MCU/Espressif/ESP32S3/ESP32_S3_WROOM_1_N8),
[product](../Hardware/Products/CircuitBoardMedics/CCC2026Badge),
[environment](../Environments/Freestanding/xtensa_esp32s3),
[profile](../Profiles/CCC2026Badge_HelloESP)) have still **not** been checked by
CCS. The image that ran was built from hand-written LLVM IR standing in for
Clef's output, through the real Composer backend.

---

## 1. The toolchain question: no esp32s3 CPU in upstream LLVM

This is the first thing to settle, because everything else is downstream of it.
It looks like a blocker at first and turns out, on inspection, not to be.

The LLVM on this machine is 22.1.8, complete with `mlir-opt`, `mlir-translate`,
`opt`, `llc`, `ld.lld` and the `llvm-*` binutils. It registers **no Xtensa target
at all** — Arch does not enable the experimental target.

Enabling it is not sufficient. Upstream LLVM's Xtensa backend exists, but its
`XtensaProcessors.td` defines exactly four processors:

```
generic    esp32    esp8266    esp32s2
```

**There is no `esp32s3`.** Espressif's fork defines it, with an
`esp32s3`-specific feature the upstream tree does not have:

```
def : Proc<"esp32s3", [FeatureDensity, FeatureSingleFloat, FeatureLoop,
                       FeatureMAC16, FeatureWindowed, ... FeatureESP32S3Ops]>;
```

The obvious conclusion — that targeting this part means adopting Espressif's
LLVM — does not survive a look at what that `Proc` line actually contains.

### Upstream is closer than the missing CPU suggests

`esp32s3` is a *processor definition* — a named bundle of features — not a
backend. Checking each feature in that bundle against upstream's
`XtensaFeatures.td`: **26 of the 29 are already there.** Only three are not:

| Missing upstream | What it is |
| --- | --- |
| `FeatureESP32S3Ops` | ESP32-S3-specific instruction extensions (PIE/DSP) |
| `FeatureHighPriInterruptsLevel7` | Parameterization of `highpriinterrupts`, which upstream has |
| `FeatureTimers3` | Parameterization of `timers`, which upstream has |

`llc` takes `-mattr` as well as `-mcpu`, so the S3 can be described by its
features without a named CPU:

```
-mattr=+density,+fp,+windowed,+mac16,+bool,+loop,+sext,+clamps,+nsa,
       +minmax,+mul16,+mul32,+mul32high,+div32,+s32c1i,+regprotect,
       +rvector,+miscsr,+dcache,+threadptr,+highpriinterrupts,
       +interrupt,+exception,+debug,+timers,+prid,+coprocessor
```

The one genuinely absent feature, `ESP32S3Ops`, gates S3-specific DSP and PIE
instructions. A UI unikernel does not emit those. The two parameterizations
affect system-register and interrupt-level descriptions rather than the
instruction selection a Clef program drives.

So the missing CPU model is probably not a blocker at all — it is a convenience
we can supply as flags.

### The options

| Option | Cost | Fits "LLD alone, clang only in Farscape"? |
| --- | --- | --- |
| **A. Build LLVM 22.1.8 with `LLVM_EXPERIMENTAL_TARGETS_TO_BUILD=Xtensa`**, drive it by `-mattr` | One LLVM build | **Yes, exactly.** Version-matched to the MLIR already installed |
| **B. Prebuilt `esp-clang` 21.1.3** (417 MB) | A download | Works, but LLVM **21** against `mlir-translate` **22** |
| **C. Build Espressif's fork with MLIR** | Larger build, repeated per rebase | Yes, but carries the fork |
| **D. Upstream the `esp32s3` Proc definition** | Real LLVM contribution work | Yes — and now plausibly small: one `Proc<>` line plus three features |

### Correction, from building it: upstream cannot LINK Xtensa

Option A was tried on 2026-09-11 and is **two thirds right**. Upstream LLVM
22.1.8 built with `LLVM_EXPERIMENTAL_TARGETS_TO_BUILD=Xtensa` gives working
**code generation** and **assembly**, and the `-mattr` substitution works better
than predicted: checking the accepted feature list, upstream has `timers3` and
`highpriinterrupts-level7` after all (they are spelled with suffixes, which an
earlier grep missed). **28 of the esp32s3 bundle's 29 features are present**;
only `ESP32S3Ops` is absent, and `llc` accepts all 28 with no diagnostics.

But **`ld.lld` in upstream LLVM cannot link Xtensa at all**:

```
ld.lld: error: unsupported e_machine value: 94
```

There is no `lld/ELF/Arch/Xtensa.cpp` upstream — the file 404s on `main` and
exists only in Espressif's fork. LLD's `setTarget` has no `EM_XTENSA` case.

**The fix used here was to port it, not to adopt the fork wholesale.** The
backend is 204 lines and its factory signature (`setXtensaTargetInfo(Ctx &)`)
matches 22.1.8's convention exactly, so it applies cleanly:

1. copy the fork's `lld/ELF/Arch/Xtensa.cpp` into the 22.1.8 tree
2. add it to `lld/ELF/CMakeLists.txt`
3. declare `setXtensaTargetInfo` in `lld/ELF/Target.h`
4. add `case EM_XTENSA:` to `setTarget` in `lld/ELF/Target.cpp`
5. append six relocations the fork added and upstream lacks —
   `R_XTENSA_PDIFF8/16/32` (57-59) and `R_XTENSA_NDIFF8/16/32` (60-62) — to
   `llvm/include/llvm/BinaryFormat/ELFRelocs/Xtensa.def`

That yields **one version-matched 22.1.8 toolchain** that generates, assembles
and links Xtensa, with no clang anywhere in the lowering path. It is built at
`~/repos/llvm-xtensa/llvm-project/build/bin`.

The data layout, obtained from `opt` rather than from clang, is:

```
e-m:e-p:32:32-i8:8:32-i16:16:32-i64:64-n32
```

### The original recommendation, for the record

Build upstream LLVM 22.1.8 with the Xtensa experimental target enabled and
select the S3 by `-mattr`. This is the option that actually satisfies the
[toolchain sovereignty doctrine](../../Farscape/docs/roadmap/05_toolchain-sovereignty-and-native-assets.md):
a pure `opt` / `llc` / `llvm-mc` / `ld.lld` lowering path, no clang anywhere in
it, and **no version skew at all** — the same 22.1.8 that `mlir-translate`
already belongs to.

That last point is the decisive one against option B. esp-clang 21 consuming
textual IR emitted by MLIR 22 is the *backwards* direction across a major
version. The bitcode rule in the sovereignty document — "Farscape's clang major
version ≤ Composer's LLVM major version" — exists precisely because
forward-compatibility is the only direction that holds.

**And esp-clang still has a job, in the role the doctrine assigns it.** Farscape
needs a C compiler to produce LVGL's per-tuple static archive, and esp-clang
21.1.3 is that compiler. 21 ≤ 22 satisfies the bitcode rule in the right
direction, so fat-LTO archives it produces are consumable by Composer's LLVM 22.
The division lands exactly where the roadmap puts it: **clang builds the
archive, LLD links it, and nothing but LLVM lowers Clef.**

Option D becomes attractive once A is working — contributing the `Proc` line
upstream needs a validated target to test against, which is what A produces.

> **Untested.** The `-mattr` substitution is derived from comparing feature
> tables, not from compiled code: the system LLVM has no Xtensa target built, so
> nothing here has been run. Verifying it is the first task of stage 1, and the
> fallback if it fails is option B.

### Get the data layout from the compiler, not from a guess

`Lowering.fs` hardcodes the Cortex-M data layout string. Do not hand-write the
Xtensa equivalent — and do not reach for `clang -emit-llvm` to find it, because
that puts clang back in the lowering path. `opt` will state it:

```sh
printf 'define void @f() {\n  ret void\n}\n' > empty.ll
opt -mtriple=xtensa-esp-elf -passes=no-op-module empty.ll -S -o - | grep 'target datalayout'
```

This technique is verified: run against `thumbv8m.main-none-eabi` on the
installed LLVM it reproduces exactly the string Composer currently hardcodes,
`e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64`. Note that `opt`
normalizes the triple on output (`thumbv8m.main-unknown-none-eabi`), which
matters because `Codegen.fs` compares declared and selected triples as strings.

A wrong data layout produces an image that links and then misbehaves on the
board, which is the most expensive class of bug available here.

---

## 2. Boot: let the mask ROM be the bootloader

The ESP32-S3 boot path is nothing like Cortex-M's, and the difference works
strongly in our favour.

A Cortex-M part fetches the initial stack pointer and reset vector from address
0 and starts running your code. The ESP32-S3 instead starts in **mask ROM** at
`0x4000_0400` — a fixed address in the 384 KB internal ROM that no image can
occupy or relocate. That ROM first-stage bootloader reads an image header from
**flash offset 0**, loads each segment to its stated address, and jumps to the
image's entry point.

Normally the thing at flash offset 0 is ESP-IDF's second-stage bootloader. It
does not have to be. **Put the unikernel there and the mask ROM loads it
directly** — no IDF, no second stage, no partition table.

### Why that is the right first target

The ROM loader copies segments into IRAM and DRAM. It cannot establish flash MMU
mappings — that is what the second-stage bootloader is for. So an image whose
segments all land in SRAM needs **no flash cache or MMU configuration
whatsoever**, and `EXTMEM` (the block the TRM's address table marks reserved and
only the SVD documents) never has to be touched.

The badge can afford this. There is 512 KB of SRAM, no PSRAM to configure, and
the entire UI working set is small: a 160 × 128 framebuffer at 16 bpp is
**40,960 bytes**. Even double-buffered that is 80 KB, leaving room for LVGL's
object tree, the Clef image and stacks.

An all-SRAM unikernel loaded straight by the mask ROM is both the shortest path
to first light *and* the configuration with the fewest premises to get wrong.
Flash execution can come later, when image size actually demands it.

### One more thing the ROM loader does for you

Because the ROM loader places each segment at its load address, **it performs
the `.data` copy**. The Cortex-M `startup.S` in HelloBlinky has an explicit
flash-to-RAM copy loop; the Xtensa equivalent does not need one. Only `.bss`
zeroing remains.

### The image format

Verified against `esp_app_format.h`. At flash offset 0:

```
esp_image_header_t                      24 bytes, packed
  magic              u8      0xE9
  segment_count      u8      1..16
  spi_mode           u8
  spi_speed:4        u8      spi_size:4 in the same byte
  entry_addr         u32     where the ROM jumps
  wp_pin             u8      0xEE = disabled
  spi_pin_drv[3]     u8
  chip_id            u16     0x0009 for ESP32-S3
  min_chip_rev       u8      superseded; kept for compatibility
  min_chip_rev_full  u16     major*100 + minor
  max_chip_rev_full  u16
  reserved[4]        u8
  hash_appended      u8      1 = 32-byte SHA-256 follows the checksum

then per segment:
  esp_image_segment_header_t { load_addr: u32; data_len: u32 }
  followed by data_len bytes

then:
  padding to a 16-byte boundary
  1 checksum byte
  32-byte SHA-256, when hash_appended = 1
```

This is a small, well-specified, testable writer — a good fit for the same
owned-image discipline `Image.fs` already applies to Cortex-M, and verifiable
byte-for-byte against `esptool image_info` before anything is flashed.

---

## 3. The vector block: the real work

This is where "MMIO vector bring-up" stops resembling the Cortex-M path.

A Cortex-M vector table is **an array of addresses**: 4 bytes per entry, entry 0
the initial stack pointer, entry 1 the reset handler, Thumb bit set on each. The
existing `Image.fs` verifies exactly that shape.

An Xtensa vector block is **1024 bytes of code** at fixed offsets from a
relocatable `VECBASE`, each entry a few instructions that jump onward. Offsets,
from the ESP32-S3 core configuration:

| Offset | Entry | Note |
| --- | --- | --- |
| `0x000` | Window overflow / underflow | 6 × 64-byte entries — **mandatory** |
| `0x180` | Level 2 interrupt | |
| `0x1C0` | Level 3 interrupt | |
| `0x200` | Level 4 interrupt | |
| `0x240` | Level 5 interrupt | |
| `0x280` | Level 6 / debug | `XCHAL_DEBUGLEVEL = 6` |
| `0x2C0` | Level 7 / NMI | |
| `0x300` | Kernel exception | |
| `0x340` | User exception | **level-1 interrupts arrive here** |
| `0x3C0` | Double exception | |

`VECBASE` ignores its low 10 bits, so the block must be 1024-byte aligned.

### Two traps with no Cortex-M analogue

**Register windows are not optional.** This core has 64 physical address
registers rotated in windows of 16 (`XCHAL_HAVE_WINDOWED = 1`). When a call
exhausts the window, the hardware raises a window overflow exception, and its
handler spills the caller's registers to the caller's stack frame. Those handlers
live at offset `0x000`.

Omit them and *ordinary nested function calls fault* — not deep recursion, just
normal call nesting, at roughly the depth a UI widget tree reaches. An image that
blinks an LED from `main` may appear to work and then die the moment LVGL lays
out a screen. Write the window vectors first, and test call depth deliberately.

**There is no level-1 interrupt vector.** Level-1 interrupts are delivered
through the user exception vector at `0x340`, mixed in with genuine exceptions.
The handler must read `EXCCAUSE` and the `INTERRUPT` register to tell an
interrupt from a bad load. Every interrupt this bring-up needs — the SYSTIMER
tick, SPI completion — is level 1.

### And there is no NVIC

A Cortex-M peripheral interrupt has a fixed vector slot. On ESP32-S3, a
peripheral source reaches the core only after the **interrupt matrix** is told
which of the core's 32 interrupt numbers to raise, by writing that number into
the source's `*_INT_MAP` register. `Registers.clef` declares the maps for SPI2
DMA, LEDC, RMT, SYSTIMER target 0, USB and GPIO.

### Order of operations at reset

Each of these is a way a first image dies silently, in the order they bite:

1. **Disarm the watchdogs.** RTC WDT, super WDT and both timer-group MWDTs are
   armed before your code runs. Each sits behind a write-protect register that
   takes a magic value first. Skip this and the board resets mid-bring-up — a
   boot loop with no output, easily mistaken for a bad image.
2. **Drive the backlight (GPIO5) low.** Sixteen pins glitch low for ~60 µs at
   power-up and the panel comes up bright white. The CircuitPython launcher
   fights this with its first statement; a bare image can win it outright.
3. **Zero `.bss`.** The ROM loader already placed `.data`.
4. **Point `VECBASE` at the image's own vector block** and confirm the window
   vectors are reachable before making any nested call.
5. **Set `CPENABLE`** if any floating-point instruction will execute. The FPU is
   a coprocessor, disabled at reset; the first float instruction traps otherwise.
   Building LVGL integer-only avoids this entirely — see §5.
6. **Ungate peripheral clocks** via `SYSTEM.PERIP_CLK_EN0/1`. A gated peripheral
   reads back zeros and discards writes, which looks exactly like a wrong address.
7. **Configure pads through IO MUX**, then route anything that needs the GPIO
   matrix. The panel does not: GPIO10/11/12 are SPI2's own IO MUX pins.
8. **Route interrupts** through the matrix and unmask them.

---

## 4. What Composer needs

The Cortex-M path is ARM-specific by design rather than by accident, so this is
a sibling backend, not a parameterization. File by file:

| File | What is ARM-specific | What Xtensa needs |
| --- | --- | --- |
| [`MCU/Target.fs`](../../Composer/src/BackEnd/MCU/Target.fs) | Rejects anything but `arm_cortex_m33` / `thumbv8m.main-none-eabi` / `cortex-m33`; requires a `CortexMImageDescriptor`; requires **code flash at address 0**; requires vectors to be one contiguous `U32` array; validates against `Abi.armAapcs` | An `XtensaImageDescriptor` sibling in BAREWire. Flash-at-zero is false here — segments load to SRAM and the *flash image* starts at offset 0. Vectors are `U8[1024]`, not `U32[n]`. Needs an Xtensa windowed-call ABI for the validator |
| [`MCU/Image.fs`](../../Composer/src/BackEnd/MCU/Image.fs) | Asserts ELF32 **ARM** (`e_machine = 40`); checks initial MSP at word 0, Thumb bit on every vector, reserved slots 8/9/10/13; `arm-none-eabi-{as,objcopy,objdump,nm,readelf}` | `e_machine = 94` (`EM_XTENSA`). Vector verification becomes "is this 1024 bytes at a 1024-aligned address with non-empty window entries", not an address-table walk. Use `llvm-objcopy`/`llvm-nm`/`llvm-readelf` from esp-clang rather than GNU binutils. Then the `esp_image_header_t` writer from §2 |
| [`MCU/Tools.fs`](../../Composer/src/BackEnd/MCU/Tools.fs) | `armToolDirectory`, `COMPOSER_ARM_GNU_BIN`, J-Link `probeLibrary` | An `xtensaToolDirectory` / `COMPOSER_ESP_CLANG_BIN`. No GNU binutils needed if the LLVM ones are used |
| [`MCU/Layout.fs`](../../Composer/src/BackEnd/MCU/Layout.fs) | `MEMORY { FLASH, RAM }` with `.data ... AT> FLASH` and `LOADADDR`; asserts `SIZEOF(.vectors) == n*4` and VTOR alignment | Three SRAM banks with distinct bus views, no LMA/VMA split (the ROM loader does the copy), `.vectors` 1024-aligned in IRAM |
| [`MCU/Probe.fs`](../../Composer/src/BackEnd/MCU/Probe.fs) | SEGGER J-Link shared library, `JLINKARM_*` | `esptool` over the USB Serial/JTAG CDC for download; OpenOCD for debug. The badge's USB cable already carries JTAG — see §6 |
| [`MCU/Pipeline.fs`](../../Composer/src/BackEnd/MCU/Pipeline.fs) | Hardcodes `"thumbv8m.main-none-eabi"` at the lowering call | Take the triple from the resolved platform core instead of the literal |
| [`LLVM/Lowering.fs`](../../Composer/src/BackEnd/LLVM/Lowering.fs) | `isM33 = triple.StartsWith("thumbv8m.main-")` plus the hardcoded data layout | An Xtensa branch with the data layout obtained as in §1 |

The good news in that table: **`Image.fs`'s structure survives**. The
discipline — optimize, reject constructs the profile cannot support, assemble
the owned startup, link with an explicit script, verify the result against the
declarations, then write build evidence — is architecture-independent. What
changes is the content of the checks, not their existence.

One check needs rethinking rather than porting. `Image.fs` rejects any image
containing `invoke`/`landingpad`/`resume` because the profile has no unwinder.
That stays true. But it should probably also reject constructs that would break
*windowed* calls, and what those are is not yet known — a question for the first
image, not the plan.

---

## 5. LVGL through Farscape

The plan is a Farscape-generated Clef binding library statically linked into the
unikernel. [Farscape](../../Farscape) parses C headers with clang and emits
typed Clef bindings, which is exactly this shape of problem.

### Configure LVGL as a freestanding library

LVGL is unusually well suited to this because it is designed for targets with no
OS. Configure it (`lv_conf.h`) to need nothing the environment cannot supply:

- **Static memory pool, no `malloc`.** LVGL can be given a fixed byte array.
- **No OS integration** — no threads, no mutexes.
- **No file system, no stdio.**
- **Integer-only**, if possible. LVGL's core can avoid floating point, which
  sidesteps the `CPENABLE` coprocessor obligation entirely and removes a whole
  class of first-image trap. Worth checking against the widgets actually wanted
  before committing.
- **One flush callback and one tick source.** The flush callback pushes a pixel
  rectangle over SPI2; the tick comes from SYSTIMER `UNIT0`. That is the entire
  porting surface — two functions.

### The binding boundary is a trust boundary

Worth being explicit, given how carefully the MMIO path is checked: LVGL is C,
and [`MMIO_CONTRACTS.md`](MMIO_CONTRACTS.md) already records that "assembly and
foreign code remain explicit trust boundaries". A `DeviceAccessPlan` constrains
what *Clef* does with MMIO. It says nothing about what linked C does.

So the boundary should be drawn where it can be defended: **LVGL renders into a
framebuffer and never touches a register.** Clef owns SPI2, the panel, the
pads and the LEDs; LVGL owns pixels. The flush callback is the only crossing,
and it hands over a buffer and a rectangle, not a peripheral. That keeps the
checked MMIO story intact and keeps LVGL replaceable.

### Build order

LVGL must be compiled for `xtensa-esp-elf -mcpu=esp32s3` by the same esp-clang
chosen in §1, into an archive that `ld.lld` links against. It joins
`provided_libraries` in the project manifest, the way HelloBlinky declares
`helloblinky_boot`. Farscape generates the Clef side; the C side is an ordinary
cross-compile with no IDF dependency.

---

## 6. Before flashing anything: back the badge up

The badge ships with CircuitPython 10.2.1 and enumerates as USB `303a:7003`
with a `CIRCUITPY` volume. Flashing replaces it.

[HelloBlinky already established the discipline](../../MCU/Renesas/EK-RA6M5/HelloBlinky/recovery):
a dated `recovery/<date>-stock` directory holding the original flash contents and
a manifest. Do the same here — read all 8 MB out to a file before the first
write. The CircuitPython image is re-downloadable, but a byte-exact dump also
captures the board's NVM, and this is a conference badge that was given away
once.

Practical notes:

- `esptool` is **not installed** on this machine, and no ESP toolchain is
  present (`~/.espressif` does not exist). Install it into a virtual environment
  rather than system-wide.
- Unmount `CIRCUITPY` cleanly first. `esptool` resets the chip into download
  mode, and pulling the volume out from under the mount is avoidable noise.
- The current `code.py` on the badge is the sample launcher and differs from
  `samples/Launcher/code.py` in the repository; the badge's own copy has been
  preserved alongside this work.
- Recovery is a plain reflash, not a repair: `esptool write_flash` with the
  CircuitPython `.bin`, and the board comes back.

### The debug channel is already there

Worth knowing before deciding how much to invest in a console: **JTAG works over
the shipped USB cable.** With no eFuses burned — factory state — the default JTAG
signal source is the USB Serial/JTAG controller, not the pins. So OpenOCD can
attach over the same Micro-USB connector that carries the console, with nothing
soldered.

The four JTAG pins are *also* broken out on adjacent pads (IO39–42) for an
external probe, and a hardware UART sits on pads IO17/IO18 if a serial console is
wanted before USB is standing up. For bare-metal work where there is no
interpreter left to print from, this is a well-equipped board.

---

## 7. Suggested order

Each stage is chosen to produce an unambiguous signal, so a failure localizes.

| Stage | Goal | Signal that it worked |
| --- | --- | --- |
| 0 | Dump the stock 8 MB flash to `recovery/` | A byte-exact file and a manifest |
| 1 | Install esp-clang; confirm `llc -mtriple=xtensa-esp-elf -mcpu=esp32s3` accepts a trivial module | An object file |
| 2 | Capture the Xtensa data layout; get MLIR 22 → esp-clang 21 textual IR through `opt` | A `.ll` that round-trips |
| 3 | Hand-write the vector block and startup in Xtensa assembly; build an image with `esptool`-verifiable headers | `esptool image_info` agrees |
| 4 | **First light**: watchdogs disarmed, backlight low, one LED lit from `main` | An LED, and a board that does not reset |
| 5 | Prove the window vectors with deliberately deep nested calls | No fault at depth |
| 6 | SYSTIMER tick through the interrupt matrix and the level-1 path | A counter advancing |
| 7 | SPI2 to the panel via native IO MUX pins; push a solid colour | A coloured screen |
| 8 | `DeviceAccessPlan` and grants; move every register touch onto `Mmio.bind32` | A `device-access.json` ledger |
| 9 | Farscape LVGL bindings; LVGL into a static framebuffer; flush callback | A widget |
| 10 | RMT driving the five WS2812s | Five LEDs under control |

Stages 0–2 are toolchain and carry no hardware risk. Stage 3 is the bulk of the
novel work. Stage 4 is the one that tells you whether the premises hold.

A note on stage 8's placement: it comes *after* first light deliberately.
`MMIO_CONTRACTS.md` notes that raw `Mmio.reg8/16/32` programs with no plan remain
supported and are recorded as unbound — so bring-up can use raw access to get a
signal, then adopt a plan once the addresses are known to be right. Debugging a
wrong address and a rejected grant simultaneously is worth avoiding.

---

## 8. Open questions

- **Does MLIR 22's textual IR parse under esp-clang 21 for the constructs Clef
  actually emits?** Settled empirically at stage 2, and the answer decides
  whether option C gets promoted early.
- **What is the Xtensa windowed-call ABI in BAREWire's validator terms?**
  `Abi.armAapcs` has no sibling. Register windows make the calling convention
  genuinely different, not just differently-named.
- **Can `Image.fs`'s rejection of non-unwindable constructs be extended to
  window-hostile ones?** Unknown until an image exists to find out.
- **Does LVGL's wanted widget set stay integer-only?** Determines whether
  `CPENABLE` and FPU state are in scope at all.
- **Second core.** The environment declares one core and does not start CPU1.
  Whether LVGL rendering wants it is a later question, and starting it brings
  its own stack, vector base and coherence obligations.
- **`EXTMEM` provenance.** If flash execution is ever needed, its address comes
  from the SVD and not the TRM, whose address table marks that range reserved.
  That should be reconciled before relying on it.

---

## 9. First light

On 2026-09-11 an image built through this backend ran on the badge and drove the
panel backlight. The chain, end to end:

```
hand-written LLVM IR  ──llc -mattr=<28 esp32s3 features>──▶  main.o
boot/startup.S        ──llvm-mc──────────────────────────▶  startup.o
                        Composer XtensaLayout.generate   ─▶  memory.ld, layout.inc
startup.o + main.o    ──ld.lld -T memory.ld─────────────▶  hello.elf  (EM_XTENSA)
hello.elf             ──Composer XtensaImage.readLoadSegments
                        + EspImage.build────────────────▶  hello.bin  (1392 bytes)
hello.bin             ──esptool load-ram──────────────────▶  the badge
                                                             backlight ON
```

The image disarms the RTC, super and timer-group watchdogs, puts GPIO5 into GPIO
mode through IO MUX, drives it high, and spins.

### What this establishes

| Claim | Evidence |
| --- | --- |
| The Xtensa toolchain works, no clang in the lowering path | `llc`, `llvm-mc` and `ld.lld` produced the artifacts |
| The vector block is correctly laid out | `llvm-nm` shows all ten entries on their required offsets, `.vectors` exactly 1024 bytes, 1024-aligned |
| Composer's linker script is correct | `ld.lld` linked against it; `__vectors` landed at the instruction window's origin |
| Composer's ELF segment reader is correct | It read both `PT_LOAD`s and excluded the zero-`filesz` BSS reservation |
| Composer's image writer is correct | The ROM loader parsed the container and jumped to the declared entry |
| MMIO declarations reach real peripherals | A pin physically changed state |
| `Ordering = "volatile"` lowers correctly | `llc` emits `memw` before each volatile store |

### What this does NOT establish

- **The window handlers have not executed.** `main` is shallow, so no window
  overflow occurred. They are assembled and correctly placed; they are not
  exercised. The first real test is a call chain deeper than the register
  window, which is exactly what a draw tree produces.
- **No interrupt has been taken**, so the interrupt matrix, the level-1 path
  through the user exception vector, and `EXCCAUSE` discrimination are untested.
- **No Clef was compiled.** The IR was hand-written to stand in for Clef's
  output. CCS has checked none of the declarations.
- **The panel, the LEDs and the buttons are untouched.**

### Closed the same day: booting from flash

The `load-ram` caveat above — that the image checksum and SHA-256 had not been
validated by hardware — was closed within the hour. The same image was written
to flash offset 0 and the badge was reset with no host attached:

```sh
esptool --port /dev/ttyACM0 --before no-reset write-flash 0 hello.bin
# "Wrote 1392 bytes ... Hash of data verified."  Readback compared equal.
# then: tap RESET
```

The backlight came on by itself. Two independent signals confirm the image ran
rather than the ROM falling back to its download loop — which would present the
same USB device id:

- **the backlight is lit**, and a rejected image leaves GPIO5 untouched and the
  panel dark;
- **the ROM download protocol does not answer**, because the image spins in a
  loop and services nothing. Execution left the ROM, and the entry point is the
  only other place it can be.

So the ROM's *boot* path — which does check the container's magic, segment
count, chip id, revision range, **checksum** and **appended SHA-256** before
loading — accepted a container produced entirely by Composer's F# writer.

The panel showed full white, which is correct: an uninitialised ST7735S with its
backlight on is white. The image contains no display code.

### Two bugs the hardware found that reading did not

**Literal pools must precede text.** Xtensa's `L32R` loads a constant at a
*negative* offset — it reaches backwards only. The generated script emitted
`*(.text*)` before `*(.literal*)`, and every `movi` in the startup code failed
to link with `relocation R_XTENSA_SLOT0_OP out of range`. Fixed in
`XtensaLayout.fs`, and the regression now asserts the *ordering* rather than
merely the presence of the pools.

**Upstream LLD cannot link Xtensa**, which no amount of reading the feature
tables would have revealed, because the gap is in LLD and the tables describe
LLVM. See §1.

### A field confirmation of a documented hazard

While the image was running, three of the five WS2812s lit reddish. The image
never touches GPIO4. That is the power-up glitch recorded in the badge's
hardware reference — GPIO4 glitches low for about 60 us at power-up, which can
clock a spurious symbol into the LED chain. An image must clear the LEDs
explicitly rather than assume they start dark.

### Reproducing

```sh
B=~/repos/llvm-xtensa/llvm-project/build/bin
ESP32S3="+density,+fp,+loop,+mac16,+windowed,+bool,+sext,+nsa,+mul16,+mul32,\
+mul32high,+s32c1i,+threadptr,+div32,+dcache,+debug,+exception,+highpriinterrupts,\
+highpriinterrupts-level7,+coprocessor,+interrupt,+rvector,+timers3,+prid,\
+regprotect,+miscsr,+minmax,+clamps"

$B/llc     -mtriple=xtensa-esp-elf -mattr="$ESP32S3" -O2 -filetype=obj main.ll -o main.o
$B/llvm-mc -triple=xtensa-esp-elf  -mattr="$ESP32S3" -filetype=obj -I boot boot/startup.S -o startup.o
$B/ld.lld  --static --gc-sections --entry=_start -T boot/memory.ld startup.o main.o -o hello.elf
# then Composer's XtensaImage/EspImage produce hello.bin, and:
esptool --port /dev/ttyACM0 --before no-reset --after no-reset load-ram hello.bin
```

`load-ram` writes no flash, so a plain RESET returns the badge to whatever is
installed there.

## 10. HelloESP runs (2026-09-12)

The Clef sources compiled by Composer — CCS, `opt`, `llc -filetype=asm`,
`llvm-mc`, `ld.lld`, `EspImage` — booted from flash offset 0 and ran the whole
workload: panel, five WS2812s over RMT, three buttons, and a 1 kHz SYSTIMER tick
taken through the interrupt matrix, the user exception vector and the owned
level-1 trampoline into a Clef `FnPtr`. Everything §9 listed as *not
established* is now established, including the window handlers (the draw tree
is deep enough) and `EXCCAUSE` discrimination.

### What the hardware found, in the order it cost a flash cycle

1. **Mistyped decimal constants.** `1356348577` is `0x50D83CA1`, not the
   watchdog key `0x50D83AA1`; the write-protect never opened, the disable was
   discarded, and the super watchdog reset the part once a second — a boot
   loop with no output. Hex literals are fine in Clef when they fit `int`.
   Cross-check every decimal against its hex comment mechanically.
2. **Field positions from memory.** Against the SVD: RMT `CONF_UPDATE` is bit
   24 and `MEM_SIZE` bits 19:16; `RMT_SYS_CONF = 0` turns the module clock
   *off*; SPI2 needs `CLK_GATE.MST_CLK_ACTIVE`, a `CMD.UPDATE` after
   configuration, and IO MUX function 4 on GPIO10/11/12; the SYSTIMER clock
   enable is bit 29. The audit that caught these parsed
   `docs/svd/esp32s3.svd` for every register the image writes; do that first.
3. **The stack was in SRAM2 Block10.** The ROM's flash-boot path
   (`ets_run_flash_bootloader` → `ROM_Boot_Cache_Init`) enables a 32 KB DCache
   over `0x3FCF_8000..0x3FCF_FFFF` and never disables it before the jump; the
   download path does not, so `esptool write-mem`/`read-mem` say SRAM2 is
   fine. The image ran to its first stack spill (`Logo.init`) and parked.
   Fixed at the layout: `romHandoverDataLimit = 0x3FCD7E00` on the silicon
   (below the ROM's shared buffers, stacks and `.bss`), `DataLimit` on the
   `XtensaImageDescriptor`, and `XtensaLayout` ends the data window — and the
   stack — there. Composer's layout tests assert it.
4. **`.rodata` in the instruction window.** LLVM made a `[4 x i16]` table
   from a `match`; the instruction bus only serves aligned 32-bit loads, so
   the first tick would have faulted. `.rodata` now goes to the DRAM `.data`
   output section (two-segment image).
5. **Panel orientation.** The glass is mounted portrait, 128×160, the
   controller's native scan; the stock firmware's `rotation=0, bgr=True` is
   MADCTL `0xC8`. Read the shipped configuration before guessing.

### How the hang was placed

SRAM does not survive the RESET button (`CHIP_PU` is a power-on reset), so
no-init fault words are noise after the fact. The USB-Serial/JTAG peripheral
is a hardware CDC-ACM endpoint — it enumerates under the image with no USB
stack — and `HelloESP/src/Console.clef` writes stage markers to it through
`EP1` and `EP1_CONF.WR_DONE` with bounded waits. `S4` then silence put the
fault inside `Logo.init`; the ELF showed that function's first stack store;
the ROM disassembly showed why that address was dead.

### Still open

Later-stage polish from the same audit: `Leds.waitIdle` polls `TX_START`,
which is write-only (use the `CH0STATUS` state field); `SPI_CMD.UPDATE` could
be re-issued per transfer; SYSTIMER period-mode write ordering; RMT
`SYS_CONF` `MEM_FORCE_PU`/`SCLK_DIV_B` as esp-idf sets them.

