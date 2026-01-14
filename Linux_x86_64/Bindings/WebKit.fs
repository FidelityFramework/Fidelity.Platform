namespace Fidelity.Platform.Bindings

/// WebKitGTK platform conduits for Linux.
///
/// NOTE: Currently using linker-based dynamic linking (-lwebkit2gtk-4.1).
/// The library is resolved at load time by the system's ld.so.
///
/// The "Bindings" namespace signals to FNCS that these are platform bindings.
/// Alex provides the actual implementations.
module WebKit =

    // ===================================================================
    // WebKitGTK Function Conduits
    // These are replaced by Alex bindings at compile time.
    // ===================================================================

    /// Create a new WebView widget
    let webViewNew () : nativeint = NativeDefault.zeroed<nativeint> ()

    /// Load a URI in the WebView
    let webViewLoadUri (webview: nativeint) (uri: string) : unit = NativeDefault.zeroed<unit> ()

    /// Load HTML content directly
    let webViewLoadHtml (webview: nativeint) (html: string) (baseUri: string) : unit = NativeDefault.zeroed<unit> ()

    /// Execute JavaScript in the WebView
    let webViewRunJavaScript (webview: nativeint) (script: string) : unit = NativeDefault.zeroed<unit> ()

    /// Get the WebView settings object
    let webViewGetSettings (webview: nativeint) : nativeint = NativeDefault.zeroed<nativeint> ()

    /// Enable developer extras (inspector)
    let settingsSetEnableDeveloperExtras (settings: nativeint) (enable: bool) : unit = NativeDefault.zeroed<unit> ()
