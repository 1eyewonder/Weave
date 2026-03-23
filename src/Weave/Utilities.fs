namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

/// <summary>
/// Listens for events on the document and invokes a callback when the event
/// target is outside all of the tracked element refs.
/// </summary>
[<JavaScript>]
module DocumentEventListener =

  type Listener = {
    mutable Handler: Option<Dom.Event -> unit>
  } with

    member this.Attach(eventName: string, refs: Dom.Element ref list, onEvent: unit -> unit) =
      this.Detach eventName

      let handler (e: Dom.Event) =
        let target = e.Target :?> Dom.Element

        let isInside =
          refs
          |> List.exists (fun r ->
            match r.Value with
            | null -> false
            | el -> el.Contains target)

        if not isInside then
          onEvent ()

      JS.Document.AddEventListener(eventName, handler)
      this.Handler <- Some handler

    member this.Detach(eventName: string) =
      match this.Handler with
      | Some handler ->
        JS.Document.RemoveEventListener(eventName, handler)
        this.Handler <- None
      | None -> ()

  let create () = { Handler = None }

  /// <summary>
  /// Returns a Doc sink that automatically attaches/detaches the listener
  /// based on a boolean view (e.g. an isOpen view).
  /// `eventName` is the first parameter so you can partially apply common events.
  /// </summary>
  let watch (eventName: string) (refs: Dom.Element ref list) (onEvent: unit -> unit) (isActive: View<bool>) =
    let listener = create ()

    isActive
    |> Doc.sinkCached (fun active ->
      if active then
        listener.Attach(eventName, refs, onEvent)
      else
        listener.Detach eventName)

  let onClick = watch "click"

  let onMouseDown = watch "mousedown"

/// <summary>
/// Provides scoped keyboard navigation for menu components (ButtonMenu, Dropdown).
/// Listens on a container element for keydown events and manages focus among
/// [role='menuitem'] descendants. The listener is scoped to the container,
/// so it does not interfere with other components on the page.
/// </summary>
[<JavaScript>]
module MenuKeyboardNav =

  let private focusItemAt (container: Dom.Element) (focusedIdx: int ref) (index: int) =
    let items = container.QuerySelectorAll("[role='menuitem']")

    if index >= 0 && index < items.Length then
      focusedIdx.Value <- index
      let target = As<Dom.Element>(items.Item(index))
      JS.Window.SetTimeout((fun () -> target?focus ()), 0) |> ignore

  let private focusByOffset (container: Dom.Element) (focusedIdx: int ref) (offset: int) =
    let items = container.QuerySelectorAll("[role='menuitem']")
    let len = items.Length

    if len > 0 then
      let targetIdx =
        if focusedIdx.Value = -1 then
          if offset > 0 then 0 else len - 1
        else
          (focusedIdx.Value + offset + len) % len

      focusItemAt container focusedIdx targetIdx

  /// <summary>
  /// Returns a Doc sink that attaches/detaches a keydown listener on the container
  /// element based on the isOpen Var. Handles next/prev navigation (custom keys),
  /// Home, End, and Escape. Escape closes the menu and returns focus to the trigger.
  /// </summary>
  let watch
    (containerRef: Dom.Element ref)
    (triggerRef: Dom.Element ref)
    (isOpen: Var<bool>)
    (nextKey: string)
    (prevKey: string)
    : Doc
    =
    let focusedIdx = ref -1
    let keyHandler: (Dom.Event -> unit) option ref = ref None
    let focusOutHandler: (Dom.Event -> unit) option ref = ref None

    let handleKeyDown (ev: Dom.KeyboardEvent) =
      let container = containerRef.Value

      match ev.Key with
      | "Escape" ->
        ev.PreventDefault()
        isOpen.Value <- false
        focusedIdx.Value <- -1
        triggerRef.Value?focus()
      | key when key = nextKey ->
        ev.PreventDefault()
        focusByOffset container focusedIdx 1
      | key when key = prevKey ->
        ev.PreventDefault()
        focusByOffset container focusedIdx -1
      | "Home" ->
        ev.PreventDefault()
        let items = container.QuerySelectorAll("[role='menuitem']")

        if items.Length > 0 then
          focusItemAt container focusedIdx 0
      | "End" ->
        ev.PreventDefault()
        let items = container.QuerySelectorAll("[role='menuitem']")

        if items.Length > 0 then
          focusItemAt container focusedIdx (items.Length - 1)
      | _ -> ()

    let handleFocusOut (ev: Dom.Event) =
      let container = containerRef.Value
      let related = ev?relatedTarget

      if isNull related || not (As<bool>(container?contains (related))) then
        isOpen.Value <- false
        focusedIdx.Value <- -1

    let detachAll () =
      match keyHandler.Value with
      | Some h ->
        containerRef.Value.RemoveEventListener("keydown", h)
        keyHandler.Value <- None
      | None -> ()

      match focusOutHandler.Value with
      | Some h ->
        containerRef.Value.RemoveEventListener("focusout", h)
        focusOutHandler.Value <- None
      | None -> ()

    isOpen.View
    |> Doc.sinkCached (fun opened ->
      detachAll ()

      if opened then
        focusedIdx.Value <- -1

        let kh = fun (e: Dom.Event) -> handleKeyDown (As<Dom.KeyboardEvent> e)
        containerRef.Value.AddEventListener("keydown", kh)
        keyHandler.Value <- Some kh

        let fh = fun (e: Dom.Event) -> handleFocusOut e
        containerRef.Value.AddEventListener("focusout", fh)
        focusOutHandler.Value <- Some fh)

