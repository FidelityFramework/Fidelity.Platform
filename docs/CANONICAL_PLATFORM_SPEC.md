# Platform declarations and compiler integration

Status: implementation reference, 2026-09-10. The historical filename is retained
for source references. The [taxonomy](../PLATFORM_STRUCTURE.md) is implemented;
[composition rules](PLATFORM_COMPOSITION.md) describe the current single-target
selection boundary.

## Responsibilities and schemas

Fidelity.Platform owns target declarations and binding packages. CCS checks their
source, resolves supported declaration shapes, and settles numeric/layout/access
facts. Composer consumes those results and drives the selected backend.

| Vocabulary | Current role | Consumers |
| --- | --- | --- |
| [Contracts/PlatformContracts.clef](../Contracts/PlatformContracts.clef) | Substrate tags, core facts, endpoints and PlatformDescriptor | Arty pin map and Meadow/GPU/NPU scaffolds |
| [BAREWire platform description](../../BAREWire/src/Platform/Description.fs) | Core, memory spaces, surfaces, buffers, transports and lifecycle | Linux/HelloArty/HelloBlinky profiles and synthetic guest |
| [BAREWire hardware descriptors](../../BAREWire/src/Hardware/Descriptors.fs) | ABI/layout and Cortex-M image vocabulary | Native bindings and Composer MCU image backend |
| [Contracts/DeviceAccess.clef](../Contracts/DeviceAccess.clef) | Regions, mappings, register requirements, grants and Clef predicates | CCS MMIO validation and Composer evidence-based lowering |

Contracts and BAREWire remain distinct, partly overlapping schemas. Contracts
collections use lists and optional toolchain overrides; BAREWire uses arrays and
string fields. Their consumers have not been replaced by a universal alias layer.
Consumers now depend on the Contracts package instead of copying its source list.
BAREWire metadata/full-library packages likewise share explicit source owners.

## Authoritative export selection

A selected platform manifest can name its root explicitly:

```toml
[platform]
description = "Fidelity.Platform.Profiles.EK_RA6M5_HelloBlinky.Description.descriptor"
runtime_model = "bare"
os = "none"
arch = "arm_cortex_m33"
```

[SourceResolver](../../clef/src/Compiler/Project/SourceResolver.fs) resolves
manifest dependencies before workload sources, diagnoses cycles, deduplicates
completed diamonds and normalized source paths, and records the selected
platform's transitive source closure.
[PlatformResolution.read](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformResolution.fs)
looks for exactly that qualified immutable binding in that closure. Plain records,
quotations and immutable aliases are supported; calls or mutable initializers do
not establish the export. References outside the closure are rejected. Missing,
malformed, conflicting and ambiguous declarations use CCS8206/8207/8208 diagnostics.

This is explicit selection, not merging. Unselected catalogue records do not
become authority. Without `[platform] description`, the legacy directory-scoped
reader prefers BAREWire PlatformDescription over Contracts PlatformDescriptor
and rejects additional descriptions of the chosen form. Existing compatibility
entry packages retain that behavior where they do not opt into explicit selection.

Declarations compile as typed PSG nodes and retain source provenance. The reader
follows supported values; it does not run arbitrary quotation programs. Use
complete typed records and literal/reference arrays. Declaration discovery includes
unreachable metadata without making it a live firmware/application table.
`readDescriptors` separately projects ABI and native-function declarations.
See [platform facts and derived state](D4b_PLATFORM_CARRIER_LOCATION.md).

Under explicit selection, auxiliary platform pin/clock/device inventory, C ABI
facts and citations are restricted to the selected source closure too. This
does not remove application-owned pin attributes, native function descriptors
or layout declarations from their respective consumers.

## Narrowing and execution checks

Source value range, pointer representation, register width and transaction width
are separate facts. A 64-bit pointer does not select a 64-bit device transaction.
[PlatformDeclaration.fill](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformDeclaration.fs)
projects named widths, representations and endpoint return bounds into the graph
context. Missing dimensions and unavailable representations are diagnosed where
required. WordSizeBits is checked against Register width; it does not replace
Pointer. The old `word_size` project key is not a width source.

