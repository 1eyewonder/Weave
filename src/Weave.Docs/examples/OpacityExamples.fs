namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module OpacityExamples =

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

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Opacity utility classes control element transparency on a stepped scale from 0 (invisible) to 100 (fully opaque) in increments of 5. Create an Opacity value with Opacity.create, then convert to a CSS class with Opacity.toClass."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [
              tableHeaderCell "Value"
              tableHeaderCell "Opacity"
              tableHeaderCell "F# code"
            ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "0" ]
              tableCell [ text "0%" ]
              tableCell [ inlineCode "Opacity.create 0 |> Opacity.toClass" ]
            ]
            tr [] [
              tableCell [ text "25" ]
              tableCell [ text "25%" ]
              tableCell [ inlineCode "Opacity.create 25 |> Opacity.toClass" ]
            ]
            tr [] [
              tableCell [ text "50" ]
              tableCell [ text "50%" ]
              tableCell [ inlineCode "Opacity.create 50 |> Opacity.toClass" ]
            ]
            tr [] [
              tableCell [ text "75" ]
              tableCell [ text "75%" ]
              tableCell [ inlineCode "Opacity.create 75 |> Opacity.toClass" ]
            ]
            tr [] [
              tableCell [ text "100" ]
              tableCell [ text "100%" ]
              tableCell [ inlineCode "Opacity.create 100 |> Opacity.toClass" ]
            ]
          ]
        ]
      ]

    let code =
      """open Weave


// Create an opacity value and apply it as a CSS class
div [ Opacity.create 50 |> Opacity.toClass |> cl ] [
    text "This element is 50% transparent"
]

// Opacity values are clamped to 0-100
let hidden = Opacity.create 0 |> Opacity.toClass   // weave-opacity-0
let half   = Opacity.create 50 |> Opacity.toClass   // weave-opacity-50
let full   = Opacity.create 100 |> Opacity.toClass  // weave-opacity-100

// Combine with other attrs
div [
    Opacity.create 75 |> Opacity.toClass |> cl
    Padding.All.small
] [
    text "75% opacity with padding"
]"""

    Helpers.codeSampleSection "How it works" description content code

  let private visualScaleSection () =
    let description =
      Helpers.bodyText
        "The full opacity scale applied to a colored surface. Each box shows its opacity percentage."

    let content =
      Grid.create (
        [
          for level in [ 0; 10; 20; 30; 40; 50; 60; 70; 80; 90; 100 ] do
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                div [
                  BrandColor.toBackgroundColor BrandColor.Primary
                  BorderRadius.All.small
                  Opacity.create level |> Opacity.toClass |> cl
                  Flex.Flex.allSizes
                  AlignItems.center
                  JustifyContent.center
                  Attr.Style "min-height" "64px"
                  Attr.Style "width" "100%"
                ] [ div [ Typography.subtitle2 ] [ text (sprintf "%d%%" level) ] ]
                div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
                  text (sprintf "%d" level)
                ]
              ],
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4,
              md = Grid.Width.create 2
            )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


// Apply opacity to any element via the class
div [
    BrandColor.toBackgroundColor BrandColor.Primary
    Opacity.create 50 |> Opacity.toClass |> cl
] [
    text "50% opacity"
]

// Build a scale programmatically
for level in [ 0; 25; 50; 75; 100 ] do
    div [
        BrandColor.toBackgroundColor BrandColor.Primary
        Opacity.create level |> Opacity.toClass |> cl
    ] [
        text (sprintf "%d%%" level)
    ]"""

    Helpers.codeSampleSection "Visual scale" description content code

  let private designTokensSection () =
    let description =
      Helpers.bodyText
        "Weave provides semantic opacity CSS custom properties for common use cases. Components reference these tokens internally."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [ tableHeaderCell "Token"; tableHeaderCell "Value"; tableHeaderCell "Purpose" ]
          ]
          tbody [] [
            tr [] [
              tableCell [ inlineCode "--weave-opacity-disabled" ]
              tableCell [ text "0.38" ]
              tableCell [ text "Disabled elements" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-opacity-disabled-container" ]
              tableCell [ text "0.6" ]
              tableCell [ text "Disabled field/select containers" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-opacity-placeholder" ]
              tableCell [ text "0.42" ]
              tableCell [ text "Input placeholder text" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-opacity-secondary" ]
              tableCell [ text "0.5" ]
              tableCell [ text "Secondary actions (clear buttons)" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-opacity-secondary-hover" ]
              tableCell [ text "0.7" ]
              tableCell [ text "Secondary action hover state" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-opacity-muted" ]
              tableCell [ text "0.8" ]
              tableCell [ text "Muted/subdued elements" ]
            ]
          ]
        ]
      ]

    let code =
      """open WebSharper.UI.Html


// Use a design token directly in an inline style
div [ Attr.Style "opacity" "var(--weave-opacity-disabled)" ] [
    text "Matches the disabled opacity used by all Weave components"
]

// Override a token for a subtree
div [ Attr.Style "--weave-opacity-disabled" "0.5" ] [
    // All disabled components inside this container
    // will use 0.5 instead of the default 0.38
    text "Custom disabled opacity"
]"""

    Helpers.codeSampleSection "Design tokens" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Opacity"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Control element transparency with a stepped utility scale and semantic design tokens."
        ]

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        visualScaleSection ()
        Helpers.divider ()
        designTokensSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
