# Renesas R7FA6M5BH3CFC

This concrete RA6M5 part/package package owns the 2 MiB code-flash and 512 KiB
SRAM declarations, the 112-entry vector layout, LQFP176 pin alternatives and the
initial 27 register transaction requirements. It does not apply this memory or
pin configuration to every RA6M5 variant. Add family-common definitions when
another part establishes their applicability; preserve each resource instance.

[Description.clef](Description.clef) owns the MemorySpace records used by
[Registers.clef](Registers.clef). Selected profiles reference those same records,
which preserves the semantic identity required by MMIO grants. No workload grant,
clock selection, stack budget or complete PlatformDescription is installed here.
The secure PPB register view is still declared in this part's reviewed map.

[PackagePins.clef](PackagePins.clef) records all 176 pins from DS-1.50 Table 1.16.
Functions are alternatives, not simultaneous assignments or PSEL encodings.
The [EK-RA6M5 source manifest](../../../../../Products/Renesas/EK_RA6M5/docs/SOURCE_MANIFEST.md)
pins the manuals and scope; the product package owns board wiring and vendor
reference assets. A full peripheral driver inventory is not yet implemented.
