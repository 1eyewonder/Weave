namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module SwipeAreaExamples =

  let private basicExample () =
    let lastSwipe = Var.Create "Swipe left or right"

    Helpers.codeSampleSection
      "Detect a swipe"
      (Helpers.bodyText
        "Use onSwipe for an action after the pointer is released. It runs once if the swipe reaches the threshold; short gestures and taps do not call it. The default threshold is 100 pixels.")
      (SwipeArea.create (
        div [
          Padding.All.large
          Attr.Style "min-height" "120px"
          SurfaceColor.BackgroundColor.paper
        ] [
          p [ Typography.h6; Attr.Create "aria-live" "polite" ] [ textView lastSwipe.View ]
        ],
        (fun swipe -> lastSwipe.Set(sprintf "Swiped %A" swipe.Direction)),
        attrs = [ SwipeArea.TouchAction.horizontal ]
      ))
      """let lastSwipe = Var.Create "Swipe left or right"

SwipeArea.create (
  div [ Padding.All.large; Attr.Style "min-height" "120px"; SurfaceColor.BackgroundColor.paper ] [
    p [ Typography.h6; Attr.Create "aria-live" "polite" ] [ textView lastSwipe.View ]
  ],
  (fun swipe -> lastSwipe.Set (sprintf "Swiped %A" swipe.Direction)),
  attrs = [ SwipeArea.TouchAction.horizontal ]
)"""

  let private surface content =
    Card.create (
      CardContent.create (div [ Padding.All.large; Attr.Style "min-height" "180px" ] [ content ]),
      attrs = [ Card.Variant.filled ]
    )

  let private directionsExample () =
    let result = Var.Create "Swipe in any direction"
    let threshold = Var.Create 60
    let enabled = Var.Create true

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        Slider.create (threshold, min = 10, max = 150, labelText = View.Const "Swipe threshold (pixels)")
      ]

    let switches =
      div [ Attr.Class "docs-appearance__actions" ] [ Switch.create (enabled, text "Enable swipe gestures") ]

    let preview =
      SwipeArea.create (
        surface (p [ Typography.h6; Attr.Create "aria-live" "polite" ] [ textView result.View ]),
        (fun swipe -> result.Set(sprintf "%A · %.0f, %.0f px" swipe.Direction swipe.DeltaX swipe.DeltaY)),
        threshold = threshold.View,
        enabled = enabled.View,
        attrs = [ SwipeArea.TouchAction.none ]
      )

    let reset () =
      threshold.Set 60
      enabled.Set true
      result.Set "Swipe in any direction"

    Helpers.appearanceSection
      [ "swipe-directions" ]
      (Helpers.bodyText
        "Drag with a mouse, pen, or one finger. The strongest axis determines Left, Right, Up, or Down. Short gestures are ignored. This example reserves both axes, so page scrolling and pinch zoom are disabled inside the card.")
      controls
      (Some switches)
      preview
      reset
      (View.Map2 SwipeAreaAppearance.source threshold.View enabled.View)

  let private movementExample () =
    let movement = Var.Create "Ready to swipe"
    let reset () = movement.Set "Ready to swipe"

    Helpers.codeSampleSection
      "Track a drag (optional)"
      (Helpers.bodyText
        "Use the optional onDrag callback when you need live feedback while the pointer is held, such as following a finger. It runs on each movement, even below the swipe threshold. onSwipe still runs only once after a qualifying release; onSwipeCancel handles a tap, short gesture, or interrupted drag. This example resets its feedback in both cases. Deltas are measured from the start in browser screen coordinates: positive X is right and positive Y is down. Negate DeltaY for positive-up coordinates.")
      (SwipeArea.create (
        surface (p [ Typography.h6 ] [ textView movement.View ]),
        (fun _ -> reset ()),
        onDrag = (fun delta -> movement.Set(sprintf "X: %.0f px / Y: %.0f px" delta.DeltaX delta.DeltaY)),
        onSwipeCancel = reset,
        attrs = [ SwipeArea.TouchAction.none ]
      ))
      """let movement = Var.Create "Ready to swipe"
let reset () = movement.Set "Ready to swipe"

SwipeArea.create (
  Card.create (CardContent.create (
    div [ Padding.All.large; Attr.Style "min-height" "180px" ] [
      p [ Typography.h6 ] [ textView movement.View ]
    ]
  ), attrs = [ Card.Variant.filled ]),
  (fun _ -> reset ()),
  onDrag = (fun delta -> movement.Set (sprintf "X: %.0f px / Y: %.0f px" delta.DeltaX delta.DeltaY)),
  onSwipeCancel = reset,
  attrs = [ SwipeArea.TouchAction.none ]
)"""

  let private navigationExample () =
    let index = Var.Create 0
    let chapters = [ "Sketch the idea"; "Connect the pieces"; "Share your work" ]

    let move step =
      index.Set((index.Value + step + chapters.Length) % chapters.Length)

    Helpers.codeSampleSection
      "Swipe Navigation"
      (Helpers.bodyText
        "Swipe left or right to browse the notebook. TouchAction.horizontal preserves vertical page scrolling and pinch zoom. Always provide buttons or another keyboard-accessible way to perform the same actions. Buttons and form controls inside an area keep their normal behavior.")
      (div [] [
        SwipeArea.create (
          surface (
            div [ Attr.Create "aria-live" "polite" ] [
              p [ Typography.overline ] [ text "Project notebook" ]
              p [ Typography.h5 ] [ textView (index.View |> View.Map(fun i -> chapters[i])) ]
              p [ Typography.caption ] [
                textView (index.View |> View.Map(fun i -> sprintf "%d / 3" (i + 1)))
              ]
            ]
          ),
          (fun swipe ->
            match swipe.Direction with
            | SwipeArea.Direction.Left -> move 1
            | SwipeArea.Direction.Right -> move -1
            | _ -> ()),
          threshold = View.Const 60,
          attrs = [ SwipeArea.TouchAction.horizontal ]
        )
        div [ Flex.Flex.allSizes; Gap.All.g2; Margin.Top.large ] [
          Button.create (text "Previous", (fun () -> move -1), attrs = [ Button.Variant.outlined ])
          Button.create (text "Next", (fun () -> move 1), attrs = [ Button.Color.primary ])
        ]
      ])
      """let index = Var.Create 0
let chapters = [ "Sketch the idea"; "Connect the pieces"; "Share your work" ]
let move step = index.Set ((index.Value + step + chapters.Length) % chapters.Length)

div [] [
  SwipeArea.create(
    Card.create (CardContent.create (
      div [ Padding.All.large; Attr.Style "min-height" "180px" ] [
        div [ Attr.Create "aria-live" "polite" ] [
          p [ Typography.overline ] [ text "Project notebook" ]
          p [ Typography.h5 ] [ textView (index.View |> View.Map (fun i -> chapters[i])) ]
          p [ Typography.caption ] [ textView (index.View |> View.Map (fun i -> sprintf "%d / 3" (i + 1))) ]
        ]
      ]
    ), attrs = [ Card.Variant.filled ]),
    (fun swipe ->
        match swipe.Direction with
        | SwipeArea.Direction.Left -> move 1
        | SwipeArea.Direction.Right -> move -1
        | _ -> ()),
    threshold = View.Const 60,
    attrs = [ SwipeArea.TouchAction.horizontal ]
  )
  div [ Flex.Flex.allSizes; Gap.All.g2; Margin.Top.large ] [
    Button.create (text "Previous", (fun () -> move -1), attrs = [ Button.Variant.outlined ])
    Button.create (text "Next", (fun () -> move 1), attrs = [ Button.Color.primary ])
  ]
]"""

  let private drawerExample () =
    let isOpen = Var.Create false
    let allowEdgeSwipe = Var.Create true
    let lastAppSwipe = Var.Create "Swipe for the app gesture"
    let closeDrawer () = isOpen.Set false

    let edgeVisible =
      (isOpen.View, allowEdgeSwipe.View)
      ||> View.Map2(fun opened allowed -> not opened && allowed)

    let edge =
      edgeVisible
      |> Doc.BindView(fun visible ->
        if not visible then
          Doc.Empty
        else
          SwipeArea.create (
            Doc.Empty,
            (fun swipe ->
              if swipe.Direction = SwipeArea.Direction.Right then
                isOpen.Set true),
            attrs = [
              SwipeArea.TouchAction.horizontal
              Attr.Class "drawer-swipe-edge"
              Attr.Create "aria-hidden" "true"
              Attr.Style "position" "absolute"
              Attr.Style "top" "0"
              Attr.Style "bottom" "0"
              Attr.Style "left" "0"
              Attr.Style "width" "24px"
              Attr.Style "z-index" "var(--weave-zindex-drawer)"
              Attr.Style "border-left" "4px solid var(--palette-primary)"
            ]
          ))

    let drawer =
      Drawer.create (
        Button.create (text "Close navigation", closeDrawer),
        isOpen.View,
        header =
          SwipeArea.create (
            DrawerHeader.create (text "Swipe left to close"),
            (fun swipe ->
              if swipe.Direction = SwipeArea.Direction.Left then
                closeDrawer ()),
            attrs = [ SwipeArea.TouchAction.horizontal ]
          ),
        overlayClose = closeDrawer,
        width = 200,
        isFixed = false
      )

    let page =
      div [ Attr.Class "weave-main-content" ] [
        SwipeArea.create (
          div [ Padding.All.small; Attr.Style "min-height" "220px" ] [
            Button.create (text "Open navigation", fun () -> isOpen.Set true)
            p [ Typography.body1; Margin.Top.large; Attr.Create "role" "status" ] [
              textView lastAppSwipe.View
            ]
          ],
          (fun swipe -> lastAppSwipe.Set(sprintf "App swipe: %A" swipe.Direction)),
          attrs = [ SwipeArea.TouchAction.horizontal ]
        )
      ]

    let preview =
      div [] [
        Switch.create (allowEdgeSwipe, text "Enable drawer edge swipe")
        div [
          Attr.Style "position" "relative"
          Attr.Style "height" "220px"
          Attr.Style "overflow" "hidden"
          SurfaceColor.BackgroundColor.paper
        ] [ DrawerContainer.create (mainContent = page, leftDrawer = drawer); edge ]
      ]

    Helpers.codeSampleSection
      "Swipe to open a drawer"
      (Helpers.bodyText
        "Swipe right from the blue edge to open; swipe left on the drawer header to close. Elsewhere, the app handles its own swipes. Turn off the edge switch when the app needs that space. Buttons and the backdrop also work.")
      preview
      """let isOpen = Var.Create false
let allowEdgeSwipe = Var.Create true
let lastAppSwipe = Var.Create "Swipe for the app gesture"
let closeDrawer () = isOpen.Set false

let edgeVisible =
  (isOpen.View, allowEdgeSwipe.View)
  ||> View.Map2 (fun opened allowed -> not opened && allowed)

let edge =
  edgeVisible |> Doc.BindView (fun visible ->
    if not visible then Doc.Empty
    else
      SwipeArea.create (
        Doc.Empty,
        (fun swipe ->
          if swipe.Direction = SwipeArea.Direction.Right then isOpen.Set true),
        attrs = [
          SwipeArea.TouchAction.horizontal
          Attr.Class "drawer-swipe-edge"
          Attr.Create "aria-hidden" "true"
          Attr.Style "position" "absolute"
          Attr.Style "top" "0"
          Attr.Style "bottom" "0"
          Attr.Style "left" "0"
          Attr.Style "width" "24px"
          Attr.Style "z-index" "var(--weave-zindex-drawer)"
          Attr.Style "border-left" "4px solid var(--palette-primary)"
        ]
      ))

let drawer =
  Drawer.create (
    Button.create (text "Close navigation", closeDrawer),
    isOpen.View,
    header = SwipeArea.create (
      DrawerHeader.create (text "Swipe left to close"),
      (fun swipe ->
        if swipe.Direction = SwipeArea.Direction.Left then closeDrawer ()),
      attrs = [ SwipeArea.TouchAction.horizontal ]
    ),
    overlayClose = closeDrawer,
    width = 200,
    isFixed = false
  )

let page =
  div [ Attr.Class "weave-main-content" ] [
    SwipeArea.create (
      div [ Padding.All.small; Attr.Style "min-height" "220px" ] [
        Button.create (text "Open navigation", fun () -> isOpen.Set true)
        p [ Typography.body1; Margin.Top.large; Attr.Create "role" "status" ] [
          textView lastAppSwipe.View
        ]
      ],
      (fun swipe -> lastAppSwipe.Set (sprintf "App swipe: %A" swipe.Direction)),
      attrs = [ SwipeArea.TouchAction.horizontal ]
    )
  ]

div [] [
  Switch.create (allowEdgeSwipe, text "Enable drawer edge swipe")
  div [
    Attr.Style "position" "relative"
    Attr.Style "height" "220px"
    Attr.Style "overflow" "hidden"
    SurfaceColor.BackgroundColor.paper
  ] [
    DrawerContainer.create (mainContent = page, leftDrawer = drawer)
    edge
  ]
]"""

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "SwipeArea is an unstyled content wrapper. Native browser gestures remain enabled unless you supply a TouchAction attribute. Browser takeover cancels the gesture. Only the primary pointer and left mouse button start a swipe. Nested areas handle their own gestures. Add data-weave-swipe-ignore to custom interactive content to exclude it.")
      [
        Helpers.apiTable "SwipeArea.create" [
          Helpers.apiParam "content" "Doc" "" "Content to wrap"
          Helpers.apiParam
            "onSwipe"
            "SwipeArea.Swipe -> unit"
            ""
            "Called once on release when the dominant-axis distance reaches the threshold; never for taps"
          Helpers.apiParam
            "?threshold"
            "View<int>"
            "View.Const 100"
            "Minimum distance in CSS pixels; negative values are treated as zero"
          Helpers.apiParam
            "?enabled"
            "View<bool>"
            "View.Const true"
            "Enables detection; disabling cancels the current gesture"
          Helpers.apiParam
            "?onDrag"
            "SwipeArea.Movement -> unit"
            "omitted"
            "While held, called on each movement with total X/Y displacement from the start, including below the threshold"
          Helpers.apiParam
            "?onSwipeCancel"
            "unit -> unit"
            "omitted"
            "Reset feedback after a short gesture, browser cancellation, capture loss, or disabling"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Root attributes, layout, and touch behavior"
        ]
        Helpers.apiTable "SwipeArea.Swipe" [
          Helpers.apiParam
            "Direction"
            "SwipeArea.Direction"
            ""
            "Left, Right, Up, or Down; equal diagonals choose the vertical axis"
          Helpers.apiParam
            "DeltaX"
            "float"
            ""
            "Horizontal displacement from start in CSS pixels; right is positive"
          Helpers.apiParam
            "DeltaY"
            "float"
            ""
            "Vertical displacement from start in CSS pixels; down is positive"
        ]
        Helpers.apiTable "SwipeArea.Movement" [
          Helpers.apiParam "DeltaX" "float" "" "Horizontal displacement from start in CSS pixels"
          Helpers.apiParam
            "DeltaY"
            "float"
            ""
            "Vertical displacement from start in CSS pixels; down is positive"
        ]
        Helpers.styleModuleTable "SwipeArea.TouchAction" [
          "horizontal", "Reserve horizontal gestures; allow vertical scrolling and pinch zoom"
          "vertical", "Reserve vertical gestures; allow horizontal scrolling and pinch zoom"
          "none", "Reserve both axes and disable browser pinch zoom inside the area"
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Swipe Area"
        Helpers.bodyText
          "Use onSwipe to respond to a completed swipe. Add onDrag only when you need live movement before release."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        directionsExample ()
        Helpers.divider ()
        movementExample ()
        Helpers.divider ()
        navigationExample ()
        Helpers.divider ()
        drawerExample ()
        Helpers.bodyText
          "The edge reserves 24 pixels while enabled. Remove it when the app needs that space; disabling SwipeArea alone keeps its touch-action styling. Document-level handlers can filter events from .drawer-swipe-edge. Browser or system edge gestures may take precedence."
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
