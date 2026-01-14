namespace Fidelity.Platform.Bindings

/// GTK3 platform conduits for Linux.
///
/// NOTE: Currently using linker-based dynamic linking (-lgtk-3).
/// The library is resolved at load time by the system's ld.so.
///
/// The "Bindings" namespace signals to FNCS that these are platform bindings.
/// Alex provides the actual implementations.
module GTK =

    // ===================================================================
    // GTK Function Conduits
    // These are replaced by Alex bindings at compile time.
    // ===================================================================

    /// Initialize GTK (call once at startup)
    let init () : unit = NativeDefault.zeroed<unit> ()

    /// Create a new top-level window
    let windowNew () : nativeint = NativeDefault.zeroed<nativeint> ()

    /// Set window title
    let windowSetTitle (window: nativeint) (title: string) : unit = NativeDefault.zeroed<unit> ()

    /// Set window default size
    let windowSetDefaultSize (window: nativeint) (width: int) (height: int) : unit = NativeDefault.zeroed<unit> ()

    /// Add a widget to a container
    let containerAdd (container: nativeint) (widget: nativeint) : unit = NativeDefault.zeroed<unit> ()

    /// Show all widgets in a container
    let widgetShowAll (widget: nativeint) : unit = NativeDefault.zeroed<unit> ()

    /// Run the GTK main loop (blocks)
    let main () : unit = NativeDefault.zeroed<unit> ()

    /// Quit the GTK main loop
    let mainQuit () : unit = NativeDefault.zeroed<unit> ()
