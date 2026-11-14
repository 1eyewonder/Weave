namespace Weave

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript

open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
type TimePicker =
  static member private createCore
    (
      value: Var<TimeSpan option>,
      suppliedField: FormField<TimeSpan option> option,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helpText: Doc,
      ?isOpen: Var<bool>,
      ?mode: View<PickerMode>,
      ?clockFormat: ClockFormat,
      ?minuteStep: int,
      ?availability: View<TimeAvailability>,
      ?formatTime: TimeSpan -> string,
      ?attrs: Attr list
    ) =
    let step = defaultArg minuteStep 1

    if step < 1 || step > 60 then
      invalidArg "minuteStep" "Minute step must be between 1 and 60."

    let clockFormat = defaultArg clockFormat ClockFormat.TwelveHour

    let field = suppliedField |> Option.defaultWith (fun () -> FormField.bind value)
    let entries = TimePickerClock.createEntries clockFormat step value
    let availability = defaultArg availability (View.Const TimeAvailability.all)

    let valueErrors =
      (availability, value.View)
      ||> View.Map2(fun availability value ->
        let available =
          Validate.optional (
            Validate.custom (TimeAvailability.allows availability) "Selected time is unavailable."
          )

        value |> (TimePickerClock.timeRules step |> Validate.andThen available))

    let draftErrors = TimePickerClock.entryErrors field.ResetVersion entries
    let errors = (valueErrors, draftErrors) ||> View.Map2 (@)

    let mode = defaultArg mode (View.Const PickerMode.Editable)
    let active = PickerRender.canInteract mode
    let opened = defaultArg isOpen (Var.Create false)
    let phase = Var.Create TimePickerClock.Hours
    let formatTime = defaultArg formatTime (TimePickerClock.format clockFormat)

    let display =
      value.View |> View.Map(Option.map formatTime >> Option.defaultValue "")

    let visibleValueErrors =
      (field.IsTouched, valueErrors)
      ||> View.Map2(fun touched errors -> if touched then errors else [])

    let popupErrors = (field.VisibleRuleErrors, visibleValueErrors) ||> View.Map2 (@)

    let outerErrors =
      (opened.View, field.Validation.VisibleErrors)
      ||> View.Map2(fun open_ errors -> if open_ then [] else errors)

    let resetVersion = Var.Create field.ResetVersion.Value

    let content () =
      TimePickerClock.render {
        Value = value
        Phase = phase
        Format = clockFormat
        MinuteStep = step
        Availability = availability
        Active = active
        Entries = entries
        ValueErrors = popupErrors
        ResetVersion = field.ResetVersion
        Touch = field.Touch
      }

    Doc.Concat [
      content
      |> PickerRender.shell {
        Name = "Choose time"
        Icon = PickerRender.icon "M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0ZM12 7v5l3 2"
        Display = display
        HasValue = value.View |> View.Map Option.isSome
        Clear =
          fun () ->
            Var.Set value None
            Var.Update field.ResetVersion ((+) 1)
        Opened = opened
        Mode = mode
        Label = defaultArg labelText (View.Const "")
        Placeholder = defaultArg placeholder (View.Const "")
        Help = helpText
        Validation = Some field.Validation
        ValidationMessages = Some outerErrors
        Attrs = PickerRender.contributeErrors field errors :: defaultArg attrs []
      }

      value.View
      |> Doc.sinkCached (fun current -> TimePickerClock.synchronizeEntries current entries)
      field.ResetVersion.View
      |> Doc.sinkCached (fun version ->
        if resetVersion.Value <> version then
          Var.Set resetVersion version
          TimePickerClock.resetEntries value.Value entries)
      opened.View |> Doc.sinkCached (fun _ -> Var.Set phase TimePickerClock.Hours)
    ]

  /// Select a time using the dial or typed hour/minute fields. Typed values apply on Enter or blur; minutes must match minuteStep.
  static member create
    (
      value: Var<TimeSpan option>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helpText: Doc,
      ?isOpen: Var<bool>,
      ?mode: View<PickerMode>,
      ?clockFormat: ClockFormat,
      ?minuteStep: int,
      ?availability: View<TimeAvailability>,
      ?formatTime: TimeSpan -> string,
      ?attrs: Attr list
    ) =
    TimePicker.createCore (
      value,
      None,
      ?labelText = labelText,
      ?placeholder = placeholder,
      ?helpText = helpText,
      ?isOpen = isOpen,
      ?mode = mode,
      ?clockFormat = clockFormat,
      ?minuteStep = minuteStep,
      ?availability = availability,
      ?formatTime = formatTime,
      ?attrs = attrs
    )

  /// Compose availability and application rules with clock/step validation. Unavailable entered values remain selected and block form submission.
  static member create
    (
      field: FormField<TimeSpan option>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helpText: Doc,
      ?isOpen: Var<bool>,
      ?mode: View<PickerMode>,
      ?clockFormat: ClockFormat,
      ?minuteStep: int,
      ?availability: View<TimeAvailability>,
      ?formatTime: TimeSpan -> string,
      ?attrs: Attr list
    ) =
    TimePicker.createCore (
      field.Value,
      Some field,
      ?labelText = labelText,
      ?placeholder = placeholder,
      ?helpText = helpText,
      ?isOpen = isOpen,
      ?mode = mode,
      ?clockFormat = clockFormat,
      ?minuteStep = minuteStep,
      ?availability = availability,
      ?formatTime = formatTime,
      ?attrs = attrs
    )
