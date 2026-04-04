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
module FieldHelpText =

  module Color =

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

    let labelText = defaultArg labelText (View.Const "")
    let showHelpText = defaultArg showHelpText (View.Const false)
    let helpText = defaultArg helpText Doc.Empty
    let enabled = defaultArg enabled (View.Const true)

    let typoAttrs = defaultArg typoAttrs [ Typography.body2 ]

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

    let hasLabel =
      labelText |> View.MapCached(fun txt -> not (System.String.IsNullOrEmpty txt))

    let legendShouldFloat = shouldFloat <&&> hasLabel

    let outlineDoc =
      Doc.Element "fieldset" [ Css.``weave-field__outline`` |> cl ] [
        Doc.Element "legend" [
          Css.``weave-field__outline-legend`` |> cl
          Attr.DynamicClassPred Css.``weave-field__outline-legend--float`` legendShouldFloat
        ] [ Html.span [] [ Doc.TextView labelText ] ]
      ]
      :> Doc

    let helpTextDoc = helpText

    let hasStartAdornment = startAdornment.IsSome

    div [
      Css.``weave-field`` |> cl
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
            inp?focus ()
          else
            let ta = el.QuerySelector("textarea")

            if not (isNull ta) then
              ta?focus ())
      ] [ label; startAdornmentDoc; inputElement; endAdornmentDoc; outlineDoc ]

      helpTextDoc
    ]
