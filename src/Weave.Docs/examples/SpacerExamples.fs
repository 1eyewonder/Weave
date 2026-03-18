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
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          yield! Padding.toClasses Padding.All.small
        ]
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
      ] [
        Button.Create(
          text "Left",
          onClick = (fun () -> ()),
          attrs = [ Button.Variant.filled; Button.Color.primary ]
        )
        Spacer.Create()
        Button.Create(
          text "Right",
          onClick = (fun () -> ()),
          attrs = [ Button.Variant.filled; Button.Color.primary ]
        )
      ]

    let code =
      """div [ cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ] ] [
    Button.Create(text "Left", onClick = (fun () -> ()))
    Spacer.Create()
    Button.Create(text "Right", onClick = (fun () -> ()))
]"""

    Helpers.codeSampleSection "Basic" description content code

  let private multipleExample () =
    let description =
      Helpers.bodyText
        "Multiple Spacers divide the remaining space equally between them, \
         distributing items evenly across the container."

    let btn label =
      Button.Create(
        text label,
        onClick = (fun () -> ()),
        attrs = [ Button.Variant.filled; Button.Color.primary ]
      )

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          yield! Padding.toClasses Padding.All.small
        ]
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
      ] [ btn "One"; Spacer.Create(); btn "Two"; Spacer.Create(); btn "Three" ]

    let code =
      """div [ cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ] ] [
    Button.Create(text "One", onClick = (fun () -> ()))
    Spacer.Create()
    Button.Create(text "Two", onClick = (fun () -> ()))
    Spacer.Create()
    Button.Create(text "Three", onClick = (fun () -> ()))
]"""

    Helpers.codeSampleSection "Multiple Spacers" description content code

  let render () =
    Container.Create(
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
      maxWidth = Container.MaxWidth.Large
    )
