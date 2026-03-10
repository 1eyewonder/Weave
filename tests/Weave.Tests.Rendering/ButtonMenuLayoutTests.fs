module Weave.Tests.Rendering.ButtonMenuLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type ButtonMenuLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "buttonmenu.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``open menu root has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#menu-open").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Open menu width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Open menu height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``trigger button is visible``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#menu-trigger").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Trigger button width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Trigger button height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``open menu items are visible``() = task {
    do! this.LoadFixture()
    let items = this.Page.Locator("#menu-open .weave-button-menu__item")
    let! count = items.CountAsync()

    Assert.Equal(3, count)

    for i in 0 .. count - 1 do
      let! box = items.Nth(i).BoundingBoxAsync()
      Assert.True(box.Width > 0.0f, $"Menu item {i} width {box.Width}px should be > 0")
      Assert.True(box.Height > 0.0f, $"Menu item {i} height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``items container is below trigger when direction is bottom``() = task {
    do! this.LoadFixture()
    let! triggerBox = this.Page.Locator("#menu-trigger").BoundingBoxAsync()
    let! itemsBox = this.Page.Locator("#menu-open .weave-button-menu__items").BoundingBoxAsync()

    Assert.True(
      itemsBox.Y >= triggerBox.Y + triggerBox.Height - 1.0f,
      $"Items container top ({itemsBox.Y}px) should be at or below trigger bottom ({triggerBox.Y + triggerBox.Height}px)"
    )
  }

  [<Fact>]
  member this.``closed menu items have zero opacity``() = task {
    do! this.LoadFixture()

    let! opacity =
      this.Page.EvaluateAsync<string>(
        "getComputedStyle(document.querySelector('#menu-closed .weave-button-menu__item')).opacity"
      )

    Assert.Equal("0", opacity)
  }
