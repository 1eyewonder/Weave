namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module Progress =

  module Color =
    let primary = cl Css.``weave-progress--primary``
    let secondary = cl Css.``weave-progress--secondary``
    let tertiary = cl Css.``weave-progress--tertiary``
    let success = cl Css.``weave-progress--success``
    let error = cl Css.``weave-progress--error``
    let warning = cl Css.``weave-progress--warning``
    let info = cl Css.``weave-progress--info``

  let private finite value =
    not (System.Double.IsNaN value || System.Double.IsInfinity value)

  let private state value indeterminate minValue maxValue =
    let lo, hi =
      if
        finite minValue
        && finite maxValue
        && maxValue > minValue
        && finite (maxValue - minValue)
      then
        minValue, maxValue
      else
        0.0, 100.0

    let valueView = defaultArg value (View.Const lo)

    let current =
      valueView
      |> View.MapCached(fun v -> if System.Double.IsNaN v then lo else Bounded.clamp lo hi v)

    let percent = current |> View.MapCached(Bounded.percentOf lo hi)
    let busy = defaultArg indeterminate (View.Const(Option.isNone value))
    lo, hi, current, percent, busy

  let private attributes lo hi current percent busy = [
    cl Css.``weave-progress``
    Attr.Create "role" "progressbar"
    Attr.Create "aria-label" "Progress"
    Attr.Create "aria-valuemin" (string lo)
    Attr.Create "aria-valuemax" (string hi)
    busy |> Attr.DynamicClassPred Css.``weave-progress--indeterminate``

    View.Map2 (fun isBusy value -> if isBusy then None else Some value) busy current
    |> Attr.DynamicCustom(fun el value ->
      match value with
      | Some v -> el.SetAttribute("aria-valuenow", string v)
      | None -> el.RemoveAttribute("aria-valuenow"))

    percent |> View.MapCached string |> Attr.DynamicStyle "--progress-value"
  ]

  module Linear =
    module Size =
      let small = cl Css.``weave-progress-linear--small``
      let medium = cl Css.``weave-progress-linear--medium``
      let large = cl Css.``weave-progress-linear--large``

    module Variant =
      let rounded = cl Css.``weave-progress-linear--rounded``
      let striped = cl Css.``weave-progress-linear--striped``

  type Linear =
    /// <summary>Displays completed work as a horizontal bar. Reads values without modifying caller state.</summary>
    /// <param name="value">Completed work in the range's units. Omit for indeterminate mode by default.</param>
    /// <param name="indeterminate">Overrides the mode reactively. True displays activity without an accessible current value.</param>
    /// <param name="min">Lower bound, default 0. Bounds are fixed at creation. Invalid ranges fall back to 0–100.</param>
    /// <param name="max">Upper bound, default 100. Must be finite and greater than min, with a finite difference.</param>
    /// <param name="bufferValue">Total prepared work from min, not an increment. Drawn behind completion and never below it. Hidden in indeterminate mode.</param>
    /// <param name="attrs">Style and native attributes, applied last. Set aria-label or aria-labelledby here to name the operation.</param>
    static member create
      (
        ?value: View<float>,
        ?indeterminate: View<bool>,
        ?min: float,
        ?max: float,
        ?bufferValue: View<float>,
        ?attrs: Attr list
      ) =
      let lo, hi, current, percent, busy =
        state value indeterminate (defaultArg min 0.0) (defaultArg max 100.0)

      div [
        yield! attributes lo hi current percent busy
        cl Css.``weave-progress-linear``
        yield! defaultArg attrs []
      ] [
        div [ cl Css.``weave-progress-linear__track``; Attr.Create "aria-hidden" "true" ] []
        match bufferValue with
        | Some buffer ->
          div [
            cl Css.``weave-progress-linear__buffer``
            Attr.Create "aria-hidden" "true"
            View.Map2
              (fun value buffered ->
                let buffered = if System.Double.IsNaN buffered then lo else buffered
                Bounded.percentOf lo hi (Operators.max value buffered))
              current
              buffer
            |> View.MapCached(fun p -> string p + "%")
            |> Attr.DynamicStyle "width"
          ] []
        | None -> ()
        div [ cl Css.``weave-progress-linear__fill``; Attr.Create "aria-hidden" "true" ] []
      ]

  module Circular =
    module Size =
      let small = cl Css.``weave-progress-circular--small``
      let medium = cl Css.``weave-progress-circular--medium``
      let large = cl Css.``weave-progress-circular--large``

  type Circular =
    /// <summary>Displays completed work as a circular ring. Reads values without modifying caller state.</summary>
    /// <param name="value">Completed work in the range's units. Omit for indeterminate mode by default.</param>
    /// <param name="indeterminate">Overrides the mode reactively. True displays activity without an accessible current value.</param>
    /// <param name="min">Lower bound, default 0. Bounds are fixed at creation. Invalid ranges fall back to 0–100.</param>
    /// <param name="max">Upper bound, default 100. Must be finite and greater than min, with a finite difference.</param>
    /// <param name="content">Optional center content. Keep it short and non-interactive. It does not replace the accessible name.</param>
    /// <param name="attrs">Style and native attributes, applied last. Set aria-label or aria-labelledby here to name the operation.</param>
    static member create
      (
        ?value: View<float>,
        ?indeterminate: View<bool>,
        ?min: float,
        ?max: float,
        ?content: Doc,
        ?attrs: Attr list
      ) =
      let lo, hi, current, percent, busy =
        state value indeterminate (defaultArg min 0.0) (defaultArg max 100.0)

      let circle className =
        Doc.SvgElement "circle" [
          cl className
          Attr.Create "cx" "24"
          Attr.Create "cy" "24"
          Attr.Create "r" "20"
          Attr.Create "pathLength" "100"
        ] []

      div [
        yield! attributes lo hi current percent busy
        cl Css.``weave-progress-circular``
        yield! defaultArg attrs []
      ] [
        Doc.SvgElement "svg" [
          cl Css.``weave-progress-circular__svg``
          Attr.Create "viewBox" "0 0 48 48"
          Attr.Create "aria-hidden" "true"
          Attr.Create "focusable" "false"
        ] [
          circle Css.``weave-progress-circular__track``
          circle Css.``weave-progress-circular__fill``
        ]
        match content with
        | Some child -> div [ cl Css.``weave-progress-circular__content`` ] [ child ]
        | None -> ()
      ]
