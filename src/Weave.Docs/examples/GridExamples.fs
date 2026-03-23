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
      content = div [ Typography.button ] [ textView label ],
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
      Helpers.bodyText "A simple grid with columns stacked vertically on all screen sizes."

    let content =
      Grid.create (
        [
          GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, attrs = [ GridItem.Span.twelve ])
          GridItem.create (
            demoBox (View.Const "Item 2") BrandColor.Secondary,
            attrs = [ GridItem.Span.twelve ]
          )
          GridItem.create (
            demoBox (View.Const "Item 3") BrandColor.Tertiary,
            attrs = [ GridItem.Span.twelve ]
          )
        ]
      )

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(myContent, attrs = [ GridItem.Span.twelve ])
        GridItem.create(myContent, attrs = [ GridItem.Span.twelve ])
        GridItem.create(myContent, attrs = [ GridItem.Span.twelve ])
    ]
)"""

    Helpers.codeSampleSection "Basic Grid" description content code

  let private equalColumnsExample () =
    let description = Helpers.bodyText "Columns with equal widths (3/12 = 25% each)."

    let content =
      Grid.create (
        [
          yield!
            [ 0..3 ]
            |> List.map (fun _ ->
              GridItem.create (
                demoBox (View.Const "1/4") BrandColor.Primary,
                attrs = [ GridItem.Span.three ]
              ))
        ]
      )

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(myContent, attrs = [ GridItem.Span.three ])
        GridItem.create(myContent, attrs = [ GridItem.Span.three ])
        GridItem.create(myContent, attrs = [ GridItem.Span.three ])
        GridItem.create(myContent, attrs = [ GridItem.Span.three ])
    ]
)"""

    Helpers.codeSampleSection "Equal Columns" description content code

  let private responsiveGridExample () =
    let description =
      Helpers.bodyText
        "Columns that change width at different breakpoints. Resize your browser to see the effect."

    let content =
      Grid.create (
        [
          let item text color =
            GridItem.create (
              demoBox text color,
              attrs = [
                GridItem.Span.twelve
                GridItem.Span.Medium.six
                GridItem.Span.Large.four
                GridItem.Span.ExtraLarge.three
              ]
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
    attrs = [
        GridItem.Span.twelve // full width on small & extra small screens
        GridItem.Span.Medium.six // half width on medium
        GridItem.Span.Large.four // third width on large
        GridItem.Span.ExtraLarge.three // quarter width on extra-large
    ]
)"""

    Helpers.codeSampleSection "Responsive Grid" description content code

  let private spacingExample () =
    let description =
      Helpers.bodyText
        "Control the gap between columns with semantic spacing values from extraSmall to extraLarge."

    let content =
      div [] [
        let item color =
          GridItem.create (demoBox (View.Const "Item") color, attrs = [ GridItem.Span.four ])

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Extra Small" ]

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          attrs = [ Grid.Spacing.extraSmall ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Small" ]

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          attrs = [ Grid.Spacing.small ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Medium (default)" ]

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          attrs = [ Grid.Spacing.medium ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Large" ]

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          attrs = [ Grid.Spacing.large ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Extra Large" ]

        Grid.create (
          [ item BrandColor.Primary; item BrandColor.Secondary; item BrandColor.Tertiary ],
          attrs = [ Grid.Spacing.extraLarge ]
        )
      ]

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(item1, attrs = [ GridItem.Span.four ])
        GridItem.create(item2, attrs = [ GridItem.Span.four ])
        GridItem.create(item3, attrs = [ GridItem.Span.four ])
    ],
    attrs = [ Grid.Spacing.small ]
)"""

    Helpers.codeSampleSection "Spacing" description content code

  let private justifyContentExample () =
    let description =
      Helpers.bodyText "Control how columns are aligned along the main axis."

    let content =
      div [] [
        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Justify: Start" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.Primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.Secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.flexStart ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: Center" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.Primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.Secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.center ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: End" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.Primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.Secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.flexEnd ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [
          text "Justify: Space Between"
        ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.Primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.Secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.spaceBetween ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: Space Around" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.Primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.Secondary,
              attrs = [ GridItem.Span.three ]
            )
          ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: Space Evenly" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.Primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.Secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.spaceEvenly ]
        )
      ]

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(item1, attrs = [ GridItem.Span.three ])
        GridItem.create(item2, attrs = [ GridItem.Span.three ])
    ],
    attrs = [ JustifyContent.center ]
)

