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
      | Size.Small -> Css.``weave-checkbox--small``
      | Size.Medium -> Css.``weave-checkbox--medium``
      | Size.Large -> Css.``weave-checkbox--large``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-checkbox--primary``
      | BrandColor.Secondary -> Css.``weave-checkbox--secondary``
      | BrandColor.Tertiary -> Css.``weave-checkbox--tertiary``
      | BrandColor.Error -> Css.``weave-checkbox--error``
      | BrandColor.Warning -> Css.``weave-checkbox--warning``
      | BrandColor.Success -> Css.``weave-checkbox--success``
      | BrandColor.Info -> Css.``weave-checkbox--info``

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
        Css.``weave-checkbox``
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

        cl Css.``weave-checkbox__input``
      ] []
      span [ cls [ Css.``weave-checkbox__span`` ] ] []
      match displayText with
      | Some v -> Body1.Create(v, View.Const false, attrs = [ cls [ Css.``weave-checkbox__label`` ] ])
      | None -> Doc.Empty
    ]
