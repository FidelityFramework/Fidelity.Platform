# Sweet Potato KeyStation reference selection

Design selection, 2026-09-18. This directory has no `.fidproj`, authoritative
platform export or deployable image. Its dependencies are requirements for a
future profile, with acceptance tracked in the
[native port plan](../../docs/SWEET_POTATO_UI_PORT.md).

| Selection | Intended owner |
| --- | --- |
| AML-S905X-CC-V2 board wiring and fitted resources | [Libre Computer product entry](../../Hardware/Products/LibreComputer/AML_S905X_CC_V2/README.md) |
| S905X CPU/GPU/display topology and shared DRAM | Future S905X silicon package |
| Native execution | Future freestanding AArch64 environment with an accepted firmware handoff |
| Hosted execution | Separate Linux AArch64 environment, Mesa Lima and Meson DRM/KMS, optionally Wayland |
| Components, reactive areas and paint semantics | [Fidelity.UI](../../../Fidelity.UI/docs/09_sweet_potato_keystation.md) |
| Physical panel | [Waveshare 7.9inch HDMI LCD, SKU 17916](../../Hardware/Products/Waveshare/7_9inch_HDMI_LCD/README.md), HDMI/USB, 400 × 1280 |
| Logical panel | Horizontal 1280 × 400 layout with matching input transform |

Native execution is the primary port objective. The Linux selection provides a
deployable alternative and hardware reference. Neither selection can currently
be obtained by substituting this board into the existing Linux x86_64 or
freestanding MCU packages.

Begin with one UI/presentation owner and bounded service snapshots. Keep
command occurrences ordered with an explicit overflow policy. Independently
owned background observation can stay active while a panel is hidden. Visual
preparation, texture retention and animation each need their own budget.

The initial graphics workload is a CPU reference renderer followed by a
restricted Mali renderer. Establish two 32-bit scanout-capable buffers as an
initial sizing case, about 3.906 MiB at the selected resolution before pitch
alignment. Add a third buffer only when the accepted submission/presentation
policy calls for it. Declare separate maxima for atlas bytes, retained layers,
geometry/command storage, GPU page tables, pending frames and damage records.
These figures are design inputs, not admitted memory or timing bounds.

Measure idle, first-view activation, prepared-view activation, live updates,
transitions and GPU recovery separately. Select a UI frame-rate target after
qualifying the panel's scanout mode. A 60 Hz scanout can coexist with slower
visual updates. The target product may omit decorative motion when resources
are constrained, while preserving control state and service operation.

The UI exercise uses synthetic KeyStation state. Root-of-trust integration,
Renesas HUK ownership, ciphered storage and service availability requirements
remain outside this reference selection's acceptance.
