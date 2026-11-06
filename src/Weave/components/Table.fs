namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module Table =

  module Density =

    let compact = cl Css.``weave-table--compact``
    let standard = cl Css.``weave-table--standard``
    let spacious = cl Css.``weave-table--spacious``

  let striped = cl Css.``weave-table--striped``
  let hoverable = cl Css.``weave-table--hoverable``

  /// Keeps the header row visible while the table body scrolls. Give the table a max height through attrs.
  let stickyHeader = cl Css.``weave-table--sticky-header``

  /// Hides the caption visually while keeping it as the table's accessible name
  let hiddenCaption = cl Css.``weave-table--hidden-caption``

  /// <summary>Sets the tint of alternate rows. Use with <c>Table.striped</c>.</summary>
  module StripeColor =

    let private tint (rgb: string) =
      Attr.Style "--table-stripe-color" $"rgba(var(--palette-%s{rgb}-rgb), var(--weave-alpha-selected))"

    let primary = tint "primary"
    let secondary = tint "secondary"
    let tertiary = tint "tertiary"
    let error = tint "error"
    let warning = tint "warning"
    let success = tint "success"
    let info = tint "info"

    let custom (value: string) = Attr.Style "--table-stripe-color" value

  /// <summary>Sets the highlight of the row under the pointer. Use with <c>Table.hoverable</c> or clickable rows.</summary>
  module HoverColor =

    let private tint (rgb: string) =
      Attr.Style "--table-hover-color" $"rgba(var(--palette-%s{rgb}-rgb), var(--weave-alpha-selected-hover))"

    let primary = tint "primary"
    let secondary = tint "secondary"
    let tertiary = tint "tertiary"
    let error = tint "error"
    let warning = tint "warning"
    let success = tint "success"
    let info = tint "info"

    let custom (value: string) = Attr.Style "--table-hover-color" value

  [<RequireQualifiedAccess>]
  type SortDirection =
    | Ascending
    | Descending

  /// <summary>The sorted column, identified by its <c>TableColumn</c> key, and the direction.</summary>
  type SortState = {
    Column: string
    Direction: SortDirection
  }

  [<RequireQualifiedAccess>]
  module SortState =

    /// <summary>The sort state after the header of <c>column</c> is activated. Cycles ascending, descending, then unsorted.</summary>
    let next (column: string) (current: SortState option) : SortState option =
      match current with
      | Some {
               Column = c
               Direction = SortDirection.Ascending
             } when c = column ->
        Some {
          Column = column
          Direction = SortDirection.Descending
        }
      | Some {
               Column = c
               Direction = SortDirection.Descending
             } when c = column -> None
      | Some _
      | None ->
        Some {
          Column = column
          Direction = SortDirection.Ascending
        }

    let ariaSort (column: string) (current: SortState option) =
      match current with
      | Some {
               Column = c
               Direction = SortDirection.Ascending
             } when c = column -> "ascending"
      | Some {
               Column = c
               Direction = SortDirection.Descending
             } when c = column -> "descending"
      | Some _
      | None -> "none"

  /// <summary>How a sortable column orders its rows.</summary>
  [<RequireQualifiedAccess>]
  type ColumnSort<'T> =
    /// The table reorders the rows with this comparison
    | Compare of ('T -> 'T -> int)
    /// The header sorts, but the rows arrive already ordered, for example from a server reading the sort state
    | External

  [<RequireQualifiedAccess>]
  module ColumnSort =

    /// <summary>Sorts rows by comparing the value <c>projection</c> returns for each.</summary>
    let by (projection: 'T -> 'Key) : ColumnSort<'T> =
      ColumnSort.Compare(fun a b -> compare (projection a) (projection b))

  type TableColumnDef<'T> = {
    Key: string
    Header: Doc
    Cell: 'T -> Doc
    Sort: ColumnSort<'T> option
    Attrs: Attr list
  }

  /// <summary>Orders <c>rows</c> by the sorted column when it compares rows itself, and keeps the order otherwise.</summary>
  let sortRows (columns: TableColumnDef<'T> list) (state: SortState option) (rows: 'T list) =
    let comparison =
      state
      |> Option.bind (fun s ->
        columns
        |> List.tryFind (fun column -> column.Key = s.Column)
        |> Option.bind (fun column ->
          match column.Sort with
          | Some(ColumnSort.Compare order) -> Some(s.Direction, order)
          | Some ColumnSort.External
          | None -> None))

    match comparison with
    | Some(SortDirection.Ascending, order) -> List.sortWith order rows
    | Some(SortDirection.Descending, order) -> List.sortWith (fun a b -> order b a) rows
    | None -> rows

