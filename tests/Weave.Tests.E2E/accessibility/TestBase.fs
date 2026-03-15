namespace Weave.Tests.E2E

open Deque.AxeCore.Commons
open Deque.AxeCore.Playwright
open Microsoft.Playwright.Xunit
open Xunit

[<AbstractClass>]
type E2ETestBase(server: TestServerFixture) =
  inherit PageTest()

  member this.NavigateTo(page: string) = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"%s{server.BaseUrl}/#%s{page}")

    let! _ =
      this.Page.WaitForSelectorAsync(
        "[data-e2e-ready]",
        Microsoft.Playwright.PageWaitForSelectorOptions(
          State = Microsoft.Playwright.WaitForSelectorState.Attached
        )
      )

    ()
  }

  member this.RunAxeScan(page: string) = task {
    do! this.NavigateTo(page)

    let options = AxeRunOptions()

    options.Rules <-
      dict [
        // Test pages are isolated component renderings, not full pages with landmarks
        "region", RuleOptions(Enabled = false)
        "landmark-one-main", RuleOptions(Enabled = false)
        "page-has-heading-one", RuleOptions(Enabled = false)
        // Disabled/placeholder elements intentionally have reduced contrast (WCAG 2.1 SC 1.4.3 exemption)
        "color-contrast", RuleOptions(Enabled = false)
      ]
      |> System.Collections.Generic.Dictionary

    let! result = this.Page.RunAxe(options)

    if result.Violations.Length > 0 then
      let violations =
        result.Violations
        |> Array.map (fun v ->
          let nodes = v.Nodes |> Array.map (fun n -> $"    - {n.Html}") |> String.concat "\n"

          $"[{v.Impact}] {v.Id}: {v.Description}\n{nodes}")
        |> String.concat "\n\n"

      Assert.Fail($"Accessibility violations on #{page}:\n\n{violations}")
  }
