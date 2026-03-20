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

  let private inlineCode (value: string) =
    span [ Typography.caption; Typography.Color.primary ] [ text value ]

  let private tableCell (children: Doc list) =
    td [ Attr.Style "padding" "8px 12px"; Attr.Style "white-space" "nowrap" ] children

  let private tableHeaderCell (label: string) =
    th [
      Attr.Style "text-align" "left"
      Attr.Style "padding" "8px 12px"
      Attr.Style "white-space" "nowrap"
      Attr.Style "border-bottom" "1px solid var(--palette-divider)"
    ] [ text label ]

  // ---------------------------------------------------------------------------
  // 1. How it works
  // ---------------------------------------------------------------------------

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

    let code =
      """open Weave


// Compose width + style + color for a complete border
div [
    BorderWidth.All.one
    Attr.Style "border-style" "solid"
    BorderColor.primary
    BorderRadius.All.small
    Padding.All.small
] [
    text "1px solid primary border with rounded corners"
]

// Directional borders
div [
    BorderWidth.Bottom.two
    Attr.Style "border-style" "solid"
    BorderColor.info
] [
    text "2px bottom border only"
]

// Mix and match any combination
div [
    BorderWidth.All.four
    Attr.Style "border-style" "dashed"
    BorderColor.warning
    BorderRadius.All.large
] [
    text "4px dashed warning border"
]"""

    Helpers.codeSampleSection "How it works" description content code

  // ---------------------------------------------------------------------------
  // 2. Playground — interactive border configurator
  // ---------------------------------------------------------------------------

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
                variant = Select.Variant.Outlined,
                labelText = View.Const "Width",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              Select.create (
                styleItems,
                selectedStyle,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Style",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              Select.create (
                colorItems,
                selectedColor,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Color",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              Select.create (
                radiusItems,
                selectedRadius,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Radius",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
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
              SurfaceColor.toBackgroundColor SurfaceColor.Background
              Padding.All.medium
              Flex.Flex.allSizes
              AlignItems.center
              JustifyContent.center
              Attr.Style "min-height" "120px"
            ] [
              div [ Typography.h5; Attr.Style "opacity" "0.8" ] [
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

    let code =
      """open Weave


// Compose all four axes for a complete border
div [
    BorderWidth.All.two
    Attr.Style "border-style" "solid"
    BorderColor.primary
    BorderRadius.All.medium
    Padding.All.medium
] [
    text "2px solid primary with medium radius"
]

// Each axis is independent — omit any to use defaults
div [
    BorderWidth.All.four
    Attr.Style "border-style" "dashed"
    BorderColor.warning
] [
    text "4px dashed warning"
]"""

    Helpers.codeSampleSection "Playground" description content code

  // ---------------------------------------------------------------------------
  // 3. Border Radius
  // ---------------------------------------------------------------------------

  let private borderRadiusSection () =
    let description =
      Helpers.bodyText
        "Border radius controls corner rounding. Apply to all corners or target specific sides and individual corners. Special values include pill (capsule shape) and circle."

    let radiusDemo (label: string) (fsharpLabel: string) (radiusAttr: Attr) =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
        div [
          BrandColor.toBackgroundColor BrandColor.Primary
          radiusAttr
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "width" "80px"
          Attr.Style "height" "80px"
        ] [ div [ Typography.subtitle2 ] [ text label ] ]
        div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [ text fsharpLabel ]
      ]

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "All corners" ]

        Grid.create (
          [
            GridItem.create (
              radiusDemo "none" "All.none" BorderRadius.All.none,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              radiusDemo "sm" "All.small" BorderRadius.All.small,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              radiusDemo "md" "All.medium" BorderRadius.All.medium,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              radiusDemo "lg" "All.large" BorderRadius.All.large,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
          ],
          spacing = Grid.GutterSpacing.create 2
        )

        div [ Typography.subtitle2; Margin.Top.small; Margin.Bottom.extraSmall ] [ text "Special shapes" ]

        Grid.create (
          [
            GridItem.create (
              radiusDemo "pill" "pill" BorderRadius.pill,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              radiusDemo "circle" "circle" BorderRadius.circle,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
          ],
          spacing = Grid.GutterSpacing.create 2
        )

        div [ Typography.subtitle2; Margin.Top.small; Margin.Bottom.extraSmall ] [ text "Directional" ]

        Grid.create (
          [
            GridItem.create (
              radiusDemo "top" "Top.large" BorderRadius.Top.large,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              radiusDemo "bottom" "Bottom.large" BorderRadius.Bottom.large,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              radiusDemo "left" "Left.large" BorderRadius.Left.large,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
            GridItem.create (
              radiusDemo "right" "Right.large" BorderRadius.Right.large,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 3
            )
          ],
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave


// All corners
div [ BorderRadius.All.small ] [ text "4px corners" ]
div [ BorderRadius.All.large ] [ text "12px corners" ]

// Special shapes
div [ BorderRadius.pill ]   [ text "Capsule shape" ]
div [ BorderRadius.circle ] [ text "Circle" ]

// Directional: round only specific sides
div [ BorderRadius.Top.large ] [ text "Rounded top only" ]

// Individual corners
div [ BorderRadius.TopLeft.large; BorderRadius.BottomRight.large ] [
    text "Diagonal corners rounded"
]"""

    Helpers.codeSampleSection "Border Radius" description content code

  // ---------------------------------------------------------------------------
  // 4. Border Width
  // ---------------------------------------------------------------------------

  let private borderWidthSection () =
    let description =
      Helpers.bodyText
        "Border width controls thickness. Apply to all sides or target specific edges. All examples use a solid style and a color for visibility."

    let widthDemo (label: string) (fsharpLabel: string) (widthAttr: Attr) =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
        div [
          widthAttr
          Attr.Style "border-style" "solid"
          BorderColor.primary
          BorderRadius.All.small
          SurfaceColor.toBackgroundColor SurfaceColor.Background
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "min-height" "64px"
          Attr.Style "width" "100%"
        ] [ div [ Typography.subtitle2 ] [ text label ] ]
        div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [ text fsharpLabel ]
      ]

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "All sides" ]

        Grid.create (
          [
            GridItem.create (
              widthDemo "0px" "All.zero" BorderWidth.All.zero,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4,
              md = Grid.Width.create 2
            )
            GridItem.create (
              widthDemo "1px" "All.one" BorderWidth.All.one,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4,
              md = Grid.Width.create 2
            )
            GridItem.create (
              widthDemo "2px" "All.two" BorderWidth.All.two,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4,
              md = Grid.Width.create 2
            )
            GridItem.create (
              widthDemo "4px" "All.four" BorderWidth.All.four,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4,
              md = Grid.Width.create 2
            )
            GridItem.create (
              widthDemo "8px" "All.eight" BorderWidth.All.eight,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4,
              md = Grid.Width.create 2
            )
          ],
          spacing = Grid.GutterSpacing.create 2
        )

        div [ Typography.subtitle2; Margin.Top.small; Margin.Bottom.extraSmall ] [ text "Directional" ]

        Grid.create (
          [
            GridItem.create (
              widthDemo "top" "Top.two" BorderWidth.Top.two,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              widthDemo "bottom" "Bottom.two" BorderWidth.Bottom.two,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              widthDemo "left" "Left.two" BorderWidth.Left.two,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              widthDemo "right" "Right.two" BorderWidth.Right.two,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              widthDemo "horiz" "Horizontal.two" BorderWidth.Horizontal.two,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.create (
              widthDemo "vert" "Vertical.two" BorderWidth.Vertical.two,
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
          ],
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave


// All sides
div [
    BorderWidth.All.two
    Attr.Style "border-style" "solid"
    BorderColor.primary
] [
    text "2px border on all sides"
]

// Directional: bottom border only
div [
    BorderWidth.Bottom.four
    Attr.Style "border-style" "solid"
    BorderColor.error
] [
    text "4px bottom border"
]

// Combine directions: horizontal only
div [
    BorderWidth.Horizontal.one
    Attr.Style "border-style" "solid"
    BorderColor.info
] [
    text "1px left and right"
]"""

    Helpers.codeSampleSection "Border Width" description content code

  // ---------------------------------------------------------------------------
  // 5. Border Style
  // ---------------------------------------------------------------------------

  let private borderStyleSection () =
    let description =
      Helpers.bodyText
        "Border style controls the line pattern. Combine with a width and color to see the effect."

    let styleDemo (label: string) (styleAttr: Attr) =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
        div [
          BorderWidth.All.two
          styleAttr
          BorderColor.primary
          BorderRadius.All.small
          SurfaceColor.toBackgroundColor SurfaceColor.Background
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "min-height" "64px"
          Attr.Style "width" "100%"
        ] [ div [ Typography.subtitle2 ] [ text label ] ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            styleDemo "Solid" (Attr.Style "border-style" "solid"),
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 2
          )
          GridItem.create (
            styleDemo "Dashed" (Attr.Style "border-style" "dashed"),
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 2
          )
          GridItem.create (
            styleDemo "Dotted" (Attr.Style "border-style" "dotted"),
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 2
          )
          GridItem.create (
            styleDemo "Double" (Attr.Style "border-style" "double"),
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 2
          )
          GridItem.create (
            styleDemo "Hidden" (Attr.Style "border-style" "hidden"),
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 2
          )
          GridItem.create (
            styleDemo "None" (Attr.Style "border-style" "none"),
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 2
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


// Each style applies the border-style CSS property
div [ BorderWidth.All.two; Attr.Style "border-style" "solid"; BorderColor.primary ] [
    text "Solid"
]

div [ BorderWidth.All.two; Attr.Style "border-style" "dashed"; BorderColor.info ] [
    text "Dashed"
]

div [ BorderWidth.All.two; Attr.Style "border-style" "dotted"; BorderColor.warning ] [
    text "Dotted"
]

// Double requires a thicker width to be visible
div [ BorderWidth.All.four; Attr.Style "border-style" "double"; BorderColor.error ] [
    text "Double"
]"""

    Helpers.codeSampleSection "Border Style" description content code

  // ---------------------------------------------------------------------------
  // 6. Border Color
  // ---------------------------------------------------------------------------

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
        SurfaceColor.toBackgroundColor SurfaceColor.Background
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
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
          GridItem.create (
            colorDemo "Secondary" BorderColor.secondary,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
          GridItem.create (
            colorDemo "Tertiary" BorderColor.tertiary,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
          GridItem.create (
            colorDemo "Info" BorderColor.info,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
          GridItem.create (
            colorDemo "Success" BorderColor.success,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
          GridItem.create (
            colorDemo "Warning" BorderColor.warning,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
          GridItem.create (
            colorDemo "Error" BorderColor.error,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
          GridItem.create (
            colorDemo "Lines Default" BorderColor.linesDefault,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 4,
            md = Grid.Width.create 3
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


// Palette colors
div [ BorderWidth.All.two; Attr.Style "border-style" "solid"; BorderColor.primary ] [
    text "Primary border"
]

div [ BorderWidth.All.two; Attr.Style "border-style" "solid"; BorderColor.error ] [
    text "Error border"
]

// Lines default — the standard divider color
div [ BorderWidth.All.one; Attr.Style "border-style" "solid"; BorderColor.linesDefault ] [
    text "Subtle border matching dividers"
]"""

    Helpers.codeSampleSection "Border Color" description content code

  // ---------------------------------------------------------------------------
  // 7. Borders on Components
  // ---------------------------------------------------------------------------

  let private bordersOnComponentsSection () =
    let description =
      Helpers.bodyText
        "Border utilities compose with existing Weave components via attrs. Here are some common patterns."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Buttons" ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Attr.Style "gap" "12px"
          Margin.Bottom.small
        ] [
          Button.primary (
            text "Dashed border",
            onClick = ignore,
            attrs = [
              Button.Variant.outlined
              Attr.Style "border-style" "dashed"
              BorderWidth.All.two
            ]
          )
          Button.error (
            text "Dotted border",
            onClick = ignore,
            attrs = [
              Button.Variant.outlined
              Attr.Style "border-style" "dotted"
              BorderWidth.All.two
            ]
          )
          Button.success (
            text "Thick border",
            onClick = ignore,
            attrs = [ Button.Variant.outlined; BorderWidth.All.four ]
          )
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Alerts" ]

        div [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          Attr.Style "gap" "12px"
          Margin.Bottom.small
        ] [
          Alert.create (
            text "Info alert with a left accent border.",
            attrs = [
              Alert.Color.info
              Alert.Variant.outlined
              BorderWidth.Left.four
              Attr.Style "border-style" "solid"
              BorderColor.info
            ]
          )
          Alert.create (
            text "Warning alert with a dashed border.",
            attrs = [
              Alert.Color.warning
              Alert.Variant.outlined
              Attr.Style "border-style" "dashed"
              BorderWidth.All.two
            ]
          )
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Chips" ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Attr.Style "gap" "8px"
          Margin.Bottom.small
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

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Cards / Surfaces" ]

        Grid.create (
          [
            GridItem.create (
              div [
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
                BorderWidth.All.one
                Attr.Style "border-style" "solid"
                BorderColor.linesDefault
                BorderRadius.All.medium
                Padding.All.small
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Subtle card" ]
                div [ Typography.body2; Attr.Style "opacity" "0.7" ] [
                  text "A surface with a thin border instead of a shadow."
                ]
              ],
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6
            )
            GridItem.create (
              div [
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
                BorderWidth.Left.four
                Attr.Style "border-style" "solid"
                BorderColor.primary
                BorderRadius.All.small
                Padding.All.small
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Accent card" ]
                div [ Typography.body2; Attr.Style "opacity" "0.7" ] [
                  text "A left accent border draws attention to key content."
                ]
              ],
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6
            )
          ],
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave


// Dashed outlined button
Button.primary(
    text "Dashed border",
    onClick = ignore,
    attrs = [
        Button.Variant.outlined
        Attr.Style "border-style" "dashed"
        BorderWidth.All.two
    ]
)

// Alert with left accent border
Alert.create(
    text "Info with accent border",
    attrs = [
        Alert.Color.info
        Alert.Variant.outlined
        BorderWidth.Left.four
        Attr.Style "border-style" "solid"
        BorderColor.info
    ]
)

// Card with subtle border instead of shadow
div [
    SurfaceColor.toBackgroundColor SurfaceColor.Surface
    BorderWidth.All.one
    Attr.Style "border-style" "solid"
    BorderColor.linesDefault
    BorderRadius.All.medium
    Padding.All.small
] [
    text "Bordered card content"
]"""

    Helpers.codeSampleSection "Borders on Components" description content code

  // ---------------------------------------------------------------------------
  // Render
  // ---------------------------------------------------------------------------

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Borders"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Control border width, style, color, and corner radius with composable utility classes. Mix and match all four axes on any element or component."
        ]

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        playgroundSection ()
        Helpers.divider ()
        borderRadiusSection ()
        Helpers.divider ()
        borderWidthSection ()
        Helpers.divider ()
        borderStyleSection ()
        Helpers.divider ()
        borderColorSection ()
        Helpers.divider ()
        bordersOnComponentsSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
