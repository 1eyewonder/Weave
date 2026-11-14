namespace Weave.Docs.Examples

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module DateTimeGuide =

  [<RequireQualifiedAccess; Struct>]
  type private DateRule =
    | Window
    | Handover
    | Event
    | Unrestricted

  let private month = DateTime.Today.AddMonths 1
  let private date day = DateTime(month.Year, month.Month, day)
  let private fieldAttrs color = [ color; Picker.Variant.outlined; Picker.Controls.clearable ]

  let private result heading message =
    div [ Attr.Class "docs-date-guide__result"; Attr.Create "role" "status" ] [
      h3 [ Typography.h6; Typography.Color.textPrimary ] [ text heading ]
      p [ Typography.body1; Typography.Color.textSecondary ] [ text message ]
    ]

  let private lesson title description instruction preview source takeaway =
    let content =
      div [ Attr.Class "docs-date-guide__experiment" ] [
        div [ Attr.Class "docs-date-guide__instructions" ] [
          h3 [ Typography.subtitle2; Typography.Color.textPrimary ] [ text "Try the Example" ]
          p [ Typography.body1; Typography.Color.textSecondary ] [ text instruction ]
        ]
        preview
        div [ Attr.Class "docs-date-guide__takeaway" ] [
          h3 [ Typography.subtitle2; Typography.Color.textPrimary ] [ text "How It Works" ]
          p [ Typography.body1; Typography.Color.textSecondary ] [ text takeaway ]
        ]
      ]

    div [ Attr.Class "docs-date-guide__lesson" ] [
      Helpers.codeSampleSectionView title (Helpers.bodyText description) content source
    ]

  let private pickerAndControls pickers controls =
    div [ Attr.Class "docs-appearance" ] [
      div [ Attr.Class "docs-appearance__preview" ] [
        div [ Attr.Class "docs-date-guide__experiment docs-date-guide__pickers" ] pickers
      ]
      div [ Attr.Class "docs-appearance__settings" ] controls
    ]

  let private ordinaryDate () =
    let selected = Var.Create<DateTime option> None

    lesson
      "Choosing a Date"
      "DatePicker works with your existing DateTime binding. Use it for an appointment date when there is no relationship to validate against another field."
      "Choose an appointment date. The component handles calendar navigation and date selection; your application keeps its usual WebSharper binding."
      (div [ Attr.Class "docs-date-guide__experiment" ] [
        DatePicker.create (
          selected,
          labelText = View.Const "Appointment date",
          attrs = fieldAttrs Picker.Color.primary
        )
      ])
      (View.Const(DateTimeGuideSource.binding ()))
      "No Range or Interval is needed for a single selection. DatePicker edits the calendar date; combine it with a time and time zone in application code when you need a specific instant."

  let private relatedDates () =
    let first = Var.Create(Some(date 1))
    let last = Var.Create(Some(date 7))

    let inspect first last =
      match first, last with
      | None, _ -> result "Choose a start date" "Both endpoints are required to validate the period."
      | _, None -> result "Choose an end date" "Both endpoints are required to validate the period."
      | Some first, Some last ->
        if first > last then
          result
            "The end is before the start"
            "Each field accepts its own date. The application must reject this pair before creating a booking."
        elif first = last then
          result "A one-day period" "The comparison accepts equal endpoints, allowing a same-day booking."
        else
          result
            "The dates are in order"
            "The comparison passes, but callers still receive two independent dates."

    let setDates start finish () =
      Var.Set first (Some(date start))
      Var.Set last (Some(date finish))

    let feedback = (first.View, last.View) ||> View.Map2 inspect |> Doc.EmbedView
    let source = (first.View, last.View) ||> View.Map2 DateTimeGuideSource.pair

    lesson
      "Choosing a Date Range"
      "A date range starts with two dates, but two valid DateTime values do not guarantee a valid period. These independent fields need a comparison to check that the end is on or after the start."
      "Put the end before the start. Both fields still accept their dates, so the application reports the problem. Restore the dates or try a one-day period."
      (pickerAndControls [
        DatePicker.create (
          first,
          labelText = View.Const "Start date",
          attrs = fieldAttrs Picker.Color.secondary
        )
        DatePicker.create (last, labelText = View.Const "End date", attrs = fieldAttrs Picker.Color.secondary)
      ] [
        (PickerExampleHelpers.actions [
          Button.create (
            text "Restore valid dates",
            setDates 1 7,
            attrs = [ Button.Color.secondary; Button.Variant.outlined ]
          )
          Button.create (
            text "Put end before start",
            setDates 7 1,
            attrs = [ Button.Color.secondary; Button.Variant.outlined ]
          )
          Button.create (
            text "Use the same date",
            setDates 4 4,
            attrs = [ Button.Color.secondary; Button.Variant.outlined ]
          )
        ])
        feedback
      ])
      source
      "This is the usual WebSharper approach: combine the bindings and validate the pair. That validation protects this form, but passing the dates elsewhere does not carry their relationship with them."

  let private rangePicker () =
    let initialRange = Range.closed (date 1) (date 7)
    let selected = Var.Create initialRange

    lesson
      "Introducing Weave's Range Type"
      "The previous example validates two independent fields whenever it needs a period. Range.closed checks that relationship at construction, then packages the endpoints in one immutable value. Pass that value through your scheduling code without repeating the ordering check."
      "This example starts with a week selected. Choose two dates in either order, or clear the field and close the calendar after just one click."
      (div [ Attr.Class "docs-date-guide__experiment" ] [
        DateRangePicker.create (
          selected,
          labelText = View.Const "Booking period",
          attrs = fieldAttrs Picker.Color.secondary
        )
      ])
      (View.Const(DateTimeGuideSource.rangePicker ()))
      "DateRangePicker commits the pair after the second selection and orders user clicks. Done closes the calendar. Range.closed validates externally supplied endpoints without swapping them. Use Range.tryGetClosed to read the selected dates when passing a booking to your application."

  let private ruleName =
    function
    | DateRule.Window -> "window"
    | DateRule.Handover -> "handover"
    | DateRule.Event -> "event"
    | DateRule.Unrestricted -> "unbounded"

  type private RuleExample = {
    Lower: LowerBounds<DateTime>
    Upper: UpperBounds<DateTime>
    Interval: Interval<DateTime> option
    Message: string
  }

  let private allowedDates () =
    let rule = Var.Create DateRule.Window
    let selected = Var.Create<DateTime option> None

    let describe rule =
      match rule with
      | DateRule.Window -> {
          Lower = LowerBounds.Inclusive(date 1)
          Upper = UpperBounds.Inclusive(date 7)
          Interval = Interval.closed (date 1) (date 7)
          Message = "Dates 1–7 are allowed, including both endpoints."
        }
      | DateRule.Handover -> {
          Lower = LowerBounds.Inclusive(date 1)
          Upper = UpperBounds.Exclusive(date 7)
          Interval = Interval.create (LowerBounds.Inclusive(date 1)) (UpperBounds.Exclusive(date 7))
          Message = "Date 7 is a handover date. An exclusive upper bound leaves it out."
        }
      | DateRule.Event -> {
          Lower = LowerBounds.Inclusive(date 4)
          Upper = UpperBounds.Inclusive(date 4)
          Interval = Some(Interval.Degenerate(date 4))
          Message = "Only date 4 is allowed. Degenerate represents exactly one value."
        }
      | DateRule.Unrestricted -> {
          Lower = LowerBounds.Unbounded
          Upper = UpperBounds.Unbounded
          Interval = Some Interval.Unbounded
          Message = "Every date is allowed. Unbounded removes both limits."
        }

    let policy rule =
      (describe rule).Interval |> Option.toList |> DateAvailability.ofIntervals

    let availability = rule.View |> View.Map policy

    let draw rule =
      let example = describe rule

      div [] [
        DateTimeGuideVisuals.dateLine example.Lower example.Upper example.Interval
        result "Allowed dates" example.Message
      ]

    let change value () =
      Var.Set selected None
      Var.Set rule value

    let controls =
      PickerExampleHelpers.actions [
        for value, label in
          [
            DateRule.Window, "Booking window"
            DateRule.Handover, "Before handover"
            DateRule.Event, "Event date only"
            DateRule.Unrestricted, "No date limit"
          ] do
          Button.create (
            text label,
            change value,
            attrs = [
              Button.Color.primary
              Button.Variant.outlined
              Attr.Dynamic
                "aria-pressed"
                (rule.View
                 |> View.Map(fun current -> if current = value then "true" else "false"))
            ]
          )
      ]

    lesson
      "Introducing Weave's Interval Type"
      "A handover rule often starts as a predicate: date >= start && date < handover. Interval stores those boundaries as data, so membership checks and calendar availability can share the rule. It supports a bounded window, a single event date, or no date limit."
      "These rules use next month. Compare a booking window with a handover boundary and a fixed event date, then open the picker. Changing the demo rule clears this example’s selection."
      (pickerAndControls [
        DatePicker.create (
          selected,
          labelText = View.Const "Allowed appointment date",
          availability = availability,
          attrs = fieldAttrs Picker.Color.primary
        )
      ] [
        controls
        rule.View |> Doc.BindView draw
        DateTimeGuideVisuals.endpointLegend ()
      ])
      (rule.View |> View.Map(ruleName >> DateTimeGuideSource.rule))
      "Range supports inclusive and exclusive bounds too. Interval adds named single-value and unrestricted cases, so DateAvailability can accept different scheduling policies through one API. The code compares a manual predicate with the equivalent interval membership check."

  let private windows () =
    [ Interval.closed (date 1) (date 7); Interval.closed (date 15) (date 21) ]
    |> List.choose id

  let private reservations () =
    [ Interval.closed (date 5) (date 6); Some(Interval.Degenerate(date 18)) ]
    |> List.choose id

  let private availability () =
    DateAvailability.ofIntervals (windows ())
    |> DateAvailability.excluding (reservations ())

  let private shareAvailability () =
    let appointment = Var.Create<DateTime option> None
    let selection = Var.Create<Range<DateTime> option> None
    let rule = availability ()

    lesson
      "Manage Complexity with Ease"
      "Published schedules change as reservations arrive and appointments move. With date predicates, you compose allowed windows and exclusions yourself. DateAvailability makes that policy reusable and retains the interval bounds the calendars need for navigation."
      "The studio offers the first and third weeks of next month. Days 5 and 6 are already reserved, and a rescheduled appointment occupies day 18. Try booking across a reserved day: a multi-day booking requires uninterrupted availability."
      (pickerAndControls [
        DatePicker.create (
          appointment,
          labelText = View.Const "Single appointment",
          availability = View.Const rule,
          attrs = fieldAttrs Picker.Color.primary
        )
        DateRangePicker.create (
          selection,
          labelText = View.Const "Booking dates",
          availability = View.Const rule,
          attrs = fieldAttrs Picker.Color.secondary
        )
      ] [
        DateTimeGuideVisuals.availabilityGrid month rule (reservations ())
        div [ Typography.body2; Attr.Class "docs-date-guide__legend" ] [
          text "✓ Available · × Already booked · – Not offered"
        ]

      ])
      (View.Const(DateTimeGuideSource.selection ()))
      "ofIntervals combines the offered windows; excluding subtracts existing reservations. Pass the resulting policy to both pickers: DatePicker checks one date, while DateRangePicker checks the entire stay, including dates between the endpoints. Add working-day rules with DateAvailability.filter when needed."

  let private analyseTime () =
    let hours =
      Range.create (LowerBounds.Inclusive(TimeSpan(9, 0, 0))) (UpperBounds.Exclusive(TimeSpan(17, 0, 0)))

    let availability =
      hours
      |> Option.map Range.toInterval
      |> Option.toList
      |> TimeAvailability.ofIntervals

    let selected = Var.Create(Some(TimeSpan(9, 0, 0)))

    let inspect =
      function
      | None -> result "No time selected" "Select a time to check against the working-hours range."
      | Some time ->
        let allowed = TimeAvailability.allows availability time

        let detail =
          if time < TimeSpan(9, 0, 0) then
            "Before opening. Choose 09:00 or later."
          elif time >= TimeSpan(17, 0, 0) then
            "At or after closing. Choose a time before 17:00."
          else
            "Within working hours."

        result
          (sprintf "%02d:%02d — %s" time.Hours time.Minutes (if allowed then "allowed" else "outside hours"))
          detail

    let source selected =
      let time = selected |> Option.defaultValue TimeSpan.Zero
      DateTimeGuideSource.time time.Hours time.Minutes

    lesson
      "Different Type, Same Wrapper"
      "TimePicker takes a TimeSpan representing a time of day, rather than a DateTime. Range is generic, so the same inclusive/exclusive rules can describe working hours without carrying an unrelated calendar date."
      "Use the preset buttons to assign 08:00, 09:00, and 17:00. The picker retains each assigned value, reports unavailable times, and restricts dial choices to the working window."
      (pickerAndControls [
        TimePicker.create (
          selected,
          labelText = View.Const "Appointment time",
          availability = View.Const availability,
          clockFormat = ClockFormat.TwelveHour,
          attrs = fieldAttrs Picker.Color.tertiary
        )

      ] [
        (PickerExampleHelpers.actions [
          Button.create (
            text "Before opening",
            (fun () -> Var.Set selected (Some(TimeSpan(8, 0, 0)))),
            attrs = [ Button.Color.tertiary; Button.Variant.outlined ]
          )
          Button.create (
            text "Opening time",
            (fun () -> Var.Set selected (Some(TimeSpan(9, 0, 0)))),
            attrs = [ Button.Color.tertiary; Button.Variant.outlined ]
          )
          Button.create (
            text "Closing time",
            (fun () -> Var.Set selected (Some(TimeSpan(17, 0, 0)))),
            attrs = [ Button.Color.tertiary; Button.Variant.outlined ]
          )
        ])
        selected.View |> Doc.BindView inspect
      ])
      (selected.View |> View.Map source)
      "Range.toInterval connects the working-hours range to TimeAvailability. The same policy drives the membership check and the picker. A FormField also blocks submission of unavailable times. TimeSpan carries no date or time zone; supply those separately when scheduling a specific instant."

  let private furtherReading () =
    Doc.Concat [
      Helpers.codeSection
        "fsharp"
        "Building a Range Pipeline"
        (Helpers.bodyText
          "Remove the time portions from a booking’s DateTime endpoints when the next operation needs calendar dates. This pipeline keeps the endpoint rules, converts the range to an interval, and filters requested dates.")
        (DateTimeGuideSource.calendarRange ())
      p [ Typography.body2; Typography.Color.textSecondary ] [
        text
          "Range and Interval also support DateOnly in .NET code. The client examples use DateTime.Date because the current WebSharper runtime does not translate DateOnly to JavaScript."
      ]
      Helpers.codeSection
        "fsharp"
        "Rescheduling a Booking Window"
        (Helpers.bodyText
          "When a booking window moves by a week, project both endpoints with Interval.mapBounds. The exclusive end stays exclusive. Use the shifted interval to filter appointment requests without rewriting the membership rule.")
        (DateTimeGuideSource.rescheduledInterval ())
      Helpers.codeSection
        "fsharp"
        "Transforming Existing Bounds"
        (Helpers.bodyText
          "If a range already has DateTime endpoints, Range.mapBounds can extract their TimeOfDay values. It preserves inclusive/exclusive markers and returns None if the new endpoints are reversed or coincide with either end excluded. It maps endpoints, not every value in the original range. Interval.mapBounds also preserves the interval case.")
        (DateTimeGuideSource.projection ())
      Helpers.codeSection
        "fsharp"
        "Scheduling Across Midnight"
        (Helpers.bodyText
          "A 22:00–02:00 schedule crosses midnight. Keep interval ordering explicit: combine times from 22:00 onward with times before 02:00. TimeAvailability restricts both intervals to the same 24-hour clock day, so this permits late-night and early-morning times without treating noon as available.")
        (DateTimeGuideSource.overnightTimeRule ())
      Helpers.codeSection
        "fsharp"
        "Understanding Invalid Bounds and Single Values"
        (Helpers.bodyText
          "Range.create and Interval.create return None when finite endpoints are reversed, or when equal endpoints exclude their shared value. Equal inclusive endpoints permit exactly one value. Use Interval.singleton for that single value, and Interval.Unbounded when every value should be allowed.")
        (DateTimeGuideSource.edgeCases ())
      Helpers.textSection "Putting It into Practice" [
        p [] [
          text
            "Use DatePicker for one calendar date, DateRangePicker for a checked pair, and TimePicker for a time of day. The component pages cover appearance, culture, read-only modes, and form validation."
        ]
        ul [] [
          li [] [ Link.create (text "Date Picker examples and API", href = "#date-picker") ]
          li [] [
            Link.create (text "Date Range Picker examples and API", href = "#date-range-picker")
          ]
          li [] [ Link.create (text "Time Picker examples and API", href = "#time-picker") ]
        ]
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Date & Time Guide"
        p [ Typography.body1; Margin.Bottom.large; Attr.Class "docs-picker-page__intro" ] [
          text
            "Choose dates with ordinary .NET values, then use Weave's range and interval types to keep related values valid and share selection rules between components."
        ]
        ordinaryDate ()
        relatedDates ()
        rangePicker ()
        allowedDates ()
        shareAvailability ()
        analyseTime ()
        furtherReading ()
        Helpers.apiSection
          (text
            "Shared date and time types. Range and Interval require comparable values; the pickers use DateTime for calendar dates and TimeSpan for times of day.")
          [
            PickerExampleHelpers.boundsApi ()
            PickerExampleHelpers.rangeApi ()
            PickerExampleHelpers.intervalApi ()
            PickerExampleHelpers.availabilityApi ()
            PickerExampleHelpers.timeAvailabilityApi ()
            Helpers.apiTable "Picker modes and clock formats" [
              Helpers.apiParam
                "PickerMode.Editable"
                "PickerMode"
                "Default"
                "Allow selection and typed time editing."
              Helpers.apiParam
                "PickerMode.ReadOnly"
                "PickerMode"
                ""
                "Display the value without allowing interaction. External binding updates still appear."
              Helpers.apiParam
                "PickerMode.Disabled"
                "PickerMode"
                ""
                "Disable interaction and use disabled field styling."
              Helpers.apiParam
                "ClockFormat.TwelveHour"
                "ClockFormat"
                "Default"
                "Hours 1–12 with AM/PM controls; the stored TimeSpan remains a 24-hour time of day."
              Helpers.apiParam
                "ClockFormat.TwentyFourHour"
                "ClockFormat"
                ""
                "Hours 00–23; the dial uses two hour rings."
            ]
          ]
      ],
      attrs = [ Container.MaxWidth.large; Attr.Class "docs-picker-page docs-date-guide" ]
    )
