namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type SelectInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``programmatic option click selects without toggling parent``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let item = root.Locator("[role='option']").Nth(1)
    do! this.Expect(item).ToBeVisibleAsync()
    let! _ = item.EvaluateAsync("el => el.click()")
    do! this.Expect(combobox).ToContainTextAsync("Alpha")
    let expected = if testId = "keyboard-multi" then "true" else "false"
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-expanded", expected)
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  [<Trait("Capability", "MobileEmulation")>]
  member _.``touch tap selects option without toggling parent``(testId: string) = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    let! _ = page.GotoAsync($"%s{fixture.BaseUrl}/#select")
    let root = page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.TapAsync()
    do! root.Locator("[role='option']").Nth(1).TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(combobox).ToContainTextAsync("Alpha")
    let expected = if testId = "keyboard-multi" then "true" else "false"
    do! Microsoft.Playwright.Assertions.Expect(combobox).ToHaveAttributeAsync("aria-expanded", expected)
  }

  [<Theory>]
  [<InlineData("restricted-single", "toggle-select-enabled", "weave-select--disabled")>]
  [<InlineData("restricted-multi", "toggle-select-enabled", "weave-select--disabled")>]
  [<InlineData("restricted-single", "toggle-select-readonly", "weave-select--readonly")>]
  [<InlineData("restricted-multi", "toggle-select-readonly", "weave-select--readonly")>]
  member this.``disabled and read-only selects ignore keyboard activation``
    (testId: string, toggleId: string, stateClass: string)
    = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    let toggle = this.Page.GetByTestId(toggleId)
    do! toggle.ClickAsync()
    do! this.Expect(root.Locator(".weave-select")).ToHaveClassAsync(Regex(stateClass))
    do! combobox.PressAsync("Enter")
    do! combobox.PressAsync("ArrowDown")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-expanded", "false")
    do! toggle.ClickAsync()
    do! this.Expect(root.Locator(".weave-select")).Not.ToHaveClassAsync(Regex(stateClass))
    do! this.Page.Mouse.MoveAsync(0.0f, 0.0f)
    do! combobox.PressAsync("Enter")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-expanded", "true")
    do! this.Expect(root.GetByLabel("Search options")).ToBeFocusedAsync()
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(4)
    do! root.GetByLabel("Search options").PressAsync("ArrowDown")

    do!
      this
        .Expect(root.Locator("[role='option']").Nth(1))
        .ToHaveClassAsync(Regex("weave-select__item--highlighted"))

    let! _ = toggle.EvaluateAsync("el => el.click()")

    do! this.Expect(root.Locator(".weave-select")).ToHaveClassAsync(Regex(stateClass))

    let! _ =
      root
        .GetByLabel("Search options")
        .EvaluateAsync(
          "el => el.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true }))"
        )

    do! this.Expect(root.Locator("[role='option']").Nth(1)).ToHaveAttributeAsync("aria-selected", "false")
    do! this.Expect(combobox).Not.ToContainTextAsync("Alpha")
  }

  [<Theory>]
  [<InlineData("restricted-single", "toggle-select-enabled", "weave-select--disabled")>]
  [<InlineData("restricted-multi", "toggle-select-enabled", "weave-select--disabled")>]
  [<InlineData("restricted-single", "toggle-select-readonly", "weave-select--readonly")>]
  [<InlineData("restricted-multi", "toggle-select-readonly", "weave-select--readonly")>]
  member this.``disabled and read-only selects cannot be cleared from keyboard``
    (testId: string, toggleId: string, stateClass: string)
    = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(combobox).ToContainTextAsync("Alpha")

    if testId = "restricted-multi" then
      do! this.Page.Keyboard.PressAsync("Escape")

    do! this.Page.GetByTestId(toggleId).ClickAsync()
    do! this.Expect(root.Locator(".weave-select")).ToHaveClassAsync(Regex(stateClass))
    let clear = root.Locator(".weave-select__clear")

    let! _ =
      clear.EvaluateAsync(
        "el => el.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true }))"
      )

    do! this.Expect(combobox).ToContainTextAsync("Alpha")
  }

  [<Fact>]
  member this.``select all updates enabled filtered values and preserves disabled selections``() = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId("select-all-multi")
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let options = root.Locator("[role='option']")
    let selectAll = root.Locator(".weave-select__select-all")
    let checkbox = selectAll.Locator(".weave-select__item-checkbox")
    do! this.Expect(options.Nth(0)).ToHaveAttributeAsync("aria-selected", "false")
    do! options.Nth(1).ClickAsync()
    do! this.Expect(options.Nth(1)).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(checkbox).ToHaveClassAsync(Regex("weave-select__item-checkbox--indeterminate"))
    do! this.Expect(selectAll).ToHaveAttributeAsync("aria-checked", "mixed")
    do! selectAll.ClickAsync()
    do! this.Expect(options.Nth(1)).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(options.Nth(2)).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(options.Nth(3)).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(options.Nth(0)).ToHaveAttributeAsync("aria-selected", "false")
    do! this.Expect(checkbox).ToHaveClassAsync(Regex("weave-select__item-checkbox--checked"))

    let! _ = this.Page.GetByTestId("disable-option").EvaluateAsync("el => el.click()")

    do! this.Expect(options.Nth(2)).ToHaveAttributeAsync("aria-disabled", "true")
    do! selectAll.ClickAsync()
    do! this.Expect(options.Nth(0)).ToHaveAttributeAsync("aria-selected", "false")
    do! this.Expect(options.Nth(1)).ToHaveAttributeAsync("aria-selected", "false")
    do! this.Expect(options.Nth(2)).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(options.Nth(3)).ToHaveAttributeAsync("aria-selected", "false")
    do! this.Expect(checkbox).Not.ToHaveClassAsync(Regex("weave-select__item-checkbox--checked"))
    let search = root.GetByLabel("Search options")
    do! search.FillAsync("Omega")
    do! this.Expect(options).ToHaveCountAsync(1)
    do! selectAll.ClickAsync()
    do! this.Expect(options.First).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(combobox).Not.ToContainTextAsync("Blocked")
    do! this.Expect(combobox).ToContainTextAsync("Dynamic")
    do! this.Expect(combobox).ToContainTextAsync("Omega")
    do! this.Expect(combobox).Not.ToContainTextAsync("Alpha")
  }

  [<Fact>]
  member this.``select all excludes an option disabled immediately before activation``() = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId("select-all-multi")
    do! root.Locator("[role='combobox']").PressAsync("Enter")
    let selectAll = root.Locator(".weave-select__select-all")

    let! _ =
      selectAll.EvaluateAsync(
        "el => { document.querySelector('[data-testid=disable-option]').click(); el.click(); }"
      )

    let options = root.Locator("[role='option']")
    do! this.Expect(options.Nth(2)).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(options.Nth(1)).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(options.Nth(2)).ToHaveAttributeAsync("aria-selected", "false")
    do! this.Expect(options.Nth(3)).ToHaveAttributeAsync("aria-selected", "true")
  }

  [<Fact>]
  member this.``searchable select all is keyboard reachable and toggles filtered enabled options``() = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId("select-all-multi")
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let search = root.GetByLabel("Search options")
    do! this.Expect(search).ToBeFocusedAsync()
    do! search.FillAsync("a")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(3)
    do! search.PressAsync("Tab")
    let selectAll = root.GetByRole(Microsoft.Playwright.AriaRole.Checkbox)
    do! this.Expect(selectAll).ToBeFocusedAsync()
    do! this.Expect(selectAll).ToHaveAttributeAsync("aria-checked", "false")
    do! selectAll.PressAsync("Space")
    do! this.Expect(selectAll).ToHaveAttributeAsync("aria-checked", "true")
    do! this.Expect(combobox).ToContainTextAsync("Alpha")
    do! this.Expect(combobox).ToContainTextAsync("Dynamic")
    do! this.Expect(combobox).ToContainTextAsync("Omega")
    do! this.Expect(combobox).Not.ToContainTextAsync("Blocked")
    do! selectAll.PressAsync("Enter")
    do! this.Expect(selectAll).ToHaveAttributeAsync("aria-checked", "false")
    do! this.Expect(combobox).Not.ToContainTextAsync("Alpha")
    do! selectAll.PressAsync("Shift+Tab")
    do! this.Expect(search).ToBeFocusedAsync()
    do! search.PressAsync("ArrowDown")
    do! this.Expect(selectAll).ToBeFocusedAsync()
    do! selectAll.PressAsync("ArrowDown")
    do! this.Expect(search).ToBeFocusedAsync()

    do!
      this
        .Expect(root.Locator("[role='option']").First)
        .ToHaveClassAsync(Regex("weave-select__item--highlighted"))

    do! search.PressAsync("Tab")
    do! this.Expect(selectAll).ToBeFocusedAsync()
    do! selectAll.PressAsync("Tab")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-expanded", "false")

    do!
      this
        .Expect(this.Page.GetByTestId("select-all-no-search").Locator("[role='combobox']"))
        .ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``arrow keys reach select all and make selected option highlight visible``() = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId("select-all-multi")
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let search = root.GetByLabel("Search options")
    do! this.Expect(search).ToBeFocusedAsync()
    do! search.PressAsync("ArrowDown")
    let selectAll = root.GetByRole(Microsoft.Playwright.AriaRole.Checkbox)
    do! this.Expect(selectAll).ToBeFocusedAsync()
    do! selectAll.PressAsync("Space")
    do! this.Expect(selectAll).ToHaveAttributeAsync("aria-checked", "true")
    do! selectAll.PressAsync("ArrowDown")
    do! this.Expect(search).ToBeFocusedAsync()
    let options = root.Locator("[role='option']")
    let alpha = options.Nth(1)
    do! this.Expect(alpha).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(alpha).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    let! outline = alpha.EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")

    let! outlineWidth =
      alpha.EvaluateAsync<string>("el => getComputedStyle(el).outlineWidth")

    Assert.Equal("solid", outline)
    Assert.Equal("2px", outlineWidth)

    let! otherOutline =
      options.Nth(2).EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")

    Assert.Equal("none", otherOutline)
    do! search.PressAsync("ArrowDown")
    do! this.Expect(options.Nth(2)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! search.PressAsync("ArrowUp")
    do! this.Expect(alpha).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! search.PressAsync("ArrowUp")
    do! this.Expect(selectAll).ToBeFocusedAsync()
    do! selectAll.PressAsync("ArrowUp")
    do! this.Expect(search).ToBeFocusedAsync()
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-activedescendant", "")
  }

  [<Fact>]
  member this.``non searchable select all can receive keyboard focus and escape``() = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId("select-all-no-search")
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let selectAll = root.GetByRole(Microsoft.Playwright.AriaRole.Checkbox)
    do! this.Expect(selectAll).ToBeVisibleAsync()
    do! combobox.PressAsync("ArrowDown")
    do! this.Expect(selectAll).ToBeFocusedAsync()
    do! selectAll.PressAsync("Space")
    do! this.Expect(selectAll).ToHaveAttributeAsync("aria-checked", "true")
    do! selectAll.PressAsync("ArrowDown")
    do! this.Expect(combobox).ToBeFocusedAsync()

    do!
      this
        .Expect(root.Locator("[role='option']").Nth(1))
        .ToHaveClassAsync(Regex("weave-select__item--highlighted"))

    do! combobox.PressAsync("ArrowUp")
    do! this.Expect(selectAll).ToBeFocusedAsync()
    do! selectAll.PressAsync("Escape")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-expanded", "false")
    do! this.Expect(combobox).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``pointer selection and clear update the displayed value``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.ClickAsync()
    do! root.Locator("[role='option']").Nth(1).ClickAsync()
    do! this.Expect(combobox).ToContainTextAsync("Alpha")

    do!
      root
        .GetByRole(
          Microsoft.Playwright.AriaRole.Button,
          Microsoft.Playwright.LocatorGetByRoleOptions(Name = "Clear selection")
        )
        .ClickAsync()

    do! this.Expect(combobox).Not.ToContainTextAsync("Alpha")
  }

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

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``ArrowDown opens and highlights first enabled option``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    do! root.Locator("[role='combobox']").PressAsync("ArrowDown")

    do!
      this
        .Expect(root.Locator("[role='option']").Nth(1))
        .ToHaveClassAsync(Regex("weave-select__item--highlighted"))
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``ArrowDown opens after a previous search``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let search = root.GetByLabel("Search options")
    do! search.FillAsync("Omega")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(1)
    do! search.PressAsync("Escape")
    do! combobox.PressAsync("ArrowDown")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(4)

    do!
      this
        .Expect(root.Locator("[role='option']").Nth(1))
        .ToHaveClassAsync(Regex("weave-select__item--highlighted"))
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``ArrowDown opens after searching only disabled options``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let search = root.GetByLabel("Search options")
    do! search.FillAsync("Blocked")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(1)
    do! search.PressAsync("Escape")
    do! combobox.PressAsync("ArrowDown")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(4)

    do!
      this
        .Expect(root.Locator("[role='option']").Nth(1))
        .ToHaveClassAsync(Regex("weave-select__item--highlighted"))
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``keyboard navigation skips disabled options and does not wrap``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.FocusAsync()
    do! combobox.PressAsync("Enter")
    let search = root.GetByLabel("Search options")
    let options = root.Locator("[role='option']")
    do! search.PressAsync("ArrowDown")
    do! this.Expect(options.Nth(1)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! search.PressAsync("ArrowUp")
    do! this.Expect(options.Nth(1)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! search.PressAsync("ArrowDown")
    do! this.Expect(options.Nth(2)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! search.PressAsync("ArrowDown")
    do! search.PressAsync("ArrowDown")
    do! this.Expect(options.Nth(3)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``disabled and newly disabled options cannot be selected with Enter``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("Enter")
    let options = root.Locator("[role='option']")
    do! options.Nth(0).DispatchEventAsync("mouseenter")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(options.Nth(0)).Not.ToHaveClassAsync(Regex("weave-select__item--selected"))
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(options.Nth(2)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    // Change the reactive value without clicking outside the open select.
    let! _ = this.Page.GetByTestId("disable-option").EvaluateAsync("el => el.click()")

    do! this.Expect(options.Nth(2)).ToHaveClassAsync(Regex("weave-select__item--disabled"))
    do! this.Expect(options.Nth(2)).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(options.Nth(2)).Not.ToHaveClassAsync(Regex("weave-select__item--selected"))
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(options.Nth(3)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! this.Page.Keyboard.PressAsync("ArrowUp")
    do! this.Expect(options.Nth(1)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(options.Nth(3)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(combobox).ToContainTextAsync("Omega")
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``disabling highlighted option immediately before Enter does not select it``(testId: string) =
    task {
      do! this.NavigateTo("select")
      let root = this.Page.GetByTestId(testId)
      let combobox = root.Locator("[role='combobox']")
      do! combobox.PressAsync("Enter")
      let search = root.GetByLabel("Search options")
      do! search.PressAsync("ArrowDown")
      do! search.PressAsync("ArrowDown")

      do!
        this
          .Expect(root.Locator("[role='option']").Nth(2))
          .ToHaveClassAsync(Regex("weave-select__item--highlighted"))

      let! _ =
        search.EvaluateAsync(
          "el => { document.querySelector('[data-testid=disable-option]').click(); el.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true })); }"
        )

      do! this.Expect(combobox).Not.ToContainTextAsync("Dynamic")
      do! this.Expect(combobox).ToHaveAttributeAsync("aria-expanded", "true")
      do! this.Expect(root.Locator("[role='option']").Nth(2)).ToHaveAttributeAsync("aria-disabled", "true")
      do! this.Expect(root.Locator("[role='option']").Nth(2)).ToHaveAttributeAsync("aria-selected", "false")
    }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``search navigation handles disabled and empty results``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    do! root.Locator("[role='combobox']").PressAsync("Enter")
    let search = root.GetByLabel("Search options")
    do! search.FillAsync("Blocked")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(1)
    do! search.PressAsync("ArrowDown")
    do! search.PressAsync("Enter")

    do!
      this.Expect(root.Locator("[role='option']")).Not.ToHaveClassAsync(Regex("weave-select__item--selected"))

    do! search.FillAsync("no such option")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(0)
    do! search.PressAsync("ArrowDown")
    do! search.PressAsync("Enter")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(0)
    do! search.FillAsync("Omega")
    do! this.Expect(root.Locator("[role='option']")).ToHaveCountAsync(1)
    do! search.PressAsync("ArrowDown")
    do! search.PressAsync("Enter")
    do! this.Expect(root.Locator("[role='combobox']")).ToContainTextAsync("Omega")
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``active descendant and option aria state follow keyboard selection``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("ArrowDown")
    let alpha = root.Locator("[role='option']").Nth(1)
    let! alphaId = alpha.GetAttributeAsync("id")
    do! this.Expect(alpha).ToHaveAttributeAsync("aria-selected", "false")
    do! this.Expect(root.Locator("[role='option']").Nth(0)).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-activedescendant", alphaId)
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(combobox).ToContainTextAsync("Alpha")

    if testId = "keyboard-single" then
      do! this.Expect(combobox).ToHaveAttributeAsync("aria-activedescendant", "")
      do! combobox.PressAsync("Enter")

    do! this.Expect(root.Locator("[role='option']").Nth(1)).ToHaveAttributeAsync("aria-selected", "true")

    if testId = "keyboard-multi" then
      do! this.Page.Keyboard.PressAsync("Enter")
      do! this.Expect(root.Locator("[role='option']").Nth(1)).ToHaveAttributeAsync("aria-selected", "false")
      do! this.Expect(combobox).Not.ToContainTextAsync("Alpha")
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``reordering options clears stale highlighted identity``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("ArrowDown")
    let options = root.Locator("[role='option']")
    do! this.Expect(options.Nth(1)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))

    let! _ = this.Page.GetByTestId("reorder-options").EvaluateAsync("el => el.click()")

    do! this.Expect(options.First).ToContainTextAsync("Omega")
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-activedescendant", "")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(root.Locator(".weave-select__item--selected")).ToHaveCountAsync(0)
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(combobox).ToContainTextAsync("Omega")
  }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``reordering immediately before Enter does not activate a different option``(testId: string) =
    task {
      do! this.NavigateTo("select")
      let root = this.Page.GetByTestId(testId)
      let combobox = root.Locator("[role='combobox']")
      do! combobox.PressAsync("ArrowDown")
      let options = root.Locator("[role='option']")
      do! this.Expect(options.Nth(1)).ToContainTextAsync("Alpha")
      do! this.Expect(options.Nth(1)).ToHaveClassAsync(Regex("weave-select__item--highlighted"))

      let! _ =
        root
          .GetByLabel("Search options")
          .EvaluateAsync(
            "el => { document.querySelector('[data-testid=reorder-options]').click(); el.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true })); }"
          )

      do! this.Expect(options.First).ToContainTextAsync("Omega")
      do! this.Expect(root.Locator(".weave-select__item--selected")).ToHaveCountAsync(0)
      do! this.Expect(combobox).Not.ToContainTextAsync("Dynamic")
      do! this.Expect(combobox).Not.ToContainTextAsync("Alpha")
    }

  [<Theory>]
  [<InlineData("keyboard-single")>]
  [<InlineData("keyboard-multi")>]
  member this.``reopening with all options disabled clears previous highlight``(testId: string) = task {
    do! this.NavigateTo("select")
    let root = this.Page.GetByTestId(testId)
    let combobox = root.Locator("[role='combobox']")
    do! combobox.PressAsync("ArrowDown")

    do!
      this
        .Expect(root.Locator("[role='option']").Nth(1))
        .ToHaveClassAsync(Regex("weave-select__item--highlighted"))

    do! this.Page.Keyboard.PressAsync("Escape")

    let! _ =
      this.Page.GetByTestId("disable-all-options").EvaluateAsync("el => el.click()")

    do! combobox.PressAsync("ArrowDown")
    do! this.Expect(root.Locator("[role='option'][aria-disabled='true']")).ToHaveCountAsync(4)
    do! this.Expect(combobox).ToHaveAttributeAsync("aria-activedescendant", "")
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(root.Locator(".weave-select__item--selected")).ToHaveCountAsync(0)
  }
