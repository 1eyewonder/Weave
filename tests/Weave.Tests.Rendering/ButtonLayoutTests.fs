module Weave.Tests.Rendering.ButtonLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type ButtonLayoutTests() =
  inherit LayoutTestBase("button")

  // Boundary pairs for CSS breakpoints: xs/sm (600px), sm/md (960px), md/lg (1280px)
  static member ViewportWidths: obj[][] = [|
    [| 599 |]
    [| 600 |]
    [| 959 |]
    [| 960 |]
    [| 1279 |]
    [| 1280 |]
  |]

  [<Theory>]
  [<InlineData("#btn-compact", 30.0f)>]
  [<InlineData("#btn-standard", 40.0f)>]
  [<InlineData("#btn-spacious", 50.0f)>]
  member this.``button meets density min height``(buttonId: string, minHeight: float32) = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator(buttonId).BoundingBoxAsync()

    Assert.True(box.Height >= minHeight, $"{buttonId} height {box.Height}px should be >= {minHeight}px")
  }

  [<Fact>]
  member this.``density ordering is preserved``() = task {
    do! this.LoadFixture()
    let! compact = this.Page.Locator("#btn-compact").BoundingBoxAsync()
    and! standard = this.Page.Locator("#btn-standard").BoundingBoxAsync()
    and! spacious = this.Page.Locator("#btn-spacious").BoundingBoxAsync()

    Assert.True(
      compact.Height < standard.Height,
      $"Compact ({compact.Height}px) should be shorter than standard ({standard.Height}px)"
    )

    Assert.True(
      standard.Height < spacious.Height,
      $"Standard ({standard.Height}px) should be shorter than spacious ({spacious.Height}px)"
    )
  }

  [<Theory>]
  [<InlineData("#btn-compact")>]
  [<InlineData("#btn-standard")>]
  [<InlineData("#btn-spacious")>]
  member this.``filled button has min width``(buttonId: string) = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator(buttonId).BoundingBoxAsync()

    Assert.True(box.Width >= 64.0f, $"{buttonId} width {box.Width}px should be >= 64px")
  }

  [<Fact>]
  member this.``icon button does not enforce min width``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#btn-icon").BoundingBoxAsync()

    Assert.True(
      box.Width < 64.0f,
      $"Icon button width {box.Width}px should be < 64px (min-width is overridden to auto)"
    )
  }

  [<Theory>]
  [<MemberData(nameof ButtonLayoutTests.ViewportWidths)>]
  member this.``full width button fills container``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! containerBox = this.Page.Locator("#full-width-container").BoundingBoxAsync()
    and! buttonBox = this.Page.Locator("#btn-full-width").BoundingBoxAsync()

    Assert.True(
      abs (buttonBox.Width - containerBox.Width) <= 1.0f,
      $"Full-width button {buttonBox.Width}px should fill its container {containerBox.Width}px at {viewportWidth}px viewport"
    )
  }

  member private this.LoadFixtureWithTheme(viewportWidth: int, theme: string) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    let! _ = this.Page.EvaluateAsync($"document.documentElement.setAttribute('data-theme', '{theme}')")
    ()
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``filled button has opaque background in both themes``(theme: string) = task {
    do! this.LoadFixtureWithTheme(1280, theme)

    let! bg =
      this.Page.Locator("#btn-filled").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.False(
      bg = "transparent" || bg = "rgba(0, 0, 0, 0)",
      $"Filled button background in {theme} theme ('{bg}') should not be transparent"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``button layout is stable across themes``(theme: string) = task {
    do! this.LoadFixture()
    let! lightBox = this.Page.Locator("#btn-standard").BoundingBoxAsync()
    let! _ = this.Page.EvaluateAsync($"document.documentElement.setAttribute('data-theme', '{theme}')")
    ()
    let! themedBox = this.Page.Locator("#btn-standard").BoundingBoxAsync()

    Assert.True(
      abs (lightBox.Height - themedBox.Height) <= 1.0f,
      $"Button height should not shift between themes (default={lightBox.Height}px, {theme}={themedBox.Height}px)"
    )

    Assert.True(
      abs (lightBox.Width - themedBox.Width) <= 1.0f,
      $"Button width should not shift between themes (default={lightBox.Width}px, {theme}={themedBox.Width}px)"
    )
  }

  [<Fact>]
  member this.``standard button height is consistent across variants``() = task {
    do! this.LoadFixture()
    let! filled = this.Page.Locator("#btn-filled").BoundingBoxAsync()
    and! outlined = this.Page.Locator("#btn-outlined").BoundingBoxAsync()
    and! text = this.Page.Locator("#btn-text").BoundingBoxAsync()

    Assert.True(
      abs (filled.Height - outlined.Height) <= 1.0f,
      $"Filled ({filled.Height}px) and outlined ({outlined.Height}px) standard buttons should have the same height"
    )

    Assert.True(
      abs (filled.Height - text.Height) <= 1.0f,
      $"Filled ({filled.Height}px) and text ({text.Height}px) standard buttons should have the same height"
    )
  }

  [<Theory>]
  [<InlineData("#btn-primary")>]
  [<InlineData("#btn-error")>]
  [<InlineData("#btn-success")>]
  member this.``color variant buttons have opaque background``(buttonId: string) = task {
    do! this.LoadFixture()

    let! bg = this.Page.Locator(buttonId).EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.False(
      bg = "transparent" || bg = "rgba(0, 0, 0, 0)",
      $"{buttonId} should have an opaque background (got '{bg}')"
    )
  }

  [<Fact>]
  member this.``color variant button height matches default filled``() = task {
    do! this.LoadFixture()
    let! filled = this.Page.Locator("#btn-filled").BoundingBoxAsync()
    and! primary = this.Page.Locator("#btn-primary").BoundingBoxAsync()
    and! error = this.Page.Locator("#btn-error").BoundingBoxAsync()

    Assert.True(
      abs (filled.Height - primary.Height) <= 1.0f,
      $"Primary ({primary.Height}px) height should match default filled ({filled.Height}px)"
    )

    Assert.True(
      abs (filled.Height - error.Height) <= 1.0f,
      $"Error ({error.Height}px) height should match default filled ({filled.Height}px)"
    )
  }

  [<Fact>]
  member this.``outlined color variant has visible border``() = task {
    do! this.LoadFixture()

    let! border =
      this.Page
        .Locator("#btn-outlined-primary")
        .EvaluateAsync<string>("el => getComputedStyle(el).borderStyle")

    Assert.False((border = "none"), $"Outlined primary button should have a visible border (got '{border}')")
  }

  [<Fact>]
  member this.``disabled button has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents =
      this.Page.Locator("#btn-disabled").EvaluateAsync<string>("el => getComputedStyle(el).pointerEvents")

    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``disabled button has default cursor``() = task {
    do! this.LoadFixture()

    let! cursor =
      this.Page.Locator("#btn-disabled").EvaluateAsync<string>("el => getComputedStyle(el).cursor")

    Assert.Equal("default", cursor)
  }

  [<Fact>]
  member this.``disabled button has no box shadow``() = task {
    do! this.LoadFixture()

    let! shadow =
      this.Page.Locator("#btn-disabled").EvaluateAsync<string>("el => getComputedStyle(el).boxShadow")

    Assert.Equal("none", shadow)
  }

  [<Fact>]
  member this.``label is contained within filled button``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#btn-filled").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#btn-filled .weave-button__label").BoundingBoxAsync()
    assertContainedWithin "filled button" "label" parentBox childBox
  }

  [<Fact>]
  member this.``label is contained within compact button``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#btn-compact").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#btn-compact .weave-button__label").BoundingBoxAsync()
    assertContainedWithin "compact button" "label" parentBox childBox
  }

  [<Fact>]
  member this.``icon content is contained within icon button``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#btn-icon").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#btn-icon > span").BoundingBoxAsync()
    assertContainedWithin "icon button" "icon content" parentBox childBox
  }

  [<Fact>]
  member this.``label fills button height``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#btn-filled").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#btn-filled .weave-button__label").BoundingBoxAsync()
    assertFillsHeight "filled button" "label" parentBox childBox
  }

  [<Fact>]
  member this.``icon fills icon button height``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#btn-icon").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#btn-icon > span").BoundingBoxAsync()
    assertFillsHeight "icon button" "icon" parentBox childBox
  }
