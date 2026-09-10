# Platform facts and compiler-derived state

Status: current implementation note with an open design boundary, audited
2026-09-10. The former D4b page compared a context beside the PSG with a new
platform head node. The useful distinction is retained here; the proposed
head-node redesign is not a prerequisite for current platform work.

## What exists

Platform facts are not located exclusively beside the graph or exclusively in
one special node. They have an authored representation and derived projections:

| Representation | Purpose | Source |
| --- | --- | --- |
| Typed declaration nodes, including quoted records | Authored facts with type and source location | [PlatformResolution](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformResolution.fs) |
| `SemanticGraph.Platform` | Context used by analyses; widths, representations and return bounds are filled from declarations | [PlatformDeclaration.fill](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformDeclaration.fs) |
| `Layouts`, `FieldRanges`, `ElementRanges`, `Codata` | Settled program facts and backend inputs | [SemanticGraph](../../clef/src/Compiler/PSGSaturation/SemanticGraph/Types.fs) |
| `SemanticGraph.Edges` | Explicit enrichment edges, including obligations and residence relationships | [Hyperedge and EdgeRole](../../clef/src/Compiler/PSGSaturation/SemanticGraph/Types.fs) |

The graph has a list of declaration roots. The ordinary
[traversal folds](../../clef/src/Compiler/PSGSaturation/SemanticGraph/Traversal.fs)
start there; the context field is not an executable root. Structural/reference
edges can also be projected from node kinds. An edge representation exists,
but that is not evidence of a completed combined cache/concurrency solver.

## Engineering distinction worth preserving

**Target declarations** state available representations, regions, endpoints and
other platform facts. **Program-derived facts** state the selected layout,
placement, ownership or obligations for a particular compilation. A future
runtime binding would introduce a third category: resources actually discovered
or granted to this execution.

Keep projections traceable to their declarations. Keep derived facts attached
to the program objects and relationships they constrain. Introducing a global
head node would not itself establish either property, prevent stale analysis
results, or prove the correctness of a placement decision.

The present code supports continuing with declaration nodes, a derived context,
and program-specific facts on the graph. That is the working baseline. Any
change to it should identify a concrete consumer the current representation
cannot serve, the required dependency/invalidation behavior, and a test that
distinguishes the alternatives. A new head node is not recommended solely to
make platform facts visible during traversal.

## Deferred narrowing

Deferring a choice constrains when information is committed, not whether it is
stored in a context, node, or edge. Preserve unresolved requirements and their
provenance until the binding or analysis has enough information to select a
representation. Then record the choice and its justification where its consumers
can read it without independently recomputing it.

Core width, value range, register access width, memory-space identity and workload
permission are separate constraints. The existing width projection implements
part of this discipline; it does not implement general resource grants or
runtime mapping. [The integration reference](CANONICAL_PLATFORM_SPEC.md) describes
that boundary.

## Concurrency and memory assumptions

A single core does not remove interrupts, device progress, DMA, or task-level
wait cycles. Cooperative scheduling restricts task switching; it does not prove
that publication, acknowledgement, rearming and sleeping cannot lose work.
Neither a scalar core count nor the placement of a platform node discharges
those obligations.

The next device-memory design must name the participants, address spaces,
ownership transfers, ordering operations and progress assumptions it reasons
about. Memory-tier and topology facts need concrete meanings and evidence, not
an inferred "single-core means trivial" shortcut. These are requirements for
future work, not claims that HelloBlinky or the hosted Ariel tests prove them.

## Predicates and the F★ connection

This is an open design question, not an implemented general predicate API.
[The Gift of Deferred Inference](../../clef-lang-site/hugo/content/blog/deferred-inference.md),
especially "Immutable Evidence" and "Pending Obligations", argues for keeping
relationships, their premises and their dependencies until a concrete decision
needs them. A predicate can express those relationships while representation
and placement remain undecided.

F★ refinement types attach propositions to values; using a value at a refined
type requires establishing the proposition. Its effect specifications also use
preconditions and postconditions to derive verification conditions. The relevant
lesson for Clef is preserving the proposition, its premises and its validity
scope through the existing inference and obligation machinery. See the official tutorials
on [refinements](https://fstar-lang.org/tutorial/book/part1/part1_getting_off_the_ground.html#boolean-refinement-types)
and [effect refinements](https://fstar-lang.org/tutorial/book/part4/part4_pure.html).

There are three separate mechanisms in the inspected Clef/BAREWire code:

- `PlatformPredicate` and `PlatformContext.Predicates` exist in
  [NativeTypes](../../clef/src/Compiler/NativeTypedTree/NativeTypes.fs).
  [ProjectChecker](../../clef/src/Compiler/Project/ProjectChecker.fs) initializes
  the map empty. No production resolver or reader of that map was found in
  the inspected clef/Composer sources. Comments claiming complete saturation
  resolution are ahead of this implementation.
- [Ariel capabilities](../CPU/Linux/x86_64/Ariel/Capabilities.clef) contain
  literal quoted booleans, but the manifest explicitly chooses the hosted
  pthread implementation. The literals alone do not prove automatic selection
  or general capability checking.
- BAREWire `Contract` carries prose, a logic tag and structured return-bound
  fields. CCS reads `Floor`/`AtMost`; arbitrary `Statement` prose is not a parsed
  predicate or a proof certificate. Separately, the graph already has typed
  `ObligationBody` cases with source references for concrete proof families.

The older [platform-predicates draft](../../clef-lang-spec/spec/platform-predicates.md)
places resolution in Alex and presents architecture-wide capability matrices.
Those claims need reconciliation with current CCS saturation and actual target
declarations. Its illustrative `assume val` pattern is an assumption boundary,
not evidence that a device has the asserted property. This audit does not amend
the external language specification or adopt those matrices as hardware facts.

For a future region access, a useful obligation might be expressed as the
following mathematical conditions (not proposed Clef syntax):

```text
offset >= 0
accessBytes > 0
offset + accessBytes <= regionExtent
(mappedBase + offset) mod requiredAlignment = 0
requestedOperation is permitted by this register's contract
mapping and ownership remain valid at this access
```

Keeping `offset + accessBytes <= regionExtent` preserves more information than
independent numeric intervals. A concrete register binding must eventually fix
its required transaction width, but that need not erase the relation that
justifies the access. Arithmetic proof does not establish the physical map;
hardware provenance and any runtime mapping guards remain explicit premises.

If this direction is adopted, a fact needs a subject, provenance, validity scope,
dependencies and evidence status: pending, established, assumed or contradicted.
Unknown must not silently mean false or true. Editing can retain a pending
obligation; an access or lowering decision must discharge it, depend on a checked
runtime guard where supported, or report the missing premise. A changed mapping
or mutable state can invalidate evidence that was valid earlier.

The engineering question is where existing typed obligations, range relations
and scoped boundary descriptors already preserve this information, and where
a declaration or projection is missing. Contracts can name the platform facts
and requirements; CCS owns inference and proof status. Trace one concrete
boundary from its premises to its final access to establish that fit.

## Disposition of the earlier page

The open "aside versus head node" choice is superseded as an account of current
implementation. The distinction between declared facts and local derived facts
remains useful. There is no accepted requirement here to add a new graph root or
to encode all future analyses in one SMT obligation. The original page remains
available in git at platform commit `6164b87`; it is not retained as a second
active design authority.
