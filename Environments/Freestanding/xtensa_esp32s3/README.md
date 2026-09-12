# Freestanding — xtensa_esp32s3

Execution contract for one Xtensa LX7 core of an ESP32-S3, with no OS, no
runtime and no bootloader beyond the chip's mask ROM.

## Status

**Declared, not executable.** The ABI facts and numeric representations are
written; no image has been compiled against them, because no toolchain on a
stock system can emit code for this target.

## Why this target is not just another `-mcpu`

The system LLVM registers no Xtensa target. Upstream LLVM *does* carry an Xtensa
backend, but `XtensaProcessors.td` upstream defines only `esp32`, `esp8266` and
`esp32s2`. There is **no `esp32s3` processor upstream** — it lives in
Espressif's fork along with `FeatureESP32S3Ops`, shipped as `esp-clang`.

The triple declared here (`xtensa-esp-elf`, `-mcpu=esp32s3`) is Espressif's
spelling. It is *not*, however, only reachable through that fork: 26 of the
`esp32s3` bundle's 29 features exist upstream, and `llc` takes `-mattr`, so an
upstream LLVM built with the Xtensa target can describe this part by feature
flags. That keeps the lowering path pure LLVM and version-matched to the
installed MLIR. See [the bring-up plan](../../../docs/ESP32S3_BRINGUP.md) §1.

## What this environment deliberately does not provide

The Cortex-M33 freestanding environment can be thin because Cortex-M does a
great deal for you at reset: it loads SP from the vector table, vectors
directly to a handler per exception, and needs no register-window machinery.
Xtensa does none of that, so the obligations below are declared explicitly
rather than assumed — each is a way a first image fails without saying why.

| Obligation | Why it bites |
| --- | --- |
| **Window overflow/underflow vectors** | 64 physical address registers rotated in windows of 16. Exhausting the window raises an exception whose handler spills registers to the stack. Omit it and *ordinary nested calls* fault — at about the depth a UI widget tree reaches. |
| **Level-1 interrupts have no vector** | They arrive through the user/kernel exception vector and must be told apart from genuine exceptions by reading `EXCCAUSE` and `INTERRUPT`. |
| **Interrupt matrix routing** | There is no NVIC. A peripheral source reaches the core only after its `*_INT_MAP` register names one of the core's 32 interrupt numbers. |
| **`CPENABLE` before floating point** | The FPU is a coprocessor, disabled at reset. The first float instruction traps until its `CPENABLE` bit is set. |
| **Three armed watchdogs** | RTC WDT, super WDT and the timer-group MWDTs all run before the image does. Neither feeding nor disarming them produces a boot loop with no output. |
| **Clock-gated peripherals** | A gated peripheral reads back zeros and discards writes. `SYSTEM.PERIP_CLK_EN0/1` comes before every other peripheral transaction. |
| **1024-byte vector alignment** | `VECBASE` ignores its low 10 bits. |

## Single core

This environment describes one hardware thread. The second core is not started.
An image that starts it owns that core's stack, vector base and cache coherence
— and a platform offering two compute blocks does not implicitly select two
compilation targets.
