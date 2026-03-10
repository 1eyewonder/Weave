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
    let description =
      Helpers.bodyText "The default configuration: a horizontal text tab strip with the first tab active."

    let content =
      Tabs.Create(
        View.Const [
          Body1.Div("Content One") |> TabDef.createText "Tab One"
          Body1.Div("Content Two") |> TabDef.createText "Tab Two"
          Body1.Div("Content Three") |> TabDef.createText "Tab Three"
        ],
        attrs = [ cl (Color.toClass BrandColor.Primary) ]
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.Create(
    View.Const [
        Body1.Div("Content One") |> TabDef.createText "Tab One"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
        Body1.Div("Content Three") |> TabDef.createText "Tab Three"
    ],
    attrs = [ cl (Color.toClass BrandColor.Primary) ]
)
"""

    Helpers.codeSampleSection "Basic Tabs" description content code

  let private colorExample () =
    let description =
      Helpers.bodyText
        "Pass a color class via attrs to tint the active indicator and tab label. All brand colors are supported."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Tabs.Create(
              View.Const [
                Body1.Div("Primary color") |> TabDef.createText "Primary"
                Body1.Div("Content Two") |> TabDef.createText "Tab Two"
              ],
              attrs = [ cl (Color.toClass BrandColor.Primary) ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Tabs.Create(
              View.Const [
                Body1.Div("Secondary color") |> TabDef.createText "Secondary"
                Body1.Div("Content Two") |> TabDef.createText "Tab Two"
              ],
              attrs = [ cl (Color.toClass BrandColor.Secondary) ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Tabs.Create(
              View.Const [
                Body1.Div("Tertiary color") |> TabDef.createText "Tertiary"
                Body1.Div("Content Two") |> TabDef.createText "Tab Two"
              ],
              attrs = [ cl (Color.toClass BrandColor.Tertiary) ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Tabs.Create(
              View.Const [
                Body1.Div("Success color") |> TabDef.createText "Success"
                Body1.Div("Content Two") |> TabDef.createText "Tab Two"
              ],
              attrs = [ cl (Color.toClass BrandColor.Success) ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Tabs.Create(
              View.Const [
                Body1.Div("Warning color") |> TabDef.createText "Warning"
                Body1.Div("Content Two") |> TabDef.createText "Tab Two"
              ],
              attrs = [ cl (Color.toClass BrandColor.Warning) ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Tabs.Create(
              View.Const [
                Body1.Div("Error color") |> TabDef.createText "Error"
                Body1.Div("Content Two") |> TabDef.createText "Tab Two"
              ],
              attrs = [ cl (Color.toClass BrandColor.Error) ]
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

Tabs.Create(
    View.Const [
        Body1.Div("Primary color") |> TabDef.createText "Primary"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
    ],
    attrs = [ cl (Color.toClass BrandColor.Primary) ]
)

Tabs.Create(
    View.Const [
        Body1.Div("Secondary color") |> TabDef.createText "Secondary"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
    ],
    attrs = [ cl (Color.toClass BrandColor.Secondary) ]
)

Tabs.Create(
    View.Const [
        Body1.Div("Tertiary color") |> TabDef.createText "Tertiary"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
    ],
    attrs = [ cl (Color.toClass BrandColor.Tertiary) ]
)

Tabs.Create(
    View.Const [
        Body1.Div("Success color") |> TabDef.createText "Success"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
    ],
    attrs = [ cl (Color.toClass BrandColor.Success) ]
)

Tabs.Create(
    View.Const [
        Body1.Div("Warning color") |> TabDef.createText "Warning"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
    ],
    attrs = [ cl (Color.toClass BrandColor.Warning) ]
)

Tabs.Create(
    View.Const [
        Body1.Div("Error color") |> TabDef.createText "Error"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
    ],
    attrs = [ cl (Color.toClass BrandColor.Error) ]
)
"""

    Helpers.codeSampleSection "Color Variants" description content code

  let private variantExample () =
    let description =
      Helpers.bodyText
        "Tabs support three visual styles: Text (default), Outlined, and Filled. Pass a variant class via attrs. For the Filled variant, use HeaderBackground.toAttr to optionally set a background tray on the header strip."

    let tabs () =
      [
        Body1.Div("Content One") |> TabDef.createText "Tab One"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
        Body1.Div("Content Three") |> TabDef.createText "Tab Three"
      ]
      |> View.Const

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [] [
              H4.Div("Text Tab", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Tabs.Create(
                tabs (),
                attrs = [ cls [ Variant.toClass Variant.Text; Color.toClass BrandColor.Primary ] ]
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            div [] [
              H4.Div("Outlined Tab", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Tabs.Create(
                tabs (),
                attrs = [ cls [ Variant.toClass Variant.Outlined; Color.toClass BrandColor.Secondary ] ]
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            div [] [
              H4.Div("Filled Tab", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Tabs.Create(
                tabs (),
                attrs = [ cls [ Variant.toClass Variant.Filled; Color.toClass BrandColor.Tertiary ] ]
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
        ],
        justify = JustifyContent.SpaceAround
      )

    let code =
      """open Weave
open Weave.Tabs

let tabs =
    View.Const [
        Body1.Div("Content One") |> TabDef.createText "Tab One"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
        Body1.Div("Content Three") |> TabDef.createText "Tab Three"
    ]

Tabs.Create(tabs, attrs = [ cls [ Variant.toClass Variant.Text; Color.toClass BrandColor.Primary ] ])

Tabs.Create(tabs, attrs = [ cls [ Variant.toClass Variant.Outlined; Color.toClass BrandColor.Secondary ] ])

Tabs.Create(tabs, attrs = [ cls [ Variant.toClass Variant.Filled; Color.toClass BrandColor.Tertiary ] ])

// Filled with a tinted header tray
Tabs.Create(
    tabs,
    attrs = [
        cls [ Variant.toClass Variant.Filled; Color.toClass BrandColor.Tertiary ]
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
      Tabs.Create(
        View.Const [
          Body1.Div("Home content")
          |> TabDef.createWithStartIcon "Home" (Icon.Create(Icon.UiActions UiActions.Home))
          Body1.Div("Settings content")
          |> TabDef.createWithStartIcon "Settings" (Icon.Create(Icon.UiActions UiActions.Settings))
          Body1.Div("Information content")
          |> TabDef.createWithStartIcon "Info" (Icon.Create(Icon.Action Action.Info))
        ],
        attrs = [ cl (Color.toClass BrandColor.Secondary) ]
      )

    let code =
      """open Weave
open Weave.Tabs
open Weave.Icons
open Weave.Icons.MaterialSymbols

Tabs.Create(
    View.Const [
        Body1.Div("Home content")
        |> TabDef.createWithStartIcon "Home" (Icon.Create(Icon.UiActions UiActions.Home))
        Body1.Div("Settings content")
        |> TabDef.createWithStartIcon "Settings" (Icon.Create(Icon.UiActions UiActions.Settings))
        Body1.Div("Information content")
        |> TabDef.createWithStartIcon "Info" (Icon.Create(Icon.Action Action.Info))
    ],
    attrs = [ cl (Color.toClass BrandColor.Secondary) ]
)
"""

    Helpers.codeSampleSection "Tabs with Icons" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Set a tab as non-interactive using TabDef.withDisabled. Disabled tabs are visually muted and cannot be clicked."

    let content =
      Tabs.Create(
        View.Const [
          Body1.Div("Content One") |> TabDef.createText "Tab One"
          Body1.Div("Content Two") |> TabDef.createText "Tab Two"
          Body1.Div("Content Three") |> TabDef.createText "Tab Three"
          Body1.Div("You should not see this")
          |> TabDef.createText "Tab Disabled"
          |> TabDef.withDisabled (View.Const true)
        ],
        attrs = [ cl (Color.toClass BrandColor.Tertiary) ]
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.Create(
    View.Const [
        Body1.Div("Content One") |> TabDef.createText "Tab One"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
        Body1.Div("Content Three") |> TabDef.createText "Tab Three"
        Body1.Div("You should not see this")
        |> TabDef.createText "Tab Disabled"
        |> TabDef.withDisabled (View.Const true)
    ],
    attrs = [ cl (Color.toClass BrandColor.Tertiary) ]
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
        BorderRadius.toClass BorderRadius.All.small |> cl
        Attr.Style "min-height" "160px"
        Attr.Style "display" "flex"
        Attr.Style "flex-direction" (if isVertical then "row" else "column")
        Attr.Style "width" "100%"
      ] [
        Tabs.Create(
          View.Const [
            Body1.Div(sprintf "%s content" label) |> TabDef.createText "Item One"
            Body1.Div("Content Two") |> TabDef.createText "Item Two"
            Body1.Div("Content Three") |> TabDef.createText "Item Three"
          ],
          position = pos,
          attrs = [ cl (Color.toClass BrandColor.Primary) ]
        )
      ]

    let content =
      Grid.Create(
        [
          GridItem.Create(makeTabs Position.Top "Top", xs = Grid.Width.create 12, md = Grid.Width.create 6)
          GridItem.Create(
            makeTabs Position.Bottom "Bottom",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.Create(makeTabs Position.Left "Left", xs = Grid.Width.create 12, md = Grid.Width.create 6)
          GridItem.Create(
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

Tabs.Create(
    View.Const [
        Body1.Div("Top content") |> TabDef.createText "Item One"
        Body1.Div("Content Two") |> TabDef.createText "Item Two"
        Body1.Div("Content Three") |> TabDef.createText "Item Three"
    ],
    position = Tabs.Position.Top,
    attrs = [ cl (Color.toClass BrandColor.Primary) ]
)

Tabs.Create(
    View.Const [
        Body1.Div("Left content") |> TabDef.createText "Item One"
        Body1.Div("Content Two") |> TabDef.createText "Item Two"
        Body1.Div("Content Three") |> TabDef.createText "Item Three"
    ],
    position = Tabs.Position.Left,
    attrs = [ cl (Color.toClass BrandColor.Primary) ]
)
"""

    Helpers.codeSampleSection "Tab Positions" description content code

  let private centeredExample () =
    let description =
      Helpers.bodyText
        "When tabs fit within the container, pass Alignment.Center to center the tab strip instead of aligning to the start."

    let content =
      Tabs.Create(
        View.Const [
          Body1.Div("Content One") |> TabDef.createText "One"
          Body1.Div("Content Two") |> TabDef.createText "Two"
        ],
        attrs = [ Alignment.toAttr Alignment.Center; cl (Color.toClass BrandColor.Secondary) ]
      )

    let code =
      """open Weave
open Weave.Tabs

Tabs.Create(
    View.Const [
        Body1.Div("Content One") |> TabDef.createText "One"
        Body1.Div("Content Two") |> TabDef.createText "Two"
    ],
    attrs = [ Alignment.toAttr Alignment.Center; cl (Color.toClass BrandColor.Secondary) ]
)
"""

    Helpers.codeSampleSection "Centered Tabs" description content code

  let private scrollableExample () =
    let description =
      Helpers.bodyText
        "When tabs overflow the container, scroll buttons appear automatically. Constrain the container width to trigger this behavior."

    let content =
      div [ Attr.Style "max-width" "500px" ] [
        Tabs.Create(
          View.Const [
            for i in 1..12 do
              Body1.Div(sprintf "Content %d" i) |> TabDef.createText (sprintf "Tab %d" i)
          ],
          attrs = [ cl (Color.toClass BrandColor.Primary) ]
        )
      ]

    let code =
      """open Weave
open Weave.Tabs

// Constrain the container width so overflow triggers scroll buttons
div [ Attr.Style "max-width" "500px" ] [
    Tabs.Create(
        View.Const [
            for i in 1..12 do
                Body1.Div(sprintf "Content %d" i) |> TabDef.createText (sprintf "Tab %d" i)
        ],
        attrs = [ cl (Color.toClass BrandColor.Primary) ]
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
        Tabs.Create(
          View.Const [
            for i in 1..12 do
              Body1.Div(sprintf "Content %d" i) |> TabDef.createText (sprintf "Tab %d" i)
          ],
          scrollBackIcon = Icon.Create(Icon.UiActions UiActions.ChevronLeft),
          scrollForwardIcon = Icon.Create(Icon.UiActions UiActions.ChevronRight),
          attrs = [ cl (Color.toClass BrandColor.Secondary) ]
        )
      ]

    let code =
      """open Weave
open Weave.Tabs
open Weave.Icons
open Weave.Icons.MaterialSymbols

div [ Attr.Style "max-width" "500px" ] [
    Tabs.Create(
        View.Const [
            for i in 1..12 do
                Body1.Div(sprintf "Content %d" i) |> TabDef.createText (sprintf "Tab %d" i)
        ],
        scrollBackIcon = Icon.Create(Icon.UiActions UiActions.ChevronLeft),
        scrollForwardIcon = Icon.Create(Icon.UiActions UiActions.ChevronRight),
        attrs = [ cl (Color.toClass BrandColor.Secondary) ]
    )
]
"""

    Helpers.codeSampleSection "Custom Scroll Icons" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText "Density controls tab padding. Pass the density class in attrs to set it per-instance."

    let tabs () =
      View.Const [
        Body1.Div("Content One") |> TabDef.createText "Tab One"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
      ]

    let content =
      let col density =
        let label = sprintf "%A" density

        div [ Density.toClass density |> cl ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          Tabs.Create(
            tabs (),
            attrs = [
              cls [
                Color.toClass BrandColor.Primary
                BorderRadius.toClass BorderRadius.All.small
              ]
              Attr.Style "border" "1px solid var(--palette-divider)"
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
open Weave.Tabs
open Weave.CssHelpers

let tabs =
    View.Const [
        Body1.Div("Content One") |> TabDef.createText "Tab One"
        Body1.Div("Content Two") |> TabDef.createText "Tab Two"
        Body1.Div("Content Three") |> TabDef.createText "Tab Three"
    ]

Tabs.Create(
    tabs,
    attrs = [
        cl (Density.toClass Density.Compact) // see here
        cl (Color.toClass BrandColor.Primary)
    ]
)

Tabs.Create(
    tabs,
    attrs = [
        cl (Density.toClass Density.Standard) // see here
        cl (Color.toClass BrandColor.Primary)
    ]
)

Tabs.Create(
    tabs,
    attrs = [
        cl (Density.toClass Density.Spacious) // see here
        cl (Color.toClass BrandColor.Primary)
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

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
      maxWidth = Container.MaxWidth.Large
    )
