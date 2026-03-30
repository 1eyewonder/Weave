namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type ListTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("list")
