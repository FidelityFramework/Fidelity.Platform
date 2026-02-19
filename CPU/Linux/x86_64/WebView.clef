namespace Fidelity.Platform.Bindings

/// WebView platform conduits.
/// Alex provides the implementation of these functions during code generation.
module WebViewConduits =
    
    /// Creates a new webview instance.
    let createWebview (debug: int) (window: nativeint) : nativeint = NativeDefault.zeroed<nativeint> ()
    
    /// Destroys a webview instance.
    let destroyWebview (webview: nativeint) : int = NativeDefault.zeroed<int> ()
    
    /// Runs the webview main loop (blocks).
    let runWebview (webview: nativeint) : int = NativeDefault.zeroed<int> ()
    
    /// Terminates the webview main loop.
    let terminateWebview (webview: nativeint) : int = NativeDefault.zeroed<int> ()
    
    /// Sets the webview window title.
    let setWebviewTitle (webview: nativeint) (title: string) : int = NativeDefault.zeroed<int> ()
    
    /// Sets the webview window size.
    let setWebviewSize (webview: nativeint) (width: int) (height: int) (hints: int) : int = NativeDefault.zeroed<int> ()
    
    /// Navigates the webview to a URL.
    let navigateWebview (webview: nativeint) (url: string) : int = NativeDefault.zeroed<int> ()
    
    /// Sets the webview HTML content.
    let setWebviewHtml (webview: nativeint) (html: string) : int = NativeDefault.zeroed<int> ()
    
    /// Injects JavaScript into the webview.
    let initWebview (webview: nativeint) (js: string) : int = NativeDefault.zeroed<int> ()
    
    /// Evaluates JavaScript in the webview.
    let evalWebview (webview: nativeint) (js: string) : int = NativeDefault.zeroed<int> ()
    
    /// Binds a native function to a JavaScript function.
    let bindWebview (webview: nativeint) (name: string) : int = NativeDefault.zeroed<int> ()
    
    /// Returns a result from a native function call to JavaScript.
    let returnWebview (webview: nativeint) (id: string) (status: int) (result: string) : int = NativeDefault.zeroed<int> ()