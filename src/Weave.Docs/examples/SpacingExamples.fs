namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module SpacingExamples =

  let private responsiveLabel (breakpoints: (Breakpoint * string) list) (fallback: string) =
    Breakpoint.browser
    |> View.Map(fun currentBp ->
      let bpOrder = [
        Breakpoint.ExtraSmall
        Breakpoint.Small
        Breakpoint.Medium
        Breakpoint.Large
        Breakpoint.ExtraLarge
        Breakpoint.ExtraExtraLarge
      ]

      let currentIdx = bpOrder |> List.findIndex ((=) currentBp)

      breakpoints
      |> List.sortByDescending (fun (bp, _) -> bpOrder |> List.findIndex ((=) bp))
      |> List.tryFind (fun (bp, _) -> currentIdx >= (bpOrder |> List.findIndex ((=) bp)))
      |> Option.map snd
      |> Option.defaultValue fallback)

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

  /// A box inside a dashed-border container that makes margin visible as the gap between them.
  let private marginDemoBox (label: string) (marginAttrs: Attr list) =
    div [
      BorderRadius.All.small
      SurfaceColor.toBackgroundColor SurfaceColor.Background
    ] [
      div [
        BorderRadius.All.small
        BrandColor.toBackgroundColor BrandColor.Primary
        Flex.Flex.allSizes
        AlignItems.center
        JustifyContent.center
        Attr.Style "min-height" "48px"
        yield! marginAttrs
      ] [ div [ Typography.subtitle2 ] [ text label ] ]
    ]

  /// A box with visible padding: outer surface shows the padding zone, inner primary box shows content.
  let private paddingDemoBox (label: string) (paddingAttrs: Attr list) =
    div [
      SurfaceColor.toBackgroundColor SurfaceColor.Background
      BorderRadius.All.small
      yield! paddingAttrs
    ] [
      div [
        BrandColor.toBackgroundColor BrandColor.Primary
        BorderRadius.All.small
        Padding.All.extraSmall
        Flex.Flex.allSizes
        AlignItems.center
        JustifyContent.center
        Attr.Style "min-height" "48px"
      ] [ div [ Typography.subtitle2 ] [ text label ] ]
    ]

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Pick a type (Margin or Padding), a direction, and a size. Weave generates the correct CSS utility classes."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Sizes" ]

        div [ Margin.Bottom.small; Attr.Style "overflow-x" "auto" ] [
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

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Directions" ]

        div [ Margin.Bottom.small; Attr.Style "overflow-x" "auto" ] [
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

        div [ Typography.body2; Attr.Style "opacity" "0.7"; Margin.Bottom.small ] [
          text "Padding follows the same pattern — replace Margin with Padding."
        ]

        div [ Typography.body2; Attr.Style "opacity" "0.7" ] [
          text "Each CSS class suffix is a number from 0 to 20, where each step equals 0.25rem (4px)"
        ]
      ]

    let code =
      """open Weave


// Margin: pick a direction and size
div [ Margin.Bottom.small ] [ // see here
    text "8px below this element"
]

// Padding: same pattern, applied inside the element
div [ Padding.All.medium ] [ // see here
    text "12px on all sides"
]

// Combine directions for fine-grained control
div [
    Margin.Bottom.large
    Padding.Horizontal.small
] [
    text "16px margin below, 8px padding left and right"
]"""

    Helpers.codeSampleSection "How it works" description content code

  let private marginSection () =
    let description =
      Helpers.bodyText
        "Margin adds space outside an element, pushing it away from its neighbors. The dashed border below shows the container boundary so the margin gap is visible."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              marginDemoBox "mb-8" [ Margin.Bottom.small ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Margin.Bottom.small"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              marginDemoBox "ma-12" [ Margin.All.medium ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Margin.All.medium"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              marginDemoBox "my-8" [ Margin.Vertical.small ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Margin.Vertical.small"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              marginDemoBox "mx-16" [ Margin.Horizontal.large ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Margin.Horizontal.large"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


// Bottom margin (most common: space between stacked elements)
div [ Margin.Bottom.small ] [
    text "Bottom margin 8px"
]

// All sides
div [ Margin.All.medium ] [
    text "All sides 12px"
]

// Multiple directions
div [
    Margin.Top.small
    Margin.Bottom.small
] [
    text "Top 8px, Bottom 8px"
]

// Vertical shorthand (top + bottom)
div [ Margin.Vertical.small ] [
    text "Top and bottom 8px"
]"""

    Helpers.codeSampleSection "Margin" description content code

  let private paddingSection () =
    let description =
      Helpers.bodyText
        "Padding adds space inside an element, between its border and its content. The examples below use a background color to make the padding region visible."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              paddingDemoBox "pa-8" [ Padding.All.small ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Padding.All.small"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              paddingDemoBox "px-12" [ Padding.Horizontal.medium ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Padding.Horizontal.medium"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              paddingDemoBox "py-16" [ Padding.Vertical.large ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Padding.Vertical.large"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
              paddingDemoBox "pt-20" [ Padding.Top.extraLarge ]
              div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                text "Padding.Top.extraLarge"
              ]
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


// All sides
div [ Padding.All.small ] [
    text "Padded content (8px all sides)"
]

// Horizontal only
div [ Padding.Horizontal.medium ] [
    text "Horizontal padding (12px left and right)"
]

// Vertical only
div [ Padding.Vertical.large ] [
    text "Vertical padding (16px top and bottom)"
]

// Combining padding directions
div [
    Padding.Top.extraSmall
    Padding.Horizontal.large
] [
    text "Top 4px, Left and Right 16px"
]"""

    Helpers.codeSampleSection "Padding" description content code

  let private responsiveSection () =
    let description =
      Helpers.bodyText
        "Spacing values can be scoped to a breakpoint so they only apply at that viewport width and above. Chain a breakpoint module between the direction and the size: Direction.Breakpoint.size. Resize your browser to see the values change."

    let content =
      let reactiveCaption (labelView: View<string>) =
        div [ Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [ textView labelView ]

      div [] [
        div [ Margin.Bottom.small; Attr.Style "overflow-x" "auto" ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [
              tr [] [
                tableHeaderCell "Breakpoint"
                tableHeaderCell "Min width"
                tableHeaderCell "Module path"
                tableHeaderCell "CSS prefix"
              ]
            ]
            tbody [] [
              tr [] [
                tableCell [ text "ExtraSmall" ]
                tableCell [ text "0px" ]
                tableCell [ inlineCode "Margin.All.ExtraSmall.small" ]
                tableCell [ inlineCode "(none)" ]
              ]
              tr [] [
                tableCell [ text "Small" ]
                tableCell [ text "600px" ]
                tableCell [ inlineCode "Margin.All.Small.small" ]
                tableCell [ inlineCode "sm-" ]
              ]
              tr [] [
                tableCell [ text "Medium" ]
                tableCell [ text "960px" ]
                tableCell [ inlineCode "Margin.All.Medium.small" ]
                tableCell [ inlineCode "md-" ]
              ]
              tr [] [
                tableCell [ text "Large" ]
                tableCell [ text "1280px" ]
                tableCell [ inlineCode "Margin.All.Large.small" ]
                tableCell [ inlineCode "lg-" ]
              ]
              tr [] [
                tableCell [ text "ExtraLarge" ]
                tableCell [ text "1920px" ]
                tableCell [ inlineCode "Margin.All.ExtraLarge.small" ]
                tableCell [ inlineCode "xl-" ]
              ]
              tr [] [
                tableCell [ text "ExtraExtraLarge" ]
                tableCell [ text "2560px" ]
                tableCell [ inlineCode "Margin.All.ExtraExtraLarge.small" ]
                tableCell [ inlineCode "xxl-" ]
              ]
            ]
          ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Live demo" ]

        div [ Typography.body2; Margin.Bottom.small; Attr.Style "opacity" "0.7" ] [
          text
            "The boxes below change their spacing at different breakpoints. Resize the browser to see the labels and values update."
        ]

        Grid.create (
          [
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                marginDemoBox "sm: ma-8" [ Margin.All.none; Margin.All.Small.small ]
                reactiveCaption (
                  responsiveLabel
                    [ (Breakpoint.Small, "margin: 8px (Small active)") ]
                    "margin: 0px (below Small)"
                )
              ],
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6
            )
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                marginDemoBox "md: ma-16" [ Margin.All.none; Margin.All.Medium.large ]
                reactiveCaption (
                  responsiveLabel
                    [ (Breakpoint.Medium, "margin: 16px (Medium active)") ]
                    "margin: 0px (below Medium)"
                )
              ],
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6
            )
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                paddingDemoBox "lg: pa-20" [ Padding.All.extraSmall; Padding.All.Large.extraLarge ]
                reactiveCaption (
                  responsiveLabel
                    [ (Breakpoint.Large, "padding: 20px (Large active)") ]
                    "padding: 4px (Extra small active)"
                )
              ],
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6
            )
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                marginDemoBox "multi-bp" [
                  Margin.All.extraSmall
                  Margin.All.Small.small
                  Margin.All.Medium.medium
                  Margin.All.Large.large
                ]
                reactiveCaption (
                  responsiveLabel
                    [
                      (Breakpoint.Small, "margin: 8px (Small active)")
                      (Breakpoint.Medium, "margin: 12px (Medium active)")
                      (Breakpoint.Large, "margin: 16px (Large active)")
                    ]
                    "margin: 4px (Extra small active)"
                )
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


// Responsive margin: applies small (8px) margin at the Small breakpoint (600px) and above
div [ Margin.All.Small.small ] [
    text "No margin on mobile, 8px margin at >= 600px"
]

// Combine a mobile default with a larger breakpoint override
div [
    Margin.All.extraSmall        // 4px on all viewports
    Margin.All.Medium.large      // 16px at >= 960px
] [
    text "4px margin on mobile, 16px at medium screens and above"
]

// Progressive scaling across multiple breakpoints
div [
    Margin.All.extraSmall          // 4px  base
    Margin.All.Small.small         // 8px  >= 600px
    Margin.All.Medium.medium       // 12px >= 960px
    Margin.All.Large.large         // 16px >= 1280px
] [
    text "Spacing grows with the viewport"
]

// Works the same way for Padding
div [
    Padding.Horizontal.small              // 8px always
    Padding.Horizontal.Large.extraLarge   // 20px >= 1280px
] [
    text "Tight on mobile, roomy on desktop"
]"""

    Helpers.codeSampleSection "Responsive breakpoints" description content code

  let private densitySection () =
    let description =
      Helpers.bodyText
        "Density controls the internal spacing of components on a three-step scale: Compact, Standard (the default), and Spacious. Apply it to a container element to affect all descendants, or pass it in a single component's attrs to override just that instance. Density is meant as a simple default option to help reduce boilerplate. You can achieve the same effect by applying the underlying CSS utility classes directly."

    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          div [] [
            Button.primary (text "Filled", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
          ]
          div [ Margin.Top.extraSmall ] [
            Button.primary (text "Outlined", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ])
          ]
          div [ Margin.Top.extraSmall ] [
            Button.primary (text "Text", onClick = (fun () -> ()), attrs = [ Button.Variant.text ])
          ]
        ]

      Grid.create (
        [
          GridItem.create (col "Compact" Density.compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.create (
            col "Standard" Density.standard,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave


// Container-level: all children inherit the density via CSS cascade
div [ Density.compact ] [ // see here
    Button.primary(
        text "Filled",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.filled
        ]
    )
    Button.primary(
        text "Outlined",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.outlined
        ]
    )
]

// Per-instance: pass the density attr in attrs to set it on one component
Button.primary(
    text "Spacious",
    onClick = (fun () -> ()),
    attrs = [
        Density.spacious // see here
        Button.Variant.filled
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Spacing"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Apply margin, padding, and density to any element using type-safe CSS utility classes."
        ]

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        marginSection ()
        Helpers.divider ()
        paddingSection ()
        Helpers.divider ()
        responsiveSection ()
        Helpers.divider ()
        densitySection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
