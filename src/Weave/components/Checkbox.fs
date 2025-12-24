namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Checkbox =

  [<RequireQualifiedAccess; Struct>]
  type Size =
    | Small
    | Medium
    | Large

  module Size =

    let toClass size =
      match size with
      | Size.Small -> Css.``checkbox--small``
      | Size.Medium -> Css.``checkbox--medium``
      | Size.Large -> Css.``checkbox--large``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``checkbox--primary``
      | BrandColor.Secondary -> Css.``checkbox--secondary``
      | BrandColor.Tertiary -> Css.``checkbox--tertiary``
      | BrandColor.Error -> Css.``checkbox--error``
      | BrandColor.Warning -> Css.``checkbox--warning``
      | BrandColor.Success -> Css.``checkbox--success``
      | BrandColor.Info -> Css.``checkbox--info``

  [<RequireQualifiedAccess; Struct>]
  type ContentPlacement =
    | Top
    | Bottom
    | Left
    | Right

open Checkbox

[<JavaScript>]
type Checkbox =

  static member Create
    (
      isChecked: Var<bool>,
      ?displayText: View<string>,
      ?enabled: View<bool>,
      ?contentPlacement: View<ContentPlacement>,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    let contentPlacement =
      defaultArg contentPlacement (View.Const ContentPlacement.Right)

    label [
      cls [
        Css.checkbox
        Flex.Inline.allSizes
        FlexWrap.NoWrap.allSizes
        AlignItems.toClass AlignItems.Center
      ]

      contentPlacement
      |> View.MapCached (function
        | ContentPlacement.Right -> true
        | _ -> false)
      |> Attr.DynamicClassPred FlexDirection.Row.allSizes

      contentPlacement
      |> View.MapCached (function
        | ContentPlacement.Left -> true
        | _ -> false)
      |> Attr.DynamicClassPred FlexDirection.RowReverse.allSizes

      contentPlacement
      |> View.MapCached (function
        | ContentPlacement.Top -> true
        | _ -> false)
      |> Attr.DynamicClassPred FlexDirection.ColumnReverse.allSizes

      contentPlacement
      |> View.MapCached (function
        | ContentPlacement.Bottom -> true
        | _ -> false)
      |> Attr.DynamicClassPred FlexDirection.Column.allSizes

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

        cl Css.checkbox__input
      ] []
      span [ cls [ Css.checkbox__span ] ] []
      match displayText with
      | Some v -> Body1.Create(v, View.Const false, attrs = [ cls [ Css.checkbox__label ] ])
      | None -> Doc.Empty
    ]
