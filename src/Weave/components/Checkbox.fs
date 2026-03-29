namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Checkbox =

  module Size =

    let small = cl Css.``weave-checkbox--small``
    let medium = cl Css.``weave-checkbox--medium``
    let large = cl Css.``weave-checkbox--large``

  module Color =

    let primary = cl Css.``weave-checkbox--primary``
    let secondary = cl Css.``weave-checkbox--secondary``
    let tertiary = cl Css.``weave-checkbox--tertiary``
    let error = cl Css.``weave-checkbox--error``
    let warning = cl Css.``weave-checkbox--warning``
    let success = cl Css.``weave-checkbox--success``
    let info = cl Css.``weave-checkbox--info``

  module ContentPlacement =

    let right = cl Css.``weave-flex-row``
    let left = cl Css.``weave-flex-row-reverse``
    let top = cl Css.``weave-flex-column-reverse``
    let bottom = cl Css.``weave-flex-column``

[<JavaScript; RequireQualifiedAccess>]
type Checkbox =

  static member create
    (isChecked: Var<bool>, ?displayText: View<string>, ?enabled: View<bool>, ?attrs: Attr list)
    =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    label [
      cl Css.``weave-checkbox``
      Flex.Inline.allSizes
      FlexWrap.NoWrap.allSizes
      AlignItems.center

      Disabled.disabledClass Css.``weave-checkbox--disabled`` enabled
      Checkbox.ContentPlacement.right
      yield! attrs
    ] [
      input [
        Attr.Prop "type" "checkbox"
        Attr.Checked isChecked
        Attr.enabled enabled

        let clickable = (isChecked.View, enabled) ||> View.zipCached

        on.clickView clickable (fun _ _ (flag, enabled) ->
          if enabled then
            not flag |> Var.Set isChecked)

        cl Css.``weave-checkbox__input``
      ] []
      span [ cls [ Css.``weave-checkbox__span`` ] ] []
      match displayText with
      | Some v ->
        div [
          Typography.body1
          Typography.noWrap
          cls [ Css.``weave-checkbox__label`` ]
          View.not enabled |> Attr.toggleColor Palette.textDisabled
        ] [ textView v ]
      | None -> Doc.Empty
    ]
