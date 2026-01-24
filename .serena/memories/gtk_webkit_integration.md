# GTK/WebKitGTK Integration

## Context

Fidelity.Platform provides GTK4 and WebKitGTK bindings for desktop UI with WebView support.

## Decision

Integrate with GTK4 and WebKitGTK via quotation-based FFI bindings:

- **GTK4**: Window management, widget hierarchy, event loop
- **WebKitGTK**: WebView widget for hosting web content

## Binding Structure

```fsharp
// GTKConduits.fs
module GTK =
    [<Extern("gtk_init")>]
    let init: unit -> unit = ...
    
    [<Extern("gtk_window_new")>]
    let windowNew: unit -> nativeptr<GtkWindow> = ...
    
    [<Extern("gtk_window_set_title")>]
    let windowSetTitle: nativeptr<GtkWindow> -> nativeptr<byte> -> unit = ...
    
    [<Extern("gtk_main")>]
    let main: unit -> unit = ...

// WebViewWrapper.fs
module WebView =
    [<Extern("webkit_web_view_new")>]
    let create: unit -> nativeptr<WebKitWebView> = ...
    
    [<Extern("webkit_web_view_load_html")>]
    let loadHtml: nativeptr<WebKitWebView> -> nativeptr<byte> -> nativeptr<byte> -> unit = ...
```

## Linking Requirements

Applications using these bindings must link against:
- `libgtk-4.so` (GTK4)
- `libwebkitgtk-6.0.so` (WebKitGTK 6.0)

Specified in `.fidproj`:
```toml
[build]
link_libraries = ["gtk-4", "webkitgtk-6.0"]
```

## Event Handling

GTK events flow through the main loop:
1. `GTK.main()` blocks and processes events
2. Signal handlers registered via `g_signal_connect`
3. Callbacks invoke F# functions via function pointers

## Related

- `quotation_binding_pattern` memory
- `/home/hhh/repos/Firefly/docs/WebView_Desktop_Architecture.md`
