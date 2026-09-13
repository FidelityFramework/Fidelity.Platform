# STM32H747I-DISCO driver scope and next boundary

Assessment, 2026-09-13. The bounded interactive HelloDISCO checkpoint is
accepted, including debugger-disconnected cold start and controls. Future
hardware work remains a light documentation scaffold. Now focus on Clef's
language/compiler support for DSP and cryptography. The unfinished language
surface is a reason to defer driver APIs and descriptor expansion: encoding
today's workarounds throughout the board would create avoidable refactoring.

The [scaffold handoff](../Profiles/STM32H747I_DISCO_Synth_Reference/scaffold/README.md)
is the pickup point after that language work. Capability names below are
organizational labels. Record hardware facts, dependencies, open decisions and
future acceptance checks now; settle executable representations when their
language support is ready. Audio silence/tone is a future resumption candidate,
not an additional task before the language checkpoint.

The [synth design](STM32H7_SYNTH_DESIGN.md) remains the architecture and numerical
acceptance plan. This document chooses the next driver boundaries; the names
below are proposed module responsibilities, not exported APIs or implemented
drivers. The [product source audit](../Hardware/Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md)
pins the local manuals, BOMs and BSP routes.

## Present capability and evidence

Inventory means source-described hardware or a candidate route. Implementation
means executable native behavior exists. Acceptance must name a particular
check, image and observation: an offset comparison, declared-access ledger,
image check, physical display, or timing measurement establishes a different
fact. There is no single “proven device” status.

| Capability and proposed boundary | Board route or dependency | Current disposition and next acceptance |
| --- | --- | --- |
| `BoardControls` → `ControlEvents` | Active-low LEDs PI12–15; joystick PK2 select, PK3 down, PK4 left, PK5 right, PK6 up with pull-ups | Native combined image accepted: the user confirmed cold start and Left/Right/Center LED controls with the debugger disconnected; Up/Down palette changes were also observed. Pure state/debounce checks remain separate evidence. |
| `DisplaySurface` → `DisplayHost` → `Panel` | AXI SRAM → LTDC → DSI → observed NT35510; PG3 reset, panel CABC backlight route | Final 250190-byte image cold-starts with correct colors before input; ELF load segments and the 230400-byte frame match readback. An intermediate image retained the complete frame across 33 palette updates with zero DSI/LTDC error status. General concurrent repaint remains unimplemented. |
| `I2c4Bus` → `Wm8994Control` | PD12 SCL / PD13 SDA, AF4; codec canonical address `0x1A`, 16-bit register addressing | Pinned BSP and WM8994 datasheet are local. Native I2C/codec control is unimplemented. First gate: bounded transactions, codec identity, reset/mute/routing readback. |
| `AudioClockPlan` → `Sai1Playback` → `PcmOutput` | SAI1 A AF6: PG7 MCLK, PE5 SCK, PE4 FS, PE6 data → WM8994 → CN11 headphones | Source-described only. BSP chooses PLL2 and DMA2 Stream1 with SAI1_A request; the stream is a workload allocation, not fixed wiring. First gate: agreed clocks/framing, silence, fixed tone and captured output. |
| `TouchInput` → `ControlEvents` | Shared I2C4; candidate addresses `0x2A`/`0x38`; PK7 interrupt, EXTI7; PG3 reset shared with panel | FT6x06 BSP supports up to two contacts, but the physical touch device/address and transforms are unobserved. Implement after the audio transport unless HelloDISCO explicitly expands to touch. Contact weight is not accepted pressure/aftertouch. |
| `SdramController` → `DisplayMemory` | FMC, 32 MiB SDRAM; BSP selects bank2 at `0xD0000000` | Inventory envelope only. Resolve UM2411 Bank1 wording against staged connectivity, derive timing/refresh from the selected clock, then memory-test and prove placement/visibility. Needed when the selected UI exceeds internal SRAM, not for the accepted banner. |
| `ReadOnlyAssetStore`, later `PresetStore` | Internal flash already holds the banner; external dual QSPI window candidate `0x90000000`, 128 MiB; CN12 SDMMC1, PC8–12/PD2 AF12, PI8 detect | ROM asset reading is implemented. QSPI/SDMMC and persistence protocols are not. Begin later with bounded reads and integrity checks; define atomic preset updates/recovery before writes. SD 4-bit mode conflicts with camera PC9/PC11 routes. |
| `CodecCapture`, `PdmCapture`, `MidiInput` | Codec SAI1 B on PE3 AF6; PDM SAI4 A on PC1/PE2 AF10 with BDMA/D3 memory; MIDI transport still to select | Inventory and interface names only. Codec input, microphone decimation, external MIDI/USB and M4 workload splitting are later selections. Default microphone PC1 routing conflicts with Ethernet MDC. |
| Radio → control messages (optional) | No onboard radio; select an external module and reconcile STMod+/Pmod/Arduino power and shared bus/pin routes | [Radio scaffold](RADIO_MODEL.md) only. Bluetooth, Wi-Fi and LoRa remain separate capability selections; no radio driver or bound vendor stack is part of HelloDISCO. |

