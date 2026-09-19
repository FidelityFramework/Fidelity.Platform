# Libre Computer AML-S905X-CC-V2: Sweet Potato

KeyStation's selected board is **Libre Computer AML-S905X-CC-V2 (Sweet Potato)**.
It uses the **Amlogic S905X (Meson GXL)**, four **Arm Cortex-A53** CPU cores and
a **Mali-450** GPU. The V2 board uses **DDR4**. These identities are established
by the manufacturer and the board-specific upstream sources below.

| Identifier or resource | Confirmed product definition |
| --- | --- |
| Manufacturer / model | Libre Computer / `AML-S905X-CC-V2` |
| SoC / family | Amlogic `S905X` / Meson `GXL` |
| CPU | Four Arm Cortex-A53 cores, AArch64 |
| GPU / Linux driver family | Arm Mali-450 (Utgard) / Lima |
| Display | Meson display pipeline, HDMI 2.0 connector |
| Memory | DDR4, standard 2 GB retail configuration. Manufacturer also lists an OEM-only 1 GB model |
| Boot/storage | MicroSD, optional eMMC slim module, 16 MB SPI boot storage |
| USB | Four USB 2.0 Type-A host ports and an internal USB header |
| Supply | USB-C, 5 V / 3 A |
| Linux board compatible | `libretech,aml-s905x-cc-v2` |
| Linux device tree | `amlogic/meson-gxl-s905x-libretech-cc-v2.dtb` |
| U-Boot configuration | `libretech-cc_v2_defconfig` |

The [manufacturer's V2 announcement](https://hub.libre.computer/t/2023-09-01-libre-computer-aml-s905x-cc-v2-sweet-potato-now-available/2831)
distinguishes the DDR4 and SPI-equipped board from V1.
[U-Boot's board guide](https://docs.u-boot.org/en/latest/board/amlogic/libretech-cc.html)
identifies the processor and V2 build selection. The pinned
[source manifest](docs/SOURCE_MANIFEST.md) records the inspected Linux device tree,
driver sources and U-Boot configuration. Implementation status, 2026-09-18:
this entry exports no platform descriptor. Native boot, display, USB and GPU
drivers are the work defined by the port plan.

## KeyStation selection

KeyStation uses the [Waveshare 7.9inch HDMI LCD](../../Waveshare/7_9inch_HDMI_LCD/README.md),
manufacturer SKU **17916**, Amazon ASIN **B087CNJYB4**, selected by the project
owner. Its physical matrix is **400 × 1280**, with **HDMI video** and **USB
capacitive touch**. Horizontal mounting in a 19-inch rack face gives a
**1280 × 400 logical layout**. The panel entry records both supplied UPCs.
Driver acceptance must establish the panel timing and matching render/input
transforms on S905X.

The [KeyStation reference profile](../../../../Profiles/SweetPotato_KeyStation_Reference/README.md)
describes the intended environment choices and resource policy. The
[native port plan](../../../../docs/SWEET_POTATO_UI_PORT.md) covers boot, display,
input and Mali. The
[Fidelity.UI design](../../../../../Fidelity.UI/docs/09_sweet_potato_keystation.md)
covers reactive areas, drawing and transitions. Key-authority behavior, Renesas
HUK integration and storage cryptography are outside this graphics scaffold.

## Source ownership for the implementation

A future S905X silicon package should own its address map, interrupt routing,
clock/reset relationships and shared memory topology. It should reference the
CPU and GPU architecture facts without counting shared DRAM again for each
device. This product entry owns V2 wiring, fitted memory and connectors.

A freestanding AArch64 environment must describe the accepted boot protocol,
CPU/runtime ABI, exceptions, timers and device access. A Linux AArch64 environment
would instead supply the hosted ABI and graphics/input bindings. The reference
profile selects one environment and the application's budgets. These packages
should be introduced as their source and consumers are implemented.

## Bring-up evidence

The [Composer SBC index](../../../../../Composer/samples/sbc/README.md) refers
to this board definition. The first executable sample must establish the S905X
firmware handoff, serial output and interrupt/timer operation before using GPIO
or graphics. Record the installed firmware, RAM reservations and physical PCB
revision with that result. Those observations qualify a running image against
this product definition.
