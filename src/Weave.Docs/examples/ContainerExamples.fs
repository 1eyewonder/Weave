namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ContainerExamples =

  let options = [
    Container.MaxWidth.extraSmall
    Container.MaxWidth.small
    Container.MaxWidth.medium
    Container.MaxWidth.large
    Container.MaxWidth.extraLarge
    Container.MaxWidth.extraExtraLarge
  ]

  let centeredText (displayText: string) =
    H6.div (displayText, attrs = [ AlignSelf.center ])

  let private variantExamples () =
    let description =
      Helpers.bodyText "Showcases each maxWidth option with a colored background for visibility."

    let content =
      div [] [
        yield!
          options
          |> List.mapi (fun i mw ->
            Container.create (
              sprintf "MaxWidth = %A" mw |> centeredText,
              attrs = [
                mw
                Attr.Style "min-height" "1vh"
                Margin.Bottom.small
                AlignItems.center
                JustifyContent.center
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

Container.create(content, attrs = [ Container.MaxWidth.extraSmall ])

Container.create(content, attrs = [ Container.MaxWidth.small ])

Container.create(content, attrs = [ Container.MaxWidth.medium ])

Container.create(content, attrs = [ Container.MaxWidth.large ])

Container.create(content, attrs = [ Container.MaxWidth.extraLarge ])

Container.create(content, attrs = [ Container.MaxWidth.extraExtraLarge ])
"""

    Helpers.codeSampleSection "MaxWidth Variants" description content code

  let private fixedWidthAndGuttersExample () =
    let description =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] [
        Body1.div (
          "The Container component supports two key layout parameters:",
          attrs = [ Margin.Bottom.small ]
        )

        Subtitle2.div ("Fixed Width")

        Body1.div (
          "With the Fixed property set to true the container will \"snap\" to the closest breakpoint.",
          attrs = [ Margin.Bottom.small ]
        )

        Subtitle2.div ("Gutters")

        Body1.div (
          "If true, horizontal padding (gutters) are applied inside the container.",
          attrs = [ Margin.Bottom.small ]
        )
      ]

    let filler content =
      Container.create (
        content,
        attrs = [
          Attr.Style "min-height" "1vh"
          BrandColor.toBackgroundColor BrandColor.Primary
          JustifyContent.center
        ]
      )

    let content =
      div [] [
        Container.create (
          "fixedWidth = false, gutters = true (default)" |> centeredText |> filler,
          fixedWidth = false,
          gutters = true,
          attrs = [
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
        Container.create (
          "fixedWidth = false, gutters = false" |> centeredText |> filler,
          fixedWidth = false,
          gutters = false,
          attrs = [
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
        Container.create (
          "fixedWidth = true, gutters = true" |> centeredText |> filler,
          fixedWidth = true,
          gutters = true,
          attrs = [
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
        Container.create (
          "fixedWidth = true, gutters = false" |> centeredText |> filler,
          fixedWidth = true,
          gutters = false,
          attrs = [
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
      ]

    let code =
      """open Weave

Container.create(
    content,
    fixedWidth = false, // see here
    gutters = true // see here
)

Container.create(
    content,
    fixedWidth = false,
    gutters = false // see here
)

Container.create(
    content,
    fixedWidth = true, // see here
    gutters = true
)

Container.create(
    content,
    fixedWidth = true, // see here
    gutters = false // see here
)
"""

    Helpers.codeSampleSection "Fixed Width & Gutters" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Container"
        Body1.div (
          "The Container component centers your content and provides responsive width constraints. Use it to wrap page sections or layouts.",
          attrs = [ Margin.Bottom.extraSmall ]
        )

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        fixedWidthAndGuttersExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