Clock, pin, IRQ, DMA-request and memory ownership belong to the selected
application/profile. Silicon packages own register requirements; the product
owns routes; drivers own protocol transitions. Reuse original `MemorySpace`
instances. A memory envelope neither initializes a device nor grants access.

The [interactive HelloDISCO record](../../MCU/ST/STM32H747I-DISCO/HelloDISCO/experiments/interactive/README.md)
owns the image and acceptance details. The intermediate run's largest foreground
tick gap was 1 ms; this is a measurement of that run, not a worst-case timing
proof. The earlier RGB565 banner's power-cycle acceptance remains a separate
checkpoint. The initial interactive cold-start test exposed corrupted colors;
startup now uses the configured, running controller and the same hidden-layer
palette state machine before first visibility. The user confirmed that a CN2
unplug/reconnect with the debugger disconnected immediately produces the correct
banner, without stripes or prior joystick input. The correction is accepted
for this image; the hardware cause of the earlier color corruption is not
claimed to be established.

## Questions to preserve in the scaffold

The following sketches preserve design intent; their operation names, data
shapes and sequencing APIs remain provisional. No new API or descriptor schema
is selected by this document. Existing HelloDISCO code remains a narrow accepted
implementation rather than the mandatory template for every future driver.

**Shared control bus.** `I2c4Bus` should have one owner and bounded transfers
using canonical 7-bit addresses, explicit register-address width and byte
order. Name NACK, timeout, bus-error and recovery outcomes. Codec startup and
later touch polling share this bus; a touch driver must not reset PG3 or
reconfigure I2C4 behind the display/codec owners. No separate general-purpose
bus scheduler is needed until more than one active consumer requires it.

**Audio output contract.** `PcmOutput` should define prepare-with-silence,
start, completed-half acquisition, publish, mute and stop. Specify sample rate,
significant sample bits, slot size/count/mask, interleaving, endian order,
transaction width, buffer length/alignment and permitted memory master. The
pinned BSP uses four SAI slots and headphone slots 0 and 2; “stereo” alone
does not describe the wire frame. The proposed 48 kHz/64-frame/32-bit-slot
[requirements](../Profiles/STM32H747I_DISCO_Synth_Reference/Requirements.clef)
remain policy until codec, SAI and DMA agree. Do not silently inherit the
BSP header's halfword DMA alignment for a different sample format.

Use one bounded refill owner and distinguish CPU-owned, published/device-owned
and completed buffer halves. Define sequence counts, underrun detection,
silence/mute recovery and stop completion. Circular DMA will not wait for a
late producer. For initial silence/tone, a small aligned pool in the already
selected AXI SRAM is a candidate, subject to DMA2 reachability and layout
checks; moving to D2 SRAM can follow with the consumed image-role extension.
This avoids making arbitrary multi-region startup or external SDRAM a gate
for the first audible sample.

