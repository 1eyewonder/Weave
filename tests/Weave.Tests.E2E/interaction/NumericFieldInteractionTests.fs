namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type NumericFieldInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

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
  member _.``touch tap increments numeric field once``(testId: string) = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    let! _ = page.GotoAsync($"%s{fixture.BaseUrl}/#numericfield")
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