/// <summary>
/// Observes the size of a DOM element using the browser ResizeObserver API
/// and exposes reactive Width and Height views.
/// </summary>
[<JavaScript>]
module ResizeListener =

  type ElementSize = {
    Width: Var<float>
    Height: Var<float>
  } with

    member this.WidthView = this.Width.View

    member this.HeightView = this.Height.View

  type private Listener = {
    mutable Handler: Option<Dom.Event -> unit>
  } with

    member this.Attach(el: Dom.Element, size: ElementSize) =
      this.Detach()

      let handler (_: Dom.Event) =
        size.Width.Value <- float el?offsetWidth
        size.Height.Value <- float el?offsetHeight

      JS.Window.AddEventListener("resize", handler)
      this.Handler <- Some handler

    member this.Detach() =
      match this.Handler with
      | Some handler ->
        JS.Window.RemoveEventListener("resize", handler)
        this.Handler <- None
      | None -> ()

  /// <summary>
  /// Creates an ElementSize tracker and begins listening for window resize events.
  /// Returns (elementSize, disposeFn) where disposeFn removes the listener.
  /// </summary>
  let observe (el: Dom.Element) : ElementSize * (unit -> unit) =
    let size = {
      Width = Var.Create(float el?offsetWidth)
      Height = Var.Create(float el?offsetHeight)
    }

    let listener = { Handler = None }
    listener.Attach(el, size)

    let dispose () = listener.Detach()
    size, dispose

  /// <summary>
  /// Creates an Attr that starts observing the element after render and
  /// stores the resulting ElementSize in the provided Var.
  /// </summary>
  let track (sizeVar: Var<ElementSize option>) =
    on.afterRender (fun el ->
      let size, _ = observe el
      sizeVar.Value <- Some size)

