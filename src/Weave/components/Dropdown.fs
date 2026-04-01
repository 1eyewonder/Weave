namespace Weave

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Dropdown =

  module AnchorOrigin =

    let topLeft = cl Css.``weave-dropdown--anchor-origin-top-left``
    let topCenter = cl Css.``weave-dropdown--anchor-origin-top-center``
    let topRight = cl Css.``weave-dropdown--anchor-origin-top-right``
    let centerLeft = cl Css.``weave-dropdown--anchor-origin-center-left``
    let center = cl Css.``weave-dropdown--anchor-origin-center-center``
    let centerRight = cl Css.``weave-dropdown--anchor-origin-center-right``
    let bottomLeft = cl Css.``weave-dropdown--anchor-origin-bottom-left``
    let bottomCenter = cl Css.``weave-dropdown--anchor-origin-bottom-center``
    let bottomRight = cl Css.``weave-dropdown--anchor-origin-bottom-right``

  module TransformOrigin =

    let topLeft = cl Css.``weave-dropdown--transform-origin-top-left``
    let topCenter = cl Css.``weave-dropdown--transform-origin-top-center``
    let topRight = cl Css.``weave-dropdown--transform-origin-top-right``
    let centerLeft = cl Css.``weave-dropdown--transform-origin-center-left``
    let center = cl Css.``weave-dropdown--transform-origin-center-center``
    let centerRight = cl Css.``weave-dropdown--transform-origin-center-right``
    let bottomLeft = cl Css.``weave-dropdown--transform-origin-bottom-left``
    let bottomCenter = cl Css.``weave-dropdown--transform-origin-bottom-center``
    let bottomRight = cl Css.``weave-dropdown--transform-origin-bottom-right``

  [<RequireQualifiedAccess; Struct>]
  type OpenOn =
    | Click
    | Hover

open Dropdown

[<JavaScript>]
type Dropdown =
  static member create
    (
      buttonContents: Doc,
      items: Doc seq,
      ?isOpen: Var<bool>,
      ?openOn: View<OpenOn>,
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
    let attrs = defaultArg attrs []

    let containerRef = ref (JS.Document.CreateElement "div")
    let buttonElRef = ref (JS.Document.CreateElement "span")

    let keyboardWatcher =
      MenuKeyboardNav.watch containerRef buttonElRef openVar "ArrowDown" "ArrowUp"

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

        Button.create (
          contents,
          onClick = onClick,
          enabled = enabled,
          attrs = [
            Attr.Create "aria-haspopup" "true"
            openVar.View
            |> View.Map(sprintf "%b")
            |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-expanded", v))
            yield! buttonAttrs
            on.afterRender (fun el -> buttonElRef.Value <- el)
            yield! onHover
          ]
        ))

    let renderItem item =
      div [
        cl Css.``weave-dropdown__item``
        Attr.Create "role" "menuitem"
        Attr.Create "tabindex" "-1"
        on.afterRender (fun el ->
          let children = el.QuerySelectorAll("button, a, [tabindex]")

          for i in 0 .. children.Length - 1 do
            As<Dom.Element>(children.Item(i)).SetAttribute("tabindex", "-1"))
      ] [ item ]

    let menuView =
      openVar.View
      |> View.MapCached(fun isOpen ->
        if isOpen then
          div
            [ cls [ Css.``weave-dropdown__list`` ]; Attr.Create "role" "menu" ]
            (items |> Seq.map renderItem)
        else
          Doc.Empty)

    let outsideClickWatcher =
      openVar.View
      |> DocumentEventListener.onMouseDown [ containerRef ] (fun () -> openVar.Value <- false)

    div [
      cl Css.``weave-dropdown``
      Attr.enabled enabled
      on.afterRender (fun el -> containerRef.Value <- el)
      yield! attrs

      on.mouseLeaveView openOn (fun _ _ openOn ->
        match openOn with
        | OpenOn.Hover -> Var.Set openVar false
        | _ -> ())
    ] [
      closeOnOutsideClick
      |> Doc.BindView(fun closeOn -> if closeOn then outsideClickWatcher else Doc.Empty)

      keyboardWatcher
      button
      Doc.EmbedView menuView
    ]

[<JavaScript>]
type DropdownItem =
  static member create(innerContents, onClick, ?enabled, ?attrs) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = [
        Button.Variant.text
        Button.Width.full
        BorderRadius.All.none

        yield! attrs |> Option.defaultValue []
      ]
    )

[<JavaScript>]
type NestedDropdown =
  static member create(buttonContents, items, ?isOpen, ?openOn, ?enabled, ?buttonAttrs, ?attrs) =
    let openOn = defaultArg openOn (View.Const OpenOn.Click)

    Dropdown.create (
      buttonContents,
      items,
      ?isOpen = isOpen,
      openOn = openOn,
      ?enabled = enabled,
      buttonAttrs = [
        Button.Variant.text
        Button.Width.full
        BorderRadius.All.none

        yield! buttonAttrs |> Option.defaultValue []
      ],
      attrs = [
        AnchorOrigin.topRight

        yield! attrs |> Option.defaultValue []
      ]
    )
