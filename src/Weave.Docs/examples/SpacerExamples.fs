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
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
      ] [
        Button.primary (text "Left", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
        Spacer.create ()
        Button.primary (text "Right", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
      ]

    let code =
      """div [ Flex.Flex.allSizes; AlignItems.center ] [
    Button.create(text "Left", onClick = (fun () -> ()))
    Spacer.create()
    Button.create(text "Right", onClick = (fun () -> ()))
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
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
      ] [ btn "One"; Spacer.create (); btn "Two"; Spacer.create (); btn "Three" ]

    let code =
      """div [ Flex.Flex.allSizes; AlignItems.center ] [
    Button.create(text "One", onClick = (fun () -> ()))
    Spacer.create()
    Button.create(text "Two", onClick = (fun () -> ()))
    Spacer.create()
    Button.create(text "Three", onClick = (fun () -> ()))
]"""

    Helpers.codeSampleSection "Multiple Spacers" description content code

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
      ],
      attrs = [ Container.MaxWidth.large ]
    )
