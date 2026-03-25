module Weave.Tests.Rendering.AlertLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type AlertLayoutTests() =
  inherit LayoutTestBase("alert")

  [<Fact>]
  member this.``standard alert has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#alert-standard").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Standard alert height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``outlined alert has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#alert-outlined").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Outlined alert height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``filled alert has positive height``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#alert-filled").BoundingBoxAsync()

    Assert.True(box.Height > 0.0f, $"Filled alert height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``all three variants stack vertically``() = task {
    do! this.LoadFixture()
    let! standard = this.Page.Locator("#alert-standard").BoundingBoxAsync()
    and! outlined = this.Page.Locator("#alert-outlined").BoundingBoxAsync()
    and! filled = this.Page.Locator("#alert-filled").BoundingBoxAsync()

    Assert.True(
      standard.Y < outlined.Y,
      $"Standard (y={standard.Y}) should be above outlined (y={outlined.Y})"
    )

    Assert.True(outlined.Y < filled.Y, $"Outlined (y={outlined.Y}) should be above filled (y={filled.Y})")
  }

  [<Fact>]
  member this.``icon is left of content``() = task {
    do! this.LoadFixture()
    let! icon = this.Page.Locator("#alert-with-icon .weave-alert__icon").BoundingBoxAsync()
    and! content = this.Page.Locator("#alert-with-icon .weave-alert__content").BoundingBoxAsync()

    Assert.True(icon.X < content.X, $"Icon (x={icon.X}) should be left of content (x={content.X})")
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``filled alert has opaque background in both themes``(theme: string) = task {
    do! this.LoadFixture()
    do! this.SetTheme(theme)

    let! bg =
      this.Page.Locator("#alert-filled").EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.False(
      bg = "transparent" || bg = "rgba(0, 0, 0, 0)",
      $"Filled alert background in {theme} theme ('{bg}') should not be transparent"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``alert layout is stable across themes``(theme: string) = task {
    do! this.LoadFixture()
    let! defaultBox = this.Page.Locator("#alert-standard").BoundingBoxAsync()
    do! this.SetTheme(theme)
    let! themedBox = this.Page.Locator("#alert-standard").BoundingBoxAsync()

    Assert.True(
      abs (defaultBox.Height - themedBox.Height) <= 1.0f,
      $"Alert height should not shift between themes (default={defaultBox.Height}px, {theme}={themedBox.Height}px)"
    )
  }

  [<Fact>]
  member this.``close button is right of content``() = task {
    do! this.LoadFixture()
    let! content = this.Page.Locator("#alert-with-close .weave-alert__content").BoundingBoxAsync()
    and! close = this.Page.Locator("#alert-with-close .weave-alert__close").BoundingBoxAsync()

    Assert.True(content.X < close.X, $"Content (x={content.X}) should be left of close button (x={close.X})")
  }

  [<Fact>]
  member this.``icon is contained within alert``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#alert-with-icon").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#alert-with-icon .weave-alert__icon").BoundingBoxAsync()
    assertContainedWithin "alert" "icon" parentBox childBox
  }

  [<Fact>]
  member this.``close button is contained within alert``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#alert-with-close").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#alert-with-close .weave-alert__close").BoundingBoxAsync()
    assertContainedWithin "alert" "close button" parentBox childBox
  }

  [<Fact(Skip = "Requires component update: icon has vertical padding inside alert")>]
  member this.``icon fills alert height``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#alert-with-icon").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#alert-with-icon .weave-alert__icon").BoundingBoxAsync()
    assertFillsHeight "alert" "icon" parentBox childBox
  }
