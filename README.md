# Fidelity.Platform

Platform binding monorepo for the Fidelity framework.

## Structure

The repository now uses a substrate-first layout to keep platform growth organized:

```
Fidelity.Platform/
├── Contracts/                           # Shared substrate-neutral contracts
├── CPU/
│   └── Linux/
│       └── X86_64/
│           └── StrixHalo/
├── MCU/
│   ├── ST/
│   │   └── STM32F7/
│   │       └── MeadowF7/
│   └── Renesas/
│       └── RA6M5/
│           └── EK_RA6M5/
├── GPU/
│   └── AMD/
│       └── RDNA3_5/
│           └── StrixHalo_iGPU/
├── NPU/
│   └── AMD/
│       └── XDNA2/
│           └── StrixHalo_NPU/
├── FPGA/
│   └── Xilinx/
│       └── Artix7/
│           └── ArtyA7_100T/
├── CGRA/                                # Reserved substrate space
├── Profiles/
│   └── StrixHalo_ArtyLab/
```

For additional details, see `PLATFORM_STRUCTURE.md`.

## Usage in fidproj

Reference the specific platform package your project targets:

```toml
[dependencies]
platform = { path = "/home/hhh/repos/Fidelity.Platform/FPGA/Xilinx/Artix7/ArtyA7_100T" }
```

Or use the current Linux/x86_64 CPU package used by existing sample projects:

```toml
[dependencies]
platform = { path = "/home/hhh/repos/Fidelity.Platform/CPU/Linux/x86_64" }
```

## Architecture

Platform bindings provide data that ultimately informs backend lowering:

1. Type/layout and substrate metadata
2. Endpoint and channel definitions
3. Device capability declarations
4. Memory and clock topology

These are expressed in source and consumed through the normal project loading path.

## Design Principle

Platform knowledge flows from binding packages, not ad hoc compiler inference.
A consistent package shape across CPU/MCU/GPU/NPU/FPGA/CGRA allows:

- Clean multi-substrate composition via profiles
- Per-target evolution without contaminating unrelated substrates
- Explicit dependency wiring from application manifests
