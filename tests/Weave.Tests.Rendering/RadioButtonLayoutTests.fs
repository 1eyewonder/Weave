module Weave.Tests.Rendering.RadioButtonLayoutTests

open Xunit

type RadioButtonLayoutTests() =
  inherit LayoutTestBase("radio")

  [<Fact>]
  member this.``radio span sizes are ordered small < medium < large``() = task {
    do! this.LoadFixture()
    let! small = this.Page.Locator("#span-small").BoundingBoxAsync()
    and! medium = this.Page.Locator("#span-medium").BoundingBoxAsync()
    and! large = this.Page.Locator("#span-large").BoundingBoxAsync()

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
    and! medium = this.Page.Locator("#span-medium").BoundingBoxAsync()
    and! large = this.Page.Locator("#span-large").BoundingBoxAsync()

    Assert.True(
      abs (small.Width - small.Height) <= 1.0f,
      $"Small span should be circular ({small.Width}px x {small.Height}px)"
    )

    Assert.True(
      abs (medium.Width - medium.Height) <= 1.0f,
      $"Medium span should be circular ({medium.Width}px x {medium.Height}px)"
    )

    Assert.True(
      abs (large.Width - large.Height) <= 1.0f,
      $"Large span should be circular ({large.Width}px x {large.Height}px)"
    )
  }

  [<Fact>]
  member this.``label appears to the right of the radio span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#span-labeled").BoundingBoxAsync()
    and! label = this.Page.Locator("#label-text").BoundingBoxAsync()

    Assert.True(label.X > span.X, $"Label (x={label.X}) should be to the right of radio span (x={span.X})")
  }

  [<Fact>]
  member this.``label is vertically aligned with the radio span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#span-labeled").BoundingBoxAsync()
    and! label = this.Page.Locator("#label-text").BoundingBoxAsync()

    let spanCenter = span.Y + span.Height / 2.0f
    let labelCenter = label.Y + label.Height / 2.0f

    Assert.True(
      abs (spanCenter - labelCenter) <= 4.0f,
      $"Label center (y={labelCenter}) should be near radio span center (y={spanCenter})"
    )
  }

  // ── Content Placement orientation tests ──

  [<Fact>]
  member this.``content right: label is to the right of span with spacing``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-right-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-right-label").BoundingBoxAsync()

    let spanRight = span.X + span.Width
    let gap = label.X - spanRight

    Assert.True(
      label.X > spanRight,
      $"Label (x={label.X}) should be to the right of span (right={spanRight})"
    )

    Assert.True(gap > 1.0f, $"Gap ({gap}px) between span and label should be greater than 1px")
  }

  [<Fact>]
  member this.``content right: label is vertically centered with span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-right-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-right-label").BoundingBoxAsync()

    let spanCenter = span.Y + span.Height / 2.0f
    let labelCenter = label.Y + label.Height / 2.0f

    Assert.True(
      abs (spanCenter - labelCenter) <= 4.0f,
      $"Label center (y={labelCenter}) should be near span center (y={spanCenter})"
    )
  }

  [<Fact>]
  member this.``content left: label is to the left of span with spacing``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-left-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-left-label").BoundingBoxAsync()

    let labelRight = label.X + label.Width
    let gap = span.X - labelRight

    Assert.True(labelRight < span.X, $"Label right edge ({labelRight}) should be left of span (x={span.X})")
    Assert.True(gap > 1.0f, $"Gap ({gap}px) between label and span should be greater than 1px")
  }

  [<Fact>]
  member this.``content left: label is vertically centered with span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-left-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-left-label").BoundingBoxAsync()

    let spanCenter = span.Y + span.Height / 2.0f
    let labelCenter = label.Y + label.Height / 2.0f

    Assert.True(
      abs (spanCenter - labelCenter) <= 4.0f,
      $"Label center (y={labelCenter}) should be near span center (y={spanCenter})"
    )
  }

  [<Fact>]
  member this.``content bottom: label is below span with spacing``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-bottom-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-bottom-label").BoundingBoxAsync()

    let spanBottom = span.Y + span.Height
    let gap = label.Y - spanBottom

    Assert.True(label.Y > spanBottom, $"Label (y={label.Y}) should be below span (bottom={spanBottom})")
    Assert.True(gap > 1.0f, $"Gap ({gap}px) between span and label should be greater than 1px")
  }

  [<Fact>]
  member this.``content bottom: label is horizontally centered with span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-bottom-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-bottom-label").BoundingBoxAsync()

    let spanCenter = span.X + span.Width / 2.0f
    let labelCenter = label.X + label.Width / 2.0f

    Assert.True(
      abs (spanCenter - labelCenter) <= 4.0f,
      $"Label center (x={labelCenter}) should be near span center (x={spanCenter})"
    )
  }

  [<Fact>]
  member this.``content top: label is above span with spacing``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-top-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-top-label").BoundingBoxAsync()

    let labelBottom = label.Y + label.Height
    let gap = span.Y - labelBottom

    Assert.True(labelBottom < span.Y, $"Label bottom ({labelBottom}) should be above span (y={span.Y})")
    Assert.True(gap > 1.0f, $"Gap ({gap}px) between label and span should be greater than 1px")
  }

  [<Fact>]
  member this.``content top: label is horizontally centered with span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#cp-top-span").BoundingBoxAsync()
    and! label = this.Page.Locator("#cp-top-label").BoundingBoxAsync()

    let spanCenter = span.X + span.Width / 2.0f
    let labelCenter = label.X + label.Width / 2.0f

    Assert.True(
      abs (spanCenter - labelCenter) <= 4.0f,
      $"Label center (x={labelCenter}) should be near span center (x={spanCenter})"
    )
  }
