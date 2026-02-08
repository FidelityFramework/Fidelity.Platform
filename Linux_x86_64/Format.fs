/// Format operations for Fidelity.Platform
/// Numeric-to-string conversion using FNCS intrinsics.
/// Same compilation pattern as Console.write — F# code flows through FNCS → PSG → Baker → Alex.
/// NOTE: float is defined BEFORE int to avoid shadowing the built-in `int` conversion function.
module Format

/// Convert float to string representation (2 decimal places)
/// Uses right-to-left digit extraction with scaled integer arithmetic.
/// Must be defined before Format.int so `int` conversion (float→int) resolves to built-in.
let float (value: float) : string =
    let buf = NativePtr.stackalloc<byte> 32
    let mutable pos = 31n
    let mutable v = value
    let isNeg = v < 0.0
    if isNeg then
        v <- 0.0 - v

    // Scale to 2 decimal places and convert to integer arithmetic
    let scaled = int (v * 100.0)
    let fracDigits = scaled % 100
    let intPart = scaled / 100

    // Fractional digits (right-to-left)
    let d1 = fracDigits % 10
    let d0 = fracDigits / 10
    NativePtr.write (NativePtr.add buf pos) (byte (d1 + 48))
    pos <- pos - 1n
    NativePtr.write (NativePtr.add buf pos) (byte (d0 + 48))
    pos <- pos - 1n

    // Decimal point
    NativePtr.write (NativePtr.add buf pos) 46uy
    pos <- pos - 1n

    // Integer part (right-to-left)
    let mutable iv = intPart
    let mutable isDone = false
    if iv = 0 then
        NativePtr.write (NativePtr.add buf pos) 48uy
        pos <- pos - 1n
        isDone <- true
    while not isDone do
        let digit = iv % 10
        NativePtr.write (NativePtr.add buf pos) (byte (digit + 48))
        pos <- pos - 1n
        iv <- iv / 10
        if iv = 0 then
            isDone <- true

    // Sign
    if isNeg then
        NativePtr.write (NativePtr.add buf pos) 45uy
        pos <- pos - 1n

    let startPos = pos + 1n
    let len = 31n - pos
    NativeStr.fromPointer (NativePtr.add buf startPos) len

/// Convert integer to string representation
/// Uses right-to-left digit extraction — same primitives as Console.readln.
let int (value: int) : string =
    let buf = NativePtr.stackalloc<byte> 21
    let mutable pos = 20n
    let mutable v = if value < 0 then 0 - value else value
    let mutable isDone = false

    if v = 0 then
        NativePtr.write (NativePtr.add buf pos) 48uy
        pos <- pos - 1n
        isDone <- true
    while not isDone do
        let digit = v % 10
        NativePtr.write (NativePtr.add buf pos) (byte (digit + 48))
        pos <- pos - 1n
        v <- v / 10
        if v = 0 then
            isDone <- true

    if value < 0 then
        NativePtr.write (NativePtr.add buf pos) 45uy
        pos <- pos - 1n

    let startPos = pos + 1n
    let len = 20n - pos
    NativeStr.fromPointer (NativePtr.add buf startPos) len
