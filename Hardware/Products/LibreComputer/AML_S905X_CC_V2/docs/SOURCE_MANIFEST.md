# Sweet Potato source manifest

Inspected 2026-09-18 for the [native UI port](../../../../../docs/SWEET_POTATO_UI_PORT.md).
This is a research inventory. No driver source is vendored here and no board was
booted as part of this review.

## Manufacturer and project references

| Source | Use in this design |
| --- | --- |
| [Libre Computer V2 announcement](https://hub.libre.computer/t/2023-09-01-libre-computer-aml-s905x-cc-v2-sweet-potato-now-available/2831) | V2 DDR4, SPI boot storage, supply and board identity |
| [Libre Computer product](https://libre.computer/products/aml-s905x-cc-v2/) | Product identity and manufacturer media claims |
| [U-Boot board guide](https://docs.u-boot.org/en/latest/board/amlogic/libretech-cc.html) | S905X board family, V2 configuration and binary firmware dependency |
| [Mesa Lima](https://docs.mesa3d.org/drivers/lima.html) | Mali-450/Utgard support, graphics API limits and render/display separation |
| [Linux Meson DRM](https://docs.kernel.org/gpu/meson.html) | Display block vocabulary and driver organization |
| [Waveshare panel wiki](https://www.waveshare.com/wiki/7.9inch_HDMI_LCD) | Selected SKU 17916, 400 × 1280 HDMI/USB panel, touch and orientation |
| [Waveshare product](https://www.waveshare.com/7.9inch-HDMI-LCD.htm) | Panel selection and physical integration reference |
| [YoshiPi carrier](https://github.com/yoshimoshi-garage/yoshipi) | Separate Pi Zero 2 W/ADC/touchscreen appliance comparison |

The project owner selected Waveshare SKU 17916, ASIN B087CNJYB4, with UPCs
738628504692 and 614961956704. The [panel entry](../../../Waveshare/7_9inch_HDMI_LCD/README.md)
records the manufacturer and Amazon sources that confirm those identifiers.
Product identity is established. Native driver behavior requires hardware acceptance.

## Source snapshots

Linux v6.12, U-Boot v2025.01 and Mesa 24.3.0 are reproducible reference selections.
They are not a recommendation to deploy these versions. SHA-256 values below
identify the exact raw bytes read. The final two Linux `master` observations
cross-check the mode filter and GE2D scope on the review date. Their URLs are
mutable, so recover those observations by an immutable revision before using
them as implementation inputs.

| Inspected source | Bytes | SHA-256 |
| --- | ---: | --- |
| [linux-v6.12/arch/arm64/boot/dts/amlogic/meson-gxl-s905x-libretech-cc-v2.dts](https://raw.githubusercontent.com/torvalds/linux/v6.12/arch/arm64/boot/dts/amlogic/meson-gxl-s905x-libretech-cc-v2.dts) | 6196 | `91f746b3e443c764ae5b15b35e8af9ddb0cdc11d1c7cf2b16b590b73fcc7661c` |
| [linux-v6.12/arch/arm64/boot/dts/amlogic/meson-gxl-s905x.dtsi](https://raw.githubusercontent.com/torvalds/linux/v6.12/arch/arm64/boot/dts/amlogic/meson-gxl-s905x.dtsi) | 375 | `968602652d3f20522de6a190e2f5070b0fdb750c61805c3dd2b12bfe2b90b086` |
| [linux-v6.12/arch/arm64/boot/dts/amlogic/meson-gxl.dtsi](https://raw.githubusercontent.com/torvalds/linux/v6.12/arch/arm64/boot/dts/amlogic/meson-gxl.dtsi) | 18222 | `78d3c9fbd79e9fbc4aa692eea025507e8193390e70cf91a2b9615891f50966dd` |
| [linux-v6.12/arch/arm64/boot/dts/amlogic/meson-gx.dtsi](https://raw.githubusercontent.com/torvalds/linux/v6.12/arch/arm64/boot/dts/amlogic/meson-gx.dtsi) | 16361 | `bceec1eb3ada19aa349a032f996530479218c5568af5ec58733b21d15d3b54cf` |
| [linux-v6.12/drivers/gpu/drm/lima/lima_device.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/lima/lima_device.c) | 12750 | `c46e6b66784ae6b21b3d334b8543fa84b8e21abee82ec00a092a4d9976b326a6` |
| [linux-v6.12/drivers/gpu/drm/lima/lima_sched.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/lima/lima_sched.c) | 13814 | `586ab1b4c60432e298c45b49f98d689b684379f7ccaece8caceb999ca301db34` |
| [linux-v6.12/drivers/gpu/drm/lima/lima_mmu.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/lima/lima_mmu.c) | 4465 | `d4297482b2705d4b645a42f168ab6cc3c7242cc6800002d07fd62a49899af91b` |
| [linux-v6.12/drivers/gpu/drm/meson/meson_drv.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/meson/meson_drv.c) | 14281 | `2141b794d10cd4aad28d58cbf36c98beacc8f80fa54b05e70f1c44ce8b8185bc` |
| [linux-v6.12/drivers/gpu/drm/meson/meson_plane.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/meson/meson_plane.c) | 17214 | `5a0595ba23010c83117c23f5481130ab5084cd4409b279d09841b40cdf729968` |
| [linux-v6.12/drivers/gpu/drm/meson/meson_overlay.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/meson/meson_overlay.c) | 27184 | `d4aea46b12db6a130dda896088aafaff425572846927a027f7d87bff3480e27a` |
| [linux-v6.12/drivers/gpu/drm/meson/meson_dw_hdmi.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/meson/meson_dw_hdmi.c) | 25235 | `7b033164663731446274a65e68a68b1eafdc4a8b19e63730c3ad0ae8fc02ff2b` |
| [linux-v6.12/drivers/media/platform/amlogic/meson-ge2d/ge2d.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/media/platform/amlogic/meson-ge2d/ge2d.c) | 26772 | `241e9ba0655b337c0abe9a42dcf1cde8055c53d78e4a749f3e00ad63e83c1aab` |
| [linux-v6.12/drivers/soc/amlogic/meson-canvas.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/soc/amlogic/meson-canvas.c) | 5499 | `6ee42f7d89fabeee25d4973b1782e1e39be2568f11f41d60b99aa332179f4639` |
| [linux-v6.12/include/uapi/drm/lima_drm.h](https://raw.githubusercontent.com/torvalds/linux/v6.12/include/uapi/drm/lima_drm.h) | 5052 | `118eafe604a7811402ab4d254a23038beac454e97d3ea03a7f96bc2382becf4e` |
| [uboot-v2025.01/libretech-cc_v2_defconfig](https://raw.githubusercontent.com/u-boot/u-boot/v2025.01/configs/libretech-cc_v2_defconfig) | 2137 | `e2db52a0f116d13638e70bbf380dd6ec78a28b3defee51b39c25479ab42d7946` |
| [mesa-24.3.0/lima_job.c](https://gitlab.freedesktop.org/mesa/mesa/-/raw/mesa-24.3.0/src/gallium/drivers/lima/lima_job.c) | 33901 | `ec494d9e3851007fb9871ae5bcab42b15a28578534d0e324616ce4d6a68407aa` |
| [mesa-24.3.0/lima_draw.c](https://gitlab.freedesktop.org/mesa/mesa/-/raw/mesa-24.3.0/src/gallium/drivers/lima/lima_draw.c) | 40372 | `ee9d207c12619f7c7e67103485f9cd88ef200a70539e022e0faf7f492735a658` |
| [linux-v6.12/arch/arm64/boot/dts/amlogic/meson-gxl-mali.dtsi](https://raw.githubusercontent.com/torvalds/linux/v6.12/arch/arm64/boot/dts/amlogic/meson-gxl-mali.dtsi) | 404 | `91cc7d1dc54d43f6d7999044c0ed2eb8e7b8c95e199c89c59f68e403132107cf` |
| [linux-v6.12/drivers/gpu/drm/meson/meson_encoder_hdmi.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/meson/meson_encoder_hdmi.c) | 14845 | `8426117331016589999d524414ce8cbb50ec9d614f569a04d2d77b285e45ee4d` |
| [linux-v6.12/drivers/gpu/drm/meson/meson_venc.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/meson/meson_venc.c) | 58512 | `f893666016848299bc3b4e2a7ed6140921236b1f4634199494856dd2d4495e18` |
| [linux-v6.12/drivers/gpu/drm/meson/meson_vclk.c](https://raw.githubusercontent.com/torvalds/linux/v6.12/drivers/gpu/drm/meson/meson_vclk.c) | 32051 | `0e262f9c260911c0eb85e36f88039327b06c419659d2dd0a3c3f0724624fba70` |
| [linux-v6.12/arch/arm64/boot/dts/amlogic/meson-gx-mali450.dtsi](https://raw.githubusercontent.com/torvalds/linux/v6.12/arch/arm64/boot/dts/amlogic/meson-gx-mali450.dtsi) | 1512 | `5fd7aebb76632f81421a622cea6003b1e881642452609786f3c1ff07ff0c30e7` |
| [linux-master/meson_venc.c](https://raw.githubusercontent.com/torvalds/linux/master/drivers/gpu/drm/meson/meson_venc.c) | 58512 | `6b8dc8df5501d9fe13cede41f80c09f1ca5d087b9a8167dfed7fa93f24949334` |
| [linux-master/ge2d.c](https://raw.githubusercontent.com/torvalds/linux/master/drivers/media/platform/amlogic/meson-ge2d/ge2d.c) | 26768 | `9203226f70468249a3ea9e09bea8e9037533b1cf5e189e7b905a5a3ed80deeb0` |

## Findings tied to source

- `meson-gxl-s905x-libretech-cc-v2.dts` supplies the V2 identity, memory reference
  and board wiring. Follow its S905X/GXL/GX and Mali includes before extracting
  individual devices. The 2 GB memory declaration is not an allocator grant.
- `libretech-cc_v2_defconfig` selects the V2 device tree and Meson video/simple
  framebuffer support. It does not establish the firmware installed on a unit.
- `meson_plane.c` distinguishes GXL linear scanout from modifier paths for other
  generations. Canvas, VENC, clock and HDMI files are separate port dependencies.
- `meson_venc_hdmi_supported_mode` allows dimensions starting at 400 × 480 in its
  non-CEA path. The selected panel's 400 × 1280 mode still needs clock qualification.
- Lima's kernel sources cover device execution and memory. Mesa's draw/job
  sources supply a separate command producer. Porting either half alone leaves
  required rendering work unimplemented.
- `ge2d.c` matches AXG and lists scaling and source-global-alpha limitations.
  The inspected GXL device-tree chain contains no GE2D node. GXL applicability
  remains a separate investigation.

Read file-level copyright and license terms before translating or incorporating
source. The Linux tree does not have one license for every driver file.

## Physical evidence to add

Record PCB/SKU photographs, power arrangement, serial boot logs, firmware/FIP
and DTB hashes, RAM/reservation maps and CPU entry state. Add panel EDID bytes,
USB descriptors/report samples and the accepted display mode. Capture reference
scenes with image, compiler and driver versions and retain completion/recovery
traces alongside timing measurements.

The [Composer SBC index](../../../../../../Composer/samples/sbc/README.md)
uses the [board entry](../README.md) as its hardware definition. Executable
samples must follow the S905X port's accepted boot and peripheral contracts.
