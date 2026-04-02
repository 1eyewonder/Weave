namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type DropdownMenuInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``dropdown menu trigger is focusable``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter on trigger opens the dropdown menu``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Space on trigger opens the dropdown menu``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync(" ")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Enter on trigger toggles the dropdown menu closed``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``ArrowDown navigates dropdown menu items``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__item").First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowUp navigates to previous dropdown menu item``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    let items = this.Page.Locator(".weave-dropdownmenu__item")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.Nth(1)).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Escape closes the open dropdown menu list``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Escape returns focus to the trigger``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Home focuses the first dropdown menu item``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    let items = this.Page.Locator(".weave-dropdownmenu__item")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.Nth(1)).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Home")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``End focuses the last dropdown menu item``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    let items = this.Page.Locator(".weave-dropdownmenu__item")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("End")
    // The last item in the test page is disabled, so End lands on the last enabled item (Nth(1))
    do! this.Expect(items.Nth(1)).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Tab from item closes the dropdown menu``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__item").First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``arrow navigation does not close the dropdown menu``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
  }
