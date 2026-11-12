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

  let private basicGridExample () =
    let description =
      Helpers.bodyText
        "Use a twelve-column span to stack items. The fill and padding make each item visible; they are ordinary public styling attributes."

    let content =
      let createItem span label =
        let content =
          div [
            BrandColor.Fill.primary
            Padding.All.extraSmall
            Typography.Align.center
            Attr.Style "inline-size" "100%"
            Attr.Style "min-inline-size" "0"
            Attr.Style "box-sizing" "border-box"
            Attr.Style "overflow-wrap" "anywhere"
          ] [ text label ]

        GridItem.create (content, attrs = [ span ])

      Grid.create (
        [
          createItem GridItem.Span.twelve "Item 1"
          createItem GridItem.Span.twelve "Item 2"
          createItem GridItem.Span.twelve "Item 3"
        ]
      )

    let code =
      """let createItem span label =
  let content =
    div [
      BrandColor.Fill.primary
      Padding.All.extraSmall
      Typography.Align.center
      Attr.Style "inline-size" "100%"
      Attr.Style "min-inline-size" "0"
      Attr.Style "box-sizing" "border-box"
      Attr.Style "overflow-wrap" "anywhere"
    ] [ text label ]

  GridItem.create (content, attrs = [ span ])

Grid.create (
  [
    createItem GridItem.Span.twelve "Item 1"
    createItem GridItem.Span.twelve "Item 2"
    createItem GridItem.Span.twelve "Item 3"
  ]
)"""

    Helpers.codeSampleSection "Basic Grid" description content code

  let private equalColumnsExample () =
    let description =
      Helpers.bodyText "Each numbered item spans three of the twelve columns: one quarter of the grid."

    let content =
      let createItem span label =
        let content =
          div [
            BrandColor.Fill.primary
            Padding.All.extraSmall
            Typography.Align.center
            Attr.Style "inline-size" "100%"
            Attr.Style "min-inline-size" "0"
            Attr.Style "box-sizing" "border-box"
            Attr.Style "overflow-wrap" "anywhere"
          ] [ text label ]

        GridItem.create (content, attrs = [ span ])

      Grid.create (
        [
          for index in 1..4 do
            createItem GridItem.Span.three (string index)
        ]
      )

    let code =
      """let createItem span label =
  let content =
    div [
      BrandColor.Fill.primary
      Padding.All.extraSmall
      Typography.Align.center
      Attr.Style "inline-size" "100%"
      Attr.Style "min-inline-size" "0"
      Attr.Style "box-sizing" "border-box"
      Attr.Style "overflow-wrap" "anywhere"
    ] [ text label ]

  GridItem.create (content, attrs = [ span ])

Grid.create (
  [
    for index in 1..4 do
      createItem GridItem.Span.three (string index)
  ]
)"""

    Helpers.codeSampleSection "Equal Columns" description content code

  let private responsiveGridExample () =
    let description =
      Helpers.bodyText
        "Resize the window: items use full width below Medium, half width at Medium, a third at Large, and a quarter at ExtraLarge."

    let content =
      let spans = [
        GridItem.Span.twelve
        GridItem.Span.Medium.six
        GridItem.Span.Large.four
        GridItem.Span.ExtraLarge.three
      ]

      Grid.create (
        [
          for index in 1..12 do
            let content =
              div [
                BrandColor.Fill.primary
                Padding.All.extraSmall
                Typography.Align.center
                Attr.Style "inline-size" "100%"
                Attr.Style "min-inline-size" "0"
                Attr.Style "box-sizing" "border-box"
                Attr.Style "overflow-wrap" "anywhere"
              ] [ text (sprintf "Item %d" index) ]

            GridItem.create (content, attrs = spans)
        ]
      )

    let code =
      """let spans = [
  GridItem.Span.twelve
  GridItem.Span.Medium.six
  GridItem.Span.Large.four
  GridItem.Span.ExtraLarge.three
]

Grid.create (
  [
    for index in 1..12 do
      let content =
        div [
          BrandColor.Fill.primary
          Padding.All.extraSmall
          Typography.Align.center
          Attr.Style "inline-size" "100%"
          Attr.Style "min-inline-size" "0"
          Attr.Style "box-sizing" "border-box"
          Attr.Style "overflow-wrap" "anywhere"
        ] [ text (sprintf "Item %d" index) ]

      GridItem.create (content, attrs = spans)
  ]
)"""

    Helpers.codeSampleSection "Responsive Grid" description content code

  let private spacingExample () =
    let description =
      Helpers.bodyText
        "Compare all five spacing values with numbered items 1, 2, and 3. Each spans four columns."

    let content =
      let createItem span label =
        let content =
          div [
            BrandColor.Fill.primary
            Padding.All.extraSmall
            Typography.Align.center
            Attr.Style "inline-size" "100%"
            Attr.Style "min-inline-size" "0"
            Attr.Style "box-sizing" "border-box"
            Attr.Style "overflow-wrap" "anywhere"
          ] [ text label ]

        GridItem.create (content, attrs = [ span ])

      let spacings = [
        "Extra small", Grid.Spacing.extraSmall
        "Small", Grid.Spacing.small
        "Medium (default)", Grid.Spacing.medium
        "Large", Grid.Spacing.large
        "Extra large", Grid.Spacing.extraLarge
      ]

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        for label, spacing in spacings do
          div [] [
            p [] [ text label ]
            Grid.create (
              [
                for index in 1..3 do
                  createItem GridItem.Span.four (string index)
              ],
              attrs = [ spacing ]
            )
          ]
      ]

    let code =
      """let createItem span label =
  let content =
    div [
      BrandColor.Fill.primary
      Padding.All.extraSmall
      Typography.Align.center
      Attr.Style "inline-size" "100%"
      Attr.Style "min-inline-size" "0"
      Attr.Style "box-sizing" "border-box"
      Attr.Style "overflow-wrap" "anywhere"
    ] [ text label ]

  GridItem.create (content, attrs = [ span ])

let spacings = [
  "Extra small", Grid.Spacing.extraSmall
  "Small", Grid.Spacing.small
  "Medium (default)", Grid.Spacing.medium
  "Large", Grid.Spacing.large
  "Extra large", Grid.Spacing.extraLarge
]

div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
  for label, spacing in spacings do
    div [] [
      p [] [ text label ]
      Grid.create (
        [
          for index in 1..3 do
            createItem GridItem.Span.four (string index)
        ],
        attrs = [ spacing ]
      )
    ]
]"""

    Helpers.codeSampleSection "Spacing" description content code

  let private justifyContentExample () =
    let description =
      Helpers.bodyText
        "Compare how items 1 and 2 distribute the remaining horizontal space. Each item spans one quarter of the grid."

    let content =
      let createItem span label =
        let content =
          div [
            BrandColor.Fill.primary
            Padding.All.extraSmall
            Typography.Align.center
            Attr.Style "inline-size" "100%"
            Attr.Style "min-inline-size" "0"
            Attr.Style "box-sizing" "border-box"
            Attr.Style "overflow-wrap" "anywhere"
          ] [ text label ]

        GridItem.create (content, attrs = [ span ])

      let alignments = [
        "Start", JustifyContent.flexStart
        "Center", JustifyContent.center
        "End", JustifyContent.flexEnd
        "Space between", JustifyContent.spaceBetween
        "Space around", JustifyContent.spaceAround
        "Space evenly", JustifyContent.spaceEvenly
      ]

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        for label, alignment in alignments do
          div [] [
            p [] [ text label ]
            Grid.create (
              [ createItem GridItem.Span.three "1"; createItem GridItem.Span.three "2" ],
              attrs = [ alignment ]
            )
          ]
      ]

    let code =
      """let createItem span label =
  let content =
    div [
      BrandColor.Fill.primary
      Padding.All.extraSmall
      Typography.Align.center
      Attr.Style "inline-size" "100%"
      Attr.Style "min-inline-size" "0"
      Attr.Style "box-sizing" "border-box"
      Attr.Style "overflow-wrap" "anywhere"
    ] [ text label ]

  GridItem.create (content, attrs = [ span ])

let alignments = [
  "Start", JustifyContent.flexStart
  "Center", JustifyContent.center
  "End", JustifyContent.flexEnd
  "Space between", JustifyContent.spaceBetween
  "Space around", JustifyContent.spaceAround
  "Space evenly", JustifyContent.spaceEvenly
]

div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
  for label, alignment in alignments do
    div [] [
      p [] [ text label ]
      Grid.create (
        [
          createItem GridItem.Span.three "1"
          createItem GridItem.Span.three "2"
        ],
        attrs = [ alignment ]
      )
    ]
]"""

    Helpers.codeSampleSection "Justify Content" description content code

  let private flexBreakExample () =
    let description =
      Helpers.bodyText "FlexBreak forces the following items onto a new row while preserving their spans."

    let content =
      let createItem span label =
        let content =
          div [
            BrandColor.Fill.primary
            Padding.All.extraSmall
            Typography.Align.center
            Attr.Style "inline-size" "100%"
            Attr.Style "min-inline-size" "0"
            Attr.Style "box-sizing" "border-box"
            Attr.Style "overflow-wrap" "anywhere"
          ] [ text label ]

        GridItem.create (content, attrs = [ span ])

      Grid.create (
        [
          createItem GridItem.Span.four "Item 1"
          FlexBreak.create ()
          createItem GridItem.Span.four "Item 2"
          createItem GridItem.Span.four "Item 3"
          createItem GridItem.Span.four "Item 4"
        ]
      )

    let code =
      """let createItem span label =
  let content =
    div [
      BrandColor.Fill.primary
      Padding.All.extraSmall
      Typography.Align.center
      Attr.Style "inline-size" "100%"
      Attr.Style "min-inline-size" "0"
      Attr.Style "box-sizing" "border-box"
      Attr.Style "overflow-wrap" "anywhere"
    ] [ text label ]

  GridItem.create (content, attrs = [ span ])

Grid.create (
  [
    createItem GridItem.Span.four "Item 1"
    FlexBreak.create ()
    createItem GridItem.Span.four "Item 2"
    createItem GridItem.Span.four "Item 3"
    createItem GridItem.Span.four "Item 4"
  ]
)"""

    Helpers.codeSampleSection "Flex Break" description content code

  let private nestedGridExample () =
    let description =
      Helpers.bodyText "Nest a Grid inside a GridItem. The inner grid has its own twelve-column span system."

    let content =
      let createItem span label =
        let content =
          div [
            BrandColor.Fill.primary
            Padding.All.extraSmall
            Typography.Align.center
            Attr.Style "inline-size" "100%"
            Attr.Style "min-inline-size" "0"
            Attr.Style "box-sizing" "border-box"
            Attr.Style "overflow-wrap" "anywhere"
          ] [ text label ]

        GridItem.create (content, attrs = [ span ])

      let nested =
        Grid.create (
          [
            createItem GridItem.Span.six "Nested 1"
            createItem GridItem.Span.six "Nested 2"
            createItem GridItem.Span.twelve "Nested 3"
          ]
        )

      Grid.create (
        [
          GridItem.create (nested, attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ])
          GridItem.create (
            div [
              BrandColor.Fill.secondary
              Padding.All.extraSmall
              Typography.Align.center
              Attr.Style "inline-size" "100%"
              Attr.Style "min-inline-size" "0"
              Attr.Style "box-sizing" "border-box"
              Attr.Style "overflow-wrap" "anywhere"
            ] [ text "Sidebar" ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ]
      )

    let code =
      """let createItem span label =
  let content =
    div [
      BrandColor.Fill.primary
      Padding.All.extraSmall
      Typography.Align.center
      Attr.Style "inline-size" "100%"
      Attr.Style "min-inline-size" "0"
      Attr.Style "box-sizing" "border-box"
      Attr.Style "overflow-wrap" "anywhere"
    ] [ text label ]

  GridItem.create (content, attrs = [ span ])

let nested =
  Grid.create (
    [
      createItem GridItem.Span.six "Nested 1"
      createItem GridItem.Span.six "Nested 2"
      createItem GridItem.Span.twelve "Nested 3"
    ]
  )

Grid.create (
  [
    GridItem.create (
      nested,
      attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
    )
    GridItem.create (
      div [
        BrandColor.Fill.secondary
        Padding.All.extraSmall
        Typography.Align.center
        Attr.Style "inline-size" "100%"
        Attr.Style "min-inline-size" "0"
        Attr.Style "box-sizing" "border-box"
        Attr.Style "overflow-wrap" "anywhere"
      ] [ text "Sidebar" ],
      attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
    )
  ]
)"""

    Helpers.codeSampleSection "Nested Grids" description content code

  let gridBuilderExample () =
    let description =
      Helpers.bodyText
        "Choose base column spans, spacing, and alignment. Each labelled field edits one item; the bars below show the selected spans in the same order. Copied source contains that layout without the editor controls."

    let itemCount = Var.Create 4

    // Each item gets its own span Var (pre-create 12 for max items)
    let spanVars = [| for _ in 0..11 -> Var.Create 3 |]

    let allJustify = [
      "Flex Start", JustifyContent.flexStart, "flexStart"
      "Center", JustifyContent.center, "center"
      "Flex End", JustifyContent.flexEnd, "flexEnd"
      "Space Between", JustifyContent.spaceBetween, "spaceBetween"
      "Space Around", JustifyContent.spaceAround, "spaceAround"
      "Space Evenly", JustifyContent.spaceEvenly, "spaceEvenly"
    ]

    let selectedJustify = Var.Create<string option>(Some "Flex Start")

    let spacings = [
      "None", Grid.Spacing.none, "none"
      "Extra small", Grid.Spacing.extraSmall, "extraSmall"
      "Small", Grid.Spacing.small, "small"
      "Medium", Grid.Spacing.medium, "medium"
      "Large", Grid.Spacing.large, "large"
      "Extra large", Grid.Spacing.extraLarge, "extraLarge"
    ]

    let selectedSpacing = Var.Create<string option>(Some "Medium")

    let spacingItems =
      spacings
      |> List.map (fun (label, _, _) -> SelectItem.create (text label, label, label))
      |> View.Const

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
        (fun count (justOpt, spacingOpt) (allSpans: int list) ->
          let spans = allSpans |> List.take (min count (List.length allSpans))

          let justifyCodeName =
            justOpt
            |> Option.bind (fun label ->
              allJustify
              |> List.tryFind (fun (l, _, _) -> l = label)
              |> Option.map (fun (_, _, c) -> c))
            |> Option.defaultValue "flexStart"

          let spacingCodeName =
            spacingOpt
            |> Option.bind (fun label ->
              spacings
              |> List.tryFind (fun (l, _, _) -> l = label)
              |> Option.map (fun (_, _, c) -> c))
            |> Option.defaultValue "medium"

          Weave.Docs.GridSource.renderWithSpacing justifyCodeName spacingCodeName spans)
        itemCount.View
        ((selectedJustify.View, selectedSpacing.View) ||> View.Map2(fun a b -> a, b))
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
        div [
          Attr.Style "display" "grid"
          Attr.Style "grid-template-columns" "repeat(auto-fit, minmax(min(100%, 14rem), 1fr))"
          Gap.All.g3
          Margin.Top.small
          Margin.Bottom.small
        ] [
          Select.create (
            justifyItems,
            selectedJustify,
            labelText = View.Const "Justify",
            attrs = [
              Select.Variant.outlined
              Select.Label.above
              Select.Width.full
              Select.PopoverWidth.unbound
              Select.Color.primary
            ]
          )
          Select.create (
            spacingItems,
            selectedSpacing,
            labelText = View.Const "Spacing",
            attrs = [
              Select.Variant.outlined
              Select.Label.above
              Select.Width.full
              Select.PopoverWidth.unbound
              Select.Color.primary
            ]
          )
        ]

        Button.primary (
          text "Reset layout",
          onClick =
            (fun () ->
              Var.Set itemCount 4
              Var.Set selectedJustify (Some "Flex Start")
              Var.Set selectedSpacing (Some "Medium")

              for span in spanVars do
                Var.Set span 3),
          attrs = [ Button.Variant.outlined; Margin.Bottom.small ]
        )

        // Editors keep a usable width independently of the selected column spans.
        div [
          Attr.Style "display" "grid"
          Attr.Style "grid-template-columns" "repeat(auto-fit, minmax(min(100%, 12rem), 1fr))"
          Gap.All.g3
          Margin.Bottom.small
        ] [
          for index in 0 .. spanVars.Length - 1 do
            let visible = itemCount.View |> View.Map(fun count -> index < count)

            div [
              Attr.DynamicStyle "display" (visible |> View.Map(fun show -> if show then "block" else "none"))
            ] [
              NumericField.create (
                spanVars.[index],
                labelText = View.Const(sprintf "Item %d span" (index + 1)),
                min = 1,
                max = 12,
                attrs = [ NumericField.Variant.outlined; NumericField.Width.full; Field.Label.above ]
              )
            ]
        ]

        ((selectedJustify.View, selectedSpacing.View) ||> View.Map2(fun a b -> a, b))
        |> Doc.BindView(fun (justOpt, spacingOpt) ->
          let justAttr =
            justOpt
            |> Option.bind (fun selectedLabel ->
              allJustify
              |> List.tryFind (fun (label, _, _) -> label = selectedLabel)
              |> Option.map (fun (_, attr, _) -> attr))
            |> Option.defaultValue JustifyContent.flexStart

          let spacingAttr =
            spacingOpt
            |> Option.bind (fun label ->
              spacings
              |> List.tryFind (fun (l, _, _) -> l = label)
              |> Option.map (fun (_, attr, _) -> attr))
            |> Option.defaultValue Grid.Spacing.medium

          Grid.create (
            [
              for index in 0 .. spanVars.Length - 1 do
                let label = sprintf "Item %d" (index + 1)

                GridItem.create (
                  Container.create (
                    Doc.Empty,
                    attrs = [
                      Attr.Create "role" "img"
                      Attr.Create "aria-label" label
                      BrandColor.Fill.primary
                      BorderWidth.All.one
                      Attr.Style "border-style" "solid"
                      BorderColor.linesDefault
                      BorderRadius.All.small
                      Attr.Style "inline-size" "100%"
                      Attr.Style "min-inline-size" "0"
                      Attr.Style "box-sizing" "border-box"
                      Attr.Style "block-size" "32px"
                    ]
                  ),
                  attrs = [
                    for span in 1..12 do
                      let selected = spanVars.[index].View |> View.Map(fun value -> value = span)
                      Attr.DynamicClassPred (sprintf "weave-grid__item--xs-%d" span) selected

                    let visible = itemCount.View |> View.Map(fun count -> index < count)

                    Attr.DynamicStyle
                      "display"
                      (visible |> View.Map(fun show -> if show then "flex" else "none"))
                  ]
                )
            ],
            attrs = [ justAttr; spacingAttr ]
          ))
      ]

    // Dynamic code via expansion panel
    div [ Margin.Bottom.small; Attr.Class "docs-section" ] [
      Helpers.sectionHeader "Grid Builder"

      div [ Margin.Bottom.extraSmall; Attr.Class "docs-description" ] [ description ]

      div [
        SurfaceColor.BackgroundColor.surface
        Attr.Class "docs-example"
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        BorderRadius.All.small
      ] [
        div [ Attr.Class "docs-example__preview" ] [ content ]

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
          |> View.MapCached(fun expanded ->
            if expanded then
              "Hide layout source"
            else
              "Show layout source")

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
            div [ Attr.Class "docs-copy-code-wrapper" ] [
              Helpers.copyButton codeStr
              pre [
                attr.tabindex "0"
                Attr.Create "aria-label" "Selected grid layout F# source code"
              ] [
                code [
                  Attr.Class "language-fsharp"
                  on.afterRender Helpers.highlightCodeElement
                  Typography.Family.mono
                ] [ text codeStr ]
              ]
            ])

        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = header,
              content = ExpansionPanelContent.create (codeContent),
              expanded = codeIsExpanded
            )
          ],
          attrs = [ Attr.Class "docs-example__code" ]
        )
      ]
    ]

  let private cardLayoutExample () =
    let description =
      Helpers.bodyText
        "Compose real Card and CardContent components in a responsive grid. Cards stack, then use two or three columns as the window grows."

    let content =
      let createCard title description =
        let content =
          div [] [ h3 [ Typography.h6 ] [ text title ]; p [] [ text description ] ]

        Card.create (CardContent.create content, attrs = [ Attr.Style "inline-size" "100%" ])

      let cards = [
        "Plan", "Sketch the next change."
        "Build", "Compose components with shared state."
        "Review", "Try the result at different screen widths."
      ]

      Grid.create (
        [
          for title, description in cards do
            GridItem.create (
              createCard title description,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six; GridItem.Span.Large.four ]
            )
        ]
      )

    let code =
      """let createCard title description =
  let content =
    div [] [ h3 [ Typography.h6 ] [ text title ]; p [] [ text description ] ]

  Card.create (
    CardContent.create content,
    attrs = [ Attr.Style "inline-size" "100%" ]
  )

let cards = [
  "Plan", "Sketch the next change."
  "Build", "Compose components with shared state."
  "Review", "Try the result at different screen widths."
]

Grid.create (
  [
    for title, description in cards do
      GridItem.create (
        createCard title description,
        attrs = [
          GridItem.Span.twelve
          GridItem.Span.Medium.six
          GridItem.Span.Large.four
        ]
      )
  ]
)"""

    Helpers.codeSampleSection "Card Layout" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Grid, GridItem, and FlexBreak.") [
      Helpers.apiTable "Grid.create" [
        Helpers.apiParam "items" "Doc list" "" "List of grid item Docs (typically built with GridItem.create)"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (spacing, alignment, etc.)"
      ]

      Helpers.apiTable "GridItem.create" [
        Helpers.apiParam "content" "Doc" "" "Content rendered inside the grid cell"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (span at various breakpoints, etc.)"
      ]

      Helpers.apiTable "FlexBreak.create" []

      Helpers.styleModuleTable "Grid.Spacing" [
        ("none", "No gap between grid items (0px)")
        ("extraSmall", "Extra-small gap (2px)")
        ("small", "Small gap (4px)")
        ("medium", "Medium gap (8px)")
        ("large", "Large gap (12px)")
        ("extraLarge", "Extra-large gap (20px)")
      ]

      Helpers.styleModuleTable "GridItem.Span (default / XS)" [
        ("one through twelve", "Column span from 1 to 12 at the default (extra-small) breakpoint")
      ]

      Helpers.styleModuleTable "GridItem.Span.Small" [
        ("one through twelve", "Column span from 1 to 12 at the small breakpoint and above")
      ]

      Helpers.styleModuleTable "GridItem.Span.Medium" [
        ("one through twelve", "Column span from 1 to 12 at the medium breakpoint and above")
      ]

      Helpers.styleModuleTable "GridItem.Span.Large" [
        ("one through twelve", "Column span from 1 to 12 at the large breakpoint and above")
      ]

      Helpers.styleModuleTable "GridItem.Span.ExtraLarge" [
        ("one through twelve", "Column span from 1 to 12 at the extra-large breakpoint and above")
      ]
    ]

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
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
