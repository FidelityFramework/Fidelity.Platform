namespace Fidelity.Platform.WebView

open Fidelity.Platform.Bindings

/// High-level wrapper for the WebView platform surface.
[<Struct>]
type WebView = { Handle: nativeint }

module WebView =
    /// Creates a new webview instance.
    let create (debug: bool) : WebView =
        let debugFlag = if debug then 1 else 0
        let handle = WebViewConduits.createWebview debugFlag 0n
        { Handle = handle }

    /// Sets the window title.
    let setTitle (w: WebView) (title: string) =
        WebViewConduits.setWebviewTitle w.Handle title |> ignore

    /// Sets the window size.
    let setSize (w: WebView) (width: int) (height: int) (isFixed: bool) =
        let hints = if isFixed then 3 else 0
        WebViewConduits.setWebviewSize w.Handle width height hints |> ignore

    /// Loads HTML directly from a string.
    let setHtml (w: WebView) (html: string) =
        WebViewConduits.setWebviewHtml w.Handle html |> ignore

    /// Navigates to a URL.
    let navigate (w: WebView) (url: string) =
        WebViewConduits.navigateWebview w.Handle url |> ignore

    /// Binds a native callback to a JS function name.
    let bind (w: WebView) (name: string) =
        WebViewConduits.bindWebview w.Handle name |> ignore

    /// Returns a result to a pending JS call.
    let return' (w: WebView) (id: string) (status: int) (result: string) =
        WebViewConduits.returnWebview w.Handle id status result |> ignore

    /// Runs the WebView event loop (Blocks).
    let run (w: WebView) =
        WebViewConduits.runWebview w.Handle |> ignore

    /// Terminates the WebView.
    let terminate (w: WebView) =
        WebViewConduits.terminateWebview w.Handle |> ignore

    /// Destroys the WebView instance.
    let destroy (w: WebView) =
        WebViewConduits.destroyWebview w.Handle |> ignore
