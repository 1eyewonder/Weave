namespace Weave.Tests.E2E

open Xunit

[<Collection("Browser input")>]
type TabsTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("tabs")

  [<Theory>]
  [<InlineData("light", 320)>]
  [<InlineData("dark", 320)>]
  [<InlineData("light", 1280)>]
  [<InlineData("dark", 1280)>]
  member this.``named filled trays preserve text contrast and selected boundary``(theme: string, width: int) =
    task {
      do! this.Page.SetViewportSizeAsync(width, 900)
      do! this.NavigatePage(this.Page, "tabs-contrast")

      let! _ =
        this.Page.EvaluateAsync("theme => document.body.dataset.theme = theme", theme)

      let! _ =
        this.Page.AddStyleTagAsync(
          Microsoft.Playwright.PageAddStyleTagOptions(
            Content = ".weave-tabs * { transition: none !important; }"
          )
        )

      for color in [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ] do
        let tabs = this.Page.GetByTestId("tabs-tray-" + color)

        for tabIndex in [ 0; 1 ] do
          for state in [ "rest"; "hover"; "focus" ] do
            let target = tabs.GetByRole(Microsoft.Playwright.AriaRole.Tab).Nth(tabIndex)
            do! this.Page.Mouse.MoveAsync(float32 (width - 1), 899.0f)
            let! _ = this.Page.EvaluateAsync("() => document.activeElement?.blur()")

            match state with
            | "hover" -> do! target.HoverAsync()
            | "focus" -> do! target.FocusAsync()
            | _ -> ()

            let! failures =
              tabs.EvaluateAsync<string[]>(
                """root => {
                const rgb = value => value.match(/[\d.]+/g).map(Number);
                const over = (front, back) => front.slice(0, 3)
                  .map((value, i) => value * (front[3] ?? 1) + back[i] * (1 - (front[3] ?? 1)));
                const luminance = channels => channels.slice(0, 3)
                  .map(channel => channel / 255)
                  .map(value => value <= .04045 ? value / 12.92 : ((value + .055) / 1.055) ** 2.4)
                  .reduce((sum, value, i) => sum + value * [.2126, .7152, .0722][i], 0);
                const ratio = (a, b) => (Math.max(luminance(a), luminance(b)) + .05)
                  / (Math.min(luminance(a), luminance(b)) + .05);
                const wrapper = root.querySelector('.weave-tabs__header-wrapper');
                const tray = rgb(getComputedStyle(wrapper).backgroundColor);
                const indicator = getComputedStyle(root.querySelector('.weave-tabs__indicator'));
                const failures = [];
                for (const control of wrapper.querySelectorAll('button')) {
                  if (!control.getBoundingClientRect().width || control.disabled) continue;
                  const style = getComputedStyle(control);
                  const under = control.getAttribute('aria-selected') === 'true'
                    ? rgb(indicator.backgroundColor) : tray;
                  const surface = over(rgb(style.backgroundColor), under);
                  const contrast = ratio(over(rgb(style.color), surface), surface);
                  if (contrast < 4.5) failures.push(`${control.textContent}: ${contrast.toFixed(2)}:1`);
                }
                const shadow = indicator.boxShadow.match(/rgba?\([^)]+\)/);
                if (!shadow || ratio(rgb(shadow[0]), tray) < 3)
                  failures.push('Selected pill needs a boundary of at least 3:1 against the tray.');
                return failures;
              }"""
              )

            let details = System.String.Join("; ", failures)
            Assert.True((failures.Length = 0), $"{theme} {width}px {color} tab{tabIndex} {state}: {details}")
    }

  [<Fact>]
  member this.``decorative tab icons preserve the text label as its name``() = task {
    do! this.NavigateTo("tabs-contrast")

    let named =
      this.Page.GetByRole(
        Microsoft.Playwright.AriaRole.Tab,
        Microsoft.Playwright.PageGetByRoleOptions(Name = "First tab", Exact = true)
      )

    do! this.Expect(named).ToHaveCountAsync(7)
  }
