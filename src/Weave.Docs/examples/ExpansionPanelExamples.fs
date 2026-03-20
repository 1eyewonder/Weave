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
          GridItem.create (col "Compact" Density.compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.create (
            col "Standard" Density.standard,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 2,
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
      ],
      attrs = [ Container.MaxWidth.large ]
    )
