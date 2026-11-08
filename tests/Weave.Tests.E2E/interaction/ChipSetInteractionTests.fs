namespace Weave.Tests.E2E

open Xunit

type ChipSetInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.NavigateToChipSet() = task {
    do! this.NavigateTo("chipset")
    // Enabled chips start with tabindex 0; -1 shows the deferred roving setup ran.
    let secondChip = this.Page.Locator(".weave-chip[role='button']").Nth(1)
    do! this.Expect(secondChip).ToHaveAttributeAsync("tabindex", "-1")
  }

  [<Fact>]
  member this.``closable chipset arrow navigation skips disabled action``() = task {
    do! this.NavigateToChipSet()
    let group = this.Page.GetByTestId("closable-chipset")
    let actions = group.Locator(".weave-chip__action[role='button']")
    do! actions.First.FocusAsync()
    do! actions.First.PressAsync("ArrowRight")
    do! this.Expect(actions.Nth(2)).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``chipset container has role group``() = task {
    do! this.NavigateToChipSet()
    let chipset = this.Page.Locator(".weave-chipset").First
    do! this.Expect(chipset).ToHaveAttributeAsync("role", "group")
  }

  [<Fact>]
  member this.``first chip is focusable via Tab``() = task {
    do! this.NavigateToChipSet()
    let firstChip = this.Page.Locator(".weave-chip[role='button']").First
    do! this.Page.Keyboard.PressAsync("Tab")
    // May need multiple tabs to reach chipset depending on page structure
    do! firstChip.FocusAsync()
    do! this.Expect(firstChip).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowRight moves focus to next chip``() = task {
    do! this.NavigateToChipSet()
    let firstChip = this.Page.Locator(".weave-chip[role='button']").First
    let secondChip = this.Page.Locator(".weave-chip[role='button']").Nth(1)
    do! firstChip.FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(secondChip).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowLeft moves focus to previous chip``() = task {
    do! this.NavigateToChipSet()
    let secondChip = this.Page.Locator(".weave-chip[role='button']").Nth(1)
    let firstChip = this.Page.Locator(".weave-chip[role='button']").First
    do! secondChip.FocusAsync()
    do! this.Expect(secondChip).ToBeFocusedAsync()
    do! this.Expect(secondChip).ToHaveAttributeAsync("tabindex", "0")
    do! this.Page.Keyboard.PressAsync("ArrowLeft")
    do! this.Expect(firstChip).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowRight wraps from last to first``() = task {
    do! this.NavigateToChipSet()
    // There are 4 chips (A, B, C, Disabled). The selector excludes disabled,
    // so 3 enabled chips at indices 0, 1, 2.
    let lastEnabled =
      this.Page.Locator(".weave-chip[role='button']:not(.weave-chip--disabled)").Nth(2)

    let firstChip = this.Page.Locator(".weave-chip[role='button']").First
    do! lastEnabled.FocusAsync()
    do! this.Expect(lastEnabled).ToBeFocusedAsync()
    do! this.Expect(lastEnabled).ToHaveAttributeAsync("tabindex", "0")
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(firstChip).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Home moves focus to first chip``() = task {
    do! this.NavigateToChipSet()
    let secondChip = this.Page.Locator(".weave-chip[role='button']").Nth(1)
    let firstChip = this.Page.Locator(".weave-chip[role='button']").First
    do! secondChip.FocusAsync()
    do! this.Expect(secondChip).ToBeFocusedAsync()
    do! this.Expect(secondChip).ToHaveAttributeAsync("tabindex", "0")
    do! this.Page.Keyboard.PressAsync("Home")
    do! this.Expect(firstChip).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``End moves focus to last enabled chip``() = task {
    do! this.NavigateToChipSet()
    let firstChip = this.Page.Locator(".weave-chip[role='button']").First

    let lastEnabled =
      this.Page.Locator(".weave-chip[role='button']:not(.weave-chip--disabled)").Nth(2)

    do! firstChip.FocusAsync()
    do! this.Expect(firstChip).ToBeFocusedAsync()
    do! this.Expect(firstChip).ToHaveAttributeAsync("tabindex", "0")
    do! this.Page.Keyboard.PressAsync("End")
    do! this.Expect(lastEnabled).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter selects a chip``() = task {
    do! this.NavigateToChipSet()
    let firstChip = this.Page.Locator(".weave-chip[role='button']").First
    do! firstChip.FocusAsync()
    do! firstChip.PressAsync("Enter")
    let selection = this.Page.Locator("[data-testid='chipset-selection']")
    do! this.Expect(selection).ToHaveTextAsync("a")
  }

  [<Fact>]
  member this.``Space selects a chip``() = task {
    do! this.NavigateToChipSet()
    let secondChip = this.Page.Locator(".weave-chip[role='button']").Nth(1)
    do! secondChip.FocusAsync()
    do! secondChip.PressAsync(" ")
    let selection = this.Page.Locator("[data-testid='chipset-selection']")
    do! this.Expect(selection).ToHaveTextAsync("b")
  }

  [<Fact>]
  member this.``roving tabindex sets first chip to 0 and others to -1``() = task {
    do! this.NavigateToChipSet()
    let firstChip = this.Page.Locator(".weave-chip[role='button']").First
    let secondChip = this.Page.Locator(".weave-chip[role='button']").Nth(1)
    do! this.Expect(firstChip).ToHaveAttributeAsync("tabindex", "0")
    do! this.Expect(secondChip).ToHaveAttributeAsync("tabindex", "-1")
  }
