namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ExpansionPanelExamples =

  let private multiplePanels () =
    let description =
      Helpers.bodyText "Expansion panels can hold any type of content and can be expanded or collapsed."

    let content =
      let headerContent n expanded =
        ExpansionPanelHeader.create (
          div [ Typography.body1 ] [ text (sprintf "Expansion Panel %i" n) ],
          expanded,
          icon = ExpansionPanelHeader.defaultIcon expanded
        )

      let expansionContent n =
        ExpansionPanelContent.create (text (sprintf "Content %i" n))

      ExpansionPanelContainer.create (
        [
          let oneExpanded = Var.Create false

          ExpansionPanel.create (
            headerContent 1 oneExpanded,
            expanded = oneExpanded,
            content = expansionContent 1
          )

          let twoExpanded = Var.Create false

          ExpansionPanel.create (
            headerContent 2 twoExpanded,
            expanded = twoExpanded,
            content = expansionContent 2
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let expanded = Var.Create false

ExpansionPanelContainer.create(
    [
        ExpansionPanel.create(
            ExpansionPanelHeader.create(
                div [ Typography.body1 ] [ text "Expansion Panel 1" ],
                expanded,
                icon = ExpansionPanelHeader.defaultIcon expanded
            ),
            expanded = expanded,
            content = ExpansionPanelContent.create(text "Content 1")
        )
    ]
)
"""

    Helpers.codeSampleSection "Expansion Panel" description content code

  let private color () =
    let description =
      Helpers.bodyText "Expansion panel headers can be colored using different brand colors."

    let content =
      let colors = [
        "Primary", ExpansionPanel.Color.primary
        "Secondary", ExpansionPanel.Color.secondary
        "Tertiary", ExpansionPanel.Color.tertiary
        "Error", ExpansionPanel.Color.error
        "Warning", ExpansionPanel.Color.warning
        "Success", ExpansionPanel.Color.success
        "Info", ExpansionPanel.Color.info
      ]

      let header expanded (label: string) colorAttr =
        ExpansionPanelHeader.create (
          div [ Typography.body1 ] [ text label ],
          expanded,
          icon = ExpansionPanelHeader.defaultIcon expanded,
          attrs = [ colorAttr ]
        )

      let content label =
        ExpansionPanelContent.create (text label)

      let panel label colorAttr expanded =
        ExpansionPanel.create (header expanded label colorAttr, expanded = expanded, content = content label)

      ExpansionPanelContainer.create (
        [
          for (label, colorAttr) in colors do
            let expanded = Var.Create false
            panel label colorAttr expanded
        ]
      )

    let code =
      """open Weave

open WebSharper.UI

let expanded = Var.Create false

ExpansionPanelContainer.create(
    [
        ExpansionPanel.create(
            ExpansionPanelHeader.create(
                div [ Typography.body1 ] [ text "Primary" ],
                expanded,
                icon = ExpansionPanelHeader.defaultIcon expanded,
                attrs = [ ExpansionPanel.Color.primary ]
            ),
            expanded = expanded,
            content = ExpansionPanelContent.create(text "Primary")
        )
    ]
)
"""

    Helpers.codeSampleSection "Coloring" description content code

  let private highlightVariants () =
    let description =
      Helpers.bodyText "Expansion panel headers can have different variants: Filled, Highlight, and None."

    let content =
      let headerContent expanded variant =
        ExpansionPanelHeader.create (
          div [ Typography.body1 ] [ text (sprintf "%A" variant) ],
          expanded,
          icon = ExpansionPanelHeader.defaultIcon expanded,
          highlightVariant = View.Const variant,
          attrs = [ ExpansionPanel.Color.primary ]
        )

      let panelContent displayText =
        ExpansionPanelContent.create (text displayText)

      ExpansionPanelContainer.create (
        [
          let oneExpanded = Var.Create false

          ExpansionPanel.create (
            headerContent oneExpanded ExpansionPanel.HeaderVariant.Filled,
            expanded = oneExpanded,
            content = panelContent "Filled"
          )

          let twoExpanded = Var.Create false

          ExpansionPanel.create (
            headerContent twoExpanded ExpansionPanel.HeaderVariant.Highlight,
            expanded = twoExpanded,
            content = panelContent "Highlight"
          )

          let threeExpanded = Var.Create false

          ExpansionPanel.create (
            headerContent threeExpanded ExpansionPanel.HeaderVariant.None,
            expanded = threeExpanded,
            content = panelContent "None"
          )
        ]
      )

    let code =
      """open Weave

open WebSharper.UI

let expanded = Var.Create false

ExpansionPanelContainer.create(
    [
        ExpansionPanel.create(
            ExpansionPanelHeader.create(
                div [ Typography.body1 ] [ text "Filled" ],
                expanded,
                icon = ExpansionPanelHeader.defaultIcon expanded,
                highlightVariant = View.Const ExpansionPanel.HeaderVariant.Filled,
                attrs = [ ExpansionPanel.Color.primary ]
            ),
            expanded = expanded,
            content = ExpansionPanelContent.create(text "Filled")
        )
    ]
)
"""

    Helpers.codeSampleSection "Header Highlight Variants" description content code

  let private focusColor () =
    let description =
      Helpers.bodyText
        "The focus outline color can be customized per-header using FocusColor.*. The default focus color is action-default."

    let content =
      let colors = [
        "Primary", ExpansionPanel.FocusColor.primary
        "Secondary", ExpansionPanel.FocusColor.secondary
        "Error", ExpansionPanel.FocusColor.error
      ]

      let header expanded (label: string) colorAttr =
        ExpansionPanelHeader.create (
          div [ Typography.body1 ] [ text label ],
          expanded,
          icon = ExpansionPanelHeader.defaultIcon expanded,
          attrs = [ colorAttr ]
        )

      let content label =
        ExpansionPanelContent.create (text (sprintf "Tab into this panel to see the %s focus outline" label))

      let panel label colorAttr expanded =
        ExpansionPanel.create (header expanded label colorAttr, expanded = expanded, content = content label)

      ExpansionPanelContainer.create (
        [
          for (label, colorAttr) in colors do
            let expanded = Var.Create false
            panel label colorAttr expanded
        ]
      )

    let code =
      """open Weave

open WebSharper.UI

let expanded = Var.Create false

ExpansionPanelContainer.create(
    [
        ExpansionPanel.create(
            ExpansionPanelHeader.create(
                div [ Typography.body1 ] [ text "Primary" ],
                expanded,
                icon = ExpansionPanelHeader.defaultIcon expanded,
                attrs = [ ExpansionPanel.FocusColor.primary ]
            ),
            expanded = expanded,
            content = ExpansionPanelContent.create(text "Primary focus outline")
        )
    ]
)
"""

    Helpers.codeSampleSection "Focus Color" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls expansion panel header padding. Pass the density class in attrs to set it per-instance on the container."

    let content =
      let col (label: string) densityAttr =
        let expanded = Var.Create false

        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          ExpansionPanelContainer.create (
            [
              ExpansionPanel.create (
                ExpansionPanelHeader.create (
                  div [ Typography.body1 ] [ text "Panel Header" ],
                  expanded,
                  icon = ExpansionPanelHeader.defaultIcon expanded
                ),
                expanded = expanded,
                content = ExpansionPanelContent.create (text "Panel content")
              )
            ]
          )
        ]

      Grid.create (
        [
          GridItem.create (
            col "Compact" Density.compact,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Standard" Density.standard,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
        ],
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave


let expanded = Var.Create false

ExpansionPanelContainer.create(
    [
        ExpansionPanel.create(
            ExpansionPanelHeader.create(div [ Typography.body1 ] [ text "Panel Header" ], expanded, icon = ExpansionPanelHeader.defaultIcon expanded),
            expanded = expanded,
            content = ExpansionPanelContent.create(text "Panel content")
        )
    ],
    attrs = [ Density.compact ]
)
"""

    Helpers.codeSampleSection "Density" description content code

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
        Helpers.apiParam "?enabled" "View<bool>" "" "Whether the panel header is clickable"
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
        color ()
        Helpers.divider ()
        highlightVariants ()
        Helpers.divider ()
        focusColor ()
        Helpers.divider ()
        densityExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
