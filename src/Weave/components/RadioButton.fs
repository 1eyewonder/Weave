namespace Weave

open WebSharper
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

  [<RequireQualifiedAccess; Struct>]
  type ContentPlacement =
    | Top
    | Bottom
    | Left
    | Right

open WebSharper.UI.Server

[<JavaScript; RequireQualifiedAccess>]
type Radio =

  static member create<'T when 'T: equality>
    (
      userSelection: Var<'T>,
      value: 'T,
      ?displayText: View<string>,
      ?enabled: View<bool>,
      ?contentPlacement: View<Radio.ContentPlacement>,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    let contentPlacement =
      defaultArg contentPlacement (View.Const Radio.ContentPlacement.Right)

    let isSelected = Var.Create false

    label [
      cl Css.``weave-radio``
      Flex.Inline.allSizes

      Disabled.disabledClass Css.``weave-radio--disabled`` enabled

      Map.ofList [
        Radio.ContentPlacement.Right, Css.``weave-flex-row``
        Radio.ContentPlacement.Left, Css.``weave-flex-row-reverse``
        Radio.ContentPlacement.Top, Css.``weave-flex-column-reverse``
        Radio.ContentPlacement.Bottom, Css.``weave-flex-column``
      ]
      |> Attr.classSelection contentPlacement

      yield! attrs
    ] [
      input [
        Attr.Prop "type" "radio"
        Attr.Checked isSelected
        Attr.enabled enabled

        on.clickViewGuarded enabled (fun () -> Var.Set userSelection value)

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
