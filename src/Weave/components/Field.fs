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
module Field =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Standard
    | Filled
    | Outlined

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

  module HelpTextColor =

    let primary = cl Css.``weave-field__help-text--primary``
    let secondary = cl Css.``weave-field__help-text--secondary``
    let tertiary = cl Css.``weave-field__help-text--tertiary``
    let error = cl Css.``weave-field__help-text--error``
    let warning = cl Css.``weave-field__help-text--warning``
    let success = cl Css.``weave-field__help-text--success``
    let info = cl Css.``weave-field__help-text--info``

[<JavaScript; RequireQualifiedAccess>]
type FieldHelpText =

  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    p [ Css.``weave-field__help-text`` |> cl; yield! attrs ] [ content ]

[<JavaScript; RequireQualifiedAccess>]
type Field =

  /// <summary>
  /// Core overload: wraps a pre-built input element with the full field chrome
  /// (label, adornments, outline, help text, variant styling, focus/disabled states).
  /// Use this when you need a custom input element (e.g. numeric, date, etc.).
  /// </summary>
  static member create
    (
      inputElement: Doc,
      isFocused: View<bool>,
      shouldFloat: View<bool>,
      ?variant: Field.Variant,
      ?labelText: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?startAdornment: Doc,
      ?endAdornment: Doc,
      ?typoAttrs: Attr list,
      ?inputId: string,
      ?attrs: Attr list
    ) =

    let variant = defaultArg variant Field.Variant.Standard
    let labelText = defaultArg labelText (View.Const "")
    let showHelpText = defaultArg showHelpText (View.Const false)
    let helpText = defaultArg helpText Doc.Empty
    let enabled = defaultArg enabled (View.Const true)

    let typoAttrs = defaultArg typoAttrs [ cl Css.``weave-typography--body2`` ]

    let attrs = defaultArg attrs List.empty

    let label =
      labelText
      |> Doc.BindView(fun txt ->
        if not (System.String.IsNullOrEmpty(txt)) then
          Html.label [
            cls [ Css.``weave-field__label`` ]
            Attr.DynamicClassPred Css.``weave-field__label--float`` shouldFloat
            match inputId with
            | Some id -> Attr.Create "for" id
            | None -> ()
          ] [ Html.span [] [ text txt ] ]
        else
          Doc.Empty)

    let startAdornmentDoc =
      match startAdornment with
      | Some adornment ->
        div [
          cls [ Css.``weave-field__adornment``; Css.``weave-field__adornment--start`` ]
        ] [ adornment ]
      | None -> Doc.Empty

    let endAdornmentDoc =
      match endAdornment with
      | Some adornment ->
        div [ cls [ Css.``weave-field__adornment``; Css.``weave-field__adornment--end`` ] ] [ adornment ]
      | None -> Doc.Empty

    let outlineDoc =
      match variant with
      | Field.Variant.Outlined ->
        Doc.Element "fieldset" [ Css.``weave-field__outline`` |> cl ] [
          Doc.Element "legend" [
            Css.``weave-field__outline-legend`` |> cl
            Attr.DynamicClassPred Css.``weave-field__outline-legend--float`` shouldFloat
          ] [
            // do not use a typography component here since we need specific
            // styling to deal with the outlined label when it hovers
            Html.span [] [ Doc.TextView labelText ]
          ]
        ]
        :> Doc
      | _ -> Doc.Empty

    let helpTextDoc = helpText

    let hasStartAdornment = startAdornment.IsSome

    div [
      Css.``weave-field`` |> cl
      match variant with
      | Field.Variant.Standard -> Field.Variant.standard
      | Field.Variant.Filled -> Field.Variant.filled
      | Field.Variant.Outlined -> Field.Variant.outlined
      yield! typoAttrs
      if hasStartAdornment then
        Css.``weave-field--has-start-adornment`` |> cl
      Attr.DynamicClassPred Css.``weave-field--focused`` isFocused
      Attr.DynamicClassPred Css.``weave-field--show-help-text`` showHelpText
      Disabled.disabledClass Css.``weave-field--disabled`` enabled
      yield! attrs
    ] [
      div [
        Css.``weave-field__control`` |> cl

        on.clickTap (fun el _ ->
          let inp = el.QuerySelector("input")

          if not (isNull inp) then
            inp?focus ())
      ] [ label; startAdornmentDoc; inputElement; endAdornmentDoc; outlineDoc ]

      helpTextDoc
    ]

  /// <summary>
  /// Convenience overload for text fields: creates the input element internally.
  /// </summary>
  static member create
    (
      value: Var<string>,
      ?variant: Field.Variant,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?shrinkLabel: View<bool>,
      ?startAdornment: Doc,
      ?endAdornment: Doc,
      ?inputAttrs: Attr list,
      ?typoAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let variant = defaultArg variant Field.Variant.Standard
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

    // When no explicit placeholder the label doubles as placeholder, so hide native placeholder.
    // When explicit placeholder is given, show it (browser hides it automatically if there's a value).
    let effectivePlaceholder =
      (placeholder, shouldFloat)
      ||> View.Map2(fun ph floated ->
        if System.String.IsNullOrEmpty(ph) then ""
        elif floated then ph
        else "")

    let inputId = WeaveId.create "weave-field"

    let inputElement =
      Doc.InputType.Text
        [
          cls [ Css.``weave-field__input`` ]
          Attr.Create "id" inputId

          Attr.DynamicProp "placeholder" effectivePlaceholder
          Attr.enabled enabled
          Attr.DynamicBool "readOnly" readOnly

          on.focus (fun _ _ -> isFocused.Value <- true)
          on.blur (fun el _ ->
            el?scrollLeft <- 0
            isFocused.Value <- false)

          yield! inputAttrs
        ]
        value

    Field.create (
      inputElement,
      isFocused.View,
      shouldFloat,
      variant = variant,
      labelText = labelText,
      ?showHelpText = showHelpText,
      ?helpText = helpText,
      enabled = enabled,
      ?startAdornment = startAdornment,
      ?endAdornment = endAdornment,
      ?typoAttrs = typoAttrs,
      inputId = inputId,
      ?attrs = attrs
    )
