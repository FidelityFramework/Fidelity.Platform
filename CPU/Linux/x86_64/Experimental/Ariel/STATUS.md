# Build status and boundary handoff

The bounded lifecycle model passes **1,747 distinct states** across counts 0–4,
one through three participants and chunk sizes one through three. It checks exact
assignment and that complete output does not imply released captures:

```sh
python tests/Ariel/lifecycle_model.py
```

The current native candidate **does not compile**. The last actual-scope build
used Composer's Debug DLL referencing `clef-scope-integration`, with verbose
intermediates. It reached saturation and returned **274 errors, 1,023 information
diagnostics**, before native emission. The files have since been moved here from
their original platform/test paths; no native result is inferred from that move.
The new location can be checked with `tests/run.sh` when the boundary is ready.

The decisive failures are intentional source-language restrictions:

- `CCS8706` rejects `nativeint`, `unativeint`, fixed-width numeric spellings and
  related annotations in the candidate and its legacy dependencies.
- `CCS8009` rejects the removed `NativePtr` operations.
- `CCS8018` rejects literal suffixes that select representation.

The governing sources are `clef-lang-spec/spec/ffi-boundary.md` §1 and
`platform-bindings.md` §1: no raw pointer is denotable, including at the platform
boundary. `clef-scope-integration/docs/fidelity/phg/Dimensional_Range_Design.md`
documents the deliberate NativePtr removal in its CS-11 account (around lines
699–732) and the CHandle/FunctionDescriptor generator direction in the CS-12
Farscape account (around lines 1428–1443). Restoring NativePtr, admitting raw
pointer integers or disabling width diagnostics would contradict that direction.

The required continuation is a narrow, compiler-owned storage/capture membrane:

1. Supply POSIX object storage through opaque `CHandle` values that are only
   passed back into declared external functions. Sizes and alignment come from
   generated target ABI metadata, with ownership attached to the pool lifetime.
2. Project a declared bounded array into `pthread_create`'s output argument and
   read its resulting thread handle through normal typed storage. The current
   opaque CHandle result cannot be dereferenced to manufacture this bridge.
3. Publish a normal typed record/closure environment across persistent carriers,
   using the compiler's actual capture layout and explicit source references for
   worker reachability. An explicit typed callback boundary must agree with that
   environment. A typed process-wide pool is one possible bounded realization;
   pointer-to-record casts are not.
4. Carry the output/input byte extent and finite representation obligations
   through the shared BAREWire dispatch gates before publication. The native
   record/array representation must preserve complete input extent evidence.
5. Run the preserved native lifecycle scenarios against that realization:
   single carrier, repeated generations, zero/tail work, competing/nested
   submission, failed callback and reuse, delayed retirement after completed
   output, partial creation and shutdown joins.

The candidate's lock protocol remains useful review material: all participants
are counted before publication, every wait rechecks a predicate, each carrier
tracks a generation, completed participants execute retirement bookkeeping before
acknowledging release, and the caller waits for release rather than merely zero
unfinished work. Partial startup joins the exact successful creations. A failed
shutdown is terminal and cannot retry already-completed joins.

None of the native lifecycle scenarios above has passed. Separate generated
pthread ABI probes exercise the host library via ctypes; they do not execute this
candidate and do not establish Ariel scheduling correctness. The independently
implemented compiler FnPtr lowering has its own native probe evidence, also
distinct from a native Ariel pool.
