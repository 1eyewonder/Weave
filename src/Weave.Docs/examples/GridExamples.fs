namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module GridExamples =

  let private demoBox (label: View<string>) backgroundColorAttr =
    Container.create (
      content = div [ Typography.button ] [ textView label ],
      attrs = [
        backgroundColorAttr
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
          GridItem.create (
            demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
            attrs = [ GridItem.Span.twelve ]
          )
          GridItem.create (
            demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
            attrs = [ GridItem.Span.twelve ]
          )
          GridItem.create (
            demoBox (View.Const "Item 3") BrandColor.BackgroundColor.tertiary,
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
                demoBox (View.Const "1/4") BrandColor.BackgroundColor.primary,
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
          let item text backgroundColorAttr =
            GridItem.create (
              demoBox text backgroundColorAttr,
              attrs = [
                GridItem.Span.twelve
                GridItem.Span.Medium.six
                GridItem.Span.Large.four
                GridItem.Span.ExtraLarge.three
              ]
            )

          item Breakpoint.browserAsText BrandColor.BackgroundColor.primary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.secondary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.tertiary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.primary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.secondary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.tertiary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.primary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.secondary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.tertiary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.primary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.secondary
          item Breakpoint.browserAsText BrandColor.BackgroundColor.tertiary
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
        let item backgroundColorAttr =
          GridItem.create (demoBox (View.Const "Item") backgroundColorAttr, attrs = [ GridItem.Span.four ])

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Extra Small" ]

        Grid.create (
          [
            item BrandColor.BackgroundColor.primary
            item BrandColor.BackgroundColor.secondary
            item BrandColor.BackgroundColor.tertiary
          ],
          attrs = [ Grid.Spacing.extraSmall ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Small" ]

        Grid.create (
          [
            item BrandColor.BackgroundColor.primary
            item BrandColor.BackgroundColor.secondary
            item BrandColor.BackgroundColor.tertiary
          ],
          attrs = [ Grid.Spacing.small ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Medium (default)" ]

        Grid.create (
          [
            item BrandColor.BackgroundColor.primary
            item BrandColor.BackgroundColor.secondary
            item BrandColor.BackgroundColor.tertiary
          ],
          attrs = [ Grid.Spacing.medium ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Large" ]

        Grid.create (
          [
            item BrandColor.BackgroundColor.primary
            item BrandColor.BackgroundColor.secondary
            item BrandColor.BackgroundColor.tertiary
          ],
          attrs = [ Grid.Spacing.large ]
        )

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Extra Large" ]

        Grid.create (
          [
            item BrandColor.BackgroundColor.primary
            item BrandColor.BackgroundColor.secondary
            item BrandColor.BackgroundColor.tertiary
          ],
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
              demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.flexStart ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: Center" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.center ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: End" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
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
              demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
              attrs = [ GridItem.Span.three ]
            )
          ],
          attrs = [ JustifyContent.spaceBetween ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: Space Around" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
              attrs = [ GridItem.Span.three ]
            )
          ]
        )

        div [ Typography.body1; Margin.Top.medium; Margin.Bottom.extraSmall ] [ text "Justify: Space Evenly" ]
        Grid.create (
          [
            GridItem.create (
              demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
              attrs = [ GridItem.Span.three ]
            )
            GridItem.create (
              demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
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
          GridItem.create (
            demoBox (View.Const "Item 1") BrandColor.BackgroundColor.primary,
            attrs = [ GridItem.Span.four ]
          )

          FlexBreak.create ()

          GridItem.create (
            demoBox (View.Const "Item 2") BrandColor.BackgroundColor.secondary,
            attrs = [ GridItem.Span.four ]
          )
          GridItem.create (
            demoBox (View.Const "Item 3") BrandColor.BackgroundColor.tertiary,
            attrs = [ GridItem.Span.four ]
          )
          GridItem.create (
            demoBox (View.Const "Item 4") BrandColor.BackgroundColor.primary,
            attrs = [ GridItem.Span.four ]
          )
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
                      demoBox (View.Const "Nested 1") BrandColor.BackgroundColor.primary,
                      attrs = [ GridItem.Span.six ]
                    )
                    GridItem.create (
                      demoBox (View.Const "Nested 2") BrandColor.BackgroundColor.secondary,
                      attrs = [ GridItem.Span.six ]
                    )
                    GridItem.create (
                      demoBox (View.Const "Nested 3") BrandColor.BackgroundColor.tertiary,
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
                demoBox (View.Const "Full Height Item") BrandColor.BackgroundColor.primary
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

  let private spanToAttr (n: int) =
    match n with
    | 1 -> GridItem.Span.one
    | 2 -> GridItem.Span.two
    | 3 -> GridItem.Span.three
    | 4 -> GridItem.Span.four
    | 5 -> GridItem.Span.five
    | 6 -> GridItem.Span.six
    | 7 -> GridItem.Span.seven
    | 8 -> GridItem.Span.eight
    | 9 -> GridItem.Span.nine
    | 10 -> GridItem.Span.ten
    | 11 -> GridItem.Span.eleven
    | 12 -> GridItem.Span.twelve
    | _ -> GridItem.Span.three

  let private spanToName (n: int) =
    match n with
    | 1 -> "one"
    | 2 -> "two"
    | 3 -> "three"
    | 4 -> "four"
    | 5 -> "five"
    | 6 -> "six"
    | 7 -> "seven"
    | 8 -> "eight"
    | 9 -> "nine"
    | 10 -> "ten"
    | 11 -> "eleven"
    | 12 -> "twelve"
    | _ -> "three"

  let private gridBuilderExample () =
    let description =
      Helpers.bodyText "Calculate your breakpoints with this tool. Move the slider to add and remove items."

    let itemCount = Var.Create 4

    // Each item gets its own span Var (pre-create 12 for max items)
    let spanVars = [| for _ in 0..11 -> Var.Create 3 |]

    let allJustify = [
      "Flex Start", JustifyContent.flexStart, "JustifyContent.flexStart"
      "Center", JustifyContent.center, "JustifyContent.center"
      "Flex End", JustifyContent.flexEnd, "JustifyContent.flexEnd"
      "Space Between", JustifyContent.spaceBetween, "JustifyContent.spaceBetween"
      "Space Around", JustifyContent.spaceAround, "JustifyContent.spaceAround"
      "Space Evenly", JustifyContent.spaceEvenly, "JustifyContent.spaceEvenly"
    ]

    let selectedJustify = Var.Create<string option>(Some "Flex Start")

    let justifyItems =
      allJustify
      |> List.map (fun (label, _, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    // Build the dynamic code string
    let codeView =
      let spanViews = spanVars |> Array.map (fun v -> v.View) |> Array.toList

      let combinedSpans =
        spanViews
        |> List.fold (fun acc sv -> (acc, sv) ||> View.Map2(fun items s -> items @ [ s ])) (View.Const [])

      View.Map3
        (fun count justOpt (allSpans: int list) ->
          let spans = allSpans |> List.take (min count (List.length allSpans))

          let justifyCodeName =
            justOpt
            |> Option.bind (fun label ->
              allJustify
              |> List.tryFind (fun (l, _, _) -> l = label)
              |> Option.map (fun (_, _, c) -> c))
            |> Option.defaultValue "JustifyContent.flexStart"

          let itemLines =
            spans
            |> List.mapi (fun i s ->
              sprintf "        GridItem.create(item%d, attrs = [ GridItem.Span.%s ])" (i + 1) (spanToName s))
            |> String.concat "\n"

          sprintf
            """open Weave

Grid.create(
    [
%s
    ],
    attrs = [ %s ]
)"""
            itemLines
            justifyCodeName)
        itemCount.View
        selectedJustify.View
        combinedSpans

    let content =
      div [] [
        // Slider for item count
        Slider.primary (
          itemCount,
          min = 1,
          max = 12,
          step = 1,
          showTickMarks = true,
          tickMarkLabels = [ for i in 1..12 -> string i ],
          labelText = View.Const "Items"
        )

        // Justify select
        div [ Margin.Top.small; Margin.Bottom.small ] [
          Select.create (
            justifyItems,
            selectedJustify,
            variant = Select.Variant.Standard,
            labelText = View.Const "Justify",
            attrs = [ Select.Width.full; Select.Color.primary ]
          )
        ]

        // Reactive grid preview
        let combined =
          let spanViews = spanVars |> Array.map (fun v -> v.View) |> Array.toList

          let combinedSpans =
            spanViews
            |> List.fold (fun acc sv -> (acc, sv) ||> View.Map2(fun items s -> items @ [ s ])) (View.Const [])

          View.Map3
            (fun count justOpt allSpans -> count, justOpt, allSpans)
            itemCount.View
            selectedJustify.View
            combinedSpans

        combined
        |> Doc.BindView(fun (count, justOpt, allSpans) ->
          let justAttr =
            justOpt
            |> Option.bind (fun label ->
              allJustify
              |> List.tryFind (fun (l, _, _) -> l = label)
              |> Option.map (fun (_, a, _) -> a))
            |> Option.defaultValue JustifyContent.flexStart

          let spans = allSpans |> List.take (min count (List.length allSpans))

          Grid.create (
            [
              for i in 0 .. (List.length spans - 1) do
                let spanVal = spans.[i]

                GridItem.create (
                  Container.create (
                    NumericField.create (
                      spanVars.[i],
                      min = 1,
                      max = 12,
                      variant = Field.Variant.Outlined,
                      attrs = [ Field.Width.full ]
                    ),
                    attrs = [
                      Flex.Flex.allSizes
                      JustifyContent.center
                      AlignItems.center
                      BorderWidth.All.one
                      Attr.Style "border-style" "solid"
                      BorderColor.linesDefault
                      BorderRadius.All.small
                      Padding.All.small
                    ]
                  ),
                  attrs = [ spanToAttr spanVal ]
                )
            ],
            attrs = [ justAttr; Grid.Spacing.medium ]
          ))
      ]

    // Dynamic code via expansion panel
    div [ Margin.Bottom.small ] [
      Helpers.sectionHeader "Grid Builder"

      div [ Margin.Bottom.extraSmall ] [ description ]

      div [
        SurfaceColor.BackgroundColor.surface
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Padding.All.small
        BorderRadius.All.small
      ] [
        content

        let codeIsExpanded = Var.Create false

        let icon =
          codeIsExpanded.View
          |> Doc.BindView(fun expanded ->
            let attrs = [ AlignItems.center ]

            if expanded then
              Icon.create (Icon.UiActions UiActions.CollapseAll, attrs = attrs)
            else
              Icon.create (Icon.UiActions UiActions.ExpandAll, attrs = attrs))

        let headerText =
          codeIsExpanded.View
          |> View.MapCached(fun expanded -> if expanded then "Hide Code" else "Show Code")

        let header =
          ExpansionPanelHeader.create (
            content = div [ Typography.subtitle2 ] [ textView headerText ],
            expanded = codeIsExpanded,
            icon = icon,
            attrs = [ ExpansionPanel.Color.primary ]
          )

        let codeContent =
          codeView
          |> Doc.BindView(fun codeStr ->
            pre [] [
              code [
                SurfaceColor.BackgroundColor.background
                Attr.Class "language-fsharp"
                on.afterRender Helpers.highlightCodeElement
                Typography.Family.mono
              ] [ text codeStr ]
            ])

        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = header,
              content = ExpansionPanelContent.create (codeContent),
              expanded = codeIsExpanded
            )
          ],
          attrs = [ Margin.Top.extraSmall ]
        )
      ]
    ]

  let private cardLayoutExample () =
    let description =
      Helpers.bodyText "A practical example using grid for a responsive card layout."

    let card title description =
      div [
        Padding.All.medium
        SurfaceColor.BackgroundColor.paper
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
        SurfaceColor.BackgroundColor.paper
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
        gridBuilderExample ()
        Helpers.divider ()
        nestedGridExample ()
        Helpers.divider ()
        cardLayoutExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
