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
      Helpers.bodyText
        "Weave provides 13 typography variants covering headings, subtitles, body text, and utility styles."

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
      Helpers.bodyText "Align text within its container using Typography.Align helpers."

    let content =
      div [] [
        let body alignment =
          div [ Typography.body1; Margin.Bottom.extraSmall; alignment ] [ text (sprintf "%A" alignment) ]

        body Typography.Align.left
        body Typography.Align.center
        body Typography.Align.right
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
      Helpers.bodyText
        "Prevent text from wrapping with Typography.noWrap, which truncates overflow with an ellipsis."

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
      Helpers.bodyText "Apply a brand color to any text element using the Typography.Color module."

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
            xs = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave

div [ Typography.h5; Typography.Color.primary ] [ text "Primary Color" ]
div [ Typography.h5; Typography.Color.secondary ] [ text "Secondary Color" ]
div [ Typography.h5; Typography.Color.tertiary ] [ text "Tertiary Color" ]
div [ Typography.h5; Typography.Color.error ] [ text "Error Color" ]
div [ Typography.h5; Typography.Color.warning ] [ text "Warning Color" ]
div [ Typography.h5; Typography.Color.success ] [ text "Success Color" ]
div [ Typography.h5; Typography.Color.info ] [ text "Info Color" ]"""

    Helpers.codeSampleSection "Colors" description content code

  let private hierarchyExamples () =
    let description =
      Helpers.bodyText
        "Combine heading, subtitle, body, and caption variants to establish a clear reading hierarchy."

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
        customWeightExamples ()
        Helpers.divider ()
        alignmentExamples ()
        Helpers.divider ()
        textWrapExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        hierarchyExamples ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
