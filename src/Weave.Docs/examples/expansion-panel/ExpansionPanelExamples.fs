namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module ExpansionPanelExamples =

  let private comparisonGrid columns items =
    let span =
      match columns with
      | 2 -> GridItem.Span.Medium.six
      | 4 -> GridItem.Span.Medium.three
      | _ -> GridItem.Span.Medium.four

    Grid.create (
      items
      |> List.map (fun item ->
        GridItem.create (item, attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; span ])),
      attrs = [ AlignItems.start ]
    )

  let private multiplePanels () =
    let description =
      Helpers.bodyText
        "Share the expanded Var between a panel and its header. Each panel below has independent state."

    let content =
      let createPanel headerAttrs label =
        let expanded = Var.Create false

        let header =
          ExpansionPanelHeader.create (
            text label,
            expanded,
            icon = ExpansionPanelHeader.defaultIcon expanded,
            attrs = headerAttrs
          )

        ExpansionPanel.create (
          header,
          content = ExpansionPanelContent.create (text (label + " content")),
          expanded = expanded
        )

      ExpansionPanelContainer.create ([ createPanel [] "Panel one"; createPanel [] "Panel two" ])

    let code =
      """let createPanel headerAttrs label =
  let expanded = Var.Create false

  let header =
    ExpansionPanelHeader.create (
      text label,
      expanded,
      icon = ExpansionPanelHeader.defaultIcon expanded,
      attrs = headerAttrs
    )

  ExpansionPanel.create (
    header,
    content = ExpansionPanelContent.create (text (label + " content")),
    expanded = expanded
  )

ExpansionPanelContainer.create (
  [ createPanel [] "Panel one"; createPanel [] "Panel two" ]
)
"""

    Helpers.codeSampleSection "Basic Expansion Panel" description content code

  let appearanceExample () =
    let color = Var.Create "Primary"
    let focus = Var.Create "Primary"
    let variant = Var.Create "Highlight"
    let density = Var.Create "Standard"
    let expanded = Var.Create false

    let choice label values (value: Var<string>) =
      let selected =
        Var.Lens value Some (fun current next -> Option.defaultValue current next)

      let items =
        values
        |> List.map (fun name -> SelectItem.create (text name, name, name))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice "Header" ExpansionAppearance.variants variant
        choice "Color" ExpansionAppearance.colors color
        choice "Focus color" ExpansionAppearance.colors focus
        choice "Density" ExpansionAppearance.densities density
      ]

    let colors =
      Map.ofList [
        "Primary", ExpansionPanel.Color.primary
        "Secondary", ExpansionPanel.Color.secondary
        "Tertiary", ExpansionPanel.Color.tertiary
        "Error", ExpansionPanel.Color.error
        "Warning", ExpansionPanel.Color.warning
        "Success", ExpansionPanel.Color.success
        "Info", ExpansionPanel.Color.info
      ]

    let focusColors =
      Map.ofList [
        "Primary", ExpansionPanel.FocusColor.primary
        "Secondary", ExpansionPanel.FocusColor.secondary
        "Tertiary", ExpansionPanel.FocusColor.tertiary
        "Error", ExpansionPanel.FocusColor.error
        "Warning", ExpansionPanel.FocusColor.warning
        "Success", ExpansionPanel.FocusColor.success
        "Info", ExpansionPanel.FocusColor.info
      ]

    let config =
      let appearance = (color.View, focus.View) ||> View.Map2(fun a b -> a, b)
      let layout = (variant.View, density.View) ||> View.Map2(fun a b -> a, b)
      (appearance, layout) ||> View.Map2(fun (a, b) (c, d) -> a, b, c, d)

    let preview =
      config
      |> Doc.BindView(fun (color, focus, variant, density) ->
        let headerVariant =
          match variant with
          | "Filled" -> ExpansionPanel.HeaderVariant.Filled
          | "None" -> ExpansionPanel.HeaderVariant.None
          | _ -> ExpansionPanel.HeaderVariant.Highlight

        let header =
          ExpansionPanelHeader.create (
            text "Details",
            expanded,
            icon = ExpansionPanelHeader.defaultIcon expanded,
            highlightVariant = View.Const headerVariant,
            attrs = [ Map.find color colors; Map.find focus focusColors ]
          )

        let panel =
          ExpansionPanel.create (
            header,
            content = ExpansionPanelContent.create (text "Panel content."),
            expanded = expanded
          )

        let attrs =
          match density with
          | "Compact" -> [ Density.compact ]
          | "Spacious" -> [ Density.spacious ]
          | _ -> []

        div [ Attr.Style "inline-size" "100%" ] [ ExpansionPanelContainer.create ([ panel ], attrs = attrs) ])

    let source =
      (config, expanded.View)
      ||> View.Map2(fun (color, focus, variant, density) isExpanded ->
        ExpansionAppearance.source color focus variant density isExpanded)

    let reset () =
      Var.Set color "Primary"
      Var.Set focus "Primary"
      Var.Set variant "Highlight"
      Var.Set density "Standard"
      Var.Set expanded false

    Helpers.appearanceSection
      [ "colors"; "header-highlight-variants"; "focus-color"; "density" ]
      (Helpers.bodyText
        "Choose a header treatment and colors. Open the panel to see its highlight; use the keyboard to see the independently chosen focus color.")
      controls
      None
      preview
      reset
      source

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for ExpansionPanel and its companion types.") [
      Helpers.apiTable "ExpansionPanel.create" [
        Helpers.apiParam "header" "Doc" "" "Header content (typically built with ExpansionPanelHeader.create)"
        Helpers.apiParam "content" "Doc" "" "Body content (typically wrapped in ExpansionPanelContent.create)"
        Helpers.apiParam
          "?expanded"
          "Var<bool>"
          "Var.Create false"
          "Two-way binding controlling whether the panel is open"
        Helpers.apiParam "?enabled" "View<bool>" "" "Whether the panel header responds to interaction"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the panel root element"
      ]

      Helpers.apiTable "ExpansionPanelHeader.create" [
        Helpers.apiParam "content" "Doc" "" "Content displayed in the header row"
        Helpers.apiParam "expanded" "Var<bool>" "" "Shared expanded state — toggled on click/keyboard"
        Helpers.apiParam
          "?enabled"
          "View<bool>"
          "View.Const true"
          "Whether the header responds to interaction"
        Helpers.apiParam "?icon" "Doc" "" "Custom expand/collapse icon (use ExpansionPanelIcon.create)"
        Helpers.apiParam
          "?highlightVariant"
          "View<HeaderVariant>"
          "View.Const Highlight"
          "Reactive header style when expanded — Filled, Highlight, or None"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the header element"
      ]

      Helpers.apiTable "ExpansionPanelContent.create" [
        Helpers.apiParam "content" "Doc" "" "Body content rendered inside the collapsible region"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the content wrapper"
      ]

      Helpers.apiTable "ExpansionPanelContainer.create" [
        Helpers.apiParam "panels" "Doc list" "" "List of ExpansionPanel docs to render as a group"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the container element"
      ]

      Helpers.apiTable "ExpansionPanelIcon.create" [
        Helpers.apiParam "unexpandedIcon" "Doc" "" "Icon shown when the panel is collapsed"
        Helpers.apiParam "expandedIcon" "Doc" "" "Icon shown when the panel is expanded"
        Helpers.apiParam "expanded" "Var<bool>" "" "Shared expanded state to switch between icons"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the icon wrapper"
      ]

      Helpers.styleModuleTable "ExpansionPanel.Color" [
        ("primary", "Primary brand color applied to header")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]

      Helpers.styleModuleTable "ExpansionPanel.FocusColor" [
        ("primary", "Primary color on focus")
        ("secondary", "Secondary color on focus")
        ("tertiary", "Tertiary color on focus")
        ("error", "Error color on focus")
        ("warning", "Warning color on focus")
        ("success", "Success color on focus")
        ("info", "Info color on focus")
      ]

      Helpers.styleModuleTable "ExpansionPanel.HeaderVariant (DU)" [
        ("Filled", "Solid background fill when expanded")
        ("Highlight", "Subtle highlight background when expanded (default)")
        ("None", "No visual change when expanded")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Expansion Panel"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The ExpansionPanel component allows for collapsible sections of content, useful for organizing information in a compact manner."
        ]

        Helpers.divider ()
        multiplePanels ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
