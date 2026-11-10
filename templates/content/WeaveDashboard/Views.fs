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
  let rec private grouped amount =
    if amount < 1000 then
      string amount
    else
      sprintf "%s,%03d" (grouped (amount / 1000)) (amount % 1000)

  let private money cents =
    sprintf "$%s.%02d" (grouped (cents / 100)) (cents % 100)

  let private whenLabel days =
    if days = 0 then "Today"
    elif days = 1 then "Yesterday"
    else sprintf "%d days ago" days

  let private statusClass status =
    match status with
    | "Succeeded" -> "success"
    | "Pending" -> "pending"
    | _ -> "failed"

  let private options values =
    values
    |> List.map (fun value -> SelectItem.create (text value, value, value))
    |> View.Const

  let private select label values (selected: Var<string option>) =
    Select.create (
      options values,
      selected,
      labelText = View.Const label,
      attrs = [ Select.Variant.outlined; Density.compact ]
    )

  let private statusPill status =
    span [ Attr.Class("status status-" + statusClass status) ] [
      span [ Attr.Class "status-dot"; Attr.Create "aria-hidden" "true" ] []
      text status
    ]

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

  let metrics (state: State.Model) =
    let ranged = state.Ranged

    let metrics =
      ranged
      |> Doc.BindView(fun items ->
        let succeeded = items |> List.filter (fun item -> item.Status = "Succeeded")
        let total = succeeded |> List.sumBy (fun item -> item.Amount)
        let count = List.length items
        let rate = if count = 0 then 0 else List.length succeeded * 100 / count

        div [ Attr.Class "metrics" ] [
          div [ Attr.Class "metric metric-primary" ] [
            div [ Attr.Class "metric-top" ] [
              span [] [ text "Successful volume" ]
              span [ Attr.Class "metric-icon"; Attr.Create "aria-hidden" "true" ] [ text "↗" ]
            ]
            strong [ Attr.Class "metric-value" ] [ text (money total) ]
            div [ Attr.Class "metric-note" ] [
              span [ Attr.Class "metric-dot" ] []
              text "Succeeded payments in selected period"
            ]
          ]

          div [ Attr.Class "metric" ] [
            div [ Attr.Class "metric-top" ] [
              span [] [ text "Payments" ]
              span [ Attr.Class "metric-icon metric-icon-teal"; Attr.Create "aria-hidden" "true" ] [
                text "◈"
              ]
            ]
            strong [ Attr.Class "metric-value" ] [ text (string count) ]
            div [ Attr.Class "metric-note" ] [ text "All payment statuses" ]
          ]

          div [ Attr.Class "metric" ] [
            div [ Attr.Class "metric-top" ] [
              span [] [ text "Success rate" ]
              span [ Attr.Class "metric-icon metric-icon-rose"; Attr.Create "aria-hidden" "true" ] [
                text "✦"
              ]
            ]
            strong [ Attr.Class "metric-value" ] [ text (sprintf "%d%%" rate) ]
            Progress.Linear.create (
              value = View.Const(float rate),
              attrs = [ Attr.Create "aria-label" "Payment success rate" ]
            )
            div [ Attr.Class "metric-note" ] [
              text (sprintf "%d of %d payments succeeded" (List.length succeeded) count)
            ]
          ]
        ])

    metrics

  let breakdown (state: State.Model) =
    let ranged = state.Ranged

    let breakdown =
      ranged
      |> Doc.BindView(fun items ->
        let total = List.length items

        let count state =
          items |> List.filter (fun item -> item.Status = state) |> List.length

        let bar state =
          let n = count state
          let percent = if total = 0 then 0 else n * 100 / total

          div [ Attr.Class "breakdown-item" ] [
            div [ Attr.Class "breakdown-label" ] [ statusPill state; strong [] [ text (string n) ] ]
            div [ Attr.Class "bar-track"; Attr.Create "aria-hidden" "true" ] [
              div [
                Attr.Class("bar-fill bar-" + statusClass state)
                Attr.Style "width" (sprintf "%d%%" percent)
              ] []
            ]
          ]

        div [ Attr.Class "breakdown" ] [
          div [ Attr.Class "breakdown-heading" ] [
            div [] [
              h2 [] [ text "Payment activity" ]
              p [] [ text "A closer look at your selected period." ]
            ]
            span [ Attr.Class "insight-mark"; Attr.Create "aria-hidden" "true" ] [ text "✦" ]
          ]

          div [ Attr.Class "breakdown-grid" ] [ bar "Succeeded"; bar "Pending"; bar "Failed" ]
        ])

    breakdown

  let transactionTable (state: State.Model) =
    let selected = state.Selected
    let visible = state.Visible
    let sort = state.Sort
    let query = state.Search.View |> View.Map(fun value -> value.Trim())

    let columns = [
      TableColumn.create (
        "customer",
        text "Customer",
        (fun (item: Transaction) ->
          div [ Attr.Class "customer-cell" ] [
            span [ Attr.Class "customer-avatar"; Attr.Create "aria-hidden" "true" ] [
              text (item.Customer.Substring(0, 1))
            ]
            span [ Attr.Class "customer-copy" ] [
              strong [] [
                Highlighter.create (View.Const item.Customer, query, attrs = [ Highlighter.Color.primary ])
              ]
              span [] [ text item.Email ]
            ]
          ]),
        sort = Table.ColumnSort.by (fun item -> item.Customer)
      )
      TableColumn.create (
        "amount",
        text "Amount",
        (fun (item: Transaction) -> span [ Attr.Class "amount" ] [ text (money item.Amount) ]),
        sort = Table.ColumnSort.by (fun item -> item.Amount)
      )
      TableColumn.create ("status", text "Status", (fun (item: Transaction) -> statusPill item.Status))
      TableColumn.create (
        "reference",
        text "Reference",
        (fun (item: Transaction) ->
          span [ Attr.Class "reference" ] [
            Highlighter.create (View.Const item.Reference, query, attrs = [ Highlighter.Color.primary ])
          ])
      )
      TableColumn.create (
        "date",
        text "Created",
        (fun (item: Transaction) -> text (whenLabel item.DaysAgo)),
        sort = Table.ColumnSort.by (fun item -> item.DaysAgo)
      )
    ]

    let sorted =
      (visible, sort.View)
      ||> View.Map2(fun items currentSort -> Table.sortRows columns currentSort items)

    let pageItems =
      (sorted, state.Page.View)
      ||> View.Map2(fun items currentPage ->
        let start = min items.Length ((max 1 currentPage - 1) * State.pageSize)
        items |> List.skip start |> List.truncate State.pageSize)

    sort.View |> View.Sink(fun _ -> Var.Set state.Page 1)

    let transactionTable =
      Table.create (
        columns,
        pageItems,
        (fun item -> item.Reference),
        onRowClick = (fun item -> Var.Set selected (Some item)),
        sort = sort,
        rowHeader = "customer",
        caption = text "Sample transactions",
        emptyContent =
          div [ Attr.Class "empty" ] [
            text "No transactions match these filters. Try a different search or status."
          ],
        attrs = [ Table.hiddenCaption; Table.hoverable ]
      )

    transactionTable

  let details (state: State.Model) =
    let selected = state.Selected

    let details =
      selected.View
      |> Doc.BindView(function
        | None -> Doc.Empty
        | Some item ->
          aside [ Attr.Class "detail"; Attr.Create "aria-label" "Transaction details" ] [
            div [ Attr.Class "detail-top" ] [
              div [] [
                div [ Attr.Class "eyebrow" ] [ text "TRANSACTION DETAILS" ]
                h2 [] [ text item.Reference ]
              ]

              button [
                Attr.Class "close-detail"
                Attr.Create "type" "button"
                on.click (fun _ _ -> Var.Set selected None)
              ] [ text "Close" ]
            ]

            div [ Attr.Class "detail-amount" ] [ text (money item.Amount) ]
            statusPill item.Status
            div [ Attr.Class "detail-rule" ] []
            div [ Attr.Class "detail-pair" ] [ span [] [ text "Customer" ]; strong [] [ text item.Customer ] ]
            div [ Attr.Class "detail-pair" ] [ span [] [ text "Email" ]; strong [] [ text item.Email ] ]
            div [ Attr.Class "detail-pair" ] [ span [] [ text "Method" ]; strong [] [ text item.Method ] ]
            div [ Attr.Class "detail-pair" ] [
              span [] [ text "Created" ]
              strong [] [ text (whenLabel item.DaysAgo) ]
            ]
            p [ Attr.Class "detail-note" ] [ text "This is fictional sample data; no payment was processed." ]
          ])

    details

  let render (state: State.Model) =
    let range = state.Range
    let status = state.Status
    let search = state.Search
    let themeToggle = themeToggle ()
    let metrics = metrics state
    let breakdown = breakdown state
    let transactionTable = transactionTable state
    let details = details state

    div [ Attr.Class "dashboard" ] [
      aside [ Attr.Class "sidebar"; Attr.Create "aria-label" "Workspace" ] [
        div [ Attr.Class "brand" ] [
          span [ Attr.Class "brand-mark" ] [ text "A" ]
          div [] [ strong [] [ text "Arcway" ]; span [] [ text "BUSINESS WORKSPACE" ] ]
        ]

        div [ Attr.Class "sidebar-label" ] [ text "WORKSPACE" ]
        div [ Attr.Class "sidebar-active" ] [
          span [ Attr.Create "aria-hidden" "true" ] [ text "▦" ]
          text "Overview"
        ]

        div [ Attr.Class "sidebar-context" ] [
          span [ Attr.Class "context-icon"; Attr.Create "aria-hidden" "true" ] [ text "✦" ]
          strong [] [ text "A clearer view" ]
          p [] [ text "All the details you need, in one thoughtful space." ]
        ]

        div [ Attr.Class "sidebar-foot" ] [
          span [ Attr.Class "account-avatar" ] [ text "A" ]
          div [] [ strong [] [ text "Arcway Studio" ]; span [] [ text "Sample workspace" ] ]
        ]
      ]

      div [ Attr.Class "main"; Attr.Create "role" "main" ] [
        header [ Attr.Class "topbar" ] [
          div [ Attr.Class "topbar-title" ] [
            span [ Attr.Class "mobile-brand" ] [ text "A" ]
            span [] [ text "Workspace" ]
            span [ Attr.Class "slash" ] [ text "/" ]
            strong [] [ text "Overview" ]
          ]

          div [ Attr.Class "header-actions" ] [
            span [ Attr.Class "sample-badge" ] [ text "Sample data" ]
            themeToggle
          ]
        ]

        div [ Attr.Class "content" ] [
          div [ Attr.Class "welcome" ] [
            div [] [
              div [ Attr.Class "eyebrow" ] [
                span [ Attr.Class "welcome-spark" ] [ text "✦" ]
                text " YOUR BUSINESS AT A GLANCE"
              ]
              h1 [] [ text "Welcome back, Alex" ]
              p [] [ text "Here's a little clarity for your day." ]
            ]

            div [ Attr.Class "range-control" ] [
              select "Time period" [ "Last 7 days"; "Last 30 days"; "Last 90 days" ] range
            ]
          ]

          metrics
          breakdown
          div [ Attr.Class "transactions-header" ] [
            div [] [
              h2 [] [ text "Transactions" ]
              p [] [ text "Explore the payments in your selected period." ]
            ]
            span [ Attr.Class "sample-note" ] [ text "FICTIONAL DATA" ]
          ]

          div [ Attr.Class "filter-row" ] [
            TextField.singleLine (
              search,
              labelText = View.Const "Search transactions",
              placeholder = View.Const "Search customer or reference",
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            )

            div [ Attr.Class "status-filter" ] [
              select "Status" [ "All statuses"; "Succeeded"; "Pending"; "Failed" ] status
            ]
          ]
          p [ Attr.Class "table-scroll-hint" ] [ text "Scroll horizontally to see more columns." ]
          div [ Attr.Class "results-layout" ] [
            div [ Attr.Class "table-panel" ] [
              transactionTable
              state.PageCount
              |> Doc.BindView(fun count ->
                if count <= 1 then
                  Doc.Empty
                else
                  div [ Attr.Class "table-pagination" ] [
                    Pagination.create (
                      View.Const count,
                      selectedPage = state.Page,
                      attrs = [ Attr.Create "aria-label" "Transaction pages" ]
                    )
                  ])
            ]
            details
          ]

          div [ Attr.Class "footer-note" ] [
            span [] [ text "A thoughtful start, made with Weave." ]
            span [] [ text "Sample data only · No real payments" ]
          ]
        ]
      ]
    ]
