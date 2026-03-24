module Weave.Tests.Rendering.TabsLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection
open Weave.Tests.Rendering.ContainmentAssertions

type TabsLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "tabs.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  member this.LoadFixture(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``horizontal tab buttons are arranged left to right``() = task {
    do! this.LoadFixture()
    let! tab0 = this.Page.Locator("#tab-0").BoundingBoxAsync()
    let! tab1 = this.Page.Locator("#tab-1").BoundingBoxAsync()
    let! tab2 = this.Page.Locator("#tab-2").BoundingBoxAsync()

    Assert.True(tab0.X < tab1.X, $"Tab 0 (x={tab0.X}) should be left of tab 1 (x={tab1.X})")
    Assert.True(tab1.X < tab2.X, $"Tab 1 (x={tab1.X}) should be left of tab 2 (x={tab2.X})")
  }

  [<Fact>]
  member this.``horizontal tab buttons share the same top edge``() = task {
    do! this.LoadFixture()
    let! tab0 = this.Page.Locator("#tab-0").BoundingBoxAsync()
    let! tab1 = this.Page.Locator("#tab-1").BoundingBoxAsync()
    let! tab2 = this.Page.Locator("#tab-2").BoundingBoxAsync()

    Assert.True(
      abs (tab0.Y - tab1.Y) <= 1.0f,
      $"Tabs 0 and 1 should share the same top (y0={tab0.Y}, y1={tab1.Y})"
    )

    Assert.True(
      abs (tab0.Y - tab2.Y) <= 1.0f,
      $"Tabs 0 and 2 should share the same top (y0={tab0.Y}, y2={tab2.Y})"
    )
  }

  [<Fact>]
  member this.``panels area is below the tab header``() = task {
    do! this.LoadFixture()
    let! header = this.Page.Locator("#tabs-header-wrapper").BoundingBoxAsync()
    let! panels = this.Page.Locator("#tabs-panels").BoundingBoxAsync()

    Assert.True(
      panels.Y >= header.Y + header.Height - 1.0f,
      $"Panels (y={panels.Y}) should be below the header (bottom={header.Y + header.Height})"
    )
  }

  [<Fact>]
  member this.``active panel has visible height``() = task {
    do! this.LoadFixture()
    let! panel0 = this.Page.Locator("#panel-0").BoundingBoxAsync()

    Assert.True(panel0.Height > 0.0f, $"Active panel height {panel0.Height}px should be > 0")
  }

  [<Fact>]
  member this.``inactive panel is hidden``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page.EvaluateAsync<string>("() => getComputedStyle(document.querySelector('#panel-1')).display")

    Assert.Equal("none", display)
  }

  [<Fact>]
  member this.``bottom tabs use column-reverse layout``() = task {
    do! this.LoadFixture()

    let! flexDirection =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#tabs-bottom')).flexDirection"
      )

    Assert.Equal("column-reverse", flexDirection)
  }

  [<Fact>]
  member this.``start tabs header is left of panels``() = task {
    do! this.LoadFixture()
    let! headerBox = this.Page.Locator("#tabs-start-header-wrapper").BoundingBoxAsync()
    let! tabsBox = this.Page.Locator("#tabs-start").BoundingBoxAsync()

    // Header wrapper should be on the left side of the tabs container
    Assert.True(
      headerBox.X <= tabsBox.X + 1.0f,
      $"Start tabs header X ({headerBox.X}px) should be at left of tabs container (X={tabsBox.X}px)"
    )
  }

  [<Theory>]
  [<InlineData(375)>]
  [<InlineData(599)>]
  [<InlineData(960)>]
  [<InlineData(1280)>]
  member this.``tabs header fills container width at all viewports``(viewportWidth: int) = task {
    do! this.LoadFixture viewportWidth
    let! tabsBox = this.Page.Locator("#tabs-top").BoundingBoxAsync()
    let! headerBox = this.Page.Locator("#tabs-header-wrapper").BoundingBoxAsync()

    Assert.True(
      abs (headerBox.Width - tabsBox.Width) <= 1.0f,
      $"Tabs header width ({headerBox.Width}px) should fill container ({tabsBox.Width}px) at {viewportWidth}px viewport"
    )
  }

  [<Theory>]
  [<InlineData("#tab-0")>]
  [<InlineData("#tab-1")>]
  [<InlineData("#tab-2")>]
  member this.``tab button is contained within header``(tabId: string) = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#tabs-header").BoundingBoxAsync()
    let! childBox = this.Page.Locator(tabId).BoundingBoxAsync()
    assertContainedWithin "header" $"tab button ({tabId})" parentBox childBox
  }

  [<Fact>]
  member this.``panels area is contained within tabs root``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#tabs-top").BoundingBoxAsync()
    let! childBox = this.Page.Locator("#tabs-panels").BoundingBoxAsync()
    assertContainedWithin "tabs root" "panels area" parentBox childBox
  }

  [<Theory>]
  [<InlineData("#tab-0")>]
  [<InlineData("#tab-1")>]
  [<InlineData("#tab-2")>]
  member this.``tab button fills header height``(tabId: string) = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#tabs-header").BoundingBoxAsync()
    let! childBox = this.Page.Locator(tabId).BoundingBoxAsync()
    assertFillsHeight "header" $"tab button ({tabId})" parentBox childBox
  }