**Clock/resource plan.** Keep one startup authority for HSE and the shared
PLL source. Display currently uses PLL3R plus the DSI PLL and requires initially
disabled PLL1/2/3; an audio PLL2 owner cannot be composed by independently
rerunning that startup routine. Derive actual MCLK/SCK/FS rates and error from
the selected divisors, then measure them; a BSP sampling-rate enum is not
proof of an exact 48 kHz clock. Preserve the timebase premise or reconfigure
its reload deliberately. Any CPU speed or supply-policy change needs its own
accepted sequence rather than being bundled into codec initialization.

**Control and display handoff.** `ControlEvents` should carry bounded note and
parameter changes independently of joystick, touch or later MIDI transport.
Coalesce parameter updates where safe, and specify overflow handling for note
on/off events. Keep pure state/projections separate from bus and frame writes.
HelloDISCO now uses one immutable L8 frame and a bounded palette transition:
disable the layer at vertical blank, wait, update its CLUT in eight foreground
batches, then re-enable at vertical blank. The stream continues with the
background while the layer is hidden. General repaint or buffer reuse still
needs a separate ownership/presentation contract when a future UI requires it.

These are documentation outlines. Until the language-readiness gate is met,
extend the executable slice only where the bounded HelloDISCO proof needs it.
Keep future register inventories, driver APIs and ownership representations in
the scaffold. Empty driver modules, permissive grants and unsupported proof
tags would overstate readiness and freeze premature choices.

## Cutline after HelloDISCO

HelloDISCO has reached its bounded completion point: one autonomous image
combines the landscape display, agreed joystick-driven state and LED animation,
with recorded frame/palette handoff, input behavior and independent cold start.
Touch and synthesizer audio remain outside that demo boundary. Preserve both
static display images as recovery/reference selections.

The final image and readbacks are retained in the
[interactive acceptance archive](../../MCU/ST/STM32H747I-DISCO/HelloDISCO/evidence/hardware/2026-09-13-interactive-initial-palette/readback-checks.json).
The proposed later split is M7 audio/compute with M4 UI.
It requires separate M4 startup/park/reset acceptance, images/stacks, shared
memory publication and peripheral ownership; this image does not establish a
ready second-core boot policy.

After language support is ready and synthesizer implementation resumes, the
first hardware candidate is **codec control plus PCM silence/tone**, with
display load included when testing refill reliability. Then add the native
monophonic DSP model and its numerical harness. Touch, larger/double surfaces,
SDRAM and preset storage follow demonstrated instrument needs. Reserve names
for microphone/input, USB MIDI, Ethernet, camera, M4 and optional foreign
renderers; no driver implementation for those is justified by the present demo.
Do not silently extend HelloDISCO into that audio milestone: the user's intended
boundary after the interactive demo is a return to language/compiler completion.
The audio requirements and unanswered contract questions can be recorded now;
its final Clef API, BAREWire representation and descriptor structure stay open.
DSP numerical semantics and cryptographic bit/constant-time requirements drive
the intervening language/compiler work. A successful display proof establishes
neither of those broader language capabilities.

Before claiming general register-protocol or memory proofs, close RM0399 and
the remaining ES0445 authority/errata gaps, preserve the SVD's interim provenance, reconcile
the physical assembly and selected pin routes, and establish DMA reachability,
ownership, cache visibility and final-image bounds. The WM8994 datasheet and
BSP are useful starting authorities; the native numerical and real-time synth
claims require the separate model, error and deadline evidence already set out
in the design.
The targeted ES0445 Rev 6 §2.13.1 review already corrected LTDC startup:
PLL3R must run before the LTDC register interface is enabled, including before
guard reads. See the [CLUT protocol record](../Hardware/Silicon/MCU/ST/STM32H7/STM32H747XIH6/docs/display/CLUT_SUPPLEMENT.md).
