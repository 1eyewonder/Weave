namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.CssHelpers

[<JavaScript>]
module WeaveStylingExamples =

  let private densitySection () =
    let description =
      Helpers.bodyText
        "Density controls the spacing and sizing of components that support it, on a three-step scale: Compact, Standard, and Spacious. Standard is the default set on the root element. Apply the density class to any ancestor element to override all descendant components. Pass it in a component's attrs to override that instance only."

    let content =
      let col density =
        let label = sprintf "%A" density

        div [ cl (Density.toClass density) ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          div [] [
            Button.Create(
              text "Filled",
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            )
          ]
          div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
            Button.Create(
              text "Outlined",
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Outlined |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            )
          ]
          div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
            Button.Create(
              text "Text",
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Text |> Button.Variant.toClass |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            )
          ]
        ]

      Grid.Create(
        [
          GridItem.Create(col Density.Compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Standard, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Spacious, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.toClass AlignItems.Start |> cl ]
      )

    let code =
      """open Weave
open Weave.CssHelpers

// Container-level: all children inherit the density via CSS cascade
div [ cl (Density.toClass Density.Compact) ] [ // see here
    Button.Create(
        text "Filled",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.Filled |> Button.Variant.toClass |> cl
            BrandColor.Primary |> Button.Color.toClass |> cl
        ]
    )
    Button.Create(
        text "Outlined",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.Outlined |> Button.Variant.toClass |> cl
            BrandColor.Primary |> Button.Color.toClass |> cl
        ]
    )
]

// Per-instance: pass the density class in attrs to set it on one component
Button.Create(
    text "Spacious",
    onClick = (fun () -> ()),
    attrs = [
        cl (Density.toClass Density.Spacious) // see here
        Button.Variant.Filled |> Button.Variant.toClass |> cl
        BrandColor.Primary |> Button.Color.toClass |> cl
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.Create(
      div [] [
        H1.Div("Density", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "Utility which controls spacing and sizing across components.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        densitySection ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
