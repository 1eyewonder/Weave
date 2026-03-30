namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type LinkTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("link")

  [<Fact>]
  member this.``link is focusable``() = task {
    do! this.NavigateTo("link")
    let link = this.Page.Locator(".weave-link").First
    do! link.FocusAsync()
    do! this.Expect(link).ToBeFocusedAsync()
  }
