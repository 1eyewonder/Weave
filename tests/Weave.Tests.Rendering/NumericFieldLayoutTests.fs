module Weave.Tests.Rendering.NumericFieldLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type NumericFieldLayoutTests() =
  inherit LayoutTestBase("numericfield")

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
    and! downBox = this.Page.Locator("#spin-down").BoundingBoxAsync()

    Assert.True(
      upBox.Y < downBox.Y,
      $"Spin up button (y={upBox.Y}) should be above spin down button (y={downBox.Y})"
    )
  }

  [<Fact>]
  member this.``spin buttons are to the right of the input``() = task {
    do! this.LoadFixture()
    let! inputBox = this.Page.Locator("#numeric-input").BoundingBoxAsync()
    and! spinBox = this.Page.Locator("#spin-buttons").BoundingBoxAsync()

    Assert.True(
      spinBox.X >= inputBox.X + inputBox.Width - 1.0f,
      $"Spin buttons left ({spinBox.X}px) should be at or past input right ({inputBox.X + inputBox.Width}px)"
    )
  }

  [<Fact>]
  member this.``spin buttons are contained within field control``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#numeric-standard .weave-field__control").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#spin-buttons").BoundingBoxAsync()
    assertContainedWithin "field control" "spin buttons" parentBox childBox
  }

  [<Fact>]
  member this.``spin-up button is contained within spin buttons``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#spin-buttons").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#spin-up").BoundingBoxAsync()
    assertContainedWithin "spin buttons" "spin-up button" parentBox childBox
  }

  [<Fact>]
  member this.``spin-down button is contained within spin buttons``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#spin-buttons").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#spin-down").BoundingBoxAsync()
    assertContainedWithin "spin buttons" "spin-down button" parentBox childBox
  }

  [<Fact>]
  member this.``outlined spin buttons are contained within field control``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#numeric-outlined .weave-field__control").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#numeric-outlined .weave-field__spin-buttons").BoundingBoxAsync()
    assertContainedWithin "field control" "outlined spin buttons" parentBox childBox
  }

  [<Fact>]
  member this.``spin buttons fill field control height``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#numeric-standard .weave-field__control").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#spin-buttons").BoundingBoxAsync()
    assertFillsHeight "field control" "spin buttons" parentBox childBox
  }

  [<Fact>]
  member this.``spin-up button fills spin buttons width``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#spin-buttons").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#spin-up").BoundingBoxAsync()
    assertFillsWidth "spin buttons" "spin-up button" parentBox childBox
  }

  [<Fact>]
  member this.``outlined spin buttons fill field control height``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#numeric-outlined .weave-field__control").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#numeric-outlined .weave-field__spin-buttons").BoundingBoxAsync()
    assertFillsHeight "field control" "outlined spin buttons" parentBox childBox
  }
