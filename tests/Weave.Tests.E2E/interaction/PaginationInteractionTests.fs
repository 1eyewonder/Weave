namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type PaginationInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button name =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.Current = this.Page.Locator("#pager [aria-current='page']")

  [<Fact>]
  member this.``navigation stops at both ends and updates the binding``() = task {
    do! this.NavigateTo "pagination"
    do! this.Expect(this.Button "First page").ToBeDisabledAsync()
    do! this.Expect(this.Button "Previous page").ToBeDisabledAsync()
    do! (this.Button "Next page").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "2"
    do! this.Expect(this.Page.Locator("#page-value")).ToHaveTextAsync "2"
    do! (this.Button "Last page").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "20"
    do! this.Expect(this.Button "Next page").ToBeDisabledAsync()
    do! this.Expect(this.Button "Last page").ToBeDisabledAsync()
    do! (this.Button "Previous page").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "19"
    do! (this.Button "First page").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``external selection and shrinking counts clamp only the display``() = task {
    do! this.NavigateTo "pagination"
    do! (this.Button "Select ten").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "10"
    do! (this.Button "Shrink count").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "3"
    do! this.Expect(this.Page.Locator("#page-value")).ToHaveTextAsync "10"
    do! this.Expect(this.Button "Next page").ToBeDisabledAsync()
    do! (this.Button "Previous page").ClickAsync()
    do! this.Expect(this.Page.Locator("#page-value")).ToHaveTextAsync "2"
    do! (this.Button "Select below range").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "1"
    do! this.Expect(this.Page.Locator("#page-value")).ToHaveTextAsync "-5"
    do! (this.Button "Select above range").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "3"
    do! this.Expect(this.Page.Locator("#page-value")).ToHaveTextAsync "100"
    do! (this.Button "Restore count").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "20"
  }

  [<Fact>]
  member this.``cached page buttons track selection count and enabled changes without replacement``() = task {
    do! this.NavigateTo "pagination"
    let first = this.Button "Go to page 1"
    let third = this.Button "Go to page 3"
    let value = this.Page.Locator("#page-value")
    do! this.Expect(first).ToHaveAttributeAsync("aria-current", "page")
    do! this.Expect(third).ToHaveAttributeAsync("aria-current", "false")
    let! originalFirst = first.ElementHandleAsync()
    let! originalThird = third.ElementHandleAsync()

    let expectRetainedButtons () = task {
      for original in [ originalFirst; originalThird ] do
        let! retained =
          original.EvaluateAsync<bool>(
            """el => {
          const name = el.getAttribute('aria-label');
          return el.isConnected && el === document.querySelector('#pager button[aria-label="' + name + '"]');
        }"""
          )

        Assert.True(
          retained,
          "Page 1 and page 3 must reuse their original DOM buttons throughout these updates"
        )
    }

    let settleRendering () = task {
      let! _ =
        this.Page.EvaluateAsync(
          """() => new Promise(resolve =>
        requestAnimationFrame(() => requestAnimationFrame(resolve)))"""
        )

      ()
    }

    // A retained button must update selection rather than keep its initial attributes.
    do! third.ClickAsync()
    do! this.Expect(value).ToHaveTextAsync "3"
    do! this.Expect(first).ToHaveAttributeAsync("aria-current", "false")
    do! this.Expect(third).ToHaveAttributeAsync("aria-current", "page")
    do! expectRetainedButtons ()

    // Shrinking the collection must not recreate retained buttons or reset their disabled state.
    do! (this.Button "Toggle enabled").ClickAsync()
    do! this.Expect(first).ToBeDisabledAsync()
    do! this.Expect(third).ToBeDisabledAsync()
    do! first.DispatchEventAsync "click"
    do! (this.Button "Shrink count").ClickAsync()
    do! this.Expect(this.Page.Locator("#pager .weave-pagination__page")).ToHaveCountAsync 3
    do! this.Expect(value).ToHaveTextAsync "3"
    do! this.Expect(third).ToHaveAttributeAsync("aria-current", "page")
    do! this.Expect(first).ToBeDisabledAsync()
    do! this.Expect(third).ToBeDisabledAsync()
    do! expectRetainedButtons ()

    // External selection still updates cached, disabled buttons and clamps to the new count.
    do! (this.Button "Select below range").ClickAsync()
    do! this.Expect(value).ToHaveTextAsync "-5"
    do! this.Expect(first).ToHaveAttributeAsync("aria-current", "page")
    do! this.Expect(third).ToHaveAttributeAsync("aria-current", "false")
    do! (this.Button "Select above range").ClickAsync()
    do! this.Expect(value).ToHaveTextAsync "100"
    do! this.Expect(first).ToHaveAttributeAsync("aria-current", "false")
    do! this.Expect(third).ToHaveAttributeAsync("aria-current", "page")
    do! this.Expect(third).ToBeDisabledAsync()
    do! expectRetainedButtons ()

    // Re-enabled handlers must use current selection and count, not creation-time values.
    do! (this.Button "Toggle enabled").ClickAsync()
    do! this.Expect(first).ToBeEnabledAsync()
    do! this.Expect(third).ToBeEnabledAsync()
    do! third.ClickAsync()
    do! settleRendering ()
    do! this.Expect(value).ToHaveTextAsync "100"
    do! first.ClickAsync()
    do! this.Expect(value).ToHaveTextAsync "1"
    do! (this.Button "Restore count").ClickAsync()
    do! this.Expect(this.Button "Go to page 20").ToBeVisibleAsync()
    do! expectRetainedButtons ()
    do! third.ClickAsync()
    do! this.Expect(value).ToHaveTextAsync "3"
    do! this.Expect(first).ToHaveAttributeAsync("aria-current", "false")
    do! this.Expect(third).ToHaveAttributeAsync("aria-current", "page")
    do! this.Expect(this.Current).ToHaveCountAsync 1
    do! expectRetainedButtons ()
  }

  [<Theory>]
  [<InlineData("Empty count")>]
  [<InlineData("Negative count")>]
  member this.``empty counts have no current page and disable all navigation``(command: string) = task {
    do! this.NavigateTo "pagination"
    do! (this.Button command).ClickAsync()
    do! this.Expect(this.Page.Locator("#pager .weave-pagination__page")).ToHaveCountAsync 0
    do! this.Expect(this.Current).ToHaveCountAsync 0
    do! this.Expect(this.Page.Locator("#pager button:disabled")).ToHaveCountAsync 4
    do! (this.Button "Restore count").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "1"
    do! this.Expect(this.Button "Next page").ToBeEnabledAsync()
  }

  [<Fact>]
  member this.``disabling blocks native and synthetic activation then reenables``() = task {
    do! this.NavigateTo "pagination"
    do! (this.Button "Toggle enabled").ClickAsync()
    do! this.Expect(this.Page.Locator("#pager button:not(:disabled)")).ToHaveCountAsync 0
    do! (this.Button "Go to page 2").DispatchEventAsync "click"
    do! (this.Button "Next page").DispatchEventAsync "click"
    do! this.Expect(this.Page.Locator("#page-value")).ToHaveTextAsync "1"
    do! (this.Button "Toggle enabled").ClickAsync()
    do! (this.Button "Go to page 2").ClickAsync()
    do! this.Expect(this.Current).ToHaveTextAsync "2"
  }

  [<Fact>]
  member this.``keyboard activation preserves page and navigation focus as the range changes``() = task {
    do! this.NavigateTo "pagination"
    let page = this.Button "Go to page 5"
    do! page.FocusAsync()
    do! page.PressAsync "Enter"
    do! this.Expect(this.Current).ToHaveTextAsync "5"
    do! this.Expect(page).ToBeFocusedAsync()
    do! this.Expect(this.Page.Locator("#pager [aria-current='page']")).ToHaveCountAsync 1
    let next = this.Button "Next page"
    do! next.FocusAsync()
    do! next.PressAsync "Space"
    do! this.Expect(this.Current).ToHaveTextAsync "6"
    do! this.Expect(next).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("default", false, true)>]
  [<InlineData("boundaries", true, true)>]
  [<InlineData("numbers-only", false, false)>]
  [<InlineData("boundaries-only", true, false)>]
  member this.``control attrs determine accessible buttons and tab order``
    (name: string, showBoundaries: bool, showSteps: bool)
    = task {
    do! this.NavigateTo "pagination-controls"
    let pager = this.Page.Locator("#controls-" + name)
    let label = if name = "default" then "Pagination" else name + " pages"

    let navigation =
      this.Page.GetByRole(AriaRole.Navigation, PageGetByRoleOptions(Name = label, Exact = true))

    do! this.Expect(navigation).ToHaveAttributeAsync("id", "controls-" + name)

    let expected = [
      if showBoundaries then
        "First page"
      if showSteps then
        "Previous page"
      for page in 1..5 do
        sprintf "Go to page %d" page
      if showSteps then
        "Next page"
      if showBoundaries then
        "Last page"
    ]

    do! this.Expect(pager.GetByRole(AriaRole.Button)).ToHaveCountAsync expected.Length

    if not showBoundaries then
      do! this.Expect(pager.Locator(".weave-pagination__boundary").First).ToBeHiddenAsync()
      do! this.Expect(pager.Locator(".weave-pagination__boundary").Last).ToBeHiddenAsync()

    if not showSteps then
      do! this.Expect(pager.Locator(".weave-pagination__step").First).ToBeHiddenAsync()
      do! this.Expect(pager.Locator(".weave-pagination__step").Last).ToBeHiddenAsync()

    do! this.Page.Locator("#before-" + name).FocusAsync()

    for buttonName in expected do
      do! this.Page.Keyboard.PressAsync "Tab"

      do!
        this
          .Expect(pager.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = buttonName, Exact = true)))
          .ToBeFocusedAsync()

    do! this.Page.Keyboard.PressAsync "Tab"
    do! this.Expect(this.Page.Locator("#after-" + name)).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("single", "1")>]
  [<InlineData("small", "1,2,3")>]
  [<InlineData("seven", "1,2,3,4,5,6,7")>]
  [<InlineData("start", "1,2,3,4,5,...,20")>]
  [<InlineData("middle", "1,...,9,10,11,...,20")>]
  [<InlineData("end", "1,...,16,17,18,19,20")>]
  [<InlineData("single-gap", "1,2,3,4,5,...,8")>]
  [<InlineData("custom", "1,2,...,8,9,10,11,12,...,19,20")>]
  [<InlineData("zero-window", "...,10,...")>]
  [<InlineData("negative-window", "...,10,...")>]
  [<InlineData("overlap", "1,2,3")>]
  [<InlineData("huge", "1,...,2147483643,2147483644,2147483645,2147483646,2147483647")>]
  member this.``ranges contain boundary and neighbor pages with single gaps filled``
    (name: string, expected: string)
    = task {
    do! this.NavigateTo "pagination-ranges"
    let pager = this.Page.Locator("#range-" + name)

    let! labels =
      pager.Locator(".weave-pagination__page, .weave-pagination__ellipsis").AllTextContentsAsync()

    let actual =
      labels
      |> Seq.map (fun value -> value.Replace("\u2026", "..."))
      |> String.concat ","

    Assert.Equal(expected, actual)
    do! this.Expect(pager.Locator(".weave-pagination__control:visible")).ToHaveCountAsync 0
    do! this.Expect(pager.Locator("[aria-current='page']")).ToHaveCountAsync 1
    do! this.Expect(pager.Locator(".weave-pagination__ellipsis:not([aria-hidden='true'])")).ToHaveCountAsync 0
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``buttons fit narrow screens and share the same height``(width: int) = task {
    do! this.NavigateTo "pagination"
    do! this.Page.SetViewportSizeAsync(width, 800)

    let! iconsLoaded =
      this.Page.EvaluateAsync<bool>(
        """async () => {
          const faces = await document.fonts.load('20px "Material Symbols Outlined"',
            'first_page chevron_left chevron_right last_page');
          return faces.length > 0 && faces.every(face => face.status === 'loaded');
        }"""
      )

    Assert.True(iconsLoaded, "The locally served icon font must load before measuring the controls")
    let! pager = this.Page.Locator("#pager").BoundingBoxAsync()
    let! buttons = this.Page.Locator("#pager button:visible").AllAsync()
    let! first = buttons[0].BoundingBoxAsync()

    for button in buttons do
      let! box = button.BoundingBoxAsync()
      Assert.True(box.X >= pager.X && box.X + box.Width <= pager.X + pager.Width + 1.0f)
      Assert.Equal(first.Height, box.Height)

    Assert.True(pager.X + pager.Width <= float32 width)
    let! icons = this.Page.Locator("#pager .weave-pagination__icon:visible").AllAsync()

    for icon in icons do
      let! glyph = icon.BoundingBoxAsync()
      let! control = icon.Locator("..").BoundingBoxAsync()
      Assert.InRange(abs ((glyph.X + glyph.Width / 2.0f) - (control.X + control.Width / 2.0f)), 0.0f, 0.5f)
      Assert.InRange(abs ((glyph.Y + glyph.Height / 2.0f) - (control.Y + control.Height / 2.0f)), 0.0f, 0.5f)

      let! advance =
        icon.EvaluateAsync<float>(
          """el => {
        const range = document.createRange();
        range.selectNodeContents(el);
        return range.getBoundingClientRect().width;
      }"""
        )

      Assert.InRange(advance, 19.0, 21.0)
      do! this.Expect(icon).ToHaveAttributeAsync("aria-hidden", "true")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``variants pass accessibility and contrast scans in both themes``(dark: bool) =
    this.RunAxeScan("pagination-ranges", contrast = true, dark = dark)

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``wrapped pagination rows remain centered with every control contained``(width: int) = task {
    do! this.NavigateTo "pagination"
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! (this.Button "Select ten").ClickAsync()

    let! geometry =
      this.Page
        .Locator("#pager")
        .EvaluateAsync<float[][]>(
          """el => {
          const root = el.getBoundingClientRect();
          return [[root.x, root.width], ...Array.from(el.children)
            .filter(e => e.getBoundingClientRect().width > 0)
            .map(e => { const r = e.getBoundingClientRect(); return [r.x, r.width, r.y]; })];
        }"""
        )

    let root = geometry.[0]

    let rows =
      geometry.[1..]
      |> Array.groupBy (fun values -> int (System.Math.Round values.[2]))

    Assert.True(
      rows.Length = (if width = 320 then 2 else 1),
      "Expected narrow wrapping and a single desktop row"
    )

    for _, cells in rows do
      let first = cells |> Array.minBy (fun values -> values.[0])
      let last = cells |> Array.maxBy (fun values -> values.[0] + values.[1])
      let midpoint = (first.[0] + last.[0] + last.[1]) / 2.0
      Assert.True(abs (midpoint - root.[0] - root.[1] / 2.0) <= 1.0, "Each line should be centered")
      Assert.True(first.[0] >= root.[0] - 1.0 && last.[0] + last.[1] <= root.[0] + root.[1] + 1.0)
  }
