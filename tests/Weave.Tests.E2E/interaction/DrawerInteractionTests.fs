namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type DrawerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``drawer has aria-label``() = task {
    do! this.NavigateTo("drawer")
    let drawer = this.Page.Locator(".weave-drawer").First
    do! this.Expect(drawer).ToHaveAttributeAsync("aria-label", "Navigation drawer")
  }

  [<Fact>]
  member this.``drawer content is visible when open``() = task {
    do! this.NavigateTo("drawer")
    let drawer = this.Page.Locator(".weave-drawer").First
    do! this.Expect(drawer).ToBeVisibleAsync()
  }
