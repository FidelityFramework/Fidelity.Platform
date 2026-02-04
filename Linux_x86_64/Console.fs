/// Console I/O operations for Fidelity.Platform
/// These use FNCS intrinsics (Sys.read, NativePtr.*, NativeStr.fromPointer)
/// and flow through the normal compilation pipeline.
/// Alex witnesses the PSG structure including while loops - no imperative construction.
///
/// NOTE: Top-level module declaration provides "Console.readln" etc. directly matching
/// the FNCS intrinsic names for write/writeln/error/errorln. This allows user code to
/// call Console.* uniformly.
module Console

// NOTE: Arena<'lifetime> is an FNCS intrinsic - no BAREWire import needed

/// Standard file descriptors
[<Literal>]
let STDIN = 0

[<Literal>]
let STDOUT = 1

[<Literal>]
let STDERR = 2

/// Read a line from stdin, returning it as a string.
/// Directly uses Sys.read intrinsic - PSG enrichment, Baker saturation,
/// and Alex witnessing handle mutable state, control flow, and FFI conversion.
/// Allocation strategy (stack vs arena) determined by escape analysis.
let readln () : string =
    let buffer = NativePtr.stackalloc<byte> 256
    let mutable pos = 0
    let mutable isDone = false
    let charBuf = NativePtr.stackalloc<byte> 1

    while not isDone && pos < 255 do
        let bytesRead = Sys.read STDIN charBuf

        if bytesRead <= 0 then
            isDone <- true
        else
            let b = NativePtr.read charBuf
            if b = 10uy || b = 13uy then  // '\n' or '\r'
                isDone <- true
            else
                NativePtr.write (NativePtr.add buffer pos) b
                pos <- pos + 1

    NativeStr.fromPointer buffer



/// Write a string to stdout.
let write (s: string) : unit =
    // In MLIR: string IS memref<?xi8>
    // Sys.write will extract BOTH pointer and length from memref
    // Length extraction happens inside Sys.write pattern via memref.dim
    let _ = Sys.write STDOUT s
    ()

/// Write a string to stdout followed by newline.
let writeln (s: string) : unit =
    write s
    let newline = NativePtr.stackalloc<byte> 1
    NativePtr.write newline 10uy
    let _ = Sys.write STDOUT newline
    ()

/// Write a string to stderr.
let error (s: string) : unit =
    // In MLIR: string IS memref<?xi8>
    // Sys.write will extract BOTH pointer and length from memref
    let _ = Sys.write STDERR s
    ()

/// Write a string to stderr followed by newline.
let errorln (s: string) : unit =
    error s
    let newline = NativePtr.stackalloc<byte> 1
    NativePtr.write newline 10uy
    let _ = Sys.write STDERR newline
    ()
