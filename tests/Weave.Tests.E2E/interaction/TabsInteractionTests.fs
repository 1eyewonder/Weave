namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit

type TabsInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``filled tab retains its rounded shape as hover moves away``() = task {
    do! this.NavigateTo("tabs-contrast")
    let tabs = this.Page.GetByTestId("tabs-tray-primary")
    let selected = tabs.Locator(".weave-tabs__tab--active")
    let neighbor = tabs.Locator(".weave-tabs__tab").Nth(1)
    let indicator = tabs.Locator(".weave-tabs__indicator")

    let! expectedRadius =
      indicator.EvaluateAsync<string>("el => getComputedStyle(el).borderTopLeftRadius")

    Assert.True(expectedRadius <> "0px", "The indicator should have rounded corners")
    do! selected.HoverAsync()
    do! this.Expect(selected).ToHaveCSSAsync("border-top-left-radius", expectedRadius)
    do! neighbor.HoverAsync()
    do! this.Expect(selected).ToHaveCSSAsync("border-top-left-radius", expectedRadius)
  }

  [<Fact>]
  member this.``programmatic click activates a tab``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    let! _ = secondTab.EvaluateAsync("el => el.click()")
    do! this.Expect(secondTab).ToHaveAttributeAsync("aria-selected", "true")
  }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap activates a tab``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "tabs")
    let secondTab = page.Locator(".weave-tabs__tab").Nth(1)
    do! secondTab.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(secondTab).ToHaveAttributeAsync("aria-selected", "true")
  }

  [<Fact>]
  member this.``tab header has role tablist``() = task {
    do! this.NavigateTo("tabs")
    let tablist = this.Page.Locator(".weave-tabs__header[role='tablist']").First
    do! this.Expect(tablist).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``tab buttons have role tab``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! this.Expect(firstTab).ToHaveAttributeAsync("role", "tab")
  }

  [<Fact>]
  member this.``panels have role tabpanel``() = task {
    do! this.NavigateTo("tabs")
    let panels = this.Page.Locator(".weave-tabs__panel[role='tabpanel']")
    do! this.Expect(panels.First).ToBeAttachedAsync()
  }

  [<Fact>]
  member this.``active tab has aria-selected true``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! this.Expect(firstTab).ToHaveAttributeAsync("aria-selected", "true")
  }

  [<Fact>]
  member this.``inactive tab has aria-selected false``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! this.Expect(secondTab).ToHaveAttributeAsync("aria-selected", "false")
  }

  [<Fact>]
  member this.``active tab has tabindex 0``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! this.Expect(firstTab).ToHaveAttributeAsync("tabindex", "0")
  }

  [<Fact>]
  member this.``inactive tab has tabindex -1``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! this.Expect(secondTab).ToHaveAttributeAsync("tabindex", "-1")
  }

  [<Fact>]
  member this.``tab has aria-controls linking to panel``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    let! ariaControls = firstTab.GetAttributeAsync("aria-controls")
    Assert.NotNull(ariaControls)
    let panel = this.Page.Locator($"#{ariaControls}")
    do! this.Expect(panel).ToBeAttachedAsync()
    do! this.Expect(panel).ToHaveAttributeAsync("role", "tabpanel")
  }

  [<Fact>]
  member this.``tab button is focusable``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! firstTab.FocusAsync()
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter activates a tab``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")
    do! this.Expect(secondTab).ToHaveClassAsync(Regex("weave-tabs__tab--active"))
  }

  [<Fact>]
  member this.``Space activates a tab``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync(" ")
    do! this.Expect(secondTab).ToHaveClassAsync(Regex("weave-tabs__tab--active"))
  }

  [<Fact>]
  member this.``ArrowRight moves focus to the next tab``() = task {
    do! this.NavigateTo("tabs")
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    do! firstTab.FocusAsync()
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(secondTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowLeft moves focus to the previous tab``() = task {
    do! this.NavigateTo("tabs")
    let secondTab = this.Page.Locator(".weave-tabs__tab").Nth(1)
    let firstTab = this.Page.Locator(".weave-tabs__tab").First
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowLeft")
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``ArrowRight wraps from last enabled tab to first tab``() = task {
    do! this.NavigateTo("tabs")
    let tabsContainer = this.Page.Locator(".weave-tabs").First
    let tabs = tabsContainer.Locator("[role='tab']:not([disabled])")
    let! count = tabs.CountAsync()
    let lastEnabledTab = tabs.Nth(count - 1)
    let firstTab = tabs.First
    do! lastEnabledTab.FocusAsync()
    do! lastEnabledTab.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("ArrowRight")
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Home moves focus to the first tab``() = task {
    do! this.NavigateTo("tabs")
    let tabsContainer = this.Page.Locator(".weave-tabs").First
    let tabs = tabsContainer.Locator("[role='tab']")
    let secondTab = tabs.Nth(1)
    let firstTab = tabs.First
    do! secondTab.FocusAsync()
    do! secondTab.PressAsync("Enter")
    do! this.Page.Keyboard.PressAsync("Home")
    do! this.Expect(firstTab).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``End moves focus to the last enabled tab``() = task {
    do! this.NavigateTo("tabs")
    let tabsContainer = this.Page.Locator(".weave-tabs").First
    let tabs = tabsContainer.Locator("[role='tab']:not([disabled])")
    let! count = tabs.CountAsync()
    let firstTab = tabs.First
    let lastEnabledTab = tabs.Nth(count - 1)
    do! firstTab.FocusAsync()
    do! this.Page.Keyboard.PressAsync("End")
    do! this.Expect(lastEnabledTab).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("top")>]
  [<InlineData("bottom")>]
  member this.``resizing reveals the selected tab and preserves manual scrolling``(position: string) = task {
    do! this.NavigateTo("tabs-resize-" + position)
    let root = this.Page.GetByTestId("resize-tabs")
    let selected = root.Locator("[role='tab']").Last
    let header = root.Locator("[role='tablist']")
    let action = this.Page.GetByTestId("panel-action")
    do! selected.ClickAsync()
    do! action.FocusAsync()
    do! this.Page.SetViewportSizeAsync(320, 900)

    let! _ =
      this.Page.WaitForFunctionAsync(
        """() => document.querySelector('[data-testid="resize-tabs"] [role="tablist"]').clientWidth < 320"""
      )

    let! _ =
      this.Page.WaitForFunctionAsync(
        """() => {
          const header = document.querySelector('[data-testid="resize-tabs"] [role="tablist"]');
          const selected = header.querySelector('[aria-selected="true"]');
          const view = header.getBoundingClientRect();
          const tab = selected.getBoundingClientRect();
          return tab.left >= view.left - 1 && tab.right <= view.right + 1;
        }""",
        null,
        Microsoft.Playwright.PageWaitForFunctionOptions(Timeout = 3000.0f)
      )

    do! this.Expect(selected).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(root.Locator("[role='tabpanel']:visible")).ToHaveTextAsync("Settings panel")
    do! this.Expect(action).ToBeFocusedAsync()
    let! selectedScroll = header.EvaluateAsync<float>("el => el.scrollLeft")
    Assert.True(selectedScroll > 0.0, "The last selected tab requires horizontal scrolling at 320px")

    do!
      root
        .GetByRole(
          Microsoft.Playwright.AriaRole.Button,
          Microsoft.Playwright.LocatorGetByRoleOptions(Name = "Scroll tabs backward")
        )
        .ClickAsync()

    let! _ =
      this.Page.WaitForFunctionAsync(
        """selectedScroll => document.querySelector('[data-testid="resize-tabs"] [role="tablist"]').scrollLeft < selectedScroll - 20""",
        selectedScroll
      )

    let! _ =
      header.EvaluateAsync(
        """el => new Promise((resolve, reject) => {
          const deadline = performance.now() + 3000;
          let previous = el.scrollLeft;
          let stable = 0;
          const check = () => {
            if (performance.now() >= deadline) {
              reject(new Error('Tab scrolling did not settle'));
              return;
            }
            stable = Math.abs(el.scrollLeft - previous) < .1 ? stable + 1 : 0;
            previous = el.scrollLeft;
            if (stable >= 6) resolve(); else requestAnimationFrame(check);
          };
          requestAnimationFrame(check);
        })"""
      )

    let! manualScroll = header.EvaluateAsync<float>("el => el.scrollLeft")
    do! this.Page.SetViewportSizeAsync(320, 800)

    let! afterHeightResize =
      header.EvaluateAsync<float>(
        """el => new Promise(resolve => requestAnimationFrame(() =>
          requestAnimationFrame(() => resolve(el.scrollLeft))))"""
      )

    Assert.True(
      abs (afterHeightResize - manualScroll) <= 1.0,
      "Height-only resizing must preserve manual header scrolling"
    )

    do! this.Expect(selected).ToHaveAttributeAsync("aria-selected", "true")
  }
