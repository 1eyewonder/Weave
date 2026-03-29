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

  module ContentPlacement =

    let right = cl Css.``weave-flex-row``
    let left = cl Css.``weave-flex-row-reverse``
    let top = cl Css.``weave-flex-column-reverse``
    let bottom = cl Css.``weave-flex-column``

[<JavaScript; RequireQualifiedAccess>]
type Switch =

  static member private render(isChecked: Var<bool>, content: Doc, enabled: View<bool>, attrs: Attr list) =
    label [
      cl Css.``weave-switch``
      Flex.Inline.allSizes
      FlexWrap.NoWrap.allSizes
      AlignItems.center

      Disabled.disabledClass Css.``weave-switch--disabled`` enabled
      Switch.ContentPlacement.right
      yield! attrs
    ] [
      div [ cls [ Css.``weave-switch__container`` ] ] [
        input [
          Attr.Prop "type" "checkbox"
          Attr.Create "role" "switch"
          Attr.Checked isChecked
          Attr.enabled enabled
          cl Css.``weave-switch__input``

          let clickable = (isChecked.View, enabled) ||> View.zipCached

          on.clickView clickable (fun _ _ (flag, enabled) ->
            if enabled then
              not flag |> Var.Set isChecked)

          on.keyDown (fun _ ev ->
            match ev with
            | Key.Activate ->
              ev.PreventDefault()
              not isChecked.Value |> Var.Set isChecked
            | _ -> ())
        ] []
        span [ cls [ Css.``weave-switch__track`` ] ] []
        span [ cls [ Css.``weave-switch__thumb`` ] ] []
      ]
      div [ View.not enabled |> Attr.toggleColor Palette.textDisabled ] [ content ]
    ]

  static member create(isChecked: Var<bool>, ?content: Doc, ?enabled: View<bool>, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    let content =
      content
      |> Option.mapOrDefault Doc.Empty (fun d -> div [ cls [ Css.``weave-switch__label`` ] ] [ d ])

    Switch.render (isChecked, content, enabled, attrs)
