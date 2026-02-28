namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Tabs =

  /// Position of the tab strip relative to the tab panel content.
  [<RequireQualifiedAccess; Struct>]
  type Position =
    | Top
    | Bottom
    | Left
    | Right
    | Start
    | End

  module Position =
    let toClass position =
      match position with
      | Position.Top -> Css.``weave-tabs--top``
      | Position.Bottom -> Css.``weave-tabs--bottom``
      | Position.Left -> Css.``weave-tabs--left``
      | Position.Right -> Css.``weave-tabs--right``
      | Position.Start -> Css.``weave-tabs--start``
      | Position.End -> Css.``weave-tabs--end``

    let isHorizontal position =
      match position with
      | Position.Top
      | Position.Bottom -> true
      | Position.Left
      | Position.Right
      | Position.Start
      | Position.End -> false

  /// Whether to center tabs when they fit within the container.
  [<RequireQualifiedAccess; Struct>]
  type Alignment =
    | Start
    | Center

  module Alignment =
    let toClass alignment =
      match alignment with
      | Alignment.Start -> None
      | Alignment.Center -> Some Css.``weave-tabs__header--centered``

  module Color =
    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-tabs--primary``
      | BrandColor.Secondary -> Css.``weave-tabs--secondary``
      | BrandColor.Tertiary -> Css.``weave-tabs--tertiary``
      | BrandColor.Error -> Css.``weave-tabs--error``
      | BrandColor.Warning -> Css.``weave-tabs--warning``
      | BrandColor.Success -> Css.``weave-tabs--success``
      | BrandColor.Info -> Css.``weave-tabs--info``

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

  module TabDef =

    let createCustom header panel = {
      Header = header
      Disabled = View.Const false
      Panel = panel
    }

    let createText label panel =
      createCustom (span [ cl Css.``weave-tabs__tab-label`` ] [ text label ]) panel

    let createWithStartIcon label (icon: Doc) panel =
      createCustom
        (Doc.Concat [
          div [ cl Css.``weave-tabs__tab-icon`` ] [ icon ]
          span [ cl Css.``weave-tabs__tab-label`` ] [ text label ]
        ])
        panel

    let createWithEndIcon label (icon: Doc) panel =
      createCustom
        (Doc.Concat [
          span [ cl Css.``weave-tabs__tab-label`` ] [ text label ]
          div [ cl Css.``weave-tabs__tab-icon`` ] [ icon ]
        ])
        panel

    let createIconOnly (icon: Doc) panel =
      createCustom (div [ cl Css.``weave-tabs__tab-icon`` ] [ icon ]) panel

    let withDisabled disabled def = { def with Disabled = disabled }

open Tabs

[<JavaScript>]
type TabPanel =

  /// Renders a single tab panel that is visible only when its index matches the active tab.
  static member Create(content: Doc, index: int, activeIndex: View<int>, ?attrs: Attr list) =
    let attrs = defaultArg attrs []

    div [
      cl Css.``weave-tabs__panel``
      activeIndex
      |> View.Map(fun ai -> if ai = index then "block" else "none")
      |> Attr.DynamicStyle "display"
      yield! attrs
    ] [ content ]

