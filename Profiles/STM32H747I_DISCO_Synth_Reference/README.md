# STM32H747I-DISCO synthesizer reference

This package records the initial workload choices for a native Clef monosynth:
three oscillators, a nonlinear ladder-style filter, envelopes, and a touch UI.
It has no `[platform]` selection and cannot produce or deploy an MCU image.
The values are proposed requirements, not hardware capabilities or verified bounds.

[Requirements.clef](Requirements.clef) keeps the first budget calculations in
authored Clef. The [design and acceptance plan](../../docs/STM32H7_SYNTH_DESIGN.md)
defines ownership, BAREWire extensions, numerical obligations, and bring-up stages.
The [source audit](../../Hardware/Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md)
records which board premises are available and which remain unresolved.

| Initial choice | Derived requirement |
| --- | --- |
| 48 kHz, 64 frames/block | 1⅓ ms block period |
| Stereo, proposed 4-byte sample slots | 512 bytes/block; 1,024 bytes for two buffers |
| 25% deadline reserve | 1 ms proposed render budget, subject to full scheduling analysis |
| 800 × 480 RGB565, two surfaces | 768,000 bytes/surface; 1,536,000 bytes total |
| 30 UI updates/second | 23,040,000 bytes/s if each update writes a full surface |

Framebuffer writes are additional to display scanout, DMA2D reads/writes,
refresh, and contention. The audio block period is a refill deadline, not the
complete touch-to-sound or codec latency. Memory placement and scan timing are
deliberately unresolved until the part and mounted board have accepted facts.

No generic floating-error, DMA-ownership, WCET, or whole-program memory-safety
proof is implied by checking this package. Verification status is recorded in
the design document. Drivers and the future executable workload will consume
the shared silicon/product declarations and own their explicit access plans.
