module Weave.Tests.Rendering.ElevationStackingTests

open Xunit

/// Validates that the z-index hierarchy across overlapping components is correct.
/// Prevents regressions like dropdowns bleeding through dialogs.
type ElevationStackingTests() =
  inherit LayoutTestBase("elevation-stacking")

  member private this.ZIndexOf(selector: string) = task {
    let! value = this.Page.Locator(selector).EvaluateAsync<string>("el => window.getComputedStyle(el).zIndex")

    return
      match System.Int32.TryParse(value) with
      | true, n -> n
      | _ -> 0
  }

  // Full hierarchy ordering: dropdown < dialog < scroll-to-top < appbar < tooltip
  [<Fact>]
  member this.``full z-index hierarchy is correctly ordered``() = task {
    do! this.LoadFixture()
    let! dropdownZ = this.ZIndexOf("#stacking-dropdown")
    and! dialogBackdropZ = this.ZIndexOf("#stacking-dialog-backdrop")
    and! dialogWindowZ = this.ZIndexOf("#stacking-dialog-window")
    and! scrollZ = this.ZIndexOf("#stacking-scroll-to-top")
    and! appbarZ = this.ZIndexOf("#stacking-appbar")
    and! tooltipZ = this.ZIndexOf("#stacking-tooltip")

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
