module Weave.Tests.Rendering.AppBarLayoutTests

open Xunit

type AppBarLayoutTests() =
  inherit LayoutTestBase("appbar")

  [<Fact>]
  member this.``static appbar spans the full viewport width``() = task {
    do! this.LoadFixture()
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()
    and! vp = this.Page.EvaluateAsync<int>("() => window.innerWidth")

    Assert.True(
      abs (bar.Width - float32 vp) <= 1.0f,
      $"AppBar width {bar.Width}px should match viewport {vp}px"
    )
  }

  [<Fact>]
  member this.``static appbar meets minimum height``() = task {
    do! this.LoadFixture()
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()

    Assert.True(bar.Height >= 64.0f, $"AppBar height {bar.Height}px should be >= 64px")
  }

  [<Fact>]
  member this.``content after static appbar is placed below it``() = task {
    do! this.LoadFixture()
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()
    and! content = this.Page.Locator("#content-after-static").BoundingBoxAsync()

    Assert.True(
      content.Y >= bar.Y + bar.Height - 1.0f,
      $"Content (y={content.Y}) should start at or below appbar bottom ({bar.Y + bar.Height})"
    )
  }

  [<Fact>]
  member this.``fixed appbar has CSS position fixed``() = task {
    do! this.LoadFixture()

    let! position = this.ComputedStyle("#appbar-fixed", "position")

    Assert.Equal("fixed", position)
  }

  [<Theory>]
  [<InlineData(375)>]
  [<InlineData(599)>]
  [<InlineData(600)>]
  [<InlineData(960)>]
  [<InlineData(1280)>]
  member this.``static appbar spans full width at all viewport sizes``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()

    Assert.True(
      abs (bar.Width - float32 viewportWidth) <= 1.0f,
      $"AppBar width {bar.Width}px should match viewport {viewportWidth}px"
    )
  }

  [<Theory>]
  [<InlineData(375)>]
  [<InlineData(1280)>]
  member this.``appbar min height is preserved at mobile and desktop``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()

    Assert.True(
      bar.Height >= 48.0f,
      $"AppBar height {bar.Height}px should be >= 48px at {viewportWidth}px viewport"
    )
  }
