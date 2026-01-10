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
      | Size.Small -> Css.``weave-switch__base--small``
      | Size.Medium -> Css.``weave-switch__base--medium``
      | Size.Large -> Css.``weave-switch__base--large``

  module Color =
    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-switch--primary``
      | BrandColor.Secondary -> Css.``weave-switch--secondary``
      | BrandColor.Tertiary -> Css.``weave-switch--tertiary``
      | BrandColor.Error -> Css.``weave-switch--error``
      | BrandColor.Warning -> Css.``weave-switch--warning``
      | BrandColor.Success -> Css.``weave-switch--success``
      | BrandColor.Info -> Css.``weave-switch--info``

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
      cls [
        Css.``weave-switch``
        Flex.Inline.allSizes
        AlignItems.toClass AlignItems.Center
      ]

      View.not enabled |> Attr.DynamicClassPred Css.``weave-switch--disabled``

      Map.ofList [
        ContentPlacement.Right, FlexDirection.Row.allSizes
        ContentPlacement.Left, FlexDirection.RowReverse.allSizes
        ContentPlacement.Top, FlexDirection.ColumnReverse.allSizes
        ContentPlacement.Bottom, FlexDirection.Column.allSizes
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
      | Some v -> Body1.Div(v, View.Const false, attrs = [ cls [ Css.``weave-switch__label`` ] ])
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
      |> Option.mapOrDefault Doc.Empty (fun d -> div [ cls [ Css.``weave-switch__label`` ] ] [ d ])

    Switch.Render(isChecked, content, enabled, contentPlacement, attrs)
