namespace Weave.Tests.E2E

open System.Threading.Tasks
open Xunit
open Deque.AxeCore.Commons
open Deque.AxeCore.Playwright
open Microsoft.Playwright

[<AbstractClass>]
type E2ETestBase(fixture: TestFixture) =
  let mutable context: IBrowserContext = null
  let mutable page: IPage = null

  member _.Page = page

  interface IAsyncLifetime with
    member _.InitializeAsync() = task {
      let! ctx, p = fixture.NewPageAsync()
      context <- ctx
      page <- p
    }

    member _.DisposeAsync() = task {
      if not (isNull context) then
        do! context.CloseAsync()
    }

  member this.NavigateTo(pageName: string) = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"%s{fixture.BaseUrl}/#%s{pageName}")

    let! _ =
      this.Page.WaitForSelectorAsync(
        "[data-e2e-ready]",
        PageWaitForSelectorOptions(State = WaitForSelectorState.Attached)
      )

    ()
  }

  member _.Expect(locator: ILocator) = Assertions.Expect(locator)

  member this.RunAxeScan(pageName: string) = task {
    do! this.NavigateTo(pageName)

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

      Assert.Fail($"Accessibility violations on #{pageName}:\n\n{violations}")
  }
