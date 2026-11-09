namespace Weave.Tests.E2E

open Xunit

type ChipInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``closing a chip does not activate its parent and respects disabled state``() = task {
    do! this.NavigateTo("chip")
    let root = this.Page.GetByTestId("chip-both")
    let close = root.Locator(".weave-chip__close")
    let actions = this.Page.GetByTestId("chip-actions")
    let closes = this.Page.GetByTestId("chip-closes")
    do! close.ClickAsync()
    do! this.Expect(closes).ToHaveTextAsync("1")
    do! this.Expect(actions).ToHaveTextAsync("0")
    do! close.PressAsync("Enter")
    do! this.Expect(closes).ToHaveTextAsync("2")
    do! this.Expect(actions).ToHaveTextAsync("0")

    let disabled =
      this.Page.GetByTestId("chip-disabled-close").Locator(".weave-chip__close")

    do! this.Expect(disabled).ToHaveAttributeAsync("tabindex", "-1")

    let! _ =
      disabled.EvaluateAsync(
        "el => { el.click(); el.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true })); }"
      )

    do! this.Expect(closes).ToHaveTextAsync("2")
    do! root.Locator(".weave-chip__action").ClickAsync()
    do! this.Expect(actions).ToHaveTextAsync("1")
  }

  [<Fact>]
  member this.``chip link supports native click and keyboard activation``() = task {
    do! this.NavigateTo("chip")
    let link = this.Page.GetByTestId("chip-link")
    let! _ = link.EvaluateAsync("el => el.click()")
    do! this.Expect(this.Page.GetByTestId("chip-actions")).ToHaveTextAsync("1")
    do! link.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("chip-actions")).ToHaveTextAsync("2")
  }

  [<Fact>]
  member this.``closing a link chip does not navigate or activate its link``() = task {
    do! this.NavigateTo("chip")
    let root = this.Page.GetByTestId("closable-chip-link")
    do! root.Locator(".weave-chip__close").ClickAsync()
    do! this.Expect(this.Page.GetByTestId("chip-closes")).ToHaveTextAsync("1")
    do! this.Expect(this.Page.GetByTestId("chip-actions")).ToHaveTextAsync("0")
    Assert.EndsWith("#chip", this.Page.Url)
  }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap activates a chip once``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "chip")
    do! page.GetByTestId("chip-both").Locator(".weave-chip__action").TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("chip-actions")).ToHaveTextAsync("1")
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("chip-closes")).ToHaveTextAsync("0")
    do! page.GetByTestId("chip-both").Locator(".weave-chip__close").TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("chip-actions")).ToHaveTextAsync("1")
    do! Microsoft.Playwright.Assertions.Expect(page.GetByTestId("chip-closes")).ToHaveTextAsync("1")
  }

  [<Fact>]
  member this.``clickable chip is focusable``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! this.Expect(chip).ToHaveAttributeAsync("tabindex", "0")
    do! chip.FocusAsync()
    do! this.Expect(chip).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``clickable chip has role button``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! this.Expect(chip).ToHaveAttributeAsync("role", "button")
  }

  [<Fact>]
  member this.``Enter activates a clickable chip``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! chip.FocusAsync()
    do! chip.PressAsync("Enter")
    let status = this.Page.Locator("[data-testid='chip-clicked']")
    do! this.Expect(status).ToHaveTextAsync("clicked")
  }

  [<Fact>]
  member this.``Space activates a clickable chip``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='clickable-chip']")
    do! chip.FocusAsync()
    do! chip.PressAsync(" ")
    let status = this.Page.Locator("[data-testid='chip-clicked']")
    do! this.Expect(status).ToHaveTextAsync("clicked")
  }

  [<Fact>]
  member this.``disabled clickable chip has tabindex -1``() = task {
    do! this.NavigateTo("chip")
    let chip = this.Page.Locator("[data-testid='disabled-chip']")
    do! this.Expect(chip).ToHaveAttributeAsync("tabindex", "-1")
  }

  [<Fact>]
  member this.``close button is focusable and has aria-label``() = task {
    do! this.NavigateTo("chip")
    let close = this.Page.Locator(".weave-chip__close").First
    do! this.Expect(close).ToHaveAttributeAsync("tabindex", "0")
    do! close.FocusAsync()
    do! this.Expect(close).ToBeFocusedAsync()
    do! this.Expect(close).ToHaveAttributeAsync("aria-label", "remove")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``chip labels contrast across variants and interaction states``(dark: bool) = task {
    do! this.NavigateTo("chip")

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

    for color in [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ] do
      for variant in [ "filled"; "outlined"; "text" ] do
        let chip = this.Page.GetByTestId("chip-color-" + color + "-" + variant)

        for state in [ "rest"; "hover"; "focus" ] do
          do! this.Page.Mouse.MoveAsync(0.0f, 0.0f)
          let! _ = this.Page.EvaluateAsync<obj>("() => document.activeElement.blur()")

          if state = "hover" then
            do! chip.HoverAsync()
          elif state = "focus" then
            do! chip.FocusAsync()

          let! contrast =
            chip.EvaluateAsync<float>(
              """el => {
            const rgb = s => s.match(/[\d.]+/g).map(Number);
            const lum = c => c.slice(0,3).map(v => v/255).map(v => v <= .04045 ? v/12.92 : ((v+.055)/1.055)**2.4).reduce((s,v,i) => s+v*[.2126,.7152,.0722][i],0);
            const fg = rgb(getComputedStyle(el).color), tint = rgb(getComputedStyle(el).backgroundColor), surface = rgb(getComputedStyle(document.body).backgroundColor);
            const bg = tint.slice(0,3).map((v,i) => v*(tint[3] ?? 1)+surface[i]*(1-(tint[3] ?? 1)));
            const a=lum(fg), b=lum(bg); return (Math.max(a,b)+.05)/(Math.min(a,b)+.05);
          }"""
            )

          Assert.True(contrast >= 4.5, $"{color} {variant} {state}, dark={dark}, contrast={contrast:F2}:1")
  }
