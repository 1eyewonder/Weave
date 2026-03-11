namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.CssHelpers

[<JavaScript>]
module SpacingExamples =

  let private inlineCode (value: string) =
    Caption.Span(value, attrs = [ Typography.Color.toClass BrandColor.Primary |> cl ])

  let private tableCell (children: Doc list) =
    td [ Attr.Style "padding" "8px 12px"; Attr.Style "white-space" "nowrap" ] children

  let private tableHeaderCell (label: string) =
    th [
      Attr.Style "text-align" "left"
      Attr.Style "padding" "8px 12px"
      Attr.Style "white-space" "nowrap"
      Attr.Style "border-bottom" "1px solid var(--palette-divider)"
    ] [ text label ]

  /// A box inside a dashed-border container that makes margin visible as the gap between them.
  let private marginDemoBox (label: string) (marginAttrs: Attr list) =
    div [
      BorderRadius.toClass BorderRadius.All.small |> cl
      SurfaceColor.toBackgroundColor SurfaceColor.Background
    ] [
      div [
        BorderRadius.toClass BorderRadius.All.small |> cl
        BrandColor.toBackgroundColor BrandColor.Primary

        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          JustifyContent.toClass JustifyContent.Center
        ]

        Attr.Style "min-height" "48px"
        yield! marginAttrs
      ] [ Subtitle2.Div(label) ]
    ]

  /// A box with visible padding: outer surface shows the padding zone, inner primary box shows content.
  let private paddingDemoBox (label: string) (paddingAttrs: Attr list) =
    div [
      SurfaceColor.toBackgroundColor SurfaceColor.Background
      BorderRadius.toClass BorderRadius.All.small |> cl
      yield! paddingAttrs
    ] [
      div [
        BrandColor.toBackgroundColor BrandColor.Primary
        BorderRadius.toClass BorderRadius.All.small |> cl
        Padding.toClasses Padding.All.extraSmall |> cls

        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          JustifyContent.toClass JustifyContent.Center
        ]

        Attr.Style "min-height" "48px"
      ] [ Subtitle2.Div(label) ]
    ]

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Pick a type (Margin or Padding), a direction, and a size. Weave generates the correct CSS utility classes."

    let content =
      div [] [
        Subtitle2.Div("Sizes", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

        div [ Margin.toClasses Margin.Bottom.small |> cls; Attr.Style "overflow-x" "auto" ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [
              tr [] [ tableHeaderCell "Size"; tableHeaderCell "Pixels"; tableHeaderCell "F# value" ]
            ]
            tbody [] [
              tr [] [
                tableCell [ text "None" ]
                tableCell [ text "0px" ]
                tableCell [ inlineCode "Margin.Bottom.none" ]
              ]
              tr [] [
                tableCell [ text "ExtraSmall" ]
                tableCell [ text "4px" ]
                tableCell [ inlineCode "Margin.Bottom.extraSmall" ]
              ]
              tr [] [
                tableCell [ text "Small" ]
                tableCell [ text "8px" ]
                tableCell [ inlineCode "Margin.Bottom.small" ]
              ]
              tr [] [
                tableCell [ text "Medium" ]
                tableCell [ text "12px" ]
                tableCell [ inlineCode "Margin.Bottom.medium" ]
              ]
              tr [] [
                tableCell [ text "Large" ]
                tableCell [ text "16px" ]
                tableCell [ inlineCode "Margin.Bottom.large" ]
              ]
              tr [] [
                tableCell [ text "ExtraLarge" ]
                tableCell [ text "20px" ]
                tableCell [ inlineCode "Margin.Bottom.extraLarge" ]
              ]
            ]
          ]
        ]

        Subtitle2.Div("Directions", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

        div [ Margin.toClasses Margin.Bottom.small |> cls; Attr.Style "overflow-x" "auto" ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [
              tr [] [
                tableHeaderCell "Direction"
                tableHeaderCell "Applies to"
                tableHeaderCell "F# helper"
                tableHeaderCell "CSS prefix"
              ]
            ]
            tbody [] [
              tr [] [
                tableCell [ inlineCode "Top" ]
                tableCell [ text "Top edge" ]
                tableCell [ inlineCode "Margin.Top.{size}" ]
                tableCell [ inlineCode "mt- / pt-" ]
              ]
              tr [] [
                tableCell [ inlineCode "Bottom" ]
                tableCell [ text "Bottom edge" ]
                tableCell [ inlineCode "Margin.Bottom.{size}" ]
                tableCell [ inlineCode "mb- / pb-" ]
              ]
              tr [] [
                tableCell [ inlineCode "Left" ]
                tableCell [ text "Left edge" ]
                tableCell [ inlineCode "Margin.Left.{size}" ]
                tableCell [ inlineCode "ml- / pl-" ]
              ]
              tr [] [
                tableCell [ inlineCode "Right" ]
                tableCell [ text "Right edge" ]
                tableCell [ inlineCode "Margin.Right.{size}" ]
                tableCell [ inlineCode "mr- / pr-" ]
              ]
              tr [] [
                tableCell [ inlineCode "Vertical" ]
                tableCell [ text "Top and bottom" ]
                tableCell [ inlineCode "Margin.Vertical.{size}" ]
                tableCell [ inlineCode "my- / py-" ]
              ]
              tr [] [
                tableCell [ inlineCode "Horizontal" ]
                tableCell [ text "Left and right" ]
                tableCell [ inlineCode "Margin.Horizontal.{size}" ]
                tableCell [ inlineCode "mx- / px-" ]
              ]
              tr [] [
                tableCell [ inlineCode "All" ]
                tableCell [ text "All four sides" ]
                tableCell [ inlineCode "Margin.All.{size}" ]
                tableCell [ inlineCode "ma- / pa-" ]
              ]
            ]
          ]
        ]

        Body2.Div(
          "Padding follows the same pattern — replace Margin with Padding.",
          attrs = [ Attr.Style "opacity" "0.7"; Margin.toClasses Margin.Bottom.small |> cls ]
        )

        Body2.Div(
          "Each CSS class suffix is a number from 0 to 20, where each step equals 0.25rem (4px)",
          attrs = [ Attr.Style "opacity" "0.7" ]
        )
      ]

    Helpers.section "How it works" description content

  let private marginSection () =
    let description =
      Helpers.bodyText
        "Margin adds space outside an element, pushing it away from its neighbors. The dashed border below shows the container boundary so the margin gap is visible."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              marginDemoBox "mb-8" [ Margin.toClasses Margin.Bottom.small |> cls ]
              Body2.Div(
                "Margin.Bottom.small",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              marginDemoBox "ma-12" [ Margin.toClasses Margin.All.medium |> cls ]
              Body2.Div(
                "Margin.All.medium",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              marginDemoBox "my-8" [ Margin.toClasses Margin.Vertical.small |> cls ]
              Body2.Div(
                "Margin.Vertical.small",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              marginDemoBox "mx-16" [ Margin.toClasses Margin.Horizontal.large |> cls ]
              Body2.Div(
                "Margin.Horizontal.large",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

// Bottom margin (most common: space between stacked elements)
div [ Margin.toClasses Margin.Bottom.small |> cls ] [
    text "Bottom margin 8px"
]

// All sides
div [ Margin.toClasses Margin.All.medium |> cls ] [
    text "All sides 12px"
]

// Multiple directions
div [
    cls [
        yield! Margin.toClasses Margin.Top.small
        yield! Margin.toClasses Margin.Bottom.small
    ]
] [
    text "Top 8px, Bottom 8px"
]

// Vertical shorthand (top + bottom)
div [ Margin.toClasses Margin.Vertical.small |> cls ] [
    text "Top and bottom 8px"
]"""

    Helpers.codeSampleSection "Margin" description content code

  let private paddingSection () =
    let description =
      Helpers.bodyText
        "Padding adds space inside an element, between its border and its content. The examples below use a background color to make the padding region visible."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              paddingDemoBox "pa-8" [ Padding.toClasses Padding.All.small |> cls ]
              Body2.Div(
                "Padding.All.small",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              paddingDemoBox "px-12" [ Padding.toClasses Padding.Horizontal.medium |> cls ]
              Body2.Div(
                "Padding.Horizontal.medium",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              paddingDemoBox "py-16" [ Padding.toClasses Padding.Vertical.large |> cls ]
              Body2.Div(
                "Padding.Vertical.large",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            div [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ] [
              paddingDemoBox "pt-20" [ Padding.toClasses Padding.Top.extraLarge |> cls ]
              Body2.Div(
                "Padding.Top.extraLarge",
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls; Attr.Style "opacity" "0.6" ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

// All sides
div [ Padding.toClasses Padding.All.small |> cls ] [
    text "Padded content (8px all sides)"
]

// Horizontal only
div [ Padding.toClasses Padding.Horizontal.medium |> cls ] [
    text "Horizontal padding (12px left and right)"
]

// Vertical only
div [ Padding.toClasses Padding.Vertical.large |> cls ] [
    text "Vertical padding (16px top and bottom)"
]

// Combining padding directions
div [
    cls [
        yield! Padding.toClasses Padding.Top.extraSmall
        yield! Padding.toClasses Padding.Horizontal.large
    ]
] [
    text "Top 4px, Left and Right 16px"
]"""

    Helpers.codeSampleSection "Padding" description content code

  let private densitySection () =
    let description =
      Helpers.bodyText
        "Density controls the internal spacing of components on a three-step scale: Compact, Standard (the default), and Spacious. Apply it to a container element to affect all descendants, or pass it in a single component's attrs to override just that instance. Density is meant as a simple default option to help reduce boilerplate. You can achieve the same effect by applying the underlying CSS utility classes directly."

    let content =
      let col density =
        let label = sprintf "%A" density

        div [ cl (Density.toClass density) ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          div [] [
            Button.Create(
              text "Filled",
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            )
          ]
          div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
            Button.Create(
              text "Outlined",
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Outlined |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            )
          ]
          div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
            Button.Create(
              text "Text",
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Text |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            )
          ]
        ]

      Grid.Create(
        [
          GridItem.Create(col Density.Compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Standard, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Spacious, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.toClass AlignItems.Start |> cl ]
      )

    let code =
      """open Weave
open Weave.CssHelpers

// Container-level: all children inherit the density via CSS cascade
div [ cl (Density.toClass Density.Compact) ] [ // see here
    Button.Create(
        text "Filled",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.Filled |> Button.Variant.toClass |> cl
            BrandColor.Primary |> Button.Color.toClass |> cl
        ]
    )
    Button.Create(
        text "Outlined",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.Outlined |> Button.Variant.toClass |> cl
            BrandColor.Primary |> Button.Color.toClass |> cl
        ]
    )
]

// Per-instance: pass the density class in attrs to set it on one component
Button.Create(
    text "Spacious",
    onClick = (fun () -> ()),
    attrs = [
        cl (Density.toClass Density.Spacious) // see here
        Button.Variant.Filled |> Button.Variant.toClass |> cl
        BrandColor.Primary |> Button.Color.toClass |> cl
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Spacing"
        Body1.Div(
          "Apply margin, padding, and density to any element using type-safe CSS utility classes.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        marginSection ()
        Helpers.divider ()
        paddingSection ()
        Helpers.divider ()
        densitySection ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
