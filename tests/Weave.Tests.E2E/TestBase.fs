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
    member _.InitializeAsync() =
      ValueTask(
        task {
          let! ctx, p = fixture.NewPageAsync()
          context <- ctx
          page <- p
        }
        :> Task
      )

    member _.DisposeAsync() =
      ValueTask(
        task {
          if not (isNull context) then
            do! context.CloseAsync()
        }
        :> Task
      )

  member _.NavigatePage(page: IPage, pageName: string) = task {
    let! _ = page.GotoAsync($"%s{fixture.BaseUrl}/#%s{pageName}")

    let! _ =
      page.WaitForSelectorAsync(
        "[data-e2e-ready]",
        PageWaitForSelectorOptions(State = WaitForSelectorState.Attached)
      )

    ()
  }

  member this.NavigateTo(pageName: string) = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    do! this.NavigatePage(this.Page, pageName)
  }

  member _.Expect(locator: ILocator) = Assertions.Expect(locator)

  member this.RunAxeScan(pageName: string, ?contrast: bool, ?dark: bool) = task {
    do! this.NavigateTo(pageName)

    let checkContrast =
      defaultArg contrast (System.Environment.GetEnvironmentVariable("WEAVE_AUDIT_CONTRAST") = "1")

    // Measure the settled state, not a transient frame during an entrance or theme transition.
    if checkContrast then
      let! _ =
        this.Page.AddStyleTagAsync(
          PageAddStyleTagOptions(
            Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
          )
        )

      ()

    if defaultArg dark (System.Environment.GetEnvironmentVariable("WEAVE_AUDIT_DARK") = "1") then
      let! _ =
        this.Page.EvaluateAsync<obj>("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let options = AxeRunOptions()

    options.Rules <-
      dict [
        // Test pages are isolated component renderings, not full pages with landmarks
        "region", RuleOptions(Enabled = false)
        "landmark-one-main", RuleOptions(Enabled = false)
        "page-has-heading-one", RuleOptions(Enabled = false)
        // Opt in to contrast review: fixtures include deliberately disabled, animated, and custom-colored examples.
        "color-contrast", RuleOptions(Enabled = checkContrast)
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
