namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module PickerAppearance =

  [<RequireQualifiedAccess; Struct>]
  type Kind =
    | Date
    | DateRange
    | Time

  type Configuration = {
    Variant: string
    Color: string
    Mode: string
    Clearable: bool
    ClockFormat: string
    MinuteStep: int
  }

  let variants = [ "standard"; "filled"; "outlined" ]
  let colors = [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ]
  let modes = [ "Editable"; "ReadOnly"; "Disabled" ]
  let clockFormats = [ "TwelveHour"; "TwentyFourHour" ]
  let minuteSteps = [ 1; 5; 10; 15; 20; 30 ]

  let defaults = {
    Variant = "outlined"
    Color = "primary"
    Mode = "Editable"
    Clearable = true
    ClockFormat = "TwelveHour"
    MinuteStep = 1
  }

  let label kind =
    match kind with
    | Kind.Date -> "Visit date"
    | Kind.DateRange -> "Booking dates"
    | Kind.Time -> "Visit time"

  let render kind current selected =
    let pickerName, valueType =
      match kind with
      | Kind.Date -> "DatePicker", "DateTime option"
      | Kind.DateRange -> "DateRangePicker", "Range<DateTime> option"
      | Kind.Time -> "TimePicker", "TimeSpan option"

    [
      yield "open System"
      yield ""
      yield "let value ="
      yield sprintf "  Var.Create<%s> (" valueType
      yield "    " + selected
      yield "  )"
      yield ""
      yield pickerName + ".create ("
      yield "  value,"
      yield sprintf "  labelText = View.Const \"%s\"," (label kind)
      if current.Mode <> "Editable" then
        yield "  mode = View.Const PickerMode." + current.Mode + ","
      if kind = Kind.Time then
        yield "  clockFormat = ClockFormat." + current.ClockFormat + ","
        yield sprintf "  minuteStep = %d," current.MinuteStep
      yield "  attrs = ["
      yield "    Picker.Variant." + current.Variant
      yield "    Picker.Color." + current.Color
      if current.Clearable then
        yield "    Picker.Controls.clearable"
      yield "  ]"
      yield ")"
    ]
    |> String.concat "\n"

  let cases kind = [
    for variant in variants do
      for color in colors do
        for mode in modes do
          for clearable in [ false; true ] do
            let current = {
              defaults with
                  Variant = variant
                  Color = color
                  Mode = mode
                  Clearable = clearable
            }

            sprintf "%A %s %s %s %b" kind variant color mode clearable, render kind current "None"
    if kind = Kind.Time then
      for format in clockFormats do
        for step in minuteSteps do
          let current = {
            defaults with
                ClockFormat = format
                MinuteStep = step
          }

          sprintf "time %s %d" format step, render kind current "Some (TimeSpan(13, 30, 0))"
    let selected =
      match kind with
      | Kind.Date -> "Some (DateTime(2026, 12, 4))"
      | Kind.DateRange -> "Range.closed DateTime.Today (DateTime.Today.AddDays 3.0)"
      | Kind.Time -> "Some (TimeSpan(9, 0, 0))"

    "selected", render kind defaults selected
  ]
