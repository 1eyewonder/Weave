namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Tabs

open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TabsExamples =

  let private comparisonGrid columns items =
    let cells =
      items
      |> List.map (fun item ->
        GridItem.create (
          item,
          attrs = [
            Attr.Style "min-inline-size" "0"
            GridItem.Span.twelve
            GridItem.Span.Small.six
            (if columns = 4 then GridItem.Span.Medium.three
             elif columns = 3 then GridItem.Span.Medium.four
             else GridItem.Span.Medium.six)
          ]
        ))

    Grid.create (cells, attrs = [ Grid.Spacing.large ])

  let private basicExample () =
    let description =
      Helpers.bodyText "The default configuration: a horizontal text tab strip with the first tab active."

    let content =
      Tabs.create (
        View.Const [
          TabItem.create ("Tab One", text "Content One")
          TabItem.create ("Tab Two", text "Content Two")
          TabItem.create ("Tab Three", text "Content Three")
        ],
        attrs = [ Color.primary ]
      )

    let code =
      """open Weave.Tabs

Tabs.create (
  View.Const [
    TabItem.create ("Tab One", text "Content One")
    TabItem.create ("Tab Two", text "Content Two")
    TabItem.create ("Tab Three", text "Content Three")
  ],
  attrs = [ Color.primary ]
)
"""

    Helpers.codeSampleSection "Basic Tabs" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Set a tab as non-interactive using the disabled parameter. Disabled tabs are visually muted and cannot be clicked."

    let content =
      Tabs.create (
        View.Const [
          TabItem.create ("Tab One", text "Content One")
          TabItem.create ("Tab Two", text "Content Two")
          TabItem.create ("Tab Three", text "Content Three")
          TabItem.create ("Tab Disabled", text "You should not see this", disabled = View.Const true)
        ],
        attrs = [ Color.tertiary ]
      )

    let code =
      """open Weave.Tabs

Tabs.create (
  View.Const [
    TabItem.create ("Tab One", text "Content One")
    TabItem.create ("Tab Two", text "Content Two")
    TabItem.create ("Tab Three", text "Content Three")
    TabItem.create (
      "Tab Disabled",
      text "You should not see this",
      disabled = View.Const true
    )
  ],
  attrs = [ Color.tertiary ]
)
"""

    Helpers.codeSampleSection "Disabled" description content code

  let private scrollableExample () =
    let description =
      Helpers.bodyText
        "When tabs overflow the container, scroll buttons appear automatically. Constrain the container width to trigger this behavior."

    let content =
      div [ Attr.Style "max-width" "500px"; Attr.Style "margin-inline" "auto" ] [
        Tabs.create (
          View.Const [
            for i in 1..12 do
              TabItem.create (sprintf "Tab %d" i, text (sprintf "Content %d" i))
          ],
          attrs = [ Color.primary ]
        )
      ]

    let code =
      """open Weave.Tabs

Tabs.create (
  View.Const [
    for i in 1..12 do
      TabItem.create (sprintf "Tab %d" i, text (sprintf "Content %d" i))
  ],
  attrs = [ Color.primary ]
)
"""

    Helpers.codeSampleSection "Scrollable Tabs" description content code

  let private customScrollIconsExample () =
    let description =
      Helpers.bodyText
        "Replace the default Unicode arrow buttons with custom icons via scrollBackIcon and scrollForwardIcon."

    let content =
      div [ Attr.Style "max-width" "500px"; Attr.Style "margin-inline" "auto" ] [
        Tabs.create (
          View.Const [
            for i in 1..12 do
              TabItem.create (sprintf "Tab %d" i, text (sprintf "Content %d" i))
          ],
          scrollBackIcon = Icon.create (Icon.UiActions UiActions.ChevronLeft),
          scrollForwardIcon = Icon.create (Icon.UiActions UiActions.ChevronRight),
          attrs = [ Color.secondary ]
        )
      ]

    let code =
      """open Weave.Tabs
open Weave.Icons
open Weave.Icons.MaterialSymbols

Tabs.create (
  View.Const [
    for i in 1..12 do
      TabItem.create (sprintf "Tab %d" i, text (sprintf "Content %d" i))
  ],
  scrollBackIcon = Icon.create (Icon.UiActions UiActions.ChevronLeft),
  scrollForwardIcon = Icon.create (Icon.UiActions UiActions.ChevronRight),
  attrs = [ Color.secondary ]
)
"""

    Helpers.codeSampleSection "Custom Scroll Icons" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Tabs, TabItem, and TabPanel.") [
      Helpers.apiTable "Tabs.create" [
        Helpers.apiParam "tabs" "View<TabDef list>" "" "Reactive list of tab definitions built with TabItem"
        Helpers.apiParam
          "?activeIndex"
          "Var<int>"
          "Var.Create 0"
          "Two-way binding for the currently active tab index"
        Helpers.apiParam
          "?position"
          "Position"
          "Top"
          "Position of the tab strip relative to the panel content"
        Helpers.apiParam
          "?scrollBackIcon"
          "Doc"
          ""
          "Custom icon for the scroll-back button (shown when tabs overflow)"
        Helpers.apiParam "?scrollForwardIcon" "Doc" "" "Custom icon for the scroll-forward button"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
      ]

      Helpers.returnTypeNote
        "TabItem.create returns a Tabs.TabDef record, not a Doc. Build items with TabItem and pass them as a reactive list to Tabs.create."

      Helpers.apiTable "TabItem.create" [
        Helpers.apiParam "label" "string" "" "Text label displayed in the tab button"
        Helpers.apiParam "panel" "Doc" "" "Content rendered when this tab is active"
        Helpers.apiParam "?startIcon" "Doc" "" "Icon rendered before the label"
        Helpers.apiParam "?endIcon" "Doc" "" "Icon rendered after the label"
        Helpers.apiParam "?disabled" "View<bool>" "View.Const false" "Whether this tab is non-interactive"
      ]

      Helpers.apiTable "TabItem.createIconOnly" [
        Helpers.apiParam "icon" "Doc" "" "Icon displayed as the sole tab content"
        Helpers.apiParam "panel" "Doc" "" "Content rendered when this tab is active"
        Helpers.apiParam "?disabled" "View<bool>" "View.Const false" "Whether this tab is non-interactive"
      ]

      Helpers.apiTable "TabItem.createCustom" [
        Helpers.apiParam "header" "Doc" "" "Fully custom header Doc for the tab button"
        Helpers.apiParam "panel" "Doc" "" "Content rendered when this tab is active"
        Helpers.apiParam "?disabled" "View<bool>" "View.Const false" "Whether this tab is non-interactive"
      ]

      Helpers.apiTable "TabPanel.create" [
        Helpers.apiParam "content" "Doc" "" "Panel content for this tab"
        Helpers.apiParam "index" "int" "" "Zero-based index of this panel"
        Helpers.apiParam "activeIndex" "View<int>" "" "View of the currently active tab index"
        Helpers.apiParam
          "?groupId"
          "int"
          ""
          "Group identifier for ARIA linking (auto-generated by Tabs.create)"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the panel element"
      ]

      Helpers.styleModuleTable "Tabs.Position (DU)" [
        ("Top", "Tab strip above the panel (default)")
        ("Bottom", "Tab strip below the panel")
        ("Left", "Tab strip to the left of the panel")
        ("Right", "Tab strip to the right of the panel")
        ("Start", "Tab strip at the inline start (LTR: left, RTL: right)")
        ("End", "Tab strip at the inline end (LTR: right, RTL: left)")
      ]

      Helpers.styleModuleTable "Tabs.Variant" [
        ("text", "Text-only tab buttons")
        ("outlined", "Outlined tab buttons with border")
        ("filled", "Filled tab buttons with background")
      ]

      Helpers.styleModuleTable "Tabs.Color" [
        ("primary", "Primary brand color for the active indicator")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]

      Helpers.styleModuleTable "Tabs.HeaderBackground" [
        ("primary", "Primary tray with its matching foreground and selected-pill boundary")
        ("secondary", "Secondary brand color background")
        ("tertiary", "Tertiary brand color background")
        ("error", "Error/red background")
        ("warning", "Warning/orange background")
        ("success", "Success/green background")
        ("info", "Info/blue background")
        ("custom value",
         "Custom CSS background resets the inherited tray foreground; pair --tabs-header-foreground when the default text color does not contrast")
      ]

      Helpers.styleModuleTable "Tabs.Alignment" [
        ("center", "Center-align tabs when they don't fill the container")
      ]
    ]

  let render () =
    Container.create (
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] [
        Helpers.pageTitle "Tabs"

        div [ Typography.body1; Margin.Bottom.small ] [
          text "Tabs organize content across different screens and views."
        ]

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        TabsAppearance.render ()
        Helpers.divider ()
        scrollableExample ()
        Helpers.divider ()
        customScrollIconsExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
