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
    Issues: Var<Issue list>
    ViewMode: Var<string>
    Search: Var<string>
    StatusFilter: Var<string option>
    AssigneeFilter: Var<string option>
    SelectedIssue: Var<int option>
    Adding: Var<bool>
    NewTitle: Var<string>
    TitleError: Var<bool>
    NewAssignee: Var<string option>
    NewStatus: Var<string option>
    NextId: Var<int>
    Visible: View<Issue list>
  }

  let create () =
    let issues = Var.Create sampleIssues
    let search = Var.Create ""
    let statusFilter = Var.Create(Some "All statuses")
    let assigneeFilter = Var.Create(Some "All assignees")

    let filters =
      (search.View, statusFilter.View)
      ||> View.Map2(fun query status -> query, status)

    let filters =
      (filters, assigneeFilter.View)
      ||> View.Map2(fun (query, status) assignee -> query, status, assignee)

    let visible =
      (issues.View, filters)
      ||> View.Map2(fun items (query, status, assignee) ->
        let query = query.Trim().ToLower()

        items
        |> List.filter (fun issue ->
          (query = "" || issue.Title.ToLower().Contains(query))
          && (status = Some "All statuses" || status = Some issue.Status)
          && (assignee = Some "All assignees" || assignee = Some issue.Assignee)))

    {
      Issues = issues
      ViewMode = Var.Create "List"
      Search = search
      StatusFilter = statusFilter
      AssigneeFilter = assigneeFilter
      SelectedIssue = Var.Create None
      Adding = Var.Create false
      NewTitle = Var.Create ""
      TitleError = Var.Create false
      NewAssignee = Var.Create(Some "Unassigned")
      NewStatus = Var.Create(Some "Backlog")
      NextId = Var.Create 106
      Visible = visible
    }

  let updateStatus (state: Model) id status =
    Var.Update
      state.Issues
      (List.map (fun issue ->
        if issue.Id = id then
          { issue with Status = status }
        else
          issue))

  let addIssue (state: Model) () =
    let title = state.NewTitle.Value.Trim()

    if title = "" then
      Var.Set state.TitleError true
    else
      Var.Set state.TitleError false

      let issue = {
        Id = state.NextId.Value
        Title = title
        Status = Option.defaultValue "Backlog" state.NewStatus.Value
        Assignee = Option.defaultValue "Unassigned" state.NewAssignee.Value
        Description = "No description yet."
      }

      Var.Update state.Issues (fun current -> current @ [ issue ])
      Var.Update state.NextId ((+) 1)
      Var.Set state.NewTitle ""
      Var.Set state.Adding false
