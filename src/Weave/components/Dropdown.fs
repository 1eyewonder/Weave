namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Dropdown =

  [<RequireQualifiedAccess; Struct>]
  type AnchorOrigin =
    | TopLeft
    | TopCenter
    | TopRight
    | CenterLeft
    | CenterRight
    | BottomLeft
    | BottomCenter
    | BottomRight

  [<JavaScript>]
  type DropdownItem = {
    Content: Doc
    OnClick: option<unit -> unit>
    Disabled: option<View<bool>>
    NestedDropdown: option<Doc>
    Attrs: Attr list
  }

  module DropdownItem =
    let Create
      (
        content: Doc,
        onClick: option<unit -> unit>,
        disabled: option<View<bool>>,
        nestedDropdown: option<Doc>,
        attrs: option<Attr list>
      )
      =
      {
        Content = content
        OnClick = onClick
        Disabled = disabled
        NestedDropdown = nestedDropdown
        Attrs = defaultArg attrs []
      }

  [<JavaScript>]
  type Dropdown =
    static member Create
      (
        innerContents: Doc,
        items: seq<DropdownItem>,
        ?isOpen: Var<bool>,
        ?anchorOrigin: AnchorOrigin,
        ?blockInteraction: View<bool>,
        ?attrs: Attr list
      ) =
      let openVar = defaultArg isOpen (Var.Create false)
      let blockInteraction = defaultArg blockInteraction (View.Const false)
      let attrs = defaultArg attrs []

      let chevron (isOpen: View<bool>) =
        span [
          cl Css.``weave-dropdown__chevron``
          Attr.DynamicClassPred Css.``weave-dropdown__chevron--open`` isOpen
        ] [ text "▼" ]

      let dropdownRoot = JS.Document.CreateElement "div"

      let button =
        button [
          cl Css.``weave-dropdown__button``
          yield! attrs
          on.click (fun _ _ -> openVar.Value <- not openVar.Value)
        ] [ innerContents; chevron openVar.View ]

      let mutable outsideClickHandler = None

      let attachOutsideClick () =
        let handler =
          fun (e: WebSharper.JavaScript.Dom.Event) ->
            let target = e.Target :?> WebSharper.JavaScript.Dom.Element

            if not (dropdownRoot.Contains(target)) then
              openVar.Value <- false

        JS.Document.AddEventListener("mousedown", handler)
        outsideClickHandler <- Some handler

      let detachOutsideClick () =
        match outsideClickHandler with
        | Some handler ->
          JS.Document.RemoveEventListener("mousedown", handler)
          outsideClickHandler <- None
        | None -> ()

      openVar.View
      |> View.Sink(fun isOpen ->
        if isOpen then
          attachOutsideClick ()
        else
          detachOutsideClick ())

      let renderItem (item: DropdownItem) =
        let disabledView = defaultArg item.Disabled (View.Const false)

        let clickAttr =
          match item.OnClick with
          | Some cb ->
            on.clickView (View.Map not disabledView) (fun _ _ enabled ->
              if enabled then
                cb ())
          | None -> Attr.Empty

        div [
          cl Css.``weave-dropdown__item``
          yield! item.Attrs
          Attr.DynamicClassPred Css.``weave-dropdown__item--disabled`` disabledView
          clickAttr
        ] [
          item.Content
          match item.NestedDropdown with
          | Some dd -> dd
          | None -> Doc.Empty
        ]

      let anchorClass =
        match anchorOrigin with
        | Some AnchorOrigin.TopLeft -> Css.``weave-dropdown__list--top-left``
        | Some AnchorOrigin.TopCenter -> Css.``weave-dropdown__list--top-center``
        | Some AnchorOrigin.TopRight -> Css.``weave-dropdown__list--top-right``
        | Some AnchorOrigin.CenterLeft -> Css.``weave-dropdown__list--center-left``
        | Some AnchorOrigin.CenterRight -> Css.``weave-dropdown__list--center-right``
        | Some AnchorOrigin.BottomLeft -> Css.``weave-dropdown__list--bottom-left``
        | Some AnchorOrigin.BottomCenter -> Css.``weave-dropdown__list--bottom-center``
        | Some AnchorOrigin.BottomRight -> Css.``weave-dropdown__list--bottom-right``
        | None -> Css.``weave-dropdown__list--bottom-left``

      let menuView =
        openVar.View
        |> View.Map(fun isOpen ->
          if isOpen then
            div
              [ cls [ Css.``weave-dropdown__list``; anchorClass ] ]
              (items |> Seq.map renderItem |> Seq.toList)
          else
            Doc.Empty)

      let mutable dropdownRoot: ref<Dom.Element option> = ref None

      div [
        cl Css.``weave-dropdown``
        on.afterRenderView blockInteraction (fun el block ->
          if not block then
            dropdownRoot.Value <- Some el)
      ] [ button; Doc.EmbedView menuView ]
