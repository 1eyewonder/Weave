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
      H1.Div("Heading 1", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H2.Div("Heading 2", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H3.Div("Heading 3", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H4.Div("Heading 4", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H5.Div("Heading 5", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      H6.Div("Heading 6", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

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
    |> Helpers.section
      "Typography Variants"
      (Helpers.bodyText "All available typography styles with their respective sizing and weights")

  let private alignmentExamples () =
    div [] [
      let body displayText alignment =
        Body1.Div(
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
    |> Helpers.section
      "Text Alignment"
      (Helpers.bodyText "Typography components support various text alignment options")

  let private textWrapExamples () =
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
    |> Helpers.section
      "Text Wrapping"
      (Helpers.bodyText "Control whether text wraps to the next line or truncates")

  let private colorExamples () =
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
    |> Helpers.section "Colors" (Helpers.bodyText "Typography can use theme colors with CSS utility classes")

  let private hierarchyExamples () =
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
    |> Helpers.section
      "Hierarchy Example"
      (Helpers.bodyText "Combining different typography styles to create visual hierarchy")

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
      ]
    )
