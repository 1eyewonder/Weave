namespace Weave.Tests.Rendering

open Microsoft.Playwright.Xunit
open System.IO
open System.Reflection

[<AbstractClass>]
type LayoutTestBase(fixtureName: string) =
  inherit PageTest()

  member _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, $"{fixtureName}.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  member this.LoadFixture(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  member this.SetTheme(theme: string) = task {
    let! _ = this.Page.EvaluateAsync($"document.documentElement.setAttribute('data-theme', '{theme}')")
    ()
  }

  member this.ComputedStyle(selector: string, property: string) = task {
    return! this.Page.Locator(selector).EvaluateAsync<string>($"el => getComputedStyle(el).{property}")
  }
