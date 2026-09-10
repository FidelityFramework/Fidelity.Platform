# Experimental Ariel region candidate

**Status: uncompiled candidate, not supported current-Clef carrier realization.** The
current compiler intentionally rejects this candidate's `NativePtr` operations,
raw pointer integers and width-named source types. No native pool or lifecycle
harness for this legacy candidate has passed. The later
[typed Ariel implementation](../../Ariel/README.md) has its own recorded native
acceptance. `Fidelity.Ariel.Experimental.fidproj` is isolated
from production packages. [STATUS.md](STATUS.md) records the failed build and the
missing legitimate boundary projection.

The source preserves a bounded publication/assignment/completion/release design
for review. It does not instantiate actors, mailboxes, an actor scheduler,
supervision or asynchronous continuations. The API and behavior below describe
the candidate, not an available production API.

```fsharp
match Fidelity.Ariel.Region.start 4 with // caller plus three persistent workers
| Error error -> handleStartupFailure error
| Ok pool ->
    let status = Fidelity.Ariel.Region.run pool count chunkSize
                     (FnPtr.ofFunction boundedMap) environment
    // Successful return retires every participant's use of the environment.
    let shutdown = Fidelity.Ariel.Region.stop pool
```

`boundedMap` has type `nativeint -> int -> int -> int`: environment, inclusive
lower bound, exclusive upper bound, then status (`0` means success). The caller
owns initialized input/environment and exclusive output slices for the complete
call. It validates the actual computation's spatial/input/layout contract before
submission. The Ariel scheduling layer supplies bounded assignment and temporal retirement; it
cannot infer a read footprint from an opaque environment pointer.

The intended entry is a compiler-owned `FnPtr.ofFunction` reference, with no captured
closure environment. Its declared environment pointer is explicit. The pointer
size for callback storage is checked against the compiler's selected typed
pointer stride. Pthread sizes and alignments come from generated target header
metadata; the implementation has no hardcoded byte offsets into POSIX objects.

`start` takes a runtime carrier budget including the caller. `startAllowed` uses
`sched_getaffinity` to count the calling thread's allowed processors, caps the
result by the declared deployment budget and falls back to one carrier if
discovery fails. The package declares
a bounded deployment ceiling and literal shared-memory/POSIX capabilities.
Allowed affinity and deployment restrictions are runtime inputs; the ceiling
does not assert that many processors are available. A single carrier creates no
thread and follows the same assignment/completion/release protocol. Capability
quotations are declaration inputs; this package alone does not claim complete
compiler predicate projection.

All publication, assignment, completion and release counters are protected by
one mutex. Condition waits always recheck their predicates under that mutex.
Each worker tracks its last generation and releases its region references before
it can participate in another generation. The caller helps execute work, then
waits for every participant's release acknowledgment. A zero remaining-work
counter is deliberately insufficient. The final release publishes its counter
and unlocks before the caller can acquire the lock and return.

Only one region may be admitted per pool. Concurrent and nested submissions
return `Busy` (16); there is no implicit queue. Invalid count/chunk/budget returns
`Invalid` (22). A failed callback stops new assignments, joins every participant's
retirement, and returns its first nonzero status. Incomplete output must not be
consumed as a successful result. Subsequent submissions start a fresh generation.

Partial startup failures join every carrier already created before freeing
storage. `stop` rejects an active region; on success it wakes parked workers,
joins them, destroys synchronization objects and frees storage. Pool destruction
requires exclusive ownership of the handle, like destruction of a POSIX mutex;
the handle is invalid after successful stop. An unexpected join failure retains
storage and returns an error rather than freeing potentially live state. Such a
pool is terminal: later run/stop attempts return `Stopped` (125); joins already
completed are never repeated.

OS carrier progress and correct initialized POSIX mutex/condition operation are
the synchronization premises. Native faults, asynchronous thread cancellation,
nonreturning callbacks and corrupt mutexes are not recovered. A synchronization
failure terminates with status 126 instead of authorizing unsafe reuse.

`runWithRetirement` adds a callback once per participant between completion and
release. It supports bounded cleanup and the delayed-retirement test. The hook
must return and follows the same environment ownership rule. `startWithLimit`
provides deterministic partial-startup failure injection for the lifecycle test.

The uncompiled native harness is `tests/Ariel.Experimental.fidproj` in this
directory. Repository `tests/Ariel/lifecycle_model.py` separately enumerates
bounded completion orders. Model success is not native ABI evidence.
Worker retirement releases only this dispatch's use: downstream consumers such
as a compositor have their own buffer-release obligation.
