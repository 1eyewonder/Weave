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
module Radio =

  module Size =

    let small = cl Css.``weave-radio--small``
    let medium = cl Css.``weave-radio--medium``
    let large = cl Css.``weave-radio--large``

  module Color =

    let primary = cl Css.``weave-radio--primary``
    let secondary = cl Css.``weave-radio--secondary``
    let tertiary = cl Css.``weave-radio--tertiary``
    let error = cl Css.``weave-radio--error``
    let warning = cl Css.``weave-radio--warning``
    let success = cl Css.``weave-radio--success``
    let info = cl Css.``weave-radio--info``

  module ContentPlacement =

    let right = cl Css.``weave-flex-row``
    let left = cl Css.``weave-flex-row-reverse``
    let top = cl Css.``weave-flex-column-reverse``
    let bottom = cl Css.``weave-flex-column``

[<JavaScript; RequireQualifiedAccess>]
type Radio =

  static member create<'T when 'T: equality>
    (userSelection: Var<'T>, value: 'T, ?displayText: View<string>, ?enabled: View<bool>, ?attrs: Attr list)
    =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    let isSelected = Var.Create false

    label [
      cl Css.``weave-radio``
      Flex.Inline.allSizes
      FlexWrap.NoWrap.allSizes
      AlignItems.center

      Disabled.disabledClass Css.``weave-radio--disabled`` enabled
      Radio.ContentPlacement.right
      yield! attrs
    ] [
      input [
        Attr.Prop "type" "radio"
        Attr.Checked isSelected
        Attr.enabled enabled

        on.clickViewGuarded enabled (fun () -> Var.Set userSelection value)

        on.keyDown (fun _ ev ->
          match ev with
          | Key.Activate ->
            ev.PreventDefault()
            Var.Set userSelection value
          | _ -> ())

        cl Css.``weave-radio__input``
      ] [
        userSelection.View
        |> Doc.sinkCached (fun userSelection ->
          if userSelection = value then
            Var.Set isSelected true
          else
            Var.Set isSelected false)
      ]
      span [ cls [ Css.``weave-radio__span`` ] ] []
      match displayText with
      | Some v ->
        span [
          Typography.body1
          cl Css.``weave-radio__label``
          JustifyContent.center
          View.not enabled |> Attr.toggleColor Palette.textDisabled
        ] [ textView v ]
      | None -> Doc.Empty
    ]
