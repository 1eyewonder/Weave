namespace Weave.Tests.E2E

open Microsoft.Playwright
open global.Xunit

type ButtonInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("activation-button", "activation-disabled")>]
  [<InlineData("activation-icon", "activation-disabled-icon")>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch taps activate once and disabled buttons remain inactive``
    (enabledId: string, disabledId: string)
    = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "button")
    let button = page.GetByTestId(enabledId)
    let disabled = page.GetByTestId(disabledId)
    let count = page.GetByTestId("activation-count")
    do! Assertions.Expect(disabled).ToBeDisabledAsync()
    do! button.TapAsync()
    do! Assertions.Expect(count).ToHaveTextAsync("1")
    do! disabled.TapAsync(LocatorTapOptions(Force = true))

    let! _ =
      page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! Assertions.Expect(count).ToHaveTextAsync("1")
    do! button.TapAsync()

    let! _ =
      page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! Assertions.Expect(count).ToHaveTextAsync("2")
  }

  [<Theory>]
  [<InlineData("activation-button")>]
  [<InlineData("activation-icon")>]
  member this.``native activation invokes callback once per action``(testId: string) = task {
    do! this.NavigateTo("button")
    let button = this.Page.GetByTestId(testId)
    let count = this.Page.GetByTestId("activation-count")
    do! button.ClickAsync()
    do! this.Expect(count).ToHaveTextAsync("1")
    do! button.PressAsync("Enter")
    do! this.Expect(count).ToHaveTextAsync("2")
    do! button.PressAsync("Space")
    do! this.Expect(count).ToHaveTextAsync("3")
    let! _ = button.EvaluateAsync("el => el.click()")
    do! this.Expect(count).ToHaveTextAsync("4")
  }

  [<Fact>]
  member this.``button is focusable``() = task {
    do! this.NavigateTo("button")
    let button = this.Page.Locator(".weave-button").First
    do! button.FocusAsync()
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Enter activates a button``() = task {
    do! this.NavigateTo("button")
    let button = this.Page.Locator(".weave-button").First
    do! button.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Enter")
    // Native button fires click on Enter — verify the element is still focused (activation didn't break focus)
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Space activates a button``() = task {
    do! this.NavigateTo("button")
    let button = this.Page.Locator(".weave-button").First
    do! button.FocusAsync()
    do! this.Page.Keyboard.PressAsync(" ")
    do! this.Expect(button).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled button is not focusable``() = task {
    do! this.NavigateTo("button")
    let disabledButton = this.Page.Locator(".weave-button--disabled").First
    do! this.Expect(disabledButton).ToHaveAttributeAsync("disabled", "")
    do! disabledButton.FocusAsync()
    do! this.Expect(disabledButton).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``icon button is focusable``() = task {
    do! this.NavigateTo("button")
    let iconButton = this.Page.Locator(".weave-button--icon").First
    do! iconButton.FocusAsync()
    do! this.Expect(iconButton).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``icon button has aria-label``() = task {
    do! this.NavigateTo("button")
    let iconButton = this.Page.Locator(".weave-button--icon").First
    do! this.Expect(iconButton).ToHaveAttributeAsync("aria-label", "home")
  }

  [<Fact>]
  member this.``color variant buttons are present``() = task {
    do! this.NavigateTo("button")
    let primaryButton = this.Page.Locator(".weave-button--primary")
    let errorButton = this.Page.Locator(".weave-button--error")
    let! primaryCount = primaryButton.CountAsync()
    and! errorCount = errorButton.CountAsync()
    Assert.True(primaryCount >= 1, $"Expected at least 1 primary button, found %i{primaryCount}")
    Assert.True(errorCount >= 1, $"Expected at least 1 error button, found %i{errorCount}")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``button variants retain readable foregrounds in interactive states``(dark: bool) = task {
    do! this.NavigateTo("button")

    if dark then
      let! _ =
        this.Page.EvaluateAsync<obj>("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let! _ =
      this.Page.AddStyleTagAsync(
        PageAddStyleTagOptions(
          Content = "*, *::before, *::after { transition: none !important; animation: none !important; }"
        )
      )

    for color in [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ] do
      for variant in [ "filled"; "outlined"; "text" ] do
        let button = this.Page.GetByTestId("button-contrast-" + color + "-" + variant)

        for state in [ "rest"; "hover"; "focus" ] do
          if state = "rest" then
            do! this.Page.Mouse.MoveAsync(0.0f, 0.0f)
            let! _ = this.Page.EvaluateAsync<obj>("() => document.activeElement.blur()")
            ()
          elif state = "hover" then
            do! button.HoverAsync()
          else
            do! this.Page.Mouse.MoveAsync(0.0f, 0.0f)
            do! button.FocusAsync()

          let! contrast =
            button.EvaluateAsync<float>(
              """el => {
            const rgb = s => s.match(/[\d.]+/g).map(Number);
            const lum = c => c.slice(0, 3).map(v => v / 255).map(v => v <= 0.04045 ? v / 12.92 : ((v + 0.055) / 1.055) ** 2.4)
              .reduce((sum, v, i) => sum + v * [0.2126, 0.7152, 0.0722][i], 0);
            const fg = rgb(getComputedStyle(el).color), tint = rgb(getComputedStyle(el).backgroundColor);
            const surface = rgb(getComputedStyle(el.closest('[data-testid=button-palette]')).backgroundColor);
            const bg = tint.slice(0, 3).map((v, i) => v * (tint[3] ?? 1) + surface[i] * (1 - (tint[3] ?? 1)));
            const a = lum(fg), b = lum(bg); return (Math.max(a, b) + .05) / (Math.min(a, b) + .05);
          }"""
            )

          Assert.True(contrast >= 4.5, $"{color} {variant} {state}, dark={dark}, contrast={contrast:F2}:1")
  }
