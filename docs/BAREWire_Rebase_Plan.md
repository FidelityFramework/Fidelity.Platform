# BAREWire integration status and remaining migration

Audited 2026-09-10. The filename is retained for existing source references.
This replaces the earlier ordered rebase plan: several steps landed through
different compiler paths, while the Contracts pin migration remains incomplete.

## Current state

| Area | Implemented | Remaining boundary |
| --- | --- | --- |
| Linux CPU | [Description.clef](../CPU/Linux/x86_64/Description.clef) uses BAREWire for core, memory and boundary facts | Declared budgets are not runtime resource discovery or universal link-time limits |
| EK-RA6M5 | [Description.fs](../MCU/Renesas/RA6M5/EK_RA6M5/Description.fs) uses BAREWire directly; Composer checks the memory projection and Cortex-M image | Initial peripheral regions/registers only; full wiring inventory is separate |
| Arty | [Description.clef](../FPGA/Xilinx/Artix7/ArtyA7_100T/ArtyA7_100T.Description.clef) declares spaces, UART buffer/transport and lifecycle | Pin map and device part still come from Contracts bindings |
| Contracts | [PlatformContracts.clef](../Contracts/PlatformContracts.clef) remains a distinct schema | It has not become aliases over BAREWire |
| Meadow/GPU/NPU leaf descriptors | Continue to include Contracts source | Scaffold records omit required `Resets`; no new full-leaf build acceptance is claimed |

Four leaf manifests currently include Contracts source directly: Arty, Meadow,
StrixHalo_iGPU and StrixHalo_NPU. RA6M5 no longer does. Migrating those consumers
must account for their actual manifests; changing a standalone Contracts package
dependency does not automatically migrate them.

## Arty authority during migration

[ArtyA7_100T.Bindings.clef](../FPGA/Xilinx/Artix7/ArtyA7_100T/ArtyA7_100T.Bindings.clef)
supplies the detailed endpoint map and device part. CCS
[PlatformBindings.pins](../../clef/src/Compiler/PSGSaturation/SemanticGraph/PlatformBindings.fs)
still matches the Contracts endpoint and descriptor shapes. Composer
[XDCTransfer](../../Composer/src/MiddleEnd/Alex/Traversal/XDCTransfer.fs) serializes
the resulting `Codata.Pins`.

The BAREWire description's `pins` surface has only `uart_tx`, `uart_rx` and
`sys_clk`. It cannot replace the existing pin map yet. Its memory and buffer
declarations have the following scope; the board sources are linked in the
[Arty README](../FPGA/Xilinx/Artix7/ArtyA7_100T/README.md).

| Declaration | Value | Interpretation |
| --- | --- | --- |
| `bram` | 622,080 bytes | Silicon total includes parity bits; port configuration and synthesis determine usable application storage |
| `registers` | 15,850 bytes | Aggregate flip-flop bit count expressed as bytes, not byte-addressable RAM |
| `flash` | 16,777,216 bytes | Board storage inventory; no application flash driver follows from this declaration |
| `uartTx` / `uart.MaxUnit` | 32 bytes | Declared buffer/transport ceiling; not a fresh computed wire-size result for every application schema |
| `sys_clk` | 100 MHz, E3 | Board clock; designs using another clock arrangement need corresponding constraints |

These spaces have no declared base. They are not an address map for an implicit
soft CPU. Capacity arithmetic alone does not establish FPGA placement or routing.

## Migration acceptance criteria

1. Define how the BAREWire endpoint vocabulary represents every pin, electrical
   standard, reset and device-part fact currently consumed from Contracts.
   Resolve gaps such as endpoint grouping and UART supported-rate sets explicitly.
2. Extend the CCS pin projection and compare its XDC output with the existing
   path using a fixed compiler, application and board declaration. Include
   clock, reset, pin identity, electrical standard and device-part checks.
3. Move the complete map only when the replacement consumer passes. Then derive
   repeated Prelude facts from the chosen authority and remove obsolete fields
   or types after checking every consuming manifest.
4. Validate actual declaration diagnostics as well as emitted applications.
   Unreachable invalid declarations can appear as information diagnostics; one
   successful example does not establish that every exported API is usable.

These are remaining acceptance criteria, not completed gates. The earlier plan's
Composer `PlatformDescriptionResolution` and `PlatformPinResolution` locations
are superseded by the current CCS readers. Its proposed universal
`memory_map.manifest` emission is not present in the inspected Composer source.
The MCU backend's in-process `Check.run` use is specific to its memory projection.
BAREWire's `Manifest.emit` and `Obligations.ofDescription` APIs exist, but their
existence does not establish automatic invocation for every target.

## Known limitations relevant to this migration

- Arty's `Prelude.Package` still refers to `Platform.xdcConstraints`, which is
  absent from the binding source. This is a source defect to address with a
  compiler gate; documentation cleanup does not repair it.
- The earlier plan's HelloArty failures were observations of older compiler
  snapshots. They are not current acceptance results and are no longer used to
  characterize today's compiler.
- Linux section capacities such as 4 KiB and the 8 MiB stack are authored
  budgets/assumptions. The declared heap ceiling is not available physical RAM,
  a container allowance, or a promise of successful allocation.
- Neither schema currently expresses the complete proposed distinction between
  available resources and workload grants. Reusing BAREWire for new MMIO contracts
  should preserve this distinction without treating the existing records as a
  completed design.

See [the audit](DOCUMENTATION_AUDIT.md) for disposition of historical documents
and [the current integration reference](CANONICAL_PLATFORM_SPEC.md) for source
ownership. The original plan is available in git at `6164b87`.
