namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type ScrollToTopInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``page threshold is inclusive and hidden buttons are inaccessible``() = task {
    do! this.NavigateTo("scroll-to-top")
    let button = this.Page.GetByTestId("scroll-page")
    do! this.Expect(button).ToBeHiddenAsync()
    do! this.Expect(button).ToBeDisabledAsync()
    do! this.Expect(button).ToHaveAttributeAsync("tabindex", "-1")
    do! this.Expect(button).ToHaveAttributeAsync("aria-hidden", "true")
    let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 299)")
    do! this.Expect(button).ToBeHiddenAsync()
    let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 300)")
    do! this.Expect(button).ToBeVisibleAsync()
    do! this.Expect(button).ToBeEnabledAsync()
    do! this.Expect(button).ToHaveAttributeAsync("aria-label", "Back to top")
    do! this.Expect(button).ToHaveAttributeAsync("aria-hidden", "false")
    let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 299)")
    do! this.Expect(button).ToBeHiddenAsync()
  }

  [<Theory>]
  [<InlineData("Enter")>]
  [<InlineData(" ")>]
  member this.``keyboard activation smoothly returns page to top``(key: string) = task {
    do! this.NavigateTo("scroll-to-top")
    let button = this.Page.GetByTestId("scroll-page")
    let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 650)")
    do! this.Expect(button).ToBeVisibleAsync()
    do! button.FocusAsync()
    do! button.PressAsync(key)
    let! _ = this.Page.WaitForFunctionAsync("() => window.scrollY === 0")
    do! this.Expect(button).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``container scrolling is independent and preserves horizontal position``() = task {
    do! this.NavigateTo("scroll-to-top")
    let panel = this.Page.Locator("#scroll-target")
    let button = this.Page.GetByTestId("scroll-panel")
    let! _ = panel.EvaluateAsync("el => { el.scrollTop = 79; el.scrollLeft = 40; }")
    do! this.Expect(button).ToBeHiddenAsync()
    let! _ = panel.EvaluateAsync("el => el.scrollTop = 80")
    do! this.Expect(button).ToBeVisibleAsync()
    do! this.Expect(this.Page.GetByTestId("scroll-page")).ToBeHiddenAsync()
    do! button.ClickAsync()

    let! _ =
      this.Page.WaitForFunctionAsync("() => document.querySelector('#scroll-target').scrollTop === 0")

    let! horizontal = panel.EvaluateAsync<int>("el => el.scrollLeft")
    Assert.Equal(40, horizontal)
    do! this.Expect(button).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``attributes override the accessible name and scroll behavior``() = task {
    do! this.NavigateTo("scroll-to-top")
    let panel = this.Page.Locator("#scroll-target")
    let button = this.Page.GetByTestId("scroll-panel")

    let! _ =
      panel.EvaluateAsync(
        """el => {
      const original = el.scrollTo.bind(el);
      el.scrollTo = options => { window.lastScrollBehavior = options.behavior; original(options); };
      el.scrollTop = 450;
    }"""
      )

    do! this.Expect(button).ToBeVisibleAsync()
    do! this.Expect(button).ToHaveAttributeAsync("aria-label", "Back to panel top")
    do! button.ClickAsync()

    let! _ =
      this.Page.WaitForFunctionAsync("() => document.querySelector('#scroll-target').scrollTop === 0")

    let! instant = this.Page.EvaluateAsync<string>("() => window.lastScrollBehavior")
    Assert.Equal("instant", instant)

    // Removing the attribute restores the default on the same mounted component.
    let! _ =
      button.EvaluateAsync("el => el.removeAttribute('data-weave-scroll-behavior')")

    let! _ = panel.EvaluateAsync("el => el.scrollTop = 450")
    do! this.Expect(button).ToBeVisibleAsync()
    do! button.ClickAsync()

    let! _ =
      this.Page.WaitForFunctionAsync("() => document.querySelector('#scroll-target').scrollTop === 0")

    let! smooth = this.Page.EvaluateAsync<string>("() => window.lastScrollBehavior")
    Assert.Equal("smooth", smooth)
  }

  [<Fact>]
  member this.``already scrolled container initializes visible``() = task {
    do! this.NavigateTo("scroll-to-top-initial")
    do! this.Expect(this.Page.GetByTestId("scroll-panel")).ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData("scroll-to-top-zero")>]
  [<InlineData("scroll-to-top-negative")>]
  member this.``zero and negative thresholds show at the top``(route: string) = task {
    do! this.NavigateTo(route)
    do! this.Expect(this.Page.GetByTestId("scroll-page")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``missing selector stays hidden when the page scrolls``() = task {
    do! this.NavigateTo("scroll-to-top")
    let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 650)")
    do! this.Expect(this.Page.GetByTestId("scroll-page")).ToBeVisibleAsync()
    do! this.Expect(this.Page.GetByTestId("scroll-missing")).ToBeHiddenAsync()
    do! this.Expect(this.Page.GetByTestId("scroll-missing")).ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``reduced motion uses instant scrolling even with smooth CSS``() = task {
    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ReducedMotion = ReducedMotion.Reduce))
    do! this.NavigateTo("scroll-to-top")

    let! _ =
      this.Page.AddStyleTagAsync(PageAddStyleTagOptions(Content = "html { scroll-behavior: smooth; }"))

    let! _ =
      this.Page.EvaluateAsync(
        """() => {
      document.scrollingElement.scrollTo({top: 650, behavior: 'instant'});
      const target = document.scrollingElement;
      const original = target.scrollTo.bind(target);
      target.scrollTo = options => { window.lastScrollBehavior = options.behavior; original(options); };
    }"""
      )

    do! this.Expect(this.Page.GetByTestId("scroll-page")).ToBeVisibleAsync()
    do! this.Page.GetByTestId("scroll-page").ClickAsync()
    let! _ = this.Page.WaitForFunctionAsync("() => window.scrollY === 0")
    let! behavior = this.Page.EvaluateAsync<string>("() => window.lastScrollBehavior")
    Assert.Equal("instant", behavior)
  }

  [<Fact>]
  member this.``repeated removal releases the page listener and remount restores it``() = task {
    use! _script =
      this.Page.AddInitScriptAsync(
        """(() => {
      const add = window.addEventListener.bind(window);
      const remove = window.removeEventListener.bind(window);
      window.scrollListeners = new Set();
      window.addEventListener = (name, handler, ...args) => {
        if (name === 'scroll') window.scrollListeners.add(handler);
        return add(name, handler, ...args);
      };
      window.removeEventListener = (name, handler, ...args) => {
        if (name === 'scroll') window.scrollListeners.delete(handler);
        return remove(name, handler, ...args);
      };
    })();"""
      )

    do! this.NavigateTo("scroll-to-top")
    let toggle = this.Page.GetByTestId("scroll-toggle")
    let! mountedCount = this.Page.EvaluateAsync<int>("() => window.scrollListeners.size")
    Assert.True(mountedCount > 0)

    for _ in 1..3 do
      do! toggle.ClickAsync()
      do! this.Expect(this.Page.GetByTestId("scroll-page")).ToHaveCountAsync(0)

      let! _ =
        this.Page.WaitForFunctionAsync("count => window.scrollListeners.size === count - 1", mountedCount)

      do! toggle.ClickAsync()

      let! _ =
        this.Page.WaitForFunctionAsync("count => window.scrollListeners.size === count", mountedCount)

      let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 650)")
      do! this.Expect(this.Page.GetByTestId("scroll-page")).ToBeVisibleAsync()
      let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 0)")
      do! this.Expect(this.Page.GetByTestId("scroll-page")).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``removing the container button releases its element listener``() = task {
    use! _script =
      this.Page.AddInitScriptAsync(
        """(() => {
      const add = Element.prototype.addEventListener;
      const remove = Element.prototype.removeEventListener;
      window.panelScrollListeners = new Set();
      Element.prototype.addEventListener = function(name, handler, ...args) {
        if (this.id === 'scroll-target' && name === 'scroll') window.panelScrollListeners.add(handler);
        return add.call(this, name, handler, ...args);
      };
      Element.prototype.removeEventListener = function(name, handler, ...args) {
        if (this.id === 'scroll-target' && name === 'scroll') window.panelScrollListeners.delete(handler);
        return remove.call(this, name, handler, ...args);
      };
    })();"""
      )

    do! this.NavigateTo("scroll-to-top-initial")
    let button = this.Page.GetByTestId("scroll-panel")
    do! this.Expect(button).ToBeVisibleAsync()
    let! count = this.Page.EvaluateAsync<int>("() => window.panelScrollListeners.size")
    Assert.Equal(1, count)
    let! _ = button.EvaluateAsync("el => el.remove()")

    let! _ =
      this.Page.WaitForFunctionAsync("() => window.panelScrollListeners.size === 0")

    let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 650)")
    do! this.Expect(this.Page.GetByTestId("scroll-page")).ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``visible buttons pass accessibility and contrast checks``(dark: bool) =
    this.RunAxeScan("scroll-to-top-accessible", contrast = true, dark = dark)

type ScrollListenerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``fallback selects the nearest visible heading regardless of DOM order``() = task {
    do! this.NavigateTo("scroll-sections")

    for name in [ "natural"; "reversed" ] do
      let active = this.Page.GetByTestId(name + "-active")
      do! this.Expect(active).ToHaveTextAsync(name + "-first")
      let panel = this.Page.GetByTestId(name + "-scroll")
      let! _ = panel.EvaluateAsync("el => el.scrollTop = 450")
      do! this.Expect(active).ToHaveTextAsync(name + "-later")
  }
