namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module ButtonMenu =

  [<RequireQualifiedAccess; Struct>]
  type Direction =
    | Top
    | Bottom
    | Left
    | Right

  module Direction =

    let top = cl Css.``weave-button-menu--top``
    let bottom = cl Css.``weave-button-menu--bottom``
    let left = cl Css.``weave-button-menu--left``
    let right = cl Css.``weave-button-menu--right``

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
      | ButtonMenu.Direction.Bottom -> "ArrowDown", "ArrowUp"
      | ButtonMenu.Direction.Top -> "ArrowUp", "ArrowDown"
      | ButtonMenu.Direction.Right -> "ArrowRight", "ArrowLeft"
      | ButtonMenu.Direction.Left -> "ArrowLeft", "ArrowRight"

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
      match direction with
      | ButtonMenu.Direction.Top -> ButtonMenu.Direction.top
      | ButtonMenu.Direction.Bottom -> ButtonMenu.Direction.bottom
      | ButtonMenu.Direction.Left -> ButtonMenu.Direction.left
      | ButtonMenu.Direction.Right -> ButtonMenu.Direction.right
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

[<JavaScript; RequireQualifiedAccess>]
type ButtonMenu =

  /// <summary>
  /// Creates a button menu with a standard text button trigger.
  /// Pass closedContent for the default button text, and optionally openContent for different text when open.
  /// </summary>
  static member create
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

    let direction = defaultArg direction ButtonMenu.Direction.Top
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
      Button.create (
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

[<JavaScript; RequireQualifiedAccess>]
type IconButtonMenu =

  /// <summary>
  /// Creates a button menu with an icon button trigger.
  /// Pass <c>closedIcon</c> for the default icon, and optionally <c>openIcon</c> for
  /// a different icon when open. If <c>openIcon</c> is not provided, the
  /// <c>closedIcon</c> rotates 45 degrees when the menu opens.
  /// </summary>
  static member create
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

    let direction = defaultArg direction ButtonMenu.Direction.Top
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
      IconButton.create (
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
