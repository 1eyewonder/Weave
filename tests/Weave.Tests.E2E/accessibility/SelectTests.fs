namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type SelectTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("select")

  [<Fact>]
  member this.``select combobox is focusable``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-select__value", activeClass)
  }

  [<Fact>]
  member this.``Enter opens the select dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    // WebSharper reactive DOM updates are async — wait for the popover to appear
    let! _ = this.Page.WaitForSelectorAsync(".weave-select__popover")
    let! expanded = combobox.GetAttributeAsync("aria-expanded")
    Assert.Equal("true", expanded)
  }

  [<Fact>]
  member this.``ArrowDown highlights the first item``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    let! _ = this.Page.WaitForSelectorAsync(".weave-select__popover")
    do! this.Page.Keyboard.PressAsync("ArrowDown")

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => (document.querySelector('.weave-select__value')?.getAttribute('aria-activedescendant') ?? '') !== ''"
      )

    let! activedescendant = combobox.GetAttributeAsync("aria-activedescendant")

    Assert.False(
      System.String.IsNullOrEmpty(activedescendant),
      "aria-activedescendant should be set after ArrowDown"
    )
  }

  [<Fact>]
  member this.``Enter selects highlighted item and closes dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    let! _ = this.Page.WaitForSelectorAsync(".weave-select__popover")
    do! this.Page.Keyboard.PressAsync("ArrowDown")

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => (document.querySelector('.weave-select__value')?.getAttribute('aria-activedescendant') ?? '') !== ''"
      )

    do! this.Page.Keyboard.PressAsync("Enter")

    let! _ =
      this.Page.WaitForSelectorAsync(
        ".weave-select__popover",
        Microsoft.Playwright.PageWaitForSelectorOptions(
          State = Microsoft.Playwright.WaitForSelectorState.Detached
        )
      )

    let! expanded = combobox.GetAttributeAsync("aria-expanded")

    Assert.True(
      expanded <> "true",
      $"Dropdown should be closed after selecting an item but aria-expanded was: {expanded}"
    )
  }

  [<Fact>]
  member this.``Escape closes the open dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    let! _ = this.Page.WaitForSelectorAsync(".weave-select__popover")
    do! this.Page.Keyboard.PressAsync("Escape")

    let! _ =
      this.Page.WaitForSelectorAsync(
        ".weave-select__popover",
        Microsoft.Playwright.PageWaitForSelectorOptions(
          State = Microsoft.Playwright.WaitForSelectorState.Detached
        )
      )

    let! expanded = combobox.GetAttributeAsync("aria-expanded")

    Assert.True(
      expanded <> "true",
      $"Dropdown should be closed after Escape but aria-expanded was: {expanded}"
    )
  }

  [<Fact>]
  member this.``Tab closes the open dropdown``() = task {
    do! this.NavigateTo("select")
    let combobox = this.Page.Locator(".weave-select__value").First
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    let! _ = this.Page.WaitForSelectorAsync(".weave-select__popover")
    do! this.Page.Keyboard.PressAsync("Tab")

    let! _ =
      this.Page.WaitForSelectorAsync(
        ".weave-select__popover",
        Microsoft.Playwright.PageWaitForSelectorOptions(
          State = Microsoft.Playwright.WaitForSelectorState.Detached
        )
      )

    let! expanded = combobox.GetAttributeAsync("aria-expanded")
    Assert.True(expanded <> "true", $"Dropdown should be closed after Tab but aria-expanded was: {expanded}")
  }
