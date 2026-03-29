namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Tabs =

  /// <summary>
  /// Position of the tab strip relative to the tab panel content.
  /// </summary>
  [<RequireQualifiedAccess; Struct>]
  type Position =
    | Top
    | Bottom
    | Left
    | Right
    | Start
    | End

  module Position =

    let top = cl Css.``weave-tabs--top``
    let bottom = cl Css.``weave-tabs--bottom``
    let left = cl Css.``weave-tabs--left``
    let right = cl Css.``weave-tabs--right``
    let start = cl Css.``weave-tabs--start``
    let end_ = cl Css.``weave-tabs--end``

    let isHorizontal position =
      match position with
      | Position.Top
      | Position.Bottom -> true
      | Position.Left
      | Position.Right
      | Position.Start
      | Position.End -> false

  /// <summary>
  /// Whether to center tabs when they fit within the container.
  /// </summary>
  module Alignment =

    let center = cl Css.``weave-tabs--centered``

  /// <summary>
  /// Visual style of the tab header strip.
  /// </summary>
  module Variant =

    let text = cl Css.``weave-tabs--text``
    let outlined = cl Css.``weave-tabs--outlined``
    let filled = cl Css.``weave-tabs--filled``

  /// <summary>
  /// Sets the background color of the tab header strip.
  /// </summary>
  module HeaderBackground =

    let primary = Attr.Style "--tabs-header-background" Palette.primary
    let secondary = Attr.Style "--tabs-header-background" Palette.secondary
    let tertiary = Attr.Style "--tabs-header-background" Palette.tertiary
    let error = Attr.Style "--tabs-header-background" Palette.error
    let warning = Attr.Style "--tabs-header-background" Palette.warning
    let success = Attr.Style "--tabs-header-background" Palette.success
    let info = Attr.Style "--tabs-header-background" Palette.info

    let custom (value: string) =
      Attr.Style "--tabs-header-background" value

  module Color =

    let primary = cl Css.``weave-tabs--primary``
    let secondary = cl Css.``weave-tabs--secondary``
    let tertiary = cl Css.``weave-tabs--tertiary``
    let error = cl Css.``weave-tabs--error``
    let warning = cl Css.``weave-tabs--warning``
    let success = cl Css.``weave-tabs--success``
    let info = cl Css.``weave-tabs--info``

  /// <summary>
  /// Represents one tab entry's configuration.
  /// </summary>
  type TabDef = {
    ///<summary>
    /// The Doc rendered inside the tab button (label, icon, or any custom content).
    /// </summary>
    Header: Doc
    /// <summary>
    /// Whether this tab is disabled. Disabled tabs are not clickable and have a distinct style.
    /// </summary>
    Disabled: View<bool>
    /// <summary>
    /// The Doc rendered in the content area when this tab is active.
    /// </summary>
    Panel: Doc
  }

open Tabs

[<JavaScript>]
type TabItem =

  /// Creates a text-labeled tab with optional start/end icons.
  static member create
    (label: string, panel: Doc, ?startIcon: Doc, ?endIcon: Doc, ?disabled: View<bool>)
    : Tabs.TabDef =
    let header =
      Doc.Concat [
        match startIcon with
        | Some icon -> div [ cl Css.``weave-tabs__tab-icon`` ] [ icon ]
        | None -> ()

        span [ cl Css.``weave-tabs__tab-label`` ] [ text label ]

        match endIcon with
        | Some icon -> div [ cl Css.``weave-tabs__tab-icon`` ] [ icon ]
        | None -> ()
      ]

    {
      Header = header
      Disabled = defaultArg disabled (View.Const false)
      Panel = panel
    }

  /// Creates an icon-only tab (no text label).
  static member createIconOnly(icon: Doc, panel: Doc, ?disabled: View<bool>) : Tabs.TabDef = {
    Header = div [ cl Css.``weave-tabs__tab-icon`` ] [ icon ]
    Disabled = defaultArg disabled (View.Const false)
    Panel = panel
  }

  /// Creates a tab with a fully custom header Doc.
  static member createCustom(header: Doc, panel: Doc, ?disabled: View<bool>) : Tabs.TabDef = {
    Header = header
    Disabled = defaultArg disabled (View.Const false)
    Panel = panel
  }

