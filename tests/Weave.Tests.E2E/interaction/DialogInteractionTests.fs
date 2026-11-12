namespace Weave.Tests.E2E

open Xunit

type DialogInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``dialog action button is focusable``() = task {
    do! this.NavigateTo("dialog")
    let actionButton = this.Page.Locator(".weave-dialog__window .weave-button").First
    do! actionButton.FocusAsync()
    do! this.Expect(actionButton).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``dialog window has role dialog``() = task {
    do! this.NavigateTo("dialog")
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialogWindow).ToHaveAttributeAsync("role", "dialog")
  }

  [<Fact>]
  member this.``dialog window has aria-modal true``() = task {
    do! this.NavigateTo("dialog")
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialogWindow).ToHaveAttributeAsync("aria-modal", "true")
  }

  [<Fact>]
  member this.``dialog window aria-labelledby references the title``() = task {
    do! this.NavigateTo("dialog")
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    let! labelledBy = dialogWindow.GetAttributeAsync("aria-labelledby")
    Assert.NotNull(labelledBy)
    let referencedTitle = this.Page.Locator($"#{labelledBy}")
    do! this.Expect(referencedTitle).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``Escape closes the dialog``() = task {
    do! this.NavigateTo("dialog")
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialogWindow).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Page.Locator(".weave-dialog")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Escape does not close a Force dialog``() = task {
    do! this.NavigateTo("dialog-force")
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialogWindow).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Page.Locator(".weave-dialog")).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``Tab does not move focus outside the dialog``() = task {
    do! this.NavigateTo("dialog")
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialogWindow).ToBeFocusedAsync()
    // Tab past all dialog elements; focus should wrap back inside
    for _ in 1..10 do
      do! this.Page.Keyboard.PressAsync("Tab")

    let focusedInsideDialog = this.Page.Locator(".weave-dialog__window :focus")
    do! this.Expect(focusedInsideDialog).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``focus returns to trigger element after dialog closes``() = task {
    do! this.NavigateTo("dialog-triggered")
    let triggerButton = this.Page.Locator("#open-dialog-btn")
    do! triggerButton.FocusAsync()
    do! this.Expect(triggerButton).ToBeFocusedAsync()
    do! triggerButton.PressAsync("Enter")
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialogWindow).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(triggerButton).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``plain title gives the dialog an accessible name``() = task {
    do! this.NavigateTo("dialog-plain")

    do!
      this
        .Expect(this.Page.GetByRole(Microsoft.Playwright.AriaRole.Dialog))
        .ToHaveAccessibleNameAsync("Plain dialog title")
  }

  [<Fact>]
  member this.``dialog with no actions keeps keyboard focus inside``() = task {
    do! this.NavigateTo("dialog-empty")
    let dialog = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialog).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(dialog).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! this.Expect(dialog).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``dialog actions remain within a narrow viewport``() = task {
    do! this.NavigateTo("dialog")
    do! this.Page.SetViewportSizeAsync(320, 640)
    let window = this.Page.Locator(".weave-dialog__window")
    let actions = this.Page.Locator(".weave-dialog__actions")
    do! this.Expect(actions).ToBeVisibleAsync()
    let! bounds = window.BoundingBoxAsync()
    Assert.NotNull(bounds)
    Assert.True(bounds.X >= 0.0f && bounds.X + bounds.Width <= 320.0f, "Dialog should fit the viewport")
  }

  [<Fact>]
  member this.``dialog backdrop blocks an open drawer``() = task {
    do! this.NavigateTo("dialog-with-drawer")
    let drawer = this.Page.GetByTestId("dialog-test-drawer")
    do! this.Expect(drawer).ToBeVisibleAsync()

    let! blocked =
      drawer.EvaluateAsync<bool>(
        """el => {
      const r = el.getBoundingClientRect();
      const target = document.elementFromPoint(r.left + r.width / 2, r.top + r.height / 2);
      return !!target && target.classList.contains('weave-dialog__backdrop');
    }"""
      )

    Assert.True(blocked, "The dialog backdrop should cover the open drawer")
  }

  [<Fact>]
  member this.``dialog includes native disclosures and the selected radio in its focus loop``() = task {
    do!
      this.Page.EmulateMediaAsync(
        Microsoft.Playwright.PageEmulateMediaOptions(
          ReducedMotion = Microsoft.Playwright.ReducedMotion.Reduce
        )
      )

    do! this.NavigateTo("dialog-native")
    let trigger = this.Page.GetByTestId("native-dialog-toggle")
    let dialog = this.Page.Locator(".weave-dialog__window")
    let first = this.Page.GetByTestId("drawer-first")
    let summary = this.Page.GetByTestId("drawer-summary")
    let nested = this.Page.GetByTestId("drawer-nested")
    let radio = this.Page.GetByTestId("drawer-radio-one")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(dialog).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(first).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! Microsoft.Playwright.Assertions.Expect(summary, "Forward Tab from first control").ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(radio).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(first).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! this.Expect(radio).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! Microsoft.Playwright.Assertions.Expect(summary, "Reverse Tab from selected radio").ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(nested).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(nested).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(radio).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Escape closes only the optional dialog nested inside a drawer``() = task {
    do! this.NavigateTo("dialog-in-drawer-optional")
    let drawerTrigger = this.Page.GetByTestId("nested-drawer-trigger")
    let dialogTrigger = this.Page.GetByTestId("nested-dialog-trigger")
    let drawer = this.Page.GetByTestId("nested-dialog-drawer")
    let dialog = this.Page.GetByRole(Microsoft.Playwright.AriaRole.Dialog)
    do! drawerTrigger.PressAsync("Enter")
    do! this.Expect(dialogTrigger).ToBeFocusedAsync()
    do! dialogTrigger.PressAsync("Enter")
    do! this.Expect(dialog).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(dialog).ToHaveCountAsync(0)
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--open"))
    do! this.Expect(dialogTrigger).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))
    do! this.Expect(drawerTrigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Escape keeps a Force dialog and its enclosing drawer open``() = task {
    do! this.NavigateTo("dialog-in-drawer-force")
    let dialogTrigger = this.Page.GetByTestId("nested-dialog-trigger")
    let drawer = this.Page.GetByTestId("nested-dialog-drawer")
    let dialog = this.Page.GetByRole(Microsoft.Playwright.AriaRole.Dialog)
    do! this.Page.GetByTestId("nested-drawer-trigger").PressAsync("Enter")
    do! this.Expect(dialogTrigger).ToBeFocusedAsync()
    do! dialogTrigger.PressAsync("Enter")
    do! this.Expect(dialog).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")

    let! _ =
      this.Page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! this.Expect(dialog).ToHaveCountAsync(1)
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--open"))
    do! this.Expect(dialog).ToBeFocusedAsync()
  }
