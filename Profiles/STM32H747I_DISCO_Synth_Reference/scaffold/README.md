# DISCO scaffold handoff

This is an organized place to resume instrument work after Clef's required
language/compiler support is ready. It contains documentation, not selectable
drivers, descriptor APIs or proof declarations. The accepted HelloDISCO images
remain the concrete reference implementation for their limited workloads.

The sequence is: **finish the HelloDISCO proof → frame the hardware scaffold →
focus on Clef for DSP and cryptography → resume instrument implementation**.
Finishing HelloDISCO does not start an audio-driver or full-device mapping phase.

## What to preserve, and what stays open

Preserve source provenance, observed board routes, memory/peripheral identities,
accepted images, hazards already encountered, dependency ordering and future
acceptance questions. Keep APIs, descriptor decomposition, storage types,
effect/ownership encoding and compiler-specific workarounds provisional.
The existing `Requirements.clef` arithmetic is a budget sketch, not the shape
of the eventual PCM or framebuffer interface.

Organize each future work item under the areas below. A short Markdown note is
enough until there is a concrete post-language implementation task; empty Clef
modules and speculative register declarations are unnecessary.

| Area | Questions and evidence to carry forward | Starting record |
| --- | --- | --- |
| Boot and lifecycle | M7/M4 ownership, boot addresses/options, reset, clocks/power/timebase, watchdog/RTC needs, startup and fault recovery | [Connection evidence](../../../Hardware/Products/ST/STM32H747I_DISCO/docs/CONNECTION_CHECK.md) |
| Memory and device transfers | Reachability, image roles, alignment/extents, MPU/cache policy, DMA/BDMA/MDMA/DMA2D requests, publication and completion | [Memory and proof design](../../../docs/STM32H7_SYNTH_DESIGN.md) |
| Board controls | Joystick/LED behavior, debounce, event ownership, optional analog controls/encoders | [Driver inventory](../../../docs/STM32H7_DRIVER_ROADMAP.md) |
| Shared control bus | I2C4 clients, canonical addresses, register widths/order, serialization, timeout/recovery and shared reset lines | [Driver inventory](../../../docs/STM32H7_DRIVER_ROADMAP.md) |
| Audio output and capture | Codec routes, SAI framing, clock error, sample/slot distinction, bounded refill, silence/mute, optional microphone/PDM paths | [Synth design](../../../docs/STM32H7_SYNTH_DESIGN.md) |
| Display and touch | Accepted NT35510 timing, immutable-frame handoff, future repaint ownership, coordinate transforms and shared resources | [Display model](../../../docs/DISPLAY_MODEL.md) |
| Storage | Internal assets, QSPI/SDMMC, SDRAM needs, read integrity, presets and interrupted-write recovery | [Product audit](../../../Hardware/Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md) |
| Diagnostics and validation | Fault/reset records, VCP/trace needs, timing measurements, reproducible binaries, numeric/reference and hardware evidence | [Accepted display profile](../../STM32H747I_DISCO_HelloDISCO_Display/README.md) |
| DSP and cryptography premises | Numeric and bit semantics, bounded storage, lowering fidelity, side-channel obligations and entropy requirements | [Language readiness](LANGUAGE_READINESS.md) |
| Optional expansion | MIDI/USB, network, camera, SPDIF/DFSDM, second-core workloads and their pin/resource conflicts | [Board routes](../../../Hardware/Products/ST/STM32H747I_DISCO/docs/CONNECTOR_MAP.md) |

## Minimal note for a future work item

Record its purpose; source-backed hardware facts; dependencies/shared resources;
requirements and unresolved choices; needed language/compiler capabilities;
and the observation or check that would accept its first implementation.
State whether a fact is inventoried, implemented, tested or observed on hardware.
Name an eventual code owner only as a responsibility, not as a frozen namespace.

Cryptography is a language/software requirement as well as a possible platform
capability. This STM32H747 has RNG and CRC, not the H757's CRYP/HASH accelerators.
Do not infer HASH hardware from the H747 header's compatibility IRQ alias.
Sources: DS12930 Rev 3 features/Table 2/§3.28 in the
[local datasheet](../../../Hardware/Silicon/MCU/ST/STM32H7/docs/stm32h747ag.pdf),
and [ST's H747/H757 comparison](https://www.st.com/en/microcontrollers-microprocessors/stm32h747-757.html).
An RNG register inventory alone does not accept an entropy or key-handling path.

## Ready to resume

The scaffold is sufficiently framed when every required area has an owner or
document location, known source gaps and dependencies are visible, and the
next experiment has an acceptance question. It does not need a full register
catalog or an executable placeholder for each peripheral.

Before resuming driver implementation, close the relevant
[language-readiness checks](LANGUAGE_READINESS.md), revisit the provisional
representations against that implementation, and resolve the selected hardware
authority gaps. Then choose one small consumer. Codec control plus PCM
silence/tone is the current first audio candidate, subject to that review.
Touch, external memory and other capabilities wait for their actual consumers.
