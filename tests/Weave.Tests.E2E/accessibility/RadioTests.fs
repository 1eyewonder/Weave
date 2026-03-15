namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type RadioTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("radio")

  [<Fact>]
  member this.``radio input is focusable``() = task {
    do! this.NavigateTo("radio")
    let input = this.Page.Locator(".weave-radio__input").First
    do! input.FocusAsync()
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-radio__input", activeClass)
  }

  [<Fact>]
  member this.``Space selects an unselected radio``() = task {
    do! this.NavigateTo("radio")
    // Focus the second radio (B), which starts unselected
    let inputB = this.Page.Locator(".weave-radio__input").Nth(1)
    do! inputB.FocusAsync()
    do! inputB.PressAsync("Space")
    let! isChecked = inputB.EvaluateAsync<bool>("el => el.checked")
    Assert.True(isChecked, "Radio B should be selected after pressing Space")
  }

  [<Fact>]
  member this.``ArrowDown moves selection to next radio in group``() = task {
    do! this.NavigateTo("radio")
    // First radio (A) starts selected; ArrowDown should move to B
    let inputA = this.Page.Locator(".weave-radio__input").First
    let inputB = this.Page.Locator(".weave-radio__input").Nth(1)
    do! inputA.FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    let! isChecked = inputB.EvaluateAsync<bool>("el => el.checked")
    Assert.True(isChecked, "Radio B should be selected after ArrowDown from radio A")
  }
