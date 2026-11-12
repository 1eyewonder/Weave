namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type TabsInitialIndicatorTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("tabs-initial-top", "left")>]
  [<InlineData("tabs-initial-left", "top")>]
  member this.``initial selected tab is placed immediately while later selection animates``
    (route: string, axis: string)
    = task {
    use! _observer =
      this.Page.AddInitScriptAsync(
        """window.indicatorTransitions = [];
        document.addEventListener('transitionrun', e => {
          if (e.target.classList.contains('weave-tabs__indicator'))
            window.indicatorTransitions.push(e.propertyName);
        });"""
      )

    do! this.NavigateTo(route)
    let indicator = this.Page.Locator(".weave-tabs__indicator")

    let active =
      this.Page.GetByRole(AriaRole.Tab, PageGetByRoleOptions(Name = "Settings", Exact = true))

    do! this.Expect(active).ToHaveAttributeAsync("aria-selected", "true")

    let! _ =
      this.Page.WaitForFunctionAsync(
        """axis => {
          const indicator = document.querySelector('.weave-tabs__indicator');
          const selected = document.querySelector('[role=tab][aria-selected=true]');
          return indicator && selected && parseFloat(indicator.style[axis]) ===
            (axis === 'left' ? selected.offsetLeft : selected.offsetTop);
        }""",
        axis
      )

    let! _ =
      this.Page.EvaluateAsync("() => new Promise(r => requestAnimationFrame(() => requestAnimationFrame(r)))")

    let! initialTransitions =
      this.Page.EvaluateAsync<string[]>("() => window.indicatorTransitions")

    Assert.DoesNotContain(axis, initialTransitions)

    do! this.Page.GetByRole(AriaRole.Tab, PageGetByRoleOptions(Name = "Overview", Exact = true)).ClickAsync()

    let! _ =
      this.Page.WaitForFunctionAsync("axis => window.indicatorTransitions.includes(axis)", axis)

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Tab, PageGetByRoleOptions(Name = "Overview")))
        .ToHaveAttributeAsync("aria-selected", "true")
  }
