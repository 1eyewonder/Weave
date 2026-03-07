module Weave.Tests.Rendering.DividerLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type DividerLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "divider.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``horizontal divider spans its container width``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#divider-h-container").BoundingBoxAsync()
    let! divider = this.Page.Locator("#divider-horizontal").BoundingBoxAsync()

    Assert.True(
      abs (divider.Width - container.Width) <= 1.0f,
      $"Horizontal divider {divider.Width}px should span the container {container.Width}px"
    )
  }

  [<Fact>]
  member this.``horizontal divider renders as a thin line``() = task {
    do! this.LoadFixture()
    let! divider = this.Page.Locator("#divider-horizontal").BoundingBoxAsync()

    // border-width is 2px, so height should be at most a few pixels
    Assert.True(divider.Height <= 4.0f, $"Horizontal divider height {divider.Height}px should be <= 4px")
  }

  [<Fact>]
  member this.``vertical divider renders as a thin line``() = task {
    do! this.LoadFixture()
    let! divider = this.Page.Locator("#divider-vertical").BoundingBoxAsync()

    // border-width is 2px on the right side, so width should be at most a few pixels
    Assert.True(divider.Width <= 4.0f, $"Vertical divider width {divider.Width}px should be <= 4px")
  }

  [<Fact>]
  member this.``vertical divider fills its flex container height``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#divider-v-container").BoundingBoxAsync()
    let! divider = this.Page.Locator("#divider-vertical").BoundingBoxAsync()

    Assert.True(
      abs (divider.Height - container.Height) <= 1.0f,
      $"Vertical divider {divider.Height}px should fill the container {container.Height}px"
    )
  }

  [<Fact>]
  member this.``middle variant has horizontal margins``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#divider-middle-container").BoundingBoxAsync()
    let! divider = this.Page.Locator("#divider-middle").BoundingBoxAsync()

    let leftMargin = divider.X - container.X
    let rightMargin = (container.X + container.Width) - (divider.X + divider.Width)

    Assert.True(leftMargin >= 14.0f, $"Middle divider left margin {leftMargin}px should be ~16px")
    Assert.True(rightMargin >= 14.0f, $"Middle divider right margin {rightMargin}px should be ~16px")
  }

  [<Fact>]
  member this.``inset variant has large left margin``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#divider-inset-container").BoundingBoxAsync()
    let! divider = this.Page.Locator("#divider-inset").BoundingBoxAsync()

    let leftMargin = divider.X - container.X

    Assert.True(leftMargin >= 70.0f, $"Inset divider left margin {leftMargin}px should be ~72px")
  }
