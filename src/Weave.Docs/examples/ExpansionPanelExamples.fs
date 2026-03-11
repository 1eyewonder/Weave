namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Container
open Weave.CssHelpers

[<JavaScript>]
module ExpansionPanelExamples =

  let private multiplePanels () =
    let description =
      Helpers.bodyText "Expansion panels can hold any type of content and can be expanded or collapsed."

    let content =
      let headerContent n expanded =
        ExpansionPanelHeader.CreateWithDefaultIcons(Body1.Div(sprintf "Expansion Panel %i" n), expanded)

      let expansionContent n =
        ExpansionPanelContent.Create(text (sprintf "Content %i" n))

      ExpansionPanelContainer.Create(
        [
          let oneExpanded = Var.Create false

          ExpansionPanel.Create(
            headerContent 1 oneExpanded,
            expanded = oneExpanded,
            content = expansionContent 1
          )

          let twoExpanded = Var.Create false

          ExpansionPanel.Create(
            headerContent 2 twoExpanded,
            expanded = twoExpanded,
            content = expansionContent 2
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let expanded = Var.Create false // see here

ExpansionPanelContainer.Create(
    [
        ExpansionPanel.Create(
            ExpansionPanelHeader.CreateWithDefaultIcons(
                Body1.Div("Expansion Panel 1"),
                expanded
            ),
            expanded = expanded,
            content = ExpansionPanelContent.Create(text "Content 1")
        )
    ]
)
"""

    Helpers.codeSampleSection "Expansion Panel" description content code

  let private color () =
    let description =
      Helpers.bodyText "Expansion panel headers can be colored using different brand colors."

    let content =
      let header expanded color =
        ExpansionPanelHeader.CreateWithDefaultIcons(
          Body1.Div(sprintf "%A" color),
          expanded,
          attrs = [ cls [ ExpansionPanel.Color.toClass color ] ]
        )

      let content (color: BrandColor) =
        ExpansionPanelContent.Create(text (sprintf "%A" color))

      let panel color expanded =
        ExpansionPanel.Create(header expanded color, expanded = expanded, content = content color)

      ExpansionPanelContainer.Create(
        [
          let primaryExpanded = Var.Create false
          panel BrandColor.Primary primaryExpanded

          let secondaryExpanded = Var.Create false
          panel BrandColor.Secondary secondaryExpanded

          let tertiaryExpanded = Var.Create false
          panel BrandColor.Tertiary tertiaryExpanded

          let errorExpanded = Var.Create false
          panel BrandColor.Error errorExpanded

          let warningExpanded = Var.Create false
          panel BrandColor.Warning warningExpanded

          let successExpanded = Var.Create false
          panel BrandColor.Success successExpanded

          let infoExpanded = Var.Create false
          panel BrandColor.Info infoExpanded
        ]
      )

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

let expanded = Var.Create false

ExpansionPanelContainer.Create(
    [
        ExpansionPanel.Create(
            ExpansionPanelHeader.CreateWithDefaultIcons(
                Body1.Div("Primary"),
                expanded,
                attrs = [ cls [ ExpansionPanel.Color.toClass BrandColor.Primary ] ] // see here
            ),
            expanded = expanded,
            content = ExpansionPanelContent.Create(text "Primary")
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
        ExpansionPanelHeader.CreateWithDefaultIcons(
          Body1.Div(sprintf "%A" variant),
          expanded,
          highlightVariant = View.Const variant,
          attrs = [ cls [ ExpansionPanel.Color.toClass BrandColor.Primary ] ]
        )

      let panelContent displayText =
        ExpansionPanelContent.Create(text displayText)

      ExpansionPanelContainer.Create(
        [
          let oneExpanded = Var.Create false

          ExpansionPanel.Create(
            headerContent oneExpanded ExpansionPanel.HeaderVariant.Filled,
            expanded = oneExpanded,
            content = panelContent "Filled"
          )

          let twoExpanded = Var.Create false

          ExpansionPanel.Create(
            headerContent twoExpanded ExpansionPanel.HeaderVariant.Highlight,
            expanded = twoExpanded,
            content = panelContent "Highlight"
          )

          let threeExpanded = Var.Create false

          ExpansionPanel.Create(
            headerContent threeExpanded ExpansionPanel.HeaderVariant.None,
            expanded = threeExpanded,
            content = panelContent "None"
          )
        ]
      )

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

let expanded = Var.Create false

ExpansionPanelContainer.Create(
    [
        ExpansionPanel.Create(
            ExpansionPanelHeader.CreateWithDefaultIcons(
                Body1.Div("Filled"),
                expanded,
                highlightVariant = View.Const ExpansionPanel.HeaderVariant.Filled, // see here
                attrs = [ cls [ ExpansionPanel.Color.toClass BrandColor.Primary ] ]
            ),
            expanded = expanded,
            content = ExpansionPanelContent.Create(text "Filled")
        )
    ]
)
"""

    Helpers.codeSampleSection "Header Highlight Variants" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls expansion panel header padding. Pass the density class in attrs to set it per-instance on the container."

    let content =
      let col density =
        let label = sprintf "%A" density
        let expanded = Var.Create false

        div [ cl (Density.toClass density) ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          ExpansionPanelContainer.Create(
            [
              ExpansionPanel.Create(
                ExpansionPanelHeader.CreateWithDefaultIcons(Body1.Div("Panel Header"), expanded),
                expanded = expanded,
                content = ExpansionPanelContent.Create(text "Panel content")
              )
            ]
          )
        ]

      Grid.Create(
        [
          GridItem.Create(col Density.Compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Standard, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Spacious, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.toClass AlignItems.Start |> cl ]
      )

    let code =
      """open Weave
open Weave.CssHelpers

let expanded = Var.Create false

ExpansionPanelContainer.Create(
    [
        ExpansionPanel.Create(
            ExpansionPanelHeader.CreateWithDefaultIcons(Body1.Div("Panel Header"), expanded),
            expanded = expanded,
            content = ExpansionPanelContent.Create(text "Panel content")
        )
    ],
    attrs = [ cl (Density.toClass Density.Compact) ] // see here
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Expansion Panel"
        Body1.Div(
          "The ExpansionPanel component allows for collapsible sections of content, useful for organizing information in a compact manner.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        multiplePanels ()
        Helpers.divider ()
        color ()
        Helpers.divider ()
        highlightVariants ()
        Helpers.divider ()
        densityExample ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
