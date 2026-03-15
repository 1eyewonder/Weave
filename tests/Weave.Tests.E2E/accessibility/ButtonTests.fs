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
    let! activeClass = this.Page.EvaluateAsync<string>("() => document.activeElement?.className ?? ''")
    Assert.Contains("weave-button", activeClass)
  }

  [<Fact>]
  member this.``Enter activates a button``() = task {
    do! this.NavigateTo("button")
    // Inject a click counter before pressing Enter
    let! _ =
      this.Page.EvaluateAsync<obj>(
        "() => { window.__clickCount = 0; document.querySelector('.weave-button').addEventListener('click', () => window.__clickCount++) }"
      )

    do! this.Page.Locator(".weave-button").First.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    let! count = this.Page.EvaluateAsync<int>("() => window.__clickCount")
    Assert.Equal(1, count)
  }

  [<Fact(Skip = "Known gap: Disabled buttons use a CSS class only, not the native disabled attr — they remain keyboard-focusable")>]
  member this.``disabled button is not focusable``() = task {
    do! this.NavigateTo("button")
    let disabledButton = this.Page.Locator(".weave-button--disabled").First
    do! disabledButton.FocusAsync()
    let! activeTag = this.Page.EvaluateAsync<string>("() => document.activeElement?.tagName ?? 'BODY'")
    Assert.NotEqual<string>("BUTTON", activeTag)
  }
