module Weave.Tests.Rendering.ChipLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type ChipLayoutTests() =
  inherit LayoutTestBase("chip")

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

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``filled chip has opaque background in both themes``(theme: string) = task {
    do! this.LoadFixture()
    do! this.SetTheme(theme)

    let! bg =
      this.Page.Locator("#chip-basic").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.False(
      bg = "transparent" || bg = "rgba(0, 0, 0, 0)",
      $"Filled chip background in {theme} theme ('{bg}') should not be transparent"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``chip layout is stable across themes``(theme: string) = task {
    do! this.LoadFixture()
    let! defaultBox = this.Page.Locator("#chip-basic").BoundingBoxAsync()
    do! this.SetTheme(theme)
    let! themedBox = this.Page.Locator("#chip-basic").BoundingBoxAsync()

    Assert.True(
      abs (defaultBox.Height - themedBox.Height) <= 1.0f,
      $"Chip height should not shift between themes (default={defaultBox.Height}px, {theme}={themedBox.Height}px)"
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
  member this.``density ordering is preserved``() = task {
    do! this.LoadFixture()
    let! compact = this.Page.Locator("#chip-compact").BoundingBoxAsync()
    and! standard = this.Page.Locator("#chip-standard").BoundingBoxAsync()
    and! spacious = this.Page.Locator("#chip-spacious").BoundingBoxAsync()

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

    and! basicRadius =
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
    and! closeBox = this.Page.Locator("#chip-closable .weave-chip__close").BoundingBoxAsync()
    assertContainedWithin "chip" "close button" chipBox closeBox
  }

  [<Fact>]
  member this.``close button fills chip height``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#chip-closable").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#chip-closable .weave-chip__close").BoundingBoxAsync()
    assertFillsHeight "chip" "close button" parentBox childBox
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

    and! unselectedBg =
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

    and! whiteSpace =
      this.Page
        .Locator("#chip-basic .weave-chip__label")
        .EvaluateAsync<string>("el => getComputedStyle(el).whiteSpace")

    and! overflow =
      this.Page
        .Locator("#chip-basic .weave-chip__label")
        .EvaluateAsync<string>("el => getComputedStyle(el).overflow")

    Assert.Equal("ellipsis", textOverflow)
    Assert.Equal("nowrap", whiteSpace)
    Assert.Equal("hidden", overflow)
  }
