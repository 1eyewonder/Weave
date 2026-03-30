module Weave.Tests.Rendering.DrawerLayoutTests

open Xunit

type DrawerLayoutTests() =
  inherit LayoutTestBase("drawer")

  [<Fact>]
  member this.``drawer has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#drawer-left").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Drawer height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``drawer width comes from CSS custom property``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#drawer-left").BoundingBoxAsync()

    // Width should match --drawer-width-left (256px) set in the fixture's :root
    Assert.True(
      abs (box.Width - 256.0f) <= 1.0f,
      $"Drawer width {box.Width}px should match --drawer-width-left (256px)"
    )
  }

  [<Fact>]
  member this.``drawer has position absolute from CSS``() = task {
    do! this.LoadFixture()

    let! position = this.ComputedStyle("#drawer-left", "position")

    Assert.Equal("absolute", position)
  }

  [<Fact>]
  member this.``drawer is to the left of main content``() = task {
    do! this.LoadFixture()
    let! drawerBox = this.Page.Locator("#drawer-left").BoundingBoxAsync()
    and! mainBox = this.Page.Locator("#drawer-container .weave-main-content").BoundingBoxAsync()

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

  [<Fact>]
  member this.``closed drawer is positioned off-screen``() = task {
    do! this.LoadFixture()

    let! left = this.Page.Locator("#drawer-closed").EvaluateAsync<string>("el => getComputedStyle(el).left")

    // Closed drawer uses negative left to move off-screen: left: calc(-1 * drawer-width)
    let leftValue = System.Double.Parse(left.Replace("px", ""))
    Assert.True(leftValue < 0.0, $"Closed drawer left ({left}) should be negative (off-screen)")
  }

  [<Fact>]
  member this.``right drawer is positioned on the right``() = task {
    do! this.LoadFixture()
    let! containerBox = this.Page.Locator("#drawer-right-container").BoundingBoxAsync()
    and! drawerBox = this.Page.Locator("#drawer-right").BoundingBoxAsync()

    // Right drawer should be at or near the right edge of its container
    let drawerRightEdge = drawerBox.X + drawerBox.Width
    let containerRightEdge = containerBox.X + containerBox.Width

    Assert.True(
      abs (drawerRightEdge - containerRightEdge) <= 1.0f,
      $"Right drawer edge ({drawerRightEdge}px) should align with container right edge ({containerRightEdge}px)"
    )
  }

  [<Theory>]
  [<InlineData(599)>]
  [<InlineData(600)>]
  [<InlineData(959)>]
  [<InlineData(960)>]
  [<InlineData(1279)>]
  [<InlineData(1280)>]
  member this.``open drawer has positive width at all viewports``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! box = this.Page.Locator("#drawer-left").BoundingBoxAsync()

    Assert.True(
      box.Width > 0.0f,
      $"Drawer width {box.Width}px should be positive at {viewportWidth}px viewport"
    )
  }

  [<Theory>]
  [<InlineData(599)>]
  [<InlineData(600)>]
  [<InlineData(959)>]
  [<InlineData(960)>]
  [<InlineData(1279)>]
  [<InlineData(1280)>]
  member this.``drawer container fills viewport width``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! box = this.Page.Locator("#drawer-container").BoundingBoxAsync()

    Assert.True(
      abs (box.Width - float32 viewportWidth) <= 1.0f,
      $"Drawer container width {box.Width}px should match viewport {viewportWidth}px"
    )
  }