Grid.create(
    [
        GridItem.create(item1, attrs = [ GridItem.Span.three ])
        GridItem.create(item2, attrs = [ GridItem.Span.three ])
    ],
    attrs = [ JustifyContent.spaceBetween ]
)"""

    Helpers.codeSampleSection "Justify Content" description content code

  let private flexBreakExample () =
    let description =
      Helpers.bodyText "Use FlexBreak to force columns onto a new row without changing their width."

    let content =
      Grid.create (
        [
          GridItem.create (demoBox (View.Const "Item 1") BrandColor.Primary, attrs = [ GridItem.Span.four ])

          FlexBreak.create ()

          GridItem.create (demoBox (View.Const "Item 2") BrandColor.Secondary, attrs = [ GridItem.Span.four ])
          GridItem.create (demoBox (View.Const "Item 3") BrandColor.Tertiary, attrs = [ GridItem.Span.four ])
          GridItem.create (demoBox (View.Const "Item 4") BrandColor.Primary, attrs = [ GridItem.Span.four ])
        ]
      )

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(item1, attrs = [ GridItem.Span.four ])

        FlexBreak.create()

        GridItem.create(item2, attrs = [ GridItem.Span.four ])
        GridItem.create(item3, attrs = [ GridItem.Span.four ])
        GridItem.create(item4, attrs = [ GridItem.Span.four ])
    ]
)"""

    Helpers.codeSampleSection "Flex Break" description content code

  let private nestedGridExample () =
    let description =
      Helpers.bodyText "Grids can be nested inside columns for complex layouts."

    let content =
      Grid.create (
        [
          GridItem.create (
            Container.create (
              [
                div [
                  Typography.body1
                  Margin.Bottom.extraSmall
                  Attr.Style "color" "var(--palette-text-primary)"
                ] [ text "Outer Grid - Left Column" ]
                Grid.create (
                  [
                    GridItem.create (
                      demoBox (View.Const "Nested 1") BrandColor.Primary,
                      attrs = [ GridItem.Span.six ]
                    )
                    GridItem.create (
                      demoBox (View.Const "Nested 2") BrandColor.Secondary,
                      attrs = [ GridItem.Span.six ]
                    )
                    GridItem.create (
                      demoBox (View.Const "Nested 3") BrandColor.Tertiary,
                      attrs = [ GridItem.Span.twelve ]
                    )
                  ]
                )
              ]
              |> Doc.Concat
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            Container.create (
              [
                div [
                  Typography.body1
                  Margin.Bottom.extraSmall
                  Attr.Style "color" "var(--palette-text-primary)"
                ] [ text "Outer Grid - Right Column" ]
                demoBox (View.Const "Full Height Item") BrandColor.Primary
              ]
              |> Doc.Concat
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ]
      )

    let code =
      """open Weave

Grid.create(
    [
        GridItem.create(
            Grid.create(
                [
                    GridItem.create(nested1, attrs = [ GridItem.Span.six ])
                    GridItem.create(nested2, attrs = [ GridItem.Span.six ])
                    GridItem.create(nested3, attrs = [ GridItem.Span.twelve ])
                ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
        )
        GridItem.create(
            sidebarContent,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
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
        div [ Typography.h5; Margin.Bottom.extraSmall ] [ text title ]
        div [ Typography.body1 ] [ text description ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            card "Feature 1" "Description of the first feature with some example text.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
          )
          GridItem.create (
            card "Feature 2" "Description of the second feature with more information.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
          )
          GridItem.create (
            card "Feature 3" "Description of the third feature explaining functionality.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
          )
          GridItem.create (
            card "Feature 4" "Description of the fourth feature with details.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
          )
          GridItem.create (
            card "Feature 5" "Description of the fifth feature and its benefits.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
          )
          GridItem.create (
            card "Feature 6" "Description of the sixth feature with advantages.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
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
        div [ Typography.h5 ] [ text title ]
        div [ Typography.body1 ] [ text description ]
    ]

Grid.create(
    [
        GridItem.create(
            card "Feature 1" "First feature description.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
        )
        GridItem.create(
            card "Feature 2" "Second feature description.",
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
        )
    ]
)"""

    Helpers.codeSampleSection "Card Layout" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Grid"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Grid component uses a 12-column system to create flexible, responsive layouts. Columns can span different numbers of grid columns at different breakpoints."
        ]

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
