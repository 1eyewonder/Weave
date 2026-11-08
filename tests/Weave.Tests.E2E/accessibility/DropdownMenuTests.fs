namespace Weave.Tests.E2E

open Xunit

type DropdownMenuTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("dropdown-menu")
