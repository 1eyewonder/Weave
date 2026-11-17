namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open System.Threading.Tasks
open Microsoft.Playwright
open Xunit

type SliderInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("integer", "5", "5", "5", "1")>]
  [<InlineData("fraction", "0.001", "0.002", "0.004", "0.001")>]
  member this.``slider model and accessible values agree across initial and external range changes``
    (id: string, initial: string, afterRight: string, maximum: string, step: string)
    = task {
    do! this.NavigateTo("slider-range")
    let root = this.Page.GetByTestId(id)
    let model = root.Locator("output")
    let slider = root.GetByRole(AriaRole.Slider)

    let expectValue (value: string) = task {
      do! this.Expect(model).ToHaveTextAsync(value)
      do! this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", value)
    }

    do! expectValue initial
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuemin", "0")
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuemax", maximum)
    do! slider.PressAsync("ArrowRight")
    do! expectValue afterRight
    do! this.Page.GetByText("Set below minimum").ClickAsync()
    do! expectValue "0"

    let! low =
      root.Locator(".weave-slider__thumb").EvaluateAsync<float>("el => parseFloat(el.style.left)")

    Assert.Equal(0.0, low)
    do! slider.PressAsync("ArrowRight")
    do! expectValue step
    do! this.Page.GetByText("Set above maximum").ClickAsync()
    do! expectValue maximum

    let! high =
      root.Locator(".weave-slider__thumb").EvaluateAsync<float>("el => parseFloat(el.style.left)")

    Assert.Equal(100.0, high)
  }

  member private this.TickPositions(slider: ILocator) =
    slider
      .Locator(".weave-slider__tick")
      .EvaluateAllAsync<float array>("ticks => ticks.map(tick => parseFloat(tick.style.left))")

  [<Fact>]
  member this.``a NaN assignment resets the fractional slider to its minimum``() = task {
    do! this.NavigateTo("slider-range")
    do! this.Page.GetByText("Set fraction to NaN").ClickAsync()
    let root = this.Page.GetByTestId("fraction")
    do! this.Expect(root.Locator("output")).ToHaveTextAsync("0")
    do! this.Expect(root.GetByRole(AriaRole.Slider)).ToHaveAttributeAsync("aria-valuenow", "0")
  }

  member private this.AdjustWithPointerThenKeyboard(page: IPage, activate: ILocator -> Task) = task {
    do! this.NavigatePage(page, "slider")

    let previous =
      page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = "Default Slider"))

    let adjusted =
      page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = "With Ticks"))

    do! previous.FocusAsync()
    do! activate adjusted
    do! Assertions.Expect(adjusted).ToHaveAttributeAsync("aria-valuenow", "5")
    do! Assertions.Expect(adjusted).ToBeFocusedAsync()
    do! page.Keyboard.PressAsync("ArrowRight")
    do! Assertions.Expect(adjusted).ToHaveAttributeAsync("aria-valuenow", "6")
    do! Assertions.Expect(previous).ToHaveAttributeAsync("aria-valuenow", "50")
  }

  [<Fact>]
  member this.``clicking the track focuses the adjusted slider for the next arrow key``() =
    this.AdjustWithPointerThenKeyboard(this.Page, fun slider -> slider.ClickAsync())

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``tapping the track focuses the adjusted slider for the next arrow key``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.AdjustWithPointerThenKeyboard(page, fun slider -> slider.TapAsync())
  }

  [<Theory>]
  [<InlineData("slider-fine-ticks")>]
  [<InlineData("slider-tiny-step")>]
  [<InlineData("slider-wide-range")>]
  member this.``fine and extreme grids show bounded distinct marks spanning the track``(pageName: string) =
    task {
      do! this.NavigateTo(pageName)
      let! positions = this.TickPositions(this.Page.GetByRole(AriaRole.Slider))
      Assert.InRange(positions.Length, 3, 21)
      Assert.Equal(0.0, Array.head positions)
      Assert.Equal(100.0, Array.last positions)
      Assert.All(Array.pairwise positions, fun (left, right) -> Assert.True(left < right))
    }

  [<Fact>]
  member this.``a sampled grid stops at the last valid step``() = task {
    do! this.NavigateTo("slider-uneven-ticks")
    let! positions = this.TickPositions(this.Page.GetByRole(AriaRole.Slider))
    Assert.InRange(positions.Length, 2, 21)
    Assert.Equal(99.0, Array.last positions)
    Assert.All(positions, fun position -> Assert.Equal(round (position / 3.0) * 3.0, position, 4))
  }

  [<Fact>]
  member this.``stepping across a visible tick updates its state and keeps the original nodes``() = task {
    do! this.NavigateTo("slider-fine-ticks")
    let slider = this.Page.GetByRole(AriaRole.Slider)
    let ticks = slider.Locator(".weave-slider__tick")
    let! count = ticks.CountAsync()
    let middle = ticks.Nth(count / 2)
    let active = Regex("weave-slider__tick--active")
    do! this.Expect(middle).Not.ToHaveClassAsync(active)

    let container = slider.Locator(".weave-slider__tick-container")
    let! original = container.EvaluateHandleAsync("el => Array.from(el.children)")

    do! slider.PressAsync("ArrowRight")
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", "50")
    do! this.Expect(middle).ToHaveClassAsync(active)
    do! slider.PressAsync("ArrowLeft")
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", "49.99")
    do! this.Expect(middle).Not.ToHaveClassAsync(active)

    let! keptNodes =
      container.EvaluateAsync<bool>(
        "(el, ticks) => ticks.length === el.childElementCount && ticks.every((tick, i) => tick === el.children[i])",
        original
      )

    Assert.True(keptNodes, "Changing the slider value must preserve the rendered tick nodes")
    do! original.DisposeAsync()
  }

  [<Fact>]
  member this.``a range wider than the largest float places the thumb at the active middle mark``() = task {
    do! this.NavigateTo("slider-wide-range")
    let slider = this.Page.GetByRole(AriaRole.Slider)

    let! thumb =
      slider.Locator(".weave-slider__thumb").EvaluateAsync<float>("el => parseFloat(el.style.left)")

    let! activeEnd =
      slider
        .Locator(".weave-slider__tick--active")
        .Last.EvaluateAsync<float>("el => parseFloat(el.style.left)")

    Assert.Equal(50.0, thumb)
    Assert.Equal(50.0, activeEnd)
  }

  [<Fact>]
  member this.``a coarse slider keeps each step and its caller-supplied labels``() = task {
    do! this.NavigateTo("slider")

    let slider =
      this.Page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = "With Ticks"))

    let! positions = this.TickPositions(slider)
    Assert.Equal<float>([| 0.0..10.0..100.0 |], positions)
    do! this.Expect(this.Page.Locator(".weave-slider__tick-label")).ToHaveTextAsync([| "0"; "5"; "10" |])
  }

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

// Firefox can route clicks from other pages to the slider while it holds pointer capture.
[<Collection("Browser input")>]
type SliderPointerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``disabling a slider during a pointer adjustment keeps focus on the previous control``() = task {
    do! this.NavigateTo("slider-dynamic")
    let slider = this.Page.GetByRole(AriaRole.Slider)
    let toggle = this.Page.GetByTestId("toggle-slider-enabled")
    do! toggle.FocusAsync()
    let! track = slider.BoundingBoxAsync()
    do! this.Page.Mouse.MoveAsync(track.X + track.Width / 4.0f, track.Y + track.Height / 2.0f)
    do! this.Page.Mouse.DownAsync()

    try
      do! this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", "25")
      let! _ = toggle.EvaluateAsync("button => button.click()")
      do! this.Expect(slider).ToHaveAttributeAsync("aria-disabled", "true")
    with ex ->
      do! this.Page.Mouse.UpAsync()
      return raise ex

    do! this.Page.Mouse.UpAsync()
    do! this.Expect(toggle).ToBeFocusedAsync()
  }
