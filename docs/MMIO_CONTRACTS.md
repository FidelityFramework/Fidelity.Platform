# Device access contracts and Clef predicates

The first implemented slice connects [Contracts/DeviceAccess.clef](../Contracts/DeviceAccess.clef),
[CCS declaration checking](../../clef/src/Compiler/PSGSaturation/SemanticGraph/DeviceAccess.fs),
[Composer lowering](../../Composer/src/MiddleEnd/Alex/Patterns/MmioPatterns.fs) and
[HelloBlinky access selection](../../MCU/Renesas/EK-RA6M5/HelloBlinky/src/Access.clef).

The shared vocabulary separates a BAREWire memory space's availability from
its address-space identity, CPU mapping, hardware transactions and a workload's
grants. Physical register inventory does not grant access. Pointer width, value
range and transaction width remain separate requirements.

| Declaration | Responsibility |
| --- | --- |
| `DeviceRegion` | References the actual BAREWire `MemorySpace` in the selected platform, its address-space identity and hardware source |
| `DeviceRegister` | Region-relative offset, transaction width, register permissions, byte order and ordering requirement |
| `DeviceMapping` | CPU-visible base/address space, lifetime and source of mapping establishment |
| `DeviceGrant` | Workload selection of registers through one mapping, permission ceiling and additional predicates |
| `DeviceAccessPlan` | Exactly one selected set of grants; an empty plan grants nothing |
| `ClefPredicate` | Named typed Boolean quotation with source provenance |

BAREWire remains the memory-layout vocabulary. A region must reference the same
space declaration as the platform; matching names or copied records do not
establish identity. No second capacity/base/alignment schema is introduced in
Contracts. `DeviceRegister` describes a transaction requirement independently of
BAREWire's concrete `FieldDescriptor.Repr` storage layout. Neither its integer
value range nor the CPU pointer width chooses the device transaction width.
CCS and the MCU image backend share the memory-space projection. CCS runs
BAREWire's alignment/granularity and overlap rules on granted regions grouped
by address-space identity. The image backend additionally checks its full
physical image layout. Existing BAREWire space capacities/base fields retain
their current 64-bit metadata bounds; this slice does not widen that schema or
add availability/tiering inference.

CCS reads a selected `DeviceAccessPlan` and establishes each used binding before
Composer lowers it. Mandatory checks cover containment, alignment, address
extent, register width, permissions and mapping lifetime. Plan selection closes
the old raw-address constructors, including indirect uses, for that workload.
Quoted `ClefPredicate` conditions add requirements; they cannot waive mandatory
checks. Predicate evaluation retains source and dependency identities. Unknown
conditions remain pending until a binding requires them, where they prevent
lowering. Physical wiring and a loader's mapping contract remain external
premises, recorded separately from arithmetic conclusions.

Declare inventory and workload selection in dedicated modules, as in
`Registers.clef` and `HelloBlinky.Access`. Keep them out of executable entry-module
initialization. `Mmio.bind16 "helloblinky-port0" "p0podr"` selects a declared grant
and register; the names must be immutable, statically known strings. They remain
compiler metadata and consume no firmware string storage. Read/write operations
use the existing opaque `Mmio8`, `Mmio16` and `Mmio32` handles.

CCS resolves each used handle through immutable aliases to its constructor,
checks the complete write-value range, and puts `MmioAccessEvidence` in graph
codata. Composer consumes that evidence. Raw `Mmio.reg8/16/32` programs with no
plan remain supported and are explicitly recorded as unbound. A selected plan
rejects reachable raw construction, including construction inside a wrapper.

## Predicate fragment and deferred information

[Predicates.fs](../../clef/src/Compiler/PSGSaturation/SemanticGraph/Predicates.fs)
decides closed integer/Boolean expressions over literals, immutable references
and declaration-record fields: arithmetic, comparisons, equality and Boolean
conditions. Arithmetic uses mathematical integers. Explicit fixed-width
arithmetic, conversions, function calls, mutable bindings and runtime-accessible
records are outside this fragment and remain pending. Division by zero cannot
establish a condition. Type errors in a used declaration or predicate dependency
cannot be hidden by the quotation's lack of runtime reachability.

