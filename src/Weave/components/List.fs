namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module WeaveList =

  /// <summary>
  /// Controls how items in a WeaveList respond to selection.
  /// </summary>
  [<RequireQualifiedAccess; Struct>]
  type SelectionMode =
    /// <summary>
    /// Clicking an item selects it; clicking another item deselects the previous one.
    /// </summary>
    | SingleSelection
    /// <summary>
    /// Like SingleSelection, but clicking the already-selected item deselects it.
    /// </summary>
    | ToggleSelection
    /// <summary>
    /// Multiple items can be selected simultaneously. A parent checkbox selects its enabled leaves.
    /// </summary>
    | MultiSelection

  module Color =

    let primary = cl Css.``weave-list-item--primary``
    let secondary = cl Css.``weave-list-item--secondary``
    let tertiary = cl Css.``weave-list-item--tertiary``
    let error = cl Css.``weave-list-item--error``
    let warning = cl Css.``weave-list-item--warning``
    let success = cl Css.``weave-list-item--success``
    let info = cl Css.``weave-list-item--info``

  /// <summary>
  /// Configuration record for a list item. Constructed via ListItem.Create.
  /// </summary>
  type ListItemDef = {
    Content: Doc
    Value: string option
    SecondaryContent: Doc option
    NestedChildren: ListChild list
    Expanded: Var<bool>
    Disabled: View<bool>
    Attrs: Attr list
  }

  /// <summary>
  /// Represents a child element within a WeaveList.
  /// </summary>
  and [<RequireQualifiedAccess>] ListChild =
    /// <summary>A selectable list item.</summary>
    | Item of ListItemDef
    /// <summary>Arbitrary content (dividers, subheaders, custom markup).</summary>
    | Content of Doc

open WeaveList

