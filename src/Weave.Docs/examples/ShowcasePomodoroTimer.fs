namespace Weave.Docs.Examples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ShowcasePomodoroTimer =

  [<JavaScript; Struct>]
  type TimerPhase =
    | Work
    | ShortBreak
    | LongBreak

  [<JavaScript; Struct>]
  type TimerState =
    | Idle
    | Running
    | Paused

  let private phaseToString (p: TimerPhase) =
    match p with
    | Work -> "Work"
    | ShortBreak -> "Short Break"
    | LongBreak -> "Long Break"

  let private phaseToBackgroundColor (p: TimerPhase) =
    match p with
    | Work -> BrandColor.BackgroundColor.primary
    | ShortBreak -> BrandColor.BackgroundColor.success
    | LongBreak -> BrandColor.BackgroundColor.tertiary

  let private fullAppSection () =
    let phase = Var.Create Work
    let state = Var.Create Idle
    let workMinutes = Var.Create 25
    let shortBreakMinutes = Var.Create 5
    let longBreakMinutes = Var.Create 15
    let roundsBeforeLong = Var.Create 4
    let completedRounds = Var.Create 0
    let autoAdvance = Var.Create false

    let remainingSeconds = Var.Create(workMinutes.Value * 60)

    let mutable intervalId: JS.Handle option = None

    let getDurationForPhase (p: TimerPhase) =
      match p with
      | Work -> workMinutes.Value * 60
      | ShortBreak -> shortBreakMinutes.Value * 60
      | LongBreak -> longBreakMinutes.Value * 60

    let clearTimer () =
      intervalId |> Option.iter (fun id -> JS.ClearInterval id)
      intervalId <- None

    let switchPhase (newPhase: TimerPhase) =
      clearTimer ()
      Var.Set phase newPhase
      Var.Set remainingSeconds (getDurationForPhase newPhase)
      Var.Set state Idle

    let rec tick () =
      if state.Value = Running then
        let next = remainingSeconds.Value - 1

        if next <= 0 then
          clearTimer ()
          Var.Set remainingSeconds 0
          Var.Set state Idle

          // Phase complete logic
          match phase.Value with
          | Work ->
            Var.Update completedRounds (fun r -> r + 1)

            let nextPhase =
              if (completedRounds.Value) % roundsBeforeLong.Value = 0 then
                LongBreak
              else
                ShortBreak

            if autoAdvance.Value then
              switchPhase nextPhase
              startTimer ()
            else
              switchPhase nextPhase
          | ShortBreak
          | LongBreak ->
            if autoAdvance.Value then
              switchPhase Work
              startTimer ()
            else
              switchPhase Work
        else
          Var.Set remainingSeconds next

    and startTimer () =
      clearTimer ()
      Var.Set state Running
      let id = JS.SetInterval tick 1000
      intervalId <- Some id

    let pauseTimer () =
      clearTimer ()
      Var.Set state Paused

    let resetTimer () =
      clearTimer ()
      Var.Set state Idle
      Var.Set remainingSeconds (getDurationForPhase phase.Value)

    let formatTime (seconds: int) =
      let m = seconds / 60
      let s = seconds % 60
      sprintf "%02d:%02d" m s

    let description =
      Helpers.bodyText
        "The complete Pomodoro Timer. Start a work session, take breaks, and track your rounds — all driven by F# state machines."

    let content =
      div [] [
        // Phase indicator chips
        div [ Flex.Flex.allSizes; JustifyContent.center; Gap.All.g2; Margin.Bottom.small ] [
          for p in [ Work; ShortBreak; LongBreak ] do
            phase.View
            |> Doc.BindView(fun currentPhase ->
              Chip.create (
                text (phaseToString p),
                attrs = [
                  if p = currentPhase then
                    Chip.Variant.filled
                    phaseToBackgroundColor p
                  else
                    Chip.Variant.outlined
                ]
              ))
        ]

        // Large countdown display
        div [
          Flex.Flex.allSizes
          JustifyContent.center
          AlignItems.center
          Margin.Bottom.small
        ] [
          div [
            Typography.h1
            Typography.Family.mono
            Typography.Align.center
            Attr.Style "font-size" "5rem"
            Attr.Style "line-height" "1.1"
          ] [ textView (remainingSeconds.View |> View.MapCached formatTime) ]
        ]

        // Rounds counter
        div [ Flex.Flex.allSizes; JustifyContent.center; Margin.Bottom.small ] [
          div [ Typography.body2; Typography.Color.textSecondary ] [
            textView (
              completedRounds.View
              |> View.MapCached(fun r -> sprintf "Rounds completed: %d" r)
            )
          ]
        ]

        // Control buttons
        div [ Flex.Flex.allSizes; JustifyContent.center; Gap.All.g2; Margin.Bottom.small ] [
          state.View
          |> Doc.BindView(fun s ->
            match s with
            | Idle ->
              Button.create (
                div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                  Icon.create (Icon.AudioVideo AudioVideo.PlayArrow)
                  text "Start"
                ],
                onClick = startTimer,
                attrs = [ Button.Color.primary; Button.Variant.filled ]
              )
            | Running ->
              Button.create (
                div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                  Icon.create (Icon.AudioVideo AudioVideo.Pause)
                  text "Pause"
                ],
                onClick = pauseTimer,
                attrs = [ Button.Color.warning; Button.Variant.filled ]
              )
            | Paused ->
              Button.create (
                div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                  Icon.create (Icon.AudioVideo AudioVideo.PlayArrow)
                  text "Resume"
                ],
                onClick = startTimer,
                attrs = [ Button.Color.primary; Button.Variant.filled ]
              ))

          Button.create (
            div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
              Icon.create (Icon.AudioVideo AudioVideo.Replay)
              text "Reset"
            ],
            onClick = resetTimer,
            attrs = [ Button.Variant.outlined ]
          )
        ]

        Divider.create (attrs = [ Margin.Vertical.small ])

        // Settings panel
        let settingsExpanded = Var.Create false

        let settingsHeader =
          ExpansionPanelHeader.create (
            content =
              div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
                Icon.create (Icon.Action Action.Build, attrs = [ Attr.Style "font-size" "18px" ])
                div [ Typography.subtitle2 ] [ text "Settings" ]
              ],
            expanded = settingsExpanded,
            attrs = [ ExpansionPanel.Color.primary ]
          )

        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = settingsHeader,
              content =
                ExpansionPanelContent.create (
                  Grid.create (
                    [
                      GridItem.create (
                        Slider.primary (
                          workMinutes,
                          min = 1,
                          max = 60,
                          step = 1,
                          labelText = (workMinutes.View |> View.MapCached(sprintf "Work duration: %d min"))
                        ),
                        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
                      )

                      GridItem.create (
                        Slider.success (
                          shortBreakMinutes,
                          min = 1,
                          max = 30,
                          step = 1,
                          labelText =
                            (shortBreakMinutes.View |> View.MapCached(sprintf "Short break: %d min"))
                        ),
                        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
                      )

                      GridItem.create (
                        Slider.tertiary (
                          longBreakMinutes,
                          min = 1,
                          max = 60,
                          step = 1,
                          labelText = (longBreakMinutes.View |> View.MapCached(sprintf "Long break: %d min"))
                        ),
                        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
                      )

                      GridItem.create (
                        NumericField.create (
                          roundsBeforeLong,
                          labelText = View.Const "Rounds before long break",
                          min = 1,
                          max = 10,
                          attrs = [ NumericField.Color.primary; NumericField.Width.full ]
                        ),
                        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
                      )

                      GridItem.create (
                        Switch.create (
                          autoAdvance,
                          div [ Typography.body2 ] [ text "Auto-advance to next phase" ],
                          attrs = [ Switch.Color.primary ]
                        ),
                        attrs = [ GridItem.Span.twelve ]
                      )
                    ]
                  )
                ),
              expanded = settingsExpanded
            )
          ]
        )
      ]

    Helpers.section "The Full App" description content

  let private stateModelSection () =
    let description =
      Helpers.bodyText
        "The timer is modeled with two discriminated unions — TimerPhase for what kind of session it is, and TimerState for whether the timer is running. Pattern matching drives all the UI logic: button labels, colors, and phase transitions."

    let content = Doc.Empty

    let code =
      """[<Struct>]
type TimerPhase =
    | Work
    | ShortBreak
    | LongBreak

[<Struct>]
type TimerState =
    | Idle
    | Running
    | Paused

let phaseToBackgroundColor (p: TimerPhase) =
    match p with
    | Work -> BrandColor.BackgroundColor.primary
    | ShortBreak -> BrandColor.BackgroundColor.success
    | LongBreak -> BrandColor.BackgroundColor.tertiary"""

    Helpers.codeSampleSection "Timer State Machine" description content code

  let private countdownSection () =
    let description =
      Helpers.bodyText
        "The countdown is a single Var<int> representing seconds remaining. View.MapCached formats it as MM:SS reactively — no polling, no imperative updates. The timer itself uses JS.SetInterval for the tick."

    let content = Doc.Empty

    let code =
      """let remainingSeconds = Var.Create (25 * 60)
let mutable intervalId: int option = None

let formatTime (seconds: int) =
    let m = seconds / 60
    let s = seconds % 60
    sprintf "%02d:%02d" m s

// Large reactive countdown display
div [
    Typography.h1
    Typography.Family.mono
    Attr.Style "font-size" "5rem"
] [
    textView (remainingSeconds.View |> View.MapCached formatTime)
]

// Timer tick — one-second interval
let startTimer () =
    let id = JS.SetInterval (fun () ->
        if state.Value = Running then
            let next = remainingSeconds.Value - 1
            if next <= 0 then
                // Phase complete — transition to next
                clearTimer ()
                Var.Set remainingSeconds 0
                Var.Set state Idle
            else
                Var.Set remainingSeconds next
    ) 1000
    intervalId <- Some id"""

    Helpers.codeSampleSection "Countdown Display" description content code

  let private controlsSection () =
    let description =
      Helpers.bodyText
        "The control buttons use Doc.BindView to reactively switch between Start, Pause, and Resume based on the current TimerState. Each state gets a different icon, label, and color — all via pattern matching."

    let content = Doc.Empty

    let code =
      """// Reactive control buttons — switch based on state
state.View
|> Doc.BindView(fun s ->
    match s with
    | Idle ->
        Button.create(
            div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                Icon.create(Icon.AudioVideo AudioVideo.PlayArrow)
                text "Start"
            ],
            onClick = startTimer,
            attrs = [ Button.Color.primary; Button.Variant.filled ]
        )
    | Running ->
        Button.create(
            div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                Icon.create(Icon.AudioVideo AudioVideo.Pause)
                text "Pause"
            ],
            onClick = pauseTimer,
            attrs = [ Button.Color.warning; Button.Variant.filled ]
        )
    | Paused ->
        Button.create(
            div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                Icon.create(Icon.AudioVideo AudioVideo.PlayArrow)
                text "Resume"
            ],
            onClick = startTimer,
            attrs = [ Button.Color.primary; Button.Variant.filled ]
        ))"""

    Helpers.codeSampleSection "Controls" description content code

  let private settingsSection () =
    let description =
      Helpers.bodyText
        "Settings use Sliders for durations, a NumericField for rounds, and a Switch for auto-advance. Everything is wrapped in an ExpansionPanel for a clean, collapsible layout. Changes take effect on the next timer reset."

    let content = Doc.Empty

    let code =
      """let workMinutes = Var.Create 25
let shortBreakMinutes = Var.Create 5
let longBreakMinutes = Var.Create 15
let roundsBeforeLong = Var.Create 4
let autoAdvance = Var.Create false

// Settings in an ExpansionPanel
Grid.create([
    GridItem.create(
        Slider.primary(
            workMinutes,
            min = 1, max = 60, step = 1,
            labelText = (workMinutes.View
                |> View.MapCached(sprintf "Work duration: %d min"))
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
    )
    GridItem.create(
        Slider.success(
            shortBreakMinutes,
            min = 1, max = 30, step = 1,
            labelText = (shortBreakMinutes.View
                |> View.MapCached(sprintf "Short break: %d min"))
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
    )
    GridItem.create(
        NumericField.create(
            roundsBeforeLong,
            labelText = View.Const "Rounds before long break",
            min = 1, max = 10,
            attrs = [ NumericField.Color.primary; NumericField.Width.full ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
    )
    GridItem.create(
        Switch.create(
            autoAdvance,
            div [ Typography.body2 ] [ text "Auto-advance" ],
            attrs = [ Switch.Color.primary ]
        ),
        attrs = [ GridItem.Span.twelve ]
    )
])"""

    Helpers.codeSampleSection "Settings Panel" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Pomodoro Timer"

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "A visual focus timer with work/break cycles and settings. This showcase demonstrates F# state machines with discriminated unions, reactive countdown logic, and composing multiple Weave components into a polished interactive app."
        ]

        Helpers.divider ()
        fullAppSection ()
        Helpers.divider ()
        stateModelSection ()
        Helpers.divider ()
        countdownSection ()
        Helpers.divider ()
        controlsSection ()
        Helpers.divider ()
        settingsSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
