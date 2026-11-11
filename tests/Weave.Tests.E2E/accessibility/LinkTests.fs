namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

type LinkTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("link")

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``link text stays readable on paper through hover in both themes``(dark: bool) = task {
    do! this.NavigateTo("link")

    let! _ =
      this.Page.AddStyleTagAsync(
        PageAddStyleTagOptions(
          Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
        )
      )

    if dark then
      let! _ =
        this.Page.EvaluateAsync("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let contrast (link: ILocator) =
      link.EvaluateAsync<float>(
        """el => {
          const channels = value => value.match(/[\d.]+/g).map(Number);
          const luminance = rgb => rgb.slice(0, 3).map(value => value / 255)
            .map(value => value <= 0.04045 ? value / 12.92 : ((value + 0.055) / 1.055) ** 2.4)
            .reduce((sum, value, i) => sum + value * [0.2126, 0.7152, 0.0722][i], 0);
          const style = getComputedStyle(el);
          const foreground = channels(style.color);
          const background = channels(getComputedStyle(el.parentElement).backgroundColor);
          const alpha = (foreground[3] ?? 1) * Number(style.opacity);
          const rendered = foreground.slice(0, 3).map((value, i) => value * alpha + background[i] * (1 - alpha));
          const a = luminance(rendered), b = luminance(background);
          return (Math.max(a, b) + 0.05) / (Math.min(a, b) + 0.05);
        }"""
      )

    for name in
      [
        "default"
        "primary"
        "secondary"
        "tertiary"
        "success"
        "error"
        "warning"
        "info"
      ] do
      let link = this.Page.GetByTestId("paper-link-" + name)
      do! this.Expect(link).ToHaveAccessibleNameAsync("Readable " + name + " link")
      do! this.Expect(link).ToHaveAttributeAsync("href", "#button")
      do! this.Page.Mouse.MoveAsync(1.0f, 1.0f)
      let! normal = contrast link
      Assert.True(normal >= 4.5, $"{name} link contrast was {normal:F2}:1 (dark={dark})")
      do! link.HoverAsync()
      let! hover = contrast link
      Assert.True(hover >= 4.5, $"{name} hovered link contrast was {hover:F2}:1 (dark={dark})")

    let defaultLink = this.Page.GetByTestId("paper-link-default")
    do! defaultLink.FocusAsync()
    do! this.Expect(defaultLink).ToBeFocusedAsync()
    do! defaultLink.PressAsync("Enter")
    Assert.EndsWith("#button", this.Page.Url)
  }
