namespace Weave.Tests.E2E

open Xunit

type AlertTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("alert")

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``all alert variants and close buttons pass contrast``(dark: bool) =
    this.RunAxeScan("alert", contrast = true, dark = dark)

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``close focus indicator follows alert foreground``(dark: bool) = task {
    do! this.NavigateTo("alert")

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

    do! this.Page.Keyboard.PressAsync("Tab")

    for color in [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ] do
      for variant in [ "standard"; "outlined"; "filled" ] do
        let alert = this.Page.GetByTestId("alert-" + color + "-" + variant)
        let close = alert.GetByRole(Microsoft.Playwright.AriaRole.Button)
        do! close.FocusAsync()

        let! styles =
          close.EvaluateAsync<string[]>(
            """el => {
          const button = getComputedStyle(el), alert = getComputedStyle(el.closest('.weave-alert'));
          return [button.color, alert.color, button.outlineColor, button.outlineStyle];
        }"""
          )

        Assert.Equal(styles.[1], styles.[0])
        Assert.Equal(styles.[1], styles.[2])
        Assert.NotEqual<string>("none", styles.[3])
  }
