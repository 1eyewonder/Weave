namespace Weave.Tests.E2E

open Xunit

type DrawerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``drawer has aria-label``() = task {
    do! this.NavigateTo("drawer")
    let drawer = this.Page.Locator(".weave-drawer").First
    do! this.Expect(drawer).ToHaveAttributeAsync("aria-label", "Navigation drawer")
  }

  [<Fact>]
  member this.``drawer content is visible when open``() = task {
    do! this.NavigateTo("drawer")
    let drawer = this.Page.Locator(".weave-drawer").First
    do! this.Expect(drawer).ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData("drawer", 390, 844)>]
  [<InlineData("drawer-responsive", 390, 844)>]
  [<InlineData("drawer", 1280, 800)>]
  [<InlineData("drawer-responsive", 1280, 800)>]
  member this.``closed drawer does not receive keyboard focus``(pageName: string, width: int, height: int) =
    task {
      do! this.NavigateTo(pageName)
      do! this.Page.SetViewportSizeAsync(width, height)
      let! _ = this.Page.GetByTestId("drawer-toggle").EvaluateAsync("el => el.click()")
      let drawer = this.Page.Locator(".weave-drawer")
      let button = this.Page.GetByTestId("drawer-button")
      do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))
      do! this.Page.GetByTestId("drawer-toggle").FocusAsync()
      do! this.Page.Keyboard.PressAsync("Tab")
      do! this.Expect(button).Not.ToBeFocusedAsync()
      do! this.Expect(this.Page.GetByTestId("main-focus-target")).ToBeFocusedAsync()
    }

  [<Fact>]
  member this.``document listener stops after its active owner is removed``() = task {
    do! this.NavigateTo("listener-removal")
    let toggle = this.Page.GetByTestId("remove-listener")
    let outside = this.Page.GetByTestId("listener-outside")
    let owner = this.Page.GetByTestId("listener-owner")
    let calls = this.Page.GetByTestId("listener-calls")

    for cycle in 1..3 do
      do! outside.ClickAsync()
      do! this.Expect(calls).ToHaveTextAsync(string (2 * cycle - 1))
      do! toggle.ClickAsync()
      do! this.Expect(owner).ToHaveCountAsync(0)
      do! this.Expect(calls).ToHaveTextAsync(string (2 * cycle))
      do! outside.ClickAsync()
      do! this.Expect(calls).ToHaveTextAsync(string (2 * cycle))
      do! toggle.ClickAsync()
      do! this.Expect(owner).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``initially open responsive drawer is visible on mobile``() = task {
    do! this.Page.SetViewportSizeAsync(390, 844)
    let! _ = this.Page.GotoAsync($"%s{fixture.BaseUrl}/#drawer-responsive")
    do! this.Expect(this.Page.GetByTestId("drawer-button")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Escape closes a temporary drawer and returns focus``() = task {
    do! this.NavigateTo("drawer-closed")
    let trigger = this.Page.GetByTestId("drawer-toggle")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    let item = this.Page.GetByTestId("drawer-button")

    do!
      this
        .Expect(this.Page.Locator(".weave-drawer"))
        .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--open"))

    do! item.FocusAsync()
    do! this.Expect(item).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")

    do!
      this
        .Expect(this.Page.Locator(".weave-drawer"))
        .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))

    do! this.Expect(trigger).ToBeFocusedAsync()
  }
