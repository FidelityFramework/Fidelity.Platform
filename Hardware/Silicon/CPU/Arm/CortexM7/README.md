# Arm Cortex-M7 facts — STM32H74x/75x specialization

[Description.clef](Description.clef) supplies numeric representations, register
width, architecture identity, the Arm System Control Space as an original
MemorySpace, and the core facts needed by an image. The numeric/FPU/MPU
configuration is explicitly scoped to **STM32H74x/75x Cortex-M7**, as specified by
PM0253 Rev 6 Table 14. Other M7 and F7 implementations can differ.

IEEE32/64 use exact signed finite endpoints and `Boundary.Exact`. This tag does
not promise exact real arithmetic or describe NaNs, infinities, subnormal
handling, rounding, or FPSCR state. Floating-point startup and numerical error
bounds remain separate obligations.

[Registers.clef](Registers.clef) retains the authored 117-register SCS inventory.
Its DeviceRegion refers directly to `Description.ppb`. The NVIC extent is the
H747 configuration (150 external slots), not an architectural maximum. Individual
grants, mappings and startup are workload-owned. A profile can select only the
three SysTick registers without accepting or using the rest of the inventory.

[Source provenance](docs/SOURCE_MANIFEST.md) records the Arm ARM and CMSIS
cross-checks. PM0253 Rev 6 is now staged; DDI0489 remains absent. The existing
implementation-register declarations still require detailed field/side-effect
reconciliation before use.

The [freestanding environment](../../../../../Environments/Freestanding/arm_cortex_m7)
selects 32-bit pointers and the hard-float Thumb ABI. This supplies no startup,
FPU enablement, linker placement or runtime implementation.

Original-source CCS ProjectChecker accepts the product and environment closures
with zero admitted errors and warnings. Each check still reports 93 raw
diagnostics classified Unreachable in existing BAREWire/Contracts sources; none
names the M7 package. No image was generated or tested on hardware by this change.
