namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open Weave.CssHelpers

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

[<JavaScript>]
module Disabled =

  /// <summary>
  /// Adds a disabled CSS class when the `enabled` view is false.
  /// </summary>
  let disabledClass (cssClass: string) (enabled: View<bool>) =
    View.not enabled |> Attr.DynamicClassPred cssClass

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
    let current: ref<Option<'T>> = ref None

    Attr.Concat [
      on.afterRender (fun _ -> view |> View.Sink(fun v -> current.Value <- Some v))
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
