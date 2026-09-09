# Bounded synchronous regions

The Ariel scheduling layer's `Fidelity.Ariel.fidproj` selects one process-wide pool of persistent pthread
carriers. The requested count includes the calling thread. `start 1` therefore
supports the full protocol without creating an OS thread. `startAllowed ()`
queries the calling process's allowed CPU mask through opaque libc handles and
caps the count at the selected platform's declared storage budget. Query failure
selects the caller alone.

The package explicitly selects the hosted Linux x86_64 pthread realization.
Literal capability quotations describe that selection; automatic selection
across target profiles is separate from this explicit package and the runtime
affinity query.

`start count`, `run count chunk work`, and `stop ()` return zero on success.
The work function receives a half-open interval and returns zero or a failure
status. `Busy` rejects nested or overlapping submissions and shutdown during an
active region; `Invalid` rejects invalid counts and chunk sizes. Only one owner
may start or stop the pool, and no caller may race teardown against a new use.

Publication, range claims, predicate waits and retirement acknowledgments use
one mutex. Every carrier observes each generation and acknowledges release after
its final callback access. `run` waits for all releases before clearing its work
reference or returning. A callback failure stops further claims and still waits
for every carrier. `runWithRetirement` exposes a final participant callback for
the native delayed-release test.

Startup rolls back only successfully initialized objects and joins every worker
actually created. `startWithLimit` adds deterministic partial-create failure
injection for native tests. Successful shutdown joins before destroying opaque
objects. A failed shutdown is terminal: the implementation retains potentially
live state and rejects another stop rather than rejoining stale thread handles.

The pool's worker and synchronization storage is bounded by platform declarations
and generated ABI layouts. `ScopedCallbackDescriptor` declarations for `run` and
both callbacks of `runWithRetirement` expose the synchronous lifetime obligation
to the compiler. The implementation satisfies it by waiting for every release
and clearing both published callbacks before returning. The declaration is a
library contract, not a general proof of arbitrary scheduler implementations.

The compiler can place a callback environment on the stack when every use is an
invocation or a call to a proved synchronous consumer. This permits a callback
to capture a BAREWire `BorrowedView<Schema>` within a generated mapping scope;
`run` completes before the mapping adapter releases the storage. The view is not
stored in a persistent application session. Stores, returned closures, partial
applications carrying the view, and consumers with unknown lifetimes are rejected.
The fresh native mapped-carrier gate verifies that realization with 64 real GBM
scopes, exact physical pixel readback and matching worker joins; both scoped
callback bodies are free of heap allocation. See the native STATUS document for
the reproducible driver and limits. Protocol evidence alone does not establish
allocation behavior.

Run `python tests/Ariel/native/run.py` from the repository root for fresh native
gates. The protocol model is separate evidence and does not replace those gates.