/// <summary>
/// Watches a DOM element and invokes a callback when it is removed from the
/// document. Uses a MutationObserver on <c>document.body</c> with subtree
/// observation. Call <c>Disconnect</c> to stop observing early.
/// </summary>
[<JavaScript>]
module DomRemovalListener =

  [<Inline "new MutationObserver($0)">]
  let private createObserver (callback: unit -> unit) : obj = X<obj>

  [<Inline "$0.observe($1, $2)">]
  let private observe (obs: obj) (target: obj) (options: obj) : unit = X<unit>

  type Listener = {
    mutable Observer: obj option
  } with

    member this.Watch(el: Dom.Element, onRemoved: unit -> unit) =
      this.Disconnect()

      let obs =
        createObserver (fun () ->
          if not (As<bool>(el?isConnected)) then
            this.Disconnect()
            onRemoved ())

      this.Observer <- Some obs
      observe obs JS.Document?body (New [ "childList" => true; "subtree" => true ])

    member this.Disconnect() =
      match this.Observer with
      | Some obs ->
        obs?disconnect ()
        this.Observer <- None
      | None -> ()

  let create () = { Observer = None }

[<JavaScript>]
module Disabled =

  /// <summary>
  /// Adds a disabled CSS class when the `enabled` view is false.
  /// </summary>
  let disabledClass (cssClass: string) (enabled: View<bool>) =
    View.not enabled |> Attr.DynamicClassPred cssClass

  /// <summary>
  /// Sets the native HTML disabled attribute when `enabled` is false.
  /// </summary>
  let nativeAttr (enabled: View<bool>) =
    Attr.DynamicPred "disabled" (View.not enabled) (View.Const "")

[<JavaScript>]
module on =

  /// <summary>
  /// Fires the callback only when `enabled` is true.
  /// </summary>
  let clickViewGuarded (enabled: View<bool>) (onClick: unit -> unit) =
    on.clickView enabled (fun _ _ isEnabled ->
      if isEnabled then
        onClick ())

  /// <summary>
  /// Handles both click and tap via the "pointerup" event.
  /// Fires the callback for primary pointer interactions (mouse left-click, touch, pen).
  /// </summary>
  let clickTap (handler: Dom.Element -> Dom.Event -> unit) =
    Attr.Handler "pointerup" (fun el ev ->
      if ev?button = 0 then
        handler el ev)

  /// <summary>
  /// Handles both click and tap via the "pointerup" event,
  /// providing the current value of a reactive view to the handler.
  /// </summary>
  /// <remarks>
  /// For input types, stick with on.clickView since these inputs have native toggle semantics tied to the click event.
  /// </remarks>
  let clickTapView (view: View<'T>) (handler: Dom.Element -> Dom.Event -> 'T -> unit) =
    let current = Var.Create None

    Attr.Concat [
      on.afterRender (fun _ -> view |> View.Sink(fun v -> Var.Set current (Some v)))
      Attr.Handler "pointerup" (fun el ev ->
        if ev?button = 0 then
          match current.Value with
          | Some v -> handler el ev v
          | None -> ())
    ]

  /// <summary>
  /// Handles both click and tap, firing the callback only when `enabled` is true.
  /// Combines pointer event handling with an enabled guard.
  /// </summary>
  /// <remarks>
  /// For input types, stick with on.clickViewGuarded since these inputs have native toggle semantics tied to the click event.
  /// </remarks>
  let clickTapViewGuarded (enabled: View<bool>) (onClick: unit -> unit) =
    clickTapView enabled (fun _ _ isEnabled ->
      if isEnabled then
        onClick ())

  /// <summary>
  /// Like <c>clickTap</c> but also fires on keyboard Enter/Space.
  /// Use on elements that don't already have their own keydown handler.
  /// </summary>
  let clickTapKey (handler: Dom.Element -> Dom.Event -> unit) =
    Attr.Concat [
      clickTap handler
      Attr.Handler "keydown" (fun el ev ->
        let key = ev?key: string

        if key = "Enter" || key = " " then
          ev.PreventDefault()
          handler el ev)
    ]

  /// <summary>
  /// Like <c>clickTapView</c> but also fires on keyboard Enter/Space.
  /// Use on elements that don't already have their own keydown handler.
  /// </summary>
  let clickTapKeyView (view: View<'T>) (handler: Dom.Element -> Dom.Event -> 'T -> unit) =
    let current = Var.Create None

    Attr.Concat [
      on.afterRender (fun _ -> view |> View.Sink(fun v -> Var.Set current (Some v)))
      Attr.Handler "pointerup" (fun el ev ->
        if ev?button = 0 then
          match current.Value with
          | Some v -> handler el ev v
          | None -> ())
      Attr.Handler "keydown" (fun el ev ->
        let key = ev?key: string

        if key = "Enter" || key = " " then
          ev.PreventDefault()

          match current.Value with
          | Some v -> handler el ev v
          | None -> ())
    ]

  /// <summary>
  /// Like <c>clickTapViewGuarded</c> but also fires on keyboard Enter/Space.
  /// Use on elements that don't already have their own keydown handler.
  /// </summary>
  let clickTapKeyViewGuarded (enabled: View<bool>) (onClick: unit -> unit) =
    clickTapKeyView enabled (fun _ _ isEnabled ->
      if isEnabled then
        onClick ())

