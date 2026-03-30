module Weave.Tests.Rendering.DropdownLayoutTests

open Xunit

type DropdownLayoutTests() =
  inherit LayoutTestBase("dropdown")

  [<Fact>]
  member this.``dropdown list has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#dropdown-list").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Dropdown list width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Dropdown list height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``items stack vertically``() = task {
    do! this.LoadFixture()
    let! item1 = this.Page.Locator("#dropdown-item-1").BoundingBoxAsync()
    and! item2 = this.Page.Locator("#dropdown-item-2").BoundingBoxAsync()
    and! item3 = this.Page.Locator("#dropdown-item-3").BoundingBoxAsync()

    Assert.True(item1.Y < item2.Y, $"Item 1 (y={item1.Y}) should be above item 2 (y={item2.Y})")

    Assert.True(item2.Y < item3.Y, $"Item 2 (y={item2.Y}) should be above item 3 (y={item3.Y})")
  }

  [<Fact>]
  member this.``each item has positive height``() = task {
    do! this.LoadFixture()
    let! item1 = this.Page.Locator("#dropdown-item-1").BoundingBoxAsync()
    and! item2 = this.Page.Locator("#dropdown-item-2").BoundingBoxAsync()
    and! item3 = this.Page.Locator("#dropdown-item-3").BoundingBoxAsync()

    Assert.True(item1.Height > 0.0f, $"Item 1 height {item1.Height}px should be > 0")
    Assert.True(item2.Height > 0.0f, $"Item 2 height {item2.Height}px should be > 0")
    Assert.True(item3.Height > 0.0f, $"Item 3 height {item3.Height}px should be > 0")
  }

  [<Fact>]
  member this.``dropdown list is positioned below trigger``() = task {
    do! this.LoadFixture()
    let! trigger = this.Page.Locator("#dropdown-open > button").BoundingBoxAsync()
    and! list = this.Page.Locator("#dropdown-list").BoundingBoxAsync()

    let triggerBottom = trigger.Y + trigger.Height

    Assert.True(
      list.Y >= triggerBottom - 1.0f,
      $"Dropdown list top ({list.Y}px) should be at or below trigger bottom ({triggerBottom}px)"
    )
  }

  [<Fact>]
  member this.``divider has minimal height``() = task {
    do! this.LoadFixture()
    let! divider = this.Page.Locator("#dropdown-divider-list .weave-dropdown__divider").BoundingBoxAsync()

    Assert.True(divider.Height <= 2.0f, $"Dropdown divider height {divider.Height}px should be <= 2px")
  }

  [<Fact>]
  member this.``anchor-origin-top-right positions list at trigger top``() = task {
    do! this.LoadFixture()
    let! trigger = this.Page.Locator("#dropdown-anchor-top-right > button").BoundingBoxAsync()
    and! list = this.Page.Locator("#dropdown-list-top-right").BoundingBoxAsync()

    // With anchor-origin-top-right + transform-origin-top-right, list aligns to trigger's top-right
    Assert.True(
      abs (list.Y - trigger.Y) <= 2.0f,
      $"Top-right anchored list top ({list.Y}px) should align to trigger top ({trigger.Y}px)"
    )
  }

  [<Fact>]
  member this.``disabled dropdown item has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents = this.ComputedStyle(".weave-dropdown__item--disabled", "pointerEvents")

    Assert.Equal("none", pointerEvents)
  }
