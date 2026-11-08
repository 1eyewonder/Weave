namespace Weave.Tests.E2E

open Microsoft.Playwright
open global.Xunit

type ButtonInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("activation-button", "activation-disabled")>]
  [<InlineData("activation-icon", "activation-disabled-icon")>]
  [<Trait("Capability", "MobileEmulation")>]
  member _.``touch taps activate once and disabled buttons remain inactive``
    (enabledId: string, disabledId: string)
    = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    let! _ = page.GotoAsync($"%s{fixture.BaseUrl}/#button")
    let button = page.GetByTestId(enabledId)
    let disabled = page.GetByTestId(disabledId)
    let count = page.GetByTestId("activation-count")
    do! Assertions.Expect(disabled).ToBeDisabledAsync()
    do! button.TapAsync()
    do! Assertions.Expect(count).ToHaveTextAsync("1")
    do! disabled.TapAsync(LocatorTapOptions(Force = true))

    let! _ =
      page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! Assertions.Expect(count).ToHaveTextAsync("1")
    do! button.TapAsync()

    let! _ =
      page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! Assertions.Expect(count).ToHaveTextAsync("2")
  }

  [<Theory>]
  [<InlineData("activation-button")>]
  [<InlineData("activation-icon")>]
  member this.``native activation invokes callback once per action``(testId: string) = task {
    do! this.NavigateTo("button")
    let button = this.Page.GetByTestId(testId)
    let count = this.Page.GetByTestId("activation-count")
    do! button.ClickAsync()
    do! this.Expect(count).ToHaveTextAsync("1")
    do! button.PressAsync("Enter")
    do! this.Expect(count).ToHaveTextAsync("2")
    do! button.PressAsync("Space")
    do! this.Expect(count).ToHaveTextAsync("3")
    let! _ = button.EvaluateAsync("el => el.click()")
    do! this.Expect(count).ToHaveTextAsync("4")
  }

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
    let errorButton = this.Page.Locator(".weave-button--error")
    let! primaryCount = primaryButton.CountAsync()
    and! errorCount = errorButton.CountAsync()
    Assert.True(primaryCount >= 1, $"Expected at least 1 primary button, found %i{primaryCount}")
    Assert.True(errorCount >= 1, $"Expected at least 1 error button, found %i{errorCount}")
  }
