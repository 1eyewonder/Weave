namespace Weave.Tests.E2E

open Microsoft.Playwright
open global.Xunit

[<Collection("E2E")>]
type ThemingInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``custom primary color reaches components``() = task {
    do! this.NavigateTo("theming")

    do!
      this
        .Expect(this.Page.GetByTestId("themed-button"))
        .ToHaveCSSAsync("background-color", "rgb(124, 58, 237)")
  }

  [<Fact>]
  member this.``switching to an empty palette clears prior overrides``() = task {
    do! this.NavigateTo("theme-switch")

    let! before =
      this.Page.EvaluateAsync<string>("() => document.body.style.getPropertyValue('--palette-primary')")

    Assert.Equal("#123456", before)
    do! this.Page.GetByTestId("theme-dark").ClickAsync()

    let! values =
      this.Page.EvaluateAsync<string[]>(
        "() => [document.body.style.getPropertyValue('--palette-primary'), document.documentElement.style.getPropertyValue('--palette-primary'), document.body.style.getPropertyValue('--palette-background'), document.body.style.getPropertyValue('--input-label-background')]"
      )

    Assert.All(values, fun value -> Assert.Equal("", value))
    do! this.Page.GetByTestId("theme-light").ClickAsync()

    let! restored =
      this.Page.EvaluateAsync<string>("() => document.body.style.getPropertyValue('--palette-primary')")

    Assert.Equal("#123456", restored)
  }

  [<Fact>]
  member this.``populated palettes switch component colors and restore them``() = task {
    do! this.NavigateTo("theme-coverage")
    let swatch = this.Page.GetByTestId("theme-swatch")
    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(18, 52, 86)")
    do! this.Page.GetByTestId("coverage-dark").ClickAsync()
    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(135, 101, 67)")
    do! this.Page.GetByTestId("coverage-light").ClickAsync()
    do! this.Expect(swatch).ToHaveCSSAsync("background-color", "rgb(18, 52, 86)")
  }

  [<Fact>]
  member this.``partial palette uses new values and falls back for omitted colors``() = task {
    do! this.NavigateTo("theme-coverage")
    let secondary = this.Page.GetByTestId("theme-secondary")
    do! this.Expect(secondary).ToHaveCSSAsync("color", "rgb(170, 187, 204)")
    do! this.Page.GetByTestId("coverage-dark").ClickAsync()
    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-theme", "dark")
    do! this.Expect(secondary).ToHaveCSSAsync("color", "rgb(89, 74, 226)")

    do!
      this
        .Expect(this.Page.GetByTestId("theme-swatch"))
        .ToHaveCSSAsync("background-color", "rgb(135, 101, 67)")
  }

  [<Fact>]
  member this.``empty palette uses the dark CSS default on a component``() = task {
    do! this.NavigateTo("theme-switch")
    do! this.Page.GetByTestId("theme-dark").ClickAsync()

    do!
      this
        .Expect(this.Page.GetByTestId("fallback-swatch"))
        .ToHaveCSSAsync("background-color", "rgb(8, 143, 253)")
  }

  [<Fact>]
  member this.``switching colors updates and clears all palette color values``() = task {
    do! this.NavigateTo("theme-coverage")

    let readValues () =
      this.Page.EvaluateAsync<string[]>(
        "() => ['', '-text', '-lighten', '-darken', '-rgb'].map(s => document.body.style.getPropertyValue('--palette-primary' + s))"
      )

    let! light = readValues ()
    Assert.Equal<string>([| "#123456"; "#fefefe"; "#234567"; "#012345"; "18, 52, 86" |], light)
    do! this.Page.GetByTestId("coverage-dark").ClickAsync()
    let! dark = readValues ()
    Assert.Equal<string>([| "#876543"; "#101010"; "#987654"; "#765432"; "135, 101, 67" |], dark)
    do! this.Page.GetByTestId("coverage-empty").ClickAsync()
    let! cleared = readValues ()
    Assert.All(cleared, fun value -> Assert.Equal("", value))
  }

  [<Theory>]
  [<InlineData("light", "rgb(18, 52, 86)")>]
  [<InlineData("dark", "rgb(135, 101, 67)")>]
  member this.``system preference selects matching palette``(mode: string, expected: string) = task {
    let scheme =
      if mode = "dark" then
        ColorScheme.Dark
      else
        ColorScheme.Light

    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ColorScheme = scheme))
    do! this.NavigateTo("theme-system")
    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-theme", mode)
    do! this.Expect(this.Page.GetByTestId("theme-swatch")).ToHaveCSSAsync("background-color", expected)
  }

  [<Fact>]
  member this.``toggle mode changes the mode attribute without applying another custom palette``() = task {
    do! this.NavigateTo("theme-coverage")
    do! this.Page.GetByTestId("coverage-toggle").ClickAsync()
    do! this.Expect(this.Page.Locator("body")).ToHaveAttributeAsync("data-theme", "dark")

    do!
      this.Expect(this.Page.GetByTestId("theme-swatch")).ToHaveCSSAsync("background-color", "rgb(18, 52, 86)")

    do! this.Page.GetByTestId("coverage-dark").ClickAsync()

    do!
      this
        .Expect(this.Page.GetByTestId("theme-swatch"))
        .ToHaveCSSAsync("background-color", "rgb(135, 101, 67)")
  }

  [<Fact>]
  member this.``switching theme clears typography omitted by new configuration``() = task {
    do! this.NavigateTo("theme-coverage")

    let! before =
      this.Page.EvaluateAsync<string>(
        "() => document.body.style.getPropertyValue('--weave-font-family-body')"
      )

    Assert.Equal("Georgia", before)
    do! this.Page.GetByTestId("coverage-empty").ClickAsync()

    let! values =
      this.Page.EvaluateAsync<string[]>(
        "() => [document.body.style.getPropertyValue('--weave-font-family-body'), document.documentElement.style.getPropertyValue('--weave-font-family-body')]"
      )

    Assert.All(values, fun value -> Assert.Equal("", value))
  }

  [<Fact>]
  member this.``empty typography restores rendered font fallback``() = task {
    do! this.NavigateTo("typography-coverage")
    let body = this.Page.GetByTestId("typography-body")

    let! defaultFont =
      body.EvaluateAsync<string>("el => getComputedStyle(el).fontFamily")

    do! this.Page.GetByTestId("typography-a").ClickAsync()
    do! this.Expect(body).ToHaveCSSAsync("font-family", "Georgia")
    do! this.Page.GetByTestId("typography-default").ClickAsync()
    do! this.Expect(body).ToHaveCSSAsync("font-family", defaultFont)
    Assert.NotEqual<string>("Georgia", defaultFont)
  }

  [<Fact>]
  member this.``typography switches between complete configurations and back``() = task {
    do! this.NavigateTo("typography-coverage")
    let body = this.Page.GetByTestId("typography-body")
    let heading = this.Page.GetByTestId("typography-heading")
    do! this.Page.GetByTestId("typography-a").ClickAsync()
    do! this.Expect(body).ToHaveCSSAsync("font-family", "Georgia")
    do! this.Expect(heading).ToHaveCSSAsync("font-family", "Verdana")
    do! this.Expect(heading).ToHaveCSSAsync("font-weight", "600")
    do! this.Page.GetByTestId("typography-b").ClickAsync()
    do! this.Expect(body).ToHaveCSSAsync("font-family", "Arial")
    do! this.Expect(heading).ToHaveCSSAsync("font-family", "Tahoma")
    do! this.Expect(heading).ToHaveCSSAsync("font-weight", "800")
    do! this.Page.GetByTestId("typography-a").ClickAsync()
    do! this.Expect(body).ToHaveCSSAsync("font-family", "Georgia")
    do! this.Expect(heading).ToHaveCSSAsync("font-family", "Verdana")
    do! this.Expect(heading).ToHaveCSSAsync("font-weight", "600")
  }

  [<Fact>]
  member this.``partial typography applies specified font and restores omitted values``() = task {
    do! this.NavigateTo("typography-coverage")
    let body = this.Page.GetByTestId("typography-body")
    let heading = this.Page.GetByTestId("typography-heading")

    let! defaultHeading =
      heading.EvaluateAsync<string>("el => getComputedStyle(el).fontFamily")

    let! defaultWeight =
      heading.EvaluateAsync<string>("el => getComputedStyle(el).fontWeight")

    do! this.Page.GetByTestId("typography-a").ClickAsync()
    do! this.Expect(heading).ToHaveCSSAsync("font-family", "Verdana")
    do! this.Expect(heading).ToHaveCSSAsync("font-weight", "600")
    do! this.Page.GetByTestId("typography-partial").ClickAsync()
    do! this.Expect(body).ToHaveCSSAsync("font-family", "Arial")
    do! this.Expect(heading).ToHaveCSSAsync("font-family", defaultHeading)
    do! this.Expect(heading).ToHaveCSSAsync("font-weight", defaultWeight)
    Assert.NotEqual<string>("Verdana", defaultHeading)
    Assert.NotEqual<string>("600", defaultWeight)
  }
