namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open WebSharper.JavaScript

[<JavaScript>]
module TypographyExamples =

  let private variantExamples () =
    let description =
      Helpers.bodyText "All available typography styles with their respective sizing and weights"

    let content =
      div [] [
        H1.H1("Heading 1", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        H2.H2("Heading 2", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        H3.H3("Heading 3", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        H4.H4("Heading 4", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        H5.H5("Heading 5", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        H6.H6("Heading 6", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

        Subtitle1.Div(
          "Subtitle 1 - Slightly larger secondary text",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Subtitle2.Div(
          "Subtitle 2 - Smaller secondary text",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Body1.Div(
          "Body 1 - This is the default body text style. It's used for most content and provides good readability for longer paragraphs of text.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Body2.Div(
          "Body 2 - A slightly smaller body text variant that can be used for less prominent content or to create visual hierarchy.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Typography.Button.Div("BUTTON TEXT", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Caption.Div(
          "Caption text - Small text for annotations",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Overline.Div("OVERLINE TEXT", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      ]

    let code =
      """open Weave
open Weave.CssHelpers

H1.H1("Heading 1")
H2.H2("Heading 2")
H3.H3("Heading 3")
H4.H4("Heading 4")
H5.H5("Heading 5")
H6.H6("Heading 6")
Subtitle1.Div("Subtitle 1 - Slightly larger secondary text")
Subtitle2.Div("Subtitle 2 - Smaller secondary text")
Body1.Div("Body 1 - This is the default body text style. It's used for most content and provides good readability for longer paragraphs of text.")
Body2.Div("Body 2 - A slightly smaller body text variant that can be used for less prominent content or to create visual hierarchy.")
Typography.Button.Div("BUTTON TEXT")
Caption.Div("Caption text - Small text for annotations")
Overline.Div("OVERLINE TEXT")
"""

    Helpers.codeSampleSection "Typography Variants" description content code

  let private alignmentExamples () =
    let description =
      Helpers.bodyText "Demonstration of text alignment options available in typography components"

    let content =
      div [] [
        let body alignment =
          Body1.Div(
            text (sprintf "%A" alignment),
            attrs = [
              cls [
                yield! Margin.toClasses Margin.Bottom.extraSmall
                Typography.Align.toClass alignment
              ]
            ]
          )

        body Typography.Align.Left
        body Typography.Align.Center
        body Typography.Align.Right
      ]

    let code =
      """open Weave
open Weave.CssHelpers

Body1.Div(
    text "Left",
    attrs = [ Typography.Align.toClass Typography.Align.Left |> cl ] // see here
)

Body1.Div(
    text "Center",
    attrs = [ Typography.Align.toClass Typography.Align.Center |> cl ] // see here
)

Body1.Div(
    text "Right",
    attrs = [ Typography.Align.toClass Typography.Align.Right |> cl ] // see here
)
"""

    Helpers.codeSampleSection "Text Alignment" description content code

  let private textWrapExamples () =
    let description =
      Helpers.bodyText "Examples of typography with and without text wrapping enabled"

    let content =
      div [] [
        div [ Margin.toClasses Margin.Bottom.extraSmall |> cls ] [
          Body2.Div(
            "With text wrapping (default):",
            attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
          )
          div [
            Attr.Style "max-width" "300px"
            Attr.Style "padding" "8px"
            Attr.Style "border" "1px solid var(--palette-divider)"
            BorderRadius.toClass BorderRadius.All.small |> cl
          ] [
            Body1.Div(
              "This is a very long line of text that will wrap naturally when it reaches the edge of its container.",
              textWrap = View.Const true
            )
          ]
        ]

        div [] [
          Body2.Div(
            "Without text wrapping (nowrap):",
            attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
          )
          div [
            Attr.Style "max-width" "300px"
            Attr.Style "padding" "8px"
            Attr.Style "border" "1px solid var(--palette-divider)"
            Attr.Style "overflow" "hidden"
            BorderRadius.toClass BorderRadius.All.small |> cl
          ] [
            Body1.Div(
              "This is a very long line of text that will not wrap and will be cut off.",
              textWrap = View.Const false
            )
          ]
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers

Body1.Div(
    "This is a very long line of text that will wrap naturally when it reaches the edge of its container.",
    textWrap = View.Const true // see here
)

Body1.Div(
    "This is a very long line of text that will wrap naturally when it reaches the edge of its container.",
    textWrap = View.Const false // see here
)
"""

    Helpers.codeSampleSection "Text Wrapping" description content code

  let private colorExamples () =
    let description =
      Helpers.bodyText "Typography components can utilize theme colors via CSS utility classes"

    let content =
      Grid.Create(
        [
          let item color =
            GridItem.Create(
              H5.Div(sprintf "%A Color" color, attrs = [ Typography.Color.toClass color |> cl ]),
              xs = Grid.Width.create 6,
              md = Grid.Width.create 4
            )

          item BrandColor.Primary
          item BrandColor.Secondary
          item BrandColor.Tertiary
          item BrandColor.Success
          item BrandColor.Error
          item BrandColor.Warning
          item BrandColor.Info
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

let item color =
    H5.Div(
        sprintf "%A Color" color,
        attrs = [ Typography.Color.toClass color |> cl ] // see here
    )

item BrandColor.Primary
item BrandColor.Secondary
item BrandColor.Tertiary
item BrandColor.Success
item BrandColor.Error
item BrandColor.Warning
item BrandColor.Info
"""

    Helpers.codeSampleSection "Colors" description content code

  let private hierarchyExamples () =
    let description =
      Helpers.bodyText "Combining different typography styles to create visual hierarchy"

    let content =
      div [] [
        H2.Div("Article Title", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Subtitle1.Div(
          "A subtitle that provides context",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        H4.Div("Section Heading", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Body1.Div(
          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        H5.Div("Subsection", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body2.Div(
          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Caption.Div("Last updated: December 20, 2025")
      ]

    let code =
      """open Weave
open Weave.CssHelpers

H2.Div("Article Title")
Subtitle1.Div("A subtitle that provides context")
H4.Div("Section Heading")
Body1.Div("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.")
Body1.Div("Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.")
H5.Div("Subsection")
Body2.Div("Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")
Caption.Div("Last updated: December 20, 2025")
"""

    Helpers.codeSampleSection "Hierarchy Example" description content code

  let render () =
    Container.Create(
      div [] [
        H1.Div("Typography Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "Typography components provide consistent text styling throughout your application with semantic meaning and visual hierarchy.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
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
      maxWidth = Container.MaxWidth.Large
    )
