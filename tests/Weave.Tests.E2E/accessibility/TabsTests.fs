namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type TabsTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("tabs")

  [<Fact>]
  member this.``tab button is focusable``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! firstTab.FocusAsync()
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact(Skip = "Known gap: Tab buttons use clickTapView (pointerup) — keyboard Enter fires a synthetic click but not pointerup, so the tab does not activate")>]
  member this.``Enter activates a tab``() = task {
    do! this.NavigateTo("tabs")
    // Focus the second tab (not active by default) and activate it with Enter
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")
    do! this.Expect(secondTab).ToHaveClassAsync(Regex("weave-tabs__tab--active"))
  }

  [<Fact(Skip = "Known gap: Tabs has no ArrowLeft/ArrowRight navigation")>]
  member this.``ArrowRight moves focus to the next tab``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! firstTab.FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(secondTab).ToBeFocusedAsync()
  }
