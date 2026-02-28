namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module ButtonMenu =

  [<RequireQualifiedAccess; Struct>]
  type Direction =
    | Top
    | Bottom
    | Left
    | Right

  module Direction =

    let toClass direction =
      match direction with
      | Direction.Top -> Css.``weave-button-menu--top``
      | Direction.Bottom -> Css.``weave-button-menu--bottom``
      | Direction.Left -> Css.``weave-button-menu--left``
      | Direction.Right -> Css.``weave-button-menu--right``

open ButtonMenu

[<JavaScript>]
module private ButtonMenuInternal =

  let renderMenu
    (triggerButton: Doc)
    (items: Doc list)
    (direction: ButtonMenu.Direction)
    (isOpen: Var<bool>)
    (openOnHover: View<bool>)
    (attrs: Attr list)
    =

    let mutable containerEl = ref (JS.Document.CreateElement "div")
    let mutable outsideClickHandler: Option<Dom.Event -> unit> = None
    let currentHover = Var.Create false
    let hoverSync = openOnHover |> Doc.sinkCached (fun v -> currentHover.Value <- v)

    let attachOutsideClick () =
      let handler (e: Dom.Event) =
        let target = e.Target :?> Dom.Element

        let isInside =
          match containerEl.Value with
          | null -> false
          | root -> root.Contains target

        if not isInside then
          isOpen.Value <- false

      JS.Document.AddEventListener("click", handler)
      outsideClickHandler <- Some handler

    let detachOutsideClick () =
      match outsideClickHandler with
      | Some handler ->
        JS.Document.RemoveEventListener("click", handler)
        outsideClickHandler <- None
      | None -> ()

    let menuItems =
      items
      |> List.mapi (fun i item ->
        div [
          cl Css.``weave-button-menu__item``
          Attr.Style "transition-delay" (sprintf "%dms" (i * 50))
        ] [ item ])

    let outsideClickWatcher =
      isOpen.View
      |> Doc.sinkCached (fun o -> if o then attachOutsideClick () else detachOutsideClick ())

    div [
      cl Css.``weave-button-menu``
      Direction.toClass direction |> cl
      isOpen.View |> Attr.DynamicClassPred Css.``weave-button-menu--open``
      on.afterRender (fun el -> containerEl.Value <- el)

      on.mouseEnter (fun _ _ ->
        if currentHover.Value then
          isOpen.Value <- true)
      on.mouseLeave (fun _ _ ->
        if currentHover.Value then
          isOpen.Value <- false)

      yield! attrs
    ] [
      outsideClickWatcher
      hoverSync
      div [ cl Css.``weave-button-menu__items`` ] menuItems
      triggerButton
    ]

[<JavaScript>]
type ButtonMenu =

  /// Creates a button menu with an icon button trigger.
  /// Pass closedIcon for the default icon, and optionally openIcon for a different icon when open.
  /// If openIcon is not provided, the closedIcon will rotate 45 degrees when opened.
  static member CreateIcon
    (
      closedIcon: Doc,
      items: Doc list,
      ?openIcon: Doc,
      ?direction: ButtonMenu.Direction,
      ?isOpen: Var<bool>,
      ?openOnHover: View<bool>,
      ?triggerAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let direction = defaultArg direction Direction.Top
    let isOpen = defaultArg isOpen (Var.Create false)
    let openOnHover = defaultArg openOnHover (View.Const false)
    let triggerAttrs = defaultArg triggerAttrs []
    let attrs = defaultArg attrs []

    let triggerIcon =
      match openIcon with
      | Some oi ->
        div [ cl Css.``weave-button-menu__trigger-icons`` ] [
          div [
            cl Css.``weave-button-menu__trigger-icon``
            cl Css.``weave-button-menu__trigger-icon--closed``
          ] [ closedIcon ]
          div [
            cl Css.``weave-button-menu__trigger-icon``
            cl Css.``weave-button-menu__trigger-icon--open``
          ] [ oi ]
        ]
      | None -> closedIcon

    let triggerButton =
      Button.CreateIcon(
        triggerIcon,
        onClick = (fun () -> isOpen.Value <- not isOpen.Value),
        attrs = [
          cl Css.``weave-button-menu__trigger``

          match openIcon with
          | None -> isOpen.View |> Attr.DynamicClassPred Css.``weave-button-menu__trigger--rotated``
          | Some _ -> Attr.Empty

          yield! triggerAttrs
        ]
      )

    ButtonMenuInternal.renderMenu triggerButton items direction isOpen openOnHover attrs

  /// Creates a button menu with a standard text button trigger.
  /// Pass closedContent for the default button text, and optionally openContent for different text when open.
  static member Create
    (
      closedContent: Doc,
      items: Doc list,
      ?openContent: Doc,
      ?direction: ButtonMenu.Direction,
      ?isOpen: Var<bool>,
      ?openOnHover: View<bool>,
      ?triggerAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let direction = defaultArg direction Direction.Top
    let isOpen = defaultArg isOpen (Var.Create false)
    let openOnHover = defaultArg openOnHover (View.Const false)
    let triggerAttrs = defaultArg triggerAttrs []
    let attrs = defaultArg attrs []

    let triggerContent =
      match openContent with
      | Some oc ->
        isOpen.View
        |> View.Map(fun o -> if o then oc else closedContent)
        |> Doc.EmbedView
      | None -> closedContent

    let triggerButton =
      Button.Create(
        triggerContent,
        onClick = (fun () -> isOpen.Value <- not isOpen.Value),
        attrs = [ cl Css.``weave-button-menu__trigger``; yield! triggerAttrs ]
      )

    ButtonMenuInternal.renderMenu triggerButton items direction isOpen openOnHover attrs
