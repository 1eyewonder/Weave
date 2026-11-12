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
  [<InlineData("persistent-left", 320, 208.0)>]
  [<InlineData("persistent-right", 320, 208.0)>]
  [<InlineData("persistent-left", 1280, 208.0)>]
  [<InlineData("persistent-right", 1280, 208.0)>]
  [<InlineData("responsive-left", 320, 0.0)>]
  [<InlineData("responsive-right", 320, 0.0)>]
  [<InlineData("responsive-left", 1280, 208.0)>]
  [<InlineData("responsive-right", 1280, 208.0)>]
  [<InlineData("mini-left", 320, 208.0)>]
  [<InlineData("mini-right", 320, 208.0)>]
  [<InlineData("mini-left", 1280, 208.0)>]
  [<InlineData("mini-right", 1280, 208.0)>]
  [<InlineData("default-left", 1280, 256.0)>]
  [<InlineData("default-right", 1280, 256.0)>]
  member this.``configured drawer width agrees with app bar and main content``
    (route: string, viewportWidth: int, expectedPush: float)
    = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    do! this.NavigatePage(this.Page, "drawer-width-" + route)

    let! _ =
      this.Page.AddStyleTagAsync(
        Microsoft.Playwright.PageAddStyleTagOptions(
          Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
        )
      )

    let! geometry =
      this.Page
        .GetByTestId("width-host")
        .EvaluateAsync<float[]>(
          """host => {
          const outer = host.getBoundingClientRect();
          const drawer = host.querySelector('.weave-drawer').getBoundingClientRect();
          const bar = host.querySelector('.weave-appbar').getBoundingClientRect();
          const main = host.querySelector('.weave-main-content').getBoundingClientRect();
          return [drawer.width, bar.left - outer.left, outer.right - bar.right,
            main.left - outer.left, outer.right - main.right];
        }"""
        )

    let expectedWidth = if route.StartsWith("default") then 256.0 else 208.0
    Assert.True(abs (geometry.[0] - expectedWidth) <= 1.0, $"Panel width was {geometry.[0]}px")

    let left = route.EndsWith("left")
    let expectedLeft = if left then expectedPush else 0.0
    let expectedRight = if left then 0.0 else expectedPush

    for index, expected in [ 1, expectedLeft; 2, expectedRight; 3, expectedLeft; 4, expectedRight ] do
      Assert.True(
        abs (geometry.[index] - expected) <= 1.0,
        $"Geometry value {index} was {geometry.[index]}px; expected {expected}px for {route} at {viewportWidth}px"
      )
  }

  [<Theory>]
  [<InlineData("left", 320)>]
  [<InlineData("right", 320)>]
  [<InlineData("left", 1280)>]
  [<InlineData("right", 1280)>]
  member this.``mini hover uses configured width while layout keeps the closed strip``
    (side: string, viewportWidth: int)
    = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    do! this.NavigatePage(this.Page, "drawer-width-hover-" + side)

    let! _ =
      this.Page.AddStyleTagAsync(
        Microsoft.Playwright.PageAddStyleTagOptions(
          Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
        )
      )

    let panel = this.Page.GetByTestId("width-drawer")
    let! closed = panel.BoundingBoxAsync()
    Assert.True(abs (closed.Width - 64.0f) <= 1.0f, $"Closed strip was {closed.Width}px")
    do! panel.HoverAsync()
    let! expanded = panel.BoundingBoxAsync()
    Assert.True(abs (expanded.Width - 208.0f) <= 1.0f, $"Hover panel was {expanded.Width}px")

    let property = if side = "left" then "margin-left" else "margin-right"

    for selector in [ ".weave-appbar"; ".weave-main-content" ] do
      let! margin =
        this.Page
          .Locator(selector)
          .EvaluateAsync<string>($"el => getComputedStyle(el).getPropertyValue('{property}')")

      Assert.Equal("64px", margin)
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``drawer header matches app bar height and centers its content``(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    do! this.NavigatePage(this.Page, "drawer-width-persistent-left")
    let! header = this.Page.GetByTestId("width-header").BoundingBoxAsync()
    and! title = this.Page.GetByTestId("width-header-title").BoundingBoxAsync()
    and! icon = this.Page.GetByTestId("width-header-icon").BoundingBoxAsync()
    and! bar = this.Page.Locator(".weave-appbar").BoundingBoxAsync()

    Assert.True(
      abs (header.Height - bar.Height) <= 1.0f,
      $"Drawer header {header.Height}px should align with app bar {bar.Height}px"
    )

    Assert.True(
      abs ((header.Y + header.Height / 2.0f) - (title.Y + title.Height / 2.0f))
      <= 1.0f,
      "Drawer header content should be vertically centered"
    )

    Assert.True(
      abs ((header.Y + header.Height / 2.0f) - (icon.Y + icon.Height / 2.0f)) <= 1.0f,
      "Drawer header icon should be vertically centered"
    )
  }

  [<Theory>]
  [<InlineData("left")>]
  [<InlineData("right")>]
  member this.``standalone mini drawer honors its configured width``(side: string) = task {
    do! this.NavigateTo("drawer-width-standalone-mini-" + side)
    let! panel = this.Page.GetByTestId("standalone-width-drawer").BoundingBoxAsync()
    Assert.True(abs (panel.Width - 208.0f) <= 1.0f, $"Standalone panel was {panel.Width}px")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``short drawer navigation plus header fits without scrolling``(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    do! this.NavigatePage(this.Page, "drawer-sizing-short")

    let! fits =
      this.Page
        .GetByTestId("sizing-drawer")
        .EvaluateAsync<bool>("el => el.scrollHeight <= el.clientHeight + 1")

    Assert.True(fits, "A header and three short navigation items should not create an empty scroll region")
    let! host = this.Page.GetByTestId("sizing-host").BoundingBoxAsync()
    and! panel = this.Page.GetByTestId("sizing-drawer").BoundingBoxAsync()
    Assert.True(abs (panel.Y - host.Y) <= 1.0f, "FullHeight drawer should begin at the top of its host")
    Assert.True(abs (panel.Height - host.Height) <= 1.0f, "FullHeight drawer should fill its host")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``long drawer navigation remains scrollable to the last item``(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    do! this.NavigatePage(this.Page, "drawer-sizing-long")
    let panel = this.Page.GetByTestId("sizing-drawer")
    let last = this.Page.GetByTestId("sizing-item-20")
    do! last.ScrollIntoViewIfNeededAsync()
    let! panelBox = panel.BoundingBoxAsync()
    and! lastBox = last.BoundingBoxAsync()
    let! scrollTop = panel.EvaluateAsync<float>("el => el.scrollTop")

    Assert.True(scrollTop > 0.0, "Long navigation should scroll within the drawer")
    Assert.True(lastBox.Y >= panelBox.Y - 1.0f, "Last navigation item should be above the visible lower edge")

    Assert.True(
      lastBox.Y + lastBox.Height <= panelBox.Y + panelBox.Height + 1.0f,
      "Last item should be reachable"
    )
  }

  [<Theory>]
  [<InlineData("temporary", 320)>]
  [<InlineData("persistent", 320)>]
  [<InlineData("responsive", 320)>]
  [<InlineData("mini", 320)>]
  [<InlineData("temporary", 1280)>]
  [<InlineData("persistent", 1280)>]
  [<InlineData("responsive", 1280)>]
  [<InlineData("mini", 1280)>]
  member this.``bounded drawer clip mode starts below the app bar``(variant: string, viewportWidth: int) =
    task {
      do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
      do! this.NavigatePage(this.Page, "drawer-clip-" + variant)
      let! host = this.Page.GetByTestId("sizing-host").BoundingBoxAsync()
      and! panel = this.Page.GetByTestId("sizing-drawer").BoundingBoxAsync()
      and! bar = this.Page.Locator(".weave-appbar").BoundingBoxAsync()

      Assert.True(abs (panel.Y - host.Y - bar.Height) <= 1.0f, $"{variant} drawer should begin below app bar")

      Assert.True(
        abs (panel.Height - (host.Height - bar.Height)) <= 1.0f,
        $"{variant} drawer should fill only the height remaining below the app bar"
      )
    }
