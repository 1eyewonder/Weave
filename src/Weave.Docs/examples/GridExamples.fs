namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave
open Weave.CssHelpers

[<JavaScript>]
module GridExamples =

  let private demoBox (label: View<string>) color =
    Container.Create(
      content = Typography.Button.Create(label),
      attrs = [
        BrandColor.toAttr color

        cls [
          BorderRadius.toClass BorderRadius.All.small
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.Center
          AlignItems.toClass AlignItems.Center
        ]
      ]
    )

  let private basicGridExample () =
    Grid.Create(
      [
        GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 12)
        GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 12)
        GridItem.Create(demoBox (View.Const "Item 3") BrandColor.Tertiary, xs = Grid.Width.create 12)
      ]
    )
    |> Helpers.section "Basic Grid" (text "A simple grid with items stacked vertically on all screen sizes")

  let private equalColumnsExample () =
    Grid.Create(
      [
        yield!
          [ 0..3 ]
          |> List.map (fun _ ->
            GridItem.Create(demoBox (View.Const "1/4") BrandColor.Primary, xs = Grid.Width.create 3))
      ]
    )
    |> Helpers.section "Equal Columns" (text "Grid items with equal widths (3/12 = 25% each)")

  let private responsiveGridExample () =
    Grid.Create(
      [
        let item text color =
          GridItem.Create(
            demoBox text color,
            sm = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4,
            xl = Grid.Width.create 3
          )

        item Breakpoint.browserAsText BrandColor.Primary
        item Breakpoint.browserAsText BrandColor.Secondary
        item Breakpoint.browserAsText BrandColor.Tertiary
        item Breakpoint.browserAsText BrandColor.Primary
        item Breakpoint.browserAsText BrandColor.Secondary
        item Breakpoint.browserAsText BrandColor.Tertiary
        item Breakpoint.browserAsText BrandColor.Primary
        item Breakpoint.browserAsText BrandColor.Secondary
        item Breakpoint.browserAsText BrandColor.Tertiary
        item Breakpoint.browserAsText BrandColor.Primary
        item Breakpoint.browserAsText BrandColor.Secondary
        item Breakpoint.browserAsText BrandColor.Tertiary
      ]
    )
    |> Helpers.section
      "Responsive Grid"
      (text "Grid items that change width at different breakpoints. Resize your browser to see the effect.")

  let private spacingExample () =
    div [] [
      let item color =
        GridItem.Create(demoBox (View.Const "Item") color, xs = Grid.Width.create 4)

      Body1.Create("Spacing: 0", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

      Grid.Create(
        [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
        spacing = Grid.GutterSpacing.create 0
      )

      Body1.Create("Spacing: 10", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

      Grid.Create(
        [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
        spacing = Grid.GutterSpacing.create 10
      )

      Body1.Create("Spacing: 20", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

      Grid.Create(
        [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
        spacing = Grid.GutterSpacing.create 20
      )
    ]
    |> Helpers.section
      "Spacing"
      (text "Control the gap between grid items with different spacing values (1-20)")

  let private justifyContentExample () =
    div [] [
      Body1.Create("Justify: Start", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.FlexStart
      )

      Body1.Create(
        "Justify: Center",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.extraSmall |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.Center
      )

      Body1.Create(
        "Justify: End",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.extraSmall |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.FlexEnd
      )

      Body1.Create(
        "Justify: Space Between",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.extraSmall |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.SpaceBetween
      )

      Body1.Create(
        "Justify: Space Around",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.extraSmall |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.SpaceAround
      )

      Body1.Create(
        "Justify: Space Evenly",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.extraSmall |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.SpaceEvenly
      )
    ]
    |> Helpers.section "Justify Content" (text "Control how grid items are aligned along the main axis")

  let private flexBreakExample () =
    Grid.Create(
      [
        GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 4)

        FlexBreak.Create()

        GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 4)
        GridItem.Create(demoBox (View.Const "Item 3") BrandColor.Tertiary, xs = Grid.Width.create 4)
        GridItem.Create(demoBox (View.Const "Item 4") BrandColor.Primary, xs = Grid.Width.create 4)
      ]
    )
    |> Helpers.section
      "Flex Break"
      (text "Use FlexBreak to force items onto a new row without changing their width")

  let private nestedGridExample () =
    Grid.Create(
      [
        GridItem.Create(
          Container.Create(
            [
              Body1.Create(
                "Outer Grid - Left Column",
                attrs = [
                  Margin.toClasses Margin.Bottom.extraSmall |> cls
                  Attr.Style "color" "var(--palette-text-primary)"
                ]
              )
              Grid.Create(
                [
                  GridItem.Create(
                    demoBox (View.Const "Nested 1") BrandColor.Primary,
                    xs = Grid.Width.create 6
                  )
                  GridItem.Create(
                    demoBox (View.Const "Nested 2") BrandColor.Secondary,
                    xs = Grid.Width.create 6
                  )
                  GridItem.Create(
                    demoBox (View.Const "Nested 3") BrandColor.Tertiary,
                    xs = Grid.Width.create 12
                  )
                ]
              )
            ]
            |> Doc.Concat
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6
        )

        GridItem.Create(
          Container.Create(
            [
              Body1.Create(
                "Outer Grid - Right Column",
                attrs = [
                  Margin.toClasses Margin.Bottom.extraSmall |> cls
                  Attr.Style "color" "var(--palette-text-primary)"
                ]
              )
              demoBox (View.Const "Full Height Item") BrandColor.Primary
            ]
            |> Doc.Concat
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6
        )
      ]
    )
    |> Helpers.section "Nested Grids" (text "Grids can be nested inside grid items for complex layouts")

  let private cardLayoutExample () =
    let card title description =
      div [
        Padding.toClasses Padding.All.medium |> cls
        SurfaceColor.toAttr SurfaceColor.Paper
        Attr.Style "border" "1px solid var(--palette-divider)"
        BorderRadius.toClass BorderRadius.All.medium |> cl
        Attr.Style "height" "100%"
      ] [
        H5.Create(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(View.Const description)
      ]

    Grid.Create(
      [
        GridItem.Create(
          card "Feature 1" "Description of the first feature with some example text.",
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6,
          lg = Grid.Width.create 4
        )
        GridItem.Create(
          card "Feature 2" "Description of the second feature with more information.",
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6,
          lg = Grid.Width.create 4
        )
        GridItem.Create(
          card "Feature 3" "Description of the third feature explaining functionality.",
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6,
          lg = Grid.Width.create 4
        )
        GridItem.Create(
          card "Feature 4" "Description of the fourth feature with details.",
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6,
          lg = Grid.Width.create 4
        )
        GridItem.Create(
          card "Feature 5" "Description of the fifth feature and its benefits.",
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6,
          lg = Grid.Width.create 4
        )
        GridItem.Create(
          card "Feature 6" "Description of the sixth feature with advantages.",
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6,
          lg = Grid.Width.create 4
        )
      ]
    )
    |> Helpers.section "Card Layout" (text "A practical example using grid for a responsive card layout")

  let render () =
    Container.Create(
      div [] [
        H1.Create("Grid Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(
          "The Grid component uses a 12-column system to create flexible, responsive layouts. Items can span different numbers of columns at different breakpoints.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        basicGridExample ()
        Helpers.divider ()
        equalColumnsExample ()
        Helpers.divider ()
        responsiveGridExample ()
        Helpers.divider ()
        spacingExample ()
        Helpers.divider ()
        justifyContentExample ()
        Helpers.divider ()
        flexBreakExample ()
        Helpers.divider ()
        nestedGridExample ()
        Helpers.divider ()
        cardLayoutExample ()
      ]
    )
