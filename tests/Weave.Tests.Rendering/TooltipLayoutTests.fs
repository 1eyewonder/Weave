module Weave.Tests.Rendering.TooltipLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type TooltipLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "tooltip.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``tooltip root has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#tooltip-root").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Tooltip root width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Tooltip root height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``top-center tooltip is visible when forced displayed``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#tooltip-top-center").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Tooltip width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Tooltip height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``top-center tooltip is above its trigger``() = task {
    do! this.LoadFixture()
    let! tooltip = this.Page.Locator("#tooltip-top-center").BoundingBoxAsync()
    let! trigger = this.Page.Locator("#tooltip-trigger").BoundingBoxAsync()

    let tooltipBottom = tooltip.Y + tooltip.Height

    Assert.True(
      tooltipBottom <= trigger.Y + 1.0f,
      $"Top-center tooltip bottom ({tooltipBottom}px) should be at or above trigger top ({trigger.Y}px)"
    )
  }

  [<Fact>]
  member this.``bottom-center tooltip is below its trigger``() = task {
    do! this.LoadFixture()
    let! tooltip = this.Page.Locator("#tooltip-bottom-center").BoundingBoxAsync()
    let! trigger = this.Page.Locator("#tooltip-trigger-bottom").BoundingBoxAsync()

    let triggerBottom = trigger.Y + trigger.Height

    Assert.True(
      tooltip.Y >= triggerBottom - 1.0f,
      $"Bottom-center tooltip top ({tooltip.Y}px) should be at or below trigger bottom ({triggerBottom}px)"
    )
  }

  [<Fact>]
  member this.``tooltip with arrow has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#tooltip-arrow").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Arrow tooltip width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Arrow tooltip height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``tooltip root renders as inline-block``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page.Locator("#tooltip-root").EvaluateAsync<string>("el => window.getComputedStyle(el).display")

    Assert.Equal("inline-block", display)
  }
