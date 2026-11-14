namespace Weave

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module internal Calendar =
  type Restrictions = {
    Availability: DateAvailability
    Minimum: DateTime option
    Maximum: DateTime option
  }

  type Config = {
    Initial: View<DateTime option>
    Selection: View<(DateTime * DateTime) option>
    Availability: View<DateAvailability>
    FirstDay: DayOfWeek option
    Active: View<bool>
    Select: DateTime -> unit
  }

  type private State = {
    Month: Var<DateTime>
    Focused: Var<DateTime>
    Root: Dom.Element ref
    PendingFocus: DateTime option ref
  }

  type private MonthView = {
    Month: DateTime
    Restrictions: Restrictions
    Culture: string
  }

  let iso (date: DateTime) =
    sprintf "%04d-%02d-%02d" date.Year date.Month date.Day

  let private month (date: DateTime) = DateTime(date.Year, date.Month, 1)

  // A noon anchor keeps calendar-day steps on the intended date across daylight-saving changes.
  let addDays offset (date: DateTime) =
    DateTime(date.Year, date.Month, date.Day, 12, 0, 0).AddDays(float offset).Date

  let allowsRange availability (first: DateTime, last: DateTime) =
    let mutable cursor = first.Date
    let last = last.Date
    let mutable valid = last |> DateAvailability.allows availability

    while valid && cursor < last do
      valid <- cursor |> DateAvailability.allows availability
      cursor <- cursor |> addDays 1

    valid

  let private clamp (restrictions: Restrictions) (date: DateTime) =
    let lower =
      restrictions.Minimum
      |> Option.map (fun date -> date.Date)
      |> Option.defaultValue DateTime.MinValue

    let upper =
      restrictions.Maximum
      |> Option.map (fun date -> date.Date)
      |> Option.defaultValue DateTime.MaxValue

    date.Date |> max lower |> min upper

  let private monthAllowed (restrictions: Restrictions) date =
    let current = month date

    (restrictions.Minimum |> Option.forall (fun first -> month first <= current))
    && (restrictions.Maximum |> Option.forall (fun last -> current <= month last))

  let allowed (restrictions: Restrictions) date =
    date |> DateAvailability.allows restrictions.Availability

  let private firstDate lower =
    match lower with
    | LowerBounds.Unbounded -> Some None
    | LowerBounds.Inclusive date -> Some(Some date)
    | LowerBounds.Exclusive date ->
      if date = DateTime.MaxValue.Date then
        None
      else
        Some(Some(date |> addDays 1))

  let private lastDate upper =
    match upper with
    | UpperBounds.Unbounded -> Some None
    | UpperBounds.Inclusive date -> Some(Some date)
    | UpperBounds.Exclusive date ->
      if date = DateTime.MinValue.Date then
        None
      else
        Some(Some(date |> addDays -1))

  let private tryGetDateLimits interval =
    match interval |> Interval.tryGetBounds with
    | None -> Some(None, None)
    | Some range ->
      match firstDate range.Lower, lastDate range.Upper with
      | Some(Some first), Some(Some last) when first > last -> None
      | Some first, Some last -> Some(first, last)
      | None, _
      | _, None -> None

  let private createRestrictions availability =
    let limits =
      availability |> DateAvailability.sourceIntervals |> List.choose tryGetDateLimits

    let minimum, maximum =
      match limits with
      | [] -> None, None
      | first :: remaining ->
        let extend (minimum, maximum) (first, last) =
          Option.map2 min minimum first, Option.map2 max maximum last

        (first, remaining) ||> List.fold extend

    {
      Availability = availability
      Minimum = minimum
      Maximum = maximum
    }

  let sample availability =
    availability
    |> View.TryGet
    |> Option.defaultValue DateAvailability.none
    |> createRestrictions

  let private restrictionsView (config: Config) =
    config.Availability |> View.Map createRestrictions

  module private Keyboard =
    [<Struct>]
    type Movement =
      | Days of dayCount: int
      | Months of monthCount: int

    let movement firstDay (date: DateTime) (event: Dom.KeyboardEvent) =
      let weekday = (int date.DayOfWeek - int firstDay + 7) % 7
      let monthStep = if event.ShiftKey then 12 else 1

      match event with
      | Key.ArrowLeft -> Some(Days -1)
      | Key.ArrowRight -> Some(Days 1)
      | Key.ArrowUp -> Some(Days -7)
      | Key.ArrowDown -> Some(Days 7)
      | Key.Home -> Some(Days -weekday)
      | Key.End -> Some(Days(6 - weekday))
      | Key.NavKey "PageUp" -> Some(Months -monthStep)
      | Key.NavKey "PageDown" -> Some(Months monthStep)
      | _ -> None

    let destination (date: DateTime) movement =
      match movement with
      | Days offset ->
        let ticks = date.Ticks + int64 offset * TimeSpan.TicksPerDay

        if ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks then
          None
        else
          Some(date |> addDays offset)
      | Months offset ->
        let index = date.Year * 12 + date.Month + offset

        if index < 13 || index > 120000 then
          None
        else
          Some(date.AddMonths offset)

    let handle firstDay restrictions focusDate date (event: Dom.KeyboardEvent) =
      match event |> movement firstDay date with
      | None -> ()
      | Some movement ->
        event.PreventDefault()

        movement
        |> destination date
        |> Option.map (clamp restrictions)
        |> Option.iter focusDate

  let private restoreFocus state (element: Dom.Element) =
    match state.PendingFocus.Value with
    | None -> ()
    | Some date ->
      if not (isNull element) then
        let target = element.QuerySelector("[data-date='" + iso date + "']")
        state.PendingFocus.Value <- None

        if not (isNull target) then
          target?focus ()

  let private focusDate state date =
    state.PendingFocus.Value <- Some date
    Var.Set state.Focused date

    if month date = state.Month.Value then
      state.Root.Value |> restoreFocus state
    else
      Var.Set state.Month (month date)

  let private parseNumber (value: string) = Int32.TryParse value

  let private changeYear restrictions (state: State) (element: Dom.Element) _ =
    match parseNumber (element?value) with
    | true, value when value >= 1 && value <= 9999 ->
      let target = DateTime(value, state.Month.Value.Month, 1) |> clamp restrictions
      // Clamping may retain the same month, so reset the native input explicitly.
      element?value <- string<int> target.Year
      target |> focusDate state
    | _ -> element?value <- string<int> state.Month.Value.Year

  let private renderHeader
    (config: Config)
    (state: State)
    (restrictions: Restrictions)
    culture
    (current: DateTime)
    =
    let previous =
      config.Active
      |> View.Map(fun active ->
        active
        && (current.Year > 1 || current.Month > 1)
        && (restrictions.Minimum |> Option.forall (fun date -> month date < current)))

    let next =
      config.Active
      |> View.Map(fun active ->
        active
        && (current.Year < 9999 || current.Month < 12)
        && (restrictions.Maximum |> Option.forall (fun date -> month date > current)))

    let selectedMonth = Var.Create(Some current.Month)

    let months =
      [
        for number in 1..12 do
          let name = Globalization.monthName culture number

          SelectItem.create (
            text name,
            name,
            number,
            disabled = View.Const(not (DateTime(current.Year, number, 1) |> monthAllowed restrictions)),
            attrs = [ Attr.Create "data-calendar-month" (string<int> number) ]
          )
      ]
      |> View.Const

    let changeMonth selected =
      match selected with
      | Some number when number <> state.Month.Value.Month ->
        let target = DateTime(state.Month.Value.Year, number, 1)

        if target |> monthAllowed restrictions then
          target |> clamp restrictions |> focusDate state
        else
          Var.Set selectedMonth (Some state.Month.Value.Month)
      | Some _
      | None -> ()

    div [ cl Css.``weave-calendar__header`` ] [
      PickerRender.iconAction "Previous month" (PickerRender.icon "m15 5-7 7 7 7") previous (fun () ->
        current.AddMonths(-1) |> focusDate state)

      selectedMonth.View |> Doc.sinkCached changeMonth

      div [ cl Css.``weave-calendar__month`` ] [
        Select.create (months, selectedMonth, labelText = View.Const "Month", enabled = config.Active)
      ]

      // The native year input commits after editing, avoiding a reset on each keystroke.
      Doc.Element "input" [
        cl Css.``weave-calendar__year``
        attr.``type`` "number"
        Attr.Create "aria-label" "Year"
        Attr.Create
          "min"
          (restrictions.Minimum
           |> Option.map (fun date -> string<int> date.Year)
           |> Option.defaultValue "1")
        Attr.Create
          "max"
          (restrictions.Maximum
           |> Option.map (fun date -> string<int> date.Year)
           |> Option.defaultValue "9999")
        Attr.enabled config.Active
        Attr.DynamicProp "value" (state.Month.View |> View.Map(fun date -> string<int> date.Year))
        on.change (changeYear restrictions state)
      ] []

      PickerRender.iconAction "Next month" (PickerRender.icon "m9 5 7 7-7 7") next (fun () ->
        current.AddMonths(1) |> focusDate state)
    ]

  let private renderDay (config: Config) state (restrictions: Restrictions) firstDay date =
    let selected =
      config.Selection
      |> View.Map(Option.exists (fun (start, finish) -> date >= start.Date && date <= finish.Date))

    let select _ _ active =
      if active && allowed restrictions date then
        config.Select date

    let navigate _ event active =
      if active then
        event |> Keyboard.handle firstDay restrictions (focusDate state) date

    button [
      attr.``type`` "button"
      cl Css.``weave-calendar__day``
      Attr.Create "data-date" (iso date)
      Attr.Create "aria-label" (iso date)
      PickerRender.boolAttr "aria-pressed" selected
      Attr.DynamicClassPred Css.``weave-calendar__day--selected`` selected

      if date = DateTime.Today then
        Attr.Create "aria-current" "date"

      // Keep unavailable dates focusable for predictable arrow-key navigation.
      PickerRender.boolAttr
        "aria-disabled"
        (config.Active
         |> View.Map(fun active -> not (active && allowed restrictions date)))

      Attr.DynamicCustom
        (fun element value -> element.SetAttribute("tabindex", value))
        (state.Focused.View
         |> View.Map(fun focused -> if focused.Date = date then "0" else "-1"))

      on.focus (fun _ _ -> Var.Set state.Focused date)
      on.clickView config.Active select
      on.keyDownView config.Active navigate
    ] [ text (string<int> date.Day) ]

  let private renderMonth config state (view: MonthView) =
    // Keep focus when an application culture or availability update rebuilds the month.
    let root = state.Root.Value
    let focusedElement = As<Dom.Element>(JS.Document?activeElement)

    if
      not (isNull root)
      && not (isNull focusedElement)
      && root.Contains focusedElement
      && state.PendingFocus.Value.IsNone
    then
      state.PendingFocus.Value <- Some state.Focused.Value

    let current = view.Month
    let restrictions = view.Restrictions

    let firstDay =
      defaultArg config.FirstDay (Globalization.firstDayOfWeek view.Culture)

    let offset = (int current.DayOfWeek - int firstDay + 7) % 7

    div [ on.afterRender (restoreFocus state) ] [
      current |> renderHeader config state restrictions view.Culture

      div [
        cl Css.``weave-calendar__title``
        Typography.srOnly
        Attr.Create "aria-live" "polite"
      ] [ text (Globalization.monthTitle view.Culture current.Year current.Month) ]

      div [ cl Css.``weave-calendar__weekdays``; Attr.Create "aria-hidden" "true" ] [
        for number in 0..6 do
          let weekday = enum<DayOfWeek>((int firstDay + number) % 7)
          span [] [ text (Globalization.weekdayName view.Culture weekday) ]
      ]

      div [
        cl Css.``weave-calendar__days``
        Attr.Create "role" "group"
        Attr.Create "aria-label" "Calendar dates"
      ] [
        for _ in 1..offset do
          span [] []
        for day in 1 .. DateTime.DaysInMonth(current.Year, current.Month) do
          DateTime(current.Year, current.Month, day)
          |> renderDay config state restrictions firstDay
      ]
    ]

  let render (config: Config) =
    let state = {
      Month = Var.Create(month DateTime.Today)
      Focused = Var.Create DateTime.Today
      Root = ref (Unchecked.defaultof<Dom.Element>)
      PendingFocus = ref None
    }

    let restrictions = restrictionsView config

    let initialDate =
      (config.Initial, restrictions)
      ||> View.Map2(fun initial restrictions -> defaultArg initial DateTime.Today |> clamp restrictions)

    let synchronize date =
      Var.Set state.Focused date
      Var.Set state.Month (month date)

    div [
      cl Css.``weave-calendar``
      on.afterRender (fun element -> state.Root.Value <- element)
    ] [
      initialDate |> Doc.sinkCached synchronize

      (state.Month.View, restrictions, Globalization.culture)
      |||> View.Map3(fun current restrictions culture -> {
        Month = current
        Restrictions = restrictions
        Culture = culture
      })
      |> Doc.BindView(renderMonth config state)
    ]
