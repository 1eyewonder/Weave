namespace WeaveStarter

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

open Domain

[<JavaScript>]
module State =
  type Model = {
    Range: Var<string option>
    Status: Var<string option>
    Search: Var<string>
    Selected: Var<Transaction option>
    Ranged: View<Transaction list>
    Visible: View<Transaction list>
    Sort: Var<Table.SortState option>
    Page: Var<int>
    PageCount: View<int>
  }

  let pageSize = 5

  let create () =
    let range = Var.Create(Some "Last 30 days")
    let status = Var.Create(Some "All statuses")
    let search = Var.Create ""
    let selected = Var.Create<Transaction option> None
    let page = Var.Create 1

    let ranged =
      range.View
      |> View.Map(fun value ->
        let limit =
          match value with
          | Some "Last 7 days" -> 7
          | Some "Last 90 days" -> 90
          | _ -> 30

        transactions |> List.filter (fun item -> item.DaysAgo < limit))

    let filters =
      (status.View, search.View) ||> View.Map2(fun status query -> status, query)

    let visible =
      (ranged, filters)
      ||> View.Map2(fun items (status, query) ->
        let query = query.Trim().ToLower()

        items
        |> List.filter (fun item ->
          (status = Some "All statuses" || status = Some item.Status)
          && (query = ""
              || item.Customer.ToLower().Contains(query)
              || item.Reference.ToLower().Contains(query))))

    let pageCount =
      visible
      |> View.Map(fun items -> max 1 ((items.Length + pageSize - 1) / pageSize))

    visible |> View.Sink(fun _ -> Var.Set page 1)

    // A hidden transaction should not remain open after changing a filter or period.
    visible
    |> View.Sink(fun items ->
      match selected.Value with
      | Some item when not (items |> List.exists (fun row -> row.Reference = item.Reference)) ->
        Var.Set selected None
      | _ -> ())

    {
      Range = range
      Status = status
      Search = search
      Selected = selected
      Ranged = ranged
      Visible = visible
      Sort = Var.Create None
      Page = page
      PageCount = pageCount
    }