For each used condition, CCS retains its declaration/expression nodes, dependency
nodes, provenance and `Established`, `Contradicted` or `Pending` status. An
established condition is a checked relation over the declarations. It is not an
SMT proof certificate, physical measurement or inferred hardware capability.
`<@ true @>` cannot waive register permissions, containment or mapping checks.

An unused mapping may retain `Base = None`, and an unused register may declare
a transaction wider than the implemented accessors. Use is the point that
requires an established mapping and supported transaction. The existing
`PlatformContext.Predicates` Boolean capability map is not used for this path.

`reset-identity` requires matching region/mapping bases and address-space names.
`boot-contract` permits a distinct CPU-visible base supplied by an external boot
contract. Both require a static, nonzero base and image lifetime in this slice.
Their physical validity is an explicit premise; this code does not verify page
tables or hardware access attributes. The arithmetic covers the whole declared
32-bit or 64-bit pointer range without using host-sized signed address arithmetic.

The first executable binding is static, image-lifetime MMIO with volatile
8/16/32-bit transactions. The declaration vocabulary permits broader transaction
requirements and deferred bases. Unsupported ordering, byte order, runtime
mapping and handle provenance must produce a diagnostic when used. This slice
does not provide page-table construction, DMA ordering, virtqueues, register
transition protocols, interrupt proofs or a cooperative scheduler.

## Evidence and acceptance

Composer writes `targets/intermediates/device-access.json` with `-k` or an
intermediate-output option. It contains per-site addresses/widths, declarations,
predicate dependencies and separate external premises. Rechecking removes the
previous ledger before validation. The ledger describes compiler checks, not a
successfully linked or deployed image.

The acceptance gates are the original HelloBlinky binary, CCS rejection cases,
optimized volatile ARM transactions, the full board netlist comparison through
CCS-checked Clef records, and the [restricted 64-bit guest profile](../Profiles/RestrictedGuest64/README.md).
F# runners live under Composer's `tests/DeviceAccess`, `tests/Mmio`, `tests/MCU`
and `tests/IOMap`. The I/O runner no longer compiles production platform sources
as F#. All authored production sources in Fidelity.Platform use `.clef`.

The gate covers `Mmio` intrinsics. Assembly and foreign code remain explicit
trust boundaries. Grants do not themselves configure an MPU, MMU or TrustZone.

Validation on 2026-09-10:

| Gate | Result |
| --- | --- |
| Device-access F# suite | 48 cases passed, including region/layout failures, malformed predicates, pending declarations and full 64-bit mapped addresses |
| Access ledger | Matches CCS codata; a rejected recheck removes an earlier ledger |
| Raw MMIO regression | 8 cases passed, including all 8/16/32-bit volatile operations after LLVM optimization and ARM lowering |
| MCU image regression | 14 checks passed against the fresh image, including malformed vectors, linker collision and stale deployment evidence |
| Hosted callbacks | 7 fresh native executables passed |
| Board I/O projection | 1,149 terminals, 355 connector contacts, 176 package pins and 38 trace links checked against the pinned netlist |
| Source migration | The three renamed platform declarations have identical git blobs; all authored Fidelity.Platform sources and manifest source entries use `.clef` |

HelloBlinky remains 2,350 bytes with SHA-256
`bd6acfa3589471faff46f3004ce1d159bff6a0650c4e9bddf6deaf091127a2dd`.
Its ledger contains 81 constructor/access sites over 27 registers and six grants.
No board download or new physical acceptance was performed for this migration.

These changes require coordinated versions of Fidelity.Platform, clef, Composer
and HelloBlinky. The predicate specification is reconciled in clef-lang-spec.
BAREWire's source is unchanged. Use the rebuilt Composer checkout; an older
binary cannot consume the new binding operations or their evidence.
