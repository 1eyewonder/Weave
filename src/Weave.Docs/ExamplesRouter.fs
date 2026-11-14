namespace Weave.Docs.Examples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.Docs.Examples.DocsRouting

[<JavaScript>]
module ExamplesRouter =

  let private githubSvg () =
    Doc.Verbatim
      """<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor"><path d="M12 0c-6.626 0-12 5.373-12 12 0 5.302 3.438 9.8 8.207 11.387.599.111.793-.261.793-.577v-2.234c-3.338.726-4.033-1.416-4.033-1.416-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.729.083-.729 1.205.084 1.839 1.237 1.839 1.237 1.07 1.834 2.807 1.304 3.492.997.107-.775.418-1.305.762-1.604-2.665-.305-5.467-1.334-5.467-5.931 0-1.311.469-2.381 1.236-3.221-.124-.303-.535-1.524.117-3.176 0 0 1.008-.322 3.301 1.23.957-.266 1.983-.399 3.003-.404 1.02.005 2.047.138 3.006.404 2.291-1.552 3.297-1.23 3.297-1.23.653 1.653.242 2.874.118 3.176.77.84 1.235 1.911 1.235 3.221 0 4.609-2.807 5.624-5.479 5.921.43.372.823 1.102.823 2.222v3.293c0 .319.192.694.801.576 4.765-1.589 8.199-6.086 8.199-11.386 0-6.627-5.373-12-12-12z"/></svg>"""

  let private renderPage (navigate: Page -> unit) page =
    match page with
    | Home -> DocsHome.render ()
    | componentPage ->
      match componentPage with
      | GettingStartedExamples -> GettingStartedExamples.render ()
      | AppBarExamples -> AppBarExamples.render ()
      | SpacerExamples -> SpacerExamples.render ()
      | ScrollToTopExamples -> ScrollToTopExamples.render ()
      | ButtonExamples -> ButtonExamples.render ()
      | ButtonGroupExamples -> ButtonGroupExamples.render ()
      | ButtonMenuExamples -> ButtonMenuExamples.render ()
      | TypographyExamples -> TypographyExamples.render ()
      | TooltipExamples -> TooltipExamples.render ()
      | GridExamples -> GridExamples.render ()
      | CheckboxExamples -> CheckboxExamples.render ()
      | CardExamples -> CardExamples.render ()
      | CarouselExamples -> CarouselExamples.render ()
      | PaginationExamples -> PaginationExamples.render ()
      | ChipExamples -> ChipExamples.render ()
      | ChipSetExamples -> ChipSetExamples.render ()
      | RadioButtonExamples -> RadioButtonExamples.render ()
      | SwitchExamples -> SwitchExamples.render ()
      | ProgressExamples -> ProgressExamples.render ()
      | SliderExamples -> SliderExamples.render ()
      | ColorPickerExamples -> ColorPickerExamples.render ()
      | DateTimeGuide -> DateTimeGuide.render ()
      | DatePickerExamples -> DatePickerExamples.render ()
      | DateRangePickerExamples -> DateRangePickerExamples.render ()
      | TimePickerExamples -> TimePickerExamples.render ()
      | ContainerExamples -> ContainerExamples.render ()
      | FormExamples -> FormExamples.render ()
      | FieldExamples -> FieldExamples.render ()
      | NumericFieldExamples -> NumericFieldExamples.render ()
      | DropdownMenuExamples -> DropdownMenuExamples.render ()
      | SelectExamples -> SelectExamples.render ()
      | ExpansionPanelExamples -> ExpansionPanelExamples.render ()
      | DialogExamples -> DialogExamples.render ()
      | DrawerExamples -> DrawerExamples.render ()
      | IconsExamples -> IconsExamples.render ()
      | TableExamples -> TableExamples.render ()
      | TabsExamples -> TabsExamples.render ()
      | TextFieldExamples -> TextFieldExamples.render ()
      | ListExamples -> ListExamples.render ()
      | AlertExamples -> AlertExamples.render ()
      | HighlighterExamples -> HighlighterExamples.render ()
      | SnackbarExamples -> SnackbarExamples.render ()
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
      | ShowcaseTaskTracker -> ShowcaseTaskTracker.render ()
      | ShowcasePomodoroTimer -> ShowcasePomodoroTimer.render ()
      | ShowcaseExpenseTracker -> ShowcaseExpenseTracker.render ()
      | ShowcaseCharacterSheet -> ShowcaseCharacterSheet.render ()
      | ReactiveHelpersExamples -> ReactiveHelpersExamples.render ()
      | EventHandlersExamples -> EventHandlersExamples.render ()
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

    let focusSection id =
      let section = JS.Document.GetElementById id

      if not (isNull section) then
        let heading = section.QuerySelector "h1, h2"

        if not (isNull heading) then
          heading.SetAttribute("tabindex", "-1")
          heading?focus ({| preventScroll = true |})

    let scrollToSectionAfterDelay (id: string) (ms: int) =
      let requestedPage = (splitHashParts (getLocationHash ())).[0]

      JS.Window.SetTimeout(
        (fun () ->
          let current = splitHashParts (getLocationHash ())

          if current.Length = 2 && current.[0] = requestedPage && current.[1] = id then
            mainEl.Value |> Option.iter (fun m -> scrollElementTo m id |> ignore)
            focusSection id),
        ms
      )
      |> ignore

    /// Section to scroll to once the page content has rendered and sections have
    /// been collected. Set during initial load or cross-page hash navigation;
    /// consumed by collectSectionsWithRetry when sections first appear.
    let mutable pendingSection: string option = initialSection

    let selectedNav = Var.Create<string option>(Some(pageToString initialPage))
    let drawerOpen = Var.Create(BrowserUtils.windowWidth.Value >= 960)
    let navSearch = Var.Create ""

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
          focusSection id
          Var.Set activeSectionId id
          // Re-scroll after layout fully settles
          let requestedPage = selectedNav.Value

          JS.Window.SetTimeout(
            (fun () ->
              let current = splitHashParts (getLocationHash ())

              if selectedNav.Value = requestedPage && current.Length = 2 && current.[1] = id then
                scrollElementTo m id |> ignore),
            300
          )
          |> ignore
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
        JS.Window.SetTimeout((fun () -> collectSectionsWithRetry (attempt + 1)), 100)
        |> ignore

    let scrollToSection (id: string) =
      mainEl.Value |> Option.iter (fun m -> scrollElementTo m id |> ignore)

    let refreshTocSections () =
      Var.Set tocSections [||]
      Var.Set activeSectionId ""
      // Try immediately, then retry with polling for async WebSharper renders
      collectSectionsWithRetry 0

    let sectionLinks page sections =
      Doc.Concat [
        for id, title in sections do
          a [
            attr.href (pageToHash page + "/" + id)
            Attr.Class "docs-toc__item"
            Attr.DynamicClassPred
              "docs-toc__item--active"
              (activeSectionId.View |> View.Map(fun active -> active = id))
            Attr.DynamicPred
              "aria-current"
              (activeSectionId.View |> View.Map(fun active -> active = id))
              (View.Const "location")
          ] [ text title ]
      ]

    let tableOfContents =
      currentPageView
      |> Doc.BindView(fun page ->
        if page = Home then
          Doc.Empty
        else
          nav [ Attr.Class "docs-toc"; Attr.Create "aria-label" "On this page" ] [
            div [ Typography.subtitle2; Attr.Class "docs-toc__title" ] [ text "On this page" ]
            tocSections.View |> Doc.BindView(sectionLinks page)
          ])

    let pageQuickNavigation page =
      if page = Home then
        Doc.Empty
      else
        tocSections.View
        |> Doc.BindView(fun sections ->
          let examples =
            sections
            |> Array.tryFind (fun (_, title) -> title <> pageToString page && title <> "API")

          nav [ Attr.Class "docs-page-quicknav"; Attr.Create "aria-label" "Page shortcuts" ] [
            match examples with
            | Some(id, _) -> a [ attr.href (pageToHash page + "/" + id) ] [ text "Examples" ]
            | None -> ()
            if sections |> Array.exists (fun (id, _) -> id = "api") then
              a [ attr.href (pageToHash page + "/api") ] [ text "API reference" ]
            details [] [ summary [] [ text "All sections" ]; div [] [ sectionLinks page sections ] ]
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
          // Set the destination before the new page can render synchronously.
          pendingSection <- sectionPart
          Var.Set selectedNav (Some(pageToString page))
        else
          sectionPart |> Option.iter (fun id -> scrollToSectionAfterDelay id 0)
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

    let labelsForGroup title =
      DocsHome.catalog
      |> List.find (fun (name, _) -> name = title)
      |> snd
      |> List.map pageToString

    let dateTimeLabels = [ "Date & Time Guide"; "Date Picker"; "Date Range Picker"; "Time Picker" ]

    let dateTimeExpanded =
      Var.Create(List.contains (pageToString initialPage) dateTimeLabels)

    let componentsLabels = labelsForGroup "Components"
    let stylingLabels = labelsForGroup "Styling"
    let utilitiesLabels = labelsForGroup "Utilities"
    let showcasesLabels = labelsForGroup "Showcases"

    let componentsExpanded =
      Var.Create(List.contains (pageToString initialPage) componentsLabels)

    let stylingExpanded =
      Var.Create(List.contains (pageToString initialPage) stylingLabels)

    let utilitiesExpanded =
      Var.Create(List.contains (pageToString initialPage) utilitiesLabels)

    let showcasesExpanded =
      Var.Create(List.contains (pageToString initialPage) showcasesLabels)

    let groupEffects =
      selectedNav.View
      |> Doc.sink (fun selected ->
        match selected with
        | Some label ->
          for labels, expanded in
            [
              componentsLabels, componentsExpanded
              dateTimeLabels, dateTimeExpanded
              stylingLabels, stylingExpanded
              utilitiesLabels, utilitiesExpanded
              showcasesLabels, showcasesExpanded
            ] do
            if List.contains label labels then
              Var.Set expanded true
        | None -> ())

    let matches (query: string) (label: string) =
      let aliases =
        label |> stringToPage |> Option.map pageSearchAliases |> Option.defaultValue ""

      let terms = (label + " " + aliases).ToLower()

      query.Trim().ToLower().Split([| ' ' |], System.StringSplitOptions.RemoveEmptyEntries)
      |> Array.forall terms.Contains

    let navLeafItem (label: string) =
      a [
        attr.href (label |> stringToPage |> Option.map pageToHash |> Option.defaultValue "#home")
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Vertical.extraSmall
        Padding.Horizontal.medium
        Cursor.pointer
        BorderRadius.All.large
        Margin.All.extraSmall
        Attr.Class "weave-nav-leaf"
        Attr.DynamicStyle
          "display"
          (navSearch.View
           |> View.Map(fun query -> if matches query label then "" else "none"))
        Attr.DynamicClassPred "weave-nav-item--active" (selectedNav.View |> View.Map(fun s -> s = Some label))
        Attr.DynamicPred
          "aria-current"
          (selectedNav.View |> View.Map(fun s -> s = Some label))
          (View.Const "page")
      ] [ span [ Typography.body2 ] [ text label ] ]

    let rec navGroup categoryIcon (label: string) (isExpanded: Var<bool>) (labels: string list) =
      navSearch.View
      |> Doc.BindView(fun query ->
        let searching = query.Trim() <> ""
        let matching = labels |> List.filter (matches query)

        if searching && List.isEmpty matching then
          Doc.Empty
        else
          div [ Attr.Class "docs-nav-group" ] [
            if searching then
              div [ Attr.Class "docs-nav-results-heading" ] [ text label ]
              div [] (matching |> List.map navLeafItem)
            else
              button [
                attr.``type`` "button"
                Flex.Flex.allSizes
                AlignItems.center
                Padding.Vertical.extraSmall
                Padding.Horizontal.small
                Cursor.pointer
                Gap.All.g2
                Attr.Class "weave-nav-group-header"
                Attr.DynamicCustom
                  (fun el v -> el.SetAttribute("aria-expanded", v))
                  (isExpanded.View
                   |> View.Map(fun expanded -> if expanded then "true" else "false"))
                on.click (fun _ _ -> Var.Update isExpanded not)
              ] [
                Icon.create (
                  categoryIcon,
                  attrs = [ Attr.Style "font-size" "18px"; Attr.Create "aria-hidden" "true" ]
                )
                span [ Typography.overline; FlexItem.Flex.allSizes; Attr.Style "opacity" "0.7" ] [
                  text label
                ]
                isExpanded.View
                |> Doc.BindView(fun exp ->
                  Icon.create (
                    (if exp then
                       Icon.Hardware Hardware.KeyboardArrowDown
                     else
                       Icon.Hardware Hardware.KeyboardArrowRight),
                    attrs = [
                      Attr.Style "font-size" "16px"
                      Attr.Style "opacity" "0.6"
                      Attr.Create "aria-hidden" "true"
                    ]
                  ))
              ]

              isExpanded.View
              |> Doc.BindView(fun exp ->
                if exp then
                  div [ Attr.Class "docs-nav-items" ] [
                    for item in labels do
                      if label = "Components" && item = "Date & Time Guide" then
                        div [ Attr.Class "docs-nav-subgroup" ] [
                          navGroup
                            (Icon.Android Android.Widgets)
                            "Date & time"
                            dateTimeExpanded
                            dateTimeLabels
                        ]
                      elif label = "Components" && List.contains item dateTimeLabels then
                        ()
                      else
                        navLeafItem item
                  ]
                else
                  Doc.Empty)
          ])

    let allNavLabels =
      [ "Home"; "Getting Started" ]
      @ componentsLabels
      @ stylingLabels
      @ utilitiesLabels
      @ showcasesLabels

    let navList =
      div [ Padding.Vertical.extraSmall ] [
        div [ Attr.Class "docs-nav-search" ] [
          TextField.singleLine (
            navSearch,
            labelText = View.Const "Find a page",
            placeholder = View.Const "Search documentation",
            endAdornment =
              (navSearch.View
               |> Doc.BindView(fun query ->
                 if System.String.IsNullOrEmpty query then
                   Doc.Empty
                 else
                   Button.create (
                     text "Clear",
                     onClick = (fun () -> Var.Set navSearch ""),
                     attrs = [
                       Button.Variant.text
                       Attr.Class "docs-nav-search__clear"
                       Attr.Create "aria-label" "Clear documentation search"
                     ]
                   ))),
            attrs = [ TextField.Variant.outlined; TextField.Label.above; TextField.Width.full ]
          )
        ]
        nav [ Attr.Create "aria-label" "Documentation navigation" ] [
          a [
            attr.href "#home"
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
            Attr.DynamicPred
              "aria-current"
              (selectedNav.View |> View.Map(fun s -> s = Some "Home"))
              (View.Const "page")
          ] [
            Icon.create (
              Icon.UiActions UiActions.Home,
              attrs = [ Attr.Style "font-size" "18px"; Attr.Create "aria-hidden" "true" ]
            )
            span [ Typography.body2 ] [ text "Home" ]
          ]

          a [
            attr.href "#getting-started"
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
            Attr.DynamicPred
              "aria-current"
              (selectedNav.View |> View.Map(fun s -> s = Some "Getting Started"))
              (View.Const "page")
          ] [
            Icon.create (
              Icon.Social Social.RocketLaunch,
              attrs = [ Attr.Style "font-size" "18px"; Attr.Create "aria-hidden" "true" ]
            )
            span [ Typography.body2 ] [ text "Getting Started" ]
          ]

          Divider.create (attrs = [ Margin.Vertical.extraSmall ])

          navSearch.View
          |> Doc.BindView(fun query ->
            if query.Trim() <> "" && not (allNavLabels |> List.exists (matches query)) then
              div [ Attr.Class "docs-nav-no-results"; Attr.Create "role" "status" ] [
                text "No matching pages."
              ]
            else
              Doc.Empty)

          navGroup (Icon.Android Android.Widgets) "Components" componentsExpanded componentsLabels

          navGroup (Icon.Images Images.Palette) "Styling" stylingExpanded stylingLabels

          navGroup (Icon.Action Action.Code) "Utilities" utilitiesExpanded utilitiesLabels

          Divider.create (attrs = [ Margin.Vertical.extraSmall ])

          navGroup (Icon.UiActions UiActions.Apps) "Showcases" showcasesExpanded showcasesLabels
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
          attrs = [
            Margin.Right.extraSmall
            Attr.Create "aria-label" "Toggle navigation"
            Attr.Create "aria-controls" "docs-navigation"
            Attr.Dynamic
              "aria-expanded"
              (drawerOpen.View
               |> View.Map(fun expanded -> if expanded then "true" else "false"))
          ]
        )

        a [ attr.href (pageToHash Home); Attr.Class "docs-brand-link" ] [
          span [ Typography.h6 ] [ text "Weave" ]
        ]

        Spacer.create ()

        a [
          attr.href "https://github.com/1eyewonder/Weave"
          attr.target "_blank"
          Attr.Create "aria-label" "Weave on GitHub (opens in a new tab)"
          attr.rel "noopener noreferrer"
          Attr.Style "color" "inherit"
          Flex.Flex.allSizes
          AlignItems.center
          Padding.Horizontal.extraSmall
        ] [ githubSvg () ]

        IconButton.create (
          Doc.Concat [
            span [
              Attr.Class "docs-theme-icon docs-theme-icon--light"
              Attr.Create "aria-hidden" "true"
            ] [ Icon.create (Icon.Android Android.DarkMode) ]
            span [
              Attr.Class "docs-theme-icon docs-theme-icon--dark"
              Attr.Create "aria-hidden" "true"
            ] [ Icon.create (Icon.Android Android.LightMode) ]
          ],
          onClick = Weave.Docs.DocsTheme.toggle,
          attrs = [ Attr.Create "aria-label" "Toggle light and dark theme" ]
        )
      ]

    let initializeMain el =
      Var.Set mainEl (Some el)
      suppressScrollToTop <- false
      // Children can render before their scrolling container is mounted.
      refreshTocSections ()

    div [
      Attr.Style "height" "100vh"
      Attr.Style "overflow" "clip"
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      SurfaceColor.BackgroundColor.background
    ] [
      a [
        attr.href "#docs-main"
        Attr.Class "docs-skip-link"
        on.click (fun _ ev ->
          ev.PreventDefault()
          mainEl.Value |> Option.iter (fun el -> el?focus ()))
      ] [ text "Skip to content" ]
      navEffects
      groupEffects

      AppBar.create (appBarContent, attrs = [ Attr.Class "docs-appbar" ])

      DrawerContainer.create (
        mainContent =
          div [
            attr.id "docs-main"
            Attr.Create "role" "main"
            attr.tabindex "-1"
            cl "weave-main-content"
            Attr.Class "docs-main-content"
            Overflow.yAuto
            Attr.Style "height" "100%"
            on.afterRender initializeMain
            on.click (fun _ ev ->
              if
                not ev.DefaultPrevented
                && ev.Button = 0
                && not (ev.CtrlKey || ev.MetaKey || ev.ShiftKey || ev.AltKey)
              then
                let target = As<Dom.Element> ev.Target
                let link = As<Dom.Element>(target?closest ("a[href]"))

                if
                  not (isNull link)
                  && not (link.HasAttribute "target")
                  && not (link.HasAttribute "download")
                then
                  let href = link.GetAttribute "href"

                  if href.StartsWith "#" then
                    let destination = splitHashParts href
                    let current = splitHashParts (getLocationHash ())

                    // Native same-hash links do not fire hashchange. They still
                    // need to move and focus their section after manual scrolling.
                    if
                      destination.Length = 2
                      && current.Length = 2
                      && destination.[0] = current.[0]
                      && destination.[1] = current.[1]
                    then
                      ev.PreventDefault()
                      scrollToSectionAfterDelay destination.[1] 0)
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
                  div [
                    on.afterRender (fun el ->
                      JS.Document.Title <- pageToString page + " — Weave Docs"
                      let heading = el.QuerySelector "h1"

                      if not (isNull heading) then
                        heading.SetAttribute("tabindex", "-1")
                        heading?focus ({| preventScroll = true |})

                      refreshTocSections ())
                  ] [ pageQuickNavigation page; renderPage navigateTo page ])
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
            attrs = [
              attr.id "docs-navigation"
              Overflow.yAuto
              Attr.Class "docs-navigation-drawer"
            ]
          ),
        attrs = [
          FlexItem.Flex.allSizes
          Attr.Style "min-height" "0"
          Attr.Style "height" "100svh"
        ]
      )
    ]
