namespace Fidelity.Platform.Bindings

/// Dynamic library loading conduits.
/// These wrap libc's dlopen/dlsym for runtime library loading.
/// Alex provides the implementation during code generation.
module DynamicLibConduits =

    /// dlopen flags
    [<Literal>]
    let RTLD_LAZY = 0x00001
    [<Literal>]
    let RTLD_NOW = 0x00002
    [<Literal>]
    let RTLD_GLOBAL = 0x00100

    /// Opens a dynamic library by path.
    /// Returns a handle (nativeint) or null pointer on failure.
    let dlopen (path: string) (flags: int) : nativeint = NativeDefault.zeroed<nativeint> ()

    /// Looks up a symbol in a loaded library.
    /// Returns a function pointer (nativeint) or null on failure.
    let dlsym (handle: nativeint) (symbol: string) : nativeint = NativeDefault.zeroed<nativeint> ()

    /// Closes a dynamic library handle.
    let dlclose (handle: nativeint) : int = NativeDefault.zeroed<int> ()

    /// Returns the last error message from dlopen/dlsym.
    let dlerror () : nativeint = NativeDefault.zeroed<nativeint> ()
