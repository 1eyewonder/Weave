namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit

type ExpansionPanelInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``programmatic click toggles expansion``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(1)
    let! _ = header.EvaluateAsync("el => el.click()")
    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "true")
  }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap toggles expansion``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "expansion-panel")
    let header = page.Locator(".weave-expansion-panel__header").Nth(1)
    do! header.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(header).ToHaveAttributeAsync("aria-expanded", "true")
  }

  [<Fact>]
  member this.``expansion panel header is reachable by Tab``() = task {
    do! this.NavigateTo("expansion-panel")
    do! this.Page.Keyboard.PressAsync("Tab")
    let header = this.Page.Locator(".weave-expansion-panel__header").First
    do! this.Expect(header).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter toggles the expansion panel``() = task {
    do! this.NavigateTo("expansion-panel")
    // The second panel starts collapsed
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(1)
    do! header.FocusAsync()
    do! header.PressAsync("Enter")
    let panel = this.Page.Locator(".weave-expansion-panel").Nth(1)
    do! this.Expect(panel).ToHaveClassAsync(Regex("weave-expansion-panel--expanded"))
  }

  [<Fact>]
  member this.``Space toggles the expansion panel``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(1)
    do! header.FocusAsync()
    do! header.PressAsync(" ")
    let panel = this.Page.Locator(".weave-expansion-panel").Nth(1)
    do! this.Expect(panel).ToHaveClassAsync(Regex("weave-expansion-panel--expanded"))
  }

  [<Fact>]
  member this.``expanded header has aria-expanded true``() = task {
    do! this.NavigateTo("expansion-panel")
    // First panel starts expanded
    let header = this.Page.Locator(".weave-expansion-panel__header").First
    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "true")
  }

  [<Fact>]
  member this.``collapsed header has aria-expanded false``() = task {
    do! this.NavigateTo("expansion-panel")
    // Second panel starts collapsed
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(1)
    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "false")
  }

  [<Fact>]
  member this.``header aria-controls matches content region id``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.Locator(".weave-expansion-panel__header").First
    do! this.Expect(header).ToHaveAttributeAsync("aria-controls", System.Text.RegularExpressions.Regex(".+"))
    let! controlsId = header.GetAttributeAsync("aria-controls")
    let region = this.Page.Locator(".weave-expansion-panel__content-wrapper").First
    do! this.Expect(region).ToHaveAttributeAsync("id", controlsId)
  }

  [<Fact>]
  member this.``content region has aria-labelledby matching header id``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.Locator(".weave-expansion-panel__header").First
    do! this.Expect(header).ToHaveAttributeAsync("id", System.Text.RegularExpressions.Regex(".+"))
    let! headerId = header.GetAttributeAsync("id")
    let region = this.Page.Locator(".weave-expansion-panel__content-wrapper").First
    do! this.Expect(region).ToHaveAttributeAsync("aria-labelledby", headerId)
  }

  [<Fact>]
  member this.``content wrapper has role region``() = task {
    do! this.NavigateTo("expansion-panel")
    let region = this.Page.Locator(".weave-expansion-panel__content-wrapper").First
    do! this.Expect(region).ToHaveAttributeAsync("role", "region")
  }

  [<Fact>]
  member this.``focus color class is present on header with custom focus color``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(2)
    do! this.Expect(header).ToHaveClassAsync(Regex("weave-expansion-panel__header--focus-error"))
  }

  [<Fact>]
  member this.``collapsed content is removed from keyboard navigation until expanded``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.Locator(".weave-expansion-panel__header").Nth(1)
    let action = this.Page.GetByTestId("collapsed-panel-action")
    do! header.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(action).Not.ToBeFocusedAsync()
    do! header.PressAsync("Enter")
    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "true")
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(action).ToBeFocusedAsync()
    do! header.PressAsync("Enter")
    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "false")
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(action).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled expansion header is not tabbable or activatable``() = task {
    do! this.NavigateTo("expansion-panel")
    let header = this.Page.GetByTestId("disabled-expansion-header")
    do! this.Expect(header).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(header).ToHaveAttributeAsync("tabindex", "-1")

    let! _ =
      header.EvaluateAsync(
        "el => { el.click(); el.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true })); }"
      )

    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "false")
  }

  [<Fact>]
  member this.``panel and header enabled states combine reactively``() = task {
    do! this.NavigateTo("expansion-panel-dynamic")
    let header = this.Page.GetByTestId("dynamic-expansion-header")
    let panelToggle = this.Page.GetByTestId("toggle-panel-enabled")
    let headerToggle = this.Page.GetByTestId("toggle-header-enabled")
    do! panelToggle.ClickAsync()
    do! this.Expect(header).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(header).ToHaveAttributeAsync("tabindex", "-1")
    let! _ = header.EvaluateAsync("el => el.click()")
    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "false")
    do! headerToggle.ClickAsync()
    do! panelToggle.ClickAsync()
    do! this.Expect(header).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(header).ToHaveAttributeAsync("tabindex", "-1")
    do! headerToggle.ClickAsync()
    do! this.Expect(header).ToHaveAttributeAsync("aria-disabled", "false")
    do! this.Expect(header).ToHaveAttributeAsync("tabindex", "0")
    do! header.PressAsync("Enter")
    do! this.Expect(header).ToHaveAttributeAsync("aria-expanded", "true")
  }
