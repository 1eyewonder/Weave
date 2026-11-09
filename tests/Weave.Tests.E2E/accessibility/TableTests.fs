namespace Weave.Tests.E2E

open Xunit

type TableTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("table")

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``default table colors pass contrast checks in both themes``(dark: bool) =
    this.RunAxeScan("table-default", contrast = true, dark = dark)
