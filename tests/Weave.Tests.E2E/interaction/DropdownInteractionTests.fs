namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type DropdownInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``dropdown trigger is focusable``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter on trigger opens the dropdown``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Space on trigger opens the dropdown``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync(" ")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Enter on trigger toggles the dropdown closed``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``ArrowDown navigates dropdown items``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(this.Page.Locator(".weave-dropdown__item").First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowUp navigates to previous dropdown item``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    let items = this.Page.Locator(".weave-dropdown__item")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.Nth(1)).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Escape closes the open dropdown list``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Escape returns focus to the trigger``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Home focuses the first dropdown item``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    let items = this.Page.Locator(".weave-dropdown__item")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(items.Nth(1)).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Home")
    do! this.Expect(items.First).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``End focuses the last dropdown item``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    let items = this.Page.Locator(".weave-dropdown__item")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("End")
    do! this.Expect(items.Last).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Tab from item closes the dropdown``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(this.Page.Locator(".weave-dropdown__item").First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``arrow navigation does not close the dropdown``() = task {
    do! this.NavigateTo("dropdown")
    let trigger = this.Page.Locator(".weave-dropdown .weave-button").First
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(this.Page.Locator(".weave-dropdown__list")).ToBeVisibleAsync()
  }
