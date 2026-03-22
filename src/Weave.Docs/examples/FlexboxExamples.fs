namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module FlexboxExamples =

  let private colorBox (label: string) (color: string) (extraAttrs: Attr list) =
    div [
      Attr.Style "background" color
      Attr.Style "color" "white"
      Attr.Style "padding" "8px 16px"
      Attr.Style "font-size" "13px"
      Attr.Style "font-weight" "600"
      Attr.Style "border-radius" "4px"
      Attr.Style "text-align" "center"
      yield! extraAttrs
    ] [ text label ]

  let private smallBox (label: string) (color: string) =
    colorBox label color [ Attr.Style "min-width" "60px" ]

  let private flexContainer (extraAttrs: Attr list) (children: Doc list) =
    div
      [
        SurfaceColor.toBackgroundColor SurfaceColor.Background
        BorderRadius.All.small
        BorderWidth.All.one
        Attr.Style "border-style" "dashed"
        BorderColor.linesDefault
        Padding.All.extraSmall
        Attr.Style "min-height" "80px"
        yield! extraAttrs
      ]
      children

  // ---------------------------------------------------------------------------
  // 1. How it works
  // ---------------------------------------------------------------------------

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "The flexbox system uses composable Attr modules. Start with Flex.Flex.allSizes to enable flex, then add direction, wrap, justify, align, and gap. All values support responsive breakpoints (xs, sm, md, lg, xl, xxl)."

    let content =
      div [
        Flex.Flex.allSizes
        FlexDirection.Row.allSizes
        JustifyContent.center
        AlignItems.center
        Gap.All.g3
        SurfaceColor.toBackgroundColor SurfaceColor.Background
        BorderRadius.All.small
        Padding.All.small
        Attr.Style "min-height" "80px"
      ] [
        smallBox "A" "var(--palette-primary)"
        smallBox "B" "var(--palette-secondary)"
        smallBox "C" "var(--palette-info)"
      ]

    let code =
      """open Weave

div [
    Flex.Flex.allSizes         // display: flex
    FlexDirection.Row.allSizes // flex-direction: row
    JustifyContent.center      // justify-content: center
    AlignItems.center          // align-items: center
    Gap.All.g3                 // gap: 12px (3 * 4px)
] [
    div [] [ text "A" ]
    div [] [ text "B" ]
    div [] [ text "C" ]
]"""

    Helpers.codeSampleSection "How it works" description content code

  // ---------------------------------------------------------------------------
  // 2. Playground
  // ---------------------------------------------------------------------------

  let private playgroundSection () =
    let description =
      Helpers.bodyText "Combine direction, justify, align, wrap, and gap to see how flex items rearrange."

    let allDirections = [
      "Row", FlexDirection.Row.allSizes
      "Column", FlexDirection.Column.allSizes
      "Row Reverse", FlexDirection.RowReverse.allSizes
      "Column Reverse", FlexDirection.ColumnReverse.allSizes
    ]

    let allJustify = [
      "Start", JustifyContent.flexStart
      "Center", JustifyContent.center
      "End", JustifyContent.flexEnd
      "Space Between", JustifyContent.spaceBetween
      "Space Around", JustifyContent.spaceAround
      "Space Evenly", JustifyContent.spaceEvenly
    ]

    let allAlign = [
      "Start", AlignItems.start
      "Center", AlignItems.center
      "End", AlignItems.end'
      "Stretch", AlignItems.stretch
      "Baseline", AlignItems.baseline
    ]

    let allWrap = [
      "No Wrap", FlexWrap.NoWrap.allSizes
      "Wrap", FlexWrap.Wrap.allSizes
      "Wrap Reverse", FlexWrap.WrapReverse.allSizes
    ]

    let allGaps = [
      "0 (0px)", Gap.All.g0
      "1 (4px)", Gap.All.g1
      "2 (8px)", Gap.All.g2
      "3 (12px)", Gap.All.g3
      "4 (16px)", Gap.All.g4
      "5 (20px)", Gap.All.g5
      "8 (32px)", Gap.All.g8
      "10 (40px)", Gap.All.g10
    ]

    let selectedDir = Var.Create<string option>(Some "Row")
    let selectedJustify = Var.Create<string option>(Some "Start")
    let selectedAlign = Var.Create<string option>(Some "Stretch")
    let selectedWrap = Var.Create<string option>(Some "No Wrap")
    let selectedGap = Var.Create<string option>(Some "3 (12px)")

    let makeItems label items =
      items |> List.map (fun (l, _) -> SelectItem.create (text l, l, l)) |> View.Const

    let content =
      div [] [
        Grid.create (
          [
            GridItem.create (
              Select.create (
                makeItems "Direction" allDirections,
                selectedDir,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Direction",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              Select.create (
                makeItems "Justify" allJustify,
                selectedJustify,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Justify Content",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              Select.create (
                makeItems "Align" allAlign,
                selectedAlign,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Align Items",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              Select.create (
                makeItems "Wrap" allWrap,
                selectedWrap,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Wrap",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              Select.create (
                makeItems "Gap" allGaps,
                selectedGap,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Gap",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
          ]
        )

        let lookupAttr allItems sel =
          sel
          |> Option.bind (fun label -> allItems |> List.tryFind (fun (l, _) -> l = label))
          |> Option.map snd
          |> Option.defaultValue Attr.Empty

        let combined =
          View.Map3 (fun d j a -> d, j, a) selectedDir.View selectedJustify.View selectedAlign.View
          |> View.Map2 (fun w (d, j, a) -> d, j, a, w) selectedWrap.View
          |> View.Map2 (fun g (d, j, a, w) -> d, j, a, w, g) selectedGap.View

        combined
        |> Doc.BindView(fun (dirSel, justSel, alignSel, wrapSel, gapSel) ->
          let dirAttr = lookupAttr allDirections dirSel
          let justAttr = lookupAttr allJustify justSel
          let alignAttr = lookupAttr allAlign alignSel
          let wrapAttr = lookupAttr allWrap wrapSel
          let gapAttr = lookupAttr allGaps gapSel

          div [ Margin.Top.small ] [
            flexContainer [
              Flex.Flex.allSizes
              dirAttr
              justAttr
              alignAttr
              wrapAttr
              gapAttr
              Attr.Style "min-height" "200px"
            ] [
              smallBox "1" "var(--palette-primary)"
              colorBox "2" "var(--palette-secondary)" [
                Attr.Style "min-width" "80px"
                Attr.Style "padding" "16px"
              ]
              smallBox "3" "var(--palette-info)"
              colorBox "4" "var(--palette-success)" [ Attr.Style "min-width" "100px" ]
              smallBox "5" "var(--palette-warning)"
            ]
          ])
      ]

    let code =
      """open Weave

// Compose all flex properties as attrs
div [
    Flex.Flex.allSizes              // see here — enable flex
    FlexDirection.Row.allSizes
    JustifyContent.spaceBetween
    AlignItems.center
    FlexWrap.Wrap.allSizes
    Gap.All.g3
] [
    div [] [ text "1" ]
    div [] [ text "2" ]
    div [] [ text "3" ]
]"""

    Helpers.codeSampleSection "Playground" description content code

  // ---------------------------------------------------------------------------
  // 3. Direction
  // ---------------------------------------------------------------------------

  let private directionSection () =
    let description =
      Helpers.bodyText
        "FlexDirection controls the main axis. Row (default) lays out horizontally, Column vertically."

    let dirDemo (label: string) (dirAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; dirAttr; Gap.All.g2 ] [
          smallBox "1" "var(--palette-primary)"
          smallBox "2" "var(--palette-secondary)"
          smallBox "3" "var(--palette-info)"
        ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            dirDemo "Row" FlexDirection.Row.allSizes,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            dirDemo "Column" FlexDirection.Column.allSizes,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            dirDemo "Row Reverse" FlexDirection.RowReverse.allSizes,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            dirDemo "Column Reverse" FlexDirection.ColumnReverse.allSizes,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ]
      )

    let code =
      """open Weave

// Row — horizontal, left to right (default)
div [ Flex.Flex.allSizes; FlexDirection.Row.allSizes ] [ ... ]

// Column — vertical, top to bottom
div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] [ ... ]

// Responsive: row on medium+, column on small
div [
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    FlexDirection.Row.md  // see here — switches to row at md
] [ ... ]"""

    Helpers.codeSampleSection "Direction" description content code

  // ---------------------------------------------------------------------------
  // 4. Justify Content
  // ---------------------------------------------------------------------------

  let private justifyContentSection () =
    let description =
      Helpers.bodyText "JustifyContent distributes items along the main axis."

    let justDemo (label: string) (justAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; justAttr; Gap.All.g2 ] [
          smallBox "A" "var(--palette-primary)"
          smallBox "B" "var(--palette-secondary)"
          smallBox "C" "var(--palette-info)"
        ]
      ]

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Attr.Style "gap" "12px" ] [
        justDemo "Start" JustifyContent.flexStart
        justDemo "Center" JustifyContent.center
        justDemo "End" JustifyContent.flexEnd
        justDemo "Space Between" JustifyContent.spaceBetween
        justDemo "Space Around" JustifyContent.spaceAround
        justDemo "Space Evenly" JustifyContent.spaceEvenly
      ]

    let code =
      """open Weave

div [ Flex.Flex.allSizes; JustifyContent.flexStart ] [ ... ]
div [ Flex.Flex.allSizes; JustifyContent.center ] [ ... ]
div [ Flex.Flex.allSizes; JustifyContent.flexEnd ] [ ... ]
div [ Flex.Flex.allSizes; JustifyContent.spaceBetween ] [ ... ]  // see here
div [ Flex.Flex.allSizes; JustifyContent.spaceAround ] [ ... ]
div [ Flex.Flex.allSizes; JustifyContent.spaceEvenly ] [ ... ]"""

    Helpers.codeSampleSection "Justify Content" description content code

  // ---------------------------------------------------------------------------
  // 5. Align Items
  // ---------------------------------------------------------------------------

  let private alignItemsSection () =
    let description =
      Helpers.bodyText
        "AlignItems positions items along the cross axis. Items with different heights make the alignment visible."

    let alignDemo (label: string) (alignAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; alignAttr; Gap.All.g2; Attr.Style "min-height" "140px" ] [
          colorBox "Tall" "var(--palette-primary)" [
            Attr.Style "height" "120px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "Short" "var(--palette-secondary)" [
            Attr.Style "height" "40px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "Medium" "var(--palette-info)" [
            Attr.Style "height" "72px"
            Attr.Style "padding" "8px 16px"
          ]
        ]
      ]

    let stretchDemo () =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text "Stretch" ]
        flexContainer [
          Flex.Flex.allSizes
          AlignItems.stretch
          Gap.All.g2
          Attr.Style "min-height" "140px"
        ] [
          colorBox "Tall" "var(--palette-primary)" [
            Attr.Style "height" "120px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "No height" "var(--palette-secondary)" [ Attr.Style "padding" "8px 16px" ]
          colorBox "No height" "var(--palette-info)" [ Attr.Style "padding" "8px 16px" ]
        ]
      ]

    let baselineDemo () =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text "Baseline" ]
        flexContainer [
          Flex.Flex.allSizes
          AlignItems.baseline
          Gap.All.g2
          Attr.Style "min-height" "140px"
        ] [
          colorBox "Large" "var(--palette-primary)" [
            Attr.Style "font-size" "28px"
            Attr.Style "padding" "24px 16px"
          ]
          colorBox "Small" "var(--palette-secondary)" [
            Attr.Style "font-size" "12px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "Medium" "var(--palette-info)" [
            Attr.Style "font-size" "18px"
            Attr.Style "padding" "16px"
          ]
        ]
      ]

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Attr.Style "gap" "12px" ] [
        alignDemo "Start" AlignItems.start
        alignDemo "Center" AlignItems.center
        alignDemo "End" AlignItems.end'
        stretchDemo ()
        baselineDemo ()
      ]

    let code =
      """open Weave

div [ Flex.Flex.allSizes; AlignItems.start ] [ ... ]
div [ Flex.Flex.allSizes; AlignItems.center ] [ ... ]    // see here
div [ Flex.Flex.allSizes; AlignItems.end' ] [ ... ]
div [ Flex.Flex.allSizes; AlignItems.stretch ] [ ... ]
div [ Flex.Flex.allSizes; AlignItems.baseline ] [ ... ]"""

    Helpers.codeSampleSection "Align Items" description content code

  // ---------------------------------------------------------------------------
  // 6. Align Content
  // ---------------------------------------------------------------------------

  let private alignContentSection () =
    let description =
      Helpers.bodyText
        "AlignContent distributes space between rows when flex items wrap onto multiple lines. It has no effect on single-line containers — you need FlexWrap.Wrap.allSizes and enough items to trigger wrapping."

    let contentDemo (label: string) (contentAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text label ]
        flexContainer [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          contentAttr
          Gap.All.g1
          Attr.Style "min-height" "160px"
          Attr.Style "max-width" "300px"
        ] [
          for i in 1..8 do
            smallBox
              (string i)
              (if i % 2 = 0 then
                 "var(--palette-secondary)"
               else
                 "var(--palette-primary)")
        ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            contentDemo "Start" AlignContent.start,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            contentDemo "Center" AlignContent.center,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            contentDemo "End" AlignContent.end',
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            contentDemo "Space Between" AlignContent.spaceBetween,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            contentDemo "Space Around" AlignContent.spaceAround,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            contentDemo "Stretch" AlignContent.stretch,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ]
      )

    let code =
      """open Weave

// AlignContent requires wrapping — no effect on single-line containers
div [
    Flex.Flex.allSizes
    FlexWrap.Wrap.allSizes
    AlignContent.spaceBetween
] [
    // enough items to wrap
]

// Responsive: center rows on medium+, stretch on small
div [
    Flex.Flex.allSizes
    FlexWrap.Wrap.allSizes
    AlignContent.stretch
    AlignContent.Medium.center
] [ ... ]"""

    Helpers.codeSampleSection "Align Content" description content code

  // ---------------------------------------------------------------------------
  // 7. Align Self
  // ---------------------------------------------------------------------------

  let private alignSelfSection () =
    let description =
      Helpers.bodyText
        "AlignSelf overrides the container's AlignItems for a single item. Place it directly on the item you want to reposition."

    let content =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [
          text "Container: AlignItems.start — individual items override"
        ]
        flexContainer [
          Flex.Flex.allSizes
          AlignItems.start
          Gap.All.g2
          Attr.Style "min-height" "120px"
        ] [
          colorBox "start (default)" "var(--palette-primary)" [ Attr.Style "padding" "24px 12px" ]
          colorBox "self: center" "var(--palette-secondary)" [ AlignSelf.center ]
          colorBox "self: end" "var(--palette-info)" [ AlignSelf.end' ]
          colorBox "self: stretch" "var(--palette-success)" [ AlignSelf.stretch ]
          colorBox "self: auto" "var(--palette-warning)" [ AlignSelf.auto ]
        ]
      ]

    let code =
      """open Weave

// Container aligns items to start
div [ Flex.Flex.allSizes; AlignItems.start ] [
    div [] [ text "Follows container (start)" ]
    div [ AlignSelf.center ] [ text "Overrides to center" ]
    div [ AlignSelf.end' ] [ text "Overrides to end" ]
    div [ AlignSelf.stretch ] [ text "Overrides to stretch" ]
]

// Responsive: auto on small, center on medium+
div [ AlignSelf.auto; AlignSelf.Medium.center ] [ text "Centered on md+" ]"""

    Helpers.codeSampleSection "Align Self" description content code

  // ---------------------------------------------------------------------------
  // 8. Gap
  // ---------------------------------------------------------------------------

  let private gapSection () =
    let description =
      Helpers.bodyText
        "Gap controls spacing between flex items. Values are multiples of 4px (g1 = 4px, g2 = 8px, g5 = 20px). Use Gap.X and Gap.Y for independent axis control."

    let gapDemo (label: string) (gapAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; gapAttr ] [
          smallBox "A" "var(--palette-primary)"
          smallBox "B" "var(--palette-secondary)"
          smallBox "C" "var(--palette-info)"
          smallBox "D" "var(--palette-success)"
        ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (gapDemo "g0 (0px)" Gap.All.g0, xs = Grid.Width.create 12, sm = Grid.Width.create 6)
          GridItem.create (gapDemo "g2 (8px)" Gap.All.g2, xs = Grid.Width.create 12, sm = Grid.Width.create 6)
          GridItem.create (
            gapDemo "g4 (16px)" Gap.All.g4,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            gapDemo "g8 (32px)" Gap.All.g8,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ]
      )

    let code =
      """open Weave

// Uniform gap
div [ Flex.Flex.allSizes; Gap.All.g3 ] [ ... ]  // 12px gap

// Independent axes
div [ Flex.Flex.allSizes; Gap.X.g4; Gap.Y.g2 ] [ ... ]  // see here

// Responsive gap — tighter on small screens
div [
    Flex.Flex.allSizes
    Gap.All.g2
    Gap.All.Medium.g4  // 16px on md+
] [ ... ]"""

    Helpers.codeSampleSection "Gap" description content code

  // ---------------------------------------------------------------------------
  // 9. Flex Item
  // ---------------------------------------------------------------------------

  let private flexItemSection () =
    let description =
      Helpers.bodyText
        "FlexItem controls grow, shrink, and order on individual items within a flex container."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Grow" ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "No grow" "var(--palette-secondary)" []
          colorBox "Grow" "var(--palette-primary)" [ FlexItem.Grow.allSizes ]
          colorBox "No grow" "var(--palette-secondary)" []
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "No Grow" ]
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [
          text "FlexItem.NoGrow prevents an item from growing even if the container has extra space."
        ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "NoGrow" "var(--palette-warning)" [ FlexItem.NoGrow.allSizes ]
          colorBox "Grow" "var(--palette-primary)" [ FlexItem.Grow.allSizes ]
          colorBox "Grow" "var(--palette-secondary)" [ FlexItem.Grow.allSizes ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Shrink vs No Shrink" ]
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [
          text "When items overflow, NoShrink prevents an item from getting smaller."
        ]
        flexContainer [
          Flex.Flex.allSizes
          FlexWrap.NoWrap.allSizes
          Gap.All.g2
          Margin.Bottom.small
          Attr.Style "max-width" "400px"
        ] [
          colorBox "Shrinks" "var(--palette-primary)" [
            FlexItem.Shrink.allSizes
            Attr.Style "min-width" "120px"
          ]
          colorBox "No Shrink" "var(--palette-error)" [
            FlexItem.NoShrink.allSizes
            Attr.Style "min-width" "200px"
          ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Flex 1 (equal sizing)" ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "flex-1" "var(--palette-primary)" [ FlexItem.Flex.allSizes ]
          colorBox "flex-1" "var(--palette-secondary)" [ FlexItem.Flex.allSizes ]
          colorBox "flex-1" "var(--palette-info)" [ FlexItem.Flex.allSizes ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Auto, Initial, and None" ]
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [
          text
            "Auto sizes based on content then grows/shrinks. Initial sizes based on content but only shrinks. None ignores flex entirely — uses the item's intrinsic width."
        ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "auto" "var(--palette-primary)" [ FlexItem.Auto.allSizes ]
          colorBox "initial" "var(--palette-secondary)" [ FlexItem.Initial.allSizes ]
          colorBox "none" "var(--palette-info)" [ FlexItem.None.allSizes ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Order" ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "order-3" "var(--palette-info)" [ Order.o3 ]
          colorBox "order-1" "var(--palette-primary)" [ Order.o1 ]
          colorBox "order-2" "var(--palette-secondary)" [ Order.o2 ]
        ]

        div [ Typography.body2; Attr.Style "opacity" "0.6" ] [
          text
            "The items above are written in DOM order 3, 1, 2 but display as 1, 2, 3 due to their order classes."
        ]
      ]

    let code =
      """open Weave

// Grow — one item fills remaining space
div [ Flex.Flex.allSizes; Gap.All.g2 ] [
    div [] [ text "Fixed" ]
    div [ FlexItem.Grow.allSizes ] [ text "I grow" ]
    div [] [ text "Fixed" ]
]

// NoGrow — prevent an item from growing
div [ Flex.Flex.allSizes ] [
    div [ FlexItem.NoGrow.allSizes ] [ text "Stays small" ]
    div [ FlexItem.Grow.allSizes ] [ text "I grow" ]
]

// NoShrink — prevent an item from shrinking when container overflows
div [ Flex.Flex.allSizes; FlexWrap.NoWrap.allSizes ] [
    div [ FlexItem.Shrink.allSizes ] [ text "I can shrink" ]
    div [ FlexItem.NoShrink.allSizes ] [ text "I keep my size" ]
]

// Flex 1 — all items share space equally
div [ Flex.Flex.allSizes; Gap.All.g2 ] [
    div [ FlexItem.Flex.allSizes ] [ text "1/3" ]
    div [ FlexItem.Flex.allSizes ] [ text "1/3" ]
    div [ FlexItem.Flex.allSizes ] [ text "1/3" ]
]

// Sizing modes: auto grows+shrinks, initial only shrinks, none ignores flex
div [ FlexItem.Auto.allSizes ] [ text "Grows and shrinks" ]
div [ FlexItem.Initial.allSizes ] [ text "Shrinks only" ]
div [ FlexItem.None.allSizes ] [ text "Uses intrinsic width" ]

// Order — rearrange visual order without changing DOM
div [ Flex.Flex.allSizes ] [
    div [ Order.o3 ] [ text "Third visually" ]
    div [ Order.o1 ] [ text "First visually" ]
    div [ Order.o2 ] [ text "Second visually" ]
]"""

    Helpers.codeSampleSection "Flex Item" description content code

  // ---------------------------------------------------------------------------
  // 10. Grow Children
  // ---------------------------------------------------------------------------

  let private growChildrenSection () =
    let description =
      Helpers.bodyText
        "GrowChildren is a container-level helper that applies flex-grow to specific child positions. Place it on the flex container — it uses CSS child selectors so you don't need to add attrs to individual items."

    let growDemo (label: string) (detail: string) (growAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.6" ] [ text label ]
        div [ Typography.caption; Margin.Bottom.extraSmall; Attr.Style "opacity" "0.5" ] [ text detail ]
        flexContainer [ Flex.Flex.allSizes; growAttr; Gap.All.g2 ] [
          smallBox "A" "var(--palette-primary)"
          smallBox "B" "var(--palette-secondary)"
          smallBox "C" "var(--palette-info)"
          smallBox "D" "var(--palette-success)"
        ]
      ]

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Attr.Style "gap" "12px" ] [
        growDemo "Start" "First child grows" FlexItem.GrowChildren.Start.allSizes
        growDemo "End" "Last child grows" FlexItem.GrowChildren.End.allSizes
        growDemo "Start and End" "First and last children grow" FlexItem.GrowChildren.StartAndEnd.allSizes
        growDemo "Middle" "All except first and last grow" FlexItem.GrowChildren.Middle.allSizes
        growDemo "All" "All children grow equally" FlexItem.GrowChildren.All.allSizes
      ]

    let code =
      """open Weave

// Only the first child grows — no attrs on children needed
div [
    Flex.Flex.allSizes
    FlexItem.GrowChildren.Start.allSizes
] [
    div [] [ text "I grow (first)" ]
    div [] [ text "Fixed" ]
    div [] [ text "Fixed" ]
]

// First and last grow — middle stays fixed
div [
    Flex.Flex.allSizes
    FlexItem.GrowChildren.StartAndEnd.allSizes
] [
    div [] [ text "Grows (first)" ]
    div [] [ text "Fixed (middle)" ]
    div [] [ text "Grows (last)" ]
]

// All children grow equally — like FlexItem.Flex but from the container
div [
    Flex.Flex.allSizes
    FlexItem.GrowChildren.All.allSizes
] [
    div [] [ text "1/3" ]
    div [] [ text "1/3" ]
    div [] [ text "1/3" ]
]

// Responsive: start-only on small, middle on medium+
div [
    Flex.Flex.allSizes
    FlexItem.GrowChildren.Start.allSizes
    FlexItem.GrowChildren.Middle.md
] [ ... ]"""

    Helpers.codeSampleSection "Grow Children" description content code

  // ---------------------------------------------------------------------------
  // Render
  // ---------------------------------------------------------------------------

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Flexbox"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Build flexible layouts with composable direction, alignment, wrapping, and gap utility classes. All values support responsive breakpoints."
        ]

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        playgroundSection ()
        Helpers.divider ()
        directionSection ()
        Helpers.divider ()
        justifyContentSection ()
        Helpers.divider ()
        alignItemsSection ()
        Helpers.divider ()
        alignContentSection ()
        Helpers.divider ()
        alignSelfSection ()
        Helpers.divider ()
        gapSection ()
        Helpers.divider ()
        flexItemSection ()
        Helpers.divider ()
        growChildrenSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
