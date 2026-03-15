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
    let button = this.Page.Locator(".weave-button-menu__trigger").First
    do! button.FocusAsync()
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact(Skip = "Known gap: ButtonMenu trigger uses clickTapViewGuarded (pointerup) — keyboard Enter fires a synthetic click but not pointerup, so the menu does not open")>]
  member this.``Enter on trigger opens the button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let button = this.Page.Locator(".weave-button-menu__trigger").First
    do! button.FocusAsync()
    do! button.PressAsync("Enter")

    do! this.Expect(this.Page.Locator(".weave-button-menu--open")).ToHaveCountAsync(1)
  }

  [<Fact(Skip = "Known gap: ButtonMenu has no ArrowDown/Up item navigation")>]
  member this.``ArrowDown navigates button menu items``() = task {
    do! this.NavigateTo("buttonmenu")
    let button = this.Page.Locator(".weave-button-menu__trigger").First
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(this.Page.Locator(".weave-button-menu__item").First).ToBeFocusedAsync()
  }

  [<Fact(Skip = "Known gap: ButtonMenu has no Escape handler")>]
  member this.``Escape closes the open button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let button = this.Page.Locator(".weave-button-menu__trigger").First
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("Escape")

    do! this.Expect(this.Page.Locator(".weave-button-menu--open")).ToHaveCountAsync(0)
  }
