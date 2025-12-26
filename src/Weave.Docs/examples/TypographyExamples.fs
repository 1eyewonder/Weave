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
    div [] [
      H1.Create("Heading 1", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H2.Create("Heading 2", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H3.Create("Heading 3", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H4.Create("Heading 4", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H5.Create("Heading 5", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H6.Create("Heading 6", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

      Subtitle1.Create(
        "Subtitle 1 - Slightly larger secondary text",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )
      Subtitle2.Create(
        "Subtitle 2 - Smaller secondary text",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )

      Body1.Create(
        "Body 1 - This is the default body text style. It's used for most content and provides good readability for longer paragraphs of text.",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )
      Body2.Create(
        "Body 2 - A slightly smaller body text variant that can be used for less prominent content or to create visual hierarchy.",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )

      Typography.Button.Create("BUTTON TEXT", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Caption.Create(
        "Caption text - Small text for annotations",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )
      Overline.Create("OVERLINE TEXT", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
    ]
    |> Helpers.section
      "Typography Variants"
      "All available typography styles with their respective sizing and weights"

  let private alignmentExamples () =
    div [] [
      let body displayText alignment =
        Body1.Create(
          View.Const displayText,
          attrs = [
            Margin.toClasses Margin.Bottom.extraSmall |> cls
            Typography.Align.toClass alignment |> cl
          ]
        )

      body "Left aligned text (default)" Typography.Align.Left
      body "Center aligned text" Typography.Align.Center
      body "Right aligned text" Typography.Align.Right

      body
        "Justified text - This is a longer piece of text that will demonstrate how justify alignment works when text wraps across multiple lines. Notice how both edges align evenly."
        Typography.Align.Justify
    ]
    |> Helpers.section "Text Alignment" "Typography components support various text alignment options"

  let private textWrapExamples () =
    div [] [
      div [ Margin.toClasses Margin.Bottom.extraSmall |> cls ] [
        Body2.Create(
          "With text wrapping (default):",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        div [
          Attr.Style "max-width" "300px"
          Attr.Style "padding" "8px"
          Attr.Style "border" "1px solid var(--palette-divider)"
          BorderRadius.toClass BorderRadius.All.small |> cl
        ] [
          Body1.Create(
            "This is a very long line of text that will wrap naturally when it reaches the edge of its container.",
            textWrap = View.Const true
          )
        ]
      ]

      div [] [
        Body2.Create(
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
          Body1.Create(
            "This is a very long line of text that will not wrap and will be cut off.",
            textWrap = View.Const false
          )
        ]
      ]
    ]
    |> Helpers.section "Text Wrapping" "Control whether text wraps to the next line or truncates"

  let private colorExamples () =
    Grid.Create(
      [
        let item color =
          GridItem.Create(
            H5.Create(sprintf "%A Color" color, attrs = [ Typography.Color.toClass color |> cl ]),
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
    |> Helpers.section "Colors" "Typography can use theme colors with CSS utility classes"

  let private hierarchyExamples () =
    div [] [
      H2.Create("Article Title", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Subtitle1.Create(
        "A subtitle that provides context",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )

      H4.Create("Section Heading", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Body1.Create(
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )
      Body1.Create(
        "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )

      H5.Create("Subsection", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Body2.Create(
        "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )
      Caption.Create("Last updated: December 20, 2025")
    ]
    |> Helpers.section "Hierarchy Example" "Combining different typography styles to create visual hierarchy"

  let render () =
    Container.Create(
      div [] [
        H1.Create("Typography Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(
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
