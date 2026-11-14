namespace Weave

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript; RequireQualifiedAccess>]
type DateRangePicker =
  static member private readSelection(selection: Range<DateTime> option) =
    let readRange range =
      match Range.tryGetClosed range with
      | Some dates -> dates
      | None ->
        invalidArg
          "value"
          "DateRangePicker selections require ordered, finite, inclusive bounds. Use Range.closed or Range.singleton."

    selection |> Option.map readRange

  static member private createCore
    (
      value: Var<Range<DateTime> option>,
      suppliedField: FormField<Range<DateTime> option> option,
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
    // Validate the initial value even before mounting, and all later values through the shared view.
    value.Value |> DateRangePicker.readSelection |> ignore
    let dates = value.View |> View.Map DateRangePicker.readSelection

    let errors =
      (availability, dates)
      ||> View.Map2(fun availability selection ->
        match selection with
        | Some dates when not (dates |> Calendar.allowsRange availability) -> [
            "Selected range includes an unavailable date."
          ]
        | Some _
        | None -> [])

    let pending = Var.Create<DateTime option> None
    let message = Var.Create ""

    let selection draft selected =
      match draft, selected with
      | Some date, _ -> Some(date, date)
      | None, Some range -> Some range
      | None, None -> None

    let selected = (pending.View, dates) ||> View.Map2 selection

    let content () =
      let select date =
        let restrictions = Calendar.sample availability

        match pending.Value with
        | None ->
          if Calendar.allowed restrictions date then
            Var.Set pending (Some date)
            Var.Set message ""
        | Some start ->
          let first, last = if start <= date then start, date else date, start

          if (first, last) |> Calendar.allowsRange restrictions.Availability then
            Var.Set value (Range.closed first last)

            Var.Set pending None
            Var.Set message ""
          else
            Var.Set message "Range includes an unavailable date. Choose another end date."

      div [] [
        Calendar.render {
          Initial = dates |> View.Map(Option.map fst)
          Selection = selected
          Availability = availability
          FirstDay = firstDayOfWeek
          Active = PickerRender.canInteract mode
          Select = select
        }

        div [ Attr.Create "role" "status" ] [
          message.View
          |> Doc.BindView(fun message ->
            if System.String.IsNullOrEmpty message then
              Doc.Empty
            else
              p [ Typography.body2; Typography.Color.error ] [ text message ])
        ]
      ]

    let display =
      dates
      |> View.Map(
        Option.map (fun (lower, upper) -> format lower + " – " + format upper)
        >> Option.defaultValue ""
      )

    let picker =
      content
      |> PickerRender.shell {
        Name = "Choose date range"
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

    Doc.Concat [
      picker
      opened.View
      |> Doc.sinkCached (fun _ ->
        Var.Set pending None
        Var.Set message "")
      value.View
      |> Doc.sinkCached (fun _ ->
        Var.Set pending None
        Var.Set message "")
    ]

  /// Selects an ordered, inclusive Range<DateTime> of calendar dates at midnight.
  /// External values retain their endpoints; unsupported supplied selections raise ArgumentException.
  static member create
    (
      value: Var<Range<DateTime> option>,
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
    DateRangePicker.createCore (
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
      field: FormField<Range<DateTime> option>,
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
    DateRangePicker.createCore (
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
