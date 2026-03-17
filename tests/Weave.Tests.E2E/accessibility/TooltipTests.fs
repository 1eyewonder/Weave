namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type TooltipTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("tooltip")

  [<Fact>]
  member this.``tooltip content has role tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[role='tooltip']").First
    do! this.Expect(tooltip).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``wrapper has aria-describedby pointing to tooltip id``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator(".weave-tooltip-root").First
    do! this.Expect(wrapper).ToHaveAttributeAsync("aria-describedby", Regex("weave-tooltip-\\d+"))
    let! describedBy = wrapper.GetAttributeAsync("aria-describedby")
    let tooltip = this.Page.Locator($"#{describedBy}")
    do! this.Expect(tooltip).ToHaveAttributeAsync("role", "tooltip")
  }

  [<Fact>]
  member this.``tooltip is hidden by default``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[role='tooltip']").First
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``hover shows the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator(".weave-tooltip-root").First
    let tooltip = this.Page.Locator("[role='tooltip']").First
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
  }
