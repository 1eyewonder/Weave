module Weave.Tests.Rendering.ElevationStackingTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

/// Validates that the z-index hierarchy across overlapping components is correct.
/// Prevents regressions like dropdowns bleeding through dialogs.
type ElevationStackingTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "elevation-stacking.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  member private this.ZIndexOf(selector: string) = task {
    let! value = this.Page.Locator(selector).EvaluateAsync<string>("el => window.getComputedStyle(el).zIndex")

    return
      match System.Int32.TryParse(value) with
      | true, n -> n
      | _ -> 0
  }

  [<Fact>]
  member this.``dialog backdrop z-index is greater than dropdown``() = task {
    do! this.LoadFixture()
    let! dropdownZ = this.ZIndexOf("#stacking-dropdown")
    let! dialogBackdropZ = this.ZIndexOf("#stacking-dialog-backdrop")

    Assert.True(
      dialogBackdropZ > dropdownZ,
      $"Dialog backdrop z-index ({dialogBackdropZ}) should be greater than dropdown ({dropdownZ})"
    )
  }

  [<Fact>]
  member this.``dialog window z-index is greater than dropdown``() = task {
    do! this.LoadFixture()
    let! dropdownZ = this.ZIndexOf("#stacking-dropdown")
    let! dialogWindowZ = this.ZIndexOf("#stacking-dialog-window")

    Assert.True(
      dialogWindowZ > dropdownZ,
      $"Dialog window z-index ({dialogWindowZ}) should be greater than dropdown ({dropdownZ})"
    )
  }

  [<Fact>]
  member this.``dialog z-index is greater than button menu``() = task {
    do! this.LoadFixture()
    let! buttonMenuZ = this.ZIndexOf("#stacking-buttonmenu-items")
    let! dialogZ = this.ZIndexOf("#stacking-dialog-backdrop")

    Assert.True(
      dialogZ > buttonMenuZ,
      $"Dialog z-index ({dialogZ}) should be greater than button menu ({buttonMenuZ})"
    )
  }

  [<Fact>]
  member this.``tooltip z-index is greater than dialog``() = task {
    do! this.LoadFixture()
    let! tooltipZ = this.ZIndexOf("#stacking-tooltip")
    let! dialogWindowZ = this.ZIndexOf("#stacking-dialog-window")

    Assert.True(
      tooltipZ > dialogWindowZ,
      $"Tooltip z-index ({tooltipZ}) should be greater than dialog window ({dialogWindowZ})"
    )
  }

  [<Fact>]
  member this.``appbar z-index is greater than dialog``() = task {
    do! this.LoadFixture()
    let! appbarZ = this.ZIndexOf("#stacking-appbar")
    let! dialogWindowZ = this.ZIndexOf("#stacking-dialog-window")

    Assert.True(
      appbarZ > dialogWindowZ,
      $"AppBar z-index ({appbarZ}) should be greater than dialog window ({dialogWindowZ})"
    )
  }

  [<Fact>]
  member this.``scroll-to-top z-index is greater than dialog``() = task {
    do! this.LoadFixture()
    let! scrollZ = this.ZIndexOf("#stacking-scroll-to-top")
    let! dialogWindowZ = this.ZIndexOf("#stacking-dialog-window")

    Assert.True(
      scrollZ > dialogWindowZ,
      $"Scroll-to-top z-index ({scrollZ}) should be greater than dialog window ({dialogWindowZ})"
    )
  }

  // Full hierarchy ordering: dropdown < dialog < scroll-to-top < appbar < tooltip
  [<Fact>]
  member this.``full z-index hierarchy is correctly ordered``() = task {
    do! this.LoadFixture()
    let! dropdownZ = this.ZIndexOf("#stacking-dropdown")
    let! dialogBackdropZ = this.ZIndexOf("#stacking-dialog-backdrop")
    let! dialogWindowZ = this.ZIndexOf("#stacking-dialog-window")
    let! scrollZ = this.ZIndexOf("#stacking-scroll-to-top")
    let! appbarZ = this.ZIndexOf("#stacking-appbar")
    let! tooltipZ = this.ZIndexOf("#stacking-tooltip")

    let hierarchy = [
      ("dropdown", dropdownZ)
      ("dialog-backdrop", dialogBackdropZ)
      ("dialog-window", dialogWindowZ)
      ("scroll-to-top", scrollZ)
      ("appbar", appbarZ)
      ("tooltip", tooltipZ)
    ]

    hierarchy
    |> List.pairwise
    |> List.iter (fun ((nameA, zA), (nameB, zB)) ->
      Assert.True(zA < zB, $"{nameA} z-index ({zA}) should be less than {nameB} z-index ({zB})"))
  }
