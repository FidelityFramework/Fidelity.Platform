/// Parse operations for Fidelity.Platform
/// String-to-numeric conversion using FNCS intrinsics.
/// Same compilation pattern as Format.int — F# code flows through FNCS → PSG → Baker → Alex.
/// NOTE: float is defined BEFORE int to avoid shadowing the built-in `int` conversion function.
module Parse

/// Convert string to float representation
/// Handles integer part and fractional part (arbitrary decimal places).
/// Must be defined before Parse.int so `int` conversion (char→int) resolves to built-in.
let float (s: string) : float =
    let len = String.length s
    let mutable pos = 0
    let isNeg = if len > 0 then String.charAt s 0 = '-' else false
    if isNeg then pos <- pos + 1

    let mutable intPart = 0
    let mutable inFrac = false
    let mutable fracPart = 0
    let mutable divisor = 1

    while pos < len do
        let c = String.charAt s pos
        if c = '.' then
            inFrac <- true
        else
            if inFrac then
                fracPart <- fracPart * 10 + ((int c) - 48)
                divisor <- divisor * 10
            else
                intPart <- intPart * 10 + ((int c) - 48)
        pos <- pos + 1

    let result = (float intPart) + (float fracPart) / (float divisor)
    if isNeg then 0.0 - result else result

/// Convert string to integer representation
/// Uses left-to-right digit accumulation — same primitives as Format.int in reverse.
let int (s: string) : int =
    let len = String.length s
    let mutable pos = 0
    let isNeg = if len > 0 then String.charAt s 0 = '-' else false
    if isNeg then pos <- pos + 1

    let mutable result = 0
    while pos < len do
        let c = String.charAt s pos
        result <- result * 10 + ((int c) - 48)
        pos <- pos + 1

    if isNeg then 0 - result else result