[<JavaScript>]
type TableColumn =

  /// <summary>Defines a table column.</summary>
  /// <param name="key">Identifies the column in the sort state and in <c>rowHeader</c>. Unique within the table.</param>
  /// <param name="header">The Doc rendered in the column header cell.</param>
  /// <param name="cell">Renders the body cell content for a row value.</param>
  /// <param name="sort">When supplied, the header becomes a button that sorts by this column.</param>
  /// <param name="attrs">Additional attributes merged onto both the header and body cells of this column, such as alignment or width.</param>
  static member create<'T>
    (key: string, header: Doc, cell: 'T -> Doc, ?sort: Table.ColumnSort<'T>, ?attrs: Attr list)
    : Table.TableColumnDef<'T> = {
    Key = key
    Header = header
    Cell = cell
    Sort = sort
    Attrs = defaultArg attrs List.empty
  }

[<JavaScript>]
module private TableInternal =

  let mutable idCounter = 0

  let nextRowHeaderId () =
    idCounter <- idCounter + 1
    sprintf "weave-table-row-header-%d" idCounter

  let selectBox (name: Attr) (isChecked: View<bool>) (isIndeterminate: View<bool>) (onToggle: Attr) =
    label [
      cls [
        Css.``weave-checkbox``
        Css.``weave-checkbox--primary``
        Css.``weave-table__select``
      ]
    ] [
      input [
        Attr.Prop "type" "checkbox"
        cl Css.``weave-checkbox__input``
        name
        Attr.DynamicProp "checked" isChecked
        Attr.DynamicProp "indeterminate" isIndeterminate
        onToggle
      ] []
      span [ cl Css.``weave-checkbox__span`` ] []
    ]

  let selectCellAttrs = [
    cl Css.``weave-table__select-cell``
    on.click (fun _ ev -> ev.StopPropagation())
  ]

