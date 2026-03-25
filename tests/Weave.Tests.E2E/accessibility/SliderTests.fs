namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type SliderTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("slider")

  [<Fact>]
  member this.``slider track container has role slider``() = task {
    do! this.NavigateTo("slider")
    let slider = this.Page.Locator("[role='slider']").First
    do! this.Expect(slider).ToHaveAttributeAsync("role", "slider")
  }

  [<Fact>]
  member this.``slider has aria-valuenow``() = task {
    do! this.NavigateTo("slider")
    let slider = this.Page.Locator("[role='slider']").First
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", Regex(".+"))
  }

  [<Fact>]
  member this.``slider has aria-valuemin``() = task {
    do! this.NavigateTo("slider")
    let slider = this.Page.Locator("[role='slider']").First
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuemin", Regex(".+"))
  }

  [<Fact>]
  member this.``slider has aria-valuemax``() = task {
    do! this.NavigateTo("slider")
    let slider = this.Page.Locator("[role='slider']").First
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuemax", Regex(".+"))
  }

  [<Fact>]
  member this.``slider is focusable``() = task {
    do! this.NavigateTo("slider")
    let slider = this.Page.Locator("[role='slider']").First
    do! slider.FocusAsync()
    do! this.Expect(slider).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled slider is not interactive``() = task {
    do! this.NavigateTo("slider")
    let disabledSlider = this.Page.Locator(".weave-slider--disabled")
    do! this.Expect(disabledSlider).ToHaveCountAsync(1)
  }
