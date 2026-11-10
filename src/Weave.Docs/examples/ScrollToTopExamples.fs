namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ScrollToTopExamples =

  let private containerExample () =
    let id = WeaveId.create "scroll-demo"

    Helpers.codeSampleSection
      "Scrollable container"
      (Helpers.bodyText
        "Scroll the panel past 150 pixels to reveal its button. This example uses an instant scroll and a custom accessible name through attrs. The button is positioned within a relative wrapper.")
      (div [ Attr.Style "position" "relative" ] [
        div [
          attr.id id
          attr.tabindex "0"
          Attr.Create "role" "region"
          Attr.Create "aria-label" "Scrollable example"
          Attr.Style "height" "260px"
          Attr.Style "overflow-y" "auto"
          Padding.All.small
          SurfaceColor.BackgroundColor.paper
        ] [
          for i in 1..12 do
            Card.create (
              CardContent.create (
                div [] [
                  h3 [ Typography.h6 ] [ text (sprintf "Chapter %d" i) ]
                  p [ Typography.body2 ] [
                    text "Keep scrolling. Use the arrow to return to the beginning of this panel."
                  ]
                ]
              ),
              attrs = [ Margin.Bottom.small ]
            )
        ]
        ScrollToTop.create (
          selector = "#" + id,
          topOffset = 150,
          attrs = [
            Button.Color.secondary
            Attr.Style "position" "absolute"
            Attr.Create "aria-label" "Back to panel top"
            ScrollToTop.Behavior.instant
          ]
        )
      ])
      """div [ Attr.Style "position" "relative" ] [
    div [ attr.id "chapters"; Attr.Style "height" "260px"; Attr.Style "overflow-y" "auto" ] [
        // Your scrollable content
    ]
    ScrollToTop.create(
        selector = "#chapters",
        topOffset = 150,
        attrs = [
            Button.Color.secondary
            Attr.Style "position" "absolute"
            Attr.Create "aria-label" "Back to panel top"
            ScrollToTop.Behavior.instant
        ])
]"""

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Scroll to Top"
        Helpers.bodyText
          "Return to the beginning of a long page or a scrollable panel. The button is hidden until the scroll threshold is reached and supports mouse, touch, Enter, and Space."
        Helpers.divider ()
        Helpers.codeSampleSection
          "Page scrolling"
          (Helpers.bodyText
            "Scroll this page down 300 pixels to reveal the button in the bottom corner. Smooth scrolling automatically becomes instant when reduced motion is preferred.")
          (Helpers.bodyText
            "Omit selector for document scrolling. This documentation uses a scrolling main pane, so its live button targets .weave-main-content.")
          """// Document scrolling:
ScrollToTop.create(attrs = [ Button.Color.primary ])

// An app shell with a scrolling main pane (as on this site):
ScrollToTop.create(
    selector = ".weave-main-content",
    attrs = [ Button.Color.primary ])"""
        Helpers.divider ()
        containerExample ()
        Helpers.divider ()
        Helpers.apiSection
          (Helpers.bodyText
            "Listeners are released when the button is removed. Hidden buttons cannot receive focus or clicks.")
          [
            Helpers.apiTable "ScrollToTop.create" [
              Helpers.apiParam
                "?selector"
                "string"
                "omitted"
                "CSS selector of an existing scrollable element; omitted uses the page. Resolved on mount; a missing target keeps the button hidden."
              Helpers.apiParam
                "?topOffset"
                "int"
                "300"
                "Show at or above this distance in pixels; negative values are clamped to zero."
              Helpers.apiParam
                "?attrs"
                "Attr list"
                "[]"
                "Button attributes, colors, and positioning. Override the default Back to top name with aria-label. Uses the filled button variant."
            ]
            Helpers.styleModuleTable "ScrollToTop.Behavior" [
              ("instant",
               "Disable smooth scrolling. Smooth scrolling is the default; reduced-motion preferences always use instant scrolling.")
            ]
          ]
        ScrollToTop.create (selector = ".weave-main-content", attrs = [ Button.Color.primary ])
      ],
      attrs = [ Container.MaxWidth.large ]
    )
