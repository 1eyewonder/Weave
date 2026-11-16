module Weave.Tests.Rendering.DividerLayoutTests

open Xunit

type DividerLayoutTests() =
  inherit LayoutTestBase("divider")

  [<Fact>]
  member this.``horizontal divider spans its container width``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#divider-h-container").BoundingBoxAsync()
    and! divider = this.Page.Locator("#divider-horizontal").BoundingBoxAsync()

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
    and! divider = this.Page.Locator("#divider-vertical").BoundingBoxAsync()

    Assert.True(
      abs (divider.Height - container.Height) <= 1.0f,
      $"Vertical divider {divider.Height}px should fill the container {container.Height}px"
    )
  }

  [<Fact>]
  member this.``given a content-sized row when a sibling sets its height then the vertical divider spans the row``
    ()
    = task {
    do! this.LoadFixture 320
    let row = this.Page.Locator("#divider-content-row")
    let! sibling = row.Locator("span").First.BoundingBoxAsync()
    and! divider = row.Locator(".weave-divider").BoundingBoxAsync()

    Assert.Equal(sibling.Height, divider.Height)
  }

  [<Fact>]
  member this.``given a block with a definite height then the vertical divider fills it``() = task {
    do! this.LoadFixture()
    let block = this.Page.Locator("#divider-block")
    let! container = block.BoundingBoxAsync()
    and! divider = block.Locator(".weave-divider").BoundingBoxAsync()

    Assert.Equal(container.Height, divider.Height)
  }

  [<Fact>]
  member this.``given an explicit shorter divider then the row does not stretch it``() = task {
    do! this.LoadFixture()
    let! divider = this.Page.Locator("#divider-short .weave-divider").BoundingBoxAsync()
    Assert.Equal(24.0f, divider.Height)
  }

  [<Fact>]
  member this.``middle variant has horizontal margins``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#divider-middle-container").BoundingBoxAsync()
    and! divider = this.Page.Locator("#divider-middle").BoundingBoxAsync()

    let leftMargin = divider.X - container.X
    let rightMargin = (container.X + container.Width) - (divider.X + divider.Width)

    Assert.True(leftMargin >= 14.0f, $"Middle divider left margin {leftMargin}px should be ~16px")
    Assert.True(rightMargin >= 14.0f, $"Middle divider right margin {rightMargin}px should be ~16px")
  }

  [<Fact>]
  member this.``inset variant has large left margin``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#divider-inset-container").BoundingBoxAsync()
    and! divider = this.Page.Locator("#divider-inset").BoundingBoxAsync()

    let leftMargin = divider.X - container.X

    Assert.True(leftMargin >= 70.0f, $"Inset divider left margin {leftMargin}px should be ~72px")
  }
