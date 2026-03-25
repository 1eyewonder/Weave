module Weave.Tests.Rendering.ExpansionPanelLayoutTests

open Xunit

type ExpansionPanelLayoutTests() =
  inherit LayoutTestBase("expansion-panel")

  [<Fact>]
  member this.``collapsed panel content wrapper has zero-fraction grid rows``() = task {
    do! this.LoadFixture()

    let! gridRows = this.ComputedStyle("#wrapper-collapsed", "gridTemplateRows")

    Assert.Equal("0px", gridRows)
  }

  [<Fact>]
  member this.``collapsed panel content is hidden via overflow``() = task {
    do! this.LoadFixture()

    let! overflow = this.ComputedStyle("#content-collapsed", "overflow")

    Assert.Equal("hidden", overflow)
  }

  [<Fact>]
  member this.``expanded panel content has positive rendered height``() = task {
    do! this.LoadFixture()

    let! height =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#content-expanded').getBoundingClientRect().height"
      )

    Assert.True(height > 0.0, $"Expanded content height {height}px should be > 0")
  }

  [<Fact>]
  member this.``expanded panel header is above its content``() = task {
    do! this.LoadFixture()
    let! header = this.Page.Locator("#header-expanded").BoundingBoxAsync()

    and! contentTop =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#content-expanded').getBoundingClientRect().top"
      )

    Assert.True(
      float (header.Y + header.Height) <= contentTop + 1.0,
      $"Header bottom (y={header.Y + header.Height}) should be at or above content top (y={contentTop})"
    )
  }

  [<Fact>]
  member this.``collapsed panel header has positive height``() = task {
    do! this.LoadFixture()
    let! header = this.Page.Locator("#header-collapsed").BoundingBoxAsync()

    Assert.True(header.Height > 0.0f, $"Collapsed panel header height {header.Height}px should be > 0")
  }

  [<Fact>]
  member this.``group panels stack vertically``() = task {
    do! this.LoadFixture()
    let! panel1 = this.Page.Locator("#group-panel-1").BoundingBoxAsync()
    and! panel2 = this.Page.Locator("#group-panel-2").BoundingBoxAsync()
    and! panel3 = this.Page.Locator("#group-panel-3").BoundingBoxAsync()

    Assert.True(panel1.Y < panel2.Y, $"Panel 1 (y={panel1.Y}) should be above panel 2 (y={panel2.Y})")
    Assert.True(panel2.Y < panel3.Y, $"Panel 2 (y={panel2.Y}) should be above panel 3 (y={panel3.Y})")
  }

  [<Fact>]
  member this.``only expanded panel in group has visible content``() = task {
    do! this.LoadFixture()

    let! content1Height =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#group-content-1').getBoundingClientRect().height"
      )

    and! content2Height =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#group-content-2').getBoundingClientRect().height"
      )

    and! content3Height =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#group-content-3').getBoundingClientRect().height"
      )

    Assert.Equal(0.0, content1Height)
    Assert.True(content2Height > 0.0, $"Expanded panel content height {content2Height}px should be > 0")
    Assert.Equal(0.0, content3Height)
  }

  [<Fact>]
  member this.``focus color class overrides the focus color custom property``() = task {
    do! this.LoadFixture()

    let! defaultColor =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#header-collapsed')).getPropertyValue('--expansion-panel-focus-color').trim()"
      )

    and! focusColor =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#header-focus-color')).getPropertyValue('--expansion-panel-focus-color').trim()"
      )

    Assert.NotEqual<string>(defaultColor, focusColor)
  }

  [<Fact>]
  member this.``expanded panel in group is taller than collapsed siblings``() = task {
    do! this.LoadFixture()
    let! panel1 = this.Page.Locator("#group-panel-1").BoundingBoxAsync()
    and! panel2 = this.Page.Locator("#group-panel-2").BoundingBoxAsync()
    and! panel3 = this.Page.Locator("#group-panel-3").BoundingBoxAsync()

    Assert.True(
      panel2.Height > panel1.Height,
      $"Expanded panel ({panel2.Height}px) should be taller than collapsed panel 1 ({panel1.Height}px)"
    )

    Assert.True(
      panel2.Height > panel3.Height,
      $"Expanded panel ({panel2.Height}px) should be taller than collapsed panel 3 ({panel3.Height}px)"
    )
  }
