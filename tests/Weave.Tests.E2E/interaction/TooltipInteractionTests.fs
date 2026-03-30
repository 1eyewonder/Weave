namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type TooltipInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``every tooltip wrapper contains a role tooltip element``() = task {
    do! this.NavigateTo("tooltip")
    let! wrapperCount = this.Page.Locator(".weave-tooltip-root").CountAsync()
    let tooltips = this.Page.Locator("[role='tooltip']")
    do! this.Expect(tooltips).ToHaveCountAsync(wrapperCount)
  }

  [<Fact>]
  member this.``wrapper has aria-describedby pointing to tooltip id``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    do! this.Expect(wrapper).ToHaveAttributeAsync("aria-describedby", Regex("weave-tooltip-\\d+"))
    let! describedBy = wrapper.GetAttributeAsync("aria-describedby")
    let tooltip = this.Page.Locator($"#{describedBy}")
    do! this.Expect(tooltip).ToHaveAttributeAsync("role", "tooltip")
  }

  [<Fact>]
  member this.``tooltip is hidden by default``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-default'] [role='tooltip']")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``hover shows the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``focus shows the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Escape dismisses tooltip shown via focus``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``Escape dismisses tooltip shown via hover when trigger is focused``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! focusable.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``focus remains on trigger after Escape``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(focusable).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``mouse leave hides the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.Page.Mouse.MoveAsync(640.0f, 600.0f)
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``focusOut hides the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``click shows the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let button = wrapper.Locator(".weave-button").First
    do! button.ClickAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``click again hides the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let button = wrapper.Locator(".weave-button").First
    do! button.ClickAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! button.ClickAsync()
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``hover does not show the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``focus does not show the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``default tooltip has top-center direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-default'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--top-center"))
  }

  [<Fact>]
  member this.``bottom tooltip has bottom-center direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-bottom'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--bottom-center"))
  }

  [<Fact>]
  member this.``left tooltip has center-left direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-left'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--center-left"))
  }

  [<Fact>]
  member this.``right tooltip has center-right direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-right'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--center-right"))
  }

  [<Fact>]
  member this.``default tooltip has arrow class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-default'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--arrow"))
  }

  [<Fact>]
  member this.``no-arrow tooltip lacks arrow class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-no-arrow'] [role='tooltip']")
    do! this.Expect(tooltip).Not.ToHaveClassAsync(Regex("weave-tooltip--arrow"))
  }

  [<Fact>]
  member this.``color tooltip has the specified palette class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-primary'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--primary"))
  }
