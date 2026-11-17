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
        Attr.Create "aria-hidden" "true"
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

        on.clickViewGuarded itemEnabled (fun () ->
          def.OnClick()
          Var.Set openVar false)

        on.mouseEnter (fun _ _ -> Var.Set highlightedIndex itemIndex)
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
  /// <param name="text">Text for keyboard navigation. Typing a character focuses the next enabled item whose text starts with it, ignoring case. Defaults to empty.</param>
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

  [<Inline "Array.from($key).length === 1">]
  static member private isSingleCharacter(key: string) =
    key.Length = 1 || (key.Length = 2 && System.Char.IsSurrogatePair(key, 0))

  [<Inline "$text.slice(0, $key.length).toUpperCase() === $key.toUpperCase()">]
  static member private startsWithCharacter (key: string) (text: string) =
    text.Length >= key.Length
    && text.Substring(0, key.Length).ToUpperInvariant() = key.ToUpperInvariant()

  /// <summary>
  /// Creates a dropdown menu component anchored to a trigger button.
  /// </summary>
  /// <param name="triggerContent">The Doc displayed inside the trigger button.</param>
  /// <param name="items">Reactive view of the menu item list. Use View.Const for static lists.</param>
  /// <param name="isOpen">Two-way binding for the open/closed state. Caller owns the Var.</param>
  /// <param name="openOn">Whether the menu opens on Click or Hover. Defaults to Click.</param>
  /// <param name="enabled">Whether the menu can open. Disabling it closes it and resets isOpen. Defaults to true.</param>
  /// <param name="closeOnOutsideClick">Whether clicking outside closes the menu. Defaults to true.</param>
  /// <param name="triggerAttrs">Attributes applied to the trigger button. For an icon-only trigger, supply an accessible name with aria-label or aria-labelledby.</param>
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
    let visible = (enabled, openVar.View) ||> View.map2Cached (&&)
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

    let findActionIndex matches (container: Dom.Element) (fromIndex: int) (direction: int) =
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
            | DropdownMenuItemKind.Action def when matches def ->
              let itemEl = container.QuerySelector(sprintf "#%s-item-%d" dropdownMenuId candidate)

              if not (isNull itemEl) && not (itemEl.ClassList.Contains(disabledClass)) then
                Some candidate
              else
                search candidate (steps + 1)
            | DropdownMenuItemKind.Action _
            | DropdownMenuItemKind.Divider -> search candidate (steps + 1)

        search fromIndex 0

    let findNextActionIndex = findActionIndex (fun _ -> true)

    let focusItemAt (index: int) =
      let container = rootRef.Value
      let itemEl = container.QuerySelector(sprintf "#%s-item-%d" dropdownMenuId index)

      if not (isNull itemEl) then
        Var.Set highlightedIndex index
        itemEl?focus ()

    let close () =
      Var.Set openVar false
      Var.Set highlightedIndex -1

    let closeAndRefocusTrigger () =
      close ()
      triggerRef.Value?focus()

    /// Handle keyboard events on the trigger button.
    /// Enter/Space are not handled here: the native button click already invokes
    /// onClick, which toggles the menu. Handling them here would
    /// cause a double-toggle.
    let handleTriggerKeyDown (ev: Dom.KeyboardEvent) =
      match ev with
      | Key.ArrowDown ->
        ev.PreventDefault()

        if not openVar.Value then
          Var.Set openVar true

        Var.Set highlightedIndex -1

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
          Var.Set openVar true

        Var.Set highlightedIndex -1

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
          close ()
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

      | Key.Tab -> close ()
      | _ when not (ev.CtrlKey || ev.MetaKey || ev.AltKey || ev?isComposing) ->
        let key = ev.Key

        if not (System.String.IsNullOrEmpty key) && DropdownMenu.isSingleCharacter key then
          let active = container.QuerySelector("[role='menuitem']:focus-within")
          let focusedId = if isNull active then "" else active.Id

          let fromIndex =
            seq { 0 .. count - 1 }
            |> Seq.tryFind (fun index -> focusedId = sprintf "%s-item-%d" dropdownMenuId index)
            |> Option.defaultValue -1

          let matches def =
            not (System.String.IsNullOrEmpty def.Text)
            && DropdownMenu.startsWithCharacter key def.Text

          match findActionIndex matches container fromIndex 1 with
          | Some index ->
            ev.PreventDefault()
            focusItemAt index
          | None -> ()
      | _ -> ()

    let triggerButton =
      let contents = [ triggerContent; Render.chevron visible ] |> Doc.Concat

      let clickHandler =
        match openOn with
        | OpenOn.Click ->
          fun () ->
            Var.Set openVar (not openVar.Value)

            if openVar.Value then
              Var.Set highlightedIndex -1
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
            (visible |> View.Map(sprintf "%b"))
          on.afterRender (fun el -> triggerRef.Value <- el)
          on.keyDownView enabled (fun _ ev canOpen ->
            if canOpen then
              handleTriggerKeyDown ev)
          yield! triggerAttrs
        ]
      )

    let popover =
      visible
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          div [
            cl Css.``weave-dropdownmenu__popover``
            on.afterRender (fun el -> popoverRef.Value <- el)
            on.click (fun _ ev -> ev?stopPropagation ())
          ] [
            div [
              cl Css.``weave-dropdownmenu__list``
              Attr.Create "id" menuId
              Attr.Create "role" "menu"
              on.keyDownView visible (fun _ ev isVisible ->
                if isVisible then
                  handleMenuKeyDown ev)
            ] [
              itemsTracked
              |> Doc.BindView(fun currentItems ->
                currentItems
                |> List.mapi (fun i kind ->
                  Render.renderItem dropdownMenuId i kind highlightedIndex openVar visible)
                |> Doc.Concat)
            ]
          ]
        else
          Doc.Empty)

    let outsideClickWatcher =
      if closeOnOutsideClick then
        visible |> DocumentEventListener.onMouseDown [ rootRef; popoverRef ] close
      else
        Doc.Empty

    let hoverAttrs =
      match openOn with
      | OpenOn.Hover -> [
          on.mouseEnterView enabled (fun _ _ canOpen ->
            if canOpen && not openVar.Value then
              Var.Set openVar true
              Var.Set highlightedIndex -1)
          on.mouseLeave (fun _ _ ->
            if openVar.Value then
              close ())
        ]
      | OpenOn.Click -> []

    [
      View.zip enabled openVar.View
      |> Doc.sinkCached (fun (canOpen, requested) ->
        if not canOpen && requested then
          close ())

      outsideClickWatcher

      div [
        cl Css.``weave-dropdownmenu``
        Attr.DynamicClassPred Css.``weave-dropdownmenu--open`` visible
        Disabled.disabledClass Css.``weave-dropdownmenu--disabled`` enabled
        on.afterRender (fun el -> rootRef.Value <- el)
        yield! hoverAttrs
        yield! attrs
      ] [ triggerButton; popover ]
    ]
    |> Doc.Concat
