namespace Fidelity.Platform.Bindings

/// High-level WebView API using dynamic GTK/WebKitGTK bindings.
/// This is the WREN stack's native backend entry point.
///
/// Architecture:
/// 1. Native binary creates GTK window + WebKitGTK widget
/// 2. WebSocket server listens on localhost (future)
/// 3. Frontend HTML/JS loads in WebView
/// 4. Frontend connects back via WebSocket (future)
/// 5. BAREWire protocol handles typed communication (future)
[<Struct>]
type WebView = {
    Window: nativeint
    Widget: nativeint
}

module WebView =

    /// Initialize GTK (call once at app startup)
    let initGTK () : unit =
        GTK.init ()

    /// Create a new WebView window.
    /// Returns a WebView record containing the GTK window and WebKit widget handles.
    let create (title: string) (width: int) (height: int) : WebView =
        // Create GTK window
        let window = GTK.windowNew ()
        GTK.windowSetTitle window title
        GTK.windowSetDefaultSize window width height

        // Create WebKit widget
        let webview = WebKit.webViewNew ()

        // Enable developer tools
        let settings = WebKit.webViewGetSettings webview
        WebKit.settingsSetEnableDeveloperExtras settings true

        // Add WebView to window
        GTK.containerAdd window webview

        { Window = window; Widget = webview }

    /// Load a URL in the WebView
    let loadUri (wv: WebView) (uri: string) : unit =
        WebKit.webViewLoadUri wv.Widget uri

    /// Load HTML content directly
    let loadHtml (wv: WebView) (html: string) : unit =
        WebKit.webViewLoadHtml wv.Widget html ""

    /// Execute JavaScript in the WebView
    let executeScript (wv: WebView) (script: string) : unit =
        WebKit.webViewRunJavaScript wv.Widget script

    /// Show the window and run GTK main loop (blocks)
    let run (wv: WebView) : unit =
        GTK.widgetShowAll wv.Window
        GTK.main ()

    /// Quit the GTK main loop
    let quit () : unit =
        GTK.mainQuit ()
