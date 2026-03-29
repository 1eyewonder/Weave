module Weave.Tests.Rendering.SwitchLayoutTests

open Xunit

type SwitchLayoutTests() =
  inherit LayoutTestBase("switch")

  [<Fact>]
  member this.``switch container is 40px wide``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#switch-container-off").BoundingBoxAsync()

    Assert.True(
      abs (container.Width - 40.0f) <= 1.0f,
      $"Switch container width {container.Width}px should be 40px"
    )
  }

  [<Fact>]
  member this.``switch container is 20px tall``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#switch-container-off").BoundingBoxAsync()

    Assert.True(
      abs (container.Height - 20.0f) <= 1.0f,
      $"Switch container height {container.Height}px should be 20px"
    )
  }

  [<Fact>]
  member this.``unchecked thumb is in the left half of the container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#switch-container-off").BoundingBoxAsync()
    and! thumb = this.Page.Locator("#thumb-off").BoundingBoxAsync()

    let thumbCenter = thumb.X + thumb.Width / 2.0f
    let containerCenter = container.X + container.Width / 2.0f

    Assert.True(
      thumbCenter < containerCenter,
      $"Unchecked thumb center (x={thumbCenter}) should be in the left half of container (center={containerCenter})"
    )
  }

  [<Fact>]
  member this.``checked thumb is in the right half of the container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#switch-container-on").BoundingBoxAsync()
    and! thumb = this.Page.Locator("#thumb-on").BoundingBoxAsync()

    let thumbCenter = thumb.X + thumb.Width / 2.0f
    let containerCenter = container.X + container.Width / 2.0f

    Assert.True(
      thumbCenter > containerCenter,
      $"Checked thumb center (x={thumbCenter}) should be in the right half of container (center={containerCenter})"
    )
  }

  [<Fact>]
  member this.``label appears to the right of the switch container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#switch-container-off").BoundingBoxAsync()
    and! label = this.Page.Locator("#switch-label").BoundingBoxAsync()

    Assert.True(
      label.X > container.X + container.Width - 1.0f,
      $"Label (x={label.X}) should be to the right of container (right edge={container.X + container.Width})"
    )
  }

  // ── Content Placement orientation tests ──

  [<Fact>]
  member this.``content right: label is to the right of container with spacing``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-right-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-right-label").BoundingBoxAsync()

    let containerRight = container.X + container.Width
    let gap = label.X - containerRight

    Assert.True(
      label.X > containerRight,
      $"Label (x={label.X}) should be to the right of container (right={containerRight})"
    )

    Assert.True(gap > 1.0f, $"Gap ({gap}px) between container and label should be greater than 1px")
  }

  [<Fact>]
  member this.``content right: label is vertically centered with container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-right-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-right-label").BoundingBoxAsync()

    let containerCenter = container.Y + container.Height / 2.0f
    let labelCenter = label.Y + label.Height / 2.0f

    Assert.True(
      abs (containerCenter - labelCenter) <= 4.0f,
      $"Label center (y={labelCenter}) should be near container center (y={containerCenter})"
    )
  }

  [<Fact>]
  member this.``content left: label is to the left of container with spacing``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-left-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-left-label").BoundingBoxAsync()

    let labelRight = label.X + label.Width
    let gap = container.X - labelRight

    Assert.True(
      labelRight < container.X,
      $"Label right edge ({labelRight}) should be left of container (x={container.X})"
    )

    Assert.True(gap > 1.0f, $"Gap ({gap}px) between label and container should be greater than 1px")
  }

  [<Fact>]
  member this.``content left: label is vertically centered with container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-left-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-left-label").BoundingBoxAsync()

    let containerCenter = container.Y + container.Height / 2.0f
    let labelCenter = label.Y + label.Height / 2.0f

    Assert.True(
      abs (containerCenter - labelCenter) <= 4.0f,
      $"Label center (y={labelCenter}) should be near container center (y={containerCenter})"
    )
  }

  [<Fact>]
  member this.``content bottom: label is below container with spacing``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-bottom-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-bottom-label").BoundingBoxAsync()

    let containerBottom = container.Y + container.Height
    let gap = label.Y - containerBottom

    Assert.True(
      label.Y > containerBottom,
      $"Label (y={label.Y}) should be below container (bottom={containerBottom})"
    )

    Assert.True(gap > 1.0f, $"Gap ({gap}px) between container and label should be greater than 1px")
  }

  [<Fact>]
  member this.``content bottom: label is horizontally centered with container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-bottom-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-bottom-label").BoundingBoxAsync()

    let containerCenter = container.X + container.Width / 2.0f
    let labelCenter = label.X + label.Width / 2.0f

    Assert.True(
      abs (containerCenter - labelCenter) <= 4.0f,
      $"Label center (x={labelCenter}) should be near container center (x={containerCenter})"
    )
  }

  [<Fact>]
  member this.``content top: label is above container with spacing``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-top-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-top-label").BoundingBoxAsync()

    let labelBottom = label.Y + label.Height
    let gap = container.Y - labelBottom

    Assert.True(
      labelBottom < container.Y,
      $"Label bottom ({labelBottom}) should be above container (y={container.Y})"
    )

    Assert.True(gap > 1.0f, $"Gap ({gap}px) between label and container should be greater than 1px")
  }

  [<Fact>]
  member this.``content top: label is horizontally centered with container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#cp-top-container").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-top-label").BoundingBoxAsync()

    let containerCenter = container.X + container.Width / 2.0f
    let labelCenter = label.X + label.Width / 2.0f

    Assert.True(
      abs (containerCenter - labelCenter) <= 4.0f,
      $"Label center (x={labelCenter}) should be near container center (x={containerCenter})"
    )
  }
