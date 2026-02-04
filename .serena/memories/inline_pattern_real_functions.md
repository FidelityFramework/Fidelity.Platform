# Inline Pattern: Real Functions for Platform Libraries

**Date:** 2026-02-04  
**Pattern:** Platform library functions should be REAL FUNCTIONS, not inline-substituted wrappers.

---

## The Pattern

```fsharp
// ✅ CORRECT: Real function, MLIR decides optimization
let write (s: string) : unit =
    let _ = Sys.write STDOUT s
    ()

// ❌ WRONG: Inline creates VarRef wrappers during PSG construction
let inline write (s: string) : unit =
    let _ = Sys.write STDOUT s
    ()
```

---

## Why

1. **Multi-target flexibility**: Different backends (CPU/GPU/TPU/NPU/μC) make different inlining decisions
2. **Clean PSG structure**: No spurious VarRef wrapper nodes pointing to Literals
3. **F# idiom preservation**: Users write `Console.write`, compiler handles adaptation
4. **MLIR norms**: Portable intermediate representation, backend-specific optimization

---

## When to Use `inline`

**ONLY for SRTP (generic constraints):**
```fsharp
let inline add x y = x + y  // Requires (+) operator resolution
```

**NOT for platform adapters:**
```fsharp
let write s = ...  // Omit inline - let MLIR decide
```

---

## Reference

See `inline_architecture_real_functions_mlir_norms` memory in FNCS and Firefly repos for full architectural rationale.

**Golden Rule:** Platform libraries adapt F# idioms to targets. Real functions preserve flexibility.
