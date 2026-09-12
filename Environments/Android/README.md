# Android platform support

This folder is a placeholder for future Android support. It currently contains
only this README; there are no Android platform declarations or bindings here.

This is where Fidelity.Platform would describe how a compiled Clef application
works with Android:

- How Android starts the application and delivers application lifecycle events.
- How compiled code calls Android system functions and passes values to them.
- Which Android facilities the application can use, with Clef declarations for
  calling those facilities.

Some calling and data-layout rules depend on the CPU architecture. Those details
would be organized beneath this folder, while shared CPU facts would remain in
`Hardware/Silicon/`. A particular phone or tablet would belong in
`Hardware/Products/`. A profile would select the hardware and Android support
needed by an application; Composer would own building and packaging it.

Creating this folder establishes where that work belongs. It does not implement
an Android application target. See the [repository structure](../../PLATFORM_STRUCTURE.md).
