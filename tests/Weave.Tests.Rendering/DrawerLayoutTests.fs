module Weave.Tests.Rendering.DrawerLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type DrawerLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "drawer.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``drawer has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#drawer-left").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Drawer height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``drawer has positive width``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#drawer-left").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Drawer width {box.Width}px should be > 0")
  }

  [<Fact>]
  member this.``drawer is to the left of main content``() = task {
    do! this.LoadFixture()
    let! drawerBox = this.Page.Locator("#drawer-left").BoundingBoxAsync()
    let! mainBox = this.Page.Locator(".weave-main-content").BoundingBoxAsync()

    Assert.True(
      drawerBox.X < mainBox.X,
      $"Drawer left edge ({drawerBox.X}px) should be to the left of main content ({mainBox.X}px)"
    )
  }

  [<Fact>]
  member this.``drawer content area has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#drawer-content").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Drawer content height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``drawer container fills viewport height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#drawer-container").BoundingBoxAsync()

    Assert.True(
      abs (box.Height - 800.0f) <= 1.0f,
      $"Drawer container height {box.Height}px should match viewport height 800px"
    )
  }

  [<Fact>]
  member this.``main content area has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#main-content").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Main content width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Main content height {box.Height}px should be > 0")
  }
