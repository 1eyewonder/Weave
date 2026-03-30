module Weave.Tests.Rendering.SizingConsistencyTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

/// Validates that components sharing the same density-spacing formula
/// produce identical rendered sizes at each density level.
/// Prevents regressions where one component's sizing drifts from its family.
type SizingConsistencyTests() =
  inherit LayoutTestBase("sizing-consistency")

  [<Theory>]
  [<InlineData("compact")>]
  [<InlineData("standard")>]
  [<InlineData("spacious")>]
  member this.``menu items - list, select, dropdown, tab items have matching height``(density: string) = task {
    do! this.LoadFixture()
    let! listItem = this.Page.Locator($"#f1-{density}-list-item").BoundingBoxAsync()
    and! selectItem = this.Page.Locator($"#f1-{density}-select-item").BoundingBoxAsync()
    and! dropdownItem = this.Page.Locator($"#f1-{density}-dropdown-item").BoundingBoxAsync()
    and! tab = this.Page.Locator($"#f1-{density}-tab").BoundingBoxAsync()

    assertHeightsMatch $"menu items ({density})" [
      ("list-item", listItem)
      ("select-item", selectItem)
      ("dropdown-item", dropdownItem)
      ("tab", tab)
    ]
  }

  [<Theory>]
  [<InlineData("compact", 38.0f)>]
  [<InlineData("standard", 48.0f)>]
  [<InlineData("spacious", 58.0f)>]
  member this.``menu items - items meet min-height for density``(density: string, minHeight: float32) = task {
    do! this.LoadFixture()
    let! listItem = this.Page.Locator($"#f1-{density}-list-item").BoundingBoxAsync()
    and! selectItem = this.Page.Locator($"#f1-{density}-select-item").BoundingBoxAsync()
    and! dropdownItem = this.Page.Locator($"#f1-{density}-dropdown-item").BoundingBoxAsync()

    for (name, box) in
      [
        ("list-item", listItem)
        ("select-item", selectItem)
        ("dropdown-item", dropdownItem)
      ] do
      Assert.True(
        box.Height >= minHeight,
        $"{density} {name} height ({box.Height}px) should be >= {minHeight}px"
      )
  }

  [<Fact>]
  member this.``menu items - density ordering compact < standard < spacious``() = task {
    do! this.LoadFixture()
    let! compact = this.Page.Locator("#f1-compact-list-item").BoundingBoxAsync()
    and! standard = this.Page.Locator("#f1-standard-list-item").BoundingBoxAsync()
    and! spacious = this.Page.Locator("#f1-spacious-list-item").BoundingBoxAsync()

    Assert.True(
      compact.Height < standard.Height,
      $"Compact list item ({compact.Height}px) should be shorter than standard ({standard.Height}px)"
    )

    Assert.True(
      standard.Height < spacious.Height,
      $"Standard list item ({standard.Height}px) should be shorter than spacious ({spacious.Height}px)"
    )
  }

  [<Theory>]
  [<InlineData("compact")>]
  [<InlineData("standard")>]
  [<InlineData("spacious")>]
  member this.``form controls - checkbox, radio, switch have matching padding``(density: string) = task {
    do! this.LoadFixture()
    let! checkboxPadding = this.ComputedStyle($"#f2-{density}-checkbox", "padding")
    and! radioPadding = this.ComputedStyle($"#f2-{density}-radio", "padding")
    and! switchPadding = this.ComputedStyle($"#f2-{density}-switch", "padding")

    assertComputedValuesMatch $"form controls ({density})" "padding" [
      ("checkbox", checkboxPadding)
      ("radio", radioPadding)
      ("switch", switchPadding)
    ]
  }

  [<Theory>]
  [<InlineData("compact", "4px")>]
  [<InlineData("standard", "8px")>]
  [<InlineData("spacious", "12px")>]
  member this.``form controls - form control padding matches density``(density: string, expected: string) = task {
    do! this.LoadFixture()
    let! padding = this.ComputedStyle($"#f2-{density}-checkbox", "padding")
    Assert.Equal(expected, padding)
  }

  [<Theory>]
  [<InlineData("compact")>]
  [<InlineData("standard")>]
  [<InlineData("spacious")>]
  member this.``alert & list items - alert and list item have matching paddingTop``(density: string) = task {
    do! this.LoadFixture()
    let! alertPt = this.ComputedStyle($"#f4-{density}-alert", "paddingTop")
    and! listPt = this.ComputedStyle($"#f4-{density}-list-item", "paddingTop")

    assertComputedValuesMatch $"alert/list padding-top ({density})" "paddingTop" [
      ("alert", alertPt)
      ("list-item", listPt)
    ]
  }

  [<Theory>]
  [<InlineData("compact")>]
  [<InlineData("standard")>]
  [<InlineData("spacious")>]
  member this.``alert & list items - alert and list item have matching paddingLeft``(density: string) = task {
    do! this.LoadFixture()
    let! alertPl = this.ComputedStyle($"#f4-{density}-alert", "paddingLeft")
    and! listPl = this.ComputedStyle($"#f4-{density}-list-item", "paddingLeft")

    assertComputedValuesMatch $"alert/list padding-left ({density})" "paddingLeft" [
      ("alert", alertPl)
      ("list-item", listPl)
    ]
  }

  [<Theory>]
  [<InlineData("standard")>]
  [<InlineData("filled")>]
  [<InlineData("outlined")>]
  member this.``field controls - select control matches field control height``(variant: string) = task {
    do! this.LoadFixture()
    let! fieldControl = this.Page.Locator($"#f5-{variant}-field .weave-field__control").BoundingBoxAsync()
    and! selectControl = this.Page.Locator($"#f5-{variant}-select .weave-field__control").BoundingBoxAsync()

    assertHeightsMatch $"field controls ({variant})" [ ("field", fieldControl); ("select", selectControl) ]
  }

  [<Theory>]
  [<InlineData("standard")>]
  [<InlineData("filled")>]
  [<InlineData("outlined")>]
  member this.``field controls - empty select control matches field control height``(variant: string) = task {
    do! this.LoadFixture()
    let! fieldControl = this.Page.Locator($"#f5-{variant}-field .weave-field__control").BoundingBoxAsync()

    and! emptySelectControl =
      this.Page.Locator($"#f5-{variant}-select-empty .weave-field__control").BoundingBoxAsync()

    assertHeightsMatch $"field controls, empty ({variant})" [
      ("field", fieldControl)
      ("empty-select", emptySelectControl)
    ]
  }
