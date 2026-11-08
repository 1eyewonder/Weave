namespace Weave.Tests.E2E

open Xunit

type ButtonGroupTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("buttongroup")
