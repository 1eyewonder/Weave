module Weave.Tests.Rendering.ListLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type ListLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "list.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``list items are stacked vertically``() = task {
    do! this.LoadFixture()
    let! item1 = this.Page.Locator("#item-1").BoundingBoxAsync()
    let! item2 = this.Page.Locator("#item-2").BoundingBoxAsync()
    let! item3 = this.Page.Locator("#item-3").BoundingBoxAsync()

    Assert.True(item2.Y > item1.Y, $"Item 2 (y={item2.Y}) should be below item 1 (y={item1.Y})")
    Assert.True(item3.Y > item2.Y, $"Item 3 (y={item3.Y}) should be below item 2 (y={item2.Y})")
  }

  [<Fact>]
  member this.``list items fill the list width``() = task {
    do! this.LoadFixture()
    let! list = this.Page.Locator("#list-normal").BoundingBoxAsync()
    let! item1 = this.Page.Locator("#item-1").BoundingBoxAsync()
    let! item2 = this.Page.Locator("#item-2").BoundingBoxAsync()

    Assert.True(
      abs (item1.Width - list.Width) <= 1.0f,
      $"Item 1 width {item1.Width}px should match list width {list.Width}px"
    )

    Assert.True(
      abs (item2.Width - list.Width) <= 1.0f,
      $"Item 2 width {item2.Width}px should match list width {list.Width}px"
    )
  }

  [<Fact>]
  member this.``normal list items meet minimum height``() = task {
    do! this.LoadFixture()
    let! item = this.Page.Locator("#item-1").BoundingBoxAsync()

    Assert.True(item.Height >= 48.0f, $"Normal list item height {item.Height}px should be >= 48px")
  }

  [<Fact>]
  member this.``dense list items are shorter than normal items``() = task {
    do! this.LoadFixture()
    let! normalItem = this.Page.Locator("#item-1").BoundingBoxAsync()
    let! denseItem = this.Page.Locator("#item-dense").BoundingBoxAsync()

    Assert.True(
      denseItem.Height < normalItem.Height,
      $"Dense item ({denseItem.Height}px) should be shorter than normal item ({normalItem.Height}px)"
    )
  }
