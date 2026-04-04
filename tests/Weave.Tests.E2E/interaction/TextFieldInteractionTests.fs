namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type TextFieldInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``invalid field has aria-invalid attribute``() = task {
    do! this.NavigateTo("textfield")
    let input = this.Page.Locator("input[aria-invalid='true']").First
    do! this.Expect(input).ToBeVisibleAsync()
    do! this.Expect(input).ToHaveAttributeAsync("aria-describedby", "text-email-error")
  }

  [<Fact>]
  member this.``aria-describedby points to visible help text``() = task {
    do! this.NavigateTo("textfield")
    let helpText = this.Page.Locator("#text-email-error")
    do! this.Expect(helpText).ToBeVisibleAsync()
    do! this.Expect(helpText).ToHaveTextAsync("Invalid email address")
  }

  [<Fact>]
  member this.``multiline field renders a textarea element``() = task {
    do! this.NavigateTo("textfield")
    let textarea = this.Page.Locator("textarea.weave-field__input").Last
    do! this.Expect(textarea).ToBeVisibleAsync()
  }
