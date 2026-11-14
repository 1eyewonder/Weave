namespace Weave.Docs

open System

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module DateTimeGuideSource =

  let private imports =
    "open System\nopen WebSharper.UI\nopen WebSharper.UI.Client\nopen WebSharper.UI.Html\nopen Weave"

  let private setup =
    "open System\nopen WebSharper.UI\nopen WebSharper.UI.Client\nopen WebSharper.UI.Html\nopen Weave\n\nlet month = DateTime.Today.AddMonths 1\n\nlet date day = DateTime(month.Year, month.Month, day)"

  let private withDates code = setup + "\n\n" + code

  let binding () =
    imports
    + "\n\nlet selected = Var.Create<DateTime option> None\n\nDatePicker.create (\n  selected,\n  labelText = View.Const \"Appointment date\",\n  attrs = [ Picker.Color.primary; Picker.Variant.outlined;\n            Picker.Controls.clearable ]\n)"

  let pair (first: DateTime option) (last: DateTime option) =
    let literal (value: DateTime option) =
      match value with
      | Some date -> sprintf "Some (DateTime(%d, %d, %d))" date.Year date.Month date.Day
      | None -> "None"

    sprintf
      "let startDate = Var.Create<DateTime option> (%s)\n\nlet endDate = Var.Create<DateTime option> (%s)\n\nlet checkDates first last =\n  match first, last with\n  | None, _ -> Error \"Choose a start date.\"\n  | _, None -> Error \"Choose an end date.\"\n  | Some first, Some last ->\n    if first <= last then Ok(first, last)\n    else Error \"The end must not precede the start.\"\n\nlet checkedDates =\n  (startDate.View, endDate.View) ||> View.Map2 checkDates\n\nDoc.Concat [\n  DatePicker.create (\n    startDate,\n    labelText = View.Const \"Start date\",\n    attrs = [ Picker.Color.secondary; Picker.Variant.outlined;\n            Picker.Controls.clearable ]\n  )\n  DatePicker.create (\n    endDate,\n    labelText = View.Const \"End date\",\n    attrs = [ Picker.Color.secondary; Picker.Variant.outlined;\n            Picker.Controls.clearable ]\n  )\n  checkedDates |> Doc.BindView (fun range ->\n    match range with\n    | Ok _ -> text \"The dates are in order.\"\n    | Error message -> text message)\n]"
      (literal first)
      (literal last)
    |> fun code -> imports + "\n\n" + code

  let rangePicker () =
    "let initialRange = Range.closed (date 1) (date 7)\n\nlet selected = Var.Create initialRange\n\nDateRangePicker.create (\n  selected,\n  labelText = View.Const \"Booking period\",\n  attrs = [ Picker.Color.secondary; Picker.Variant.outlined;\n            Picker.Controls.clearable ]\n)"
    |> withDates

  let ruleDefinition name =
    let interval =
      match name with
      | "window" -> "let interval = Interval.closed (date 1) (date 7)"
      | "handover" ->
        "let interval =\n  Interval.create\n    (LowerBounds.Inclusive (date 1))\n    (UpperBounds.Exclusive (date 7))"
      | "event" -> "let interval = Some (Interval.Degenerate (date 4))"
      | "unbounded" -> "let interval: Interval<DateTime> option = Some Interval.Unbounded"
      | _ -> invalidArg "name" "Unknown availability example."

    let manual =
      match name with
      | "window" -> "value >= date 1 && value <= date 7"
      | "handover" -> "value >= date 1 && value < date 7"
      | "event" -> "value = date 4"
      | _ -> "true"

    withDates (
      "// A manual rule with ordinary DateTime comparisons.\nlet allowsManually value =\n  "
      + manual
      + "\n\n// Store the same rule as data for membership and picker availability.\n"
      + interval
      + "\n\nlet allowsDate date =\n  interval |> Option.exists (fun rule -> Interval.inInterval rule date)"
    )

  let rule name =
    ruleDefinition name
    + "\n\nlet availability = interval |> Option.toList |> DateAvailability.ofIntervals\n\nlet selected = Var.Create<DateTime option> None\n\nDatePicker.create (\n  selected,\n  labelText = View.Const \"Allowed appointment date\",\n  availability = View.Const availability,\n  attrs = [ Picker.Color.primary; Picker.Variant.outlined;\n            Picker.Controls.clearable ]\n)"

  let bookingRule () =
    "// With ordinary DateTime comparisons, write the membership rule directly.\nlet isAvailableManually value =\n  let inOfferedWindow =\n    (value >= date 1 && value <= date 7)\n    || (value >= date 15 && value <= date 21)\n\n  let alreadyBooked =\n    (value >= date 5 && value <= date 6) || value = date 18\n\n  inOfferedWindow && not alreadyBooked\n\n// With Weave, compose the policy once and pass it to both pickers.\nlet windows =\n  [ Interval.closed (date 1) (date 7)\n    Interval.closed (date 15) (date 21) ]\n  |> List.choose id\n\nlet reservations =\n  [ Interval.closed (date 5) (date 6)\n    Some (Interval.Degenerate (date 18)) ]\n  |> List.choose id\n\nlet availability =\n  DateAvailability.ofIntervals windows\n  |> DateAvailability.excluding reservations"
    |> withDates

  let selection () =
    bookingRule ()
    + "\n\n// The policy is shared; each picker keeps its own selection.\nlet appointment = Var.Create<DateTime option> None\n\nlet selection = Var.Create<Range<DateTime> option> None\n\nDoc.Concat [\n  DatePicker.create (\n    appointment,\n    labelText = View.Const \"Single appointment\",\n    availability = View.Const availability,\n    attrs = [ Picker.Color.primary; Picker.Variant.outlined;\n              Picker.Controls.clearable ]\n  )\n  DateRangePicker.create (\n    selection,\n    labelText = View.Const \"Booking dates\",\n    availability = View.Const availability,\n    attrs = [ Picker.Color.secondary; Picker.Variant.outlined;\n              Picker.Controls.clearable ]\n  )\n]"

  let timeRule () =
    imports
    + "\n\n"
    + "let workingHours =\n  Range.create\n    (LowerBounds.Inclusive (TimeSpan(9, 0, 0)))\n    (UpperBounds.Exclusive (TimeSpan(17, 0, 0)))\n\nlet availability =\n  workingHours\n  |> Option.map Range.toInterval\n  |> Option.toList\n  |> TimeAvailability.ofIntervals\n\nlet isWithinHours time =\n  TimeAvailability.allows availability time"

  let time hours minutes =
    timeRule ()
    + sprintf
        "\n\nlet selected = Var.Create(Some (TimeSpan(%d, %d, 0)))\n\nDoc.Concat [\n  TimePicker.create (\n    selected,\n    labelText = View.Const \"Appointment time\",\n    availability = View.Const availability,\n    clockFormat = ClockFormat.TwelveHour,\n    attrs = [ Picker.Color.tertiary; Picker.Variant.outlined;\n              Picker.Controls.clearable ]\n  )\n  p [] [\n    Doc.TextView (\n      selected.View |> View.Map (fun value ->\n        match value with\n        | None -> \"No time selected\"\n        | Some time ->\n          if isWithinHours time then \"Within working hours\"\n          else \"Outside working hours\")\n    )\n  ]\n]"
        hours
        minutes

  let overnightTimeRule () =
    imports
    + "\n\n"
    + "let overnight =\n  [ Interval.create\n      (LowerBounds.Inclusive (TimeSpan(22, 0, 0)))\n      UpperBounds.Unbounded\n    Interval.create\n      LowerBounds.Unbounded\n      (UpperBounds.Exclusive (TimeSpan(2, 0, 0))) ]\n  |> List.choose id\n  |> TimeAvailability.ofIntervals\n\nlet acceptsLate = TimeAvailability.allows overnight (TimeSpan(23, 0, 0))\n// true\n\nlet acceptsEarly = TimeAvailability.allows overnight (TimeSpan(1, 0, 0))\n// true\n\nlet acceptsNoon = TimeAvailability.allows overnight (TimeSpan(12, 0, 0))\n// false"

  let projection () =
    "let datedHours =\n  Range.create\n    (LowerBounds.Inclusive ((date 1).AddHours 9.0))\n    (UpperBounds.Exclusive ((date 1).AddHours 17.0))\n\nlet hours =\n  datedHours |> Option.bind (Range.mapBounds (fun date -> date.TimeOfDay))\n// Inclusive and exclusive markers are preserved.\n// Reversed endpoints return None.\n// Equal endpoints also return None if either end is excluded."
    |> withDates

  let edgeCases () =
    "let oneDay = Interval.singleton (date 4)\n// Degenerate (date 4): exactly one allowed date.\n\nlet contradictory =\n  Interval.create\n    (LowerBounds.Exclusive (date 4))\n    (UpperBounds.Exclusive (date 4))\n// None: no date can be both after and before day 4.\n\nlet reversed = Interval.closed (date 7) (date 1)\n// None: the start is after the end.\n\nlet sameDay = Interval.closed (date 4) (date 4)\n// Some (Bounded range): equal inclusive bounds permit day 4.\n\nlet acceptsEvent = Interval.inInterval oneDay (date 4)\n// true\n\nlet acceptsSameDay =\n  sameDay\n  |> Option.exists (fun interval -> Interval.inInterval interval (date 4))\n// true\n\nlet acceptsUnbounded = Interval.inInterval Interval.Unbounded (date 4)\n// true: Unbounded permits every value, not invalid bounds."
    |> withDates

  let calendarRange () =
    "let booking =\n  Range.closed ((date 1).AddHours 9.0) ((date 7).AddHours 17.0)\n\nlet calendarDates =\n  booking\n  |> Option.bind (Range.mapBounds (fun date -> date.Date))\n  |> Option.map Range.toInterval\n\nlet requestedDates =\n  [ 1; 4; 7; 8 ] |> List.map date\n\nlet acceptedDates =\n  requestedDates\n  |> List.filter (fun day ->\n    calendarDates\n    |> Option.exists (fun interval -> Interval.inInterval interval day))\n// Days 1, 4, and 7. The time portions were removed from the endpoints."
    |> withDates

  let rescheduledInterval () =
    "let bookingWindow =\n  Interval.create\n    (LowerBounds.Inclusive (date 1))\n    (UpperBounds.Exclusive (date 7))\n\nlet shiftedWindow =\n  bookingWindow\n  |> Option.bind (Interval.mapBounds (fun day -> day.AddDays 7.0))\n\nlet requestedDates = [ 7; 8; 10; 13; 14 ] |> List.map date\n\nlet acceptedDates =\n  shiftedWindow\n  |> Option.map (fun interval ->\n    requestedDates |> List.filter (Interval.inInterval interval))\n// Some [date 8; date 10; date 13]. Day 14 remains excluded.\n\nlet shiftedBounds = shiftedWindow |> Option.bind Interval.tryGetBounds"
    |> withDates

  let cases () = [
    "binding", binding ()
    "overnight time availability", overnightTimeRule ()
    "calendar-date range pipeline", calendarRange ()
    "rescheduled interval pipeline", rescheduledInterval ()
    for first, last in
      [
        Some(DateTime(2026, 12, 1)), Some(DateTime(2026, 12, 7))
        Some(DateTime(2026, 12, 7)), Some(DateTime(2026, 12, 1))
        Some(DateTime(2026, 12, 4)), Some(DateTime(2026, 12, 4))
        None, Some(DateTime(2026, 12, 7))
        Some(DateTime(2026, 12, 1)), None
      ] do
      sprintf "pair %A %A" first last, pair first last
    "range picker", rangePicker ()
    for name in [ "window"; "handover"; "event"; "unbounded" ] do
      name, rule name
    "shared availability", selection ()
    for hour in [ 0; 8; 9; 12; 17; 21; 23 ] do
      sprintf "time %d" hour, time hour 0
    "projection", projection ()
    "edge cases", edgeCases ()
  ]
