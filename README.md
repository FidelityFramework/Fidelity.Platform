# Fidelity.Platform

Platform declarations and native bindings for the Fidelity framework. Shared
silicon facts, physical products, execution environments and workload profiles
have separate source owners. CCS resolves the selected declarations; Composer
uses their evidence to compile, package and deploy the artifact.

## Structure

- `Contracts/`: shared MMIO, admission-evidence and BAREWire-backed handoff contracts.
- `AbstractMachines/`: cBPF/eBPF instruction descriptions; no executable backend.
- `Hardware/Silicon/`: architecture, part and package facts.
- `Hardware/Products/`: component selection, wiring and available resources.
- `Hardware/VirtualMachines/`: guest-machine descriptions; currently synthetic.
- `Environments/`: execution ABI, host-provided services, native bindings and admission rules.
- `Protocols/`: reserved protocol implementations, including virtio.
- `Profiles/`: execution selections, workload budgets and explicitly marked reference catalogues.

[PLATFORM_STRUCTURE.md](PLATFORM_STRUCTURE.md) lists current packages and support
status. The former top-level CPU/MCU/GPU/NPU/FPGA package paths have moved; source
namespaces are retained where needed for existing APIs.

## Selecting a platform

Applications select a package through their `platform` dependency. For example,
from the sibling HelloBlinky repository:

```toml
[dependencies]
platform = { path = "../../../../Fidelity.Platform/Profiles/EK_RA6M5_HelloBlinky/Fidelity.Platform.fidproj" }
```

The selected package declares its authoritative export:

```toml
[platform]
description = "Fidelity.Platform.Profiles.EK_RA6M5_HelloBlinky.Description.descriptor"
runtime_model = "bare"
os = "none"
arch = "arm_cortex_m33"
```

Other selections include [Linux x86_64](Profiles/Linux_x86_64_Default),
[HelloArty](Profiles/ArtyA7_HelloArty) and the compiler-only
[restricted guest](Profiles/RestrictedGuest64). Linux binding packages remain
under [Environments/Linux/x86_64](Environments/Linux/x86_64).

Profiles reference shared declarations through explicit dependencies. Available
hardware does not grant application access: HelloBlinky owns its mappings, grants
and timing predicate. A product with several compute blocks does not implicitly
select several compilation targets.

## Admission and accelerator handoff references

[Strix Halo + Arty A7](Profiles/StrixHalo_ArtyLab/README.md) is the physical
reference for ThreeBody. Its handoff declarations distinguish CPU/GPU shared
backing, NIC buffer ownership and FPGA request completion. The
[Apple Silicon / Metal reference](Profiles/AppleSilicon_Metal_FPGA_Reference/README.md)
uses the same shared contracts with environment-specific allocation and
visibility obligations. Shared physical memory is accounted once; accessible
views do not create additional capacity or establish device mappings.

[BPF reference selections](Profiles/BPF_Reference/README.md) describe separate
Linux eBPF, Windows eBPF and macOS classic-BPF hosts. Their checks keep safety
obligations, verifier analysis work, execution work and elapsed-time budgets
distinct. Missing host facts remain pending; estimates and measurements do not
discharge hard bounds.

These are checked `.clef` source packages and executable reference consistency
checks. They do not yet provide Composer BPF emission, actual verifier/load
integration, GPU allocation or a working FPGA sidecar protocol. Reference
catalogues do not select a deployable `[platform] description`.

Run the [F# reference checks](tests/Admission/README.md), including CCS checks of
the original package sources, from this repository:

```sh
dotnet run --project tests/Admission/Admission.Tests.fsproj
```

See [compiler integration](docs/CANONICAL_PLATFORM_SPEC.md),
[admission and accelerator handoffs](docs/ADMISSION_AND_SIDECARS.md),
[composition rules](docs/PLATFORM_COMPOSITION.md) and the
[documentation index](docs/README.md). General resource instantiation, memory-domain
composition, virtio drivers and OCI orchestration remain further work.
