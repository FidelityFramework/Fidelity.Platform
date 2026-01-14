/// Console I/O operations for Fidelity.Platform
/// These use FNCS intrinsics (Sys.read, NativePtr.*, NativeStr.fromPointer)
/// and flow through the normal compilation pipeline.
/// Alex witnesses the PSG structure including while loops - no imperative construction.
///
/// NOTE: Top-level module declaration provides "Console.readln" etc. directly matching
/// the FNCS intrinsic names for write/writeln/error/errorln. This allows user code to
/// call Console.* uniformly.
module Console

open BAREWire.Core.Capability

/// Standard file descriptors
[<Literal>]
let STDIN = 0

[<Literal>]
let STDOUT = 1

[<Literal>]
let STDERR = 2

/// Read a line from stdin into the provided buffer.
/// Returns the number of bytes read (excluding newline).
/// The loop structure flows through FNCS → PSG → Alex as a WhileLoop node.
let readLineInto (buffer: nativeptr<byte>) (maxLength: int) : int =
    let mutable pos = 0
    let mutable isDone = false
    // Single-byte buffer for reading one character at a time
    let charBuf = NativePtr.stackalloc<byte> 1

    while not isDone && pos < (maxLength - 1) do
        // Read one byte from stdin
        let bytesRead = Sys.read STDIN charBuf 1

        if bytesRead <= 0 then
            // EOF or error
            isDone <- true
        else
            let b = NativePtr.read charBuf
            if b = 10uy || b = 13uy then  // '\n' or '\r'
                isDone <- true
            else
                // Store byte in buffer and advance
                NativePtr.write (NativePtr.add buffer pos) b
                pos <- pos + 1

    pos

/// Read a line from stdin, returning it as a string.
/// WARNING: Allocates on readln's stack - returned string is INVALID after return!
/// Use readlnFrom with an arena for strings that must outlive this call.
let readln () : string =
    let buffer = NativePtr.stackalloc<byte> 256
    let len = readLineInto buffer 256
    NativeStr.fromPointer buffer len

/// Read a line from stdin using arena for string storage.
/// The string data lives in the arena, surviving this function's return.
/// This is the safe alternative to readln() for strings that must outlive the call.
let readlnFrom (arena: byref<Arena<'lifetime>>) : string =
    let buffer = Arena.alloc &arena 256
    let bufferPtr = NativePtr.ofNativeInt<byte> buffer
    let len = readLineInto bufferPtr 256
    NativeStr.fromPointer bufferPtr len

/// Write a string to stdout.
let write (s: string) : unit =
    // string in Fidelity is a fat pointer: (ptr, len)
    // Access via .Pointer and .Length members
    let _ = Sys.write STDOUT s.Pointer s.Length
    ()

/// Write a string to stdout followed by newline.
let writeln (s: string) : unit =
    write s
    let newline = NativePtr.stackalloc<byte> 1
    NativePtr.write newline 10uy
    let _ = Sys.write STDOUT newline 1
    ()

/// Write a string to stderr.
let error (s: string) : unit =
    let _ = Sys.write STDERR s.Pointer s.Length
    ()

/// Write a string to stderr followed by newline.
let errorln (s: string) : unit =
    error s
    let newline = NativePtr.stackalloc<byte> 1
    NativePtr.write newline 10uy
    let _ = Sys.write STDERR newline 1
    ()
