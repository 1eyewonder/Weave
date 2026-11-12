namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module SpacerExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText "A Spacer consumes remaining space in a flex row."

    let content =
      div [ Flex.Flex.allSizes; AlignItems.center ] [
        Button.primary (text "Left", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
        Spacer.create ()
        Button.primary (text "Right", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
      ]

    let code =
      """div [ Flex.Flex.allSizes; AlignItems.center ] [
  Button.primary (
    text "Left",
    onClick = (fun () -> ()),
    attrs = [ Button.Variant.filled ]
  )
  Spacer.create ()
  Button.primary (
    text "Right",
    onClick = (fun () -> ()),
    attrs = [ Button.Variant.filled ]
  )
]"""

    Helpers.codeSampleSection "Basic" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Spacer.") [
      Helpers.apiTable "Spacer.create" [
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the spacer element"
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Spacer"
        Helpers.bodyText
          "Spacer is a zero-content element that expands to fill all \
           available space in a flex container (flex: 1 1 auto). \
           Place it between items to push them apart."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        SpacerConfigurator.render ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
