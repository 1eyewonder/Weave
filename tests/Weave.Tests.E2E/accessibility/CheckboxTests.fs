namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type CheckboxTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("checkbox")

  [<Fact>]
  member this.``checkbox input is focusable``() = task {
    do! this.NavigateTo("checkbox")
    let input = this.Page.Locator(".weave-checkbox__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Space toggles an unchecked checkbox to checked``() = task {
    do! this.NavigateTo("checkbox")
    let input = this.Page.Locator(".weave-checkbox__input").First
    do! input.FocusAsync()
    do! input.PressAsync("Space")
    do! this.Expect(input).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``disabled checkbox is not focusable``() = task {
    do! this.NavigateTo("checkbox")
    let disabledInput = this.Page.Locator(".weave-checkbox__input[disabled]").First
    do! disabledInput.FocusAsync()
    do! this.Expect(disabledInput).Not.ToBeFocusedAsync()
  }
