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
      Helpers.bodyText
        "Place a Spacer between two elements inside any flex container. \
         It expands to consume all remaining space, pushing siblings to \
         opposite edges."

    let content =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.All.small
        SurfaceColor.BackgroundColor.paper
      ] [
        Button.primary (text "Left", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
        Spacer.create ()
        Button.primary (text "Right", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
      ]

    let code =
      """open Weave

div [ Flex.Flex.allSizes; AlignItems.center ] [
    Button.primary(text "Left", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
    Spacer.create()  // see here
    Button.primary(text "Right", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
]"""

    Helpers.codeSampleSection "Basic" description content code

  let private multipleExample () =
    let description =
      Helpers.bodyText
        "Multiple Spacers divide the remaining space equally between them, \
         distributing items evenly across the container."

    let btn label =
      Button.primary (text label, onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])

    let content =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.All.small
        SurfaceColor.BackgroundColor.paper
      ] [ btn "One"; Spacer.create (); btn "Two"; Spacer.create (); btn "Three" ]

    let code =
      """open Weave

div [ Flex.Flex.allSizes; AlignItems.center ] [
    Button.primary(text "One", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
    Spacer.create()  // see here
    Button.primary(text "Two", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
    Spacer.create()  // see here
    Button.primary(text "Three", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
]"""

    Helpers.codeSampleSection "Multiple Spacers" description content code

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
        multipleExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
