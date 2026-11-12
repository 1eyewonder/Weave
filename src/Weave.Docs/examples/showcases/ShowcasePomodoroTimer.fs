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

  let private phaseToChipColor (p: TimerPhase) =
    match p with
    | Work -> Chip.Color.primary
    | ShortBreak -> Chip.Color.success
    | LongBreak -> Chip.Color.tertiary

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

    let stopOnRemoval element =
      let removal = DomRemovalListener.create ()
      removal.Watch(element, clearTimer)

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
      div [ on.afterRender stopOnRemoval ] [
        // Phase indicator chips
        div [
          Flex.Flex.allSizes
          JustifyContent.center
          FlexWrap.Wrap.allSizes
          Gap.All.g2
          Margin.Bottom.small
        ] [
          for p in [ Work; ShortBreak; LongBreak ] do
            phase.View
            |> Doc.BindView(fun currentPhase ->
              Chip.create (
                text (phaseToString p),
                attrs = [
                  if p = currentPhase then
                    Chip.Variant.filled
                    phaseToChipColor p
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
            Attr.Style "font-variant-numeric" "tabular-nums"
            Typography.Align.center
            Attr.Style "font-size" "clamp(2.5rem, 12vw, 5rem)"
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
        div [
          Flex.Flex.allSizes
          JustifyContent.center
          FlexWrap.Wrap.allSizes
          Gap.All.g2
          Margin.Bottom.small
        ] [
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

  let private stateSource =
    """[<Struct>]
type TimerState =
  | Idle
  | Running
  | Paused"""

  let private settingsSource =
    """let workMinutes = Var.Create 25

let settings =
  Slider.primary (
    workMinutes,
    min = 1,
    max = 60,
    labelText =
      (workMinutes.View |> View.MapCached(sprintf "Work duration: %d min"))
  )"""

  let private countdownSource =
    """open WebSharper.JavaScript

let state = Var.Create Idle
let remainingSeconds = Var.Create(workMinutes.Value * 60)
let mutable intervalId: JS.Handle option = None

let clearTimer () =
  intervalId |> Option.iter JS.ClearInterval
  intervalId <- None

let stopOnRemoval element =
  let removal = DomRemovalListener.create ()
  removal.Watch(element, clearTimer)

let resetTimer () =
  clearTimer ()
  Var.Set state Idle
  Var.Set remainingSeconds (workMinutes.Value * 60)

let tick () =
  let next = max 0 (remainingSeconds.Value - 1)
  Var.Set remainingSeconds next

  if next = 0 then
    clearTimer ()
    Var.Set state Idle

let startTimer () =
  clearTimer ()

  if remainingSeconds.Value = 0 then
    Var.Set remainingSeconds (workMinutes.Value * 60)

  Var.Set state Running
  intervalId <- Some(JS.SetInterval tick 1000)

let pauseTimer () =
  clearTimer ()
  Var.Set state Paused

let formatTime seconds =
  sprintf "%02d:%02d" (seconds / 60) (seconds % 60)

let countdown =
  div [ Typography.h1; Typography.Family.mono ] [
    textView (remainingSeconds.View |> View.MapCached formatTime)
  ]"""

  let private controlsSource =
    """let toggleTimer () =
  match state.Value with
  | Running -> pauseTimer ()
  | Idle
  | Paused -> startTimer ()

let controlLabel =
  state.View
  |> View.MapCached(fun current ->
    match current with
    | Idle -> "Start"
    | Running -> "Pause"
    | Paused -> "Resume")

let controls =
  div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g3 ] [
    Button.primary (
      textView controlLabel,
      onClick = toggleTimer,
      attrs = [ Button.Variant.filled ]
    )
    Button.create (
      text "Reset",
      onClick = resetTimer,
      attrs = [ Button.Variant.outlined ]
    )
  ]

div [
  Flex.Flex.allSizes
  FlexDirection.Column.allSizes
  Gap.All.g3
  on.afterRender stopOnRemoval
] [
  // Changing the duration resets the current session.
  Doc.sinkCached (fun _ -> resetTimer ()) workMinutes.View
  settings
  countdown
  controls
]"""

  // These are the exact displayed blocks, in their learning order.
  let private sourceParts = [ stateSource; settingsSource; countdownSource; controlsSource ]

  let private combinedSource = String.concat "\n\n" sourceParts

  let private sourceSections () =
    div [] [

      Helpers.codeSection
        "fsharp"
        "State Model"
        (Helpers.bodyText
          "Start here. This smaller example runs one work session. The full demo above adds work and break phases, round counting, and automatic phase changes.")
        stateSource

      Helpers.codeSection
        "fsharp"
        "Duration Setting"
        (Helpers.bodyText
          "Continues State Model. Define workMinutes and a labelled Slider. Changing this setting will reset the session in the final composition.")
        settingsSource

      Helpers.codeSection
        "fsharp"
        "Countdown Logic"
        (Helpers.bodyText
          "Continues State Model and Duration Setting. Keep one interval handle, clear it on pause, reset, or DOM removal, and stop at zero. Starting from zero begins a new session. The removal listener prevents an orphan interval after navigation.")
        countdownSource

      Helpers.codeSection
        "fsharp"
        "Controls & Composition"
        (Helpers.bodyText
          "Continues all previous sections. Update a stable button label so Start, Pause, and Resume keep keyboard focus. Return the settings, countdown, and actions as one Doc.")
        controlsSource

      Helpers.codeSection
        "fsharp"
        "Combined example"
        (Helpers.bodyText
          "Copy the same sections in order. Use the application setup in Getting Started and paste the source into its JavaScript module. The final expression returns a complete single-session timer; phase switching belongs to the richer demo above.")
        combinedSource
    ]

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
        Helpers.bodyText
          "Build the single-session timer from top to bottom. Each section continues the preceding sections; Combined example copies the same sequence in one block."
        sourceSections ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
