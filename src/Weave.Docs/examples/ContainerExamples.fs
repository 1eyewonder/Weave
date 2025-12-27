namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave
open Weave.Container
open Weave.CssHelpers

[<JavaScript>]
module ContainerExamples =

  let options = [
    MaxWidth.ExtraSmall
    MaxWidth.Small
    MaxWidth.Medium
    MaxWidth.Large
    MaxWidth.ExtraLarge
    MaxWidth.ExtraExtraLarge
  ]

  let variantExamples () =
    div [] [
      yield!
        options
        |> List.mapi (fun i mw ->
          Container.Create(
            Body2.Create(
              sprintf "MaxWidth = %A" mw,
              attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
            ),
            maxWidth = mw,
            attrs = [
              Margin.toClasses Margin.Bottom.small |> cls
              if i % 2 = 0 then BrandColor.toAttr BrandColor.Primary
              elif i % 3 = 0 then BrandColor.toAttr BrandColor.Secondary
              else BrandColor.toAttr BrandColor.Tertiary
            ]
          ))
    ]
    |> Helpers.section
      "MaxWidth Variants"
      "Showcases each maxWidth option with a colored background for visibility."

  let fixedWidthAndGuttersExample () =
    let centeredText (displayText: string) =
      H4.Create(displayText, attrs = [ cls [ AlignSelf.toClass AlignSelf.Center ] ])

    let filler content =
      Container.Create(
        content,
        attrs = [
          BrandColor.toAttr BrandColor.Primary
          cls [ JustifyContent.toClass JustifyContent.Center ]
        ]
      )

    div [] [
      Body1.Create(
        "The Container component supports two key layout parameters:",
        attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
      )
      ul [] [
        li [] [
          strong [] [ text "fixedWidth:" ]
          text "With the Fixed property set to true the container will \"snap\" to the closest breakpoint."
        ]
        li [] [
          strong [] [ text "gutters:" ]
          text " If true (default), horizontal padding (gutters) are applied inside the container."
        ]
      ]
      Helpers.divider ()
      Body2.Create(
        "Below are combinations of fixedWidth and gutters:",
        attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
      )
      Container.Create(
        "fixedWidth = false, gutters = true (default)" |> centeredText |> filler,
        fixedWidth = false,
        gutters = true,
        attrs = [
          cls [
            yield! Margin.toClasses Margin.Bottom.small
            JustifyContent.toClass JustifyContent.Center
          ]
          SurfaceColor.toAttr SurfaceColor.BackgroundDarker
        ]
      )
      Container.Create(
        "fixedWidth = false, gutters = false" |> centeredText |> filler,
        fixedWidth = false,
        gutters = false,
        attrs = [
          cls [
            yield! Margin.toClasses Margin.Bottom.small
            JustifyContent.toClass JustifyContent.Center
          ]
          SurfaceColor.toAttr SurfaceColor.BackgroundDarker
        ]
      )
      Container.Create(
        "fixedWidth = true, gutters = true" |> centeredText |> filler,
        fixedWidth = true,
        gutters = true,
        attrs = [
          cls [
            yield! Margin.toClasses Margin.Bottom.small
            JustifyContent.toClass JustifyContent.Center
          ]
          SurfaceColor.toAttr SurfaceColor.BackgroundDarker
        ]
      )
      Container.Create(
        "fixedWidth = true, gutters = false" |> centeredText |> filler,
        fixedWidth = true,
        gutters = false,
        attrs = [
          cls [
            yield! Margin.toClasses Margin.Bottom.small
            JustifyContent.toClass JustifyContent.Center
          ]
          SurfaceColor.toAttr SurfaceColor.BackgroundDarker
        ]
      )
    ]
    |> Helpers.section
      "Fixed Width & Gutters"
      "Demonstrates all combinations of the fixedWidth and gutters parameters."

  let render () =
    Container.Create(
      div [] [
        H1.Create("Container Component", attrs = [ Margin.toClasses Margin.Bottom.medium |> cls ])
        Body1.Create(
          "The Container component centers your content and provides responsive width constraints. Use it to wrap page sections or layouts.",
          attrs = [ Margin.toClasses Margin.Bottom.medium |> cls ]
        )

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        fixedWidthAndGuttersExample ()
      ]
    )
