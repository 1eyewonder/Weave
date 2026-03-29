namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

open Weave.Icons.MaterialSymbols

[<JavaScript>]
module DrawerExamples =

  let private navList () =
    let item (label: string) =
      div [ Padding.Horizontal.small; Padding.Vertical.extraSmall ] [
        div [ Typography.body1 ] [ text label ]
      ]

    div [] [ item "Dashboard"; item "Profile"; item "Settings"; item "Reports" ]

  /// A nav list for the Mini drawer example. Each item shows a leading icon
  /// at all times. Text labels are always in the DOM but hidden via the
  /// weave-drawer-mini-label CSS class — revealed by the drawer open/hover
  /// state without requiring reactive F# wiring in the content.
  let private miniNavList isOpen =
    let item (isOpen: View<bool>) (icon: Icon) (label: string) =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Horizontal.small
        Padding.Vertical.extraSmall

        isOpen
        |> View.Map(fun isOpen -> if isOpen then "left" else "center")
        |> Attr.DynamicStyle "justify-content"

        Attr.Style "gap" "16px"
        Attr.Style "cursor" "pointer"
      ] [
        Icon.create (icon)

        isOpen
        |> Doc.BindView(fun isOpen ->
          if isOpen then
            span [ Typography.body1; Attr.Class "weave-drawer-mini-label" ] [ text label ]
          else
            Doc.Empty)
      ]

    div [] [
      item isOpen (Icon.Text Text.Dashboard) "Dashboard"
      item isOpen (Icon.Social Social.Person) "Profile"
      item isOpen (Icon.UiActions UiActions.Settings) "Settings"
      item isOpen (Icon.Business Business.BarChart) "Reports"
    ]

  let private toolbar (label: string) (toggleButton: Doc) =
    div [
      Flex.Flex.allSizes
      AlignItems.center
      Padding.Horizontal.medium
      Padding.Vertical.extraSmall
      Attr.Style "gap" "12px"
    ] [ toggleButton; div [ Typography.h6 ] [ text label ] ]

  let private pageContent () =
    div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Padding.All.small ] [
      yield!
        [ 1..5 ]
        |> List.map (fun i -> div [ Typography.body2 ] [ text (sprintf "Content paragraph %d" i) ])
    ]

  /// Wraps a demo in a bounded, relatively-positioned box so that
  /// position:absolute drawers (isFixed = false) stay inside the preview.
  let private demoBox (child: Doc) =
    div [
      Attr.Style "position" "relative"
      Attr.Style "height" "280px"
      Attr.Style "overflow" "hidden"
      Attr.Style "isolation" "isolate"
      SurfaceColor.BackgroundColor.background
      BorderRadius.All.small
    ] [ child ]

  let private filledButton (label: string) (onClick: unit -> unit) =
    Button.primary (text label, onClick = onClick, attrs = [ Button.Variant.filled ])

  let private temporaryExample () =
    let description =
      Helpers.bodyText
        "A Temporary drawer overlays the content when open. Clicking the \
         backdrop or the toggle button closes it. It does not shift the AppBar \
         or main content."

    let isOpen = Var.Create false

    let preview =
      demoBox (
        DrawerContainer.create (
          mainContent =
            Doc.Concat [
              AppBar.create (
                toolbar
                  "Temporary Drawer"
                  (Button.primary (
                    isOpen.View
                    |> View.Map(fun o -> if o then "Close" else "Open")
                    |> View.Map text
                    |> Doc.EmbedView,
                    onClick = (fun () -> Var.Set isOpen (not isOpen.Value)),
                    attrs = [ Button.Variant.filled ]
                  )),
                attrs = [ BrandColor.BackgroundColor.primary ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.create (
              navList (),
              isOpen.View,
              position = Drawer.Position.Left,
              overlayClose = (fun () -> Var.Set isOpen false),
              isFixed = false
            )
        )
      )

    let code =
      """let isOpen = Var.Create false

DrawerContainer.create(
    mainContent = Doc.Concat [
        AppBar.create(toolbar "My App" openButton)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.create(
            navList,
            isOpen.View,
            position = Drawer.Position.Left,
            overlayClose = (fun () -> Var.Set isOpen false)
        )
)"""

    Helpers.codeSampleSection "Temporary" description preview code

  let private persistentExample () =
    let description =
      Helpers.bodyText
        "A Persistent drawer stays open alongside the content; it shifts the \
         AppBar and main content to the side rather than overlaying them. \
         There is no backdrop."

    let isOpen = Var.Create false

    let toggleButton =
      Button.primary (
        isOpen.View
        |> View.Map(fun o -> if o then "Close" else "Open")
        |> View.Map text
        |> Doc.EmbedView,
        onClick = (fun () -> Var.Set isOpen (not isOpen.Value)),
        attrs = [ Button.Variant.filled ]
      )

    let preview =
      demoBox (
        DrawerContainer.create (
          mainContent =
            Doc.Concat [
              AppBar.create (
                toolbar "Persistent Drawer" toggleButton,
                attrs = [ BrandColor.BackgroundColor.secondary ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.create (
              navList (),
              isOpen.View,
              variant = Drawer.Variant.Persistent,
              position = Drawer.Position.Left,
              isFixed = false
            )
        )
      )

    let code =
      """let isOpen = Var.Create false

DrawerContainer.create(
    mainContent = Doc.Concat [
        AppBar.create(toolbar "My App" toggleButton)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.create(
            navList,
            isOpen.View,
            variant = Drawer.Variant.Persistent,
            position = Drawer.Position.Left
            // No overlayClose — persistent drawers have no backdrop
        )
)"""

    Helpers.codeSampleSection "Persistent" description preview code

  let private responsiveExample () =
    let description =
      Helpers.bodyText
        "A Responsive drawer acts like Persistent above its configured \
         breakpoint and like Temporary below it. Use the Breakpoint parameter \
         to set the switch point (default: MD)."

    let isOpen = Var.Create false

    let toggleButton =
      filledButton "Toggle" (fun () -> Var.Set isOpen (not isOpen.Value))

    let preview =
      demoBox (
        DrawerContainer.create (
          mainContent =
            Doc.Concat [
              AppBar.create (
                toolbar "Responsive Drawer" toggleButton,
                attrs = [ BrandColor.BackgroundColor.tertiary ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.create (
              navList (),
              isOpen.View,
              variant = Drawer.Variant.Responsive,
              position = Drawer.Position.Left,
              breakpoint = Drawer.DrawerBreakpoint.At Breakpoint.Medium,
              overlayClose = (fun () -> Var.Set isOpen false),
              isFixed = false
            )
        )
      )

    let code =
      """let isOpen = Var.Create false

DrawerContainer.create(
    mainContent = Doc.Concat [
        AppBar.create(...)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.create(
            navList,
            isOpen.View,
            variant = Drawer.Variant.Responsive,
            position = Drawer.Position.Left,
            breakpoint = Drawer.DrawerBreakpoint.At Breakpoint.Medium,
            overlayClose = (fun () -> Var.Set isOpen false)
        )
)"""

    Helpers.codeSampleSection "Responsive" description preview code

  let private miniExample () =
    let description =
      Helpers.bodyText
        "A Mini drawer collapses to a narrow strip (icon rail) when closed and \
         expands to full width when opened. Enable \"Hover expand\" to have the \
         drawer expand as an overlay on pointer hover — without shifting the \
         layout."

    let isOpen = Var.Create false
    let hoverEnabled = Var.Create false

    let controls =
      Doc.Concat [
        Button.primary (
          isOpen.View
          |> View.Map(fun o -> if o then "Collapse" else "Expand")
          |> View.Map text
          |> Doc.EmbedView,
          onClick = (fun () -> Var.Set isOpen (not isOpen.Value)),
          attrs = [ Button.Variant.filled ]
        )
        Switch.create (hoverEnabled, div [ Typography.body1 ] [ text "Hover expand" ])
      ]

    let preview =
      demoBox (
        DrawerContainer.create (
          mainContent =
            Doc.Concat [
              AppBar.create (toolbar "Mini Drawer" controls, attrs = [ BrandColor.BackgroundColor.info ])
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.create (
              miniNavList isOpen.View,
              isOpen.View,
              variant = Drawer.Variant.Mini,
              position = Drawer.Position.Left,
              breakpoint = Drawer.DrawerBreakpoint.Always,
              expandOnHover = hoverEnabled.View,
              isFixed = false,
              attrs = [
                on.mouseEnter (fun _ _ ->
                  if hoverEnabled.Value then
                    Var.Set isOpen true)
                on.mouseLeave (fun _ _ ->
                  if hoverEnabled.Value then
                    Var.Set isOpen false)
              ]
            )
        )
      )

    let code =
      """let isOpen = Var.Create false
let hoverEnabled = Var.Create false

DrawerContainer.create(
    mainContent = Doc.Concat [
        AppBar.create(...)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.create(
            miniNavList (),
            isOpen.View,
            variant = Drawer.Variant.Mini,
            position = Drawer.Position.Left,
            breakpoint = Drawer.DrawerBreakpoint.Always,
            expandOnHover = hoverEnabled.View
        )
)"""

    Helpers.codeSampleSection "Mini" description preview code

  let private headerExample () =
    let description =
      Helpers.bodyText
        "An optional DrawerHeader renders a toolbar-height row at the top of \
         the drawer, aligned with the AppBar. Pass dense = true to match a \
         dense AppBar."

    let isOpen = Var.Create false

    let preview =
      demoBox (
        DrawerContainer.create (
          mainContent =
            Doc.Concat [
              AppBar.create (
                toolbar
                  "Drawer with Header"
                  (filledButton "Toggle" (fun () -> Var.Set isOpen (not isOpen.Value))),
                attrs = [ BrandColor.BackgroundColor.error ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.create (
              navList (),
              isOpen.View,
              position = Drawer.Position.Left,
              header =
                DrawerHeader.create (
                  div [ Flex.Flex.allSizes; AlignItems.center; Padding.Horizontal.small ] [
                    div [ Typography.h6 ] [ text "Navigation" ]
                  ]
                ),
              overlayClose = (fun () -> Var.Set isOpen false),
              isFixed = false
            )
        )
      )

    let code =
      """Drawer.create(
    navList,
    isOpen.View,
    position = Drawer.Position.Left,
    header =
        DrawerHeader.create(
            div [] [ div [ Typography.h6 ] [ text "Navigation" ] ]
        ),
    overlayClose = (fun () -> Var.Set isOpen false)
)"""

    Helpers.codeSampleSection "Drawer Header" description preview code

  let private clipModeExample () =
    let description =
      Helpers.bodyText
        "ClipMode controls how the drawer relates to the AppBar vertically. \
         AppBar: place the AppBar outside (above) the DrawerContainer so it spans the full width — \
         the drawer fills only the remaining height. \
         FullHeight (default): AppBar lives inside mainContent, drawer extends to the full container height."

    let clipMode = Var.Create Drawer.ClipMode.AppBar
    let isOpen = Var.Create true

    let toggleButton =
      isOpen.View
      |> Doc.BindView(fun opened ->
        Button.primary (
          text (if opened then "Close Drawer" else "Open Drawer"),
          onClick = (fun () -> Var.Update isOpen not),
          attrs = [ Button.Variant.outlined ]
        ))

    let clipButton (mode: Drawer.ClipMode) (label: string) =
      clipMode.View
      |> Doc.BindView(fun current ->
        Button.primary (
          text label,
          onClick = (fun () -> Var.Set clipMode mode),
          attrs = [
            if current = mode then
              Button.Variant.filled
            else
              Button.Variant.outlined
          ]
        ))

    let preview =
      div [] [
        // Controls row: clip mode selector + open/close toggle
        div [
          Flex.Flex.allSizes
          Margin.Bottom.extraSmall
          Attr.Style "gap" "8px"
          Attr.Style "flex-wrap" "wrap"
        ] [
          clipButton Drawer.ClipMode.AppBar "AppBar"
          clipButton Drawer.ClipMode.FullHeight "FullHeight"
          toggleButton
        ]

        // Flex-column container: AppBar sits above DrawerContainer in AppBar mode,
        // or inside mainContent in FullHeight mode.
        div [
          Attr.Style "display" "flex"
          Attr.Style "flex-direction" "column"
          Attr.Style "height" "360px"
          Attr.Style "overflow" "hidden"
          SurfaceColor.BackgroundColor.background
        ] [
          clipMode.View
          |> Doc.BindView(fun mode ->
            match mode with
            | Drawer.ClipMode.AppBar ->
              // 3-panel layout: AppBar spans full width; DrawerContainer fills the rest.
              Doc.Concat [
                AppBar.create (
                  toolbar "Clip Mode Demo" (div [] []),
                  attrs = [ BrandColor.BackgroundColor.warning ]
                )
                DrawerContainer.create (
                  mainContent = div [ cl "weave-main-content" ] [ pageContent () ],
                  leftDrawer =
                    Drawer.create (
                      navList (),
                      isOpen.View,
                      variant = Drawer.Variant.Persistent,
                      position = Drawer.Position.Left,
                      isFixed = false
                    ),
                  attrs = [ Attr.Style "flex" "1"; Attr.Style "min-height" "0" ]
                )
              ]
            | Drawer.ClipMode.FullHeight
            | _ ->
              // AppBar is inside mainContent; drawer extends the full container height.
              DrawerContainer.create (
                mainContent =
                  Doc.Concat [
                    AppBar.create (
                      toolbar "Clip Mode Demo" (div [] []),
                      attrs = [ BrandColor.BackgroundColor.warning ]
                    )
                    div [ cl "weave-main-content" ] [ pageContent () ]
                  ],
                leftDrawer =
                  Drawer.create (
                    navList (),
                    isOpen.View,
                    variant = Drawer.Variant.Persistent,
                    position = Drawer.Position.Left,
                    clipMode = Drawer.ClipMode.FullHeight,
                    isFixed = false
                  ),
                attrs = [ Attr.Style "flex" "1"; Attr.Style "min-height" "0" ]
              ))
        ]
      ]

    let code =
      """// AppBar mode — AppBar spans full width above the DrawerContainer:
AppBar.create(toolbar ...)
DrawerContainer.create(
    mainContent = div [ cl "weave-main-content" ] [ pageContent ],
    leftDrawer = Drawer.create(navList, isOpen.View, variant = Drawer.Variant.Persistent)
)

// FullHeight mode (default) — AppBar is inside mainContent, drawer covers full height:
DrawerContainer.create(
    mainContent = Doc.Concat [ AppBar.create(toolbar ...); div [ cl "weave-main-content" ] [ pageContent ] ],
    leftDrawer = Drawer.create(navList, isOpen.View,
                                         variant = Drawer.Variant.Persistent,
                                         clipMode = Drawer.ClipMode.FullHeight)
)"""

    Helpers.codeSampleSection "Clip Mode" description preview code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Drawer"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Drawers provide navigation panels that can slide in from any edge of \
             the screen. Wrap side drawers in a DrawerContainer so the AppBar and \
             main content area shift automatically."
        ]
        Helpers.divider ()
        temporaryExample ()
        Helpers.divider ()
        persistentExample ()
        Helpers.divider ()
        responsiveExample ()
        Helpers.divider ()
        miniExample ()
        Helpers.divider ()
        headerExample ()
        Helpers.divider ()
        clipModeExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
