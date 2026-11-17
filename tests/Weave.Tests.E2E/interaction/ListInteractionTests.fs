namespace Weave.Tests.E2E

open Xunit
open System.Text.RegularExpressions

type ListInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``parent checkbox follows availability of all leaves``() = task {
    do! this.NavigateTo("list")
    let group = this.Page.GetByTestId("bulk-list")
    let row = group.Locator(".weave-list-item").First
    let checkbox = row.Locator(".weave-list-item__checkbox")
    let toggle = group.GetByText("Toggle department availability")
    do! this.Expect(checkbox).ToHaveCountAsync(1)
    do! toggle.ClickAsync()
    do! this.Expect(checkbox).ToHaveCountAsync(0)
    do! row.ClickAsync()
    do! this.Expect(group.Locator(".weave-list-nested")).ToHaveCountAsync(0)
    do! toggle.ClickAsync()
    do! this.Expect(checkbox).ToHaveCountAsync(1)
    do! checkbox.ClickAsync()
    do! this.Expect(this.Page.GetByTestId("bulk-selection")).ToHaveTextAsync("archived,sales,support")
    do! toggle.ClickAsync()
    do! this.Expect(checkbox).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``parent selection follows enabled leaves and preserves a disabled selection``() = task {
    do! this.NavigateTo("list")
    let group = this.Page.GetByTestId("bulk-list")
    let parent = group.Locator(".weave-list-item__checkbox").First
    let selection = this.Page.GetByTestId("bulk-selection")
    do! parent.ClickAsync()
    do! this.Expect(selection).ToHaveTextAsync("archived,sales,support")
    do! this.Expect(parent).ToHaveClassAsync(Regex("checkbox--checked"))
    do! parent.ClickAsync()
    do! this.Expect(selection).ToHaveTextAsync("archived")
    do! this.Expect(parent).Not.ToHaveClassAsync(Regex("checkbox--indeterminate|checkbox--checked"))
    do! group.GetByText("Sales", Microsoft.Playwright.LocatorGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(parent).ToHaveClassAsync(Regex("checkbox--indeterminate"))
    do! group.GetByText("Toggle archived availability").ClickAsync()
    do! parent.ClickAsync()
    do! parent.ClickAsync()
    do! this.Expect(selection).ToHaveTextAsync("")
    do! group.GetByText("Toggle archived availability").ClickAsync()
    do! parent.ClickAsync()
    do! this.Expect(selection).ToHaveTextAsync("sales,support")
  }

  [<Fact>]
  member this.``the parent indicator ignores its own value and informational rows have no checkbox``() = task {
    do! this.NavigateTo("list")
    let group = this.Page.GetByTestId("bulk-list")
    do! group.GetByText("Select group value").ClickAsync()
    let parent = group.Locator(".weave-list-item__checkbox").First
    do! this.Expect(parent).Not.ToHaveClassAsync(Regex("checkbox--indeterminate|checkbox--checked"))
    do! this.Expect(group.Locator(".weave-list-item__checkbox")).ToHaveCountAsync(4)
    do! group.GetByText("More information").ClickAsync()
    do! this.Expect(group.GetByText("Read only")).ToBeVisibleAsync()
    do! this.Expect(this.Page.GetByTestId("bulk-selection")).ToHaveTextAsync("group")
  }

  [<Fact>]
  member this.``list click selects and nested checkbox does not expand group``() = task {
    do! this.NavigateTo("list")
    let row = this.Page.Locator("[aria-label='Test items'] .weave-list-item").First
    let! _ = row.EvaluateAsync("el => el.click()")
    do! this.Expect(this.Page.GetByTestId("list-selection")).ToHaveTextAsync("1")
    let group = this.Page.GetByTestId("nested-list")

    let! _ =
      group.Locator(".weave-list-item__checkbox").First.EvaluateAsync("el => el.click()")

    do! this.Expect(this.Page.GetByTestId("nested-selected")).ToHaveTextAsync("a,b")
    do! this.Expect(group.Locator(".weave-list-nested")).ToHaveCountAsync(0)
  }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap selects a list item``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "list")
    do! page.Locator("[aria-label='Test items'] .weave-list-item").First.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("list-selection")).ToHaveTextAsync("1")
    let group = page.GetByTestId("nested-list")
    do! group.Locator(".weave-list-item__checkbox").First.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("nested-selected")).ToHaveTextAsync("a,b")
    do! Microsoft.Playwright.Assertions.Expect(group.Locator(".weave-list-nested")).ToHaveCountAsync(0)
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``selected list text remains readable across colors``(dark: bool) = task {
    do! this.NavigateTo("list")

    if dark then
      let! _ =
        this.Page.EvaluateAsync<obj>("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let! _ =
      this.Page.AddStyleTagAsync(
        Microsoft.Playwright.PageAddStyleTagOptions(
          Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
        )
      )

    let rows =
      this.Page.GetByTestId("selected-palette").Locator(".weave-list-item--selected")

    do! this.Expect(rows).ToHaveCountAsync(7)

    for i in 0..6 do
      let row = rows.Nth(i)

      for hover in [ false; true ] do
        if hover then
          do! row.HoverAsync()

        let! contrasts =
          row.EvaluateAsync<float[]>(
            """el => {
          const rgb = s => s.match(/[\d.]+/g).map(Number);
          const lum = c => c.slice(0, 3).map(v => v / 255).map(v => v <= 0.04045 ? v / 12.92 : ((v + 0.055) / 1.055) ** 2.4)
            .reduce((sum, v, i) => sum + v * [0.2126, 0.7152, 0.0722][i], 0);
          const fg = rgb(getComputedStyle(el).color);
          const secondary = rgb(getComputedStyle(el.querySelector('.weave-list-item__secondary-content')).color);
          const tint = rgb(getComputedStyle(el).backgroundColor);
          const surface = rgb(getComputedStyle(el.closest('.weave-list')).backgroundColor);
          const bg = tint.slice(0, 3).map((v, i) => v * (tint[3] ?? 1) + surface[i] * (1 - (tint[3] ?? 1)));
          const ratio = c => { const a = lum(c), b = lum(bg); return (Math.max(a,b) + .05) / (Math.min(a,b) + .05); };
          return [ratio(fg), ratio(secondary)];
        }"""
          )

        Assert.All(
          contrasts,
          fun c -> Assert.True(c >= 4.5, $"Selected row {i}, hover={hover}, dark={dark}, contrast={c:F2}:1")
        )
  }
