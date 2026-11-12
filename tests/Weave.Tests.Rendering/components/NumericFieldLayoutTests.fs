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

    let! parentBox =
      this.Page.Locator("#numeric-standard .weave-field__control").BoundingBoxAsync()

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

    let! parentBox =
      this.Page.Locator("#numeric-outlined .weave-field__control").BoundingBoxAsync()

    and! childBox =
      this.Page.Locator("#numeric-outlined .weave-field__spin-buttons").BoundingBoxAsync()

    assertContainedWithin "field control" "outlined spin buttons" parentBox childBox
  }

  [<Fact>]
  member this.``spin buttons fill field control height``() = task {
    do! this.LoadFixture()

    let! parentBox =
      this.Page.Locator("#numeric-standard .weave-field__control").BoundingBoxAsync()

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

    let! parentBox =
      this.Page.Locator("#numeric-outlined .weave-field__control").BoundingBoxAsync()

    and! childBox =
      this.Page.Locator("#numeric-outlined .weave-field__spin-buttons").BoundingBoxAsync()

    assertFillsHeight "field control" "outlined spin buttons" parentBox childBox
  }

  [<Fact>]
  member this.``floated label has scale transform applied``() = task {
    do! this.LoadFixture()
    let! transform = this.ComputedStyle("#numeric-label", "transform")
    // scale(0.75) is serialized as a matrix: matrix(0.75, 0, 0, 0.75, tx, ty)
    Assert.Contains("0.75", transform)
  }

  [<Fact>]
  member this.``input font size is inherited from typography class``() = task {
    do! this.LoadFixture()

    let! fontSize =
      this.ComputedStyle("#numeric-standard .weave-field__input", "fontSize")

    Assert.False(
      fontSize = "" || fontSize = "0px",
      $"NumericField input font-size ('{fontSize}') should be set via the weave-typography--body2 class"
    )
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``full width field leaves room for siblings in a bounded column``(width: int) = task {
    do! this.LoadFixture(width)
    let! column = this.Page.Locator("#numeric-column").BoundingBoxAsync()
    let! label = this.Page.Locator("#numeric-column-label").BoundingBoxAsync()
    let! field = this.Page.Locator("#numeric-column-field").BoundingBoxAsync()
    let! modifier = this.Page.Locator("#numeric-column-modifier").BoundingBoxAsync()

    assertContainedWithin "column" "label" column label
    assertContainedWithin "column" "full width field" column field
    assertContainedWithin "column" "modifier" column modifier
    Assert.True(label.Y + label.Height <= field.Y + tolerance)
    Assert.True(field.Y + field.Height <= modifier.Y + tolerance)

    Assert.True(
      abs (field.Width - (column.Width - 32.0f)) <= tolerance,
      "Full width should fill the column inside its padding without stretching vertically"
    )
  }

  [<Theory>]
  [<InlineData(320, "compact", "empty")>]
  [<InlineData(320, "compact", "populated")>]
  [<InlineData(320, "standard", "empty")>]
  [<InlineData(320, "standard", "populated")>]
  [<InlineData(320, "spacious", "empty")>]
  [<InlineData(320, "spacious", "populated")>]
  [<InlineData(1280, "compact", "empty")>]
  [<InlineData(1280, "compact", "populated")>]
  [<InlineData(1280, "standard", "empty")>]
  [<InlineData(1280, "standard", "populated")>]
  [<InlineData(1280, "spacious", "empty")>]
  [<InlineData(1280, "spacious", "populated")>]
  member this.``variants have equal visible control heights for empty and populated values``
    (width: int, density: string, state: string)
    = task {
    do! this.LoadFixture(width)
    let matrix = this.Page.Locator("#numericfield-variant-sizing")

    let! _ =
      matrix.EvaluateAsync("(el, density) => el.className = 'weave-density--' + density", density)

    let mutable heights = []

    for variant in [ "standard"; "filled"; "outlined" ] do
      let field = this.Page.Locator($"#numericfield-sizing-{variant}-{state}")
      let! control = field.Locator(".weave-field__control").BoundingBoxAsync()
      heights <- (variant, control.Height) :: heights

      let! label = field.Locator(".weave-field__label span").BoundingBoxAsync()
      let input = field.Locator(".weave-field__input")
      let! inputBox = input.BoundingBoxAsync()

      let! paddingTop =
        input.EvaluateAsync<float>("el => parseFloat(getComputedStyle(el).paddingTop)")

      Assert.True(
        float (label.Y + label.Height) <= float inputBox.Y + paddingTop + 1.0,
        $"{variant} floating label should remain above the entered text"
      )

    let controlHeights = heights |> List.map snd
    let difference = List.max controlHeights - List.min controlHeights

    Assert.True(
      difference <= 1.0f,
      $"{density} {state} controls should have equal heights; measured {heights}"
    )
  }

  [<Fact>]
  member this.``hidden spin buttons leave no empty end adornment``() = task {
    do! this.LoadFixture()
    let field = this.Page.Locator("#numeric-no-spin")
    let adornment = field.Locator(".weave-field__adornment--end")
    let! display = adornment.EvaluateAsync<string>("el => getComputedStyle(el).display")
    Assert.Equal("none", display)
  }
