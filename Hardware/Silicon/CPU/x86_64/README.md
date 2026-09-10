# x86-64 architecture facts

[Architecture.clef](Architecture.clef) owns the shared 64-bit register width,
integer representations and baseline SSE2 floating-point representations.
This is an architecture package shared by multiple processor vendors; it is
not a description of one chip, an OS or an executable machine.

The selected environment owns pointer representation, ABI, runtime and software
numeric support. This package declares no `TargetCore`, memory budget or active
`PlatformDescription`. Its [manifest](Fidelity.Platform.fidproj) is a dependency
of the Linux environment and may be reused by other explicit compositions.
