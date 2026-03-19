namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DividerExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText "A basic horizontal divider used to separate content sections."

    let content =
      div [] [
        Body1.div ("Content above the divider", attrs = [ Margin.Bottom.extraSmall ])
        Divider.create ()
        Body1.div ("Content below the divider", attrs = [ Margin.Top.extraSmall ])
      ]

    let code =
      """open Weave

Divider.create()
"""

    Helpers.codeSampleSection "Basic" description content code

  let private variantExamples () =
    let description =
      Helpers.bodyText
        "Dividers come in three variants: FullWidth (default), Inset (indented from the left), and Middle (inset from both sides)."

    let content =
      div [] [
        let row (label: string) variant =
          div [ Margin.Bottom.small ] [
            Caption.div (label, attrs = [ Margin.Bottom.extraSmall ])
            Divider.create (attrs = [ variant ])
          ]

        row "FullWidth" Divider.Variant.fullWidth
        row "Inset" Divider.Variant.inset
        row "Middle" Divider.Variant.middle
      ]

    let code =
      """open Weave


Divider.create(
    attrs = [ Divider.Variant.fullWidth ] // see here
)

Divider.create(
    attrs = [ Divider.Variant.inset ] // see here
)

Divider.create(
    attrs = [ Divider.Variant.middle ] // see here
)
"""

    Helpers.codeSampleSection "Variants" description content code

  let private orientationExamples () =
    let description =
      Helpers.bodyText
        "Dividers can be rendered vertically by setting orientation to Vertical. Use them inside flex containers to separate inline content."

    let content =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        JustifyContent.center
        Attr.Style "height" "80px"
        Attr.Style "gap" "16px"
      ] [
        Body1.div ("Left")
        Divider.create (attrs = [ Divider.Orientation.vertical; Attr.Style "align-self" "stretch" ])
        Body1.div ("Center")
        Divider.create (attrs = [ Divider.Orientation.vertical; Attr.Style "align-self" "stretch" ])
        Body1.div ("Right")
      ]

    let code =
      """open Weave


div [
    cls [
        Flex.Flex.allSizes
        AlignItems.center
    ]
    Attr.Style "height" "80px"
    Attr.Style "gap" "16px"
] [
    Body1.div("Left")
    Divider.create(attrs = [ Divider.Orientation.vertical; Attr.Style "align-self" "stretch" ])
    Body1.div("Center")
    Divider.create(attrs = [ Divider.Orientation.vertical; Attr.Style "align-self" "stretch" ])
    Body1.div("Right")
]
"""

    Helpers.codeSampleSection "Orientation" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Divider"
        Body1.div (
          "Dividers are thin lines that group content in lists and layouts.",
          attrs = [ Margin.Bottom.extraSmall ]
        )

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        orientationExamples ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
