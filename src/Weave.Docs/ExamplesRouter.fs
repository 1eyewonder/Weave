namespace Weave.Docs.Examples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.CssHelpers.Animation
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.Docs.Examples.DocsRouting

[<JavaScript>]
module ExamplesRouter =

  let private githubSvg =
    Doc.Verbatim
      """<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor"><path d="M12 0c-6.626 0-12 5.373-12 12 0 5.302 3.438 9.8 8.207 11.387.599.111.793-.261.793-.577v-2.234c-3.338.726-4.033-1.416-4.033-1.416-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.729.083-.729 1.205.084 1.839 1.237 1.839 1.237 1.07 1.834 2.807 1.304 3.492.997.107-.775.418-1.305.762-1.604-2.665-.305-5.467-1.334-5.467-5.931 0-1.311.469-2.381 1.236-3.221-.124-.303-.535-1.524.117-3.176 0 0 1.008-.322 3.301 1.23.957-.266 1.983-.399 3.003-.404 1.02.005 2.047.138 3.006.404 2.291-1.552 3.297-1.23 3.297-1.23.653 1.653.242 2.874.118 3.176.77.84 1.235 1.911 1.235 3.221 0 4.609-2.807 5.624-5.479 5.921.43.372.823 1.102.823 2.222v3.293c0 .319.192.694.801.576 4.765-1.589 8.199-6.086 8.199-11.386 0-6.627-5.373-12-12-12z"/></svg>"""

  let private renderPage (navigate: Page -> unit) page =
    match page with
    | Home ->
      div [] [
        // Above-fold hero — full-bleed, no max-width cap
        div [ Attr.Class "docs-home-above-fold docs-weave-pattern" ] [
          div [ Attr.Class "docs-hero" ] [

            div [
              Flex.Flex.allSizes
              FlexDirection.Column.allSizes
              AlignItems.center
              Margin.Bottom.medium
            ] [
              img [
                attr.src "assets/weave-logo.png"
                attr.alt "Weave Logo"
                Attr.Style "height" "80px"
                Attr.Style "object-fit" "contain"
                Margin.Bottom.extraSmall
              ] []
              div [ Typography.h2; Margin.Bottom.extraSmall ] [ text "Weave" ]
              div [
                Typography.body1
                Attr.Style "font-style" "italic"
                Attr.Style "opacity" "0.7"
              ] [ text "Threading Logic. Fabricating UI." ]
            ]

            div [ Attr.Class "docs-hero-cta" ] [
              Button.create (
                div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                  text "Get Started"
                  Icon.create (Icon.Social Social.RocketLaunch, attrs = [ Attr.Style "font-size" "18px" ])
                ],
                onClick = (fun () -> navigate GettingStartedExamples),
                attrs = [ Button.Variant.outlined; Button.Color.primary ]
              )

              Button.create (
                div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                  Icon.create (Icon.Action Action.Stars, attrs = [ Attr.Style "font-size" "18px" ])
                  text "GitHub"
                ],
                onClick =
                  (fun () -> JS.Window?``open``("https://github.com/1eyewonder/Weave", "_blank") |> ignore),
                attrs = [ Button.Variant.outlined ]
              )
            ]
          ]

          // Scroll indicator
          div [ Attr.Class "docs-scroll-indicator" ] [
            div [
              Typography.Color.textSecondary
              Typography.overline
              AnimationEmphasis.bounce
              AnimationDuration.long
              AnimationEasing.accelerate
              Animate.replayEvery 2000
            ] [ text "Scroll" ]
            Icon.create (
              Icon.Hardware Hardware.KeyboardArrowDown,
              attrs = [
                Typography.Color.textSecondary
                AnimationEmphasis.bounce
                AnimationDuration.long
                AnimationEasing.accelerate
                Animate.replayEvery 2000
              ]
            )
          ]
        ]

        // Gradient divider — full-bleed
        div [ Attr.Class "docs-section-divider" ] []

        // Below-fold content — constrained for readability
        Container.create (
          div [] [

            let categorySection (title: string) (items: (string * Page) list) =
              div [
                Margin.Bottom.medium
                AnimationEntrance.fadeIn
                AnimationDuration.medium
                AnimationEasing.accelerate
                Animate.onScrollWith 0.1 "0px 0px -80px 0px" true
              ] [
                div [ Typography.h5; Margin.Bottom.small ] [ text title ]
                Grid.create (
                  items
                  |> List.mapi (fun i (label, page) ->
                    GridItem.create (
                      div [
                        Flex.Flex.allSizes
                        FlexDirection.Column.allSizes
                        SurfaceColor.toBackgroundColor SurfaceColor.Surface
                        Cursor.pointer
                        Attr.Style "height" "100%"
                        Overflow.hidden
                        Attr.Style "box-sizing" "border-box"
                        Attr.Class "docs-component-card"
                        on.click (fun _ _ -> navigate page)
                      ] [
                        ComponentPreviews.forPage page
                        div [
                          Flex.Flex.allSizes
                          JustifyContent.center
                          AlignItems.center
                          Padding.Horizontal.extraSmall
                          Padding.Vertical.extraSmall
                          FlexItem.Grow.allSizes
                        ] [
                          div [
                            Typography.body2
                            Typography.Align.center
                            Attr.Class "docs-component-card__label"
                          ] [ text label ]
                        ]
                      ],
                      attrs = [
                        GridItem.Span.six
                        GridItem.Span.Small.four
                        GridItem.Span.Medium.three
                        Flex.Flex.allSizes
                        FlexDirection.Column.allSizes
                        AnimationEntrance.fadeIn
                        AnimationDelay.stagger (min (i + 1) 10)
                        Animate.onScrollWith 0.1 "0px 0px -80px 0px" true
                      ]
                    )),
                  attrs = [ AlignItems.stretch ]
                )
              ]

            div [
              Margin.Bottom.medium
              AnimationEntrance.fadeIn
              AnimationDuration.medium
              AnimationEasing.accelerate
              Animate.onScrollWith 0.1 "0px 0px -80px 0px" true
            ] [
              div [ Attr.Class "docs-why-weave-label" ] [ text "Why Weave" ]
              div [ Attr.Class "docs-why-weave-heading" ] [ text "Write less. Express more." ]

              Grid.create (
                [
                  "Type-Safe by Default",
                  Icon.Action Action.Verified,
                  "Styled through DUs and typed helpers — with full access to WebSharper's raw functionality when you need flexibility."

                  "Functional-First API",
                  Icon.Action Action.Code,
                  "Clean camelCase APIs, optional params, and composable attrs. Designed for F#, not ported from another ecosystem."

                  "Built-In Theming",
                  Icon.Images Images.Palette,
                  "Light and dark mode with CSS custom properties. One-line toggle, no config."

                  "Seamless WebSharper Integration",
                  Icon.Action Action.Extension,
                  "Built on WebSharper's reactive model. Var/View bindings, native Attr/Doc — everything composes naturally."

                  "Less Boilerplate",
                  Icon.UiActions UiActions.Bolt,
                  "Sensible defaults out of the box. Customize through ?attrs only when you actually need to."

                  "Structured Styling",
                  Icon.Maps Maps.Layers,
                  "BEM + CSS custom properties. Themed by default, overridable by design."
                ]
                |> List.mapi (fun i (title, icon, desc) ->
                  GridItem.create (
                    div [
                      SurfaceColor.toBackgroundColor SurfaceColor.Surface
                      Padding.All.small
                      BorderRadius.All.large
                      Attr.Class "docs-value-card docs-weave-pattern"
                      Flex.Flex.allSizes
                      FlexDirection.Column.allSizes
                      Gap.All.g2
                      Attr.Style "height" "100%"
                      Attr.Style "box-sizing" "border-box"
                    ] [
                      Icon.create (
                        icon,
                        attrs = [ BrandColor.toColor BrandColor.Primary; Attr.Style "font-size" "28px" ]
                      )

                      div [ Typography.h6 ] [ text title ]

                      div [ Typography.body2; Attr.Style "opacity" "0.8" ] [ text desc ]
                    ],
                    attrs = [
                      GridItem.Span.twelve
                      GridItem.Span.Small.six
                      GridItem.Span.Medium.four
                      Flex.Flex.allSizes
                      FlexDirection.Column.allSizes
                      AnimationEntrance.fadeIn
                      AnimationDelay.stagger (min (i + 1) 6)
                      Animate.onScrollWith 0.1 "0px 0px -20px 0px" true
                    ]
                  )),
                attrs = [ AlignItems.stretch ]
              )
            ]

            // Components — scroll-reveal: fades in as user scrolls down
            categorySection "Components" [
              "Alert", AlertExamples
              "App Bar", AppBarExamples
              "Button", ButtonExamples
              "Button Group", ButtonGroupExamples
              "Button Menu", ButtonMenuExamples
              "Checkbox", CheckboxExamples
              "Chip", ChipExamples
              "Chip Set", ChipSetExamples
              "Container", ContainerExamples
              "Dialog", DialogExamples
              "Divider", DividerExamples
              "Drawer", DrawerExamples
              "Dropdown", DropdownExamples
              "Expansion Panel", ExpansionPanelExamples
              "Field", FieldExamples
              "Grid", GridExamples
              "Icons", IconsExamples
              "Link", LinkExamples
              "List", ListExamples
              "Numeric Field", NumericFieldExamples
              "Radio Button", RadioButtonExamples
              "Select", SelectExamples
              "Spacer", SpacerExamples
              "Switch", SwitchExamples
              "Tabs", TabsExamples
              "Tooltip", TooltipExamples
              "Typography", TypographyExamples
            ]

            // Styling — scroll-reveal: fades in as user scrolls further
            categorySection "Styling" [
              "Spacing", SpacingExamples
              "Opacity", OpacityExamples
              "Borders", BorderExamples
              "Display", DisplayExamples
              "Elevation", ElevationExamples
              "Flexbox", FlexboxExamples
              "Transitions", TransitionExamples
              "Animations", AnimationExamples
              "Theming", ThemingExamples
            ]
          ],
          attrs = [ Container.MaxWidth.large ]
        )
      ]
    | componentPage ->
      match componentPage with
      | GettingStartedExamples -> GettingStartedExamples.render ()
      | AppBarExamples -> AppBarExamples.render ()
      | SpacerExamples -> SpacerExamples.render ()
      | ButtonExamples -> ButtonExamples.render ()
      | ButtonGroupExamples -> ButtonGroupExamples.render ()
      | ButtonMenuExamples -> ButtonMenuExamples.render ()
      | TypographyExamples -> TypographyExamples.render ()
      | TooltipExamples -> TooltipExamples.render ()
      | GridExamples -> GridExamples.render ()
      | CheckboxExamples -> CheckboxExamples.render ()
      | ChipExamples -> ChipExamples.render ()
      | ChipSetExamples -> ChipSetExamples.render ()
      | RadioButtonExamples -> RadioButtonExamples.render ()
      | SwitchExamples -> SwitchExamples.render ()
      | ContainerExamples -> ContainerExamples.render ()
      | FieldExamples -> FieldExamples.render ()
      | NumericFieldExamples -> NumericFieldExamples.render ()
      | DropdownExamples -> DropdownExamples.render ()
      | SelectExamples -> SelectExamples.render ()
      | ExpansionPanelExamples -> ExpansionPanelExamples.render ()
      | DialogExamples -> DialogExamples.render ()
      | DrawerExamples -> DrawerExamples.render ()
      | IconsExamples -> IconsExamples.render ()
      | TabsExamples -> TabsExamples.render ()
      | ListExamples -> ListExamples.render ()
      | AlertExamples -> AlertExamples.render ()
      | LinkExamples -> LinkExamples.render ()
      | DividerExamples -> DividerExamples.render ()
      | SpacingExamples -> SpacingExamples.render ()
      | OpacityExamples -> OpacityExamples.render ()
      | TransitionExamples -> TransitionExamples.render ()
      | AnimationExamples -> AnimationExamples.render ()
      | BorderExamples -> BorderExamples.render ()
      | DisplayExamples -> DisplayExamples.render ()
      | ElevationExamples -> ElevationExamples.render ()
      | FlexboxExamples -> FlexboxExamples.render ()
      | ThemingExamples -> ThemingExamples.render ()
      | Home -> Doc.Empty

  let render () =
    let initialHash = getLocationHash ()
    let initialParts = splitHashParts initialHash
    let initialPageHash = initialParts.[0]

    let initialSection =
      if initialParts.Length > 1 then
        Some initialParts.[1]
      else
        None

    let initialPage = initialPageHash |> hashToPage |> Option.defaultValue Home

    // Stored reference to the .weave-main-content element, set via on.afterRender
    let mainEl = Var.Create<Dom.Element option> None

    let scrollMainToTop () =
      mainEl.Value |> Option.iter (fun el -> el?scrollTop <- 0)

    let scrollElementTo (m: Dom.Element) (id: string) =
      let target = JS.Document.GetElementById id

      if not (isNull target) then
        m?scrollTop <-
          As<float>(m?scrollTop) + target.GetBoundingClientRect().Top
          - m.GetBoundingClientRect().Top
          - 16.0

        true
      else
        false

    let scrollToSectionAfterDelay (id: string) (ms: int) =
      JS.Window?setTimeout(
        (fun () -> mainEl.Value |> Option.iter (fun m -> scrollElementTo m id |> ignore)),
        ms
      )
      |> ignore

    /// Section to scroll to once the page content has rendered and sections have
    /// been collected. Set during initial load or cross-page hash navigation;
    /// consumed by collectSectionsWithRetry when sections first appear.
    let mutable pendingSection: string option = initialSection

    let selectedNav = Var.Create<string option>(Some(pageToString initialPage))
    let drawerOpen = Var.Create true

    let navigateTo (page: Page) =
      setLocationHash (pageToHash page)
      Var.Set selectedNav (Some(pageToString page))

    let currentPageView =
      selectedNav.View
      |> View.Map(fun sel -> sel |> Option.bind stringToPage |> Option.defaultValue Home)

    let activeSectionId = Var.Create ""

    let tocSections = Var.Create<(string * string)[]> [||]

    let rec collectSectionsWithRetry attempt =
      let headers = JS.Document.QuerySelectorAll ".section-header[id]"

      let sections = [|
        for i in 0 .. headers.Length - 1 do
          let h = As<Dom.Element>(headers.Item i)
          h.Id, As<string>(h?textContent).Replace("#", "").Trim()
      |]

      Var.Set tocSections sections

      if sections.Length > 0 then
        // If there's a pending section scroll (from initial URL or cross-page nav),
        // scroll to it now that the content is in the DOM.
        match pendingSection, mainEl.Value with
        | Some id, Some m ->
          pendingSection <- None
          scrollElementTo m id |> ignore
          Var.Set activeSectionId id
          // Re-scroll after layout fully settles
          JS.Window?setTimeout((fun () -> scrollElementTo m id |> ignore), 300) |> ignore
        | _ ->
          mainEl.Value
          |> Option.iter (fun m ->
            let activeId = ScrollListener.detectActiveSection m ".section-header[id]" 80.0

            let id =
              if System.String.IsNullOrEmpty activeId && sections.Length > 0 then
                fst sections.[0]
              else
                activeId

            Var.Set activeSectionId id)
      elif attempt < 20 then
        JS.Window?setTimeout((fun () -> collectSectionsWithRetry (attempt + 1)), 100)
        |> ignore

    let scrollToSection (id: string) =
      mainEl.Value |> Option.iter (fun m -> scrollElementTo m id |> ignore)

    let refreshTocSections () =
      Var.Set tocSections [||]
      Var.Set activeSectionId ""
      // Try immediately, then retry with polling for async WebSharper renders
      collectSectionsWithRetry 0

    let tableOfContents =
      currentPageView
      |> Doc.BindView(fun page ->
        match page with
        | Home -> Doc.Empty
        | _ ->
          div [ Attr.Class "docs-toc" ] [
            div [ Typography.subtitle2; Attr.Class "docs-toc__title" ] [ text "Contents" ]

            tocSections.View
            |> Doc.BindView(fun sections ->
              Doc.Concat [
                for id, title in sections do
                  div [
                    Attr.Class "docs-toc__item"

                    Attr.DynamicClassPred
                      "docs-toc__item--active"
                      (activeSectionId.View |> View.Map(fun a -> a = id))

                    on.click (fun _ _ ->
                      scrollToSection id
                      Var.Set activeSectionId id

                      let pageHash =
                        selectedNav.Value
                        |> Option.bind stringToPage
                        |> Option.map pageToHash
                        |> Option.defaultValue "#home"

                      replaceStateHash (pageHash + "/" + id))
                  ] [ div [ Typography.body2 ] [ text title ] ]
              ])
          ])

    let mutable suppressScrollToTop = initialSection.IsSome

    let navEffects =
      selectedNav.View
      |> Doc.sink (fun _ ->
        if suppressScrollToTop && mainEl.Value.IsSome then
          suppressScrollToTop <- false
        elif not suppressScrollToTop then
          scrollMainToTop ()

        if BrowserUtils.windowWidth.Value < 960 then
          Var.Set drawerOpen false)

    onHashChange (fun hash ->
      let parts = splitHashParts hash
      let pagePart = parts.[0]
      let sectionPart = if parts.Length > 1 then Some parts.[1] else None

      match hashToPage pagePart with
      | Some page ->
        let currentName = selectedNav.Value |> Option.defaultValue ""
        let isPageChange = pageToString page <> currentName

        if isPageChange then
          Var.Set selectedNav (Some(pageToString page))

        // If the page changed, the new content renders async — defer the scroll
        // until collectSectionsWithRetry finds the new sections in the DOM.
        // If we're already on the page, the element is present so a short delay is fine.
        sectionPart
        |> Option.iter (fun id ->
          if isPageChange then
            pendingSection <- Some id
          else
            scrollToSectionAfterDelay id 50)
      | None ->
        // Bare section slug from an in-page anchor link (e.g. href="#variants")
        // Rewrite the history entry to the combined "#page/section" format
        let sectionSlug = stripHash hash

        let pageHash =
          selectedNav.Value
          |> Option.bind stringToPage
          |> Option.map pageToHash
          |> Option.defaultValue "#home"

        replaceStateHash (pageHash + "/" + sectionSlug)
        scrollToSectionAfterDelay sectionSlug 0)

    let componentsExpanded = Var.Create true
    let stylingExpanded = Var.Create true

    let navLeafItem (label: string) =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Vertical.extraSmall
        Padding.Horizontal.medium
        Cursor.pointer
        BorderRadius.All.large
        Margin.All.extraSmall
        Attr.Class "weave-nav-leaf"
        Attr.DynamicClassPred "weave-nav-item--active" (selectedNav.View |> View.Map(fun s -> s = Some label))
        on.click (fun _ _ -> stringToPage label |> Option.iter navigateTo)
      ] [ div [ Typography.body2 ] [ text label ] ]

    let navGroup categoryIcon (label: string) (isExpanded: Var<bool>) items =
      div [] [
        div [
          Flex.Flex.allSizes
          AlignItems.center
          Padding.Vertical.extraSmall
          Padding.Horizontal.small
          Cursor.pointer
          Gap.All.g2
          Attr.Class "weave-nav-group-header"
          on.click (fun _ _ -> Var.Update isExpanded not)
        ] [
          Icon.create (categoryIcon, attrs = [ Attr.Style "font-size" "18px" ])
          div [ Typography.overline; FlexItem.Flex.allSizes; Attr.Style "opacity" "0.7" ] [ text label ]
          isExpanded.View
          |> Doc.BindView(fun exp ->
            Icon.create (
              (if exp then
                 Icon.Hardware Hardware.KeyboardArrowDown
               else
                 Icon.Hardware Hardware.KeyboardArrowRight),
              attrs = [ Attr.Style "font-size" "16px"; Attr.Style "opacity" "0.6" ]
            ))
        ]
        isExpanded.View
        |> Doc.BindView(fun exp -> if exp then div [] items else Doc.Empty)
      ]

    let navList =
      div [ Padding.Vertical.extraSmall ] [
        div [
          Flex.Flex.allSizes
          AlignItems.center
          Padding.Vertical.extraSmall
          Padding.Horizontal.small
          Cursor.pointer
          BorderRadius.All.large
          Margin.All.extraSmall
          Gap.All.g2
          Attr.Class "weave-nav-leaf"
          Attr.DynamicClassPred
            "weave-nav-item--active"
            (selectedNav.View |> View.Map(fun s -> s = Some "Home"))
          on.click (fun _ _ -> navigateTo Home)
        ] [
          Icon.create (Icon.UiActions UiActions.Home, attrs = [ Attr.Style "font-size" "18px" ])
          div [ Typography.body2 ] [ text "Home" ]
        ]

        div [
          Flex.Flex.allSizes
          AlignItems.center
          Padding.Vertical.extraSmall
          Padding.Horizontal.small
          Cursor.pointer
          BorderRadius.All.large
          Margin.All.extraSmall
          Gap.All.g2
          Attr.Class "weave-nav-leaf"
          Attr.DynamicClassPred
            "weave-nav-item--active"
            (selectedNav.View |> View.Map(fun s -> s = Some "Getting Started"))
          on.click (fun _ _ -> navigateTo GettingStartedExamples)
        ] [
          Icon.create (Icon.Social Social.RocketLaunch, attrs = [ Attr.Style "font-size" "18px" ])
          div [ Typography.body2 ] [ text "Getting Started" ]
        ]

        Divider.create (attrs = [ Margin.Vertical.extraSmall ])

        navGroup (Icon.Android Android.Widgets) "Components" componentsExpanded [
          navLeafItem "Alert"
          navLeafItem "App Bar"
          navLeafItem "Button"
          navLeafItem "Button Group"
          navLeafItem "Button Menu"
          navLeafItem "Checkbox"
          navLeafItem "Chip"
          navLeafItem "Chip Set"
          navLeafItem "Container"
          navLeafItem "Dialog"
          navLeafItem "Divider"
          navLeafItem "Drawer"
          navLeafItem "Dropdown"
          navLeafItem "Expansion Panel"
          navLeafItem "Field"
          navLeafItem "Grid"
          navLeafItem "Icons"
          navLeafItem "Link"
          navLeafItem "List"
          navLeafItem "Numeric Field"
          navLeafItem "Radio Button"
          navLeafItem "Select"
          navLeafItem "Spacer"
          navLeafItem "Switch"
          navLeafItem "Tabs"
          navLeafItem "Tooltip"
          navLeafItem "Typography"
        ]

        navGroup (Icon.Images Images.Palette) "Styling" stylingExpanded [
          navLeafItem "Spacing"
          navLeafItem "Opacity"
          navLeafItem "Borders"
          navLeafItem "Display"
          navLeafItem "Elevation"
          navLeafItem "Flexbox"
          navLeafItem "Transitions"
          navLeafItem "Animations"
          navLeafItem "Theming"
        ]
      ]

    let appBarContent =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Horizontal.small
        Padding.Vertical.extraSmall
      ] [
        IconButton.create (
          Icon.create (Icon.UiActions UiActions.Menu),
          onClick = (fun () -> Var.Set drawerOpen (not drawerOpen.Value)),
          attrs = [ Margin.Right.extraSmall ]
        )

        div [
          Flex.Flex.allSizes
          AlignItems.center
          Gap.All.g2
          Cursor.pointer
          on.click (fun _ _ -> navigateTo Home)
        ] [ div [ Typography.h6 ] [ text "Weave" ] ]

        Spacer.create ()

        a [
          attr.href "https://github.com/1eyewonder/Weave"
          attr.target "_blank"
          Attr.Style "color" "inherit"
          Flex.Flex.allSizes
          AlignItems.center
          Padding.Horizontal.extraSmall
        ] [ githubSvg ]

        IconButton.create (
          Theme.current.View
          |> Doc.BindView(fun mode ->
            match mode with
            | Theming.Light -> Icon.create (Icon.Android Android.DarkMode)
            | Theming.Dark -> Icon.create (Icon.Android Android.LightMode)),
          onClick =
            (fun () ->
              let newMode = Theming.toggleMode ()
              Var.Set Theme.current newMode)
        )
      ]

    div [
      Attr.Style "height" "100vh"
      Overflow.hidden
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      SurfaceColor.toBackgroundColor SurfaceColor.Background
    ] [
      navEffects

      AppBar.create (appBarContent, attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ])

      DrawerContainer.create (
        mainContent =
          div [
            cl "weave-main-content"
            Overflow.yAuto
            Attr.Style "scroll-behavior" "smooth"
            Attr.Style "height" "100%"
            on.afterRender (fun el -> Var.Set mainEl (Some el))
            ScrollListener.trackSections ".section-header[id]" 80.0 (fun sectionId ->
              Var.Set activeSectionId sectionId

              if tocSections.Value.Length = 0 then
                collectSectionsWithRetry 0

              let pageHash =
                selectedNav.Value
                |> Option.bind stringToPage
                |> Option.map pageToHash
                |> Option.defaultValue "#home"

              if sectionId <> "" then
                replaceStateHash (pageHash + "/" + sectionId)
              else
                replaceStateHash pageHash)
          ] [
            div [ Attr.Class "docs-page-layout" ] [
              div [
                FlexItem.Flex.allSizes
                Attr.Style "min-width" "0"
                Padding.All.Medium.small
                Padding.All.ExtraSmall.extraSmall
              ] [
                currentPageView
                |> Doc.BindView(fun page ->
                  div [ on.afterRender (fun _ -> refreshTocSections ()) ] [ renderPage navigateTo page ])
              ]

              tableOfContents
            ]
          ],
        leftDrawer =
          Drawer.create (
            navList,
            drawerOpen.View,
            variant = Drawer.Variant.Responsive,
            position = Drawer.Position.Left,
            breakpoint = Drawer.DrawerBreakpoint.At Breakpoint.Medium,
            overlayClose = (fun () -> Var.Set drawerOpen false),
            isFixed = false,
            attrs = [ Overflow.yAuto ]
          ),
        attrs = [ FlexItem.Flex.allSizes; Attr.Style "min-height" "0" ]
      )
    ]
