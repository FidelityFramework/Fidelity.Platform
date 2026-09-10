# Admission contracts and accelerator handoffs

Implemented reference structure, 2026-09-10. Strix Halo plus Arty A7 is the
physical reference for ThreeBody. Apple Silicon/Metal is a second realization
of the same host/GPU/sidecar boundaries. Windows supplies a distinct eBPF host.

## What exists

- [Admission contracts and checks](../Contracts/Admission.clef): instruction
  machines, host requirements, profile facts, budget accounting and report
  consistency checks. [Manifest](../Contracts/Fidelity.Platform.Admission.fidproj).
- [Handoff contracts and checks](../Contracts/Sidecar.clef): producer/consumer
  boundaries using existing BAREWire `BufferSchema` and `Transport` records.
- [eBPF](../AbstractMachines/eBPF/Machine.clef) and
  [cBPF](../AbstractMachines/cBPF/Machine.clef) instruction-width descriptions.
- Reference host subsets for [Linux eBPF](../Environments/Linux/eBPF/Contract.clef),
  [Windows eBPF](../Environments/Windows/eBPF/Contract.clef), and
  [macOS classic BPF](../Environments/macOS/BPF/Contract.clef).
- Shared-memory semantics for [HIP/ROCm](../Environments/Linux/x86_64/ROCm/MemoryContract.clef)
  and [Metal](../Environments/macOS/Metal/Contract.clef).
- Independent [BPF reference selections](../Profiles/BPF_Reference/Profile.clef),
  [Strix Halo/Arty handoffs](../Profiles/StrixHalo_ArtyLab/Handoffs.clef) and
  [Apple Silicon handoffs](../Profiles/AppleSilicon_Metal_FPGA_Reference/Handoffs.clef).

These are source packages and an executable reference checker, not a BPF
backend, native loader, bytecode verifier, GPU allocator or working network
protocol. CCS checks their source closures. Composer does not yet extract these
admission records into its compilation pipeline. The reference profiles have
no `[platform] description` and cannot substitute for a deployable selection.

`Check.run` checks the consistency of supplied reports. It does not authenticate
evidence strings, compute content digests, validate proofs or establish that the
obligation list covers a program. An empty finding list means only that the
supplied records passed these checks. There is deliberately no `Admitted` result.

## Ownership and narrowing

| Owner | Facts |
| --- | --- |
| `Contracts/` | Shared admission and handoff vocabulary; no assumed register width, OS, or global budget |
| `AbstractMachines/` | Instruction encoding and arithmetic widths, independently of a host ABI |
| `Hardware/Silicon/` | CPU/GPU/NPU/FPGA identities and capabilities |
| `Hardware/Products/` | Concrete assembly, installed memory, NIC, physical links and board revision |
| `Environments/` | Hook/context/helper contracts, admission gates, bindings and visibility rules |
| `Protocols/` | Future sidecar wire format, negotiation, sequencing and failure policy |
| `Profiles/` | Selected participants, exact host facts, permitted subset and workload budgets |

OS BPF contracts sit directly under their environment because they are not
native CPU ABI packages. Architecture-specific loader structures still belong
under the appropriate architecture. eBPF's 64-bit registers do not establish
host pointers, map record layout or a Windows calling convention. Its 64-bit
instruction slot does not mean every instruction occupies one slot.

BAREWire already owns spaces, buffers, surfaces, endpoints, limits and
`Since`/`Until` availability metadata. No duplicate layout or version vocabulary
is introduced here. Version intervals are useful initial filters; backports,
kernel configuration, privilege, program type and installed extensions require
explicit facts before a capability is granted. These reference subsets expose
no helpers or maps yet; absent features cannot be used by a report.

## Modeling the verifier at design time

Maintain separate obligations and quantities:

| Quantity | Accounting scope | Appropriate evidence |
| --- | --- | --- |
| Stack usage | Frames on each reachable call chain, including backend spills and applicable tail-call constraints | Final layout and target-specific call analysis |
| Program size | Emitted instruction slots, including multi-slot instructions | Final artifact inspection |
| Verifier work | Target-specific state exploration or instruction visits | Heuristic during editing; recorded actual verifier work where exposed |
| Execution work | Instructions and helper calls on a bounded invocation path | Proven path bounds with helper costs kept separate |
| Elapsed time | Named stage or complete transaction | Measurements under stated conditions, or a justified worst-case timing model |
| Storage and traffic | Live buffers, map instances, payload plus framing, queue depth | BAREWire layout and ownership/lifetime analysis |

A bound on verifier work is not a bound on executed instructions. Neither is a
deadline. Linux host scheduling, JIT output, cache state, GPU queues, driver
behavior, network loss and FPGA clock-domain crossings remain separate inputs.
For ThreeBody, a timing model needs CPU preparation, GPU completion, transmit
queueing, wire time, FPGA service, return transfer and host consumption. A
percentile from a benchmark remains a measurement; it cannot discharge a hard
deadline. FPGA synthesis/implementation results and host measurements should be
reported beside the estimates that preceded them.

The reference checker matches metric, unit and scope. Missing budgets, facts or
obligations stay pending. Estimates and measurements remain advisory and cannot
discharge hard budgets. Negative or ambiguous evidence, stale profile/artifact
identity and features outside the selected subset are findings.

The intended compiler path is:

1. Resolve the instruction machine, exact environment and workload subset.
2. Derive source-located obligations from typed Clef and BAREWire declarations.
   Existing `ClefPredicate` expressions remain the source requirement mechanism;
   the strings in these reference reports are evidence references, not a second
   predicate language or executable proof callbacks.
3. Preserve deferred obligations while editing. Discharge bounds, provenance,
   initialization, call permissions and resource obligations before emission.
