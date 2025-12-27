namespace Weave

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Dropdown =

  /// Position on the button where the dropdown menu is anchored.
  [<RequireQualifiedAccess; Struct>]
  type AnchorOrigin =
    | TopLeft
    | TopCenter
    | TopRight
    | CenterLeft
    | CenterCenter
    | CenterRight
    | BottomLeft
    | BottomCenter
    | BottomRight

  module AnchorOrigin =

    let toString anchor =
      match anchor with
      | AnchorOrigin.TopLeft -> "Top Left"
      | AnchorOrigin.TopCenter -> "Top Center"
      | AnchorOrigin.TopRight -> "Top Right"
      | AnchorOrigin.CenterLeft -> "Center Left"
      | AnchorOrigin.CenterCenter -> "Center Center"
      | AnchorOrigin.CenterRight -> "Center Right"
      | AnchorOrigin.BottomLeft -> "Bottom Left"
      | AnchorOrigin.BottomCenter -> "Bottom Center"
      | AnchorOrigin.BottomRight -> "Bottom Right"

  /// Area of the dropdown menu that is aligned to the anchor point on the button.
  [<RequireQualifiedAccess; Struct>]
  type TransformOrigin =
    | TopLeft
    | TopCenter
    | TopRight
    | CenterLeft
    | CenterCenter
    | CenterRight
    | BottomLeft
    | BottomCenter
    | BottomRight

  module TransformOrigin =

    let toString origin =
      match origin with
      | TransformOrigin.TopLeft -> "Top Left"
      | TransformOrigin.TopCenter -> "Top Center"
      | TransformOrigin.TopRight -> "Top Right"
      | TransformOrigin.CenterLeft -> "Center Left"
      | TransformOrigin.CenterCenter -> "Center Center"
      | TransformOrigin.CenterRight -> "Center Right"
      | TransformOrigin.BottomLeft -> "Bottom Left"
      | TransformOrigin.BottomCenter -> "Bottom Center"
      | TransformOrigin.BottomRight -> "Bottom Right"

  [<RequireQualifiedAccess; Struct>]
  type OpenOn =
    | Click
    | Hover

open Dropdown

