namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ButtonMenuInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``button menu trigger is focusable``() = task {
    do! this.NavigateTo("buttonmenu")
    let button = this.Page.Locator(".weave-button-menu__trigger").First
    do! button.FocusAsync()
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter on trigger opens the button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")

    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
  }

  [<Fact>]
  member this.``Space on trigger opens the button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    do! button.FocusAsync()
    do! button.PressAsync(" ")

    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
  }

  [<Fact>]
  member this.``Enter on trigger toggles the button menu closed``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! button.PressAsync("Enter")
    do! this.Expect(container).Not.ToHaveClassAsync(Regex("weave-button-menu--open"))
  }

  // Default direction is Top, so ArrowUp = next, ArrowDown = prev
  [<Fact>]
  member this.``next key navigates to first menu item (Top direction uses ArrowUp)``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    let items = container.Locator(".weave-button-menu__item")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowUp navigates forward and ArrowDown navigates backward (Top direction)``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    let items = container.Locator(".weave-button-menu__item")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(items.First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(items.Last).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Escape closes the open button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(container).Not.ToHaveClassAsync(Regex("weave-button-menu--open"))
  }

  [<Fact>]
  member this.``Escape returns focus to the trigger``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Home focuses the first menu item``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    let items = container.Locator(".weave-button-menu__item")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(items.Last).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Home")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``End focuses the last menu item``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    let items = container.Locator(".weave-button-menu__item")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("End")
    do! this.Expect(items.Last).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Tab from trigger closes the button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(container).Not.ToHaveClassAsync(Regex("weave-button-menu--open"))
  }

  [<Fact>]
  member this.``arrow navigation does not close the button menu``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let button = container.Locator(".weave-button-menu__trigger")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
  }

  [<Fact>]
  member this.``horizontal menu navigates with ArrowRight and ArrowLeft``() = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator("[data-testid='horizontal-menu']")
    let button = container.Locator(".weave-button-menu__trigger")
    let items = container.Locator(".weave-button-menu__item")
    do! button.FocusAsync()
    do! button.PressAsync("Enter")

    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))

    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(items.First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(items.Last).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowLeft")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("Enter")>]
  [<InlineData("Space")>]
  member this.``keyboard activates the focused menu item``(key: string) = task {
    do! this.NavigateTo("buttonmenu")
    let container = this.Page.Locator(".weave-button-menu").First
    let trigger = container.Locator(".weave-button-menu__trigger")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(container).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(container.Locator(".weave-button-menu__item").First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync(key)
    do! this.Expect(this.Page.GetByTestId("menu-action")).ToHaveTextAsync("Action 1")
  }

  [<Fact>]
  member this.``disabled menu button cannot activate and wrapped link can``() = task {
    do! this.NavigateTo("buttonmenu")
    let menu = this.Page.GetByTestId("edge-menu")
    let trigger = menu.Locator(".weave-button-menu__trigger")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(menu).ToHaveClassAsync(Regex("weave-button-menu--open"))
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    let disabled = menu.Locator("[role=menuitem]").First
    do! this.Expect(disabled).ToBeFocusedAsync()
    do! this.Expect(disabled).ToHaveAccessibleNameAsync("Unavailable")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("menu-action")).ToHaveTextAsync("none")
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    let link = menu.Locator("[role=menuitem]").Last
    do! this.Expect(link).ToBeFocusedAsync()
    do! this.Expect(link).ToHaveAccessibleNameAsync("Wrapped link")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("menu-action")).ToHaveTextAsync("link")
  }
