# Platform composition

Status: implemented single-target composition, 2026-09-10. The
[taxonomy](../PLATFORM_STRUCTURE.md) is the live package structure. This document
replaces the migration sequence with the resulting ownership and selection
rules. General hardware instantiation and multiple execution targets remain
outside this increment.

## One authoritative execution target

An application selects a platform package. That package can depend on reusable
silicon facts, product or virtual-machine inventory, an execution environment
and image/workload requirements. Its `[platform] description` names one fully
qualified immutable `PlatformDescription` or `PlatformDescriptor` export:

```toml
[platform]
description = "Fidelity.Platform.Profiles.EK_RA6M5_HelloBlinky.Description.descriptor"
runtime_model = "bare"
os = "none"
arch = "arm_cortex_m33"
```

The export may be declared in a sibling dependency. CCS follows a plain record,
quotation or immutable alias and preserves the underlying declaration identity.
The export and its referenced declarations must belong to the selected package's
transitive source closure. A declaration in the application or an unrelated
dependency cannot become authority because it was loaded first. Missing,
ambiguous, mutable, malformed or out-of-closure exports are diagnosed.

[PlatformResolution.read](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformResolution.fs)
implements this selection. Packages without an explicit export retain the
legacy directory-scoped, BAREWire-preferred reader. That compatibility path is
not a general composition mechanism. Explicit selection does not merge the two
descriptor schemas: Arty's Contracts pin consumer remains separate from its
selected BAREWire image description.

For explicit selections, the auxiliary pin/clock/device inventory, platform
C ABI facts and platform citations are also scoped to the selected source
closure. Application pin attributes and application-owned native function/layout
descriptors retain their separate roles. An unrelated dependency cannot widen
the selected platform through those auxiliary readers.

## Dependency and source ownership

[SourceResolver](../../clef/src/Compiler/Project/SourceResolver.fs) distinguishes
active dependency paths from completed paths: cycles fail, while repeated
dependency diamonds load once. Source paths are normalized and deduplicated in
dependency/source order. The resolver also carries the selected platform's
source closure and collects transitive link libraries.

Shared sources have dedicated package owners. Full
[BAREWire](../../BAREWire/src/BAREWire.fidproj) depends on
[PlatformMetadata](../../BAREWire/src/BAREWire.PlatformMetadata.fidproj), which
uses [BindingMetadata](../../BAREWire/src/BAREWire.BindingMetadata.fidproj).
Linux similarly separates its environment source owner and default-description
source owner from binding/composition entry packages. These dependency diamonds
do not recreate declaration nodes. Normalized absolute paths provide current
source identity; there is no general package/version or hardware-instance model.

## Ownership realized by the migration

| Composition | Shared facts | Selection |
| --- | --- | --- |
| [EK-RA6M5](../Profiles/EK_RA6M5_HelloBlinky) | Cortex-M33 integer facts; exact BH3CFC memory/vector/package/register inventory; product nets/control wiring; freestanding ABI | Profile owns the 8 KiB stack and image descriptor; HelloBlinky owns clock/PWM policy, mappings and grants |
| [Linux x86_64](../Profiles/Linux_x86_64_Default) | Shared x86_64 facts and Linux execution/native interfaces | Default profile owns preserved section, arena and stack budgets; these are not detected memory allowances |
| [HelloArty](../Profiles/ArtyA7_HelloArty) | Xilinx part/package capacities and Digilent wiring/clock/storage | Profile owns report schema, buffer and UART requirements; Contracts remains the detailed pin authority |
| [RestrictedGuest64](../Profiles/RestrictedGuest64) | Shared x86_64 facts, freestanding ABI and synthetic guest resource inventory | Compiler fixture supplies grants and mappings; no VM boot, page tables or virtio |

GPU/NPU descriptors remain silicon scaffolds. ROCm/XRT native bindings belong
to the Linux environment integration. Their presence does not establish working
accelerator kernels or shared CPU/GPU/NPU memory topology.

## Preserve resources through references

The selected description uses supported literal records/arrays and immutable
references. Arbitrary construction helpers and array concatenation are not
metadata evaluation mechanisms. A shared inventory must not install a workload
`DeviceAccessPlan` merely because it is imported.

[DeviceAccess](../../clef/src/Compiler/PSGSaturation/SemanticGraph/DeviceAccess.fs)
requires a granted region's `MemorySpace` to be the original node in the selected
description. Registers and mappings must reference the same `DeviceRegion`.
Copied records with equal fields do not satisfy these checks. The HelloBlinky
extraction preserves those identities across the new package boundaries.

