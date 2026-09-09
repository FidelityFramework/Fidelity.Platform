# Native carrier gate status

Evidence recorded 2026-09-09 using the current `clef-scope-integration` compiler
and Composer Debug build 20. All six gates passed in one invocation of
`python tests/Ariel/native/run.py`. These are fresh LLVM/LLD executables executing
current typed Clef sources and generated pthread/libc declarations; there is no
C carrier implementation or host-language substitute for the Clef source.

| Gate | Observed result |
| --- | --- |
| Allocation | PASS: optional opaque handle allocated, matched and freed |
| CreateJoin | PASS: native pthread callback updates typed shared state; caller joins |
| EmptyOutput | PASS: exact admission message and SIGABRT before foreign output write |
| Affinity | PASS: inherited mask and masks restricted to three and one CPU |
| Startup | PASS: partial creation rollback followed by successful four-carrier startup/shutdown |
| Lifecycle | PASS: persistent carriers 1/2/4, 100 generations each, exact assignment, failure/reuse and delayed release |

The artifact directory is `/tmp/ariel-native-xxjn13nl`. Each gate retains its
`probe`, `compile.log` and compiler intermediates. The lifecycle executable is
`/tmp/ariel-native-xxjn13nl/Lifecycle/probe`. Fresh execution is reproducible with
the driver; temporary artifacts are local evidence and may eventually expire.

The successful build's SHA-256 fingerprints are:

- Composer.dll: `da601de810c26645bb135a2a69131bc7d04648390e271800f43792c591022073`
- Clef.Compiler.Service.dll: `d14e4d82b460f44eb8b9c7ae7b456af5856e52ccce693bc7238941a6664c57e6`

Lifecycle also checks invalid arguments, nested submission/shutdown rejection,
partial creation failure after zero/one/two workers, and stopped-pool rejection.
Noncaller retirement callbacks delay before their final capture access. Both a
zero-work region and a callback-failure region must observe every retirement
before returning; finishing all work alone cannot satisfy these tests.

The native gates exposed compiler defects in function-value alias calls,
callback parameter ranges, array index signedness, zero initialization and
callback-result representation meets. They were repaired in compiler/lowering
code and the full suite was rerun. Companion Composer tests pass seven index
signedness cases and a fresh native array test with ordinary and deliberately
poisoned allocator storage (`tests/MemoryArrays/run.py`).

The bounded transition model passes independently: 1,747 assignment/completion/
release states and repeated generation-token reuse checks. This is separate
evidence from the native lifecycle and the HelloWayland native renderer gate.

Coverage does not include a separately created competing submitter (nested
submission is covered), allocation/initialization failure injection, or injected
join failure. Startup and teardown require exclusive lifetime ownership as
documented in the scheduling layer. The package explicitly selects Linux x86_64;
automatic cross-target selection from capability predicates is separate work.

Compiler-owned callback environment lifetime placement remains a separate
allocation obligation. The emitted scheduling functions `participate` and
`runWithRetirement` contain no heap allocation. Repeated application closure
construction requires its own scope/region lifetime evidence.

The later scoped mapping gate also passed with Composer build 41. Run
`python3 tests/ariel-window/run_mapped_carriers.py` from HelloWayland to reproduce
64 real GBM scopes through one persistent four-carrier pool. It verifies native
U32 pixels before every unmap, requires noncaller completion in every frame,
and observes exactly three successful carrier creates and matching joins.
Both scoped callback bodies contain no heap allocation. Evidence is retained
at `/tmp/hello-wayland-mapped-carriers-m082sa87`. This exercises the new
`ScopedCallbackDescriptor` contracts and `BorrowedView<Schema>` capture path;
it does not replace the actual animated window acceptance.