[<JavaScript; RequireQualifiedAccess>]
type Table =

  /// <summary>Creates a data table rendered as a semantic <c>table</c> element.</summary>
  /// <param name="columns">Column definitions created with <c>TableColumn.create</c>.</param>
  /// <param name="rows">Reactive list of row values.</param>
  /// <param name="rowKey">Stable identity for each row. Unchanged rows are not re-rendered when <c>rows</c> updates, and selection is tracked by this key.</param>
  /// <param name="onRowClick">When supplied, rows activate on click, and the first cell holds a button that activates the row from the keyboard.</param>
  /// <param name="selectedKey">When supplied, activating a row selects it and the selected row is marked with <c>aria-current</c>.</param>
  /// <param name="selectedKeys">When supplied, a leading checkbox column selects any number of rows, with a header checkbox that selects or clears every row shown.</param>
  /// <param name="sort">The sort state. Supply it to start sorted or to react to sorting, such as sorting on a server. Defaults to unsorted.</param>
  /// <param name="rowHeader">Key of the column whose cells label their rows, rendered as row header cells.</param>
  /// <param name="caption">The table's caption, which is also its accessible name. Hide it visually with <c>Table.hiddenCaption</c>.</param>
  /// <param name="emptyContent">Doc rendered in a full-width row when <c>rows</c> is empty.</param>
  /// <param name="attrs">Additional attributes (density, striped, hoverable, sticky header, colors, etc.) merged onto the root element.</param>
  static member create<'T>
    (
      columns: Table.TableColumnDef<'T> list,
      rows: View<'T list>,
      rowKey: 'T -> string,
      ?onRowClick: 'T -> unit,
      ?selectedKey: Var<string option>,
      ?selectedKeys: Var<Set<string>>,
      ?sort: Var<Table.SortState option>,
      ?rowHeader: string,
      ?caption: Doc,
      ?emptyContent: Doc,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs List.empty
    let sort = defaultArg sort (Var.Create None)
    let isInteractive = Option.isSome onRowClick || Option.isSome selectedKey

    let columnCount =
      List.length columns + (if Option.isSome selectedKeys then 1 else 0)

    let activate (row: 'T) =
      selectedKey |> Option.iter (fun key -> Var.Set key (Some(rowKey row)))
      onRowClick |> Option.iter (fun handler -> handler row)

    let sortIndicator (key: string) =
      let arrow =
        sort.View
        |> View.Map(fun state ->
          match state with
          | Some {
                   Column = c
                   Direction = Table.SortDirection.Descending
                 } when c = key -> "▼"
          | Some _
          | None -> "▲")

      span [
        cl Css.``weave-table__sort-indicator``
        Attr.Create "aria-hidden" "true"
        Attr.DynamicClassPred
          Css.``weave-table__sort-indicator--active``
          (sort.View |> View.Map(Option.exists (fun s -> s.Column = key)))
      ] [ textView arrow ]

    let headerCell (column: Table.TableColumnDef<'T>) =
      match column.Sort with
      | Some _ ->
        th [
          cl Css.``weave-table__header-cell``
          attr.scope "col"
          Attr.Dynamic "aria-sort" (sort.View |> View.Map(Table.SortState.ariaSort column.Key))
          yield! column.Attrs
        ] [
          button [
            attr.``type`` "button"
            cl Css.``weave-table__sort-button``
            on.click (fun _ _ -> Var.Update sort (Table.SortState.next column.Key))
          ] [ column.Header; sortIndicator column.Key ]
        ]
      | None ->
        th [ cl Css.``weave-table__header-cell``; attr.scope "col"; yield! column.Attrs ] [ column.Header ]

    let sortedRows =
      (rows, sort.View) ||> View.Map2(fun rs state -> Table.sortRows columns state rs)

    let selectAllHeader (keys: Var<Set<string>>) =
      let coverage =
        (sortedRows, keys.View)
        ||> View.Map2(fun rs selected ->
          let shown = rs |> List.map rowKey |> Set.ofList
          shown, Set.intersect shown selected)

      let allSelected =
        coverage
        |> View.Map(fun (shown, checkedShown) -> not shown.IsEmpty && checkedShown = shown)

      let someSelected =
        coverage
        |> View.Map(fun (shown, checkedShown) -> not checkedShown.IsEmpty && checkedShown <> shown)

      let toggleAll =
        on.changeView coverage (fun _ _ (shown, checkedShown) ->
          if not shown.IsEmpty && checkedShown = shown then
            Var.Update keys (fun selected -> Set.difference selected shown)
          else
            Var.Update keys (Set.union shown))

      th [
        cl Css.``weave-table__header-cell``
        attr.scope "col"
        yield! TableInternal.selectCellAttrs
      ] [
        TableInternal.selectBox
          (Attr.Create "aria-label" "Select all rows")
          allSelected
          someSelected
          toggleAll
      ]

    let rowAction (row: 'T) (content: Doc) =
      button [
        attr.``type`` "button"
        cl Css.``weave-table__row-action``
        on.click (fun _ ev ->
          ev.StopPropagation()
          activate row)
      ] [ content ]

    let rowSelect (keys: Var<Set<string>>) (key: string) (name: Attr) =
      let isChecked = keys.View |> View.Map(Set.contains key)

      let toggle () =
        Var.Update keys (fun selected ->
          if selected.Contains key then
            selected.Remove key
          else
            selected.Add key)

      td [ cl Css.``weave-table__cell``; yield! TableInternal.selectCellAttrs ] [
        TableInternal.selectBox name isChecked (View.Const false) (on.change (fun _ _ -> toggle ()))
      ]

    let renderRow (row: 'T) =
      let key = rowKey row
      let headerId = rowHeader |> Option.map (fun _ -> TableInternal.nextRowHeaderId ())

      let isCurrent =
        selectedKey
        |> Option.map (fun selected -> selected.View |> View.Map((=) (Some key)))
        |> Option.defaultValue (View.Const false)

      let isChecked =
        selectedKeys
        |> Option.map (fun keys -> keys.View |> View.Map(Set.contains key))
        |> Option.defaultValue (View.Const false)

      let rowAttrs = [
        if isInteractive then
          cl Css.``weave-table__row--clickable``
          on.click (fun _ _ -> activate row)

        Attr.DynamicClassPred Css.``weave-table__row--selected`` ((isCurrent, isChecked) ||> View.Map2 (||))
        Attr.DynamicPred "aria-current" isCurrent (View.Const "true")
      ]

      let selectName =
        match headerId with
        | Some id -> Attr.Create "aria-labelledby" id
        | None -> Attr.Create "aria-label" "Select row"

      let cell (index: int) (column: Table.TableColumnDef<'T>) =
        let content =
          if isInteractive && index = 0 then
            rowAction row (column.Cell row)
          else
            column.Cell row

        if rowHeader = Some column.Key then
          th [
            cls [ Css.``weave-table__cell``; Css.``weave-table__row-header`` ]
            attr.scope "row"
            Attr.bindOption attr.id headerId
            yield! column.Attrs
          ] [ content ]
        else
          td [ cl Css.``weave-table__cell``; yield! column.Attrs ] [ content ]

      tr [ cl Css.``weave-table__row``; yield! rowAttrs ] [
        match selectedKeys with
        | Some keys -> rowSelect keys key selectName
        | None -> Doc.Empty

        yield! List.mapi cell columns
      ]

    let emptyRow =
      match emptyContent with
      | Some content ->
        rows
        |> Doc.BindView(fun rs ->
          if List.isEmpty rs then
            tr [ cl Css.``weave-table__row`` ] [
              td [
                cl Css.``weave-table__cell``
                cl Css.``weave-table__empty``
                attr.colspan (string<int> columnCount)
              ] [ content ]
            ]
          else
            Doc.Empty)
      | None -> Doc.Empty

    div [ cl Css.``weave-table``; yield! attrs ] [
      table [ cl Css.``weave-table__table`` ] [
        match caption with
        | Some content -> Html.caption [ cl Css.``weave-table__caption`` ] [ content ]
        | None -> Doc.Empty

        thead [ cl Css.``weave-table__head`` ] [
          tr [] [
            match selectedKeys with
            | Some keys -> selectAllHeader keys
            | None -> Doc.Empty

            yield! List.map headerCell columns
          ]
        ]
        tbody [ cl Css.``weave-table__body`` ] [
          sortedRows |> Doc.BindSeqCachedBy rowKey renderRow
          emptyRow
        ]
      ]
    ]
