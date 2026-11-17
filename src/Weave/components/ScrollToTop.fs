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

  [<Direct """
    if (customElements.get($name)) return;
    function ScrollListener() { return Reflect.construct(HTMLElement, [], ScrollListener); }
    Object.setPrototypeOf(ScrollListener.prototype, HTMLElement.prototype);
    ScrollListener.prototype.connectedCallback = $connected;
    ScrollListener.prototype.disconnectedCallback = $disconnected;
    customElements.define($name, ScrollListener);
  """>]
  let internal registerScrollListener
    (name: string)
    (connected: ThisAction<Dom.Element>)
    (disconnected: ThisAction<Dom.Element>)
    : unit
    =
    X<unit>

  module Behavior =
    /// Return immediately instead of animating the scroll.
    let instant = Attr.Create "data-weave-scroll-behavior" "instant"

[<JavaScript; RequireQualifiedAccess>]
type ScrollToTop =

  /// Creates an accessible icon button that appears once the target is scrolled
  /// at least topOffset pixels (default 300). Omit selector to scroll the page.
  /// The selector is resolved each time the button mounts. A missing target keeps the button hidden.
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

        if JS.Document.ActiveElement = buttonRef.Value then
          let start =
            if selector.IsSome then
              target.Value
            else
              As<Dom.Element> JS.Document.Body

          let temporaryTabIndex = not (start.HasAttribute "tabindex")

          if temporaryTabIndex then
            start.SetAttribute("tabindex", "-1")

          start?focus (New [ "preventScroll" => true ])

          if temporaryTabIndex then
            start.RemoveAttribute "tabindex"

        target.Value?scrollTo(
          New [
            "top" => 0
            "behavior" => if instant || reducedMotion then "instant" else "smooth"
          ]
        )

    let handler (_: Dom.Event) = update ()

    let listener =
      let name = "weave-scroll-to-top-listener"

      ScrollToTop.registerScrollListener
        name
        (ThisAction<Dom.Element>(fun marker -> marker?attach marker?parentElement))
        (ThisAction<Dom.Element>(fun marker -> marker?detach ()))

      Doc.Element name [
        Attr.Create "hidden" ""
        Attr.Prop "attach" (fun (button: Dom.Element) ->
          buttonRef.Value <- button

          target.Value <-
            match selector with
            | Some value -> JS.Document.QuerySelector value
            | None -> As<Dom.Element> JS.Document?scrollingElement

          if not (isNull target.Value) then
            match selector with
            | Some _ -> target.Value.AddEventListener("scroll", handler)
            | None -> JS.Window.AddEventListener("scroll", handler)

          update ())
        Attr.Prop "detach" (fun () ->
          if not (isNull target.Value) then
            match selector with
            | Some _ -> target.Value.RemoveEventListener("scroll", handler)
            | None -> JS.Window.RemoveEventListener("scroll", handler)

          target.Value <- null
          buttonRef.Value <- null)
      ] []

    let icon =
      Icon.create (
        Icons.MaterialSymbols.Icon.Hardware Icons.MaterialSymbols.Hardware.KeyboardArrowUp,
        attrs = [ Attr.Create "aria-hidden" "true" ]
      )

    IconButton.create (
      Doc.Concat [ icon; listener ],
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
        yield! defaultArg attrs []
      ]
    )
