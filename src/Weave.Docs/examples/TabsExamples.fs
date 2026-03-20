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

  let private basicExample () =
    let description =
      Helpers.bodyText "The default configuration: a horizontal text tab strip with the first tab active."

    let content =
      Tabs.create (
        View.Const [
          div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
          div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
          div [ Typography.body1 ] [ text "Content Three" ]
          |> TabDef.createText "Tab Three"
        ],
        attrs = [ Color.primary ]
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
        div [ Typography.body1 ] [ text "Content Three" ] |> TabDef.createText "Tab Three"
    ],
    attrs = [ Color.primary ]
)
"""

    Helpers.codeSampleSection "Basic Tabs" description content code

  let private colorExample () =
    let description =
      Helpers.bodyText
        "Pass a color class via attrs to tint the active indicator and tab label. All brand colors are supported."

    let content =
      Grid.create (
        [
          GridItem.create (
            Tabs.create (
              View.Const [
                div [ Typography.body1 ] [ text "Primary color" ] |> TabDef.createText "Primary"
                div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
              ],
              attrs = [ Color.primary ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Tabs.create (
              View.Const [
                div [ Typography.body1 ] [ text "Secondary color" ]
                |> TabDef.createText "Secondary"
                div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
              ],
              attrs = [ Color.secondary ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Tabs.create (
              View.Const [
                div [ Typography.body1 ] [ text "Tertiary color" ]
                |> TabDef.createText "Tertiary"
                div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
              ],
              attrs = [ Color.tertiary ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Tabs.create (
              View.Const [
                div [ Typography.body1 ] [ text "Success color" ] |> TabDef.createText "Success"
                div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
              ],
              attrs = [ Color.success ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Tabs.create (
              View.Const [
                div [ Typography.body1 ] [ text "Warning color" ] |> TabDef.createText "Warning"
                div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
              ],
              attrs = [ Color.warning ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Tabs.create (
              View.Const [
                div [ Typography.body1 ] [ text "Error color" ] |> TabDef.createText "Error"
                div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
              ],
              attrs = [ Color.error ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )
        ]
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Primary color" ] |> TabDef.createText "Primary"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
    ],
    attrs = [ Color.primary ]
)

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Secondary color" ] |> TabDef.createText "Secondary"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
    ],
    attrs = [ Color.secondary ]
)

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Tertiary color" ] |> TabDef.createText "Tertiary"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
    ],
    attrs = [ Color.tertiary ]
)

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Success color" ] |> TabDef.createText "Success"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
    ],
    attrs = [ Color.success ]
)

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Warning color" ] |> TabDef.createText "Warning"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
    ],
    attrs = [ Color.warning ]
)

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Error color" ] |> TabDef.createText "Error"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
    ],
    attrs = [ Color.error ]
)
"""

    Helpers.codeSampleSection "Color Variants" description content code

  let private variantExample () =
    let description =
      Helpers.bodyText
        "Tabs support three visual styles: Text (default), Outlined, and Filled. Pass a variant class via attrs. For the Filled variant, use HeaderBackground.toAttr to optionally set a background tray on the header strip."

    let tabs () =
      [
        div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
        div [ Typography.body1 ] [ text "Content Three" ]
        |> TabDef.createText "Tab Three"
      ]
      |> View.Const

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              div [ Typography.h4; Margin.Bottom.extraSmall ] [ text "Text Tab" ]
              Tabs.create (tabs (), attrs = [ Variant.text; Color.primary ])
            ],
            xs = Grid.Width.create 12,
            lg = Grid.Width.create 6
          )

          GridItem.create (
            div [] [
              div [ Typography.h4; Margin.Bottom.extraSmall ] [ text "Outlined Tab" ]
              Tabs.create (tabs (), attrs = [ Variant.outlined; Color.secondary ])
            ],
            xs = Grid.Width.create 12,
            lg = Grid.Width.create 6
          )

          GridItem.create (
            div [] [
              div [ Typography.h4; Margin.Bottom.extraSmall ] [ text "Filled Tab" ]
              Tabs.create (tabs (), attrs = [ Variant.filled; Color.tertiary ])
            ],
            xs = Grid.Width.create 12,
            lg = Grid.Width.create 6
          )
        ],
        justify = JustifyContent.spaceAround
      )

    let code =
      """open Weave
open Weave.Tabs

let tabs =
    View.Const [
        div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
        div [ Typography.body1 ] [ text "Content Three" ] |> TabDef.createText "Tab Three"
    ]

Tabs.create(tabs, attrs = [ Variant.text; Color.primary ])

Tabs.create(tabs, attrs = [ Variant.outlined; Color.secondary ])

Tabs.create(tabs, attrs = [ Variant.filled; Color.tertiary ])

// Filled with a tinted header tray
Tabs.create(
    tabs,
    attrs = [
        Variant.filled; Color.tertiary
        HeaderBackground.toAttr Color.Tertiary
    ]
)
"""

    Helpers.codeSampleSection "Variants" description content code

  let private iconExample () =
    let description =
      Helpers.bodyText
        "Tabs can include icons alongside the label using createWithStartIcon or createWithEndIcon."

    let content =
      Tabs.create (
        View.Const [
          div [ Typography.body1 ] [ text "Home content" ]
          |> TabDef.createWithStartIcon "Home" (Icon.create (Icon.UiActions UiActions.Home))
          div [ Typography.body1 ] [ text "Settings content" ]
          |> TabDef.createWithStartIcon "Settings" (Icon.create (Icon.UiActions UiActions.Settings))
          div [ Typography.body1 ] [ text "Information content" ]
          |> TabDef.createWithStartIcon "Info" (Icon.create (Icon.Action Action.Info))
        ],
        attrs = [ Color.secondary ]
      )

    let code =
      """open Weave
open Weave.Tabs
open Weave.Icons
open Weave.Icons.MaterialSymbols

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Home content" ]
        |> TabDef.createWithStartIcon "Home" (Icon.create(Icon.UiActions UiActions.Home))
        div [ Typography.body1 ] [ text "Settings content" ]
        |> TabDef.createWithStartIcon "Settings" (Icon.create(Icon.UiActions UiActions.Settings))
        div [ Typography.body1 ] [ text "Information content" ]
        |> TabDef.createWithStartIcon "Info" (Icon.create(Icon.Action Action.Info))
    ],
    attrs = [ Color.secondary ]
)
"""

    Helpers.codeSampleSection "Tabs with Icons" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Set a tab as non-interactive using TabDef.withDisabled. Disabled tabs are visually muted and cannot be clicked."

    let content =
      Tabs.create (
        View.Const [
          div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
          div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
          div [ Typography.body1 ] [ text "Content Three" ]
          |> TabDef.createText "Tab Three"
          div [ Typography.body1 ] [ text "You should not see this" ]
          |> TabDef.createText "Tab Disabled"
          |> TabDef.withDisabled (View.Const true)
        ],
        attrs = [ Color.tertiary ]
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
        div [ Typography.body1 ] [ text "Content Three" ] |> TabDef.createText "Tab Three"
        div [ Typography.body1 ] [ text "You should not see this" ]
        |> TabDef.createText "Tab Disabled"
        |> TabDef.withDisabled (View.Const true)
    ],
    attrs = [ Color.tertiary ]
)
"""

    Helpers.codeSampleSection "Disabled Tabs" description content code

  let private positionExample () =
    let description =
      Helpers.bodyText
        "The tab strip can be placed on any side of the content panel: Top, Bottom, Left, Right, Start, or End."

    let makeTabs pos label =
      let isVertical =
        match pos with
        | Position.Left
        | Position.Right
        | Position.Start
        | Position.End -> true
        | _ -> false

      div [
        Attr.Style "border" "1px solid var(--palette-divider)"
        BorderRadius.All.small
        Attr.Style "min-height" "160px"
        Attr.Style "display" "flex"
        Attr.Style "flex-direction" (if isVertical then "row" else "column")
        Attr.Style "width" "100%"
      ] [
        Tabs.create (
          View.Const [
            div [ Typography.body1 ] [ text (sprintf "%s content" label) ]
            |> TabDef.createText "Item One"
            div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Item Two"
            div [ Typography.body1 ] [ text "Content Three" ]
            |> TabDef.createText "Item Three"
          ],
          position = pos,
          attrs = [ Color.primary ]
        )
      ]

    let content =
      Grid.create (
        [
          GridItem.create (makeTabs Position.Top "Top", xs = Grid.Width.create 12, md = Grid.Width.create 6)
          GridItem.create (
            makeTabs Position.Bottom "Bottom",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.create (makeTabs Position.Left "Left", xs = Grid.Width.create 12, md = Grid.Width.create 6)
          GridItem.create (
            makeTabs Position.Right "Right",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 4
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Top content" ] |> TabDef.createText "Item One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Item Two"
        div [ Typography.body1 ] [ text "Content Three" ] |> TabDef.createText "Item Three"
    ],
    position = Tabs.Position.Top,
    attrs = [ Color.primary ]
)

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Left content" ] |> TabDef.createText "Item One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Item Two"
        div [ Typography.body1 ] [ text "Content Three" ] |> TabDef.createText "Item Three"
    ],
    position = Tabs.Position.Left,
    attrs = [ Color.primary ]
)
"""

    Helpers.codeSampleSection "Tab Positions" description content code

  let private centeredExample () =
    let description =
      Helpers.bodyText
        "When tabs fit within the container, pass Alignment.Center to center the tab strip instead of aligning to the start."

    let content =
      Tabs.create (
        View.Const [
          div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "One"
          div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Two"
        ],
        attrs = [ Alignment.center; Color.secondary ]
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.create(
    View.Const [
        div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Two"
    ],
    attrs = [ Alignment.center; Color.secondary ]
)
"""

    Helpers.codeSampleSection "Centered Tabs" description content code

  let private scrollableExample () =
    let description =
      Helpers.bodyText
        "When tabs overflow the container, scroll buttons appear automatically. Constrain the container width to trigger this behavior."

    let content =
      div [ Attr.Style "max-width" "500px" ] [
        Tabs.create (
          View.Const [
            for i in 1..12 do
              div [ Typography.body1 ] [ text (sprintf "Content %d" i) ]
              |> TabDef.createText (sprintf "Tab %d" i)
          ],
          attrs = [ Color.primary ]
        )
      ]

    let code =
      """open Weave
open Weave.Tabs

// Constrain the container width so overflow triggers scroll buttons
div [ Attr.Style "max-width" "500px" ] [
    Tabs.create(
        View.Const [
            for i in 1..12 do
                div [ Typography.body1 ] [ text (sprintf "Content %d" i) ] |> TabDef.createText (sprintf "Tab %d" i)
        ],
        attrs = [ Color.primary ]
    )
]
"""

    Helpers.codeSampleSection "Scrollable Tabs" description content code

  let private customScrollIconsExample () =
    let description =
      Helpers.bodyText
        "Replace the default Unicode arrow buttons with custom icons via scrollBackIcon and scrollForwardIcon."

    let content =
      div [ Attr.Style "max-width" "500px" ] [
        Tabs.create (
          View.Const [
            for i in 1..12 do
              div [ Typography.body1 ] [ text (sprintf "Content %d" i) ]
              |> TabDef.createText (sprintf "Tab %d" i)
          ],
          scrollBackIcon = Icon.create (Icon.UiActions UiActions.ChevronLeft),
          scrollForwardIcon = Icon.create (Icon.UiActions UiActions.ChevronRight),
          attrs = [ Color.secondary ]
        )
      ]

    let code =
      """open Weave
open Weave.Tabs
open Weave.Icons
open Weave.Icons.MaterialSymbols

div [ Attr.Style "max-width" "500px" ] [
    Tabs.create(
        View.Const [
            for i in 1..12 do
                div [ Typography.body1 ] [ text (sprintf "Content %d" i) ] |> TabDef.createText (sprintf "Tab %d" i)
        ],
        scrollBackIcon = Icon.create(Icon.UiActions UiActions.ChevronLeft),
        scrollForwardIcon = Icon.create(Icon.UiActions UiActions.ChevronRight),
        attrs = [ Color.secondary ]
    )
]
"""

    Helpers.codeSampleSection "Custom Scroll Icons" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText "Density controls tab padding. Pass the density class in attrs to set it per-instance."

    let tabs () =
      View.Const [
        div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
      ]

    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          Tabs.create (
            tabs (),
            attrs = [
              Color.primary
              BorderRadius.All.small
              Attr.Style "border" "1px solid var(--palette-divider)"
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
open Weave.Tabs


let tabs =
    View.Const [
        div [ Typography.body1 ] [ text "Content One" ] |> TabDef.createText "Tab One"
        div [ Typography.body1 ] [ text "Content Two" ] |> TabDef.createText "Tab Two"
        div [ Typography.body1 ] [ text "Content Three" ] |> TabDef.createText "Tab Three"
    ]

Tabs.create(
    tabs,
    attrs = [
        Density.compact
        Color.primary
    ]
)

Tabs.create(
    tabs,
    attrs = [
        Density.standard
        Color.primary
    ]
)

Tabs.create(
    tabs,
    attrs = [
        Density.spacious
        Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] [
        Helpers.pageTitle "Tabs"

        div [ Typography.body1; Margin.Bottom.small ] [
          text "Tabs organize content across different screens and views."
        ]

        basicExample ()
        Helpers.divider ()
        colorExample ()
        Helpers.divider ()
        variantExample ()
        Helpers.divider ()
        iconExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        positionExample ()
        Helpers.divider ()
        centeredExample ()
        Helpers.divider ()
        scrollableExample ()
        Helpers.divider ()
        customScrollIconsExample ()
        Helpers.divider ()
        densityExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
