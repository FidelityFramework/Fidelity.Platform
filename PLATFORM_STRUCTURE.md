# Fidelity.Platform Structure

This repository is organized substrate-first so platform bindings do not collapse into an unstructured bucket.

## Canonical Layout

- `Contracts/` - shared substrate-neutral contract types used by leaf packages
- `CPU/<OS>/<ISA>/` - hosted CPU packages; the current leaf is `CPU/Linux/x86_64`
- `MCU/<Vendor>/<Family>/<Board>/` - microcontroller substrate packages
- `GPU/<Vendor>/<Family>/<Device>/` - GPU substrate packages
- `NPU/<Vendor>/<Family>/<Device>/` - NPU substrate packages
- `FPGA/<Vendor>/<Family>/<Board>/` - FPGA substrate packages
- `CGRA/<Vendor>/<Family>/<Device>/` - CGRA substrate packages (reserved)
- `Profiles/<ProfileName>/` - curated multi-substrate bundles for specific development setups

## Current Leaves

- `CPU/Linux/x86_64`
- `MCU/ST/STM32F7/MeadowF7`
- `MCU/Renesas/RA6M5/EK_RA6M5`
- `GPU/AMD/RDNA3_5/StrixHalo_iGPU`
- `NPU/AMD/XDNA2/StrixHalo_NPU`
- `FPGA/Xilinx/Artix7/ArtyA7_100T`
- `Profiles/StrixHalo_ArtyLab`

## Notes

- New packages use `.fidproj` as the dependency boundary expected by CCS/Composer source resolution.
- External application projects (e.g., `HelloArty`) can point their `platform` dependency at one leaf package.

## Package status

Directory presence is not a claim of working hardware support. Linux x86_64 has
separate compiler-surface, native binding, display and Ariel manifests. Arty has
both Contracts pin bindings and an additive BAREWire description. EK-RA6M5 has
the accepted HelloBlinky path and a complete physical wiring inventory, with an
initial subset of peripheral registers implemented.

Meadow, GPU and NPU leaf descriptors remain scaffolds; the latter two also have
generated hosted library bindings. Their manifests do not establish working
device kernels or complete topology/memory models. CGRA is reserved.
`Profiles/StrixHalo_ArtyLab` lists dependencies; it is not an implemented general
multi-platform resource resolver. [The audit](docs/DOCUMENTATION_AUDIT.md) records
specific gaps, including incomplete scaffold records.

`CPU/Linux/x86_64/Experimental/` preserves explicitly selected legacy candidates.
It is separate from the typed production packages. Source filenames and package
dependencies come from each `.fidproj`; no universal `Platform.fs` file is required.
