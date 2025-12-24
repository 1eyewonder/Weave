namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Switch =

  [<RequireQualifiedAccess; Struct>]
  type Size =
    | Small
    | Medium
    | Large

  module Size =
    let toClass size =
      match size with
      | Size.Small -> Css.``switch__base--small``
      | Size.Medium -> Css.``switch__base--medium``
      | Size.Large -> Css.``switch__base--large``

  module Color =
    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``switch--primary``
      | BrandColor.Secondary -> Css.``switch--secondary``
      | BrandColor.Tertiary -> Css.``switch--tertiary``
      | BrandColor.Error -> Css.``switch--error``
      | BrandColor.Warning -> Css.``switch--warning``
      | BrandColor.Success -> Css.``switch--success``
      | BrandColor.Info -> Css.``switch--info``

  [<RequireQualifiedAccess; Struct>]
  type ContentPlacement =
    | Top
    | Bottom
    | Left
    | Right

open Switch

[<JavaScript>]
type Switch =

  static member private Render
    (
      isChecked: Var<bool>,
      content: Doc,
      enabled: View<bool>,
      contentPlacement: View<ContentPlacement>,
      attrs: Attr list
    ) =
    label [
      cls [ Css.switch; Flex.Inline.allSizes; AlignItems.toClass AlignItems.Center ]

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
      div [ cls [ Css.``switch__container`` ] ] [
        input [
          Attr.Prop "type" "checkbox"
          Attr.Checked isChecked
          Attr.enabled enabled
          cl Css.switch__input

          let clickable = (isChecked.View, enabled) ||> View.zipCached

          on.clickView clickable (fun _ _ (flag, enabled) ->
            if enabled then
              not flag |> Var.Set isChecked)
        ] []
        span [ cls [ Css.``switch__track`` ] ] []
        span [ cls [ Css.``switch__thumb`` ] ] []
      ]
      content
    ]

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

    let content =
      match displayText with
      | Some v -> Body1.Create(v, View.Const false, attrs = [ cls [ Css.switch__label ] ])
      | None -> Doc.Empty

    Switch.Render(isChecked, content, enabled, contentPlacement, attrs)

  static member Create
    (
      isChecked: Var<bool>,
      ?content: Doc,
      ?enabled: View<bool>,
      ?contentPlacement: View<ContentPlacement>,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs []
    let enabled = defaultArg enabled (View.Const true)

    let contentPlacement =
      defaultArg contentPlacement (View.Const ContentPlacement.Right)

    let content =
      content
      |> Option.mapOrDefault Doc.Empty (fun d -> div [ cls [ Css.switch__label ] ] [ d ])

    Switch.Render(isChecked, content, enabled, contentPlacement, attrs)
