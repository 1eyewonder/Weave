namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type SelectTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("select")

  [<Fact>]
  member this.``select combobox is focusable``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! this.Expect(combobox).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter opens the select dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeVisibleAsync()
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-expanded", "true")
  }

  [<Fact>]
  member this.``ArrowDown highlights the first item``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-activedescendant", Regex(".+"))
  }

  [<Fact>]
  member this.``Enter selects highlighted item and closes dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-activedescendant", Regex(".+"))
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeHiddenAsync()
    do! this.Expect(combobox).Not.ToHaveAttributeAsync("aria-expanded", "true")
  }

  [<Fact>]
  member this.``Escape closes the open dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeHiddenAsync()
    do! this.Expect(combobox).Not.ToHaveAttributeAsync("aria-expanded", "true")
  }

  [<Fact>]
  member this.``Tab closes the open dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(this.Page.Locator(".weave-select__popover")).ToBeHiddenAsync()
    do! this.Expect(combobox).Not.ToHaveAttributeAsync("aria-expanded", "true")
  }
