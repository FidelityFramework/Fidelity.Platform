namespace Fidelity.Platform

/// High-level WebView API using GTK/WebKitGTK platform bindings.
/// This is the WREN stack's native backend entry point.
[<Struct>]
type WebView = {
    Window: nativeint
    Widget: nativeint
}

module WebView =

    /// Initialize GTK (call once at app startup)
    let initGTK () : unit =
        Bindings.gtkInit ()

    /// Create a new WebView window.
    let create (title: string) (width: int) (height: int) : WebView =
        // Create GTK window
        let window = Bindings.gtkWindowNew ()
        Bindings.gtkWindowSetTitle window title
        Bindings.gtkWindowSetDefaultSize window width height

        // Create WebKit widget
        let webview = Bindings.webViewNew ()

        // Enable developer tools
        let settings = Bindings.webViewGetSettings webview
        Bindings.settingsSetEnableDeveloperExtras settings true

        // Add WebView to window
        Bindings.gtkContainerAdd window webview

        { Window = window; Widget = webview }

    /// Load a URL in the WebView
    let loadUri (wv: WebView) (uri: string) : unit =
        Bindings.webViewLoadUri wv.Widget uri

    /// Load HTML content directly
    let loadHtml (wv: WebView) (html: string) : unit =
        Bindings.webViewLoadHtml wv.Widget html ""

    /// Execute JavaScript in the WebView
    let executeScript (wv: WebView) (script: string) : unit =
        Bindings.webViewRunJavaScript wv.Widget script

    /// Show the window and run GTK main loop (blocks)
    let run (wv: WebView) : unit =
        Bindings.gtkWidgetShowAll wv.Window
        Bindings.gtkMain ()

    /// Quit the GTK main loop
    let quit () : unit =
        Bindings.gtkMainQuit ()
