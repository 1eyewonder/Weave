namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit

type SliderInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

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
    let disabledSlider = this.Page.Locator(".weave-slider--disabled [role='slider']")
    do! this.Expect(disabledSlider).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(disabledSlider).ToHaveAttributeAsync("tabindex", "-1")
    let! before = disabledSlider.GetAttributeAsync("aria-valuenow")

    let! _ =
      disabledSlider.EvaluateAsync(
        "el => el.dispatchEvent(new KeyboardEvent('keydown', { key: 'ArrowRight', bubbles: true }))"
      )

    do! this.Expect(disabledSlider).ToHaveAttributeAsync("aria-valuenow", before)
  }

  [<Fact>]
  member this.``keyboard focused slider shows a visible outline``() = task {
    do! this.NavigateTo("slider")
    let slider = this.Page.Locator("[role='slider']").First
    do! slider.FocusAsync()
    let! style = slider.EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")
    Assert.NotEqual<string>("none", style)
  }

  [<Fact>]
  member this.``slider responds to reactive enabled changes``() = task {
    do! this.NavigateTo("slider-dynamic")
    let slider = this.Page.GetByRole(Microsoft.Playwright.AriaRole.Slider)
    let toggle = this.Page.GetByTestId("toggle-slider-enabled")
    do! this.Expect(slider).ToHaveAttributeAsync("tabindex", "0")
    do! toggle.ClickAsync()
    do! this.Expect(slider).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(slider).ToHaveAttributeAsync("tabindex", "-1")
    let! before = slider.GetAttributeAsync("aria-valuenow")

    let! _ =
      slider.EvaluateAsync(
        "el => el.dispatchEvent(new KeyboardEvent('keydown', { key: 'ArrowRight', bubbles: true }))"
      )

    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", before)
    do! toggle.ClickAsync()
    do! this.Expect(slider).ToHaveAttributeAsync("aria-disabled", "false")
    do! this.Expect(slider).ToHaveAttributeAsync("tabindex", "0")
    do! slider.PressAsync("ArrowRight")
    do! this.Expect(slider).Not.ToHaveAttributeAsync("aria-valuenow", before)
  }
