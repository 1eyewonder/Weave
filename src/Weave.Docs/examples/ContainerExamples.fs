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
    div [ Typography.h6; AlignSelf.center ] [ text displayText ]

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
                if i % 2 = 0 then BrandColor.BackgroundColor.primary
                elif i % 3 = 0 then BrandColor.BackgroundColor.secondary
                else BrandColor.BackgroundColor.tertiary
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
        div [ Typography.body1; Margin.Bottom.small ] [
          text "The Container component supports two optional layout modifiers passed via attrs:"
        ]

        div [ Typography.subtitle2 ] [ text "Container.fixedWidth" ]

        div [ Typography.body1; Margin.Bottom.small ] [
          text "Causes the container to snap to the nearest breakpoint width rather than growing fluidly."
        ]

        div [ Typography.subtitle2 ] [ text "Container.gutters" ]

        div [ Typography.body1; Margin.Bottom.small ] [
          text "Adds horizontal padding inside the container so content does not touch the edges."
        ]
      ]

    let filler content =
      Container.create (
        content,
        attrs = [
          Attr.Style "min-height" "1vh"
          BrandColor.BackgroundColor.primary
          JustifyContent.center
        ]
      )

    let content =
      div [] [
        Container.create (
          "gutters (no fixedWidth)" |> centeredText |> filler,
          attrs = [
            Container.gutters
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.BackgroundColor.background
          ]
        )
        Container.create (
          "neither gutters nor fixedWidth" |> centeredText |> filler,
          attrs = [
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.BackgroundColor.background
          ]
        )
        Container.create (
          "fixedWidth + gutters" |> centeredText |> filler,
          attrs = [
            Container.fixedWidth
            Container.gutters
            Margin.Bottom.small
            JustifyContent.center
            SurfaceColor.BackgroundColor.background
          ]
        )
        Container.create (
          "fixedWidth (no gutters)" |> centeredText |> filler,
          attrs = [
            Container.fixedWidth
            JustifyContent.center
            SurfaceColor.BackgroundColor.background
          ]
        )
      ]

    let code =
      """open Weave

// Add gutters only — pass Container.gutters through attrs
Container.create(content, attrs = [ Container.gutters ])

// Add fixed-width snapping only
Container.create(content, attrs = [ Container.fixedWidth ])

// Both modifiers together
Container.create(content, attrs = [ Container.fixedWidth; Container.gutters ])

// Neither (plain fluid container — no attrs needed)
Container.create(content)
"""

    Helpers.codeSampleSection "Fixed Width & Gutters" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Container"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Container component centers your content and provides responsive width constraints. Use it to wrap page sections or layouts."
        ]

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        fixedWidthAndGuttersExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
