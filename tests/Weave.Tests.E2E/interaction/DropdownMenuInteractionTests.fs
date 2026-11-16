namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type DropdownMenuInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("Enter")>]
  [<InlineData(" ")>]
  [<InlineData("click")>]
  member this.``disabling a focused menu blocks activation and closes its bound state``(activation: string) =
    task {
      do! this.NavigateTo("dropdown-menu-dynamic")

      let trigger =
        this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Actions"))

      let item = this.Page.GetByRole(AriaRole.Menuitem)
      let activations = this.Page.GetByTestId("activations")

      let openAndFocusAction () = task {
        do! trigger.PressAsync("Enter")
        do! this.Expect(item).ToBeVisibleAsync()
        do! trigger.PressAsync("ArrowDown")
        do! this.Expect(item).ToBeFocusedAsync()
      }

      do! openAndFocusAction ()

      // Update availability without moving focus, then activate before the popup can be removed.
      let! _ =
        item.EvaluateAsync(
          """(item, activation) => {
          document.querySelector('[data-testid="toggle-availability"]').click();
          if (activation === 'click') item.click();
          else item.dispatchEvent(new KeyboardEvent('keydown', {key: activation, bubbles: true, cancelable: true}));
        }""",
          activation
        )

      do! this.Expect(trigger).ToBeDisabledAsync()
      do! this.Expect(activations).ToHaveTextAsync("0")
      do! this.Expect(this.Page.GetByRole(AriaRole.Menu)).ToHaveCountAsync(0)
      do! this.Expect(trigger).ToHaveAttributeAsync("aria-expanded", "false")
      do! this.Expect(this.Page.GetByTestId("open-state")).ToHaveTextAsync("false")

      do! this.Page.GetByTestId("toggle-availability").ClickAsync()
      do! this.Expect(trigger).ToBeEnabledAsync()
      do! this.Expect(trigger).ToHaveAttributeAsync("aria-expanded", "false")
      do! openAndFocusAction ()

      if activation = "click" then
        do! item.ClickAsync()
      else
        do! this.Page.Keyboard.PressAsync(activation)

      do! this.Expect(activations).ToHaveTextAsync("1")
    }

  [<Fact>]
  member this.``disabled hover menu rejects open requests until enabled and explicitly reopened``() = task {
    do! this.NavigateTo("dropdown-menu-dynamic-hover")

    let trigger =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Actions"))

    let menu = this.Page.GetByRole(AriaRole.Menu)
    let toggle = this.Page.GetByTestId("toggle-availability")

    let requestOpen =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Request open"))

    do! toggle.ClickAsync()
    do! this.Expect(trigger).ToBeDisabledAsync()
    do! requestOpen.ClickAsync()

    // Let the external open request reach the UI before checking that it was rejected.
    let! _ =
      this.Page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! this.Expect(menu).ToHaveCountAsync(0)
    do! this.Expect(this.Page.GetByTestId("open-state")).ToHaveTextAsync("false")
    do! toggle.ClickAsync()
    do! this.Expect(trigger).ToBeEnabledAsync()
    do! this.Expect(trigger).ToHaveAttributeAsync("aria-expanded", "false")
    do! trigger.HoverAsync()
    do! this.Expect(menu).ToBeVisibleAsync()
    do! this.Expect(this.Page.GetByTestId("open-state")).ToHaveTextAsync("true")
  }

  [<Fact>]
  member this.``programmatic click activates menu item and closes menu``() = task {
    do! this.NavigateTo("dropdown-menu")
    do! this.Page.Locator(".weave-dropdownmenu .weave-button").First.ClickAsync()
    let item = this.Page.Locator(".weave-dropdownmenu__item").First
    do! this.Expect(item).ToBeVisibleAsync()
    let! _ = item.EvaluateAsync("el => el.click()")
    do! this.Expect(this.Page.GetByTestId("dropdown-action")).ToHaveTextAsync("item 1")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``keyboard activation calls the menu action``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.PressAsync("Enter")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__item").First).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("dropdown-action")).ToHaveTextAsync("item 1")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToHaveCountAsync(0)
  }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap activates a dropdown item``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "dropdown-menu")
    do! page.Locator(".weave-dropdownmenu .weave-button").First.TapAsync()
    do! page.Locator(".weave-dropdownmenu__item").First.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("dropdown-action")).ToHaveTextAsync("item 1")
  }

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

  [<Theory>]
  [<InlineData("dropdown-menu", "Open Menu")>]
  [<InlineData("dropdown-menu-icon", "More actions")>]
  member this.``Given a named dropdown trigger When toggled with Enter Then it keeps its name and reports the menu state``
    (route: string, name: string)
    = task {
    do! this.NavigateTo(route)

    let trigger =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

    let menu = this.Page.GetByRole(AriaRole.Menu)
    do! this.Expect(trigger).ToHaveAttributeAsync("aria-expanded", "false")
    do! trigger.PressAsync("Enter")
    do! this.Expect(menu).ToBeVisibleAsync()
    do! this.Expect(trigger).ToHaveAttributeAsync("aria-expanded", "true")
    do! trigger.PressAsync("Enter")
    do! this.Expect(menu).ToHaveCountAsync(0)
    do! this.Expect(trigger).ToHaveAttributeAsync("aria-expanded", "false")
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
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__item").First).ToBeFocusedAsync()
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

  [<Fact>]
  member this.``rapid navigation and dismissal keeps focus on the trigger``() = task {
    do! this.NavigateTo("dropdown-menu")
    let trigger = this.Page.Locator(".weave-dropdownmenu .weave-button").First
    do! trigger.FocusAsync()

    let! _ =
      trigger.EvaluateAsync(
        "el => { el.dispatchEvent(new KeyboardEvent('keydown', {key:'ArrowDown', bubbles:true})); el.dispatchEvent(new KeyboardEvent('keydown', {key:'Escape', bubbles:true})); }"
      )

    do! this.Expect(this.Page.Locator(".weave-dropdownmenu__list")).ToHaveCountAsync(0)

    let! _ =
      this.Page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! this.Expect(trigger).ToBeFocusedAsync()
  }