[<JavaScript>]
type Tabs =

  /// Scrolls the tab header so that the tab at `targetIndex` is visible.
  static member private ScrollToTab(headerEl: Dom.Element, targetIndex: int, position: Position) =
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

  /// Checks whether the tab container overflows and needs scroll buttons.
  static member private CheckOverflow(headerEl: Dom.Element, position: Position) =
    let isHorizontal = Position.isHorizontal position

    if isHorizontal then
      headerEl?scrollWidth > headerEl?clientWidth
    else
      headerEl?scrollHeight > headerEl?clientHeight

  /// Scrolls the header by one "page" in the given direction.
  static member private ScrollByPage(headerEl: Dom.Element, position: Position, forward: bool) =
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

  /// Updates the indicator element to match the position and size of the active tab.
  static member private UpdateIndicator(headerEl: Dom.Element, activeIndex: int, position: Position) =
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

  static member Create
    (
      tabs: TabDef list,
      ?activeIndex: Var<int>,
      ?position: Position,
      ?alignment: Alignment,
      ?color: BrandColor,
      ?scrollBackIcon: Doc,
      ?scrollForwardIcon: Doc,
      ?attrs: Attr list
    ) =

    let activeIndex = defaultArg activeIndex (Var.Create 0)
    let position = defaultArg position Position.Top
    let alignment = defaultArg alignment Alignment.Start
    let color = defaultArg color BrandColor.Primary
    let attrs = defaultArg attrs []

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
      | Some el -> Var.Set showScrollButtons (Tabs.CheckOverflow(el, position))
      | None -> ()

    let updateIndicator () =
      match headerRef.Value with
      | Some el -> Tabs.UpdateIndicator(el, activeIndex.Value, position)
      | None -> ()

    let scrollBack () =
      match headerRef.Value with
      | Some el -> Tabs.ScrollByPage(el, position, false)
      | None -> ()

    let scrollForward () =
      match headerRef.Value with
      | Some el -> Tabs.ScrollByPage(el, position, true)
      | None -> ()

    let selectTab index =
      Var.Set activeIndex index

      match headerRef.Value with
      | Some el ->
        Tabs.ScrollToTab(el, index, position)
        JS.SetTimeout (fun () -> updateIndicator ()) 0 |> ignore
      | None -> ()

    let tabItem (tabDef: TabDef) (index: int) =
      let isActive = activeIndex.View |> View.Map(fun ai -> ai = index)

      button [
        cl Css.``weave-tabs__tab``
        attr.``type`` "button"
        isActive |> Attr.DynamicClassPred Css.``weave-tabs__tab--active``
        tabDef.Disabled |> Attr.DynamicClassPred Css.``weave-tabs__tab--disabled``
        Attr.enabled (View.not tabDef.Disabled)
        on.clickTapView tabDef.Disabled (fun _ _ disabled ->
          if not disabled then
            selectTab index)
      ] [ tabDef.Header ]

    let indicator =
      div [ cl Css.``weave-tabs__indicator``; BrandColor.toBackgroundColor color ] []

    let header =
      div [
        cl Css.``weave-tabs__header``
        Alignment.toClass alignment |> Attr.bindOption cl
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
        // On scroll event recalc overflow
        Attr.Handler "scroll" (fun _ _ -> updateOverflow ())
      ] [
        yield! tabs |> List.mapi (fun i t -> tabItem t i)

        indicator

        // Reactive sink: whenever activeIndex changes, update the indicator
        Doc.sinkCached (fun _ -> updateIndicator ()) activeIndex.View
      ]

    let scrollBackBtn =
      button [
        cl Css.``weave-tabs__scroll-btn``
        attr.``type`` "button"
        on.clickTap (fun _ _ -> scrollBack ())
        View.not showScrollButtons.View |> Attr.DynamicClassPred Css.``d-none``
      ] [ scrollBackIcon ]

    let scrollFwdBtn =
      button [
        cl Css.``weave-tabs__scroll-btn``
        attr.``type`` "button"
        on.clickTap (fun _ _ -> scrollForward ())
        View.not showScrollButtons.View |> Attr.DynamicClassPred Css.``d-none``
      ] [ scrollForwardIcon ]

    let panels =
      div [ cl Css.``weave-tabs__panels`` ] [
        yield! tabs |> List.mapi (fun i t -> TabPanel.Create(t.Panel, i, activeIndex.View))
      ]

    div [
      cl Css.``weave-tabs``
      cl (Position.toClass position)
      cl (Color.toClass color)
      yield! attrs
    ] [
      div [ cl Css.``weave-tabs__header-wrapper`` ] [ scrollBackBtn; header; scrollFwdBtn ]
      panels
    ]
