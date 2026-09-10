namespace Fidelity.Platform.MCU.Renesas.RA6M5.EK_RA6M5

open BAREWire.Platform
open BAREWire.Hardware

/// R7FA6M5BH3CFC / EK-RA6M5 v1 facts. See docs/SOURCE_MANIFEST.md.
/// Composer reads these declarations from CCS's graph and checks the image
/// memory/vector layout in-process with BAREWire.
module Description =
    let pointerWidth: WidthDeclaration = { Name = "Pointer"; Bits = 32 }
    let registerWidth: WidthDeclaration = { Name = "Register"; Bits = 32 }

    // The integer representations the core offers natively: two's-complement
    // and unsigned at 8, 16, 32 and 64 bits, wrapping at their boundaries
    // (64-bit arithmetic is register pairs on ARMv8-M, still native and
    // wrapping). Ranges as exact decimal text.
    let repInt8: Representation = { Name = "int8"; Capability = Capability.Native; Family = RepresentationFamily.Int; Bits = 8; MinMagnitude = "-128"; MaxMagnitude = "127"; Boundary = Boundary.Wrap }
    let repInt16: Representation = { Name = "int16"; Capability = Capability.Native; Family = RepresentationFamily.Int; Bits = 16; MinMagnitude = "-32768"; MaxMagnitude = "32767"; Boundary = Boundary.Wrap }
    let repInt32: Representation = { Name = "int32"; Capability = Capability.Native; Family = RepresentationFamily.Int; Bits = 32; MinMagnitude = "-2147483648"; MaxMagnitude = "2147483647"; Boundary = Boundary.Wrap }
    let repInt64: Representation = { Name = "int64"; Capability = Capability.Native; Family = RepresentationFamily.Int; Bits = 64; MinMagnitude = "-9223372036854775808"; MaxMagnitude = "9223372036854775807"; Boundary = Boundary.Wrap }
    let repUInt8: Representation = { Name = "uint8"; Capability = Capability.Native; Family = RepresentationFamily.UInt; Bits = 8; MinMagnitude = "0"; MaxMagnitude = "255"; Boundary = Boundary.Wrap }
    let repUInt16: Representation = { Name = "uint16"; Capability = Capability.Native; Family = RepresentationFamily.UInt; Bits = 16; MinMagnitude = "0"; MaxMagnitude = "65535"; Boundary = Boundary.Wrap }
    let repUInt32: Representation = { Name = "uint32"; Capability = Capability.Native; Family = RepresentationFamily.UInt; Bits = 32; MinMagnitude = "0"; MaxMagnitude = "4294967295"; Boundary = Boundary.Wrap }
    let repUInt64: Representation = { Name = "uint64"; Capability = Capability.Native; Family = RepresentationFamily.UInt; Bits = 64; MinMagnitude = "0"; MaxMagnitude = "18446744073709551615"; Boundary = Boundary.Wrap }

    let core: TargetCore = {
        Os = "none"
        Arch = "arm_cortex_m33"
        WordSizeBits = 32
        Endianness = "little"
        Runtime = "bare"
        Triple = "thumbv8m.main-none-eabi"
        CpuModel = "cortex-m33"
        Widths = [| pointerWidth; registerWidth |]
        Representations = [| repInt8; repInt16; repInt32; repInt64; repUInt8; repUInt16; repUInt32; repUInt64 |]
    }

    let flash: MemorySpace = {
        Name = "flash"; Kind = MemoryKind.Flash; Base = Some 0; Capacity = 2097152
        Alignment = 512; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadExecute
        Notes = "Code flash; board part and IDAU access must be checked before download."; MapKind = ""; Since = ""; Until = ""
    }
    let sram: MemorySpace = {
        Name = "sram"; Kind = MemoryKind.Sram; Base = Some 536870912; Capacity = 524288
        Alignment = 8; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadWrite
        Notes = "Physical SRAM; reset-secure image requires accessible allocations."; MapKind = ""; Since = ""; Until = ""
    }
    let icu: MemorySpace = {
        Name = "icu"; Kind = MemoryKind.Peripheral; Base = Some 1073766400; Capacity = 4096
        Alignment = 4; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadWrite
        Notes = "ICU registers have individual byte/word access widths."; MapKind = ""; Since = ""; Until = ""
    }
    let cpscu: MemorySpace = {
        Name = "cpscu"; Kind = MemoryKind.Peripheral; Base = Some 1073774592; Capacity = 4096
        Alignment = 4; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadWrite
        Notes = "Peripheral security attribution; ICUSARG is at offset 0x70, protected by PRCR.PRC4."
        MapKind = ""; Since = ""; Until = ""
    }
    let system: MemorySpace = {
        Name = "system"; Kind = MemoryKind.Peripheral; Base = Some 1073864704; Capacity = 4096
        Alignment = 4; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadWrite
        Notes = "System control, clock selection and protection."; MapKind = ""; Since = ""; Until = ""
    }
    let port0: MemorySpace = {
        Name = "port0"; Kind = MemoryKind.Peripheral; Base = Some 1074266112; Capacity = 32
        Alignment = 2; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadWrite
        Notes = "GPIO port 0 PODR/PIDR/PDR."; MapKind = ""; Since = ""; Until = ""
    }
    let pfs: MemorySpace = {
        Name = "pfs"; Kind = MemoryKind.Peripheral; Base = Some 1074268160; Capacity = 1536
        Alignment = 4; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadWrite
        Notes = "PFS plus PMISC protection/security controls."; MapKind = ""; Since = ""; Until = ""
    }
    let ppb: MemorySpace = {
        Name = "core"; Kind = MemoryKind.Peripheral; Base = Some 3758153728; Capacity = 4096
        Alignment = 4; Granularity = 1; Growth = Growth.Fixed; Access = Access.ReadWrite
        Notes = "Secure PPB: SysTick, NVIC, SCB."; MapKind = ""; Since = ""; Until = ""
    }

    // Natural vector storage is U32[112]; placement requires 512 alignment.
    let vectorLayout: StructDescriptor = {
        Name = "ArmV8MVectors"
        Layout = { Size = 448; Alignment = 4; Fields = [|
            { Name = "entries"; Offset = 0; Repr = Repr.U32; Count = 112
              Access = AccessKind.ReadOnly; BitFields = [||]; Documentation = Some "16 core + 96 ICU entries" }
        |] }
        Documentation = Some "Renesas HW 13.3; Arm VTOR placement alignment is a separate requirement."
    }

    let vectorAlignment = 512
    let stackBytes = 8192
    let image: CortexMImageDescriptor = {
        FlashSpace = "flash"; RamSpace = "sram"; VectorLayout = "ArmV8MVectors"
        VectorAlignment = vectorAlignment; StackBytes = stackBytes
        EntrySymbol = "Reset_Handler"; DebugDevice = "R7FA6M5BH"
        PartNumber = "R7FA6M5BH3CFC"; PartNumberAddress = 16810224
        PreservedOptionAddress = 16818432; PreservedOptionBytes = 8
    }
    let resetIclkHz = 2000000
    let applicationIclkHz = 8000000
    let pwmInterruptHz = 10000
    let tickHz = 1000
    let bluePin = 6
    let greenPin = 7
    let redPin = 8
    let colorPin = 5
    let pacePin = 4
    let colorEvent = 11
    let paceEvent = 10

    let lifecycle: LifecycleFacts = {
        Clocks = [| { Name = "reset-ICLK"; FrequencyHz = 2000000 } |]
        Resets = [| { Name = "RES"; External = true; ActiveHigh = false } |]
        Entry = "Reset_Handler"; Teardown = "Fault_Handler"; Persistence = Persistence.Volatile
    }
    let descriptor: PlatformDescription = {
        Id = "mcu-renesas-ra6m5-ek-ra6m5"; DisplayName = "Renesas EK-RA6M5"
        Substrate = "mcu"; Core = Some core
        Spaces = [| flash; sram; icu; cpscu; system; port0; pfs; ppb |]
        Surfaces = [||]; Buffers = [||]; Transports = [||]; Lifecycle = lifecycle
        Notes = [| "First slice: secure reset, GPIO, SysTick and two ICU routes. No RTOS, FSP HAL, heap, DMA or option-memory payload."
                   "Peripheral access widths and circuit assumptions are recorded in Registers.clef and docs/HELLOBLINKY_HARDWARE.md." |]
        Limits = [| { Name = "external-vectors"; Value = 96 } |]
    }
