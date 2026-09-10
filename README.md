# Fidelity.Platform

Platform declarations and native bindings for the Fidelity framework. Shared
silicon facts, physical products, execution environments and workload profiles
have separate source owners. CCS resolves the selected declarations; Composer
uses their evidence to compile, package and deploy the artifact.

## Structure

- `Contracts/`: shared requirements, including BAREWire-backed MMIO contracts.
- `Hardware/Silicon/`: architecture, part and package facts.
- `Hardware/Products/`: component selection, wiring and available resources.
- `Hardware/VirtualMachines/`: guest-machine descriptions; currently synthetic.
- `Environments/`: execution ABI, runtime services and native bindings.
- `Protocols/`: reserved protocol implementations, including virtio.
- `Profiles/`: explicit selection and resource budgets for one execution target.

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

See [compiler integration](docs/CANONICAL_PLATFORM_SPEC.md),
[composition rules](docs/PLATFORM_COMPOSITION.md) and the
[documentation index](docs/README.md). General resource instantiation, memory-domain
composition, virtio drivers and OCI orchestration remain further work.
