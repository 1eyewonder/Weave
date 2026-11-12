namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ScrollToTopExamples =

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Scroll to Top"
        Helpers.bodyText
          "Return to the beginning of a long page or a scrollable panel. The button is hidden until the scroll threshold is reached and supports mouse, touch, Enter, and Space."
        Helpers.divider ()
        Helpers.codeSection
          "fsharp"
          "Page scrolling"
          (Helpers.bodyText
            "Scroll this documentation pane down 300 pixels to reveal its live button. Omit selector in a page whose document scrolls; this app shell scrolls .weave-main-content instead. Reduced motion makes smooth scrolling instant.")
          """ScrollToTop.create (
  selector = ".weave-main-content",
  attrs = [ Button.Color.primary ]
)"""
        Helpers.divider ()
        ScrollTopConfigurator.render ()
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
