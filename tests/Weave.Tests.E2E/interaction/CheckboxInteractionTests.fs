namespace Weave.Tests.E2E

open Xunit

type CheckboxInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``checkbox input is focusable``() = task {
    do! this.NavigateTo("checkbox")
    let input = this.Page.Locator(".weave-checkbox__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Space toggles an unchecked checkbox to checked``() = task {
    do! this.NavigateTo("checkbox")
    let input = this.Page.Locator(".weave-checkbox__input").First
    do! input.FocusAsync()
    do! input.PressAsync("Space")
    do! this.Expect(input).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``disabled checkbox is not focusable``() = task {
    do! this.NavigateTo("checkbox")
    let disabledInput = this.Page.Locator(".weave-checkbox__input[disabled]").First
    do! disabledInput.FocusAsync()
    do! this.Expect(disabledInput).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``focus-visible outline appears on the span when input is focused``() = task {
    do! this.NavigateTo("checkbox")
    let input = this.Page.Locator(".weave-checkbox__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
    let span = this.Page.Locator(".weave-checkbox__span").First

    let! outlineStyle =
      span.EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")

    Assert.NotEqual<string>("none", outlineStyle)
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``checked color marks contrast with their fills``(dark: bool) = task {
    do! this.NavigateTo("checkbox")

    if dark then
      let! _ =
        this.Page.EvaluateAsync<obj>("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let! _ =
      this.Page.AddStyleTagAsync(
        Microsoft.Playwright.PageAddStyleTagOptions(
          Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
        )
      )

    for color in [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ] do
      let span =
        this.Page.Locator($"[data-testid='checkbox-color-{color}'] .weave-checkbox__span")

      let! contrast =
        span.EvaluateAsync<float>(
          """el => {
        const rgb = s => s.match(/[\d.]+/g).slice(0, 3).map(Number);
        const lum = c => c.map(v => v / 255).map(v => v <= 0.04045 ? v / 12.92 : ((v + 0.055) / 1.055) ** 2.4)
          .reduce((sum, v, i) => sum + v * [0.2126, 0.7152, 0.0722][i], 0);
        const a = lum(rgb(getComputedStyle(el).backgroundColor));
        const b = lum(rgb(getComputedStyle(el, '::after').borderBottomColor));
        return (Math.max(a, b) + 0.05) / (Math.min(a, b) + 0.05);
      }"""
        )

      Assert.True(contrast >= 3.0, $"{color} checkmark contrast was {contrast:F2}:1 (dark={dark})")
  }

  [<Fact>]
  member this.``disabled checked checkbox retains a visible checkmark``() = task {
    do! this.NavigateTo("checkbox")
    let root = this.Page.GetByTestId("disabled-checked-checkbox")
    do! this.Expect(root.Locator("input")).ToBeCheckedAsync()
    do! this.Expect(root.Locator("input")).ToBeDisabledAsync()

    let! visible =
      root
        .Locator(".weave-checkbox__span")
        .EvaluateAsync<bool>(
          """el => {
      const mark = getComputedStyle(el, '::after');
      return Number(mark.opacity) > 0 && new DOMMatrix(mark.transform).a !== 0;
    }"""
        )

    Assert.True(visible, "A disabled checked checkbox must retain its checkmark")
  }
