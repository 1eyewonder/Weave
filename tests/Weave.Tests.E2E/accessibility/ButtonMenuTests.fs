namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ButtonMenuTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("buttonmenu")

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
