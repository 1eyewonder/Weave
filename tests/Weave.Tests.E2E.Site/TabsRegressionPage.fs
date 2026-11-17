namespace Weave.Tests.E2E.Site

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons.MaterialSymbols

[<JavaScript>]
type private TabsCaller =

  static member create
    (
      tabs: View<Tabs.TabDef list>,
      ?activeIndex: Var<int>,
      ?position: Tabs.Position,
      ?scrollBackIcon: Doc,
      ?scrollForwardIcon: Doc,
      ?attrs: Attr list
    ) =
    Tabs.create (
      tabs,
      ?activeIndex = activeIndex,
      ?position = position,
      ?scrollBackIcon = scrollBackIcon,
      ?scrollForwardIcon = scrollForwardIcon,
      ?attrs = attrs
    )

[<JavaScript>]
module TabsRegressionPage =

  let namedGroups () =
    let tabs () =
      View.Const [
        TabItem.create ("Overview", text "Overview panel")
        TabItem.createIconOnly (
          Icon.create (Icon.UiActions UiActions.Settings, attrs = [ Attr.Create "aria-hidden" "true" ]),
          text "Preferences panel",
          label = "Settings"
        )
      ]

    div [] [
      Tabs.create (
        tabs (),
        tabListAttrs = [ Attr.Create "aria-label" "Project" ],
        attrs = [ Attr.Create "data-testid" "project-tabs" ]
      )
      h2 [ attr.id "account-heading" ] [ text "Account" ]
      Tabs.create (
        tabs (),
        position = Tabs.Position.Left,
        tabListAttrs = [ Attr.Create "aria-labelledby" "account-heading" ],
        attrs = [ Attr.Create "data-testid" "account-tabs" ]
      )
    ]

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
    TabsCaller.create (
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
