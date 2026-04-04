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
    span [ Typography.caption; Typography.Color.primary ] [ text value ]

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
        "Animations are built from four composable axes: what plays (kind), how long (duration), what curve (easing), and when it triggers (AnimationOn). Each axis is a module of Attr let-bindings — compose them directly in attrs. AnimationOn is CSS-only (hover/focus); for JS-driven triggers like click or timer, see the Animate module below."

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
              tableCell [ inlineCode "AnimationEntrance.fadeIn" ]
            ]
            tr [] [
              tableCell [ text "AnimationExit" ]
              tableCell [ text "Elements leaving" ]
              tableCell [ inlineCode "AnimationExit.fadeOut" ]
            ]
            tr [] [
              tableCell [ text "AnimationEmphasis" ]
              tableCell [ text "Drawing attention" ]
              tableCell [ inlineCode "AnimationEmphasis.pulse" ]
            ]
            tr [] [
              tableCell [ text "AnimationDuration" ]
              tableCell [ text "Override duration" ]
              tableCell [ inlineCode "AnimationDuration.short" ]
            ]
            tr [] [
              tableCell [ text "AnimationEasing" ]
              tableCell [ text "Override easing curve" ]
              tableCell [ inlineCode "AnimationEasing.bounce" ]
            ]
            tr [] [
              tableCell [ text "AnimationDistance" ]
              tableCell [ text "Override travel distance" ]
              tableCell [ inlineCode "AnimationDistance.large" ]
            ]
            tr [] [
              tableCell [ text "AnimationScale" ]
              tableCell [ text "Override scale intensity" ]
              tableCell [ inlineCode "AnimationScale.extraSmall" ]
            ]
            tr [] [
              tableCell [ text "AnimationOn" ]
              tableCell [ text "CSS-only trigger (hover/focus)" ]
              tableCell [ inlineCode "AnimationOn.hover" ]
            ]
            tr [] [
              tableCell [ text "AnimationKind" ]
              tableCell [ text "Suppress all animations" ]
              tableCell [ inlineCode "AnimationKind.suppress" ]
            ]
            tr [] [
              tableCell [ text "TransitionEasing" ]
              tableCell [ text "Override transition easing on a subtree" ]
              tableCell [ inlineCode "TransitionEasing.bounce" ]
            ]
          ]
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Apply an entrance animation — plays once on mount by default
div [ AnimationEntrance.fadeIn ] [
    text "I fade in when rendered"
]

// Compose all axes: kind + duration + easing + distance + trigger
div [
    AnimationEmphasis.bounce
    AnimationDuration.short
    AnimationEasing.bounce
    AnimationDistance.large
    AnimationOn.hover
] [
    text "Bounces further on hover"
]

// Override distance and scale for dramatic entrances
div [
    AnimationEntrance.slideUpIn
    AnimationDuration.long
    AnimationDistance.extraLarge
] [
    text "Slides up extra far"
]

