namespace Weave.Tests.E2E

open Xunit

type SnackbarTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``palette variants and actions pass accessibility and contrast``(dark: bool) =
    this.RunAxeScan("snackbar-palette", contrast = true, dark = dark)
