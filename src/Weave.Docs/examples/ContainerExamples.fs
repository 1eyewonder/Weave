namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ContainerExamples =

  let options = [
    "ExtraSmall", Container.MaxWidth.extraSmall
    "Small", Container.MaxWidth.small
    "Medium", Container.MaxWidth.medium
    "Large", Container.MaxWidth.large
    "ExtraLarge", Container.MaxWidth.extraLarge
    "ExtraExtraLarge", Container.MaxWidth.extraExtraLarge
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
          |> List.mapi (fun i (name, attr) ->
            Container.create (
              sprintf "MaxWidth = %s" name |> centeredText,
              attrs = [
                attr
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
          "The Container component supports two optional layout modifiers passed via attrs:",
          attrs = [ Margin.Bottom.small ]
        )

        Subtitle2.div ("Container.fixedWidth")

        Body1.div (
          "Causes the container to snap to the nearest breakpoint width rather than growing fluidly.",
          attrs = [ Margin.Bottom.small ]
        )

        Subtitle2.div ("Container.gutters")

        Body1.div (
          "Adds horizontal padding inside the container so content does not touch the edges.",
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
          "gutters (no fixedWidth)" |> centeredText |> filler,
          attrs = [
            Container.gutters // see here
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
        Container.create (
          "neither gutters nor fixedWidth" |> centeredText |> filler,
          attrs = [
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
        Container.create (
          "fixedWidth + gutters" |> centeredText |> filler,
          attrs = [
            Container.fixedWidth // see here
            Container.gutters // see here
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
        Container.create (
          "fixedWidth (no gutters)" |> centeredText |> filler,
          attrs = [
            Container.fixedWidth // see here
            JustifyContent.center
            SurfaceColor.toBackgroundColor SurfaceColor.Background
          ]
        )
      ]

    let code =
      """open Weave

// Add gutters only — pass Container.gutters through attrs
Container.create(content, attrs = [ Container.gutters ]) // see here

// Add fixed-width snapping only
Container.create(content, attrs = [ Container.fixedWidth ]) // see here

// Both modifiers together
Container.create(content, attrs = [ Container.fixedWidth; Container.gutters ]) // see here

// Neither (plain fluid container — no attrs needed)
Container.create(content)
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
