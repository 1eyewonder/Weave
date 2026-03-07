module Weave.Tests.Rendering.SwitchLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type SwitchLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "switch.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

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
    let! thumb = this.Page.Locator("#thumb-off").BoundingBoxAsync()

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
    let! thumb = this.Page.Locator("#thumb-on").BoundingBoxAsync()

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
    let! label = this.Page.Locator("#switch-label").BoundingBoxAsync()

    Assert.True(
      label.X > container.X + container.Width - 1.0f,
      $"Label (x={label.X}) should be to the right of container (right edge={container.X + container.Width})"
    )
  }