[<JavaScript>]
module ScrollListener =

  /// <summary>
  /// Detects the topmost visible section heading inside a scrollable container.
  /// Returns the id of the best matching heading, or <c>""</c> if none qualifies.
  /// </summary>
  let detectActiveSection (container: Dom.Element) (sectionSelector: string) (threshold: float) : string =
    let headers = JS.Document.QuerySelectorAll sectionSelector
    let mTop = container.GetBoundingClientRect().Top
    let mutable bestId = ""
    let mutable bestRelTop = -1.0e10
    let mutable lastVisibleId = ""

    for i in 0 .. headers.Length - 1 do
      let h = As<Dom.Element>(headers.Item i)
      let relTop = h.GetBoundingClientRect().Top - mTop

      if relTop <= threshold && relTop > bestRelTop then
        bestRelTop <- relTop
        bestId <- h.Id

      let containerHeight = As<float>(container?clientHeight)

      if relTop >= 0.0 && relTop < containerHeight then
        lastVisibleId <- h.Id

    if
      System.String.IsNullOrEmpty bestId
      && not (System.String.IsNullOrEmpty lastVisibleId)
    then
      bestId <- lastVisibleId

    bestId

  /// <summary>
  /// Returns an <c>Attr</c> that, when applied to a scrollable element, tracks
  /// the topmost visible section heading and fires <c>onSection</c> with its id.
  /// A heading qualifies when its top edge is at or below <c>threshold</c> pixels
  /// from the container top; the best (closest-to-top) qualifying heading wins.
  /// Fires with <c>""</c> when no heading qualifies (e.g. scrolled to the very top).
  /// Uses requestAnimationFrame to throttle work to once per paint.
  /// </summary>
  let trackSections (sectionSelector: string) (threshold: float) (onSection: string -> unit) : Attr =
    on.afterRender (fun el ->
      let mutable ticking = false

      el.AddEventListener(
        "scroll",
        fun (_: Dom.Event) ->
          if not ticking then
            ticking <- true

            JS.Window?requestAnimationFrame(fun _ ->
              ticking <- false
              onSection (detectActiveSection el sectionSelector threshold))
            |> ignore
      ))

