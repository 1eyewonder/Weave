namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Radio =

  [<RequireQualifiedAccess; Struct>]
  type Size =
    | Small
    | Medium
    | Large

  module Size =
    let toClass size =
      match size with
      | Size.Small -> Css.``weave-radio--small``
      | Size.Medium -> Css.``weave-radio--medium``
      | Size.Large -> Css.``weave-radio--large``

  module Color =
    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-radio--primary``
      | BrandColor.Secondary -> Css.``weave-radio--secondary``
      | BrandColor.Tertiary -> Css.``weave-radio--tertiary``
      | BrandColor.Error -> Css.``weave-radio--error``
      | BrandColor.Warning -> Css.``weave-radio--warning``
      | BrandColor.Success -> Css.``weave-radio--success``
      | BrandColor.Info -> Css.``weave-radio--info``

  [<RequireQualifiedAccess; Struct>]
  type ContentPlacement =
    | Top
    | Bottom
    | Left
    | Right

open Radio
open WebSharper.UI.Server

[<JavaScript>]
type Radio =

  static member Create<'T when 'T: equality>
    (
      userSelection: Var<'T>,
      value: 'T,
      ?displayText: View<string>,
      ?enabled: View<bool>,
      ?contentPlacement: View<ContentPlacement>,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    let contentPlacement =
      defaultArg contentPlacement (View.Const ContentPlacement.Right)

    let isSelected = Var.Create false

    label [
      cls [ Css.``weave-radio``; Flex.Inline.allSizes ]

      View.not enabled |> Attr.DynamicClassPred Css.``weave-radio--disabled``

      Map.ofList [
        ContentPlacement.Right, FlexDirection.Row.allSizes
        ContentPlacement.Left, FlexDirection.RowReverse.allSizes
        ContentPlacement.Top, FlexDirection.ColumnReverse.allSizes
        ContentPlacement.Bottom, FlexDirection.Column.allSizes
      ]
      |> Attr.classSelection contentPlacement

      yield! attrs
    ] [
      input [
        Attr.Prop "type" "radio"
        Attr.Checked isSelected
        Attr.enabled enabled

        on.clickView enabled (fun _ _ enabled ->
          if enabled then
            Var.Set userSelection value)

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
          cls [
            Css.``weave-radio__label``
            Css.``weave-typography--body1``
            JustifyContent.toClass JustifyContent.Center
          ]
          View.not enabled |> Attr.toggleColor Palette.textDisabled
        ] [ textView v ]
      | None -> Doc.Empty
    ]
