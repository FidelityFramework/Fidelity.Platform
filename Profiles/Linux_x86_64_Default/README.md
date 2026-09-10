# Default Linux x86-64 profile

The [manifest](Fidelity.Platform.fidproj) selects
`Fidelity.Platform.Linux_x86_64.Description.description`. This profile composes
the [Linux environment](../../Environments/Linux/x86_64/README.md) with explicit
application compatibility budgets; it does not require a specific PC or board.

[CompatibilityBudgets.clef](CompatibilityBudgets.clef) preserves the former
platform's HelloProof-derived section budgets, 4096-byte scope arena, stack-limit
assumption and virtual-address-space ceiling. They are not measured resources or
universal Linux guarantees. A workload requiring another layout must select
and justify its own description.

[Description.clef](Description.clef) retains the original platform ID, memory
and buffer values. Console's read capacity is referenced from its environment
library API; the selected write buffer remains bounded by this profile's
constant-section budget.

The [description package](Fidelity.Platform.Description.fidproj) is the only
source owner for the budgets and assembled description. Both this full profile
and the environment's lightweight CompilerSurface selector depend on it, so
dependency diamonds preserve the same semantic declaration identities.

## Migration acceptance — 2026-09-10

The existing Composer `HelloDimensionsProof` sample compiled through this full
profile to an x86-64 Linux ELF executable. Entering `Fidelity` produced
`Hello, Fidelity!` and exit status 0. This exercises shared native floating-point
facts, measured arithmetic, Console input/output and the selected profile's
buffer declarations. It is an application check, not acceptance of every
hosted library or experimental binding.
