# Linux x86-64 WrenHello profile

The [manifest](Fidelity.Platform.fidproj) selects `Fidelity.Platform.Profiles.Linux_x86_64_WrenHello.Description.description`: the [default Linux selection](../Linux_x86_64_Default/README.md) with one budget replaced. The [description](Description.clef) reuses the default profile's `text`, `data`, `bss`, `stack`, `arena` and `heap` declarations and the environment's core, syscalls, transports and lifecycle, and declares a 32-page `rodata` budget for the welded UI bundle that WrenHello embeds as a string literal (about 49 KiB at WrenHello 0.3.0). `Console.write`'s source buffer is bounded by the same budget.

The budget is an application policy, not a measured Linux capacity. When the embedded bundle outgrows it, the compiler reports the overflow against this declaration and the capacity is revised here.
