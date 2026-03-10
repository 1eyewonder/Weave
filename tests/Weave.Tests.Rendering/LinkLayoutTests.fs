module Weave.Tests.Rendering.LinkLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type LinkLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "link.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``standard link has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#link-standard").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Standard link width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Standard link height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``icon link has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#link-icon").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Icon link width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Icon link height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``start icon is left of text``() = task {
    do! this.LoadFixture()
    let! icon = this.Page.Locator("#link-with-start-icon .weave-link__start-icon").BoundingBoxAsync()
    let! text = this.Page.Locator("#link-with-start-icon .weave-link__text").BoundingBoxAsync()

    Assert.True(icon.X < text.X, $"Start icon (x={icon.X}) should be left of text (x={text.X})")
  }

  [<Fact>]
  member this.``primary link is visible``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#link-primary").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Primary link height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``secondary link is visible``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#link-secondary").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Secondary link height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``disabled link has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents =
      this.Page
        .Locator("#link-disabled")
        .EvaluateAsync<string>("el => window.getComputedStyle(el).pointerEvents")

    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``link renders as inline-flex``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page.Locator("#link-standard").EvaluateAsync<string>("el => window.getComputedStyle(el).display")

    Assert.Equal("inline-flex", display)
  }
