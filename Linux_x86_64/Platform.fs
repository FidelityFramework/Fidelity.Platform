/// Linux x86-64 platform bindings.
/// Quotation-based platform descriptor that flows through the pipeline to Alex.
namespace Fidelity.Platform.Linux_x86_64

open Microsoft.FSharp.Quotations
open Fidelity.Platform.Types

module Platform =

    /// Type layouts for Linux x86-64 (LP64 data model)
    /// - int is 32-bit
    /// - long is 64-bit
    /// - pointer is 64-bit
    let typeLayouts: Expr<Map<string, TypeLayout>> = <@
        Map.ofList [
            // Signed integers
            "int8",      { Size = 1; Alignment = 1 }
            "int16",     { Size = 2; Alignment = 2 }
            "int32",     { Size = 4; Alignment = 4 }
            "int",       { Size = 4; Alignment = 4 }   // F# int = int32
            "int64",     { Size = 8; Alignment = 8 }
            "nativeint", { Size = 8; Alignment = 8 }   // 64-bit on x86-64

            // Unsigned integers
            "uint8",     { Size = 1; Alignment = 1 }
            "byte",      { Size = 1; Alignment = 1 }   // F# byte = uint8
            "uint16",    { Size = 2; Alignment = 2 }
            "uint32",    { Size = 4; Alignment = 4 }
            "uint64",    { Size = 8; Alignment = 8 }
            "unativeint",{ Size = 8; Alignment = 8 }   // 64-bit on x86-64

            // Floating point
            "float32",   { Size = 4; Alignment = 4 }
            "single",    { Size = 4; Alignment = 4 }   // F# single = float32
            "float",     { Size = 8; Alignment = 8 }   // F# float = float64
            "float64",   { Size = 8; Alignment = 8 }
            "double",    { Size = 8; Alignment = 8 }

            // Pointers
            "nativeptr", { Size = 8; Alignment = 8 }
            "voidptr",   { Size = 8; Alignment = 8 }

            // Special
            "bool",      { Size = 1; Alignment = 1 }
            "char",      { Size = 2; Alignment = 2 }   // F# char is UTF-16
            "unit",      { Size = 0; Alignment = 1 }
        ]
    @>

    /// Linux x86-64 syscall convention (System V AMD64 ABI)
    /// Arguments: RDI, RSI, RDX, R10, R8, R9
    /// Return: RAX (also RDX for 128-bit returns)
    /// Syscall number: RAX
    /// Clobbered: RCX, R11
    let syscallConvention: Expr<SyscallConvention> = <@
        { CallingConvention = SysV_AMD64
          ArgRegisters = [| RDI; RSI; RDX; R10; R8; R9 |]
          ReturnRegister = RAX
          SyscallNumberRegister = RAX
          SyscallInstruction = Syscall }
    @>

    /// Memory regions for userspace Linux
    let memoryRegions: Expr<MemoryRegion list> = <@
        [ { Name = "Stack"
            Properties = MemoryProperties.Read ||| MemoryProperties.Write }
          { Name = "Heap"
            Properties = MemoryProperties.Read ||| MemoryProperties.Write }
          { Name = "Text"
            Properties = MemoryProperties.Read ||| MemoryProperties.Execute }
          { Name = "Data"
            Properties = MemoryProperties.Read ||| MemoryProperties.Write }
          { Name = "Rodata"
            Properties = MemoryProperties.Read } ]
    @>

    /// Complete platform descriptor for Linux x86-64
    let platform: Expr<PlatformDescriptor> = <@
        { Architecture = X86_64
          OperatingSystem = Linux
          WordSize = 64
          Endianness = Little
          TypeLayouts =
            Map.ofList [
                "int8", { Size = 1; Alignment = 1 }
                "int16", { Size = 2; Alignment = 2 }
                "int32", { Size = 4; Alignment = 4 }
                "int", { Size = 4; Alignment = 4 }
                "int64", { Size = 8; Alignment = 8 }
                "nativeint", { Size = 8; Alignment = 8 }
                "uint8", { Size = 1; Alignment = 1 }
                "byte", { Size = 1; Alignment = 1 }
                "uint16", { Size = 2; Alignment = 2 }
                "uint32", { Size = 4; Alignment = 4 }
                "uint64", { Size = 8; Alignment = 8 }
                "unativeint", { Size = 8; Alignment = 8 }
                "float32", { Size = 4; Alignment = 4 }
                "float", { Size = 8; Alignment = 8 }
                "float64", { Size = 8; Alignment = 8 }
                "nativeptr", { Size = 8; Alignment = 8 }
                "bool", { Size = 1; Alignment = 1 }
                "char", { Size = 2; Alignment = 2 }
                "unit", { Size = 0; Alignment = 1 }
            ]
          SyscallConvention =
            { CallingConvention = SysV_AMD64
              ArgRegisters = [| RDI; RSI; RDX; R10; R8; R9 |]
              ReturnRegister = RAX
              SyscallNumberRegister = RAX
              SyscallInstruction = Syscall }
          MemoryRegions =
            [ { Name = "Stack"; Properties = MemoryProperties.Read ||| MemoryProperties.Write }
              { Name = "Heap"; Properties = MemoryProperties.Read ||| MemoryProperties.Write }
              { Name = "Text"; Properties = MemoryProperties.Read ||| MemoryProperties.Execute }
              { Name = "Data"; Properties = MemoryProperties.Read ||| MemoryProperties.Write }
              { Name = "Rodata"; Properties = MemoryProperties.Read } ]
          EntryPointABI =
            { Kind = ArgcArgv
              CFunctionName = "main"
              CParameters = [("argc", "i32"); ("argv", "ptr")]  // int argc, char** argv
              CReturnType = "i32"
              FSharpSignature = "array<string> -> int" } }
    @>
