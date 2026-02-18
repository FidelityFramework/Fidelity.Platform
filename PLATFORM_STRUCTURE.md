# Fidelity.Platform Structure

This repository is organized substrate-first so platform bindings do not collapse into an unstructured bucket.

## Canonical Layout

- `Contracts/` - shared substrate-neutral contract types used by leaf packages
- `CPU/<OS>/<ISA>/<Device>/` - CPU substrate packages
- `MCU/<Vendor>/<Family>/<Board>/` - microcontroller substrate packages
- `GPU/<Vendor>/<Family>/<Device>/` - GPU substrate packages
- `NPU/<Vendor>/<Family>/<Device>/` - NPU substrate packages
- `FPGA/<Vendor>/<Family>/<Board>/` - FPGA substrate packages
- `CGRA/<Vendor>/<Family>/<Device>/` - CGRA substrate packages (reserved)
- `Profiles/<ProfileName>/` - curated multi-substrate bundles for specific development setups

## Current Leaves

- `CPU/Linux/X86_64/StrixHalo`
- `MCU/ST/STM32F7/MeadowF7`
- `MCU/Renesas/RA6M5/EK_RA6M5`
- `GPU/AMD/RDNA3_5/StrixHalo_iGPU`
- `NPU/AMD/XDNA2/StrixHalo_NPU`
- `FPGA/Xilinx/Artix7/ArtyA7_100T`
- `Profiles/StrixHalo_ArtyLab`

## Notes

- Existing `Linux_x86_64/` remains in place as the active legacy package currently referenced by existing samples.
- New packages use `.fidproj` as the dependency boundary expected by CCS/Composer source resolution.
- External application projects (e.g., `HelloArty`) can point their `platform` dependency at one leaf package.
