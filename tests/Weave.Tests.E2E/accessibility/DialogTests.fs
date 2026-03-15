namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type DialogTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("dialog")

  [<Fact>]
  member this.``dialog action button is focusable``() = task {
    do! this.NavigateTo("dialog")
    let actionButton = this.Page.Locator(".weave-dialog__window .weave-button").First
    do! actionButton.FocusAsync()
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-button", activeClass)
  }

  [<Fact(Skip = "Known gap: Dialog has no Escape handler")>]
  member this.``Escape closes the dialog``() = task {
    do! this.NavigateTo("dialog")
    do! this.Page.Keyboard.PressAsync("Escape")
    let! count = this.Page.Locator(".weave-dialog").CountAsync()
    Assert.Equal(0, count)
  }

  [<Fact(Skip = "Known gap: Dialog has no focus trap")>]
  member this.``Tab does not move focus outside the dialog``() = task {
    do! this.NavigateTo("dialog")
    let actionButton = this.Page.Locator(".weave-dialog__window .weave-button").First
    do! actionButton.FocusAsync()
    // Tab past all dialog elements; focus should wrap back inside
    for _ in 1..10 do
      do! this.Page.Keyboard.PressAsync("Tab")

    let! isInsideDialog =
      this.Page.EvaluateAsync<bool>(
        "() => document.querySelector('.weave-dialog__window').contains(document.activeElement)"
      )

    Assert.True(isInsideDialog, "Focus should remain inside the dialog window")
  }
