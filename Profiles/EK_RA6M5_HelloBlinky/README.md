# EK-RA6M5 HelloBlinky profile

This is the selected, single-target platform package for
[HelloBlinky](../../../MCU/Renesas/EK-RA6M5/HelloBlinky). Its manifest explicitly
selects `Fidelity.Platform.Profiles.EK_RA6M5_HelloBlinky.Description.descriptor`.

The profile depends on the [EK-RA6M5 product](../../Hardware/Products/Renesas/EK_RA6M5)
and the [freestanding Cortex-M33 environment](../../Environments/Freestanding/arm_cortex_m33).
It supplies one PlatformDescription and one CortexMImageDescriptor, including the
image's 8 KiB stack reservation and preserved-option-byte extent. The original
platform ID is retained to keep the established image-evidence identity stable.

Physical MemorySpace records, vector layout and device identity remain owned by
the concrete silicon package. The profile references those original declarations;
it does not reconstruct equal-looking regions. The environment owns the TargetCore
projection. The product owns package selection, control wiring and netlist facts.

HelloBlinky owns its clock/PWM selections in `src/Configuration.clef`, register
mappings and grants in `src/Access.clef`, and startup/interrupt policy in its source
and project settings. Those application sources are not profile dependencies.
A standalone profile import therefore supplies inventory/image requirements
without introducing an active DeviceAccessPlan.

This extraction does not add scheduling, protection or device drivers. See
[physical acceptance](../../../MCU/Renesas/EK-RA6M5/HelloBlinky/docs/ACCEPTANCE.md) for
the accepted firmware baseline; compiler regression and renewed board acceptance
remain distinct evidence.
