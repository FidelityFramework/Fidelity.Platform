# Freestanding Cortex-M33 environment

This package projects the supported `thumbv8m.main-none-eabi` / Cortex-M33
soft-float execution contract into BAREWire's TargetCore. The 32-bit pointer
representation is an ABI selection. Integer/register capabilities are direct
references to [shared silicon facts](../../../Hardware/Silicon/CPU/Arm/CortexM33).

Startup, physical memory, vector placement, stack size and active MMIO grants
are supplied by the selected product/image/application composition. This package
has no complete PlatformDescription and assumes no RTOS. The working example is
[EK_RA6M5_HelloBlinky](../../../Profiles/EK_RA6M5_HelloBlinky).
