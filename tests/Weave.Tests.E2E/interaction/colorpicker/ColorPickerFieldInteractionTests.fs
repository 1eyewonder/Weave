namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

type ColorPickerFieldInteractionTests(fixture: TestFixture) =
  inherit ColorPickerTestBase(fixture)

  [<Fact>]
  member this.``Given a field with hidden format tabs when opened then its tabs are unavailable``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("rgb-only").GetByRole(AriaRole.Button).First.ClickAsync()
    do! this.Expect(this.Dialog).ToBeVisibleAsync()
    do! this.Expect(this.Dialog.GetByRole(AriaRole.Tab)).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given a field starting in RGB with hidden tabs when a channel changes then its color updates``
    ()
    = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("rgb-only").GetByRole(AriaRole.Button).First.ClickAsync()
    do! this.Dialog.GetByLabel("G", LocatorGetByLabelOptions(Exact = true)).FillAsync("128")
    do! this.Expect(this.Value).ToHaveTextAsync("#FF8000")
  }

  [<Fact>]
  member this.``Given an open picker when Escape is pressed then focus returns to its trigger``() = task {
    do! this.NavigateTo("color-picker")
    do! this.OpenField()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Dialog).ToHaveCountAsync(0)
    do! this.Expect(this.Trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Given an open picker when an outside pointer is pressed then it closes``() = task {
    do! this.NavigateTo("color-picker")
    do! this.OpenField()
    do! this.Page.Mouse.ClickAsync(1000.f, 100.f)
    do! this.Expect(this.Dialog).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given an open picker when Tab leaves its last control then it closes``() = task {
    do! this.NavigateTo("color-picker")
    do! this.OpenField()
    do! this.Done.PressAsync("Tab")
    do! this.Expect(this.Dialog).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given an open picker when Done is clicked then focus returns to its trigger``() = task {
    do! this.NavigateTo("color-picker")
    do! this.OpenField()
    do! this.Done.ClickAsync()
    do! this.Expect(this.Dialog).ToHaveCountAsync(0)
    do! this.Expect(this.Trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Given an open picker when editing is disabled then it closes``() = task {
    do! this.NavigateTo("color-picker")
    do! this.OpenField()
    let! _ = this.Page.GetByTestId("enabled").EvaluateAsync("element => element.click()")
    do! this.Expect(this.Dialog).ToHaveCountAsync(0)
    do! this.Expect(this.Trigger).ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given unavailable editing when open is requested then the request is cleared``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("readonly").ClickAsync()
    do! this.Page.GetByTestId("open").ClickAsync()
    do! this.Expect(this.Page.GetByTestId("open-state")).ToHaveTextAsync("false")
    do! this.Expect(this.Dialog).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given external open state when Escape closes the picker then state becomes false``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("open").ClickAsync()
    do! this.Expect(this.Dialog.Locator(".weave-colorpicker__spectrum")).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Page.GetByTestId("open-state")).ToHaveTextAsync("false")
  }

  [<Theory>]
  [<InlineData(320, 568)>]
  [<InlineData(390, 844)>]
  member this.``Given a narrow screen when opened then the panel fits the viewport``
    (width: int, height: int)
    = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.SetViewportSizeAsync(width, height)
    do! this.OpenField()
    let! bounds = this.Dialog.BoundingBoxAsync()
    Assert.True(bounds.X >= 0.f && bounds.X + bounds.Width <= float32 width)
    Assert.True(bounds.Y >= 15.f && bounds.Y + bounds.Height <= float32 height - 15.f)

    let! overflow =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth > innerWidth")

    Assert.False(overflow, "The popup must fit the viewport")
  }

  [<Theory>]
  [<InlineData(320, 568)>]
  [<InlineData(390, 844)>]
  member this.``Given a short screen when opened then Done remains visible``(width: int, height: int) = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.SetViewportSizeAsync(width, height)
    do! this.OpenField()
    let! bounds = this.Done.BoundingBoxAsync()
    Assert.True(bounds.Y >= 16.f && bounds.Y + bounds.Height <= float32 height - 16.f)
  }

  [<Fact>]
  member this.``Given an open desktop picker when the viewport becomes narrow then it moves inside the viewport``
    ()
    = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.SetViewportSizeAsync(1280, 900)
    do! this.OpenField()
    do! this.Page.SetViewportSizeAsync(320, 568)
    do! this.Expect(this.Dialog).ToHaveCSSAsync("top", "16px")
    let! bounds = this.Dialog.BoundingBoxAsync()
    Assert.True(bounds.X >= 16.f && bounds.X + bounds.Width <= 304.f)
  }

  [<Fact>]
  member this.``Given a mobile picker when its palette shrinks then it stays above the bottom margin``() =
    task {
      do! this.NavigateTo("color-picker")
      do! this.Page.SetViewportSizeAsync(390, 844)
      do! this.OpenField()

      let! _ =
        this.Page.GetByTestId("clear-swatches").EvaluateAsync("element => element.click()")

      do! this.Expect(this.Dialog.Locator(".weave-colorpicker__palette")).ToHaveCountAsync(0)

      let! bottomGap =
        this.Dialog.EvaluateAsync<float>(
          "element => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(() => resolve(innerHeight - element.getBoundingClientRect().bottom))))"
        )

      Assert.InRange(bottomGap, 15., 17.)
    }

  [<Fact; Trait("Capability", "MobileEmulation")>]
  member this.``Given touch input when Done is tapped then the picker closes``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "color-picker")
    do! page.GetByTestId("field").GetByRole(AriaRole.Button).TapAsync()
    let dialog = page.GetByRole(AriaRole.Dialog)
    do! Assertions.Expect(dialog.Locator(".weave-colorpicker__spectrum")).ToBeFocusedAsync()
    do! dialog.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Done", Exact = true)).TapAsync()
    do! Assertions.Expect(dialog).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given a closed picker then all temporary resize listeners are removed``() = task {
    let script =
      """(() => {
      window.pickerResizeHandlers = new Set();
      const add = window.addEventListener.bind(window);
      const remove = window.removeEventListener.bind(window);
      window.addEventListener = (type, handler, options) => {
        if (type === 'resize') window.pickerResizeHandlers.add(handler);
        return add(type, handler, options);
      };
      window.removeEventListener = (type, handler, options) => {
        if (type === 'resize') window.pickerResizeHandlers.delete(handler);
        return remove(type, handler, options);
      };
    })();"""

    use! _script = this.Page.AddInitScriptAsync(script)
    do! this.NavigateTo("color-picker")

    let! _ =
      this.Page.EvaluateAsync("() => new Promise(resolve => setTimeout(resolve, 0))")

    let! baseline =
      this.Page.EvaluateAsync<int>("() => window.pickerResizeHandlers.size")

    do! this.OpenField()
    do! this.Done.ClickAsync()
    do! this.Expect(this.Dialog).ToHaveCountAsync(0)

    let! _ =
      this.Page.EvaluateAsync("() => new Promise(resolve => setTimeout(resolve, 0))")

    let! remaining =
      this.Page.EvaluateAsync<int>("() => window.pickerResizeHandlers.size")

    Assert.Equal(baseline, remaining)
  }
