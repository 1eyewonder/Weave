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
    let triggerEl = ref (JS.Document.CreateElement "button")
    let currentHover = Var.Create false
    let hoverSync = openOnHover |> Doc.sinkCached (fun v -> currentHover.Value <- v)

    let nextKey, prevKey =
      match direction with
      | Direction.Bottom -> "ArrowDown", "ArrowUp"
      | Direction.Top -> "ArrowUp", "ArrowDown"
      | Direction.Right -> "ArrowRight", "ArrowLeft"
      | Direction.Left -> "ArrowLeft", "ArrowRight"

    let keyboardWatcher =
      MenuKeyboardNav.watch containerEl triggerEl isOpen nextKey prevKey

    let menuItems =
      items
      |> List.mapi (fun i item ->
        div [
          cl Css.``weave-button-menu__item``
          Attr.Create "role" "menuitem"
          Attr.Create "tabindex" "-1"
          Attr.Style "transition-delay" (sprintf "%dms" (i * 50))
          on.afterRender (fun el ->
            let children = el.QuerySelectorAll("button, a, [tabindex]")

            for i in 0 .. children.Length - 1 do
              As<Dom.Element>(children.Item(i)).SetAttribute("tabindex", "-1"))
        ] [ item ])

    let outsideClickWatcher =
      isOpen.View
      |> DocumentEventListener.onClick [ containerEl ] (fun () -> isOpen.Value <- false)

    div [
      cl Css.``weave-button-menu``
      Direction.toClass direction |> cl
      isOpen.View |> Attr.DynamicClassPred Css.``weave-button-menu--open``

      on.afterRender (fun el ->
        containerEl.Value <- el
        let trigger = el.QuerySelector("." + Css.``weave-button-menu__trigger``)

        if not (isNull trigger) then
          triggerEl.Value <- trigger)

      on.mouseEnter (fun _ _ ->
        if currentHover.Value then
          isOpen.Value <- true)
      on.mouseLeave (fun _ _ ->
        if currentHover.Value then
          isOpen.Value <- false)

      yield! attrs
    ] [
      outsideClickWatcher
      keyboardWatcher
      hoverSync
      div [ cl Css.``weave-button-menu__items``; Attr.Create "role" "menu" ] menuItems
      triggerButton
    ]

[<JavaScript>]
type ButtonMenu =

  /// <summary>
  /// Creates a button menu with an icon button trigger.
  /// Pass closedIcon for the default icon, and optionally openIcon for a different icon when open.
  /// If openIcon is not provided, the closedIcon will rotate 45 degrees when opened.
  /// </summary>
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
            cls [
              Css.``weave-button-menu__trigger-icon``
              Css.``weave-button-menu__trigger-icon--closed``
            ]
          ] [ closedIcon ]
          div [
            cls [
              Css.``weave-button-menu__trigger-icon``
              Css.``weave-button-menu__trigger-icon--open``
            ]
          ] [ oi ]
        ]
      | None -> closedIcon

    let triggerButton =
      Button.CreateIcon(
        triggerIcon,
        onClick = (fun () -> isOpen.Value <- not isOpen.Value),
        attrs = [
          Attr.Create "aria-label" "Menu"
          cl Css.``weave-button-menu__trigger``
          Attr.Create "aria-haspopup" "true"
          isOpen.View
          |> View.Map(sprintf "%b")
          |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-expanded", v))

          match openIcon with
          | None -> isOpen.View |> Attr.DynamicClassPred Css.``weave-button-menu__trigger--rotated``
          | Some _ -> Attr.Empty

          yield! triggerAttrs
        ]
      )

    ButtonMenuInternal.renderMenu triggerButton items direction isOpen openOnHover attrs

  /// <summary>
  /// Creates a button menu with a standard text button trigger.
  /// Pass closedContent for the default button text, and optionally openContent for different text when open.
  /// </summary>
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
        attrs = [
          cl Css.``weave-button-menu__trigger``
          Attr.Create "aria-haspopup" "true"
          isOpen.View
          |> View.Map(sprintf "%b")
          |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-expanded", v))
          yield! triggerAttrs
        ]
      )

    ButtonMenuInternal.renderMenu triggerButton items direction isOpen openOnHover attrs
