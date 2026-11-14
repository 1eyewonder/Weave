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
module internal TimePickerClock =
  [<Struct>]
  type Phase =
    | Hours
    | Minutes

  type Entry = {
    Field: FormField<string>
    Edited: Var<bool>
    Rule: Validator<string>
    Revision: Var<int>
    Display: TimeSpan option -> string
    Displayed: Var<string>
  }

  type Entries = {
    Hour: Entry
    Minute: Entry
  }

  type Context = {
    Value: Var<TimeSpan option>
    Phase: Var<Phase>
    Format: ClockFormat
    MinuteStep: int
    Availability: View<TimeAvailability>
    Active: View<bool>
    Entries: Entries
    ValueErrors: View<string list>
    ResetVersion: Var<int>
    Touch: unit -> unit
  }

  let private normalize (time: TimeSpan) =
    if time < TimeSpan.Zero || time >= TimeSpan.FromDays(1.0) then
      TimeSpan.Zero
    else
      TimeSpan(time.Hours, time.Minutes, 0)

  let private getTime context =
    context.Value.Value |> Option.defaultValue TimeSpan.Zero |> normalize

  let private toHour12 hour = if hour % 12 = 0 then 12 else hour % 12

  let private getHour format (time: TimeSpan) =
    match format with
    | ClockFormat.TwelveHour -> toHour12 time.Hours
    | ClockFormat.TwentyFourHour -> time.Hours

  let format format (time: TimeSpan) =
    match format with
    | ClockFormat.TwelveHour ->
      sprintf "%d:%02d %s" (toHour12 time.Hours) time.Minutes (if time.Hours >= 12 then "PM" else "AM")
    | ClockFormat.TwentyFourHour -> sprintf "%02d:%02d" time.Hours time.Minutes

  let private getNumber phase context =
    match phase with
    | Hours -> getTime context |> getHour context.Format
    | Minutes -> (getTime context).Minutes

  let private canInteract context =
    context.Active |> View.TryGet = Some true

  let private resetEntry current entry =
    entry.Field.Reset()
    let text = entry.Display current
    Var.Set entry.Displayed text
    Var.Set entry.Field.Value text
    Var.Set entry.Edited false

  let private commit context value =
    if canInteract context then
      context.Touch()
      Var.Set context.Value (Some value)

  let private commitDial phase context value =
    if canInteract context then
      commit context value

      let entry =
        if phase = Hours then
          context.Entries.Hour
        else
          context.Entries.Minute

      resetEntry (Some value) entry

  let private timeWithNumber phase context value =
    let current = getTime context

    match phase with
    | Hours ->
      let selectedHour =
        match context.Format with
        | ClockFormat.TwelveHour -> value % 12 + (if current.Hours >= 12 then 12 else 0)
        | ClockFormat.TwentyFourHour -> value

      TimeSpan(selectedHour, current.Minutes, 0)
    | Minutes -> TimeSpan(current.Hours, value, 0)

  let private sampleAvailability context =
    context.Availability |> View.TryGet |> Option.defaultValue TimeAvailability.none

  let private timeInHour context availability hour =
    let candidate minute = TimeSpan(hour, minute, 0)

    let allowed minute =
      candidate minute |> TimeAvailability.allows availability

    let current = (getTime context).Minutes

    let minute =
      if current % context.MinuteStep = 0 && allowed current then
        Some current
      else
        [ 0 .. context.MinuteStep .. 59 ] |> List.tryFind allowed

    minute |> Option.map candidate

  let private dialTime phase context availability value =
    match phase with
    | Hours ->
      let hour = (timeWithNumber Hours context value).Hours
      timeInHour context availability hour
    | Minutes ->
      let candidate = timeWithNumber Minutes context value

      if candidate |> TimeAvailability.allows availability then
        Some candidate
      else
        None

  let private availableNumbers phase context availability =
    let choices =
      match phase, context.Format with
      | Hours, ClockFormat.TwelveHour -> [ 1..12 ]
      | Hours, ClockFormat.TwentyFourHour -> [ 0..23 ]
      | Minutes, ClockFormat.TwelveHour
      | Minutes, ClockFormat.TwentyFourHour -> [ 0 .. context.MinuteStep .. 59 ]

    choices
    |> List.filter (fun value -> dialTime phase context availability value |> Option.isSome)

  let private setNumber phase context value =
    value
    |> dialTime phase context (sampleAvailability context)
    |> Option.iter (commitDial phase context)

  let private select phase context value =
    if canInteract context then
      match value |> dialTime phase context (sampleAvailability context) with
      | None -> ()
      | Some time ->
        commitDial phase context time

        match phase with
        | Hours -> Var.Set context.Phase Minutes
        | Minutes -> ()

  let private periodTime isPm context availability =
    let first = if isPm then 12 else 0
    let hour = (getTime context).Hours % 12 + first

    match timeInHour context availability hour with
    | Some time -> Some time
    | None -> [ first .. first + 11 ] |> List.tryPick (timeInHour context availability)

  let private getLimits phase context =
    match phase, context.Format with
    | Hours, ClockFormat.TwelveHour -> 1, 12
    | Hours, ClockFormat.TwentyFourHour -> 0, 23
    | Minutes, ClockFormat.TwelveHour
    | Minutes, ClockFormat.TwentyFourHour -> 0, 59 / context.MinuteStep * context.MinuteStep

  let private isAsciiDigit c = c >= '0' && c <= '9'

  let private tryNumber minimum maximum (raw: string) =
    let digits = raw.Length > 0 && raw.Length <= 2 && (raw |> Seq.forall isAsciiDigit)

    match Int32.TryParse raw with
    | true, number when digits && number >= minimum && number <= maximum -> Some number
    | _ -> None

  let createEntries format step (value: Var<TimeSpan option>) =
    let create phase minimum maximum =
      let message =
        if phase = Hours then
          sprintf "Enter hours from %d to %d." minimum maximum
        else
          sprintf "Enter minutes from 0 to %d in steps of %d." maximum step

      let rule =
        Validate.custom
          (fun raw ->
            match tryNumber minimum maximum raw with
            | Some number -> phase = Hours || number % step = 0
            | None -> false)
          message

      let display current =
        current
        |> Option.map (fun (time: TimeSpan) ->
          sprintf "%02d" (if phase = Hours then getHour format time else time.Minutes))
        |> Option.defaultValue ""

      let initial = display value.Value

      {
        Field = FormField.create (initial, rules = [ rule ])
        Edited = Var.Create false
        Rule = rule
        Revision = Var.Create 0
        Display = display
        Displayed = Var.Create initial
      }

    let minimum, maximum = if format = ClockFormat.TwelveHour then 1, 12 else 0, 23

    {
      Hour = create Hours minimum maximum
      Minute = create Minutes 0 (59 / step * step)
    }

  let resetEntries current entries =
    resetEntry current entries.Hour
    resetEntry current entries.Minute

  let private synchronizeEntry current entry =
    // A change to the other part of the time must not replace an in-progress draft.
    if entry.Displayed.Value <> entry.Display current then
      resetEntry current entry

  let synchronizeEntries current entries =
    synchronizeEntry current entries.Hour
    synchronizeEntry current entries.Minute

  let entryErrors (resetVersion: Var<int>) entries =
    let errors entry =
      let current = (entry.Revision.View, resetVersion.View) ||> View.Map2 (=)

      (current, entry.Edited.View, entry.Field.Errors)
      |||> View.Map3(fun current edited errors -> if current && edited then errors else [])

    (errors entries.Hour, errors entries.Minute) ||> View.Map2 (@)

  let timeRules step : Validator<TimeSpan option> =
    Validate.optional (
      Validate.all [
        Validate.atLeast TimeSpan.Zero "Time must be on or after 00:00."
        Validate.lessThan (TimeSpan.FromDays 1.0) "Time must be before 24:00."
      ]
      |> Validate.andThen (
        Validate.custom
          (fun time -> time.Ticks % TimeSpan.TicksPerMinute = 0L && time.Minutes % step = 0)
          (sprintf "Choose a whole minute in steps of %d." step)
      )
    )

  let private handleKey phase context (_: Dom.Element) (event: Dom.KeyboardEvent) =
    if
      (As<Dom.Element> event.Target).GetAttribute("role") = "slider"
      && canInteract context
    then
      let choices = availableNumbers phase context (sampleAvailability context)

      let move offset =
        let current = getNumber phase context

        let candidates =
          if offset > 0 then
            choices |> List.filter (fun value -> value > current)
          else
            choices |> List.rev |> List.filter (fun value -> value < current)

        candidates |> List.tryHead |> Option.iter (setNumber phase context)

      match event with
      | Key.ArrowUp
      | Key.ArrowRight ->
        event.PreventDefault()
        move 1
      | Key.ArrowDown
      | Key.ArrowLeft ->
        event.PreventDefault()
        move -1
      | Key.Home ->
        event.PreventDefault()
        choices |> List.tryHead |> Option.iter (setNumber phase context)
      | Key.End ->
        event.PreventDefault()
        choices |> List.tryLast |> Option.iter (setNumber phase context)
      | Key.Activate ->
        event.PreventDefault()
        getNumber phase context |> select phase context
      | _ -> ()

  let private getRadius phase format value =
    match phase, format with
    | Hours, ClockFormat.TwentyFourHour when value >= 1 && value <= 12 -> 27.0
    | Hours, ClockFormat.TwentyFourHour
    | Hours, ClockFormat.TwelveHour
    | Minutes, ClockFormat.TwelveHour
    | Minutes, ClockFormat.TwentyFourHour -> 40.0

  let private getAngle phase value =
    match phase with
    | Hours -> float (value % 12) * Math.PI / 6.0
    | Minutes -> float value * Math.PI / 30.0

  let private getPosition phase format value =
    let angle = getAngle phase value
    let radius = getRadius phase format value
    50.0 + Math.Sin(angle) * radius, 50.0 - Math.Cos(angle) * radius

  let private getPointerValue phase context (element: Dom.Element) (event: Dom.MouseEvent) =
    let bounds = element.GetBoundingClientRect()
    let x = float event.ClientX - bounds.Left - bounds.Width / 2.0
    let y = float event.ClientY - bounds.Top - bounds.Height / 2.0
    let radians = (Math.Atan2(x, -y) + 2.0 * Math.PI) % (2.0 * Math.PI)

    match phase with
    | Hours ->
      let clockHour = int (Math.Floor(radians * 6.0 / Math.PI + 0.5)) % 12

      match context.Format with
      | ClockFormat.TwelveHour -> toHour12 clockHour
      | ClockFormat.TwentyFourHour ->
        let inner = Math.Sqrt(x * x + y * y) < bounds.Width * 0.335

        if inner then toHour12 clockHour
        elif clockHour = 0 then 0
        else clockHour + 12
    | Minutes ->
      let raw = radians * 30.0 / Math.PI

      let distance value =
        let difference = abs (float value - raw)
        min difference (60.0 - difference)

      [ 0 .. context.MinuteStep .. 59 ] |> List.minBy distance

  let private attachPointers phase context (element: Dom.Element) =
    let dragging = ref false

    let sample (event: Dom.Event) =
      getPointerValue phase context element (As<Dom.MouseEvent> event)

    let start (event: Dom.Event) =
      if canInteract context then
        event.PreventDefault()
        let slider = element.QuerySelector("[role='slider']")

        if not (isNull slider) then
          slider?focus ()

        dragging.Value <- true
        element?setPointerCapture (event?pointerId)
        event |> sample |> setNumber phase context

    let move (event: Dom.Event) =
      if dragging.Value then
        event |> sample |> setNumber phase context

    let finish (event: Dom.Event) =
      if dragging.Value then
        dragging.Value <- false
        event |> sample |> select phase context

    let cancel (_: Dom.Event) = dragging.Value <- false
    element.AddEventListener("pointerdown", start)
    element.AddEventListener("pointermove", move)
    element.AddEventListener("pointerup", finish)
    element.AddEventListener("pointercancel", cancel)

  let private renderHand phase context =
    let selected =
      context.Value.View
      |> View.Map(fun value -> value |> Option.defaultValue TimeSpan.Zero |> normalize)

    let getEndpoint value =
      let number =
        match phase with
        | Hours -> getHour context.Format value
        | Minutes -> value.Minutes

      number |> getPosition phase context.Format

    let endpoint = selected |> View.Map getEndpoint

    let coordinate project =
      endpoint |> View.Map(project >> sprintf "%f")

    Doc.SvgElement "svg" [
      cl Css.``weave-time-picker__hand``
      Attr.Create "viewBox" "0 0 100 100"
      Attr.Create "aria-hidden" "true"
      Attr.DynamicStyle
        "visibility"
        (context.Value.View
         |> View.Map(fun value -> if value.IsSome then "visible" else "hidden"))
    ] [
      Doc.SvgElement "line" [
        Attr.Create "x1" "50"
        Attr.Create "y1" "50"
        Attr.Dynamic "x2" (coordinate fst)
        Attr.Dynamic "y2" (coordinate snd)
      ] []
      Doc.SvgElement "circle" [ Attr.Create "cx" "50"; Attr.Create "cy" "50"; Attr.Create "r" "1.2" ] []
      Doc.SvgElement "circle" [
        Attr.Dynamic "cx" (coordinate fst)
        Attr.Dynamic "cy" (coordinate snd)
        Attr.Create "r" "6.8"
      ] []
    ]

  let private renderNumber phase context value =
    let x, y = getPosition phase context.Format value

    let selected =
      context.Value.View
      |> View.Map(fun current -> current.IsSome && getNumber phase context = value)

    let keyboardSelect (_: Dom.Element) (event: Dom.MouseEvent) =
      // Pointer selection is handled by the dial, including drags from a number.
      if event.Detail = 0 then
        value |> select phase context

    let display =
      match phase, context.Format with
      | Hours, ClockFormat.TwelveHour -> string<int> value
      | Hours, ClockFormat.TwentyFourHour
      | Minutes, ClockFormat.TwelveHour
      | Minutes, ClockFormat.TwentyFourHour -> sprintf "%02d" value

    let numberAttrs = [
      cl Css.``weave-time-picker__number``
      Attr.Style "left" (sprintf "%f%%" x)
      Attr.Style "top" (sprintf "%f%%" y)
    ]

    button [
      attr.``type`` "button"
      yield! numberAttrs
      Attr.Create "tabindex" "-1"
      Attr.Create
        "aria-label"
        (sprintf
          "%s %02d"
          (match phase with
           | Hours -> "Hour"
           | Minutes -> "Minute")
          value)
      Attr.DynamicClassPred Css.``weave-time-picker__number--selected`` selected
      PickerRender.boolAttr "aria-pressed" selected
      Attr.enabled (
        (context.Active, context.Value.View, context.Availability)
        |||> View.Map3(fun active _ availability ->
          active && (dialTime phase context availability value |> Option.isSome))
      )
      on.click keyboardSelect
    ] [ text display ]

  let private renderDial context phase =
    let minimum, maximum = getLimits phase context
    let selected = context.Value.View |> View.Map(fun _ -> getNumber phase context)

    let choices =
      match phase, context.Format with
      | Hours, ClockFormat.TwelveHour -> [ 1..12 ]
      | Hours, ClockFormat.TwentyFourHour -> [ 0..23 ]
      | Minutes, ClockFormat.TwelveHour
      | Minutes, ClockFormat.TwentyFourHour ->
        // Keep fine-grained dials legible while printing only selectable values.
        let labelStep =
          if context.MinuteStep < 5 then
            context.MinuteStep * 5
          else
            context.MinuteStep

        [ 0..labelStep..59 ]

    let focusSlider (element: Dom.Element) =
      let slider = element.QuerySelector("[role='slider']")

      let focused = As<Dom.Element>(JS.Document?activeElement)

      let typing =
        not (isNull focused)
        && not (isNull (focused.Closest(".weave-time-picker__toolbar input")))

      if not (isNull slider) && not typing then
        slider?focus ()

    div [
      cl Css.``weave-time-picker__clock``
      on.keyDown (handleKey phase context)
      on.afterRender (attachPointers phase context)
      on.afterRender focusSlider
    ] [
      div [
        cl Css.``weave-time-picker__dial``
        Attr.Create "role" "slider"
        Attr.Create "tabindex" "0"
        Attr.Create "data-picker-focus" ""
        Attr.Create
          "aria-label"
          (match phase with
           | Hours -> "Hours"
           | Minutes -> "Minutes")
        PickerRender.boolAttr
          "aria-disabled"
          ((context.Active, context.Value.View, context.Availability)
           |||> View.Map3(fun active _ availability ->
             not active || List.isEmpty (availableNumbers phase context availability)))
        Attr.Create "aria-valuemin" (string<int> minimum)
        Attr.Create "aria-valuemax" (string<int> maximum)
        Attr.Dynamic "aria-valuenow" (selected |> View.Map string<int>)
      ] []

      renderHand phase context

      div [ Attr.Create "role" "group"; Attr.Create "aria-label" "Clock numbers" ] [
        for value in choices do
          value |> renderNumber phase context
      ]
    ]

  let private renderTimeField phase context =
    let entry =
      if phase = Hours then
        context.Entries.Hour
      else
        context.Entries.Minute

    let field = entry.Field
    let edited = entry.Edited
    let label = if phase = Hours then "Hours" else "Minutes"

    let display = entry.Display
    let draft = field.Value
    resetEntry context.Value.Value entry
    let minimum, maximum = getLimits phase context

    let apply (element: Dom.Element) =
      let raw = As<string>(element?value)

      Var.Set draft raw
      context.Touch()
      field.Touch()

      match tryNumber minimum maximum raw with
      | Some number when List.isEmpty (entry.Rule raw) && canInteract context ->
        let time = timeWithNumber phase context number

        if context.Value.Value <> Some time then
          commit context time

        Var.Set draft (display context.Value.Value)
        Var.Set edited false
        true
      | _ -> false

    Doc.Concat [
      TextField.singleLine (
        draft,
        placeholder = View.Const "00",
        enabled = context.Active,
        typoAttrs = [],
        inputAttrs = [
          Attr.Create "aria-label" label
          Attr.Create "inputmode" "numeric"
          // Native length limit without TextField's optional character counter.
          Attr.Create "maxlength" "2"
          Attr.Create "pattern" "[0-9]{0,2}"
          Attr.Handler "beforeinput" (fun _ event ->
            let data = As<string>(event?data)

            if not (String.IsNullOrEmpty data) && not (data |> Seq.forall isAsciiDigit) then
              event.PreventDefault())
          Attr.Handler "paste" (fun _ event ->
            let data = As<string>(event?clipboardData?getData("text"))

            if not (String.IsNullOrEmpty data) && not (data |> Seq.forall isAsciiDigit) then
              event.PreventDefault())
          Attr.Create "autocomplete" "off"
          yield!
            ValidationInput.attrs {
              field.Validation with
                  Touch =
                    fun () ->
                      if edited.Value then
                        field.Touch()
            }
          on.focus (fun element _ ->
            synchronizeEntry context.Value.Value entry
            Var.Set context.Phase phase
            element?select ())
          on.input (fun element _ ->
            let raw = As<string>(element?value)

            let digits =
              raw
              |> Seq.filter isAsciiDigit
              |> Seq.truncate 2
              |> Seq.toArray
              |> fun chars -> System.String(chars)

            if raw <> digits then
              element?value <- digits

            Var.Set draft digits
            Var.Set entry.Revision context.ResetVersion.Value
            Var.Set edited true)
          on.blur (fun element _ ->
            if edited.Value then
              apply element |> ignore)
          on.keyDown (fun element event ->
            match event with
            | Key.Enter ->
              event.PreventDefault()

              if apply element then
                match phase with
                | Minutes -> ()
                | Hours ->
                  let minute =
                    element.Closest(".weave-time-picker").QuerySelector("input[aria-label='Minutes']")

                  if not (isNull minute) then
                    minute?focus ()
            | _ -> ())
        ],
        attrs = [
          TextField.Variant.standard
          cl Css.``weave-time-picker__value``
          Attr.Dynamic
            "data-active"
            (context.Phase.View
             |> View.Map(fun current -> if current = phase then "true" else "false"))
        ]
      )
    ]

  let private renderPeriod period context =
    let isPm = period = "PM"

    let change () =
      sampleAvailability context
      |> periodTime isPm context
      |> Option.iter (commit context)

    let enabled =
      (context.Active, context.Value.View, context.Availability)
      |||> View.Map3(fun active _ availability ->
        active && (periodTime isPm context availability |> Option.isSome))

    Button.create (
      text period,
      change,
      enabled = enabled,
      attrs = [
        Button.Variant.text
        cl Css.``weave-time-picker__period``
        PickerRender.boolAttr
          "aria-pressed"
          (context.Value.View |> View.Map(fun _ -> ((getTime context).Hours >= 12) = isPm))
      ]
    )

  let render context =
    let feedback (validation: FieldValidation) =
      FieldHelpText.create (
        Doc.TextView(validation.VisibleErrors |> View.Map(String.concat "\n")),
        attrs = [
          Attr.Create "id" validation.ErrorId
          Attr.Create "role" "status"
          FieldHelpText.Color.error
          Attr.Class "weave-time-picker__feedback"
          Attr.Style "white-space" "pre-line"
          Attr.DynamicStyle
            "display"
            (validation.VisibleErrors
             |> View.Map(fun errors -> if List.isEmpty errors then "none" else "block"))
        ]
      )

    div [ cl Css.``weave-time-picker`` ] [
      div [ cl Css.``weave-time-picker__toolbar`` ] [
        div [ cl Css.``weave-time-picker__time`` ] [
          renderTimeField Hours context
          span [ Attr.Class "weave-time-picker__separator"; Attr.Create "aria-hidden" "true" ] [ text ":" ]
          renderTimeField Minutes context
        ]

        match context.Format with
        | ClockFormat.TwelveHour ->
          div [ cl Css.``weave-time-picker__periods`` ] [
            renderPeriod "AM" context
            renderPeriod "PM" context
          ]
        | ClockFormat.TwentyFourHour -> Doc.Empty
      ]

      feedback context.Entries.Hour.Field.Validation
      feedback context.Entries.Minute.Field.Validation
      feedback {
        ErrorId = WeaveId.create "weave-time-availability"
        VisibleErrors = context.ValueErrors
        Touch = ignore
      }

      div [ cl Css.``weave-time-picker__status``; Attr.Create "role" "status" ] [
        context.Phase.View
        |> View.Map(function
          | Hours -> "Choose hour"
          | Minutes -> "Choose minutes")
        |> Doc.TextView
      ]

      context.Phase.View |> Doc.BindView(renderDial context)
    ]
