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
module DropdownMenu =

  /// <summary>
  /// Determines how the dropdown menu trigger opens the menu.
  /// </summary>
  [<RequireQualifiedAccess; Struct>]
  type OpenOn =
    | Click
    | Hover

  module Color =

    let primary = cl Css.``weave-dropdownmenu--primary``
    let secondary = cl Css.``weave-dropdownmenu--secondary``
    let tertiary = cl Css.``weave-dropdownmenu--tertiary``
    let error = cl Css.``weave-dropdownmenu--error``
    let warning = cl Css.``weave-dropdownmenu--warning``
    let success = cl Css.``weave-dropdownmenu--success``
    let info = cl Css.``weave-dropdownmenu--info``

  module Width =

    let full = cl Css.``weave-dropdownmenu--full-width``

  module PopoverWidth =

    let unbound = cl Css.``weave-dropdownmenu--popover-unbound``

  module AnchorOrigin =

    let topLeft = cl Css.``weave-dropdownmenu--anchor-origin-top-left``
    let topCenter = cl Css.``weave-dropdownmenu--anchor-origin-top-center``
    let topRight = cl Css.``weave-dropdownmenu--anchor-origin-top-right``
    let centerLeft = cl Css.``weave-dropdownmenu--anchor-origin-center-left``
    let center = cl Css.``weave-dropdownmenu--anchor-origin-center-center``
    let centerRight = cl Css.``weave-dropdownmenu--anchor-origin-center-right``
    let bottomLeft = cl Css.``weave-dropdownmenu--anchor-origin-bottom-left``
    let bottomCenter = cl Css.``weave-dropdownmenu--anchor-origin-bottom-center``
    let bottomRight = cl Css.``weave-dropdownmenu--anchor-origin-bottom-right``

  module TransformOrigin =

    let topLeft = cl Css.``weave-dropdownmenu--transform-origin-top-left``
    let topCenter = cl Css.``weave-dropdownmenu--transform-origin-top-center``
    let topRight = cl Css.``weave-dropdownmenu--transform-origin-top-right``
    let centerLeft = cl Css.``weave-dropdownmenu--transform-origin-center-left``
    let center = cl Css.``weave-dropdownmenu--transform-origin-center-center``
    let centerRight = cl Css.``weave-dropdownmenu--transform-origin-center-right``
    let bottomLeft = cl Css.``weave-dropdownmenu--transform-origin-bottom-left``
    let bottomCenter = cl Css.``weave-dropdownmenu--transform-origin-bottom-center``
    let bottomRight = cl Css.``weave-dropdownmenu--transform-origin-bottom-right``

  type DropdownMenuItemDef = {
    Content: Doc
    Text: string
    OnClick: unit -> unit
    Icon: Doc option
    Disabled: View<bool>
    Attrs: Attr list
  }

  [<RequireQualifiedAccess>]
  type DropdownMenuItemKind =
    | Action of DropdownMenuItemDef
    | Divider

  module Render =

    let chevron (isOpen: View<bool>) =
      span [
        cl Css.``weave-dropdownmenu__chevron``
        Attr.DynamicClassPred Css.``weave-dropdownmenu__chevron--open`` isOpen
      ] [ text "▼" ]

    let divider () =
      div [ cl Css.``weave-dropdownmenu__divider``; Attr.Create "role" "separator" ] []

    let actionItem
      (dropdownMenuId: string)
      (itemIndex: int)
      (def: DropdownMenuItemDef)
      (highlightedIndex: Var<int>)
      (openVar: Var<bool>)
      (enabled: View<bool>)
      =
      let itemEnabled =
        (enabled, def.Disabled) ||> View.map2Cached (fun e d -> e && not d)

      let isHighlighted =
        highlightedIndex.View |> View.MapCached(fun hi -> hi = itemIndex)

      let itemId = sprintf "%s-item-%d" dropdownMenuId itemIndex

      div [
        cl Css.``weave-dropdownmenu__item``
        Attr.DynamicClassPred Css.``weave-dropdownmenu__item--highlighted`` isHighlighted
        Attr.DynamicClassPred Css.``weave-dropdownmenu__item--disabled`` def.Disabled
        Attr.Create "id" itemId
        Attr.Create "role" "menuitem"
        Attr.Create "tabindex" "-1"
        Attr.DynamicCustom
          (fun el v -> el.SetAttribute("aria-disabled", v))
          (def.Disabled |> View.Map(fun d -> if d then "true" else "false"))

        on.clickTapViewGuarded itemEnabled (fun () ->
          def.OnClick()
          openVar.Value <- false)

        on.mouseEnter (fun _ _ -> highlightedIndex.Value <- itemIndex)
        yield! def.Attrs
      ] [
        match def.Icon with
        | Some icon -> span [ cl Css.``weave-dropdownmenu__item-icon`` ] [ icon ]
        | None -> ()

        span [ cl Css.``weave-dropdownmenu__item-text`` ] [ def.Content ]
      ]

    let renderItem
      (dropdownMenuId: string)
      (itemIndex: int)
      (kind: DropdownMenuItemKind)
      (highlightedIndex: Var<int>)
      (openVar: Var<bool>)
      (enabled: View<bool>)
      =
      match kind with
      | DropdownMenuItemKind.Action def ->
        actionItem dropdownMenuId itemIndex def highlightedIndex openVar enabled
      | DropdownMenuItemKind.Divider -> divider ()

