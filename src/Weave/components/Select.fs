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

  type Variant = Field.Variant

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
    let fitContent = cl Css.``weave-select--fit-content``

  type SelectItemDef<'T when 'T: comparison> = {
    Content: Doc
    Value: 'T
    Text: string
    SelectedContent: Doc option
    Disabled: View<bool>
    Attrs: Attr list
  }

  module Render =

    let chevron () =
      span [ cl Css.``weave-select__chevron`` ] [ text "\u25BC" ]

    let clearButton (onClear: unit -> unit) =
      span [
        cl Css.``weave-select__clear``
        Attr.Create "tabindex" "0"
        Attr.Create "role" "button"
        Attr.Create "aria-label" "Clear selection"
        on.clickTap (fun _ ev ->
          ev?stopPropagation ()
          onClear ())
        on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
          if ev.Key = "Enter" || ev.Key = " " then
            ev.PreventDefault()
            ev?stopPropagation ()
            onClear ())
      ] [ text "\u00D7" ]

    let searchInput (searchText: Var<string>) (onKeyDown: Dom.KeyboardEvent -> unit) =
      div [ cl Css.``weave-select__search`` ] [
        Doc.InputType.Text
          [
            cl Css.``weave-select__search-input``
            Attr.Create "placeholder" "Search..."
            Attr.Create "aria-label" "Search options"
            on.afterRender (fun el -> el?focus ())
            on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
              match ev.Key with
              | "ArrowDown"
              | "ArrowUp"
              | "Enter"
              | "Escape" ->
                ev.PreventDefault()
                onKeyDown ev
              | "Tab" -> onKeyDown ev
              | _ -> ())
          ]
          searchText
      ]

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
        Attr.DynamicProp "aria-selected" (isSelected |> View.Map(fun s -> if s then "true" else "false"))
        on.clickTapViewGuarded itemEnabled (fun () ->
          selectedValue.Value <- Some def.Value
          isOpen.Value <- false)
        on.mouseEnter (fun _ _ -> highlightedIndex.Value <- index)
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
          selectedValues.Value <- Set.remove def.Value current
        else
          selectedValues.Value <- Set.add def.Value current

      let itemId = sprintf "%s-item-%d" selectId index

      let isHighlighted = highlightedIndex.View |> View.MapCached(fun hi -> hi = index)

      div [
        cl Css.``weave-select__item``
        Attr.DynamicClassPred Css.``weave-select__item--selected`` isSelected
        Attr.DynamicClassPred Css.``weave-select__item--highlighted`` isHighlighted
        Attr.DynamicClassPred Css.``weave-select__item--disabled`` def.Disabled
        Attr.Create "id" itemId
        Attr.Create "role" "option"
        Attr.DynamicProp "aria-selected" (isSelected |> View.Map(fun s -> if s then "true" else "false"))
        on.clickTapViewGuarded itemEnabled toggle
        on.mouseEnter (fun _ _ -> highlightedIndex.Value <- index)
        yield! def.Attrs
      ] [
        span [
          cl Css.``weave-select__item-checkbox``
          Attr.DynamicClassPred Css.``weave-select__item-checkbox--checked`` isSelected
        ] []
        span [ cl Css.``weave-select__item-content`` ] [ def.Content ]
      ]

    let selectAllRow
      (items: View<SelectItemDef<'T> list>)
      (selectedValues: Var<Set<'T>>)
      (enabled: View<bool>)
      (selectAllText: string)
      =
      let itemsSnapshot = ref ([]: SelectItemDef<'T> list)

      let allSelected =
        (items, selectedValues.View)
        ||> View.Map2(fun defs sel ->
          itemsSnapshot.Value <- defs
          let enabledValues = defs |> List.map (fun d -> d.Value)

          not (List.isEmpty enabledValues)
          && enabledValues |> List.forall (fun v -> Set.contains v sel))

      let isIndeterminate =
        (items, selectedValues.View)
        ||> View.Map2(fun defs sel ->
          let enabledValues = defs |> List.map (fun d -> d.Value)
          let anySelected = enabledValues |> List.exists (fun v -> Set.contains v sel)
          let allSel = enabledValues |> List.forall (fun v -> Set.contains v sel)
          anySelected && not allSel)

      let toggle () =
        let current = selectedValues.Value
        let currentItems = itemsSnapshot.Value |> List.map (fun d -> d.Value)
        let allSel = currentItems |> List.forall (fun v -> Set.contains v current)

        if allSel then
          selectedValues.Value <- currentItems |> List.fold (fun acc v -> Set.remove v acc) current
        else
          selectedValues.Value <- currentItems |> List.fold (fun acc v -> Set.add v acc) current

      div [ cl Css.``weave-select__select-all``; on.clickTapViewGuarded enabled toggle ] [
        span [
          cl Css.``weave-select__item-checkbox``
          Attr.DynamicClassPred Css.``weave-select__item-checkbox--checked`` allSelected
          Attr.DynamicClassPred Css.``weave-select__item-checkbox--indeterminate`` isIndeterminate
        ] []
        span [ cl Css.``weave-select__item-content`` ] [ text selectAllText ]
      ]

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
    : Select.SelectItemDef<'T> =
    {
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
      ?variant: Variant,
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

    let variant = defaultArg variant Variant.Standard
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

    let filteredItems =
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

    let filteredSnapshot = ref ([]: SelectItemDef<'T> list)

    let filteredItemsTracked =
      filteredItems
      |> View.Map(fun defs ->
        filteredSnapshot.Value <- defs
        defs)

    // Element refs for outside-click detection
    let selectRootRef = ref (JS.Document.CreateElement "div")
    let popoverRef = ref (JS.Document.CreateElement "div")

    let selectId = WeaveId.create "weave-select"

    let activeDescendantView =
      highlightedIndex.View
      |> View.MapCached(fun idx -> if idx >= 0 then sprintf "%s-item-%d" selectId idx else "")

    let handleKeyNav (ev: Dom.KeyboardEvent) =
      let items = filteredSnapshot.Value
      let count = List.length items

      match ev.Key with
      | "Enter" ->
        if not openVar.Value then
          openVar.Value <- true
          highlightedIndex.Value <- -1
          searchText.Value <- ""
        else
          let idx = highlightedIndex.Value

          if idx >= 0 && idx < count then
            let def = items.[idx]
            selectedValue.Value <- Some def.Value
            openVar.Value <- false
      | "Escape" ->
        if openVar.Value then
          openVar.Value <- false
      | "ArrowDown" ->
        if not openVar.Value then
          openVar.Value <- true
          highlightedIndex.Value <- 0
          searchText.Value <- ""
        else
          let next = highlightedIndex.Value + 1

          if next < count then
            highlightedIndex.Value <- next
      | "ArrowUp" ->
        if openVar.Value && highlightedIndex.Value > 0 then
          highlightedIndex.Value <- highlightedIndex.Value - 1
      | "Tab" ->
        if openVar.Value then
          openVar.Value <- false
      | _ -> ()

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

        on.focus (fun _ _ -> isFocused.Value <- true)
        on.blur (fun _ _ -> isFocused.Value <- false)

        on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
          match ev.Key with
          | "Enter"
          | " "
          | "Escape"
          | "ArrowDown"
          | "ArrowUp" ->
            ev.PreventDefault()
            handleKeyNav ev
          | "Tab" -> handleKeyNav ev
          | _ -> ())
      ] [
        Doc.EmbedView selectedDisplay
        span [ cl Css.``weave-select__placeholder`` ] [ Doc.TextView effectivePlaceholder ]
      ]

    let endAdornment =
      div [ cl Css.``weave-select__actions`` ] [
        Render.clearButton (fun () -> selectedValue.Value <- None)
        Render.chevron ()
      ]

    let popover =
      openVar.View
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          div [
            cl Css.``weave-select__popover``
            on.afterRender (fun el -> popoverRef.Value <- el)
            on.clickTap (fun _ ev -> ev?stopPropagation ())
          ] [
            if searchable then
              Render.searchInput searchText handleKeyNav

            div [ cl Css.``weave-select__list``; Attr.Create "role" "listbox" ] [
              filteredItemsTracked
              |> Doc.BindView(fun filtered ->
                if List.isEmpty filtered then
                  div [ cl Css.``weave-select__no-items`` ] [ noItemsContent ]
                else
                  filtered
                  |> List.mapi (fun i def ->
                    Render.singleItem selectId i def selectedValue openVar highlightedIndex enabled)
                  |> Doc.Concat)
            ]
          ]
        else
          Doc.Empty)

    let outsideClickWatcher =
      openVar.View
      |> DocumentEventListener.onMouseDown [ selectRootRef; popoverRef ] (fun () -> openVar.Value <- false)

    [
      outsideClickWatcher

      div [
        cl Css.``weave-select``
        Disabled.disabledClass Css.``weave-select--disabled`` enabled
        Attr.DynamicClassPred Css.``weave-select--open`` openVar.View
        Attr.DynamicClassPred Css.``weave-select--focused`` (isFocused.View <||> openVar.View)
        Attr.DynamicClassPred Css.``weave-select--readonly`` readOnly
        Attr.DynamicClassPred Css.``weave-select--has-value`` hasValue
        Attr.DynamicClassPred Css.``weave-select--clearable`` clearable
        on.clickTapViewGuarded interactable (fun () ->
          openVar.Value <- not openVar.Value

          if openVar.Value then
            highlightedIndex.Value <- -1
            searchText.Value <- "")
        on.afterRender (fun el -> selectRootRef.Value <- el)
        yield! attrs
      ] [
        Render.sizer items
        Field.create (
          inputElement,
          isFocused.View <||> openVar.View,
          shouldFloat,
          variant = variant,
          labelText = labelText,
          ?showHelpText = showHelpText,
          ?helpText = helpText,
          enabled = enabled,
          endAdornment = endAdornment
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
      ?variant: Variant,
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

    let variant = defaultArg variant Variant.Standard
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

    let filteredItems =
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

    let filteredSnapshot = ref ([]: SelectItemDef<'T> list)

    let filteredItemsTracked =
      filteredItems
      |> View.Map(fun defs ->
        filteredSnapshot.Value <- defs
        defs)

    let selectRootRef = ref (JS.Document.CreateElement "div")
    let popoverRef = ref (JS.Document.CreateElement "div")

    let selectId = WeaveId.create "weave-select"

    let activeDescendantView =
      highlightedIndex.View
      |> View.MapCached(fun idx -> if idx >= 0 then sprintf "%s-item-%d" selectId idx else "")

    let handleKeyNav (ev: Dom.KeyboardEvent) =
      let items = filteredSnapshot.Value
      let count = List.length items

      match ev.Key with
      | "Enter" ->
        if not openVar.Value then
          openVar.Value <- true
          highlightedIndex.Value <- -1
          searchText.Value <- ""
        else
          let idx = highlightedIndex.Value

          if idx >= 0 && idx < count then
            let def = items.[idx]
            let current = selectedValues.Value

            if Set.contains def.Value current then
              selectedValues.Value <- Set.remove def.Value current
            else
              selectedValues.Value <- Set.add def.Value current
      | "Escape" ->
        if openVar.Value then
          openVar.Value <- false
      | "ArrowDown" ->
        if not openVar.Value then
          openVar.Value <- true
          highlightedIndex.Value <- 0
          searchText.Value <- ""
        else
          let next = highlightedIndex.Value + 1

          if next < count then
            highlightedIndex.Value <- next
      | "ArrowUp" ->
        if openVar.Value && highlightedIndex.Value > 0 then
          highlightedIndex.Value <- highlightedIndex.Value - 1
      | "Tab" ->
        if openVar.Value then
          openVar.Value <- false
      | _ -> ()

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

        on.focus (fun _ _ -> isFocused.Value <- true)
        on.blur (fun _ _ -> isFocused.Value <- false)

        on.keyDown (fun _ (ev: Dom.KeyboardEvent) ->
          match ev.Key with
          | "Enter"
          | " "
          | "Escape"
          | "ArrowDown"
          | "ArrowUp" ->
            ev.PreventDefault()
            handleKeyNav ev
          | "Tab" -> handleKeyNav ev
          | _ -> ())
      ] [
        Doc.EmbedView selectedDisplay
        span [ cl Css.``weave-select__placeholder`` ] [ Doc.TextView effectivePlaceholder ]
      ]

    let endAdornment =
      div [ cl Css.``weave-select__actions`` ] [
        Render.clearButton (fun () -> selectedValues.Value <- Set.empty)
        Render.chevron ()
      ]

    let popover =
      openVar.View
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          div [
            cl Css.``weave-select__popover``
            on.afterRender (fun el -> popoverRef.Value <- el)
            on.clickTap (fun _ ev -> ev?stopPropagation ())
          ] [
            if searchable then
              Render.searchInput searchText handleKeyNav

            if showSelectAll then
              Render.selectAllRow filteredItemsTracked selectedValues enabled selectAllText
              div [ cl Css.``weave-select__divider`` ] []

            div [
              cl Css.``weave-select__list``
              Attr.Create "role" "listbox"
              Attr.Create "aria-multiselectable" "true"
            ] [
              filteredItemsTracked
              |> Doc.BindView(fun filtered ->
                if List.isEmpty filtered then
                  div [ cl Css.``weave-select__no-items`` ] [ noItemsContent ]
                else
                  filtered
                  |> List.mapi (fun i def ->
                    Render.multiItem selectId i def selectedValues highlightedIndex enabled)
                  |> Doc.Concat)
            ]
          ]
        else
          Doc.Empty)

    let outsideClickWatcher =
      openVar.View
      |> DocumentEventListener.onMouseDown [ selectRootRef; popoverRef ] (fun () -> openVar.Value <- false)

    [
      outsideClickWatcher

      div [
        cl Css.``weave-select``
        cl Css.``weave-select--multi``
        Disabled.disabledClass Css.``weave-select--disabled`` enabled
        Attr.DynamicClassPred Css.``weave-select--open`` openVar.View
        Attr.DynamicClassPred Css.``weave-select--focused`` (isFocused.View <||> openVar.View)
        Attr.DynamicClassPred Css.``weave-select--readonly`` readOnly
        Attr.DynamicClassPred Css.``weave-select--has-value`` hasValue
        Attr.DynamicClassPred Css.``weave-select--clearable`` clearable
        on.clickTapViewGuarded interactable (fun () ->
          openVar.Value <- not openVar.Value

          if openVar.Value then
            highlightedIndex.Value <- -1
            searchText.Value <- "")
        on.afterRender (fun el -> selectRootRef.Value <- el)
        yield! attrs
      ] [
        Render.sizer items
        Field.create (
          inputElement,
          isFocused.View <||> openVar.View,
          shouldFloat,
          variant = variant,
          labelText = labelText,
          ?showHelpText = showHelpText,
          ?helpText = helpText,
          enabled = enabled,
          endAdornment = endAdornment
        )
        popover
      ]
    ]
    |> Doc.Concat
