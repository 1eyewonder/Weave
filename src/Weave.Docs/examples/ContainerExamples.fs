namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ContainerExamples =

  let options = [
    Container.MaxWidth.ExtraSmall
    Container.MaxWidth.Small
    Container.MaxWidth.Medium
    Container.MaxWidth.Large
    Container.MaxWidth.ExtraLarge
    Container.MaxWidth.ExtraExtraLarge
  ]

  let centeredText (displayText: string) =
    H6.Div(displayText, attrs = [ cls [ AlignSelf.toClass AlignSelf.Center ] ])

  let private variantExamples () =
    let description =
      Helpers.bodyText "Showcases each maxWidth option with a colored background for visibility."

    let content =
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

    let code =
      """open Weave
open Weave.Container

Container.Create(
    content,
    maxWidth = MaxWidth.ExtraSmall // see here
)

Container.Create(
    content,
    maxWidth = MaxWidth.Small // see here
)

Container.Create(
    content,
    maxWidth = MaxWidth.Medium // see here
)

Container.Create(
    content,
    maxWidth = MaxWidth.Large // see here
)

Container.Create(
    content,
    maxWidth = MaxWidth.ExtraLarge // see here
)

Container.Create(
    content,
    maxWidth = MaxWidth.ExtraExtraLarge // see here
)
"""

    Helpers.codeSampleSection "MaxWidth Variants" description content code

  let private fixedWidthAndGuttersExample () =
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

    let content =
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

    let code =
      """open Weave

Container.Create(
    content,
    fixedWidth = false, // see here
    gutters = true // see here
)

Container.Create(
    content,
    fixedWidth = false,
    gutters = false // see here
)

Container.Create(
    content,
    fixedWidth = true, // see here
    gutters = true
)

Container.Create(
    content,
    fixedWidth = true, // see here
    gutters = false // see here
)
"""

    Helpers.codeSampleSection "Fixed Width & Gutters" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Container"
        Body1.Div(
          "The Container component centers your content and provides responsive width constraints. Use it to wrap page sections or layouts.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        fixedWidthAndGuttersExample ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
