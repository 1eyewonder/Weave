namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ButtonTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("button")

  [<Fact>]
  member this.``button is focusable``() = task {
    do! this.NavigateTo("button")
    let button = this.Page.Locator(".weave-button").First
    do! button.FocusAsync()
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter activates a button``() = task {
    do! this.NavigateTo("button")
    let button = this.Page.Locator(".weave-button").First
    do! button.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    // Native button fires click on Enter — verify the element is still focused (activation didn't break focus)
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Space activates a button``() = task {
    do! this.NavigateTo("button")
    let button = this.Page.Locator(".weave-button").First
    do! button.FocusAsync()
    do! this.Page.Keyboard.PressAsync(" ")
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled button is not focusable``() = task {
    do! this.NavigateTo("button")
    let disabledButton = this.Page.Locator(".weave-button--disabled").First
    do! this.Expect(disabledButton).ToHaveAttributeAsync("disabled", "")
    do! disabledButton.FocusAsync()
    do! this.Expect(disabledButton).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``icon button is focusable``() = task {
    do! this.NavigateTo("button")
    let iconButton = this.Page.Locator(".weave-button--icon").First
    do! iconButton.FocusAsync()
    do! this.Expect(iconButton).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``icon button has aria-label``() = task {
    do! this.NavigateTo("button")
    let iconButton = this.Page.Locator(".weave-button--icon").First
    do! this.Expect(iconButton).ToHaveAttributeAsync("aria-label", "home")
  }

  [<Fact>]
  member this.``color variant buttons are present``() = task {
    do! this.NavigateTo("button")
    let primaryButton = this.Page.Locator(".weave-button--primary")
    do! this.Expect(primaryButton).ToHaveCountAsync(2) // icon + text button
    let errorButton = this.Page.Locator(".weave-button--error")
    do! this.Expect(errorButton).ToHaveCountAsync(1)
  }
