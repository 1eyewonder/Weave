namespace Weave.Tests.E2E

open Xunit

[<Collection("E2E")>]
type ListInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``list click selects and nested checkbox does not expand group``() = task {
    do! this.NavigateTo("list")
    let row = this.Page.Locator("[aria-label='Test items'] .weave-list-item").First
    let! _ = row.EvaluateAsync("el => el.click()")
    do! this.Expect(this.Page.GetByTestId("list-selection")).ToHaveTextAsync("1")
    let group = this.Page.GetByTestId("nested-list")

    let! _ =
      group.Locator(".weave-list-item__checkbox").First.EvaluateAsync("el => el.click()")

    do! this.Expect(this.Page.GetByTestId("nested-selected")).ToHaveTextAsync("a,b")
    do! this.Expect(group.Locator(".weave-list-nested")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member _.``touch tap selects a list item``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    let! _ = page.GotoAsync($"%s{fixture.BaseUrl}/#list")
    do! page.Locator("[aria-label='Test items'] .weave-list-item").First.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("list-selection")).ToHaveTextAsync("1")
    let group = page.GetByTestId("nested-list")
    do! group.Locator(".weave-list-item__checkbox").First.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("nested-selected")).ToHaveTextAsync("a,b")
    do! Microsoft.Playwright.Assertions.Expect(group.Locator(".weave-list-nested")).ToHaveCountAsync(0)
  }
