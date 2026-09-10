# Linux x86-64 environment

[Environment.clef](Environment.clef) owns the Linux ABI, libc process lifecycle,
declared service surfaces and software numeric choices. It references shared
[x86-64 architecture facts](../../../Hardware/Silicon/CPU/x86_64/README.md).
Pointer width remains an ABI declaration here; it does not determine an MMIO
transaction width.

The [environment metadata package](Fidelity.Platform.Environment.fidproj)
owns Console and Environment sources once. Console's line capacity remains an
explicit library API constraint. The [binding package](Fidelity.Platform.fidproj)
adds runtime helpers and APIs without selecting an application memory budget.

Applications select the [default Linux profile](../../../Profiles/Linux_x86_64_Default/README.md)
or another implemented profile. The
[compiler surface](Fidelity.Platform.CompilerSurface.fidproj) is a lightweight
selector of the same default profile description, with its metadata dependencies
and Console but without the rest of the binding tree.

Linux bindings, display integration and Ariel remain in this environment.
[ROCm](ROCm/README.md) and [XRT](XRT/README.md) are CPU-hosted library integrations;
their presence does not create a GPU/NPU compilation target. The separate
`Experimental` packages retain their recorded status.
