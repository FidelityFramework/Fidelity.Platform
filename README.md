# Fidelity.Platform

Platform binding monorepo for the Fidelity framework.

## Structure

Each platform has its own subdirectory with quotation-based bindings:

```
Fidelity.Platform/
├── Linux_x86_64/       # Linux on x86-64
├── Linux_ARM64/        # Linux on ARM64 (future)
├── Windows_x86_64/     # Windows on x86-64 (future)
├── MacOS_x86_64/       # macOS on x86-64 (future)
├── MacOS_ARM64/        # macOS on ARM64/Apple Silicon (future)
├── BareMetal_ARM32/    # Bare-metal ARM Cortex-M (future)
└── ...
```

## Usage in fidproj

Reference the specific platform your project targets:

```toml
[dependencies]
alloy = { path = "/home/hhh/repos/Alloy/src" }
platform = { path = "/home/hhh/repos/Fidelity.Platform/Linux_x86_64" }
```

## Architecture

Platform bindings provide:

1. **Type Layouts** - Size and alignment for primitive types (`int`, `nativeint`, `pointer`, etc.)
2. **Syscall Conventions** - Calling convention, register assignments, syscall instruction
3. **Syscall Numbers** - Platform-specific syscall numbers
4. **Memory Regions** - Stack, heap, peripheral memory semantics

These are expressed as **F# quotations** that flow through the compilation pipeline
unchanged until Alex witnesses them and generates platform-specific MLIR.

## Design Principle

Platform knowledge flows "from the top" - it comes from binding libraries, not from
compiler inference. This enables:

- GPU platforms (CUDA, ROCm, AVX-512)
- NPU platforms (tensor accelerators)
- FPGA platforms (programmable logic)
- Embedded platforms (ARM Cortex-M, RISC-V)

All follow the same pattern: quotation-based bindings → PSG → Alex witnesses → MLIR.
