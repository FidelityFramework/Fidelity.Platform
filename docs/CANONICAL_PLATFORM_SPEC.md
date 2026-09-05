<!-- Canonical Fidelity.Platform design doc.
Produced by an archaeology-first design pass (git-evidence-backed). Key claims spot-verified
against the working tree before saving. This is a DESIGN DOC TO RATIFY, not implemented code.
Source pass: platform2.js / run wf_0d5debb1-b29. -->

Fidelity.Platform — Canonical Spec (Design Doc) 

Fidelity . Platform 
Canonical spec · design doc to ratify 

01 Archaeology Verdict 
02 The Canonical Spec 
2b Developer Experience 
03 Resolution Path 
04 RA6M5 as Instance 
05 Migration Plan 
5b Vestige Kill-List 
06 Decisions to Sign Off 
07 Open Questions / Risks 

Fidelity.Platform · for architect review 

# The canonical platform spec, and the ghost it must bury

One live descriptor schema, one dead one, and a resolution path that fakes the OS it was handed. This doc ratifies the canonical structure that serves all six substrates, carries platform facts on the mechanism that actually works today, and lets the EK-RA6M5 credential fall out as a correct instance — not a special-case patch. Design to ratify, not code to run.

Driver EK-RA6M5 unikernel 
Target triple thumbv8m.main-none-eabi 
Repos Platform / Composer / clef 
Source of truth Quotation (decided) 
Status Pre-implementation 

01 
## Archaeology verdict: live, vestigial, transitional

What must be removed, not reconciled. Every verdict is git-backed and re-verified on current disk (Platform `Wayland`, Composer `CIRCT`, clef `fidelity`). The recurring cause is one signature: a prior agent that added-alongside and never removed.

LIVE — current intent, build on it 
VESTIGIAL — dead, remove it 
TRANSITIONAL — stopgap, replace it 

### The two-descriptor question, settled

The prime suspect was two coexisting `PlatformDescriptor` schemas. They are not a live design to reconcile. There is one live schema and one ghost , and the evidence is unambiguous.

Vestigial ISA/ABI descriptor — Fidelity.Platform.Types.PlatformDescriptor 
CPU/Linux/x86_64/Types.clef:108 . Fields `WordSize`, `Endianness`, `SyscallConvention`, `EntryPointABI`, `OperatingSystem` (which is the only `BareMetal`-bearing type in the tree). Born cf5fa92 (2026-01-13, pre-taxonomy) and byte-identical to birth ever since — every later touch was a pure move ( 0c3c0dd renamed the file with zero content lines changed). Consumed by nothing: grep for `SyscallConvention`, `EntryPointABI`, `Endianness`, `EntryPointKind` across Composer + clef returns empty. It compiles into the PSG and is read by no one — the strongest possible vestige signal. Its job was silently taken over by hardcoded `SyscallNumbers` + string-matching `resolveOSArch`. Kill it. Its concepts (OS, endianness, entry-point ABI, word size) are legitimately needed and are absorbed into the canonical spec — only the dead vessel dies.

Live Endpoint descriptor — Fidelity.Platform.Contracts.PlatformDescriptor 
Contracts/PlatformContracts.clef:90 . Fields `Substrate`, `Vendor`, `Family`, `Device`, `Clocks`, `Resets`, `Uarts`, `DedicatedPins`. Born a1eccbc (2026-02-18, the taxonomy commit), last real +23-line edit dd675be (2026-02-21) — actively evolving. Consumed by the live quotation-carried channel: `PlatformPinResolution` walks PSG record bindings by type-name and feeds XDC generation. This is the schema every non-CPU leaf (incl. both MCU leaves) `open`s and instantiates. Current intent. Its one gap: it carries hardware endpoints but zero ISA/ABI facts — the canonical spec must extend it to carry them.

### The resolution-path residue

The retooling twin commits cf23fef (Composer) / c98367ea1 (clef), 23 seconds apart, introduced the string-match channel and left three add-alongside casualties:

| 
| | Element | Location | Status | Why 

| 
| resolveOSArch | PlatformConfig.fs:177 | Transitional | String-matches a bare arch and hardcodes `Linux` on every arm ; folds `arm_cortex_m33` to `ARM32_Thumb`. cf23fef regressed a more OS-aware predecessor. Live-called at MLIRGeneration.fs:40 . The exact mechanism the quotation must replace. 

| 
| PlatformSection.OS / metadata.OS | FidprojLoader.fs:58,195 | Wire, don't kill | Defined-but-dead: parsed since c98367ea1 , never read in any commit (git-proven). This is the single structural break — OS is captured then dropped, forcing the downstream fake. Revive it, don't delete it. 

