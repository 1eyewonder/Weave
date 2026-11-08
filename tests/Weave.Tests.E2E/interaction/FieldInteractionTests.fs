namespace Weave.Tests.E2E

open Xunit

type FieldInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``clicking field control focuses its input``() = task {
    do! this.NavigateTo("field")
    let control = this.Page.Locator(".weave-field__control").First
    let! _ = control.EvaluateAsync("el => el.click()")
    do! this.Expect(control.Locator("input")).ToBeFocusedAsync()
  }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touching field control focuses its input``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "field")
    let control = page.Locator(".weave-field__control").First
    do! control.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(control.Locator("input")).ToBeFocusedAsync()
  }

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
