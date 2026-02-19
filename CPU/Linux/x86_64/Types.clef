/// Platform binding types for quotation-based platform descriptors.
/// These types appear inside quotations and are inspected at compile time.
module Fidelity.Platform.Types

/// Type layout descriptor - size and alignment in bytes
[<Struct>]
type TypeLayout = {
    Size: int
    Alignment: int
}

/// CPU architecture
type Architecture =
    | X86_64
    | X86
    | ARM64
    | ARM32
    | RISCV64
    | RISCV32

/// Operating system family
type OperatingSystem =
    | Linux
    | Windows
    | MacOS
    | FreeBSD
    | BareMetal

/// Byte order
type Endianness =
    | Little
    | Big

/// x86-64 registers (for syscall conventions)
type Register =
    | RAX | RBX | RCX | RDX
    | RSI | RDI | RBP | RSP
    | R8 | R9 | R10 | R11
    | R12 | R13 | R14 | R15

/// Syscall instruction type
type SyscallInstruction =
    | Syscall       // x86-64 syscall
    | Int80         // x86 int 0x80
    | Svc           // ARM SVC
    | Ecall         // RISC-V ecall

/// Calling convention
type CallingConvention =
    | SysV_AMD64    // Linux/macOS x86-64
    | Win64         // Windows x86-64
    | AAPCS         // ARM
    | RISCV         // RISC-V

/// Syscall convention descriptor
[<Struct>]
type SyscallConvention = {
    CallingConvention: CallingConvention
    ArgRegisters: Register array
    ReturnRegister: Register
    SyscallNumberRegister: Register
    SyscallInstruction: SyscallInstruction
}

/// Memory region properties
[<System.Flags>]
type MemoryProperties =
    | None = 0
    | Read = 1
    | Write = 2
    | Execute = 4
    | Volatile = 8
    | NoCache = 16

/// Memory region descriptor
[<Struct>]
type MemoryRegion = {
    Name: string
    Properties: MemoryProperties
}

/// Entry point parameter categories (platform-dependent ABI)
type EntryPointKind =
    | ArgcArgv      // Standard C: int argc, char** argv (POSIX/Linux)
    | WinMain       // Windows: HINSTANCE, HINSTANCE, LPSTR, int nCmdShow
    | Naked         // No parameters (freestanding/bare metal)

/// Entry point ABI descriptor - defines C boundary for program entry
/// This is a platform binding just like SyscallConvention
type EntryPointABI = {
    /// The kind of entry point (determines C signature)
    Kind: EntryPointKind

    /// C function name at ABI level (e.g., "main", "_start", "WinMain")
    CFunctionName: string

    /// C parameter types in order [("argc", "i32"); ("argv", "ptr")]
    CParameters: (string * string) list

    /// C return type
    CReturnType: string

    /// F# semantic type expected by user code (e.g., "array<string> -> int")
    FSharpSignature: string
}

/// Complete platform descriptor
type PlatformDescriptor = {
    Architecture: Architecture
    OperatingSystem: OperatingSystem
    WordSize: int
    Endianness: Endianness
    TypeLayouts: Map<string, TypeLayout>
    SyscallConvention: SyscallConvention
    MemoryRegions: MemoryRegion list
    EntryPointABI: EntryPointABI
}

// ═══════════════════════════════════════════════════════════════════════════
// Socket Types
// ═══════════════════════════════════════════════════════════════════════════

/// IPv4 socket address (struct sockaddr_in)
/// Layout: sin_family (2) + sin_port (2) + sin_addr (4) + sin_zero (8) = 16 bytes
[<Struct>]
type SockAddrIn = {
    sin_family: int16       // Address family (AF_INET = 2)
    sin_port: uint16        // Port number (network byte order)
    sin_addr: uint32        // IPv4 address (network byte order)
    sin_zero: int64         // Padding to reach 16 bytes
}

/// Poll file descriptor structure (struct pollfd) - legacy, prefer epoll
/// Layout: fd (4) + events (2) + revents (2) = 8 bytes
[<Struct>]
type PollFd = {
    fd: int                 // File descriptor
    events: int16           // Requested events (POLLIN, POLLOUT, etc.)
    revents: int16          // Returned events
}

/// epoll event structure (struct epoll_event)
/// Layout: events (4) + padding (4) + data (8) = 16 bytes (packed on x86-64)
/// Note: Linux x86-64 packs this to 12 bytes, but we use 16 for alignment
[<Struct>]
type EpollEvent = {
    events: uint32          // Event flags (EPOLLIN, EPOLLOUT, etc.)
    data: uint64            // User data (typically the fd or a pointer)
}
