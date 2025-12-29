namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.JavaScript
open Weave.CssHelpers

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

open Dialog

[<JavaScript>]
type DialogTitle =

  static member Create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    div [ cls [ Css.``weave-dialog__title`` ]; yield! attrs ] [ content ]

[<JavaScript>]
type DialogContent =

  static member Create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    div [ cls [ Css.``weave-dialog__content`` ]; yield! attrs ] [ content ]

[<JavaScript>]
type Dialog =

  static member Create
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
          | Interaction.Optional closeDialog -> closeDialog ()
          | _ -> ()
      ] []
      div [ cls [ Css.``weave-dialog__window`` ] ] [ title; content ]
    ]
