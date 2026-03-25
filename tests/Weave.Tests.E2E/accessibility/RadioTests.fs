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
    do! this.Expect(input).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Space selects an unselected radio``() = task {
    do! this.NavigateTo("radio")
    // Focus the second radio (B), which starts unselected
    let inputB = this.Page.Locator(".weave-radio__input").Nth(1)
    do! inputB.FocusAsync()
    do! inputB.PressAsync("Space")
    do! this.Expect(inputB).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Enter selects an unselected radio``() = task {
    do! this.NavigateTo("radio")
    let inputB = this.Page.Locator(".weave-radio__input").Nth(1)
    do! inputB.FocusAsync()
    do! inputB.PressAsync("Enter")
    do! this.Expect(inputB).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Enter on already-selected radio keeps it selected``() = task {
    do! this.NavigateTo("radio")
    let inputA = this.Page.Locator(".weave-radio__input").First
    do! inputA.FocusAsync()
    do! inputA.PressAsync("Enter")
    do! this.Expect(inputA).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``ArrowDown moves selection to next radio in group``() = task {
    do! this.NavigateTo("radio")
    // First radio (A) starts selected; ArrowDown should move to B
    let inputA = this.Page.Locator(".weave-radio__input").First
    let inputB = this.Page.Locator(".weave-radio__input").Nth(1)
    do! inputA.FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(inputB).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``disabled radio is not focusable``() = task {
    do! this.NavigateTo("radio")
    let disabledInput = this.Page.Locator(".weave-radio__input[disabled]").First
    do! disabledInput.FocusAsync()
    do! this.Expect(disabledInput).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``focus-visible outline appears on the span when input is focused``() = task {
    do! this.NavigateTo("radio")
    let input = this.Page.Locator(".weave-radio__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
    let span = this.Page.Locator(".weave-radio__span").First

    let! outlineStyle = span.EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")

    Assert.NotEqual<string>("none", outlineStyle)
  }
