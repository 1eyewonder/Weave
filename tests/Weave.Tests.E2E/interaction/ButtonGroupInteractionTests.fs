namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type ButtonGroupInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``split menu actions keep their hover highlight inside a button group``() = task {
    do! this.NavigateTo("buttongroup")
    let menu = this.Page.GetByTestId("grouped-split-menu")
    let trigger = menu.Locator(".weave-button-menu__trigger")
    let action = this.Page.GetByTestId("grouped-split-action")

    let! _ =
      this.Page.AddStyleTagAsync(
        PageAddStyleTagOptions(
          Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
        )
      )

    do! trigger.ClickAsync()
    do! this.Expect(menu).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-button-menu--open"))
    do! this.Page.Mouse.MoveAsync(0.0f, 0.0f)

    let! idleBackground =
      action.EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    do! action.HoverAsync()

    let! hoverBackground =
      action.EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.NotEqual<string>(idleBackground, hoverBackground)
  }
