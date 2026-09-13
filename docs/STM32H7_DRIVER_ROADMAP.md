# STM32H747I-DISCO driver scope and next boundary

Assessment, 2026-09-13. Finish HelloDISCO's agreed joystick/LED behavior in one
display image, then take the requested Clef language/compiler checkpoint.
Name the synthesizer capabilities and their contracts now; the next hardware
increment when instrument work resumes is one audio-output chain producing
silence and a fixed tone. A complete peripheral register catalog, SDRAM,
touch, a general graphics runtime and USB MIDI are not prerequisites for that
first audio milestone.

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
| `BoardControls` → `ControlEvents` | Active-low LEDs PI12–15; joystick PK2 select, PK3 down, PK4 left, PK5 right, PK6 up with pull-ups | GPIO native code and pure state/debounce checks exist. Keep those results distinct from complete combined joystick/display hardware acceptance. |
| `DisplaySurface` → `DisplayHost` → `Panel` | AXI SRAM → LTDC → DSI → observed NT35510; PG3 reset, panel CABC backlight route | Native banner is visible, frame bytes match the asset, and CN2 power-cycle return is user-confirmed. A concurrent repaint protocol is not implemented by the immutable-frame result. |
| `I2c4Bus` → `Wm8994Control` | PD12 SCL / PD13 SDA, AF4; codec canonical address `0x1A`, 16-bit register addressing | Pinned BSP and WM8994 datasheet are local. Native I2C/codec control is unimplemented. First gate: bounded transactions, codec identity, reset/mute/routing readback. |
| `AudioClockPlan` → `Sai1Playback` → `PcmOutput` | SAI1 A AF6: PG7 MCLK, PE5 SCK, PE4 FS, PE6 data → WM8994 → CN11 headphones | Source-described only. BSP chooses PLL2 and DMA2 Stream1 with SAI1_A request; the stream is a workload allocation, not fixed wiring. First gate: agreed clocks/framing, silence, fixed tone and captured output. |
| `TouchInput` → `ControlEvents` | Shared I2C4; candidate addresses `0x2A`/`0x38`; PK7 interrupt, EXTI7; PG3 reset shared with panel | FT6x06 BSP supports up to two contacts, but the physical touch device/address and transforms are unobserved. Implement after the audio transport unless HelloDISCO explicitly expands to touch. Contact weight is not accepted pressure/aftertouch. |
| `SdramController` → `DisplayMemory` | FMC, 32 MiB SDRAM; BSP selects bank2 at `0xD0000000` | Inventory envelope only. Resolve UM2411 Bank1 wording against staged connectivity, derive timing/refresh from the selected clock, then memory-test and prove placement/visibility. Needed when the selected UI exceeds internal SRAM, not for the accepted banner. |
| `ReadOnlyAssetStore`, later `PresetStore` | Internal flash already holds the banner; external dual QSPI window candidate `0x90000000`, 128 MiB; CN12 SDMMC1, PC8–12/PD2 AF12, PI8 detect | ROM asset reading is implemented. QSPI/SDMMC and persistence protocols are not. Begin later with bounded reads and integrity checks; define atomic preset updates/recovery before writes. SD 4-bit mode conflicts with camera PC9/PC11 routes. |
| `CodecCapture`, `PdmCapture`, `MidiInput` | Codec SAI1 B on PE3 AF6; PDM SAI4 A on PC1/PE2 AF10 with BDMA/D3 memory; MIDI transport still to select | Inventory and interface names only. Codec input, microphone decimation, external MIDI/USB and M4 workload splitting are later selections. Default microphone PC1 routing conflicts with Ethernet MDC. |

Clock, pin, IRQ, DMA-request and memory ownership belong to the selected
application/profile. Silicon packages own register requirements; the product
owns routes; drivers own protocol transitions. Reuse original `MemorySpace`
instances. A memory envelope neither initializes a device nor grants access.

## Small outlines worth settling now

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
`DisplaySurface` needs one explicit ownership/presentation transition before
HelloDISCO repaints; the currently published banner is immutable. Pick a
bounded pause/repaint/resume or completed-buffer handoff appropriate to the
small demo, and test its visible result before introducing a general renderer.

These are contract outlines. Add register declarations and Clef APIs only with
their concrete consumer, bounded protocol and validation. Empty driver modules,
permissive grants and unsupported proof tags would overstate readiness.

## Cutline after HelloDISCO

HelloDISCO is complete when one autonomous image combines the accepted
landscape display with the agreed joystick-driven state and LED animation,
and records its render handoff, input behavior and independent reset result.
Touch and synthesizer audio remain outside that demo boundary. Preserve both
static display images as recovery/reference selections.

When synthesizer implementation resumes, the next executable increment is
**codec control plus PCM silence/tone**, with
display load included when testing refill reliability. Then add the native
monophonic DSP model and its numerical harness. Touch, larger/double surfaces,
SDRAM and preset storage follow demonstrated instrument needs. Reserve names
for microphone/input, USB MIDI, Ethernet, camera, M4 and optional foreign
renderers; no driver implementation for those is justified by the present demo.
Do not silently extend HelloDISCO into that audio milestone: the user's intended
boundary after the interactive demo is a return to language/compiler completion.
The audio contract can be settled now without starting its register implementation.

Before claiming general register-protocol or memory proofs, close RM0399 and
ES0445 authority/errata gaps, preserve the SVD's interim provenance, reconcile
the physical assembly and selected pin routes, and establish DMA reachability,
ownership, cache visibility and final-image bounds. The WM8994 datasheet and
BSP are useful starting authorities; the native numerical and real-time synth
claims require the separate model, error and deadline evidence already set out
in the design.
