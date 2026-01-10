namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
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

  let centeredText (displayText: string) =
    H6.Div(displayText, attrs = [ cls [ AlignSelf.toClass AlignSelf.Center ] ])

  let variantExamples () =
    div [] [
      yield!
        options
        |> List.mapi (fun i mw ->
          Container.Create(
            sprintf "MaxWidth = %A" mw |> centeredText,
            maxWidth = mw,
            attrs = [
              Attr.Style "min-height" "1vh"
              cls [
                yield! Margin.toClasses Margin.Bottom.small
                AlignItems.toClass AlignItems.Center
                JustifyContent.toClass JustifyContent.Center
              ]
              if i % 2 = 0 then
                BrandColor.toBackgroundColor BrandColor.Primary
              elif i % 3 = 0 then
                BrandColor.toBackgroundColor BrandColor.Secondary
              else
                BrandColor.toBackgroundColor BrandColor.Tertiary
            ]
          ))
    ]
    |> Helpers.section
      "MaxWidth Variants"
      (Helpers.bodyText "Showcases each maxWidth option with a colored background for visibility.")

  let fixedWidthAndGuttersExample () =
    let description =
      div [ cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] ] [
        Body1.Div(
          "The Container component supports two key layout parameters:",
          attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
        )

        Subtitle2.Div("Fixed Width")

        Body1.Div(
          "With the Fixed property set to true the container will \"snap\" to the closest breakpoint.",
          attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
        )

        Subtitle2.Div("Gutters")

        Body1.Div(
          "If true, horizontal padding (gutters) are applied inside the container.",
          attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
        )
      ]

    let filler content =
      Container.Create(
        content,
        attrs = [
          Attr.Style "min-height" "1vh"
          BrandColor.toBackgroundColor BrandColor.Primary
          cls [ JustifyContent.toClass JustifyContent.Center ]
        ]
      )

    div [] [
      Container.Create(
        "fixedWidth = false, gutters = true (default)" |> centeredText |> filler,
        fixedWidth = false,
        gutters = true,
        attrs = [
          cls [
            yield! Margin.toClasses Margin.Bottom.small
            JustifyContent.toClass JustifyContent.Center
          ]
          SurfaceColor.toBackgroundColor SurfaceColor.Background
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
          SurfaceColor.toBackgroundColor SurfaceColor.Background
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
          SurfaceColor.toBackgroundColor SurfaceColor.Background
        ]
      )
      Container.Create(
        "fixedWidth = true, gutters = false" |> centeredText |> filler,
        fixedWidth = true,
        gutters = false,
        attrs = [
          cls [ JustifyContent.toClass JustifyContent.Center ]
          SurfaceColor.toBackgroundColor SurfaceColor.Background
        ]
      )
    ]
    |> Helpers.section "Fixed Width & Gutters" description

  let render () =
    Container.Create(
      div [] [
        H1.Div("Container Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "The Container component centers your content and provides responsive width constraints. Use it to wrap page sections or layouts.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        fixedWidthAndGuttersExample ()
      ]
    )
