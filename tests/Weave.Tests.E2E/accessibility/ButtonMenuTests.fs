namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ButtonMenuTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("buttonmenu")

  [<Fact>]
  member this.``button menu trigger is focusable``() = task {
    do! this.NavigateTo("buttonmenu")
    let trigger = this.Page.Locator(".weave-button-menu__trigger")
    do! trigger.FocusAsync()
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-button", activeClass)
  }

  [<Fact(Skip = "Known gap: ButtonMenu trigger uses clickTapViewGuarded (pointerup) — keyboard Enter fires a synthetic click but not pointerup, so the menu does not open")>]
  member this.``Enter on trigger opens the button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let trigger = this.Page.Locator(".weave-button-menu__trigger")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    let! _ = this.Page.WaitForSelectorAsync(".weave-button-menu--open")

    let! hasOpenClass =
      this.Page.EvaluateAsync<bool>(
        "() => document.querySelector('.weave-button-menu').classList.contains('weave-button-menu--open')"
      )

    Assert.True(hasOpenClass, "Button menu should have weave-button-menu--open class after Enter")
  }

  [<Fact(Skip = "Known gap: ButtonMenu has no ArrowDown/Up item navigation")>]
  member this.``ArrowDown navigates button menu items``() = task {
    do! this.NavigateTo("buttonmenu")
    let trigger = this.Page.Locator(".weave-button-menu__trigger")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-button-menu__item", activeClass)
  }

  [<Fact(Skip = "Known gap: ButtonMenu has no Escape handler")>]
  member this.``Escape closes the open button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let trigger = this.Page.Locator(".weave-button-menu__trigger")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("Escape")

    let! hasOpenClass =
      this.Page.EvaluateAsync<bool>(
        "() => document.querySelector('.weave-button-menu').classList.contains('weave-button-menu--open')"
      )

    Assert.False(hasOpenClass, "Button menu should be closed after Escape")
  }
