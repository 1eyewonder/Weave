namespace Weave.Docs.Examples

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module DatePickerExamples =

  let private bookingExample () =
    let date = Var.Create<DateTime option> None

    let selected =
      date.View
      |> View.Map(
        Option.map PickerExampleHelpers.dateText
        >> Option.defaultValue "Choose your visit date"
      )

    Helpers.codeSampleSection
      "Start with a booking"
      (Helpers.bodyText
        "Open the calendar and choose a date. Choosing a day updates the value and keeps the calendar open for review. Done or Escape closes it. Clear returns the value to None.")
      (PickerExampleHelpers.columns
        (DatePicker.create (
          date,
          labelText = View.Const "Studio visit",
          placeholder = View.Const "Choose a date",
          helpText = FieldHelpText.create (text "One date for your appointment."),
          attrs = [ Picker.Variant.outlined; Picker.Width.full; Picker.Controls.clearable ]
        ))
        (PickerExampleHelpers.summary
          "Studio booking"
          selected
          "Your selection appears here as soon as you choose a day."))
      """open System

let date = Var.Create<DateTime option> None

let selected =
  date.View |> View.Map(Option.map (fun d -> d.ToString("yyyy-MM-dd")))

DatePicker.create (
  date,
  labelText = View.Const "Studio visit",
  placeholder = View.Const "Choose a date",
  helpText = FieldHelpText.create (text "One date for your appointment."),
  attrs =
    [ Picker.Variant.outlined; Picker.Width.full; Picker.Controls.clearable ]
)
// Use date.View to update the rest of your UI."""

  let private cultureExample () =
    let date = Var.Create(Some DateTime.Today)

    Helpers.codeSampleSection
      "Localize the calendar and format the field"
      (Helpers.bodyText
        "The browser culture supplies month names, weekday labels, and week start. These buttons change the shared application culture, including other mounted calendars. Open both fields after switching cultures to compare the default week order with an explicit Sunday override. formatDate controls only the selected-value text.")
      (PickerExampleHelpers.columns
        (PickerExampleHelpers.previews [
          DatePicker.create (
            date,
            labelText = View.Const "Culture default / ISO date",
            attrs = [ Picker.Variant.outlined; Picker.Width.full ]
          )
          DatePicker.create (
            date,
            labelText = View.Const "Sunday first / day-month-year",
            firstDayOfWeek = DayOfWeek.Sunday,
            formatDate = (fun date -> date.ToString("dd/MM/yyyy")),
            attrs = [ Picker.Variant.outlined; Picker.Width.full ]
          )
        ])
        (PickerExampleHelpers.cultureButtons ()))
      """open System

let date = Var.Create(Some DateTime.Today)
Globalization.setCulture "en-GB" // Shared application setting.

div
  []
  [
    DatePicker.create (
      date,
      labelText = View.Const "Culture default / ISO date"
    )
    DatePicker.create (
      date,
      labelText = View.Const "Sunday first / day-month-year",
      firstDayOfWeek = DayOfWeek.Sunday,
      formatDate = (fun date -> date.ToString("dd/MM/yyyy"))
    )
  ]"""

  let private modeExample () =
    let date = Var.Create(Some DateTime.Today)
    let mode = Var.Create PickerMode.Editable

    Helpers.codeSampleSection
      "Change appearance and interaction"
      (Helpers.bodyText
        "Use the same Field style attributes as other Weave inputs. A reactive PickerMode controls the entire picker. Read only keeps the value readable; Disabled prevents interaction. External Var updates still work in either mode.")
      (PickerExampleHelpers.columns
        (DatePicker.create (
          date,
          labelText = View.Const "Review date",
          mode = mode.View,
          attrs = [
            Picker.Variant.outlined
            Picker.Label.above
            Picker.Color.secondary
            Picker.Controls.clearable
          ]
        ))
        (PickerExampleHelpers.stack [
          PickerExampleHelpers.modeControl mode
          Button.primary (
            text "Set to today",
            onClick = (fun () -> Var.Set date (Some DateTime.Today)),
            attrs = [ Button.Variant.outlined ]
          )
        ]))
      """open System

let date = Var.Create(Some DateTime.Today)
let mode = Var.Create PickerMode.Editable

let picker =
  DatePicker.create (
    date,
    mode = mode.View,
    attrs =
      [
        Picker.Variant.filled
        Picker.Color.secondary
        Picker.Controls.clearable
      ]
  )

Var.Set mode PickerMode.ReadOnly
Var.Set date (Some DateTime.Today)
picker"""

  let private validationExample () =
    let date =
      FormField.create ((None: DateTime option), rules = [ Validate.isSome "Choose a visit date." ])

    let booking = Form.model { let! selected = date in return selected }
    let result = Var.Create "Choose a date, then confirm your booking."

    let confirm selected =
      selected
      |> Option.iter (fun selected ->
        Var.Set result ("Visit confirmed for " + PickerExampleHelpers.dateText selected))

    Helpers.codeSampleSection
      "Validate before confirming"
      (Helpers.bodyText
        "Pass a FormField directly to the picker. Try confirming an empty field to reveal the required message, then select a date and submit again. Help text is displayed automatically.")
      (Form.create (
        booking,
        confirm,
        [
          DatePicker.create (
            date,
            labelText = View.Const "Visit date",
            helpText = FieldHelpText.create (text "Required to reserve your place."),
            attrs = [ Picker.Variant.outlined; Picker.Controls.clearable ]
          )
          PickerExampleHelpers.submitButton "Confirm visit"
          div [ Typography.body2; Attr.Create "role" "status" ] [ textView result.View ]
        ],
        attrs = PickerExampleHelpers.formAttrs
      ))
      """open System

let date =
  FormField.create (
    (None: DateTime option),
    rules = [ Validate.isSome "Choose a visit date." ]
  )

let booking = Form.model { let! selected = date in return selected }
let result = Var.Create ""

let confirmBooking (selected: DateTime option) =
  selected
  |> Option.iter (fun d ->
    Var.Set result ("Visit confirmed for " + d.ToString("yyyy-MM-dd")))

Form.create (
  booking,
  confirmBooking,
  [
    DatePicker.create (
      date,
      labelText = View.Const "Visit date",
      helpText = FieldHelpText.create (text "Required to reserve your place.")
    )
    button [ attr.``type`` "submit" ] [ text "Confirm visit" ]
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
        Helpers.pageTitle "Date Picker"
        p [ Typography.body1; Margin.Bottom.small; Attr.Class "docs-picker-page__intro" ] [
          text "Choose a single calendar date. Explore appearance, localization, and form validation."
        ]
        PickerExampleHelpers.guideLink ()
        bookingExample ()
        PickerConfigurator.render PickerAppearance.Kind.Date
        cultureExample ()
        modeExample ()
        validationExample ()

        Helpers.apiSection
          (text "Both create overloads return a Doc and accept the same optional parameters.")
          [
            Helpers.apiTable "DatePicker.create" [
              Helpers.apiParam
                "value"
                "Var<DateTime option> or FormField<DateTime option>"
                ""
                "Optional selection; pass a Var or a FormField for automatic validation and blur tracking."
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
                "Controls selectable calendar dates and validates stored values, including external updates and schedule changes. Unavailable dates remain selected and block submission when bound to a FormField. Navigation limits come from the allowed intervals."
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
