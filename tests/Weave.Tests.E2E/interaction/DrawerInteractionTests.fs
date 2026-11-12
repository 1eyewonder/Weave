namespace Weave.Tests.E2E

open Xunit

[<Collection("Browser input")>]
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
    do! this.NavigatePage(this.Page, "drawer-responsive")
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

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(390)>]
  member this.``opening an overlay drawer moves and contains keyboard focus``(width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 844)
    do! this.NavigatePage(this.Page, "drawer-closed")
    let trigger = this.Page.GetByTestId("drawer-toggle")
    let item = this.Page.GetByTestId("drawer-button")
    do! trigger.FocusAsync()
    do! this.Expect(trigger).ToBeFocusedAsync()
    do! trigger.PressAsync("Enter")

    do!
      this
        .Expect(this.Page.Locator(".weave-drawer"))
        .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--open"))

    do!
      Microsoft.Playwright.Assertions.Expect(item, "Opening the drawer moves focus inside").ToBeFocusedAsync()

    do! this.Page.Keyboard.PressAsync("Tab")
    do! Microsoft.Playwright.Assertions.Expect(item, "After Tab").ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! Microsoft.Playwright.Assertions.Expect(item, "After Shift+Tab").ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``responsive drawer contains focus only while its overlay is visible``() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    do! this.NavigatePage(this.Page, "drawer-responsive")
    let item = this.Page.GetByTestId("drawer-button")
    let main = this.Page.GetByTestId("main-focus-target")
    let overlay = this.Page.Locator(".weave-drawer__overlay")
    do! this.Expect(overlay).ToBeHiddenAsync()
    do! item.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(main).ToBeFocusedAsync()
    do! this.Page.SetViewportSizeAsync(390, 844)
    do! this.Expect(overlay).ToBeVisibleAsync()
    do! this.Expect(item).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(item).ToBeFocusedAsync()
    do! this.Page.SetViewportSizeAsync(1280, 800)
    do! this.Expect(overlay).ToBeHiddenAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(main).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("drawer-native-temporary")>]
  [<InlineData("drawer-native-responsive")>]
  member this.``overlay drawer follows native disclosure and radio focus order``(pageName: string) = task {
    do!
      this.Page.EmulateMediaAsync(
        Microsoft.Playwright.PageEmulateMediaOptions(
          ReducedMotion = Microsoft.Playwright.ReducedMotion.Reduce
        )
      )

    do! this.Page.SetViewportSizeAsync(390, 844)
    do! this.NavigatePage(this.Page, pageName)
    let trigger = this.Page.GetByTestId("drawer-toggle")
    let first = this.Page.GetByTestId("drawer-first")
    let summary = this.Page.GetByTestId("drawer-summary")
    let nested = this.Page.GetByTestId("drawer-nested")
    let radio = this.Page.GetByTestId("drawer-radio-one")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(first).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! Microsoft.Playwright.Assertions.Expect(summary, "Forward Tab from first control").ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(radio).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(first).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! this.Expect(radio).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! Microsoft.Playwright.Assertions.Expect(summary, "Reverse Tab from selected radio").ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(nested).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(nested).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(radio).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(first).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! this.Expect(radio).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("drawer-summary-temporary")>]
  [<InlineData("drawer-summary-responsive")>]
  member this.``collapsed disclosure summary remains the drawer tab boundary``(pageName: string) = task {
    do!
      this.Page.EmulateMediaAsync(
        Microsoft.Playwright.PageEmulateMediaOptions(
          ReducedMotion = Microsoft.Playwright.ReducedMotion.Reduce
        )
      )

    do! this.Page.SetViewportSizeAsync(320, 844)
    do! this.NavigatePage(this.Page, pageName)
    let trigger = this.Page.GetByTestId("drawer-toggle")
    let summary = this.Page.GetByTestId("drawer-only-summary")
    let nested = this.Page.GetByTestId("drawer-only-nested")
    do! trigger.FocusAsync()
    do! trigger.PressAsync("Enter")
    do! this.Expect(summary).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(summary).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! this.Expect(summary).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    do! this.Expect(nested).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(nested).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(summary).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Shift+Tab")
    do! this.Expect(nested).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(trigger).ToBeFocusedAsync()
  }
