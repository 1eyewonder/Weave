module Weave.Tests.Rendering.SelectLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type SelectLayoutTests() =
  inherit LayoutTestBase("select")

  [<Fact>]
  member this.``select root has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#select-closed").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Select width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Select height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``closed popover is hidden``() = task {
    do! this.LoadFixture()

    let! opacity = this.ComputedStyle("#select-closed .weave-select__popover", "opacity")
    and! pointerEvents = this.ComputedStyle("#select-closed .weave-select__popover", "pointerEvents")

    Assert.Equal("0", opacity)
    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``open popover is visible and interactive``() = task {
    do! this.LoadFixture()

    let! opacity = this.ComputedStyle("#select-open .weave-select__popover", "opacity")
    and! pointerEvents = this.ComputedStyle("#select-open .weave-select__popover", "pointerEvents")

    Assert.Equal("1", opacity)
    Assert.Equal("auto", pointerEvents)
  }

  [<Fact>]
  member this.``popover is positioned below the select``() = task {
    do! this.LoadFixture()
    let! root = this.Page.Locator("#select-open").BoundingBoxAsync()
    and! popover = this.Page.Locator("#select-open .weave-select__popover").BoundingBoxAsync()

    let rootBottom = root.Y + root.Height

    Assert.True(
      popover.Y >= rootBottom - 1.0f,
      $"Popover top ({popover.Y}px) should be at or below select bottom ({rootBottom}px)"
    )
  }

  [<Fact>]
  member this.``items stack vertically``() = task {
    do! this.LoadFixture()
    let! item1 = this.Page.Locator("#select-item-1").BoundingBoxAsync()
    and! item2 = this.Page.Locator("#select-item-2").BoundingBoxAsync()
    and! item3 = this.Page.Locator("#select-item-3").BoundingBoxAsync()

    Assert.True(item1.Y < item2.Y, $"Item 1 (y={item1.Y}) should be above item 2 (y={item2.Y})")
    Assert.True(item2.Y < item3.Y, $"Item 2 (y={item2.Y}) should be above item 3 (y={item3.Y})")
  }

  [<Fact>]
  member this.``disabled select has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents = this.ComputedStyle("#select-disabled", "pointerEvents")

    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``disabled item has pointer-events none``() = task {
    do! this.LoadFixture()

    let! pointerEvents = this.ComputedStyle("#select-open .weave-select__item--disabled", "pointerEvents")

    Assert.Equal("none", pointerEvents)
  }

  [<Fact>]
  member this.``full-width select fills its container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#select-full-width-container").BoundingBoxAsync()
    and! select = this.Page.Locator("#select-full-width").BoundingBoxAsync()

    Assert.True(
      abs (select.Width - container.Width) <= 1.0f,
      $"Full-width select ({select.Width}px) should fill its container ({container.Width}px)"
    )
  }

  [<Fact>]
  member this.``fit-content sizer is in flow``() = task {
    do! this.LoadFixture()

    let! position = this.ComputedStyle("#select-fit-content .weave-select__sizer", "position")

    Assert.Equal("relative", position)
  }

  [<Fact>]
  member this.``default sizer is out of flow and hidden``() = task {
    do! this.LoadFixture()

    let! position = this.ComputedStyle("#select-closed .weave-select__sizer", "position")
    and! visibility = this.ComputedStyle("#select-closed .weave-select__sizer", "visibility")

    Assert.Equal("absolute", position)
    Assert.Equal("hidden", visibility)
  }

  [<Fact>]
  member this.``clear button is hidden by default``() = task {
    do! this.LoadFixture()

    let! display = this.ComputedStyle("#select-closed .weave-select__clear", "display")

    Assert.Equal("none", display)
  }

  [<Fact>]
  member this.``clear button is visible when clearable with value``() = task {
    do! this.LoadFixture()

    let! display = this.ComputedStyle("#select-clearable-value .weave-select__clear", "display")

    Assert.NotEqual<string>("none", display)
  }

  [<Fact>]
  member this.``readonly select hides the popover``() = task {
    do! this.LoadFixture()

    let! display = this.ComputedStyle("#select-readonly .weave-select__popover", "display")

    Assert.Equal("none", display)
  }

  [<Fact>]
  member this.``search input sits above the list``() = task {
    do! this.LoadFixture()
    let! search = this.Page.Locator("#select-searchable .weave-select__search").BoundingBoxAsync()
    and! list = this.Page.Locator("#select-searchable .weave-select__list").BoundingBoxAsync()

    let searchBottom = search.Y + search.Height

    Assert.True(
      searchBottom <= list.Y + 1.0f,
      $"Search bottom ({searchBottom}px) should be at or above list top ({list.Y}px)"
    )
  }

  [<Fact>]
  member this.``select-all and divider are above the list``() = task {
    do! this.LoadFixture()
    let! selectAll = this.Page.Locator("#select-multi-open .weave-select__select-all").BoundingBoxAsync()
    and! divider = this.Page.Locator("#select-multi-open .weave-select__divider").BoundingBoxAsync()
    and! list = this.Page.Locator("#select-multi-open .weave-select__list").BoundingBoxAsync()

    Assert.True(
      selectAll.Y < divider.Y,
      $"Select-all (y={selectAll.Y}) should be above divider (y={divider.Y})"
    )

    Assert.True(divider.Y < list.Y, $"Divider (y={divider.Y}) should be above list (y={list.Y})")
  }

  [<Fact>]
  member this.``divider has minimal height``() = task {
    do! this.LoadFixture()
    let! divider = this.Page.Locator("#select-multi-open .weave-select__divider").BoundingBoxAsync()

    Assert.True(divider.Height <= 2.0f, $"Divider height {divider.Height}px should be <= 2px")
  }

  [<Fact>]
  member this.``no-items message is centered``() = task {
    do! this.LoadFixture()

    let! justifyContent = this.ComputedStyle("#select-no-items .weave-select__no-items", "justifyContent")

    and! alignItems = this.ComputedStyle("#select-no-items .weave-select__no-items", "alignItems")

    Assert.Equal("center", justifyContent)
    Assert.Equal("center", alignItems)
  }

  [<Fact>]
  member this.``multi-select checkbox has dimensions``() = task {
    do! this.LoadFixture()

    let! checkbox =
      this.Page.Locator("#select-multi-open .weave-select__item-checkbox").First.BoundingBoxAsync()

    Assert.True(checkbox.Width > 0.0f, $"Checkbox width {checkbox.Width}px should be > 0")
    Assert.True(checkbox.Height > 0.0f, $"Checkbox height {checkbox.Height}px should be > 0")

    Assert.True(
      abs (checkbox.Width - checkbox.Height) <= 2.0f,
      $"Checkbox should be roughly square (width={checkbox.Width}, height={checkbox.Height})"
    )
  }

  [<Fact>]
  member this.``chevron is contained within select root``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#select-closed").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#select-closed .weave-select__chevron").BoundingBoxAsync()
    assertContainedWithin "select root" "chevron" parentBox childBox
  }

  [<Fact>]
  member this.``clear button is contained within select root``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#select-clearable-value").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#select-clearable-value .weave-select__clear").BoundingBoxAsync()
    assertContainedWithin "select root" "clear button" parentBox childBox
  }

  [<Fact>]
  member this.``items are contained within popover list``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#select-open .weave-select__list").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#select-item-1").BoundingBoxAsync()
    assertContainedWithin "popover list" "item" parentBox childBox
  }

  [<Fact>]
  member this.``items fill popover list width``() = task {
    do! this.LoadFixture()
    let! parentBox = this.Page.Locator("#select-open .weave-select__list").BoundingBoxAsync()
    and! childBox = this.Page.Locator("#select-item-1").BoundingBoxAsync()
    assertFillsWidth "popover list" "item" parentBox childBox
  }
