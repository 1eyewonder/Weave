namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers.Animation

[<JavaScript>]
module AnimationExamples =

  let private inlineCode (value: string) =
    Caption.Span(value, attrs = [ Typography.Color.toClass BrandColor.Primary |> cl ])

  let private tableCell (children: Doc list) =
    td [ Attr.Style "padding" "8px 12px"; Attr.Style "white-space" "nowrap" ] children

  let private tableHeaderCell (label: string) =
    th [
      Attr.Style "text-align" "left"
      Attr.Style "padding" "8px 12px"
      Attr.Style "white-space" "nowrap"
      Attr.Style "border-bottom" "1px solid var(--palette-divider)"
    ] [ text label ]

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Animations are built from four composable axes: what plays (kind), how long (duration), what curve (easing), and when it triggers (AnimationOn). All four work the same way — each is a DU with a toClass function, and you compose them via cls in attrs. AnimationOn is CSS-only (hover/focus); for JS-driven triggers like click or timer, see the Animate module below."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [ tableHeaderCell "Axis"; tableHeaderCell "Purpose"; tableHeaderCell "Example" ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "AnimationEntrance" ]
              tableCell [ text "Elements appearing" ]
              tableCell [ inlineCode "AnimationEntrance.toClass AnimationEntrance.FadeIn" ]
            ]
            tr [] [
              tableCell [ text "AnimationExit" ]
              tableCell [ text "Elements leaving" ]
              tableCell [ inlineCode "AnimationExit.toClass AnimationExit.FadeOut" ]
            ]
            tr [] [
              tableCell [ text "AnimationEmphasis" ]
              tableCell [ text "Drawing attention" ]
              tableCell [ inlineCode "AnimationEmphasis.toClass AnimationEmphasis.Pulse" ]
            ]
            tr [] [
              tableCell [ text "AnimationDuration" ]
              tableCell [ text "Override duration" ]
              tableCell [ inlineCode "AnimationDuration.toClass AnimationDuration.Short" ]
            ]
            tr [] [
              tableCell [ text "AnimationEasing" ]
              tableCell [ text "Override easing curve" ]
              tableCell [ inlineCode "AnimationEasing.toClass AnimationEasing.Bounce" ]
            ]
            tr [] [
              tableCell [ text "AnimationOn" ]
              tableCell [ text "CSS-only trigger (hover/focus)" ]
              tableCell [ inlineCode "AnimationOn.toClass AnimationOn.Hover" ]
            ]
            tr [] [
              tableCell [ text "AnimationKind" ]
              tableCell [ text "Wrapper for any animation" ]
              tableCell [
                inlineCode "AnimationKind.toClass (AnimationKind.Entrance AnimationEntrance.FadeIn)"
              ]
            ]
          ]
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Apply an entrance animation — plays once on mount by default
div [ cl (AnimationEntrance.toClass AnimationEntrance.FadeIn) ] [
    text "I fade in when rendered"
]

// Compose all four axes: kind + duration + easing + trigger
div [
    cls [
        AnimationEmphasis.toClass AnimationEmphasis.Bounce
        AnimationDuration.toClass AnimationDuration.Short
        AnimationEasing.toClass AnimationEasing.Bounce
        AnimationOn.toClass AnimationOn.Hover  // see here — CSS-only trigger
    ]
] [
    text "Bounces on hover"
]