| 
| fromPlatformContext + empty | PlatformConfig.fs:208,163 | Dead | No external callers (verified). MLIRGeneration inlined the calls in the retooling and left the wrappers behind. But note: `fromPlatformContext` is the second caller of `resolveOSArch` — it must be deleted before resolveOSArch or the build breaks. Deletion order matters. 

| 
| FreestandingStartup.forPlatform | NativeTypes.fs:337 | Dead | String-matches `"Linux_x86_64"`, but `PlatformId` is now bare arch (`"x86_64"`, `"arm_cortex_m33"`) — returns `None` for every current input , not just MCU. The platformId format changed out from under it. A third copy of syscall/ABI facts. 

Survey corrections confirmed on disk 
Two premises in the original survey were wrong and the doc reflects the corrected truth: (1) clef has no `OperatingSystem` type in production — the `BareMetal` case the survey placed in clef actually lives in the dead Types.clef ISA/ABI descriptor. (2) The FPGA "quotation" path is not a `<@ @>` quotation at all — it is plain `let`-bound records read structurally from the PSG. This distinction drives the entire spec (see §2b).

02 
## The canonical spec

The quotation is authoritative (decided — do not re-open). This section fixes the schema, the taxonomy axes, and the one mechanism constraint that governs everything.

### The mechanism constraint drives the design

Before any type: the `<@ @>` quotation is inert at the backend today . `SemanticKind.Quote` has exactly one consumer in all of Composer — `SSAAssignment.fs:1000 → false`. No evaluator, no splicer, no `Expr`-reflection exists. And the DU-import wall is real : Contracts string-aliases `SubstrateKind = string` et al. specifically to avoid cross-assembly DU layout failures. So "quotation-centric" cannot mean an `Expr` interpreter carrying rich DUs. It is realized on the only built mechanism :

