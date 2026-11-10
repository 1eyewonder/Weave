namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module ScrollToTop =

  module Behavior =
    /// Return immediately instead of animating the scroll.
    let instant = Attr.Create "data-weave-scroll-behavior" "instant"

[<JavaScript; RequireQualifiedAccess>]
type ScrollToTop =

  /// Creates an accessible icon button that appears once the target is scrolled
  /// at least topOffset pixels (default 300). Omit selector to scroll the page.
  /// The selector is resolved once after mounting; a missing target keeps the button hidden.
  /// Smooth scrolling respects reduced motion. Pass Behavior.instant through attrs
  /// to disable it. Override the accessible name with an aria-label attribute.
  static member create(?selector: string, ?topOffset: int, ?attrs: Attr list) =
    let topOffset = max 0 (defaultArg topOffset 300)
    let visible = Var.Create false
    let target = ref<Dom.Element> null
    let buttonRef = ref<Dom.Element> null

    let update () =
      let next =
        not (isNull target.Value) && As<float> target.Value?scrollTop >= float topOffset

      if visible.Value <> next then
        visible.Set next

    let scroll () =
      if not (isNull target.Value) then
        let reducedMotion = JS.Window.MatchMedia("(prefers-reduced-motion: reduce)").Matches
        let instant = buttonRef.Value.GetAttribute("data-weave-scroll-behavior") = "instant"

        target.Value?scrollTo(
          New [
            "top" => 0
            "behavior" => if instant || reducedMotion then "instant" else "smooth"
          ]
        )

    IconButton.create (
      Icon.create (
        Icons.MaterialSymbols.Icon.Hardware Icons.MaterialSymbols.Hardware.KeyboardArrowUp,
        attrs = [ Attr.Create "aria-hidden" "true" ]
      ),
      scroll,
      enabled = visible.View,
      attrs = [
        cl Css.``weave-scroll-to-top``
        Button.Variant.filled
        Attr.Create "aria-label" "Back to top"
        visible.View |> Attr.DynamicClassPred Css.``weave-scroll-to-top--visible``
        visible.View |> View.Map(not >> string<bool>) |> Attr.Dynamic "aria-hidden"
        visible.View
        |> View.Map(fun v -> if v then "0" else "-1")
        |> Attr.Dynamic "tabindex"
        on.afterRender (fun el ->
          buttonRef.Value <- el

          target.Value <-
            match selector with
            | Some value -> JS.Document.QuerySelector value
            | None -> As<Dom.Element> JS.Document?scrollingElement

          if not (isNull target.Value) then
            let handler (_: Dom.Event) = update ()

            let detach () =
              match selector with
              | Some _ -> target.Value.RemoveEventListener("scroll", handler)
              | None -> JS.Window.RemoveEventListener("scroll", handler)

              target.Value <- null
              buttonRef.Value <- null

            match selector with
            | Some _ -> target.Value.AddEventListener("scroll", handler)
            | None -> JS.Window.AddEventListener("scroll", handler)

            update ()
            let removal = DomRemovalListener.create ()
            removal.Watch(el, detach))
        yield! defaultArg attrs []
      ]
    )