/// <summary>
/// JS-driven animation helpers for reactive enter/exit, replay, and toggle.
/// For CSS-only animation modifiers (duration, easing, iteration, triggers),
/// see the Animation module types.
/// </summary>
[<JavaScript>]
module Animate =

  /// <summary>
  /// Toggles between enter and exit animation classes based on a boolean view.
  /// For elements that remain in the DOM (e.g., drawers, expansion panels).
  /// Neither class is applied until the first state change, so elements that
  /// start inactive do not play an exit animation on initial render.
  /// </summary>
  let toggleClass (pair: AnimationPair) (isActive: View<bool>) : Attr =
    let enterClass = AnimationEntrance.toClass pair.Enter
    let exitClass = AnimationExit.toClass pair.Exit
    let hasChanged = Var.Create false

    let guardedView =
      isActive
      |> View.Map(fun active ->
        if active then
          hasChanged.Value <- true

        active)

    let shouldAnimate = View.Map2 (fun changed _ -> changed) hasChanged.View guardedView

    Attr.Concat [
      Attr.DynamicClassPred
        enterClass
        (View.Map2 (fun guard active -> guard && active) shouldAnimate isActive)

      Attr.DynamicClassPred
        exitClass
        (View.Map2 (fun guard active -> guard && not active) shouldAnimate isActive)
    ]

  /// <summary>
  /// Extended version of <c>show</c> with additional options.
  /// </summary>
  /// <param name="wrapperAttrs">Additional attributes for the animation wrapper div
  /// (e.g., duration/easing overrides, flex/grid styles). The wrapper is the element
  /// that carries the enter/exit animation class.</param>
  /// <param name="onExitComplete">Called after the exit animation finishes and the
  /// content is removed from the DOM. Use this instead of hardcoded timeouts to
  /// synchronize cleanup (e.g., removing items from a ListModel).</param>
  let showWith
    (pair: AnimationPair)
    (isVisible: View<bool>)
    (content: unit -> Doc)
    (wrapperAttrs: Attr list)
    (onExitComplete: (unit -> unit) option)
    : Doc
    =
    let enterClass = AnimationEntrance.toClass pair.Enter
    let exitClass = AnimationExit.toClass pair.Exit
    let currentState = Var.Create Doc.Empty
    let hasBeenShown = Var.Create false

    isVisible
    |> View.Sink(fun visible ->
      if visible then
        hasBeenShown.Value <- true

        let doc = div [ cl enterClass; yield! wrapperAttrs ] [ content () ]

        currentState.Value <- doc
      else if hasBeenShown.Value then
        let exitDoc =
          div [
            cl exitClass
            yield! wrapperAttrs

            on.afterRender (fun el ->
              el.AddEventListener(
                "animationend",
                fun (ev: Dom.Event) ->
                  // Guard against bubbled animationend from child elements
                  if ev.Target = (el :> Dom.EventTarget) then
                    currentState.Value <- Doc.Empty

                    match onExitComplete with
                    | Some cb -> cb ()
                    | None -> ()
              ))
          ] [ content () ]

        currentState.Value <- exitDoc)

    currentState.View |> Doc.EmbedView

  /// <summary>
  /// Renders content reactively with enter/exit animations.
  /// When <c>isVisible</c> becomes true, renders the content with the enter class.
  /// When <c>isVisible</c> becomes false, applies the exit class, waits for the
  /// animationend event, then removes the element from the DOM.
  /// </summary>
  let show (pair: AnimationPair) (isVisible: View<bool>) (content: unit -> Doc) : Doc =
    showWith pair isVisible content [] None

  /// <summary>
  /// Replays the element's animation on a recurring interval by removing and
  /// re-adding the animation-name CSS property. Compose with any animation
  /// kind class — the first play is immediate, then replays every
  /// <paramref name="intervalMs"/> milliseconds.
  /// </summary>
  let private replay (el: Dom.Element) =
    el?style?animationName <- "none"
    el?offsetHeight |> ignore
    el?style?animationName <- ""

  /// <summary>
  /// Replays the element's animation on a recurring interval by removing and
  /// re-adding the animation-name CSS property. Compose with any animation
  /// kind class — the first play is immediate, then replays every
  /// <paramref name="intervalMs"/> milliseconds. The interval self-clears
  /// when the element is removed from the DOM.
  /// Do not combine with AnimationOn trigger classes or Animate.replayOnClick —
  /// these are mutually exclusive trigger mechanisms.
  /// </summary>
  let replayEvery (intervalMs: int) : Attr =
    on.afterRender (fun (el: Dom.Element) ->
      let handle = ref Unchecked.defaultof<JS.Handle>

      handle.Value <-
        JS.SetInterval
          (fun () ->
            if As<bool> el?isConnected then
              replay el
            else
              JS.ClearInterval handle.Value)
          intervalMs)

  /// <summary>
  /// Replays the element's animation each time it is clicked. The initial
  /// mount animation is suppressed — the first play only happens on click.
  /// Do not combine with AnimationOn trigger classes or Animate.replayEvery —
  /// these are mutually exclusive trigger mechanisms.
  /// </summary>
  let replayOnClick: Attr =
    on.afterRender (fun (el: Dom.Element) ->
      el?style?animationName <- "none"
      el.AddEventListener("click", fun (_: Dom.Event) -> replay el))

  /// <summary>
  /// Sets up an IntersectionObserver that controls the element's animation
  /// play state based on viewport intersection.
  ///
  /// On enter: resets the animation via the name-removal technique (same as
  /// <c>replay</c>) then sets <c>animationPlayState</c> to <c>running</c>.
  /// The name reset is synchronous (no paint between removal and restore),
  /// so the user only sees the animation play from its "from" keyframe.
  ///
  /// On leave (when <c>once</c> is false): pauses the animation so it can
  /// be replayed on next entry.
  ///
  /// When <c>once</c> is true: disconnects the observer after the first
  /// trigger — the animation plays once and stays at its "to" keyframe.
  /// </summary>
  [<Inline """(function() {
    var obs = new IntersectionObserver(function(entries, o) {
      for (var i = 0; i < entries.length; i++) {
        var el = entries[i].target;
        if (entries[i].isIntersecting) {
          el.style.animationName = 'none';
          void el.offsetHeight;
          el.style.animationName = '';
          el.style.animationPlayState = 'running';
          if ($3) o.unobserve(el);
        } else if (!$3) {
          el.style.animationPlayState = 'paused';
        }
      }
    }, { threshold: $1, rootMargin: $2 });
    obs.observe($0);
  })()""">]
  let private setupScrollTrigger
    (el: Dom.Element)
    (threshold: float)
    (rootMargin: string)
    (once: bool)
    : unit
    =
    X<unit>

  /// <summary>
  /// Defers the element's CSS animation until it scrolls into the viewport.
  /// Compose with any <c>AnimationEntrance</c> class plus optional
  /// <c>AnimationDuration</c>, <c>AnimationDelay</c>, and
  /// <c>AnimationEasing</c> overrides — the animation starts paused
  /// (holding its "from" keyframe via <c>animation-fill-mode: both</c>)
  /// and resumes when the IntersectionObserver fires.
  /// </summary>
  /// <param name="threshold">
  /// Fraction of the element that must be visible to trigger (0.0–1.0).
  /// Default: <c>0.1</c>.
  /// </param>
  /// <param name="rootMargin">
  /// Margin around the viewport root, e.g. <c>"0px 0px -40px 0px"</c>
  /// to trigger 40px before the bottom edge. Default: <c>"0px"</c>.
  /// </param>
  /// <param name="once">
  /// When <c>true</c> (default), the observer disconnects after the first
  /// trigger. Set to <c>false</c> for elements that should re-animate
  /// each time they re-enter the viewport.
  /// </param>
  /// <remarks>
  /// Do not combine with <c>AnimationOn</c> trigger classes —
  /// they override <c>animation-name</c> which conflicts with the
  /// paused play-state approach used here.
  /// </remarks>
  let onScrollWith (threshold: float) (rootMargin: string) (once: bool) : Attr =
    Attr.Concat [
      Attr.Style "animation-play-state" "paused"
      on.afterRender (fun (el: Dom.Element) -> setupScrollTrigger el threshold rootMargin once)
    ]

  /// <summary>
  /// Defers the element's CSS animation until it scrolls into the viewport,
  /// using default settings (threshold: 0.1, no root margin, triggers once).
  /// For custom IntersectionObserver settings, use <c>onScrollWith</c>.
  /// </summary>
  let onScroll: Attr = onScrollWith 0.1 "0px" true
