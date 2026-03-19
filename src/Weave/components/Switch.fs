namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Switch =

  module Size =

    let small = cl Css.``weave-switch__base--small``
    let medium = cl Css.``weave-switch__base--medium``
    let large = cl Css.``weave-switch__base--large``

  module Color =

    let primary = cl Css.``weave-switch--primary``
    let secondary = cl Css.``weave-switch--secondary``
    let tertiary = cl Css.``weave-switch--tertiary``
    let error = cl Css.``weave-switch--error``
    let warning = cl Css.``weave-switch--warning``
    let success = cl Css.``weave-switch--success``
    let info = cl Css.``weave-switch--info``

  [<RequireQualifiedAccess; Struct>]
  type ContentPlacement =
    | Top
    | Bottom
    | Left
    | Right

[<JavaScript; RequireQualifiedAccess>]
type Switch =

  static member private render
    (
      isChecked: Var<bool>,
      content: Doc,
      enabled: View<bool>,
      contentPlacement: View<Switch.ContentPlacement>,
      attrs: Attr list
    ) =
    label [
      cl Css.``weave-switch``
      Flex.Inline.allSizes
      AlignItems.center

      Disabled.disabledClass Css.``weave-switch--disabled`` enabled

      Map.ofList [
        Switch.ContentPlacement.Right, Css.``flex-row``
        Switch.ContentPlacement.Left, Css.``flex-row-reverse``
        Switch.ContentPlacement.Top, Css.``flex-column-reverse``
        Switch.ContentPlacement.Bottom, Css.``flex-column``
      ]
      |> Attr.classSelection contentPlacement

      yield! attrs
    ] [
      div [ cls [ Css.``weave-switch__container`` ] ] [
        input [
          Attr.Prop "type" "checkbox"
          Attr.Checked isChecked
          Attr.enabled enabled
          cl Css.``weave-switch__input``

          let clickable = (isChecked.View, enabled) ||> View.zipCached

          on.clickView clickable (fun _ _ (flag, enabled) ->
            if enabled then
              not flag |> Var.Set isChecked)
        ] []
        span [ cls [ Css.``weave-switch__track`` ] ] []
        span [ cls [ Css.``weave-switch__thumb`` ] ] []
      ]
      div [ View.not enabled |> Attr.toggleColor Palette.textDisabled ] [ content ]
    ]

  static member create
    (
      isChecked: Var<bool>,
      ?content: Doc,
      ?enabled: View<bool>,
      ?contentPlacement: View<Switch.ContentPlacement>,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    let contentPlacement =
      defaultArg contentPlacement (View.Const Switch.ContentPlacement.Right)

    let content =
      content
      |> Option.mapOrDefault Doc.Empty (fun d -> div [ cls [ Css.``weave-switch__label`` ] ] [ d ])

    Switch.render (isChecked, content, enabled, contentPlacement, attrs)