The current MMIO reader permits at most one plan in the graph. No plan remains
valid for the legacy unbound path. There is no independent export selector for
plans; reusable inventory therefore contains none.

## Execution reconciliation and limits

For explicit descriptions containing a core, CCS checks declared architecture,
OS and runtime against selected package metadata. `bare` and `freestanding` are
normalized as equivalent runtime labels. Existing width/representation checks
still apply. CCS additionally checks the supported triple architecture and OS
components against the core: current cases include x86_64 Linux/none and
`thumbv8m.main-none-eabi`. This is not a general LLVM triple-alias validator.
Workload backend selection must agree with a selected non-library
platform package. Composer requires an explicit CPU/MCU selection to provide a
core and nonempty target triple, and rejects a different CLI triple before
lowering. The MCU backend additionally requires its implemented Cortex-M33
Thumb soft-float contract.

Those are concrete checks, not universal ABI validation. Compatibility paths
and other backend consumers retain their documented limitations. Pointer width,
device transaction width and source value range remain independent facts.

BAREWire memory-space base/capacity metadata remains signed 64-bit. MMIO checks
group granted regions by address-space identity; ordinary BAREWire layout checks
do not implement arbitrary cross-domain composition. CPU virtual, guest physical
and accelerator addresses must not be concatenated into an unqualified map.

Repeated chips need distinct resource-instance identities. Shared backing must
be counted once and related to each mapping. Availability/tiering, negotiated or
discovered resources, DMA visibility, queue ownership and ordering require their
own consumers and evidence. A product inventory containing several compute blocks
does not create several compilation contexts. Those are remaining engineering
boundaries, alongside virtio and OCI orchestration.

## Validation boundary

The maintained [PlatformComposition F# runner](../../Composer/tests/PlatformComposition)
checks real temporary Clef projects through the compiler. The
[PlatformCatalog F# runner](../../Composer/tests/PlatformCatalog) checks the
repository manifests, source ownership and resolved closures. These are maintained
test projects, not application build scripts. The detailed static MMIO baseline
is in [MMIO_CONTRACTS.md](MMIO_CONTRACTS.md).

Completed taxonomy regression gates, 2026-09-10:

| Gate | Result |
| --- | --- |
| Platform composition | 41 cases: dependency identity/cycles, explicit selection, closure-scoped auxiliary facts, target identity and hardware elaboration |
| Device access | 48 cases, including original-node grants, rejected lookalike regions and 64-bit mappings |
| Raw MMIO | 8 cases, including optimized 8/16/32-bit volatile accesses |
| MCU image | 14 checks, including vector errors, linker collision, unsupported import and stale evidence |
| Hosted callbacks | 7 native scenarios |
| Product I/O projection | 1,149 terminals, 355 connector contacts, 176 package pins and 38 trace links match the pinned netlist |
| Platform catalogue | 178 manifests and 6,138 source references checked through the maintained F# catalogue runner |

The MCU acceptance case also invokes Composer with an incompatible CLI triple,
an incompatible workload backend and a selected environment missing its triple.
Each fails before lowering and invalidates stale access evidence. The case was
rerun successfully after the final compiler rebuild.

The extracted HelloBlinky build retains its accepted 2,350-byte binary, 112-vector
layout, 8 KiB stack, 27 register bindings, six grants and 81 access sites. The
[acceptance record](../../MCU/Renesas/EK-RA6M5/HelloBlinky/docs/ACCEPTANCE.md)
separates that compiler/image regression from prior physical-board acceptance.
Vendor reference assets retain their original bytes and provenance. Filesystem
relocation, successful source checking and hardware acceptance remain separate
claims.

Final integration also built and ran the existing Linux greeting application,
and generated fresh HelloArty SystemVerilog/XDC through CIRCT. All 25 HDL ports
matched their constraints and the vendor pin/electrical-standard inventory.
The [Arty acceptance note](../Profiles/ArtyA7_HelloArty/README.md) records the
existing 25 MHz heuristic versus 100 MHz physical-clock distinction; this run
does not establish new Vivado timing closure or hardware deployment.

A separate migration audit verified 5,019 vendor/reference assets byte for byte
at their new locations and checked that Git ignore rules hide no required new
source or previously tracked moved asset. The generic ARM build-output rule now
has a targeted exception for the Cortex-M33 source branch.
