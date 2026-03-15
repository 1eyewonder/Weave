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
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-switch__input", activeClass)
  }

  [<Fact>]
  member this.``Space toggles an off switch to on``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! input.PressAsync("Space")
    let! isChecked = input.EvaluateAsync<bool>("el => el.checked")
    Assert.True(isChecked, "Switch should be on after pressing Space")
  }

  [<Fact>]
  member this.``disabled switch is not focusable``() = task {
    do! this.NavigateTo("switch")
    let disabledInput = this.Page.Locator(".weave-switch__input[disabled]").First
    do! disabledInput.FocusAsync()
    let! activeTag = this.Page.EvaluateAsync<string>("() => document.activeElement?.tagName ?? 'BODY'")
    Assert.NotEqual<string>("INPUT", activeTag)
  }