[<JavaScript>]
type TabPanel =

  /// <summary>
  /// Renders a single tab panel that is visible only when its index matches the active tab.
  /// </summary>
  static member create(content: Doc, index: int, activeIndex: View<int>, ?groupId: int, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    let idAttrs =
      match groupId with
      | Some gid -> [
          Attr.Create "id" (sprintf "weave-tabs-%d-panel-%d" gid index)
          Attr.Create "aria-labelledby" (sprintf "weave-tabs-%d-tab-%d" gid index)
        ]
      | None -> []

    div [
      cl Css.``weave-tabs__panel``
      Attr.Create "role" "tabpanel"
      Attr.Create "tabindex" "0"
      yield! idAttrs
      activeIndex
      |> View.Map(fun ai -> if ai = index then "block" else "none")
      |> Attr.DynamicStyle "display"
      yield! attrs
    ] [ content ]

[<JavaScript>]
module private TabsInternal =

  /// <summary>
  /// Scrolls the tab header so that the tab at `targetIndex` is visible.
  /// </summary>
  let scrollToTab (headerEl: Dom.Element) (targetIndex: int) (position: Position) =
    let tabs = headerEl.QuerySelectorAll(".weave-tabs__tab")

    if targetIndex >= 0 && targetIndex < tabs.Length then
      let tab = tabs.[targetIndex] :?> Dom.Element
      let isHorizontal = Position.isHorizontal position

      if isHorizontal then
        let tabLeft = tab?offsetLeft: float
        let tabWidth = tab?offsetWidth: float
        let headerScroll = headerEl?scrollLeft: float
        let headerWidth = headerEl?offsetWidth: float

        if tabLeft < headerScroll then
          headerEl?scrollLeft <- tabLeft
        elif tabLeft + tabWidth > headerScroll + headerWidth then
          headerEl?scrollLeft <- tabLeft + tabWidth - headerWidth
      else
        let tabTop = tab?offsetTop: float
        let tabHeight = tab?offsetHeight: float
        let headerScroll = headerEl?scrollTop: float
        let headerHeight = headerEl?offsetHeight: float

        if tabTop < headerScroll then
          headerEl?scrollTop <- tabTop
        elif tabTop + tabHeight > headerScroll + headerHeight then
          headerEl?scrollTop <- tabTop + tabHeight - headerHeight

  /// <summary>
  /// Checks whether the tab container overflows and needs scroll buttons.
  /// </summary>
  let checkOverflow (headerEl: Dom.Element) (position: Position) =
    let isHorizontal = Position.isHorizontal position

    if isHorizontal then
      headerEl?scrollWidth > headerEl?clientWidth
    else
      headerEl?scrollHeight > headerEl?clientHeight

  /// <summary>
  /// Scrolls the header by one "page" in the given direction.
  /// </summary>
  let scrollByPage (headerEl: Dom.Element) (position: Position) (forward: bool) =
    let isHorizontal = Position.isHorizontal position

    if isHorizontal then
      let amount: float = headerEl?offsetWidth
      let current: float = headerEl?scrollLeft

      headerEl?scrollLeft <-
        if forward then
          current + amount
        else
          max 0.0 (current - amount)
    else
      let amount: float = headerEl?offsetHeight
      let current: float = headerEl?scrollTop

      headerEl?scrollTop <-
        if forward then
          current + amount
        else
          max 0.0 (current - amount)

  /// <summary>
  /// Updates the indicator element to match the position and size of the active tab.
  /// </summary>
  let updateIndicator (headerEl: Dom.Element) (activeIndex: int) (position: Position) =
    let tabs = headerEl.QuerySelectorAll(".weave-tabs__tab")
    let indicatorEl = headerEl.QuerySelector(".weave-tabs__indicator")

    if not (isNull indicatorEl) && activeIndex >= 0 && activeIndex < tabs.Length then
      let tab = tabs.[activeIndex] :?> Dom.Element
      let isHorizontal = Position.isHorizontal position

      if isHorizontal then
        let tabLeft = tab?offsetLeft: float
        let tabWidth = tab?offsetWidth: float
        indicatorEl?style?left <- string<float> tabLeft + "px"
        indicatorEl?style?width <- string<float> tabWidth + "px"
      else
        let tabTop = tab?offsetTop: float
        let tabHeight = tab?offsetHeight: float
        indicatorEl?style?top <- string<float> tabTop + "px"
        indicatorEl?style?height <- string<float> tabHeight + "px"

  let mutable private nextGroupId = 0

  let newGroupId () =
    let id = nextGroupId
    nextGroupId <- nextGroupId + 1
    id

  let private isTabDisabled (el: Dom.Element) : bool = el?disabled

  let findEnabledTabIndex (headerEl: Dom.Element) (fromIndex: int) (direction: int) =
    let tabs = headerEl.QuerySelectorAll("[role='tab']")
    let count = tabs.Length

    if count = 0 then
      None
    else
      let mutable i = (fromIndex + direction + count) % count
      let mutable iterations = 0

      while iterations < count && isTabDisabled (tabs.[i] :?> Dom.Element) do
        i <- (i + direction + count) % count
        iterations <- iterations + 1

      if iterations < count then Some i else None

  let findFirstEnabledTabIndex (headerEl: Dom.Element) =
    let tabs = headerEl.QuerySelectorAll("[role='tab']")
    let count = tabs.Length
    let mutable i = 0

    while i < count && isTabDisabled (tabs.[i] :?> Dom.Element) do
      i <- i + 1

    if i < count then Some i else None

  let findLastEnabledTabIndex (headerEl: Dom.Element) =
    let tabs = headerEl.QuerySelectorAll("[role='tab']")
    let count = tabs.Length
    let mutable i = count - 1

    while i >= 0 && isTabDisabled (tabs.[i] :?> Dom.Element) do
      i <- i - 1

    if i >= 0 then Some i else None

  let focusTabAtIndex (headerEl: Dom.Element) (index: int) =
    let tabs = headerEl.QuerySelectorAll("[role='tab']")

    if index >= 0 && index < tabs.Length then
      let el = tabs.[index] :?> Dom.Element
      el?focus ()

[<JavaScript>]
type Tabs =

  static member create
    (
      tabs: View<TabDef list>,
      ?activeIndex: Var<int>,
      ?position: Position,
      ?scrollBackIcon: Doc,
      ?scrollForwardIcon: Doc,
      ?attrs: Attr list
    ) =

    let activeIndex = defaultArg activeIndex (Var.Create 0)
    let position = defaultArg position Position.Top
    let attrs = defaultArg attrs []
    let groupId = TabsInternal.newGroupId ()

    let headerRef = Var.Create<Dom.Element option> None
    let showScrollButtons = Var.Create false

    let scrollBackIcon =
      defaultArg
        scrollBackIcon
        (span [ cl Css.``weave-tabs__scroll-icon`` ] [
          text (
            if Position.isHorizontal position then
              "\u2039"
            else
              "\u25B2"
          )
        ])

    let scrollForwardIcon =
      defaultArg
        scrollForwardIcon
        (span [ cl Css.``weave-tabs__scroll-icon`` ] [
          text (
            if Position.isHorizontal position then
              "\u203A"
            else
              "\u25BC"
          )
        ])

    let updateOverflow () =
      match headerRef.Value with
      | Some el -> Var.Set showScrollButtons (TabsInternal.checkOverflow el position)
      | None -> ()

    let updateIndicator () =
      match headerRef.Value with
      | Some el -> TabsInternal.updateIndicator el activeIndex.Value position
      | None -> ()

    let scrollBack () =
      match headerRef.Value with
      | Some el -> TabsInternal.scrollByPage el position false
      | None -> ()

    let scrollForward () =
      match headerRef.Value with
      | Some el -> TabsInternal.scrollByPage el position true
      | None -> ()

    let selectTab index =
      Var.Set activeIndex index

      match headerRef.Value with
      | Some el ->
        TabsInternal.scrollToTab el index position
        JS.SetTimeout (fun () -> updateIndicator ()) 0 |> ignore
      | None -> ()

    let tabItem (tabDef: TabDef) (index: int) =
      let isActive = activeIndex.View |> View.Map(fun ai -> ai = index)

      let navigateTo findFn =
        match headerRef.Value with
        | Some el ->
          match findFn el with
          | Some targetIndex ->
            selectTab targetIndex
            TabsInternal.focusTabAtIndex el targetIndex
          | None -> ()
        | None -> ()

      let isHorizontal = Position.isHorizontal position
      let nextKey = if isHorizontal then "ArrowRight" else "ArrowDown"
      let prevKey = if isHorizontal then "ArrowLeft" else "ArrowUp"

      button [
        cl Css.``weave-tabs__tab``
        attr.``type`` "button"
        Attr.Create "role" "tab"
        Attr.Create "id" (sprintf "weave-tabs-%d-tab-%d" groupId index)
        Attr.Create "aria-controls" (sprintf "weave-tabs-%d-panel-%d" groupId index)

        isActive
        |> View.Map(fun active -> if active then "true" else "false")
        |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-selected", v))

        isActive
        |> View.Map(fun active -> if active then "0" else "-1")
        |> Attr.DynamicCustom(fun el v -> el.SetAttribute("tabindex", v))

        isActive |> Attr.DynamicClassPred Css.``weave-tabs__tab--active``
        tabDef.Disabled |> Attr.DynamicClassPred Css.``weave-tabs__tab--disabled``
        Attr.enabled (View.not tabDef.Disabled)

        on.clickTapView tabDef.Disabled (fun _ _ disabled ->
          if not disabled then
            selectTab index)

        on.keyDown (fun _ ev ->
          match ev with
          | Key.NavKey nextKey ->
            ev.PreventDefault()
            navigateTo (fun el -> TabsInternal.findEnabledTabIndex el index 1)
          | Key.NavKey prevKey ->
            ev.PreventDefault()
            navigateTo (fun el -> TabsInternal.findEnabledTabIndex el index -1)
          | Key.Home ->
            ev.PreventDefault()
            navigateTo TabsInternal.findFirstEnabledTabIndex
          | Key.End ->
            ev.PreventDefault()
            navigateTo TabsInternal.findLastEnabledTabIndex
          | Key.Activate ->
            ev.PreventDefault()
            selectTab index
          | _ -> ())
      ] [ tabDef.Header ]

    let indicator = div [ cl Css.``weave-tabs__indicator`` ] []

    let header =
      div [
        cl Css.``weave-tabs__header``
        Attr.Create "role" "tablist"
        on.afterRender (fun el ->
          Var.Set headerRef (Some el)

          JS.SetTimeout
            (fun () ->
              updateOverflow ()
              updateIndicator ()

              JS.Window.AddEventListener(
                "resize",
                System.Action<Dom.Event>(fun _ ->
                  updateOverflow ()
                  updateIndicator ())
              ))
            0
          |> ignore)

        on.scroll (fun _ _ -> updateOverflow ())
      ] [
        tabs |> Doc.BindView(List.mapi (fun i t -> tabItem t i) >> Doc.Concat)

        indicator

        Doc.sinkCached (fun _ -> updateIndicator ()) activeIndex.View
      ]

    let scrollBackBtn =
      button [
        cl Css.``weave-tabs__scroll-btn``
        attr.``type`` "button"
        Attr.Create "tabindex" "-1"
        Attr.Create "aria-label" "Scroll tabs backward"
        on.clickTap (fun _ _ -> scrollBack ())
        View.not showScrollButtons.View |> Attr.DynamicClassPred Css.``weave-d-none``
      ] [ scrollBackIcon ]

    let scrollFwdBtn =
      button [
        cl Css.``weave-tabs__scroll-btn``
        attr.``type`` "button"
        Attr.Create "tabindex" "-1"
        Attr.Create "aria-label" "Scroll tabs forward"
        on.clickTap (fun _ _ -> scrollForward ())
        View.not showScrollButtons.View |> Attr.DynamicClassPred Css.``weave-d-none``
      ] [ scrollForwardIcon ]

    let panels =
      div [ cl Css.``weave-tabs__panels`` ] [
        tabs
        |> Doc.BindView(
          List.mapi (fun i t -> TabPanel.create (t.Panel, i, activeIndex.View, groupId = groupId))
          >> Doc.Concat
        )
      ]

    div [
      cl Css.``weave-tabs``
      match position with
      | Position.Top -> Position.top
      | Position.Bottom -> Position.bottom
      | Position.Left -> Position.left
      | Position.Right -> Position.right
      | Position.Start -> Position.start
      | Position.End -> Position.end_
      yield! attrs
    ] [
      div [ cl Css.``weave-tabs__header-wrapper`` ] [ scrollBackBtn; header; scrollFwdBtn ]
      panels
    ]
