namespace Weave

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript; RequireQualifiedAccess>]
type DatePicker =
  static member private createCore
    (
      value: Var<DateTime option>,
      suppliedField: FormField<DateTime option> option,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helpText: Doc,
      ?isOpen: Var<bool>,
      ?mode: View<PickerMode>,
      ?availability: View<DateAvailability>,
      ?firstDayOfWeek: DayOfWeek,
      ?formatDate: DateTime -> string,
      ?attrs: Attr list
    ) =
    let mode = defaultArg mode (View.Const PickerMode.Editable)
    let opened = defaultArg isOpen (Var.Create false)
    let availability = defaultArg availability (View.Const DateAvailability.all)
    let field = suppliedField |> Option.defaultWith (fun () -> FormField.bind value)
    let format = defaultArg formatDate Calendar.iso
    let selected = value.View |> View.Map(Option.map (fun date -> date, date))

    let errors =
      (availability, value.View)
      ||> View.Map2(fun availability selection ->
        match selection with
        | Some date when not (date |> DateAvailability.allows availability) -> [
            "Selected date is unavailable."
          ]
        | Some _
        | None -> [])

    let content () =
      let select date =
        let restrictions = Calendar.sample availability

        if Calendar.allowed restrictions date then
          Var.Set value (Some date)

      Calendar.render {
        Initial = value.View
        Selection = selected
        Availability = availability
        FirstDay = firstDayOfWeek
        Active = PickerRender.canInteract mode
        Select = select
      }

    let display = value.View |> View.Map(Option.map format >> Option.defaultValue "")

    let picker =
      content
      |> PickerRender.shell {
        Name = "Choose date"
        Icon =
          PickerRender.icon
            "M5 4h14a2 2 0 0 1 2 2v14H3V6a2 2 0 0 1 2-2ZM7 2v4M17 2v4M3 9h18M7 13h2M12 13h2M7 17h2M12 17h2"
        Display = display
        HasValue = value.View |> View.Map Option.isSome
        Clear = fun () -> Var.Set value None
        Opened = opened
        Mode = mode
        Label = defaultArg labelText (View.Const "")
        Placeholder = defaultArg placeholder (View.Const "")
        Help = helpText
        Validation = Some field.Validation
        ValidationMessages = None
        Attrs = PickerRender.contributeErrors field errors :: defaultArg attrs []
      }

    picker

  static member create
    (
      value: Var<DateTime option>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helpText: Doc,
      ?isOpen: Var<bool>,
      ?mode: View<PickerMode>,
      ?availability: View<DateAvailability>,
      ?firstDayOfWeek: DayOfWeek,
      ?formatDate: DateTime -> string,
      ?attrs: Attr list
    ) =
    DatePicker.createCore (
      value,
      None,
      ?labelText = labelText,
      ?placeholder = placeholder,
      ?helpText = helpText,
      ?isOpen = isOpen,
      ?mode = mode,
      ?availability = availability,
      ?firstDayOfWeek = firstDayOfWeek,
      ?formatDate = formatDate,
      ?attrs = attrs
    )

  /// Compose availability constraints with the field's application rules.
  /// Unavailable stored values remain entered and block form submission, including after availability changes.
  static member create
    (
      field: FormField<DateTime option>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?helpText: Doc,
      ?isOpen: Var<bool>,
      ?mode: View<PickerMode>,
      ?availability: View<DateAvailability>,
      ?firstDayOfWeek: DayOfWeek,
      ?formatDate: DateTime -> string,
      ?attrs: Attr list
    ) =
    DatePicker.createCore (
      field.Value,
      Some field,
      ?labelText = labelText,
      ?placeholder = placeholder,
      ?helpText = helpText,
      ?isOpen = isOpen,
      ?mode = mode,
      ?availability = availability,
      ?firstDayOfWeek = firstDayOfWeek,
      ?formatDate = formatDate,
      ?attrs = attrs
    )
