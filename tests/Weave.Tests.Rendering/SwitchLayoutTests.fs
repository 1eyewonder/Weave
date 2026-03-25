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
