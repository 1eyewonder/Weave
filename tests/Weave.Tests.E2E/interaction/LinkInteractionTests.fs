namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type LinkInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("link-action")>]
  [<InlineData("icon-action")>]
  member this.``a callback without a destination preserves the route and scroll position``(testId: string) =
    task {
      do! this.NavigateTo("link")
      let action = this.Page.GetByTestId(testId)
      do! action.ScrollIntoViewIfNeededAsync()
      let! before = this.Page.EvaluateAsync<float>("window.scrollY")
      Assert.True(before > 0.0)
      do! action.PressAsync("Enter")
      do! this.Expect(this.Page.GetByTestId("link-count")).ToHaveTextAsync("1")
      let! hash = this.Page.EvaluateAsync<string>("location.hash")
      Assert.Equal("#link", hash)
      let! after = this.Page.EvaluateAsync<float>("window.scrollY")
      Assert.Equal(before, after)
      do! this.Expect(action).ToHaveRoleAsync(AriaRole.Button)
      do! this.Expect(action).ToHaveAttributeAsync("type", "button")
      do! action.PressAsync("Space")
      do! this.Expect(this.Page.GetByTestId("link-count")).ToHaveTextAsync("2")
    }

  [<Theory>]
  [<InlineData("Destination")>]
  [<InlineData("Go to destination")>]
  member this.``disabling and re-enabling a destination updates navigation and its accessible state``
    (name: string)
    = task {
    do! this.NavigateTo("link")

    let link =
      this.Page.GetByRole(AriaRole.Link, PageGetByRoleOptions(Name = name, Exact = true))

    let toggle =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Toggle links"))

    do! toggle.ClickAsync()
    do! this.Expect(link).ToBeDisabledAsync()
    let! disabledHref = link.GetAttributeAsync("href")
    Assert.Null(disabledHref)
    let! _ = link.EvaluateAsync("el => el.click()")
    do! this.Expect(this.Page.GetByTestId("link-count")).ToHaveTextAsync("0")
    let! disabledHash = this.Page.EvaluateAsync<string>("location.hash")
    Assert.Equal("#link", disabledHash)
    do! toggle.ClickAsync()
    do! this.Expect(link).ToBeEnabledAsync()
    do! this.Expect(link).ToHaveAttributeAsync("href", "#destination")
    do! link.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("link-count")).ToHaveTextAsync("1")
    let! enabledHash = this.Page.EvaluateAsync<string>("location.hash")
    Assert.Equal("#destination", enabledHash)
  }

  [<Theory>]
  [<InlineData("count-link")>]
  [<InlineData("count-icon-link")>]
  member this.``native link clicks and Enter invoke callback once``(testId: string) = task {
    do! this.NavigateTo("link")
    let link = this.Page.GetByTestId(testId)
    let count = this.Page.GetByTestId("link-count")
    let! _ = link.EvaluateAsync("el => el.click()")
    do! this.Expect(count).ToHaveTextAsync("1")
    do! link.PressAsync("Enter")
    do! this.Expect(count).ToHaveTextAsync("2")

    let! prevented =
      this.Page
        .GetByTestId("disabled-count-link")
        .EvaluateAsync<bool>(
          "el => !el.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true }))"
        )

    Assert.True(prevented)
    do! this.Expect(count).ToHaveTextAsync("2")
  }

  [<Theory>]
  [<InlineData("count-link")>]
  [<InlineData("count-icon-link")>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap activates a link``(testId: string) = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "link")
    do! page.GetByTestId(testId).TapAsync()

    let! _ =
      page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("link-count")).ToHaveTextAsync("1")
  }

  [<Fact>]
  member this.``a destination retains its caller-supplied role as its availability changes``() = task {
    do! this.NavigateTo("link")

    let item =
      this.Page.GetByRole(AriaRole.Menuitem, PageGetByRoleOptions(Name = "Menu destination"))

    let toggle =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Toggle links"))

    do! this.Expect(item).ToBeEnabledAsync()
    do! toggle.ClickAsync()
    do! this.Expect(item).ToBeDisabledAsync()
    do! toggle.ClickAsync()
    do! this.Expect(item).ToBeEnabledAsync()
  }

  [<Fact>]
  member this.``link is focusable``() = task {
    do! this.NavigateTo("link")
    let link = this.Page.Locator(".weave-link").First
    do! link.FocusAsync()
    do! this.Expect(link).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``no underline link keeps its dimensions through hover and focus``() = task {
    do! this.NavigateTo("link")

    let link =
      this.Page.GetByRole(
        Microsoft.Playwright.AriaRole.Link,
        Microsoft.Playwright.PageGetByRoleOptions(Name = "No Underline", Exact = true)
      )

    let label = link.Locator(".weave-link__text")
    do! this.Page.Mouse.MoveAsync(1.0f, 1.0f)
    let! normal = link.BoundingBoxAsync()
    do! link.HoverAsync()
    let! hovered = link.BoundingBoxAsync()

    let! border =
      label.EvaluateAsync<string>("el => getComputedStyle(el).borderBottomWidth")

    Assert.Equal("0px", border)
    Assert.InRange(abs (hovered.Height - normal.Height), 0.0f, 0.1f)
    do! link.FocusAsync()
    let! focused = link.BoundingBoxAsync()
    Assert.InRange(abs (focused.Height - normal.Height), 0.0f, 0.1f)
  }
