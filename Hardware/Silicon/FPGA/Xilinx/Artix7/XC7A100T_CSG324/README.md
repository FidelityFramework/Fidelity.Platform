# XC7A100T, CSG324, speed grade -1

[Part.clef](Part.clef) owns part identity and the on-chip BRAM/register inventory
extracted from the Arty A7-100T description. No board clock, external flash,
connector, UART policy or active platform description is declared here.

The [silicon datasheet](docs/ds181_Artix_7_Data_Sheet.pdf) moved unchanged from
the product source pack. Existing resource counts remain traced to the
[Digilent reference manual](<../../../../../Products/Digilent/ArtyA7_100T/docs/Arty A7 Reference Manual - Digilent Reference.html>)
feature table for this part. BRAM capacity includes parity bits; usable capacity
depends on the instantiated port width, as recorded in the declaration.

The [Arty product](../../../../../Products/Digilent/ArtyA7_100T/README.md) references
these declarations and supplies assembly-specific facts. The package suffix
and speed grade describe the currently selected part, without claiming that
other Artix-7 variants have the same resources.
