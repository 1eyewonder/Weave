namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Drawer =

  /// <summary>
  /// Controls how the drawer is displayed and interacts with the layout.
  /// </summary>
  [<RequireQualifiedAccess; Struct>]
  type Variant =
    /// <summary>
    /// Overlays the content with an optional backdrop. Does not shift the
    /// AppBar or main content.
    /// </summary>
    | Temporary
    /// <summary>
    /// Always visible at its configured width; shifts the AppBar and main
    /// content to the side.
    /// </summary>
    | Persistent
    /// <summary>
    /// Behaves like <c>Persistent</c> above its breakpoint and like
    /// <c>Temporary</c> below it.
    /// </summary>
    | Responsive
    /// <summary>
    /// Collapses to a narrow icon strip and expands to full width on open.
    /// Layout always reserves the mini-strip width.
    /// </summary>
    | Mini

  /// <summary>The edge of the container the drawer slides from.</summary>
  [<RequireQualifiedAccess; Struct>]
  type Position =
    | Left
    | Right

  /// <summary>Controls how the drawer interacts with the AppBar vertically.</summary>
  [<RequireQualifiedAccess; Struct>]
  type ClipMode =
    /// <summary>
    /// Drawer starts below the AppBar — its top edge is offset by the AppBar
    /// height so the AppBar remains fully visible above the drawer.
    /// </summary>
    | AppBar
    /// <summary>
    /// Drawer renders at full height, overlapping the AppBar. The AppBar is
    /// not pushed and the drawer extends to the top of the container.
    /// </summary>
    | FullHeight

  /// <summary>
  /// Breakpoint at which Responsive and Mini drawers switch between their
  /// two states. Use <c>None</c> to always stay in compact/temporary mode
  /// or <c>Always</c> to always stay expanded.
  /// </summary>
  [<RequireQualifiedAccess>]
  type DrawerBreakpoint =
    /// <summary>Drawer switches at the specified breakpoint.</summary>
    | At of Breakpoint
    /// <summary>Drawer is always in compact/temporary state — never pushes layout.</summary>
    | None
    /// <summary>Drawer is always expanded — always pushes layout.</summary>
    | Always

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Temporary -> Css.``weave-drawer--temporary``
      | Variant.Persistent -> Css.``weave-drawer--persistent``
      | Variant.Responsive -> Css.``weave-drawer--responsive``
      | Variant.Mini -> Css.``weave-drawer--mini``

    let toOverlayClass variant =
      match variant with
      | Variant.Temporary -> Some Css.``weave-drawer__overlay--temporary``
      | Variant.Responsive -> Some Css.``weave-drawer__overlay--responsive``
      | Variant.Mini -> Some Css.``weave-drawer__overlay--mini``
      | Variant.Persistent -> Option.None

  module Position =

    let toClass position =
      match position with
      | Position.Left -> Css.``weave-drawer--pos-left``
      | Position.Right -> Css.``weave-drawer--pos-right``

  module ClipMode =

    let toDrawerClass clipMode =
      match clipMode with
      | ClipMode.AppBar -> Some Css.``weave-drawer--clip-appbar``
      | ClipMode.FullHeight -> Some Css.``weave-drawer--clip-fullheight``

  module DrawerBreakpoint =

    let toDrawerClass =
      function
      | DrawerBreakpoint.At bp ->
        match bp with
        | Breakpoint.ExtraSmall -> Some Css.``weave-drawer--xs``
        | Breakpoint.Small -> Some Css.``weave-drawer--sm``
        | Breakpoint.Medium -> Some Css.``weave-drawer--md``
        | Breakpoint.Large -> Some Css.``weave-drawer--lg``
        | Breakpoint.ExtraLarge -> Some Css.``weave-drawer--xl``
        | Breakpoint.ExtraExtraLarge -> Some Css.``weave-drawer--xxl``
      | DrawerBreakpoint.None
      | DrawerBreakpoint.Always -> Option.None

    let toOverlayClass =
      function
      | DrawerBreakpoint.At bp ->
        match bp with
        | Breakpoint.ExtraSmall -> Some Css.``weave-drawer__overlay--xs``
        | Breakpoint.Small -> Some Css.``weave-drawer__overlay--sm``
        | Breakpoint.Medium -> Some Css.``weave-drawer__overlay--md``
        | Breakpoint.Large -> Some Css.``weave-drawer__overlay--lg``
        | Breakpoint.ExtraLarge -> Some Css.``weave-drawer__overlay--xl``
        | Breakpoint.ExtraExtraLarge -> Some Css.``weave-drawer__overlay--xxl``
      | DrawerBreakpoint.None
      | DrawerBreakpoint.Always -> Option.None

    let toContainerClass =
      function
      | DrawerBreakpoint.At bp ->
        match bp with
        | Breakpoint.ExtraSmall -> Css.``weave-drawer-container--bp-xs``
        | Breakpoint.Small -> Css.``weave-drawer-container--bp-sm``
        | Breakpoint.Medium -> Css.``weave-drawer-container--bp-md``
        | Breakpoint.Large -> Css.``weave-drawer-container--bp-lg``
        | Breakpoint.ExtraLarge -> Css.``weave-drawer-container--bp-xl``
        | Breakpoint.ExtraExtraLarge -> Css.``weave-drawer-container--bp-xxl``
      | DrawerBreakpoint.None -> Css.``weave-drawer-container--bp-none``
      | DrawerBreakpoint.Always -> Css.``weave-drawer-container--bp-always``

  module Density =

    let compact = cl Css.``weave-drawer__header--compact``
    let standard = cl Css.``weave-drawer__header--standard``
    let spacious = cl Css.``weave-drawer__header--spacious``

  /// <summary>
  /// An immutable specification for a single drawer pane. Construct one via
  /// <c>Drawer.Create</c> and pass it to <c>DrawerContainer.Create</c> or
  /// <c>Drawer.Render</c>.
  /// </summary>
  type Config = {
    Content: Doc
    Open: View<bool>
    Variant: Variant
    Position: Position
    ClipMode: ClipMode
    Breakpoint: DrawerBreakpoint
    OverlayClose: (unit -> unit) option
    IsFixed: bool
    Width: int option
    Header: Doc option
    /// <summary>
    /// When <c>true</c>, the mini variant expands to full width on pointer
    /// hover without shifting the layout (overlay behaviour). Has no effect
    /// on other variants.
    /// </summary>
    ExpandOnHover: View<bool>
    Attrs: Attr list
  }

