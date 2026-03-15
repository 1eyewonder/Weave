namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type DropdownTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("dropdown")

  [<Fact>]
  member this.``dropdown trigger is focusable``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-button", activeClass)
  }

  [<Fact(Skip = "Known gap: Dropdown trigger uses clickTapViewGuarded (pointerup) — keyboard Enter fires a synthetic click but not pointerup, so the list does not open")>]
  member this.``Enter on trigger opens the dropdown``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Page.Locator(".weave-dropdown__list").WaitForAsync()
    let! count = this.Page.Locator(".weave-dropdown__list").CountAsync()
    Assert.True(count > 0, "Dropdown list should be in DOM after Enter")
  }

  [<Fact(Skip = "Known gap: Dropdown has no ArrowDown/Up item navigation")>]
  member this.``ArrowDown navigates dropdown items``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-dropdown__item", activeClass)
  }

  [<Fact(Skip = "Known gap: Dropdown has no Escape handler")>]
  member this.``Escape closes the open dropdown list``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("Escape")
    let! count = this.Page.Locator(".weave-dropdown__list").CountAsync()
    Assert.Equal(0, count)
  }
