module Weave.Tests.Rendering.GridLayoutTests

open Xunit

type GridLayoutTests() =
  inherit LayoutTestBase("grid")

  [<Fact>]
  member this.``xs-6 items each take half the grid width``() = task {
    do! this.LoadFixture 1280
    let! grid = this.Page.Locator("#grid-xs6").BoundingBoxAsync()
    and! itemA = this.Page.Locator("#item-xs6-a").BoundingBoxAsync()
    and! itemB = this.Page.Locator("#item-xs6-b").BoundingBoxAsync()

    let expected = grid.Width / 2.0f

    Assert.True(
      abs (itemA.Width - expected) <= 1.0f,
      $"Item A width {itemA.Width}px should be half the grid width {grid.Width}px"
    )

    Assert.True(
      abs (itemB.Width - expected) <= 1.0f,
      $"Item B width {itemB.Width}px should be half the grid width {grid.Width}px"
    )
  }

  [<Fact>]
  member this.``xs-12 item fills the grid width``() = task {
    do! this.LoadFixture 1280
    let! grid = this.Page.Locator("#grid-xs12").BoundingBoxAsync()
    and! item = this.Page.Locator("#item-xs12").BoundingBoxAsync()

    Assert.True(
      abs (item.Width - grid.Width) <= 1.0f,
      $"xs-12 item {item.Width}px should fill the grid {grid.Width}px"
    )
  }

  [<Fact>]
  member this.``xs-12 sm-6 items stack at xs viewport``() = task {
    do! this.LoadFixture 599
    let! grid = this.Page.Locator("#grid-responsive").BoundingBoxAsync()
    and! itemA = this.Page.Locator("#item-responsive-a").BoundingBoxAsync()
    and! itemB = this.Page.Locator("#item-responsive-b").BoundingBoxAsync()

    Assert.True(
      abs (itemA.Width - grid.Width) <= 1.0f,
      $"Item A {itemA.Width}px should be full grid width {grid.Width}px at xs viewport"
    )

    Assert.True(
      abs (itemB.Width - grid.Width) <= 1.0f,
      $"Item B {itemB.Width}px should be full grid width {grid.Width}px at xs viewport"
    )

    // Stacked: B should be below A
    Assert.True(itemB.Y > itemA.Y, $"Item B (y={itemB.Y}) should be below item A (y={itemA.Y}) when stacked")
  }

  [<Fact>]
  member this.``xs-12 sm-6 items go side by side at sm viewport``() = task {
    do! this.LoadFixture 600
    let! grid = this.Page.Locator("#grid-responsive").BoundingBoxAsync()
    and! itemA = this.Page.Locator("#item-responsive-a").BoundingBoxAsync()
    and! itemB = this.Page.Locator("#item-responsive-b").BoundingBoxAsync()

    let expected = grid.Width / 2.0f

    Assert.True(
      abs (itemA.Width - expected) <= 1.0f,
      $"Item A {itemA.Width}px should be half the grid width {grid.Width}px at sm viewport"
    )

    Assert.True(
      abs (itemB.Width - expected) <= 1.0f,
      $"Item B {itemB.Width}px should be half the grid width {grid.Width}px at sm viewport"
    )

    // Side by side: both items on the same row
    Assert.True(
      abs (itemA.Y - itemB.Y) <= 1.0f,
      $"Items should be on the same row at sm viewport (A y={itemA.Y}, B y={itemB.Y})"
    )
  }
