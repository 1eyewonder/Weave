module Weave.Tests.Rendering.RadioButtonLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type RadioButtonLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "radio.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``radio span sizes are ordered small < medium < large``() = task {
    do! this.LoadFixture()
    let! small = this.Page.Locator("#span-small").BoundingBoxAsync()
    let! medium = this.Page.Locator("#span-medium").BoundingBoxAsync()
    let! large = this.Page.Locator("#span-large").BoundingBoxAsync()

    Assert.True(
      small.Width < medium.Width,
      $"Small ({small.Width}px) should be smaller than medium ({medium.Width}px)"
    )

    Assert.True(
      medium.Width < large.Width,
      $"Medium ({medium.Width}px) should be smaller than large ({large.Width}px)"
    )
  }

  [<Fact>]
  member this.``radio spans are circular``() = task {
    do! this.LoadFixture()
    let! small = this.Page.Locator("#span-small").BoundingBoxAsync()
    let! medium = this.Page.Locator("#span-medium").BoundingBoxAsync()
    let! large = this.Page.Locator("#span-large").BoundingBoxAsync()

    Assert.True(
      abs (small.Width - small.Height) <= 1.0f,
      $"Small span should be circular ({small.Width}px × {small.Height}px)"
    )

    Assert.True(
      abs (medium.Width - medium.Height) <= 1.0f,
      $"Medium span should be circular ({medium.Width}px × {medium.Height}px)"
    )

    Assert.True(
      abs (large.Width - large.Height) <= 1.0f,
      $"Large span should be circular ({large.Width}px × {large.Height}px)"
    )
  }

  [<Fact>]
  member this.``label appears to the right of the radio span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#span-labeled").BoundingBoxAsync()
    let! label = this.Page.Locator("#label-text").BoundingBoxAsync()

    Assert.True(
      label.X > span.X,
      $"Label (x={label.X}) should be to the right of radio span (x={span.X})"
    )
  }

  [<Fact>]
  member this.``label is vertically aligned with the radio span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#span-labeled").BoundingBoxAsync()
    let! label = this.Page.Locator("#label-text").BoundingBoxAsync()

    let spanCenter = span.Y + span.Height / 2.0f
    let labelCenter = label.Y + label.Height / 2.0f

    Assert.True(
      abs (spanCenter - labelCenter) <= 4.0f,
      $"Label center (y={labelCenter}) should be near radio span center (y={spanCenter})"
    )
  }
