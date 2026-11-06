namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ThemingInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``custom primary color reaches components``() = task {
    do! this.NavigateTo("theming")

    do!
      this
        .Expect(this.Page.GetByTestId("themed-button"))
        .ToHaveCSSAsync("background-color", "rgb(124, 58, 237)")
  }
