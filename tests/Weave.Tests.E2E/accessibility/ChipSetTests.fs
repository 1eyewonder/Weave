namespace Weave.Tests.E2E

open Xunit

[<Collection("E2E")>]
type ChipSetTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("chipset")
