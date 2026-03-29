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
        "Opacity utility classes control element transparency on a stepped scale from 0 (invisible) to 100 (fully opaque) in increments of 10. Apply an opacity class via the Opacity module callsites (e.g. Opacity.fifty for 50%)."

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
              tableCell [ inlineCode "Opacity.zero" ]
            ]
            tr [] [
              tableCell [ text "10" ]
              tableCell [ text "10%" ]
              tableCell [ inlineCode "Opacity.ten" ]
            ]
            tr [] [
              tableCell [ text "20" ]
              tableCell [ text "20%" ]
              tableCell [ inlineCode "Opacity.twenty" ]
            ]
            tr [] [
              tableCell [ text "30" ]
              tableCell [ text "30%" ]
              tableCell [ inlineCode "Opacity.thirty" ]
            ]
            tr [] [
              tableCell [ text "40" ]
              tableCell [ text "40%" ]
              tableCell [ inlineCode "Opacity.forty" ]
            ]
            tr [] [
              tableCell [ text "50" ]
              tableCell [ text "50%" ]
              tableCell [ inlineCode "Opacity.fifty" ]
            ]
            tr [] [
              tableCell [ text "60" ]
              tableCell [ text "60%" ]
              tableCell [ inlineCode "Opacity.sixty" ]
            ]
            tr [] [
              tableCell [ text "70" ]
              tableCell [ text "70%" ]
              tableCell [ inlineCode "Opacity.seventy" ]
            ]
            tr [] [
              tableCell [ text "80" ]
              tableCell [ text "80%" ]
              tableCell [ inlineCode "Opacity.eighty" ]
            ]
            tr [] [
              tableCell [ text "90" ]
              tableCell [ text "90%" ]
              tableCell [ inlineCode "Opacity.ninety" ]
            ]
            tr [] [
              tableCell [ text "100" ]
              tableCell [ text "100%" ]
              tableCell [ inlineCode "Opacity.hundred" ]
            ]
          ]
        ]
      ]

    let code =
      """open Weave

// Apply an opacity class directly
div [ Opacity.fifty ] [
    text "This element is 50% transparent"
]

// Named callsites for each step of 10
// Opacity.zero       → weave-opacity-0
// Opacity.fifty      → weave-opacity-50
// Opacity.hundred    → weave-opacity-100

// Combine with other attrs
div [
    Opacity.seventy
    Padding.All.small
] [
    text "70% opacity with padding"
]"""

    Helpers.codeSampleSection "How it works" description content code

  let private visualScaleSection () =
    let description =
      Helpers.bodyText
        "The full opacity scale applied to a colored surface. Each box shows its opacity percentage."

    let opacityLevels = [
      0, Opacity.zero
      10, Opacity.ten
      20, Opacity.twenty
      30, Opacity.thirty
      40, Opacity.forty
      50, Opacity.fifty
      60, Opacity.sixty
      70, Opacity.seventy
      80, Opacity.eighty
      90, Opacity.ninety
      100, Opacity.hundred
    ]

    let content =
      Grid.create (
        [
          for (level, opacity) in opacityLevels do
            GridItem.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
                div [
                  BrandColor.BackgroundColor.primary
                  BorderRadius.All.small
                  opacity
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
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.two ]
            )
        ]
      )

    let code =
      """open Weave

// Apply opacity to any element via the module callsite
div [
    BrandColor.BackgroundColor.primary
    Opacity.fifty
] [
    text "50% opacity"
]

// Each step of 10 has a named callsite
div [ BrandColor.BackgroundColor.primary; Opacity.zero ] [ text "0%" ]
div [ BrandColor.BackgroundColor.primary; Opacity.thirty ] [ text "30%" ]
div [ BrandColor.BackgroundColor.primary; Opacity.seventy ] [ text "70%" ]
div [ BrandColor.BackgroundColor.primary; Opacity.hundred ] [ text "100%" ]"""

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
