namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module SwipeArea =

  [<RequireQualifiedAccess>]
  type Direction =
    | Left
    | Right
    | Up
    | Down

  /// Displacement from the start, in CSS pixels. Right and down are positive.
  type Movement = {
    DeltaX: float
    DeltaY: float
  }

  type Swipe = {
    Direction: Direction
    DeltaX: float
    DeltaY: float
  }

  module TouchAction =

    /// Reserve horizontal gestures while allowing vertical scrolling and pinch zoom.
    let horizontal = cl Css.``weave-swipearea--horizontal``
    /// Reserve vertical gestures while allowing horizontal scrolling and pinch zoom.
    let vertical = cl Css.``weave-swipearea--vertical``
    /// Reserve gestures in both axes. Also disables browser pinch zoom in this area.
    let none = cl Css.``weave-swipearea--no-touch-action``

[<JavaScript; RequireQualifiedAccess>]
type SwipeArea =

  /// Adds swipe gestures to content. onSwipe runs once on release when the
  /// dominant-axis displacement reaches threshold (100 CSS pixels by default).
  /// onDrag is optional and reports displacement while the pointer is held,
  /// including movement below the threshold. onSwipeCancel runs for taps, short
  /// gestures, browser cancellation, capture loss, or disabling during a gesture.
  /// Provide buttons or another keyboard-accessible way to perform the same
  /// actions. Native scrolling remains enabled by default.
  static member create
    (
      content: Doc,
      onSwipe: SwipeArea.Swipe -> unit,
      ?threshold: View<int>,
      ?enabled: View<bool>,
      ?onDrag: SwipeArea.Movement -> unit,
      ?onSwipeCancel: unit -> unit,
      ?attrs: Attr list
    ) =
    let threshold = defaultArg threshold (View.Const 100)
    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs []
    let mutable active: (int * float * float) option = None

    let release (el: Dom.Element) pointerId =
      if As<bool>(el?hasPointerCapture pointerId) then
        el?releasePointerCapture pointerId

    let cancel el =
      match active with
      | None -> ()
      | Some(pointerId, _, _) ->
        active <- None
        release el pointerId
        onSwipeCancel |> Option.iter (fun callback -> callback ())

    let ownsPointer (ev: Dom.Event) =
      match active with
      | Some(pointerId, _, _) -> pointerId = As<int> ev?pointerId
      | None -> false

    div [
      cl Css.``weave-swipearea``
      enabled
      |> Attr.DynamicCustom(fun el value ->
        if not value then
          cancel el)

      Attr.HandlerView "pointerdown" enabled (fun el event isEnabled ->
        let ev = As<Dom.MouseEvent> event
        let target = As<Dom.Element> ev.Target
        // Nested areas own their gestures; controls retain their native interaction.
        let area = target?closest ".weave-swipearea"

        let control =
          target?closest
            "button, a, input, select, textarea, [contenteditable]:not([contenteditable='false']), [role='button'], [role='slider'], [data-weave-swipe-ignore]"

        if
          isEnabled
          && active.IsNone
          && ev.Button = 0
          && As<bool> ev?isPrimary
          && As<bool>(el?isSameNode area)
          && isNull control
        then
          let pointerId = As<int> ev?pointerId
          active <- Some(pointerId, float ev.ClientX, float ev.ClientY)
          el?setPointerCapture pointerId)

      Attr.HandlerView "pointermove" enabled (fun el event isEnabled ->
        let ev = As<Dom.MouseEvent> event

        if ownsPointer ev then
          // Recover if a mouse button was released while the window was inactive.
          if not isEnabled || ev.Buttons = 0 then
            cancel el
          else
            match active with
            | Some(_, x, y) ->
              onDrag
              |> Option.iter (fun callback ->
                callback {
                  DeltaX = float ev.ClientX - x
                  DeltaY = float ev.ClientY - y
                })
            | None -> ())

      Attr.HandlerView "pointerup" (View.zip enabled threshold) (fun el event (isEnabled, threshold) ->
        let ev = As<Dom.MouseEvent> event

        if not isEnabled then
          if ownsPointer ev then
            cancel el
        elif ownsPointer ev then
          match active with
          | Some(pointerId, x, y) ->
            active <- None
            release el pointerId
            let dx = float ev.ClientX - x
            let dy = float ev.ClientY - y
            let distance = max (abs dx) (abs dy)
            // A tap never becomes a swipe, even with a zero threshold.
            if distance > 0.0 && distance >= float (max 0 threshold) then
              let direction =
                if abs dx > abs dy then
                  if dx > 0.0 then
                    SwipeArea.Direction.Right
                  else
                    SwipeArea.Direction.Left
                else if dy > 0.0 then
                  SwipeArea.Direction.Down
                else
                  SwipeArea.Direction.Up

              onSwipe {
                Direction = direction
                DeltaX = dx
                DeltaY = dy
              }
            else
              onSwipeCancel |> Option.iter (fun callback -> callback ())
          | None -> ())

      on.pointerCancel (fun el ev ->
        if ownsPointer ev then
          cancel el)

      Attr.Handler "lostpointercapture" (fun el ev ->
        if ownsPointer ev then
          cancel el)

      Attr.Handler "dragstart" (fun _ ev ->
        if active.IsSome then
          ev.PreventDefault())

      yield! attrs
    ] [ content ]
