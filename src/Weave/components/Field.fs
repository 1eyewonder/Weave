namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave.CssHelpers

[<JavaScript>]
module Field =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Standard
    | Filled
    | Outlined

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Standard -> Css.``weave-field--standard``
      | Variant.Filled -> Css.``weave-field--filled``
      | Variant.Outlined -> Css.``weave-field--outlined``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-field--primary``
      | BrandColor.Secondary -> Css.``weave-field--secondary``
      | BrandColor.Tertiary -> Css.``weave-field--tertiary``
      | BrandColor.Error -> Css.``weave-field--error``
      | BrandColor.Warning -> Css.``weave-field--warning``
      | BrandColor.Success -> Css.``weave-field--success``
      | BrandColor.Info -> Css.``weave-field--info``

  [<RequireQualifiedAccess; Struct>]
  type Width =
    | Full
    | Auto

  module Width =

    let toClass width =
      match width with
      | Width.Full -> Some Css.``weave-field--full-width``
      | Width.Auto -> None

  module HelpTextColor =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-field__help-text--primary``
      | BrandColor.Secondary -> Css.``weave-field__help-text--secondary``
      | BrandColor.Tertiary -> Css.``weave-field__help-text--tertiary``
      | BrandColor.Error -> Css.``weave-field__help-text--error``
      | BrandColor.Warning -> Css.``weave-field__help-text--warning``
      | BrandColor.Success -> Css.``weave-field__help-text--success``
      | BrandColor.Info -> Css.``weave-field__help-text--info``

open Field

[<JavaScript>]
type FieldHelpText =

  static member Create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    p [ Css.``weave-field__help-text`` |> cl; yield! attrs ] [ content ]

[<JavaScript>]
type Field =

  /// Core overload: wraps a pre-built input element with the full field chrome
  /// (label, adornments, outline, help text, variant styling, focus/disabled states).
  /// Use this when you need a custom input element (e.g. numeric, date, etc.).
  static member Create
    (
      inputElement: Doc,
      isFocused: View<bool>,
      shouldFloat: View<bool>,
      ?variant: Variant,
      ?labelText: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?enabled: View<bool>,
      ?startAdornment: Doc,
      ?endAdornment: Doc,
      ?typoAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let variant = defaultArg variant Variant.Standard
    let labelText = defaultArg labelText (View.Const "")
    let showHelpText = defaultArg showHelpText (View.Const false)
    let helpText = defaultArg helpText Doc.Empty
    let enabled = defaultArg enabled (View.Const true)

    let typoAttrs =
      defaultArg typoAttrs [ Typography.Typo.toClass Typography.Typo.Body2 |> cl ]

    let attrs = defaultArg attrs List.empty

    let label =
      labelText
      |> Doc.BindView(fun txt ->
        if not (System.String.IsNullOrEmpty(txt)) then
          Html.label [
            cls [ Css.``weave-field__label`` ]
            Attr.DynamicClassPred Css.``weave-field__label--float`` shouldFloat
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
      | Variant.Outlined ->
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
      Variant.toClass variant |> cl
      yield! typoAttrs
      if hasStartAdornment then
        Css.``weave-field--has-start-adornment`` |> cl
      Attr.DynamicClassPred Css.``weave-field--focused`` isFocused
      Attr.DynamicClassPred Css.``weave-field--show-help-text`` showHelpText
      View.not enabled |> Attr.DynamicClassPred Css.``weave-field--disabled``
      yield! attrs
    ] [
      div [
        Css.``weave-field__control`` |> cl

        on.click (fun el _ ->
          let inp = el.QuerySelector("input")

          if not (isNull inp) then
            inp?focus ())
      ] [ label; startAdornmentDoc; inputElement; endAdornmentDoc; outlineDoc ]

      helpTextDoc
    ]

  /// Convenience overload for text fields: creates the input element internally.
  static member Create
    (
      value: Var<string>,
      ?variant: Variant,
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

    let variant = defaultArg variant Variant.Standard
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

    let inputElement =
      Doc.InputType.Text
        [
          cls [ Css.``weave-field__input`` ]

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

    Field.Create(
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
      ?attrs = attrs
    )
