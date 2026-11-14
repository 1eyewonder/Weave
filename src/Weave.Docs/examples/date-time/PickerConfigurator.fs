namespace Weave.Docs.Examples

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module PickerConfigurator =

  let private mode name =
    match name with
    | "ReadOnly" -> PickerMode.ReadOnly
    | "Disabled" -> PickerMode.Disabled
    | _ -> PickerMode.Editable

  let private attrs (current: PickerAppearance.Configuration) = [
    match current.Variant with
    | "standard" -> Picker.Variant.standard
    | "filled" -> Picker.Variant.filled
    | _ -> Picker.Variant.outlined
    match current.Color with
    | "secondary" -> Picker.Color.secondary
    | "tertiary" -> Picker.Color.tertiary
    | "error" -> Picker.Color.error
    | "warning" -> Picker.Color.warning
    | "success" -> Picker.Color.success
    | "info" -> Picker.Color.info
    | _ -> Picker.Color.primary
    if current.Clearable then
      Picker.Controls.clearable
  ]

  let private dateLiteral (date: DateTime) =
    sprintf "DateTime(%d, %d, %d)" date.Year date.Month date.Day

  let render kind =
    let appearance = Var.Create PickerAppearance.defaults
    let date = Var.Create<DateTime option> None
    let range = Var.Create<Range<DateTime> option> None
    let time = Var.Create<TimeSpan option> None

    let clearable =
      Var.Lens appearance (fun x -> x.Clearable) (fun x value -> { x with Clearable = value })

    let select label choices get set =
      let selected =
        Var.Lens appearance (get >> Some) (fun current value ->
          match value with
          | Some value -> set current value
          | None -> current)

      let display value =
        match value with
        | "TwelveHour" -> "12-hour"
        | "TwentyFourHour" -> "24-hour"
        | "ReadOnly" -> "Read Only"
        | value when value.Length > 0 -> value.Substring(0, 1).ToUpper() + value.Substring(1)
        | _ -> value

      let items =
        choices
        |> List.map (fun value -> SelectItem.create (text (display value), display value, value))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [ Select.Variant.outlined; Select.Label.above; Select.Width.full ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        select "Variant" PickerAppearance.variants (fun x -> x.Variant) (fun x value -> {
          x with
              Variant = value
        })
        select "Color" PickerAppearance.colors (fun x -> x.Color) (fun x value -> { x with Color = value })
        select "Interaction" PickerAppearance.modes (fun x -> x.Mode) (fun x value -> { x with Mode = value })
        if kind = PickerAppearance.Kind.Time then
          select "Clock Format" PickerAppearance.clockFormats (fun x -> x.ClockFormat) (fun x value -> {
            x with
                ClockFormat = value
          })

          select
            "Minute Step"
            (PickerAppearance.minuteSteps |> List.map string<int>)
            (fun x -> string<int> x.MinuteStep)
            (fun x value -> { x with MinuteStep = int value })
      ]

    let preview (current: PickerAppearance.Configuration) =
      let label = View.Const(PickerAppearance.label kind)
      let mode = View.Const(mode current.Mode)

      match kind with
      | PickerAppearance.Kind.Date ->
        DatePicker.create (date, labelText = label, mode = mode, attrs = attrs current)
      | PickerAppearance.Kind.DateRange ->
        DateRangePicker.create (range, labelText = label, mode = mode, attrs = attrs current)
      | PickerAppearance.Kind.Time ->
        let clockFormat =
          if current.ClockFormat = "TwelveHour" then
            ClockFormat.TwelveHour
          else
            ClockFormat.TwentyFourHour

        TimePicker.create (
          time,
          labelText = label,
          mode = mode,
          clockFormat = clockFormat,
          minuteStep = current.MinuteStep,
          attrs = attrs current
        )

    let rangeLiteral selected =
      match selected |> Option.bind Range.tryGetClosed with
      | None -> "None"
      | Some(first, last) -> sprintf "Range.closed (%s) (%s)" (dateLiteral first) (dateLiteral last)

    let selected =
      match kind with
      | PickerAppearance.Kind.Date ->
        date.View
        |> View.Map(Option.map (dateLiteral >> sprintf "Some (%s)") >> Option.defaultValue "None")
      | PickerAppearance.Kind.DateRange -> range.View |> View.Map rangeLiteral
      | PickerAppearance.Kind.Time ->
        time.View
        |> View.Map(
          Option.map (fun time -> sprintf "Some (TimeSpan(%d, %d, 0))" time.Hours time.Minutes)
          >> Option.defaultValue "None"
        )

    let source = (appearance.View, selected) ||> View.Map2(PickerAppearance.render kind)

    let reset () =
      Var.Set appearance PickerAppearance.defaults
      Var.Set date None
      Var.Set range None
      Var.Set time None

    Helpers.appearanceSection
      []
      (Helpers.bodyText
        "Combine field appearance, theme color, clear controls, and interaction mode. Select a value to see it in the generated source. Reset restores the initial settings and selection.")
      controls
      (Some(Switch.create (clearable, text "Allow clearing")))
      (appearance.View |> Doc.BindView preview)
      reset
      source