Author Quoted typed record Developer writes the descriptor value as a quotation, `<@ { ... } @>` (the spec's form, clef-lang-spec platform-bindings.md "Platform Descriptor"), or plainly; the quotation is kept because it carries the record's type over facts that arrive stringly typed or untyped (vendor documents, board packs, project files). Decided 2026-09-04. 
CCS → PSG Elaborated to Binding clef lowers the `let` to a typed `RecordExpr` node that survives into the flattened graph, inside a `Quote` node when quoted; the reader follows the quotation to the record and never evaluates it. 
Backend Structural extraction Read by field-name string, exactly as `PlatformPinResolution` already does for FPGA pins. 

This is quotation- centric authoring (the descriptor is the single authored source of truth) realized over the live carrier. It is buildable now, blocked on no interpreter. Everything downstream follows from this call.

### Schema: extend the live descriptor with a TargetCore block

Keep the live `Contracts.PlatformDescriptor` (endpoints). Add a `TargetCore` block carrying the ISA/ABI facts the endpoint schema lacks — string-aliased throughout so it clears the DU-import wall. Every cross-substrate field is optional/defaulted so no substrate pays for another's fields (this is the schema-shape rule that keeps all six coherent).

Contracts/PlatformContracts.clef canonical · string-aliased, DU-wall-safe 
` // String aliases stay — they are the live constraint, not a smell. 
type SubstrateKind = string // "CPU" | "MCU" | "FPGA" | "GPU" | "NPU" | "CGRA" 
type OsName = string // "linux" | "windows" | "macos" | "freebsd" | "none" 
type ArchName = string // "x86_64" | "arm_cortex_m33" | "aarch64" | ... 
type Endianness = string // "little" | "big" 
type RuntimeName = string // "libc" | "freestanding" | "bare" | "rocm" | "xdna" 

// NEW: the ISA/ABI facts the endpoint schema never carried. 
// This is what the dead Types.clef descriptor held — reborn as a live, carried field. 
type TargetCore = {
Os: OsName 
Arch: ArchName 
WordSizeBits: int 
Endianness: Endianness 
Runtime: RuntimeName 
// Optional overrides — framework derives when None (see §3). 
TripleOverride: string option // force a triple; else derived from (Os,Arch,Runtime) 
CpuModel: string option // -mcpu, e.g. "cortex-m33"; distinguishes M33 vs M55 
}

type PlatformDescriptor = {
Id: string ; DisplayName: string 
Substrate: SubstrateKind 
Vendor: string ; Family: string ; Device: string 
Package: string ; SpeedGrade: string 
// ISA/ABI half — None for pure-hardware leaves (FPGA has no core to describe). 
Core: TargetCore option
// Endpoint half — ALL default to [] so no leaf must restate another's fields. 
Clocks: ClockEndpoint list
Resets: ResetEndpoint list // FIX: was required; must default [] (see kill-list) 
Groups: PinGroup list
Uarts: UartEndpoint list
DedicatedPins: PinEndpoint list
Notes: string list
}` 

Rung-0 schema fix, upstream of everything 
`Resets` is required today with no default, and only the FPGA leaf sets it . That means four of six leaf descriptors (RA6M5, STM32, GPU, NPU) do not compile right now — verified: RA6M5's Platform.clef omits `Resets`. Making cross-substrate fields optional/defaulted (`Resets = []`, `Core = None`) is the precondition to any MCU compile. It sits above all the triple work.

### Taxonomy: substrate-dependent axes (ratify the committed trajectory)

Axes are substrate-dependent, not a uniform Substrate/Vendor/Family/Device . This is not a guess — commit 0c3c0dd ("x86_64 shift") deliberately deleted the `CPU/Linux/X86_64/StrixHalo` device-axis leaf and reshaped CPU to `Substrate/OS/ISA` with no device axis. That is the architect's last structural act. It is also what the resolver independently wants: CPU/MCU resolve to a triple (an ISA/ABI fact); FPGA resolves to a device part → pin map (a taxonomy-location fact). Different key-spaces, different axes.

| 
| | Substrate | Folder axes | Example leaf | `[platform]` tuple carries 

| | CPU | Substrate / OS / ISA | CPU/Linux/x86_64 | os, arch, word_size, runtime_model 

| | MCU | Substrate / Vendor / Family / Device | MCU/Renesas/RA6M5/EK_RA6M5 | os(=none), arch, word_size, runtime_model 

| | FPGA | Substrate / Vendor / Family / Device | FPGA/Xilinx/Artix7/ArtyA7_100T | substrate, vendor, family, device, clock_mhz… 

| | GPU | Substrate / Vendor / Family / Device | GPU/AMD/RDNA3_5/StrixHalo_iGPU | arch, word_size, runtime_model 

| | NPU | Substrate / Vendor / Family / Device | NPU/AMD/XDNA2/StrixHalo_NPU | arch, runtime_model 

| | Profiles | Substrate / Composite | Profiles/StrixHalo_ArtyLab | none — composes other leaves (see §7) 

The MCU is the hybrid : it needs the ISA/ABI tuple (for the triple) and the vendor/device path (for pins/peripherals). That is precisely why it "falls out" only once the descriptor carries both halves. The `[platform]` tuple is the minimal bootstrap (what's needed to locate/load the descriptor and build a `PlatformContext` before the descriptor is elaborated); the descriptor record is the authoritative body . Where they overlap on MCU, the descriptor is source-of-truth and a check should flag divergence rather than silently prefer one.

2b 
## The developer experience

The crux the architect named. A quotation-centric spec easily becomes ceremony — a dev hand-writing a giant descriptor. It must not. The move is ordinary F# record-update-with-holes: the developer states intent; the framework synthesizes the rest.

### What the developer writes vs. what the framework carries

The developer writes their declaration of intent — the board's identity and the few facts unique to it. The framework fills defaults, derives the triple, and (later) sources endpoint maps from board docs. Here is the EK-RA6M5, authored, against the fully-resolved descriptor the framework produces from it:

Authored by the developer ~14 lines · intent 
` namespace Fidelity.Platform.MCU
.Renesas.RA6M5.EK_RA6M5
open Fidelity.Platform.Contracts

module Platform =
let descriptor =
{ Renesas.Ra6m5 .defaults with 
Device = "EK-RA6M5" 
Package = "Board" 
// four ADC channels, the only 
// board-unique wiring: 
Groups = [ Adc .ch0 "A3" Unit0 
Adc .ch1 "A4" Unit0 
Adc .ch2 "A0" Unit1 
Adc .ch3 "A1" Unit1 ] }` 

Resolved by the framework ~40 fields · synthesized 
`{ Id = "mcu-renesas-ra6m5-ek-ra6m5" 
Substrate = "MCU" ; Vendor = "Renesas" 
Family = "RA6M5" ; Device = "EK-RA6M5" 
Core = Some { // from family 
Os = "none" ; Arch = "arm_cortex_m33" 
WordSizeBits = 32
Endianness = "little" 
Runtime = "bare" 
TripleOverride =
Some "thumbv8m.main-none-eabi" // derived 
CpuModel = Some "cortex-m33" } // derived 
Clocks = [ /* family PLL tree */ ]
Resets = [ /* family reset */ ]
Groups = [ /* 4 authored + family GPIO */ ]
Uarts = [ /* family default */ ] … } ` 

The ergonomic move is that this is ordinary F# — a record-update expression against a family default, with holes for what's board-unique. No macro, no quotation ceremony, no restating the ISA. It stays their declaration because `Renesas.Ra6m5.defaults` is just another value they can read, override, or ignore.

### Composition: board inherits from family inherits from vendor

Inheritance is F# record spreads, three layers deep, nothing restated at any layer:

Vendor → Family → Board plain record-update, no framework magic 
` // VENDOR — the ISA/ABI floor shared by all Renesas RA parts 
module Renesas =
let raCore = { Os = "none" ; Endianness = "little" ; Runtime = "bare" 
TripleOverride = None ; CpuModel = None } // derived downstream 

// FAMILY — RA6M5 pins the core to Cortex-M33/32-bit, adds the clock/reset tree 
module Renesas.Ra6m5 =
let defaults : PlatformDescriptor =
{ emptyDescriptor with 
Vendor = "Renesas" ; Family = "RA6M5" 
Core = Some { Renesas .raCore with Arch = "arm_cortex_m33" ; WordSizeBits = 32 }
Clocks = raClockTree; Resets = raReset } // endpoints default [] otherwise 

// BOARD — EK_RA6M5 only names Device + the four board-unique ADC channels (above) ` 

A new RA6M5 board restates nothing but its device name and its unique wiring. A new RA-family part restates only its arch/clock differences. The vendor floor is authored once. This is the "board-inherits-from-family-inherits-from-vendor" the crux asked for, with zero ceremony.

### Is quotation-carrying LIVE or does it need Composer work?

Honest answer, split by half of the descriptor:

| 
| | Descriptor half | Carrier | Status today 

| 
| Endpoints (pins/clocks/uarts) | PSG record binding → structural field-name extraction | Live — `PlatformPinResolution` does exactly this for FPGA today. Generalize the guard beyond `TargetPlatform.FPGA`. 

| 
| ISA/ABI (`Core`: os/arch/word/runtime) | `PlatformContext` scalars, bootstrapped from `[platform]` tuple | Needs Composer work — buildable now via §3's four changes, but as scoped the ISA/ABI facts still flow from the tuple , not yet read out of the descriptor record. See the seam note below. 

| 
| `<@ @>` Expr quotation evaluation | a real `Expr` interpreter / splicer | Unbuilt & walled — `SemanticKind.Quote` is a no-op passthrough; DU-import wall blocks rich typed carrying. The design does not build on this. 

The residual seam, stated plainly 
For CPU/MCU, the authoritative source today is still the `[platform]` tuple, because `buildPlatformContext` reads `PlatformMetadata`, not the descriptor record. So "the descriptor is single source of truth" is true for the endpoint half (live, FPGA-proven) and aspirational for the ISA/ABI half until a descriptor-elaboration pass reads `Core` from the PSG the way pins are read. RA6M5 works anyway because the tuple carries enough. Closing this seam (elaborate `Core` structurally, mirror-check it against the tuple) is the one piece of genuinely new Composer work the full quotation-centric vision needs beyond §3 — and it is small, because the FPGA path already proves the extraction pattern.

03 
## The canonical resolution path

Carried facts into (OS, arch, word-size, runtime-mode, triple, endpoints), replacing the string-match stopgap. Four threaded changes plus the type cases that make the facts representable. Composes with the uncommitted Codegen.fs Phase-0 fix, unchanged.

.fidproj [platform] runtime_model, os, arch, word_size — already parsed, os included 
clef context + TargetOS field stop dropping metadata.OS — wire it through 
Composer resolveTarget reads carried OS — no hardcoded Linux, no mis-fold 
Composer toTriple (OS, Arch, Runtime) → LLVM triple — the missing map 
Codegen Phase-0 plumbing triple already reaches llc / clang (uncommitted fix) 

#### Change 1 — type cases (fill gaps; replaces nothing)

Alex/Dialects/Core/Types.fs . `OSFamily` gains `| None_` (bare-metal / no-OS). `Architecture` gains `| ARMv8M_Main` (Cortex-M33/M55 → thumbv8m.main) — distinct from `ARM32_Thumb` (armv7). `platformWordWidth` / `platformWordType` map `ARMv8M_Main → IntWidth 32`. Verified blast radius: 2 non-wildcard match sites for the arch case, 0 for the OS case (SyscallNumbers already has `| _ -> failwithf`). Both new DUs live inside Composer — they never cross the import wall.

#### Change 2 — carry OS through clef (wire the defined-but-dead field)

NativeTypes.fs:391 — `PlatformContext` gains `TargetOS: string option` (string, not a DU: matches how `PlatformId`/`Arch` already travel, and honors the DU-import wall). ProjectChecker.fs — in both context-building branches set `TargetOS = metadata.OS`. This is the single wire that revives the parsed-but-dropped `os`. Breaks 4 record constructors atomically (all in the same two files).

REPLACE: the stopgap resolveOSArch — delete 
` // PlatformConfig.fs:177 — hardcodes Linux, 
// mis-folds Cortex-M to armv7 Thumb 
let resolveOSArch ctx =
match ctx.PlatformId with 
| id when id.Contains "x86_64" ->
( Linux , X86_64 )
| id when id.Contains
"arm_cortex_m33" ->
( Linux , ARM32_Thumb ) // WRONG×2 
| _ -> ( Linux , X86_64 ) // silent fake ` 

WITH: reads carried facts resolveTarget — new 
` // gated to SubstrateKind ∈ {CPU, MCU} 
let resolveTarget ctx =
let os =
match ctx.TargetOS with 
| Some "linux" -> Linux 
| Some "none" -> None_ // now possible 
| Some o -> parseOs o
| None -> Linux // only when absent 
let arch =
match ctx.PlatformId with 
| "x86_64" -> X86_64 
| "arm_cortex_m33" 
| "arm_cortex_m55" -> ARMv8M_Main 
| other -> parseArch other
(os, arch)` 

Two substantive corrections vs. the stopgap: OS is read, not hardcoded ; `arm_cortex_m33` maps to the v8-M arch, not the wrong `(Linux, ARM32_Thumb)`. This restores the OS discrimination the retooling regressed, sourced from the carried fact.

Mandatory gate — do not skip 
`resolveTarget` / `toTriple` must be gated to `SubstrateKind ∈ {CPU, MCU}` . GPU carries `arch="rdna3_5"`, NPU carries `arch="xdna2"` — neither is an LLVM-triple arch, and both compile via other backends (ROCm/AIE). A strict `failwithf`-on-unknown resolver would regress GPU/NPU from silently-compiling to hard-failing . The gate keeps them out of the CPU triple path entirely. The old `resolveOSArch` hid this behind its `_ -> (Linux, X86_64)` fallthrough; the canonical path makes the gate explicit instead.

#### Change 3 — the missing triple derivation (adds; CLI override preserved)

New BackEnd/LLVM/TripleResolution.fs . This is the gap that does not exist at all today — the triple currently comes only from CLI `--target`.

BackEnd/LLVM/TripleResolution.fs new · fills the UNBUILT gap 
` let toTriple (os: OSFamily ) (arch: Architecture ) (rt: RuntimeMode ) : string =
match os, arch with 
| None_ , ARMv8M_Main -> "thumbv8m.main-none-eabi" // RA6M5 
| None_ , ARM32_Thumb -> "thumbv7m-none-eabi" 
| None_ , RISCV32 -> "riscv32-unknown-none-elf" 
| Linux , X86_64 -> "x86_64-unknown-linux-gnu" 
| Linux , ARM64 -> "aarch64-unknown-linux-gnu" 
| MacOS , ARM64 -> "aarch64-apple-darwin" 
| /* … */ -> failwithf "no triple for %A/%A" os arch

// -mcpu carries the CPU model — NOT the DU case. This is where M33 vs M55 
// stays distinct even though both share the ARMv8M_Main arch. Sourced from Core.CpuModel. 
let mcpuFor (core: TargetCore option) : string option =
core |> Option.bind (fun c -> c.CpuModel) // e.g. Some "cortex-m33" ` 

Injection at CompilationOrchestrator.fs:192 : `TargetTripleOverride = options.TargetTriple |> Option.orElse derivedTriple`. CLI `--target` still wins by `orElse` ordering; the derivation replaces "host-only" as the default. The `-mcpu` hook is exactly the one the Codegen comment reserved ("layered in later once the platform tuple carries it") — now it does.

#### Change 4 — keep `resolveRuntimeMode` (the sound half)

PlatformConfig.fs:189 (`Bare/Freestanding → Freestanding`, `Libc → Console`) is live and correct — build on it unchanged.

Composes With the uncommitted Codegen.fs Phase-0 fix 
`Codegen.fs:39` computes `isHostTarget = (targetTriple = getDefaultTarget())`. The resolver produces the triple the Phase-0 fix routes : RA6M5's `thumbv8m.main-none-eabi` ≠ host → `isHostTarget=false` → llc gets `-mtriple=`, clang gets `-target`. The hosted CPU path (triple == host) stays byte-identical. They compose exactly, no edits to the fix.

04 
## RA6M5 as a correct instance

`os=none / arch=arm_cortex_m33 / word=32 / bare` resolving end-to-end to `thumbv8m.main-none-eabi` + `cortex-m33` + freestanding. No special-casing — every stage is the same one every substrate uses; only the tuple axes and the `toTriple` arm differ.

MCU/Renesas/RA6M5/EK_RA6M5/Fidelity.Platform.fidproj input · verified on disk 
` [compilation] target = "mcu" 
[platform] runtime_model = "bare" os = "none" arch = "arm_cortex_m33" word_size = 32` 

| 
| | Stage | File / function | Before (broken) | After (canonical) 

| | Parse tuple | FidprojLoader.fs:190 | all 4 parsed; `OS="none"` captured | unchanged — already correct 

| | Build context | ProjectChecker.fs:46 | PlatformId, word→32 dims, Bare — OS dropped | + `TargetOS = Some "none"` 

| | Resolve OS/arch | resolveTarget | `(Linux, ARM32_Thumb)` — both wrong | `(None_, ARMv8M_Main)` — corrected 

| | Word type | platformWordType | — | `ARMv8M_Main → IntWidth 32` 

| | Runtime mode | resolveRuntimeMode:196 | `Bare → Freestanding` | unchanged — correct (bare startup, not Linux `_start`) 

| | Syscalls | SyscallNumbers | would fail if reached | bare path emits none; guarded by runtime mode 

| | Triple | toTriple | did not exist → host `x86_64…linux` | `(None_, ARMv8M_Main) →` thumbv8m.main-none-eabi 

| | -mcpu | mcpuFor | — | `Some "cortex-m33"` 

| | Inject | CompilationOrchestrator:192 | CLI-only (host default) | `CLI |> orElse derived` (CLI still overrides) 

| | Route to tools | Codegen.fs:39 (Phase-0) | host-identical, no cross triple | `isHostTarget=false` → llc `-mtriple`, clang `-target` 

Result RA6M5 falls out as a correct instance, not a patch 
OS honored (not faked Linux), arch corrected (v8-M, not armv7 Thumb), triple derived from the descriptor/tuple (not CLI-only), riding the already-made Codegen fix. Its endpoint descriptor lives at `MCU/Renesas/RA6M5/EK_RA6M5/Platform.clef` — today a scaffold with empty endpoint maps (and, until the rung-0 fix, non-compiling because it omits `Resets`). Once `Resets = []` defaults and the four changes land, the leaf resolves correctly with zero RA6M5-specific code in Composer.

Honestly deferred: targeting vs. bootability 
The canonical path makes RA6M5 correctly targeted — the right triple reaches the tools. Two Phase-1 items remain, named and out of the resolver's scope: (i) the bare-metal link needs `lld` + a linker script in Codegen (the file's own comment says so); (ii) `FreestandingStartup` must be re-keyed off `(TargetOS, Architecture)` to select a real Cortex-M reset vector instead of returning `None`. The resolver makes the facts available for both; neither blocks the triple resolving correctly. The credential targets correctly now; it boots after Phase 1.

05 
## Migration plan

Safe ordered steps. The working CPU sample stays green at every rung — verified: it routes through the metadata branch with `os="linux"`, and its resolved triple equals host, so it lands on the byte-identical hosted path. Buildable-now steps are separated from the one that needs new Composer work.

| 
| | # | Step | Touches | Keeps CPU green because… | Kind 

| | 0a | Default `Resets = []`, `Core = None` in the descriptor type; make cross-substrate fields optional | Contracts/PlatformContracts.clef | CPU descriptor unaffected; unblocks the 4 non-compiling leaves | Now 

| | 0b | Split socket structs (`SockAddrIn` etc.) out of `Types.clef` into a `Sockets` module | CPU/Linux/x86_64/ | De-risks the descriptor kill; sockets aren't in the ISA/ABI descriptor | Now 

| | 1 | Add `OSFamily.None_`, `Architecture.ARMv8M_Main` + word-width arms | Alex/Dialects/Core/Types.fs | Additive DU cases; CPU matches `X86_64`/`Linux` unchanged | Now 

| | 2 | Add `PlatformContext.TargetOS`; wire `metadata.OS` in both branches | NativeTypes.fs, ProjectChecker.fs | CPU gets `TargetOS = Some "linux"` → resolves to same `Linux` | Now 

| | 3 | Delete `fromPlatformContext` + `empty` FIRST (the dead 2nd caller of resolveOSArch) | PlatformConfig.fs | No live callers; removing the 2nd caller unblocks step 4 | Now 

| | 4 | Replace `resolveOSArch` with gated `resolveTarget`; update the `MLIRGeneration.fs:40` call | PlatformConfig.fs, MLIRGeneration.fs | CPU: `(Linux, X86_64)` identical to before; gate excludes GPU/NPU | Now 

| | 5 | Add `TripleResolution.toTriple` + `mcpuFor`; inject with `orElse` at orchestrator | new TripleResolution.fs, CompilationOrchestrator.fs | CPU triple == host → `isHostTarget=true` → byte-identical | Now 

| | 6 | Remove the dead ISA/ABI descriptor + quotation (kill-list §5b) | Types.clef, Platform.clef, fidproj | Consumed by nothing; sockets already extracted in 0b | Now 

| | 7 | Elaborate `Core` from the descriptor PSG (close the seam); generalize pin-resolution guard beyond FPGA | PlatformPinResolution.fs, new pass | CPU facts still valid whether from tuple or descriptor (mirror-checked) | Composer work 

| | P1 | Bare-metal link (`lld` + linker script); re-key `FreestandingStartup` to `(TargetOS,Arch)` | Codegen.fs, NativeTypes.fs | Only affects bare targets; CPU is libc/Console | Phase 1 

Buildable now: steps 0a–6 — scalar facts on `PlatformContext` plus two small functions, zero reliance on the inert `Quote` evaluator or a cross-assembly DU. RA6M5 targets correctly at the end of step 6. Needs Composer work: step 7 closes the "descriptor drives ISA/ABI" seam (small, patterned on the live FPGA extraction). Phase 1 is bootability, deliberately deferred.

5b 
## The vestige-removal kill-list

Architect instruction: vestiges FULLY REMOVED, not left dormant, so they cannot inspire drift. This is decisive, not tentative — each entry gives the symbol, the git evidence it is dead, what must land first, and the deletion order. Every removal is confirmed safe: nothing live depends on it.

Delete outright 
Replace / re-key (no "or") 
Wire, do not delete 

| 
| | # | Symbol / file | Git evidence it is vestigial | Precondition (land first) | Action 

| 
| K1 | Types.clef — ISA/ABI PlatformDescriptor + supporting DUs (Endianness, SyscallConvention, EntryPointABI, OperatingSystem…) | Byte-identical since birth cf5fa92 ; consumed by nothing (grep empty across Composer+clef); role taken by SyscallNumbers + resolveOSArch | 0b (split sockets out); Core facts absorbed into the canonical `Core` block | Delete 

| 
| K2 | Platform.clef — platform: Expr<PlatformDescriptor> quotation, typeLayouts, syscallConvention, memoryRegions | The dead CPU quotation; zero reads repo-wide; `Quote` is a no-op passthrough | K1 (same schema); remove from CPU fidproj `Compile` list | Delete 

| 
| K3 | PlatformConfig.fs — resolveOSArch | Regressed predecessor in cf23fef ; hardcodes Linux; mis-folds Cortex-M | K4 must delete first (removes the 2nd caller); then rewire MLIRGeneration:40 to `resolveTarget` | Replace 

| 
| K4 | PlatformConfig.fs — fromPlatformContext + empty | No external callers (verified); inlined away in retooling, wrappers left behind | none — this is the first deletion , it unblocks K3 | Delete 

| 
| K5 | NativeTypes.fs — FreestandingStartup.defaultLinux_x86_64 / forPlatform string-match | Matches `"Linux_x86_64"` but PlatformId is now bare arch → `None` for all inputs; third copy of syscall/ABI facts | Re-key off `(TargetOS, Architecture)` in Phase 1 — commit to re-key OR delete, no "or" | Re-key (P1) 

| 
| K6 | PLATFORM_STRUCTURE.md / README.md — CPU/Linux/X86_64/StrixHalo leaf, legacy Linux_x86_64 package refs, CPU/<OS>/<ISA>/<Device> axis | Docs froze at the a1eccbc pre-shift state; the StrixHalo leaf was deleted in 0c3c0dd ; the Linux_x86_64 folder is gone from the tree | none — doc-only | Rewrite 

| 
| K7 | CPU/Linux/x86_64/Fidelity.Platform.fidproj — name = "Fidelity.Platform.Linux_x86_64" | Legacy flat-package name survived the folder move in 0c3c0dd ; every other leaf uses the taxonomy convention | none — rename to `Fidelity.Platform.CPU.Linux.x86_64` | Rename 

| 
| K8 | FidprojLoader.fs — PlatformSection.OS / metadata.OS | Defined-but-dead: parsed since c98367ea1 , never read (git-proven) | Change 2 wires it — do NOT delete | Wire 

Two corrections that change the kill-list 
Deletion order is load-bearing: K4 (dead wrappers) must be deleted before K3 (resolveOSArch), because `fromPlatformContext` is the second caller of `resolveOSArch` — delete K3 first and the build breaks. Socket retention reason inverted: the original archaeology said "retain sockets because `WebSocket/Server.clef` uses them," but that file is not in any fidproj sources list and `SockAddrIn` is used nowhere compiled. The socket split (0b) is de-risking on intent , not required for a green build — but do it anyway so `Fidelity.Platform.Types` can be removed wholesale in K1.

06 
## Decisions for the architect

Source-of-truth is decided (quotation-centric) — not re-opened here. These are the calls this pass makes that need your sign-off, each with its tradeoff.

D1 Taxonomy axes are substrate-dependent 
CPU by `Substrate/OS/ISA` (no device axis); MCU/FPGA/GPU/NPU by `Substrate/Vendor/Family/Device`. Tradeoff: gives up a single uniform folder shape, but matches the two genuine key-spaces (triple vs. pin-map) and ratifies your own last structural act ( 0c3c0dd removed the CPU device axis on purpose). The alternative — forcing CPU back under Vendor/Device — re-introduces exactly what you deleted.

Recommend: RATIFY. It is the last committed intent and what the resolver independently demands. 

D2 One descriptor schema, extended — not two reconciled 
Keep the live `Contracts.PlatformDescriptor`, add an optional `Core` (TargetCore) block for ISA/ABI; delete the dead `Types.PlatformDescriptor` entirely. Tradeoff: the endpoint schema wasn't designed to carry ISA/ABI facts, so `Core` is a graft — but a clean one (optional, string-aliased) that closes the exact gap the dead schema was the only thing filling.

Recommend: RATIFY. The alternative (revive the ISA/ABI schema) canonicalizes a five-week-old ghost. 

D3 TargetOS travels as `string option`, not a DU 
Honor the DU-import wall: OS/arch/runtime cross the assembly boundary as closed-vocabulary strings, parsed to rich DUs only inside Composer. Tradeoff: loses compile-time exhaustiveness at the boundary (a typo'd `os` is a runtime `failwithf`, not a type error). Mitigated by parse-time validation against the closed vocabulary.

Recommend: RATIFY as the lower-risk canonical choice. The alternative (fix the DU-import wall first) is a larger Composer project that blocks the MCU on unrelated work. 

D4 The `<@ @>` syntax is the authoring surface and carries the facts by structure (decided 2026-09-04) 
The descriptor is authored as a quotation; the compiler reads the typed record inside it structurally, by type name and field name (clef `PSGSaturation/SemanticGraph/PlatformResolution.fs` follows the `Quote` node to the record), and never evaluates it. No `Expr` evaluator is on the fact-carrying path, and none is needed for it; the DU-import wall is honoured by the closed-vocabulary strings above. A plain record is read the same way, so the leaves as written today are read without rewriting; the quotation is the form of record because it keeps type-carrying information over sources that are stringly typed or not typed at all.

Sign-off received (2026-09-04): "I DEFINITELY want to keep Don Syme's quotations for Fidelity.Platform - it's a way to keep type-carrying information from sources that may be stringly typed or not typed at all." The Expr-evaluator question is closed: reading is not evaluating. 

07 
## Open questions & risks

Honest residuals, including what the archaeology could not fully resolve.

- The `Profiles/*` composite is unmodeled. `Profiles/StrixHalo_ArtyLab` is `target="cpu"` with no `[platform]` block — it composes GPU + NPU + FPGA leaves via dependencies. It hits the `| None, None -> None` arm and gets no `PlatformContext`, so it never reaches the triple path (doesn't crash). But the taxonomy's "every leaf carries facts" story has this exception. Open: do Profiles inherit the host triple (orchestration-only), or do they need a composite descriptor? Recommend orchestration-only for now; name it in the taxonomy so it isn't rediscovered as a bug. 

- The ISA/ABI seam (step 7) is real Composer work, not yet buildable-now. Until `Core` is elaborated structurally from the descriptor, CPU/MCU ISA/ABI facts flow from the `[platform]` tuple. The design is honest that "descriptor is single source of truth" is live for pins, deferred for ISA/ABI. Risk: if step 7 slips, the tuple and descriptor can silently disagree on MCU — mitigated by the mirror-check, but the check must actually be written. 

- M33 vs M55 rides on `-mcpu`, not the DU. `ARMv8M_Main` folds both; only `Core.CpuModel` keeps them distinct. If a leaf omits `CpuModel`, an M55 silently gets M33 codegen. Mitigation: make `CpuModel` required whenever `Arch` is a folded family, or derive a default per arch. Needs a small policy decision. 

- Endianness is carried but unused so far. `TargetCore.Endianness` is in the schema for completeness (the dead descriptor had it) but no current target is big-endian and nothing reads it yet. Kept as a first-class field to avoid re-adding it later; flagged so it isn't mistaken for live wiring. 

- Archaeology could not fully rule out worktree drift. The `/.claude/worktrees/hardcore-knuth/` mirror carries an in-progress `MCU/Renesas/RA6E2/FPB-RA6E2` board not on `main`. This doc is scoped to `main`; if that board is intended to land, it is a second MCU instance that should validate the "falls out as an instance" claim — a good future test, not a current blocker. 

Where to save this 
Recommend `Fidelity.Platform/docs/CANONICAL_PLATFORM_SPEC.md` (the `docs/` directory does not exist yet on the `Wayland` branch — create it). Pair it with the doc-drift rewrites of `PLATFORM_STRUCTURE.md` and `README.md` (kill-list K6) so the canonical structure and its documentation land together, rather than leaving the same doc-lag that produced the drift this pass is cleaning up.

Fidelity.Platform canonical spec · design to ratify, not code to run

Verified read-only: Platform@Wayland · Composer@CIRCT · clef@fidelity · all verdicts git-backed

Key commits: cf5fa92 (ISA/ABI birth) · a1eccbc (taxonomy birth) · 0c3c0dd (x86_64 shift) · cf23fef/c98367ea1 (retooling) · dd675be (last endpoint edit)
