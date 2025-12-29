namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers
open WebSharper.UI.Client

[<JavaScript>]
module ExpansionPanel =

  [<RequireQualifiedAccess; Struct>]
  type ExpansionSelection =
    | Single
    | Multiple

  module Color =

    let toColor color =
      match color with
      | BrandColor.Primary -> Css.``weave-expansion-panel__header--primary``
      | BrandColor.Secondary -> Css.``weave-expansion-panel__header--secondary``
      | BrandColor.Tertiary -> Css.``weave-expansion-panel__header--tertiary``
      | BrandColor.Error -> Css.``weave-expansion-panel__header--error``
      | BrandColor.Warning -> Css.``weave-expansion-panel__header--warning``
      | BrandColor.Success -> Css.``weave-expansion-panel__header--success``
      | BrandColor.Info -> Css.``weave-expansion-panel__header--info``

  [<RequireQualifiedAccess; Struct>]
  type HeaderVariant =
    | Filled
    | Highlight
    | None

open ExpansionPanel

[<JavaScript>]
type ExpansionPanelContainer =

  static member Create(panels: Doc list, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    div [ cls [ Css.``weave-expansion-panels`` ]; yield! attrs ] panels

[<JavaScript>]
type ExpansionPanelIcon =

  static member Create(unexpandedIcon: Doc, expandedIcon: Doc, expanded: Var<bool>, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    div [ cls [ Css.``weave-expansion-panel__icon`` ]; yield! attrs ] [
      expanded.View
      |> Doc.BindView(fun isExpanded -> if isExpanded then expandedIcon else unexpandedIcon)
    ]

[<JavaScript>]
type ExpansionPanelHeader =

  static member private Create
    (
      content: Doc,
      expanded: Var<bool>,
      ?enabled: View<bool>,
      ?icon: Doc,
      ?highlightVariant: View<HeaderVariant>,
      ?attrs: Attr list
    ) =
    let variant = defaultArg highlightVariant (View.Const HeaderVariant.Highlight)

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs []

    let children =
      match icon with
      | Some icon -> [ content; icon ]
      | None -> [ content ]

    div
      [
        cls [
          Css.``weave-expansion-panel__header``
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          AlignContent.toClass AlignContent.SpaceBetween
          JustifyContent.toClass JustifyContent.SpaceBetween
        ]
        Attr.Style "width" "100%"

        let clickable = View.zipCached expanded.View enabled

        on.clickView clickable
        <| fun _ _ (isExpanded, isEnabled) ->
          if isEnabled then
            Var.Set expanded (not isExpanded)

        Map.ofList [
          HeaderVariant.Filled, Css.``weave-expansion-panel__header--filled``
          HeaderVariant.Highlight, Css.``weave-expansion-panel__header--highlight``
        ]
        |> Attr.classSelection variant

        yield! attrs
      ]
      children

  static member CreateWithDefaultIcons
    (
      content: Doc,
      expanded: Var<bool>,
      ?highlightVariant: View<HeaderVariant>,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    ExpansionPanelHeader.Create(
      content = content,
      expanded = expanded,
      icon =
        ExpansionPanelIcon.Create(
          H6.Create("+", attrs = [ Attr.Style "text-align" "center" ]),
          H6.Create("-", attrs = [ Attr.Style "text-align" "center" ]),
          expanded = expanded
        ),
      ?highlightVariant = highlightVariant,
      ?enabled = enabled,
      ?attrs = attrs
    )

  static member CreateWithNoIcons
    (
      content: Doc,
      expanded: Var<bool>,
      ?highlightVariant: View<HeaderVariant>,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    ExpansionPanelHeader.Create(
      content = content,
      expanded = expanded,
      ?highlightVariant = highlightVariant,
      ?enabled = enabled,
      ?attrs = attrs
    )

  static member CreateWithCustomIcons
    (
      content: Doc,
      icon: Doc,
      expanded: Var<bool>,
      ?highlightVariant: View<HeaderVariant>,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    ExpansionPanelHeader.Create(
      content = content,
      expanded = expanded,
      icon = icon,
      ?highlightVariant = highlightVariant,
      ?enabled = enabled,
      ?attrs = attrs
    )

[<JavaScript>]
type ExpansionPanel =

  static member Create
    (header: Doc, content: Doc, ?expanded: Var<bool>, ?enabled: View<bool>, ?attrs: Attr list)
    =
    let expanded = defaultArg expanded (Var.Create false)
    let attrs = defaultArg attrs []

    let contentDiv =
      div [ cls [ Css.``weave-expansion-panel__content`` ]; yield! attrs ] [ content ]

    div [
      cls [ Css.``weave-expansion-panel`` ]

      expanded.View |> Attr.DynamicClassPred Css.``weave-expansion-panel--expanded``

      View.not expanded.View
      |> Attr.DynamicClassPred Css.``weave-expansion-panel--collapsed``

      yield! attrs
    ] [ header; contentDiv ]
