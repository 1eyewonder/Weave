namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type SwitchTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("switch")

  [<Fact>]
  member this.``switch input is focusable``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Space toggles an off switch to on``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! input.PressAsync("Space")
    do! this.Expect(input).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Enter toggles an off switch to on``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! input.PressAsync("Enter")
    do! this.Expect(input).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Enter toggles an on switch to off``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").Nth(1)
    do! input.FocusAsync()
    do! this.Expect(input).ToBeCheckedAsync()
    do! input.PressAsync("Enter")
    do! this.Expect(input).Not.ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Tab navigates between switch inputs``() = task {
    do! this.NavigateTo("switch")
    let firstInput = this.Page.Locator(".weave-switch__input").First
    let secondInput = this.Page.Locator(".weave-switch__input").Nth(1)
    do! firstInput.FocusAsync()
    do! this.Expect(firstInput).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(secondInput).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled switch is not focusable``() = task {
    do! this.NavigateTo("switch")
    let disabledInput = this.Page.Locator(".weave-switch__input[disabled]").First
    do! disabledInput.FocusAsync()
    do! this.Expect(disabledInput).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled switch is skipped by Tab``() = task {
    do! this.NavigateTo("switch")
    let secondInput = this.Page.Locator(".weave-switch__input").Nth(1)
    do! secondInput.FocusAsync()
    // The third switch is disabled — Tab should skip it and not leave focus on it
    do! this.Page.Keyboard.PressAsync("Tab")
    let disabledInput = this.Page.Locator(".weave-switch__input[disabled]").First
    do! this.Expect(disabledInput).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``switch input has role switch``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! this.Expect(input).ToHaveAttributeAsync("role", "switch")
  }

  [<Fact>]
  member this.``focus-visible outline appears on the track when input is focused``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
    let track = this.Page.Locator(".weave-switch__track").First

    let! outlineStyle = track.EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")

    Assert.NotEqual<string>("none", outlineStyle)
  }
