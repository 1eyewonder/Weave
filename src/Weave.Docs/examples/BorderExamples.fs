namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module BorderExamples =

  let private inlineCode = Helpers.inlineCode

  let private tableCell (children: Doc list) =
    td [ Attr.Style "padding" "8px 12px"; Attr.Style "overflow-wrap" "anywhere" ] children

  let private tableHeaderCell (label: string) =
    th [
      Attr.Style "text-align" "start"
      Attr.Create "scope" "col"
      Attr.Style "padding" "8px 12px"
      Attr.Style "overflow-wrap" "anywhere"
      Attr.Style "border-bottom" "1px solid var(--palette-divider)"
    ] [ text label ]

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Borders are composed from three independent axes: width (how thick), style (solid, dashed, etc.), and radius (corner rounding). Width, color, and radius are F# modules you compose in attrs. Border style is set with a plain Attr.Style call — keeping it close to CSS without an extra indirection."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Border Width" ]

        div [ Margin.Bottom.small; Attr.Style "overflow-x" "auto" ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [
              tr [] [ tableHeaderCell "Size"; tableHeaderCell "Pixels"; tableHeaderCell "F# value" ]
            ]
            tbody [] [
              tr [] [
                tableCell [ text "Zero" ]
                tableCell [ text "0px" ]
                tableCell [ inlineCode "BorderWidth.All.zero" ]
              ]
              tr [] [
                tableCell [ text "One" ]
                tableCell [ text "1px" ]
                tableCell [ inlineCode "BorderWidth.All.one" ]
              ]
              tr [] [
                tableCell [ text "Two" ]
                tableCell [ text "2px" ]
                tableCell [ inlineCode "BorderWidth.All.two" ]
              ]
              tr [] [
                tableCell [ text "Four" ]
                tableCell [ text "4px" ]
                tableCell [ inlineCode "BorderWidth.All.four" ]
              ]
              tr [] [
                tableCell [ text "Eight" ]
                tableCell [ text "8px" ]
                tableCell [ inlineCode "BorderWidth.All.eight" ]
              ]
            ]
          ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Border Style" ]

        div [ Margin.Bottom.small; Attr.Style "overflow-x" "auto" ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [ tr [] [ tableHeaderCell "Style"; tableHeaderCell "F# usage" ] ]
            tbody [] [
              tr [] [
                tableCell [ text "Solid" ]
                tableCell [ inlineCode "Attr.Style \"border-style\" \"solid\"" ]
              ]
              tr [] [
                tableCell [ text "Dashed" ]
                tableCell [ inlineCode "Attr.Style \"border-style\" \"dashed\"" ]
              ]
              tr [] [
                tableCell [ text "Dotted" ]
                tableCell [ inlineCode "Attr.Style \"border-style\" \"dotted\"" ]
              ]
              tr [] [
                tableCell [ text "Double" ]
                tableCell [ inlineCode "Attr.Style \"border-style\" \"double\"" ]
              ]
              tr [] [
                tableCell [ text "Hidden" ]
                tableCell [ inlineCode "Attr.Style \"border-style\" \"hidden\"" ]
              ]
              tr [] [
                tableCell [ text "None" ]
                tableCell [ inlineCode "Attr.Style \"border-style\" \"none\"" ]
              ]
            ]
          ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Border Width Directions" ]

        div [ Margin.Bottom.small; Attr.Style "overflow-x" "auto" ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [
              tr [] [
                tableHeaderCell "Direction"
                tableHeaderCell "Applies to"
                tableHeaderCell "F# helper"
              ]
            ]
            tbody [] [
              tr [] [
                tableCell [ text "All" ]
                tableCell [ text "All four sides" ]
                tableCell [ inlineCode "BorderWidth.All.{size}" ]
              ]
              tr [] [
                tableCell [ text "Top" ]
                tableCell [ text "Top edge" ]
                tableCell [ inlineCode "BorderWidth.Top.{size}" ]
              ]
              tr [] [
                tableCell [ text "Right" ]
                tableCell [ text "Right edge" ]
                tableCell [ inlineCode "BorderWidth.Right.{size}" ]
              ]
              tr [] [
                tableCell [ text "Bottom" ]
                tableCell [ text "Bottom edge" ]
                tableCell [ inlineCode "BorderWidth.Bottom.{size}" ]
              ]
              tr [] [
                tableCell [ text "Left" ]
                tableCell [ text "Left edge" ]
                tableCell [ inlineCode "BorderWidth.Left.{size}" ]
              ]
              tr [] [
                tableCell [ text "Horizontal" ]
                tableCell [ text "Left and right" ]
                tableCell [ inlineCode "BorderWidth.Horizontal.{size}" ]
              ]
              tr [] [
                tableCell [ text "Vertical" ]
                tableCell [ text "Top and bottom" ]
                tableCell [ inlineCode "BorderWidth.Vertical.{size}" ]
              ]
            ]
          ]
        ]
      ]

    Helpers.section "How it works" description content

  // 2. Playground — interactive border configurator
  let private playgroundSection () =
    let description =
      Helpers.bodyText
        "Pick a border width, style, color, and radius to preview the effect. The preview updates live as you change the selections."

    let allWidths = [
      "0px", BorderWidth.All.zero
      "1px", BorderWidth.All.one
      "2px", BorderWidth.All.two
      "4px", BorderWidth.All.four
      "8px", BorderWidth.All.eight
    ]

    let allStyles = [
      "Solid", Attr.Style "border-style" "solid"
      "Dashed", Attr.Style "border-style" "dashed"
      "Dotted", Attr.Style "border-style" "dotted"
      "Double", Attr.Style "border-style" "double"
      "Hidden", Attr.Style "border-style" "hidden"
      "None", Attr.Style "border-style" "none"
    ]

    let allColors = [
      "Primary", BorderColor.primary
      "Secondary", BorderColor.secondary
      "Tertiary", BorderColor.tertiary
      "Info", BorderColor.info
      "Success", BorderColor.success
      "Warning", BorderColor.warning
      "Error", BorderColor.error
      "Lines Default", BorderColor.linesDefault
    ]

    let allRadii = [
      "None (0px)", BorderRadius.All.none
      "Small (4px)", BorderRadius.All.small
      "Medium (8px)", BorderRadius.All.medium
      "Large (12px)", BorderRadius.All.large
      "Pill", BorderRadius.pill
      "Circle", BorderRadius.circle
    ]

    let selectedWidth = Var.Create<string option>(Some "2px")
    let selectedStyle = Var.Create<string option>(Some "Solid")
    let selectedColor = Var.Create<string option>(Some "Primary")
    let selectedRadius = Var.Create<string option>(Some "Medium (8px)")

    let widthItems =
      allWidths
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let styleItems =
      allStyles
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let colorItems =
      allColors
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let radiusItems =
      allRadii
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let content =
      div [] [
        Grid.create (
          [
            GridItem.create (
              Select.create (
                widthItems,
                selectedWidth,
                labelText = View.Const "Width",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
            )
            GridItem.create (
              Select.create (
                styleItems,
                selectedStyle,
                labelText = View.Const "Style",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
            )
            GridItem.create (
              Select.create (
                colorItems,
                selectedColor,
                labelText = View.Const "Color",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
            )
            GridItem.create (
              Select.create (
                radiusItems,
                selectedRadius,
                labelText = View.Const "Radius",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
            )
          ]
        )

        View.Map3 (fun w s c -> w, s, c) selectedWidth.View selectedStyle.View selectedColor.View
        |> View.Map2 (fun r (w, s, c) -> w, s, c, r) selectedRadius.View
        |> Doc.BindView(fun (wSel, sSel, cSel, rSel) ->
          let widthAttr =
            wSel
            |> Option.bind (fun label -> allWidths |> List.tryFind (fun (l, _) -> l = label))
            |> Option.map snd
            |> Option.defaultValue Attr.Empty

          let styleAttr =
            sSel
            |> Option.bind (fun label -> allStyles |> List.tryFind (fun (l, _) -> l = label))
            |> Option.map snd
            |> Option.defaultValue Attr.Empty

          let colorAttr =
            cSel
            |> Option.bind (fun label -> allColors |> List.tryFind (fun (l, _) -> l = label))
            |> Option.map snd
            |> Option.defaultValue Attr.Empty

          let radiusAttr =
            rSel
            |> Option.bind (fun label -> allRadii |> List.tryFind (fun (l, _) -> l = label))
            |> Option.map snd
            |> Option.defaultValue Attr.Empty

          div [ Margin.Top.small ] [
            div [
              widthAttr
              styleAttr
              colorAttr
              radiusAttr
              SurfaceColor.BackgroundColor.background
              Padding.All.medium
              Flex.Flex.allSizes
              AlignItems.center
              JustifyContent.center
              Attr.Style "min-height" "120px"
            ] [
              div [ Typography.h5; Typography.Color.textSecondary ] [
                text (
                  sprintf
                    "%s %s %s"
                    (wSel |> Option.defaultValue "")
                    (sSel |> Option.map (fun s -> s.ToLower()) |> Option.defaultValue "")
                    (cSel |> Option.map (fun s -> s.ToLower()) |> Option.defaultValue "")
                )
              ]
            ]
          ])
      ]

    Helpers.section "Playground" description content

  // 3. Border Radius
  let private borderRadiusSection () =
    let description =
      Helpers.bodyText
        "Border radius controls corner rounding. Apply to all corners or target specific sides and individual corners. Special values include pill (capsule shape) and circle."

    let samples = [
      "None", [ BorderRadius.All.none ]
      "Small", [ BorderRadius.All.small ]
      "Medium", [ BorderRadius.All.medium ]
      "Large", [ BorderRadius.All.large ]
      "Pill", [ BorderRadius.pill; Attr.Style "width" "120px" ]
      "Circle", [ BorderRadius.circle ]
      "Top.large", [ BorderRadius.Top.large ]
      "Bottom.large", [ BorderRadius.Bottom.large ]
      "Left.large", [ BorderRadius.Left.large ]
      "Right.large", [ BorderRadius.Right.large ]
    ]

    let content =
      Grid.create [
        for label, radius in samples do
          GridItem.create (
            div [
              Flex.Flex.allSizes
              FlexDirection.Column.allSizes
              AlignItems.center
              Gap.All.g2
            ] [
              div [
                BrandColor.Fill.primary
                Attr.Style "width" "80px"
                Attr.Style "height" "80px"
                yield! radius
              ] []
              div [ Typography.body2; Typography.Align.center ] [ text label ]
            ],
            attrs = [
              GridItem.Span.six
              GridItem.Span.Small.four
              GridItem.Span.Medium.three
              Flex.Flex.allSizes
              JustifyContent.center
            ]
          )
      ]

    let code =
      """div [
  BorderRadius.All.none
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.All.small
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.All.medium
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.All.large
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.pill
  BrandColor.Fill.primary
  Attr.Style "width" "120px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.circle
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.Top.large
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.Bottom.large
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.Left.large
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []

div [
  BorderRadius.Right.large
  BrandColor.Fill.primary
  Attr.Style "width" "80px"
  Attr.Style "height" "80px"
] []"""

    Helpers.codeSampleSection "Border Radius" description content code

  let private borderWidthSection () =
    let description =
      Helpers.bodyText
        "Border width controls thickness. Apply to all sides or target specific edges. All examples use a solid style and a color for visibility."

    let samples = [
      "All.zero", BorderWidth.All.zero
      "All.one", BorderWidth.All.one
      "All.two", BorderWidth.All.two
      "All.four", BorderWidth.All.four
      "All.eight", BorderWidth.All.eight
      "Top.two", BorderWidth.Top.two
      "Bottom.two", BorderWidth.Bottom.two
      "Left.two", BorderWidth.Left.two
      "Right.two", BorderWidth.Right.two
      "Horizontal.two", BorderWidth.Horizontal.two
      "Vertical.two", BorderWidth.Vertical.two
    ]

    let content =
      Grid.create [
        for label, width in samples do
          GridItem.create (
            div [
              width
              Attr.Style "border-style" "solid"
              BorderColor.primary
              SurfaceColor.BackgroundColor.background
              Typography.Align.center
              Padding.All.small
            ] [ text label ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
      ]

    let code =
      """div [
  BorderWidth.All.zero
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "All.zero" ]

div [
  BorderWidth.All.one
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "All.one" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "All.two" ]

div [
  BorderWidth.All.four
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "All.four" ]

div [
  BorderWidth.All.eight
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "All.eight" ]

div [
  BorderWidth.Top.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "Top.two" ]

div [
  BorderWidth.Bottom.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "Bottom.two" ]

div [
  BorderWidth.Left.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "Left.two" ]

div [
  BorderWidth.Right.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "Right.two" ]

div [
  BorderWidth.Horizontal.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "Horizontal.two" ]

div [
  BorderWidth.Vertical.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "Vertical.two" ]"""

    Helpers.codeSampleSection "Border Width" description content code

  let private borderStyleSection () =
    let description =
      Helpers.bodyText
        "Border style controls the line pattern. Combine with a width and color to see the effect."

    let styles = [ "solid"; "dashed"; "dotted"; "double"; "hidden"; "none" ]

    let content =
      Grid.create [
        for style in styles do
          GridItem.create (
            div [
              BorderWidth.All.four
              Attr.Style "border-style" style
              BorderColor.primary
              SurfaceColor.BackgroundColor.background
              Typography.Align.center
              Padding.All.small
            ] [ text style ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
      ]

    let code =
      """div [
  BorderWidth.All.four
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "solid" ]

div [
  BorderWidth.All.four
  Attr.Style "border-style" "dashed"
  BorderColor.primary
  Padding.All.small
] [ text "dashed" ]

div [
  BorderWidth.All.four
  Attr.Style "border-style" "dotted"
  BorderColor.primary
  Padding.All.small
] [ text "dotted" ]

div [
  BorderWidth.All.four
  Attr.Style "border-style" "double"
  BorderColor.primary
  Padding.All.small
] [ text "double" ]

div [
  BorderWidth.All.four
  Attr.Style "border-style" "hidden"
  BorderColor.primary
  Padding.All.small
] [ text "hidden" ]

div [
  BorderWidth.All.four
  Attr.Style "border-style" "none"
  BorderColor.primary
  Padding.All.small
] [ text "none" ]"""

    Helpers.codeSampleSection "Border Style" description content code

  let private borderColorSection () =
    let description =
      Helpers.bodyText
        "Border colors map to the palette. Each color sets border-color to the corresponding CSS custom property."

    let colorDemo (label: string) (colorAttr: Attr) =
      div [
        BorderWidth.All.two
        Attr.Style "border-style" "solid"
        colorAttr
        BorderRadius.All.small
        SurfaceColor.BackgroundColor.background
        Padding.All.small
        Flex.Flex.allSizes
        AlignItems.center
        JustifyContent.center
        Attr.Style "min-height" "48px"
      ] [ div [ Typography.subtitle2 ] [ text label ] ]

    let content =
      Grid.create (
        [
          GridItem.create (
            colorDemo "Primary" BorderColor.primary,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            colorDemo "Secondary" BorderColor.secondary,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            colorDemo "Tertiary" BorderColor.tertiary,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            colorDemo "Info" BorderColor.info,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            colorDemo "Success" BorderColor.success,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            colorDemo "Warning" BorderColor.warning,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            colorDemo "Error" BorderColor.error,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            colorDemo "Lines Default" BorderColor.linesDefault,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
        ]
      )

    let code =
      """div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.primary
  Padding.All.small
] [ text "Primary" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.secondary
  Padding.All.small
] [ text "Secondary" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.tertiary
  Padding.All.small
] [ text "Tertiary" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.info
  Padding.All.small
] [ text "Info" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.success
  Padding.All.small
] [ text "Success" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.warning
  Padding.All.small
] [ text "Warning" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.error
  Padding.All.small
] [ text "Error" ]

div [
  BorderWidth.All.two
  Attr.Style "border-style" "solid"
  BorderColor.linesDefault
  Padding.All.small
] [ text "Lines default" ]"""

    Helpers.codeSampleSection "Border Color" description content code

  // 7. Borders on Components
  let private bordersOnComponentsSection () =
    let buttons =
      div [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        JustifyContent.spaceEvenly
        Gap.All.g3
      ] [
        Button.primary (
          text "Dashed border",
          onClick = ignore,
          attrs = [ Button.Variant.outlined; Attr.Style "border-style" "dashed" ]
        )
        Button.error (
          text "Dotted border",
          onClick = ignore,
          attrs = [ Button.Variant.outlined; Attr.Style "border-style" "dotted" ]
        )
        Button.success (
          text "Thick border",
          onClick = ignore,
          attrs = [ Button.Variant.outlined; BorderWidth.All.four ]
        )
      ]

    let buttonsSource =
      """Button.primary (
  text "Dashed border",
  onClick = ignore,
  attrs = [ Button.Variant.outlined; Attr.Style "border-style" "dashed" ]
)

Button.error (
  text "Dotted border",
  onClick = ignore,
  attrs = [ Button.Variant.outlined; Attr.Style "border-style" "dotted" ]
)

Button.success (
  text "Thick border",
  onClick = ignore,
  attrs = [ Button.Variant.outlined; BorderWidth.All.four ]
)"""

    let alerts =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        Alert.create (
          text "Info alert with a left accent border.",
          attrs = [
            Alert.Color.info
            Alert.Variant.outlined
            BorderWidth.Left.four
            BorderColor.info
          ]
        )
        Alert.create (
          text "Warning alert with a dashed border.",
          attrs = [
            Alert.Color.warning
            Alert.Variant.outlined
            Attr.Style "border-style" "dashed"
          ]
        )
      ]

    let alertsSource =
      """Alert.create (
  text "Info alert with a left accent border.",
  attrs = [
    Alert.Color.info
    Alert.Variant.outlined
    BorderWidth.Left.four
    BorderColor.info
  ]
)

Alert.create (
  text "Warning alert with a dashed border.",
  attrs = [
    Alert.Color.warning
    Alert.Variant.outlined
    Attr.Style "border-style" "dashed"
  ]
)"""

    let chips =
      div [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        JustifyContent.spaceEvenly
        Gap.All.g2
      ] [
        Chip.create (
          text "Dashed chip",
          attrs = [
            Chip.Variant.outlined
            Chip.Color.primary
            Attr.Style "border-style" "dashed"
          ]
        )
        Chip.create (
          text "Dotted chip",
          attrs = [
            Chip.Variant.outlined
            Chip.Color.secondary
            Attr.Style "border-style" "dotted"
          ]
        )
        Chip.create (
          text "Thick chip",
          attrs = [ Chip.Variant.outlined; Chip.Color.error; BorderWidth.All.two ]
        )
      ]

    let chipsSource =
      """Chip.create (
  text "Dashed chip",
  attrs = [
    Chip.Variant.outlined
    Chip.Color.primary
    Attr.Style "border-style" "dashed"
  ]
)

Chip.create (
  text "Dotted chip",
  attrs = [
    Chip.Variant.outlined
    Chip.Color.secondary
    Attr.Style "border-style" "dotted"
  ]
)

Chip.create (
  text "Thick chip",
  attrs = [ Chip.Variant.outlined; Chip.Color.error; BorderWidth.All.two ]
)"""

    let cards =
      Grid.create [
        GridItem.create (
          Card.create (CardContent.create (text "Subtle card"), attrs = [ BorderColor.linesDefault ]),
          attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
        )
        GridItem.create (
          Card.create (
            CardContent.create (text "Dashed card"),
            attrs = [ BorderColor.primary; Attr.Style "border-style" "dashed" ]
          ),
          attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
        )
      ]

    let cardsSource =
      """Card.create (
  CardContent.create (text "Subtle card"),
  attrs = [ BorderColor.linesDefault ]
)

Card.create (
  CardContent.create (text "Dashed card"),
  attrs = [ BorderColor.primary; Attr.Style "border-style" "dashed" ]
)"""

    div [] [
      Helpers.sectionHeader "Borders on Components"

      Helpers.codeSampleSection
        "Borders on buttons"
        (Helpers.bodyText "Outlined buttons keep their variant while border style changes.")
        buttons
        buttonsSource

      Helpers.codeSampleSection
        "Borders on alerts"
        (Helpers.bodyText "Compose border utilities with outlined alerts.")
        alerts
        alertsSource

      Helpers.codeSampleSection
        "Borders on chips"
        (Helpers.bodyText "Outlined chips can use the same border utilities.")
        chips
        chipsSource

      Helpers.codeSampleSection
        "Borders on cards"
        (Helpers.bodyText
          "Apply border attributes to a real Card. Its layout and content padding stay with the component.")
        cards
        cardsSource

    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Borders"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Control border width, style, color, and corner radius with composable utility classes. Mix and match all four axes on any element or component."
        ]

        Helpers.divider ()
        borderWidthSection ()
        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        playgroundSection ()
        Helpers.divider ()
        borderRadiusSection ()
        Helpers.divider ()
        borderStyleSection ()
        Helpers.divider ()
        borderColorSection ()
        Helpers.divider ()
        bordersOnComponentsSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
