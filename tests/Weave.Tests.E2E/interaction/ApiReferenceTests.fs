namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright
open Deque.AxeCore.Commons
open Deque.AxeCore.Playwright

[<Collection("Browser input")>]
type ApiReferenceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Filter =
    this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = "Filter API", Exact = true))

  member private this.CreateTable =
    this.Page.GetByRole(
      AriaRole.Table,
      PageGetByRoleOptions(Name = "Specimen.create API reference", Exact = true)
    )

  member private this.ColorTable =
    this.Page.GetByRole(
      AriaRole.Table,
      PageGetByRoleOptions(Name = "Specimen.Color style reference", Exact = true)
    )

  member private this.ResetTable =
    this.Page.GetByRole(
      AriaRole.Table,
      PageGetByRoleOptions(Name = "Specimen.reset API reference", Exact = true)
    )

  member private this.Load(?width: int) = task {
    do! this.Page.SetViewportSizeAsync(defaultArg width 1280, 900)
    do! this.NavigatePage(this.Page, "api-reference")

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => ['api-reference-style', 'api-reference-highlighting-style'].every(id => document.getElementById(id)?.sheet)"
      )

    do! this.Expect(this.Filter).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``one named filter controls semantic native tables``() = task {
    do! this.Load()
    do! this.Expect(this.Page.GetByRole(AriaRole.Textbox)).ToHaveCountAsync(1)
    do! this.Expect(this.Page.GetByRole(AriaRole.Table)).ToHaveCountAsync(3)
    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToHaveTextAsync("8 API entries")
    do! this.Expect(this.CreateTable.GetByRole(AriaRole.Columnheader)).ToHaveCountAsync(4)
    do! this.Expect(this.CreateTable.GetByRole(AriaRole.Rowheader)).ToHaveCountAsync(5)

    let! tag = this.CreateTable.EvaluateAsync<string>("el => el.tagName")
    Assert.Equal("TABLE", tag)
    let! controls = this.Filter.GetAttributeAsync("aria-controls")
    Assert.False(System.String.IsNullOrWhiteSpace controls)
    do! this.Expect(this.Page.Locator("#" + controls)).ToHaveCountAsync(1)
    do! this.Expect(this.CreateTable.Locator("th[scope='row']")).ToHaveCountAsync(5)
    do! this.Expect(this.CreateTable.Locator("caption")).ToHaveTextAsync("Specimen.create API reference")
  }

  [<Theory>]
  [<InlineData("count", "count")>]
  [<InlineData("view<bool>", "?enabled")>]
  [<InlineData("const false", "?enabled")>]
  [<InlineData("styling", "?attrs")>]
  [<InlineData("  EnAbLeD   bOoL  ", "?enabled")>]
  [<InlineData("[]", "?attrs")>]
  member this.``filter searches names types defaults and descriptions``(query: string, name: string) = task {
    do! this.Load()
    do! this.Filter.FillAsync(query)
    do! this.Expect(this.CreateTable.GetByRole(AriaRole.Rowheader)).ToHaveTextAsync([| name |])
    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToHaveTextAsync("1 of 8 entries")
    do! this.Expect(this.Filter).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("Specimen.create")>]
  [<InlineData("Doc")>]
  [<InlineData("Keyboard")>]
  member this.``title return type and associated notes retain all member rows``(query: string) = task {
    do! this.Load()
    do! this.Filter.FillAsync(query)

    do!
      this
        .Expect(this.CreateTable.GetByRole(AriaRole.Rowheader))
        .ToHaveTextAsync([| "count"; "?enabled"; "?attrs"; "value"; "value" |])

    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToHaveTextAsync("5 of 8 entries")

    do!
      this.Expect(this.Page.GetByText("Keyboard support applies only to Specimen.create.")).ToBeVisibleAsync()

    do!
      this.Expect(this.Page.GetByText("Palette choices apply only to Specimen.Color.")).Not.ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``option search keeps its own explanatory note``() = task {
    do! this.Load()
    do! this.Filter.FillAsync("danger")
    do! this.Expect(this.ColorTable.GetByRole(AriaRole.Rowheader)).ToHaveTextAsync([| "error" |])
    do! this.Expect(this.Page.GetByText("Palette choices apply only to Specimen.Color.")).ToBeVisibleAsync()

    do!
      this
        .Expect(this.Page.GetByText("Keyboard support applies only to Specimen.create."))
        .Not.ToBeVisibleAsync()

    do! this.Expect(this.CreateTable).Not.ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``zero results clears back to original order and input focus``() = task {
    do! this.Load()
    do! this.Filter.FillAsync("nothing-matches-this")
    do! this.Expect(this.Page.GetByRole(AriaRole.Table)).ToHaveCountAsync(0)
    do! this.Expect(this.Page.Locator(".docs-api-no-results")).ToBeVisibleAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToHaveTextAsync("0 of 8 entries")

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Clear filter", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Filter).ToHaveValueAsync("")
    do! this.Expect(this.Filter).ToBeFocusedAsync()
    do! this.Expect(this.Page.Locator(".docs-api-no-results")).Not.ToBeVisibleAsync()

    do!
      this
        .Expect(this.CreateTable.GetByRole(AriaRole.Rowheader))
        .ToHaveTextAsync([| "count"; "?enabled"; "?attrs"; "value"; "value" |])

    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToHaveTextAsync("8 API entries")
  }

  [<Fact>]
  member this.``Escape clears a query while keeping the filter focused``() = task {
    do! this.Load()
    do! this.Filter.FillAsync("enabled")
    do! this.Filter.PressAsync("Escape")
    do! this.Expect(this.Filter).ToHaveValueAsync("")
    do! this.Expect(this.Filter).ToBeFocusedAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Table)).ToHaveCountAsync(3)
  }

  [<Fact>]
  member this.``duplicate parameter names keep separate stable rows``() = task {
    do! this.Load()

    do!
      this
        .Expect(
          this.CreateTable.GetByRole(
            AriaRole.Rowheader,
            LocatorGetByRoleOptions(Name = "value", Exact = true)
          )
        )
        .ToHaveCountAsync(2)

    do! this.Filter.FillAsync("second")
    do! this.Expect(this.CreateTable.GetByRole(AriaRole.Rowheader)).ToHaveTextAsync([| "value" |])

    do!
      this
        .Expect(this.CreateTable.Locator(".docs-api-type"))
        .ToHaveTextAsync("View<Map<string, Result<int option, string list>>>")

    do! this.Filter.FillAsync("")

    do!
      this
        .Expect(
          this.CreateTable.GetByRole(
            AriaRole.Rowheader,
            LocatorGetByRoleOptions(Name = "value", Exact = true)
          )
        )
        .ToHaveCountAsync(2)
  }

  [<Fact>]
  member this.``no argument members remain discoverable and count as one entry``() = task {
    do! this.Load()
    do! this.Filter.FillAsync("reset")
    do! this.Expect(this.Page.GetByRole(AriaRole.Table)).ToHaveCountAsync(1)

    do!
      this
        .Expect(this.ResetTable.GetByText("No parameters.", LocatorGetByTextOptions(Exact = true)))
        .ToBeVisibleAsync()

    do! this.Expect(this.ResetTable.GetByRole(AriaRole.Rowheader)).ToHaveCountAsync(0)
    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToHaveTextAsync("1 of 8 entries")
  }

  [<Fact>]
  member this.``mobile rows wrap long types without empty default labels or horizontal overflow``() = task {
    do! this.Load(width = 320)
    do! this.Expect(this.CreateTable.GetByRole(AriaRole.Rowheader)).ToHaveCountAsync(5)

    let count =
      this.CreateTable.Locator("tbody tr").Filter(LocatorFilterOptions(HasText = "Total record count"))

    do! this.Expect(count.Locator("[data-label='Default']")).Not.ToBeVisibleAsync()

    let longType =
      this.CreateTable
        .Locator("tbody tr")
        .Filter(LocatorFilterOptions(HasText = "Second overload"))
        .Locator("[data-label='Type']")

    do! this.Expect(longType).ToHaveTextAsync("View<Map<string, Result<int option, string list>>>")

    let! fits =
      longType.EvaluateAsync<bool>("el => el.scrollWidth <= el.clientWidth + 1")

    Assert.True(fits, "The full type must wrap within its labelled mobile cell")

    let! pageFits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    let! tableFits =
      this.CreateTable.EvaluateAsync<bool>("el => el.scrollWidth <= el.clientWidth + 1")

    Assert.True(pageFits, "API references must not create page-level horizontal overflow")
    Assert.True(tableFits, "The native semantic table must fit at 320px")
  }

  [<Fact>]
  member this.``leading contextual guidance appears before its member table``() = task {
    do! this.Load()

    let! note =
      this.Page.GetByText("Parameters use declaration order.").BoundingBoxAsync()

    and! table = this.CreateTable.BoundingBoxAsync()
    Assert.True(note.Y + note.Height <= table.Y + 1.0f, "Leading API guidance must precede its table")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``type signatures share code block syntax colors without changing source``(dark: bool) = task {
    do! this.Load()

    if dark then
      let! _ =
        this.Page.EvaluateAsync("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let! _ =
      this.Page.EvaluateAsync(
        """() => {
          const pre = document.createElement('pre');
          const code = document.createElement('code');
          code.id = 'annotation-type-comparison';
          code.className = 'language-fsharp';
          code.textContent = 'let value: View<Map<string, Result<int option, string list>>> = Unchecked.defaultof<_>';
          pre.append(code);
          document.body.append(pre);
          window.highlightCodeElement(code);
        }"""
      )

    let source = "View<Map<string, Result<int option, string list>>>"

    let longType () =
      this.CreateTable
        .Locator("tbody tr")
        .Filter(LocatorFilterOptions(HasText = "Second overload"))
        .Locator("code.docs-api-type")

    let verifySourceAndTokens () = task {
      do! this.Expect(longType ()).ToHaveTextAsync(source)

      do!
        this
          .Expect((longType ()).Locator(".hljs-title.class_"))
          .ToHaveTextAsync([| "View"; "Map"; "Result" |])

      let primitives =
        (longType ())
          .Locator(".hljs-type")
          .Filter(
            LocatorFilterOptions(
              HasTextRegex = System.Text.RegularExpressions.Regex("^(string|int|option|list)$")
            )
          )

      do! this.Expect(primitives).ToHaveTextAsync([| "string"; "int"; "option"; "string"; "list" |])

      let! matchingColors =
        (longType ())
          .EvaluateAsync<bool>(
            """el => {
            const annotation = document.querySelector('#annotation-type-comparison .hljs-type');
            const colors = [...el.querySelectorAll('.hljs-title.class_, .hljs-type')]
              .map(token => getComputedStyle(token).color);
            return annotation !== null && colors.length > 0
              && getComputedStyle(el).fontFamily === getComputedStyle(annotation.closest('code')).fontFamily
              && colors.every(color => color === getComputedStyle(annotation).color)
              && colors[0] !== getComputedStyle(el.querySelector('.hljs-operator')).color;
          }"""
          )

      Assert.True(
        matchingColors,
        "API signatures must share the monospace family and type palette used by actual F# code annotations"
      )
    }

    do! verifySourceAndTokens ()
    do! this.Filter.FillAsync("nothing-matches-this")
    do! this.Expect(this.Page.GetByRole(AriaRole.Table)).ToHaveCountAsync(0)
    do! this.Filter.FillAsync("")
    do! verifySourceAndTokens ()
  }

  [<Fact>]
  member this.``filter stays at the same viewport position when results shrink or disappear``() = task {
    do! this.Load()
    do! this.Filter.ClickAsync()
    let! before = this.Filter.BoundingBoxAsync()

    for query, count in [ "View<bool>", "1 of 8 entries"; "nothing-matches-this", "0 of 8 entries" ] do
      do! this.Filter.FillAsync(query)
      do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToHaveTextAsync(count)
      let! after = this.Filter.BoundingBoxAsync()

      Assert.True(
        abs (before.Y - after.Y) <= 2.0f,
        $"Filtering for {query} moved the input from {before.Y}px to {after.Y}px"
      )

      do! this.Expect(this.Filter).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``reference controls and tables pass accessibility checks in both themes``(dark: bool) = task {
    do! this.Load()

    if dark then
      let! _ =
        this.Page.EvaluateAsync("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let options = AxeRunOptions()

    options.Rules <-
      dict [
        "region", RuleOptions(Enabled = false)
        "landmark-one-main", RuleOptions(Enabled = false)
        "page-has-heading-one", RuleOptions(Enabled = false)
      ]
      |> System.Collections.Generic.Dictionary

    let! result = this.Page.RunAxe(options)

    let details =
      result.Violations
      |> Array.map (fun violation -> violation.Id + ": " + violation.Description)
      |> String.concat "\n"

    Assert.True(result.Violations.Length = 0, details)
  }
