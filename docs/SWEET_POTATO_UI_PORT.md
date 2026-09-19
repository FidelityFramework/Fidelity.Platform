# Sweet Potato native UI port

Design dated 2026-09-18. The target is a Clef runtime on the Libre Computer
AML-S905X-CC-V2 with native display, touch and selected Mali rendering. Linux is
the reference for hardware behavior and a separate hosted deployment choice.
The [product entry](../Hardware/Products/LibreComputer/AML_S905X_CC_V2/README.md)
and [KeyStation reference profile](../Profiles/SweetPotato_KeyStation_Reference/README.md)
are documentation scaffolds. No hardware acceptance is claimed here.

The UI contract is described in
[Fidelity.UI's KeyStation design](../../Fidelity.UI/docs/09_sweet_potato_keystation.md).
This document assigns the lower-level work needed to make that contract run.
The key-authority service, Renesas HUK and encrypted storage remain separate.

## Boot boundary and execution environment

U-Boot documents the S905X boot image's dependency on Amlogic firmware components
whose source is unavailable. A Clef unikernel can own execution after an accepted
firmware handoff. Replacing Boot ROM, DDR initialization and all secure firmware
is a separate scope and is not assumed by this port. See the
[U-Boot board and firmware instructions](https://docs.u-boot.org/en/latest/board/amlogic/libretech-cc.html).

Pin the boot medium, firmware/FIP image, U-Boot build, device tree and selected
handoff protocol. Record the entry exception level, CPU state, MMU/cache state,
physical RAM map, reserved regions, secondary-core state and available firmware
services. Admit only the memory the runtime actually owns. Firmware and device
reservations must survive allocator initialization.

A UEFI path needs the correct memory map and Boot Services termination. If it
uses a GOP-provided framebuffer, retain its address, extent, pitch and format
and establish their post-handoff validity. Do not continue calling Boot Services
after exit. A direct U-Boot handoff needs an equally explicit entry and memory
contract. A firmware-initialized screen is useful early evidence, but it does
not demonstrate native display mode initialization.

The inspected V2 U-Boot configuration enables Meson video and device-tree simple
framebuffer support. It is a research reference, not an accepted boot artifact.
Its text address and UART settings must not be copied into a new image without
the corresponding memory and board checks. See the
[pinned configuration](https://github.com/u-boot/u-boot/blob/v2025.01/configs/libretech-cc_v2_defconfig).

The freestanding environment needs an AArch64 image and ABI, entry code,
exception vectors, GIC interrupt handling, an architectural timer, MMIO access,
cache operations and a bounded allocator. Bring up one core first. Secondary
cores can remain parked until their boot and ownership protocol is accepted.
Compiler output alone does not establish any of these hardware behaviors.

## Hardware and driver decomposition

The inspected Linux device tree identifies `libretech,aml-s905x-cc-v2` and
`amlogic,meson-gxl`. Its include chain selects the S905X and Mali-450 topology.
Treat this as source evidence to reconcile with manuals and the physical unit.
The [source manifest](../Hardware/Products/LibreComputer/AML_S905X_CC_V2/docs/SOURCE_MANIFEST.md)
pins the files behind the following work packages.

| Work package | Responsibilities | First independent evidence |
| --- | --- | --- |
| Board boot | Memory, serial console, entry state and firmware reservations | Repeatable serial report from the Clef image |
| CPU/device substrate | Interrupts, timer, MMIO, cache visibility, allocation | Interrupt/timer operation and bounded memory tests |
| Meson display | Power, clocks/resets, Canvas, OSD/VIU, VPP, VENC and HDMI bridge/PHY | Native mode initialization and alternating CPU-rendered surfaces |
| USB host/input | Host controller, PHY, VBUS, hub enumeration and HID reports | Recorded touch contacts and calibrated coordinates |
| Mali device | Power/reset/clock, MMU, job submission, interrupts and recovery | Known render with completion and checked readback |
| Mali renderer | Shader binaries, command/state generation, tiler resources and bounded drawing | CPU/GPU comparison of the admitted paint operations |
| UI realization | Reactive areas, text, input/focus, damage and transitions | Repeated interaction with compatible visual and hit-test revisions |

The first display implementation should use one accepted linear RGB format and
two surfaces with explicit ownership. Canvas supplies address/stride mapping to
the Meson pipeline. OSD/VIU fetches pixels, VPP processes/composes them, VENC
generates timing and the HDMI block transmits it. The driver must initialize the
necessary clocks, resets and PHY, read the panel's EDID, handle connection state
and switch buffers at an accepted presentation boundary. Multiple planes,
hardware scaling and rotation can follow only after separate acceptance.

The GXL primary-plane path in the inspected Linux driver exposes linear
modifiers. Support for compressed buffers on other Meson generations does not
establish it here. Record format, pitch, alignment, address range and cache
policy for each surface. Distinguish rendering completion from the point at
which scanout stops reading an older surface. See
[primary-plane setup](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/meson/meson_plane.c),
[Canvas](https://github.com/torvalds/linux/blob/v6.12/drivers/soc/amlogic/meson-canvas.c)
and [HDMI integration](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/meson/meson_dw_hdmi.c).

The USB path needs more than an HID parser. The inspected GXL topology includes
a DWC3 host route, USB2 PHY and board power controls. Establish the actual host
controller mode, root/external hub topology, interrupt and DMA requirements,
VBUS and connector routing before parsing the panel's touch reports. Preserve
contact identity, press/release and cancellation semantics in the normalized
input stream. Disconnect must retire contacts and device-owned buffers.

## Panel mode qualification

The selected [Waveshare 7.9inch HDMI LCD](../Hardware/Products/Waveshare/7_9inch_HDMI_LCD/README.md),
SKU 17916 and ASIN B087CNJYB4, has a 400 × 1280 physical matrix, HDMI video
and USB touch. Record EDID bytes/checksum, accepted pixel
clock and porch/sync values, refresh rate, USB descriptors and report samples.
Keep the panel's active area, PCB outline, mounting holes, connector clearances
and cable bend space in the mechanical design.

The Linux Meson non-CEA mode check accepts horizontal sizes 400 through 1920 and
vertical sizes 480 through 1920, with restricted mode flags. Thus a 400 × 1280
candidate passes its dimension tests, whereas a 1280 × 400 candidate does not.
Clock synthesis, encoder setup and the actual panel still require validation.
The proposed path uses portrait scanout with a rotated 1280 × 400 UI. Check the
render transform and inverse touch transform together. See
[`meson_venc_hdmi_supported_mode`](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/meson/meson_venc.c),
[HDMI mode validation](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/meson/meson_encoder_hdmi.c)
and [clock programming](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/meson/meson_vclk.c).

Neither HDMI 2.0 marketing nor a successful Raspberry Pi configuration proves
this panel mode works on the selected S905X firmware or driver. Resolve that
question before building UI assumptions around a finished rack assembly.

## A restricted Mali port

Mali-450 uses the Utgard architecture and the Lima driver family. Linux's kernel
driver manages the device, memory and execution. Mesa generates graphics state,
commands and shader programs. A native port needs responsibilities from both.
The inspected references are Linux v6.12 and Mesa 24.3.0, selected as stable
source snapshots rather than recommendations for a production Linux image.

Start with a known shader set and a bounded draw vocabulary: solid/textured
triangles, scissor clips, premultiplied alpha and glyph masks. CPU code can shape
text and construct geometry. Offline shader preparation is an explicit build
dependency until a Clef producer exists. LLVM's AArch64 output does not generate
Mali GPU instructions.

The native device path must cover GPU identification, power and reset ordering,
clock selection, MMU page tables, device-visible storage, cache maintenance,
geometry/pixel submission, interrupts, completion and timeouts. The geometry
processor finishing does not mean that the pixel processor has finished writing
the surface. Tiler heaps, command streams and referenced textures must remain
alive for the complete job. Record the supported render-state combinations and
reject unsupported jobs before submission. The relevant starting points are
[device initialization](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/lima/lima_device.c),
[scheduling](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/lima/lima_sched.c)
and [MMU management](https://github.com/torvalds/linux/blob/v6.12/drivers/gpu/drm/lima/lima_mmu.c).

Mesa's [draw generation](https://gitlab.freedesktop.org/mesa/mesa/-/blob/mesa-24.3.0/src/gallium/drivers/lima/lima_draw.c)
and [job construction](https://gitlab.freedesktop.org/mesa/mesa/-/blob/mesa-24.3.0/src/gallium/drivers/lima/lima_job.c)
show why a binding to the kernel half alone cannot render a UI. A first native
implementation can deliberately support fewer states and shader forms, while
preserving their actual hardware encoding and ordering requirements.

Develop the first GPU workload independently of scanout: render into a target,
wait for completion, establish CPU visibility and compare readback. Then connect
the completed target to the accepted display path. If rendering and scanout
representations differ, introduce and measure the required resolve or copy.

Timeout recovery needs a device-specific sequence that stops access, resets
the affected engines and retires or quarantines resources. Cancelling an area
or abandoning a job handle cannot establish this. If recovery leaves ownership
uncertain, retain the affected allocations until a stronger reset boundary.

## GE2D and video decoding

GE2D merits a separate study if a GXL-specific blitter would simplify rotation or
copies. The inspected upstream driver matches `amlogic,axg-ge2d`. Its own missing
features list includes scaling and source global alpha. The inspected GXL device
tree chain does not supply a GE2D node. This does not prove the silicon lacks the
block. It means that this driver is insufficient evidence for a GXL UI port.
Establish registers, clock/reset/interrupt routing and required operations before
adding it to the profile. See the
[GE2D implementation](https://github.com/torvalds/linux/blob/v6.12/drivers/media/platform/amlogic/meson-ge2d/ge2d.c).

The AVE10 video-decoding claim concerns compressed media. The first KeyStation
panel needs no video decoder driver. Codec/profile support, firmware and video
surface integration would each need their own acceptance if playback becomes
a product requirement.

## Farscape and the full-port option

For hosted Linux, bind userspace graphics and input interfaces: EGL/GLES and
GBM/DRM/KMS for a direct-display application, or the selected Wayland route.
Mesa Lima and Linux Meson provide the hardware implementation. Linux AArch64
bindings need their own ABI/package selection.

For native execution, a temporary C implementation is possible if its required
services are supplied explicitly. Linux allocation, DMA mapping, fence objects,
interrupt handling, work queues and DRM infrastructure do not arrive through an
FFI declaration. A small isolated algorithm or shader tool is a more bounded
interoperability step than importing a kernel driver's infrastructure.

The proposed full port implements the runtime driver and restricted command
producer in Clef. Retain source provenance and file-level license obligations
for translated code. Rewriting syntax or generating Farscape bindings does not
remove those obligations. The source manifest records research inputs without
vendoring their implementations.

## Memory, ownership and scheduling

Account physical DRAM once. CPU mappings, GPU virtual addresses and display/DMA
views describe access to that storage. For each handoff record backing extent,
representation, device reachability, visibility operations, producer completion
and consumer release. The shared
[Sidecar contracts](../Contracts/Sidecar.clef) offer reference vocabulary, but
their structural checks neither establish a mapping nor perform synchronization.

The presentation owner should bound pending work. Under load, it may coalesce
replaceable visual states and discard obsolete unsubmitted results. Preserve
ordered application commands separately. Retain every resource referenced by
submitted work until all device consumers release it.

At the selected resolution, two tightly packed 32-bit surfaces consume
4,096,000 bytes. Continuous 60 Hz scanout reads 122.88 MB/s before overhead.
The [UI budget calculation](../../Fidelity.UI/docs/09_sweet_potato_keystation.md#the-rack-mounted-display)
also counts rendering and rotation. Reserve additional bounded storage for
atlases, layers, geometry, job descriptors, GPU page tables and input queues.
Determine allocation alignment and address constraints from the accepted driver.

Measure service interference with GPU/display traffic and CPU rendering. A
predictable hardware inventory simplifies measurement, but does not establish
a worst-case deadline by itself. Background projection updates, prewarming and
animation clocks must each have an explicit owner and resource allowance.

## Acceptance ladder

| Gate | Required evidence | What remains outside that gate |
| --- | --- | --- |
| 0. Identify | Board revision/SKU, firmware and DTB hashes, RAM reservations, Linux device inventory, panel EDID and touch reports | Native execution |
| 1. Boot | Repeated Clef serial output, exception/timer operation and recorded handoff state on one core | Graphics and secondary-core scheduling |
| 2. CPU surface | Known pattern and layout rendered with correct pitch/cache handling into an accepted framebuffer | Native mode setup if firmware supplied the screen |
| 3. Native display/input | Meson initialization, exact panel timing, safe buffer switching, USB contacts and rotation verified together | GPU rendering |
| 4. Reactive areas | CPU reference scene with selective changes, text resize, overlap, focus and repeated disposal | Accelerated equivalence |
| 5. Mali | Known draw/readback, completion, visibility, bounded resources and fault recovery, followed by scanout | General OpenGL or arbitrary shaders |
| 6. Product drawing | Glyphs/images, alpha and clips compared with CPU reference, selected fades and retargeting, bounded queues | Unmeasured combinations and unselected operations |
| 7. Deployment | Repeated cold boots, idle/active/first-use measurements, input latency, sustained load, cable reconnect and recovery | Key-authority and storage-crypto acceptance |

Use the same deterministic scenes on the hosted and native routes. Include
removing a translucent panel, interrupting a fade, hiding a view with outstanding
GPU work, changing text width and reopening a prepared view after cache eviction.
Compare forced full redraw with partial updates. Record image differences with
an explicit tolerance where GPU precision differs, while checking geometry,
clipping and resource ownership exactly where applicable.

The source pack should retain the image/compiler/firmware versions, scene inputs,
capture method and measured distributions. A successful screenshot alone cannot
establish correct buffer reuse, input latency or recovery.

The remaining implementation work starts with the freestanding AArch64 board
path and the selected panel's S905X mode test, followed by the register and
initialization work for Meson and Mali. None requires changing Fidelity.UI's component
syntax. They determine which renderer and presentation capabilities the future
profile can honestly select.
