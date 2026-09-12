# iOS platform support

This folder is a placeholder for future iOS support. It currently contains
only this README; there are no iOS platform declarations or bindings here.

This is where Fidelity.Platform would describe how a compiled Clef application
works with iOS:

- How iOS starts the application and delivers application lifecycle events.
- How compiled code calls iOS system functions and passes values to them.
- Which iOS facilities the application can use, with Clef declarations for
  calling those facilities.

Some calling and data-layout rules depend on the CPU architecture. Those details
would be organized beneath this folder, while shared CPU facts would remain in
`Hardware/Silicon/`. A particular iPhone would belong in `Hardware/Products/`.
A profile would select the hardware and iOS support needed by an application;
Composer would own building and packaging it.

Creating this folder establishes where that work belongs. It does not implement
an iOS application target. See the [repository structure](../../PLATFORM_STRUCTURE.md).
