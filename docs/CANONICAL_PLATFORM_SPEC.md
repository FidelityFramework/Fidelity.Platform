# Platform declarations and compiler integration

Status: implementation reference, audited 2026-09-10. The filename is retained
because source comments refer to it. This replaces the earlier HTML design
proposal; it does not claim that every platform has one fully unified schema.
Revision anchors and remaining defects are in [the audit](DOCUMENTATION_AUDIT.md).

## Responsibilities and current schemas

Fidelity.Platform supplies target declarations and binding packages. CCS checks
their source, resolves supported declaration shapes, and settles numeric and
layout facts. Composer consumes those results and drives the selected backend.

| Vocabulary | Current role | Consumers |
| --- | --- | --- |
| [Contracts/PlatformContracts.clef](../Contracts/PlatformContracts.clef) | Shared tags, `TargetCore`, board endpoints and `PlatformDescriptor` | Arty endpoint bindings; Meadow, GPU and NPU scaffold descriptors |
| [BAREWire platform description](../../BAREWire/src/Platform/Description.fs) | `TargetCore`, memory spaces, surfaces, buffers, transports and lifecycle facts | Linux CPU, Arty's additive description, and RA6M5 |
| [BAREWire hardware descriptors](../../BAREWire/src/Hardware/Descriptors.fs) | Physical ABI/layout and Cortex-M image vocabulary | Generated bindings and Composer's MCU backend |

Contracts is currently a real, separate schema, **not a type-alias layer over
BAREWire**. Its collections are lists and its triple/CPU overrides are optional;
BAREWire uses arrays and string fields. The duplication is migration work, not
evidence that either family can be deleted. Several leaf manifests include the
Contracts source directly; the existence of its own `.fidproj` does not mean
all leaves depend on it as a package.

## The mechanism constraint

Declarations compile with the application into typed PSG records. The reader
follows supported bindings, references and `Quote` nodes to their values; it does
not run a general quotation evaluator. Quotations remain a supported authoring
form. Existing literal record declarations do not need to be wrapped merely to
be read. Arbitrary helper calls or partially specified records are not implied
by this mechanism. Use complete, explicitly typed records for new facts until
the reader and its tests support another form.

[PlatformResolution.fs](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformResolution.fs)
defines the actual supported shapes. `read` prefers a BAREWire
`PlatformDescription` when present, otherwise a Contracts `PlatformDescriptor`.
It reports additional descriptions of the selected form as ambiguous. It does
not merge a collection of platform descriptions or prove equivalence between
the two forms. Declaration defects have located CCS8206/8207/8208 diagnostics.
`readDescriptors` separately projects ABI and native function declarations.

The declaration scan includes bindings outside executable reachability. A
platform declaration can inform compilation without becoming a live application
table. Ordinary emission traversal still begins at the program's declaration
roots. See [platform facts and derived state](D4b_PLATFORM_CARRIER_LOCATION.md).

## TargetCore and numeric narrowing

Source value ranges, named core widths, and device transaction widths are
different facts. A 64-bit pointer declaration does not make every register or
value 64 bits. Both current core vocabularies declare named `Widths` and numeric
`Representations`; the Linux and RA6M5 leaves supply `Pointer` and `Register`.

[PlatformDeclaration.fill](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformDeclaration.fs)
projects widths, representations and endpoint return bounds into the graph's
`PlatformContext`. Missing dimensions and unavailable representations are
diagnosed where required. `WordSizeBits` remains in both schemas and is checked
against a declared `Register` width; it is not a replacement for `Pointer`.
The project key `word_size` is no longer a width source and is reported as unused.

This is an implemented narrowing boundary. A fully general resource-grant,
runtime mapping, or memory-tier selection system is not implemented by those
fields. A representation marked `native` is a declaration; it does not by itself
prove instruction count, hardware execution cost, or every operation's support.

## Project loading and backend selection

1. [ProjectChecker](../../clef/src/Compiler/Project/ProjectChecker.fs) builds the
   initial context from the selected package and project metadata. Its numeric
   maps start empty and are filled from the compiled declaration.
2. [MLIRGeneration](../../Composer/src/MiddleEnd/MLIRGeneration.fs) reads the
   declared architecture and the context's settled widths. The implementation
   still has architecture-name matching and an unknown-name x86_64 fallback.
   This is a remaining implementation limitation, not a portability guarantee.
3. [CompilationOrchestrator](../../Composer/src/Core/CompilationOrchestrator.fs)
   supplies the declared triple, pointer width and CPU model to the backend;
   an explicit CLI target override takes precedence over the declared triple.
4. The MCU backend additionally requires agreement between the resolved image
   contract and target settings. Other backends have their own checks. There is
   no demonstrated universal reconciliation of every overlapping project,
   Contracts and BAREWire identity field, including OS/runtime/endianness.

Do not use folder names as numeric facts or document a generic triple derivation
that the code does not implement. The CPU package still has the public name
`Fidelity.Platform.Linux_x86_64`; its directory is `CPU/Linux/x86_64`.

## FPGA path

[PlatformBindings.pins](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformBindings.fs)
still reads Contracts `PinEndpoint`, `ClockEndpoint`, `ResetEndpoint` and
`PlatformDescriptor` values, and relates them to design pin attributes. The
result is carried in `graph.Codata.Pins`.
[XDCTransfer](../../Composer/src/MiddleEnd/Alex/Traversal/XDCTransfer.fs) serializes
that mapping. The old Composer `PlatformPinResolution` path named by earlier
documents is no longer its location.

Arty's BAREWire description is additive. Its three surface endpoints do not
replace the broader Contracts pin map or device-part declaration. The remaining
migration and its acceptance criteria are in [BAREWire integration status](BAREWire_Rebase_Plan.md).

## MCU path and evidence boundary

The [EK-RA6M5 package](../MCU/Renesas/RA6M5/EK_RA6M5/Fidelity.Platform.fidproj)
selects package pins, board nets, a BAREWire description/image contract and the
initial register declarations. Its old `Platform.clef` scaffold is gone.

Composer owns layout generation, assembly/linking, image checks and optional
SEGGER deployment. [MCU_Backend.md](../../Composer/docs/MCU_Backend.md) documents
the actual commands, tool dependencies and F# regression projects.
[HelloBlinky acceptance](../../MCU/Renesas/EK-RA6M5/HelloBlinky/docs/ACCEPTANCE.md)
records the board result and the limits of that evidence.

The eight MMIO cases check source rejection and optimized exact-width volatile
access. Fourteen MCU checks cover image/linker failures. The platform I/O test
compares 1,149 terminals against a pinned netlist and checks package/header
coverage. These are distinct from proof of interrupt interleavings, stack
adequacy, memory protection, or a cooperative scheduler. Wiring completeness is
not completeness of the implemented peripheral register map or driver set.

## Extension boundary

The shared Contracts location is appropriate for substrate-neutral vocabulary,
but extending it must account for the BAREWire vocabulary already in use.
Resource availability, workload grants, address-space identity, mapping lifetime,
memory attributes and device transaction policy need explicit consumers and
checks before being presented as supported APIs. Virtio, DMA ownership and
wait/rearm protocols remain design work. This document records the current
foundation; it does not prescribe a new MMIO implementation plan.