div [
    AnimationEntrance.scaleIn
    AnimationScale.extraSmall
] [
    text "Barely perceptible scale"
]"""

    Helpers.codeSampleSection "How it works" description content code

  let private playgroundSection () =
    let description =
      Helpers.bodyText
        "Pick an animation, duration, and easing to preview the effect. Click \"Play\" to replay."

    let allAnimations = [
      "Entrance: Fade In", AnimationEntrance.fadeIn
      "Entrance: Scale In", AnimationEntrance.scaleIn
      "Entrance: Scale Y In", AnimationEntrance.scaleYIn
      "Entrance: Slide Up", AnimationEntrance.slideUpIn
      "Entrance: Slide Down", AnimationEntrance.slideDownIn
      "Entrance: Slide Left", AnimationEntrance.slideLeftIn
      "Entrance: Slide Right", AnimationEntrance.slideRightIn
      "Exit: Fade Out", AnimationExit.fadeOut
      "Exit: Scale Out", AnimationExit.scaleOut
      "Exit: Scale Y Out", AnimationExit.scaleYOut
      "Exit: Slide Up", AnimationExit.slideUpOut
      "Exit: Slide Down", AnimationExit.slideDownOut
      "Exit: Slide Left", AnimationExit.slideLeftOut
      "Exit: Slide Right", AnimationExit.slideRightOut
      "Emphasis: Pulse", AnimationEmphasis.pulse
      "Emphasis: Shake", AnimationEmphasis.shake
      "Emphasis: Bounce", AnimationEmphasis.bounce
    ]

    let allDurations = [
      "Shortest (100ms)", Some AnimationDuration.shortest
      "Shorter (200ms)", Some AnimationDuration.shorter
      "Short (300ms)", Some AnimationDuration.short
      "Standard (400ms)", Some AnimationDuration.standard
      "Medium (500ms)", Some AnimationDuration.medium
      "Long (600ms)", Some AnimationDuration.long
      "Longer (800ms)", Some AnimationDuration.longer
      "Longest (1200ms)", Some AnimationDuration.longest
    ]

    let allEasings = [
      "Standard", Some AnimationEasing.standard
      "Decelerate", Some AnimationEasing.decelerate
      "Accelerate", Some AnimationEasing.accelerate
      "Bounce", Some AnimationEasing.bounce
    ]

    let allDistances = [
      "Extra Small", Some AnimationDistance.extraSmall
      "Small", Some AnimationDistance.small
      "Medium", Some AnimationDistance.medium
      "Large", Some AnimationDistance.large
      "Extra Large", Some AnimationDistance.extraLarge
    ]

    let allScales = [
      "Extra Small", Some AnimationScale.extraSmall
      "Small", Some AnimationScale.small
      "Medium", Some AnimationScale.medium
      "Large", Some AnimationScale.large
      "Extra Large", Some AnimationScale.extraLarge
    ]

    let selectedAnimation = Var.Create<string option>(Some "Entrance: Fade In")
    let selectedDuration = Var.Create<string option>(Some "Standard (400ms)")
    let selectedEasing = Var.Create<string option>(Some "Decelerate")
    let selectedDistance = Var.Create<string option>(Some "Medium")
    let selectedScale = Var.Create<string option>(Some "Medium")
    let replayKey = Var.Create 0

    let animationItems =
      allAnimations
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let durationItems =
      allDurations
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let easingItems =
      allEasings
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let distanceItems =
      allDistances
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let scaleItems =
      allScales
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let content =
      div [] [
        Grid.create (
          [
            GridItem.create (
              Select.create (
                animationItems,
                selectedAnimation,
                labelText = View.Const "Animation",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
            )
            GridItem.create (
              Select.create (
                durationItems,
                selectedDuration,
                labelText = View.Const "Duration",
                placeholder = View.Const "Default",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four ]
            )
            GridItem.create (
              Select.create (
                easingItems,
                selectedEasing,
                labelText = View.Const "Easing",
                placeholder = View.Const "Default",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four ]
            )
            GridItem.create (
              Select.create (
                distanceItems,
                selectedDistance,
                labelText = View.Const "Distance",
                placeholder = View.Const "Default",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.six ]
            )
            GridItem.create (
              Select.create (
                scaleItems,
                selectedScale,
                labelText = View.Const "Scale",
                placeholder = View.Const "Default",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.six ]
            )
          ]
        )

        div [ Margin.Vertical.small ] [
          Button.primary (
            text "Play",
            onClick = (fun () -> replayKey.Value <- replayKey.Value + 1),
            attrs = [ Button.Variant.filled; Button.Width.full ]
          )
        ]

        View.Map3
          (fun animSel durSel easSel -> animSel, durSel, easSel)
          selectedAnimation.View
          selectedDuration.View
          selectedEasing.View
        |> View.Map2 (fun distSel tuple -> tuple, distSel) selectedDistance.View
        |> View.Map2 (fun scaleSel (tuple, distSel) -> tuple, distSel, scaleSel) selectedScale.View
        |> View.Map2
          (fun key ((animSel, durSel, easSel), distSel, scaleSel) ->
            key, animSel, durSel, easSel, distSel, scaleSel)
          replayKey.View
        |> Doc.BindView(fun (_, animSel, durSel, easSel, distSel, scaleSel) ->
          let animAttr =
            animSel
            |> Option.bind (fun label -> allAnimations |> List.tryFind (fun (l, _) -> l = label))
            |> Option.map snd

          let durAttr =
            durSel
            |> Option.bind (fun label -> allDurations |> List.tryFind (fun (l, _) -> l = label))
            |> Option.bind snd
            |> Option.defaultValue Attr.Empty

          let easAttr =
            easSel
            |> Option.bind (fun label -> allEasings |> List.tryFind (fun (l, _) -> l = label))
            |> Option.bind snd
            |> Option.defaultValue Attr.Empty

          let distAttr =
            distSel
            |> Option.bind (fun label -> allDistances |> List.tryFind (fun (l, _) -> l = label))
            |> Option.bind snd
            |> Option.defaultValue Attr.Empty

          let scaleAttr =
            scaleSel
            |> Option.bind (fun label -> allScales |> List.tryFind (fun (l, _) -> l = label))
            |> Option.bind snd
            |> Option.defaultValue Attr.Empty

          let animLabel = animSel |> Option.defaultValue "Fade In"

          match animAttr with
          | Some kindAttr ->
            div [
              BrandColor.BackgroundColor.primary
              BorderRadius.All.medium
              kindAttr
              durAttr
              easAttr
              distAttr
              scaleAttr
              Flex.Flex.allSizes
              AlignItems.center
              JustifyContent.center
              Attr.Style "min-height" "120px"
              Attr.Style "color" "white"
            ] [ div [ Typography.h5 ] [ text animLabel ] ]
          | None ->
            div [
              Attr.Style "min-height" "120px"
              Flex.Flex.allSizes
              AlignItems.center
              JustifyContent.center
              Attr.Style "opacity" "0.5"
            ] [ div [ Typography.body1 ] [ text "Select an animation to preview" ] ])
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Compose any entrance with optional duration, easing, distance, and scale
div [
    AnimationEntrance.slideUpIn
    AnimationDuration.long
    AnimationEasing.bounce
    AnimationDistance.extraLarge
] [
    text "Slow bouncy slide — extra far"
]

// Override scale for a subtle entrance
div [
    AnimationEntrance.scaleIn
    AnimationScale.extraSmall
] [
    text "Barely perceptible scale"
]

// Each axis is independent — use just the kind for defaults
div [ AnimationEntrance.fadeIn ] [
    text "Uses default duration, easing, distance, and scale"
]"""

    Helpers.codeSampleSection "Playground" description content code

  // 2b. Distance & Scale — side-by-side comparison of all 5 levels
  let private distanceAndScaleSection () =
    let description =
      Helpers.bodyText
        "AnimationDistance controls how far elements translate during slide, shake, and bounce animations. AnimationScale controls how dramatically elements shrink or grow during scale and pulse animations. Each has five levels from ExtraSmall (subtle) to ExtraLarge (dramatic). Medium matches the root defaults."

    let replayDistance = Var.Create 0
    let replayScale = Var.Create 0

    let distanceLevels = [
      "XS", AnimationDistance.extraSmall
      "S", AnimationDistance.small
      "M", AnimationDistance.medium
      "L", AnimationDistance.large
      "XL", AnimationDistance.extraLarge
    ]

    let scaleLevels = [
      "XS", AnimationScale.extraSmall
      "S", AnimationScale.small
      "M", AnimationScale.medium
      "L", AnimationScale.large
      "XL", AnimationScale.extraLarge
    ]

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ textView (View.Const "Distance (slide up)") ]

        div [ Margin.Bottom.small ] [
          Button.primary (
            text "Replay",
            onClick = (fun () -> replayDistance.Value <- replayDistance.Value + 1),
            attrs = [ Button.Variant.outlined ]
          )
        ]

        replayDistance.View
        |> Doc.BindView(fun _ ->
          div [
            Flex.Flex.allSizes
            FlexWrap.Wrap.allSizes
            Attr.Style "gap" "12px"
            Margin.Bottom.medium
          ] [
            yield!
              distanceLevels
              |> List.map (fun (label, distAttr) ->
                div [
                  BrandColor.BackgroundColor.primary
                  BorderRadius.All.medium
                  AnimationEntrance.slideUpIn
                  AnimationDuration.long
                  distAttr
                  Flex.Flex.allSizes
                  AlignItems.center
                  JustifyContent.center
                  Attr.Style "min-height" "80px"
                  Attr.Style "min-width" "80px"
                  Attr.Style "flex" "1"
                  Attr.Style "color" "white"
                ] [ div [ Typography.body1 ] [ text label ] ])
          ])

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ textView (View.Const "Scale (scale in)") ]

        div [ Margin.Bottom.small ] [
          Button.primary (
            text "Replay",
            onClick = (fun () -> replayScale.Value <- replayScale.Value + 1),
            attrs = [ Button.Variant.outlined ]
          )
        ]

        replayScale.View
        |> Doc.BindView(fun _ ->
          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Attr.Style "gap" "12px" ] [
            yield!
              scaleLevels
              |> List.map (fun (label, scaleAttr) ->
                div [
                  BrandColor.BackgroundColor.secondary
                  BorderRadius.All.medium
                  AnimationEntrance.scaleIn
                  AnimationDuration.long
                  scaleAttr
                  Flex.Flex.allSizes
                  AlignItems.center
                  JustifyContent.center
                  Attr.Style "min-height" "80px"
                  Attr.Style "min-width" "80px"
                  Attr.Style "flex" "1"
                  Attr.Style "color" "white"
                ] [ div [ Typography.body1 ] [ text label ] ])
          ])
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Override distance — controls how far slides, shakes, and bounces travel
div [
    AnimationEntrance.slideUpIn
    AnimationDistance.extraSmall   // barely moves (8px)
] [ text "Subtle slide" ]