open DropdownMenu

[<JavaScript>]
type DropdownMenuItem =

  /// <summary>
  /// Creates an actionable menu item for a DropdownMenu.
  /// </summary>
  /// <param name="content">The Doc to render as the item's visible content.</param>
  /// <param name="onClick">Callback invoked when the item is activated. The menu closes automatically.</param>
  /// <param name="text">Accessible text for the item. Used for type-ahead search. Defaults to empty string.</param>
  /// <param name="icon">Optional leading icon Doc.</param>
  /// <param name="disabled">Reactive view controlling the disabled state. Defaults to false.</param>
  /// <param name="attrs">Additional attributes merged onto the item element.</param>
  static member create
    (content: Doc, onClick: unit -> unit, ?text: string, ?icon: Doc, ?disabled: View<bool>, ?attrs: Attr list)
    : DropdownMenu.DropdownMenuItemKind =
    DropdownMenu.DropdownMenuItemKind.Action {
      Content = content
      Text = defaultArg text ""
      OnClick = onClick
      Icon = icon
      Disabled = defaultArg disabled (View.Const false)
      Attrs = defaultArg attrs []
    }

  /// <summary>
  /// Creates a visual divider between groups of menu items.
  /// </summary>
  static member divider() : DropdownMenu.DropdownMenuItemKind =
    DropdownMenu.DropdownMenuItemKind.Divider

