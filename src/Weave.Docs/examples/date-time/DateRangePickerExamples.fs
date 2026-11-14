namespace Weave.Docs.Examples

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module DateRangePickerExamples =

  let private countNights lower upper =
    let dayNumber (date: DateTime) =
      let year = date.Year - 1

      let previousMonths =
        [ 1 .. date.Month - 1 ]
        |> List.sumBy (fun month -> DateTime.DaysInMonth(date.Year, month))

      365 * year + year / 4 - year / 100 + year / 400 + previousMonths + date.Day

    dayNumber upper - dayNumber lower

  let private describeStay selection =
    match selection |> Option.bind Range.tryGetClosed with
    | None -> "Your next escape starts here"
    | Some(lower, upper) ->
      let nights = countNights lower upper

      sprintf
        "%s to %s / %d %s"
        (PickerExampleHelpers.dateText lower)
        (PickerExampleHelpers.dateText upper)
        nights
        (if nights = 1 then "night" else "nights")

  let private bookingExample () =
    let stay = Var.Create<Range<DateTime> option> None

    Helpers.codeSampleSection
      "Plan a stay"
      (Helpers.bodyText
        "Choose a start date, then an end date. Both endpoints are included and a completed range is stored in one Range<DateTime> value. Choosing the earlier date second still produces an ordered range. The calendar stays open after the second date; Done closes it. Closing after only one click discards the unfinished selection.")
      (PickerExampleHelpers.columns
        (DateRangePicker.create (
          stay,
          labelText = View.Const "Check-in / check-out",
          helpText = FieldHelpText.create (text "Choose two dates to complete your stay."),
          attrs = [ Picker.Variant.outlined; Picker.Width.full; Picker.Controls.clearable ]
        ))
        (PickerExampleHelpers.summary
          "Weekend escape"
          (stay.View |> View.Map describeStay)
          "Nights are counted by calendar date, so daylight-saving changes do not change the stay length. The calendar includes both selected dates."))
      """open System

// Count calendar nights independently of daylight-saving offsets.
let countNights lower upper =
  let dayNumber (date: DateTime) =
    let year = date.Year - 1
    let previousMonths =
      [ 1 .. date.Month - 1 ]
      |> List.sumBy (fun month -> DateTime.DaysInMonth(date.Year, month))

    365 * year + year / 4 - year / 100 + year / 400
    + previousMonths + date.Day

  dayNumber upper - dayNumber lower

let stay = Var.Create<Range<DateTime> option> None

let nights =
  stay.View
  |> View.Map(
    Option.bind Range.tryGetClosed
    >> Option.map (fun (lower, upper) -> countNights lower upper)
  )

DateRangePicker.create (
  stay,
  labelText = View.Const "Check-in / check-out",
  helpText =
    FieldHelpText.create (text "Choose two dates to complete your stay."),
  attrs =
    [ Picker.Variant.outlined; Picker.Width.full; Picker.Controls.clearable ]
)"""

  let private cultureExample () =
    let startDate = DateTime.Today

    let stay = Var.Create(Range.closed startDate (startDate.AddDays 3.0))

    Helpers.codeSampleSection
      "Localize and format a range"
      (Helpers.bodyText
        "Calendar labels and the default week start follow the shared application culture. The buttons affect all mounted calendars. The two fields share a value so you can compare ISO output with a custom format and explicit Monday-first calendar.")
      (PickerExampleHelpers.columns
        (PickerExampleHelpers.previews [
          DateRangePicker.create (
            stay,
            labelText = View.Const "Culture default / ISO dates",
            attrs = [ Picker.Variant.outlined; Picker.Width.full ]
          )
          DateRangePicker.create (
            stay,
            labelText = View.Const "Monday first / day-month-year",
            firstDayOfWeek = DayOfWeek.Monday,
            formatDate = (fun date -> date.ToString("dd/MM/yyyy")),
            attrs = [ Picker.Variant.outlined; Picker.Width.full ]
          )
        ])
        (PickerExampleHelpers.cultureButtons ()))
      """open System

let stay =
  Var.Create(Range.closed DateTime.Today (DateTime.Today.AddDays 3.0))

Globalization.setCulture "en-US"

div
  []
  [
    DateRangePicker.create (stay)
    DateRangePicker.create (
      stay,
      firstDayOfWeek = DayOfWeek.Monday,
      formatDate = (fun date -> date.ToString("dd/MM/yyyy"))
    )
  ]"""

  let private modeExample () =
    let startDate = DateTime.Today

    let stay = Var.Create(Range.closed startDate (startDate.AddDays 3.0))

    let mode = Var.Create PickerMode.Editable

    Helpers.codeSampleSection
      "Review an existing booking"
      (Helpers.bodyText
        "Prepopulate the Var for an existing booking. Use a reactive mode for editable, read-only, or disabled states, and attribute modules for appearance. The mode applies to both dates together.")
      (PickerExampleHelpers.columns
        (DateRangePicker.create (
          stay,
          labelText = View.Const "Reserved dates",
          mode = mode.View,
          attrs = [
            Picker.Variant.outlined
            Picker.Label.above
            Picker.Color.secondary
            Picker.Controls.clearable
          ]
        ))
        (PickerExampleHelpers.stack [ PickerExampleHelpers.modeControl mode ]))
      """open System

let stay =
  Var.Create(Range.closed DateTime.Today (DateTime.Today.AddDays 3.0))

let mode = Var.Create PickerMode.Editable

let picker =
  DateRangePicker.create (
    stay,
    labelText = View.Const "Reserved dates",
    mode = mode.View,
    attrs =
      [
        Picker.Variant.filled
        Picker.Color.secondary
        Picker.Controls.clearable
      ]
  )

Var.Set mode PickerMode.ReadOnly
picker"""

  let private validationExample () =
    let stay =
      FormField.create (
        (None: Range<DateTime> option),
        rules = [ Validate.isSome "Choose both booking dates." ]
      )

    let booking = Form.model { let! selected = stay in return selected }
    let result = Var.Create "Complete a range to reserve your stay."

    let confirm selected =
      selected
      |> Option.iter (fun range -> Var.Set result ("Reserved: " + describeStay (Some range)))

    Helpers.codeSampleSection
      "Require a complete range"
      (Helpers.bodyText
        "Picker selections require ordered, finite, inclusive endpoints. A partial selection does not satisfy a required FormField. Try submitting before choosing both endpoints. The same validation and form patterns used by other Weave fields apply here.")
      (Form.create (
        booking,
        confirm,
        [
          DateRangePicker.create (
            stay,
            labelText = View.Const "Stay dates",
            helpText = FieldHelpText.create (text "Both check-in and check-out are required."),
            attrs = [ Picker.Variant.outlined; Picker.Controls.clearable ]
          )
          PickerExampleHelpers.submitButton "Reserve stay"
          div [ Typography.body2; Attr.Create "role" "status" ] [ textView result.View ]
        ],
        attrs = PickerExampleHelpers.formAttrs
      ))
      """open System

// Count calendar nights independently of daylight-saving offsets.
let countNights lower upper =
  let dayNumber (date: DateTime) =
    let year = date.Year - 1
    let previousMonths =
      [ 1 .. date.Month - 1 ]
      |> List.sumBy (fun month -> DateTime.DaysInMonth(date.Year, month))

    365 * year + year / 4 - year / 100 + year / 400
    + previousMonths + date.Day

  dayNumber upper - dayNumber lower

let stay =
  FormField.create (
    (None: Range<DateTime> option),
    rules = [ Validate.isSome "Choose both booking dates." ]
  )

let booking = Form.model { let! selected = stay in return selected }
let result = Var.Create ""

let reserveStay (selected: Range<DateTime> option) =
  selected
  |> Option.bind Range.tryGetClosed
  |> Option.iter (fun (lower, upper) ->
    let nights = countNights lower upper
    let label = if nights = 1 then "night" else "nights"
    Var.Set result (sprintf "Reserved for %d %s" nights label))

Form.create (
  booking,
  reserveStay,
  [
    DateRangePicker.create (stay, labelText = View.Const "Stay dates")
    button [ attr.``type`` "submit" ] [ text "Reserve stay" ]
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
        Helpers.pageTitle "Date Range Picker"
        p [ Typography.body1; Margin.Bottom.small; Attr.Class "docs-picker-page__intro" ] [
          text
            "Collect an ordered pair of dates in one field. Explore appearance, localization, and required-range validation."
        ]
        PickerExampleHelpers.guideLink ()
        bookingExample ()
        PickerConfigurator.render PickerAppearance.Kind.DateRange
        cultureExample ()
        modeExample ()
        validationExample ()

        Helpers.apiSection
          (text "Both create overloads return a Doc and accept the same optional parameters.")
          [
            Helpers.apiTable "DateRangePicker.create" [
              Helpers.apiParam
                "value"
                "Var<Range<DateTime> option> or FormField<Range<DateTime> option>"
                ""
                "Ordered, finite, inclusive selection. Create it with Range.closed or Range.singleton. Unsupported supplied selections raise ArgumentException. Pass a FormField for automatic validation and blur tracking."
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
                "?availability"
                "View<DateAvailability>"
                "View.Const DateAvailability.all"
                "Every calendar date in a selection must satisfy this reactive rule, including dates between the endpoints. Stored ranges are retained and revalidated when the value or availability changes; unavailable ranges block FormField submission. Navigation limits come from the allowed intervals."
              Helpers.apiParam
                "?firstDayOfWeek"
                "DayOfWeek"
                "Application culture"
                "Explicit override of the culture default."
              Helpers.apiParam
                "?formatDate"
                "DateTime -> string"
                "ISO yyyy-MM-dd"
                "Format the selected date display; does not affect localized calendar labels."
            ]
            Helpers.styleModuleTable "Picker styles" [
              "Variant.standard / filled / outlined", "Field appearance"
              "Color.primary / secondary / tertiary / error / warning / success / info", "Theme palette"
              "Label.above", "Persistent outlined label"
              "Width.full", "Fill available width"
              "Controls.clearable", "Show Clear for a selected value"
              "Controls.clearableWhen (View<bool>)", "Reactively control the same class through an Attr"
            ]
            Helpers.apiTable "Globalization" [
              Helpers.apiParam
                "Globalization.setCulture name"
                "string -> unit"
                "Browser culture"
                "Set a shared BCP 47 culture such as en-US, en-GB, or fr-FR."
              Helpers.apiParam "Globalization.culture" "View<string>" "" "Read-only application-culture view."
            ]
          ]
      ],
      attrs = [ Container.MaxWidth.large; Attr.Class "docs-picker-page" ]
    )
