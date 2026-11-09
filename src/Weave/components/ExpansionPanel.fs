namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module ExpansionPanel =

  // Both the header and its containing panel can independently disable activation.
  let internal syncHeaderAvailability (el: Dom.Element) =
    let disabled =
      el.GetAttribute("data-weave-header-disabled") = "true"
      || el.GetAttribute("data-weave-panel-disabled") = "true"

    el.SetAttribute("aria-disabled", if disabled then "true" else "false")
    el.SetAttribute("tabindex", if disabled then "-1" else "0")

  [<RequireQualifiedAccess; Struct>]
  type ExpansionSelection =
    | Single
    | Multiple

  module Color =

    let primary = cl Css.``weave-expansion-panel__header--primary``
    let secondary = cl Css.``weave-expansion-panel__header--secondary``
    let tertiary = cl Css.``weave-expansion-panel__header--tertiary``
    let error = cl Css.``weave-expansion-panel__header--error``
    let warning = cl Css.``weave-expansion-panel__header--warning``
    let success = cl Css.``weave-expansion-panel__header--success``
    let info = cl Css.``weave-expansion-panel__header--info``

  module FocusColor =

    let primary = cl Css.``weave-expansion-panel__header--focus-primary``
    let secondary = cl Css.``weave-expansion-panel__header--focus-secondary``
    let tertiary = cl Css.``weave-expansion-panel__header--focus-tertiary``
    let error = cl Css.``weave-expansion-panel__header--focus-error``
    let warning = cl Css.``weave-expansion-panel__header--focus-warning``
    let success = cl Css.``weave-expansion-panel__header--focus-success``
    let info = cl Css.``weave-expansion-panel__header--focus-info``

  // HeaderVariant is kept as a DU because it is used with Attr.classSelection for reactive switching.
  [<RequireQualifiedAccess; Struct>]
  type HeaderVariant =
    | Filled
    | Highlight
    | None

  module HeaderVariant =

    /// <summary>Filled header background modifier.</summary>
    let filled = cl Css.``weave-expansion-panel__header--filled``
    /// <summary>Highlight header background modifier.</summary>
    let highlight = cl Css.``weave-expansion-panel__header--highlight``

[<JavaScript; RequireQualifiedAccess>]
type ExpansionPanelContainer =

  static member create(panels: Doc list, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    div [ cls [ Css.``weave-expansion-panels`` ]; yield! attrs ] panels

[<JavaScript; RequireQualifiedAccess>]
type ExpansionPanelIcon =

  static member create(unexpandedIcon: Doc, expandedIcon: Doc, expanded: Var<bool>, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    div [ cls [ Css.``weave-expansion-panel__icon`` ]; yield! attrs ] [
      expanded.View
      |> Doc.BindView(fun isExpanded -> if isExpanded then expandedIcon else unexpandedIcon)
    ]

[<JavaScript; RequireQualifiedAccess>]
type ExpansionPanelHeader =

  /// <summary>Creates the default expansion toggle icon (+ when collapsed, − when expanded).</summary>
  static member defaultIcon(expanded: Var<bool>) =
    ExpansionPanelIcon.create (
      div [ Typography.h6; Attr.Style "text-align" "center" ] [ text "+" ],
      div [ Typography.h6; Attr.Style "text-align" "center" ] [ text "-" ],
      expanded = expanded
    )

  static member create
    (
      content: Doc,
      expanded: Var<bool>,
      ?enabled: View<bool>,
      ?icon: Doc,
      ?highlightVariant: View<ExpansionPanel.HeaderVariant>,
      ?attrs: Attr list
    ) =
    let variant =
      defaultArg highlightVariant (View.Const ExpansionPanel.HeaderVariant.Highlight)

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs []

    let children =
      match icon with
      | Some icon -> [ content; icon ]
      | None -> [ content ]

    let enabledRef: bool ref = ref true

    div
      [
        cl Css.``weave-expansion-panel__header``
        Flex.Flex.allSizes
        AlignItems.center
        AlignContent.spaceBetween
        JustifyContent.spaceBetween
        Attr.Style "width" "100%"
        enabled
        |> Attr.DynamicCustom(fun el isEnabled ->
          el.SetAttribute("data-weave-header-disabled", if isEnabled then "false" else "true")
          ExpansionPanel.syncHeaderAvailability el)
        Attr.Create "role" "button"
        on.afterRender (fun _ -> enabled |> View.Sink(fun v -> enabledRef.Value <- v))
        expanded.View
        |> View.Map(fun v -> if v then "true" else "false")
        |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-expanded", v))

        let clickable = View.zipCached expanded.View enabled

        on.clickView clickable
        <| fun el _ (isExpanded, isEnabled) ->
          if isEnabled && el.GetAttribute("aria-disabled") <> "true" then
            Var.Set expanded (not isExpanded)

        on.keyDown (fun el ev ->
          match ev with
          | Key.Activate ->
            ev.PreventDefault()

            if enabledRef.Value && el.GetAttribute("aria-disabled") <> "true" then
              Var.Set expanded (not expanded.Value)
          | _ -> ())

        Map.ofList [
          ExpansionPanel.HeaderVariant.Filled, Css.``weave-expansion-panel__header--filled``
          ExpansionPanel.HeaderVariant.Highlight, Css.``weave-expansion-panel__header--highlight``
        ]
        |> Attr.classSelection variant

        yield! attrs
      ]
      children

[<JavaScript; RequireQualifiedAccess>]
type ExpansionPanelContent =

  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    div [ cls [ Css.``weave-expansion-panel__content`` ]; yield! attrs ] [ content ]

[<JavaScript; RequireQualifiedAccess>]
type ExpansionPanel =

  static member create
    (header: Doc, content: Doc, ?expanded: Var<bool>, ?enabled: View<bool>, ?attrs: Attr list)
    =
    let expanded = defaultArg expanded (Var.Create false)
    let enabled = defaultArg enabled (View.Const true)
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
          headerEl.SetAttribute("id", headerId)

          enabled
          |> View.Sink(fun isEnabled ->
            headerEl.SetAttribute("data-weave-panel-disabled", if isEnabled then "false" else "true")
            ExpansionPanel.syncHeaderAvailability headerEl))

      yield! attrs
    ] [
      header
      div [
        cl Css.``weave-expansion-panel__content-wrapper``
        Attr.Create "id" panelId
        Attr.Create "role" "region"
        Attr.Create "aria-labelledby" headerId
        expanded.View
        |> Attr.DynamicCustom(fun el isExpanded ->
          if isExpanded then
            el.RemoveAttribute("inert")
          else
            el.SetAttribute("inert", ""))
      ] [ content ]
    ]