[<JavaScript>]
type Dropdown =
  static member Create
    (
      buttonContents: Doc,
      items: seq<Doc>,
      ?isOpen: Var<bool>,
      ?openOn: View<OpenOn>,
      ?anchorOrigin: View<AnchorOrigin>,
      ?transformOrigin: View<TransformOrigin>,
      ?enabled: View<bool>,
      ?closeOnOutsideClick: View<bool>,
      ?buttonAttrs: Attr list,
      ?attrs: Attr list
    ) =
    let openVar = defaultArg isOpen (Var.Create false)
    let openOn = defaultArg openOn (View.Const OpenOn.Click)
    let enabled = defaultArg enabled (View.Const true)
    let closeOnOutsideClick = defaultArg closeOnOutsideClick (View.Const true)
    let buttonAttrs = defaultArg buttonAttrs []
    let anchorOrigin = defaultArg anchorOrigin (View.Const AnchorOrigin.BottomLeft)

    let transformOrigin =
      defaultArg transformOrigin (View.Const TransformOrigin.TopLeft)

    let attrs = defaultArg attrs []

    let mutable dropdownRoot = ref (JS.Document.CreateElement "div")
    let buttonRef = ref None

    let chevron (isOpen: View<bool>) =
      span [
        cl Css.``weave-dropdown__chevron``
        Attr.DynamicClassPred Css.``weave-dropdown__chevron--open`` isOpen
      ] [ text "▼" ]

    let button =
      let contents = [ buttonContents; chevron openVar.View ] |> Doc.Concat

      openOn
      |> Doc.BindView(fun openOn ->
        let onClick =
          match openOn with
          | OpenOn.Click -> fun () -> openVar.Value <- not openVar.Value
          | OpenOn.Hover -> fun () -> ()

        let onHover =
          match openOn with
          | OpenOn.Click -> []
          | OpenOn.Hover -> [ on.mouseEnter (fun _ _ -> Var.Set openVar true) ]

        Button.Create(
          contents,
          onClick = onClick,
          enabled = enabled,
          attrs = [
            yield! buttonAttrs
            on.afterRender (fun el -> buttonRef.Value <- Some el)
            yield! onHover
          ]
        ))

    let renderItem item =
      div [ cl Css.``weave-dropdown__item`` ] [ item ]

    let mutable outsideClickHandler = None

    let attachOutsideClick () =
      let handler (e: Dom.Event) =
        let target = e.Target :?> Dom.Element

        let isInsideDropdown =
          match dropdownRoot.Value with
          | null -> false
          | root -> root.Contains(target)

        let isInsideButton =
          match buttonRef.Value with
          | Some btn -> btn.Contains(target)
          | None -> false

        if not isInsideDropdown && not isInsideButton then
          openVar.Value <- false

      JS.Document.AddEventListener("mousedown", handler)
      outsideClickHandler <- Some handler

    let detachOutsideClick () =
      match outsideClickHandler with
      | Some handler ->
        JS.Document.RemoveEventListener("mousedown", handler)
        outsideClickHandler <- None
      | None -> ()

    let menuView =
      openVar.View
      |> View.MapCached(fun isOpen ->
        if isOpen then
          div
            [
              cls [ Css.``weave-dropdown__list`` ]

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.TopLeft -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-top-left``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.TopCenter -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-top-center``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.TopRight -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-top-right``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.CenterLeft -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-center-left``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.CenterCenter -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-center-center``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.CenterRight -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-center-right``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.BottomLeft -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-bottom-left``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.BottomCenter -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-bottom-center``

              anchorOrigin
              |> View.MapCached (function
                | AnchorOrigin.BottomRight -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--anchor-origin-bottom-right``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.TopLeft -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-top-left``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.TopCenter -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-top-center``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.TopRight -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-top-right``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.CenterLeft -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-center-left``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.CenterCenter -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-center-center``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.CenterRight -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-center-right``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.BottomLeft -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-bottom-left``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.BottomCenter -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-bottom-center``

              transformOrigin
              |> View.MapCached (function
                | TransformOrigin.BottomRight -> true
                | _ -> false)
              |> Attr.DynamicClassPred Css.``weave-dropdown__list--transform-origin-bottom-right``

              on.afterRender (fun el -> dropdownRoot.Value <- el)
            ]
            (items |> Seq.map renderItem)
        else
          Doc.Empty)

    let outsideClickWatcher =
      openVar.View
      |> Doc.sinkCached (fun isOpen ->
        if isOpen then
          attachOutsideClick ()
        else
          detachOutsideClick ())

    div [
      cl Css.``weave-dropdown``
      Attr.enabled enabled
      yield! attrs

      on.mouseLeaveView openOn (fun _ _ openOn ->
        match openOn with
        | OpenOn.Hover -> Var.Set openVar false
        | _ -> ())
    ] [
      closeOnOutsideClick
      |> Doc.BindView(fun closeOn -> if closeOn then outsideClickWatcher else Doc.Empty)

      button
      Doc.EmbedView menuView
    ]

[<JavaScript>]
type DropdownItem =
  static member Create(innerContents, onClick, ?enabled, ?icon, ?iconPosition, ?attrs) =
    Button.Create(
      innerContents,
      onClick,
      ?enabled = enabled,
      ?icon = icon,
      ?iconPosition = iconPosition,
      attrs = [
        cls [
          Button.Variant.toClass Button.Variant.Text
          Button.Width.toClass Button.Width.Full |> Option.defaultValue ""
          BorderRadius.toClass BorderRadius.All.none
        ]

        yield! attrs |> Option.defaultValue []
      ]
    )

[<JavaScript>]
type NestedDropdown =
  static member Create
    (buttonContents, items, ?isOpen, ?openOn, ?anchorOrigin, ?transformOrigin, ?enabled, ?buttonAttrs, ?attrs)
    =
    let anchorOrigin = defaultArg anchorOrigin (View.Const AnchorOrigin.TopRight)
    let openOn = defaultArg openOn (View.Const OpenOn.Hover)

    Dropdown.Create(
      buttonContents,
      items,
      ?isOpen = isOpen,
      openOn = openOn,
      anchorOrigin = anchorOrigin,
      ?transformOrigin = transformOrigin,
      ?enabled = enabled,
      buttonAttrs = [
        cls [
          Button.Variant.toClass Button.Variant.Text
          Button.Width.toClass Button.Width.Full |> Option.defaultValue ""
          BorderRadius.toClass BorderRadius.All.none
        ]

        yield! buttonAttrs |> Option.defaultValue []
      ],
      ?attrs = attrs
    )
