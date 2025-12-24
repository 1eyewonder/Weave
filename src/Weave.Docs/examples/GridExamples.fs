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

  let private section title description content =
    div [ Margin.toClasses Margin.Bottom.extraLarge |> cls ] [
      H3.Create(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
      Body1.Create(View.Const description, attrs = [ Margin.toClasses Margin.Bottom.medium |> cls ])
      div [
        Padding.toClasses Padding.All.medium |> cls
        SurfaceColor.toAttr SurfaceColor.Paper
        BorderRadius.toClass BorderRadius.All.small |> cl
      ] [ content ]
    ]

  let private demoBox (label: View<string>) color =
    Container.Create(
      content = Typography.Button.Create(label),
      attrs = [
        BrandColor.toAttr color
        BorderRadius.toClass BorderRadius.All.small |> cl
        Attr.Style "display" "flex"
        Attr.Style "justify-content" "center"
        Attr.Style "align-items" "center"
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
    |> section "Basic Grid" "A simple grid with items stacked vertically on all screen sizes"

  let private equalColumnsExample () =
    Grid.Create(
      [
        yield!
          [ 0..3 ]
          |> List.map (fun _ ->
            GridItem.Create(demoBox (View.Const "1/4") BrandColor.Primary, xs = Grid.Width.create 3))
      ]
    )
    |> section "Equal Columns" "Grid items with equal widths (3/12 = 25% each)"

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
      ]
    )
    |> section
      "Responsive Grid"
      "Grid items that change width at different breakpoints. Resize your browser to see the effect."

  let private spacingExample () =
    div [] [
      let item color =
        GridItem.Create(demoBox (View.Const "Item") color, xs = Grid.Width.create 4)

      Body2.Create("Spacing: 0", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])

      Grid.Create(
        [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
        spacing = Grid.GutterSpacing.create 0
      )

      Body2.Create("Spacing: 10", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])

      Grid.Create(
        [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
        spacing = Grid.GutterSpacing.create 10
      )

      Body2.Create("Spacing: 20", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])

      Grid.Create(
        [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
        spacing = Grid.GutterSpacing.create 20
      )
    ]
    |> section "Spacing" "Control the gap between grid items with different spacing values (1-20)"

  let private justifyContentExample () =
    div [] [
      Body2.Create("Justify: Start", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.FlexStart
      )

      Body2.Create(
        "Justify: Center",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.small |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.Center
      )

      Body2.Create(
        "Justify: End",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.small |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.FlexEnd
      )

      Body2.Create(
        "Justify: Space Between",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.small |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.SpaceBetween
      )

      Body2.Create(
        "Justify: Space Around",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.small |> cls
        ]
      )
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
        ],
        justify = JustifyContent.SpaceAround
      )

      Body2.Create(
        "Justify: Space Evenly",
        attrs = [
          Margin.toClasses Margin.Top.medium |> cls
          Margin.toClasses Margin.Bottom.small |> cls
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
    |> section "Justify Content" "Control how grid items are aligned along the main axis"

  let private flexBreakExample () =
    Grid.Create(
      [
        GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 4)
        GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 4)

        FlexBreak.Create()

        GridItem.Create(demoBox (View.Const "Item 3") BrandColor.Tertiary, xs = Grid.Width.create 4)
        GridItem.Create(demoBox (View.Const "Item 4") BrandColor.Info, xs = Grid.Width.create 4)
      ]
    )
    |> section "Flex Break" "Use FlexBreak to force items onto a new row without changing their width"

  let private nestedGridExample () =
    Grid.Create(
      [
        GridItem.Create(
          Container.Create(
            [
              Body2.Create(
                "Outer Grid - Left Column",
                attrs = [
                  Margin.toClasses Margin.Bottom.small |> cls
                  Attr.Style "color" "var(--palette-text-primary)"
                ]
              )
              Grid.Create(
                [
                  GridItem.Create(
                    demoBox (View.Const "Nested 1") BrandColor.Success,
                    xs = Grid.Width.create 6
                  )
                  GridItem.Create(
                    demoBox (View.Const "Nested 2") BrandColor.Warning,
                    xs = Grid.Width.create 6
                  )
                  GridItem.Create(demoBox (View.Const "Nested 3") BrandColor.Error, xs = Grid.Width.create 12)
                ]
              )
            ]
            |> Doc.Concat
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6
        )

        GridItem.Create(
          div [] [
            Body2.Create(
              "Outer Grid - Right Column",
              attrs = [
                Margin.toClasses Margin.Bottom.small |> cls
                Attr.Style "color" "var(--palette-text-primary)"
              ]
            )
            demoBox (View.Const "Full Height Item") BrandColor.Info
          ],
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6
        )
      ]
    )
    |> section "Nested Grids" "Grids can be nested inside grid items for complex layouts"

  let private cardLayoutExample () =
    let card title description =
      div [
        Padding.toClasses Padding.All.medium |> cls
        SurfaceColor.toAttr SurfaceColor.Paper
        Attr.Style "border" "1px solid var(--palette-divider)"
        BorderRadius.toClass BorderRadius.All.medium |> cl
        Attr.Style "height" "100%"
      ] [
        H5.Create(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
        Body2.Create(View.Const description)
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
    |> section "Card Layout" "A practical example using grid for a responsive card layout"

  let private formLayoutExample () =
    Grid.Create(
      [
        GridItem.Create(
          div [] [
            Body2.Create("First Name", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            div [
              Padding.toClasses Padding.All.small |> cls
              SurfaceColor.toAttr SurfaceColor.Paper
              BorderRadius.toClass BorderRadius.All.small |> cl
            ] [ text "Input field" ]
          ],
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6
        )

        GridItem.Create(
          div [] [
            Body2.Create("Last Name", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            div [
              Padding.toClasses Padding.All.small |> cls
              SurfaceColor.toAttr SurfaceColor.Paper
              BorderRadius.toClass BorderRadius.All.small |> cl
            ] [ text "Input field" ]
          ],
          xs = Grid.Width.create 12,
          md = Grid.Width.create 6
        )

        GridItem.Create(
          div [] [
            Body2.Create("Email Address", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            div [
              Padding.toClasses Padding.All.small |> cls
              SurfaceColor.toAttr SurfaceColor.Paper
              BorderRadius.toClass BorderRadius.All.small |> cl
            ] [ text "Input field" ]
          ],
          xs = Grid.Width.create 12
        )

        GridItem.Create(
          div [] [
            Body2.Create("Message", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            div [
              Padding.toClasses Padding.All.small |> cls
              SurfaceColor.toAttr SurfaceColor.Paper
              Attr.Style "min-height" "100px"
              BorderRadius.toClass BorderRadius.All.small |> cl
            ] [ text "Textarea field" ]
          ],
          xs = Grid.Width.create 12
        )

        GridItem.Create(
          Button.Create(
            text "Submit",
            onClick = (fun () -> ()),
            attrs = [
              Button.Variant.Filled |> Button.Variant.toClass |> cl
              Button.Color.toClass BrandColor.Primary |> cl
            ]
          ),
          xs = Grid.Width.create 12
        )
      ]
    )
    |> section "Form Layout" "Using grid to create a responsive form layout"

  let render () =
    Container.Create(
      div [] [
        H1.Create("Grid Component", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
        Body1.Create(
          "The Grid component uses a 12-column system to create flexible, responsive layouts. Items can span different numbers of columns at different breakpoints.",
          attrs = [ Margin.toClasses Margin.Bottom.extraLarge |> cls ]
        )

        basicGridExample ()
        equalColumnsExample ()
        responsiveGridExample ()
        spacingExample ()
        justifyContentExample ()
        flexBreakExample ()
        nestedGridExample ()
        cardLayoutExample ()
        formLayoutExample ()
      ]
    )
