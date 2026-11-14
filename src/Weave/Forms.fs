namespace Weave

open WebSharper
open WebSharper.UI

/// A synchronous rule returns all of its error messages, or an empty list.
[<JavaScript>]
type Validator<'T> = 'T -> string list

[<JavaScript; RequireQualifiedAccess>]
module Validate =

  let custom predicate message : Validator<'T> =
    fun value -> if predicate value then [] else [ message ]

  /// Validate a derived value, retaining the original field value on success.
  let on project (rule: Validator<'U>) : Validator<'T> = fun value -> rule (project value)

  // WebSharper's generic comparer gives NaN an ordering. Reject it explicitly
  // so numeric bounds have the same meaning on .NET and in JavaScript.
  let private isOrdered value =
    match box value with
    | :? float as number -> not (System.Double.IsNaN number)
    | :? float32 as number -> not (System.Single.IsNaN number)
    | _ -> true

  let atLeast minimum message : Validator<'T> =
    custom (fun value -> isOrdered value && isOrdered minimum && value >= minimum) message

  let atMost maximum message : Validator<'T> =
    custom (fun value -> isOrdered value && isOrdered maximum && value <= maximum) message

  let greaterThan minimum message : Validator<'T> =
    custom (fun value -> isOrdered value && isOrdered minimum && value > minimum) message

  let lessThan maximum message : Validator<'T> =
    custom (fun value -> isOrdered value && isOrdered maximum && value < maximum) message

  /// Inclusive bounds; inverted or NaN bounds are configuration errors.
  let between minimum maximum message : Validator<'T> =
    if not (isOrdered minimum && isOrdered maximum) || minimum > maximum then
      invalidArg
        "maximum"
        "Bounds must not be NaN, and the maximum must be greater than or equal to the minimum."

    custom (fun value -> isOrdered value && value >= minimum && value <= maximum) message

  let equalTo expected message : Validator<'T> = custom ((=) expected) message

  let notEqualTo unexpected message : Validator<'T> = custom ((<>) unexpected) message

  /// Supply the measurement explicitly, e.g. List.length, Array.length, or Set.count.
  let minLengthBy (length: 'T -> int) minimum message : Validator<'T> = on length (atLeast minimum message)

  let maxLengthBy (length: 'T -> int) maximum message : Validator<'T> = on length (atMost maximum message)

  /// Run every rule, preserving declaration order.
  let all (rules: Validator<'T> list) : Validator<'T> =
    fun value -> rules |> List.collect (fun rule -> rule value)

  /// Run the next rule only after the first succeeds.
  let andThen (next: Validator<'T>) (first: Validator<'T>) : Validator<'T> =
    fun value ->
      match first value with
      | [] -> next value
      | errors -> errors

  let required message : Validator<string> =
    custom (System.String.IsNullOrWhiteSpace >> not) message

  let minLength length message : Validator<string> =
    custom (fun value -> not (isNull value) && value.Length >= length) message

  let maxLength length message : Validator<string> =
    custom (fun value -> isNull value || value.Length <= length) message

  let isSome message : Validator<'T option> = custom Option.isSome message

  let isTrue message : Validator<bool> = custom id message

  /// Skip the rule for None; use isSome to require a selection.
  let optional (rule: Validator<'T>) : Validator<'T option> =
    function
    | Some value -> rule value
    | None -> []

[<JavaScript>]
type FormError = {
  FieldId: string
  Message: string
}

/// Shared presentation state for any input, including custom controls.
[<JavaScript>]
type FieldValidation = {
  ErrorId: string
  VisibleErrors: View<string list>
  Touch: unit -> unit
}

[<JavaScript>]
type FormModel<'T>
  internal (result: View<Result<'T, FormError list>>, touch: unit -> unit, reset: unit -> unit) =
  member _.Result = result

  member _.IsValid =
    result
    |> View.Map(function
      | Ok _ -> true
      | Error _ -> false)

  member _.Touch() = touch ()
  member _.Reset() = reset ()

  /// Reveal all errors and sample current values once. Calls onValid only on success.
  member _.Submit(onValid: 'T -> unit) =
    touch ()

    result
    |> View.Get(function
      | Ok value -> onValid value
      | Error _ -> ())

[<JavaScript>]
type FormField<'T when 'T: equality> internal (value: Var<'T>, rules: View<Validator<'T> list>) =
  let initialValue = value.Value
  let touched = Var.Create false
  let resetVersion = Var.Create 0
  let id = WeaveId.create "weave-validation"

  let ruleErrors =
    View.Map2 (fun current validators -> Validate.all validators current) value.View rules

  let contributions = Var.Create(Map.empty<string, View<string list>>)

  let contributedErrors =
    contributions.View
    |> View.Bind(fun sources ->
      sources
      |> Map.toList
      |> List.fold (fun combined (_, messages) -> (combined, messages) ||> View.Map2 (@)) (View.Const []))

  let errors = (ruleErrors, contributedErrors) ||> View.Map2 (@)

  let visibleErrors =
    View.Map2 (fun show messages -> if show then messages else []) touched.View errors

  let touch () = touched.Value <- true

  let reset () =
    value.Value <- initialValue
    touched.Value <- false
    Var.Update resetVersion ((+) 1)

  let result =
    View.Map2
      (fun current messages ->
        if List.isEmpty messages then
          Ok current
        else
          Error(
            messages
            |> List.map (fun message -> {
              FieldId = id
              Message = message
            })
          ))
      value.View
      errors

  // Custom controls contribute parser/constraint errors to the same form model.
  // The control owns registration and removes its contribution when unmounted.
  member internal _.ContributeErrors(owner, messages) =
    Var.Update contributions (Map.add owner messages)

  member internal _.RemoveErrors(owner) =
    Var.Update contributions (Map.remove owner)

  member internal _.VisibleRuleErrors =
    (touched.View, ruleErrors)
    ||> View.Map2(fun show messages -> if show then messages else [])

  member internal _.ResetVersion = resetVersion
  member _.Value = value
  member _.Errors = errors
  member _.IsTouched = touched.View
  member _.IsDirty = value.View |> View.Map(fun current -> current <> initialValue)

  member _.Validation = {
    ErrorId = id
    VisibleErrors = visibleErrors
    Touch = touch
  }

  member _.Model = FormModel(result, touch, reset)
  member _.Touch() = touch ()
  member _.Reset() = reset ()

[<JavaScript; RequireQualifiedAccess>]
type FormField =
  /// rulesView replaces rules when supplied. Use it for reactive cross-field rules.
  static member create(initialValue: 'T, ?rules: Validator<'T> list, ?rulesView: View<Validator<'T> list>) =
    FormField<'T>(Var.Create initialValue, defaultArg rulesView (View.Const(defaultArg rules [])))

  static member internal bind(value: Var<'T>) = FormField<'T>(value, View.Const [])

[<JavaScript; RequireQualifiedAccess>]
module FormModel =
  let map f (model: FormModel<'T>) =
    FormModel(model.Result |> View.Map(Result.map f), model.Touch, model.Reset)

  let zip (left: FormModel<'T>) (right: FormModel<'U>) =
    let result =
      View.Map2
        (fun a b ->
          match a, b with
          | Ok x, Ok y -> Ok(x, y)
          | Error x, Error y -> Error(x @ y)
          | Error errors, _
          | _, Error errors -> Error errors)
        left.Result
        right.Result

    FormModel(
      result,
      (fun () ->
        left.Touch()
        right.Touch()),
      (fun () ->
        left.Reset()
        right.Reset())
    )

/// Applicative form composition: independent fields accumulate every error.
[<JavaScript>]
type FormBuilder() =
  member _.Source(field: FormField<'T>) = field.Model
  member _.Source(model: FormModel<'T>) = model
  member _.MergeSources(left, right) = FormModel.zip left right
  member _.BindReturn(model, f) = FormModel.map f model

  member _.Return(value) =
    FormModel(View.Const(Ok value), ignore, ignore)
