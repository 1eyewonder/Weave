namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module EventHandlersExamples =

  let private nativeButtonSection () =
    let count = Var.Create 0
    let enabled = Var.Create true

    let description =
      Helpers.bodyText
        "Use a native button for actions. It supports mouse, touch, keyboard, and programmatic activation; enabled controls its disabled state."

    let content =
      div [] [
        Switch.create (enabled, content = text "Enabled")
        Button.create (text "Activate", (fun () -> Var.Update count (fun n -> n + 1)), enabled = enabled.View)
        div [] [ text "Count: "; textView (count.View |> View.Map string) ]
      ]

    let code =
      """let count = Var.Create 0
let enabled = Var.Create true

Button.create (
  text "Activate",
  (fun () -> Var.Update count (fun n -> n + 1)),
  enabled = enabled.View
)"""

    Helpers.codeSampleSection "Native button activation" description content code

  let private focusSection () =
    let description =
      Helpers.bodyText
        "on.focusIn and on.focusOut fire when an element or any of its descendants gain or lose focus. Unlike the native focus event, these bubble — making them useful for tracking focus within a container."

    let content =
      let isFocused = Var.Create false

      div [] [
        div [
          SurfaceColor.BackgroundColor.surface
          BorderRadius.All.small
          Padding.All.medium
          on.focusIn (fun _ _ -> Var.Set isFocused true) // see here
          on.focusOut (fun _ _ -> Var.Set isFocused false)
          isFocused.View
          |> View.Map(fun f -> if f then "2px solid var(--palette-primary)" else "none")
          |> Attr.DynamicStyle "outline"
          Attr.Style "outline-offset" "2px"
        ] [
          div [ Margin.Bottom.small; Typography.body2 ] [
            text "Focus state: "
            textView (isFocused.View |> View.Map(fun f -> if f then "Focused" else "Not focused"))
          ]
          TextField.singleLine (Var.Create "", labelText = View.Const "Tab into me")
        ]
      ]

    let code =
      """let isFocused = Var.Create false

div [
  on.focusIn (fun el ev -> Var.Set isFocused true)   // see here
  on.focusOut (fun el ev -> Var.Set isFocused false)
] [
  TextField.singleLine (Var.Create "", labelText = View.Const "Tab into me")
]"""

    Helpers.codeSampleSection "on.focusIn / on.focusOut" description content code

  let private boundedClampSection () =
    let description =
      Helpers.bodyText
        "Bounded.clamp restricts a value to a [min, max] range. Bounded.stepUp and stepDown increment or decrement by a step size, always staying within bounds."

    let content =
      let value = Var.Create 50.0

      div [] [
        div [ Flex.Flex.allSizes; Gap.All.g4; AlignItems.center ] [
          Button.create (
            text "\u2212 10",
            onClick = (fun () -> Var.Update value (Bounded.stepDown 0.0 100.0 10.0)), // see here
            attrs = [ Button.Variant.outlined ]
          )
          div [ Typography.h5; Attr.Style "min-width" "80px"; Typography.Align.center ] [
            textView (value.View |> View.Map(fun v -> sprintf "%.0f" v))
          ]
          Button.create (
            text "+ 10",
            onClick = (fun () -> Var.Update value (Bounded.stepUp 0.0 100.0 10.0)),
            attrs = [ Button.Variant.outlined ]
          )
        ]
        div [ Margin.Top.small; Typography.body2; Opacity.sixty ] [ text "Range: 0\u2013100, step: 10" ]
      ]

    let code =
      """let value = Var.Create 50.0

// Decrement by 10, clamped to [0, 100]
Button.create (
  text "\u2212 10",
  onClick = (fun () ->
    Var.Update value (Bounded.stepDown 0.0 100.0 10.0))  // see here
)

// Increment by 10, clamped to [0, 100]
Button.create (
  text "+ 10",
  onClick = (fun () ->
    Var.Update value (Bounded.stepUp 0.0 100.0 10.0))
)"""

    Helpers.codeSampleSection "Bounded.clamp / stepUp / stepDown" description content code

  let private boundedPercentSection () =
    let description =
      Helpers.bodyText
        "Bounded.percentOf converts a value to a percentage within a range. Bounded.snapToStep rounds to the nearest step grid point."

    let content =
      let raw = Var.Create 37

      div [] [
        Slider.create (raw, min = 0, max = 100, labelText = View.Const "Raw value")
        div [ Margin.Top.small; Typography.body1 ] [
          textView (
            raw.View
            |> View.Map(fun v ->
              let fv = float v

              sprintf
                "Raw: %d | Percent: %.0f%% | Snapped to 5: %.0f"
                v
                (Bounded.percentOf 0.0 100.0 fv) // see here
                (Bounded.snapToStep 0.0 100.0 5.0 fv))
          )
        ]
      ]

    let code =
      """let raw = Var.Create 37

// Convert to percentage within [0, 100]
Bounded.percentOf 0.0 100.0 37.0  // see here — returns 37.0

// Snap to nearest step of 5 within [0, 100]
Bounded.snapToStep 0.0 100.0 5.0 37.0  // returns 35.0"""

    Helpers.codeSampleSection "Bounded.percentOf / snapToStep" description content code

  let private keyboardPatternsSection () =
    let description =
      Helpers.bodyText
        "Weave uses internal active patterns for component keyboard navigation. For an action, prefer a native button and its built-in activation behavior."

    let content =
      div [ Typography.body2 ] [
        p [] [ text "Available internal active patterns for keyboard events:" ]
        ul [] [
          li [] [ Helpers.inlineCode "Key.Enter"; text " \u2014 matches the Enter key" ]
          li [] [ Helpers.inlineCode "Key.Space"; text " \u2014 matches the Space key" ]
          li [] [
            Helpers.inlineCode "Key.Activate"
            text " \u2014 matches Enter or Space (standard ARIA activation)"
          ]
          li [] [ Helpers.inlineCode "Key.Escape"; text " \u2014 matches the Escape key" ]
          li [] [ Helpers.inlineCode "Key.Tab"; text " \u2014 matches the Tab key" ]
          li [] [
            Helpers.inlineCode "Key.ArrowUp / ArrowDown / ArrowLeft / ArrowRight"
            text " \u2014 directional navigation"
          ]
          li [] [
            Helpers.inlineCode "Key.Home / Key.End"
            text " \u2014 jump to first/last item"
          ]
          li [] [
            Helpers.inlineCode "Key.NavKey"
            text " \u2014 parameterized match against any key string"
          ]
        ]
        p [] [
          text
            "These patterns are internal to Weave and used by components. Prefer a native button for actions so the browser handles activation."
        ]
      ]

    let code =
      """// Weave's internal Key patterns match on ev.Key. Your own handlers can do the same:
let menuKeys =
  on.keyDown (fun el ev ->
    match ev.Key with
    | "Enter" | " " ->
      ev.PreventDefault()
      // Handle Enter or Space
    | "Escape" ->
      // Close menu/dialog
      ()
    | "ArrowDown" ->
      ev.PreventDefault()
      // Move focus to next item
    | _ -> ()
  )

// For an action, use a native button:
Button.create (text "Activate", (fun () -> ()))"""

    Helpers.codeSampleSection "Keyboard Active Patterns" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Event Handlers"
        Helpers.bodyText
          "Weave provides accessible event handlers, numeric utilities, and keyboard active patterns for building interactive components beyond the standard library."
        Helpers.divider ()
        nativeButtonSection ()
        Helpers.divider ()
        focusSection ()
        Helpers.divider ()
        boundedClampSection ()
        Helpers.divider ()
        boundedPercentSection ()
        Helpers.divider ()
        keyboardPatternsSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
