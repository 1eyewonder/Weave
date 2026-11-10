namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

// Exercise the compiled WebSharper components, including geometry and ARIA updates.
type ProgressTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``progress passes accessibility scan``() = this.RunAxeScan("progress")

  [<Fact>]
  member this.``progress passes dark theme accessibility scan``() =
    this.RunAxeScan("progress", dark = true)

  [<Fact>]
  member this.``measured values update both shapes and their accessible range``() = task {
    do! this.NavigateTo("progress")

    for id, name in [ "linear", "Download"; "circular", "Download circle" ] do
      let progress = this.Page.GetByTestId(id)
      do! this.Expect(progress).ToHaveAccessibleNameAsync(name)
      do! this.Expect(progress).ToHaveAttributeAsync("aria-valuemin", "10")
      do! this.Expect(progress).ToHaveAttributeAsync("aria-valuemax", "110")
      do! this.Expect(progress).ToHaveAttributeAsync("aria-valuenow", "35")

    let! track = this.Page.GetByTestId("linear").BoundingBoxAsync()

    let! fill =
      this.Page.GetByTestId("linear").Locator(".weave-progress-linear__fill").BoundingBoxAsync()

    Assert.InRange(float (fill.Width / track.Width), 0.249, 0.251)
    do! this.Page.GetByTestId("advance").ClickAsync()

    for id in [ "linear"; "circular" ] do
      do! this.Expect(this.Page.GetByTestId(id)).ToHaveAttributeAsync("aria-valuenow", "60")

    do!
      this
        .Expect(this.Page.GetByTestId("linear").Locator(".weave-progress-linear__fill"))
        .ToHaveCSSAsync("width", string (track.Width / 2.0f) + "px")

    do!
      this
        .Expect(this.Page.GetByTestId("circular").Locator(".weave-progress-circular__fill"))
        .ToHaveCSSAsync("stroke-dashoffset", Regex(@"^(calc\()?50px\)?$"))
  }

  [<Fact>]
  member this.``unknown mode removes current value and can return to measured mode``() = task {
    do! this.NavigateTo("progress")

    for id in [ "unknown-linear"; "unknown-circular" ] do
      do! this.Expect(this.Page.GetByTestId(id)).Not.ToHaveAttributeAsync("aria-valuenow", Regex(".*"))
      let! value = this.Page.GetByTestId(id).GetAttributeAsync("aria-valuenow")
      Assert.Null(value)

    do! this.Page.GetByTestId("toggle-mode").ClickAsync()

    for id in [ "linear"; "circular" ] do
      do! this.Expect(this.Page.GetByTestId(id)).Not.ToHaveAttributeAsync("aria-valuenow", Regex(".*"))
      let! value = this.Page.GetByTestId(id).GetAttributeAsync("aria-valuenow")
      Assert.Null(value)

    do!
      this.Expect(this.Page.GetByTestId("linear").Locator(".weave-progress-linear__buffer")).ToBeHiddenAsync()

    do! this.Page.GetByTestId("advance").ClickAsync()
    do! this.Page.GetByTestId("toggle-mode").ClickAsync()

    for id in [ "linear"; "circular" ] do
      do! this.Expect(this.Page.GetByTestId(id)).ToHaveAttributeAsync("aria-valuenow", "60")
  }

  [<Fact>]
  member this.``out of range and nonfinite values stay bounded``() = task {
    do! this.NavigateTo("progress")

    for button, expected in [ "upper", "110"; "lower", "10"; "nan", "10"; "infinity", "110" ] do
      do! this.Page.GetByTestId(button).ClickAsync()

      for id in [ "linear"; "circular" ] do
        do! this.Expect(this.Page.GetByTestId(id)).ToHaveAttributeAsync("aria-valuenow", expected)

    let invalidRange = this.Page.GetByTestId("invalid-range")
    do! this.Expect(invalidRange).ToHaveAttributeAsync("aria-valuemin", "0")
    do! this.Expect(invalidRange).ToHaveAttributeAsync("aria-valuemax", "100")
    do! this.Expect(invalidRange).ToHaveAttributeAsync("aria-valuenow", "50")
  }

  [<Fact>]
  member this.``buffer uses range and never trails completion``() = task {
    do! this.NavigateTo("progress")
    let bar = this.Page.GetByTestId("linear")
    let! track = bar.BoundingBoxAsync()
    let! buffer = bar.Locator(".weave-progress-linear__buffer").BoundingBoxAsync()
    Assert.InRange(float (buffer.Width / track.Width), 0.749, 0.751)
    do! this.Page.GetByTestId("buffer").ClickAsync()

    do!
      this
        .Expect(bar.Locator(".weave-progress-linear__buffer"))
        .ToHaveCSSAsync("width", string (track.Width / 4.0f) + "px")

    let! buffer = bar.Locator(".weave-progress-linear__buffer").BoundingBoxAsync()
    let! fill = bar.Locator(".weave-progress-linear__fill").BoundingBoxAsync()
    Assert.Equal(fill.Width, buffer.Width)
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``linear fills its container and circular content stays centered``(width: int) = task {
    do! this.NavigateTo("progress")
    do! this.Page.SetViewportSizeAsync(width, 800)
    let! bar = this.Page.GetByTestId("linear").BoundingBoxAsync()
    Assert.InRange(float bar.Width, float width - 33.0, float width - 31.0)
    let circle = this.Page.GetByTestId("circular")
    let! bounds = circle.BoundingBoxAsync()
    Assert.Equal(56.0f, bounds.Width)
    Assert.Equal(bounds.Width, bounds.Height)
    let! content = circle.Locator(".weave-progress-circular__content").BoundingBoxAsync()
    Assert.Equal(bounds.X, content.X)
    Assert.Equal(bounds.Y, content.Y)
    Assert.Equal(bounds.Width, content.Width)
    do! this.Expect(circle).ToContainTextAsync("Done")
  }

  [<Fact>]
  member this.``RTL progress fills from the right``() = task {
    do! this.NavigateTo("progress")
    let bar = this.Page.GetByTestId("rtl-linear")
    let! bounds = bar.BoundingBoxAsync()
    let! fill = bar.Locator(".weave-progress-linear__fill").BoundingBoxAsync()
    Assert.InRange(float (fill.X + fill.Width - bounds.X - bounds.Width), -0.1, 0.1)
    Assert.InRange(float (fill.Width / bounds.Width), 0.249, 0.251)
  }

  [<Fact>]
  member this.``reduced motion stops both unknown indicators``() = task {
    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ReducedMotion = ReducedMotion.Reduce))
    do! this.NavigateTo("progress")

    do!
      this
        .Expect(this.Page.GetByTestId("unknown-linear").Locator(".weave-progress-linear__fill"))
        .ToHaveCSSAsync("animation-name", "none")

    do!
      this
        .Expect(this.Page.GetByTestId("unknown-circular").Locator("svg"))
        .ToHaveCSSAsync("animation-name", "none")

    do! this.Expect(this.Page.GetByTestId("unknown-linear")).ToBeVisibleAsync()
    do! this.Expect(this.Page.GetByTestId("unknown-circular")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``standard motion animates unknown indicators``() = task {
    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ReducedMotion = ReducedMotion.NoPreference))
    do! this.NavigateTo("progress")

    do!
      this
        .Expect(this.Page.GetByTestId("unknown-linear").Locator(".weave-progress-linear__fill"))
        .ToHaveCSSAsync("animation-name", "weave-progress-linear")

    do!
      this
        .Expect(this.Page.GetByTestId("unknown-circular").Locator("svg"))
        .ToHaveCSSAsync("animation-name", "weave-progress-circular")
  }
