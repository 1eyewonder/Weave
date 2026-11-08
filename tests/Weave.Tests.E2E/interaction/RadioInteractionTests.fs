namespace Weave.Tests.E2E

open Xunit

type RadioInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

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

    let! outlineStyle =
      span.EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")

    Assert.NotEqual<string>("none", outlineStyle)
  }

  [<Fact>]
  member this.``radio navigation keeps same-named groups in separate forms isolated``() = task {
    do! this.NavigateTo("radio-groups")
    let first = this.Page.GetByTestId("first-radio-form").Locator("input")
    let second = this.Page.GetByTestId("second-radio-form").Locator("input")
    do! first.Nth(0).FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowDown")
    do! this.Expect(first.Nth(1)).ToBeFocusedAsync()
    do! this.Expect(first.Nth(1)).ToBeCheckedAsync()
    do! this.Expect(second.Nth(0)).ToBeCheckedAsync()
    do! this.Expect(second.Nth(1)).Not.ToBeCheckedAsync()
  }
