# Fidelity.Platform structure

Implemented taxonomy, 2026-09-10. Hardware identity, execution environment,
communication protocol and workload selection have separate owners. Shared
silicon declarations can support several products; one product can support
several execution profiles.

```text
Fidelity.Platform/
├── Contracts/
├── AbstractMachines/
│   ├── eBPF/                     # Reference description; no backend
│   └── cBPF/                     # Reference description; no backend
├── Hardware/
│   ├── Silicon/
│   │   ├── CPU/
│   │   ├── MCU/
│   │   ├── GPU/
│   │   ├── NPU/
│   │   ├── FPGA/
│   │   └── CGRA/                 # Reserved
│   ├── Products/
│   │   └── <manufacturer>/<product>/[<verified-hardware-revision>/]
│   └── VirtualMachines/
│       └── <provider>/<machine>/
├── Environments/
│   ├── Linux/                    # x86_64 bindings; eBPF reference contract
│   ├── Freestanding/
│   │   ├── arm_cortex_m33/
│   │   └── x86_64/
│   ├── Windows/                  # eBPF reference contract; no native target
│   ├── macOS/                    # BPF/Metal reference contracts; no native target
│   ├── Android/                  # Reserved
│   └── iOS/                      # Reserved
├── Protocols/
│   └── Virtio/                   # Reserved; no driver implementation
└── Profiles/
```

## Ownership

| Area | Responsibility |
| --- | --- |
| `Contracts/` | Shared requirements, regions, mappings, transactions, grants and predicates; BAREWire supplies memory-layout vocabulary |
| `AbstractMachines/` | Instruction semantics/widths, independent of the native host CPU and OS admission policy |
| `Hardware/Silicon/<kind>/<vendor>/<family>/...` | Architecture, chip/compute-block capabilities, peripheral interfaces and concrete part/package facts; generic x86_64 facts do not require a vendor partition |
| `Hardware/Products/<manufacturer>/<product>/` | Physical assembly: component selection, memory configuration, wiring, clocks and connectors |
| `Hardware/VirtualMachines/<provider>/<machine>/` | Machine presented to a guest: memory/resources and, when implemented, boot and discovery requirements |
| `Environments/<environment>/<architecture>/` | ABI, runtime services, native bindings and execution requirements |
| `Environments/<environment>/<instruction-host>/` | Architecture-independent reference admission/attachment contracts; native loader ABI remains architecture-specific |
| `Protocols/` | Communication layouts, negotiation, sequencing and transports |
| `Profiles/<profile>/` | Selected hardware/environment composition and image or workload budgets |

Products include development boards, accelerator cards and complete systems.
Product and silicon manufacturers can differ. A hardware-revision directory
requires applicability evidence: EK-RA6M5 schematic issue 3.0 does not establish
a physical Rev3 board. Supported source design and unconfirmed physical artwork
or population remain explicit in product documentation.

Family reuse uses package dependencies and immutable references. Each concrete
part must establish which peripherals and pins it contains. A future SoC model
must own shared topology and reference its CPU/GPU/NPU blocks; it must not count
shared RAM again under every compute category. Repeated physical instances and
general topology composition still need implementation.

PC, mobile, SBC, development board and server are descriptive categories. They
do not select widths, runtime services or permissions. Composer owns packaging
and deployment; OCI would be an orchestration selection, not a hardware branch.

## Current packages and status

| Selection or inventory | Status |
| --- | --- |
| [EK_RA6M5_HelloBlinky](Profiles/EK_RA6M5_HelloBlinky) | Accepted MCU image composed from [Cortex-M33 facts](Hardware/Silicon/CPU/Arm/CortexM33), the [R7FA6M5BH3CFC part/package](Hardware/Silicon/MCU/Renesas/RA6M5/R7FA6M5BH3CFC), [EK-RA6M5 wiring](Hardware/Products/Renesas/EK_RA6M5) and the freestanding environment |
| [Linux_x86_64_Default](Profiles/Linux_x86_64_Default) | Hosted Linux/libc selection; architecture facts, services/bindings and preserved application budgets have separate owners |
| [ArtyA7_HelloArty](Profiles/ArtyA7_HelloArty) | Application report/buffer/UART requirements over [Digilent product wiring](Hardware/Products/Digilent/ArtyA7_100T) and [Xilinx part facts](Hardware/Silicon/FPGA/Xilinx/Artix7/XC7A100T_CSG324); Contracts still supplies the operative pin map |
| [RestrictedGuest64](Profiles/RestrictedGuest64) | Synthetic 64-bit-pointer/32-bit-MMIO compiler fixture using a [synthetic machine](Hardware/VirtualMachines/Synthetic/RestrictedGuest64) and freestanding x86_64 facts; no VM boot or virtio |
| [MeadowF7](Hardware/Products/WildernessLabs/MeadowF7) | Product scaffold and reference pack; no accepted MCU bring-up |
| [StrixHalo_iGPU](Hardware/Silicon/GPU/AMD/RDNA3_5/StrixHalo_iGPU), [StrixHalo_NPU](Hardware/Silicon/NPU/AMD/XDNA2/StrixHalo_NPU) | Silicon scaffolds; Linux-hosted [ROCm](Environments/Linux/x86_64/ROCm) and [XRT](Environments/Linux/x86_64/XRT) binding packages are separate |
| [StrixHalo_ArtyLab](Profiles/StrixHalo_ArtyLab) | ThreeBody physical reference catalogue and checked handoff descriptions; not a general multi-target resource resolver |
| [BPF_Reference](Profiles/BPF_Reference) | Unresolved Linux/Windows/macOS admission selections; no emitted or loaded BPF artifact |
| [AppleSilicon_Metal_FPGA_Reference](Profiles/AppleSilicon_Metal_FPGA_Reference) | Synthetic shared-storage/Ethernet handoffs; no native executable or FPGA protocol |

Reference-only ST product packs, the
[Renesas FPB-RA6E2 pack](Hardware/Products/Renesas/FPB_RA6E2) and reserved branches
do not claim executable support. Linux's
[Experimental](Environments/Linux/x86_64/Experimental) sources
remain explicitly selected legacy candidates, separate from typed packages.

## Package and selection rules

`.fidproj` manifests own source lists and dependencies. Authored production
sources use `.clef`; there is no mandatory source filename. Some public namespaces
retain pre-migration spelling, while live package paths use this hierarchy.

Migrated profiles select one fully qualified `[platform] description` export.
CCS accepts it only within the selected package's transitive source closure,
preserving declaration identity across sibling packages. It does not merge
unrelated catalogue descriptions. See the implemented
[composition rules](docs/PLATFORM_COMPOSITION.md) and
[compiler integration](docs/CANONICAL_PLATFORM_SPEC.md).