div [
    AnimationEntrance.slideUpIn
    AnimationDistance.extraLarge   // dramatic travel (64px)
] [ text "Big slide" ]

// Override scale — controls how much elements shrink/grow
div [
    AnimationEntrance.scaleIn
    AnimationScale.extraSmall     // nearly invisible scale (0.95)
] [ text "Gentle scale" ]

div [
    AnimationEntrance.scaleIn
    AnimationScale.extraLarge     // dramatic scale from 0.6
] [ text "Dramatic scale" ]

// Compose distance and scale with other axes
div [
    AnimationEntrance.slideUpIn
    AnimationDuration.long
    AnimationEasing.bounce
    AnimationDistance.large
] [ text "Long bouncy slide" ]"""

    Helpers.codeSampleSection "Distance & scale" description content code

  let private triggerOnMountSection () =
    let description =
      Helpers.bodyText
        "By default, an animation plays once when the element enters the DOM. This is the simplest trigger and requires no extra setup."

    let content =
      div [
        BrandColor.BackgroundColor.primary
        BorderRadius.All.medium
        AnimationEntrance.fadeIn
        Flex.Flex.allSizes
        AlignItems.center
        JustifyContent.center
        Attr.Style "min-height" "80px"
        Attr.Style "color" "white"
      ] [ div [ Typography.body1 ] [ text "I faded in when this page loaded" ] ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Animation plays once when the element mounts — no trigger needed
div [ AnimationEntrance.fadeIn ] [
    text "I fade in when rendered"
]"""

    Helpers.codeSampleSection "On mount (default)" description content code

  let private triggerOnHoverFocusSection () =
    let description =
      Helpers.bodyText
        "AnimationOn defers playback to a CSS pseudo-state, composing the same way as kind and duration. The animation does not play on mount — it waits for hover, focus, or both. No JS is involved. Hover is gated behind @media (hover: hover) so it does not stick on touch devices."

    let content =
      div [] [
        div [ Typography.subtitle2 ] [ textView (View.Const "Hover triggers") ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Attr.Style "gap" "12px"
          Margin.Bottom.small
        ] [
          Button.primary (
            text "Bounce on hover",
            onClick = ignore,
            attrs = [
              Button.Variant.filled
              AnimationEmphasis.bounce
              AnimationOn.hover
              AnimationDuration.short
            ]
          )

          Button.error (
            text "Shake on hover",
            onClick = ignore,
            attrs = [
              Button.Variant.filled
              AnimationEmphasis.shake
              AnimationOn.hover
              AnimationDuration.short
            ]
          )
        ]

        div [ Typography.subtitle2 ] [ textView (View.Const "Focus triggers (use Tab to navigate)") ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Attr.Style "gap" "12px" ] [
          Button.secondary (
            text "Bounce on focus",
            onClick = ignore,
            attrs = [
              Button.Variant.filled
              AnimationEmphasis.bounce
              AnimationOn.focus
              AnimationDuration.short
            ]
          )

          Button.info (
            text "Pulse on hover or focus",
            onClick = ignore,
            attrs = [ Button.Variant.filled; AnimationEmphasis.pulse; AnimationOn.hoverFocus ]
          )
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Bounce on hover — pure CSS, no JS needed
Button.primary(
    text "Bounce on hover",
    onClick = ignore,
    attrs = [
        Button.Variant.filled
        AnimationEmphasis.bounce
        AnimationOn.hover
        AnimationDuration.short
    ]
)

// Respond to both hover and focus
Button.info(
    text "Pulse on hover or focus",
    onClick = ignore,
    attrs = [
        Button.Variant.filled
        AnimationEmphasis.pulse
        AnimationOn.hoverFocus
    ]
)"""

    Helpers.codeSampleSection "On hover / focus" description content code

  let private triggerOnClickSection () =
    let description =
      Helpers.bodyText
        "Animate.replayOnClick replays the animation on each click via a JS event listener. The initial mount animation is automatically suppressed — the element appears static until the first click. Unlike AnimationOn, this is an Attr you add alongside your animation classes."

    let emphasisButton label colorAttr emphasisAttr =
      Button.create (
        text label,
        onClick = ignore,
        attrs = [
          Button.Variant.filled
          colorAttr
          emphasisAttr
          AnimationDuration.medium
          Animate.replayOnClick
        ]
      )

    let content =
      div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Attr.Style "gap" "12px" ] [
        emphasisButton "Pulse" Button.Color.primary AnimationEmphasis.pulse
        emphasisButton "Shake" Button.Color.error AnimationEmphasis.shake
        emphasisButton "Bounce" Button.Color.success AnimationEmphasis.bounce
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Pulse on click — initial mount animation is suppressed automatically
Button.primary(
    text "Pulse",
    onClick = ignore,
    attrs = [
        Button.Variant.filled
        AnimationEmphasis.pulse
        AnimationDuration.medium
        Animate.replayOnClick
    ]
)"""

    Helpers.codeSampleSection "On click" description content code

  let private triggerOnTimerSection () =
    let description =
      Helpers.bodyText
        "Animate.replayEvery replays an animation on a recurring JS interval. The first play is immediate on mount, then it replays every N milliseconds. The interval self-clears when the element is removed from the DOM. Useful for notification badges and ambient motion."

    let content =
      div [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        AlignItems.center
        Attr.Style "gap" "16px"
      ] [
        Chip.create (
          text "New messages",
          attrs = [
            Chip.Variant.filled
            Chip.Color.primary
            AnimationEmphasis.pulse
            AnimationDuration.medium
            Animate.replayEvery 3000
          ]
        )

        Chip.create (
          text "Action required",
          attrs = [
            Chip.Variant.filled
            Chip.Color.error
            AnimationEmphasis.shake
            AnimationDuration.short
            Animate.replayEvery 4000
          ]
        )

        Chip.create (
          text "Update available",
          attrs = [
            Chip.Variant.filled
            Chip.Color.success
            AnimationEmphasis.bounce
            AnimationDuration.short
            Animate.replayEvery 5000
          ]
        )
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Pulse every 3 seconds — no Var, no re-render
Chip.create(
    text "New messages",
    attrs = [
        Chip.Variant.filled
        Chip.Color.primary
        AnimationEmphasis.pulse
        AnimationDuration.medium
        Animate.replayEvery 3000
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
        div [ Margin.Bottom.small ] [
          Button.primary (
            text "Toggle",
            onClick = (fun () -> isActive.Value <- not isActive.Value),
            attrs = [ Button.Variant.filled ]
          )
        ]

        div [
          BrandColor.BackgroundColor.secondary
          BorderRadius.All.medium
          Animate.toggleClass AnimationPair.fadeInOut isActive.View
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "min-height" "80px"
          Attr.Style "color" "white"
        ] [ div [ Typography.body1 ] [ text "I toggle between fade-in and fade-out" ] ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let isActive = Var.Create false

// Element stays in DOM — classes toggle reactively
div [
    Animate.toggleClass AnimationPair.fadeInOut isActive.View
] [
    text "I animate between enter/exit states"
]

Button.create(
    text "Toggle",
    onClick = (fun () -> isActive.Value <- not isActive.Value)
)"""

    Helpers.codeSampleSection "On reactive state change" description content code

  let private triggerOnMountUnmountSection () =
    let description =
      Helpers.bodyText
        "Animate.show mounts content with an entrance animation and listens for the animationend event before removing it from the DOM. Without this, hidden elements would disappear instantly with no exit animation."

    let isVisible = Var.Create true

    let content =
      div [] [
        div [ Margin.Bottom.small ] [
          Button.primary (
            text "Toggle visibility",
            onClick = (fun () -> isVisible.Value <- not isVisible.Value),
            attrs = [ Button.Variant.filled ]
          )
        ]

        div [ Attr.Style "min-height" "80px" ] [
          Animate.show AnimationPair.slideUpInOut isVisible.View
          <| fun () ->
            Alert.create (
              text "This alert mounts and unmounts with animation!",
              attrs = [ Alert.Color.success; Alert.Variant.filled ]
            )
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let isVisible = Var.Create true

// Content is added to DOM on enter, removed after exit animation completes
Animate.show
    AnimationPair.slideUpInOut
    isVisible.View
    (fun () ->
        Alert.create(
            text "Animated mount/unmount!",
            attrs = [
                Alert.Variant.filled
            ]
        ))

Button.create(
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
        div [ Margin.Bottom.small ] [
          Button.primary (
            text "Replay stagger",
            onClick =
              (fun () ->
                isVisible.Value <- false
                JavaScript.JS.SetTimeout (fun () -> isVisible.Value <- true) 50 |> ignore),
            attrs = [ Button.Variant.filled ]
          )
        ]

        Animate.show AnimationPair.fadeInOut isVisible.View
        <| fun () ->
          WeaveList.create (
            items
            |> List.mapi (fun i label ->
              ListItem.create (
                text label,
                value = label,
                attrs = [
                  AnimationEntrance.slideRightIn
                  AnimationDuration.standard
                  AnimationDelay.stagger (i + 1)
                ]
              )),
            attrs = [ WeaveList.Color.primary ]
          )
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let items = [ "Design mockups"; "Implement components"; "Write unit tests" ]

WeaveList.create(
    items
    |> List.mapi (fun i label ->
        ListItem.create(
            text label,
            value = label,
            attrs = [
                AnimationEntrance.slideRightIn
                AnimationDuration.standard
                // Stagger: delay--1, delay--2, delay--3 ... (clamped to 1-10)
                AnimationDelay.stagger (i + 1)
            ]
        )),
    attrs = [ WeaveList.Color.primary ]
)"""

    Helpers.codeSampleSection "Staggered list" description content code

  let private animatedAlertsSection () =
    let description =
      Helpers.bodyText
        "Use Animate.show with a ListModel to animate alerts in and out. Each alert gets its own isVisible Var so the exit animation plays before removal from the model."

    let alerts = ListModel.Create (fun (id, _, _, _) -> id) []

    let nextId = Var.Create 0

    let alertConfigs = [
      Alert.Color.success, "Operation completed successfully!", AnimationPair.slideRightInOut
      Alert.Color.error, "Something went wrong.", AnimationPair.slideLeftInOut
      Alert.Color.warning, "Careful — this action is irreversible.", AnimationPair.scaleInOut
      Alert.Color.info, "New version available.", AnimationPair.fadeInOut
    ]

    let addAlert () =
      let idx = nextId.Value % alertConfigs.Length
      let colorAttr, msg, pair = alertConfigs.[idx]
      let id = nextId.Value
      nextId.Value <- id + 1
      alerts.Add(id, colorAttr, msg, pair)

    let removeAlert id = alerts.RemoveByKey id

    let content =
      div [] [
        div [ Margin.Bottom.small ] [
          Button.primary (text "Add alert", onClick = addAlert, attrs = [ Button.Variant.filled ])
        ]

        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Attr.Style "gap" "8px" ] [
          alerts.View
          |> Doc.BindSeqCachedBy (fun (id, _, _, _) -> id) (fun (id, colorAttr, msg, pair) ->
            let isVisible = Var.Create true

            div [] [
              Animate.showWith
                pair
                isVisible.View
                (fun () ->
                  Alert.create (
                    text msg,
                    onClose = (fun () -> isVisible.Value <- false),
                    attrs = [ colorAttr; Alert.Variant.filled ]
                  ))
                [ AnimationDuration.standard ]
                (Some(fun () -> removeAlert id))
            ])
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


let alerts = ListModel.Create (fun (id, _, _, _) -> id) []

let alertConfigs = [
    Alert.Color.success, "Operation completed successfully!", AnimationPair.slideRightInOut
    Alert.Color.error, "Something went wrong.", AnimationPair.slideLeftInOut
    Alert.Color.warning, "Careful — this action is irreversible.", AnimationPair.scaleInOut
    Alert.Color.info, "New version available.", AnimationPair.fadeInOut
]

// Add alert — each gets its own isVisible for exit animation
let addAlert () =
    let idx = nextId % alertConfigs.Length
    let colorAttr, msg, pair = alertConfigs.[idx]
    alerts.Add(nextId, colorAttr, msg, pair)

// Render with Doc.BindSeqCachedBy so only new items animate
alerts.View
|> Doc.BindSeqCachedBy
    (fun (id, _, _, _) -> id)
    (fun (id, colorAttr, msg, pair) ->
        let isVisible = Var.Create true

        // showWith passes duration to the wrapper and removes from
        // ListModel only after the exit animation completes
        Animate.showWith
            pair
            isVisible.View
            (fun () ->
                Alert.create(
                    text msg,
                    onClose = (fun () -> isVisible.Value <- false),
                    attrs = [
                        colorAttr
                        Alert.Variant.filled
                    ]
                ))
            [ AnimationDuration.standard ]
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
        div [ Margin.Bottom.small ] [
          Button.primary (text "Add chip", onClick = addChip, attrs = [ Button.Variant.filled ])
        ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Attr.Style "gap" "8px" ] [
          activeChips.View
          |> Doc.BindSeqCachedBy (fun (id, _) -> id) (fun (id, label) ->
            let isVisible = Var.Create true

            Animate.showWith
              AnimationPair.scaleInOut
              isVisible.View
              (fun () ->
                Chip.create (
                  text label,
                  onClose = (fun () -> isVisible.Value <- false),
                  attrs = [ Chip.Variant.filled; Chip.Color.primary ]
                ))
              [ AnimationDuration.short; AnimationEasing.decelerate ]
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
    AnimationPair.scaleInOut
    isVisible.View
    (fun () ->
        Chip.create(
            text "WebSharper",
            onClose = (fun () -> isVisible.Value <- false),
            attrs = [
                Chip.Variant.filled
                Chip.Color.primary
            ]
        ))
    [ AnimationDuration.short
      AnimationEasing.decelerate ]
    (Some (fun () -> chips.RemoveByKey id))"""

    Helpers.codeSampleSection "Animated chips" description content code

  let private designTokensSection () =
    let description =
      Helpers.bodyText
        "Animation intensity is controlled by CSS custom properties. Use AnimationDistance and AnimationScale for the five predefined levels, or override the raw tokens on any element or subtree for fine-grained control. Under prefers-reduced-motion: reduce, all distances and scales collapse automatically."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          textView (View.Const "AnimationDistance levels")
        ]

        div [ Typography.body2; Margin.Bottom.small ] [
          text "Each level sets all four "
          inlineCode "--weave-animation-distance-*"
          text " tokens proportionally. Slide, shake, and bounce keyframes reference these tokens."
        ]

        div [ Attr.Style "overflow-x" "auto"; Margin.Bottom.medium ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [
              tr [] [
                tableHeaderCell "Level"
                tableHeaderCell "Multiplier"
                tableHeaderCell "Slide travel"
                tableHeaderCell "Shake travel"
                tableHeaderCell "Bounce travel"
              ]
            ]
            tbody [] [
              tr [] [
                tableCell [ inlineCode "AnimationDistance.extraSmall" ]
                tableCell [ text "0.25x" ]
                tableCell [ text "8px" ]
                tableCell [ text "2px" ]
                tableCell [ text "4px" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationDistance.small" ]
                tableCell [ text "0.5x" ]
                tableCell [ text "16px" ]
                tableCell [ text "4px" ]
                tableCell [ text "8px" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationDistance.medium" ]
                tableCell [ text "1x" ]
                tableCell [ text "32px" ]
                tableCell [ text "8px" ]
                tableCell [ text "16px" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationDistance.large" ]
                tableCell [ text "1.5x" ]
                tableCell [ text "48px" ]
                tableCell [ text "12px" ]
                tableCell [ text "24px" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationDistance.extraLarge" ]
                tableCell [ text "2x" ]
                tableCell [ text "64px" ]
                tableCell [ text "16px" ]
                tableCell [ text "32px" ]
              ]
            ]
          ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          textView (View.Const "AnimationScale levels")
        ]

        div [ Typography.body2; Margin.Bottom.small ] [
          text "Each level sets all four "
          inlineCode "--weave-animation-scale-*"
          text " tokens. Scale-in/out, scale-y-in/out, and pulse keyframes reference these tokens."
        ]

        div [ Attr.Style "overflow-x" "auto"; Margin.Bottom.medium ] [
          table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
            thead [] [
              tr [] [
                tableHeaderCell "Level"
                tableHeaderCell "Scale in/out"
                tableHeaderCell "Scale Y in/out"
                tableHeaderCell "Pulse"
              ]
            ]
            tbody [] [
              tr [] [
                tableCell [ inlineCode "AnimationScale.extraSmall" ]
                tableCell [ text "0.95" ]
                tableCell [ text "0.9" ]
                tableCell [ text "1.03" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationScale.small" ]
                tableCell [ text "0.9" ]
                tableCell [ text "0.8" ]
                tableCell [ text "1.05" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationScale.medium" ]
                tableCell [ text "0.8" ]
                tableCell [ text "0.65" ]
                tableCell [ text "1.1" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationScale.large" ]
                tableCell [ text "0.7" ]
                tableCell [ text "0.5" ]
                tableCell [ text "1.15" ]
              ]
              tr [] [
                tableCell [ inlineCode "AnimationScale.extraLarge" ]
                tableCell [ text "0.6" ]
                tableCell [ text "0.35" ]
                tableCell [ text "1.2" ]
              ]
            ]
          ]
        ]

        div [ Typography.body2 ] [
          text "Under "
          inlineCode "prefers-reduced-motion: reduce"
          text
            ", all distances collapse to 0px and all scales collapse to 1 (identity), regardless of which level is applied. The "
          inlineCode "--weave-stagger-delay"
          text " token (default 50ms) is also zeroed."
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Preferred: use the typed F# modules for predefined levels
div [
    AnimationEntrance.slideUpIn
    AnimationDistance.large        // all distance tokens scaled to 1.5x
] [ text "Longer slide distance" ]

div [
    AnimationEntrance.scaleIn
    AnimationScale.extraSmall     // very subtle scale (0.95)
] [ text "Gentle scale entrance" ]

// Escape hatch: override individual CSS custom properties directly
div [ Attr.Style "--weave-animation-distance-lg" "100px" ] [
    div [ AnimationEntrance.slideUpIn ] [
        text "Slides exactly 100px"
    ]
]

// Adjust stagger timing
div [ Attr.Style "--weave-stagger-delay" "100ms" ] [
    // Items stagger at 100ms intervals instead of 50ms
]"""

    Helpers.codeSampleSection "Design tokens" description content code

  let private transitionEasingSection () =
    let description =
      Helpers.bodyText
        "TransitionEasing overrides the CSS transition-timing-function on an element and its entire subtree. Unlike AnimationEasing (which targets keyframe animations), TransitionEasing affects CSS transitions — the property changes that components use for interactive state like tab indicators sliding, switches toggling, or accordions expanding. Drop it onto any component's attrs to change how its transitions feel."

    let selectedEasing = Var.Create<string option>(Some "Standard (default)")

    let allEasings = [
      "Standard (default)", Attr.Empty
      "Decelerate", TransitionEasing.decelerate
      "Accelerate", TransitionEasing.accelerate
      "Bounce", TransitionEasing.bounce
      "Smooth", TransitionEasing.smooth
    ]

    let easingItems =
      allEasings
      |> List.map (fun (label, _) -> SelectItem.create (text label, label, label))
      |> View.Const

    let easingAttrView =
      selectedEasing.View
      |> View.MapCached(fun sel ->
        sel
        |> Option.bind (fun label -> allEasings |> List.tryFind (fun (l, _) -> l = label))
        |> Option.map snd
        |> Option.defaultValue Attr.Empty)

    let content =
      div [] [
        div [ Margin.Bottom.small ] [
          Grid.create [
            GridItem.create (
              Select.create (
                easingItems,
                selectedEasing,
                labelText = View.Const "Transition Easing",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Variant.outlined; Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
            )
          ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          textView (View.Const "Tabs — indicator slides between tabs")
        ]

        easingAttrView
        |> Doc.BindView(fun easAttr ->
          Tabs.create (
            View.Const [
              TabItem.create (
                "Dashboard",
                div [ Typography.body1; Padding.All.small ] [ text "Dashboard content" ]
              )
              TabItem.create (
                "Settings",
                div [ Typography.body1; Padding.All.small ] [ text "Settings content" ]
              )
              TabItem.create (
                "Profile",
                div [ Typography.body1; Padding.All.small ] [ text "Profile content" ]
              )
            ],
            attrs = [ Tabs.Color.primary; Tabs.Variant.filled; easAttr ]
          ))

        div [ Typography.subtitle2; Margin.Top.small; Margin.Bottom.extraSmall ] [
          textView (View.Const "Switch — thumb slides between states")
        ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Attr.Style "gap" "24px"
          AlignItems.center
        ] [
          easingAttrView
          |> Doc.BindView(fun easAttr ->
            div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [
              Switch.create (
                Var.Create false,
                content = text "Primary",
                attrs = [ Switch.Color.primary; easAttr ]
              )
            ])

          easingAttrView
          |> Doc.BindView(fun easAttr ->
            div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [
              Switch.create (
                Var.Create false,
                content = text "Secondary",
                attrs = [ Switch.Color.secondary; easAttr ]
              )
            ])

          easingAttrView
          |> Doc.BindView(fun easAttr ->
            div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [
              Switch.create (
                Var.Create false,
                content = text "Success",
                attrs = [ Switch.Color.success; easAttr ]
              )
            ])
        ]
      ]

    let code =
      """open Weave
open Weave.CssHelpers.Animation


// Add spring easing to Tabs — the indicator bounces between tabs
Tabs.create(
    View.Const [
        TabItem.create ("Dashboard", dashboardContent)
        TabItem.create ("Settings", settingsContent)
        TabItem.create ("Profile", profileContent)
    ],
    attrs = [
        Tabs.Color.primary
        TransitionEasing.bounce   // springy indicator
    ]
)

// Add spring easing to a Switch toggle
Switch.create(
    isEnabled,
    content = text "Dark mode",
    attrs = [
        Switch.Color.primary
        TransitionEasing.bounce   // thumb overshoots slightly
    ]
)

// Available easing curves:
// TransitionEasing.standard     — default ease (no-op, useful for resetting)
// TransitionEasing.decelerate   — ease-out (fast start, slow end)
// TransitionEasing.accelerate   — ease-in (slow start, fast end)
// TransitionEasing.bounce       — spring overshoot (playful, physical)
// TransitionEasing.smooth       — smooth ease (subtle, refined)

// Works on any component with CSS transitions — just add to attrs.
// Falls back to standard easing under prefers-reduced-motion."""

    Helpers.codeSampleSection "Transition easing" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Animations"
        Helpers.bodyText
          "Apply entrance, exit, and emphasis animations to any element with composable CSS classes. Each animation is built from six axes — kind, duration, easing, distance, scale, and trigger — all composed the same way as direct Attr bindings. For JS-driven triggers (click, timer, reactive state), use the Animate module. For overriding transition curves on existing components (e.g., springy tab indicators), see TransitionEasing."
        Helpers.divider ()

        howItWorksSection ()
        Helpers.divider ()

        playgroundSection ()
        Helpers.divider ()

        distanceAndScaleSection ()
        Helpers.divider ()

        Helpers.sectionHeader "When do animations play?"

        div [ Typography.body1; Margin.Bottom.small ] [
          text
            "There are six ways to trigger an animation. The default is on mount. AnimationOn (hover, focus, hover-focus) is a CSS-only trigger that composes as direct Attr bindings just like kind, duration, and easing. The remaining triggers — click, timer, reactive state, and mount/unmount — are JS-based and live in the Animate module as Attr helpers."
        ]

        triggerOnMountSection ()
        Helpers.divider ()

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          textView (View.Const "CSS triggers (AnimationOn)")
        ]

        triggerOnHoverFocusSection ()
        Helpers.divider ()

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          textView (View.Const "JS triggers (Animate module)")
        ]

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

        Helpers.sectionHeader "Transition easing"

        div [ Typography.body1; Margin.Bottom.small ] [
          text
            "TransitionEasing is separate from AnimationEasing — it targets CSS transitions (property changes) rather than CSS animations (keyframes). Drop it onto any component to override how its internal transitions feel. The bounce curve uses "
          inlineCode "cubic-bezier(0.34, 1.56, 0.64, 1)"
          text " and falls back to standard easing under "
          inlineCode "prefers-reduced-motion: reduce"
          text "."
        ]

        transitionEasingSection ()
        Helpers.divider ()

        Helpers.sectionHeader "Customizing components"

        div [ Typography.body1; Margin.Bottom.small ] [
          text "These recipes show how to compose triggers and animations on real Weave components."
        ]

        animatedAlertsSection ()
        Helpers.divider ()
        animatedChipsSection ()
        Helpers.divider ()

        designTokensSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