open Drawer

[<JavaScript>]
type DrawerHeader =

  /// <summary>
  /// Creates an optional header row inside a drawer. Its minimum height
  /// matches the AppBar so the two align when both are visible.
  /// </summary>
  /// <param name="content">The content to render inside the header.</param>
  /// <param name="attrs">Additional attributes applied to the root element.</param>
  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    div [ cl Css.``weave-drawer__header``; yield! attrs ] [ content ]

[<JavaScript>]
type Drawer =

  /// <summary>
  /// Constructs a <c>Drawer.Config</c> for the given variant. Pass the result
  /// to <c>DrawerContainer.create</c> or render it directly with
  /// <c>Drawer.Render</c>.
  /// </summary>
  /// <param name="content">The content to render inside the drawer.</param>
  /// <param name="isOpen">A view that controls whether the drawer is open.</param>
  /// <param name="variant">
  /// Controls how the drawer displays and interacts with the layout.
  /// Defaults to <c>Temporary</c>.
  /// </param>
  /// <param name="position">The edge the drawer slides from. Defaults to <c>Left</c>.</param>
  /// <param name="clipMode">
  /// How the drawer interacts with the AppBar vertically. Relevant for
  /// <c>Persistent</c>. Use <c>ClipMode.AppBar</c> when the AppBar lives
  /// <em>inside</em> the DrawerContainer so the drawer starts below it.
  /// Defaults to <c>FullHeight</c>.
  /// </param>
  /// <param name="breakpoint">
  /// Breakpoint at which <c>Responsive</c> and <c>Mini</c> drawers switch
  /// between their two states. Defaults to <c>MD</c>.
  /// </param>
  /// <param name="overlayClose">
  /// Optional callback invoked when the user clicks the overlay backdrop.
  /// Relevant for <c>Temporary</c> and <c>Responsive</c>.
  /// </param>
  /// <param name="expandOnHover">
  /// When true the <c>Mini</c> drawer expands to full width on pointer hover
  /// without shifting the layout. Defaults to false.
  /// </param>
  /// <param name="isFixed">
  /// When true the drawer is positioned relative to the viewport. Defaults to true.
  /// </param>
  /// <param name="width">Optional custom width in pixels, overriding the CSS custom property.</param>
  /// <param name="header">
  /// Optional header Doc rendered above the drawer content.
  /// Use <c>DrawerHeader.create</c> to build one.
  /// </param>
  /// <param name="attrs">Additional attributes applied to the drawer root element.</param>
  static member create
    (
      content: Doc,
      isOpen: View<bool>,
      ?variant: Drawer.Variant,
      ?position: Drawer.Position,
      ?clipMode: Drawer.ClipMode,
      ?breakpoint: Drawer.DrawerBreakpoint,
      ?overlayClose: unit -> unit,
      ?expandOnHover: View<bool>,
      ?isFixed: bool,
      ?width: int,
      ?header: Doc,
      ?attrs: Attr list
    ) : Drawer.Config =
    {
      Content = content
      Open = isOpen
      Variant = defaultArg variant Variant.Temporary
      Position = defaultArg position Position.Left
      ClipMode = defaultArg clipMode ClipMode.FullHeight
      Breakpoint = defaultArg breakpoint (DrawerBreakpoint.At Breakpoint.Medium)
      OverlayClose = overlayClose
      IsFixed = defaultArg isFixed true
      Width = width
      Header = header
      ExpandOnHover = defaultArg expandOnHover (View.Const false)
      Attrs = defaultArg attrs []
    }

  /// <summary>
  /// Internal helper. Builds the drawer panel and backdrop overlay as a
  /// <c>(drawerDoc, overlayDoc)</c> pair sharing the same animation-suppression
  /// state. <c>DrawerContainer.Create</c> uses this so the overlay can be
  /// rendered as a sibling of the drawer at container level — allowing
  /// <c>position: absolute; inset: 0</c> to fill the whole container rather
  /// than being clipped to the drawer's own constrained width.
  /// </summary>
  static member internal RenderPair(config: Drawer.Config) : Doc * Doc =
    // --initial suppresses slide animations on the very first render.
    // It is cleared after the first open/close interaction.
    let isInitial = Var.Create true
    let mutable firstEventSeen = false

    let trackInitial =
      Doc.sink
        (fun _ ->
          if firstEventSeen then
            Var.Set isInitial false
          else
            firstEventSeen <- true)
        config.Open

    let widthAttr =
      config.Width
      |> Option.map (fun w ->
        let prop =
          match config.Position with
          | Position.Left
          | Position.Right -> "--drawer-width"

        Attr.Style prop (sprintf "%dpx" w))
      |> Attr.bindOption id

    let overlayDoc =
      match config.Variant with
      | Variant.Persistent -> Doc.Empty
      | _ ->
        div [
          cls [
            Css.``weave-drawer__overlay``
            // Fixed drawers need position:fixed on the overlay so it covers
            // the viewport rather than just the nearest positioned ancestor.
            if config.IsFixed then
              Css.``weave-drawer__overlay--fixed``
            yield! Variant.toOverlayClass config.Variant |> Option.toList
            yield! DrawerBreakpoint.toOverlayClass config.Breakpoint |> Option.toList
          ]
          Attr.DynamicClassPred Css.``weave-drawer__overlay--open`` config.Open
          Attr.DynamicClassPred Css.``weave-drawer--initial`` isInitial.View
          on.click (fun _ _ -> config.OverlayClose |> Option.iter (fun close -> close ()))
        ] []

    let drawerDoc =
      div [
        cls [
          Css.``weave-drawer``
          Variant.toClass config.Variant
          Position.toClass config.Position
          if config.IsFixed then
            Css.``weave-drawer--fixed``
          yield! ClipMode.toDrawerClass config.ClipMode |> Option.toList
          yield! DrawerBreakpoint.toDrawerClass config.Breakpoint |> Option.toList
        ]
        widthAttr
        Attr.DynamicClassPred Css.``weave-drawer--open`` config.Open
        Attr.DynamicClassPred Css.``weave-drawer--closed`` (config.Open |> View.Map not)
        Attr.DynamicClassPred Css.``weave-drawer--initial`` isInitial.View
        Attr.DynamicClassPred Css.``weave-drawer--hover-expand`` config.ExpandOnHover
        yield! config.Attrs
      ] [
        trackInitial
        config.Header |> Option.defaultWith (fun () -> Doc.Empty)
        div [ cl Css.``weave-drawer__content`` ] [ config.Content ]
      ]

    drawerDoc, overlayDoc

  /// <summary>
  /// Renders a <c>Drawer.Config</c> into a <c>Doc</c>. For layout-aware usage
  /// — where the drawer shifts the AppBar and main content area — use
  /// <c>DrawerContainer.Create</c> instead, which calls this internally.
  /// </summary>
  static member Render(config: Drawer.Config) : Doc =
    let drawerDoc, overlayDoc = Drawer.RenderPair config
    Doc.Concat [ drawerDoc; overlayDoc ]

