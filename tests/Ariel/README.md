# Ariel verification

Run `python tests/Ariel/lifecycle_model.py` from the repository root. The model
enumerates 1,747 states, checking exact assignment and separate completion and
capture release. It makes no claim about POSIX scheduling or a native ABI.

The current typed implementation is `Environments/Linux/x86_64/Ariel/Region.clef`, selected
by `Fidelity.Ariel.fidproj`. It uses ordinary typed state, arrays and functions;
opaque `CHandle` values cross only the generated pthread/libc boundaries.

Run the native gates with `python tests/Ariel/native/run.py`. Use
`--composer /absolute/path/to/Composer.dll` to select another current compiler,
or `--stages Allocation CreateJoin` for a focused gate. Every invocation creates
fresh temporary executable paths, compiles actual Clef through LLVM/LLD, executes
with timeouts and retains compiler logs/intermediates. A failed compile cannot
run an older binary.

The stages cover opaque allocation, actual pthread callback creation/joining,
rejection before an undersized output-array write, partial startup cleanup,
allowed affinity under restricted masks, and persistent pool lifecycle. Lifecycle
covers carrier counts 1/2/4, repeated generations, exact assignment, tails, invalid
arguments, nested rejection, callback failure/reuse, and delayed retirement after
zero work and callback failure. A completed work counter alone cannot pass the
retirement check. See [native/STATUS.md](native/STATUS.md) for observed results and
the remaining coverage limits.

The historical raw-pointer candidate remains isolated under
`Environments/Linux/x86_64/Experimental/Ariel/`; it is not selected by these gates or the
typed package. Its source is not a sanctioned current-Clef boundary.
