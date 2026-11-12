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
      div [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        AlignItems.center
        JustifyContent.center
        Gap.All.g4
      ] [
        Switch.create (enabled, content = text "Enabled")
        Button.create (text "Activate", (fun () -> Var.Update count (fun n -> n + 1)), enabled = enabled.View)
        div [] [ text "Count: "; textView (count.View |> View.Map string) ]
      ]

    let code =
      """let count = Var.Create 0
let enabled = Var.Create true

Doc.Concat [
  Switch.create (enabled, content = text "Enabled")

  Button.create (
    text "Activate",
    (fun () -> Var.Update count (fun n -> n + 1)),
    enabled = enabled.View
  )

  textView (count.View |> View.Map(sprintf "Count: %d"))
]"""

    Helpers.codeSampleSection "Native button activation" description content code

  let private focusSection () =
    let description =
      Helpers.bodyText
        "on.focusIn and on.focusOut fire when an element or any of its descendants gain or lose focus. Unlike the native focus event, these bubble — making them useful for tracking focus within a container."

    let content =
      let isFocused = Var.Create false

      div [
        Attr.Style "inline-size" "fit-content"
        Attr.Style "max-inline-size" "min(32rem, 100%)"
        Attr.Style "margin-inline" "auto"
      ] [
        div [
          SurfaceColor.BackgroundColor.surface
          BorderRadius.All.small
          Padding.All.extraSmall
          on.focusIn (fun _ _ -> Var.Set isFocused true)
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
  SurfaceColor.BackgroundColor.surface
  BorderRadius.All.small
  Padding.All.extraSmall
  on.focusIn (fun _ _ -> Var.Set isFocused true)
  on.focusOut (fun _ _ -> Var.Set isFocused false)
  isFocused.View
  |> View.Map(fun f ->
    if f then "2px solid var(--palette-primary)" else "none")
  |> Attr.DynamicStyle "outline"
  Attr.Style "outline-offset" "2px"
] [
  div [ Margin.Bottom.small; Typography.body2 ] [
    text "Focus state: "
    textView (
      isFocused.View
      |> View.Map(fun f -> if f then "Focused" else "Not focused")
    )
  ]
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
        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Gap.All.g3
          AlignItems.center
          JustifyContent.center
        ] [
          Button.create (
            text "\u2212 10",
            onClick = (fun () -> Var.Update value (Bounded.stepDown 0.0 100.0 10.0)),
            attrs = [ Button.Variant.outlined ]
          )
          div [ Typography.h5; Attr.Style "min-width" "3ch"; Typography.Align.center ] [
            textView (value.View |> View.Map(fun v -> sprintf "%.0f" v))
          ]
          Button.create (
            text "+ 10",
            onClick = (fun () -> Var.Update value (Bounded.stepUp 0.0 100.0 10.0)),
            attrs = [ Button.Variant.outlined ]
          )
        ]
        div [
          Margin.Top.small
          Typography.body2
          Typography.Color.textSecondary
          Typography.Align.center
        ] [ text "Range: 0\u2013100, step: 10" ]
      ]

    let code =
      """let value = Var.Create 50.0

Doc.Concat [
  Button.create (
    text "\u2212 10",
    onClick = (fun () -> Var.Update value (Bounded.stepDown 0.0 100.0 10.0)),
    attrs = [ Button.Variant.outlined ]
  )

  textView (value.View |> View.Map(fun v -> sprintf "%.0f" v))

  Button.create (
    text "+ 10",
    onClick = (fun () -> Var.Update value (Bounded.stepUp 0.0 100.0 10.0)),
    attrs = [ Button.Variant.outlined ]
  )
]"""

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
                (Bounded.percentOf 0.0 100.0 fv)
                (Bounded.snapToStep 0.0 100.0 5.0 fv))
          )
        ]
      ]

    let code =
      """let raw = Var.Create 37

Doc.Concat [
  Slider.create (raw, min = 0, max = 100, labelText = View.Const "Raw value")

  textView (
    raw.View
    |> View.Map(fun v ->
      let fv = float v

      sprintf
        "Raw: %d | Percent: %.0f%% | Snapped to 5: %.0f"
        v
        (Bounded.percentOf 0.0 100.0 fv)
        (Bounded.snapToStep 0.0 100.0 5.0 fv))
  )
]"""

    Helpers.codeSampleSection "Bounded.percentOf / snapToStep" description content code

  let private keyboardPatternsSection () =
    let content =
      let search = Var.Create ""

      let clearOnEscape =
        on.keyDown (fun _ event ->
          if event.Key = "Escape" then
            event.PreventDefault()
            Var.Set search "")

      TextField.singleLine (
        search,
        labelText = View.Const "Press Escape to clear",
        inputAttrs = [ clearOnEscape ]
      )

    Helpers.codeSampleSection
      "Keyboard Active Patterns"
      (Helpers.bodyText
        "Weave uses internal active patterns for keyboard navigation. In application code, match event.Key for custom behavior. Try Escape to clear this field; use native buttons for Enter and Space activation.")
      (div [ Flex.Flex.allSizes; JustifyContent.center ] [ content ])
      """let search = Var.Create ""

let clearOnEscape =
  on.keyDown (fun _ event ->
    if event.Key = "Escape" then
      event.PreventDefault()
      Var.Set search "")

TextField.singleLine (
  search,
  labelText = View.Const "Press Escape to clear",
  inputAttrs = [ clearOnEscape ]
)"""

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
