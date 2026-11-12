namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module DrawerExamples =

  let private workspace (isOpen: Var<bool>) (selected: Var<string>) =
    let toggle () = Var.Update isOpen not

    let icon symbol =
      Icon.create (symbol, attrs = [ Attr.Create "aria-hidden" "true" ])

    let item label symbol onClick =
      let active = View.Map2 (=) selected.View label

      Button.create (
        Doc.Concat [ icon symbol; span [ cl "weave-drawer-mini-label" ] [ textView label ] ],
        onClick = onClick,
        attrs = [
          Button.Width.full
          Attr.Style "min-width" "0"
          Attr.Style "justify-content" "flex-start"
          Attr.Dynamic "aria-label" label
          Attr.Dynamic "aria-current" (active |> View.Map(fun value -> if value then "page" else "false"))
          Attr.DynamicClassPred "weave-button--primary" active
          Attr.DynamicClassPred "weave-button--filled" active
        ]
      )

    let toggleLabel =
      isOpen.View
      |> View.Map(fun value -> if value then "Close drawer" else "Open drawer")

    let navigation =
      nav [
        Attr.Create "aria-label" "Workspace navigation"
        Attr.Style "padding" "8px"
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Gap.All.g2
      ] [
        item toggleLabel (Icon.UiActions UiActions.MenuOpen) toggle
        item (View.Const "Projects") (Icon.Text Text.FolderOpen) (fun () -> Var.Set selected "Projects")
        item (View.Const "Activity") (Icon.Action Action.Schedule) (fun () -> Var.Set selected "Activity")
      ]

    let toolbar =
      AppBar.create (
        div [
          Flex.Flex.allSizes
          AlignItems.center
          Gap.All.g2
          Attr.Style "padding" "8px"
          Attr.Style "min-height" "var(--appbar-height)"
          Attr.Style "box-sizing" "border-box"
        ] [
          IconButton.create (
            icon (Icon.UiActions UiActions.Menu),
            onClick = toggle,
            attrs = [
              Attr.Dynamic "aria-label" toggleLabel
              Attr.Dynamic "aria-expanded" (isOpen.View |> View.Map string)
            ]
          )
          span [ Typography.subtitle1 ] [ text "Workspace" ]
        ]
      )

    let detail =
      selected.View
      |> View.Map(fun page ->
        if page = "Projects" then
          "Component library"
        else
          "Updated an example")

    let page =
      div [
        cl "weave-main-content"
        Attr.Style "min-inline-size" "160px"
        Padding.All.extraSmall
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Gap.All.g3
      ] [
        h3 [ Typography.h5 ] [ textView selected.View ]
        p [ Typography.body2 ] [ text "Your workspace at a glance." ]
        Card.create (
          CardContent.create (
            Doc.Concat [
              div [ Attr.Style "flex" "1 1 12rem"; Attr.Style "min-width" "0" ] [
                h4 [ Typography.subtitle2 ] [ text "Docs refresh" ]
                p [ Typography.body2 ] [ textView detail ]
              ]
              Chip.create (
                text "In progress",
                attrs = [
                  Chip.Color.secondary
                  Chip.Variant.filled
                  Attr.Style "margin-inline-start" "auto"
                ]
              )
            ],
            attrs = [
              Padding.All.extraSmall
              Flex.Flex.allSizes
              FlexWrap.Wrap.allSizes
              AlignItems.center
              Gap.All.g3
            ]
          )
        )
      ]

    navigation, toolbar, page

  let private demoBox content =
    div [
      Attr.Style "position" "relative"
      Attr.Style "inline-size" "100%"
      Attr.Style "height" "340px"
      Attr.Style "overflow" "hidden"
      Attr.Style "isolation" "isolate"
    ] [ content ]

  let appearanceExample () =
    let variant = Var.Create "Temporary"
    let position = Var.Create "Left"
    let clip = Var.Create "Full height"
    let isOpen = Var.Create false
    let selectedPage = Var.Create "Projects"

    let choice label values (value: Var<string>) =
      let selected =
        Var.Lens value Some (fun current next ->
          Var.Set isOpen false
          Option.defaultValue current next)

      let items =
        values
        |> List.map (fun name -> SelectItem.create (text name, name, name))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice "Variant" DrawerAppearance.variants variant
        choice "Position" DrawerAppearance.positions position
        div [ Attr.Style "grid-column" "1 / -1" ] [ choice "Clip mode" DrawerAppearance.clips clip ]
      ]

    let config =
      let style = (variant.View, position.View) ||> View.Map2(fun a b -> a, b)
      (style, clip.View) ||> View.Map2(fun (a, b) c -> a, b, c)

    let preview =
      config
      |> Doc.BindView(fun (variant, position, clip) ->
        let variant =
          match variant with
          | "Persistent" -> Drawer.Variant.Persistent
          | "Responsive" -> Drawer.Variant.Responsive
          | "Mini" -> Drawer.Variant.Mini
          | _ -> Drawer.Variant.Temporary

        let side =
          if position = "Right" then
            Drawer.Position.Right
          else
            Drawer.Position.Left

        let clipping =
          if clip = "Below app bar" then
            Drawer.ClipMode.AppBar
          else
            Drawer.ClipMode.FullHeight

        let navigation, toolbar, page = workspace isOpen selectedPage

        let drawer =
          Drawer.create (
            navigation,
            isOpen.View,
            variant = variant,
            position = side,
            clipMode = clipping,
            overlayClose = (fun () -> Var.Set isOpen false),
            width = 208,
            isFixed = false
          )

        let left = if side = Drawer.Position.Left then Some drawer else None
        let right = if side = Drawer.Position.Right then Some drawer else None

        demoBox (
          DrawerContainer.create (
            mainContent = Doc.Concat [ toolbar; page ],
            ?leftDrawer = left,
            ?rightDrawer = right,
            attrs = [ SurfaceColor.BackgroundColor.background ]
          )
        ))

    let source =
      (config, isOpen.View)
      ||> View.Map2(fun (variant, position, clip) openNow ->
        DrawerAppearance.source variant position clip openNow)

    let reset () =
      Var.Set isOpen false
      Var.Set selectedPage "Projects"
      Var.Set variant "Temporary"
      Var.Set position "Left"
      Var.Set clip "Full height"

    Helpers.appearanceSection
      [ "temporary"; "persistent"; "responsive"; "clip-mode" ]
      (Helpers.bodyText
        "Choose a drawer mode, side, and relationship to the app bar. Open the workspace navigation to compare overlay and layout behavior. Responsive pushes content above its viewport breakpoint and overlays below it; Mini changes at its breakpoint. Below app bar keeps the toolbar visible above the drawer. On narrow screens, Persistent leaves less room for content; Responsive overlays it instead. The code omits workspace details and the inline preview host for brevity.")
      controls
      None
      preview
      reset
      source

  let private miniExample () =
    let content =
      let isOpen = Var.Create false
      let hoverEnabled = Var.Create false
      let navigation, toolbar, page = workspace isOpen (Var.Create "Projects")

      let app =
        DrawerContainer.create (
          mainContent = Doc.Concat [ toolbar; page ],
          leftDrawer =
            Drawer.create (
              navigation,
              isOpen.View,
              variant = Drawer.Variant.Mini,
              breakpoint = Drawer.DrawerBreakpoint.Always,
              expandOnHover = hoverEnabled.View,
              width = 208,
              isFixed = false
            ),
          attrs = [ SurfaceColor.BackgroundColor.background ]
        )

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        Switch.create (hoverEnabled, content = text "Expand on hover")
        demoBox app
      ]

    Helpers.codeSampleSection
      "Mini"
      (Helpers.bodyText
        "Mini keeps an icon rail visible while closed. Expand it with either menu control, or enable hover expansion. Selecting a destination updates the page. The code omits workspace details and the inline preview host for brevity.")
      content
      """open Weave.Icons.MaterialSymbols

let isOpen = Var.Create false
let hoverEnabled = Var.Create false
let toggle () = Var.Update isOpen not

let navigation =
  nav [ Attr.Create "aria-label" "Workspace navigation" ] [
    Icon.create (Icon.Text Text.FolderOpen)
    span [ cl "weave-drawer-mini-label" ] [ text "Projects" ]
    // Other navigation items removed for brevity.
  ]

let toolbar =
  AppBar.create (Button.create (text "Toggle drawer", onClick = toggle))

// Workspace content and inline preview host removed for brevity.
let page = div [ cl "weave-main-content" ] [ text "Workspace" ]

let app =
  DrawerContainer.create (
    mainContent = Doc.Concat [ toolbar; page ],
    leftDrawer =
      Drawer.create (
        navigation,
        isOpen.View,
        variant = Drawer.Variant.Mini,
        breakpoint = Drawer.DrawerBreakpoint.Always,
        expandOnHover = hoverEnabled.View,
        width = 208,
        isFixed = false
      )
  )

Doc.Concat [
  Switch.create (hoverEnabled, content = text "Expand on hover")
  app
]"""

  let private headerExample () =
    let content =
      let isOpen = Var.Create false
      let navigation, toolbar, page = workspace isOpen (Var.Create "Projects")

      demoBox (
        DrawerContainer.create (
          mainContent = Doc.Concat [ toolbar; page ],
          leftDrawer =
            Drawer.create (
              navigation,
              isOpen.View,
              variant = Drawer.Variant.Temporary,
              header =
                DrawerHeader.create (
                  Doc.Concat [
                    Icon.create (Icon.Text Text.Dashboard, attrs = [ Attr.Create "aria-hidden" "true" ])
                    span [ Typography.subtitle1 ] [ text "Workspace" ]
                  ],
                  attrs = [ Gap.All.g2 ]
                ),
              overlayClose = (fun () -> Var.Set isOpen false),
              width = 208,
              isFixed = false
            ),
          attrs = [ SurfaceColor.BackgroundColor.background ]
        )
      )

    Helpers.codeSampleSection
      "Drawer Header"
      (Helpers.bodyText
        "DrawerHeader adds a toolbar-height heading above the navigation. The header is part of the drawer, so it moves with the panel. The code omits workspace details and the inline preview host for brevity.")
      content
      """let isOpen = Var.Create false
let toggle () = Var.Update isOpen not

// Navigation details and inline preview host removed for brevity.
let navigation = nav [ Attr.Create "aria-label" "Workspace navigation" ] [
  text "Projects"
]

let toolbar =
  AppBar.create (Button.create (text "Toggle drawer", onClick = toggle))

// Workspace content removed for brevity.
let page = div [ cl "weave-main-content" ] [ text "Workspace" ]

DrawerContainer.create (
  mainContent = Doc.Concat [ toolbar; page ],
  leftDrawer =
    Drawer.create (
      navigation,
      isOpen.View,
      variant = Drawer.Variant.Temporary,
      header = DrawerHeader.create (text "Workspace"),
      overlayClose = (fun () -> Var.Set isOpen false),
      width = 208,
      isFixed = false
    )
)"""

  let private variantGuidanceSection () =
    let description =
      Helpers.bodyText
        "Choose how navigation should interact with the page and how often it should remain available."

    let content =
      Helpers.guidanceCard "Choosing a drawer variant" [
        Helpers.guidanceBullet
          "Temporary"
          "Overlays the page without shifting it; useful for mobile or infrequent navigation."
        Helpers.guidanceBullet
          "Persistent"
          "Shifts the page while open; useful when desktop navigation shares space with content."
        Helpers.guidanceBullet
          "Responsive"
          "Uses Persistent above its breakpoint and Temporary below it for adaptive layouts."
        Helpers.guidanceBullet
          "Mini"
          "Keeps a compact icon rail and expands when opened or hovered; useful for frequent navigation."
      ]

    Helpers.sectionPlain "Choosing a Variant" description content

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText "Complete API reference for Drawer, DrawerContainer, and DrawerHeader.")
      [
        Helpers.returnTypeNote
          "Drawer.create returns a Drawer.Config record, not a Doc. Pass the config to DrawerContainer.create for layout-aware usage, or call Drawer.Render to get a standalone Doc."

        Helpers.apiTable "Drawer.create" [
          Helpers.apiParam "content" "Doc" "" "The content to render inside the drawer"
          Helpers.apiParam "isOpen" "View<bool>" "" "Reactive view controlling whether the drawer is open"
          Helpers.apiParam
            "?variant"
            "Drawer.Variant"
            "Temporary"
            "How the drawer displays and interacts with the layout"
          Helpers.apiParam "?position" "Drawer.Position" "Left" "The edge the drawer slides from"
          Helpers.apiParam
            "?clipMode"
            "Drawer.ClipMode"
            "FullHeight"
            "How the drawer interacts with the AppBar vertically"
          Helpers.apiParam
            "?breakpoint"
            "Drawer.DrawerBreakpoint"
            "At Medium"
            "Breakpoint at which Responsive and Mini drawers switch states"
          Helpers.apiParam
            "?overlayClose"
            "unit -> unit"
            ""
            "Callback invoked when the user clicks the overlay backdrop"
          Helpers.apiParam
            "?expandOnHover"
            "View<bool>"
            "View.Const false"
            "Mini drawer expands to full width on pointer hover without shifting layout"
          Helpers.apiParam
            "?isFixed"
            "bool"
            "true"
            "Whether the drawer is positioned relative to the viewport"
          Helpers.apiParam "?width" "int" "" "Custom width in pixels, overriding the CSS custom property"
          Helpers.apiParam
            "?header"
            "Doc"
            ""
            "Optional header Doc rendered above drawer content; use DrawerHeader.create"
          Helpers.apiParam
            "?attrs"
            "Attr list"
            "[]"
            "Additional attributes applied to the drawer root element"
        ]

        Helpers.apiTable "DrawerContainer.create" [
          Helpers.apiParam "mainContent" "Doc" "" "Primary page content, including the AppBar if used"
          Helpers.apiParam
            "?leftDrawer"
            "Drawer.Config"
            ""
            "Left-side drawer config produced by Drawer.create"
          Helpers.apiParam
            "?rightDrawer"
            "Drawer.Config"
            ""
            "Right-side drawer config produced by Drawer.create"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the container element"
        ]

        Helpers.apiTable "DrawerHeader.create" [
          Helpers.apiParam "content" "Doc" "" "Content rendered inside the header"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the header element"
        ]

        Helpers.styleModuleTable "Drawer.Variant (DU)" [
          ("Temporary", "Overlays content with an optional backdrop; does not shift AppBar or main content")
          ("Persistent", "Pushes AppBar and main content while open; closes without an overlay")
          ("Responsive", "Persistent above breakpoint, Temporary below it")
          ("Mini", "Collapses to narrow icon strip; expands to full width on open")
        ]

        Helpers.styleModuleTable "Drawer.Position (DU)" [
          ("Left", "Drawer slides from the left edge")
          ("Right", "Drawer slides from the right edge")
        ]

        Helpers.styleModuleTable "Drawer.ClipMode (DU)" [
          ("AppBar", "Drawer starts below the AppBar")
          ("FullHeight", "Drawer renders at full height, overlapping the AppBar")
        ]

        Helpers.styleModuleTable "Drawer.DrawerBreakpoint (DU)" [
          ("At Breakpoint", "Drawer switches at the specified breakpoint (ExtraSmall through ExtraExtraLarge)")
          ("None", "Always in compact/temporary state — never pushes layout")
          ("Always", "Always expanded — always pushes layout")
        ]

        Helpers.styleModuleTable "Drawer.Density" [
          ("compact", "Compact header height")
          ("standard", "Standard header height")
          ("spacious", "Spacious header height")
        ]
      ]

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
        div [ Typography.body2; Margin.Bottom.extraSmall ] [
          text "Inline previews use isFixed = false. Use the default isFixed = true for a viewport drawer."
        ]
        variantGuidanceSection ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        miniExample ()
        Helpers.divider ()
        headerExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
