namespace Weave.Tests.E2E.Site

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TabsRegressionPage =

  let render () =
    let palettes = [
      "primary", Tabs.Color.primary, Tabs.HeaderBackground.primary
      "secondary", Tabs.Color.secondary, Tabs.HeaderBackground.secondary
      "tertiary", Tabs.Color.tertiary, Tabs.HeaderBackground.tertiary
      "error", Tabs.Color.error, Tabs.HeaderBackground.error
      "warning", Tabs.Color.warning, Tabs.HeaderBackground.warning
      "success", Tabs.Color.success, Tabs.HeaderBackground.success
      "info", Tabs.Color.info, Tabs.HeaderBackground.info
    ]

    div [] [
      for name, color, background in palettes do
        Tabs.create (
          View.Const [
            TabItem.create (
              "First tab",
              text "First panel",
              startIcon = Icon.create (Icon.UiActions UiActions.Home),
              endIcon = Icon.create (Icon.UiActions UiActions.Settings)
            )
            TabItem.create ("Second tab", text "Second panel")
            TabItem.create ("Third tab", text "Third panel")
            TabItem.create ("Fourth tab", text "Fourth panel")
          ],
          attrs = [
            Tabs.Variant.filled
            color
            background
            Attr.Create "data-testid" ("tabs-tray-" + name)
          ]
        )
    ]

  let initial position =
    Tabs.create (
      View.Const [
        TabItem.create ("Overview", text "Overview panel")
        TabItem.create ("Activity", text "Activity panel")
        TabItem.create ("Settings", text "Settings panel")
      ],
      activeIndex = Var.Create 2,
      position = position
    )

  let resizable position =
    div [] [
      div [ Attr.Style "max-width" "900px" ] [
        Tabs.create (
          View.Const [
            for label in [ "Identity"; "Abilities"; "Inventory"; "Skills"; "History"; "Settings" ] do
              TabItem.create (label, text (label + " panel"))
          ],
          position = position,
          attrs = [ Attr.Create "data-testid" "resize-tabs" ]
        )
      ]
      Button.create (text "Panel action", ignore, attrs = [ Attr.Create "data-testid" "panel-action" ])
    ]
