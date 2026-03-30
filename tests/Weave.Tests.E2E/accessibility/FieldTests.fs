namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type FieldTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("field")

  [<Fact>]
  member this.``invalid field has aria-invalid attribute``() = task {
    do! this.NavigateTo("field")
    let input = this.Page.Locator("input[aria-invalid='true']").First
    do! this.Expect(input).ToBeVisibleAsync()
    do! this.Expect(input).ToHaveAttributeAsync("aria-describedby", "email-error")
  }

  [<Fact>]
  member this.``aria-describedby points to visible help text``() = task {
    do! this.NavigateTo("field")
    let helpText = this.Page.Locator("#email-error")
    do! this.Expect(helpText).ToBeVisibleAsync()
    do! this.Expect(helpText).ToHaveTextAsync("Invalid email address")
  }
