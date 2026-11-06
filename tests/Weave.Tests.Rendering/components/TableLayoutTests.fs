module Weave.Tests.Rendering.TableLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type TableLayoutTests() =
  inherit LayoutTestBase("table")

  member private this.RowHeight(id: string) = task {
    let! box =
      this.Page.Locator($"#{id} tbody .weave-table__row").First.BoundingBoxAsync()

    return box.Height
  }

  [<Fact>]
  member this.``compact rows are shorter than standard rows``() = task {
    do! this.LoadFixture()
    let! compact = this.RowHeight("table-compact")
    let! standard = this.RowHeight("table-standard")

    Assert.True(
      compact < standard,
      $"Compact row ({compact}px) should be shorter than standard ({standard}px)"
    )
  }

  [<Fact>]
  member this.``spacious rows are taller than standard rows``() = task {
    do! this.LoadFixture()
    let! standard = this.RowHeight("table-standard")
    let! spacious = this.RowHeight("table-spacious")

    Assert.True(
      spacious > standard,
      $"Spacious row ({spacious}px) should be taller than standard ({standard}px)"
    )
  }

  [<Fact>]
  member this.``table fills its frame width``() = task {
    do! this.LoadFixture()
    let! frame = this.Page.Locator("#table-standard").BoundingBoxAsync()

    and! table =
      this.Page.Locator("#table-standard .weave-table__table").BoundingBoxAsync()

    Assert.True(
      table.Width >= frame.Width - 2.0f - tolerance,
      $"Table ({table.Width}px) should fill the frame ({frame.Width}px) inside its border"
    )
  }

  [<Fact>]
  member this.``narrow frame scrolls instead of growing``() = task {
    do! this.LoadFixture()
    let! frame = this.Page.Locator("#table-narrow").BoundingBoxAsync()

    let! scrollWidth =
      this.Page.Locator("#table-narrow").EvaluateAsync<int>("el => el.scrollWidth")

    Assert.True(
      float32 scrollWidth > frame.Width,
      $"Content ({scrollWidth}px) should be wider than the frame ({frame.Width}px)"
    )

    Assert.True(frame.Width <= 160.0f + tolerance, $"Frame ({frame.Width}px) should keep its 160px width")
  }

  [<Fact>]
  member this.``sort indicator sits inside the sort button after the label``() = task {
    do! this.LoadFixture()

    let! button =
      this.Page.Locator("#table-sortable .weave-table__sort-button").BoundingBoxAsync()

    and! indicator =
      this.Page.Locator("#table-sortable .weave-table__sort-indicator").BoundingBoxAsync()

    assertContainedWithin "sort button" "sort indicator" button indicator

    Assert.True(
      indicator.X > button.X,
      $"Indicator (x={indicator.X}) should be right of the label start (x={button.X})"
    )
  }

  [<Fact>]
  member this.``row action button stays inside its cell``() = task {
    do! this.LoadFixture()
    let! cell = this.Page.Locator("#table-sortable td").First.BoundingBoxAsync()

    and! action =
      this.Page.Locator("#table-sortable .weave-table__row-action").BoundingBoxAsync()

    assertContainedWithin "cell" "row action" cell action
  }

  [<Fact>]
  member this.``select column only takes the checkbox width``() = task {
    do! this.LoadFixture()

    let! cell =
      this.Page.Locator("#table-selectable tbody .weave-table__select-cell").BoundingBoxAsync()

    and! box = this.Page.Locator("#select-box").BoundingBoxAsync()
    and! frame = this.Page.Locator("#table-selectable").BoundingBoxAsync()

    assertContainedWithin "select cell" "checkbox" cell box

    Assert.True(
      cell.Width < frame.Width / 4.0f,
      $"Select cell ({cell.Width}px) should stay narrow in a {frame.Width}px table"
    )
  }

  [<Fact>]
  member this.``row header cell keeps body text weight``() = task {
    do! this.LoadFixture()

    let! rowHeader = this.ComputedStyle("#row-alpha", "fontWeight")
    let! cell = this.ComputedStyle("#table-standard tbody td:first-child", "fontWeight")

    Assert.Equal(cell, rowHeader)
  }
