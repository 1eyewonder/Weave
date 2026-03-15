namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ExpansionPanelTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("expansion-panel")

  [<Fact>]
  member this.``expansion panel header is reachable by Tab``() = task {
    do! this.NavigateTo("expansion-panel")
    do! this.Page.Keyboard.PressAsync("Tab")
    let header = this.Page.Locator(".weave-expansion-panel__header").First
    do! this.Expect(header).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter toggles the expansion panel``() = task {
    do! this.NavigateTo("expansion-panel")
    // The second panel starts collapsed
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(1)
    do! header.FocusAsync()
    do! header.PressAsync("Enter")
    let panel = this.Page.Locator(".weave-expansion-panel").Nth(1)
    do! this.Expect(panel).ToHaveClassAsync(Regex("weave-expansion-panel--expanded"))
  }

  [<Fact>]
  member this.``focus color class is present on header with custom focus color``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(2)
    do! this.Expect(header).ToHaveClassAsync(Regex("weave-expansion-panel__header--focus-error"))
  }
