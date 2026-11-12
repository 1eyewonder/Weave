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

  /// A box inside a dashed-border container that makes margin visible as the gap between them.
  let private marginDemoBox (label: string) (marginAttrs: Attr list) =
    div [
      BorderRadius.All.small
      SurfaceColor.BackgroundColor.background
      Attr.Style "display" "flow-root"
    ] [
      div [
        BorderRadius.All.small
        BrandColor.Fill.primary
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
      SurfaceColor.BackgroundColor.background
      BorderRadius.All.small
      yield! paddingAttrs
    ] [
      div [
        BrandColor.Fill.primary
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
              tr [] [
                tableHeaderCell "Size"
                tableHeaderCell "Value at a 16px root"
                tableHeaderCell "F# value"
              ]
            ]
            tbody [] [
              tr [] [
                tableCell [ text "None" ]
                tableCell [ text "0rem (0px)" ]
                tableCell [ inlineCode "Margin.Bottom.none" ]
              ]
              tr [] [
                tableCell [ text "ExtraSmall" ]
                tableCell [ text "1rem (16px)" ]
                tableCell [ inlineCode "Margin.Bottom.extraSmall" ]
              ]
              tr [] [
                tableCell [ text "Small" ]
                tableCell [ text "2rem (32px)" ]
                tableCell [ inlineCode "Margin.Bottom.small" ]
              ]
              tr [] [
                tableCell [ text "Medium" ]
                tableCell [ text "3rem (48px)" ]
                tableCell [ inlineCode "Margin.Bottom.medium" ]
              ]
              tr [] [
                tableCell [ text "Large" ]
                tableCell [ text "4rem (64px)" ]
                tableCell [ inlineCode "Margin.Bottom.large" ]
              ]
              tr [] [
                tableCell [ text "ExtraLarge" ]
                tableCell [ text "5rem (80px)" ]
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

        div [ Typography.body2; Typography.Color.textSecondary; Margin.Bottom.small ] [
          text "Padding follows the same pattern — replace Margin with Padding."
        ]

        div [ Typography.body2; Typography.Color.textSecondary ] [
          text
            "Each numeric CSS class suffix counts 0.25rem steps, from 0 to 20. One step is 4px at a 16px root font size; all values scale with that root size."
        ]
      ]

    Helpers.section "How it works" description content

  let private marginSection () =
    let description =
      Helpers.bodyText
        "Margin adds space outside an element, pushing it away from its neighbors. The surrounding background makes the margin gap visible."

    let samples = [
      "Margin.Bottom.small", Margin.Bottom.small
      "Margin.All.medium", Margin.All.medium
      "Margin.Vertical.small", Margin.Vertical.small
      "Margin.Horizontal.large", Margin.Horizontal.large
    ]

    let content =
      Grid.create [
        for label, spacing in samples do
          GridItem.create (
            div [ Attr.Style "inline-size" "100%" ] [
              p [ Typography.body2; Margin.Bottom.extraSmall ] [ text label ]
              div [ SurfaceColor.BackgroundColor.background; Attr.Style "display" "flow-root" ] [
                div [ spacing; BrandColor.Fill.primary; Padding.All.small ] [ text "Box" ]
              ]
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
      ]

    let code =
      """div [ SurfaceColor.BackgroundColor.background; Attr.Style "display" "flow-root" ] [
  div [ Margin.Bottom.small; BrandColor.Fill.primary; Padding.All.small ] [
    text "Box"
  ]
]

div [ SurfaceColor.BackgroundColor.background; Attr.Style "display" "flow-root" ] [
  div [ Margin.All.medium; BrandColor.Fill.primary; Padding.All.small ] [
    text "Box"
  ]
]

div [ SurfaceColor.BackgroundColor.background; Attr.Style "display" "flow-root" ] [
  div [ Margin.Vertical.small; BrandColor.Fill.primary; Padding.All.small ] [
    text "Box"
  ]
]

div [ SurfaceColor.BackgroundColor.background; Attr.Style "display" "flow-root" ] [
  div [ Margin.Horizontal.large; BrandColor.Fill.primary; Padding.All.small ] [
    text "Box"
  ]
]"""

    Helpers.codeSampleSection "Margin" description content code

  let private paddingSection () =
    let description =
      Helpers.bodyText
        "Padding adds space inside an element, between its border and its content. The examples below use a background color to make the padding region visible."

    let samples = [
      "Padding.All.small", Padding.All.small
      "Padding.Horizontal.medium", Padding.Horizontal.medium
      "Padding.Vertical.large", Padding.Vertical.large
      "Padding.Top.extraLarge", Padding.Top.extraLarge
    ]

    let content =
      Grid.create [
        for label, spacing in samples do
          GridItem.create (
            div [ Attr.Style "inline-size" "100%" ] [
              p [ Typography.body2; Margin.Bottom.extraSmall ] [ text label ]
              div [ spacing; SurfaceColor.BackgroundColor.background ] [
                div [ BrandColor.Fill.primary; Padding.All.extraSmall ] [ text "Box" ]
              ]
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
      ]

    let code =
      """div [ Padding.All.small; SurfaceColor.BackgroundColor.background ] [
  div [ BrandColor.Fill.primary; Padding.All.extraSmall ] [
    text "Box"
  ]
]

div [ Padding.Horizontal.medium; SurfaceColor.BackgroundColor.background ] [
  div [ BrandColor.Fill.primary; Padding.All.extraSmall ] [
    text "Box"
  ]
]

div [ Padding.Vertical.large; SurfaceColor.BackgroundColor.background ] [
  div [ BrandColor.Fill.primary; Padding.All.extraSmall ] [
    text "Box"
  ]
]

div [ Padding.Top.extraLarge; SurfaceColor.BackgroundColor.background ] [
  div [ BrandColor.Fill.primary; Padding.All.extraSmall ] [
    text "Box"
  ]
]"""

    Helpers.codeSampleSection "Padding" description content code

  let private responsiveSection () =
    let description =
      Helpers.bodyText
        "Spacing values can be scoped to a breakpoint so they only apply at that viewport width and above. Chain a breakpoint module between the direction and the size: Direction.Breakpoint.size. Resize your browser to see the values change."

    let content =
      let reactiveCaption (labelView: View<string>) =
        div [ Margin.Top.extraSmall; Typography.Color.textSecondary ] [ textView labelView ]

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

        div [ Typography.body2; Margin.Bottom.small; Typography.Color.textSecondary ] [
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
                    [ (Breakpoint.Small, "margin: 2rem (Small active)") ]
                    "margin: 0px (below Small)"
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                marginDemoBox "md: ma-16" [ Margin.All.none; Margin.All.Medium.large ]
                reactiveCaption (
                  responsiveLabel
                    [ (Breakpoint.Medium, "margin: 4rem (Medium active)") ]
                    "margin: 0px (below Medium)"
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                paddingDemoBox "lg: pa-20" [ Padding.All.extraSmall; Padding.All.Large.extraLarge ]
                reactiveCaption (
                  responsiveLabel
                    [ (Breakpoint.Large, "padding: 5rem (Large active)") ]
                    "padding: 1rem (Extra small active)"
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
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
                      (Breakpoint.Small, "margin: 2rem (Small active)")
                      (Breakpoint.Medium, "margin: 3rem (Medium active)")
                      (Breakpoint.Large, "margin: 4rem (Large active)")
                    ]
                    "margin: 1rem (Extra small active)"
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )
          ],
          attrs = [ Grid.Spacing.small ]
        )
      ]

    Helpers.section "Responsive breakpoints" description content

  let private densitySection () =
    let description =
      Helpers.bodyText
        "Density controls component spacing on three steps: Compact, Standard (the default), and Spacious. Apply it to a parent to share the density, or pass it in one component's attrs to override that instance."

    let content =
      let column density =
        div [
          density
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          AlignItems.center
        ] [
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

      Helpers.densityComparison [
        "Compact", column Density.compact
        "Standard", column Density.standard
        "Spacious", column Density.spacious
      ]

    let code =
      """div [ Density.compact ] [
  Button.primary (
    text "Filled",
    onClick = ignore,
    attrs = [ Button.Variant.filled ]
  )
  Button.primary (
    text "Outlined",
    onClick = ignore,
    attrs = [ Button.Variant.outlined ]
  )
  Button.primary (
    text "Text",
    onClick = ignore,
    attrs = [ Button.Variant.text ]
  )
]

div [ Density.standard ] [
  Button.primary (
    text "Filled",
    onClick = ignore,
    attrs = [ Button.Variant.filled ]
  )
  Button.primary (
    text "Outlined",
    onClick = ignore,
    attrs = [ Button.Variant.outlined ]
  )
  Button.primary (
    text "Text",
    onClick = ignore,
    attrs = [ Button.Variant.text ]
  )
]

div [ Density.spacious ] [
  Button.primary (
    text "Filled",
    onClick = ignore,
    attrs = [ Button.Variant.filled ]
  )
  Button.primary (
    text "Outlined",
    onClick = ignore,
    attrs = [ Button.Variant.outlined ]
  )
  Button.primary (
    text "Text",
    onClick = ignore,
    attrs = [ Button.Variant.text ]
  )
]"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Spacing"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Apply margin, padding, and density to any element using type-safe CSS utility classes."
        ]

        Helpers.divider ()
        marginSection ()
        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        paddingSection ()
        Helpers.divider ()
        responsiveSection ()
        Helpers.divider ()
        densitySection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
