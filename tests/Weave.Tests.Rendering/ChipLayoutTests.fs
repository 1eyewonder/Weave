module Weave.Tests.Rendering.ChipLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type ChipLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "chip.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``chip has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#chip-basic").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Chip width {box.Width}px should be positive")
    Assert.True(box.Height > 0.0f, $"Chip height {box.Height}px should be positive")
  }

  [<Fact>]
  member this.``chip uses inline-flex display``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page.Locator("#chip-basic").EvaluateAsync<string>("el => getComputedStyle(el).display")

    Assert.Equal("inline-flex", display)
  }

  [<Fact>]
  member this.``chip has pill border-radius``() = task {
    do! this.LoadFixture()

    let! radius =
      this.Page.Locator("#chip-basic").EvaluateAsync<string>("el => getComputedStyle(el).borderRadius")

    Assert.Equal("9999px", radius)
  }

  [<Fact>]
  member this.``filled chip has opaque background``() = task {
    do! this.LoadFixture()

    let! bg =
      this.Page.Locator("#chip-basic").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.False(
      bg.Contains("rgba") && bg.EndsWith(", 0)"),
      $"Filled chip background '{bg}' should not be fully transparent"
    )
  }

  [<Fact>]
  member this.``outlined chip has transparent background``() = task {
    do! this.LoadFixture()

    let! bg =
      this.Page.Locator("#chip-outlined").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    let isTransparent = bg = "transparent" || bg = "rgba(0, 0, 0, 0)"

    Assert.True(isTransparent, $"Outlined chip background '{bg}' should be transparent")
  }

  [<Fact>]
  member this.``text chip has transparent background``() = task {
    do! this.LoadFixture()

    let! bg =
      this.Page.Locator("#chip-text").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    let isTransparent = bg = "transparent" || bg = "rgba(0, 0, 0, 0)"

    Assert.True(isTransparent, $"Text chip background '{bg}' should be transparent")
  }

  [<Fact>]
  member this.``outlined chip has visible border``() = task {
    do! this.LoadFixture()

    let! borderWidth =
      this.Page.Locator("#chip-outlined").EvaluateAsync<string>("el => getComputedStyle(el).borderWidth")

    Assert.Equal("1px", borderWidth)
  }

  [<Fact>]
  member this.``compact chip is shorter than spacious chip``() = task {
    do! this.LoadFixture()
    let! compact = this.Page.Locator("#chip-compact").BoundingBoxAsync()
    let! spacious = this.Page.Locator("#chip-spacious").BoundingBoxAsync()

    Assert.True(
      compact.Height < spacious.Height,
      $"Compact chip ({compact.Height}px) should be shorter than spacious chip ({spacious.Height}px)"
    )
  }

  [<Fact>]
  member this.``density ordering is preserved``() = task {
    do! this.LoadFixture()
    let! compact = this.Page.Locator("#chip-compact").BoundingBoxAsync()
    let! standard = this.Page.Locator("#chip-standard").BoundingBoxAsync()
    let! spacious = this.Page.Locator("#chip-spacious").BoundingBoxAsync()

    Assert.True(
      compact.Height < standard.Height,
      $"Compact ({compact.Height}px) should be shorter than standard ({standard.Height}px)"
    )

    Assert.True(
      standard.Height < spacious.Height,
      $"Standard ({standard.Height}px) should be shorter than spacious ({spacious.Height}px)"
    )
  }

  [<Fact>]
  member this.``disabled chip has reduced opacity``() = task {
    do! this.LoadFixture()

    let! opacity =
      this.Page.Locator("#chip-disabled").EvaluateAsync<string>("el => getComputedStyle(el).opacity")

    let opacityValue = System.Double.Parse(opacity)

    Assert.True(opacityValue < 1.0, $"Disabled chip opacity ({opacity}) should be less than 1")
  }

  [<Fact>]
  member this.``disabled chip has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents =
      this.Page.Locator("#chip-disabled").EvaluateAsync<string>("el => getComputedStyle(el).pointerEvents")

    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``label chip has non-pill border-radius``() = task {
    do! this.LoadFixture()

    let! labelRadius =
      this.Page.Locator("#chip-label").EvaluateAsync<string>("el => getComputedStyle(el).borderRadius")

    let! basicRadius =
      this.Page.Locator("#chip-basic").EvaluateAsync<string>("el => getComputedStyle(el).borderRadius")

    Assert.NotEqual<string>(labelRadius, basicRadius)
  }

  [<Fact>]
  member this.``chip with content contains content element``() = task {
    do! this.LoadFixture()
    let! contentCount = this.Page.Locator("#chip-icon .weave-chip__content").CountAsync()

    Assert.True(contentCount > 0, "Chip with content should contain a .weave-chip__content element")
  }

  [<Fact>]
  member this.``empty content element is hidden``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page
        .Locator("#chip-empty-content .weave-chip__content")
        .EvaluateAsync<string>("el => getComputedStyle(el).display")

    Assert.Equal("none", display)
  }

  [<Fact>]
  member this.``close button is contained within chip bounds``() = task {
    do! this.LoadFixture()
    let! chipBox = this.Page.Locator("#chip-closable").BoundingBoxAsync()
    let! closeBox = this.Page.Locator("#chip-closable .weave-chip__close").BoundingBoxAsync()

    Assert.True(
      closeBox.X >= chipBox.X - 1.0f
      && closeBox.X + closeBox.Width <= chipBox.X + chipBox.Width + 1.0f,
      $"Close button (x:{closeBox.X}, w:{closeBox.Width}) should be within chip bounds (x:{chipBox.X}, w:{chipBox.Width})"
    )

    Assert.True(
      closeBox.Y >= chipBox.Y - 1.0f
      && closeBox.Y + closeBox.Height <= chipBox.Y + chipBox.Height + 1.0f,
      $"Close button (y:{closeBox.Y}, h:{closeBox.Height}) should be within chip bounds (y:{chipBox.Y}, h:{chipBox.Height})"
    )
  }

  [<Fact>]
  member this.``close button has reduced opacity``() = task {
    do! this.LoadFixture()

    let! opacity =
      this.Page
        .Locator("#chip-closable .weave-chip__close")
        .EvaluateAsync<string>("el => getComputedStyle(el).opacity")

    let opacityValue = System.Double.Parse(opacity)

    Assert.True(abs (opacityValue - 0.7) < 0.01, $"Close button opacity ({opacity}) should be 0.7")
  }

  [<Fact>]
  member this.``close button has pointer cursor``() = task {
    do! this.LoadFixture()

    let! cursor =
      this.Page
        .Locator("#chip-closable .weave-chip__close")
        .EvaluateAsync<string>("el => getComputedStyle(el).cursor")

    Assert.Equal("pointer", cursor)
  }

  [<Fact>]
  member this.``selected chip has different background than unselected``() = task {
    do! this.LoadFixture()

    let! selectedBg =
      this.Page.Locator("#chip-selected").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    let! unselectedBg =
      this.Page.Locator("#chip-basic").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.NotEqual<string>(selectedBg, unselectedBg)
  }

  [<Fact>]
  member this.``clickable chip has pointer cursor``() = task {
    do! this.LoadFixture()

    let! cursor =
      this.Page.Locator("#chip-clickable").EvaluateAsync<string>("el => getComputedStyle(el).cursor")

    Assert.Equal("pointer", cursor)
  }

  [<Fact>]
  member this.``non-clickable chip has default cursor``() = task {
    do! this.LoadFixture()

    let! cursor = this.Page.Locator("#chip-basic").EvaluateAsync<string>("el => getComputedStyle(el).cursor")

    Assert.Equal("default", cursor)
  }

  [<Fact>]
  member this.``label text has ellipsis overflow``() = task {
    do! this.LoadFixture()

    let! textOverflow =
      this.Page
        .Locator("#chip-basic .weave-chip__label")
        .EvaluateAsync<string>("el => getComputedStyle(el).textOverflow")

    let! whiteSpace =
      this.Page
        .Locator("#chip-basic .weave-chip__label")
        .EvaluateAsync<string>("el => getComputedStyle(el).whiteSpace")

    let! overflow =
      this.Page
        .Locator("#chip-basic .weave-chip__label")
        .EvaluateAsync<string>("el => getComputedStyle(el).overflow")

    Assert.Equal("ellipsis", textOverflow)
    Assert.Equal("nowrap", whiteSpace)
    Assert.Equal("hidden", overflow)
  }
