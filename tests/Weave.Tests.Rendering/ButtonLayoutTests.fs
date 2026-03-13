module Weave.Tests.Rendering.ButtonLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type ButtonLayoutTests() =
  inherit PageTest()

  // Boundary pairs for CSS breakpoints: xs/sm (600px), sm/md (960px), md/lg (1280px)
  static member ViewportWidths: obj[][] = [|
    [| 599 |]
    [| 600 |]
    [| 959 |]
    [| 960 |]
    [| 1279 |]
    [| 1280 |]
  |]

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "button.html")

  member this.LoadFixture(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``compact button min height``() = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator("#btn-compact").BoundingBoxAsync()

    Assert.True(box.Height >= 30.0f, $"Compact button height {box.Height}px should be >= 30px")
  }

  [<Fact>]
  member this.``standard button min height``() = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator("#btn-standard").BoundingBoxAsync()

    Assert.True(box.Height >= 40.0f, $"Standard button height {box.Height}px should be >= 40px")
  }

  [<Fact>]
  member this.``spacious button min height``() = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator("#btn-spacious").BoundingBoxAsync()

    Assert.True(box.Height >= 50.0f, $"Spacious button height {box.Height}px should be >= 50px")
  }

  [<Fact>]
  member this.``density ordering is preserved``() = task {
    do! this.LoadFixture 1280
    let! compact = this.Page.Locator("#btn-compact").BoundingBoxAsync()
    let! standard = this.Page.Locator("#btn-standard").BoundingBoxAsync()
    let! spacious = this.Page.Locator("#btn-spacious").BoundingBoxAsync()

    Assert.True(
      compact.Height < standard.Height,
      $"Compact ({compact.Height}px) should be shorter than standard ({standard.Height}px)"
    )

    Assert.True(
      standard.Height < spacious.Height,
      $"Standard ({standard.Height}px) should be shorter than spacious ({spacious.Height}px)"
    )
  }

  [<Theory>]
  [<InlineData("#btn-compact")>]
  [<InlineData("#btn-standard")>]
  [<InlineData("#btn-spacious")>]
  member this.``filled button has min width``(buttonId: string) = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator(buttonId).BoundingBoxAsync()

    Assert.True(box.Width >= 64.0f, $"{buttonId} width {box.Width}px should be >= 64px")
  }

  [<Fact>]
  member this.``icon button does not enforce min width``() = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator("#btn-icon").BoundingBoxAsync()

    Assert.True(
      box.Width < 64.0f,
      $"Icon button width {box.Width}px should be < 64px (min-width is overridden to auto)"
    )
  }

  [<Theory>]
  [<MemberData(nameof ButtonLayoutTests.ViewportWidths)>]
  member this.``full width button fills container``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! containerBox = this.Page.Locator("#full-width-container").BoundingBoxAsync()
    let! buttonBox = this.Page.Locator("#btn-full-width").BoundingBoxAsync()

    Assert.True(
      abs (buttonBox.Width - containerBox.Width) <= 1.0f,
      $"Full-width button {buttonBox.Width}px should fill its container {containerBox.Width}px at {viewportWidth}px viewport"
    )
  }

  member private this.LoadFixtureWithTheme(viewportWidth: int, theme: string) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    let! _ = this.Page.EvaluateAsync($"document.documentElement.setAttribute('data-theme', '{theme}')")
    ()
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``filled button has opaque background in both themes``(theme: string) = task {
    do! this.LoadFixtureWithTheme(1280, theme)

    let! bg =
      this.Page.Locator("#btn-filled").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.False(
      bg = "transparent" || bg = "rgba(0, 0, 0, 0)",
      $"Filled button background in {theme} theme ('{bg}') should not be transparent"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``button layout is stable across themes``(theme: string) = task {
    do! this.LoadFixture 1280
    let! lightBox = this.Page.Locator("#btn-standard").BoundingBoxAsync()
    let! _ = this.Page.EvaluateAsync($"document.documentElement.setAttribute('data-theme', '{theme}')")
    ()
    let! themedBox = this.Page.Locator("#btn-standard").BoundingBoxAsync()

    Assert.True(
      abs (lightBox.Height - themedBox.Height) <= 1.0f,
      $"Button height should not shift between themes (default={lightBox.Height}px, {theme}={themedBox.Height}px)"
    )

    Assert.True(
      abs (lightBox.Width - themedBox.Width) <= 1.0f,
      $"Button width should not shift between themes (default={lightBox.Width}px, {theme}={themedBox.Width}px)"
    )
  }

  [<Fact>]
  member this.``standard button height is consistent across variants``() = task {
    do! this.LoadFixture 1280
    let! filled = this.Page.Locator("#btn-filled").BoundingBoxAsync()
    let! outlined = this.Page.Locator("#btn-outlined").BoundingBoxAsync()
    let! text = this.Page.Locator("#btn-text").BoundingBoxAsync()

    Assert.True(
      abs (filled.Height - outlined.Height) <= 1.0f,
      $"Filled ({filled.Height}px) and outlined ({outlined.Height}px) standard buttons should have the same height"
    )

    Assert.True(
      abs (filled.Height - text.Height) <= 1.0f,
      $"Filled ({filled.Height}px) and text ({text.Height}px) standard buttons should have the same height"
    )
  }