// AnimationOn is optional — without it, the animation plays on mount
div [
    cls [
        AnimationEntrance.toClass AnimationEntrance.SlideUpIn
        AnimationDuration.toClass AnimationDuration.Long
        AnimationEasing.toClass AnimationEasing.Bounce
    ]
] [
    text "Slides up slowly with a bounce"
]"""

    Helpers.codeSampleSection "How it works" description content code

  // ---------------------------------------------------------------------------
  // 2. Playground — interactive picker for animation / duration / easing
  // ---------------------------------------------------------------------------

  let private playgroundSection () =
    let description =
      Helpers.bodyText
        "Pick an animation, duration, and easing to preview the effect. Click \"Play\" to replay."

    let allAnimations = [
      "Entrance: Fade In", AnimationKind.Entrance AnimationEntrance.FadeIn
      "Entrance: Scale In", AnimationKind.Entrance AnimationEntrance.ScaleIn
      "Entrance: Scale Y In", AnimationKind.Entrance AnimationEntrance.ScaleYIn
      "Entrance: Slide Up", AnimationKind.Entrance AnimationEntrance.SlideUpIn
      "Entrance: Slide Down", AnimationKind.Entrance AnimationEntrance.SlideDownIn
      "Entrance: Slide Left", AnimationKind.Entrance AnimationEntrance.SlideLeftIn
      "Entrance: Slide Right", AnimationKind.Entrance AnimationEntrance.SlideRightIn
      "Exit: Fade Out", AnimationKind.Exit AnimationExit.FadeOut
      "Exit: Scale Out", AnimationKind.Exit AnimationExit.ScaleOut
      "Exit: Scale Y Out", AnimationKind.Exit AnimationExit.ScaleYOut
      "Exit: Slide Up", AnimationKind.Exit AnimationExit.SlideUpOut
      "Exit: Slide Down", AnimationKind.Exit AnimationExit.SlideDownOut
      "Exit: Slide Left", AnimationKind.Exit AnimationExit.SlideLeftOut
      "Exit: Slide Right", AnimationKind.Exit AnimationExit.SlideRightOut
      "Emphasis: Pulse", AnimationKind.Emphasis AnimationEmphasis.Pulse
      "Emphasis: Shake", AnimationKind.Emphasis AnimationEmphasis.Shake
      "Emphasis: Bounce", AnimationKind.Emphasis AnimationEmphasis.Bounce
    ]

    let allDurations = [
      "Shortest (100ms)", Some AnimationDuration.Shortest
      "Shorter (200ms)", Some AnimationDuration.Shorter
      "Short (300ms)", Some AnimationDuration.Short
      "Standard (400ms)", Some AnimationDuration.Standard
      "Medium (500ms)", Some AnimationDuration.Medium
      "Long (600ms)", Some AnimationDuration.Long
      "Longer (800ms)", Some AnimationDuration.Longer
      "Longest (1200ms)", Some AnimationDuration.Longest
    ]

    let allEasings = [
      "Standard", Some AnimationEasing.Standard
      "Decelerate", Some AnimationEasing.Decelerate
      "Accelerate", Some AnimationEasing.Accelerate
      "Bounce", Some AnimationEasing.Bounce
    ]

    let selectedAnimation = Var.Create<string option>(Some "Entrance: Fade In")
    let selectedDuration = Var.Create<string option>(Some "Standard (400ms)")
    let selectedEasing = Var.Create<string option>(Some "Decelerate")
    let replayKey = Var.Create 0

    let animationItems =
      allAnimations
      |> List.map (fun (label, _) -> Select.SelectItemDef.create (text label) label label)
      |> View.Const

    let durationItems =
      allDurations
      |> List.map (fun (label, _) -> Select.SelectItemDef.create (text label) label label)
      |> View.Const

    let easingItems =
      allEasings
      |> List.map (fun (label, _) -> Select.SelectItemDef.create (text label) label label)
      |> View.Const

    let content =
      div [] [
        Grid.Create(
          [
            GridItem.Create(
              Select.Create(
                animationItems,
                selectedAnimation,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Animation",
                placeholder = View.Const "Choose...",
                attrs = [ cls [ Select.Color.toClass BrandColor.Primary ] ]
              ),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 4
            )
            GridItem.Create(
              Select.Create(
                durationItems,
                selectedDuration,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Duration",
                placeholder = View.Const "Default",
                attrs = [ cls [ Select.Color.toClass BrandColor.Primary ] ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
            GridItem.Create(
              Select.Create(
                easingItems,
                selectedEasing,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Easing",
                placeholder = View.Const "Default",
                attrs = [ cls [ Select.Color.toClass BrandColor.Primary ] ]
              ),
              xs = Grid.Width.create 6,
              sm = Grid.Width.create 4
            )
          ]
        )

        div [ Margin.toClasses Margin.Vertical.small |> cls ] [
          Button.Create(
            text "Play",
            onClick = (fun () -> replayKey.Value <- replayKey.Value + 1),
            attrs = [ Button.Variant.filled; Button.Color.primary; Button.Width.full ]
          )
        ]

        View.Map3
          (fun animSel durSel easSel -> animSel, durSel, easSel)
          selectedAnimation.View
          selectedDuration.View
          selectedEasing.View
        |> View.Map2 (fun key (animSel, durSel, easSel) -> key, animSel, durSel, easSel) replayKey.View
        |> Doc.BindView(fun (_, animSel, durSel, easSel) ->
          let animKind =
            animSel
            |> Option.bind (fun label -> allAnimations |> List.tryFind (fun (l, _) -> l = label))
            |> Option.map snd

          let durClass =
            durSel
            |> Option.bind (fun label -> allDurations |> List.tryFind (fun (l, _) -> l = label))
            |> Option.bind snd
            |> Option.map AnimationDuration.toClass

          let easClass =
            easSel
            |> Option.bind (fun label -> allEasings |> List.tryFind (fun (l, _) -> l = label))
            |> Option.bind snd
            |> Option.map AnimationEasing.toClass

          let animLabel = animSel |> Option.defaultValue "Fade In"

          match animKind with
          | Some kind ->
            let classes =
              [ Some(AnimationKind.toClass kind); durClass; easClass ] |> List.choose id

            div [
              BrandColor.toBackgroundColor BrandColor.Primary
              BorderRadius.toClass BorderRadius.All.medium |> cl
              cls classes
              cls [
                Flex.Flex.allSizes
                AlignItems.toClass AlignItems.Center
                JustifyContent.toClass JustifyContent.Center
              ]
              Attr.Style "min-height" "120px"
              Attr.Style "color" "white"
            ] [ H5.Div(animLabel) ]
          | None ->
            div [
              Attr.Style "min-height" "120px"
              cls [
                Flex.Flex.allSizes
                AlignItems.toClass AlignItems.Center
                JustifyContent.toClass JustifyContent.Center
              ]
              Attr.Style "opacity" "0.5"
            ] [ Body1.Div("Select an animation to preview") ])
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Compose any animation kind with optional duration and easing overrides
div [
    cls [
        AnimationEntrance.toClass AnimationEntrance.SlideUpIn
        AnimationDuration.toClass AnimationDuration.Long
        AnimationEasing.toClass AnimationEasing.Bounce
    ]
] [
    text "Slow bouncy slide"
]

// Each axis is independent — use just the animation kind for defaults
div [ cl (AnimationEntrance.toClass AnimationEntrance.FadeIn) ] [
    text "Uses default duration and easing"
]"""

    Helpers.codeSampleSection "Playground" description content code

  let private triggerOnMountSection () =
    let description =
      Helpers.bodyText
        "By default, an animation plays once when the element enters the DOM. This is the simplest trigger and requires no extra setup."

    let content =
      div [
        BrandColor.toBackgroundColor BrandColor.Primary
        BorderRadius.toClass BorderRadius.All.medium |> cl
        cl (AnimationEntrance.toClass AnimationEntrance.FadeIn)
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          JustifyContent.toClass JustifyContent.Center
        ]
        Attr.Style "min-height" "80px"
        Attr.Style "color" "white"
      ] [ Body1.Div("I faded in when this page loaded") ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Animation plays once when the element mounts — no trigger needed
div [
    cl (AnimationEntrance.toClass AnimationEntrance.FadeIn)  // see here
] [
    text "I fade in when rendered"
]"""

    Helpers.codeSampleSection "On mount (default)" description content code

  let private triggerOnHoverFocusSection () =
    let description =
      Helpers.bodyText
        "AnimationOn defers playback to a CSS pseudo-state via toClass, the same pattern as kind and duration. The animation does not play on mount — it waits for hover, focus, or both. No JS is involved. Hover is gated behind @media (hover: hover) so it does not stick on touch devices."

    let content =
      div [] [
        Subtitle2.Div(View.Const "Hover triggers")

        div [
          cls [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes ]
          Attr.Style "gap" "12px"
          Margin.toClasses Margin.Bottom.small |> cls
        ] [
          Button.Create(
            text "Bounce on hover",
            onClick = ignore,
            attrs = [
              Button.Variant.filled
              Button.Color.primary
              cls [
                AnimationEmphasis.toClass AnimationEmphasis.Bounce
                AnimationOn.toClass AnimationOn.Hover // see here
                AnimationDuration.toClass AnimationDuration.Short
              ]
            ]
          )

          Button.Create(
            text "Shake on hover",
            onClick = ignore,
            attrs = [
              Button.Variant.filled
              Button.Color.error
              cls [
                AnimationEmphasis.toClass AnimationEmphasis.Shake
                AnimationOn.toClass AnimationOn.Hover
                AnimationDuration.toClass AnimationDuration.Short
              ]
            ]
          )
        ]

        Subtitle2.Div(View.Const "Focus triggers (use Tab to navigate)")

        div [ cls [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes ]; Attr.Style "gap" "12px" ] [
          Button.Create(
            text "Bounce on focus",
            onClick = ignore,
            attrs = [
              Button.Variant.filled
              Button.Color.secondary
              cls [
                AnimationEmphasis.toClass AnimationEmphasis.Bounce
                AnimationOn.toClass AnimationOn.Focus // see here
                AnimationDuration.toClass AnimationDuration.Short
              ]
            ]
          )

          Button.Create(
            text "Pulse on hover or focus",
            onClick = ignore,
            attrs = [
              Button.Variant.filled
              Button.Color.info
              cls [
                AnimationEmphasis.toClass AnimationEmphasis.Pulse
                AnimationOn.toClass AnimationOn.HoverFocus // see here
              ]
            ]
          )
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Bounce on hover — pure CSS, no JS needed
Button.Create(
    text "Bounce on hover",
    onClick = ignore,
    attrs = [
        Button.Variant.filled
        Button.Color.primary
        cls [
            AnimationEmphasis.toClass AnimationEmphasis.Bounce
            AnimationOn.toClass AnimationOn.Hover  // see here
            AnimationDuration.toClass AnimationDuration.Short
        ]
    ]
)

// Respond to both hover and focus
Button.Create(
    text "Pulse on hover or focus",
    onClick = ignore,
    attrs = [
        Button.Variant.filled
        Button.Color.info
        cls [
            AnimationEmphasis.toClass AnimationEmphasis.Pulse
            AnimationOn.toClass AnimationOn.HoverFocus  // see here
        ]
    ]
)"""

    Helpers.codeSampleSection "On hover / focus" description content code

  let private triggerOnClickSection () =
    let description =
      Helpers.bodyText
        "Animate.replayOnClick replays the animation on each click via a JS event listener. The initial mount animation is automatically suppressed — the element appears static until the first click. Unlike AnimationOn, this is an Attr you add alongside your animation classes."

    let emphasisButton label color emphasis =
      Button.Create(
        text label,
        onClick = ignore,
        attrs = [
          Button.Variant.filled
          Button.Color.toAttr color
          cls [
            AnimationEmphasis.toClass emphasis
            AnimationDuration.toClass AnimationDuration.Medium
          ]
          Animate.replayOnClick // see here
        ]
      )

    let content =
      div [ cls [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes ]; Attr.Style "gap" "12px" ] [
        emphasisButton "Pulse" BrandColor.Primary AnimationEmphasis.Pulse
        emphasisButton "Shake" BrandColor.Error AnimationEmphasis.Shake
        emphasisButton "Bounce" BrandColor.Success AnimationEmphasis.Bounce
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Pulse on click — initial mount animation is suppressed automatically
Button.Create(
    text "Pulse",
    onClick = ignore,
    attrs = [
        Button.Variant.filled
        Button.Color.primary
        cls [
            AnimationEmphasis.toClass AnimationEmphasis.Pulse
            AnimationDuration.toClass AnimationDuration.Medium
        ]
        Animate.replayOnClick  // see here
    ]
)"""

    Helpers.codeSampleSection "On click" description content code

  let private triggerOnTimerSection () =
    let description =
      Helpers.bodyText
        "Animate.replayEvery replays an animation on a recurring JS interval. The first play is immediate on mount, then it replays every N milliseconds. The interval self-clears when the element is removed from the DOM. Useful for notification badges and ambient motion."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          AlignItems.toClass AlignItems.Center
        ]
        Attr.Style "gap" "16px"
      ] [
        Chip.Create(
          text "New messages",
          attrs = [
            cls [
              Chip.Variant.Filled |> Chip.Variant.toClass
              BrandColor.Primary |> Chip.Color.toClass
              AnimationEmphasis.toClass AnimationEmphasis.Pulse
              AnimationDuration.toClass AnimationDuration.Medium
            ]
            Animate.replayEvery 3000 // see here
          ]
        )

        Chip.Create(
          text "Action required",
          attrs = [
            cls [
              Chip.Variant.Filled |> Chip.Variant.toClass
              BrandColor.Error |> Chip.Color.toClass
              AnimationEmphasis.toClass AnimationEmphasis.Shake
              AnimationDuration.toClass AnimationDuration.Short
            ]
            Animate.replayEvery 4000
          ]
        )

        Chip.Create(
          text "Update available",
          attrs = [
            cls [
              Chip.Variant.Filled |> Chip.Variant.toClass
              BrandColor.Success |> Chip.Color.toClass
              AnimationEmphasis.toClass AnimationEmphasis.Bounce
              AnimationDuration.toClass AnimationDuration.Short
            ]
            Animate.replayEvery 5000
          ]
        )
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Pulse every 3 seconds — no Var, no re-render
Chip.Create(
    text "New messages",
    attrs = [
        cls [
            Chip.Variant.Filled |> Chip.Variant.toClass
            BrandColor.Primary |> Chip.Color.toClass
            AnimationEmphasis.toClass AnimationEmphasis.Pulse
            AnimationDuration.toClass AnimationDuration.Medium
        ]
        Animate.replayEvery 3000  // see here
    ]
)"""

    Helpers.codeSampleSection "On a timer" description content code

  let private triggerOnStateChangeSection () =
    let description =
      Helpers.bodyText
        "Animate.toggleClass switches between entrance and exit animation classes based on a reactive View<bool>. The element stays in the DOM — use this for drawers, expansion panels, and other elements that toggle visibility without mounting or unmounting. Neither class is applied until the first state change, so elements that start inactive do not flash an exit animation."

    let isActive = Var.Create false

    let content =
      div [] [
        div [ Margin.toClasses Margin.Bottom.small |> cls ] [
          Button.Create(
            text "Toggle",
            onClick = (fun () -> isActive.Value <- not isActive.Value),
            attrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]

        div [
          BrandColor.toBackgroundColor BrandColor.Secondary
          BorderRadius.toClass BorderRadius.All.medium |> cl
          Animate.toggleClass AnimationPair.fadeInOut isActive.View // see here
          cls [
            Flex.Flex.allSizes
            AlignItems.toClass AlignItems.Center
            JustifyContent.toClass JustifyContent.Center
          ]
          Attr.Style "min-height" "80px"
          Attr.Style "color" "white"
        ] [ Body1.Div("I toggle between fade-in and fade-out") ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let isActive = Var.Create false

// Element stays in DOM — classes toggle reactively
div [
    Animate.toggleClass AnimationPair.fadeInOut isActive.View  // see here
] [
    text "I animate between enter/exit states"
]

Button.Create(
    text "Toggle",
    onClick = (fun () -> isActive.Value <- not isActive.Value)
)"""

    Helpers.codeSampleSection "On reactive state change" description content code

  let private triggerOnMountUnmountSection () =
    let description =
      Helpers.bodyText
        "Animate.show mounts content with an entrance animation and listens for the animationend event before removing it from the DOM. Without this, hidden elements would disappear instantly with no exit animation."

    let isVisible = Var.Create false

    let content =
      div [] [
        div [ Margin.toClasses Margin.Bottom.small |> cls ] [
          Button.Create(
            text "Toggle visibility",
            onClick = (fun () -> isVisible.Value <- not isVisible.Value),
            attrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]

        div [ Attr.Style "min-height" "80px" ] [
          Animate.show AnimationPair.slideUpInOut isVisible.View // see here
          <| fun () ->
            Alert.Create(
              text "This alert mounts and unmounts with animation!",
              attrs = [
                Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Success)
                |> Option.map cl
                |> Option.defaultValue Attr.Empty
                Alert.Variant.Filled |> Alert.Variant.toClass |> cl
              ]
            )
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let isVisible = Var.Create false

// Content is added to DOM on enter, removed after exit animation completes
Animate.show
    AnimationPair.slideUpInOut  // see here
    isVisible.View
    (fun () ->
        Alert.Create(
            text "Animated mount/unmount!",
            attrs = [
                Alert.Variant.Filled |> Alert.Variant.toClass |> cl
            ]
        ))

Button.Create(
    text "Toggle",
    onClick = (fun () -> isVisible.Value <- not isVisible.Value)
)"""

    Helpers.codeSampleSection "On mount / unmount" description content code

  let private animationPairsSection () =
    let description =
      Helpers.bodyText
        "AnimationPair bundles a typed entrance and exit together for use with Animate helpers. Predefined pairs handle natural direction reversal (e.g., slide-up entrance pairs with slide-down exit)."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [ tableHeaderCell "Pair"; tableHeaderCell "Entrance"; tableHeaderCell "Exit" ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "fadeInOut" ]
              tableCell [ inlineCode "FadeIn" ]
              tableCell [ inlineCode "FadeOut" ]
            ]
            tr [] [
              tableCell [ text "scaleInOut" ]
              tableCell [ inlineCode "ScaleIn" ]
              tableCell [ inlineCode "ScaleOut" ]
            ]
            tr [] [
              tableCell [ text "scaleYInOut" ]
              tableCell [ inlineCode "ScaleYIn" ]
              tableCell [ inlineCode "ScaleYOut" ]
            ]
            tr [] [
              tableCell [ text "slideUpInOut" ]
              tableCell [ inlineCode "SlideUpIn" ]
              tableCell [ inlineCode "SlideDownOut" ]
            ]
            tr [] [
              tableCell [ text "slideDownInOut" ]
              tableCell [ inlineCode "SlideDownIn" ]
              tableCell [ inlineCode "SlideUpOut" ]
            ]
            tr [] [
              tableCell [ text "slideLeftInOut" ]
              tableCell [ inlineCode "SlideLeftIn" ]
              tableCell [ inlineCode "SlideRightOut" ]
            ]
            tr [] [
              tableCell [ text "slideRightInOut" ]
              tableCell [ inlineCode "SlideRightIn" ]
              tableCell [ inlineCode "SlideLeftOut" ]
            ]
          ]
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Use a predefined pair
let pair = AnimationPair.fadeInOut
// pair.Enter = AnimationEntrance.FadeIn
// pair.Exit  = AnimationExit.FadeOut

// Create a custom pair — mix and match any entrance with any exit
let custom = AnimationPair.create AnimationEntrance.ScaleIn AnimationExit.SlideDownOut

// Use with Animate.toggleClass for DOM-resident elements
div [
    Animate.toggleClass AnimationPair.scaleInOut isExpanded.View
] [ content ]

// Use with Animate.show for mount/unmount with exit animation
Animate.show
    AnimationPair.fadeInOut
    isVisible.View
    (fun () -> div [] [ text "Animated content" ])"""

    Helpers.codeSampleSection "Animation pairs" description content code

  let private staggeredListSection () =
    let description =
      Helpers.bodyText
        "Apply entrance animations with staggered delays to list items for a cascading reveal effect. Each item uses an animation-delay utility class so they appear one after another."

    let isVisible = Var.Create false

    let items = [
      "Design mockups"
      "Implement components"
      "Write unit tests"
      "Review pull request"
      "Deploy to staging"
    ]

    let content =
      div [] [
        div [ Margin.toClasses Margin.Bottom.small |> cls ] [
          Button.Create(
            text "Replay stagger",
            onClick =
              (fun () ->
                isVisible.Value <- false
                JavaScript.JS.SetTimeout (fun () -> isVisible.Value <- true) 50 |> ignore),
            attrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]

        Animate.show AnimationPair.fadeInOut isVisible.View
        <| fun () ->
          WeaveList.Create(
            items
            |> List.mapi (fun i label ->
              ListItem.Create(
                text label,
                value = label,
                attrs = [
                  cl (AnimationEntrance.toClass AnimationEntrance.SlideRightIn)
                  cl (AnimationDuration.toClass AnimationDuration.Standard)
                  AnimationDelay.stagger (i + 1) |> cl // see here
                ]
              )),
            attrs = [ WeaveList.Color.toClass BrandColor.Primary |> cl ]
          )
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let items = [ "Design mockups"; "Implement components"; "Write unit tests" ]

WeaveList.Create(
    items
    |> List.mapi (fun i label ->
        ListItem.Create(
            text label,
            value = label,
            attrs = [
                cl (AnimationEntrance.toClass AnimationEntrance.SlideRightIn)
                cl (AnimationDuration.toClass AnimationDuration.Standard)
                // Stagger: delay--1, delay--2, delay--3 ... (clamped to 1-10)
                AnimationDelay.stagger (i + 1) |> cl  // see here
            ]
        )),
    attrs = [ WeaveList.Color.toClass BrandColor.Primary |> cl ]
)"""

    Helpers.codeSampleSection "Staggered list" description content code

  let private animatedAlertsSection () =
    let description =
      Helpers.bodyText
        "Use Animate.show with a ListModel to animate alerts in and out. Each alert gets its own isVisible Var so the exit animation plays before removal from the model."

    let alerts = ListModel.Create (fun (id, _, _, _) -> id) []

    let nextId = Var.Create 0

    let alertConfigs = [
      BrandColor.Success, "Operation completed successfully!", AnimationPair.slideRightInOut
      BrandColor.Error, "Something went wrong.", AnimationPair.slideLeftInOut
      BrandColor.Warning, "Careful — this action is irreversible.", AnimationPair.scaleInOut
      BrandColor.Info, "New version available.", AnimationPair.fadeInOut
    ]

    let addAlert () =
      let idx = nextId.Value % alertConfigs.Length
      let color, msg, pair = alertConfigs.[idx]
      let id = nextId.Value
      nextId.Value <- id + 1
      alerts.Add(id, color, msg, pair)

    let removeAlert id = alerts.RemoveByKey id

    let content =
      div [] [
        div [ Margin.toClasses Margin.Bottom.small |> cls ] [
          Button.Create(
            text "Add alert",
            onClick = addAlert,
            attrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]

        div [
          cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ]
          Attr.Style "gap" "8px"
        ] [
          alerts.View
          |> Doc.BindSeqCachedBy (fun (id, _, _, _) -> id) (fun (id, color, msg, pair) ->
            let isVisible = Var.Create true

            div [] [
              Animate.showWith
                pair
                isVisible.View
                (fun () ->
                  Alert.Create(
                    text msg,
                    onClose = (fun () -> isVisible.Value <- false),
                    attrs = [
                      Alert.AlertColor.toClass (Alert.AlertColor.BrandColor color)
                      |> Option.map cl
                      |> Option.defaultValue Attr.Empty
                      Alert.Variant.Filled |> Alert.Variant.toClass |> cl
                    ]
                  ))
                [ AnimationDuration.toClass AnimationDuration.Standard |> cl ]
                (Some(fun () -> removeAlert id))
            ])
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let alerts = ListModel.Create (fun (id, _, _) -> id) []

// Add alert — each gets its own isVisible for exit animation
let addAlert color msg pair =
    alerts.Add(nextId, color, msg, pair)

// Render with Doc.BindSeqCachedBy so only new items animate
alerts.View
|> Doc.BindSeqCachedBy
    (fun (id, _, _) -> id)
    (fun (id, color, msg) ->
        let isVisible = Var.Create true

        // showWith passes duration to the wrapper and removes from
        // ListModel only after the exit animation completes
        Animate.showWith
            AnimationPair.slideRightInOut
            isVisible.View
            (fun () ->
                Alert.Create(
                    text msg,
                    onClose = (fun () -> isVisible.Value <- false),
                    attrs = [
                        Alert.AlertColor.toClass (Alert.AlertColor.BrandColor color)
                        |> Option.map cl |> Option.defaultValue Attr.Empty
                        Alert.Variant.Filled |> Alert.Variant.toClass |> cl
                    ]
                ))
            [ AnimationDuration.toClass AnimationDuration.Standard |> cl ]
            (Some (fun () -> alerts.RemoveByKey id)))"""

    Helpers.codeSampleSection "Animated alerts" description content code

  let private animatedChipsSection () =
    let description =
      Helpers.bodyText
        "Chips can animate in when added and scale out when removed. Use ListModel with Doc.BindSeqCachedBy so only the new chip animates — existing chips stay put."

    let chipLabels = [| "F#"; "WebSharper"; "Weave"; "Sass"; "Playwright"; "Expecto"; ".NET" |]
    let activeChips = ListModel.Create (fun (id, _) -> id) []
    let nextChipId = Var.Create 0

    let addChip () =
      let idx = nextChipId.Value % chipLabels.Length
      let id = nextChipId.Value
      nextChipId.Value <- id + 1
      activeChips.Add(id, chipLabels.[idx])

    let removeChip id = activeChips.RemoveByKey id

    let content =
      div [] [
        div [ Margin.toClasses Margin.Bottom.small |> cls ] [
          Button.Create(
            text "Add chip",
            onClick = addChip,
            attrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]

        div [ cls [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes ]; Attr.Style "gap" "8px" ] [
          activeChips.View
          |> Doc.BindSeqCachedBy (fun (id, _) -> id) (fun (id, label) ->
            let isVisible = Var.Create true

            Animate.showWith
              AnimationPair.scaleInOut
              isVisible.View
              (fun () ->
                Chip.Create(
                  text label,
                  onClose = (fun () -> isVisible.Value <- false),
                  attrs = [
                    cls [
                      Chip.Variant.Filled |> Chip.Variant.toClass
                      BrandColor.Primary |> Chip.Color.toClass
                    ]
                  ]
                ))
              [
                AnimationDuration.toClass AnimationDuration.Short |> cl
                AnimationEasing.toClass AnimationEasing.Decelerate |> cl
              ]
              (Some(fun () -> removeChip id)))
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let isVisible = Var.Create true

// Scale a chip in when added, scale out when removed.
// showWith puts duration/easing on the wrapper and calls removeChip
// after the exit animation completes — no hardcoded timeout needed.
Animate.showWith
    AnimationPair.scaleInOut  // see here
    isVisible.View
    (fun () ->
        Chip.Create(
            text "WebSharper",
            onClose = (fun () -> isVisible.Value <- false),
            attrs = [
                cls [
                    Chip.Variant.Filled |> Chip.Variant.toClass
                    BrandColor.Primary |> Chip.Color.toClass
                ]
            ]
        ))
    [ AnimationDuration.toClass AnimationDuration.Short |> cl
      AnimationEasing.toClass AnimationEasing.Decelerate |> cl ]
    (Some (fun () -> chips.RemoveByKey id))"""

    Helpers.codeSampleSection "Animated chips" description content code

  let private designTokensSection () =
    let description =
      Helpers.bodyText
        "Animation intensity is controlled by CSS custom properties. Override them on any element or subtree to tune distance, scale, and stagger timing. Under prefers-reduced-motion: reduce, distances and scales collapse automatically."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [
              tableHeaderCell "Token"
              tableHeaderCell "Default"
              tableHeaderCell "Reduced motion"
            ]
          ]
          tbody [] [
            tr [] [
              tableCell [ inlineCode "--weave-animation-distance-sm" ]
              tableCell [ text "8px" ]
              tableCell [ text "0px" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-animation-distance-md" ]
              tableCell [ text "16px" ]
              tableCell [ text "0px" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-animation-distance-lg" ]
              tableCell [ text "32px" ]
              tableCell [ text "0px" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-animation-distance-xl" ]
              tableCell [ text "64px" ]
              tableCell [ text "0px" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-animation-scale-sm" ]
              tableCell [ text "0.9" ]
              tableCell [ text "1" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-animation-scale-md" ]
              tableCell [ text "0.8" ]
              tableCell [ text "1" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-animation-scale-lg" ]
              tableCell [ text "0.65" ]
              tableCell [ text "1" ]
            ]
            tr [] [
              tableCell [ inlineCode "--weave-stagger-delay" ]
              tableCell [ text "50ms" ]
              tableCell [ text "0ms" ]
            ]
          ]
        ]
      ]

    let code =
      """// Override animation distance for a subtree
div [ Attr.Style "--weave-animation-distance-lg" "32px" ] [
    // Slide animations inside here travel twice as far
    div [ cl (AnimationEntrance.toClass AnimationEntrance.SlideUpIn) ] [
        text "Longer slide distance"
    ]
]

// Override scale for more dramatic entrances
div [ Attr.Style "--weave-animation-scale-md" "0.5" ] [
    div [ cl (AnimationEntrance.toClass AnimationEntrance.ScaleIn) ] [
        text "Scales from 50% instead of 90%"
    ]
]

// Adjust stagger timing
div [ Attr.Style "--weave-stagger-delay" "100ms" ] [
    // Items stagger at 100ms intervals instead of 50ms
]"""

    Helpers.codeSampleSection "Design tokens" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Animations"
        Helpers.bodyText
          "Apply entrance, exit, and emphasis animations to any element with composable CSS classes. Each animation is built from four axes — kind, duration, easing, and trigger — all composed the same way via toClass and cls. For JS-driven triggers (click, timer, reactive state), use the Animate module instead."
        Helpers.divider ()

        howItWorksSection ()
        Helpers.divider ()

        playgroundSection ()
        Helpers.divider ()

        Helpers.sectionHeader "When do animations play?"

        Body1.Div(
          "There are six ways to trigger an animation. The default is on mount. AnimationOn (hover, focus, hover-focus) is a CSS-only trigger that composes via toClass and cls just like kind, duration, and easing. The remaining triggers — click, timer, reactive state, and mount/unmount — are JS-based and live in the Animate module as Attr helpers.",
          attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
        )

        triggerOnMountSection ()
        Helpers.divider ()

        Subtitle2.Div(
          View.Const "CSS triggers (AnimationOn)",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        triggerOnHoverFocusSection ()
        Helpers.divider ()

        Subtitle2.Div(
          View.Const "JS triggers (Animate module)",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        triggerOnClickSection ()
        Helpers.divider ()
        triggerOnTimerSection ()
        Helpers.divider ()
        triggerOnStateChangeSection ()
        Helpers.divider ()
        triggerOnMountUnmountSection ()
        Helpers.divider ()

        animationPairsSection ()
        Helpers.divider ()

        staggeredListSection ()
        Helpers.divider ()

        Helpers.sectionHeader "Customizing components"

        Body1.Div(
          "These recipes show how to compose triggers and animations on real Weave components.",
          attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
        )

        animatedAlertsSection ()
        Helpers.divider ()
        animatedChipsSection ()
        Helpers.divider ()

        designTokensSection ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
