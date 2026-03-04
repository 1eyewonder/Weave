namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers

[<JavaScript>]
module DividerExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText "A basic horizontal divider used to separate content sections."

    let content =
      div [] [
        Body1.Div("Content above the divider", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Divider.Create()
        Body1.Div("Content below the divider", attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls ])
      ]

    let code =
      """open Weave

Divider.Create()
"""

    Helpers.codeSampleSection "Basic" description content code

  let private variantExamples () =
    let description =
      Helpers.bodyText
        "Dividers come in three variants: FullWidth (default), Inset (indented from the left), and Middle (inset from both sides)."

    let content =
      div [] [
        let row (label: string) variant =
          div [ Margin.toClasses Margin.Bottom.small |> cls ] [
            Caption.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            Divider.Create(attrs = [ Divider.Variant.toClass variant |> cl ])
          ]

        row "FullWidth" Divider.Variant.FullWidth
        row "Inset" Divider.Variant.Inset
        row "Middle" Divider.Variant.Middle
      ]

    let code =
      """open Weave
open Weave.CssHelpers

Divider.Create(
    attrs = [ Divider.Variant.toClass Divider.Variant.FullWidth |> cl ] // see here
)

Divider.Create(
    attrs = [ Divider.Variant.toClass Divider.Variant.Inset |> cl ] // see here
)

Divider.Create(
    attrs = [ Divider.Variant.toClass Divider.Variant.Middle |> cl ] // see here
)
"""

    Helpers.codeSampleSection "Variants" description content code

  let private orientationExamples () =
    let description =
      Helpers.bodyText
        "Dividers can be rendered vertically by setting orientation to Vertical. Use them inside flex containers to separate inline content."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          JustifyContent.toClass JustifyContent.Center
        ]
        Attr.Style "height" "80px"
        Attr.Style "gap" "16px"
      ] [
        Body1.Div("Left")
        Divider.Create(
          orientation = Divider.Orientation.Vertical,
          attrs = [ Attr.Style "align-self" "stretch" ]
        )
        Body1.Div("Center")
        Divider.Create(
          orientation = Divider.Orientation.Vertical,
          attrs = [ Attr.Style "align-self" "stretch" ]
        )
        Body1.Div("Right")
      ]

    let code =
      """open Weave
open Weave.CssHelpers

div [
    cls [
        Flex.Flex.allSizes
        AlignItems.toClass AlignItems.Center
    ]
    Attr.Style "height" "80px"
    Attr.Style "gap" "16px"
] [
    Body1.Div("Left")
    Divider.Create(
        orientation = Divider.Orientation.Vertical, // see here
        attrs = [ Attr.Style "align-self" "stretch" ]
    )
    Body1.Div("Center")
    Divider.Create(
        orientation = Divider.Orientation.Vertical, // see here
        attrs = [ Attr.Style "align-self" "stretch" ]
    )
    Body1.Div("Right")
]
"""

    Helpers.codeSampleSection "Orientation" description content code

  let render () =
    Container.Create(
      div [] [
        H1.Div("Divider Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "Dividers are thin lines that group content in lists and layouts.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        orientationExamples ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
