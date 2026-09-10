# CGRA integration requirements

Status: researched design note, 2026-09-10. No CGRA compiler backend, Maverick-2
payload ABI or hardware acceptance is implemented by this package.

## The execution interface

A dataflow fabric still needs defined operations, numeric behavior, memory
semantics and execution rules even when it has no sequential CPU instruction
stream. A complete part may also contain conventional instruction processors.
NextSilicon describes Maverick-2's runtime-reconfigured dataflow fabric and
embedded RISC-V cores for serial work. Its software uses runtime observations to
adapt execution. Those mechanisms need separate descriptions within one physical
part. [Vendor architecture account](https://www.nextsilicon.com/insights/elads-blog-Maverick2-launch/)

ELF identifies an object format, architecture and entry/section information.
Architecture-specific payload semantics require an additional ABI contract.
An ELF file could contain host code, control-processor code or an embedded
accelerator payload; its use does not identify the fabric programming model.
[ELF header specification](https://gabi.xinuos.com/elf/02-eheader.html)

For Maverick-2, the first integration should consume the vendor compiler/runtime
interface and let Composer orchestrate that toolchain. Implementing independent
placement and routing would require a much deeper specification. Public vendor
material describes a profiler, Chip Viewer and Projection Viewer, but the pages
reviewed here do not document a supported external IR interface, payload ABI or
loader format. Arbitrary LLVM IR compatibility must therefore remain unproven.
[NextSilicon tool overview](https://www.nextsilicon.com/tech/)

## Reported MLIR pipeline

Recorded 2026-09-10: the project maintainer reports that Elad Raz described
lifting into MLIR before lowering back to LLVM. This is a report of a direct
conversation, separate from the public sources above. It establishes a useful
integration lead; the dialects, lifting input, pass pipeline and supported
external entry point have not yet been supplied.

An internal MLIR stage does not identify the LLVM dialect as the tile-mapping
representation. MLIR's LLVM dialect preserves LLVM IR instruction semantics.
Tile topology, mapping and configuration could instead be represented by custom
operations, attributes or analyses before conversion into it. That is a possible
implementation, not a claim about NextSilicon's internals.
[LLVM dialect semantics](https://mlir.llvm.org/docs/Dialects/LLVM/)

The preferred eventual Composer boundary is a versioned MLIR input contract
that retains the structure and memory facts needed by the vendor's mapping
passes. Composer could lower directly to that boundary if NextSilicon supports
it. Otherwise, the supported vendor compiler remains the initial integration.
The architecture can accommodate both without making ELF the platform contract.

Ask for one small example before and after mapping, its dialect definitions
and verifiers, the reproducible pass pipeline, and a statement of what later
runtime adaptation may change while preserving behavior. Together with memory
and launch semantics, those artifacts would provide a practical counterpart to
the inspectable lowering used with MLIR-AIE and CIRCT. A stable static tile
placement is not required if the vendor instead specifies and exposes the
relevant adaptive-execution guarantees.

## Materials to obtain

Ask the vendor for a compiler integration/evaluation kit with the following
versioned artifacts. No request has been sent on the user's behalf.

| Material | Required contents |
| --- | --- |
| Compiler contract | Accepted source/IR/frontend interface, exact versions, numeric and memory semantics, target attributes, diagnostics and a minimal supported build; for MLIR, dialect definitions/verifiers, legal input stage and reproducible pass pipeline |
| Runtime and payload ABI | Host/control/fabric artifact roles, file/section formats, relocations where used, launch arguments, completion/errors and firmware/driver compatibility |
| Memory contract | Address domains, allocation/alignment, transfers or shared backing, visibility, synchronization, ownership and lifetime |
| Architecture description | Supported operations and representations, capacity limits, tile/interconnect constraints exposed to clients, and which choices the vendor runtime owns |
| Validation access | Simulator or evaluation system, known input/output examples and telemetry that establishes execution on the intended accelerator |

KIT's public Maverick-2 access page describes restricted evaluation access, but
its software and Hello World sections are currently unfilled. It is an access
lead rather than a compiler or binary-format specification.
[KIT system documentation](https://docs.nhr.kit.edu/clusters/ftp/nextsilicon/)

## Fidelity artifacts

The concrete deliverable would include:

- Silicon declarations for documented capabilities and limits, with unknown
  values retained as unresolved requirements rather than guessed constants.
- Product declarations for the selected card/module and revision, including
  host attachment and memory configuration.
- Environment bindings for the supported vendor runtime and its host ABI.
- BAREWire layouts and explicit address/ownership relationships at the boundary.
- A Composer adapter that supplies the vendor's accepted input, checks tool
  versions/results, and loads or launches through the supported runtime.
- A selected profile and a small acceptance workload with deterministic inputs
  and specified arithmetic semantics.

For each acceptance run, retain source and input hashes, the selected profile,
tool/firmware versions, vendor input and output artifacts, diagnostics, expected
and actual results, and telemetry showing accelerator execution. Export mapping
or resource reports where supported. Dynamic adaptation means one fixed physical
layout may not be a stable artifact; record the vendor's semantic guarantees and
the observations actually available instead.

Start with a small integer dataflow workload and compare its result with a
specified reference. Include an unsupported-operation rejection and a memory
boundary case. A correct host fallback result alone is insufficient evidence of
fabric execution. Simulator agreement, successful hardware execution and formal
proof remain distinct acceptance claims.
