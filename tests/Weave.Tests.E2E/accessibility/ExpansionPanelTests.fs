namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ExpansionPanelTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("expansion-panel")

  [<Fact(Skip = "Known gap: ExpansionPanel header has no tabindex — not reachable by Tab")>]
  member this.``expansion panel header is reachable by Tab``() = task {
    do! this.NavigateTo("expansion-panel")
    do! this.Page.Keyboard.PressAsync("Tab")
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-expansion-panel__header", activeClass)
  }

  [<Fact(Skip = "Known gap: ExpansionPanel header is a <div>, not a <button> — Enter/Space do not toggle")>]
  member this.``Enter toggles the expansion panel``() = task {
    do! this.NavigateTo("expansion-panel")
    // The second panel starts collapsed
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(1)
    do! header.FocusAsync()
    do! header.PressAsync("Enter")

    let! isExpanded =
      this.Page.EvaluateAsync<bool>(
        "() => document.querySelectorAll('.weave-expansion-panel')[1].classList.contains('weave-expansion-panel--expanded')"
      )

    Assert.True(isExpanded, "Panel should be expanded after pressing Enter on its header")
  }
