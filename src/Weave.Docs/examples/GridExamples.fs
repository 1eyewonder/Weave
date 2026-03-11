namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave

[<JavaScript>]
module GridExamples =

  let private demoBox (label: View<string>) color =
    Container.Create(
      content = Typography.Button.Div(label),
      attrs = [
        BrandColor.toBackgroundColor color

        cls [
          BorderRadius.toClass BorderRadius.All.small
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.Center
          AlignItems.toClass AlignItems.Center
        ]
      ]
    )

  let private basicGridExample () =
    let description =
      Helpers.bodyText "A simple grid with items stacked vertically on all screen sizes."

    let content =
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 12)
          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 12)
          GridItem.Create(demoBox (View.Const "Item 3") BrandColor.Tertiary, xs = Grid.Width.create 12)
        ]
      )

    let code =
      """open Weave

Grid.Create(
    [
        GridItem.Create(
            myContent,
            xs = Grid.Width.create 12 // see here
        )
        GridItem.Create(
            myContent,
            xs = Grid.Width.create 12
        )
        GridItem.Create(
            myContent,
            xs = Grid.Width.create 12
        )
    ]
)"""

    Helpers.codeSampleSection "Basic Grid" description content code

  let private equalColumnsExample () =
    let description = Helpers.bodyText "Grid items with equal widths (3/12 = 25% each)."

    let content =
      Grid.Create(
        [
          yield!
            [ 0..3 ]
            |> List.map (fun _ ->
              GridItem.Create(demoBox (View.Const "1/4") BrandColor.Primary, xs = Grid.Width.create 3))
        ]
      )

    let code =
      """open Weave

Grid.Create(
    [
        GridItem.Create(myContent, xs = Grid.Width.create 3) // see here
        GridItem.Create(myContent, xs = Grid.Width.create 3)
        GridItem.Create(myContent, xs = Grid.Width.create 3)
        GridItem.Create(myContent, xs = Grid.Width.create 3)
    ]
)"""

    Helpers.codeSampleSection "Equal Columns" description content code

  let private responsiveGridExample () =
    let description =
      Helpers.bodyText
        "Grid items that change width at different breakpoints. Resize your browser to see the effect."

    let content =
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

    let code =
      """open Weave

GridItem.Create(
    myContent,
    sm = Grid.Width.create 12, // full width on small screens
    md = Grid.Width.create 6,  // half width on medium
    lg = Grid.Width.create 4,  // third width on large
    xl = Grid.Width.create 3   // quarter width on extra-large
)"""

    Helpers.codeSampleSection "Responsive Grid" description content code

  let private spacingExample () =
    let description =
      Helpers.bodyText "Control the gap between grid items with different spacing values (1-20)."

    let content =
      div [] [
        let item color =
          GridItem.Create(demoBox (View.Const "Item") color, xs = Grid.Width.create 4)

        Body1.Div("Spacing: 0", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

        Grid.Create(
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          spacing = Grid.GutterSpacing.create 0
        )

        Body1.Div("Spacing: 10", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

        Grid.Create(
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          spacing = Grid.GutterSpacing.create 10
        )

        Body1.Div("Spacing: 20", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

        Grid.Create(
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          spacing = Grid.GutterSpacing.create 20
        )
      ]

    let code =
      """open Weave

Grid.Create(
    [
        GridItem.Create(item1, xs = Grid.Width.create 4)
        GridItem.Create(item2, xs = Grid.Width.create 4)
        GridItem.Create(item3, xs = Grid.Width.create 4)
    ],
    spacing = Grid.GutterSpacing.create 10 // see here
)"""

    Helpers.codeSampleSection "Spacing" description content code

  let private justifyContentExample () =
    let description =
      Helpers.bodyText "Control how grid items are aligned along the main axis."

    let content =
      div [] [
        Body1.Div("Justify: Start", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Grid.Create(
          [
            GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
            GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
          ],
          justify = JustifyContent.FlexStart
        )

        Body1.Div(
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

        Body1.Div(
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

        Body1.Div(
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

        Body1.Div(
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

        Body1.Div(
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

    let code =
      """open Weave

Grid.Create(
    [
        GridItem.Create(item1, xs = Grid.Width.create 3)
        GridItem.Create(item2, xs = Grid.Width.create 3)
    ],
    justify = JustifyContent.Center // see here
)

Grid.Create(
    [
        GridItem.Create(item1, xs = Grid.Width.create 3)
        GridItem.Create(item2, xs = Grid.Width.create 3)
    ],
    justify = JustifyContent.SpaceBetween // see here
)"""

    Helpers.codeSampleSection "Justify Content" description content code

  let private flexBreakExample () =
    let description =
      Helpers.bodyText "Use FlexBreak to force items onto a new row without changing their width."

    let content =
      Grid.Create(
        [
          GridItem.Create(demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 4)

          FlexBreak.Create()

          GridItem.Create(demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 4)
          GridItem.Create(demoBox (View.Const "Item 3") BrandColor.Tertiary, xs = Grid.Width.create 4)
          GridItem.Create(demoBox (View.Const "Item 4") BrandColor.Primary, xs = Grid.Width.create 4)
        ]
      )

    let code =
      """open Weave

Grid.Create(
    [
        GridItem.Create(item1, xs = Grid.Width.create 4)

        FlexBreak.Create() // see here

        GridItem.Create(item2, xs = Grid.Width.create 4)
        GridItem.Create(item3, xs = Grid.Width.create 4)
        GridItem.Create(item4, xs = Grid.Width.create 4)
    ]
)"""

    Helpers.codeSampleSection "Flex Break" description content code

  let private nestedGridExample () =
    let description =
      Helpers.bodyText "Grids can be nested inside grid items for complex layouts."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Container.Create(
              [
                Body1.Div(
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
                Body1.Div(
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

    let code =
      """open Weave

Grid.Create(
    [
        GridItem.Create(
            Grid.Create( // see here
                [
                    GridItem.Create(nested1, xs = Grid.Width.create 6)
                    GridItem.Create(nested2, xs = Grid.Width.create 6)
                    GridItem.Create(nested3, xs = Grid.Width.create 12)
                ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
        )
        GridItem.Create(
            sidebarContent,
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
        )
    ]
)"""

    Helpers.codeSampleSection "Nested Grids" description content code

  let private cardLayoutExample () =
    let description =
      Helpers.bodyText "A practical example using grid for a responsive card layout."

    let card title description =
      div [
        Padding.toClasses Padding.All.medium |> cls
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
        Attr.Style "border" "1px solid var(--palette-divider)"
        BorderRadius.toClass BorderRadius.All.medium |> cl
        Attr.Style "height" "100%"
      ] [
        H5.Div(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(View.Const description)
      ]

    let content =
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

    let code =
      """open Weave

open WebSharper.UI
open WebSharper.UI.Html

let card title description =
    div [
        Padding.toClasses Padding.All.medium |> cls
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
        BorderRadius.toClass BorderRadius.All.medium |> cl
    ] [
        H5.Div(View.Const title)
        Body1.Div(View.Const description)
    ]

Grid.Create(
    [
        GridItem.Create(
            card "Feature 1" "First feature description.",
            xs = Grid.Width.create 12, // see here
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
        )
        GridItem.Create(
            card "Feature 2" "Second feature description.",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
        )
    ]
)"""

    Helpers.codeSampleSection "Card Layout" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Grid"
        Body1.Div(
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
      ],
      maxWidth = Container.MaxWidth.Large
    )
