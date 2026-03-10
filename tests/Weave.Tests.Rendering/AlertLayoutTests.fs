module Weave.Tests.Rendering.AlertLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type AlertLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "alert.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``standard alert has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#alert-standard").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Standard alert height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``outlined alert has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#alert-outlined").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Outlined alert height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``filled alert has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#alert-filled").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Filled alert height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``all three variants stack vertically``() = task {
    do! this.LoadFixture()
    let! standard = this.Page.Locator("#alert-standard").BoundingBoxAsync()
    let! outlined = this.Page.Locator("#alert-outlined").BoundingBoxAsync()
    let! filled = this.Page.Locator("#alert-filled").BoundingBoxAsync()

    Assert.True(
      standard.Y < outlined.Y,
      $"Standard (y={standard.Y}) should be above outlined (y={outlined.Y})"
    )

    Assert.True(outlined.Y < filled.Y, $"Outlined (y={outlined.Y}) should be above filled (y={filled.Y})")
  }

  [<Fact>]
  member this.``icon is left of content``() = task {
    do! this.LoadFixture()
    let! icon = this.Page.Locator("#alert-with-icon .weave-alert__icon").BoundingBoxAsync()
    let! content = this.Page.Locator("#alert-with-icon .weave-alert__content").BoundingBoxAsync()

    Assert.True(icon.X < content.X, $"Icon (x={icon.X}) should be left of content (x={content.X})")
  }

  [<Fact>]
  member this.``close button is right of content``() = task {
    do! this.LoadFixture()
    let! content = this.Page.Locator("#alert-with-close .weave-alert__content").BoundingBoxAsync()
    let! close = this.Page.Locator("#alert-with-close .weave-alert__close").BoundingBoxAsync()

    Assert.True(content.X < close.X, $"Content (x={content.X}) should be left of close button (x={close.X})")
  }
