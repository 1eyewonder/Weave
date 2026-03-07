module Weave.Tests.Rendering.ButtonGroupLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type ButtonGroupLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "button-group.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``horizontal group buttons share the same top edge``() = task {
    do! this.LoadFixture()
    let! btn1 = this.Page.Locator("#btn-h-1").BoundingBoxAsync()
    let! btn2 = this.Page.Locator("#btn-h-2").BoundingBoxAsync()
    let! btn3 = this.Page.Locator("#btn-h-3").BoundingBoxAsync()

    Assert.True(
      abs (btn1.Y - btn2.Y) <= 1.0f,
      $"Buttons 1 and 2 should share the same top (y1={btn1.Y}, y2={btn2.Y})"
    )

    Assert.True(
      abs (btn1.Y - btn3.Y) <= 1.0f,
      $"Buttons 1 and 3 should share the same top (y1={btn1.Y}, y3={btn3.Y})"
    )
  }

  [<Fact>]
  member this.``horizontal group buttons are arranged left to right``() = task {
    do! this.LoadFixture()
    let! btn1 = this.Page.Locator("#btn-h-1").BoundingBoxAsync()
    let! btn2 = this.Page.Locator("#btn-h-2").BoundingBoxAsync()
    let! btn3 = this.Page.Locator("#btn-h-3").BoundingBoxAsync()

    Assert.True(btn1.X < btn2.X, $"Button 1 (x={btn1.X}) should be left of button 2 (x={btn2.X})")
    Assert.True(btn2.X < btn3.X, $"Button 2 (x={btn2.X}) should be left of button 3 (x={btn3.X})")
  }

  [<Fact>]
  member this.``horizontal group buttons all have the same height``() = task {
    do! this.LoadFixture()
    let! btn1 = this.Page.Locator("#btn-h-1").BoundingBoxAsync()
    let! btn2 = this.Page.Locator("#btn-h-2").BoundingBoxAsync()
    let! btn3 = this.Page.Locator("#btn-h-3").BoundingBoxAsync()

    Assert.True(
      abs (btn1.Height - btn2.Height) <= 1.0f,
      $"Buttons 1 and 2 should have the same height ({btn1.Height}px vs {btn2.Height}px)"
    )

    Assert.True(
      abs (btn1.Height - btn3.Height) <= 1.0f,
      $"Buttons 1 and 3 should have the same height ({btn1.Height}px vs {btn3.Height}px)"
    )
  }

  [<Fact>]
  member this.``vertical group buttons are arranged top to bottom``() = task {
    do! this.LoadFixture()
    let! btn1 = this.Page.Locator("#btn-v-1").BoundingBoxAsync()
    let! btn2 = this.Page.Locator("#btn-v-2").BoundingBoxAsync()
    let! btn3 = this.Page.Locator("#btn-v-3").BoundingBoxAsync()

    Assert.True(btn1.Y < btn2.Y, $"Button 1 (y={btn1.Y}) should be above button 2 (y={btn2.Y})")
    Assert.True(btn2.Y < btn3.Y, $"Button 2 (y={btn2.Y}) should be above button 3 (y={btn3.Y})")
  }

  [<Fact>]
  member this.``vertical group buttons share the same left edge``() = task {
    do! this.LoadFixture()
    let! btn1 = this.Page.Locator("#btn-v-1").BoundingBoxAsync()
    let! btn2 = this.Page.Locator("#btn-v-2").BoundingBoxAsync()
    let! btn3 = this.Page.Locator("#btn-v-3").BoundingBoxAsync()

    Assert.True(
      abs (btn1.X - btn2.X) <= 1.0f,
      $"Buttons 1 and 2 should share the same left edge (x1={btn1.X}, x2={btn2.X})"
    )

    Assert.True(
      abs (btn1.X - btn3.X) <= 1.0f,
      $"Buttons 1 and 3 should share the same left edge (x1={btn1.X}, x3={btn3.X})"
    )
  }

  [<Fact>]
  member this.``middle button in horizontal group has connected border radii``() = task {
    do! this.LoadFixture()

    let! topRightRadius =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#btn-h-1')).borderTopRightRadius"
      )

    let! middleTopLeft =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#btn-h-2')).borderTopLeftRadius"
      )

    let! middleTopRight =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#btn-h-2')).borderTopRightRadius"
      )

    // First button's right side should be squared off
    Assert.Equal("0px", topRightRadius)

    // Middle button should be squared off on both sides
    Assert.Equal("0px", middleTopLeft)
    Assert.Equal("0px", middleTopRight)
  }
