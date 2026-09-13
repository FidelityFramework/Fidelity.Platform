# Freestanding STM32H74x/75x Cortex-M7 environment

[Description.clef](Description.clef) selects `thumbv7em-none-eabihf`,
`cortex-m7`,32-bit pointers and the numeric representations of the explicitly
STM32H74x/75x-specialized core package. Other M7 implementations may require
different FPU/MPU capabilities.

Startup must establish the execution address, vectors, stack/data, actual clocks,
FPU permission if used, and core ownership. This ABI descriptor performs none of
those actions. It supplies no operating system or C HAL.

CCS ProjectChecker accepts the original source closure with zero admitted
errors/warnings; existing dependency diagnostics remain classified Unreachable.
Backend/image and hardware acceptance are separate.

