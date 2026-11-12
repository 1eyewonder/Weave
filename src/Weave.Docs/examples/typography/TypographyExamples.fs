namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

open WebSharper.JavaScript

[<JavaScript>]
module TypographyExamples =

  let private variantExamples () =
    let content =
      let variants = [
        "Heading 1", Typography.h1
        "Heading 2", Typography.h2
        "Heading 3", Typography.h3
        "Heading 4", Typography.h4
        "Heading 5", Typography.h5
        "Heading 6", Typography.h6
        "Subtitle 1", Typography.subtitle1
        "Subtitle 2", Typography.subtitle2
        "Body 1", Typography.body1
        "Body 2", Typography.body2
        "Button text", Typography.button
        "Caption", Typography.caption
        "Overline", Typography.overline
      ]

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        for label, variant in variants do
          div [ variant ] [ text label ]
      ]

    Helpers.codeSampleSection
      "Typography Variants"
      (Helpers.bodyText
        "Choose a visual variant independently of the HTML element. Use heading elements that reflect the structure of your own page.")
      content
      """div [ Typography.h1 ] [ text "Heading 1" ]

div [ Typography.h2 ] [ text "Heading 2" ]

div [ Typography.h3 ] [ text "Heading 3" ]

div [ Typography.h4 ] [ text "Heading 4" ]

div [ Typography.h5 ] [ text "Heading 5" ]

div [ Typography.h6 ] [ text "Heading 6" ]

div [ Typography.subtitle1 ] [ text "Subtitle 1" ]

div [ Typography.subtitle2 ] [ text "Subtitle 2" ]

div [ Typography.body1 ] [ text "Body 1" ]

div [ Typography.body2 ] [ text "Body 2" ]

div [ Typography.button ] [ text "Button text" ]

div [ Typography.caption ] [ text "Caption" ]

div [ Typography.overline ] [ text "Overline" ]"""

  let private weightExamples () =
    let content =
      let weights = [
        "Default H4 (700)", Attr.Empty
        "Light H4 (300)", Typography.Weight.light
        "Medium H4 (500)", Typography.Weight.medium
        "Bold H4 (700)", Typography.Weight.bold
      ]

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        for label, weight in weights do
          div [ Typography.h4; weight ] [ text label ]
      ]

    Helpers.codeSampleSection
      "Font Weights"
      (Helpers.bodyText
        "H4 defaults to bold/700. Weight attributes override that default for an individual element; this comparison uses the same text style throughout.")
      content
      """div [ Typography.h4 ] [ text "Default H4 (700)" ]

div [ Typography.h4; Typography.Weight.light ] [ text "Light H4 (300)" ]

div [ Typography.h4; Typography.Weight.medium ] [ text "Medium H4 (500)" ]

div [ Typography.h4; Typography.Weight.bold ] [ text "Bold H4 (700)" ]"""

  let private fontFamilyExamples () =
    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        div [ Typography.body1 ] [ text "Body 1 with its default body family" ]
        div [ Typography.body1; Typography.Family.display ] [ text "Body 1 with the display family" ]
        div [ Typography.h5; Typography.Family.body ] [ text "H5 with the body family" ]
        div [ Typography.body1 ] [
          text "Use "
          code [ Typography.Family.mono ] [ text "Typography.Family.mono" ]
          text " for an inline code fragment."
        ]
      ]

    Helpers.codeSampleSection
      "Font Family"
      (Helpers.bodyText
        "Family attributes choose the shared display, body, or monospace role without changing the variant size.")
      content
      """div [ Typography.body1 ] [ text "Body 1 with its default body family" ]

div [ Typography.body1; Typography.Family.display ] [
  text "Body 1 with the display family"
]

div [ Typography.h5; Typography.Family.body ] [
  text "H5 with the body family"
]

div [ Typography.body1 ] [
  text "Use "
  code [ Typography.Family.mono ] [ text "Typography.Family.mono" ]
  text " for an inline code fragment."
]"""

  let private customWeightExamples () =
    let description =
      Helpers.bodyText
        "Weave's typography scale uses CSS custom properties for all settings. You can override any variant's weight (or size, line-height, letter-spacing) globally by redefining the custom property in your own CSS."

    let content =
      div [] [
        div [ Typography.h3; Margin.Bottom.extraSmall ] [ text "Default H3 (weight 700)" ]

        div [ Attr.Style "--typography-h3-weight" "400" ] [
          div [ Typography.h3; Margin.Bottom.extraSmall ] [ text "H3 with --typography-h3-weight: 400" ]
        ]

        Divider.create (attrs = [ Margin.Bottom.small; Margin.Top.small ])

        div [ Typography.caption; Margin.Bottom.extraSmall ] [
          text
            "Tip: To change a variant's weight globally, override the CSS custom property in your app's stylesheet:"
        ]
        div [ Typography.caption; Typography.Weight.bold ] [ text ":root { --typography-h1-weight: 400; }" ]
      ]

    let code =
      """/* Load after Weave to override the heading defaults. */
:root {
  --typography-h3-weight: 400;
}

/* Or scope the override to one container. */
.my-section {
  --typography-h3-weight: 400;
}"""

    Helpers.codeSampleSectionFor "css" "Customizing via CSS Custom Properties" description content code

  let private textWrapExamples () =
    let sentence =
      "This sentence is long enough to show how a narrow region handles text."

    let content =
      let cell label body =
        GridItem.create (
          div [ Attr.Style "inline-size" "100%"; Attr.Style "max-inline-size" "18rem" ] [
            p [ Typography.body2; Margin.Bottom.extraSmall ] [ text label ]
            body
          ],
          attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
        )

      Grid.create (
        [
          cell "Wrapping" (div [ Typography.body1 ] [ text sentence ])
          cell
            "No wrapping; scroll to read"
            (div [
              Overflow.xAuto
              attr.tabindex "0"
              Attr.Create "role" "region"
              Attr.Create "aria-label" "Unwrapped sentence"
            ] [
              div [ Typography.body1; Typography.noWrap; Attr.Style "inline-size" "max-content" ] [
                text sentence
              ]
            ])
        ],
        attrs = [ Grid.Spacing.large ]
      )

    Helpers.codeSampleSection
      "Text Wrapping"
      (Helpers.bodyText
        "Wrapping is the default. When a line must stay unwrapped, keep it in a named, keyboard-scrollable region so its full text stays available.")
      content
      """let sentence =
  "This sentence is long enough to show how a narrow region handles text."

div [ Typography.body1; Attr.Style "max-inline-size" "18rem" ] [
  text sentence
]

div [
  Attr.Style "max-inline-size" "18rem"
  Overflow.xAuto
  attr.tabindex "0"
  Attr.Create "role" "region"
  Attr.Create "aria-label" "Unwrapped sentence"
] [
  div [
    Typography.body1
    Typography.noWrap
    Attr.Style "inline-size" "max-content"
  ] [ text sentence ]
]"""

  let private hierarchyExamples () =
    let content =
      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Gap.All.g3
        Attr.Style "max-inline-size" "65ch"
        Attr.Style "margin-inline" "auto"
      ] [
        h3 [ Typography.h4 ] [ text "A useful first step" ]
        p [ Typography.subtitle1 ] [ text "Start with one component and its state." ]
        p [ Typography.body1 ] [
          text "Build a small interaction before adding more options. "
          text "Try it with the keyboard, then check it at a narrow width."
        ]
        h4 [ Typography.h5 ] [ text "Keep the result easy to inspect" ]
        p [ Typography.body1 ] [
          text "A clear label and a short example help the next reader understand "
          text "what changes when they interact."
        ]
        p [ Typography.caption ] [ text "Illustrative article content" ]
      ]

    Helpers.codeSampleSection
      "Hierarchy Example"
      (Helpers.bodyText
        "Combine a small set of roles for a readable article. Semantic heading levels continue from this section; visual sizes remain a separate choice.")
      content
      """div [
  Flex.Flex.allSizes
  FlexDirection.Column.allSizes
  Gap.All.g3
  Attr.Style "max-inline-size" "65ch"
] [
  h3 [ Typography.h4 ] [ text "A useful first step" ]
  p [ Typography.subtitle1 ] [
    text "Start with one component and its state."
  ]
  p [ Typography.body1 ] [
    text "Build a small interaction before adding more options. "
    text "Try it with the keyboard, then check it at a narrow width."
  ]
  h4 [ Typography.h5 ] [ text "Keep the result easy to inspect" ]
  p [ Typography.body1 ] [
    text "A clear label and a short example help the next reader understand "
    text "what changes when they interact."
  ]
  p [ Typography.caption ] [ text "Illustrative article content" ]
]"""

  let private customizingFontsExamples () =
    let description =
      Helpers.bodyText
        "Weave's typography system uses role tokens for both font families and font weights. Override these to match your chosen fonts without touching individual variants."

    let content =
      div [] [
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Weave defines role tokens on :root that all typography variants inherit from:"
        ]

        div [ Margin.Bottom.small ] [
          div [ Typography.body2; Typography.Weight.bold; Margin.Bottom.extraSmall ] [ text "Font families:" ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-font-family-display — headings (h1-h6)"
          ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-font-family-body — body text, subtitles, UI controls"
          ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-font-family-mono — code and monospace contexts"
          ]
        ]

        div [ Margin.Bottom.small ] [
          div [ Typography.body2; Typography.Weight.bold; Margin.Bottom.extraSmall ] [ text "Font weights:" ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-weight-light — Typography.Weight.light"
          ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-weight-regular — body text, subtitles, captions"
          ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-weight-bold — headings and emphasized text"
          ]
        ]

        div [ Typography.caption; Margin.Bottom.extraSmall ] [
          text
            "Weight-role tokens let you remap the entire scale to match any font's available weights. Per-variant escape hatches (e.g. --typography-h6-weight) still work for fine-grained control."
        ]
      ]

    let code =
      """/* app.css, loaded after Weave. These are system font stacks. */
:root {
  --weave-font-family-display: Georgia, serif;
  --weave-font-family-body: Arial, sans-serif;
  --weave-font-family-mono: Consolas, monospace;

  --weave-weight-light: 400;
  --weave-weight-regular: 400;
  --weave-weight-bold: 700;
}

/* Override one variant independently when needed. */
:root { --typography-h6-weight: 400; }"""

    Helpers.codeSampleSectionFor "css" "Customizing Fonts" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Typography"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Typography components provide consistent text styling throughout your application with semantic meaning and visual hierarchy."
        ]

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        TypographyAppearance.render ()
        Helpers.divider ()
        weightExamples ()
        Helpers.divider ()
        fontFamilyExamples ()
        Helpers.divider ()
        customWeightExamples ()
        Helpers.divider ()
        textWrapExamples ()
        Helpers.divider ()
        hierarchyExamples ()
        Helpers.divider ()
        customizingFontsExamples ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
