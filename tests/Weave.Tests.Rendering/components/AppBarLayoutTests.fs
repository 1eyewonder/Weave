module Weave.Tests.Rendering.AppBarLayoutTests

open Xunit

type AppBarLayoutTests() =
  inherit LayoutTestBase("appbar")

  [<Fact>]
  member this.``static appbar spans the full viewport width``() = task {
    do! this.LoadFixture()
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()
    and! vp = this.Page.EvaluateAsync<int>("() => window.innerWidth")

    Assert.True(
      abs (bar.Width - float32 vp) <= 1.0f,
      $"AppBar width {bar.Width}px should match viewport {vp}px"
    )
  }

  [<Fact>]
  member this.``static appbar meets minimum height``() = task {
    do! this.LoadFixture()
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()

    Assert.True(bar.Height >= 64.0f, $"AppBar height {bar.Height}px should be >= 64px")
  }

  [<Fact>]
  member this.``content after static appbar is placed below it``() = task {
    do! this.LoadFixture()
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()
    and! content = this.Page.Locator("#content-after-static").BoundingBoxAsync()

    Assert.True(
      content.Y >= bar.Y + bar.Height - 1.0f,
      $"Content (y={content.Y}) should start at or below appbar bottom ({bar.Y + bar.Height})"
    )
  }

  [<Fact>]
  member this.``fixed appbar has CSS position fixed``() = task {
    do! this.LoadFixture()

    let! position = this.ComputedStyle("#appbar-fixed", "position")

    Assert.Equal("fixed", position)
  }

  [<Theory>]
  [<InlineData(375)>]
  [<InlineData(599)>]
  [<InlineData(600)>]
  [<InlineData(960)>]
  [<InlineData(1280)>]
  member this.``static appbar spans full width at all viewport sizes``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()

    Assert.True(
      abs (bar.Width - float32 viewportWidth) <= 1.0f,
      $"AppBar width {bar.Width}px should match viewport {viewportWidth}px"
    )
  }

  [<Theory>]
  [<InlineData(375)>]
  [<InlineData(1280)>]
  member this.``appbar min height is preserved at mobile and desktop``(viewportWidth: int) = task {
    do! this.LoadFixture(viewportWidth)
    let! bar = this.Page.Locator("#appbar-static").BoundingBoxAsync()

    Assert.True(
      bar.Height >= 48.0f,
      $"AppBar height {bar.Height}px should be >= 48px at {viewportWidth}px viewport"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``appbar buttons retain readable foregrounds in every palette and state``(theme: string) = task {
    do! this.LoadFixture()
    do! this.SetTheme theme

    let! _ =
      this.Page.AddStyleTagAsync(
        Microsoft.Playwright.PageAddStyleTagOptions(
          Content = ".weave-button { transition: none !important; }"
        )
      )

    let baseline = this.Page.Locator("#button-baseline")
    let nested = this.Page.Locator("#button-in-appbar")
    let! appbarColor = this.ComputedStyle("#appbar-button-host", "color")

    for color in [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ] do
      for variant in [ "filled"; "outlined"; "text" ] do
        let classes = $"weave-button weave-button--{color} weave-button--{variant}"
        let! _ = baseline.EvaluateAsync("(el, classes) => el.className = classes", classes)
        let! _ = nested.EvaluateAsync("(el, classes) => el.className = classes", classes)

        for state in [ "rest"; "hover"; "focus" ] do
          let activate (button: Microsoft.Playwright.ILocator) = task {
            do! this.Page.Mouse.MoveAsync(0.0f, 0.0f)
            let! _ = button.EvaluateAsync("el => el.blur()")

            match state with
            | "hover" -> do! button.HoverAsync()
            | "focus" -> do! button.FocusAsync()
            | _ -> ()
          }

          do! activate baseline
          let! standalone = baseline.EvaluateAsync<string>("el => getComputedStyle(el).color")
          do! activate nested
          let! actual = nested.EvaluateAsync<string>("el => getComputedStyle(el).color")

          if variant = "filled" then
            Assert.Equal(standalone, actual)
          else
            Assert.Equal(appbarColor, actual)

          let! contrast =
            nested.EvaluateAsync<float>(
              """el => {
                const channels = value => value.match(/[\d.]+/g).map(Number);
                const blend = (base, layer) => {
                  const rgba = channels(layer);
                  const alpha = rgba.length > 3 ? rgba[3] : 1;
                  return base.map((channel, index) => rgba[index] * alpha + channel * (1 - alpha));
                };
                const surfaces = [];
                for (let node = el; node; node = node.parentElement)
                  surfaces.push(getComputedStyle(node).backgroundColor);
                const background = surfaces.reverse().reduce(blend, [255, 255, 255]);
                const foreground = channels(getComputedStyle(el).color);
                const luminance = rgb => rgb.slice(0, 3)
                  .map(channel => channel / 255)
                  .map(value => value <= .04045 ? value / 12.92 : ((value + .055) / 1.055) ** 2.4)
                  .reduce((sum, value, index) => sum + value * [.2126, .7152, .0722][index], 0);
                const light = Math.max(luminance(foreground), luminance(background));
                const dark = Math.min(luminance(foreground), luminance(background));
                return (light + .05) / (dark + .05);
              }"""
            )

          Assert.True(contrast >= 4.5, $"{theme} {color} {variant} {state}: {contrast:F2}:1, required 4.5:1")

          if variant = "outlined" then
            let! border =
              nested.EvaluateAsync<string>("el => getComputedStyle(el).borderTopColor")

            Assert.Equal(actual, border)

    let! inherited = this.ComputedStyle("#button-uncolored", "color")
    Assert.Equal(appbarColor, inherited)
    let! disabled = this.ComputedStyle("#button-disabled", "color")
    let! enabled = this.ComputedStyle("#button-in-appbar", "color")
    Assert.NotEqual<string>(enabled, disabled)
  }
