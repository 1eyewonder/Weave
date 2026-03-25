module Weave.Tests.Rendering.SpacerLayoutTests

open Xunit

type SpacerLayoutTests() =
  inherit LayoutTestBase("spacer")

  [<Fact>]
  member this.``spacer occupies space between siblings``() = task {
    do! this.LoadFixture()
    let! spacerBox = this.Page.Locator("#spacer").BoundingBoxAsync()

    Assert.True(spacerBox.Width > 0.0f, $"Spacer width {spacerBox.Width}px should be > 0")
  }

  [<Fact>]
  member this.``spacer pushes right item to end of container``() = task {
    do! this.LoadFixture()
    let! containerBox = this.Page.Locator("#flex-row").BoundingBoxAsync()
    and! rightBox = this.Page.Locator("#item-right").BoundingBoxAsync()

    let rightEdge = rightBox.X + rightBox.Width
    let containerRight = containerBox.X + containerBox.Width

    Assert.True(
      abs (rightEdge - containerRight) <= 1.0f,
      $"Right item edge ({rightEdge}px) should align with container right ({containerRight}px)"
    )
  }

  [<Fact>]
  member this.``total layout width equals container width``() = task {
    do! this.LoadFixture()
    let! containerBox = this.Page.Locator("#flex-row").BoundingBoxAsync()
    and! leftBox = this.Page.Locator("#item-left").BoundingBoxAsync()
    and! spacerBox = this.Page.Locator("#spacer").BoundingBoxAsync()
    and! rightBox = this.Page.Locator("#item-right").BoundingBoxAsync()

    let totalWidth = leftBox.Width + spacerBox.Width + rightBox.Width

    Assert.True(
      abs (totalWidth - containerBox.Width) <= 1.0f,
      $"Total children width ({totalWidth}px) should equal container width ({containerBox.Width}px)"
    )
  }

  [<Fact>]
  member this.``without spacer items are adjacent``() = task {
    do! this.LoadFixture()
    let! leftBox = this.Page.Locator("#no-spacer-left").BoundingBoxAsync()
    and! rightBox = this.Page.Locator("#no-spacer-right").BoundingBoxAsync()

    let gap = rightBox.X - (leftBox.X + leftBox.Width)

    Assert.True(gap <= 1.0f, $"Without spacer, gap between items ({gap}px) should be approximately 0")
  }
