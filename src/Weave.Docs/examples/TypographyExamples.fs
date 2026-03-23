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
    let description =
      Helpers.bodyText "All available typography styles with their respective sizing and weights"

    let content =
      div [] [
        h1 [ Typography.h1; Margin.Bottom.extraSmall ] [ text "Heading 1" ]
        h2 [ Typography.h2; Margin.Bottom.extraSmall ] [ text "Heading 2" ]
        h3 [ Typography.h3; Margin.Bottom.extraSmall ] [ text "Heading 3" ]
        h4 [ Typography.h4; Margin.Bottom.extraSmall ] [ text "Heading 4" ]
        h5 [ Typography.h5; Margin.Bottom.extraSmall ] [ text "Heading 5" ]
        h6 [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Heading 6" ]

        div [ Typography.subtitle1; Margin.Bottom.extraSmall ] [
          text "Subtitle 1 - Slightly larger secondary text"
        ]
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Subtitle 2 - Smaller secondary text" ]

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Body 1 - This is the default body text style. It's used for most content and provides good readability for longer paragraphs of text."
        ]
        div [ Typography.body2; Margin.Bottom.extraSmall ] [
          text
            "Body 2 - A slightly smaller body text variant that can be used for less prominent content or to create visual hierarchy."
        ]

        div [ Typography.button; Margin.Bottom.extraSmall ] [ text "BUTTON TEXT" ]
        div [ Typography.caption; Margin.Bottom.extraSmall ] [
          text "Caption text - Small text for annotations"
        ]
        div [ Typography.overline; Margin.Bottom.extraSmall ] [ text "OVERLINE TEXT" ]
      ]

    let code =
      """open Weave


h1 [ Typography.h1 ] [ text "Heading 1" ]
h2 [ Typography.h2 ] [ text "Heading 2" ]
h3 [ Typography.h3 ] [ text "Heading 3" ]
h4 [ Typography.h4 ] [ text "Heading 4" ]
h5 [ Typography.h5 ] [ text "Heading 5" ]
h6 [ Typography.h6 ] [ text "Heading 6" ]
div [ Typography.subtitle1 ] [ text "Subtitle 1 - Slightly larger secondary text" ]
div [ Typography.subtitle2 ] [ text "Subtitle 2 - Smaller secondary text" ]
div [ Typography.body1 ] [ text "Body 1 - This is the default body text style." ]
div [ Typography.body2 ] [ text "Body 2 - A slightly smaller body text variant." ]
div [ Typography.button ] [ text "BUTTON TEXT" ]
div [ Typography.caption ] [ text "Caption text - Small text for annotations" ]
div [ Typography.overline ] [ text "OVERLINE TEXT" ]
"""

    Helpers.codeSampleSection "Typography Variants" description content code

  let private weightExamples () =
    let description =
      Helpers.bodyText
        "Override the default font weight of any typography variant using the Weight module. Each variant has a default weight (e.g., H1 defaults to light/300), but you can change it via attrs."

    let content =
      div [] [
        div [ Typography.h4; Margin.Bottom.extraSmall ] [ text "Default H4 (weight 400)" ]
        div [ Typography.h4; Typography.Weight.light; Margin.Bottom.extraSmall ] [
          text "Light H4 (weight 300)"
        ]
        div [ Typography.h4; Typography.Weight.medium; Margin.Bottom.extraSmall ] [
          text "Medium H4 (weight 500)"
        ]
        div [ Typography.h4; Typography.Weight.bold; Margin.Bottom.extraSmall ] [
          text "Bold H4 (weight 700)"
        ]

        Divider.create (attrs = [ Margin.Bottom.small; Margin.Top.small ])

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Default Body1 (weight 400)" ]
        div [ Typography.body1; Typography.Weight.light; Margin.Bottom.extraSmall ] [
          text "Light Body1 (weight 300)"
        ]
        div [ Typography.body1; Typography.Weight.medium; Margin.Bottom.extraSmall ] [
          text "Medium Body1 (weight 500)"
        ]
        div [ Typography.body1; Typography.Weight.bold; Margin.Bottom.extraSmall ] [
          text "Bold Body1 (weight 700)"
        ]
      ]

    let code =
      """open Weave


// Override any variant's default weight via attrs
div [ Typography.h4 ] [ text "Default H4 (weight 400)" ]
div [ Typography.h4; Typography.Weight.light ] [ text "Light H4 (weight 300)" ]
div [ Typography.h4; Typography.Weight.medium ] [ text "Medium H4 (weight 500)" ]
div [ Typography.h4; Typography.Weight.bold ] [ text "Bold H4 (weight 700)" ]

div [ Typography.body1 ] [ text "Default Body1 (weight 400)" ]
div [ Typography.body1; Typography.Weight.light ] [ text "Light Body1 (weight 300)" ]
div [ Typography.body1; Typography.Weight.medium ] [ text "Medium Body1 (weight 500)" ]
div [ Typography.body1; Typography.Weight.bold ] [ text "Bold Body1 (weight 700)" ]
"""

    Helpers.codeSampleSection "Font Weights" description content code

  let private fontFamilyExamples () =
    let description =
      Helpers.bodyText
        "Apply a font-role family to any element using the Family module. This is useful when you want to apply the display, body, or monospace font to an element that doesn't normally use it."

    let content =
      div [] [
        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Default body1 (uses body font family)" ]
        div [ Typography.body1; Typography.Family.display; Margin.Bottom.extraSmall ] [
          text "Body1 with display font family"
        ]

        Divider.create (attrs = [ Margin.Bottom.small; Margin.Top.small ])

        div [ Typography.h5; Margin.Bottom.extraSmall ] [ text "Default H5 (uses display font family)" ]
        div [ Typography.h5; Typography.Family.body; Margin.Bottom.extraSmall ] [
          text "H5 with body font family"
        ]

        Divider.create (attrs = [ Margin.Bottom.small; Margin.Top.small ])

        div [ Typography.body2; Margin.Bottom.extraSmall ] [ text "Inline code example:" ]
        div [ Typography.body1 ] [
          text "Use the "
          span [ Typography.Family.mono ] [ text "Typography.Family.mono" ]
          text " attr for inline code snippets."
        ]
      ]

    let code =
      """open Weave


// Apply the display font to body text
div [ Typography.body1; Typography.Family.display ] [ text "Body1 with display font" ]

// Apply the body font to a heading
div [ Typography.h5; Typography.Family.body ] [ text "H5 with body font" ]

// Apply monospace for inline code
div [ Typography.body1 ] [
    text "Use the "
    span [ Typography.Family.mono ] [ text "myFunction" ]
    text " helper for inline code."
]
"""

    Helpers.codeSampleSection "Font Family" description content code

  let private customWeightExamples () =
    let description =
      Helpers.bodyText
        "Weave's typography scale uses CSS custom properties for all settings. You can override any variant's weight (or size, line-height, letter-spacing) globally by redefining the custom property in your own CSS."

    let content =
      div [] [
        div [ Typography.h3; Margin.Bottom.extraSmall ] [ text "Default H3 (weight 400)" ]

        div [ Attr.Style "--typography-h3-weight" "700" ] [
          div [ Typography.h3; Margin.Bottom.extraSmall ] [ text "H3 with --typography-h3-weight: 700" ]
        ]

        Divider.create (attrs = [ Margin.Bottom.small; Margin.Top.small ])

        div [ Typography.caption; Margin.Bottom.extraSmall ] [
          text
            "Tip: To change a variant's weight globally, override the CSS custom property in your app's stylesheet:"
        ]
        div [ Typography.caption; Typography.Weight.bold ] [ text ":root { --typography-h1-weight: 400; }" ]
      ]

    let code =
      """/* In your app's CSS, override the custom property: */
:root {
    --typography-h1-weight: 400;   /* make H1 regular instead of light */
    --typography-h3-weight: 700;   /* make H3 bold instead of regular */
}

/* Or scope it to a container: */
.my-hero-section {
    --typography-h1-weight: 700;   /* bold H1 only in hero */
}

/* Available custom properties per variant:
   --typography-{variant}-weight
   --typography-{variant}-size
   --typography-{variant}-family
   --typography-{variant}-lineheight
   --typography-{variant}-letterspacing
   --typography-{variant}-text-transform
*/
"""

    Helpers.codeSampleSection "Customizing via CSS Custom Properties" description content code

  let private alignmentExamples () =
    let description =
      Helpers.bodyText "Demonstration of text alignment options available in typography components"

    let content =
      div [] [
        let body label alignment =
          div [ Typography.body1; Margin.Bottom.extraSmall; alignment ] [ text (sprintf "%s" label) ]

        body "Left" Typography.Align.left
        body "Center" Typography.Align.center
        body "Right" Typography.Align.right
      ]

    let code =
      """open Weave


div [ Typography.body1; Typography.Align.left ] [ text "Left" ]

div [ Typography.body1; Typography.Align.center ] [ text "Center" ]

div [ Typography.body1; Typography.Align.right ] [ text "Right" ]
"""

    Helpers.codeSampleSection "Text Alignment" description content code

  let private textWrapExamples () =
    let description =
      Helpers.bodyText "Examples of typography with and without text wrapping enabled"

    let content =
      div [] [
        div [ Margin.Bottom.extraSmall ] [
          div [ Typography.body2; Margin.Bottom.extraSmall ] [ text "With text wrapping (default):" ]
          div [
            Attr.Style "max-width" "300px"
            Attr.Style "padding" "8px"
            Attr.Style "border" "1px solid var(--palette-divider)"
            BorderRadius.All.small
          ] [
            div [ Typography.body1 ] [
              text
                "This is a very long line of text that will wrap naturally when it reaches the edge of its container."
            ]
          ]
        ]

        div [] [
          div [ Typography.body2; Margin.Bottom.extraSmall ] [ text "Without text wrapping (nowrap):" ]
          div [
            Attr.Style "max-width" "300px"
            Attr.Style "padding" "8px"
            Attr.Style "border" "1px solid var(--palette-divider)"
            Attr.Style "overflow" "hidden"
            BorderRadius.All.small
          ] [
            div [ Typography.body1; Typography.noWrap ] [
              text "This is a very long line of text that will not wrap and will be cut off."
            ]
          ]
        ]
      ]

    let code =
      """open Weave


// With text wrapping (default)
div [ Typography.body1 ] [
    text "This is a very long line of text that will wrap naturally."
]

// Without text wrapping
div [ Typography.body1; Typography.noWrap ] [
    text "This is a very long line of text that will not wrap."
]
"""

    Helpers.codeSampleSection "Text Wrapping" description content code

  let private colorExamples () =
    let description =
      Helpers.bodyText "Typography components can utilize theme colors via CSS utility classes"

    let content =
      let colors = [
        "Primary", Typography.Color.primary
        "Secondary", Typography.Color.secondary
        "Tertiary", Typography.Color.tertiary
        "Success", Typography.Color.success
        "Error", Typography.Color.error
        "Warning", Typography.Color.warning
        "Info", Typography.Color.info
      ]

      Grid.create (
        colors
        |> List.map (fun (label, colorAttr) ->
          GridItem.create (
            div [ Typography.h5; colorAttr ] [ text (sprintf "%s Color" label) ],
            attrs = [ GridItem.Span.six; GridItem.Span.Medium.four ]
          ))
      )

    let code =
      """open Weave


let colors = [
    "Primary", Typography.Color.primary
    "Secondary", Typography.Color.secondary
    "Tertiary", Typography.Color.tertiary
    "Success", Typography.Color.success
    "Error", Typography.Color.error
    "Warning", Typography.Color.warning
    "Info", Typography.Color.info
]

colors
|> List.map (fun (label, colorAttr) ->
    div [ Typography.h5; colorAttr ] [
        text (sprintf "%s Color" label)
    ]
)
"""

    Helpers.codeSampleSection "Colors" description content code

  let private hierarchyExamples () =
    let description =
      Helpers.bodyText "Combining different typography styles to create visual hierarchy"

    let content =
      div [] [
        div [ Typography.h2; Margin.Bottom.extraSmall ] [ text "Article Title" ]
        div [ Typography.subtitle1; Margin.Bottom.extraSmall ] [ text "A subtitle that provides context" ]

        div [ Typography.h4; Margin.Bottom.extraSmall ] [ text "Section Heading" ]
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris."
        ]
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."
        ]

        div [ Typography.h5; Margin.Bottom.extraSmall ] [ text "Subsection" ]
        div [ Typography.body2; Margin.Bottom.extraSmall ] [
          text
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        ]
        div [ Typography.caption ] [ text "Last updated: December 20, 2025" ]
      ]

    let code =
      """open Weave


div [ Typography.h2 ] [ text "Article Title" ]
div [ Typography.subtitle1 ] [ text "A subtitle that provides context" ]
div [ Typography.h4 ] [ text "Section Heading" ]
div [ Typography.body1 ] [ text "Lorem ipsum dolor sit amet, consectetur adipiscing elit." ]
div [ Typography.body1 ] [ text "Duis aute irure dolor in reprehenderit in voluptate velit." ]
div [ Typography.h5 ] [ text "Subsection" ]
div [ Typography.body2 ] [ text "Excepteur sint occaecat cupidatat non proident." ]
div [ Typography.caption ] [ text "Last updated: December 20, 2025" ]
"""

    Helpers.codeSampleSection "Hierarchy Example" description content code

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
            text "--weave-weight-light — display headings (h1, h2)"
          ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-weight-regular — body text, subtitles, captions"
          ]
          div [ Typography.body2; Margin.Bottom.extraSmall ] [
            text "--weave-weight-bold — emphasized UI text (h6, button, overline)"
          ]
        ]

        div [ Typography.caption; Margin.Bottom.extraSmall ] [
          text
            "Weight-role tokens let you remap the entire scale to match any font's available weights. Per-variant escape hatches (e.g. --typography-h6-weight) still work for fine-grained control."
        ]
      ]

    let code =
      """/* 1. Load your custom fonts (e.g. in your HTML <head>): */
<link href="https://fonts.googleapis.com/css2?family=Playfair+Display:wght@400;700&family=Inter:wght@300;400;700&display=swap" rel="stylesheet" />

/* 2. Override the role tokens in CSS: */
:root {
    /* Font families */
    --weave-font-family-display: "Playfair Display", serif;
    --weave-font-family-body: "Inter", sans-serif;
    --weave-font-family-mono: "JetBrains Mono", monospace;

    /* Remap weights to match your font's available weights */
    --weave-weight-light: 400;   /* Inter has no 300 — use 400 */
    --weave-weight-regular: 400;
    --weave-weight-bold: 700;
}

/* 3. Or override programmatically in F#: */
open Weave.Theming

let theme =
    ThemeBuilder.empty
    |> ThemeBuilder.withDisplayFont "\"Playfair Display\", serif"
    |> ThemeBuilder.withBodyFont "\"Inter\", sans-serif"
    |> ThemeBuilder.withWeightLight "400"
    |> ThemeBuilder.withWeightBold "700"

Theming.initialize theme Light

(* Per-variant escape hatch — override a single variant: *)
(* :root { --typography-h6-weight: 500; } *)
"""

    Helpers.codeSampleSection "Customizing Fonts" description content code

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
        weightExamples ()
        Helpers.divider ()
        fontFamilyExamples ()
        Helpers.divider ()
        customWeightExamples ()
        Helpers.divider ()
        alignmentExamples ()
        Helpers.divider ()
        textWrapExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        hierarchyExamples ()
        Helpers.divider ()
        customizingFontsExamples ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
