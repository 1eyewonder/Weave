namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module FlexboxExamples =

  let private colorBox (label: string) (color: Attr) (extraAttrs: Attr list) =
    div [
      color
      Typography.subtitle2
      Attr.Style "padding" "8px 16px"
      Attr.Style "border-radius" "4px"
      Attr.Style "text-align" "center"
      yield! extraAttrs
    ] [ text label ]

  let private smallBox (label: string) (color: Attr) =
    colorBox label color [ Attr.Style "min-width" "60px" ]

  let private flexContainer (extraAttrs: Attr list) (children: Doc list) =
    div
      [
        SurfaceColor.BackgroundColor.background
        BorderRadius.All.small
        BorderWidth.All.one
        Attr.Style "border-style" "dashed"
        BorderColor.linesDefault
        Padding.All.extraSmall
        Attr.Style "min-height" "80px"
        Attr.Style "overflow-x" "auto"
        yield! extraAttrs
      ]
      children

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Start with Flex.Flex.allSizes, then compose direction, wrapping, alignment, and gap attributes. Source includes the colored items; comparison labels and dashed frames are omitted. Responsive variants are available from xs through xxl."

    let content =
      div [
        Flex.Flex.allSizes
        FlexDirection.Row.allSizes
        JustifyContent.center
        AlignItems.center
        Gap.All.g3
        SurfaceColor.BackgroundColor.background
        BorderRadius.All.small
        Padding.All.small
        Attr.Style "min-height" "80px"
      ] [
        smallBox "A" BrandColor.Fill.primary
        smallBox "B" BrandColor.Fill.secondary
        smallBox "C" BrandColor.Fill.info
      ]

    let code =
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

