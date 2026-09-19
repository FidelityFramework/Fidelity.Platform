# Products

Products include development boards, accelerator cards and complete systems.
Each product owns assembly details and source-backed hardware applicability,
and references its silicon packages. The product manufacturer can differ from
the chip vendor: Arty is a Digilent product using Xilinx silicon; Meadow is a
Wilderness Labs product using ST silicon.

Use revision subdirectories when the hardware revision is established. Keep
schematic issue, PCB design revision and physically observed revision distinct;
the current EK-RA6M5 and Arty source packs document their applicability explicitly.
Application clocks, buffer budgets and access grants belong to selected profiles
or applications, rather than becoming universal product properties.

[Libre Computer AML-S905X-CC-V2](LibreComputer/AML_S905X_CC_V2/README.md), known as
Sweet Potato, has a board/source inventory and a proposed KeyStation graphics
port. Its documentation separates the S905X GPU, display and video decoder.
Native boot and drivers remain implementation work.

[Waveshare 7.9inch HDMI LCD](Waveshare/7_9inch_HDMI_LCD/README.md), SKU 17916,
is KeyStation's selected HDMI/USB panel. Its product entry records the ASIN,
UPCs and physical-to-logical orientation.
