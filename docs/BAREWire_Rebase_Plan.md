# BAREWire rebase plan: the Contracts alias layer and the Arty description

Readiness Audit §4 step 9 of the BAREWire repository (`docs/Readiness Audit.md`)
says two things about this repository: `Contracts/PlatformContracts.clef`
becomes an alias layer over the BAREWire platform types so that no leaf
breaks, and the Arty A7-100T description gains block RAM and register memory
spaces, the UART transmit buffer schema for HelloArty's `ArtyReport`, and the
UART transport. This document is the plan for both: what each Contracts type
maps to, which Arty facts are declared and from which source, and the order
of edits that keeps the compiler's pin resolution working throughout.

The vocabulary is BAREWire's `src/Platform/{Tags,Description,Check,Manifest,
Obligations}.fs` (BAREWire docs/11). Its records are plain, its tags are
string aliases with constant modules, exactly as `PlatformContracts.clef`
already spells its own, so the two layers clear the same cross-assembly
union-layout wall and can coexist in one compilation.

## 1. Where things stand

Done, additively, in this tree:

- `FPGA/Xilinx/Artix7/ArtyA7_100T/ArtyA7_100T.Description.clef`: the Arty
  A7-100T as a `BAREWire.Platform.PlatformDescription` value (three memory
  spaces, a `pins` surface with the three pins a declared transport or clock
  binds, the `uartTx` buffer schema, the `uart` transport, clock and reset as
  lifecycle). Every fact is a module-level `let name: Type = { ... }` record
  with an explicit annotation, the form the compiler extracts structurally
  from the PSG (`docs/CANONICAL_PLATFORM_SPEC.md`, "The mechanism
  constraint").
- `FPGA/Xilinx/Artix7/ArtyA7_100T/Fidelity.Platform.fidproj`: the file is in
  `sources` after the bindings, and `[dependencies]` carries
  `barewire = { path = "../../../../../BAREWire/src/BAREWire.fidproj" }`.
- `CPU/Linux/x86_64/Description.clef` and its fidproj, the CPU counterpart,
  landed in the same pass; it is the HelloProof target and is not covered
  here beyond the ordering notes in §4.

Not done, by design (see §4 for the order):

- `Contracts/PlatformContracts.clef` is unchanged. Nothing in it is renamed,
  removed, or re-typed. The alias layer is realised as a *second, parallel
  value* on the Arty side rather than as a rewrite of the Contracts types,
  because the four type names the compiler matches by name must survive
  until the compiler reads the description instead.
- The pin map (60 `PinEndpoint`s) and the device part stay in
  `Platform.descriptor`. The description's `pins` surface declares only the
  pins a declared transport or clock binds today.
- `Prelude.clockHz` and `ticksPerMs` are not yet derived from the
  description's clock.

## 2. Type mapping: Contracts to BAREWire

`Contracts` is not a package a leaf depends on; each leaf's fidproj lists
`../../../../Contracts/PlatformContracts.clef` as a *source*. Five leaves do
(Arty, Strix Halo iGPU, Strix Halo NPU, Meadow F7, EK-RA6M5). An `open
BAREWire.Platform` inside Contracts would therefore force the BAREWire
dependency on every leaf at once. The alias layer is instead adopted per
leaf: a leaf that takes the dependency writes its `Description.clef`; a leaf
that has not keeps compiling untouched. That is what "no leaf breaks" means
operationally.

| Contracts type | BAREWire counterpart | Notes |
| --- | --- | --- |
| `SubstrateKind` (string, constant module) | `PlatformDescription.Substrate: string` | Same spellings (`"FPGA"`, `"CPU"`, ...). BAREWire declares no constant module for it; `SubstrateKind` stays the constant module, and a description writes the same literal. Stays as the only home of the constants. |
| `PinDirection` | none | Stays: FPGA-specific. A pin's direction is already determined by which record the design's `[<Pin>]` field sits in (`Inputs` or `LedOutputs`); `PinEndpoint.Direction` restates it. When XDC is projected from the description, direction comes from the design type, not from the endpoint. |
| `ElectricalStandard` | `Default { Key = "io-standard"; Value }` on a `BoundarySurface` of kind `Pins` | A surface-wide default (docs/11: "the IO-standard default lives in a `Notes` string and in the compiler" is the gap this closes). A board with two banks at different standards declares two `pins` surfaces, one per standard; there is no per-endpoint override and none is needed for the Arty (every user pin is LVCMOS33). |
| `ActiveLevel` | `Reset.ActiveHigh: bool` | |
| `ResetKind` | `Reset.External: bool` | |
| `PinEndpoint { LogicalName; PackagePin; Direction; Standard; Description }` | `Endpoint { Name = LogicalName; Location = EndpointKind.PackagePin; Address = PackagePin; Contracts; Signature = ""; Since; Until }` | `Direction` and `Standard` per the two rows above; `Description` has no field (BAREWire endpoints carry contracts, not prose; a board note goes in `PlatformDescription.Notes`). |
| `ClockEndpoint { Name; FrequencyHz; PackagePin; Standard; Description }` | `Clock { Name; FrequencyHz }` in `LifecycleFacts.Clocks`, plus an `Endpoint` (`PackagePin`) on the `pins` surface for its package pin | The clock splits in two because a frequency is a lifecycle fact and a pin is a surface fact. The two share the name (`sys_clk`) so an observer can join them. |
| `ResetEndpoint { Name; Kind; PackagePin; Standard; ActiveLevel; Description }` | `Reset { Name; External; ActiveHigh }` in `LifecycleFacts.Resets`, plus an `Endpoint` on `pins` only when `External` | The Arty's reset is internal (no pin), so only the `Reset` record exists. |
| `EndpointGroup { Name; Width; Pins; Description }` | none | Stays: FPGA-specific. A group is a naming convention over endpoints (`led[0..3]`); its `Width` is `Array.length` of the members once they are endpoints on a surface, which retires the unchecked literal the audit names. |
| `UartEndpoint { Name; Tx; Rx; DefaultBaud; SupportedBaudRates; Description }` | `Transport { Name; Kind = TransportKind.Uart; Endpoints = [| tx; rx |] (by endpoint name); RateHz = DefaultBaud; MaxUnit; Ordered = true; Schema }` | `SupportedBaudRates` has no home and stays in Contracts until a rate set is a declared fact something reads. `MaxUnit` and `Schema` are new content: the largest unit is one framed `ArtyReport`. |
| `PlatformDescriptor { Id; DisplayName; Substrate; Vendor; Family; Device; Package; SpeedGrade; Clocks; Resets; Groups; Uarts; DedicatedPins; Notes }` | `PlatformDescription { Id; DisplayName; Substrate; Core = None; Spaces; Surfaces; Buffers; Transports; Lifecycle; Notes; Limits }` | Same `Id` on both values so they name one board. `Vendor`, `Family`, `Device`, `Package`, `SpeedGrade` stay: FPGA-specific, they form the Vivado device part (`xc7a100tcsg324-1`) the compiler builds in `extractDevicePart`, and they are also in the fidproj's `[platform]` table. `Core` is `None` for a fabric. `Spaces`, `Buffers`, `Transports` are new content. |
| `PlatformDescriptor.allPins`, `pinMap`, `packagePinMap` (`list`, `Map`) | `PlatformDescription.tryFindEndpoint`, `hasEndpoint`, `tryFindSurface` (array scans) | The Contracts helpers use `list` and `Map`, which the native surface does not lower (BAREWire docs/12, `arrays`, `no-map`); the BAREWire lookups are the portable spelling. |

Two Contracts idioms carry over unchanged and are worth naming: the
string-alias tag with a `[<RequireQualifiedAccess>]` constant module (BAREWire
docs/12 credits Contracts as the origin), and the typed module-level record
value as the compiler's extraction unit.

## 3. Arty facts declared, with sources

All sources are under `FPGA/Xilinx/Artix7/ArtyA7_100T/docs/` or in this
tree. Precedence follows `README.md` there: XDC for pins, the reference
manual for board semantics, the schematic for connectivity, the silicon
datasheet for electrical context.

| Fact | Value | Declared as | Source |
| --- | --- | --- | --- |
| Block RAM total | 622,080 bytes (4,860 Kbit; "607.5 KB") | `MemorySpace bram`, kind `block-ram`, granularity 2,048 (the data half of one RAMB18), no base | Reference manual, Artix-7 feature table, Arty A7-100 column; feature list item 2 |
| Flip-flop total | 15,850 bytes (126,800 flip-flops) | `MemorySpace registers`, kind `registers`, granularity 1, no base | Reference manual, feature table, Arty A7-100 column |
| Quad-SPI flash | 16,777,216 bytes (128 Mbit) | `MemorySpace flash`, kind `flash`, read-only, granularity 65,536 (the 64 KB erase sector all listed parts share) | Reference manual §5.2 and its feature list ("16MB Quad-SPI Flash"); Table 5.2.1 for the part by board revision |
| IO standard | LVCMOS33 | `Default io-standard` on surface `pins` | Reference manual; `Arty-A7-100-Master.xdc` (every user pin); `Platform.descriptor` note 1 |
| UART pins | `uart_tx` = D10, `uart_rx` = A9 | `Endpoint`s of kind `package-pin` on `pins` | `Arty-A7-100-Master.xdc` lines 83–84 (`uart_rxd_out`, `uart_txd_in`, named from the PC's side); `Pins.uartTx`/`Pins.uartRx` |
| System clock pin | E3 | `Endpoint sys_clk` on `pins` | `Arty-A7-100-Master.xdc` line 7; reference manual §7 |
| System clock frequency | 100,000,000 Hz | `Clock sys_clk` in `lifecycle` | Reference manual §7; `Pins.sysClk.FrequencyHz` |
| Reset | internal power-on reset, active-high | `Reset rst` in `lifecycle` | `Pins.resetEndpoint` and its comment (no user-accessible reset pin) |
| UART bridge | FT2232HQ, 115,200 baud, ordered | `Transport uart`, `RateHz = 115200`, `Endpoints = [| "uart_tx"; "uart_rx" |]` | Reference manual §8; `Uart.usbUart.DefaultBaud` (the manual states no rate; 115,200 is this tree's convention) |
| `ArtyReport` on the wire | at most 12 payload bytes (two one-byte BARE enum tags, one zigzag `int` of at most 10 bytes) plus the 9-byte BAREWire envelope (4-byte length prefix, 5-byte header) = 21; declared ceiling 32 | `BufferSchema uartTx` (`Schema = "ArtyReport"`, `Capacity = 32`, `Framing = length-prefixed`, `Space = "bram"`, `Lifetime = program`) and `Transport.MaxUnit = 32` | `~/repos/HelloArty/src/Shared/Contract.clef`; BAREWire docs/05 for the envelope. The exact payload size is what the `[<BAREWireSchema>]` consumer (Audit step 8) will compute with `Analysis.wireSize`; 32 is the ceiling the obligations check until then. |
| Persistence | volatile | `LifecycleFacts.Persistence` | Configuration is reloaded from flash at power-on (reference manual §4.2) |

The observers agree on this value: `Check.run` is empty; `Manifest.emit`
prints three `space` lines, one `buffer`, one `surface` with three endpoints
and one default, one `transport`, one `clock`, one `reset`, and an empty
`MEMORY {}` block (no space has a base: the fabric is not address-mapped
until a design instantiates it); `Obligations.ofDescription` yields
`capacity_positive_uarttx`, `capacity_uarttx` (32 <= 622,080) and
`fits_uart_uarttx` (32 <= 32), each `unsat` under cvc5 in refutation form.

Deliberately not declared, and why:

| Fact | Why not yet |
| --- | --- |
| DDR3L, 256 MB, MT41K128M16JT-125 (reference manual §5.1) | `MemoryKind` has no DRAM kind. Adding one is a one-line BAREWire change; it should land with a first design that instantiates a MIG controller, so the kind arrives with its reader. |
| Distributed (LUT) RAM | The reference manual states LUT and slice counts, not a LUT-RAM figure. The Xilinx DS180 overview does (1,188 Kb for the XC7A100T) but is not under `docs/`; declare it when the datasheet is added. |
| DSP slices, clock management tiles, XADC | Not memory, not a surface, not a transport. If a resource budget is wanted, it is a `Limit` (`PlatformDescription.Limits`), and the obligation that reads it does not exist yet. |
| Pmod, ChipKit analog, SPI, I2C, Ethernet PHY, power telemetry | Deferred scope of the bindings (`README.md`, `.serena/memories/arty_a7_binding_gap_audit.md`); they enter the `pins` surface when the pin map is re-homed (§4 step 4). |
| The other 57 user pins | Stay in `Platform.descriptor` until Composer projects XDC from the description (§4 step 3). Declaring them twice would create a second authority for the pin map. |

## 4. Order of edits that keeps `PlatformPinResolution` working

The compiler's pin resolution (`Composer/src/MiddleEnd/PSGElaboration/
PlatformPinResolution.fs`) walks every `Binding` node and matches the
*short type name* of its value: `PinEndpoint`, `ClockEndpoint`,
`ResetEndpoint`, `PlatformDescriptor`. `HardwareModuleWitness` follows the
chain `Design.Clock -> Endpoints.clock -> Pins.sysClk -> ClockEndpoint` to
mark it consumed. `XDCTransfer` then writes the XDC from the resulting
coeffect. Renaming any of those four types, or moving `Pins.sysClk`, breaks
HelloArty. BAREWire's names are disjoint (`PlatformDescription`,
`Endpoint`, `Clock`, `Reset`), so both value families can live in one PSG
without either extractor seeing the other's records.

1. **Additive description (done).** `Description.clef` and the fidproj
   dependency. Gate: HelloArty compiles exactly as before with the platform
   tree, and the description is observed on the .NET side (`Check.run`
   empty, manifest and ledger as in §3). This step touches no Contracts
   type.
2. **The compiler reads the description (Composer).** A
   `PlatformDescriptionResolution` pass extracts `PlatformDescription` and
   its parts by type name, for every target, and runs `Check.run`,
   `Manifest.emit` (to `memory_map.manifest` beside `constraints.xdc`), and
   `Obligations.ofDescription`. Gate: HelloArty's intermediates gain the
   manifest and the three Arty obligations; the XDC is unchanged because it
   still comes from the pin coeffect.
3. **XDC from the description (Composer).** `XDCTransfer` learns to take
   `Endpoint`s of kind `package-pin` from surfaces of kind `pins`, the IO
   standard from the surface `Default`, the direction from the design's
   `Inputs`/`Outputs` record that the `[<Pin>]` field sits in, the clock
   from `LifecycleFacts.Clocks` joined by name to its endpoint, and the
   device part from the fidproj `[platform]` table. Gate: the XDC produced
   from the description is byte-identical to the one produced from the pin
   coeffect for HelloArty.
4. **Re-home the pin map (this tree).** Add the 57 remaining user pins to
   the `pins` surface (or to a second surface if a bank at another standard
   appears), with names identical to the `LogicalName`s. Group widths become
   counts of members. Gate: step 3's identity check still holds.
5. **Derive what is now duplicated (this tree).** `Prelude.clockHz` and
   `ticksPerMs` read `Description.sysClk.FrequencyHz`; `Endpoints.clock`
   points at the description's clock. This is the step that changes the
   `HardwareModuleWitness` chain, so it goes after step 3, when the witness
   no longer needs `ClockEndpoint`. Note BAREWire docs/12 `literal-values`:
   a plain module-level primitive read across modules is a native-surface
   gap on the CPU path; on the FPGA path `clockHz` works today and the
   derived form must be checked against the same gate.
6. **Retire the Contracts endpoint types.** Only now do `PinEndpoint`,
   `ClockEndpoint`, `ResetEndpoint`, `EndpointGroup`, `UartEndpoint` become
   thin aliases or go, and `PlatformDescriptor` shrinks to the device-part
   block (`Vendor`, `Family`, `Device`, `Package`, `SpeedGrade`) beside a
   reference to the description. The other four leaves migrate one at a
   time by writing their own `Description.clef` and taking the dependency;
   until a leaf does, it keeps the full Contracts types by keeping the old
   file in its sources.

The invariant across steps 1–5: `PlatformPinResolution` finds the same
four type names, the same `Pins.sysClk` chain, and the same device part it
finds today, so `constraints.xdc` never changes until step 3 proves the two
projections identical.

## 5. What the compiler surface imposed on `Description.clef`

These are the BAREWire docs/12 rules that shaped the file; each site in the
file carries the same rule id in a comment where a spelling was chosen for
the compiler rather than for the reader.

- `record-inference`: `Clock` (BAREWire) and `ClockEndpoint` (Contracts)
  share the labels `Name` and `FrequencyHz`, and the checker types a record
  literal by its labels, last declaration winning; the `sysClk: Clock`
  literal uses type-qualified labels (`Clock.Name = ...`). The same hazard
  exists for `Name` on every record here; the annotation on the `let`
  resolves it for all but the clock, whose label set is a strict subset of
  `ClockEndpoint`'s.
- `string-tags`, `literal-values`: every tag is a constant from a BAREWire
  module (`MemoryKind.BlockRam`, `Framing.LengthPrefixed`, ...), the
  cross-module spelling the description is meant to use. Under the pinned
  native snapshot those constants are unwitnessed (`string-constant`), as
  are string equality and `Array.length`, so the description is observed on
  the .NET side (the compiler's own reference to `BAREWire.fsproj`) until the
  compiler closes those gaps; the FPGA lowering never evaluates the
  description at all, it only extracts it.
- `Substrate = "FPGA"` is a literal rather than `SubstrateKind.FPGA` so the
  file does not `open Fidelity.Platform.Contracts`, which would put every
  Contracts label into scope for record inference.

## 6. Findings on the current tree, outside the scope of this plan

- `ArtyA7_100T.Prelude.clef` lines 141–142 (`Package.descriptor =
  Platform.descriptor`, `Package.xdcConstraints = Platform.xdcConstraints`)
  do not resolve under Composer: `Platform.` is the compiler's intrinsic
  module there ("Unknown Platform intrinsic ... Available: sizeof,
  wordSize"), and `xdcConstraints` left the bindings in dd675be. The module
  is unreachable from HelloArty, so the checker demotes both to info lines.
  `Package` is dead code and should go when step 5 touches the Prelude.
- `ArtyA7_100T.Bindings.clef` line 238 (`chipKitDigitalGpioPins`, a
  `List.map` over a tuple list) is reported as a type mismatch info line
  under both the pinned and the rebuilt Composer; it too is unreachable
  from HelloArty. The pin re-homing in step 4 replaces it with endpoint
  records.
- HelloArty does not compile with the pinned Composer snapshot
  (`9a1a60b`) with or without `Description.clef` in the platform sources:
  `DUConstruct: FPGA DU with payload not yet supported` (the
  `ArtyReport voption`), and the current rebuilt Composer stops later at
  width inference for the `ValueNone` branch. The bitstream in
  `HelloArty/src/FPGA/targets` is from February's compiler. Neither failure
  involves the description; both are the FPGA lowering of `Outputs.UartReport`.
- The BAREWire record shapes are still moving (`BufferSchema.Slot` and
  `Endpoint.Signature` arrived while this description was being written,
  and both `Description.clef` files carry them). A record literal is total,
  so every new field is a compile error in every leaf that declares that
  record; until the shapes settle, the `.NET` gate (compile the description
  file verbatim against `BAREWire.fsproj`, run the three observers) is the
  cheap check to run after a BAREWire update.
