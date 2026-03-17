namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

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
