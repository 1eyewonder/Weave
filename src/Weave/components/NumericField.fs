namespace Weave

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers

[<JavaScript>]
module NumericField =

  type Variant = Field.Variant

  let internal clamp lo hi v = Operators.max lo (Operators.min hi v)

open NumericField

[<JavaScript>]
type NumericField =

  /// Creates a numeric field for integer values.
  static member Create
    (
      value: Var<int>,
      ?variant: Variant,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?shrinkLabel: View<bool>,
      ?startAdornment: Doc,
      ?min: int,
      ?max: int,
      ?step: View<int>,
      ?showSpinButtons: View<bool>,
      ?enableArrowKeys: View<bool>,
      ?enableMouseWheel: View<bool>,
      ?upIcon: Doc,
      ?downIcon: Doc,
      ?attrs: Attr list
    ) =

    let variant = defaultArg variant Variant.Standard
    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let shrinkLabel = defaultArg shrinkLabel (View.Const false)
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
    let hasValue = View.Const true

    let hasExplicitPlaceholder =
      placeholder |> View.MapCached(fun p -> not (String.IsNullOrEmpty(p)))

    let shouldFloat =
      isFocused.View <||> hasValue <||> shrinkLabel <||> hasExplicitPlaceholder

    let effectivePlaceholder =
      (placeholder, shouldFloat)
      ||> View.Map2(fun ph floated ->
        if String.IsNullOrEmpty(ph) then ""
        elif floated then ph
        else "")

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
          let clamped = clamp minVal maxVal n

          if value.Value <> clamped then
            value.Value <- clamped
        | false, _ -> ())

    let increment step =
      value.Value <- clamp minVal maxVal (value.Value + step)

    let decrement step =
      value.Value <- clamp minVal maxVal (value.Value - step)

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

    // Build the native input element.
    let inputElement =
      Doc.InputType.Text
        [
          cls [ Css.``weave-field__input``; Css.``weave-typography--body2`` ]
          attr.``type`` "number"

          Attr.DynamicProp "placeholder" effectivePlaceholder
          Attr.enabled enabled
          Attr.DynamicBool "readOnly" readOnly

          on.focus (fun _ _ -> isFocused.Value <- true)

          let stringView = stringVar.View |> View.Map Int32.TryParse

          on.blurView stringView (fun el _ inputToString ->
            el?scrollLeft <- 0
            isFocused.Value <- false
            // On blur, clamp and reset display to the canonical valid value.
            match inputToString with
            | true, n -> value.Value <- clamp minVal maxVal n
            | false, _ -> ()

            stringVar.Value <- string<int> value.Value)

          on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
            match ev.Key with
            | "ArrowUp" ->
              ev.PreventDefault()

              if currentEditable.Value && currentArrowKeys.Value then
                increment currentStep.Value
            | "ArrowDown" ->
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
          div [ Css.``weave-field__spin-buttons`` |> cl ] [
            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Increment"
              on.clickView clickState (fun _ _ (step, canEdit) ->
                if canEdit then
                  increment step)
            ] [ upIcon ]

            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Decrement"
              on.clickView clickState (fun _ _ (step, canEdit) ->
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
      Field.Create(
        inputElement,
        isFocused.View,
        shouldFloat,
        variant = variant,
        labelText = labelText,
        ?showHelpText = showHelpText,
        ?helpText = helpText,
        enabled = enabled,
        endAdornment = spinButtonsDoc,
        ?startAdornment = startAdornment,
        attrs = [
          Css.``weave-field--numeric`` |> cl
          Attr.DynamicClassPred Css.``weave-field--has-spin`` showSpinButtons
          yield! attrs
        ]
      )
    ]
    |> Doc.Concat

  /// Creates a numeric field for floating-point values.
  static member Create
    (
      value: Var<float>,
      ?variant: Variant,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?shrinkLabel: View<bool>,
      ?startAdornment: Doc,
      ?min: float,
      ?max: float,
      ?step: View<float>,
      ?showSpinButtons: View<bool>,
      ?enableArrowKeys: View<bool>,
      ?enableMouseWheel: View<bool>,
      ?upIcon: Doc,
      ?downIcon: Doc,
      ?attrs: Attr list
    ) =

    let variant = defaultArg variant Variant.Standard
    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let shrinkLabel = defaultArg shrinkLabel (View.Const false)
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

    let hasValue = View.Const true

    let hasExplicitPlaceholder =
      placeholder |> View.MapCached(fun p -> not (String.IsNullOrEmpty(p)))

    let shouldFloat =
      isFocused.View <||> hasValue <||> shrinkLabel <||> hasExplicitPlaceholder

    let effectivePlaceholder =
      (placeholder, shouldFloat)
      ||> View.Map2(fun ph floated ->
        if String.IsNullOrEmpty(ph) then ""
        elif floated then ph
        else "")

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
          let clamped = clamp minVal maxVal n

          if value.Value <> clamped then
            value.Value <- clamped
        | _ -> ())

    let increment step =
      value.Value <- clamp minVal maxVal (value.Value + step)

    let decrement step =
      value.Value <- clamp minVal maxVal (value.Value - step)

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

    let inputElement =
      Doc.InputType.Text
        [
          cls [ Css.``weave-field__input``; Css.``weave-typography--body2`` ]
          attr.``type`` "number"
          Attr.Create "step" "any"

          Attr.DynamicProp "placeholder" effectivePlaceholder
          Attr.enabled enabled
          Attr.DynamicBool "readOnly" readOnly

          on.focus (fun _ _ -> isFocused.Value <- true)

          let stringView = stringVar.View |> View.Map Double.TryParse

          on.blurView stringView (fun el _ inputToString ->
            el?scrollLeft <- 0
            isFocused.Value <- false

            match inputToString with
            | true, n when not (Double.IsNaN(n)) -> value.Value <- clamp minVal maxVal n
            | _ -> ()

            stringVar.Value <- string<float> value.Value)

          on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
            match ev.Key with
            | "ArrowUp" ->
              ev.PreventDefault()

              if currentEditable.Value && currentArrowKeys.Value then
                increment currentStep.Value
            | "ArrowDown" ->
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
          div [ Css.``weave-field__spin-buttons`` |> cl ] [
            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Increment"
              on.clickView clickState (fun _ _ (step, canEdit) ->
                if canEdit then
                  increment step)
            ] [ upIcon ]

            button [
              Css.``weave-field__spin-btn`` |> cl
              attr.``type`` "button"
              Attr.Tab.natural
              Attr.Create "aria-label" "Decrement"
              on.clickView clickState (fun _ _ (step, canEdit) ->
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
      Field.Create(
        inputElement,
        isFocused.View,
        shouldFloat,
        variant = variant,
        labelText = labelText,
        ?showHelpText = showHelpText,
        ?helpText = helpText,
        enabled = enabled,
        endAdornment = spinButtonsDoc,
        ?startAdornment = startAdornment,
        attrs = [
          Css.``weave-field--numeric`` |> cl
          Attr.DynamicClassPred Css.``weave-field--has-spin`` showSpinButtons
          yield! attrs
        ]
      )
    ]
    |> Doc.Concat
