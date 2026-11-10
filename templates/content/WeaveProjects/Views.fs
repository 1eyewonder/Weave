namespace WeaveStarter

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

open Domain

[<JavaScript>]
module Views =
  let private statusClass status =
    match status with
    | "Done" -> "done"
    | "In progress" -> "active"
    | _ -> "backlog"

  let private options values =
    values
    |> List.map (fun value -> SelectItem.create (text value, value, value))
    |> View.Const

  let private select label values (selected: Var<string option>) =
    Select.create (
      options values,
      selected,
      labelText = View.Const label,
      attrs = [ Select.Variant.outlined ]
    )

  let private actionButton label action =
    Button.create (text label, onClick = action, attrs = [ Button.Variant.outlined ])

  let private themeToggle () =
    let themeMode = Var.Create Light

    let themeToggle =
      button [
        Attr.Class "theme-toggle"
        Attr.Create "type" "button"
        on.click (fun _ _ -> Var.Set themeMode (toggleMode ()))
      ] [
        span [ Attr.Class "theme-icon"; Attr.Create "aria-hidden" "true" ] [ text "◐" ]
        themeMode.View
        |> View.Map(fun mode -> if mode = Light then "Dark mode" else "Light mode")
        |> Doc.TextView
      ]

    themeToggle

  let issueViews (state: State.Model) =
    let viewMode = state.ViewMode
    let selectedIssue = state.SelectedIssue
    let visible = state.Visible
    let query = state.Search.View |> View.Map(fun value -> value.Trim())

    let issueCard issue =
      div [ Attr.Class "issue" ] [
        button [
          Attr.Class "issue-main"
          Attr.Create "type" "button"
          on.click (fun _ _ -> Var.Set selectedIssue (Some issue.Id))
        ] [
          span [ Attr.Class "issue-id" ] [ text (sprintf "ORB-%d" issue.Id) ]
          span [ Attr.Class "issue-title" ] [
            Highlighter.create (View.Const issue.Title, query, attrs = [ Highlighter.Color.primary ])
          ]
        ]

        div [ Attr.Class "issue-meta" ] [
          span [ Attr.Class("pill status-" + statusClass issue.Status) ] [ text issue.Status ]
          span [ Attr.Class "assignee" ] [
            span [ Attr.Class "avatar"; Attr.Create "aria-hidden" "true" ] [
              text (
                if issue.Assignee = "Unassigned" then
                  "–"
                else
                  issue.Assignee.Substring(0, 1)
              )
            ]
            text issue.Assignee
          ]
        ]
      ]

    let issueViews =
      (visible, viewMode.View)
      ||> View.Map2(fun items mode -> items, mode)
      |> Doc.BindView(fun (items, mode) ->
        if List.isEmpty items then
          div [ Attr.Class "empty" ] [
            text "No issues match your filters. Try a different search or filter."
          ]
        elif mode = "Board" then
          div [ Attr.Class "board" ] [
            for status in statuses do
              let group = items |> List.filter (fun issue -> issue.Status = status)

              div [ Attr.Class("board-column column-" + statusClass status) ] [
                div [ Attr.Class "group-heading" ] [ text (sprintf "%s  ·  %d" status group.Length) ]
                if List.isEmpty group then
                  div [ Attr.Class "empty-column" ] [ text "No issues" ]
                for issue in group do
                  issueCard issue
              ]
          ]
        else
          div [ Attr.Class "list" ] [
            for status in statuses do
              let group = items |> List.filter (fun issue -> issue.Status = status)

              if not (List.isEmpty group) then
                div [ Attr.Class("list-group column-" + statusClass status) ] [
                  div [ Attr.Class "group-heading" ] [ text (sprintf "%s  ·  %d" status group.Length) ]
                  for issue in group do
                    issueCard issue
                ]
          ])

    issueViews

  let details (state: State.Model) =
    let issues = state.Issues
    let selectedIssue = state.SelectedIssue
    let updateStatus = State.updateStatus state

    let details =
      (issues.View, selectedIssue.View)
      ||> View.Map2(fun items selected -> items |> List.tryFind (fun issue -> Some issue.Id = selected))
      |> Doc.BindView(function
        | None -> Doc.Empty
        | Some issue ->
          aside [ Attr.Class "detail"; Attr.Create "aria-label" "Issue details" ] [
            div [ Attr.Class "detail-top" ] [
              span [ Attr.Class "issue-id" ] [ text (sprintf "ORB-%d" issue.Id) ]
              actionButton "Close details" (fun () -> Var.Set selectedIssue None)
            ]
            h2 [] [ text issue.Title ]
            p [] [ text issue.Description ]
            div [ Attr.Class "detail-fields" ] [
              div [ Attr.Class "status-controls" ] [
                span [] [ text "Move to" ]
                div [ Attr.Class "actions" ] [
                  for status in statuses do
                    actionButton status (fun () -> updateStatus issue.Id status)
                ]
              ]

              div [ Attr.Class "detail-person" ] [
                span [] [ text "Assignee" ]
                strong [] [ text issue.Assignee ]
              ]
            ]
          ])

    details

  let addForm (state: State.Model) =
    let adding = state.Adding
    let newAssignee = state.NewAssignee
    let newStatus = state.NewStatus

    let addForm =
      adding.View
      |> Doc.BindView(fun openForm ->
        if not openForm then
          Doc.Empty
        else
          let title =
            FormField.create ("", rules = [ Validate.required "Enter an issue title." ])

          let issueTitle = Form.model {
            let! value = title
            return value.Trim()
          }

          Form.create (
            issueTitle,
            State.addIssue state,
            [
              h2 [] [ text "New issue" ]
              TextField.singleLine (
                title,
                labelText = View.Const "Issue title",
                placeholder = View.Const "What needs to be done?",
                attrs = [ TextField.Variant.outlined; TextField.Width.full ]
              )
              div [ Attr.Class "new-fields" ] [
                select "Status" statuses newStatus
                select "Assignee" people newAssignee
              ]
              div [ Attr.Class "actions" ] [
                button [
                  Attr.Create "type" "submit"
                  Attr.Class "weave-button"
                  Button.Color.primary
                  Button.Variant.filled
                  Typography.button
                ] [ span [ Attr.Class "weave-button__label" ] [ text "Add issue" ] ]
                actionButton "Cancel" (fun () -> Var.Set adding false)
              ]
            ],
            attrs = [ Attr.Class "new-issue" ]
          ))

    addForm

  let render (state: State.Model) =
    let issues = state.Issues
    let viewMode = state.ViewMode
    let search = state.Search
    let statusFilter = state.StatusFilter
    let assigneeFilter = state.AssigneeFilter
    let adding = state.Adding
    let themeToggle = themeToggle ()
    let issueViews = issueViews state
    let details = details state
    let addForm = addForm state

    div [ Attr.Class "workspace" ] [
      aside [ Attr.Class "sidebar"; Attr.Create "aria-label" "Workspace" ] [
        div [ Attr.Class "brand" ] [ span [ Attr.Class "brand-mark" ] [ text "O" ]; text "Orbit workspace" ]
        div [ Attr.Class "sidebar-label" ] [ text "WORKSPACE" ]
        div [ Attr.Class "sidebar-current" ] [ text "Projects" ]
        div [ Attr.Class "sidebar-label" ] [ text "CURRENT PROJECT" ]
        div [ Attr.Class "project-name" ] [ text "◈  Customer experience" ]
        div [ Attr.Class "sidebar-foot" ] [
          span [ Attr.Class "sidebar-foot-icon"; Attr.Create "aria-hidden" "true" ] [ text "✦" ]
          div [] [
            strong [] [ text "A little more focus" ]
            p [] [ text "Sample workspace · Changes are temporary" ]
          ]
        ]
      ]

      div [ Attr.Class "main"; Attr.Create "role" "main" ] [
        header [ Attr.Class "topbar" ] [
          span [] [ text "Projects / Customer experience" ]
          div [ Attr.Class "header-actions" ] [
            span [ Attr.Class "sample" ] [ text "Sample data" ]
            themeToggle
          ]
        ]

        div [ Attr.Class "content" ] [
          div [ Attr.Class "project-hero" ] [
            div [ Attr.Class "heading" ] [
              div [] [
                div [ Attr.Class "eyebrow" ] [ text "PROJECT WORKSPACE" ]
                h1 [] [ text "Customer experience" ]
                p [ Attr.Class "subtitle" ] [ text "A calmer, clearer first experience for every customer." ]
              ]

              Button.primary (
                text "+ New issue",
                onClick = (fun () -> Var.Set adding true),
                attrs = [ Button.Variant.filled ]
              )
            ]

            div [ Attr.Class "project-summary" ] [
              span [ Attr.Class "pill status-active" ] [ text "In progress" ]
              span [] [ text "Team Orbit" ]
              span [] [ text "Target: this quarter" ]
            ]

            issues.View
            |> Doc.BindView(fun all ->
              let total = List.length all

              let doneCount =
                all |> List.filter (fun issue -> issue.Status = "Done") |> List.length

              let percent = if total = 0 then 0 else doneCount * 100 / total

              div [ Attr.Class "project-progress" ] [
                div [ Attr.Class "progress-copy" ] [
                  span [ Attr.Class "progress-label" ] [ text "PROJECT PROGRESS" ]
                  strong [] [ text (sprintf "%d%% complete" percent) ]
                  span [ Attr.Class "progress-caption" ] [
                    text (sprintf "%d of %d issues done" doneCount total)
                  ]
                ]

                Progress.Linear.create (
                  value = View.Const(float percent),
                  attrs = [ Attr.Create "aria-label" "Project progress" ]
                )
              ])
          ]

          addForm
          div [ Attr.Class "section-head" ] [
            h2 [] [ text "Issues" ]
            div [ Attr.Class "view-switch"; Attr.Create "aria-label" "Issue view" ] [
              viewMode.View
              |> Doc.BindView(fun mode ->
                div [] [
                  button [
                    Attr.Create "type" "button"
                    Attr.Create "aria-pressed" (if mode = "List" then "true" else "false")
                    on.click (fun _ _ -> Var.Set viewMode "List")
                  ] [ text "List" ]
                  button [
                    Attr.Create "type" "button"
                    Attr.Create "aria-pressed" (if mode = "Board" then "true" else "false")
                    on.click (fun _ _ -> Var.Set viewMode "Board")
                  ] [ text "Board" ]
                ])
            ]
          ]

          div [ Attr.Class "filters" ] [
            TextField.singleLine (
              search,
              labelText = View.Const "Search issues",
              placeholder = View.Const "Search titles",
              attrs = [ TextField.Variant.outlined ]
            )
            select "Status" ("All statuses" :: statuses) statusFilter
            select "Assignee" ("All assignees" :: people) assigneeFilter
          ]

          div [ Attr.Class "workspace-body" ] [ div [ Attr.Class "issue-area" ] [ issueViews ]; details ]
        ]
      ]
      Snackbar.create state.Snackbars
    ]
