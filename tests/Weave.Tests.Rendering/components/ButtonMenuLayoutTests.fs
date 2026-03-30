module Weave.Tests.Rendering.ButtonMenuLayoutTests

open Xunit

type ButtonMenuLayoutTests() =
  inherit LayoutTestBase("buttonmenu")

  [<Fact>]
  member this.``open menu root has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#menu-open").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Open menu width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Open menu height {box.Height}px should be > 0")
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
    and! itemsBox = this.Page.Locator("#menu-open .weave-button-menu__items").BoundingBoxAsync()

    Assert.True(
      itemsBox.Y >= triggerBox.Y + triggerBox.Height - 1.0f,
      $"Items container top ({itemsBox.Y}px) should be at or below trigger bottom ({triggerBox.Y + triggerBox.Height}px)"
    )
  }

  [<Fact>]
  member this.``closed menu items have zero opacity``() = task {
    do! this.LoadFixture()

    let! opacity = this.ComputedStyle("#menu-closed .weave-button-menu__item", "opacity")

    Assert.Equal("0", opacity)
  }
