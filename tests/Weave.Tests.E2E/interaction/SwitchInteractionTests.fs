namespace Weave.Tests.E2E

open Xunit

type SwitchInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``switch input is focusable``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Space toggles an off switch to on``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! input.PressAsync("Space")
    do! this.Expect(input).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Enter toggles an off switch to on``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! input.PressAsync("Enter")
    do! this.Expect(input).ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Enter toggles an on switch to off``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").Nth(1)
    do! input.FocusAsync()
    do! this.Expect(input).ToBeCheckedAsync()
    do! input.PressAsync("Enter")
    do! this.Expect(input).Not.ToBeCheckedAsync()
  }

  [<Fact>]
  member this.``Tab navigates between switch inputs``() = task {
    do! this.NavigateTo("switch")
    let firstInput = this.Page.Locator(".weave-switch__input").First
    let secondInput = this.Page.Locator(".weave-switch__input").Nth(1)
    do! firstInput.FocusAsync()
    do! this.Expect(firstInput).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(secondInput).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled switch is not focusable``() = task {
    do! this.NavigateTo("switch")
    let disabledInput = this.Page.Locator(".weave-switch__input[disabled]").First
    do! disabledInput.FocusAsync()
    do! this.Expect(disabledInput).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``disabled switch is skipped by Tab``() = task {
    do! this.NavigateTo("switch")
    let secondInput = this.Page.Locator(".weave-switch__input").Nth(1)
    do! secondInput.FocusAsync()
    // The third switch is disabled — Tab should skip it and not leave focus on it
    do! this.Page.Keyboard.PressAsync("Tab")
    let disabledInput = this.Page.Locator(".weave-switch__input[disabled]").First
    do! this.Expect(disabledInput).Not.ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``switch input has role switch``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! this.Expect(input).ToHaveAttributeAsync("role", "switch")
  }

  [<Fact>]
  member this.``focus-visible outline appears on the track when input is focused``() = task {
    do! this.NavigateTo("switch")
    let input = this.Page.Locator(".weave-switch__input").First
    do! input.FocusAsync()
    do! this.Expect(input).ToBeFocusedAsync()
    let track = this.Page.Locator(".weave-switch__track").First

    let! outlineStyle =
      track.EvaluateAsync<string>("el => getComputedStyle(el).outlineStyle")

    Assert.NotEqual<string>("none", outlineStyle)
  }

  [<Fact>]
  member this.``disabled switch retains a visible thumb in both states``() = task {
    do! this.NavigateTo("switch")
    let off = this.Page.Locator(".weave-switch--disabled").First
    let on = this.Page.GetByTestId("disabled-on-switch")
    do! this.Expect(off.Locator("input")).Not.ToBeCheckedAsync()
    do! this.Expect(on.Locator("input")).ToBeCheckedAsync()
    do! this.Expect(off.Locator(".weave-switch__thumb")).ToBeVisibleAsync()
    do! this.Expect(on.Locator(".weave-switch__thumb")).ToBeVisibleAsync()

    let! offLeft =
      off.Locator(".weave-switch__thumb").EvaluateAsync<float>("el => parseFloat(getComputedStyle(el).left)")

    let! onLeft =
      on.Locator(".weave-switch__thumb").EvaluateAsync<float>("el => parseFloat(getComputedStyle(el).left)")

    Assert.True(onLeft > offLeft, "The thumb must distinguish disabled on and off states")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``selection indicators contrast with the surrounding surface``(dark: bool) = task {
    for pageName, prefix, selector in
      [
        "radio", "radio-color-", ".weave-radio__span"
        "switch", "switch-color-", ".weave-switch__thumb"
        "slider", "slider-color-", ".weave-slider__thumb"
      ] do
      let! _ = this.Page.GotoAsync("about:blank")
      do! this.NavigateTo(pageName)

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
        let indicator = this.Page.GetByTestId(prefix + color).Locator(selector)

        let! contrast =
          indicator.EvaluateAsync<float>(
            """el => {
          const kind = el.classList.contains("weave-radio__span") ? "radio" : "background";
          const rgb = s => s.match(/[\d.]+/g).map(Number);
          const lum = c => c.slice(0,3).map(v => v/255).map(v => v <= .04045 ? v/12.92 : ((v+.055)/1.055)**2.4).reduce((s,v,i) => s+v*[.2126,.7152,.0722][i],0);
          const foreground = kind === 'radio' ? getComputedStyle(el, '::after').backgroundColor : getComputedStyle(el).backgroundColor;
          const background = kind === 'radio' ? getComputedStyle(el).backgroundColor : getComputedStyle(document.body).backgroundColor;
          const a=lum(rgb(foreground)), b=lum(rgb(background)); return (Math.max(a,b)+.05)/(Math.min(a,b)+.05);
        }"""
          )

        Assert.True(contrast >= 3.0, $"{pageName} {color}, dark={dark}, indicator contrast={contrast:F2}:1")
  }

  [<Fact>]
  member this.``selection marks remain visible in forced colors``() = task {
    do!
      this.Page.EmulateMediaAsync(
        Microsoft.Playwright.PageEmulateMediaOptions(ForcedColors = Microsoft.Playwright.ForcedColors.Active)
      )

    do! this.NavigateTo("radio")

    let radio =
      this.Page.GetByTestId("radio-color-primary").Locator(".weave-radio__span")

    let! radioColors =
      radio.EvaluateAsync<string[]>(
        "el => [getComputedStyle(el).backgroundColor, getComputedStyle(el, '::after').backgroundColor]"
      )

    Assert.NotEqual<string>(radioColors.[0], radioColors.[1])
    let! _ = this.Page.GotoAsync("about:blank")
    do! this.NavigateTo("switch")
    let switchRoot = this.Page.GetByTestId("switch-color-primary")

    let! thumb =
      switchRoot
        .Locator(".weave-switch__thumb")
        .EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    let! track =
      switchRoot
        .Locator(".weave-switch__track")
        .EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    Assert.NotEqual<string>(track, thumb)
    let! _ = this.Page.GotoAsync("about:blank")
    do! this.NavigateTo("checkbox")

    let checkbox =
      this.Page.GetByTestId("checkbox-color-primary").Locator(".weave-checkbox__span")

    let! checkboxColors =
      checkbox.EvaluateAsync<string[]>(
        "el => [getComputedStyle(el).backgroundColor, getComputedStyle(el, '::after').borderBottomColor]"
      )

    Assert.NotEqual<string>(checkboxColors.[0], checkboxColors.[1])
    let! _ = this.Page.GotoAsync("about:blank")
    do! this.NavigateTo("slider")

    let slider =
      this.Page.GetByTestId("slider-color-primary").Locator(".weave-slider__thumb")

    let! sliderColors =
      slider.EvaluateAsync<string[]>(
        "el => [getComputedStyle(document.body).backgroundColor, getComputedStyle(el).backgroundColor]"
      )

    Assert.NotEqual<string>(sliderColors.[0], sliderColors.[1])
  }
