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
        H1.h1 ("Heading 1", attrs = [ Margin.Bottom.extraSmall ])
        H2.h2 ("Heading 2", attrs = [ Margin.Bottom.extraSmall ])
        H3.h3 ("Heading 3", attrs = [ Margin.Bottom.extraSmall ])
        H4.h4 ("Heading 4", attrs = [ Margin.Bottom.extraSmall ])
        H5.h5 ("Heading 5", attrs = [ Margin.Bottom.extraSmall ])
        H6.h6 ("Heading 6", attrs = [ Margin.Bottom.extraSmall ])

        Subtitle1.div ("Subtitle 1 - Slightly larger secondary text", attrs = [ Margin.Bottom.extraSmall ])
        Subtitle2.div ("Subtitle 2 - Smaller secondary text", attrs = [ Margin.Bottom.extraSmall ])

        Body1.div (
          "Body 1 - This is the default body text style. It's used for most content and provides good readability for longer paragraphs of text.",
          attrs = [ Margin.Bottom.extraSmall ]
        )
        Body2.div (
          "Body 2 - A slightly smaller body text variant that can be used for less prominent content or to create visual hierarchy.",
          attrs = [ Margin.Bottom.extraSmall ]
        )

        Typography.ButtonText.div ("BUTTON TEXT", attrs = [ Margin.Bottom.extraSmall ])
        Caption.div ("Caption text - Small text for annotations", attrs = [ Margin.Bottom.extraSmall ])
        Overline.div ("OVERLINE TEXT", attrs = [ Margin.Bottom.extraSmall ])
      ]

    let code =
      """open Weave


H1.h1("Heading 1")
H2.h2("Heading 2")
H3.h3("Heading 3")
H4.h4("Heading 4")
H5.h5("Heading 5")
H6.h6("Heading 6")
Subtitle1.div("Subtitle 1 - Slightly larger secondary text")
Subtitle2.div("Subtitle 2 - Smaller secondary text")
Body1.div("Body 1 - This is the default body text style. It's used for most content and provides good readability for longer paragraphs of text.")
Body2.div("Body 2 - A slightly smaller body text variant that can be used for less prominent content or to create visual hierarchy.")
Typography.ButtonText.div("BUTTON TEXT")
Caption.div("Caption text - Small text for annotations")
Overline.div("OVERLINE TEXT")
"""

    Helpers.codeSampleSection "Typography Variants" description content code

  let private alignmentExamples () =
    let description =
      Helpers.bodyText "Demonstration of text alignment options available in typography components"

    let content =
      div [] [
        let body alignment =
          Body1.div (
            text (sprintf "%A" alignment),
            attrs = [
              Margin.Bottom.extraSmall
              alignment // TODO: dynamic alignment
            ]
          )

        body Typography.Align.left
        body Typography.Align.center
        body Typography.Align.right
      ]

    let code =
      """open Weave


Body1.div(
    text "Left",
    attrs = [ Typography.Align.left ] // see here
)

Body1.div(
    text "Center",
    attrs = [ Typography.Align.center ] // see here
)

Body1.div(
    text "Right",
    attrs = [ Typography.Align.right ] // see here
)
"""

    Helpers.codeSampleSection "Text Alignment" description content code

  let private textWrapExamples () =
    let description =
      Helpers.bodyText "Examples of typography with and without text wrapping enabled"

    let content =
      div [] [
        div [ Margin.Bottom.extraSmall ] [
          Body2.div ("With text wrapping (default):", attrs = [ Margin.Bottom.extraSmall ])
          div [
            Attr.Style "max-width" "300px"
            Attr.Style "padding" "8px"
            Attr.Style "border" "1px solid var(--palette-divider)"
            BorderRadius.All.small
          ] [
            Body1.div (
              "This is a very long line of text that will wrap naturally when it reaches the edge of its container.",
              textWrap = View.Const true
            )
          ]
        ]

        div [] [
          Body2.div ("Without text wrapping (nowrap):", attrs = [ Margin.Bottom.extraSmall ])
          div [
            Attr.Style "max-width" "300px"
            Attr.Style "padding" "8px"
            Attr.Style "border" "1px solid var(--palette-divider)"
            Attr.Style "overflow" "hidden"
            BorderRadius.All.small
          ] [
            Body1.div (
              "This is a very long line of text that will not wrap and will be cut off.",
              textWrap = View.Const false
            )
          ]
        ]
      ]

    let code =
      """open Weave


Body1.div(
    "This is a very long line of text that will wrap naturally when it reaches the edge of its container.",
    textWrap = View.Const true // see here
)

Body1.div(
    "This is a very long line of text that will wrap naturally when it reaches the edge of its container.",
    textWrap = View.Const false // see here
)
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
            H5.div (sprintf "%s Color" label, attrs = [ colorAttr ]),
            xs = Grid.Width.create 6,
            md = Grid.Width.create 4
          )),
        spacing = Grid.GutterSpacing.create 2
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
    H5.div(
        sprintf "%s Color" label,
        attrs = [ colorAttr ] // see here
    )
)
"""

    Helpers.codeSampleSection "Colors" description content code

  let private hierarchyExamples () =
    let description =
      Helpers.bodyText "Combining different typography styles to create visual hierarchy"

    let content =
      div [] [
        H2.div ("Article Title", attrs = [ Margin.Bottom.extraSmall ])
        Subtitle1.div ("A subtitle that provides context", attrs = [ Margin.Bottom.extraSmall ])

        H4.div ("Section Heading", attrs = [ Margin.Bottom.extraSmall ])
        Body1.div (
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.",
          attrs = [ Margin.Bottom.extraSmall ]
        )
        Body1.div (
          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
          attrs = [ Margin.Bottom.extraSmall ]
        )

        H5.div ("Subsection", attrs = [ Margin.Bottom.extraSmall ])
        Body2.div (
          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
          attrs = [ Margin.Bottom.extraSmall ]
        )
        Caption.div ("Last updated: December 20, 2025")
      ]

    let code =
      """open Weave


H2.div("Article Title")
Subtitle1.div("A subtitle that provides context")
H4.div("Section Heading")
Body1.div("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.")
Body1.div("Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.")
H5.div("Subsection")
Body2.div("Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")
Caption.div("Last updated: December 20, 2025")
"""

    Helpers.codeSampleSection "Hierarchy Example" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Typography"
        Body1.div (
          "Typography components provide consistent text styling throughout your application with semantic meaning and visual hierarchy.",
          attrs = [ Margin.Bottom.extraSmall ]
        )

        Helpers.divider ()
        variantExamples ()
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
