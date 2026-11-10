namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript; RequireQualifiedAccess>]
module Form =
  let model = FormBuilder()

[<JavaScript; RequireQualifiedAccess>]
module ValidationInput =
  /// Apply to the actual input of a custom control. Described-by IDs are space separated.
  let attrs (validation: FieldValidation) = [
    Attr.Dynamic
      "aria-invalid"
      (validation.VisibleErrors
       |> View.Map(fun errors -> if List.isEmpty errors then "false" else "true"))
    Attr.Dynamic
      "aria-describedby"
      (validation.VisibleErrors
       |> View.Map(fun errors -> if List.isEmpty errors then "" else validation.ErrorId))
    on.blur (fun _ _ -> validation.Touch())
  ]

[<JavaScript; RequireQualifiedAccess>]
type Form =
  /// A native form: Enter submits, invalid data is blocked, reset restores initial values.
  /// Validation is synchronous; onSubmit owns any asynchronous work and server errors.
  static member create(model: FormModel<'T>, onSubmit: 'T -> unit, content: Doc list, ?attrs: Attr list) =
    Doc.Element
      "form"
      [
        Attr.Create "novalidate" "novalidate"
        on.submit (fun _ event ->
          event.PreventDefault()
          model.Submit onSubmit)
        on.reset (fun _ event ->
          event.PreventDefault()
          model.Reset())
        yield! defaultArg attrs []
      ]
      content
