namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

type PageReadinessTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("slider", "[role='slider']", "aria-valuenow", 11)>]
  [<InlineData("slider", "[role='slider']", "aria-label", 11)>]
  [<InlineData("chip", "[data-testid='clickable-chip']", "tabindex", 1)>]
  [<InlineData("dropdown-menu", ".weave-dropdownmenu .weave-button", "aria-expanded", 1)>]
  member this.``ready marker follows initial dynamic bindings``
    (pageName: string, selector: string, attribute: string, count: int)
    = task {
    let! _ = this.Page.GotoAsync($"%s{fixture.BaseUrl}/#%s{pageName}")

    let! _ =
      this.Page.WaitForSelectorAsync(
        "[data-e2e-ready]",
        PageWaitForSelectorOptions(State = WaitForSelectorState.Attached)
      )

    let! values =
      this.Page
        .Locator(selector)
        .EvaluateAllAsync<string[]>("(els, attr) => els.map(el => el.getAttribute(attr))", attribute)

    Assert.Equal(count, values.Length)

    Assert.All(
      values,
      fun value ->
        Assert.False(System.String.IsNullOrEmpty value, $"Missing {attribute} after page readiness")
    )
  }

  [<Fact>]
  member this.``ready marker waits for the initial render synchronization``() = task {
    // Hold animation-frame callbacks so the initial DOM can be inspected before WebSharper synchronizes it.
    use! _script =
      this.Page.AddInitScriptAsync(
        """(() => {
          const requestFrame = window.requestAnimationFrame.bind(window);
          const pending = [];
          window.requestAnimationFrame = callback => { pending.push(callback); return pending.length; };
          window.releaseInitialFrames = () => {
            window.requestAnimationFrame = requestFrame;
            for (const callback of pending) requestFrame(callback);
          };
        })()"""
      )

    let! _ = this.Page.GotoAsync($"%s{fixture.BaseUrl}/#slider")

    let! _ =
      this.Page.WaitForSelectorAsync(
        "[role='slider']",
        PageWaitForSelectorOptions(State = WaitForSelectorState.Attached)
      )

    let! readyBeforeSync =
      this.Page.EvaluateAsync<bool>("() => document.querySelector('[data-e2e-ready]') !== null")

    Assert.False(readyBeforeSync, "The page cannot be ready before its initial render synchronization")
    let! _ = this.Page.EvaluateAsync("() => window.releaseInitialFrames()")

    let! _ =
      this.Page.WaitForSelectorAsync(
        "[data-e2e-ready]",
        PageWaitForSelectorOptions(State = WaitForSelectorState.Attached)
      )

    let! namesAndValuesReady =
      this.Page.EvaluateAsync<bool>(
        "() => [...document.querySelectorAll('[role=slider]')].every(el => el.hasAttribute('aria-label') && el.hasAttribute('aria-valuenow'))"
      )

    Assert.True(namesAndValuesReady, "Initial slider bindings should be applied when the page is ready")
  }