4. Recheck the final artifact after lowering, register allocation, relocations
   and any native conversion that changes what the host will admit.
5. Run the actual target gate on the pinned host. Record artifact digest,
   resolved-profile digest, tool/verifier identity, verdict and diagnostic log.
   Verification, loading, attachment and successful operation are separate results.

Safety alone does not guarantee verifier recognition within its analysis budget.
The agreement goal is restricted to a supported subset and pinned gate. A kernel
acceptance result does not prove payload preservation or numerical precision.
This reference implementation does not perform steps 2–5.

## OS distinctions

Linux's verifier tracks initialization, scalar ranges and pointer provenance;
the program type supplies its context and allowed operations. Exact acceptance
depends on the target implementation. The general documentation contains some
historical descriptions, so its prose alone is insufficient to pin modern loop
or call rules. [Linux verifier](https://docs.kernel.org/bpf/verifier.html),
[instruction specification](https://docs.kernel.org/bpf/standardization/instruction-set.html).

Windows uses its own eBPF and XDP packages. Current Microsoft guidance describes
native signed-driver artifacts; pin the packages, extension interfaces, conversion
tools and signing policy. Running PREVAIL alone is not Windows driver loading or
attachment. A future portable source subset needs separate output/ABI/gate checks
for each host. [Microsoft integration guide](https://github.com/microsoft/xdp-for-windows/blob/main/docs/ebpf.md).

macOS classic BPF installs instruction arrays and returns matching capture
records; writing the descriptor sends frames. Its return convention is capture
length, not an XDP action. Packet loads also have execution-time bounds checks.
The inspected XNU source declares a 512-instruction filter limit and 16 scratch
words; these are source observations to verify against a selected build, not
defaults assigned to every BPF host. [BPF API](https://github.com/apple-oss-distributions/xnu/blob/main/bsd/man/man4/bpf.4),
[validator](https://github.com/apple-oss-distributions/xnu/blob/main/bsd/net/bpf_filter.c),
[definitions](https://github.com/apple-oss-distributions/xnu/blob/main/bsd/net/bpf.h).

Upstream links are reference provenance, not immutable deployment pins. Every
host profile intentionally requires build/configuration evidence before use.

## UMA and the ThreeBody substrate

Strix Halo CPU and GPU access a common physical memory pool. GPUVM mappings and
GTT/TTM limits govern GPU access; installed capacity and BIOS reservations belong
to the actual system. One physical pool must be counted once, with allocations
and accessible views accounted separately. Do not add CPU RAM and GPU-accessible
RAM as independent capacity. [AMD Strix Halo guidance](https://rocm.docs.amd.com/en/docs-7.2.0/how-to/system-optimization/strixhalo.html).

HIP allocation flags and coherence mode determine visibility obligations;
fine-grained coherence still requires correct synchronization. Metal shared
resources also require explicit CPU/GPU access coordination. The common contract
is ownership over backing storage; allocation/import and completion mechanisms
remain environment-specific. [HIP coherence](https://rocm.docs.amd.com/projects/HIP/en/latest/how-to/hip_runtime_api/memory_management/coherence_control.html),
[Metal shared storage](https://developer.apple.com/documentation/metal/mtlstoragemode/shared).

```text
Strix CPU <-> HIP-mapped shared allocation <-> Strix GPU
    |
    +-> AF_XDP TX / NIC -> Ethernet -> Arty FPGA
    +<- AF_XDP RX / XDP <- Ethernet <- Arty FPGA

Apple CPU <-> Metal shared allocation <-> Apple GPU
    |
    +<-> macOS BPF / Ethernet <-> FPGA
```

The Strix profile references HIP and Linux eBPF contracts. Its existing physical
catalogue references the GPU/NPU scaffolds and Arty product. USB programming/UART
bring-up is distinct from the proposed Ethernet workload link. Arty Ethernet
pin coverage, MAC/PHY integration and protocol logic remain work.

The handoff fixtures use a synthetic 1024-byte capacity and unresolved layout
identities. They are not ThreeBody's numerical schema. The existing BAREWire
124-byte test layout remains a separate bounded fixture, not a deployed codec.
Production needs its actual schema, envelope, Ethernet/VLAN policy, capture
truncation checks and buffer-size accounting before these values are selected.

AF_XDP NIC zero-copy does not establish aliasing with a HIP allocation. Its
TX completion permits UMEM reuse; a correlated FPGA reply establishes a different
application event. macOS BPF capture copies likewise prevent inferring a shared
GPU/NIC/FPGA allocation. PCIe DMA requires explicit mapping evidence. NPU access
needs its own import and completion contract. The structural checks only compare
declared payload capacities and identity; they do not validate a DMA mapping,
framing, memory permissions or synchronization implementation.

Strix Halo is a useful local example for later server memory-domain designs.
Server topology, NUMA/coherence scope, CXL/RoCE support and remote access latency
must be described independently; they cannot inherit a laptop/APU contract by
analogy. No Fidelity service or runtime is introduced by this structure.

## Validation and next implementation boundary

Run `dotnet run --project tests/Admission/Admission.Tests.fsproj` in this repo.
The F# runner compiles unchanged `.clef` files through temporary `.fs` filenames
under ignored `obj/`, and checks the original `.fidproj` source closures with
CCS. It tests boundary limits, absent/ambiguous evidence, stale artifacts,
advisory versus hard bounds, OS separation and UMA handoff failures. It does
not load a kernel program or exercise a GPU or FPGA.

The next vertical slice should bind a small Linux packet classifier to a pinned
host, emit its final artifact through Composer, derive actual obligations and
compare design-time findings with the kernel gate. Add BAREWire map layout and
AF_XDP redirection after that admission path works. Keep ThreeBody integration
independent of this initial compiler acceptance test.
