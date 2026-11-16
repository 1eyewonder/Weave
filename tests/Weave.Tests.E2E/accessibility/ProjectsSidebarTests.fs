namespace Weave.Tests.E2E

open System
open System.IO
open Deque.AxeCore.Commons
open Deque.AxeCore.Playwright
open Microsoft.Playwright
open Xunit

type ProjectsSidebarTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``given a Projects theme then its sidebar labels meet normal text contrast``(theme: string) =
    task {
      do!
        this.Page.SetContentAsync(
          $"""<body data-theme="%s{theme}"><div class="workspace"><aside class="sidebar">
          <div class="sidebar-label">WORKSPACE</div>
          <div class="sidebar-label">CURRENT PROJECT</div>
        </aside></div></body>"""
        )

      let stylesheet =
        Path.Combine(AppContext.BaseDirectory, "templates", "projects-workspace.css")

      let! _ = this.Page.AddStyleTagAsync(PageAddStyleTagOptions(Path = stylesheet))

      let options =
        AxeRunOptions(RunOnly = RunOnlyOptions(Type = "rule", Values = ResizeArray [ "color-contrast" ]))

      let! result = this.Page.RunAxe(options)
      Assert.Empty(result.Violations)
      let passed = Assert.Single(result.Passes)
      Assert.Equal(2, passed.Nodes.Length)
    }
