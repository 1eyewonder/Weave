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
      Helpers.bodyText "A horizontal divider separates adjacent content sections."

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        text "Content above the divider"

        Divider.create ()

        text "Content below the divider"
      ]

    let code = """Divider.create ()"""

    Helpers.codeSampleSection "Basic" description content code

  let private orientationExamples () =
    let description =
      Helpers.bodyText "Use Divider.Orientation.vertical inside a flex row with a defined height."

    let content =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        JustifyContent.center
        Attr.Style "height" "80px"
        Attr.Style "gap" "16px"
      ] [
        div [ Typography.body1 ] [ text "Left" ]
        Divider.create (attrs = [ Divider.Orientation.vertical ])
        div [ Typography.body1 ] [ text "Center" ]
        Divider.create (attrs = [ Divider.Orientation.vertical ])
        div [ Typography.body1 ] [ text "Right" ]
      ]

    let code =
      """div [
  Flex.Flex.allSizes
  AlignItems.center
  Attr.Style "height" "80px"
  Attr.Style "gap" "16px"
] [
  div [ Typography.body1 ] [ text "Left" ]
  Divider.create (attrs = [ Divider.Orientation.vertical ])
  div [ Typography.body1 ] [ text "Center" ]
  Divider.create (attrs = [ Divider.Orientation.vertical ])
  div [ Typography.body1 ] [ text "Right" ]
]"""

    Helpers.codeSampleSection "Orientation" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Divider.") [
      Helpers.apiTable "Divider.create" [
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (variant, orientation, spacing, etc.)"
      ]

      Helpers.styleModuleTable "Divider.Variant" [
        ("fullWidth", "Full-width divider spanning the container")
        ("inset", "Inset divider with left padding")
        ("middle", "Middle divider with padding on both sides")
      ]

      Helpers.styleModuleTable "Divider.Orientation" [ ("vertical", "Renders the divider as a vertical bar") ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Divider"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Dividers are thin lines that group content in lists and layouts."
        ]

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        ReferenceAppearance.divider ()
        Helpers.divider ()
        orientationExamples ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
