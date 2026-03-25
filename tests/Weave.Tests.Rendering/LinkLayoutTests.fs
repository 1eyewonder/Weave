module Weave.Tests.Rendering.LinkLayoutTests

open Xunit

type LinkLayoutTests() =
  inherit LayoutTestBase("link")

  [<Fact>]
  member this.``standard link has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#link-standard").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Standard link width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Standard link height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``icon link has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#link-icon").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Icon link width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Icon link height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``start icon is left of text``() = task {
    do! this.LoadFixture()
    let! icon = this.Page.Locator("#link-with-start-icon .weave-link__start-icon").BoundingBoxAsync()
    and! text = this.Page.Locator("#link-with-start-icon .weave-link__text").BoundingBoxAsync()

    Assert.True(icon.X < text.X, $"Start icon (x={icon.X}) should be left of text (x={text.X})")
  }

  [<Fact>]
  member this.``disabled link has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents =
      this.Page
        .Locator("#link-disabled")
        .EvaluateAsync<string>("el => window.getComputedStyle(el).pointerEvents")

    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``link renders as inline-flex``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page.Locator("#link-standard").EvaluateAsync<string>("el => window.getComputedStyle(el).display")

    Assert.Equal("inline-flex", display)
  }
