namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TransitionExamples =

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
        "Transition speed controls how fast component animations play. Weave uses CSS custom properties for all transition durations, so you can override them per-element or subtree. Apply a speed class via the TransitionSpeed module callsites."

    let content =
      div [
        Attr.Class "docs-table-scroll"
        Attr.Style "overflow-x" "auto"
        attr.tabindex "0"
        Attr.Create "aria-label" "Utility reference"
      ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [ tableHeaderCell "Speed"; tableHeaderCell "Effect"; tableHeaderCell "F# code" ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "None" ]
              tableCell [ text "Instant (0ms)" ]
              tableCell [ inlineCode "TransitionSpeed.none" ]
            ]
            tr [] [
              tableCell [ text "Fast" ]
              tableCell [ text "Half duration (faster)" ]
              tableCell [ inlineCode "TransitionSpeed.fast" ]
            ]
            tr [] [
              tableCell [ text "Standard" ]
              tableCell [ text "Default" ]
              tableCell [ inlineCode "TransitionSpeed.standard" ]
            ]
            tr [] [
              tableCell [ text "Slow" ]
              tableCell [ text "Double duration (slower)" ]
              tableCell [ inlineCode "TransitionSpeed.slow" ]
            ]
          ]
        ]
      ]

    Helpers.section "How it works" description content

  let private liveComparisonSection () =
    let description =
      Helpers.bodyText
        "Hover over or focus each button below to see the same hover transition at different speeds. The speed attribute is applied directly to each button. Apply it to a parent when several components should share the override."

    let content =
      let speedColumn (label: string) (speed: Attr) =
        div [ Flex.Flex.allSizes; JustifyContent.center ] [
          Button.primary (text label, onClick = ignore, attrs = [ Button.Variant.filled; speed ])
        ]

      Grid.create (
        [
          GridItem.create (
            speedColumn "None" TransitionSpeed.none,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
          )
          GridItem.create (
            speedColumn "Fast" TransitionSpeed.fast,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
          )
          GridItem.create (
            speedColumn "Standard" TransitionSpeed.standard,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
          )
          GridItem.create (
            speedColumn "Slow" TransitionSpeed.slow,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.three ]
          )
        ]
      )

    let code =
      """Button.primary (
  text "None",
  onClick = ignore,
  attrs = [ Button.Variant.filled; TransitionSpeed.none ]
)

Button.primary (
  text "Fast",
  onClick = ignore,
  attrs = [ Button.Variant.filled; TransitionSpeed.fast ]
)

Button.primary (
  text "Standard",
  onClick = ignore,
  attrs = [ Button.Variant.filled; TransitionSpeed.standard ]
)

Button.primary (
  text "Slow",
  onClick = ignore,
  attrs = [ Button.Variant.filled; TransitionSpeed.slow ]
)"""

    Helpers.codeSampleSection "Live comparison" description content code

  let private durationTokensSection () =
    let description =
      Helpers.bodyText
        "Under the hood, the speed classes override these CSS custom properties. All component transitions reference these tokens."

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
              tableHeaderCell "Token"
              tableHeaderCell "Standard"
              tableHeaderCell "Fast"
              tableHeaderCell "Slow"
            ]
          ]
          tbody [] [
            tr [] [
              tableCell [ inlineCode "--weave-duration-shortest" ]
              tableCell [ text "50ms" ]
              tableCell [ text "25ms" ]
              tableCell [ text "100ms" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-duration-shorter" ]
              tableCell [ text "100ms" ]
              tableCell [ text "50ms" ]
              tableCell [ text "200ms" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-duration-short" ]
              tableCell [ text "150ms" ]
              tableCell [ text "75ms" ]
              tableCell [ text "300ms" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-duration-standard" ]
              tableCell [ text "200ms" ]
              tableCell [ text "100ms" ]
              tableCell [ text "400ms" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-duration-medium" ]
              tableCell [ text "250ms" ]
              tableCell [ text "125ms" ]
              tableCell [ text "500ms" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-duration-long" ]
              tableCell [ text "300ms" ]
              tableCell [ text "150ms" ]
              tableCell [ text "600ms" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-duration-drawer" ]
              tableCell [ text "225ms" ]
              tableCell [ text "112ms" ]
              tableCell [ text "450ms" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-duration-ripple" ]
              tableCell [ text "600ms" ]
              tableCell [ text "300ms" ]
              tableCell [ text "1200ms" ]
            ]
          ]
        ]
      ]

    Helpers.section "Duration tokens" description content

  let private reducedMotionSection () =
    let description =
      Helpers.bodyText
        "When the user's OS has \"Reduce motion\" enabled, all duration tokens are automatically set to 0s. No code changes needed. This is built into the design system via a prefers-reduced-motion: reduce media query on :root."

    let content =
      div [] [
        div [ Typography.body2; Typography.Color.textSecondary ] [
          text
            "To test reduced motion in Chrome DevTools: open the Rendering panel, then set \"Emulate CSS media feature prefers-reduced-motion\" to \"reduce\". All Weave transitions will become instant."
        ]
      ]

    Helpers.section "Reduced motion" description content

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Transitions"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Control animation speed across components with transition speed classes and duration tokens."
        ]

        Helpers.divider ()
        liveComparisonSection ()
        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        durationTokensSection ()
        Helpers.divider ()
        reducedMotionSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
