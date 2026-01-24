# Quotation Binding Pattern

## Context

Fidelity.Platform uses F# quotations to express platform bindings that Alex witnesses into MLIR.

## Decision

Platform bindings are expressed as **quotations with extern declarations**:

```fsharp
// In GTKConduits.fs
[<Extern("gtk_window_new")>]
let gtkWindowNew: unit -> nativeptr<GtkWindow> = 
    <@ fun () -> Unchecked.defaultof<_> @>

// The quotation body is never executed - Alex sees the Extern attribute
// and generates an FFI call
```

## Pattern Components

1. **Extern attribute**: Marks function as external, provides symbol name
2. **Type signature**: Defines the FFI interface (return type, parameters)
3. **Quotation body**: Placeholder (defaultof), never executed
4. **Alex witnessing**: Extern functions generate `llvm.call` to external symbol

## Rationale

1. **Type-safe FFI**: F# type system validates binding signatures
2. **No runtime overhead**: Quotations are compile-time only
3. **Declarative**: Bindings are data, not code
4. **Inspectable**: Can analyze bindings before code generation

## MLIR Generation

```fsharp
// F# binding
[<Extern("gtk_window_new")>]
let gtkWindowNew: unit -> nativeptr<GtkWindow> = ...

// Generated MLIR
llvm.func @gtk_window_new() -> !llvm.ptr attributes {sym_visibility = "private"}
// ... at call site:
%window = llvm.call @gtk_window_new() : () -> !llvm.ptr
```

## Related

- `platform_role` memory
- `/home/hhh/repos/Firefly/docs/Quotation_Based_Memory_Architecture.md`
