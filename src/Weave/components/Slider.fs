namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module Slider =

  module Color =

    let primary = cl Css.``weave-slider--primary``
    let secondary = cl Css.``weave-slider--secondary``
    let tertiary = cl Css.``weave-slider--tertiary``
    let error = cl Css.``weave-slider--error``
    let warning = cl Css.``weave-slider--warning``
    let success = cl Css.``weave-slider--success``
    let info = cl Css.``weave-slider--info``

[<JavaScript; RequireQualifiedAccess>]
type Slider =

  static member private renderSlider
    (
      valueAsFloat: Var<float>,
      minVal: float,
      maxVal: float,
      stepVal: float,
      labelText: View<string>,
      showTickMarks: bool,
      tickMarkLabels: string list,
      enabled: View<bool>,
      attrs: Attr list
    ) =

    let pct =
      valueAsFloat.View |> View.MapCached(fun v -> Bounded.percentOf minVal maxVal v)

    let tickPositions =
      if showTickMarks && stepVal > 0.0 then
        let count = int ((maxVal - minVal) / stepVal)

        [
          for i in 0..count -> Bounded.percentOf minVal maxVal (minVal + float i * stepVal)
        ]
      else
        []

    let updateFromPointer (el: Dom.Element) (clientX: float) =
      let rect = el?getBoundingClientRect ()
      let x: float = clientX - (rect?left: float)
      let w: float = rect?width: float

      if w > 0.0 then
        let ratio = Bounded.clamp 0.0 1.0 (x / w)
        let raw = minVal + ratio * (maxVal - minVal)
        let snapped = Bounded.snapToStep minVal maxVal stepVal raw
        valueAsFloat.Value <- snapped

    let isDragging = Var.Create false

    let onPointerDown =
      Attr.Handler "pointerdown" (fun el ev ->
        ev.PreventDefault()
        el?setPointerCapture (ev?pointerId)
        isDragging.Value <- true
        updateFromPointer el (float ev?clientX))

    let onPointerMove =
      Attr.Handler "pointermove" (fun el ev ->
        if isDragging.Value then
          updateFromPointer el (float ev?clientX))

    let onPointerUp = Attr.Handler "pointerup" (fun _ _ -> isDragging.Value <- false)

    let onPointerCancel =
      Attr.Handler "pointercancel" (fun _ _ -> isDragging.Value <- false)

    let label =
      labelText
      |> Doc.BindView(fun txt ->
        if not (System.String.IsNullOrEmpty txt) then
          div [ cl Css.``weave-slider__label`` ] [ text txt ]
        else
          Doc.Empty)

    let thumbDoc =
      div [
        cl Css.``weave-slider__thumb``
        Attr.DynamicStyle "left" (pct |> View.Map(sprintf "%.4f%%"))
      ] []

    let fillDoc =
      div [
        cl Css.``weave-slider__fill``
        Attr.DynamicStyle "width" (pct |> View.Map(sprintf "%.4f%%"))
      ] []

    let trackDoc = div [ cl Css.``weave-slider__track`` ] []

    let tickMarksDoc =
      if List.isEmpty tickPositions then
        Doc.Empty
      else
        div [ cl Css.``weave-slider__tick-container`` ] [
          for tickPct in tickPositions do
            pct
            |> Doc.BindView(fun currentPct ->
              let isActive = tickPct <= currentPct

              div [
                cl Css.``weave-slider__tick``
                if isActive then
                  cl Css.``weave-slider__tick--active``
                Attr.Style "left" (sprintf "%.4f%%" tickPct)
              ] [])
        ]

    let tickLabelsDoc =
      if List.isEmpty tickMarkLabels then
        Doc.Empty
      else
        div [ cl Css.``weave-slider__tick-labels`` ] [
          tickMarkLabels
          |> List.map (fun lbl -> div [ cl Css.``weave-slider__tick-label`` ] [ text lbl ])
          |> Doc.Concat
        ]

    let trackContainer =
      div [
        cl Css.``weave-slider__track-container``
        Attr.Create "role" "slider"
        Attr.Create "tabindex" "0"
        valueAsFloat.View
        |> View.Map(sprintf "%.2f")
        |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-valuenow", v))

        labelText
        |> Attr.DynamicCustom(fun el v ->
          if not (System.String.IsNullOrEmpty v) then
            el.SetAttribute("aria-label", v)
          else
            el.RemoveAttribute("aria-label"))

        Attr.Create "aria-valuemin" (sprintf "%.2f" minVal)
        Attr.Create "aria-valuemax" (sprintf "%.2f" maxVal)

        onPointerDown
        onPointerMove
        onPointerUp
        onPointerCancel

        on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
          match ev.Key with
          | "ArrowRight"
          | "ArrowUp" ->
            ev.PreventDefault()
            valueAsFloat.Value <- Bounded.snapToStep minVal maxVal stepVal (valueAsFloat.Value + stepVal)
          | "ArrowLeft"
          | "ArrowDown" ->
            ev.PreventDefault()
            valueAsFloat.Value <- Bounded.snapToStep minVal maxVal stepVal (valueAsFloat.Value - stepVal)
          | "Home" ->
            ev.PreventDefault()
            valueAsFloat.Value <- minVal
          | "End" ->
            ev.PreventDefault()
            valueAsFloat.Value <- maxVal
          | _ -> ())
      ] [ trackDoc; fillDoc; tickMarksDoc; thumbDoc ]

    div [
      cl Css.``weave-slider``

      Disabled.disabledClass Css.``weave-slider--disabled`` enabled

      yield! attrs
    ] [ label; trackContainer; tickLabelsDoc ]

  /// <summary>
  /// Creates a slider for integer values.
  /// </summary>
  static member create
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =

    let minVal = float (defaultArg min 0)
    let maxVal = float (defaultArg max 100)
    let stepVal = float (defaultArg step 1)
    let labelText = defaultArg labelText (View.Const "")
    let showTickMarks = defaultArg showTickMarks false
    let tickMarkLabels = defaultArg tickMarkLabels []
    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    // Bridge Var<int> to Var<float>
    let floatVar = Var.Create(float value.Value)

    let syncToFloat =
      value.View
      |> Doc.BindView(fun v ->
        let f = float v

        if floatVar.Value <> f then
          floatVar.Value <- f

        Doc.Empty)

    let syncToInt =
      floatVar.View
      |> Doc.BindView(fun f ->
        let i = int (round f)

        if value.Value <> i then
          value.Value <- i

        Doc.Empty)

    Doc.Concat [
      syncToFloat
      syncToInt
      Slider.renderSlider (
        floatVar,
        minVal,
        maxVal,
        stepVal,
        labelText,
        showTickMarks,
        tickMarkLabels,
        enabled,
        attrs
      )
    ]

  /// <summary>
  /// Creates a slider for floating-point values.
  /// </summary>
  static member create
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =

    let minVal = defaultArg min 0.0
    let maxVal = defaultArg max 100.0
    let stepVal = defaultArg step 1.0
    let labelText = defaultArg labelText (View.Const "")
    let showTickMarks = defaultArg showTickMarks false
    let tickMarkLabels = defaultArg tickMarkLabels []
    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    Slider.renderSlider (
      value,
      minVal,
      maxVal,
      stepVal,
      labelText,
      showTickMarks,
      tickMarkLabels,
      enabled,
      attrs
    )

  /// <summary>Creates a primary-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.primary</c>.</summary>
  static member primary
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.primary :: defaultArg attrs []
    )

  /// <summary>Creates a primary-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.primary</c>.</summary>
  static member primary
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.primary :: defaultArg attrs []
    )

  /// <summary>Creates a secondary-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.secondary</c>.</summary>
  static member secondary
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.secondary :: defaultArg attrs []
    )

  /// <summary>Creates a secondary-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.secondary</c>.</summary>
  static member secondary
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.secondary :: defaultArg attrs []
    )

  /// <summary>Creates a tertiary-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.tertiary</c>.</summary>
  static member tertiary
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.tertiary :: defaultArg attrs []
    )

  /// <summary>Creates a tertiary-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.tertiary</c>.</summary>
  static member tertiary
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.tertiary :: defaultArg attrs []
    )

  /// <summary>Creates an error-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.error</c>.</summary>
  static member error
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.error :: defaultArg attrs []
    )

  /// <summary>Creates an error-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.error</c>.</summary>
  static member error
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.error :: defaultArg attrs []
    )

  /// <summary>Creates a warning-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.warning</c>.</summary>
  static member warning
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.warning :: defaultArg attrs []
    )

  /// <summary>Creates a warning-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.warning</c>.</summary>
  static member warning
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.warning :: defaultArg attrs []
    )

  /// <summary>Creates a success-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.success</c>.</summary>
  static member success
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.success :: defaultArg attrs []
    )

  /// <summary>Creates a success-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.success</c>.</summary>
  static member success
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.success :: defaultArg attrs []
    )

  /// <summary>Creates an info-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.info</c>.</summary>
  static member info
    (
      value: Var<int>,
      ?min: int,
      ?max: int,
      ?step: int,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.info :: defaultArg attrs []
    )

  /// <summary>Creates an info-colored slider. Shorthand for <c>Slider.create</c> with <c>Slider.Color.info</c>.</summary>
  static member info
    (
      value: Var<float>,
      ?min: float,
      ?max: float,
      ?step: float,
      ?labelText: View<string>,
      ?showTickMarks: bool,
      ?tickMarkLabels: string list,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    Slider.create (
      value,
      ?min = min,
      ?max = max,
      ?step = step,
      ?labelText = labelText,
      ?showTickMarks = showTickMarks,
      ?tickMarkLabels = tickMarkLabels,
      ?enabled = enabled,
      attrs = Slider.Color.info :: defaultArg attrs []
    )
