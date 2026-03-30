namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type TabsInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``tab header has role tablist``() = task {
    do! this.NavigateTo("tabs")
    let tablist = this.Page.Locator(".weave-tabs__header[role='tablist']").First
    do! this.Expect(tablist).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``tab buttons have role tab``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! this.Expect(firstTab).ToHaveAttributeAsync("role", "tab")
  }

  [<Fact>]
  member this.``panels have role tabpanel``() = task {
    do! this.NavigateTo("tabs")
    let panels = this.Page.Locator(".weave-tabs__panel[role='tabpanel']")
    do! this.Expect(panels.First).ToBeAttachedAsync()
  }

  [<Fact>]
  member this.``active tab has aria-selected true``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! this.Expect(firstTab).ToHaveAttributeAsync("aria-selected", "true")
  }

  [<Fact>]
  member this.``inactive tab has aria-selected false``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! this.Expect(secondTab).ToHaveAttributeAsync("aria-selected", "false")
  }

  [<Fact>]
  member this.``active tab has tabindex 0``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! this.Expect(firstTab).ToHaveAttributeAsync("tabindex", "0")
  }

  [<Fact>]
  member this.``inactive tab has tabindex -1``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! this.Expect(secondTab).ToHaveAttributeAsync("tabindex", "-1")
  }

  [<Fact>]
  member this.``tab has aria-controls linking to panel``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    let! ariaControls = firstTab.GetAttributeAsync("aria-controls")
    Assert.NotNull(ariaControls)
    let panel = this.Page.Locator($"#{ariaControls}")
    do! this.Expect(panel).ToBeAttachedAsync()
    do! this.Expect(panel).ToHaveAttributeAsync("role", "tabpanel")
  }

  [<Fact>]
  member this.``tab button is focusable``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! firstTab.FocusAsync()
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter activates a tab``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")
    do! this.Expect(secondTab).ToHaveClassAsync(Regex("weave-tabs__tab--active"))
  }

  [<Fact>]
  member this.``Space activates a tab``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync(" ")
    do! this.Expect(secondTab).ToHaveClassAsync(Regex("weave-tabs__tab--active"))
  }

  [<Fact>]
  member this.``ArrowRight moves focus to the next tab``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! firstTab.FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(secondTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowLeft moves focus to the previous tab``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowLeft")
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowRight wraps from last enabled tab to first tab``() = task {
    do! this.NavigateTo("tabs")
    let tabsContainer = this.Page.Locator(".weave-tabs").First
    let tabs = tabsContainer.Locator("[role='tab']:not([disabled])")
    let! count = tabs.CountAsync()
    let lastEnabledTab = tabs.Nth(count - 1)
    let firstTab = tabs.First
    do! lastEnabledTab.FocusAsync()
    do! lastEnabledTab.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Home moves focus to the first tab``() = task {
    do! this.NavigateTo("tabs")
    let tabsContainer = this.Page.Locator(".weave-tabs").First
    let tabs = tabsContainer.Locator("[role='tab']")
    let secondTab = tabs.Nth(1)
    let firstTab = tabs.First
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("Home")
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``End moves focus to the last enabled tab``() = task {
    do! this.NavigateTo("tabs")
    let tabsContainer = this.Page.Locator(".weave-tabs").First
    let tabs = tabsContainer.Locator("[role='tab']:not([disabled])")
    let! count = tabs.CountAsync()
    let firstTab = tabs.First
    let lastEnabledTab = tabs.Nth(count - 1)
    do! firstTab.FocusAsync()
    do! this.Page.Keyboard.PressAsync("End")
    do! this.Expect(lastEnabledTab).ToBeFocusedAsync()
  }