For explicit selections with a core, CCS reconciles architecture, OS and runtime
with selected package metadata. `bare` and `freestanding` are equivalent runtime
labels for that check. Workload backend selection must agree with a selected
non-library platform package.
[CompilationOrchestrator](../../Composer/src/Core/CompilationOrchestrator.fs)
requires explicit CPU/MCU profiles to supply a core and nonempty target triple,
and rejects a conflicting CLI target triple before lowering. Thus those profiles
cannot silently select the build host because their triple is absent.

CCS also checks the architecture and OS components of supported triple forms
against the core, including x86_64 Linux/none and `thumbv8m.main-none-eabi`.
It does not implement LLVM's complete target-triple alias grammar.

These checks do not establish universal ABI/toolchain compatibility. The legacy
path retains limitations, and
[MLIRGeneration.architectureOf](../../Composer/src/MiddleEnd/MLIRGeneration.fs)
still has architecture-name matching and an unknown-name fallback. The MCU backend
adds its specific Cortex-M33 Thumb soft-float checks. A Native representation is
an authored capability claim, not a proof of cost or every operation's support.

## FPGA composition

[Arty silicon](../Hardware/Silicon/FPGA/Xilinx/Artix7/XC7A100T_CSG324) owns the part,
package and capacities; [Digilent product bindings](../Hardware/Products/Digilent/ArtyA7_100T)
own board wiring, clock and storage. The
[HelloArty profile](../Profiles/ArtyA7_HelloArty) owns report, buffer and UART choices.

[PlatformBindings.pins](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformBindings.fs)
still reads Contracts PinEndpoint, ClockEndpoint, ResetEndpoint and
PlatformDescriptor records, relates them to design attributes and emits
`graph.Codata.Pins`.
[XDCTransfer](../../Composer/src/MiddleEnd/Alex/Traversal/XDCTransfer.fs) serializes
that mapping. The selected BAREWire profile adds memory/buffer/lifecycle metadata;
its smaller surface list does not replace the broader pin map. See
[BAREWire integration](BAREWire_Rebase_Plan.md).

## MCU composition and evidence

[EK_RA6M5_HelloBlinky](../Profiles/EK_RA6M5_HelloBlinky) explicitly selects one
PlatformDescription and CortexMImageDescriptor. It references the original
[R7FA6M5BH3CFC spaces and vectors](../Hardware/Silicon/MCU/Renesas/RA6M5/R7FA6M5BH3CFC)
and [freestanding execution core](../Environments/Freestanding/arm_cortex_m33).
The [product](../Hardware/Products/Renesas/EK_RA6M5) owns board wiring, the full
netlist and vendor provenance. HelloBlinky owns timing, mappings and grants.

Composer owns BAREWire layout validation, startup/linker constants, assembly,
linking, image verification and optional SEGGER deployment. No application C,
Python or shell build wrapper participates. The extracted build preserves the
accepted 2,350-byte firmware and 112-vector image; see
[acceptance](../../MCU/Renesas/EK-RA6M5/HelloBlinky/docs/ACCEPTANCE.md) and
[MCU_Backend.md](../../Composer/docs/MCU_Backend.md).

[MMIO contracts](MMIO_CONTRACTS.md) describe the static grant/predicate consumer.
Its evidence does not establish runtime page tables, DMA ordering, physical
memory attribution, concurrency or cooperative scheduling. The restricted guest
exercises 64-bit mappings with 32-bit transactions; it does not implement virtio.

## Remaining boundaries

Single-target composition is implemented. General SoC/resource instances,
qualified cross-domain memory composition, negotiated resources and multiple
execution targets are not. Hosted budgets remain profile assumptions. GPU/NPU
scaffolds, reserved OS/protocol directories and generated binding inventories do
not establish executable device support. The dated
[audit](DOCUMENTATION_AUDIT.md) preserves earlier findings and their disposition.
