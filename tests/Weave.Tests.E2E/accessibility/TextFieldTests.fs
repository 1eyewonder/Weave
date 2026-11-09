namespace Weave.Tests.E2E

open Xunit

type TextFieldTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("textfield")

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``default colors pass contrast checks in both themes``(dark: bool) =
    this.RunAxeScan("textfield", contrast = true, dark = dark)
