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

  let multiplePanels () =
    let headerContent n expanded =
      ExpansionPanelHeader.CreateWithDefaultIcons(Body1.Create(sprintf "Expansion Panel %i" n), expanded)

    ExpansionPanelContainer.Create(
      [
        let oneExpanded = Var.Create false
        ExpansionPanel.Create(headerContent 1 oneExpanded, expanded = oneExpanded, content = text "Content 1")

        let twoExpanded = Var.Create false
        ExpansionPanel.Create(headerContent 2 twoExpanded, expanded = twoExpanded, content = text "Content 2")
      ]
    )
    |> Helpers.section
      "Expansion Panel"
      (Helpers.bodyText "Expansion panels can hold any type of content and can be expanded or collapsed.")

  let color () =
    let header expanded color =
      ExpansionPanelHeader.CreateWithDefaultIcons(
        Body1.Create(sprintf "%A" color),
        expanded,
        attrs = [ cls [ ExpansionPanel.Color.toColor color ] ]
      )

    let panel color expanded =
      ExpansionPanel.Create(header expanded color, expanded = expanded, content = text (sprintf "%A" color))

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
    |> Helpers.section
      "Coloring"
      (Helpers.bodyText "Expansion panel headers can be colored using different brand colors.")

  let highlightVariants () =
    let headerContent expanded variant =
      ExpansionPanelHeader.CreateWithDefaultIcons(
        Body1.Create(sprintf "%A" variant),
        expanded,
        highlightVariant = View.Const variant,
        attrs = [ cls [ ExpansionPanel.Color.toColor BrandColor.Primary ] ]
      )

    ExpansionPanelContainer.Create(
      [
        let oneExpanded = Var.Create false

        ExpansionPanel.Create(
          headerContent oneExpanded ExpansionPanel.HeaderVariant.Filled,
          expanded = oneExpanded,
          content = text "Filled"
        )

        let twoExpanded = Var.Create false

        ExpansionPanel.Create(
          headerContent twoExpanded ExpansionPanel.HeaderVariant.Highlight,
          expanded = twoExpanded,
          content = text "Highlight"
        )

        let threeExpanded = Var.Create false

        ExpansionPanel.Create(
          headerContent threeExpanded ExpansionPanel.HeaderVariant.None,
          expanded = threeExpanded,
          content = text "None"
        )
      ]
    )
    |> Helpers.section
      "Header Highlight Variants"
      (Helpers.bodyText "Expansion panel headers can have different variants: Filled, Highlight, and None.")

  let render () =
    Container.Create(
      div [] [
        H1.Create("ExpansionPanel Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(
          "The ExpansionPanel component allows for collapsible sections of content, useful for organizing information in a compact manner.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        multiplePanels ()
        Helpers.divider ()
        color ()
        Helpers.divider ()
        highlightVariants ()
      ]
    )
