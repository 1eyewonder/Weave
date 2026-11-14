namespace Weave.Docs.Examples

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module TimePickerExamples =

  let private bookingExample () =
    let time = Var.Create<TimeSpan option> None

    let selected =
      time.View
      |> View.Map(
        Option.map PickerExampleHelpers.timeText
        >> Option.defaultValue "Choose your session time"
      )

    Helpers.codeSampleSection
      "Schedule a session"
      (Helpers.bodyText
        "Type in the popup's hour and minute fields, or use the dial. Each field accepts at most two digits (0-9). Enter applies an hour and moves to minutes; Enter in minutes applies the time and keeps the popup open. Tab applies the current field. Empty, invalid, or off-step drafts leave the stored time unchanged. Dial selections update immediately and keep the popup open for review. Done and Escape close it, and Clear removes the value.")
      (PickerExampleHelpers.columns
        (TimePicker.create (
          time,
          labelText = View.Const "Design consultation",
          placeholder = View.Const "Choose a time",
          helpText = FieldHelpText.create (text "Times are local to your appointment."),
          attrs = [ Picker.Variant.outlined; Picker.Width.full; Picker.Controls.clearable ]
        ))
        (PickerExampleHelpers.summary
          "Your consultation"
          selected
          "A TimeSpan stores the time of day. Add a date and time zone in your application when booking a specific instant."))
      """open System

let time = Var.Create<TimeSpan option> None

let selected =
  time.View
  |> View.Map(Option.map (fun t -> sprintf "%02d:%02d" t.Hours t.Minutes))

TimePicker.create (
  time,
  labelText = View.Const "Design consultation",
  placeholder = View.Const "Choose a time",
  helpText =
    FieldHelpText.create (text "Times are local to your appointment."),
  attrs =
    [ Picker.Variant.outlined; Picker.Width.full; Picker.Controls.clearable ]
)"""

  let private clockFormatExample () =
    let time = Var.Create(Some(TimeSpan(13, 30, 0)))

    let storedValue =
      time.View
      |> View.Map(
        Option.map PickerExampleHelpers.timeText
        >> Option.defaultValue "No time selected"
      )

    Helpers.codeSampleSection
      "Compare 12-hour and 24-hour clocks"
      (Helpers.bodyText
        "Open each clock to compare the dials. TwelveHour is the default and adds AM/PM controls; TwentyFourHour uses two hour rings. Both fields share the same TimeSpan, so changing one updates the other. The clock format changes presentation, independently of calendar culture.")
      (PickerExampleHelpers.columns
        (PickerExampleHelpers.previews [
          TimePicker.create (
            time,
            labelText = View.Const "12-hour clock",
            clockFormat = ClockFormat.TwelveHour,
            attrs = [ Picker.Variant.outlined; Picker.Width.full ]
          )
          TimePicker.create (
            time,
            labelText = View.Const "24-hour clock",
            clockFormat = ClockFormat.TwentyFourHour,
            attrs = [ Picker.Variant.outlined; Picker.Width.full ]
          )
        ])
        (PickerExampleHelpers.summary
          "Shared time of day"
          storedValue
          "13:30 and 1:30 PM represent the same value. No date or time-zone conversion is involved."))
      """open System

let time = Var.Create(Some(TimeSpan(13, 30, 0)))

div
  []
  [
    TimePicker.create (
      time,
      labelText = View.Const "12-hour clock",
      clockFormat = ClockFormat.TwelveHour
    )
    TimePicker.create (
      time,
      labelText = View.Const "24-hour clock",
      clockFormat = ClockFormat.TwentyFourHour
    )
  ]"""

  let private minuteStepExample () =
    let time = Var.Create(Some(TimeSpan(9, 0, 0)))
    let step = Var.Create(Some 15)

    let items =
      PickerAppearance.minuteSteps
      |> List.map (fun step ->
        let label =
          if step = 1 then
            "Every Minute"
          else
            sprintf "Every %d Minutes" step

        SelectItem.create (text label, label, step))
      |> View.Const

    let preview =
      step.View
      |> Doc.BindView(fun step ->
        TimePicker.create (
          time,
          labelText = View.Const "Appointment time",
          minuteStep = defaultArg step 15,
          attrs = [
            Picker.Variant.outlined
            Picker.Label.above
            Picker.Color.primary
            Picker.Width.full
          ]
        ))

    Helpers.codeSampleSection
      "Offer appointment slots"
      (Helpers.bodyText
        "Change Minute Step, then open the picker to compare the available minutes. The step applies to the dial and typed input. Off-step values are retained and shown as invalid rather than rounded. Quarter-hour slots allow 00, 15, 30, and 45.")
      (PickerExampleHelpers.columns
        preview
        (Select.create (
          items,
          step,
          labelText = View.Const "Minute Step",
          attrs = [
            Select.Variant.outlined
            Select.Label.above
            Select.Width.full
            Select.Color.primary
          ]
        )))
      """open System

let time = Var.Create(Some(TimeSpan(9, 0, 0)))

let step = Var.Create(Some 15)

let items =
  [ 1; 5; 10; 15; 20; 30 ]
  |> List.map (fun step ->
    let label =
      if step = 1 then "Every Minute" else sprintf "Every %d Minutes" step
    SelectItem.create (text label, label, step))
  |> View.Const

let picker =
  step.View
  |> Doc.BindView (fun step ->
    TimePicker.create (
      time,
      labelText = View.Const "Appointment time",
      minuteStep = defaultArg step 15,
      attrs = [ Picker.Variant.outlined; Picker.Label.above; Picker.Color.primary ]
    ))

Doc.Concat [
  picker
  Select.create (
    items,
    step,
    labelText = View.Const "Minute Step",
    attrs = [ Select.Variant.outlined; Select.Label.above ]
  )
]"""

  let private modeExample () =
    let time = Var.Create(Some(TimeSpan(17, 45, 0)))
    let mode = Var.Create PickerMode.Editable

    let formatTime (time: TimeSpan) =
      sprintf "%02dh %02dm" time.Hours time.Minutes

    Helpers.codeSampleSection
      "Format and review a schedule"
      (Helpers.bodyText
        "formatTime changes the field text while clockFormat controls the dial. A reactive PickerMode handles read-only and disabled states. Use the preset buttons to see that external updates still appear in either mode.")
      (PickerExampleHelpers.columns
        (TimePicker.create (
          time,
          labelText = View.Const "End of day",
          mode = mode.View,
          formatTime = formatTime,
          attrs = [
            Picker.Variant.outlined
            Picker.Label.above
            Picker.Color.secondary
            Picker.Controls.clearable
          ]
        ))
        (PickerExampleHelpers.stack [
          PickerExampleHelpers.modeControl mode
          PickerExampleHelpers.actions [
            Button.primary (
              text "Set 09:00",
              onClick = (fun () -> Var.Set time (Some(TimeSpan(9, 0, 0)))),
              attrs = [ Button.Variant.outlined ]
            )
            Button.primary (
              text "Set 17:45",
              onClick = (fun () -> Var.Set time (Some(TimeSpan(17, 45, 0)))),
              attrs = [ Button.Variant.outlined ]
            )
          ]
        ]))
      """open System

let time = Var.Create(Some(TimeSpan(17, 45, 0)))
let mode = Var.Create PickerMode.Editable

let formatTime (time: TimeSpan) =
  sprintf "%02dh %02dm" time.Hours time.Minutes

let picker =
  TimePicker.create (
    time,
    labelText = View.Const "End of day",
    mode = mode.View,
    formatTime = formatTime,
    attrs = [ Picker.Variant.filled; Picker.Color.secondary ]
  )

Var.Set mode PickerMode.ReadOnly
Var.Set time (Some(TimeSpan(9, 0, 0)))
picker"""

  let private validationExample () =
    let openingHour = Var.Create 9

    let availability =
      openingHour.View
      |> View.Map(fun hour ->
        Interval.create
          (LowerBounds.Inclusive(TimeSpan(hour, 0, 0)))
          (UpperBounds.Exclusive(TimeSpan(17, 0, 0)))
        |> Option.toList
        |> TimeAvailability.ofIntervals
        |> TimeAvailability.excluding [ Interval.singleton (TimeSpan(12, 30, 0)) ])

    let openingText =
      openingHour.View
      |> View.Map(fun hour -> sprintf "Opens at %02d:00; closes at 17:00. 12:30 is reserved." hour)

    let time =
      FormField.create ((None: TimeSpan option), rules = [ Validate.isSome "Choose a reminder time." ])

    let reminder = Form.model { let! selected = time in return selected }
    let result = Var.Create "Choose a time, then save your reminder."

    let confirm selected =
      selected
      |> Option.iter (fun selected ->
        Var.Set result ("Reminder set for " + PickerExampleHelpers.timeText selected))

    Helpers.codeSampleSection
      "Validate a reminder"
      (Helpers.bodyText
        "TimeAvailability limits the dial to working hours and excludes the 12:30 reservation. Change the opening time below and watch the help text update. An entered time stays selected if the new rule makes it unavailable; validation blocks saving until it is corrected. FormField combines this policy with required, clock, and minute-step rules.")
      (Form.create (
        reminder,
        confirm,
        [
          TimePicker.create (
            time,
            labelText = View.Const "Daily reminder",
            availability = availability,
            clockFormat = ClockFormat.TwentyFourHour,
            minuteStep = 5,
            helpText = FieldHelpText.create (textView openingText),
            attrs = [ Picker.Variant.outlined; Picker.Controls.clearable ]
          )
          PickerExampleHelpers.actions [
            Button.primary (
              text "Open at 10:00",
              (fun () -> Var.Set openingHour 10),
              enabled = (openingHour.View |> View.Map((<>) 10)),
              attrs = [ Button.Variant.outlined ]
            )
            Button.primary (
              text "Restore 09:00 opening",
              (fun () -> Var.Set openingHour 9),
              enabled = (openingHour.View |> View.Map((<>) 9)),
              attrs = [ Button.Variant.outlined ]
            )
          ]
          PickerExampleHelpers.submitButton "Save reminder"
          div [ Typography.body2; Attr.Create "role" "status" ] [ textView result.View ]
        ],
        attrs = PickerExampleHelpers.formAttrs
      ))
      """open System

let openingHour = Var.Create 9

let availability =
  openingHour.View
  |> View.Map (fun hour ->
    Interval.create
      (LowerBounds.Inclusive (TimeSpan(hour, 0, 0)))
      (UpperBounds.Exclusive (TimeSpan(17, 0, 0)))
    |> Option.toList
    |> TimeAvailability.ofIntervals
    |> TimeAvailability.excluding [ Interval.singleton (TimeSpan(12, 30, 0)) ])

let openingText =
  openingHour.View
  |> View.Map (fun hour ->
    sprintf "Opens at %02d:00; closes at 17:00. 12:30 is reserved." hour)

let time =
  FormField.create (
    (None: TimeSpan option),
    rules = [ Validate.isSome "Choose a reminder time." ]
  )

let reminder = Form.model { let! selected = time in return selected }
let result = Var.Create ""

let saveReminder (selected: TimeSpan option) =
  selected
  |> Option.iter (fun t ->
    Var.Set result (sprintf "Reminder set for %02d:%02d" t.Hours t.Minutes))

Form.create (
  reminder,
  saveReminder,
  [
    TimePicker.create (
      time,
      labelText = View.Const "Daily reminder",
      availability = availability,
      clockFormat = ClockFormat.TwentyFourHour,
      minuteStep = 5,
      helpText = FieldHelpText.create (textView openingText)
    )
    Button.primary (
      text "Open at 10:00",
      (fun () -> Var.Set openingHour 10),
      enabled = (openingHour.View |> View.Map ((<>) 10)),
      attrs = [ Button.Variant.outlined ]
    )
    Button.primary (
      text "Restore 09:00 opening",
      (fun () -> Var.Set openingHour 9),
      enabled = (openingHour.View |> View.Map ((<>) 9)),
      attrs = [ Button.Variant.outlined ]
    )
    button [ attr.``type`` "submit" ] [ text "Save reminder" ]
    div [] [ textView result.View ]
  ],
  attrs =
    [
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      AlignItems.start
      Gap.All.g4
    ]
)"""

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Time Picker"
        p [ Typography.body1; Margin.Bottom.small; Attr.Class "docs-picker-page__intro" ] [
          text
            "Choose a time of day with an analog clock. Compare clock formats, control minute precision, and build a validated reminder form."
        ]
        PickerExampleHelpers.guideLink ()
        bookingExample ()
        PickerConfigurator.render PickerAppearance.Kind.Time
        clockFormatExample ()
        minuteStepExample ()
        modeExample ()
        validationExample ()

        Helpers.apiSection
          (text "Both create overloads return a Doc and accept the same optional parameters.")
          [
            Helpers.apiTable "TimePicker.create" [
              Helpers.apiParam
                "value"
                "Var<TimeSpan option> or FormField<TimeSpan option>"
                ""
                "Clock bounds, minuteStep, and availability are always validated. Pass the FormField itself, rather than field.Value, to include picker constraints in form submission. Its rules add application validation without discarding entered times."
              Helpers.apiParam "?labelText" "View<string>" "View.Const \"\"" "Field label."
              Helpers.apiParam "?placeholder" "View<string>" "View.Const \"\"" "Empty-selection hint."
              Helpers.apiParam "?helpText" "Doc" "None" "Help content, shown automatically when supplied."
              Helpers.apiParam "?isOpen" "Var<bool>" "Internal false" "Two-way popup state."
              Helpers.apiParam
                "?mode"
                "View<PickerMode>"
                "View.Const PickerMode.Editable"
                "Editable, ReadOnly, or Disabled."
              Helpers.apiParam
                "?attrs"
                "Attr list"
                "[]"
                "Root attributes, including Picker styles and Controls.clearable."
              Helpers.apiParam
                "?clockFormat"
                "ClockFormat"
                "ClockFormat.TwelveHour"
                "TwelveHour or TwentyFourHour; controls the dial and default display."
              Helpers.apiParam
                "?minuteStep"
                "int"
                "1"
                "Selectable minute interval, from 1 to 60, starting at zero. Printed minute labels are always selectable."
              Helpers.apiParam
                "?availability"
                "View<TimeAvailability>"
                "View.Const TimeAvailability.all"
                "Disable unavailable dial slots and revalidate stored times when the rule changes. An hour is enabled if any stepped minute is available. Hour selection preserves the current minute when allowed, otherwise chooses the first available stepped minute. AM/PM also chooses an eligible hour if the matching hour is unavailable. Typed/external unavailable values remain entered and block FormField submission."
              Helpers.apiParam
                "?formatTime"
                "TimeSpan -> string"
                "HH:mm or h:mm tt"
                "Custom selected-time display."
            ]
            Helpers.styleModuleTable "Picker styles" [
              "Variant.standard / filled / outlined", "Field appearance"
              "Color.primary / secondary / tertiary / error / warning / success / info", "Theme palette"
              "Label.above", "Persistent outlined label"
              "Width.full", "Fill available width"
              "Controls.clearable", "Show Clear for a selected value"
              "Controls.clearableWhen (View<bool>)", "Reactively control the same class through an Attr"
            ]
          ]
      ],
      attrs = [ Container.MaxWidth.large; Attr.Class "docs-picker-page" ]
    )
