namespace Weave

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Select =

  module Variant =

    let standard = cl Css.``weave-field--standard``
    let filled = cl Css.``weave-field--filled``
    let outlined = cl Css.``weave-field--outlined``

  /// Use with the outlined variant for a persistent label above the control.
  module Label =

    let above = cl Css.``weave-field--label-above``

  module Color =

    let primary = cl Css.``weave-select--primary``
    let secondary = cl Css.``weave-select--secondary``
    let tertiary = cl Css.``weave-select--tertiary``
    let error = cl Css.``weave-select--error``
    let warning = cl Css.``weave-select--warning``
    let success = cl Css.``weave-select--success``
    let info = cl Css.``weave-select--info``

  module Width =

    let full = cl Css.``weave-select--full-width``
    let dynamic = cl Css.``weave-select--dynamic``

  module PopoverWidth =

    let unbound = cl Css.``weave-select--popover-unbound``

  module AnchorOrigin =

    let topLeft = cl Css.``weave-select--anchor-origin-top-left``
    let topCenter = cl Css.``weave-select--anchor-origin-top-center``
    let topRight = cl Css.``weave-select--anchor-origin-top-right``
    let centerLeft = cl Css.``weave-select--anchor-origin-center-left``
    let center = cl Css.``weave-select--anchor-origin-center-center``
    let centerRight = cl Css.``weave-select--anchor-origin-center-right``
    let bottomLeft = cl Css.``weave-select--anchor-origin-bottom-left``
    let bottomCenter = cl Css.``weave-select--anchor-origin-bottom-center``
    let bottomRight = cl Css.``weave-select--anchor-origin-bottom-right``

  module TransformOrigin =

    let topLeft = cl Css.``weave-select--transform-origin-top-left``
    let topCenter = cl Css.``weave-select--transform-origin-top-center``
    let topRight = cl Css.``weave-select--transform-origin-top-right``
    let centerLeft = cl Css.``weave-select--transform-origin-center-left``
    let center = cl Css.``weave-select--transform-origin-center-center``
    let centerRight = cl Css.``weave-select--transform-origin-center-right``
    let bottomLeft = cl Css.``weave-select--transform-origin-bottom-left``
    let bottomCenter = cl Css.``weave-select--transform-origin-bottom-center``
    let bottomRight = cl Css.``weave-select--transform-origin-bottom-right``

  type SelectItemDef<'T when 'T: comparison> = {
    Content: Doc
    Value: 'T
    Text: string
    SelectedContent: Doc option
    Disabled: View<bool>
    Attrs: Attr list
  }

  module internal Keyboard =

    // Keep each option and its disabled state together when the view updates.
    let withDisabledStates (items: View<SelectItemDef<'T> list>) =
      items
      |> View.Bind(fun defs ->
        (View.Const [], List.rev defs)
        ||> List.fold (fun acc def ->
          View.Map2 (fun disabled rest -> (def, disabled) :: rest) def.Disabled acc))

    let resetOnItemChanges
      (items: View<SelectItemDef<'T> list>)
      (highlightedIndex: Var<int>)
      (previous: 'T list option ref)
      =
      items
      |> View.Map(fun defs -> defs |> List.map (fun def -> def.Value))
      |> Doc.sinkCached (fun values ->
        match previous.Value with
        | Some old when old <> values -> Var.Set highlightedIndex -1
        | _ -> ()

        previous.Value <- Some values)

    let move (items: (SelectItemDef<'T> * bool) list) current step =
      let indexed = items |> List.mapi (fun index (_, disabled) -> index, disabled)
      let candidates = if step > 0 then indexed else List.rev indexed

      candidates
      |> List.tryFind (fun (index, disabled) ->
        not disabled && if step > 0 then index > current else index < current)
      |> Option.map fst

    let filteredItems searchable (searchText: Var<string>) (items: View<SelectItemDef<'T> list>) =
      if searchable then
        (searchText.View, items)
        ||> View.Map2(fun query defs ->
          if String.IsNullOrWhiteSpace query then
            defs
          else
            let q = query.ToLower()
            defs |> List.filter (fun def -> def.Text.ToLower().Contains(q)))
      else
        items

    let activeDescendant selectId (highlightedIndex: Var<int>) (isOpen: Var<bool>) =
      (highlightedIndex.View, isOpen.View)
      ||> View.Map2(fun idx opened ->
        if opened && idx >= 0 then
          sprintf "%s-item-%d" selectId idx
        else
          "")

    let handleKey
      (isOpen: Var<bool>)
      (searchText: Var<string>)
      (highlightedIndex: Var<int>)
      (select: SelectItemDef<'T> -> unit)
      (items: (SelectItemDef<'T> * bool) list)
      (allItems: (SelectItemDef<'T> * bool) list)
      (ev: Dom.KeyboardEvent)
      =
      match ev with
      | Key.Enter ->
        if not isOpen.Value then
          Var.Set isOpen true
          Var.Set highlightedIndex -1
          Var.Set searchText ""
        else
          let idx = highlightedIndex.Value

          if idx >= 0 && idx < items.Length then
            let item, disabled = items.[idx]

            if not disabled then
              select item
      | Key.Escape
      | Key.Tab ->
        if isOpen.Value then
          Var.Set isOpen false
      | Key.ArrowDown ->
        if not isOpen.Value then
          Var.Set isOpen true
          Var.Set searchText ""
          Var.Set highlightedIndex (move allItems -1 1 |> Option.defaultValue -1)
        else
          move items highlightedIndex.Value 1 |> Option.iter (Var.Set highlightedIndex)
      | Key.ArrowUp ->
        if isOpen.Value then
          move items highlightedIndex.Value -1 |> Option.iter (Var.Set highlightedIndex)
      | _ -> ()

    let itemOrderIsCurrent (items: View<SelectItemDef<'T> list>) (previous: 'T list option ref) =
      match View.TryGet items, previous.Value with
      | Some current, Some old -> (current |> List.map (fun item -> item.Value)) = old
      | _ -> false

  module Render =

    let chevron () =
      span [ cl Css.``weave-select__chevron`` ] [ text "\u25BC" ]

    let clearButtonGuarded (interactable: View<bool>) (onClear: unit -> unit) =
      span [
        cl Css.``weave-select__clear``
        Attr.Create "tabindex" "0"
        Attr.Create "role" "button"
        Attr.Create "aria-label" "Clear selection"
        on.clickView interactable (fun _ ev canInteract ->
          ev?stopPropagation ()

          if canInteract then
            onClear ())
        on.keyDownView interactable (fun _ ev canInteract ->
          match ev with
          | Key.Activate when canInteract ->
            ev.PreventDefault()
            ev?stopPropagation ()
            onClear ()
          | _ -> ())
      ] [ text "\u00D7" ]

    let clearButton onClear =
      clearButtonGuarded (View.Const true) onClear

    let internal searchInputWithState
      (state: View<'T>)
      (canInteract: 'T -> bool)
      (tabToSelectAll: bool)
      (searchText: Var<string>)
      (highlightedIndex: Var<int>)
      (onKeyDown: 'T -> Dom.KeyboardEvent -> unit)
      =
      div [ cl Css.``weave-select__search`` ] [
        Doc.InputType.Text
          [
            cl Css.``weave-select__search-input``
            Attr.Create "placeholder" "Search..."
            Attr.Create "aria-label" "Search options"
            on.afterRender (fun el -> el?focus ())
            on.input (fun _ _ -> Var.Set highlightedIndex -1)
            on.keyDownView state (fun _ ev current ->
              match ev with
              | Key.ArrowDown
              | Key.ArrowUp
              | Key.Enter when canInteract current ->
                ev.PreventDefault()
                onKeyDown current ev
              | Key.Escape ->
                ev.PreventDefault()
                onKeyDown current ev
              | Key.Tab when tabToSelectAll && not ev.ShiftKey && canInteract current -> ()
              | Key.Tab -> onKeyDown current ev
              | _ -> ())
          ]
          searchText
      ]

    let searchInputGuarded interactable searchText highlightedIndex onKeyDown =
      searchInputWithState interactable id false searchText highlightedIndex (fun _ ev -> onKeyDown ev)

    let searchInput searchText highlightedIndex onKeyDown =
      searchInputGuarded (View.Const true) searchText highlightedIndex onKeyDown

    let singleItem
      (selectId: string)
      (index: int)
      (def: SelectItemDef<'T>)
      (selectedValue: Var<'T option>)
      (isOpen: Var<bool>)
      (highlightedIndex: Var<int>)
      (enabled: View<bool>)
      =
      let isSelected =
        selectedValue.View |> View.MapCached(fun sel -> sel = Some def.Value)

      let itemEnabled =
        (enabled, def.Disabled) ||> View.map2Cached (fun e d -> e && not d)

      let itemId = sprintf "%s-item-%d" selectId index

      let isHighlighted = highlightedIndex.View |> View.MapCached(fun hi -> hi = index)

      div [
        cl Css.``weave-select__item``
        Attr.DynamicClassPred Css.``weave-select__item--selected`` isSelected
        Attr.DynamicClassPred Css.``weave-select__item--highlighted`` isHighlighted
        Attr.DynamicClassPred Css.``weave-select__item--disabled`` def.Disabled
        Attr.Create "id" itemId
        Attr.Create "role" "option"
        Attr.DynamicCustom
          (fun el value -> el.SetAttribute("aria-selected", value))
          (isSelected |> View.Map(sprintf "%b"))
        Attr.DynamicCustom
          (fun el value -> el.SetAttribute("aria-disabled", value))
          (def.Disabled |> View.Map(sprintf "%b"))
        on.clickViewGuarded itemEnabled (fun () ->
          Var.Set selectedValue (Some def.Value)
          Var.Set isOpen false)
        on.mouseEnter (fun _ _ -> Var.Set highlightedIndex index)
        yield! def.Attrs
      ] [ span [ cl Css.``weave-select__item-content`` ] [ def.Content ] ]

    let multiItem
      (selectId: string)
      (index: int)
      (def: SelectItemDef<'T>)
      (selectedValues: Var<Set<'T>>)
      (highlightedIndex: Var<int>)
      (enabled: View<bool>)
      =
      let isSelected =
        selectedValues.View |> View.MapCached(fun sel -> Set.contains def.Value sel)

      let itemEnabled =
        (enabled, def.Disabled) ||> View.map2Cached (fun e d -> e && not d)

      let toggle () =
        let current = selectedValues.Value

        if Set.contains def.Value current then
          Var.Set selectedValues (Set.remove def.Value current)
        else
          Var.Set selectedValues (Set.add def.Value current)

      let itemId = sprintf "%s-item-%d" selectId index

      let isHighlighted = highlightedIndex.View |> View.MapCached(fun hi -> hi = index)

      div [
        cl Css.``weave-select__item``
        Attr.DynamicClassPred Css.``weave-select__item--selected`` isSelected
        Attr.DynamicClassPred Css.``weave-select__item--highlighted`` isHighlighted
        Attr.DynamicClassPred Css.``weave-select__item--disabled`` def.Disabled
        Attr.Create "id" itemId
        Attr.Create "role" "option"
        Attr.DynamicCustom
          (fun el value -> el.SetAttribute("aria-selected", value))
          (isSelected |> View.Map(sprintf "%b"))
        Attr.DynamicCustom
          (fun el value -> el.SetAttribute("aria-disabled", value))
          (def.Disabled |> View.Map(sprintf "%b"))
        on.clickViewGuarded itemEnabled toggle
        on.mouseEnter (fun _ _ -> Var.Set highlightedIndex index)
        yield! def.Attrs
      ] [
        span [
          cl Css.``weave-select__item-checkbox``
          Attr.DynamicClassPred Css.``weave-select__item-checkbox--checked`` isSelected
        ] []
        span [ cl Css.``weave-select__item-content`` ] [ def.Content ]
      ]

    let internal selectAllRowWithAttrs
      (items: View<SelectItemDef<'T> list>)
      (selectedValues: Var<Set<'T>>)
      (enabled: View<bool>)
      (selectAllText: string)
      (attrs: Attr list)
      =
      let enabledValues =
        items
        |> Keyboard.withDisabledStates
        |> View.Map(fun entries ->
          entries
          |> List.choose (fun (item, disabled) -> if disabled then None else Some item.Value))

      let allSelected =
        (enabledValues, selectedValues.View)
        ||> View.Map2(fun values sel ->
          not (List.isEmpty values) && values |> List.forall (fun v -> Set.contains v sel))

      let isIndeterminate =
        (enabledValues, selectedValues.View)
        ||> View.Map2(fun values sel ->
          let anySelected = values |> List.exists (fun v -> Set.contains v sel)
          let allSel = values |> List.forall (fun v -> Set.contains v sel)
          anySelected && not allSel)

      let toggle currentItems =
        let current = selectedValues.Value
        let allSel = currentItems |> List.forall (fun v -> Set.contains v current)

        if allSel then
          Var.Set selectedValues (currentItems |> List.fold (fun acc v -> Set.remove v acc) current)
        else
          Var.Set selectedValues (currentItems |> List.fold (fun acc v -> Set.add v acc) current)

      button [
        attr.``type`` "button"
        cl Css.``weave-select__select-all``
        Attr.Create "role" "checkbox"
        Attr.enabled enabled
        Attr.DynamicCustom
          (fun el value -> el.SetAttribute("aria-checked", value))
          (View.Map2
            (fun all partial ->
              if partial then "mixed"
              elif all then "true"
              else "false")
            allSelected
            isIndeterminate)
        on.clickView enabled (fun _ _ canInteract ->
          match canInteract, View.TryGet enabled, View.TryGet items with
          | true, Some true, Some currentItems ->
            currentItems
            |> List.choose (fun item ->
              match View.TryGet item.Disabled with
              | Some false -> Some item.Value
              | _ -> None)
            |> toggle
          | _ -> ())
        yield! attrs
      ] [
        span [
          cl Css.``weave-select__item-checkbox``
          Attr.DynamicClassPred Css.``weave-select__item-checkbox--checked`` allSelected
          Attr.DynamicClassPred Css.``weave-select__item-checkbox--indeterminate`` isIndeterminate
        ] []
        span [ cl Css.``weave-select__item-content`` ] [ text selectAllText ]
      ]

    let selectAllRow items selectedValues enabled selectAllText =
      selectAllRowWithAttrs items selectedValues enabled selectAllText []

    let sizer (items: View<SelectItemDef<'T> list>) =
      items
      |> Doc.BindView(fun defs ->
        div [ cl Css.``weave-select__sizer``; Attr.Create "aria-hidden" "true" ] [
          for def in defs do
            div [ cl Css.``weave-select__sizer-item`` ] [ text def.Text ]
        ])

open Select

[<JavaScript>]
type SelectItem =

  static member create<'T when 'T: comparison>
    (content: Doc, text: string, value: 'T, ?selectedContent: Doc, ?disabled: View<bool>, ?attrs: Attr list)
    : Select.SelectItemDef<'T> = {
    Content = content
    Value = value
    Text = text
    SelectedContent = selectedContent
    Disabled = defaultArg disabled (View.Const false)
    Attrs = defaultArg attrs []
  }

[<JavaScript>]
type Select =

  static member create<'T when 'T: comparison>
    (
      items: View<SelectItemDef<'T> list>,
      selectedValue: Var<'T option>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?clearable: View<bool>,
      ?searchable: bool,
      ?isOpen: Var<bool>,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?noItemsContent: Doc,
      ?attrs: Attr list
    ) =

    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let clearable = defaultArg clearable (View.Const false)
    let searchable = defaultArg searchable false
    let openVar = defaultArg isOpen (Var.Create false)
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let noItemsContent = defaultArg noItemsContent (text "No items found")
    let attrs = defaultArg attrs List.empty

    let interactable =
      (enabled, readOnly) ||> View.map2Cached (fun en ro -> en && not ro)

    let isFocused = Var.Create false
    let searchText = Var.Create ""
    let highlightedIndex = Var.Create -1

    // Selected display: look up value in current items, show nothing if stale
    let selectedDisplay =
      (selectedValue.View, items)
      ||> View.Map2(fun sel defs ->
        match sel with
        | Some v ->
          match defs |> List.tryFind (fun d -> d.Value = v) with
          | Some def ->
            match def.SelectedContent with
            | Some c -> c
            | None -> text def.Text
          | None -> Doc.Empty
        | None -> Doc.Empty)

    let hasValue = selectedValue.View |> View.MapCached Option.isSome

    let hasExplicitPlaceholder =
      placeholder |> View.MapCached(fun p -> not (String.IsNullOrEmpty p))

    let shouldFloat = openVar.View <||> hasValue <||> hasExplicitPlaceholder

    let effectivePlaceholder =
      (placeholder, hasValue)
      ||> View.Map2(fun ph hv ->
        if hv then ""
        elif String.IsNullOrEmpty ph then ""
        else ph)

    let filteredItems = Keyboard.filteredItems searchable searchText items

    let filteredOptions = Keyboard.withDisabledStates filteredItems
    let allOptions = Keyboard.withDisabledStates items

    let keyboardState =
      View.Map2 (fun filtered all -> filtered, all) filteredOptions allOptions
      |> View.Map2 (fun canInteract (filtered, all) -> canInteract, filtered, all) interactable

    let filteredDisabledStateSink =
      filteredOptions
      |> View.Map(fun options -> options |> List.exists (fun (_, disabled) -> not disabled))
      |> Doc.sinkCached (fun hasEnabled ->
        if not hasEnabled then
          Var.Set highlightedIndex -1)

    let previousItemOrder = ref None

    let itemChangesSink =
      Keyboard.resetOnItemChanges items highlightedIndex previousItemOrder

    // Element refs for outside-click detection
    let selectRootRef = ref (JS.Document.CreateElement "div")
    let popoverRef = ref (JS.Document.CreateElement "div")

    let selectId = WeaveId.create "weave-select"

    let activeDescendantView =
      Keyboard.activeDescendant selectId highlightedIndex openVar

    let handleKeyNav (_, filtered, all) ev =
      let canSelect =
        match ev with
        | Key.Enter when openVar.Value -> Keyboard.itemOrderIsCurrent items previousItemOrder
        | _ -> true

      if canSelect then
        Keyboard.handleKey
          openVar
          searchText
          highlightedIndex
          (fun def ->
            Var.Set selectedValue (Some def.Value)
            Var.Set openVar false)
          filtered
          all
          ev

    let inputElement =
      div [
        cls [ Css.``weave-field__input``; Css.``weave-select__value`` ]
        Attr.Create "tabindex" "0"
        Attr.Create "role" "combobox"
        Attr.Create "aria-haspopup" "listbox"
        Attr.DynamicCustom
          (fun el v -> el.SetAttribute("aria-expanded", v))
          (openVar.View |> View.Map(sprintf "%b"))
        Attr.DynamicCustom (fun el v -> el.SetAttribute("aria-activedescendant", v)) activeDescendantView
        Attr.DynamicCustom
          (fun el v ->
            if not (String.IsNullOrEmpty v) then
              el.SetAttribute("aria-label", v))
          labelText

        on.focus (fun _ _ -> Var.Set isFocused true)
        on.blur (fun _ _ -> Var.Set isFocused false)

        on.keyDownView keyboardState (fun _ ev ((canInteract, _, _) as state) ->
          match ev with
          | Key.Enter
          | Key.Space
          | Key.ArrowDown
          | Key.ArrowUp when canInteract ->
            ev.PreventDefault()
            handleKeyNav state ev
          | Key.Escape ->
            ev.PreventDefault()
            handleKeyNav state ev
          | Key.Tab -> handleKeyNav state ev
          | _ -> ())
      ] [
        Doc.EmbedView selectedDisplay
        span [ cl Css.``weave-select__placeholder`` ] [ Doc.TextView effectivePlaceholder ]
      ]

    let endAdornment =
      div [ cl Css.``weave-select__actions`` ] [
        Render.clearButtonGuarded interactable (fun () -> Var.Set selectedValue None)
        Render.chevron ()
      ]

    let popover =
      openVar.View
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          div [
            cl Css.``weave-select__popover``
            on.afterRender (fun el -> popoverRef.Value <- el)
            on.click (fun _ ev -> ev?stopPropagation ())
          ] [
            if searchable then
              Render.searchInputWithState
                keyboardState
                (fun (canInteract, _, _) -> canInteract)
                false
                searchText
                highlightedIndex
                handleKeyNav

            div [ cl Css.``weave-select__list``; Attr.Create "role" "listbox" ] [
              filteredItems
              |> Doc.BindView(fun filtered ->
                if List.isEmpty filtered then
                  div [ cl Css.``weave-select__no-items`` ] [ noItemsContent ]
                else
                  filtered
                  |> List.mapi (fun i def ->
                    Render.singleItem selectId i def selectedValue openVar highlightedIndex interactable)
                  |> Doc.Concat)
            ]
          ]
        else
          Doc.Empty)

    let outsideClickWatcher =
      openVar.View
      |> DocumentEventListener.onMouseDown [ selectRootRef; popoverRef ] (fun () -> Var.Set openVar false)

    [
      outsideClickWatcher
      filteredDisabledStateSink
      itemChangesSink

      div [
        cl Css.``weave-select``
        Disabled.disabledClass Css.``weave-select--disabled`` enabled
        Attr.DynamicClassPred Css.``weave-select--open`` openVar.View
        Attr.DynamicClassPred Css.``weave-select--focused`` (isFocused.View <||> openVar.View)
        Attr.DynamicClassPred Css.``weave-select--readonly`` readOnly
        Attr.DynamicClassPred Css.``weave-select--has-value`` hasValue
        Attr.DynamicClassPred Css.``weave-select--clearable`` clearable
        on.clickViewGuarded interactable (fun () ->
          Var.Set openVar (not openVar.Value)

          if openVar.Value then
            Var.Set highlightedIndex -1
            Var.Set searchText "")
        on.afterRender (fun el -> selectRootRef.Value <- el)
        yield! attrs
      ] [
        Render.sizer items
        Field.create (
          inputElement,
          isFocused.View <||> openVar.View,
          shouldFloat,
          labelText = labelText,
          ?showHelpText = showHelpText,
          ?helpText = helpText,
          enabled = enabled,
          endAdornment = endAdornment,
          attrs = attrs
        )
        popover
      ]
    ]
    |> Doc.Concat

[<JavaScript>]
type MultiSelect =

  static member create<'T when 'T: comparison>
    (
      items: View<SelectItemDef<'T> list>,
      selectedValues: Var<Set<'T>>,
      ?labelText: View<string>,
      ?placeholder: View<string>,
      ?showHelpText: View<bool>,
      ?helpText: Doc,
      ?selectionText: Set<'T> -> string,
      ?clearable: View<bool>,
      ?searchable: bool,
      ?showSelectAll: bool,
      ?selectAllText: string,
      ?isOpen: Var<bool>,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?noItemsContent: Doc,
      ?attrs: Attr list
    ) =

    let labelText = defaultArg labelText (View.Const "")
    let placeholder = defaultArg placeholder (View.Const "")
    let clearable = defaultArg clearable (View.Const false)
    let searchable = defaultArg searchable false
    let showSelectAll = defaultArg showSelectAll false
    let selectAllText = defaultArg selectAllText "Select All"
    let openVar = defaultArg isOpen (Var.Create false)
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let noItemsContent = defaultArg noItemsContent (text "No items found")
    let attrs = defaultArg attrs List.empty

    let interactable =
      (enabled, readOnly) ||> View.map2Cached (fun en ro -> en && not ro)

    let isFocused = Var.Create false
    let searchText = Var.Create ""
    let highlightedIndex = Var.Create -1

    let defaultSelectionText (sel: Set<'T>) (defs: SelectItemDef<'T> list) =
      if Set.isEmpty sel then
        ""
      else
        defs
        |> List.filter (fun d -> Set.contains d.Value sel)
        |> List.map (fun d -> d.Text)
        |> String.concat ", "

    let selectedDisplay =
      (selectedValues.View, items)
      ||> View.Map2(fun sel defs ->
        if Set.isEmpty sel then
          Doc.Empty
        else
          match selectionText with
          | Some f -> text (f sel)
          | None -> text (defaultSelectionText sel defs))

    let hasValue =
      selectedValues.View |> View.MapCached(fun sel -> not (Set.isEmpty sel))

    let hasExplicitPlaceholder =
      placeholder |> View.MapCached(fun p -> not (String.IsNullOrEmpty p))

    let shouldFloat = openVar.View <||> hasValue <||> hasExplicitPlaceholder

    let effectivePlaceholder =
      (placeholder, hasValue)
      ||> View.Map2(fun ph hv ->
        if hv then ""
        elif String.IsNullOrEmpty ph then ""
        else ph)

    let filteredItems = Keyboard.filteredItems searchable searchText items

    let filteredOptions = Keyboard.withDisabledStates filteredItems
    let allOptions = Keyboard.withDisabledStates items

    let keyboardState =
      View.Map2 (fun filtered all -> filtered, all) filteredOptions allOptions
      |> View.Map2 (fun canInteract (filtered, all) -> canInteract, filtered, all) interactable

    let filteredDisabledStateSink =
      filteredOptions
      |> View.Map(fun options -> options |> List.exists (fun (_, disabled) -> not disabled))
      |> Doc.sinkCached (fun hasEnabled ->
        if not hasEnabled then
          Var.Set highlightedIndex -1)

    let previousItemOrder = ref None

    let itemChangesSink =
      Keyboard.resetOnItemChanges items highlightedIndex previousItemOrder

    let selectRootRef = ref (JS.Document.CreateElement "div")
    let popoverRef = ref (JS.Document.CreateElement "div")
    let inputRef = ref (JS.Document.CreateElement "div")
    let selectAllRef = ref (JS.Document.CreateElement "button")

    let selectId = WeaveId.create "weave-select"

    let activeDescendantView =
      Keyboard.activeDescendant selectId highlightedIndex openVar

    let handleKeyNav (_, filtered, all) ev =
      let canSelect =
        match ev with
        | Key.Enter when openVar.Value -> Keyboard.itemOrderIsCurrent items previousItemOrder
        | _ -> true

      if canSelect then
        Keyboard.handleKey
          openVar
          searchText
          highlightedIndex
          (fun def ->
            let current = selectedValues.Value

            if Set.contains def.Value current then
              Var.Set selectedValues (Set.remove def.Value current)
            else
              Var.Set selectedValues (Set.add def.Value current))
          filtered
          all
          ev

    let handleMultiKeyNav ((canInteract, filtered, _) as state) ev =
      let isBeforeFirstOption =
        highlightedIndex.Value < 0
        || Keyboard.move filtered -1 1 = Some highlightedIndex.Value

      match ev with
      | Key.ArrowDown when showSelectAll && canInteract && openVar.Value && highlightedIndex.Value < 0 ->
        selectAllRef.Value?focus()
      | Key.ArrowUp when showSelectAll && canInteract && openVar.Value && isBeforeFirstOption ->
        Var.Set highlightedIndex -1
        selectAllRef.Value?focus()
      | _ -> handleKeyNav state ev

    let inputElement =
      div [
        cls [ Css.``weave-field__input``; Css.``weave-select__value`` ]
        Attr.Create "tabindex" "0"
        Attr.Create "role" "combobox"
        Attr.Create "aria-haspopup" "listbox"
        on.afterRender (fun el -> inputRef.Value <- el)
        Attr.DynamicCustom
          (fun el v -> el.SetAttribute("aria-expanded", v))
          (openVar.View |> View.Map(sprintf "%b"))
        Attr.DynamicCustom (fun el v -> el.SetAttribute("aria-activedescendant", v)) activeDescendantView
        Attr.DynamicCustom
          (fun el v ->
            if not (String.IsNullOrEmpty v) then
              el.SetAttribute("aria-label", v))
          labelText

        on.focus (fun _ _ -> Var.Set isFocused true)
        on.blur (fun _ _ -> Var.Set isFocused false)

        on.keyDownView keyboardState (fun _ ev ((canInteract, _, _) as state) ->
          match ev with
          | Key.Enter
          | Key.Space
          | Key.ArrowDown
          | Key.ArrowUp when canInteract ->
            ev.PreventDefault()
            handleMultiKeyNav state ev
          | Key.Escape ->
            ev.PreventDefault()
            handleKeyNav state ev
          | Key.Tab when showSelectAll && openVar.Value && canInteract && not ev.ShiftKey ->
            ev.PreventDefault()
            selectAllRef.Value?focus()
          | Key.Tab -> handleKeyNav state ev
          | _ -> ())
      ] [
        Doc.EmbedView selectedDisplay
        span [ cl Css.``weave-select__placeholder`` ] [ Doc.TextView effectivePlaceholder ]
      ]

    let endAdornment =
      div [ cl Css.``weave-select__actions`` ] [
        Render.clearButtonGuarded interactable (fun () -> Var.Set selectedValues Set.empty)
        Render.chevron ()
      ]

    let popover =
      openVar.View
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          div [
            cl Css.``weave-select__popover``
            on.afterRender (fun el -> popoverRef.Value <- el)
            on.click (fun _ ev -> ev?stopPropagation ())
          ] [
            if searchable then
              Render.searchInputWithState
                keyboardState
                (fun (canInteract, _, _) -> canInteract)
                showSelectAll
                searchText
                highlightedIndex
                handleMultiKeyNav

            if showSelectAll then
              Render.selectAllRowWithAttrs filteredItems selectedValues interactable selectAllText [
                on.afterRender (fun el -> selectAllRef.Value <- el)
                on.keyDownView keyboardState (fun _ ev (canInteract, filtered, _) ->
                  match ev with
                  | Key.ArrowDown when canInteract ->
                    ev.PreventDefault()
                    let search = popoverRef.Value.QuerySelector(".weave-select__search-input")

                    if isNull search then
                      inputRef.Value?focus()
                    else
                      search?focus ()

                    Var.Set highlightedIndex (Keyboard.move filtered -1 1 |> Option.defaultValue -1)
                  | Key.ArrowUp when canInteract ->
                    ev.PreventDefault()
                    let search = popoverRef.Value.QuerySelector(".weave-select__search-input")

                    if isNull search then
                      inputRef.Value?focus()
                    else
                      search?focus ()
                  | Key.Escape ->
                    ev.PreventDefault()
                    Var.Set openVar false
                    inputRef.Value?focus()
                  | Key.Tab when not ev.ShiftKey -> Var.Set openVar false
                  | _ -> ())
              ]

              div [ cl Css.``weave-select__divider`` ] []

            div [
              cl Css.``weave-select__list``
              Attr.Create "role" "listbox"
              Attr.Create "aria-multiselectable" "true"
            ] [
              filteredItems
              |> Doc.BindView(fun filtered ->
                if List.isEmpty filtered then
                  div [ cl Css.``weave-select__no-items`` ] [ noItemsContent ]
                else
                  filtered
                  |> List.mapi (fun i def ->
                    Render.multiItem selectId i def selectedValues highlightedIndex interactable)
                  |> Doc.Concat)
            ]
          ]
        else
          Doc.Empty)

    let outsideClickWatcher =
      openVar.View
      |> DocumentEventListener.onMouseDown [ selectRootRef; popoverRef ] (fun () -> Var.Set openVar false)

    [
      outsideClickWatcher
      filteredDisabledStateSink
      itemChangesSink

      div [
        cl Css.``weave-select``
        cl Css.``weave-select--multi``
        Disabled.disabledClass Css.``weave-select--disabled`` enabled
        Attr.DynamicClassPred Css.``weave-select--open`` openVar.View
        Attr.DynamicClassPred Css.``weave-select--focused`` (isFocused.View <||> openVar.View)
        Attr.DynamicClassPred Css.``weave-select--readonly`` readOnly
        Attr.DynamicClassPred Css.``weave-select--has-value`` hasValue
        Attr.DynamicClassPred Css.``weave-select--clearable`` clearable
        on.clickViewGuarded interactable (fun () ->
          Var.Set openVar (not openVar.Value)

          if openVar.Value then
            Var.Set highlightedIndex -1
            Var.Set searchText "")
        on.afterRender (fun el -> selectRootRef.Value <- el)
        yield! attrs
      ] [
        Render.sizer items
        Field.create (
          inputElement,
          isFocused.View <||> openVar.View,
          shouldFloat,
          labelText = labelText,
          ?showHelpText = showHelpText,
          ?helpText = helpText,
          enabled = enabled,
          endAdornment = endAdornment,
          attrs = attrs
        )
        popover
      ]
    ]
    |> Doc.Concat