[<JavaScript>]
type DropdownMenu =

  /// <summary>
  /// Creates a dropdown menu component anchored to a trigger button.
  /// </summary>
  /// <param name="triggerContent">The Doc displayed inside the trigger button.</param>
  /// <param name="items">Reactive view of the menu item list. Use View.Const for static lists.</param>
  /// <param name="isOpen">Two-way binding for the open/closed state. Caller owns the Var.</param>
  /// <param name="openOn">Whether the menu opens on Click or Hover. Defaults to Click.</param>
  /// <param name="enabled">Reactive view controlling whether the dropdown menu trigger is interactive. Defaults to true.</param>
  /// <param name="closeOnOutsideClick">Whether clicking outside closes the menu. Defaults to true.</param>
  /// <param name="triggerAttrs">Additional attributes applied to the trigger button.</param>
  /// <param name="attrs">Additional attributes applied to the root container.</param>
  static member create
    (
      triggerContent: Doc,
      items: View<DropdownMenuItemKind list>,
      ?isOpen: Var<bool>,
      ?openOn: OpenOn,
      ?enabled: View<bool>,
      ?closeOnOutsideClick: bool,
      ?triggerAttrs: Attr list,
      ?attrs: Attr list
    ) =

    let openVar = defaultArg isOpen (Var.Create false)
    let openOn = defaultArg openOn OpenOn.Click
    let enabled = defaultArg enabled (View.Const true)
    let closeOnOutsideClick = defaultArg closeOnOutsideClick true
    let triggerAttrs = defaultArg triggerAttrs []
    let attrs = defaultArg attrs []

    let highlightedIndex = Var.Create -1
    let dropdownMenuId = WeaveId.create "weave-dropdownmenu"
    let menuId = sprintf "%s-menu" dropdownMenuId

    // Snapshot for synchronous access in keyboard handlers
    let itemsSnapshot = ref ([]: DropdownMenuItemKind list)

    let itemsTracked =
      items
      |> View.Map(fun defs ->
        itemsSnapshot.Value <- defs
        defs)

    // Element refs for outside-click detection and focus management
    let rootRef = ref (JS.Document.CreateElement "div")
    let popoverRef = ref (JS.Document.CreateElement "div")
    let triggerRef = ref (JS.Document.CreateElement "button")

    let findNextActionIndex (container: Dom.Element) (fromIndex: int) (direction: int) =
      let currentItems = itemsSnapshot.Value
      let count = List.length currentItems

      if count = 0 then
        None
      else
        let disabledClass = Css.``weave-dropdownmenu__item--disabled``

        let rec search idx steps =
          if steps >= count then
            None
          else
            let candidate = (idx + direction + count) % count

            match currentItems.[candidate] with
            | DropdownMenuItemKind.Action _ ->
              let itemEl = container.QuerySelector(sprintf "#%s-item-%d" dropdownMenuId candidate)

              let isDisabled = not (isNull itemEl) && itemEl.ClassList.Contains(disabledClass)

              if not isDisabled then
                Some candidate
              else
                search candidate (steps + 1)
            | DropdownMenuItemKind.Divider -> search candidate (steps + 1)

        search fromIndex 0

    let focusItemAt (index: int) =
      let container = rootRef.Value
      let itemEl = container.QuerySelector(sprintf "#%s-item-%d" dropdownMenuId index)

      if not (isNull itemEl) then
        highlightedIndex.Value <- index
        JS.Window.SetTimeout((fun () -> itemEl?focus ()), 0) |> ignore

    let closeAndRefocusTrigger () =
      openVar.Value <- false
      highlightedIndex.Value <- -1
      triggerRef.Value?focus()

    /// Handle keyboard events on the trigger button.
    /// Enter/Space are NOT handled here — Button's built-in clickTapKeyViewGuarded
    /// already fires onClick, which toggles the menu. Handling them here would
    /// cause a double-toggle.
    let handleTriggerKeyDown (ev: Dom.KeyboardEvent) =
      match ev with
      | Key.ArrowDown ->
        ev.PreventDefault()

        if not openVar.Value then
          openVar.Value <- true

        highlightedIndex.Value <- -1

        // Deferred — popover needs to render before we can query items
        JS.Window.SetTimeout(
          (fun () ->
            match findNextActionIndex rootRef.Value -1 1 with
            | Some idx -> focusItemAt idx
            | None -> ()),
          0
        )
        |> ignore

      | Key.ArrowUp ->
        ev.PreventDefault()

        if not openVar.Value then
          openVar.Value <- true

        highlightedIndex.Value <- -1

        JS.Window.SetTimeout(
          (fun () ->
            let lastIndex = List.length itemsSnapshot.Value

            match findNextActionIndex rootRef.Value lastIndex -1 with
            | Some idx -> focusItemAt idx
            | None -> ()),
          0
        )
        |> ignore

      | Key.Escape ->
        if openVar.Value then
          ev.PreventDefault()
          closeAndRefocusTrigger ()

      | Key.Tab ->
        if openVar.Value then
          openVar.Value <- false
          highlightedIndex.Value <- -1
      | _ -> ()

    /// Handle keyboard events when focus is on a menu item.
    /// Installed on the menu list container so it catches events bubbling from items.
    let handleMenuKeyDown (ev: Dom.KeyboardEvent) =
      let container = rootRef.Value
      let currentItems = itemsSnapshot.Value
      let count = List.length currentItems

      match ev with
      | Key.ArrowDown ->
        ev.PreventDefault()

        match findNextActionIndex container highlightedIndex.Value 1 with
        | Some idx -> focusItemAt idx
        | None -> ()

      | Key.ArrowUp ->
        ev.PreventDefault()

        match findNextActionIndex container highlightedIndex.Value -1 with
        | Some idx -> focusItemAt idx
        | None -> ()

      | Key.Activate ->
        ev.PreventDefault()
        let idx = highlightedIndex.Value

        if idx >= 0 && idx < count then
          match currentItems.[idx] with
          | DropdownMenuItemKind.Action def ->
            let itemEl = container.QuerySelector(sprintf "#%s-item-%d" dropdownMenuId idx)

            let disabledClass = Css.``weave-dropdownmenu__item--disabled``

            let isDisabled = not (isNull itemEl) && itemEl.ClassList.Contains(disabledClass)

            if not isDisabled then
              def.OnClick()
              closeAndRefocusTrigger ()
          | _ -> ()

      | Key.Escape ->
        ev.PreventDefault()
        closeAndRefocusTrigger ()

      | Key.Home ->
        ev.PreventDefault()

        match findNextActionIndex container -1 1 with
        | Some idx -> focusItemAt idx
        | None -> ()

      | Key.End ->
        ev.PreventDefault()

        if count > 0 then
          match findNextActionIndex container count -1 with
          | Some idx -> focusItemAt idx
          | None -> ()

      | Key.Tab ->
        openVar.Value <- false
        highlightedIndex.Value <- -1
      | _ -> ()

    let triggerButton =
      let contents = [ triggerContent; Render.chevron openVar.View ] |> Doc.Concat

      let clickHandler =
        match openOn with
        | OpenOn.Click ->
          fun () ->
            openVar.Value <- not openVar.Value

            if openVar.Value then
              highlightedIndex.Value <- -1
        | OpenOn.Hover -> fun () -> ()

      Button.create (
        contents,
        onClick = clickHandler,
        enabled = enabled,
        attrs = [
          Attr.Create "aria-haspopup" "menu"
          Attr.Create "aria-controls" menuId
          Attr.DynamicCustom
            (fun el v -> el.SetAttribute("aria-expanded", v))
            (openVar.View |> View.Map(sprintf "%b"))
          on.afterRender (fun el -> triggerRef.Value <- el)
          on.keyDown (fun _ ev -> handleTriggerKeyDown ev)
          yield! triggerAttrs
        ]
      )

    let popover =
      openVar.View
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          div [
            cl Css.``weave-dropdownmenu__popover``
            on.afterRender (fun el -> popoverRef.Value <- el)
            on.clickTap (fun _ ev -> ev?stopPropagation ())
          ] [
            div [
              cl Css.``weave-dropdownmenu__list``
              Attr.Create "id" menuId
              Attr.Create "role" "menu"
              on.keyDown (fun _ ev -> handleMenuKeyDown ev)
            ] [
              itemsTracked
              |> Doc.BindView(fun currentItems ->
                currentItems
                |> List.mapi (fun i kind ->
                  Render.renderItem dropdownMenuId i kind highlightedIndex openVar enabled)
                |> Doc.Concat)
            ]
          ]
        else
          Doc.Empty)

    let outsideClickWatcher =
      if closeOnOutsideClick then
        openVar.View
        |> DocumentEventListener.onMouseDown [ rootRef; popoverRef ] (fun () ->
          openVar.Value <- false
          highlightedIndex.Value <- -1)
      else
        Doc.Empty

    let hoverAttrs =
      match openOn with
      | OpenOn.Hover -> [
          on.mouseEnter (fun _ _ ->
            if not openVar.Value then
              openVar.Value <- true
              highlightedIndex.Value <- -1)
          on.mouseLeave (fun _ _ ->
            if openVar.Value then
              openVar.Value <- false
              highlightedIndex.Value <- -1)
        ]
      | OpenOn.Click -> []

    [
      outsideClickWatcher

      div [
        cl Css.``weave-dropdownmenu``
        Attr.DynamicClassPred Css.``weave-dropdownmenu--open`` openVar.View
        Disabled.disabledClass Css.``weave-dropdownmenu--disabled`` enabled
        on.afterRender (fun el -> rootRef.Value <- el)
        yield! hoverAttrs
        yield! attrs
      ] [ triggerButton; popover ]
    ]
    |> Doc.Concat
