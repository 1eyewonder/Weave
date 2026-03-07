module Weave.Tests.Rendering.ButtonLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type ButtonLayoutTests() =
  inherit PageTest()

  // Boundary pairs for CSS breakpoints: xs/sm (600px), sm/md (960px), md/lg (1280px)
  static member ViewportWidths: obj[][] =
    [| [| 599 |]; [| 600 |]; [| 959 |]; [| 960 |]; [| 1279 |]; [| 1280 |] |]

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
  member this.``small button min height``() = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator("#btn-small").BoundingBoxAsync()

    Assert.True(box.Height >= 30.0f, $"Small button height {box.Height}px should be >= 30px")
  }

  [<Fact>]
  member this.``medium button min height``() = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator("#btn-medium").BoundingBoxAsync()

    Assert.True(box.Height >= 40.0f, $"Medium button height {box.Height}px should be >= 40px")
  }

  [<Fact>]
  member this.``large button min height``() = task {
    do! this.LoadFixture 1280
    let! box = this.Page.Locator("#btn-large").BoundingBoxAsync()

    Assert.True(box.Height >= 50.0f, $"Large button height {box.Height}px should be >= 50px")
  }

  [<Fact>]
  member this.``size ordering is preserved``() = task {
    do! this.LoadFixture 1280
    let! small = this.Page.Locator("#btn-small").BoundingBoxAsync()
    let! medium = this.Page.Locator("#btn-medium").BoundingBoxAsync()
    let! large = this.Page.Locator("#btn-large").BoundingBoxAsync()

    Assert.True(
      small.Height < medium.Height,
      $"Small ({small.Height}px) should be shorter than medium ({medium.Height}px)"
    )

    Assert.True(
      medium.Height < large.Height,
      $"Medium ({medium.Height}px) should be shorter than large ({large.Height}px)"
    )
  }

  [<Theory>]
  [<InlineData("#btn-small")>]
  [<InlineData("#btn-medium")>]
  [<InlineData("#btn-large")>]
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

  [<Fact>]
  member this.``medium button height is consistent across variants``() = task {
    do! this.LoadFixture 1280
    let! filled = this.Page.Locator("#btn-medium").BoundingBoxAsync()
    let! outlined = this.Page.Locator("#btn-medium-outlined").BoundingBoxAsync()
    let! text = this.Page.Locator("#btn-medium-text").BoundingBoxAsync()

    Assert.True(
      abs (filled.Height - outlined.Height) <= 1.0f,
      $"Filled ({filled.Height}px) and outlined ({outlined.Height}px) medium buttons should have the same height"
    )

    Assert.True(
      abs (filled.Height - text.Height) <= 1.0f,
      $"Filled ({filled.Height}px) and text ({text.Height}px) medium buttons should have the same height"
    )
  }
