module Weave.Tests.Rendering.NumericFieldLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type NumericFieldLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "numericfield.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``numeric field has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#numeric-standard").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Numeric field width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Numeric field height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``spin buttons container is visible``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#spin-buttons").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Spin buttons width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Spin buttons height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``spin up and down buttons are stacked vertically``() = task {
    do! this.LoadFixture()
    let! upBox = this.Page.Locator("#spin-up").BoundingBoxAsync()
    let! downBox = this.Page.Locator("#spin-down").BoundingBoxAsync()

    Assert.True(
      upBox.Y < downBox.Y,
      $"Spin up button (y={upBox.Y}) should be above spin down button (y={downBox.Y})"
    )
  }

  [<Fact>]
  member this.``spin buttons are to the right of the input``() = task {
    do! this.LoadFixture()
    let! inputBox = this.Page.Locator("#numeric-input").BoundingBoxAsync()
    let! spinBox = this.Page.Locator("#spin-buttons").BoundingBoxAsync()

    Assert.True(
      spinBox.X >= inputBox.X + inputBox.Width - 1.0f,
      $"Spin buttons left ({spinBox.X}px) should be at or past input right ({inputBox.X + inputBox.Width}px)"
    )
  }
