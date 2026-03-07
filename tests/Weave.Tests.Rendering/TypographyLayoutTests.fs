module Weave.Tests.Rendering.TypographyLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type TypographyLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "typography.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  member this.GetFontSizePx(selector: string) = task {
    return!
      this.Page.EvaluateAsync<float>(
        $"() => parseFloat(getComputedStyle(document.querySelector('{selector}')).fontSize)"
      )
  }

  [<Fact>]
  member this.``heading sizes decrease from h1 to h6``() = task {
    do! this.LoadFixture()
    let! h1 = this.GetFontSizePx "#h1"
    let! h2 = this.GetFontSizePx "#h2"
    let! h3 = this.GetFontSizePx "#h3"
    let! h4 = this.GetFontSizePx "#h4"
    let! h5 = this.GetFontSizePx "#h5"
    let! h6 = this.GetFontSizePx "#h6"

    Assert.True(h1 > h2, $"h1 ({h1}px) should be larger than h2 ({h2}px)")
    Assert.True(h2 > h3, $"h2 ({h2}px) should be larger than h3 ({h3}px)")
    Assert.True(h3 > h4, $"h3 ({h3}px) should be larger than h4 ({h4}px)")
    Assert.True(h4 > h5, $"h4 ({h4}px) should be larger than h5 ({h5}px)")
    Assert.True(h5 > h6, $"h5 ({h5}px) should be larger than h6 ({h6}px)")
  }

  [<Fact>]
  member this.``h6 font size is larger than body1``() = task {
    do! this.LoadFixture()
    let! h6 = this.GetFontSizePx "#h6"
    let! body1 = this.GetFontSizePx "#body1"

    Assert.True(h6 > body1, $"h6 ({h6}px) should be larger than body1 ({body1}px)")
  }

  [<Fact>]
  member this.``body1 font size is larger than body2``() = task {
    do! this.LoadFixture()
    let! body1 = this.GetFontSizePx "#body1"
    let! body2 = this.GetFontSizePx "#body2"

    Assert.True(body1 > body2, $"body1 ({body1}px) should be larger than body2 ({body2}px)")
  }

  [<Fact>]
  member this.``body2 font size is larger than caption``() = task {
    do! this.LoadFixture()
    let! body2 = this.GetFontSizePx "#body2"
    let! caption = this.GetFontSizePx "#caption"

    Assert.True(body2 > caption, $"body2 ({body2}px) should be larger than caption ({caption}px)")
  }

  [<Fact>]
  member this.``nowrap text does not wrap to multiple lines``() = task {
    do! this.LoadFixture()
    let! nowrap = this.Page.Locator("#nowrap").BoundingBoxAsync()
    let! wrap = this.Page.Locator("#wrap").BoundingBoxAsync()

    Assert.True(
      nowrap.Height < wrap.Height,
      $"Nowrap element ({nowrap.Height}px tall) should be shorter than wrapping element ({wrap.Height}px tall)"
    )
  }
