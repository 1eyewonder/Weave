module Weave.Tests.Rendering.CheckboxLayoutTests

open Xunit

type CheckboxLayoutTests() =
  inherit LayoutTestBase("checkbox")

  [<Fact>]
  member this.``checkbox sizes are ordered small < medium < large``() = task {
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
  member this.``checkbox spans are square``() = task {
    do! this.LoadFixture()
    let! small = this.Page.Locator("#span-small").BoundingBoxAsync()
    and! medium = this.Page.Locator("#span-medium").BoundingBoxAsync()
    and! large = this.Page.Locator("#span-large").BoundingBoxAsync()

    Assert.True(
      abs (small.Width - small.Height) <= 1.0f,
      $"Small span should be square ({small.Width}px x {small.Height}px)"
    )

    Assert.True(
      abs (medium.Width - medium.Height) <= 1.0f,
      $"Medium span should be square ({medium.Width}px x {medium.Height}px)"
    )

    Assert.True(
      abs (large.Width - large.Height) <= 1.0f,
      $"Large span should be square ({large.Width}px x {large.Height}px)"
    )
  }

  [<Fact>]
  member this.``label appears to the right of the checkbox span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#span-labeled").BoundingBoxAsync()
    and! label = this.Page.Locator("#label-text").BoundingBoxAsync()

    Assert.True(label.X > span.X, $"Label (x={label.X}) should be to the right of checkbox span (x={span.X})")
  }

  [<Fact>]
  member this.``label is vertically aligned with the checkbox span``() = task {
    do! this.LoadFixture()
    let! span = this.Page.Locator("#span-labeled").BoundingBoxAsync()
    and! label = this.Page.Locator("#label-text").BoundingBoxAsync()

    let spanCenter = span.Y + span.Height / 2.0f
    let labelCenter = label.Y + label.Height / 2.0f

    Assert.True(
      abs (spanCenter - labelCenter) <= 4.0f,
      $"Label center (y={labelCenter}) should be near checkbox center (y={spanCenter})"
    )
  }
