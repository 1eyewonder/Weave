namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type AnimationAccessibilityTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("animation")

[<Collection("E2E")>]
type AnimationClassTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``fade-in-static has weave-animation--fade-in class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='fade-in-static']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation--fade-in"))
  }

  [<Fact>]
  member this.``bounce-static has weave-animation--bounce class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='bounce-static']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation--bounce"))
  }

  [<Fact>]
  member this.``pulse-hover has weave-animation--pulse class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='pulse-hover']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation--pulse"))
  }

  [<Fact>]
  member this.``pulse-hover has weave-animation-on--hover class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='pulse-hover']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation-on--hover"))
  }

[<Collection("E2E")>]
type AnimationComputedStyleTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``bounce animation uses linear timing function``() = task {
    do! this.NavigateTo("animation")

    let! easing =
      this.Page.EvaluateAsync<string>(
        """() => {
          const el = document.querySelector('[data-testid="bounce-static"]')
          return getComputedStyle(el).animationTimingFunction
        }"""
      )

    Assert.Equal("linear", easing)
  }

  [<Fact>]
  member this.``fade-in-static has animation-name weave-fade-in``() = task {
    do! this.NavigateTo("animation")

    let! name =
      this.Page.EvaluateAsync<string>(
        """() => getComputedStyle(document.querySelector('[data-testid="fade-in-static"]')).animationName"""
      )

    Assert.Equal("weave-fade-in", name)
  }

[<Collection("E2E")>]
type AnimationSuppressTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``suppress-container has weave-animation--none class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='suppress-container']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation--none"))
  }

  [<Fact>]
  member this.``suppress-child animation-duration is 0s``() = task {
    do! this.NavigateTo("animation")

    let! duration =
      this.Page.EvaluateAsync<string>(
        """() => getComputedStyle(document.querySelector('[data-testid="suppress-child"]')).animationDuration"""
      )

    Assert.Equal("0s", duration)
  }

[<Collection("E2E")>]
type AnimationShowTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``show-wrapper is absent from DOM when isVisible is false``() = task {
    do! this.NavigateTo("animation")
    let wrapper = this.Page.Locator("[data-testid='show-wrapper']")
    do! this.Expect(wrapper).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``show-wrapper appears after clicking show-toggle-btn``() = task {
    do! this.NavigateTo("animation")
    do! this.Page.Locator("[data-testid='show-toggle-btn']").ClickAsync()
    let wrapper = this.Page.Locator("[data-testid='show-wrapper']")
    do! this.Expect(wrapper).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``show-wrapper has weave-animation--fade-in class after show``() = task {
    do! this.NavigateTo("animation")
    do! this.Page.Locator("[data-testid='show-toggle-btn']").ClickAsync()
    let wrapper = this.Page.Locator("[data-testid='show-wrapper']")
    do! this.Expect(wrapper).ToHaveClassAsync(Regex("weave-animation--fade-in"))
  }

  [<Fact>]
  member this.``show-wrapper is removed from DOM after hiding``() = task {
    do! this.NavigateTo("animation")
    let btn = this.Page.Locator("[data-testid='show-toggle-btn']")
    do! btn.ClickAsync()
    let wrapper = this.Page.Locator("[data-testid='show-wrapper']")
    do! this.Expect(wrapper).ToBeVisibleAsync()
    do! btn.ClickAsync()
    do! this.Expect(wrapper).ToHaveCountAsync(0)
  }

[<Collection("E2E")>]
type AnimationToggleClassTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``toggle-target has no animation class on initial render``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='toggle-target']")
    do! this.Expect(el).Not.ToHaveClassAsync(Regex("weave-animation--fade-in"))
    do! this.Expect(el).Not.ToHaveClassAsync(Regex("weave-animation--fade-out"))
  }

  [<Fact>]
  member this.``toggle-target has fade-in class after first click``() = task {
    do! this.NavigateTo("animation")
    do! this.Page.Locator("[data-testid='toggle-btn']").ClickAsync()
    let el = this.Page.Locator("[data-testid='toggle-target']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation--fade-in"))
  }

  [<Fact>]
  member this.``toggle-target has fade-out class and not fade-in after second click``() = task {
    do! this.NavigateTo("animation")
    let btn = this.Page.Locator("[data-testid='toggle-btn']")
    do! btn.ClickAsync()
    do! btn.ClickAsync()
    let el = this.Page.Locator("[data-testid='toggle-target']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation--fade-out"))
    do! this.Expect(el).Not.ToHaveClassAsync(Regex("weave-animation--fade-in"))
  }

[<Collection("E2E")>]
type AnimationReducedMotionTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  let emulateReducedMotion (page: Microsoft.Playwright.IPage) =
    page.EmulateMediaAsync(
      Microsoft.Playwright.PageEmulateMediaOptions(ReducedMotion = Microsoft.Playwright.ReducedMotion.Reduce)
    )

  [<Fact>]
  member this.``prefers-reduced-motion sets --weave-animation-duration-standard to 0s``() = task {
    do! this.NavigateTo("animation")
    do! emulateReducedMotion this.Page

    let! value =
      this.Page.EvaluateAsync<string>(
        """() => getComputedStyle(document.documentElement).getPropertyValue('--weave-animation-duration-standard').trim()"""
      )

    Assert.Equal("0s", value)
  }

  [<Fact>]
  member this.``prefers-reduced-motion sets --weave-animation-distance-lg to 0px``() = task {
    do! this.NavigateTo("animation")
    do! emulateReducedMotion this.Page

    let! value =
      this.Page.EvaluateAsync<string>(
        """() => getComputedStyle(document.documentElement).getPropertyValue('--weave-animation-distance-lg').trim()"""
      )

    Assert.Equal("0px", value)
  }

  [<Fact>]
  member this.``prefers-reduced-motion sets --weave-stagger-delay to 0ms``() = task {
    do! this.NavigateTo("animation")
    do! emulateReducedMotion this.Page

    let! value =
      this.Page.EvaluateAsync<string>(
        """() => getComputedStyle(document.documentElement).getPropertyValue('--weave-stagger-delay').trim()"""
      )

    Assert.Equal("0ms", value)
  }

[<Collection("E2E")>]
type AnimationStaggerTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``stagger-1 has weave-animation-delay--1 class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='stagger-1']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation-delay--1"))
  }

  [<Fact>]
  member this.``stagger-2 has weave-animation-delay--2 class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='stagger-2']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation-delay--2"))
  }

  [<Fact>]
  member this.``stagger-3 has weave-animation-delay--3 class``() = task {
    do! this.NavigateTo("animation")
    let el = this.Page.Locator("[data-testid='stagger-3']")
    do! this.Expect(el).ToHaveClassAsync(Regex("weave-animation-delay--3"))
  }
