# Documentation engineering audit — 2026-09-10

## Scope and evidence

Reviewed the three root design documents, repository navigation, authored leaf
guides, generated-report notices and test/experimental status documents. Vendor
manuals, downloaded web pages and example bundles are reference assets; this
audit does not repeat their electrical verification or rewrite their contents.

Source baseline:

| Repository | Revision |
| --- | --- |
| Fidelity.Platform | `6164b876284e01122c8ced54ac7252cf0d3438c6` |
| clef | `3b74dd020b53d610930dc0c275ba5189da990d24` |
| Composer | `7bf2c831e6d771fc607a864961437e2b501fafbd` |
| BAREWire | `98be0f9be7f9bc00768930f2d207db1ca104174d` |

The revisions identify the inspected source, not newly rerun acceptance of every
package. The preceding main-branch convergence passed the RA6M5 F# I/O check and
a Composer HelloBlinky rebuild with the accepted binary hash. This pass changes
documentation only; it does not change firmware or compiler behavior. A .NET/F#
inspection checked 33 Markdown documents and all 146 local Markdown links, with
no missing targets or HTML documents under Markdown suffixes. `git diff --check`
also passed. The inspection ran from standard input and added no test script or
build dependency. Structural claims were checked against the named sources.

## Disposition

| Material | Finding | Disposition |
| --- | --- | --- |
| [CANONICAL_PLATFORM_SPEC.md](CANONICAL_PLATFORM_SPEC.md) | HTML under a Markdown suffix; mixes pre-implementation claims, completed changes and hypothetical APIs | Rewritten as a source-linked implementation reference; retain filename for code references |
| [D4b_PLATFORM_CARRIER_LOCATION.md](D4b_PLATFORM_CARRIER_LOCATION.md) | Overlong HTML decision brief; incomplete account of declaration nodes/context/derived graph state; unsafe single-core simplification | Keep the useful distinction in a short Markdown note; remove the obsolete binary choice and unsupported proof claims |
| [BAREWire_Rebase_Plan.md](BAREWire_Rebase_Plan.md) | Some work landed, but via different reader locations; alias migration remains incomplete | Replace chronology with current state and remaining acceptance criteria |
| [README](../README.md), [structure](../PLATFORM_STRUCTURE.md), [MCU](../MCU/README.md), [CGRA](../CGRA/README.md) | Removed CPU path and assumed `Platform.fs` filenames | Correct paths and describe manifests as the source list authority |
| [Arty README](../FPGA/Xilinx/Artix7/ArtyA7_100T/README.md) | `.fs` references are stale; additive BAREWire description missing from ownership explanation | Correct filenames and link migration status; retain vendor-source precedence |
| [RA6M5 docs](../MCU/Renesas/RA6M5/EK_RA6M5/docs/README.md) | Useful, sourced material; manifest coverage table and missing-schematic prose contradict accepted bring-up | Retain and reconcile implemented subset, complete wiring inventory and unimplemented drivers |
| [Meadow source pack](../MCU/ST/STM32F7/MeadowF7/docs/README.md) | Placeholder manifest ignores actual staged PDFs/web snapshot | Inventory existing assets without assigning unverified revisions or claiming a completed pin map |
| [Ariel](../CPU/Linux/x86_64/Ariel/README.md), [test guide](../tests/Ariel/README.md), [native status](../tests/Ariel/native/STATUS.md) | Production API and dated native evidence are useful; temporary artifacts may expire | Retain, link source/status and distinguish recorded results from a new audit run |
| [Experimental Ariel](../CPU/Linux/x86_64/Experimental/Ariel/README.md), [status](../CPU/Linux/x86_64/Experimental/Ariel/STATUS.md), [experimental pthread](../CPU/Linux/x86_64/Experimental/Pthread/README.md) | Historical failure language can be read as applying to the later typed implementation | Explicitly scope failures and proposals to the preserved raw-pointer candidate |
| [CPU report](../CPU/Linux/x86_64/LAYER3-REPORT.md), [GPU report](../GPU/AMD/RDNA3_5/LAYER3-REPORT.md), [NPU report](../NPU/AMD/XDNA2/LAYER3-REPORT.md) | Generated inventories contain obsolete generic NativePtr guidance | Preserve as generation records with status notices; do not use as current API guidance or native acceptance |
| Pthread bridge reports ([typed](../CPU/Linux/x86_64/Bindings/PthreadBridge/REPORT.md), [experimental](../CPU/Linux/x86_64/Experimental/Pthread/Bindings/PthreadBridge/REPORT.md)) | Small generation inventories, different source generations | Retain with explicit scope; generated counts are not fresh test results |

