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
    Container.create (
      content = Typography.ButtonText.div (label),
      attrs = [
        BrandColor.toBackgroundColor color
        BorderRadius.All.small
        Flex.Flex.allSizes
        JustifyContent.center
        AlignItems.center
      ]
    )

  let private basicGridExample () =
    let description =
      Helpers.bodyText "A simple grid with items stacked vertically on all screen sizes."

    let content =
      Grid.create (
        [
          GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 12)
          GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 12)
          GridItem.create (demoBox (View.Const "Item 3") BrandColor.Tertiary, xs = Grid.Width.create 12)
        ]
      )

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(
            myContent,
            xs = Grid.Width.create 12 // see here
        )
        GridItem.create(
            myContent,
            xs = Grid.Width.create 12
        )
        GridItem.create(
            myContent,
            xs = Grid.Width.create 12
        )
    ]
)"""

    Helpers.codeSampleSection "Basic Grid" description content code

  let private equalColumnsExample () =
    let description = Helpers.bodyText "Grid items with equal widths (3/12 = 25% each)."

    let content =
      Grid.create (
        [
          yield!
            [ 0..3 ]
            |> List.map (fun _ ->
              GridItem.create (demoBox (View.Const "1/4") BrandColor.Primary, xs = Grid.Width.create 3))
        ]
      )

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(myContent, xs = Grid.Width.create 3) // see here
        GridItem.create(myContent, xs = Grid.Width.create 3)
        GridItem.create(myContent, xs = Grid.Width.create 3)
        GridItem.create(myContent, xs = Grid.Width.create 3)
    ]
)"""

    Helpers.codeSampleSection "Equal Columns" description content code

  let private responsiveGridExample () =
    let description =
      Helpers.bodyText
        "Grid items that change width at different breakpoints. Resize your browser to see the effect."

    let content =
      Grid.create (
        [
          let item text color =
            GridItem.create (
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

GridItem.create(
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
          GridItem.create (demoBox (View.Const "Item") color, xs = Grid.Width.create 4)

        Body1.div ("Spacing: 0", attrs = [ Margin.Bottom.extraSmall ])

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          spacing = Grid.GutterSpacing.create 0
        )

        Body1.div ("Spacing: 10", attrs = [ Margin.Bottom.extraSmall ])

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          spacing = Grid.GutterSpacing.create 10
        )

        Body1.div ("Spacing: 20", attrs = [ Margin.Bottom.extraSmall ])

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          spacing = Grid.GutterSpacing.create 20
        )
      ]

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(item1, xs = Grid.Width.create 4)
        GridItem.create(item2, xs = Grid.Width.create 4)
        GridItem.create(item3, xs = Grid.Width.create 4)
    ],
    spacing = Grid.GutterSpacing.create 10 // see here
)"""

    Helpers.codeSampleSection "Spacing" description content code

  let private justifyContentExample () =
    let description =
      Helpers.bodyText "Control how grid items are aligned along the main axis."

    let content =
      div [] [
        Body1.div ("Justify: Start", attrs = [ Margin.Bottom.extraSmall ])
        Grid.create (
          [
            GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
            GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
          ],
          justify = JustifyContent.flexStart
        )

        Body1.div ("Justify: Center", attrs = [ Margin.Top.medium; Margin.Bottom.extraSmall ])
        Grid.create (
          [
            GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
            GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
          ],
          justify = JustifyContent.center
        )

        Body1.div ("Justify: End", attrs = [ Margin.Top.medium; Margin.Bottom.extraSmall ])
        Grid.create (
          [
            GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
            GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
          ],
          justify = JustifyContent.flexEnd
        )

        Body1.div ("Justify: Space Between", attrs = [ Margin.Top.medium; Margin.Bottom.extraSmall ])
        Grid.create (
          [
            GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
            GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
          ],
          justify = JustifyContent.spaceBetween
        )

        Body1.div ("Justify: Space Around", attrs = [ Margin.Top.medium; Margin.Bottom.extraSmall ])
        Grid.create (
          [
            GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
            GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
          ],
          justify = JustifyContent.spaceAround
        )

        Body1.div ("Justify: Space Evenly", attrs = [ Margin.Top.medium; Margin.Bottom.extraSmall ])
        Grid.create (
          [
            GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 3)
            GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 3)
          ],
          justify = JustifyContent.spaceEvenly
        )
      ]

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(item1, xs = Grid.Width.create 3)
        GridItem.create(item2, xs = Grid.Width.create 3)
    ],
    justify = JustifyContent.center // see here
)

Grid.create(
    [
        GridItem.create(item1, xs = Grid.Width.create 3)
        GridItem.create(item2, xs = Grid.Width.create 3)
    ],
    justify = JustifyContent.spaceBetween // see here
)"""

    Helpers.codeSampleSection "Justify Content" description content code

  let private flexBreakExample () =
    let description =
      Helpers.bodyText "Use FlexBreak to force items onto a new row without changing their width."

    let content =
      Grid.create (
        [
          GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, xs = Grid.Width.create 4)

          FlexBreak.create ()

          GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, xs = Grid.Width.create 4)
          GridItem.create (demoBox (View.Const "Item 3") BrandColor.Tertiary, xs = Grid.Width.create 4)
          GridItem.create (demoBox (View.Const "Item 4") BrandColor.Primary, xs = Grid.Width.create 4)
        ]
      )

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(item1, xs = Grid.Width.create 4)

        FlexBreak.create() // see here

        GridItem.create(item2, xs = Grid.Width.create 4)
        GridItem.create(item3, xs = Grid.Width.create 4)
        GridItem.create(item4, xs = Grid.Width.create 4)
    ]
)"""

    Helpers.codeSampleSection "Flex Break" description content code

  let private nestedGridExample () =
    let description =
      Helpers.bodyText "Grids can be nested inside grid items for complex layouts."

    let content =
      Grid.create (
        [
          GridItem.create (
            Container.create (
              [
                Body1.div (
                  "Outer Grid - Left Column",
                  attrs = [ Margin.Bottom.extraSmall; Attr.Style "color" "var(--palette-text-primary)" ]
                )
                Grid.create (
                  [
                    GridItem.create (
                      demoBox (View.Const "Nested 1") BrandColor.Primary,
                      xs = Grid.Width.create 6
                    )
                    GridItem.create (
                      demoBox (View.Const "Nested 2") BrandColor.Secondary,
                      xs = Grid.Width.create 6
                    )
                    GridItem.create (
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

          GridItem.create (
            Container.create (
              [
                Body1.div (
                  "Outer Grid - Right Column",
                  attrs = [ Margin.Bottom.extraSmall; Attr.Style "color" "var(--palette-text-primary)" ]
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

Grid.create(
    [
        GridItem.create(
            Grid.create( // see here
                [
                    GridItem.create(nested1, xs = Grid.Width.create 6)
                    GridItem.create(nested2, xs = Grid.Width.create 6)
                    GridItem.create(nested3, xs = Grid.Width.create 12)
                ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
        )
        GridItem.create(
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
        Padding.All.medium
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
        Attr.Style "border" "1px solid var(--palette-divider)"
        BorderRadius.All.medium
        Attr.Style "height" "100%"
      ] [
        H5.div (View.Const title, attrs = [ Margin.Bottom.extraSmall ])
        Body1.div (View.Const description)
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            card "Feature 1" "Description of the first feature with some example text.",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
          )
          GridItem.create (
            card "Feature 2" "Description of the second feature with more information.",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
          )
          GridItem.create (
            card "Feature 3" "Description of the third feature explaining functionality.",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
          )
          GridItem.create (
            card "Feature 4" "Description of the fourth feature with details.",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
          )
          GridItem.create (
            card "Feature 5" "Description of the fifth feature and its benefits.",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
          )
          GridItem.create (
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
        Padding.All.medium
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
        BorderRadius.All.medium
    ] [
        H5.div(View.Const title)
        Body1.div(View.Const description)
    ]

Grid.create(
    [
        GridItem.create(
            card "Feature 1" "First feature description.",
            xs = Grid.Width.create 12, // see here
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
        )
        GridItem.create(
            card "Feature 2" "Second feature description.",
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6,
            lg = Grid.Width.create 4
        )
    ]
)"""

    Helpers.codeSampleSection "Card Layout" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Grid"
        Body1.div (
          "The Grid component uses a 12-column system to create flexible, responsive layouts. Items can span different numbers of columns at different breakpoints.",
          attrs = [ Margin.Bottom.extraSmall ]
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
      attrs = [ Container.MaxWidth.large ]
    )
