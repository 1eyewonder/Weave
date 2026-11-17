namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ContainerExamples =

  let private fixedWidthAndGuttersExample () =
    let description =
      Helpers.bodyText
        "Container.gutters adds horizontal padding. Container.fixedWidth snaps the width to breakpoint sizes. Resize the window to compare them; both attributes can be combined."

    let content =
      let createExample layoutAttrs label =
        let content = div [ BrandColor.Fill.primary ] [ text label ]
        Container.create (content, attrs = [ SurfaceColor.BackgroundColor.paper; yield! layoutAttrs ])

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        createExample [ Container.gutters ] "Gutters"

        createExample [ Container.fixedWidth ] "Fixed width"

        createExample [ Container.fixedWidth; Container.gutters ] "Both"

        createExample [] "Neither"
      ]

    let code =
      """Container.create (
  div [ BrandColor.Fill.primary ] [ text "Gutters" ],
  attrs = [ SurfaceColor.BackgroundColor.paper; Container.gutters ]
)

Container.create (
  div [ BrandColor.Fill.primary ] [ text "Fixed width" ],
  attrs = [ SurfaceColor.BackgroundColor.paper; Container.fixedWidth ]
)

Container.create (
  div [ BrandColor.Fill.primary ] [ text "Both" ],
  attrs = [
    SurfaceColor.BackgroundColor.paper
    Container.fixedWidth
    Container.gutters
  ]
)

Container.create (
  div [ BrandColor.Fill.primary ] [ text "Neither" ],
  attrs = [ SurfaceColor.BackgroundColor.paper ]
)"""

    Helpers.codeSampleSection "Fixed Width & Gutters" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Container.") [
      Helpers.apiTable "Container.create" [
        Helpers.apiParam "content" "Doc" "" "The content to render inside the container"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (max width, fixed width, gutters, etc.)"
      ]

      Helpers.styleModuleTable "Container.MaxWidth" [
        ("extraSmall", "Max width constrained to extra-small breakpoint")
        ("small", "Max width constrained to small breakpoint")
        ("medium", "Max width constrained to medium breakpoint")
        ("large", "Max width constrained to large breakpoint")
        ("extraLarge", "Max width constrained to extra-large breakpoint")
        ("extraExtraLarge", "Max width constrained to extra-extra-large breakpoint")
      ]

      Helpers.styleModuleTable "Container (module-level)" [
        ("fixedWidth", "Container uses fixed width instead of fluid max-width")
        ("gutters", "Adds horizontal padding to the container edges")
        ("contentHeight",
         "Sizes the container to its contents and prevents it from growing to fill a flex column")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Container"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Container centers content and provides responsive width constraints. By default it fills its parent's height and grows in a flex layout. For stacked page sections, pass Container.contentHeight in attrs so each section fits its content."
        ]

        Helpers.divider ()
        ContainerConfigurator.render ()
        Helpers.divider ()
        fixedWidthAndGuttersExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