/// <summary>
/// A scrollable list container for displaying text, avatars, icons, and interactive items.
/// </summary>
[<JavaScript>]
type WeaveList =

  /// <summary>
  /// Creates a list container.
  /// </summary>
  /// <param name="children">Child elements (ListItem, ListSubheader, or arbitrary Doc wrapped in ListChild.Content).</param>
  /// <param name="selectedValue">Shared reactive variable for single/toggle selection across items.</param>
  /// <param name="selectedValues">Shared reactive variable for multi-selection across items.</param>
  /// <param name="selectionMode">Determines click behaviour. Defaults to SingleSelection.</param>
  /// <param name="readOnly">When true, items display their state but do not respond to clicks.</param>
  /// <param name="bordered">When true, draws a border around the list. Defaults to true.</param>
  /// <param name="attrs">Additional HTML attributes.</param>
  static member create
    (
      children: ListChild list,
      ?selectedValue: Var<string option>,
      ?selectedValues: Var<Set<string>>,
      ?selectionMode: SelectionMode,
      ?readOnly: View<bool>,
      ?bordered: View<bool>,
      ?attrs: Attr list
    ) =
    let readOnly = defaultArg readOnly (View.Const false)
    let bordered = defaultArg bordered (View.Const true)
    let selectionMode = defaultArg selectionMode SelectionMode.SingleSelection
    let attrs = defaultArg attrs []

    let isMultiSelect = Option.isSome selectedValues

    let rec collectLeafValues (children: ListChild list) =
      children
      |> List.collect (fun child ->
        match child with
        | ListChild.Content _ -> []
        | ListChild.Item def ->
          if List.isEmpty def.NestedChildren then
            match def.Value with
            | Some value -> [
                def.Disabled |> View.Map(fun disabled -> if disabled then None else Some value)
              ]
            | None -> []
          else
            collectLeafValues def.NestedChildren)

    let rec renderChild (child: ListChild) : Doc =
      match child with
      | ListChild.Content doc -> doc
      | ListChild.Item def -> renderItem def

    and renderItem (def: ListItemDef) : Doc =
      let hasNestedItems = not (List.isEmpty def.NestedChildren)

      let nestedLeafValues =
        if hasNestedItems && isMultiSelect then
          collectLeafValues def.NestedChildren
        else
          []

      let hasNestedLeafValues = not (List.isEmpty nestedLeafValues)

      let enabledLeafValues =
        nestedLeafValues |> View.Sequence |> View.Map(Seq.choose id >> Set.ofSeq)

      let parentCoverage =
        match selectedValues with
        | Some selection when hasNestedLeafValues ->
          (enabledLeafValues, selection.View)
          ||> View.Map2(fun enabled selected -> enabled, Set.intersect enabled selected)
        | _ -> View.Const(Set.empty, Set.empty)

      let isSelected =
        match def.Value, selectedValue, selectedValues with
        | _, _, Some _ when hasNestedLeafValues ->
          parentCoverage
          |> View.MapCached(fun (enabled, selectedLeaves) -> not enabled.IsEmpty && selectedLeaves = enabled)
        | Some v, Some sv, _ -> sv.View |> View.MapCached(fun sel -> sel = Some v)
        | Some v, _, Some svs -> svs.View |> View.MapCached(fun sel -> Set.contains v sel)
        | _ -> View.Const false

      let isIndeterminate =
        parentCoverage
        |> View.MapCached(fun (enabled, selectedLeaves) ->
          not selectedLeaves.IsEmpty && selectedLeaves <> enabled)

      let handleRowClick () =
        if hasNestedItems then
          def.Expanded.Value <- not def.Expanded.Value

        match def.Value, selectedValue, selectedValues with
        | Some v, Some sv, _ ->
          match selectionMode with
          | SelectionMode.SingleSelection -> Var.Set sv (Some v)
          | SelectionMode.ToggleSelection ->
            if sv.Value = Some v then
              Var.Set sv None
            else
              Var.Set sv (Some v)
          | _ -> ()
        | Some v, _, Some svs when not hasNestedLeafValues ->
          if Set.contains v svs.Value then
            Var.Set svs (Set.remove v svs.Value)
          else
            Var.Set svs (Set.add v svs.Value)
        | _ -> ()

      let handleParentCheckboxClick enabled =
        match selectedValues with
        | Some svs ->
          Var.Update svs (fun selected ->
            if Set.isSubset enabled selected then
              Set.difference selected enabled
            else
              Set.union selected enabled)
        | None -> ()

      let interactable = View.not def.Disabled <&&> View.not readOnly

      let isSelectable = def.Value.IsSome || hasNestedLeafValues

      let hasRowAction = hasNestedItems || (def.Value.IsSome && selectedValue.IsSome)

      let contentId = WeaveId.create "weave-list-item-label"

      let checkedState =
        (isSelected, isIndeterminate)
        ||> View.Map2(fun selected indeterminate ->
          if indeterminate then "mixed"
          elif selected then "true"
          else "false")

      let controlState () = [
        interactable
        |> View.Map(fun enabled -> if enabled then "0" else "-1")
        |> Attr.Dynamic "tabindex"
        interactable |> View.Map(not >> string<bool>) |> Attr.Dynamic "aria-disabled"
      ]

      let checkbox =
        span [
          cl Css.``weave-list-item__checkbox``
          Attr.DynamicClassPred Css.``weave-list-item__checkbox--checked`` isSelected
          Attr.DynamicClassPred Css.``weave-list-item__checkbox--indeterminate`` isIndeterminate
          if isMultiSelect && isSelectable then
            Attr.Create "role" "checkbox"
            Attr.Create "aria-labelledby" contentId
            checkedState |> Attr.Dynamic "aria-checked"
            yield! controlState ()

            on.keyDownView (View.zip interactable enabledLeafValues) (fun el ev (isInteractable, enabled) ->
              match ev with
              | Key.Activate when ev.Target = el ->
                ev.PreventDefault()
                ev.StopPropagation()

                if isInteractable then
                  if hasNestedLeafValues then
                    handleParentCheckboxClick enabled
                  else
                    handleRowClick ()
              | _ -> ())

          if hasNestedLeafValues then
            on.clickView (View.zip interactable enabledLeafValues) (fun _ ev (isInteractable, enabled) ->
              ev.StopPropagation()

              if isInteractable then
                handleParentCheckboxClick enabled)
        ] []

      let itemRow =
        div [
          cl Css.``weave-list-item``
          if isSelectable then
            cl Css.``weave-list-item--selectable``
          Attr.DynamicClassPred Css.``weave-list-item--disabled`` def.Disabled
          Attr.DynamicClassPred Css.``weave-list-item--readonly`` readOnly
          Attr.DynamicClassPred Css.``weave-list-item--selected`` isSelected
          on.clickViewGuarded interactable handleRowClick
          yield! def.Attrs
        ] [
          if isMultiSelect && isSelectable then
            if hasNestedLeafValues then
              enabledLeafValues
              |> Doc.BindView(fun enabled -> if Set.isEmpty enabled then Doc.Empty else checkbox)
            else
              checkbox

          div [
            cl Css.``weave-list-item__content``
            Attr.Create "id" contentId
            if hasRowAction then
              Attr.Create "role" "button"

              if hasNestedItems then
                def.Expanded.View |> View.Map string<bool> |> Attr.Dynamic "aria-expanded"
              elif selectedValue.IsSome then
                isSelected |> View.Map string<bool> |> Attr.Dynamic "aria-pressed"

              yield! controlState ()

              on.keyDownView interactable (fun el ev canInteract ->
                match ev with
                | Key.Activate when ev.Target = el ->
                  ev.PreventDefault()

                  if canInteract then
                    handleRowClick ()
                | _ -> ())
          ] [
            def.Content
            match def.SecondaryContent with
            | Some sec -> div [ cl Css.``weave-list-item__secondary-content`` ] [ sec ]
            | None -> Doc.Empty
          ]

          if hasNestedItems then
            span [
              cl Css.``weave-list-item__expand-icon``
              Attr.DynamicClassPred Css.``weave-list-item__expand-icon--expanded`` def.Expanded.View
            ] [ text "▼" ]
        ]

      if hasNestedItems then
        let nestedContent =
          def.Expanded.View
          |> Doc.BindView(fun isExpanded ->
            if isExpanded then
              div [ cl Css.``weave-list-nested`` ] (def.NestedChildren |> List.map renderChild)
            else
              Doc.Empty)

        Doc.Concat [ itemRow; nestedContent ]
      else
        itemRow

    div
      [
        cl Css.``weave-list``
        Attr.DynamicClassPred Css.``weave-list--readonly`` readOnly
        Attr.DynamicClassPred Css.``weave-list--bordered`` bordered
        yield! attrs
      ]
      (children |> List.map renderChild)

