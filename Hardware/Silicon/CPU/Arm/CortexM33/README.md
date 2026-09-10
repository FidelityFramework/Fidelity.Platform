# Arm Cortex-M33 facts

[Description.clef](Description.clef) supplies the existing M33 integer
representations, register width, architecture identity and little-endian selection.
It supplies no memory map, pointer ABI, image layout or workload grant.

The [freestanding environment](../../../../../Environments/Freestanding/arm_cortex_m33)
selects the 32-bit pointer ABI and Thumb soft-float toolchain contract using these
facts. The representation catalogue remains the compiler's current integer
capability model; it is not a claim of complete Arm instruction or FPU support.
