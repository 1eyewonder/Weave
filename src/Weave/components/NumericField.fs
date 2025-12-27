namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers
open WebSharper.UI.Client

[<JavaScript>]
module NumericField =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Text
    | Filled
    | Outlined

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Text -> Css.``weave-input--text``
      | Variant.Filled -> Css.``weave-input--filled``
      | Variant.Outlined -> Css.``weave-input--outlined``

  [<RequireQualifiedAccess; Struct>]
  type Margin =
    | None
    | Dense
    | Normal

  module Margin =

    let toClass margin =
      match margin with
      | Margin.None -> Css.``weave-input--margin-none``
      | Margin.Dense -> Css.``weave-input--margin-dense``
      | Margin.Normal -> Css.``weave-input--margin-normal``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-input--primary``
      | BrandColor.Secondary -> Css.``weave-input--secondary``
      | BrandColor.Tertiary -> Css.``weave-input--tertiary``
      | BrandColor.Error -> Css.``weave-input--error``
      | BrandColor.Warning -> Css.``weave-input--warning``
      | BrandColor.Success -> Css.``weave-input--success``
      | BrandColor.Info -> Css.``weave-input--info``

open NumericField

[<JavaScript>]
type NumericField =

  [<Inline>]
  static member inline private Create<'T
    when ^T: (static member (+): ^T * ^T -> ^T)
    and ^T: (static member (-): ^T * ^T -> ^T)
    and ^T: (static member One: ^T)
    and ^T: comparison>
    (
      f: seq<Attr> -> Var<CheckedInput<'T>> -> Doc,
      value: Var<CheckedInput<'T>>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helperText: View<string>,
      ?errorText: View<string>,
      ?min: View<'T>,
      ?max: View<'T>,
      ?step: View<'T>,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?error: (View<CheckedInput<'T>> -> View<bool>),
      ?attrs: Attr list
    ) =
    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let helperText = defaultArg helperText (View.Const "")
    let errorText = defaultArg errorText (View.Const "")
    let step = defaultArg step (View.Const LanguagePrimitives.GenericOne<'T>)
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let errorView = defaultArg error (fun _ -> View.Const false) <| value.View
    let attrs = defaultArg attrs List.empty
    let min = ViewOption.sequence min
    let max = ViewOption.sequence max

    let isFocused = Var.Create false

    let increment max step =
      match value.Value with
      | Valid(v, _) ->
        let newValue = v + step

        match max with
        | Some maxVal when newValue <= maxVal -> value.Value <- CheckedInput.Make newValue
        | None -> value.Value <- CheckedInput.Make newValue
        | _ -> ()
      | _ -> ()

    let decrement min step =
      match value.Value with
      | Valid(v, _) ->
        let newValue = v - step

        match min with
        | Some minVal when newValue >= minVal -> value.Value <- CheckedInput.Make newValue
        | None -> value.Value <- CheckedInput.Make newValue
        | _ -> ()
      | _ -> ()

    let handleKeyDown (min, max, step) (event: Dom.KeyboardEvent) =
      match event.Key with
      | "ArrowUp" ->
        event.PreventDefault()
        increment max step
      | "ArrowDown" ->
        event.PreventDefault()
        decrement min step
      | _ -> ()

    let label =
      labelText
      |> Doc.BindView(fun txt ->
        if not <| System.String.IsNullOrEmpty(txt) then
          label [
            Css.``weave-input__label`` |> cl
            Attr.DynamicClassPred Css.``weave-input__label--focused`` isFocused.View
          ] [ text txt ]
        else
          Doc.Empty)

    let inputContainer =
      value
      |> f [
        Css.``weave-input__root`` |> cl
        attr.``type`` "number"
        Attr.DynamicProp "placeholder" placeholder
        Attr.enabled enabled
        Attr.DynamicClassPred "readonly" readOnly

        on.focus (fun _ _ -> isFocused.Value <- true)
        on.blur (fun _ _ -> isFocused.Value <- false)

        let constraints = (min, max, step) |||> View.zip3

        on.keyDownView constraints (fun _ event constraints -> handleKeyDown constraints event)
      ]

    let spinButtons =
      View.Map2 (fun enabled ro -> enabled && not ro) enabled readOnly
      |> Doc.BindView(fun shouldShow ->
        if shouldShow then
          div [ Css.``weave-input__numeric-spin`` |> cl ] [

            (max, step)
            ||> View.zipCached
            |> Doc.BindView(fun (max, step) ->
              Button.Create(
                innerContents = text "▲",
                onClick = (fun () -> increment max step),
                enabled = enabled,
                attrs = [
                  Variant.toClass Variant.Text |> cl
                  Attr.Tab.skip
                  Attr.Create "aria-label" "Increment"
                ]
              ))

            (min, step)
            ||> View.zipCached
            |> Doc.BindView(fun (min, step) ->
              Button.Create(
                innerContents = text "▼",
                onClick = (fun () -> decrement min step),
                enabled = enabled,
                attrs = [
                  Variant.toClass Variant.Text |> cl
                  Attr.Tab.skip
                  Attr.Create "aria-label" "Decrement"
                ]
              ))
          ]
        else
          Doc.Empty)

    let helperText =
      (helperText, errorView)
      ||> View.zipCached
      |> Doc.BindView(fun (helper, isError) ->
        if not (System.String.IsNullOrEmpty(helper)) && not isError then
          div [ Css.``weave-input__helper-text`` |> cl ] [ text helper ]
        else
          Doc.Empty)

    let errorText =
      (errorText, errorView)
      ||> View.zipCached
      |> Doc.BindView(fun (errTxt, isError) ->
        if not (System.String.IsNullOrEmpty(errTxt)) && isError then
          div [ Css.``weave-input__error-text`` |> cl ] [ text errTxt ]
        else
          Doc.Empty)

    div [
      Css.``weave-input`` |> cl
      Css.``weave-input--field`` |> cl
      Css.``weave-input--hide-native-spin`` |> cl
      View.not enabled |> Attr.DynamicClassPred Css.``weave-input--disabled``
      Attr.DynamicClassPred Css.``weave-input--error`` errorView
      yield! attrs
    ] [
      label

      div [
        Css.``weave-input__input-wrapper`` |> cl

        (enabled, readOnly)
        ||> View.map2Cached (fun en ro -> en && not ro)
        |> Attr.DynamicClassPred Css.``weave-input__input-wrapper--with-spin``
      ] [ inputContainer; spinButtons ]

      helperText
      errorText
    ]

  [<Inline>]
  static member inline Create
    (
      value: Var<CheckedInput<int>>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helperText: View<string>,
      ?errorText: View<string>,
      ?min: View<int>,
      ?max: View<int>,
      ?step: View<int>,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?error: (View<CheckedInput<int>> -> View<bool>),
      ?attrs: Attr list
    ) =
    NumericField.Create<int>(
      Doc.InputType.Int,
      value,
      ?labelText = labelText,
      ?placeholder = placeholder,
      ?helperText = helperText,
      ?errorText = errorText,
      ?min = min,
      ?max = max,
      ?step = step,
      ?enabled = enabled,
      ?readOnly = readOnly,
      ?error = error,
      ?attrs = attrs
    )

  [<Inline>]
  static member inline Create
    (
      value: Var<CheckedInput<float>>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helperText: View<string>,
      ?errorText: View<string>,
      ?min: View<float>,
      ?max: View<float>,
      ?step: View<float>,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?error: (View<CheckedInput<float>> -> View<bool>),
      ?attrs: Attr list
    ) =
    NumericField.Create<float>(
      Doc.InputType.Float,
      value,
      ?labelText = labelText,
      ?placeholder = placeholder,
      ?helperText = helperText,
      ?errorText = errorText,
      ?min = min,
      ?max = max,
      ?step = step,
      ?enabled = enabled,
      ?readOnly = readOnly,
      ?error = error,
      ?attrs = attrs
    )
