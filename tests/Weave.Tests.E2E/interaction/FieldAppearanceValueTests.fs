namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type FieldAppearanceValueTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``field source captures escaped current value``(width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigatePage(this.Page, "field-appearance")

    let input =
      this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = "Name", Exact = true))

    do! this.Expect(input).ToBeVisibleAsync()
    do! input.FillAsync("Ada \"Q\" \\ café 👍")

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    let source =
      this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

    do! this.Expect(source).ToContainTextAsync("Ada \\\"Q\\\" \\\\ café")
    do! this.Expect(source).ToContainTextAsync("\\uD83D\\uDC4D")
    do! input.FillAsync(System.String('a', 100))
    do! this.Expect(source).ToContainTextAsync("let value =\n  Var.Create (")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(source).ToContainTextAsync("let value = Var.Create \"\"")
  }
