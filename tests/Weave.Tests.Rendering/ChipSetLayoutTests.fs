module Weave.Tests.Rendering.ChipSetLayoutTests

open Xunit

type ChipSetLayoutTests() =
  inherit LayoutTestBase("chipset")

  [<Fact>]
  member this.``chipset container has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#chipset-basic").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"ChipSet width {box.Width}px should be positive")
    Assert.True(box.Height > 0.0f, $"ChipSet height {box.Height}px should be positive")
  }

  [<Fact>]
  member this.``chipset uses flex display``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page.Locator("#chipset-basic").EvaluateAsync<string>("el => getComputedStyle(el).display")

    Assert.Equal("flex", display)
  }

  [<Fact>]
  member this.``chipset has flex-wrap wrap``() = task {
    do! this.LoadFixture()

    let! flexWrap =
      this.Page.Locator("#chipset-basic").EvaluateAsync<string>("el => getComputedStyle(el).flexWrap")

    Assert.Equal("wrap", flexWrap)
  }

  [<Fact>]
  member this.``chipset has align-items center``() = task {
    do! this.LoadFixture()

    let! alignItems =
      this.Page.Locator("#chipset-basic").EvaluateAsync<string>("el => getComputedStyle(el).alignItems")

    Assert.Equal("center", alignItems)
  }

  [<Fact>]
  member this.``chips are arranged horizontally``() = task {
    do! this.LoadFixture()
    let! chip0 = this.Page.Locator("#chipset-chip-0").BoundingBoxAsync()
    and! chip1 = this.Page.Locator("#chipset-chip-1").BoundingBoxAsync()
    and! chip2 = this.Page.Locator("#chipset-chip-2").BoundingBoxAsync()

    // Same Y position (within tolerance)
    Assert.True(
      abs (chip0.Y - chip1.Y) <= 1.0f,
      $"Chip 0 (y={chip0.Y}) and Chip 1 (y={chip1.Y}) should be at the same vertical position"
    )

    Assert.True(
      abs (chip1.Y - chip2.Y) <= 1.0f,
      $"Chip 1 (y={chip1.Y}) and Chip 2 (y={chip2.Y}) should be at the same vertical position"
    )

    // Increasing X position
    Assert.True(chip1.X > chip0.X, $"Chip 1 (x={chip1.X}) should be to the right of Chip 0 (x={chip0.X})")

    Assert.True(chip2.X > chip1.X, $"Chip 2 (x={chip2.X}) should be to the right of Chip 1 (x={chip1.X})")
  }

  [<Fact>]
  member this.``chips have approximately 8px gap between them``() = task {
    do! this.LoadFixture()
    let! chip0 = this.Page.Locator("#chipset-chip-0").BoundingBoxAsync()
    and! chip1 = this.Page.Locator("#chipset-chip-1").BoundingBoxAsync()

    let gap = chip1.X - (chip0.X + chip0.Width)

    Assert.True(abs (gap - 8.0f) <= 1.0f, $"Gap between chips ({gap}px) should be approximately 8px")
  }

  [<Fact>]
  member this.``mixed chipset selected chip has different background than unselected``() = task {
    do! this.LoadFixture()

    let! selectedBg =
      this.Page
        .Locator("#chipset-mixed-selected")
        .EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    and! unselectedBg =
      this.Page
        .Locator("#chipset-mixed-unselected")
        .EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.NotEqual<string>(selectedBg, unselectedBg)
  }
