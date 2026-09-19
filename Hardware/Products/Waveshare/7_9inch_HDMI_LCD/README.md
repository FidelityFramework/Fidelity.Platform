# Waveshare 7.9inch HDMI LCD

Selected KeyStation display, confirmed by the project owner on 2026-09-18.

| Identity | Value | Source |
| --- | --- | --- |
| Manufacturer | Waveshare | Manufacturer product page |
| Part number | `7.9inch HDMI LCD` | Manufacturer product page |
| Manufacturer SKU | `17916` | Manufacturer product page |
| Amazon ASIN | `B087CNJYB4` | Owner-supplied selection and Amazon listing |
| UPCs | `738628504692`, `614961956704` | Owner-supplied identifiers, confirmed in Amazon listing |

The [Waveshare product page](https://www.waveshare.com/7.9inch-HDMI-LCD.htm)
identifies SKU 17916 as a 7.9-inch IPS panel with a **400 × 1280** physical
matrix, **HDMI display input** and **USB capacitive touch**, supporting up to
five contacts. Its default orientation is portrait. The selected
[Amazon listing](https://www.amazon.com/dp/B087CNJYB4) records the retail identity.
Use the [manufacturer wiki](https://www.waveshare.com/wiki/7.9inch_HDMI_LCD)
for setup and mechanical resources.

## KeyStation integration

The host is [Libre Computer AML-S905X-CC-V2](../../LibreComputer/AML_S905X_CC_V2/README.md).
KeyStation mounts the panel horizontally in a 19-inch rack face and uses a
**1280 × 400 logical layout**. The native plan rotates rendered content into the
400 × 1280 scanout surface and applies the corresponding inverse transform to
touch coordinates. HDMI and USB are separate display and input connections.

Product identity is fixed. Driver acceptance still requires the unit's EDID,
accepted Meson timing, USB descriptors and contact reports. Record those with
firmware/image versions when testing the connected hardware. The
[port plan](../../../../docs/SWEET_POTATO_UI_PORT.md#panel-mode-qualification)
defines that work. Manufacturer support for another host does not establish
the Clef driver implementation.
