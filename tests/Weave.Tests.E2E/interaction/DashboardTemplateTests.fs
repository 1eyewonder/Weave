namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type DashboardTemplateTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``given two pages of transactions when sorting by Created then both follow the selected direction``
    ()
    = task {
    do! this.NavigateTo("dashboard-template")
    do! this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Time period")).ClickAsync()
    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = "Last 90 days")).ClickAsync()

    let table =
      this.Page.GetByRole(AriaRole.Table, PageGetByRoleOptions(Name = "Sample transactions"))

    let created =
      table.GetByRole(AriaRole.Columnheader, LocatorGetByRoleOptions(Name = "Created"))

    let sort = created.GetByRole(AriaRole.Button)
    let dates = table.Locator("tbody tr td:last-child")

    let next =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next page"))

    do! sort.PressAsync("Enter")
    do! this.Expect(created).ToHaveAttributeAsync("aria-sort", "ascending")
    do! this.Expect(dates.First).ToHaveTextAsync("76 days ago")
    do! next.ClickAsync()
    do! this.Expect(dates.Last).ToHaveTextAsync("Today")

    do! sort.PressAsync("Space")
    do! this.Expect(created).ToHaveAttributeAsync("aria-sort", "descending")
    do! this.Expect(dates.First).ToHaveTextAsync("Today")
    do! next.ClickAsync()
    do! this.Expect(dates.Last).ToHaveTextAsync("76 days ago")
  }
