namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
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
    do! triggerButton.ClickAsync()
    let dialogWindow = this.Page.Locator(".weave-dialog__window")
    do! this.Expect(dialogWindow).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(triggerButton).ToBeFocusedAsync()
  }
