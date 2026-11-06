namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright.Xunit
open Microsoft.Playwright

[<Collection("E2E")>]
type CardInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``heading passed as the title is exposed as a heading``() = task {
    do! this.NavigateTo("card")

    let heading =
      this.Page.GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = "Bordered card", Level = 2))

    do! this.Expect(heading).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``heading passed as the title keeps the header title size``() = task {
    do! this.NavigateTo("card")

    let! plainSize =
      this.Page
        .Locator(".weave-card__title", PageLocatorOptions(HasText = "Elevated card"))
        .EvaluateAsync<string>("el => getComputedStyle(el).fontSize")

    let heading =
      this.Page.GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = "Bordered card", Level = 2))

    do! this.Expect(heading).ToHaveCSSAsync("font-size", plainSize)
  }

  [<Fact>]
  member this.``plain text title is not a heading``() = task {
    do! this.NavigateTo("card")

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = "Elevated card")))
        .ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``action area activates the primary action``() = task {
    do! this.NavigateTo("card")

    do! this.Page.GetByTestId("action-card").Locator(".weave-card__content").ClickAsync()
    do! this.Expect(this.Page.GetByTestId("opened-card")).ToHaveTextAsync("clickable")
  }

  [<Fact>]
  member this.``action area activates from the keyboard``() = task {
    do! this.NavigateTo("card")

    do! this.Page.GetByTestId("action-card").Locator(".weave-card__action-area").FocusAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("opened-card")).ToHaveTextAsync("clickable")
  }

  [<Fact>]
  member this.``secondary action outside the area does not trigger the primary action``() = task {
    do! this.NavigateTo("card")

    do!
      this.Page
        .GetByTestId("action-card")
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Secondary"))
        .ClickAsync()

    do! this.Expect(this.Page.GetByTestId("opened-card")).ToHaveTextAsync("secondary")
  }

  [<Fact>]
  member this.``action area with an href renders as a link``() = task {
    do! this.NavigateTo("card")

    let link =
      this.Page
        .GetByTestId("link-card")
        .GetByRole(AriaRole.Link, LocatorGetByRoleOptions(Name = "Linked card"))

    do! this.Expect(link).ToHaveAttributeAsync("href", "#card")
  }

  [<Fact>]
  member this.``filled card has no visible border``() = task {
    do! this.NavigateTo("card")

    do!
      this.Expect(this.Page.GetByTestId("filled-card")).ToHaveCSSAsync("border-top-color", "rgba(0, 0, 0, 0)")
  }

  [<Fact>]
  member this.``default card has a visible border``() = task {
    do! this.NavigateTo("card")

    do! this.Expect(this.Page.GetByTestId("bordered-card")).ToHaveCSSAsync("border-top-width", "1px")
  }
