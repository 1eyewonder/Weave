namespace Weave

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module NumericField =

  module Variant =

    let standard = cl Css.``weave-field--standard``
    let filled = cl Css.``weave-field--filled``
    let outlined = cl Css.``weave-field--outlined``

  module Color =

    let primary = cl Css.``weave-field--primary``
    let secondary = cl Css.``weave-field--secondary``
    let tertiary = cl Css.``weave-field--tertiary``
    let error = cl Css.``weave-field--error``
    let warning = cl Css.``weave-field--warning``
    let success = cl Css.``weave-field--success``
    let info = cl Css.``weave-field--info``

  module Width =

    let full = cl Css.``weave-field--full-width``

[<JavaScript>]
type NumericField =

  /// <summary>
  /// Creates a numeric field for integer values.
  /// </summary>
  static member create
    (
      value: Var<int>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?startAdornment: Doc,
      ?min: int,
      ?max: int,
      ?step: View<int>,
      ?showSpinButtons: View<bool>,
      ?enableArrowKeys: View<bool>,
      ?enableMouseWheel: View<bool>,
      ?upIcon: Doc,
      ?downIcon: Doc,
      ?inputAttrs: Attr list,
      ?typoAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let inputAttrs = defaultArg inputAttrs List.empty
    let minVal = defaultArg min Int32.MinValue
    let maxVal = defaultArg max Int32.MaxValue
    let step = defaultArg step (View.Const 1)
    let showSpinButtons = defaultArg showSpinButtons (View.Const true)
    let enableArrowKeys = defaultArg enableArrowKeys (View.Const true)
    let enableMouseWheel = defaultArg enableMouseWheel (View.Const true)
    let upIcon = defaultArg upIcon (text "▲")
    let downIcon = defaultArg downIcon (text "▼")
    let attrs = defaultArg attrs List.empty

    let isFocused = Var.Create false
    let editable = (enabled, readOnly) ||> View.map2Cached (fun en ro -> en && not ro)

    // String bridge for the HTML input binding.
    let stringVar = Var.Create(string<int> value.Value)

    // Numeric fields always "have a value" so the label should always float.
    let shouldFloat = View.Const true

    let effectivePlaceholder = placeholder

    // Sync: int -> string (when value changes programmatically, e.g. increment/decrement).
    let syncToString =
      value.View
      |> Doc.sinkCached (fun v ->
        let s = string<int> v

        if stringVar.Value <> s then
          stringVar.Value <- s)

    // Sync: string -> int (as the user types valid integers).
    let syncToInt =
      stringVar.View
      |> Doc.sinkCached (fun s ->
        match Int32.TryParse(s) with
        | true, n ->
          let clamped = Bounded.clamp minVal maxVal n

          if value.Value <> clamped then
            value.Value <- clamped
        | false, _ -> ())

    let increment step =
      value.Value <- Bounded.stepUp minVal maxVal step value.Value

    let decrement step =
      value.Value <- Bounded.stepDown minVal maxVal step value.Value

    // Snapshot vars for event handlers that can't use *View helpers.
    let currentStep = Var.Create 1
    let currentEditable = Var.Create true
    let currentArrowKeys = Var.Create true
    let currentMouseWheel = Var.Create true
    let stepSync = step |> Doc.sinkCached (fun s -> currentStep.Value <- s)
    let editableSync = editable |> Doc.sinkCached (fun e -> currentEditable.Value <- e)

    let arrowKeysSync =
      enableArrowKeys |> Doc.sinkCached (fun v -> currentArrowKeys.Value <- v)

    let mouseWheelSync =
      enableMouseWheel |> Doc.sinkCached (fun v -> currentMouseWheel.Value <- v)

    let clickState = View.Map2 (fun s e -> s, e) step editable

    let inputId = WeaveId.create "weave-field"

    let inputElement =
      Doc.InputType.Text
        [
          cls [ Css.``weave-field__input`` ]
          Attr.Create "id" inputId
          attr.``type`` "number"

          Attr.DynamicProp "placeholder" effectivePlaceholder
          Attr.enabled enabled
          Attr.DynamicBool "readOnly" readOnly

          yield! inputAttrs

          on.focus (fun _ _ -> isFocused.Value <- true)

          let stringView = stringVar.View |> View.Map Int32.TryParse

          on.blurView stringView (fun el _ inputToString ->
            el?scrollLeft <- 0
            isFocused.Value <- false
            // On blur, clamp and reset display to the canonical valid value.
            match inputToString with
            | true, n -> value.Value <- Bounded.clamp minVal maxVal n
            | false, _ -> ()

            stringVar.Value <- string<int> value.Value)

          on.keyDown (fun _ ev ->
            match ev with
            | Key.ArrowUp ->
              ev.PreventDefault()

              if currentEditable.Value && currentArrowKeys.Value then
                increment currentStep.Value
            | Key.ArrowDown ->
              ev.PreventDefault()

              if currentEditable.Value && currentArrowKeys.Value then
                decrement currentStep.Value
            | _ -> ())

          on.wheel (fun _ (ev: Dom.WheelEvent) ->
            if isFocused.Value && currentEditable.Value && currentMouseWheel.Value then
              ev.PreventDefault()
              let dy: float = ev.DeltaY

              if dy < 0.0 then
                increment currentStep.Value
              elif dy > 0.0 then
                decrement currentStep.Value)
        ]
        stringVar

    // Spin buttons (up / down) — only rendered when showSpinButtons is true.
    let spinButtonsDoc =
      showSpinButtons
      |> Doc.BindView(fun show ->
        if show then
          div [
            Css.``weave-field__spin-buttons`` |> cl
            // Prevent mousedown from stealing focus from the input, which
            // causes a blur→focus flicker and a 1px layout shift while held.
            on.mouseDown (fun _ ev -> ev.PreventDefault())
          ] [
            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Increment"
              on.clickTapView clickState (fun _ _ (step, canEdit) ->
                if canEdit then
                  increment step)
            ] [ upIcon ]

            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Decrement"
              on.clickTapView clickState (fun _ _ (step, canEdit) ->
                if canEdit then
                  decrement step)
            ] [ downIcon ]
          ]
        else
          Doc.Empty)

    [
      syncToString
      syncToInt
      stepSync
      editableSync
      arrowKeysSync
      mouseWheelSync
      Field.create (
        inputElement,
        isFocused.View,
        shouldFloat,
        labelText = labelText,
        ?showHelpText = showHelpText,
        ?helpText = helpText,
        enabled = enabled,
        endAdornment = spinButtonsDoc,
        ?startAdornment = startAdornment,
        ?typoAttrs = typoAttrs,
        inputId = inputId,
        attrs = [
          Css.``weave-field--numeric`` |> cl
          Attr.DynamicClassPred Css.``weave-field--has-spin`` showSpinButtons
          yield! attrs
        ]
      )
    ]
    |> Doc.Concat

  /// <summary>
  /// Creates a numeric field for floating-point values.
  /// </summary>
  static member create
    (
      value: Var<float>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?startAdornment: Doc,
      ?min: float,
      ?max: float,
      ?step: View<float>,
      ?showSpinButtons: View<bool>,
      ?enableArrowKeys: View<bool>,
      ?enableMouseWheel: View<bool>,
      ?upIcon: Doc,
      ?downIcon: Doc,
      ?inputAttrs: Attr list,
      ?typoAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let inputAttrs = defaultArg inputAttrs List.empty
    let minVal = defaultArg min -infinity
    let maxVal = defaultArg max infinity
    let step = defaultArg step (View.Const 1.0)
    let showSpinButtons = defaultArg showSpinButtons (View.Const true)
    let enableArrowKeys = defaultArg enableArrowKeys (View.Const true)
    let enableMouseWheel = defaultArg enableMouseWheel (View.Const true)
    let upIcon = defaultArg upIcon (text "▲")
    let downIcon = defaultArg downIcon (text "▼")
    let attrs = defaultArg attrs List.empty

    let isFocused = Var.Create false
    let editable = (enabled, readOnly) ||> View.map2Cached (fun en ro -> en && not ro)

    let stringVar = Var.Create(string<float> value.Value)

    // Numeric fields always "have a value" so the label should always float.
    let shouldFloat = View.Const true

    let effectivePlaceholder = placeholder

    let syncToString =
      value.View
      |> Doc.sinkCached (fun v ->
        let s = string<float> v

        if stringVar.Value <> s then
          stringVar.Value <- s)

    let syncToFloat =
      stringVar.View
      |> Doc.sinkCached (fun s ->
        match Double.TryParse(s) with
        | true, n when not (Double.IsNaN(n)) ->
          let clamped = Bounded.clamp minVal maxVal n

          if value.Value <> clamped then
            value.Value <- clamped
        | _ -> ())

    // Counts the decimal places in a float's string representation.
    // Used to round step results back to a clean precision (avoids 3.24 + 0.1 = 3.3400000000000003).
    let decimalPlaces (f: float) =
      let s = string f

      match s.IndexOf('.') with
      | -1 -> 0
      | i -> s.Length - i - 1

    // Rounds f to dp decimal places using Operators.round (avoids WebSharper.JavaScript.Math conflicts).
    let roundToDecimals (dp: int) (f: float) =
      let factor = pown 10.0 dp
      round (f * factor) / factor

    let increment step =
      let raw = Bounded.stepUp minVal maxVal step value.Value
      let a, b = decimalPlaces step, decimalPlaces value.Value
      value.Value <- roundToDecimals (if a >= b then a else b) raw

    let decrement step =
      let raw = Bounded.stepDown minVal maxVal step value.Value
      let a, b = decimalPlaces step, decimalPlaces value.Value
      value.Value <- roundToDecimals (if a >= b then a else b) raw

    let currentStep = Var.Create 1.0
    let currentEditable = Var.Create true
    let currentArrowKeys = Var.Create true
    let currentMouseWheel = Var.Create true
    let stepSync = step |> Doc.sinkCached (fun s -> currentStep.Value <- s)
    let editableSync = editable |> Doc.sinkCached (fun e -> currentEditable.Value <- e)

    let arrowKeysSync =
      enableArrowKeys |> Doc.sinkCached (fun v -> currentArrowKeys.Value <- v)

    let mouseWheelSync =
      enableMouseWheel |> Doc.sinkCached (fun v -> currentMouseWheel.Value <- v)

    let clickState = View.Map2 (fun s e -> s, e) step editable

    let inputId = WeaveId.create "weave-field"

    let inputElement =
      Doc.InputType.Text
        [
          cls [ Css.``weave-field__input`` ]
          Attr.Create "id" inputId
          attr.``type`` "number"
          Attr.Create "step" "any"

          Attr.DynamicProp "placeholder" effectivePlaceholder
          Attr.enabled enabled
          Attr.DynamicBool "readOnly" readOnly

          yield! inputAttrs

          on.focus (fun _ _ -> isFocused.Value <- true)

          let stringView = stringVar.View |> View.Map Double.TryParse

          on.blurView stringView (fun el _ inputToString ->
            el?scrollLeft <- 0
            isFocused.Value <- false

            match inputToString with
            | true, n when not (Double.IsNaN(n)) -> value.Value <- Bounded.clamp minVal maxVal n
            | _ -> ()

            stringVar.Value <- string<float> value.Value)

          on.keyDown (fun _ ev ->
            match ev with
            | Key.ArrowUp ->
              ev.PreventDefault()

              if currentEditable.Value && currentArrowKeys.Value then
                increment currentStep.Value
            | Key.ArrowDown ->
              ev.PreventDefault()

              if currentEditable.Value && currentArrowKeys.Value then
                decrement currentStep.Value
            | _ -> ())

          on.wheel (fun _ (ev: Dom.WheelEvent) ->
            if isFocused.Value && currentEditable.Value && currentMouseWheel.Value then
              ev.PreventDefault()
              let dy: float = ev.DeltaY

              if dy < 0.0 then
                increment currentStep.Value
              elif dy > 0.0 then
                decrement currentStep.Value)
        ]
        stringVar

    let spinButtonsDoc =
      showSpinButtons
      |> Doc.BindView(fun show ->
        if show then
          div [
            Css.``weave-field__spin-buttons`` |> cl
            // Prevent mousedown from stealing focus from the input, which
            // causes a blur→focus flicker and a 1px layout shift while held.
            on.mouseDown (fun _ ev -> ev.PreventDefault())
          ] [
            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Increment"
              on.clickTapView clickState (fun _ _ (step, canEdit) ->
                if canEdit then
                  increment step)
            ] [ upIcon ]

            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Decrement"
              on.clickTapView clickState (fun _ _ (step, canEdit) ->
                if canEdit then
                  decrement step)
            ] [ downIcon ]
          ]
        else
          Doc.Empty)

    [
      syncToString
      syncToFloat
      stepSync
      editableSync
      arrowKeysSync
      mouseWheelSync
      Field.create (
        inputElement,
        isFocused.View,
        shouldFloat,
        labelText = labelText,
        ?showHelpText = showHelpText,
        ?helpText = helpText,
        enabled = enabled,
        endAdornment = spinButtonsDoc,
        ?startAdornment = startAdornment,
        ?typoAttrs = typoAttrs,
        inputId = inputId,
        attrs = [
          Css.``weave-field--numeric`` |> cl
          Attr.DynamicClassPred Css.``weave-field--has-spin`` showSpinButtons
          yield! attrs
        ]
      )
    ]
    |> Doc.Concat
