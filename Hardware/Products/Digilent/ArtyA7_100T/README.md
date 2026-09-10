# Digilent Arty A7-100T product

This product package retains the operative pin bindings and application Prelude
for the Arty A7-100T. Existing public module names are retained so application
pin attributes and source imports remain stable across the physical move.

The documented applicability remains **Rev D/E**. No physical board revision
has been newly observed during this migration, and no revision folder is
invented. Board revisions can populate different configuration-flash parts;
the existing declaration records that distinction.

## Source ownership

- [Bindings](ArtyA7_100T.Bindings.clef): clocks, pin identity, wiring, endpoint
  groups, the existing UART default and the operative Contracts descriptor.
- [Prelude](ArtyA7_100T.Prelude.clef): application-facing color, signal and design
  types. The unresolved `Package.xdcConstraints` export was removed; Composer
  produces XDC from CCS pin evidence.
- [Inventory](Inventory.clef): product flash, described surface and lifecycle.
- [Xilinx part](../../../Silicon/FPGA/Xilinx/Artix7/XC7A100T_CSG324/README.md):
  part/package/speed-grade identity and on-chip BRAM/register resources.
- [HelloArty profile](../../../../Profiles/ArtyA7_HelloArty/README.md): selected
  report schema, buffer ceiling, UART transport and assembled BAREWire description.

The [product manifest](Fidelity.Platform.fidproj) selects its Contracts descriptor.
HelloArty selects the profile, which carries the BAREWire description and depends
on this product. The BAREWire surface currently has three endpoints; the full
pin map remains authoritative in the Contracts bindings.

## Source documents and coverage

The preserved source pack contains the Digilent reference manual, schematic,
master XDC files and their saved assets. The
[silicon datasheet](../../../Silicon/FPGA/Xilinx/Artix7/XC7A100T_CSG324/docs/ds181_Artix_7_Data_Sheet.pdf)
moved to its part owner unchanged.

Mapped endpoints cover the 100 MHz clock at E3, discrete green LEDs, RGB
channels, buttons, switches, USB-UART and ChipKit digital GPIO. Reset is the
existing internal power-on reset; the board's PROGRAM_B control is not a user
reset GPIO. Pmod, analog variants, Ethernet and other peripheral drivers remain
outside the current binding coverage. This taxonomy migration adds no drivers.
