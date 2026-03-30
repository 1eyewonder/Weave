module Weave.Tests.Rendering.SliderLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type SliderLayoutTests() =
  inherit LayoutTestBase("slider")

  [<Fact>]
  member this.``slider root has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#slider-basic").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Slider width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Slider height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``slider fills its container width``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#slider-full-width-container").BoundingBoxAsync()
    and! slider = this.Page.Locator("#slider-full-width").BoundingBoxAsync()

    Assert.True(
      abs (slider.Width - container.Width) <= 1.0f,
      $"Slider ({slider.Width}px) should fill its container ({container.Width}px)"
    )
  }

  // ── Structural mechanism tests ──────────────────────────────────────
  // The slider positions its thumb/fill via inline styles set by JS at
  // runtime.  These tests verify the CSS prerequisites that make that
  // mechanism work — if any of these break, inline left/width values
  // would no longer produce the correct visual result.

  [<Fact>]
  member this.``track container has position relative``() = task {
    do! this.LoadFixture()
    let! position = this.ComputedStyle("#slider-track-container", "position")
    Assert.Equal("relative", position)
  }

  [<Fact>]
  member this.``thumb has position absolute``() = task {
    do! this.LoadFixture()
    let! position = this.ComputedStyle("#slider-thumb", "position")
    Assert.Equal("absolute", position)
  }

  [<Fact>]
  member this.``thumb has translateX centering transform``() = task {
    do! this.LoadFixture()
    let! transform = this.ComputedStyle("#slider-thumb", "transform")
    // translateX(-50%) serializes as matrix(1, 0, 0, 1, -8, 0) for a 16px thumb
    // The key check: the X translation is negative half the thumb width
    let! thumbWidth =
      this.Page.Locator("#slider-thumb").EvaluateAsync<float>("el => el.getBoundingClientRect().width")

    let expectedTx = -thumbWidth / 2.0

    Assert.Contains("matrix", transform)
    // Parse the tx component from matrix(a, b, c, d, tx, ty)
    let parts = transform.Replace("matrix(", "").Replace(")", "").Split(',')
    let tx = System.Double.Parse(parts.[4].Trim())

    Assert.True(
      abs (tx - expectedTx) <= 1.0,
      $"Thumb transform tx ({tx}) should be -{thumbWidth / 2.0} (translateX(-50%%))"
    )
  }

  [<Fact>]
  member this.``fill has position absolute``() = task {
    do! this.LoadFixture()
    let! position = this.ComputedStyle("#slider-fill", "position")
    Assert.Equal("absolute", position)
  }

  [<Fact>]
  member this.``fill starts at left edge of track``() = task {
    do! this.LoadFixture()
    let! left = this.ComputedStyle("#slider-fill", "left")
    Assert.Equal("0px", left)
  }

  [<Fact>]
  member this.``track is full width within track container``() = task {
    do! this.LoadFixture()
    let! trackContainer = this.Page.Locator("#slider-track-container").BoundingBoxAsync()

    and! track = this.Page.Locator("#slider-basic .weave-slider__track").BoundingBoxAsync()

    Assert.True(
      abs (track.Width - trackContainer.Width) <= 1.0f,
      $"Track ({track.Width}px) should fill track container ({trackContainer.Width}px)"
    )
  }

  [<Fact>]
  member this.``track container fills slider width``() = task {
    do! this.LoadFixture()
    let! slider = this.Page.Locator("#slider-basic").BoundingBoxAsync()
    and! trackContainer = this.Page.Locator("#slider-track-container").BoundingBoxAsync()

    Assert.True(
      abs (trackContainer.Width - slider.Width) <= 1.0f,
      $"Track container ({trackContainer.Width}px) should fill slider ({slider.Width}px)"
    )
  }

  // ── Inline-style positioning sanity checks ──────────────────────────
  // These verify that inline left/width values (set by JS in the real
  // component) produce correct rendered positions given the structural
  // CSS above.  They are intentionally testing the inline values present
  // in the fixture — the structural tests above guard against CSS
  // regressions that would break the mechanism.

  [<Fact>]
  member this.``thumb is positioned at 50% for a half-value slider``() = task {
    do! this.LoadFixture()
    let! trackContainer = this.Page.Locator("#slider-track-container").BoundingBoxAsync()
    and! thumb = this.Page.Locator("#slider-thumb").BoundingBoxAsync()

    let thumbCenter = thumb.X + thumb.Width / 2.0f
    let expectedCenter = trackContainer.X + trackContainer.Width * 0.5f

    Assert.True(
      abs (thumbCenter - expectedCenter) <= 2.0f,
      $"Thumb center ({thumbCenter}px) should be near 50 percent ({expectedCenter}px)"
    )
  }

  [<Fact>]
  member this.``thumb at 0% is at the left edge``() = task {
    do! this.LoadFixture()
    let! trackContainer = this.Page.Locator("#slider-zero .weave-slider__track-container").BoundingBoxAsync()
    and! thumb = this.Page.Locator("#slider-thumb-zero").BoundingBoxAsync()

    let thumbCenter = thumb.X + thumb.Width / 2.0f

    Assert.True(
      abs (thumbCenter - trackContainer.X) <= 2.0f,
      $"Thumb at 0 percent center ({thumbCenter}px) should be near track left ({trackContainer.X}px)"
    )
  }

  [<Fact>]
  member this.``thumb at 100% is at the right edge``() = task {
    do! this.LoadFixture()
    let! trackContainer = this.Page.Locator("#slider-full .weave-slider__track-container").BoundingBoxAsync()
    and! thumb = this.Page.Locator("#slider-thumb-full").BoundingBoxAsync()

    let thumbCenter = thumb.X + thumb.Width / 2.0f
    let trackRight = trackContainer.X + trackContainer.Width

    Assert.True(
      abs (thumbCenter - trackRight) <= 2.0f,
      $"Thumb at 100 percent center ({thumbCenter}px) should be near track right ({trackRight}px)"
    )
  }

  [<Fact>]
  member this.``fill width matches thumb position``() = task {
    do! this.LoadFixture()
    let! trackContainer = this.Page.Locator("#slider-track-container").BoundingBoxAsync()
    and! fill = this.Page.Locator("#slider-fill").BoundingBoxAsync()

    let fillPercent = fill.Width / trackContainer.Width * 100.0f

    Assert.True(
      abs (fillPercent - 50.0f) <= 2.0f,
      $"Fill width ({fillPercent} percent) should be approximately 50 percent"
    )
  }

  [<Fact>]
  member this.``label is above the track container``() = task {
    do! this.LoadFixture()
    let! label = this.Page.Locator("#slider-label").BoundingBoxAsync()

    and! trackContainer =
      this.Page.Locator("#slider-labeled .weave-slider__track-container").BoundingBoxAsync()

    Assert.True(
      label.Y + label.Height <= trackContainer.Y + 1.0f,
      $"Label bottom ({label.Y + label.Height}px) should be at or above track top ({trackContainer.Y}px)"
    )
  }

  [<Fact>]
  member this.``disabled slider has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents = this.ComputedStyle("#slider-disabled", "pointerEvents")

    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``disabled slider has reduced opacity``() = task {
    do! this.LoadFixture()

    let! opacity =
      this.Page.Locator("#slider-disabled").EvaluateAsync<string>("el => getComputedStyle(el).opacity")

    let opacityValue = System.Double.Parse(opacity)

    Assert.True(opacityValue < 1.0, $"Disabled slider opacity ({opacity}) should be less than 1")
  }

  [<Fact>]
  member this.``thumb is contained within track container``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#slider-track-container").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#slider-thumb").BoundingBoxAsync()
    assertContainedWithin "track container" "thumb" parentBox childBox
  }

  [<Fact>]
  member this.``tick labels are below the track container``() = task {
    do! this.LoadFixture()
    let! trackContainer = this.Page.Locator("#slider-ticks .weave-slider__track-container").BoundingBoxAsync()
    and! tickLabels = this.Page.Locator("#slider-tick-labels").BoundingBoxAsync()

    Assert.True(
      tickLabels.Y >= trackContainer.Y + trackContainer.Height - 1.0f,
      $"Tick labels top ({tickLabels.Y}px) should be at or below track bottom ({trackContainer.Y + trackContainer.Height}px)"
    )
  }

  [<Fact>]
  member this.``tick container is present when ticks are shown``() = task {
    do! this.LoadFixture()
    let! tickContainer = this.Page.Locator("#slider-tick-container").BoundingBoxAsync()

    Assert.True(tickContainer.Width > 0.0f, $"Tick container width {tickContainer.Width}px should be > 0")
  }

  [<Fact>]
  member this.``thumb has circular shape``() = task {
    do! this.LoadFixture()
    let! thumb = this.Page.Locator("#slider-thumb").BoundingBoxAsync()

    Assert.True(
      abs (thumb.Width - thumb.Height) <= 1.0f,
      $"Thumb should be circular ({thumb.Width}px x {thumb.Height}px)"
    )
  }
