namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module TextField =

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

[<JavaScript; RequireQualifiedAccess>]
type TextField =

  static member private createField
    (
      inputElement: Doc,
      isFocused: Var<bool>,
      shouldFloat: View<bool>,
      labelText: View<string>,
      showHelpText: View<bool> option,
      helpText: Doc option,
      enabled: View<bool>,
      startAdornment: Doc option,
      endAdornment: Doc option,
      maxLength: int option,
      debounce: int option,
      onChange: (string -> unit) option,
      value: View<string>,
      isMultiline: bool,
      typoAttrs: Attr list option,
      inputId: string,
      attrs: Attr list option
    ) =

    let debounceSinkDoc =
      match onChange with
      | Some callback ->
        let timeoutHandle = ref (None: JS.Handle option)

        value
        |> Doc.sinkCached (fun v ->
          match debounce with
          | Some delay ->
            match timeoutHandle.Value with
            | Some h -> JS.ClearTimeout h
            | None -> ()

            timeoutHandle.Value <- Some(JS.SetTimeout (fun () -> callback v) delay)
          | None -> callback v)
      | None -> Doc.Empty

    let counterDoc =
      match maxLength with
      | Some ml ->
        Html.span [ Css.``weave-field__counter`` |> cl ] [
          Doc.TextView(value |> View.Map(fun v -> $"{v.Length}/{ml}"))
        ]
      | None -> Doc.Empty

    let fieldDoc =
      Field.create (
        inputElement,
        isFocused.View,
        shouldFloat,
        labelText = labelText,
        ?showHelpText = showHelpText,
        ?helpText = helpText,
        enabled = enabled,
        ?startAdornment = startAdornment,
        ?endAdornment = endAdornment,
        ?typoAttrs = typoAttrs,
        inputId = inputId,
        attrs = [
          if isMultiline then
            Css.``weave-field--multiline`` |> cl
          yield! defaultArg attrs []
        ]
      )

    [ debounceSinkDoc; fieldDoc; counterDoc ] |> Doc.Concat

  static member singleLine
    (
      value: Var<string>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?shrinkLabel: View<bool>,
      ?startAdornment: Doc,
      ?endAdornment: Doc,
      ?maxLength: int,
      ?debounce: int,
      ?onChange: string -> unit,
      ?inputAttrs: Attr list,
      ?typoAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let shrinkLabel = defaultArg shrinkLabel (View.Const false)
    let inputAttrs = defaultArg inputAttrs List.empty

    let isFocused = Var.Create false

    let hasValue =
      value.View |> View.MapCached(fun v -> not (System.String.IsNullOrEmpty(v)))

    let hasExplicitPlaceholder =
      placeholder |> View.MapCached(fun p -> not (System.String.IsNullOrEmpty(p)))

    let shouldFloat =
      isFocused.View <||> hasValue <||> shrinkLabel <||> hasExplicitPlaceholder

    let effectivePlaceholder =
      (placeholder, shouldFloat)
      ||> View.Map2(fun ph floated ->
        if System.String.IsNullOrEmpty(ph) then ""
        elif floated then ph
        else "")

    let inputId = WeaveId.create "weave-text-field"

    let inputElement =
      Doc.InputType.Text
        [
          cls [ Css.``weave-field__input`` ]
          Attr.Create "id" inputId
          Attr.DynamicProp "placeholder" effectivePlaceholder
          Attr.enabled enabled
          Attr.DynamicBool "readOnly" readOnly

          match maxLength with
          | Some ml -> Attr.Create "maxlength" (string ml)
          | None -> ()

          on.focus (fun _ _ -> isFocused.Value <- true)

          on.blur (fun el _ ->
            el?scrollLeft <- 0
            isFocused.Value <- false)

          yield! inputAttrs
        ]
        value

    TextField.createField (
      inputElement,
      isFocused,
      shouldFloat,
      labelText,
      showHelpText,
      helpText,
      enabled,
      startAdornment,
      endAdornment,
      maxLength,
      debounce,
      onChange,
      value.View,
      false,
      typoAttrs,
      inputId,
      attrs
    )

  static member multiLine
    (
      value: Var<string>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?shrinkLabel: View<bool>,
      ?startAdornment: Doc,
      ?endAdornment: Doc,
      ?maxLength: int,
      ?debounce: int,
      ?onChange: string -> unit,
      ?inputAttrs: Attr list,
      ?typoAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let shrinkLabel = defaultArg shrinkLabel (View.Const false)
    let inputAttrs = defaultArg inputAttrs List.empty

    let isFocused = Var.Create false

    let hasValue =
      value.View |> View.MapCached(fun v -> not (System.String.IsNullOrEmpty(v)))

    let hasExplicitPlaceholder =
      placeholder |> View.MapCached(fun p -> not (System.String.IsNullOrEmpty(p)))

    let shouldFloat =
      isFocused.View <||> hasValue <||> shrinkLabel <||> hasExplicitPlaceholder

    let effectivePlaceholder =
      (placeholder, shouldFloat)
      ||> View.Map2(fun ph floated ->
        if System.String.IsNullOrEmpty(ph) then ""
        elif floated then ph
        else "")

    let inputId = WeaveId.create "weave-text-field"

    let inputElement: Doc =
      Doc.Element "textarea" [
        cls [ Css.``weave-field__input`` ]
        Attr.Create "id" inputId
        Attr.DynamicProp "placeholder" effectivePlaceholder
        Attr.enabled enabled
        Attr.DynamicBool "readOnly" readOnly

        match maxLength with
        | Some ml -> Attr.Create "maxlength" (string ml)
        | None -> ()

        Attr.DynamicProp "value" value.View
        on.input (fun el _ -> value.Value <- el?value)
        on.focus (fun _ _ -> isFocused.Value <- true)
        on.blur (fun _ _ -> isFocused.Value <- false)
        yield! inputAttrs
      ] []
      :> Doc

    TextField.createField (
      inputElement,
      isFocused,
      shouldFloat,
      labelText,
      showHelpText,
      helpText,
      enabled,
      startAdornment,
      endAdornment,
      maxLength,
      debounce,
      onChange,
      value.View,
      true,
      typoAttrs,
      inputId,
      attrs
    )
