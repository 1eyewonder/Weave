module Weave.Tests.Rendering.AppBarLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type AppBarLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "appbar.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``static appbar spans the full viewport width``() = task {
    do! this.LoadFixture()
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()
    let! vp = this.Page.EvaluateAsync<int>("() => window.innerWidth")

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
    let! content = this.Page.Locator("#content-after-static").BoundingBoxAsync()

    Assert.True(
      content.Y >= bar.Y + bar.Height - 1.0f,
      $"Content (y={content.Y}) should start at or below appbar bottom ({bar.Y + bar.Height})"
    )
  }

  [<Fact>]
  member this.``fixed appbar has CSS position fixed``() = task {
    do! this.LoadFixture()

    let! position =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#appbar-fixed')).position"
      )

    Assert.Equal("fixed", position)
  }
