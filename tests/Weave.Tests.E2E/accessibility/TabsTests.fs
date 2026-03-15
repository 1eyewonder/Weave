namespace Weave.Tests.E2E

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
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-tabs__tab", activeClass)
  }

  [<Fact(Skip = "Known gap: Tab buttons use clickTapView (pointerup) — keyboard Enter fires a synthetic click but not pointerup, so the tab does not activate")>]
  member this.``Enter activates a tab``() = task {
    do! this.NavigateTo("tabs")
    // Focus the second tab (not active by default) and activate it with Enter
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => document.querySelectorAll('.weave-tabs__tab')[1].classList.contains('weave-tabs__tab--active')"
      )

    let! hasActiveClass =
      secondTab.EvaluateAsync<bool>("el => el.classList.contains('weave-tabs__tab--active')")

    Assert.True(hasActiveClass, "Second tab should be active after pressing Enter")
  }

  [<Fact(Skip = "Known gap: Tabs has no ArrowLeft/ArrowRight navigation")>]
  member this.``ArrowRight moves focus to the next tab``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! firstTab.FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-tabs__tab", activeClass)

    let! isFirst =
      this.Page.EvaluateAsync<bool>(
        "() => document.activeElement === document.querySelectorAll('.weave-tabs__tab')[0]"
      )

    Assert.False(isFirst, "Focus should have moved away from the first tab")
  }
