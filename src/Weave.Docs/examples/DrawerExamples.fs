namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module DrawerExamples =

  let private navList () =
    let item (label: string) =
      div [
        cls [
          yield! Padding.toClasses Padding.Horizontal.small
          yield! Padding.toClasses Padding.Vertical.extraSmall
        ]
      ] [ Body1.Div(label) ]

    div [] [ item "Dashboard"; item "Profile"; item "Settings"; item "Reports" ]

  /// A nav list for the Mini drawer example. Each item shows a leading icon
  /// at all times. Text labels are always in the DOM but hidden via the
  /// weave-drawer-mini-label CSS class — revealed by the drawer open/hover
  /// state without requiring reactive F# wiring in the content.
  let private miniNavList isOpen =
    let item (isOpen: View<bool>) (icon: Icon) (label: string) =
      div [
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          yield! Padding.toClasses Padding.Horizontal.small
          yield! Padding.toClasses Padding.Vertical.extraSmall
        ]

        isOpen
        |> View.Map(fun isOpen -> if isOpen then "left" else "center")
        |> Attr.DynamicStyle "justify-content"

        Attr.Style "gap" "16px"
        Attr.Style "cursor" "pointer"
      ] [
        Icon.Create(icon)

        isOpen
        |> Doc.BindView(fun isOpen ->
          if isOpen then
            Body1.Span(label, attrs = [ Attr.Class "weave-drawer-mini-label" ])
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
      cls [
        Flex.Flex.allSizes
        AlignItems.toClass AlignItems.Center
        yield! Padding.toClasses Padding.Horizontal.medium
        yield! Padding.toClasses Padding.Vertical.extraSmall
      ]
      Attr.Style "gap" "12px"
    ] [ toggleButton; H6.Div(label) ]

  let private pageContent () =
    div [
      cls [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        yield! Padding.toClasses Padding.All.small
      ]
    ] [
      yield! [ 1..5 ] |> List.map (fun i -> Body2.Div(sprintf "Content paragraph %d" i))
    ]

  /// Wraps a demo in a bounded, relatively-positioned box so that
  /// position:absolute drawers (isFixed = false) stay inside the preview.
  let private demoBox (child: Doc) =
    div [
      Attr.Style "position" "relative"
      Attr.Style "height" "280px"
      Attr.Style "overflow" "hidden"
      Attr.Style "isolation" "isolate"
      SurfaceColor.toBackgroundColor SurfaceColor.Background
      BorderRadius.toClass BorderRadius.All.small |> cl
    ] [ child ]

  let private filledButton (label: string) (onClick: unit -> unit) =
    Button.Create(
      text label,
      onClick = onClick,
      attrs = [
        cls [
          Button.Color.toClass BrandColor.Primary
          Button.Variant.toClass Button.Variant.Filled
        ]
      ]
    )

  let private temporaryExample () =
    let description =
      Helpers.bodyText
        "A Temporary drawer overlays the content when open. Clicking the \
         backdrop or the toggle button closes it. It does not shift the AppBar \
         or main content."

    let isOpen = Var.Create false

    let preview =
      demoBox (
        DrawerContainer.Create(
          mainContent =
            Doc.Concat [
              AppBar.Create(
                toolbar
                  "Temporary Drawer"
                  (Button.Create(
                    isOpen.View
                    |> View.Map(fun o -> if o then "Close" else "Open")
                    |> View.Map text
                    |> Doc.EmbedView,
                    onClick = (fun () -> Var.Set isOpen (not isOpen.Value)),
                    attrs = [
                      cls [
                        Button.Color.toClass BrandColor.Primary
                        Button.Variant.toClass Button.Variant.Filled
                      ]
                    ]
                  )),
                position = AppBar.Position.Static,
                attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.CreateTemporary(
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

DrawerContainer.Create(
    mainContent = Doc.Concat [
        AppBar.Create(toolbar "My App" openButton, position = AppBar.Position.Static)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.CreateTemporary(
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
      Button.Create(
        isOpen.View
        |> View.Map(fun o -> if o then "Close" else "Open")
        |> View.Map text
        |> Doc.EmbedView,
        onClick = (fun () -> Var.Set isOpen (not isOpen.Value)),
        attrs = [
          cls [
            Button.Color.toClass BrandColor.Primary
            Button.Variant.toClass Button.Variant.Filled
          ]
        ]
      )

    let preview =
      demoBox (
        DrawerContainer.Create(
          mainContent =
            Doc.Concat [
              AppBar.Create(
                toolbar "Persistent Drawer" toggleButton,
                position = AppBar.Position.Static,
                attrs = [ BrandColor.toBackgroundColor BrandColor.Secondary ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.CreatePersistent(navList (), isOpen.View, position = Drawer.Position.Left, isFixed = false)
        )
      )

    let code =
      """let isOpen = Var.Create false

DrawerContainer.Create(
    mainContent = Doc.Concat [
        AppBar.Create(toolbar "My App" toggleButton, position = AppBar.Position.Static)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.CreatePersistent(
            navList,
            isOpen.View,
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
        DrawerContainer.Create(
          mainContent =
            Doc.Concat [
              AppBar.Create(
                toolbar "Responsive Drawer" toggleButton,
                position = AppBar.Position.Static,
                attrs = [ BrandColor.toBackgroundColor BrandColor.Tertiary ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.CreateResponsive(
              navList (),
              isOpen.View,
              position = Drawer.Position.Left,
              breakpoint = Drawer.DrawerBreakpoint.At Breakpoint.Medium,
              overlayClose = (fun () -> Var.Set isOpen false),
              isFixed = false
            )
        )
      )

    let code =
      """let isOpen = Var.Create false

DrawerContainer.Create(
    mainContent = Doc.Concat [
        AppBar.Create(...)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.CreateResponsive(
            navList,
            isOpen.View,
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
        Button.Create(
          isOpen.View
          |> View.Map(fun o -> if o then "Collapse" else "Expand")
          |> View.Map text
          |> Doc.EmbedView,
          onClick = (fun () -> Var.Set isOpen (not isOpen.Value)),
          attrs = [
            cls [
              Button.Color.toClass BrandColor.Primary
              Button.Variant.toClass Button.Variant.Filled
            ]
          ]
        )
        Switch.Create(hoverEnabled, displayText = View.Const "Hover expand")
      ]

    let preview =
      demoBox (
        DrawerContainer.Create(
          mainContent =
            Doc.Concat [
              AppBar.Create(
                toolbar "Mini Drawer" controls,
                position = AppBar.Position.Static,
                attrs = [ BrandColor.toBackgroundColor BrandColor.Info ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.CreateMini(
              miniNavList isOpen.View,
              isOpen.View,
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

DrawerContainer.Create(
    mainContent = Doc.Concat [
        AppBar.Create(...)
        div [ cl "weave-main-content" ] [ pageContent ]
    ],
    leftDrawer =
        Drawer.CreateMini(
            miniNavList (),
            isOpen.View,
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
        DrawerContainer.Create(
          mainContent =
            Doc.Concat [
              AppBar.Create(
                toolbar
                  "Drawer with Header"
                  (filledButton "Toggle" (fun () -> Var.Set isOpen (not isOpen.Value))),
                position = AppBar.Position.Static,
                attrs = [ BrandColor.toBackgroundColor BrandColor.Error ]
              )
              div [ cl "weave-main-content" ] [ pageContent () ]
            ],
          leftDrawer =
            Drawer.CreateTemporary(
              navList (),
              isOpen.View,
              position = Drawer.Position.Left,
              header =
                DrawerHeader.Create(
                  div [
                    cls [
                      Flex.Flex.allSizes
                      AlignItems.toClass AlignItems.Center
                      yield! Padding.toClasses Padding.Horizontal.small
                    ]
                  ] [ H6.Div("Navigation") ]
                ),
              overlayClose = (fun () -> Var.Set isOpen false),
              isFixed = false
            )
        )
      )

    let code =
      """Drawer.CreateTemporary(
    navList,
    isOpen.View,
    position = Drawer.Position.Left,
    header =
        DrawerHeader.Create(
            div [] [ H6.Div("Navigation") ]
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
        Button.Create(
          text (if opened then "Close Drawer" else "Open Drawer"),
          onClick = (fun () -> Var.Update isOpen not),
          attrs = [
            cls [
              Button.Color.toClass BrandColor.Primary
              Button.Variant.toClass Button.Variant.Outlined
            ]
          ]
        ))

    let clipButton (mode: Drawer.ClipMode) (label: string) =
      clipMode.View
      |> Doc.BindView(fun current ->
        Button.Create(
          text label,
          onClick = (fun () -> Var.Set clipMode mode),
          attrs = [
            cls [
              Button.Color.toClass BrandColor.Primary
              if current = mode then
                Button.Variant.toClass Button.Variant.Filled
              else
                Button.Variant.toClass Button.Variant.Outlined
            ]
          ]
        ))

    let preview =
      div [] [
        // Controls row: clip mode selector + open/close toggle
        div [
          cls [ Flex.Flex.allSizes; yield! Margin.toClasses Margin.Bottom.extraSmall ]
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
          SurfaceColor.toBackgroundColor SurfaceColor.Background
        ] [
          clipMode.View
          |> Doc.BindView(fun mode ->
            match mode with
            | Drawer.ClipMode.AppBar ->
              // 3-panel layout: AppBar spans full width; DrawerContainer fills the rest.
              Doc.Concat [
                AppBar.Create(
                  toolbar "Clip Mode Demo" (div [] []),
                  position = AppBar.Position.Static,
                  attrs = [ BrandColor.toBackgroundColor BrandColor.Warning ]
                )
                DrawerContainer.Create(
                  mainContent = div [ cl "weave-main-content" ] [ pageContent () ],
                  leftDrawer =
                    Drawer.CreatePersistent(
                      navList (),
                      isOpen.View,
                      position = Drawer.Position.Left,
                      isFixed = false
                    ),
                  attrs = [ Attr.Style "flex" "1"; Attr.Style "min-height" "0" ]
                )
              ]
            | Drawer.ClipMode.FullHeight
            | _ ->
              // AppBar is inside mainContent; drawer extends the full container height.
              DrawerContainer.Create(
                mainContent =
                  Doc.Concat [
                    AppBar.Create(
                      toolbar "Clip Mode Demo" (div [] []),
                      position = AppBar.Position.Static,
                      attrs = [ BrandColor.toBackgroundColor BrandColor.Warning ]
                    )
                    div [ cl "weave-main-content" ] [ pageContent () ]
                  ],
                leftDrawer =
                  Drawer.CreatePersistent(
                    navList (),
                    isOpen.View,
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
AppBar.Create(toolbar ...)
DrawerContainer.Create(
    mainContent = div [ cl "weave-main-content" ] [ pageContent ],
    leftDrawer = Drawer.CreatePersistent(navList, isOpen.View)
)

// FullHeight mode (default) — AppBar is inside mainContent, drawer covers full height:
DrawerContainer.Create(
    mainContent = Doc.Concat [ AppBar.Create(toolbar ...); div [ cl "weave-main-content" ] [ pageContent ] ],
    leftDrawer = Drawer.CreatePersistent(navList, isOpen.View,
                                         clipMode = Drawer.ClipMode.FullHeight)
)"""

    Helpers.codeSampleSection "Clip Mode" description preview code

  let render () =
    Container.Create(
      div [] [
        H1.Div("Drawer Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "Drawers provide navigation panels that can slide in from any edge of \
           the screen. Wrap side drawers in a DrawerContainer so the AppBar and \
           main content area shift automatically.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
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
      maxWidth = Container.MaxWidth.Large
    )
