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
        "Opacity utility classes control element transparency on a stepped scale from 0 (invisible) to 100 (fully opaque) in increments of 10. Apply an opacity class via the Opacity module callsites (e.g. Opacity.fifty for 50%)."

    let content =
      div [
        Attr.Class "docs-table-scroll"
        Attr.Style "overflow-x" "auto"
        attr.tabindex "0"
        Attr.Create "aria-label" "Utility reference"
      ] [
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

    Helpers.section "How it works" description content

  let private visualScaleSection () =
    let description =
      Helpers.bodyText
        "The full opacity scale applied to colored samples. Labels stay fully opaque so every value remains readable."

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
                  BrandColor.Fill.primary
                  BorderRadius.All.small
                  opacity
                  Flex.Flex.allSizes
                  AlignItems.center
                  JustifyContent.center
                  Attr.Style "height" "64px"
                  Attr.Style "width" "64px"
                ] []
                div [ Typography.body2; Margin.Top.extraSmall; Typography.Color.textSecondary ] [
                  text (sprintf "%d%%" level)
                ]
              ],
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.two ]
            )
        ]
      )

    let code =
      """let sample opacity =
  div [
    opacity
    BrandColor.Fill.primary
    BorderRadius.All.small
    Attr.Style "width" "64px"
    Attr.Style "height" "64px"
  ] []

sample Opacity.zero
sample Opacity.ten
sample Opacity.twenty
sample Opacity.thirty
sample Opacity.forty
sample Opacity.fifty
sample Opacity.sixty
sample Opacity.seventy
sample Opacity.eighty
sample Opacity.ninety
sample Opacity.hundred"""

    Helpers.codeSampleSection "Visual scale" description content code

  let private designTokensSection () =
    let description =
      Helpers.bodyText
        "Weave provides semantic opacity CSS custom properties for common use cases. Components reference these tokens internally."

    let content =
      div [
        Attr.Class "docs-table-scroll"
        Attr.Style "overflow-x" "auto"
        attr.tabindex "0"
        Attr.Create "aria-label" "Utility reference"
      ] [
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

    Helpers.section "Design tokens" description content

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Opacity"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Control element transparency with a stepped utility scale and semantic design tokens."
        ]

        Helpers.divider ()
        visualScaleSection ()
        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        designTokensSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
