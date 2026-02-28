namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Tabs
open Weave.CssHelpers
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TabsExamples =

  let private basicExample () =
    let description = Helpers.bodyText "A simple horizontal tab strip at the top."

    let content =
      Tabs.Create(
        [
          TabDef.createText "Tab One" (Body1.Div("Content One"))
          TabDef.createText "Tab Two" (Body1.Div("Content Two"))
          TabDef.createText "Tab Three" (Body1.Div("Content Three"))
        ]
      )

    let code =
      """open Weave

Tabs.Create(
    [
        TabDef.create "Tab One" (Body1.Div("Content One"))
        TabDef.create "Tab Two" (Body1.Div("Content Two"))
        TabDef.create "Tab Three" (Body1.Div("Content Three"))
    ]
)
"""

    Helpers.codeSampleSection "Basic Tabs" description content code

  let private iconExample () =
    let description =
      Helpers.bodyText "Tabs can be adorned with icons next to the label."

    let content =
      Tabs.Create(
        [
          TabDef.createWithStartIcon
            "Home"
            (Icon.Create(Icon.UiActions UiActions.Home))
            (Body1.Div("Home content"))
          TabDef.createWithStartIcon
            "Settings"
            (Icon.Create(Icon.UiActions UiActions.Settings))
            (Body1.Div("Settings content"))
          TabDef.createWithStartIcon
            "Info"
            (Icon.Create(Icon.Action Action.Info))
            (Body1.Div("Information content"))
        ]
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

Tabs.Create(
    [
        TabDef.createWithStartIcon
            "Home"
            (Icon.Create(Icon.UiActions UiActions.Home))
            (Body1.Div("Home content"))
        TabDef.createWithStartIcon
            "Settings"
            (Icon.Create(Icon.UiActions UiActions.Settings))
            (Body1.Div("Settings content"))
        TabDef.createWithStartIcon
            "Info"
            (Icon.Create(Icon.Action Action.Info))
            (Body1.Div("Information content"))
    ]
)
"""

    Helpers.codeSampleSection "Tabs with Icons" description content code

  let private disabledExample () =
    let description = Helpers.bodyText "Individual tabs can be disabled."

    let content =
      Tabs.Create(
        [
          TabDef.createText "Tab One" (Body1.Div("Content One"))
          TabDef.createText "Tab Two" (Body1.Div("Content Two"))
          TabDef.createText "Tab Three" (Body1.Div("Content Three"))
          TabDef.createText "Tab Disabled" (Body1.Div("You should not see this"))
          |> TabDef.withDisabled (View.Const true)
        ]
      )

    let code =
      """open Weave

Tabs.Create(
    [
        TabDef.create "Tab One" (Body1.Div("Content One"))
        TabDef.create "Tab Two" (Body1.Div("Content Two"))
        TabDef.create "Tab Three" (Body1.Div("Content Three"))
        TabDef.create "Tab Disabled" (Body1.Div("You should not see this"))
        |> TabDef.withDisabled (View.Const true)
    ]
)
"""

    Helpers.codeSampleSection "Disabled Tabs" description content code

  let private colorExample () =
    let description =
      Helpers.bodyText "The active indicator and text support all brand colors."

    let content =
      div [
        cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ]
        Attr.Style "gap" "24px"
      ] [
        Tabs.Create(
          [
            TabDef.createText "Primary" (Body1.Div("Primary color"))
            TabDef.createText "Tab Two" (Body1.Div("Content Two"))
          ],
          color = BrandColor.Primary
        )
        Tabs.Create(
          [
            TabDef.createText "Secondary" (Body1.Div("Secondary color"))
            TabDef.createText "Tab Two" (Body1.Div("Content Two"))
          ],
          color = BrandColor.Secondary
        )
        Tabs.Create(
          [
            TabDef.createText "Error" (Body1.Div("Error color"))
            TabDef.createText "Tab Two" (Body1.Div("Content Two"))
          ],
          color = BrandColor.Error
        )
      ]

    let code =
      """open Weave

Tabs.Create(
    [
        TabDef.create "Primary" (Body1.Div("Primary color"))
        TabDef.create "Tab Two" (Body1.Div("Content Two"))
    ],
    color = BrandColor.Primary
)

Tabs.Create(
    [
        TabDef.create "Secondary" (Body1.Div("Secondary color"))
        TabDef.create "Tab Two" (Body1.Div("Content Two"))
    ],
    color = BrandColor.Secondary
)
"""

    Helpers.codeSampleSection "Color Variants" description content code

  let private positionExample () =
    let description =
      Helpers.bodyText "Tabs support six positions: Top, Bottom, Left, Right, Start, and End."

    let makeTabs pos label =
      let isVertical =
        match pos with
        | Tabs.Position.Left
        | Tabs.Position.Right
        | Tabs.Position.Start
        | Tabs.Position.End -> true
        | _ -> false

      div [
        Attr.Style "border" "1px solid var(--palette-divider)"
        BorderRadius.toClass BorderRadius.All.small |> cl
        Attr.Style "min-height" "160px"
        Attr.Style "display" "flex"
        Attr.Style "flex-direction" (if isVertical then "row" else "column")
        Attr.Style "width" "100%"
      ] [
        Tabs.Create(
          [
            TabDef.createText "Item One" (Body1.Div(sprintf "%s content" label))
            TabDef.createText "Item Two" (Body1.Div("Content Two"))
            TabDef.createText "Item Three" (Body1.Div("Content Three"))
          ],
          position = pos
        )
      ]

    let content =
      Grid.Create(
        [
          GridItem.Create(
            makeTabs Tabs.Position.Top "Top",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.Create(
            makeTabs Tabs.Position.Bottom "Bottom",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.Create(
            makeTabs Tabs.Position.Left "Left",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.Create(
            makeTabs Tabs.Position.Right "Right",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 4
      )

    let code =
      """open Weave

Tabs.Create(
    [
        TabDef.create "Item One" (Body1.Div("Top content"))
        TabDef.create "Item Two" (Body1.Div("Content Two"))
        TabDef.create "Item Three" (Body1.Div("Content Three"))
    ],
    position = Tabs.Position.Top
)

Tabs.Create(
    [
        TabDef.create "Item One" (Body1.Div("Left content"))
        TabDef.create "Item Two" (Body1.Div("Content Two"))
        TabDef.create "Item Three" (Body1.Div("Content Three"))
    ],
    position = Tabs.Position.Left
)
"""

    Helpers.codeSampleSection "Tab Positions" description content code

  let private centeredExample () =
    let description =
      Helpers.bodyText
        "When tabs fit within the container, they can be centered instead of aligned at the start."

    let content =
      Tabs.Create(
        [
          TabDef.createText "One" (Body1.Div("Content One"))
          TabDef.createText "Two" (Body1.Div("Content Two"))
        ],
        alignment = Tabs.Alignment.Center
      )

    let code =
      """open Weave

Tabs.Create(
    [
        TabDef.create "One" (Body1.Div("Content One"))
        TabDef.create "Two" (Body1.Div("Content Two"))
    ],
    alignment = Tabs.Alignment.Center
)
"""

    Helpers.codeSampleSection "Centered Tabs" description content code

  let private scrollableExample () =
    let description =
      Helpers.bodyText "When tabs overflow the container, scroll buttons appear to navigate."

    let content =
      div [ Attr.Style "max-width" "500px" ] [
        Tabs.Create(
          [
            for i in 1..12 do
              TabDef.createText (sprintf "Tab %d" i) (Body1.Div(sprintf "Content %d" i))
          ]
        )
      ]

    let code =
      """open Weave

// Constrain the container width so overflow triggers scroll buttons
div [ Attr.Style "max-width" "500px" ] [
    Tabs.Create(
        [
            for i in 1..12 do
                TabDef.create (sprintf "Tab %d" i) (Body1.Div(sprintf "Content %d" i))
        ]
    )
]
"""

    Helpers.codeSampleSection "Scrollable Tabs" description content code

  let private customScrollIconsExample () =
    let description =
      Helpers.bodyText
        "The default scroll buttons use simple Unicode arrows, but you can provide your own icons via the scrollBackIcon and scrollForwardIcon parameters."

    let content =
      div [ Attr.Style "max-width" "500px" ] [
        Tabs.Create(
          [
            for i in 1..12 do
              TabDef.createText (sprintf "Tab %d" i) (Body1.Div(sprintf "Content %d" i))
          ],
          scrollBackIcon = Icon.Create(Icon.UiActions UiActions.ChevronLeft),
          scrollForwardIcon = Icon.Create(Icon.UiActions UiActions.ChevronRight)
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

div [ Attr.Style "max-width" "500px" ] [
    Tabs.Create(
        [
            for i in 1..12 do
                TabDef.create (sprintf "Tab %d" i) (Body1.Div(sprintf "Content %d" i))
        ],
        scrollBackIcon = Icon.Create(Icon.UiActions UiActions.ChevronLeft),
        scrollForwardIcon = Icon.Create(Icon.UiActions UiActions.ChevronRight)
    )
]
"""

    Helpers.codeSampleSection "Custom Scroll Icons" description content code

  let render () =
    Container.Create(
      div [ cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] ] [
        H1.Div("Tabs", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

        Body1.Div(
          "Tabs organize content across different screens and views.",
          attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
        )

        basicExample ()
        Helpers.divider ()
        iconExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        colorExample ()
        Helpers.divider ()
        positionExample ()
        Helpers.divider ()
        centeredExample ()
        Helpers.divider ()
        scrollableExample ()
        Helpers.divider ()
        customScrollIconsExample ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