/// <summary>
/// An individual item within a <see cref="WeaveList"/>.
/// Selection, read-only state, and color are managed by the parent WeaveList.
/// Per-item color can be applied via attrs (e.g. cl Css.``weave-list-item--secondary``).
/// </summary>
[<JavaScript>]
type ListItem =

  /// <summary>
  /// Creates a list item definition.
  /// </summary>
  /// <param name="content">Primary content displayed in the item (text, icons, avatars, etc.).</param>
  /// <param name="value">The selectable value for this item. Required when using selection on the parent list.</param>
  /// <param name="secondaryContent">Optional secondary line rendered below the primary content.</param>
  /// <param name="nestedChildren">Optional child items rendered as an expandable nested list.</param>
  /// <param name="expanded">Reactive variable controlling nested list expansion. Created automatically if omitted.</param>
  /// <param name="disabled">When true, the item is non-interactive and visually dimmed.</param>
  /// <param name="attrs">Additional HTML attributes applied to the item row.</param>
  static member create
    (
      content: Doc,
      ?value: string,
      ?secondaryContent: Doc,
      ?nestedChildren: ListChild list,
      ?expanded: Var<bool>,
      ?disabled: View<bool>,
      ?attrs: Attr list
    ) : ListChild =
    ListChild.Item {
      Content = content
      Value = value
      SecondaryContent = secondaryContent
      NestedChildren = defaultArg nestedChildren []
      Expanded = defaultArg expanded (Var.Create false)
      Disabled = defaultArg disabled (View.Const false)
      Attrs = defaultArg attrs []
    }

/// <summary>
/// A non-interactive heading rendered inside a <see cref="WeaveList"/>.
/// </summary>
[<JavaScript>]
type ListSubheader =

  /// <summary>
  /// Creates a list subheader.
  /// </summary>
  /// <param name="content">The subheader content.</param>
  /// <param name="inset">When true, aligns the subheader with items that have icons (adds left padding).</param>
  /// <param name="attrs">Additional HTML attributes.</param>
  static member create(content: Doc, ?inset: bool, ?attrs: Attr list) : ListChild =
    let attrs = defaultArg attrs []
    let inset = defaultArg inset false

    ListChild.Content(
      div [
        cl Css.``weave-list-subheader``
        if inset then
          cl Css.``weave-list-subheader--inset``
        yield! attrs
      ] [ content ]
    )
