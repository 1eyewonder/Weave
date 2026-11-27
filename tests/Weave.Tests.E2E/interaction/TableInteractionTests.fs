namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

type TableInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Rows =
    this.Page.Locator("[data-testid='clickable-table'] tbody .weave-table__row")

  member private this.FirstCellText(index: int) =
    this.Rows.Nth(index).Locator("td").First

  [<Fact>]
  member this.``create overloads preserve positional callbacks and combine labels with header content``() =
    task {
      do! this.NavigateTo("table")
      let legacy = this.Page.GetByTestId("positional-callback-table")

      do!
        legacy.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Alpha", Exact = true)).ClickAsync()

      do! this.Expect(this.Page.GetByTestId("clicked-row")).ToHaveTextAsync("Alpha")
      let combined = this.Page.GetByTestId("combined-overload-table")
      do! this.Expect(combined.Locator("thead #combined-header-id")).ToHaveCountAsync(1)
      do! this.Expect(combined.Locator("tbody #combined-header-id")).ToHaveCountAsync(0)

      do!
        this
          .Expect(
            combined.GetByRole(
              AriaRole.Checkbox,
              LocatorGetByRoleOptions(Name = "Select row Named Alpha", Exact = true)
            )
          )
          .ToHaveCountAsync(1)
    }

  [<Fact>]
  member this.``clickable row exposes a button in its first cell``() = task {
    do! this.NavigateTo("table")

    let action =
      this.Rows.First.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Beta"))

    do! this.Expect(action).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Enter on the row button activates the row``() = task {
    do! this.NavigateTo("table")

    let action = this.Rows.Nth(1).Locator(".weave-table__row-action")

    do! action.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("clicked-row")).ToHaveTextAsync("Alpha")
  }

  [<Fact>]
  member this.``clicking elsewhere in a row activates it``() = task {
    do! this.NavigateTo("table")

    do! this.Rows.Nth(2).Locator("td").Nth(1).ClickAsync()
    do! this.Expect(this.Page.GetByTestId("clicked-row")).ToHaveTextAsync("Gamma")
  }

  [<Fact>]
  member this.``activated row is marked as current``() = task {
    do! this.NavigateTo("table")

    do! this.Rows.Nth(2).ClickAsync()
    do! this.Expect(this.Rows.Nth(2)).ToHaveAttributeAsync("aria-current", "true")
    do! this.Expect(this.Rows.Nth(0)).Not.ToHaveAttributeAsync("aria-current", "true")
  }

  [<Fact>]
  member this.``empty table shows empty content``() = task {
    do! this.NavigateTo("table")

    do!
      this
        .Expect(this.Page.Locator("[data-testid='empty-table'] .weave-table__empty"))
        .ToHaveTextAsync("No rows")
  }

  [<Fact>]
  member this.``header cells use column scope``() = task {
    do! this.NavigateTo("table")
    let header = this.Page.Locator("[data-testid='clickable-table'] th").First
    do! this.Expect(header).ToHaveAttributeAsync("scope", "col")
  }

  [<Fact>]
  member this.``sortable header starts unsorted``() = task {
    do! this.NavigateTo("table")
    let header = this.Page.Locator("[data-testid='clickable-table'] th").First
    do! this.Expect(header).ToHaveAttributeAsync("aria-sort", "none")
    do! this.Expect(this.FirstCellText(0)).ToHaveTextAsync("Beta")
  }

  [<Fact>]
  member this.``activating a sortable header sorts ascending``() = task {
    do! this.NavigateTo("table")
    let header = this.Page.Locator("[data-testid='clickable-table'] th").First

    do! header.Locator(".weave-table__sort-button").ClickAsync()
    do! this.Expect(header).ToHaveAttributeAsync("aria-sort", "ascending")
    do! this.Expect(this.FirstCellText(0)).ToHaveTextAsync("Alpha")
  }

  [<Fact>]
  member this.``activating a sortable header twice sorts descending``() = task {
    do! this.NavigateTo("table")
    let header = this.Page.Locator("[data-testid='clickable-table'] th").First
    let sortButton = header.Locator(".weave-table__sort-button")

    do! sortButton.ClickAsync()
    do! sortButton.ClickAsync()
    do! this.Expect(header).ToHaveAttributeAsync("aria-sort", "descending")
    do! this.Expect(this.FirstCellText(0)).ToHaveTextAsync("Gamma")
  }

  [<Fact>]
  member this.``sticky header stays at the top while the body scrolls``() = task {
    do! this.NavigateTo("table")
    let table = this.Page.GetByTestId("sticky-table")
    let header = table.Locator("th").First

    let! before = header.BoundingBoxAsync()
    let! _ = table.EvaluateAsync("el => { el.scrollTop = el.scrollHeight }")
    let! after = header.BoundingBoxAsync()

    Assert.Equal(before.Y, after.Y, 1)
  }

  [<Fact>]
  member this.``caption names the table``() = task {
    do! this.NavigateTo("table")

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Table, PageGetByRoleOptions(Name = "Clickable rows")))
        .ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``hidden caption still names the table``() = task {
    do! this.NavigateTo("table")

    let table =
      this.Page.GetByRole(AriaRole.Table, PageGetByRoleOptions(Name = "Selectable rows"))

    do! this.Expect(table).ToHaveCountAsync(1)
    do! this.Expect(table.Locator("caption")).ToHaveCSSAsync("width", "1px")
  }

  [<Fact>]
  member this.``custom stripe color tints alternate rows``() = task {
    do! this.NavigateTo("table")

    do! this.Expect(this.Rows.Nth(1)).ToHaveCSSAsync("background-color", "rgb(255, 0, 0)")
  }

  [<Fact>]
  member this.``custom hover color highlights the row under the pointer``() = task {
    do! this.NavigateTo("table")

    do! this.Rows.Nth(0).HoverAsync()
    do! this.Expect(this.Rows.Nth(0)).ToHaveCSSAsync("background-color", "rgb(0, 0, 255)")
  }

  [<Fact>]
  member this.``row header column renders row header cells``() = task {
    do! this.NavigateTo("table")

    let header =
      this.Page
        .GetByTestId("select-table")
        .GetByRole(AriaRole.Rowheader, LocatorGetByRoleOptions(Name = "Beta"))

    do! this.Expect(header).ToHaveAttributeAsync("scope", "row")
  }

  [<Fact>]
  member this.``each row checkbox has a name even without a row header``() = task {
    do! this.NavigateTo("table")
    let table = this.Page.GetByTestId("current-and-checked-table")

    for name in [ "Alpha"; "Beta"; "Gamma" ] do
      do!
        this
          .Expect(
            table.GetByRole(
              AriaRole.Checkbox,
              LocatorGetByRoleOptions(Name = "Select row " + name, Exact = true)
            )
          )
          .ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``column attributes remain on body cells while row names stay distinct``() = task {
    do! this.NavigateTo("table")
    let table = this.Page.GetByTestId("select-table")

    for key in [ "name"; "count" ] do
      do! this.Expect(table.Locator("thead #select-" + key)).ToHaveCountAsync(1)

    do! this.Expect(table.Locator("tbody [id='select-count']")).ToHaveCountAsync(3)
    do! this.Expect(table.Locator("tbody .selection-column")).ToHaveCountAsync(6)
    do! this.Page.GetByText("Update column ID").ClickAsync()
    do! this.Expect(table.Locator("thead #updated-name")).ToHaveCountAsync(1)

    for name in [ "Alpha"; "Beta"; "Gamma" ] do
      let checkbox =
        table.GetByRole(AriaRole.Checkbox, LocatorGetByRoleOptions(Name = name, Exact = true))

      do! this.Expect(checkbox).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``header content IDs remain unique and reactive without changing shared cells``() = task {
    do! this.NavigateTo("table")
    let table = this.Page.GetByTestId("header-attributes-table")
    do! this.Expect(table.Locator("thead #scoped-name")).ToHaveCountAsync(1)
    do! this.Expect(table.Locator("thead #scoped-count")).ToHaveCountAsync(1)
    do! this.Expect(table.Locator("tbody #scoped-name, tbody #scoped-count")).ToHaveCountAsync(0)
    do! this.Expect(table.Locator("thead .shared-cell")).ToHaveCountAsync(2)
    do! this.Expect(table.Locator("tbody .shared-cell")).ToHaveCountAsync(6)
    do! this.Page.GetByText("Update scoped ID").ClickAsync()
    do! this.Expect(table.Locator("thead #new-scoped-name")).ToHaveCountAsync(1)
    do! this.Expect(table.Locator("#scoped-name, tbody #new-scoped-name")).ToHaveCountAsync(0)

    for name in [ "Alpha"; "Beta"; "Gamma" ] do
      do!
        this
          .Expect(table.GetByRole(AriaRole.Checkbox, LocatorGetByRoleOptions(Name = name, Exact = true)))
          .ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``custom row labels avoid opaque keys and fall back to the row header``() = task {
    do! this.NavigateTo("table")
    let table = this.Page.GetByTestId("named-selection-table")

    let named =
      table.GetByRole(
        AriaRole.Checkbox,
        LocatorGetByRoleOptions(Name = "Select row Beta: 2 items", Exact = true)
      )

    do! this.Expect(named).ToHaveCountAsync(1)

    do!
      this
        .Expect(table.GetByRole(AriaRole.Checkbox, LocatorGetByRoleOptions(Name = "Gamma", Exact = true)))
        .ToHaveCountAsync(1)

    do!
      this
        .Expect(
          table.GetByRole(
            AriaRole.Checkbox,
            LocatorGetByRoleOptions(Name = "Select row opaque-Beta", Exact = true)
          )
        )
        .ToHaveCountAsync(0)

    do! named.FocusAsync()
    do! named.PressAsync("Space")
    do! this.Expect(named).ToBeCheckedAsync()
    let fallback = this.Page.GetByTestId("empty-selection-labels")

    do!
      this
        .Expect(
          fallback.GetByRole(
            AriaRole.Checkbox,
            LocatorGetByRoleOptions(Name = "Select row Named Beta", Exact = true)
          )
        )
        .ToHaveCountAsync(1)

    do!
      this
        .Expect(
          fallback.GetByRole(
            AriaRole.Checkbox,
            LocatorGetByRoleOptions(Name = "Select row opaque-Gamma", Exact = true)
          )
        )
        .ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``row checkbox is named by its row header``() = task {
    do! this.NavigateTo("table")

    let checkbox =
      this.Page
        .GetByTestId("select-table")
        .GetByRole(AriaRole.Checkbox, LocatorGetByRoleOptions(Name = "Gamma"))

    do! checkbox.FocusAsync()
    do! checkbox.PressAsync("Space")
    do! this.Expect(this.Page.GetByTestId("checked-rows")).ToHaveTextAsync("Alpha,Gamma")
  }

  [<Fact>]
  member this.``select all checkbox is mixed when some rows are selected``() = task {
    do! this.NavigateTo("table")

    let selectAll =
      this.Page
        .GetByTestId("select-table")
        .GetByRole(AriaRole.Checkbox, LocatorGetByRoleOptions(Name = "Select all rows"))

    do! this.Expect(selectAll).ToHaveJSPropertyAsync("indeterminate", true)
  }

  [<Fact>]
  member this.``select all checkbox selects every row then clears them``() = task {
    do! this.NavigateTo("table")

    let selectAll =
      this.Page
        .GetByTestId("select-table")
        .GetByRole(AriaRole.Checkbox, LocatorGetByRoleOptions(Name = "Select all rows"))

    do! selectAll.FocusAsync()
    do! selectAll.PressAsync("Space")
    do! this.Expect(this.Page.GetByTestId("checked-rows")).ToHaveTextAsync("Alpha,Beta,Gamma")
    do! this.Expect(selectAll).ToBeCheckedAsync()

    do! this.Page.GetByTestId("select-table").Locator("thead .weave-checkbox__span").ClickAsync()

    do! this.Expect(this.Page.GetByTestId("checked-rows")).ToHaveTextAsync("")
  }

  [<Fact>]
  member this.``supplied sort state shows as the initial sort``() = task {
    do! this.NavigateTo("table")
    let header = this.Page.GetByTestId("external-table").Locator("th").Nth(1)

    do! this.Expect(header).ToHaveAttributeAsync("aria-sort", "descending")

    do!
      this.Expect(this.Page.GetByTestId("external-table").Locator("tbody td").First).ToHaveTextAsync("Alpha")
  }

  [<Fact>]
  member this.``external sort updates the sort state without reordering rows itself``() = task {
    do! this.NavigateTo("table")
    let table = this.Page.GetByTestId("external-table")

    do! table.Locator(".weave-table__sort-button").ClickAsync()
    do! this.Expect(this.Page.GetByTestId("external-sort")).ToHaveTextAsync("unsorted")
    do! this.Expect(table.Locator("tbody td").First).ToHaveTextAsync("Beta")
  }

  [<Fact>]
  member this.``current row stays highlighted when checkboxes are also shown``() = task {
    do! this.NavigateTo("table")

    let row =
      this.Page.GetByTestId("current-and-checked-table").Locator("tbody .weave-table__row").Nth(1)

    let checkbox = row.Locator(".weave-checkbox__input")

    do! row.Locator("td").Last.ClickAsync()
    do! checkbox.FocusAsync()
    do! checkbox.PressAsync("Space")
    do! checkbox.PressAsync("Space")
    do! this.Expect(checkbox).Not.ToBeCheckedAsync()
    do! this.Expect(row).ToHaveAttributeAsync("aria-current", "true")
    do! this.Expect(row).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-table__row--selected"))
  }
