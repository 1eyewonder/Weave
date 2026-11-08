namespace Weave.Tests.E2E

open Microsoft.Playwright
open global.Xunit

type ThemingInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``mode helpers switch the rendered CSS defaults``() = task {
    do! this.NavigateTo("theme")
    let swatch = this.Page.GetByTestId("theme-swatch")

    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-theme", "light")

    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(1, 100, 252)")
    do! this.Page.GetByTestId("theme-toggle").ClickAsync()

    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-theme", "dark")

    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-observed-mode", "dark")

    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(8, 143, 253)")
    do! this.Page.GetByTestId("theme-light").ClickAsync()

    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-observed-mode", "light")

    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(1, 100, 252)")
    do! this.Page.GetByTestId("theme-dark").ClickAsync()

    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-observed-mode", "dark")
  }

  [<Fact>]
  member this.``application CSS customizes component tokens in both modes``() = task {
    do! this.NavigateTo("theme")

    let css =
      """
      :root { --weave-font-family-body: Georgia; }
      body[data-theme=light] {
        --palette-primary: #123456; --palette-primary-rgb: 18, 52, 86;
        --palette-surface: #ddeeff; --palette-text-primary-rgb: 18, 52, 86;
      }
      body[data-theme=dark] {
        --palette-primary: #654321; --palette-primary-rgb: 101, 67, 33;
        --palette-surface: #202333; --palette-text-primary-rgb: 230, 235, 245;
        --typography-body1-family: Arial;
      }
    """

    let! _ = this.Page.AddStyleTagAsync(PageAddStyleTagOptions(Content = css))
    let swatch = this.Page.GetByTestId("theme-swatch")
    let card = this.Page.GetByTestId("theme-card")
    let body = this.Page.GetByTestId("theme-body")
    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(18, 52, 86)")
    do! this.Expect(card).ToHaveCSSAsync("background-color", "rgb(221, 238, 255)")
    do! this.Expect(card).ToHaveCSSAsync("border-top-color", "rgba(18, 52, 86, 0.12)")
    do! this.Expect(body).ToHaveCSSAsync("font-family", "Georgia")
    do! this.Page.GetByTestId("theme-toggle").ClickAsync()
    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(101, 67, 33)")
    do! this.Expect(card).ToHaveCSSAsync("background-color", "rgb(32, 35, 51)")

    do! this.Expect(card).ToHaveCSSAsync("border-top-color", "rgba(230, 235, 245, 0.12)")

    do! this.Expect(body).ToHaveCSSAsync("font-family", "Arial")
    do! this.Page.GetByTestId("theme-toggle").ClickAsync()
    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(18, 52, 86)")
  }

  [<Fact>]
  member this.``mode changes preserve application inline variables``() = task {
    do! this.NavigateTo("theme")

    let! _ =
      this.Page.EvaluateAsync("() => document.body.style.setProperty('--palette-primary', '#456789')")

    do! this.Page.GetByTestId("theme-toggle").ClickAsync()

    do!
      this
        .Expect(this.Page.GetByTestId("theme-swatch"))
        .ToHaveCSSAsync("background-color", "rgb(69, 103, 137)")

    let! value =
      this.Page.EvaluateAsync<string>("() => document.body.style.getPropertyValue('--palette-primary')")

    Assert.Equal("#456789", value)
  }

  [<Fact>]
  member this.``missing mode is treated as light``() = task {
    do! this.NavigateTo("theme")
    let! _ = this.Page.EvaluateAsync("() => document.body.removeAttribute('data-theme')")
    do! this.Page.GetByTestId("theme-toggle").ClickAsync()

    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-theme", "dark")

    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-observed-mode", "dark")
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``system preference selects matching mode``(mode: string) = task {
    let scheme =
      if mode = "dark" then
        ColorScheme.Dark
      else
        ColorScheme.Light

    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ColorScheme = scheme))
    do! this.NavigateTo("theme-system")
    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-theme", mode)
  }
