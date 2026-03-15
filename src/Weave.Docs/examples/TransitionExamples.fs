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

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Transition speed controls how fast component animations play. Weave uses CSS custom properties for all transition durations, so you can override them per-element or subtree. Apply a speed class via TransitionSpeed.toClass in attrs."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [ tableHeaderCell "Speed"; tableHeaderCell "Effect"; tableHeaderCell "F# code" ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "None" ]
              tableCell [ text "Instant (0ms)" ]
              tableCell [ inlineCode "TransitionSpeed.toClass TransitionSpeed.None" ]
            ]
            tr [] [
              tableCell [ text "Fast" ]
              tableCell [ text "Half speed" ]
              tableCell [ inlineCode "TransitionSpeed.toClass TransitionSpeed.Fast" ]
            ]
            tr [] [
              tableCell [ text "Standard" ]
              tableCell [ text "Default" ]
              tableCell [ inlineCode "TransitionSpeed.toClass TransitionSpeed.Standard" ]
            ]
            tr [] [
              tableCell [ text "Slow" ]
              tableCell [ text "Double speed" ]
              tableCell [ inlineCode "TransitionSpeed.toClass TransitionSpeed.Slow" ]
            ]
          ]
        ]
      ]

    let code =
      """open Weave


// Apply a transition speed to a single element
div [ cl (TransitionSpeed.toClass TransitionSpeed.Fast) ] [
    text "Animations inside here run at half the default duration"
]

// Disable transitions entirely
div [ cl (TransitionSpeed.toClass TransitionSpeed.None) ] [
    text "No transitions — all changes are instant"
]

// The speed class overrides CSS custom properties,
// so all children inherit the new durations
div [ cl (TransitionSpeed.toClass TransitionSpeed.Slow) ] [
    Button.Create(
        text "Slow hover",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.Filled |> Button.Variant.toClass |> cl
            BrandColor.Primary |> Button.Color.toClass |> cl
        ]
    )
]"""

    Helpers.codeSampleSection "How it works" description content code

  let private liveComparisonSection () =
    let description =
      Helpers.bodyText
        "Hover over each button below to see the same hover transition at different speeds. The speed class is applied to a wrapper div, so all children inherit the speed override."

    let content =
      let speedColumn (label: string) (speed: TransitionSpeed) =
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Center
          ]
        ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          div [ cl (TransitionSpeed.toClass speed) ] [
            Button.Create(
              text "Hover me",
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            )
          ]
        ]

      Grid.Create(
        [
          GridItem.Create(
            speedColumn "None" TransitionSpeed.None,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 3
          )
          GridItem.Create(
            speedColumn "Fast" TransitionSpeed.Fast,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 3
          )
          GridItem.Create(
            speedColumn "Standard" TransitionSpeed.Standard,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 3
          )
          GridItem.Create(
            speedColumn "Slow" TransitionSpeed.Slow,
            xs = Grid.Width.create 6,
            sm = Grid.Width.create 3
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


// Wrap a component in a div with a speed class.
// The button inherits the transition duration override.
let speedColumn (speed: TransitionSpeed) =
    div [ cl (TransitionSpeed.toClass speed) ] [
        Button.Create(
            text "Hover me",
            onClick = (fun () -> ()),
            attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
            ]
        )
    ]

// None = instant, Fast = half, Standard = default, Slow = double
speedColumn TransitionSpeed.None
speedColumn TransitionSpeed.Fast
speedColumn TransitionSpeed.Standard
speedColumn TransitionSpeed.Slow"""

    Helpers.codeSampleSection "Live comparison" description content code

  let private durationTokensSection () =
    let description =
      Helpers.bodyText
        "Under the hood, the speed classes override these CSS custom properties. All component transitions reference these tokens."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
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

    let code =
      """// Use a duration token in a custom inline transition
div [
    Attr.Style "transition" "background-color var(--weave-duration-standard) ease"
] [
    text "Uses the same duration as Weave components"
]

// Override a single token for a subtree
div [ Attr.Style "--weave-duration-standard" "500ms" ] [
    // All components inside use 500ms for their standard transitions
    text "Custom standard duration"
]"""

    Helpers.codeSampleSection "Duration tokens" description content code

  let private reducedMotionSection () =
    let description =
      Helpers.bodyText
        "When the user's OS has \"Reduce motion\" enabled, all duration tokens are automatically set to 0s. No code changes needed. This is built into the design system via a prefers-reduced-motion: reduce media query on :root."

    let content =
      div [] [
        Body2.Div(
          "To test reduced motion in Chrome DevTools: open the Rendering panel, then set \"Emulate CSS media feature prefers-reduced-motion\" to \"reduce\". All Weave transitions will become instant.",
          attrs = [ Attr.Style "opacity" "0.7" ]
        )
      ]

    let code =
      """// No F# code needed — the CSS handles it automatically.
// The :root media query sets all --weave-duration-* tokens to 0s
// when the user prefers reduced motion.

// @media (prefers-reduced-motion: reduce) {
//   :root {
//     --weave-duration-shortest: 0s;
//     --weave-duration-shorter:  0s;
//     --weave-duration-short:    0s;
//     --weave-duration-standard: 0s;
//     --weave-duration-medium:   0s;
//     --weave-duration-long:     0s;
//     --weave-duration-drawer:   0s;
//     --weave-duration-ripple:   0s;
//   }
// }"""

    Helpers.codeSampleSection "Reduced motion" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Transitions"
        Body1.Div(
          "Control animation speed across components with transition speed classes and duration tokens.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        liveComparisonSection ()
        Helpers.divider ()
        durationTokensSection ()
        Helpers.divider ()
        reducedMotionSection ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
