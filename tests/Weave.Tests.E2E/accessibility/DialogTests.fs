namespace Weave.Tests.E2E

open Xunit

type DialogTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("dialog")

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``default colors pass contrast checks in both themes``(dark: bool) =
    this.RunAxeScan("dialog", contrast = true, dark = dark)
