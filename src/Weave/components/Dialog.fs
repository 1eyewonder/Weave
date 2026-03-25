namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.JavaScript
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Dialog =

  [<RequireQualifiedAccess; Struct>]
  type DialogPosition =
    | Center
    | TopCenter
    | BottomCenter
    | CenterRight
    | CenterLeft

  module Blur =

    let toAttr isBlurred =
      isBlurred
      |> View.MapCached(fun isBlurred -> if isBlurred then "blur(5px)" else "none")
      |> Attr.DynamicStyle "backdrop-filter"

  [<RequireQualifiedAccess; Struct>]
  type Interaction =
    | Force
    | Optional of closeDialog: (unit -> unit)

[<JavaScript>]
module private DialogInternal =

  let mutable idCounter = 0

  let nextTitleId () =
    idCounter <- idCounter + 1
    sprintf "weave-dialog-title-%d" idCounter

  let focusableSelector =
    "a[href], button:not([disabled]), textarea:not([disabled]), input:not([disabled]), select:not([disabled]), [tabindex]:not([tabindex=\"-1\"])"

  let trapFocus (container: Dom.Element) (ev: Dom.KeyboardEvent) =
    let focusable = container.QuerySelectorAll(focusableSelector)

    if focusable.Length > 0 then
      let first = As<Dom.Element>(focusable.Item 0)
      let last = As<Dom.Element>(focusable.Item(focusable.Length - 1))
      let active = As<Dom.Element>(JS.Document?activeElement)

      if ev.ShiftKey then
        if active = first || active = container then
          ev.PreventDefault()
          last?focus ()
      else if active = last then
        ev.PreventDefault()
        first?focus ()

open Dialog
open DialogInternal

[<JavaScript>]
type DialogTitle =

  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    div [ cls [ Css.``weave-dialog__title`` ]; yield! attrs ] [ content ]

[<JavaScript>]
type DialogContent =

  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    div [ cls [ Css.``weave-dialog__content`` ]; yield! attrs ] [ content ]

[<JavaScript>]
type Dialog =

  static member create
    (
      title: Doc,
      content: Doc,
      ?blurBackground: View<bool>,
      ?dialogPosition: View<DialogPosition>,
      ?dialogInteraction: View<Interaction>,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs []
    let blur = defaultArg blurBackground (View.Const true)

    let forceInteraction = defaultArg dialogInteraction (View.Const Interaction.Force)

    let dialogPosition = defaultArg dialogPosition (View.Const DialogPosition.Center)

    let titleId = nextTitleId ()
    let previousFocus: obj option ref = ref None
    let removalListener = DomRemovalListener.create ()

    let restoreFocus () =
      removalListener.Disconnect()

      match previousFocus.Value with
      | Some el -> el?focus ()
      | None -> ()

    div [
      cls [ Css.``weave-dialog`` ]

      Map.ofList [
        DialogPosition.Center, Css.``weave-dialog--center``
        DialogPosition.TopCenter, Css.``weave-dialog--topcenter``
        DialogPosition.BottomCenter, Css.``weave-dialog--bottomcenter``
        DialogPosition.CenterRight, Css.``weave-dialog--centerright``
        DialogPosition.CenterLeft, Css.``weave-dialog--centerleft``
      ]
      |> Attr.classSelection dialogPosition

      yield! attrs
    ] [
      div [
        cl Css.``weave-dialog__backdrop``
        Blur.toAttr blur

        on.mouseDownView forceInteraction
        <| fun _ _ interaction ->
          match interaction with
          | Interaction.Optional closeDialog ->
            restoreFocus ()
            closeDialog ()
          | _ -> ()
      ] []
      div [
        cls [ Css.``weave-dialog__window`` ]
        Attr.Create "role" "dialog"
        Attr.Create "aria-modal" "true"
        Attr.Create "aria-labelledby" titleId
        Attr.Create "tabindex" "-1"

        on.afterRender (fun el ->
          let active = JS.Document?activeElement

          previousFocus.Value <-
            if active <> null && active <> JS.Undefined then
              Some active
            else
              None

          let titleEl = el.QuerySelector("." + Css.``weave-dialog__title``)

          if not (isNull titleEl) then
            titleEl?id <- titleId

          el?focus ()

          removalListener.Watch(el, restoreFocus))

        on.keyDownView forceInteraction
        <| fun el ev interaction ->
          match ev with
          | Key.Escape ->
            match interaction with
            | Interaction.Optional closeDialog ->
              restoreFocus ()
              closeDialog ()
            | Interaction.Force -> ()
          | Key.Tab -> trapFocus el ev
          | _ -> ()
      ] [ title; content ]
    ]
