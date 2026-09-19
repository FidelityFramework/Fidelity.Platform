# Fidelity.Platform structure

Taxonomy updated 2026-09-14: the WebSocket protocol moved under `Protocols/`, leaving its Linux socket server in the environment. 2026-09-13 added a documentation-only Radio scaffold.
Hardware identity, execution environment,
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
│   │   ├── Radio/                # Inventory and future discrete parts
│   │   └── CGRA/                 # Reserved
│   ├── Products/
│   │   └── <manufacturer>/<product>/[<verified-hardware-revision>/]
│   └── VirtualMachines/
│       └── <provider>/<machine>/
├── Environments/
│   ├── Linux/                    # x86_64 bindings; eBPF reference contract
│   ├── Freestanding/
│   │   ├── arm_cortex_m33/
│   │   ├── arm_cortex_m7/
│   │   ├── xtensa_esp32s3/
│   │   └── x86_64/
│   ├── Windows/                  # eBPF reference contract; no native target
│   ├── macOS/                    # BPF/Metal reference contracts; no native target
│   ├── Android/                  # Reserved
│   └── iOS/                      # Reserved
├── Protocols/
│   ├── Radio/                    # Design scaffold; no radio drivers
│   ├── Virtio/                   # Reserved; no driver implementation
│   └── WebSocket/                # RFC 6455 frames, handshake, types; environments bind the socket
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

[Radio hardware](Hardware/Silicon/Radio/README.md) reserves discrete-radio part
ownership and indexes integrated MCU radios without duplicating them.
[WebSocket](Protocols/WebSocket/README.md) holds RFC 6455 as protocol source shared
by every environment; `Environments/Linux/x86_64/WebSocket/` keeps only the socket-bound
server and its descriptor types. [Radio protocols](Protocols/Radio/README.md) groups Bluetooth, Wi-Fi and future
LoRa work. The [radio model](docs/RADIO_MODEL.md) keeps board wiring, protocol
semantics, environment bindings and application messages in their owning layers.

PC, mobile, SBC, development board and server are descriptive categories. They
do not select widths, runtime services or permissions. Composer owns packaging
and deployment; OCI would be an orchestration selection, not a hardware branch.

## Current packages and status

| Selection or inventory | Status |
| --- | --- |
| [EK_RA6M5_HelloBlinky](Profiles/EK_RA6M5_HelloBlinky) | Accepted MCU image composed from [Cortex-M33 facts](Hardware/Silicon/CPU/Arm/CortexM33), the [R7FA6M5BH3CFC part/package](Hardware/Silicon/MCU/Renesas/RA6M5/R7FA6M5BH3CFC), [EK-RA6M5 wiring](Hardware/Products/Renesas/EK_RA6M5) and the freestanding environment |
| [CCC2026Badge_HelloESP](Profiles/CCC2026Badge_HelloESP/README.md) | Compiled display/LED/button/interrupt workload observed on hardware; Wi-Fi/BLE outside acceptance |
| [STM32H747I_DISCO_HelloDISCO_Interactive](Profiles/STM32H747I_DISCO_HelloDISCO_Interactive) | Interactive banner/joystick/LED workload and independent cold start accepted; audio, touch, storage and optional radio remain scaffold work |
| [Linux_x86_64_Default](Profiles/Linux_x86_64_Default) | Hosted Linux/libc selection; architecture facts, services/bindings and preserved application budgets have separate owners |
| [Linux_x86_64_WrenHello](Profiles/Linux_x86_64_WrenHello) | The default Linux selection with a 32-page constant-section budget for WrenHello's embedded UI bundle |
| [ArtyA7_HelloArty](Profiles/ArtyA7_HelloArty) | Application report/buffer/UART requirements over [Digilent product wiring](Hardware/Products/Digilent/ArtyA7_100T) and [Xilinx part facts](Hardware/Silicon/FPGA/Xilinx/Artix7/XC7A100T_CSG324); Contracts still supplies the operative pin map |
| [RestrictedGuest64](Profiles/RestrictedGuest64) | Synthetic 64-bit-pointer/32-bit-MMIO compiler fixture using a [synthetic machine](Hardware/VirtualMachines/Synthetic/RestrictedGuest64) and freestanding x86_64 facts; no VM boot or virtio |
| [MeadowF7](Hardware/Products/WildernessLabs/MeadowF7) | Product scaffold and reference pack; no accepted MCU bring-up |
| [Sweet Potato](Hardware/Products/LibreComputer/AML_S905X_CC_V2/README.md) and [KeyStation reference](Profiles/SweetPotato_KeyStation_Reference/README.md) | Board/source inventory and native HDMI/touch/Mali port plan; no platform export or accepted hardware execution |
| [Waveshare 7.9inch HDMI LCD](Hardware/Products/Waveshare/7_9inch_HDMI_LCD/README.md) | Selected KeyStation panel, SKU 17916 and ASIN B087CNJYB4; HDMI/USB integration follows the native port plan |
| [StrixHalo_iGPU](Hardware/Silicon/GPU/AMD/RDNA3_5/StrixHalo_iGPU), [StrixHalo_NPU](Hardware/Silicon/NPU/AMD/XDNA2/StrixHalo_NPU) | Silicon scaffolds; Linux-hosted [ROCm](Environments/Linux/x86_64/ROCm) and [XRT](Environments/Linux/x86_64/XRT) binding packages are separate |
| [StrixHalo_ArtyLab](Profiles/StrixHalo_ArtyLab) | ThreeBody physical reference catalogue and checked handoff descriptions; not a general multi-target resource resolver |
| [BPF_Reference](Profiles/BPF_Reference) | Unresolved Linux/Windows/macOS admission selections; no emitted or loaded BPF artifact |
| [AppleSilicon_Metal_FPGA_Reference](Profiles/AppleSilicon_Metal_FPGA_Reference) | Synthetic shared-storage/Ethernet handoffs; no native executable or FPGA protocol |

Other reference-only ST product packs, the
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
