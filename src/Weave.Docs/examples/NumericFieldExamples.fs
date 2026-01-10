namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave
open Weave.CssHelpers

[<JavaScript>]
module NumericFieldExamples =

  let private basicExample () =
    let value = Var.Create(CheckedInput.Make 0)

    NumericField.Create(
      value,
      labelText = View.Const "Basic Numeric Field",
      helperText = View.Const "Enter a number",
      min = View.Const 0,
      max = View.Const 100,
      step = View.Const 1
    )
    |> Helpers.section "Basic Usage" (Helpers.bodyText "A simple numeric field with min, max, and step.")

  let private disabledExample () =
    let value = Var.Create(CheckedInput.Make 42)

    NumericField.Create(
      value,
      labelText = View.Const "Disabled Field",
      enabled = View.Const false,
      helperText = View.Const "This field is disabled"
    )
    |> Helpers.section
      "Disabled State"
      (Helpers.bodyText "Numeric fields can be disabled using the enabled parameter.")

  let private errorExample () =
    let value = Var.Create(CheckedInput.Make 10)

    let errorView =
      value.View
      |> View.Map(fun v ->
        match v with
        | Valid(v, _) when v < 5 -> true
        | _ -> false)

    NumericField.Create(
      value,
      labelText = View.Const "Error State",
      errorText = View.Const "Value must be at least 5",
      error = (fun _ -> errorView)
    )
    |> Helpers.section "Error State" (Helpers.bodyText "Show error text when the value is invalid.")

  let private floatExample () =
    let value = Var.Create(CheckedInput.Make 3.14)

    NumericField.Create(
      value,
      labelText = View.Const "Float Field",
      helperText = View.Const "Supports floating point values",
      min = View.Const 0.0,
      max = View.Const 10.0,
      step = View.Const 0.1
    )
    |> Helpers.section "Float Support" (Helpers.bodyText "NumericField supports both int and float types.")

  let render () =
    Container.Create(
      div [] [
        H1.Div("NumericField Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "NumericField allows users to input and adjust numeric values with optional constraints and error handling.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Body1.Div(
          "⚠️ Note: I am working on other components right now, so this component needs more polishing than others.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        errorExample ()
        Helpers.divider ()
        floatExample ()
      ]
    )
