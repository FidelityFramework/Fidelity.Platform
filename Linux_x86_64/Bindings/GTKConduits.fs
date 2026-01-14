namespace Fidelity.Platform

/// GTK3 and WebKitGTK platform conduits for Linux.
/// Uses FnPtr.fromSymbol for FFI calls per fsnative-spec/spec/ffi-boundary.md
///
/// Design:
/// - Private FnPtr declarations bind to C symbols at link time
/// - Public functions invoke through the pointers
/// - Option<nativeptr<T>> used for nullable C pointers (None ↔ NULL)
/// - nativeint used for opaque GTK widget handles
module Bindings =

    // ═══════════════════════════════════════════════════════════════════════════
    // GTK3 CORE BINDINGS
    // ═══════════════════════════════════════════════════════════════════════════

    // gtk_init(int *argc, char ***argv) - both nullable
    let private gtk_init_ptr =
        FnPtr.fromSymbol<Option<nativeptr<int>> -> Option<nativeptr<nativeptr<nativeptr<byte>>>> -> unit> "gtk_init"

    /// GTK initialization - call once at startup
    let gtkInit () : unit =
        FnPtr.invoke gtk_init_ptr None None

    // gtk_window_new(GtkWindowType type) -> GtkWidget*
    // GtkWindowType: GTK_WINDOW_TOPLEVEL = 0
    let private gtk_window_new_ptr =
        FnPtr.fromSymbol<int -> nativeint> "gtk_window_new"

    /// Create a new GTK window (toplevel)
    let gtkWindowNew () : nativeint =
        FnPtr.invoke gtk_window_new_ptr 0  // GTK_WINDOW_TOPLEVEL

    // gtk_window_set_title(GtkWindow *window, const gchar *title)
    let private gtk_window_set_title_ptr =
        FnPtr.fromSymbol<nativeint -> nativeptr<byte> -> unit> "gtk_window_set_title"

    /// Set window title
    let gtkWindowSetTitle (window: nativeint) (title: string) : unit =
        FnPtr.invoke gtk_window_set_title_ptr window title.Pointer

    // gtk_window_set_default_size(GtkWindow *window, gint width, gint height)
    let private gtk_window_set_default_size_ptr =
        FnPtr.fromSymbol<nativeint -> int -> int -> unit> "gtk_window_set_default_size"

    /// Set window default size
    let gtkWindowSetDefaultSize (window: nativeint) (width: int) (height: int) : unit =
        FnPtr.invoke gtk_window_set_default_size_ptr window width height

    // gtk_container_add(GtkContainer *container, GtkWidget *widget)
    let private gtk_container_add_ptr =
        FnPtr.fromSymbol<nativeint -> nativeint -> unit> "gtk_container_add"

    /// Add a widget to a container
    let gtkContainerAdd (container: nativeint) (widget: nativeint) : unit =
        FnPtr.invoke gtk_container_add_ptr container widget

    // gtk_widget_show_all(GtkWidget *widget)
    let private gtk_widget_show_all_ptr =
        FnPtr.fromSymbol<nativeint -> unit> "gtk_widget_show_all"

    /// Show all widgets in a container
    let gtkWidgetShowAll (widget: nativeint) : unit =
        FnPtr.invoke gtk_widget_show_all_ptr widget

    // gtk_main(void)
    let private gtk_main_ptr =
        FnPtr.fromSymbol<unit -> unit> "gtk_main"

    /// Run the GTK main loop (blocks)
    let gtkMain () : unit =
        FnPtr.invoke gtk_main_ptr ()

    // gtk_main_quit(void)
    let private gtk_main_quit_ptr =
        FnPtr.fromSymbol<unit -> unit> "gtk_main_quit"

    /// Quit the GTK main loop
    let gtkMainQuit () : unit =
        FnPtr.invoke gtk_main_quit_ptr ()

    // ═══════════════════════════════════════════════════════════════════════════
    // WEBKIT2GTK BINDINGS
    // ═══════════════════════════════════════════════════════════════════════════

    // webkit_web_view_new(void) -> WebKitWebView*
    let private webkit_web_view_new_ptr =
        FnPtr.fromSymbol<unit -> nativeint> "webkit_web_view_new"

    /// Create a new WebView widget
    let webViewNew () : nativeint =
        FnPtr.invoke webkit_web_view_new_ptr ()

    // webkit_web_view_load_uri(WebKitWebView *web_view, const gchar *uri)
    let private webkit_web_view_load_uri_ptr =
        FnPtr.fromSymbol<nativeint -> nativeptr<byte> -> unit> "webkit_web_view_load_uri"

    /// Load a URI in the WebView
    let webViewLoadUri (webview: nativeint) (uri: string) : unit =
        FnPtr.invoke webkit_web_view_load_uri_ptr webview uri.Pointer

    // webkit_web_view_load_html(WebKitWebView *web_view, const gchar *content, const gchar *base_uri)
    // base_uri can be NULL
    let private webkit_web_view_load_html_ptr =
        FnPtr.fromSymbol<nativeint -> nativeptr<byte> -> Option<nativeptr<byte>> -> unit> "webkit_web_view_load_html"

    /// Load HTML content directly
    let webViewLoadHtml (webview: nativeint) (html: string) (baseUri: string) : unit =
        let baseUriPtr = if baseUri.Length = 0 then None else Some baseUri.Pointer
        FnPtr.invoke webkit_web_view_load_html_ptr webview html.Pointer baseUriPtr

    // webkit_web_view_run_javascript(WebKitWebView *web_view, const gchar *script,
    //                                GCancellable *cancellable, GAsyncReadyCallback callback, gpointer user_data)
    // Simplified: cancellable, callback, user_data all NULL for sync-style usage
    let private webkit_web_view_run_javascript_ptr =
        FnPtr.fromSymbol<nativeint -> nativeptr<byte> -> Option<nativeint> -> Option<nativeint> -> Option<nativeint> -> unit> "webkit_web_view_run_javascript"

    /// Execute JavaScript in the WebView
    let webViewRunJavaScript (webview: nativeint) (script: string) : unit =
        FnPtr.invoke webkit_web_view_run_javascript_ptr webview script.Pointer None None None

    // webkit_web_view_get_settings(WebKitWebView *web_view) -> WebKitSettings*
    let private webkit_web_view_get_settings_ptr =
        FnPtr.fromSymbol<nativeint -> nativeint> "webkit_web_view_get_settings"

    /// Get the WebView settings object
    let webViewGetSettings (webview: nativeint) : nativeint =
        FnPtr.invoke webkit_web_view_get_settings_ptr webview

    // webkit_settings_set_enable_developer_extras(WebKitSettings *settings, gboolean enabled)
    let private webkit_settings_set_enable_developer_extras_ptr =
        FnPtr.fromSymbol<nativeint -> int -> unit> "webkit_settings_set_enable_developer_extras"

    /// Enable developer extras (inspector)
    let settingsSetEnableDeveloperExtras (settings: nativeint) (enable: bool) : unit =
        let enabled = if enable then 1 else 0
        FnPtr.invoke webkit_settings_set_enable_developer_extras_ptr settings enabled

    // ═══════════════════════════════════════════════════════════════════════════
    // GLIB BINDINGS (for thread-safe dispatch)
    // ═══════════════════════════════════════════════════════════════════════════

    // g_idle_add(GSourceFunc function, gpointer data) -> guint
    // GSourceFunc is: gboolean (*)(gpointer)
    let private g_idle_add_ptr =
        FnPtr.fromSymbol<nativeint -> nativeint -> uint32> "g_idle_add"

    /// Add a callback to be called from GTK main loop (thread-safe dispatch)
    /// callback: GSourceFunc pointer (function returning gboolean)
    /// data: user data pointer passed to callback
    /// Returns: source ID (can be used to remove with g_source_remove)
    let gIdleAdd (callback: nativeint) (data: nativeint) : uint32 =
        FnPtr.invoke g_idle_add_ptr callback data
