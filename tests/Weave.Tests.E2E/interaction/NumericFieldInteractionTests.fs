namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type NumericFieldInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("bounded-int", "3", "7")>]
  [<InlineData("bounded-float", "4.5", "5.5")>]
  member this.``bounded numeric fields expose their limits and support Tab stepping with arrow keys off``
    (testId: string, minimum: string, maximum: string)
    = task {
    do! this.NavigateTo("numericfield")
    let root = this.Page.GetByTestId(testId)
    let input = root.GetByRole(AriaRole.Spinbutton)
    let increment = root.GetByLabel("Increment")
    let decrement = root.GetByLabel("Decrement")
    do! this.Expect(input).ToHaveAttributeAsync("min", minimum)
    do! this.Expect(input).ToHaveAttributeAsync("max", maximum)
    do! input.PressAsync("ArrowUp")
    do! this.Expect(input).ToHaveValueAsync("5")
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(increment).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(decrement).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Space")
    do! this.Expect(input).ToHaveValueAsync(minimum)
    do! this.Expect(decrement).ToBeDisabledAsync()
    do! input.PressAsync("Tab")
    do! this.Expect(increment).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(input).ToHaveValueAsync("5")
    do! increment.PressAsync("Enter")
    do! this.Expect(input).ToHaveValueAsync(maximum)
    do! this.Expect(increment).ToBeDisabledAsync()
    do! this.Expect(decrement).ToBeEnabledAsync()

    for toggle in [ "Toggle read-only"; "Toggle enabled" ] do
      let control =
        this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = toggle))

      do! control.ClickAsync()
      do! this.Expect(increment).ToBeDisabledAsync()
      do! this.Expect(decrement).ToBeDisabledAsync()
      do! control.ClickAsync()
      do! this.Expect(decrement).ToBeEnabledAsync()
  }

  [<Theory>]
  [<InlineData("numeric-int")>]
  [<InlineData("numeric-float")>]
  member this.``a pressed spin button retains its click and focus when visibility stays true``
    (testId: string)
    = task {
    do! this.NavigateTo("numericfield")
    let field = this.Page.GetByTestId(testId)
    let input = field.Locator("input")

    let increment =
      field.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Increment"))

    let setSix =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Set spin values to six"))

    let setZero =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Set spin values to zero"))

    do! increment.FocusAsync()
    do! increment.HoverAsync()
    do! this.Page.Mouse.DownAsync()
    do! setSix.DispatchEventAsync("click")
    do! this.Expect(input).ToHaveValueAsync("6")
    do! this.Expect(increment).ToBeFocusedAsync()
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(input).ToHaveValueAsync("7")

    do! setZero.ClickAsync()
    do! this.Expect(field.GetByRole(AriaRole.Button)).ToHaveCountAsync(0)
    do! setSix.ClickAsync()
    do! field.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Decrement")).PressAsync("Enter")
    do! this.Expect(input).ToHaveValueAsync("5")
  }

  [<Theory>]
  [<InlineData("numeric-int")>]
  [<InlineData("numeric-float")>]
  member this.``spin button supports keyboard and programmatic activation``(testId: string) = task {
    do! this.NavigateTo("numericfield")
    let root = this.Page.GetByTestId(testId)
    let input = root.Locator("input")
    let increment = root.GetByLabel("Increment")
    do! increment.PressAsync("Enter")
    do! this.Expect(input).ToHaveValueAsync("6")
    do! increment.PressAsync("Space")
    do! this.Expect(input).ToHaveValueAsync("7")
    let! _ = increment.EvaluateAsync("el => el.click()")
    do! this.Expect(input).ToHaveValueAsync("8")
  }

  [<Theory>]
  [<InlineData("numeric-int")>]
  [<InlineData("numeric-float")>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap increments numeric field once``(testId: string) = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "numericfield")
    let root = page.GetByTestId(testId)
    do! root.GetByLabel("Increment").TapAsync()

    let! _ =
      page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! Microsoft.Playwright.Assertions.Expect(root.Locator("input")).ToHaveValueAsync("6")
    do! root.GetByLabel("Decrement").TapAsync()

    let! _ =
      page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! Microsoft.Playwright.Assertions.Expect(root.Locator("input")).ToHaveValueAsync("5")
  }

  [<Fact>]
  member this.``invalid numeric field has aria-invalid attribute``() = task {
    do! this.NavigateTo("numericfield")
    let input = this.Page.Locator("input[aria-invalid='true']").First
    do! this.Expect(input).ToBeVisibleAsync()
    do! this.Expect(input).ToHaveAttributeAsync("aria-describedby", "numeric-error")
  }

  [<Fact>]
  member this.``aria-describedby points to visible help text``() = task {
    do! this.NavigateTo("numericfield")
    let helpText = this.Page.Locator("#numeric-error")
    do! this.Expect(helpText).ToBeVisibleAsync()
    do! this.Expect(helpText).ToHaveTextAsync("Value exceeds maximum")
  }
