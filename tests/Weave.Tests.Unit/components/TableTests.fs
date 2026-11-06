module Weave.Tests.Unit.TableTests

open Expecto
open WebSharper.UI
open Weave

let private columns: Table.TableColumnDef<string * int> list = [
  TableColumn.create ("name", Doc.Empty, (fun _ -> Doc.Empty), sort = Table.ColumnSort.by fst)
  TableColumn.create ("stock", Doc.Empty, (fun _ -> Doc.Empty), sort = Table.ColumnSort.External)
  TableColumn.create ("color", Doc.Empty, (fun _ -> Doc.Empty))
]

let private rows = [ "Beta", 2; "Alpha", 3; "Gamma", 1 ]

let private ascending column =
  Some(
    {
      Column = column
      Direction = Table.SortDirection.Ascending
    }
    : Table.SortState
  )

let private descending column =
  Some(
    {
      Column = column
      Direction = Table.SortDirection.Descending
    }
    : Table.SortState
  )

[<Tests>]
let tableTests =
  testList "Table" [

    testList "SortState.next" [
      testCase "given an unsorted table when a header is activated then that column sorts ascending"
      <| fun () ->
        Expect.equal
          (Table.SortState.next "stock" None)
          (ascending "stock")
          "Unsorted should become ascending"

      testCase "given an ascending column when its header is activated then it sorts descending"
      <| fun () ->
        Expect.equal
          (Table.SortState.next "stock" (ascending "stock"))
          (descending "stock")
          "Ascending should become descending"

      testCase "given a descending column when its header is activated then the table is unsorted"
      <| fun () ->
        Expect.isNone (Table.SortState.next "stock" (descending "stock")) "Descending should become unsorted"

      testCase "given a sorted column when another header is activated then the other column sorts ascending"
      <| fun () ->
        Expect.equal
          (Table.SortState.next "name" (descending "stock"))
          (ascending "name")
          "A new column should start ascending"
    ]

    testList "SortState.ariaSort" [
      testCase "given an unsorted table then every header reports none"
      <| fun () -> Expect.equal (Table.SortState.ariaSort "name" None) "none" "Unsorted header should be none"

      testCase "given an ascending column then its header reports ascending"
      <| fun () ->
        Expect.equal
          (Table.SortState.ariaSort "stock" (ascending "stock"))
          "ascending"
          "Sorted header should be ascending"

      testCase "given a descending column then its header reports descending"
      <| fun () ->
        Expect.equal
          (Table.SortState.ariaSort "stock" (descending "stock"))
          "descending"
          "Sorted header should be descending"

      testCase "given another column is sorted then this header reports none"
      <| fun () ->
        Expect.equal
          (Table.SortState.ariaSort "name" (ascending "stock"))
          "none"
          "Other headers should be none"
    ]

    testList "sortRows" [
      testCase "given no sort state then rows keep their order"
      <| fun () ->
        Expect.equal (Table.sortRows columns None rows) rows "Unsorted rows should keep their order"

      testCase "given an ascending compared column then rows are ordered by it"
      <| fun () ->
        Expect.equal
          (Table.sortRows columns (ascending "name") rows |> List.map fst)
          [ "Alpha"; "Beta"; "Gamma" ]
          "Rows should be ascending by name"

      testCase "given a descending compared column then rows are ordered in reverse"
      <| fun () ->
        Expect.equal
          (Table.sortRows columns (descending "name") rows |> List.map fst)
          [ "Gamma"; "Beta"; "Alpha" ]
          "Rows should be descending by name"

      testCase "given an externally sorted column then rows keep the order they arrived in"
      <| fun () ->
        Expect.equal
          (Table.sortRows columns (ascending "stock") rows)
          rows
          "External sort should not reorder rows"

      testCase "given a sort state for an unknown column then rows keep their order"
      <| fun () ->
        Expect.equal
          (Table.sortRows columns (ascending "missing") rows)
          rows
          "Unknown columns should not reorder rows"
    ]
  ]
