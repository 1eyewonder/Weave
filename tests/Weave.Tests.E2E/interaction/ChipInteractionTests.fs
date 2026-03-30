namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ChipInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``clickable chip is focusable``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! chip.FocusAsync()
    do! this.Expect(chip).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``clickable chip has role button``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! this.Expect(chip).ToHaveAttributeAsync("role", "button")
  }

  [<Fact>]
  member this.``Enter activates a clickable chip``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! chip.FocusAsync()
    do! chip.PressAsync("Enter")
    let status = this.Page.Locator("[data-testid='chip-clicked']")
    do! this.Expect(status).ToHaveTextAsync("clicked")
  }

  [<Fact>]
  member this.``Space activates a clickable chip``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! chip.FocusAsync()
    do! chip.PressAsync(" ")
    let status = this.Page.Locator("[data-testid='chip-clicked']")
    do! this.Expect(status).ToHaveTextAsync("clicked")
  }

  [<Fact>]
  member this.``disabled clickable chip has tabindex -1``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='disabled-chip']")
    do! this.Expect(chip).ToHaveAttributeAsync("tabindex", "-1")
  }

  [<Fact>]
  member this.``close button is focusable and has aria-label``() = task {
    do! this.NavigateTo("chip")
    let close = this.Page.Locator(".weave-chip__close").First
    do! close.FocusAsync()
    do! this.Expect(close).ToBeFocusedAsync()
    do! this.Expect(close).ToHaveAttributeAsync("aria-label", "remove")
  }