[<JavaScript>]
type DrawerContainer =

  /// <summary>
  /// Creates a full-page layout container that co-ordinates one or two side
  /// drawers with the AppBar and main content area. Modifier classes are
  /// applied reactively so the AppBar and main content shift correctly when
  /// drawers open or close.
  /// </summary>
  /// <remarks>
  /// The AppBar and main-content elements must be <b>direct children</b> of
  /// the <c>weave-drawer-container__main</c> slot rendered by this component.
  /// Pass them as siblings via <c>Doc.Concat</c> (rather than wrapping them in
  /// an extra <c>div</c>) so the push CSS selectors can reach them:
  /// <code>
  /// DrawerContainer.Create(
  ///     mainContent = Doc.Concat [
  ///         AppBar.Create(...)
  ///         div [ cl "weave-main-content" ] [ content ]
  ///     ], ...
  /// )
  /// </code>
  /// Using a wrapper <c>div</c> will prevent the AppBar and main content from
  /// being shifted when the drawer opens. Nested AppBars (e.g. inside example
  /// previews) are intentionally not affected because they sit deeper in the
  /// tree.
  /// Top and Bottom drawers are always overlay-only and therefore do not
  /// require a container; render them with <c>Drawer.Render</c> directly.
  /// </remarks>
  /// <param name="mainContent">
  /// The primary page content, including the AppBar if one is used.
  /// </param>
  /// <param name="leftDrawer">
  /// Optional left-side drawer config produced by <c>Drawer.Create</c>.
  /// </param>
  /// <param name="rightDrawer">
  /// Optional right-side drawer config produced by <c>Drawer.Create</c>.
  /// </param>
  /// <param name="attrs">Additional attributes applied to the container element.</param>
  static member create
    (mainContent: Doc, ?leftDrawer: Drawer.Config, ?rightDrawer: Drawer.Config, ?attrs: Attr list)
    : Doc =

    let attrs = defaultArg attrs []

    let drawerContainerAttrs (config: Drawer.Config) =
      let sideClass =
        match config.Position with
        | Position.Left -> Css.``weave-drawer-container--side-left``
        | Position.Right -> Css.``weave-drawer-container--side-right``

      let bpClass = DrawerBreakpoint.toContainerClass config.Breakpoint

      [
        cl sideClass
        cl bpClass

        Map [
          (true, Variant.Responsive), Css.``weave-drawer-container--open-responsive``
          (true, Variant.Persistent), Css.``weave-drawer-container--open-persistent``
          (true, Variant.Mini), Css.``weave-drawer-container--open-mini``
          (false, Variant.Mini), Css.``weave-drawer-container--closed-mini``
        ]
        |> Attr.classSelection (config.Open |> View.Map(fun o -> o, config.Variant))
      ]

    let containerAttrs: Attr list = [
      cl Css.``weave-drawer-container``
      yield! leftDrawer |> Option.map drawerContainerAttrs |> Option.defaultValue []
      yield! rightDrawer |> Option.map drawerContainerAttrs |> Option.defaultValue []
      yield! attrs
    ]

    // Render each drawer as a (drawerDoc, overlayDoc) pair so the backdrops
    // can be placed outside the drawer elements. An overlay inside a narrow
    // drawer div would be clipped to that width; as siblings in the container
    // they can use position:absolute; inset:0 to cover the full container.
    let leftPair = leftDrawer |> Option.map Drawer.RenderPair
    let rightPair = rightDrawer |> Option.map Drawer.RenderPair

    div containerAttrs [
      // Drawer panels
      leftPair |> Option.map fst |> Option.defaultValue Doc.Empty
      rightPair |> Option.map fst |> Option.defaultValue Doc.Empty
      // Overlay backdrops — siblings at container level, not inside the drawer
      leftPair |> Option.map snd |> Option.defaultValue Doc.Empty
      rightPair |> Option.map snd |> Option.defaultValue Doc.Empty
      div [ cl Css.``weave-drawer-container__main`` ] [ mainContent ]
    ]
