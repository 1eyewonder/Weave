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
  let private options values =
    values
    |> List.map (fun value -> SelectItem.create (text value, value, value))
    |> View.Const

  let private navButton label active action =
    button [
      Attr.Class "tab"
      Attr.Create "type" "button"
      Attr.Create "aria-pressed" (if active then "true" else "false")
      on.click (fun _ _ -> action ())
    ] [ text label ]

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

  // The README is a regular, editable view rather than a rendered Markdown file.
  let private readme selected navigateFolder =
    div [ Attr.Class "readme" ] [
      div [ Attr.Class "readme-head" ] [
        span [ Attr.Class "readme-icon" ] [ text "▤" ]
        strong [] [ text "README.md" ]
      ]

      div [ Attr.Class "readme-body" ] [
        div [ Attr.Class "readme-hero" ] [
          div [ Attr.Class "readme-hero-copy" ] [
            div [ Attr.Class "readme-eyebrow" ] [
              span [ Attr.Class "eyebrow-spark"; Attr.Create "aria-hidden" "true" ] [ text "✦" ]
              text " A LITTLE SPACE TO BUILD BIG THINGS"
            ]
            h2 [] [
              text (
                if selected = Some "preview" then
                  "Northstar preview"
                else
                  "Hello, Northstar."
              )
            ]
            p [] [
              text (
                if selected = Some "preview" then
                  "An early look at the next iteration, shaped by the ideas we are exploring together."
                else
                  "A thoughtful starting point for your next project. Bring the ideas, explore the source, and make something useful together."
              )
            ]

            button [
              Attr.Class "explore-button"
              Attr.Create "type" "button"
              on.click (fun _ _ -> navigateFolder (if selected = Some "preview" then "preview" else "src"))
            ] [
              text (
                if selected = Some "preview" then
                  "Explore ideas  ↗"
                else
                  "Explore the source  ↗"
              )
            ]
          ]

          div [ Attr.Class "orbit-art"; Attr.Create "aria-hidden" "true" ] [
            span [ Attr.Class "orbit-ring orbit-ring-outer" ] []
            span [ Attr.Class "orbit-ring orbit-ring-inner" ] []
            span [ Attr.Class "orbit-core" ] [ text "✦" ]
            span [ Attr.Class "orbit-dot orbit-dot-one" ] []
            span [ Attr.Class "orbit-dot orbit-dot-two" ] []
          ]
        ]

        div [ Attr.Class "readme-section" ] [
          div [ Attr.Class "section-kicker" ] [ text "THE PROJECT AT A GLANCE" ]
          div [ Attr.Class "readme-features" ] [
            div [ Attr.Class "feature" ] [
              span [ Attr.Class "feature-symbol feature-lilac"; Attr.Create "aria-hidden" "true" ] [
                text "◇"
              ]
              strong [] [ text "Make a start" ]
              p [] [ text "Explore a small, approachable F# codebase." ]
            ]

            div [ Attr.Class "feature" ] [
              span [ Attr.Class "feature-symbol feature-peach"; Attr.Create "aria-hidden" "true" ] [
                text "↗"
              ]
              strong [] [ text "Build together" ]
              p [] [ text "Use issues to share ideas and follow progress." ]
            ]

            div [ Attr.Class "feature" ] [
              span [ Attr.Class "feature-symbol feature-mint"; Attr.Create "aria-hidden" "true" ] [ text "✧" ]
              strong [] [ text "Make it yours" ]
              p [] [ text "Replace this sample content with your own story." ]
            ]
          ]
        ]
      ]
    ]

  let private about () =
    aside [ Attr.Class "about"; Attr.Create "aria-label" "About this repository" ] [
      h2 [] [ text "About" ]
      p [] [ text "A little space for good ideas to become useful things." ]
      div [ Attr.Class "topics" ] [
        span [] [ text "starter" ]
        span [] [ text "fsharp" ]
        span [] [ text "community" ]
      ]

      div [ Attr.Class "about-rule" ] []
      div [ Attr.Class "about-stat" ] [ span [] [ text "◈" ]; text " F# · Sample repository" ]
      div [ Attr.Class "about-stat" ] [ span [] [ text "▤" ]; text " README and contributor guide" ]
      div [ Attr.Class "about-note" ] [
        div [ Attr.Class "about-note-symbol"; Attr.Create "aria-hidden" "true" ] [ text "✦" ]
        strong [] [ text "Your next idea starts here" ]
        p [] [ text "Replace the sample files and metadata with your project's own story." ]
      ]

      div [ Attr.Class "about-section-title" ] [ text "WORKSPACE SNAPSHOT" ]
      div [ Attr.Class "snapshot" ] [
        div [] [ strong [] [ text "2" ]; span [] [ text "Branches" ] ]
        div [] [
          strong [] [
            text (string (issues |> List.filter (fun issue -> issue.State = "Open") |> List.length))
          ]
          span [] [ text "Open issues" ]
        ]
      ]
    ]

  let codeView (state: State.Model) =
    let branch = state.Branch
    let folder = state.Folder
    let selectedFile = state.SelectedFile
    let navigateFolder = State.navigateFolder state

    let codeView =
      let current =
        (branch.View, folder.View) ||> View.Map2(fun selected path -> selected, path)

      (current, selectedFile.View)
      ||> View.Map2(fun (selected, path) file -> selected, path, file)
      |> Doc.BindView(fun (selected, path, file) ->
        let entries =
          if selected = Some "preview" then
            previewFiles
          else
            mainFiles

        div [ Attr.Class "code-layout" ] [
          div [ Attr.Class "code-main" ] [
            div [ Attr.Class "repo-toolbar" ] [
              div [ Attr.Class "branch-control" ] [
                Select.create (
                  options [ "main"; "preview" ],
                  branch,
                  labelText = View.Const "Branch",
                  attrs = [ Select.Variant.outlined; Density.compact ]
                )
              ]

              div [ Attr.Class "branch-caption" ] [
                span [ Attr.Class "caption-dot" ] []
                text "2 sample branches"
              ]

              div [ Attr.Class "toolbar-meta" ] [
                span [ Attr.Class "toolbar-meta-dot" ] [ text "◈" ]
                text (
                  sprintf "%d items" ((entries |> List.filter (fun item -> item.Path = path)) |> List.length)
                )
              ]
            ]

            div [
              Attr.Class(
                if path = "" && Option.isNone file then
                  "breadcrumbs root-crumb"
                else
                  "breadcrumbs"
              )
              Attr.Create "aria-label" "Repository path"
            ] [
              button [
                Attr.Class "crumb"
                Attr.Create "type" "button"
                on.click (fun _ _ -> navigateFolder "")
              ] [ text "northstar" ]
              if path <> "" then
                span [ Attr.Class "slash" ] [ text "/" ]

                button [
                  Attr.Class "crumb"
                  Attr.Create "type" "button"
                  on.click (fun _ _ -> navigateFolder path)
                ] [ text path ]
              match file with
              | Some entry ->
                span [ Attr.Class "slash" ] [ text "/" ]
                span [ Attr.Class "crumb-current" ] [ text entry.Name ]
              | None -> ()
            ]
            match file with
            | Some entry ->
              div [ Attr.Class "file-panel" ] [
                div [ Attr.Class "panel-head" ] [
                  strong [] [ text entry.Name ]
                  span [ Attr.Class "muted" ] [ text "Sample file" ]
                ]
                pre [ Attr.Class "file-content" ] [ text entry.Contents ]
              ]
            | None ->
              div [ Attr.Class "file-panel" ] [
                div [ Attr.Class "commit-line" ] [
                  div [ Attr.Class "commit-author" ] [
                    span [ Attr.Class "avatar" ] [ text "M" ]
                    strong [] [ text "Maya Chen" ]
                    span [ Attr.Class "commit-message" ] [
                      text (
                        if selected = Some "preview" then
                          "Explore the next release"
                        else
                          "Organize the starter modules"
                      )
                    ]
                  ]
                  span [ Attr.Class "commit-time" ] [
                    text (if selected = Some "preview" then "today" else "yesterday")
                  ]
                ]

                for entry in entries |> List.filter (fun item -> item.Path = path) do
                  button [
                    Attr.Class "file-row"
                    Attr.Create "type" "button"
                    on.click (fun _ _ ->
                      if entry.Folder then
                        navigateFolder entry.Name
                      else
                        Var.Set selectedFile (Some entry))
                  ] [
                    span [ Attr.Class "file-name" ] [
                      span [
                        Attr.Class(
                          if entry.Folder then
                            "file-icon folder-icon"
                          else
                            "file-icon"
                        )
                        Attr.Create "aria-hidden" "true"
                      ] [ text (if entry.Folder then "▰" else "▤") ]
                      strong [] [ text entry.Name ]
                    ]
                    span [ Attr.Class "file-message" ] [ text entry.Message ]
                    span [ Attr.Class "file-updated" ] [ text entry.Updated ]
                  ]
              ]

              if path = "" then
                readme selected navigateFolder
          ]

          about ()
        ])

    codeView

  let issueView (state: State.Model) =
    let stateFilter = state.StateFilter
    let query = state.Query
    let selectedIssue = state.SelectedIssue

    let issueView =
      let filters =
        (stateFilter.View, query.View) ||> View.Map2(fun state search -> state, search)

      selectedIssue.View
      |> Doc.BindView(fun selected ->
        match selected with
        | Some issue ->
          div [ Attr.Class "issue-detail" ] [
            button [
              Attr.Class "back-link"
              Attr.Create "type" "button"
              on.click (fun _ _ -> Var.Set selectedIssue None)
            ] [ text "← Back to issues" ]
            div [ Attr.Class "detail-heading" ] [
              h2 [] [ text issue.Title ]
              span [ Attr.Class "detail-number" ] [ text (sprintf "#%d" issue.Id) ]
            ]

            div [ Attr.Class "detail-meta" ] [
              span [
                Attr.Class(
                  if issue.State = "Open" then
                    "state-pill state-open"
                  else
                    "state-pill state-closed"
                )
              ] [ text issue.State ]
              text (sprintf "%s opened this sample issue" issue.Author)
            ]

            div [ Attr.Class "comment" ] [
              div [ Attr.Class "comment-head" ] [
                span [ Attr.Class "avatar" ] [ text (issue.Author.Substring(0, 1)) ]
                strong [] [ text issue.Author ]
                span [] [ text " shared an update" ]
              ]
              p [] [ text issue.Description ]
            ]
          ]
        | None ->
          div [ Attr.Class "issues-page" ] [
            div [ Attr.Class "issues-intro" ] [
              div [] [
                h2 [] [ text "Issues" ]
                p [] [ text "Keep the conversation moving, one idea at a time." ]
              ]
              span [ Attr.Class "sample-badge" ] [ text "SAMPLE DATA" ]
            ]

            div [ Attr.Class "issue-filters" ] [
              TextField.singleLine (
                query,
                labelText = View.Const "Search issues",
                placeholder = View.Const "Search by title",
                attrs = [ TextField.Variant.outlined; TextField.Width.full ]
              )

              Select.create (
                options [ "Open"; "Closed"; "All" ],
                stateFilter,
                labelText = View.Const "State",
                attrs = [ Select.Variant.outlined ]
              )
            ]

            filters
            |> Doc.BindView(fun (state, search) ->
              let matching =
                issues
                |> List.filter (fun issue ->
                  (state = Some "All" || state = Some issue.State)
                  && (search.Trim() = "" || issue.Title.ToLower().Contains(search.Trim().ToLower())))

              div [ Attr.Class "issue-list" ] [
                div [ Attr.Class "issue-list-head" ] [
                  strong [] [
                    text (
                      sprintf "%d %s" matching.Length (if matching.Length = 1 then "issue" else "issues")
                    )
                  ]
                  span [] [ text "Filtered sample issues" ]
                ]

                if List.isEmpty matching then
                  div [ Attr.Class "empty" ] [
                    text "No issues match your search. Try another title or state."
                  ]
                for issue in matching do
                  button [
                    Attr.Class "issue-row"
                    Attr.Create "type" "button"
                    on.click (fun _ _ -> Var.Set selectedIssue (Some issue))
                  ] [
                    span [
                      Attr.Class(
                        if issue.State = "Open" then
                          "issue-indicator open"
                        else
                          "issue-indicator closed"
                      )
                      Attr.Create "aria-hidden" "true"
                    ] [ text (if issue.State = "Open" then "◉" else "✓") ]
                    span [ Attr.Class "issue-copy" ] [
                      span [ Attr.Class "issue-title" ] [
                        Highlighter.create (
                          View.Const issue.Title,
                          View.Const(search.Trim()),
                          attrs = [ Highlighter.Color.primary ]
                        )
                      ]
                      span [ Attr.Class "issue-subtitle" ] [
                        text (sprintf "#%d · %s · %s" issue.Id issue.State issue.Author)
                      ]
                    ]
                    span [ Attr.Class "issue-label" ] [ text issue.Label ]
                  ]
              ])
          ])

    issueView

  let render (state: State.Model) =
    let page = state.Page
    let switchPage = State.switchPage state
    let themeToggle = themeToggle ()
    let codeView = codeView state
    let issueView = issueView state

    div [ Attr.Class "app" ] [
      header [ Attr.Class "global-header" ] [
        div [ Attr.Class "global-brand" ] [
          span [ Attr.Class "brand-mark" ] [ text "N" ]
          span [] [ text "Northstar" ]
          span [ Attr.Class "brand-divider" ] [ text "/" ]
          span [ Attr.Class "brand-sub" ] [ text "Workspace" ]
        ]

        div [ Attr.Class "header-actions" ] [
          span [ Attr.Class "demo-note" ] [ text "A sample repository" ]
          themeToggle
        ]
      ]

      div [ Attr.Class "repo-header" ] [
        div [ Attr.Class "repo-title-row" ] [
          div [ Attr.Class "repo-identity" ] [
            div [ Attr.Class "repo-icon"; Attr.Create "aria-hidden" "true" ] [ text "◇" ]
            div [] [
              div [ Attr.Class "repo-owner" ] [ text "STUDIO NORTH / REPOSITORIES" ]
              h1 [] [ text "northstar" ]
            ]
          ]
          span [ Attr.Class "visibility" ] [ text "Public sample" ]
        ]
        p [ Attr.Class "repo-description" ] [
          text "A thoughtful starting point for building something together."
        ]

        div [ Attr.Class "repo-tabs"; Attr.Create "aria-label" "Repository sections" ] [
          page.View
          |> Doc.BindView(fun active ->
            div [] [
              navButton "Code" (active = "Code") (fun () -> switchPage "Code")
              navButton "Issues" (active = "Issues") (fun () -> switchPage "Issues")
            ])
        ]
      ]

      div [ Attr.Class "content"; Attr.Create "role" "main" ] [
        page.View
        |> Doc.BindView(fun active -> if active = "Code" then codeView else issueView)
      ]

      footer [ Attr.Class "footer" ] [
        span [] [ text "NORTHSTAR  /  REPOSITORY WORKSPACE" ]
        span [] [ text "An editable Weave starter · Sample data only" ]
      ]
    ]
