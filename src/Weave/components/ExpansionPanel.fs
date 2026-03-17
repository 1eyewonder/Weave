namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module ExpansionPanel =

  [<RequireQualifiedAccess; Struct>]
  type ExpansionSelection =
    | Single
    | Multiple

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-expansion-panel__header--primary``
      | BrandColor.Secondary -> Css.``weave-expansion-panel__header--secondary``
      | BrandColor.Tertiary -> Css.``weave-expansion-panel__header--tertiary``
      | BrandColor.Error -> Css.``weave-expansion-panel__header--error``
      | BrandColor.Warning -> Css.``weave-expansion-panel__header--warning``
      | BrandColor.Success -> Css.``weave-expansion-panel__header--success``
      | BrandColor.Info -> Css.``weave-expansion-panel__header--info``

  module FocusColor =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-expansion-panel__header--focus-primary``
      | BrandColor.Secondary -> Css.``weave-expansion-panel__header--focus-secondary``
      | BrandColor.Tertiary -> Css.``weave-expansion-panel__header--focus-tertiary``
      | BrandColor.Error -> Css.``weave-expansion-panel__header--focus-error``
      | BrandColor.Warning -> Css.``weave-expansion-panel__header--focus-warning``
      | BrandColor.Success -> Css.``weave-expansion-panel__header--focus-success``
      | BrandColor.Info -> Css.``weave-expansion-panel__header--focus-info``

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

    let enabledRef: bool ref = ref true

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
        Attr.Create "tabindex" "0"
        Attr.Create "role" "button"
        on.afterRender (fun _ -> enabled |> View.Sink(fun v -> enabledRef.Value <- v))
        expanded.View
        |> View.Map(fun v -> if v then "true" else "false")
        |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-expanded", v))

        let clickable = View.zipCached expanded.View enabled

        on.clickTapView clickable
        <| fun _ _ (isExpanded, isEnabled) ->
          if isEnabled then
            Var.Set expanded (not isExpanded)

        on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
          if ev.Key = "Enter" || ev.Key = " " then
            ev.PreventDefault()

            if enabledRef.Value then
              Var.Set expanded (not expanded.Value))

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
          H6.Div("+", attrs = [ Attr.Style "text-align" "center" ]),
          H6.Div("-", attrs = [ Attr.Style "text-align" "center" ]),
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
type ExpansionPanelContent =

  static member Create(content: Doc, ?gutters: View<bool>, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    let gutters = defaultArg gutters (View.Const true)

    div [
      cls [ Css.``weave-expansion-panel__content`` ]
      gutters
      |> Attr.DynamicClassPred Css.``weave-expansion-panel__content--with-gutters``
      yield! attrs
    ] [ content ]

[<JavaScript>]
type ExpansionPanel =

  static member Create
    (header: Doc, content: Doc, ?expanded: Var<bool>, ?enabled: View<bool>, ?attrs: Attr list)
    =
    let expanded = defaultArg expanded (Var.Create false)
    let attrs = defaultArg attrs []
    let headerId = WeaveId.create "weave-expansion-panel-header"
    let panelId = WeaveId.create "weave-expansion-panel-content"

    div [
      cls [ Css.``weave-expansion-panel`` ]

      expanded.View |> Attr.DynamicClassPred Css.``weave-expansion-panel--expanded``

      View.not expanded.View
      |> Attr.DynamicClassPred Css.``weave-expansion-panel--collapsed``

      on.afterRender (fun el ->
        let headerEl = el.QuerySelector("." + Css.``weave-expansion-panel__header``)

        if not (isNull headerEl) then
          headerEl.SetAttribute("aria-controls", panelId)
          headerEl.SetAttribute("id", headerId))

      yield! attrs
    ] [
      header
      div [
        cl Css.``weave-expansion-panel__content-wrapper``
        Attr.Create "id" panelId
        Attr.Create "role" "region"
        Attr.Create "aria-labelledby" headerId
      ] [ content ]
    ]
