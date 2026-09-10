# BAREWire integration status

Updated 2026-09-10. The filename is retained for source references. Source/package
composition is implemented; unifying the remaining Contracts pin schema still
requires consumer and acceptance work.

## Current state

| Area | Implemented | Remaining boundary |
| --- | --- | --- |
| [Linux profile](../Profiles/Linux_x86_64_Default) | BAREWire description composes reusable execution/core facts and explicit compatibility budgets | Budgets are not discovered memory or universal link-time limits |
| [EK-RA6M5 profile](../Profiles/EK_RA6M5_HelloBlinky) | BAREWire spaces, vectors and Cortex-M image; Composer checks layout and image; MMIO grants preserve original region nodes | Initial register subset; full wiring inventory does not implement every driver |
| [HelloArty profile](../Profiles/ArtyA7_HelloArty) | BAREWire report, buffer, transport and lifecycle requirements reference product/silicon inventory | Contracts still supplies the detailed pin map |
| [Contracts](../Contracts/PlatformContracts.clef) | Shared package dependency; ClockEndpoint frequency uses Clef int | Still a distinct schema, not BAREWire type aliases |
| Meadow/GPU/NPU descriptors | Relocated to product/silicon ownership with complete empty Resets fields | Scaffolds, not working MCU/accelerator implementations |

Full [BAREWire](../../BAREWire/src/BAREWire.fidproj) depends on
[PlatformMetadata](../../BAREWire/src/BAREWire.PlatformMetadata.fidproj), which
uses [BindingMetadata](../../BAREWire/src/BAREWire.BindingMetadata.fidproj).
The manifests now assign one owner to shared declaration sources, allowing the
full library and metadata-only views in one compilation. Source-resolution
diamonds are deduplicated and true cycles fail. See
[composition](PLATFORM_COMPOSITION.md).

## Arty's two current consumers

[Product bindings](../Hardware/Products/Digilent/ArtyA7_100T/ArtyA7_100T.Bindings.clef)
reference the [Xilinx part](../Hardware/Silicon/FPGA/Xilinx/Artix7/XC7A100T_CSG324)
and supply board endpoints. CCS
[PlatformBindings.pins](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformBindings.fs)
reads the Contracts endpoint and descriptor shapes; Composer
[XDCTransfer](../../Composer/src/MiddleEnd/Alex/Traversal/XDCTransfer.fs) serializes
`Codata.Pins`. The Prelude no longer refers to the nonexistent xdcConstraints
member identified by the earlier audit.

The HelloArty profile's BAREWire pins surface contains uart_tx, uart_rx and
sys_clk. It does not replace the larger Contracts pin map. Explicit root selection
chooses the BAREWire profile for its consumer; it does not assert equivalence
between the schemas.

| Declaration | Value | Scope |
| --- | --- | --- |
| BRAM | 622,080 bytes | Silicon total includes parity; synthesis/port configuration determine usable storage |
| Registers | 15,850 bytes | Aggregate flip-flop bits expressed as bytes, not byte-addressable RAM |
| Flash | 16,777,216 bytes | Product storage inventory, without an implied flash driver |
| UART buffer/maximum unit | 32 bytes | HelloArty profile budget, not a derived bound for arbitrary schemas |
| sys_clk | 100 MHz, E3 | Product clock; other arrangements need corresponding constraints |

These FPGA spaces have no base address. They do not describe an implicit soft-CPU
map or establish placement/routing feasibility.

## Remaining schema migration

1. Cover every pin, electrical standard, reset and part fact used by the current
   Contracts consumer in the replacement vocabulary. Resolve endpoint grouping
   and supported UART rates explicitly.
2. Extend the CCS pin projection and compare generated XDC, clock, reset,
   electrical standards and part selection against the existing application.
3. Replace the old consumer only after those gates pass; remove duplicated types
   and fields only after checking all manifest consumers.
4. Check declaration diagnostics as well as successful reachable applications.
   An unreachable invalid export can appear as information and remain unusable.

No universal memory_map.manifest emission or automatic invocation of every
BAREWire observer follows from package composition. MCU image validation and
MMIO domain checks are concrete consumers. `Manifest.emit` and
`Obligations.ofDescription` remain APIs with their own call sites and scope.

Available resources, workload budgets, permission grants, mappings and runtime
allocation remain distinct. [Static MMIO](MMIO_CONTRACTS.md) implements one grant
consumer; general memory domains, instances, DMA ordering and resource negotiation
remain further work. The original rebase proposal remains in git at `6164b87`;
[the audit](DOCUMENTATION_AUDIT.md) records its disposition.