No archive copy of the superseded design pages is added. Their exact originals
remain in the baseline commit, keeping only one active account of each topic.

## Findings that require code work, not prose repair

| Finding | Evidence | Required follow-up before claiming support |
| --- | --- | --- |
| Contracts and BAREWire are distinct, partly overlapping schemas | [Contracts](../Contracts/PlatformContracts.clef), [BAREWire description](../../BAREWire/src/Platform/Description.fs) | Migrate consumers with equivalent layout/pin/diagnostic gates; do not remove either schema solely because it overlaps |
| Multi-platform composition is not a resolved resource model | [Profile manifest](../Profiles/StrixHalo_ArtyLab/Fidelity.Platform.fidproj), [PlatformResolution.read](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformResolution.fs) | Define composition/selection; a second selected-form description currently produces ambiguity, not a merged inventory |
| Three scaffold descriptors omit the required `Resets` field | [Meadow](../MCU/ST/STM32F7/MeadowF7/Platform.clef), [GPU](../GPU/AMD/RDNA3_5/StrixHalo_iGPU/Platform.clef), [NPU](../NPU/AMD/XDNA2/StrixHalo_NPU/Platform.clef) | Repair and compile each intended entry; no fresh failure count is asserted here |
| Arty Prelude has an unresolved member reference | [Prelude.Package](../FPGA/Xilinx/Artix7/ArtyA7_100T/ArtyA7_100T.Prelude.clef) names `Platform.xdcConstraints`, absent from bindings | Remove or replace under an application/API gate; successful reachable code does not validate this export |
| Legacy generated bridge packages remain beside newer typed packages | [WaylandBridge](../CPU/Linux/x86_64/Bindings/WaylandBridge/ProtocolDispatch.clef), [ROCm callbacks](../GPU/AMD/RDNA3_5/StrixHalo_iGPU/ROCmBridge/Callbacks.clef), [XRT callbacks](../NPU/AMD/XDNA2/StrixHalo_NPU/XRTBridge/Callbacks.clef) | Reconcile consumers and regenerate or retire unsupported raw-address surfaces; generation reports are not compatibility evidence |
| Architecture selection has residual inference/fallback | [MLIRGeneration.architectureOf](../../Composer/src/MiddleEnd/MLIRGeneration.fs) | Diagnose unsupported identities and check target-field agreement before claiming generic cross-target correctness |
| Hosted memory capacities include assumptions and a virtual-address ceiling | [Linux description](../CPU/Linux/x86_64/Description.clef) | Distinguish authored budgets from discovered/granted memory and runtime limits |
| MMIO handles lack a general mapped-region/grant contract | [MmioPatterns](../../Composer/src/MiddleEnd/Alex/Patterns/MmioPatterns.fs) | Define region provenance, mapping lifetime, access policy and ordering with target-specific enforcement |
| Platform predicate design is ahead of the inspected implementation | [PlatformContext](../../clef/src/Compiler/NativeTypedTree/NativeTypes.fs), [ProjectChecker](../../clef/src/Compiler/Project/ProjectChecker.fs) | The predicate map starts empty; no production map resolver/consumer was found. Reconcile the spec before treating quoted capabilities as proof or automatic dispatch |

The graph already carries typed obligation bodies and source references; a
predicate design should build on that evidence path. The F★/deferred-inference
connection is assessed in [the carrier note](D4b_PLATFORM_CARRIER_LOCATION.md).

## Verification and harness debt

The maintained MCU checks are compiled F# projects documented by
[Composer](../../Composer/docs/MCU_Backend.md) and the
[RA6M5 I/O map](../MCU/Renesas/RA6M5/EK_RA6M5/docs/IO_MAP.md).

Python runners remain under `tests/Ariel`, `tests/Pthread` and
`tests/WaylandNative`; the experimental Ariel candidate also has a shell runner.
This documentation audit does not migrate those independent harnesses or rerun
their suites. Their tests and recorded results must not be represented as
Python-free, or as evidence that the experimental candidate compiles. Replacing
them should preserve the native scenarios and failure checks in compiled F#
projects rather than merely translating command wrappers.

Before adding an MMIO plan, use this reference set to state the missing contract,
the source of each assumption, where narrowing occurs and which consumer will
enforce it. A device inventory, a permission grant and a proved access are three
different claims.