div [
  Flex.Flex.allSizes
  FlexDirection.Row.allSizes
  JustifyContent.center
  AlignItems.center
  Gap.All.g3
] [
  box "A" BrandColor.Fill.primary []
  box "B" BrandColor.Fill.secondary []
  box "C" BrandColor.Fill.info []
]"""

    Helpers.codeSampleSection "How it works" description content code

  // 2. Playground
  let private playgroundSection () =
    let description =
      Helpers.bodyText
        "Combine direction, justify, align, wrap, and gap to see how flex items rearrange. The source shows the initial layout; the preview controls let you explore other combinations."

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
                labelText = View.Const "Direction",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four ]
            )
            GridItem.create (
              Select.create (
                makeItems "Justify" allJustify,
                selectedJustify,
                labelText = View.Const "Justify Content",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four ]
            )
            GridItem.create (
              Select.create (
                makeItems "Align" allAlign,
                selectedAlign,
                labelText = View.Const "Align Items",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four ]
            )
            GridItem.create (
              Select.create (
                makeItems "Wrap" allWrap,
                selectedWrap,
                labelText = View.Const "Wrap",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four ]
            )
            GridItem.create (
              Select.create (
                makeItems "Gap" allGaps,
                selectedGap,
                labelText = View.Const "Gap",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four ]
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
              smallBox "1" BrandColor.Fill.primary
              colorBox "2" BrandColor.Fill.secondary [
                Attr.Style "min-width" "80px"
                Attr.Style "padding" "16px"
              ]
              smallBox "3" BrandColor.Fill.info
              colorBox "4" BrandColor.Fill.success [ Attr.Style "min-width" "100px" ]
              smallBox "5" BrandColor.Fill.warning
            ]
          ])
      ]

    let code =
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

// Initial playground layout. Change the preview controls to explore.
div [
  Flex.Flex.allSizes
  FlexDirection.Row.allSizes
  JustifyContent.flexStart
  AlignItems.stretch
  FlexWrap.NoWrap.allSizes
  Gap.All.g3
  Attr.Style "min-height" "200px"
] [
  box "1" BrandColor.Fill.primary []
  box "2" BrandColor.Fill.secondary [
    Attr.Style "min-width" "80px"
    Padding.All.small
  ]
  box "3" BrandColor.Fill.info []
  box "4" BrandColor.Fill.success [ Attr.Style "min-width" "100px" ]
  box "5" BrandColor.Fill.warning []
]"""

    Helpers.codeSampleSection "Playground" description content code

  // 3. Direction
  let private directionSection () =
    let description =
      Helpers.bodyText
        "FlexDirection controls the main axis. Row (default) lays out horizontally, Column vertically."

    let dirDemo (label: string) (dirAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; dirAttr; Gap.All.g2 ] [
          smallBox "1" BrandColor.Fill.primary
          smallBox "2" BrandColor.Fill.secondary
          smallBox "3" BrandColor.Fill.info
        ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            dirDemo "Row" FlexDirection.Row.allSizes,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            dirDemo "Column" FlexDirection.Column.allSizes,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            dirDemo "Row Reverse" FlexDirection.RowReverse.allSizes,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            dirDemo "Column Reverse" FlexDirection.ColumnReverse.allSizes,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

let row direction =
  div [ Flex.Flex.allSizes; direction; Gap.All.g2 ] [
    box "1" BrandColor.Fill.primary []
    box "2" BrandColor.Fill.secondary []
    box "3" BrandColor.Fill.info []
  ]

row FlexDirection.Row.allSizes
row FlexDirection.Column.allSizes
row FlexDirection.RowReverse.allSizes
row FlexDirection.ColumnReverse.allSizes"""

    Helpers.codeSampleSection "Direction" description content code

  // 4. Justify Content
  let private justifyContentSection () =
    let description =
      Helpers.bodyText "JustifyContent distributes items along the main axis."

    let justDemo (label: string) (justAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; justAttr; Gap.All.g2 ] [
          smallBox "A" BrandColor.Fill.primary
          smallBox "B" BrandColor.Fill.secondary
          smallBox "C" BrandColor.Fill.info
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
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

let items () = [
  box "A" BrandColor.Fill.primary []
  box "B" BrandColor.Fill.secondary []
  box "C" BrandColor.Fill.info []
]

let row justify =
  div [ Flex.Flex.allSizes; justify; Gap.All.g2 ] (items ())

row JustifyContent.flexStart
row JustifyContent.center
row JustifyContent.flexEnd
row JustifyContent.spaceBetween
row JustifyContent.spaceAround
row JustifyContent.spaceEvenly"""

    Helpers.codeSampleSection "Justify Content" description content code

  // 5. Align Items
  let private alignItemsSection () =
    let description =
      Helpers.bodyText
        "AlignItems positions items along the cross axis. Items with different heights make the alignment visible."

    let alignDemo (label: string) (alignAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; alignAttr; Gap.All.g2; Attr.Style "min-height" "140px" ] [
          colorBox "Tall" BrandColor.Fill.primary [
            Attr.Style "height" "120px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "Short" BrandColor.Fill.secondary [
            Attr.Style "height" "40px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "Medium" BrandColor.Fill.info [
            Attr.Style "height" "72px"
            Attr.Style "padding" "8px 16px"
          ]
        ]
      ]

    let stretchDemo () =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text "Stretch" ]
        flexContainer [
          Flex.Flex.allSizes
          AlignItems.stretch
          Gap.All.g2
          Attr.Style "min-height" "140px"
        ] [
          colorBox "Tall" BrandColor.Fill.primary [
            Attr.Style "height" "120px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "No height" BrandColor.Fill.secondary [ Attr.Style "padding" "8px 16px" ]
          colorBox "No height" BrandColor.Fill.info [ Attr.Style "padding" "8px 16px" ]
        ]
      ]

    let baselineDemo () =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text "Baseline" ]
        flexContainer [
          Flex.Flex.allSizes
          AlignItems.baseline
          Gap.All.g2
          Attr.Style "min-height" "140px"
        ] [
          colorBox "Large" BrandColor.Fill.primary [
            Attr.Style "font-size" "28px"
            Attr.Style "padding" "24px 16px"
          ]
          colorBox "Small" BrandColor.Fill.secondary [
            Attr.Style "font-size" "12px"
            Attr.Style "padding" "8px 16px"
          ]
          colorBox "Medium" BrandColor.Fill.info [
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
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

let row alignment children =
  div
    [
      Flex.Flex.allSizes
      alignment
      Gap.All.g2
      Attr.Style "min-height" "140px"
    ]
    children

let sizedItems () = [
  box "Tall" BrandColor.Fill.primary [ Attr.Style "height" "120px" ]
  box "Short" BrandColor.Fill.secondary [ Attr.Style "height" "40px" ]
  box "Medium" BrandColor.Fill.info [ Attr.Style "height" "72px" ]
]

row AlignItems.start (sizedItems ())
row AlignItems.center (sizedItems ())
row AlignItems.end' (sizedItems ())

row AlignItems.stretch [
  box "Tall" BrandColor.Fill.primary [ Attr.Style "height" "120px" ]
  box "No height" BrandColor.Fill.secondary []
  box "No height" BrandColor.Fill.info []
]

row AlignItems.baseline [
  box "Large" BrandColor.Fill.primary [
    Attr.Style "font-size" "28px"
    Attr.Style "padding" "24px 16px"
  ]
  box "Small" BrandColor.Fill.secondary [ Attr.Style "font-size" "12px" ]
  box "Medium" BrandColor.Fill.info [
    Attr.Style "font-size" "18px"
    Attr.Style "padding" "16px"
  ]
]"""

    Helpers.codeSampleSection "Align Items" description content code

  // 6. Align Content
  let private alignContentSection () =
    let description =
      Helpers.bodyText
        "AlignContent distributes space between rows when flex items wrap onto multiple lines. It has no effect on single-line containers — you need FlexWrap.Wrap.allSizes and enough items to trigger wrapping."

    let contentDemo (label: string) (contentAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text label ]
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
                 BrandColor.Fill.secondary
               else
                 BrandColor.Fill.primary)
        ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            contentDemo "Start" AlignContent.start,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            contentDemo "Center" AlignContent.center,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            contentDemo "End" AlignContent.end',
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            contentDemo "Space Between" AlignContent.spaceBetween,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            contentDemo "Space Around" AlignContent.spaceAround,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            contentDemo "Stretch" AlignContent.stretch,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

let row alignment =
  div [
    Flex.Flex.allSizes
    FlexWrap.Wrap.allSizes
    alignment
    Gap.All.g1
    Attr.Style "min-height" "160px"
    Attr.Style "max-width" "300px"
  ] [
    for number in 1..8 do
      let color =
        if number % 2 = 0 then
          BrandColor.Fill.secondary
        else
          BrandColor.Fill.primary

      box (string number) color []
  ]

row AlignContent.start
row AlignContent.center
row AlignContent.end'
row AlignContent.spaceBetween
row AlignContent.spaceAround
row AlignContent.stretch"""

    Helpers.codeSampleSection "Align Content" description content code

  // 7. Align Self
  let private alignSelfSection () =
    let description =
      Helpers.bodyText
        "AlignSelf overrides the container's AlignItems for a single item. Place it directly on the item you want to reposition."

    let content =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [
          text "Container: AlignItems.start — individual items override"
        ]
        flexContainer [
          Flex.Flex.allSizes
          AlignItems.start
          Gap.All.g2
          Attr.Style "min-height" "120px"
        ] [
          colorBox "start (default)" BrandColor.Fill.primary [ Attr.Style "padding" "24px 12px" ]
          colorBox "self: center" BrandColor.Fill.secondary [ AlignSelf.center ]
          colorBox "self: end" BrandColor.Fill.info [ AlignSelf.end' ]
          colorBox "self: stretch" BrandColor.Fill.success [ AlignSelf.stretch ]
          colorBox "self: auto" BrandColor.Fill.warning [ AlignSelf.auto ]
        ]
      ]

    let code =
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

div [
  Flex.Flex.allSizes
  AlignItems.start
  Gap.All.g2
  Attr.Style "min-height" "120px"
] [
  box "start (default)" BrandColor.Fill.primary [
    Attr.Style "padding" "24px 12px"
  ]
  box "self: center" BrandColor.Fill.secondary [ AlignSelf.center ]
  box "self: end" BrandColor.Fill.info [ AlignSelf.end' ]
  box "self: stretch" BrandColor.Fill.success [ AlignSelf.stretch ]
  box "self: auto" BrandColor.Fill.warning [ AlignSelf.auto ]
]"""

    Helpers.codeSampleSection "Align Self" description content code

  // 8. Gap
  let private gapSection () =
    let description =
      Helpers.bodyText
        "Gap controls spacing between flex items. Values are multiples of 4px (g1 = 4px, g2 = 8px, g5 = 20px). Use Gap.X and Gap.Y for independent axis control."

    let gapDemo (label: string) (gapAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text label ]
        flexContainer [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; gapAttr ] [
          smallBox "A" BrandColor.Fill.primary
          smallBox "B" BrandColor.Fill.secondary
          smallBox "C" BrandColor.Fill.info
          smallBox "D" BrandColor.Fill.success
        ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            gapDemo "g0 (0px)" Gap.All.g0,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            gapDemo "g2 (8px)" Gap.All.g2,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            gapDemo "g4 (16px)" Gap.All.g4,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            gapDemo "g8 (32px)" Gap.All.g8,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

let row gap =
  div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; gap ] [
    box "A" BrandColor.Fill.primary []
    box "B" BrandColor.Fill.secondary []
    box "C" BrandColor.Fill.info []
    box "D" BrandColor.Fill.success []
  ]

row Gap.All.g0
row Gap.All.g2
row Gap.All.g4
row Gap.All.g8"""

    Helpers.codeSampleSection "Gap" description content code

  // 9. Flex Item
  let private flexItemSection () =
    let description =
      Helpers.bodyText
        "FlexItem controls grow, shrink, and order on individual items within a flex container."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Grow" ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "No grow" BrandColor.Fill.secondary []
          colorBox "Grow" BrandColor.Fill.primary [ FlexItem.Grow.allSizes ]
          colorBox "No grow" BrandColor.Fill.secondary []
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "No Grow" ]
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [
          text "FlexItem.NoGrow prevents an item from growing even if the container has extra space."
        ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "NoGrow" BrandColor.Fill.warning [ FlexItem.NoGrow.allSizes ]
          colorBox "Grow" BrandColor.Fill.primary [ FlexItem.Grow.allSizes ]
          colorBox "Grow" BrandColor.Fill.secondary [ FlexItem.Grow.allSizes ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Shrink vs No Shrink" ]
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [
          text "When items overflow, NoShrink prevents an item from getting smaller."
        ]
        flexContainer [
          Flex.Flex.allSizes
          FlexWrap.NoWrap.allSizes
          Gap.All.g2
          Margin.Bottom.small
          Attr.Style "max-width" "400px"
        ] [
          colorBox "Shrinks" BrandColor.Fill.primary [
            FlexItem.Shrink.allSizes
            Attr.Style "min-width" "120px"
          ]
          colorBox "No Shrink" BrandColor.Fill.error [
            FlexItem.NoShrink.allSizes
            Attr.Style "min-width" "200px"
          ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Flex 1 (equal sizing)" ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "flex-1" BrandColor.Fill.primary [ FlexItem.Flex.allSizes ]
          colorBox "flex-1" BrandColor.Fill.secondary [ FlexItem.Flex.allSizes ]
          colorBox "flex-1" BrandColor.Fill.info [ FlexItem.Flex.allSizes ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Auto, Initial, and None" ]
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [
          text
            "Auto sizes based on content then grows/shrinks. Initial sizes based on content but only shrinks. None ignores flex entirely — uses the item's intrinsic width."
        ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "auto" BrandColor.Fill.primary [ FlexItem.Auto.allSizes ]
          colorBox "initial" BrandColor.Fill.secondary [ FlexItem.Initial.allSizes ]
          colorBox "none" BrandColor.Fill.info [ FlexItem.None.allSizes ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Order" ]
        flexContainer [ Flex.Flex.allSizes; Gap.All.g2; Margin.Bottom.small ] [
          colorBox "order-3" BrandColor.Fill.info [ Order.o3 ]
          colorBox "order-1" BrandColor.Fill.primary [ Order.o1 ]
          colorBox "order-2" BrandColor.Fill.secondary [ Order.o2 ]
        ]

        div [ Typography.body2; Typography.Color.textSecondary ] [
          text
            "The items above are written in DOM order 3, 1, 2 but display as 1, 2, 3. Keep meaningful reading and keyboard order in the DOM; use visual ordering only when it does not change that meaning."
        ]
      ]

    let code =
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

let row (attrs: Attr list) children =
  div [ Flex.Flex.allSizes; Gap.All.g2; yield! attrs ] children

row [] [
  box "No grow" BrandColor.Fill.secondary []
  box "Grow" BrandColor.Fill.primary [ FlexItem.Grow.allSizes ]
  box "No grow" BrandColor.Fill.secondary []
]

row [] [
  box "NoGrow" BrandColor.Fill.warning [ FlexItem.NoGrow.allSizes ]
  box "Grow" BrandColor.Fill.primary [ FlexItem.Grow.allSizes ]
  box "Grow" BrandColor.Fill.secondary [ FlexItem.Grow.allSizes ]
]

row [ FlexWrap.NoWrap.allSizes; Attr.Style "max-width" "400px" ] [
  box "Shrinks" BrandColor.Fill.primary [
    FlexItem.Shrink.allSizes
    Attr.Style "min-width" "120px"
  ]
  box "No Shrink" BrandColor.Fill.error [
    FlexItem.NoShrink.allSizes
    Attr.Style "min-width" "200px"
  ]
]

row [] [
  box "flex-1" BrandColor.Fill.primary [ FlexItem.Flex.allSizes ]
  box "flex-1" BrandColor.Fill.secondary [ FlexItem.Flex.allSizes ]
  box "flex-1" BrandColor.Fill.info [ FlexItem.Flex.allSizes ]
]

row [] [
  box "auto" BrandColor.Fill.primary [ FlexItem.Auto.allSizes ]
  box "initial" BrandColor.Fill.secondary [ FlexItem.Initial.allSizes ]
  box "none" BrandColor.Fill.info [ FlexItem.None.allSizes ]
]

row [] [
  box "order-3" BrandColor.Fill.info [ Order.o3 ]
  box "order-1" BrandColor.Fill.primary [ Order.o1 ]
  box "order-2" BrandColor.Fill.secondary [ Order.o2 ]
]"""

    Helpers.codeSampleSection "Flex Item" description content code

  // 10. Grow Children
  let private growChildrenSection () =
    let description =
      Helpers.bodyText
        "GrowChildren is a container-level helper that applies flex-grow to specific child positions. Place it on the flex container — it uses CSS child selectors so you don't need to add attrs to individual items."

    let growDemo (label: string) (detail: string) (growAttr: Attr) =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text label ]
        div [ Typography.caption; Margin.Bottom.extraSmall; Typography.Color.textSecondary ] [ text detail ]
        flexContainer [ Flex.Flex.allSizes; growAttr; Gap.All.g2 ] [
          smallBox "A" BrandColor.Fill.primary
          smallBox "B" BrandColor.Fill.secondary
          smallBox "C" BrandColor.Fill.info
          smallBox "D" BrandColor.Fill.success
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
      """let box label (color: Attr) (attrs: Attr list) =
  div [
    color
    Typography.subtitle2
    Padding.Vertical.extraSmall
    Padding.Horizontal.small
    BorderRadius.All.small
    Attr.Style "text-align" "center"
    Attr.Style "min-width" "60px"
    yield! attrs
  ] [ text label ]

let row grow =
  div [ Flex.Flex.allSizes; grow; Gap.All.g2 ] [
    box "A" BrandColor.Fill.primary []
    box "B" BrandColor.Fill.secondary []
    box "C" BrandColor.Fill.info []
    box "D" BrandColor.Fill.success []
  ]

row FlexItem.GrowChildren.Start.allSizes
row FlexItem.GrowChildren.End.allSizes
row FlexItem.GrowChildren.StartAndEnd.allSizes
row FlexItem.GrowChildren.Middle.allSizes
row FlexItem.GrowChildren.All.allSizes"""

    Helpers.codeSampleSection "Grow Children" description content code

  // Render
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
