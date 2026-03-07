module Weave.Tests.Rendering.ContainerLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type ContainerLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "container.html")

  member this.LoadFixture(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``maxwidth-xs container is constrained to 444px at wide viewport``() = task {
    do! this.LoadFixture 600
    let! container = this.Page.Locator("#container-xs").BoundingBoxAsync()

    Assert.True(
      container.Width <= 444.0f + 1.0f,
      $"xs container width {container.Width}px should be <= 444px at 600px viewport"
    )
  }

  [<Fact>]
  member this.``maxwidth-xs container is centered horizontally``() = task {
    do! this.LoadFixture 600
    let! container = this.Page.Locator("#container-xs").BoundingBoxAsync()

    let leftMargin = container.X
    let rightMargin = 600.0f - (container.X + container.Width)

    Assert.True(
      abs (leftMargin - rightMargin) <= 1.0f,
      $"Container should be centered: left margin {leftMargin}px vs right margin {rightMargin}px"
    )
  }

  [<Fact>]
  member this.``maxwidth-sm container fills viewport below breakpoint``() = task {
    do! this.LoadFixture 400
    let! container = this.Page.Locator("#container-sm").BoundingBoxAsync()

    Assert.True(
      abs (container.Width - 400.0f) <= 1.0f,
      $"sm container width {container.Width}px should fill the 400px viewport"
    )
  }

  [<Fact>]
  member this.``gutters container has 16px padding at narrow viewport``() = task {
    do! this.LoadFixture 400

    let! paddingLeft =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#container-gutters')).paddingLeft"
      )

    Assert.Equal("16px", paddingLeft)
  }

  [<Fact>]
  member this.``gutters container has 24px padding at wide viewport``() = task {
    do! this.LoadFixture 600

    let! paddingLeft =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#container-gutters')).paddingLeft"
      )

    Assert.Equal("24px", paddingLeft)
  }
