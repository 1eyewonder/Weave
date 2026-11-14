namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

type TooltipInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``opening and closing a tooltip emits no JavaScript errors``() = task {
    do! this.NavigateTo("tooltip")
    let errors = ResizeArray<string>()
    this.Page.PageError.Add(fun error -> errors.Add error)
    let wrapper = this.Page.GetByTestId("tooltip-default")
    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.Locator("[role='tooltip']")
    do! trigger.FocusAsync()

    let! _ =
      this.Page.EvaluateAsync(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    Assert.True(errors.Count = 0, System.String.Join("\n", errors))
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! trigger.PressAsync("Escape")
    Assert.True(errors.Count = 0, System.String.Join("\n", errors))
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``keyboard activation toggles a click tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.GetByTestId("tooltip-click")
    let button = wrapper.Locator("button")
    do! button.PressAsync("Enter")
    do! this.Expect(wrapper.Locator("[role='tooltip']")).ToBeVisibleAsync()
    do! button.PressAsync("Space")
    do! this.Expect(wrapper.Locator("[role='tooltip']")).ToBeHiddenAsync()
  }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``touch tap toggles a click tooltip``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "tooltip")
    let wrapper = page.GetByTestId("tooltip-click")
    let button = wrapper.Locator("button")
    do! button.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(wrapper.Locator("[role='tooltip']")).ToBeVisibleAsync()
    do! button.TapAsync()
    do! Microsoft.Playwright.Assertions.Expect(wrapper.Locator("[role='tooltip']")).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``every tooltip wrapper contains a role tooltip element``() = task {
    do! this.NavigateTo("tooltip")
    let! wrapperCount = this.Page.Locator(".weave-tooltip-root").CountAsync()
    Assert.True(wrapperCount > 0, "Expected at least one tooltip wrapper")
    let tooltips = this.Page.Locator("[role='tooltip']")
    do! this.Expect(tooltips).ToHaveCountAsync(wrapperCount)
  }

  [<Fact>]
  member this.``tooltip description is connected to the actual native trigger``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.GetByTestId("tooltip-default")
    let trigger = wrapper.Locator("button")
    let! tooltipId = wrapper.GetAttributeAsync("aria-describedby")
    let! describedBy = trigger.GetAttributeAsync("aria-describedby")

    Assert.True(
      not (isNull describedBy) && describedBy.Split(' ') |> Array.contains tooltipId,
      $"Native trigger lacks tooltip description {tooltipId}: {describedBy}"
    )
  }

  [<Fact>]
  member this.``custom tooltip ID is used by the trigger description``() = task {
    let errors = ResizeArray<string>()
    this.Page.PageError.Add(fun error -> errors.Add error)
    do! this.NavigateTo("tooltip-custom-id")
    let wrapper = this.Page.GetByTestId("tooltip-custom-id")
    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.Locator(".weave-tooltip")
    do! this.Expect(tooltip).ToHaveAttributeAsync("id", "custom-tooltip")
    do! trigger.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.Expect(trigger).ToHaveAttributeAsync("aria-describedby", "custom-tooltip")
    do! trigger.PressAsync("Escape")
    do! this.Expect(tooltip).ToBeHiddenAsync()
    Assert.Empty(errors)
  }

  [<Fact>]
  member this.``nested tooltip descriptions use each rendered bubble``() = task {
    let errors = ResizeArray<string>()
    this.Page.PageError.Add(fun error -> errors.Add error)
    do! this.NavigateTo("tooltip-nested")
    let outer = this.Page.GetByTestId("tooltip-outer")
    let inner = this.Page.GetByTestId("tooltip-inner")
    do! this.Expect(outer.Locator(".weave-tooltip").Last).ToHaveAttributeAsync("id", "outer-tooltip")
    do! this.Expect(inner.Locator(".weave-tooltip")).ToHaveAttributeAsync("id", "inner-tooltip")
    let! outerDescription = outer.GetAttributeAsync("aria-describedby")
    let! innerDescription = inner.GetAttributeAsync("aria-describedby")
    Assert.Equal("outer-tooltip", outerDescription)
    Assert.Contains("inner-tooltip", innerDescription)
    Assert.Empty(errors)
  }

  [<Fact>]
  member this.``tooltip description preserves existing IDs without adding duplicates``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.GetByTestId("tooltip-default")
    let trigger = wrapper.Locator("button")
    let! tooltipId = wrapper.GetAttributeAsync("aria-describedby")

    let! _ =
      trigger.EvaluateAsync("el => el.setAttribute('aria-describedby', 'existing-help  other-help')")

    do! trigger.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! trigger.FocusAsync()
    let! describedBy = trigger.GetAttributeAsync("aria-describedby")

    let ids =
      if isNull describedBy then
        [||]
      else
        describedBy.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)

    Assert.Contains("existing-help", ids)
    Assert.Contains("other-help", ids)
    Assert.Equal(1, ids |> Array.filter ((=) tooltipId) |> Array.length)
  }

  [<Fact>]
  member this.``tooltip description connects a replacement native trigger on focus``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.GetByTestId("tooltip-default")
    let! tooltipId = wrapper.GetAttributeAsync("aria-describedby")

    let! _ =
      wrapper.EvaluateAsync(
        """el => {
          const original = el.querySelector('button');
          const replacement = original.cloneNode(true);
          replacement.removeAttribute('aria-describedby');
          original.replaceWith(replacement);
        }"""
      )

    let trigger = wrapper.Locator("button")
    do! trigger.FocusAsync()
    let! describedBy = trigger.GetAttributeAsync("aria-describedby")

    Assert.True(
      not (isNull describedBy) && describedBy.Split(' ') |> Array.contains tooltipId,
      $"Replacement trigger lacks tooltip description {tooltipId}: {describedBy}"
    )
  }

  [<Fact>]
  member this.``wrapper has aria-describedby pointing to tooltip id``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    do! this.Expect(wrapper).ToHaveAttributeAsync("aria-describedby", Regex("weave-tooltip-\\d+"))
    let! describedBy = wrapper.GetAttributeAsync("aria-describedby")
    let tooltip = this.Page.Locator($"#{describedBy}")
    do! this.Expect(tooltip).ToHaveAttributeAsync("role", "tooltip")
  }

  [<Fact>]
  member this.``tooltip is hidden by default``() = task {
    do! this.Page.Mouse.MoveAsync(1200.0f, 700.0f)
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-default'] [role='tooltip']")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``hover shows the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``focus shows the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Escape dismisses tooltip shown via focus``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``Escape dismisses tooltip shown via hover when trigger is focused``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! focusable.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``focus remains on trigger after Escape``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(focusable).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``mouse leave hides the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.Page.Mouse.MoveAsync(640.0f, 600.0f)
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``focusOut hides the tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-default']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Tab")
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``click shows the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let button = wrapper.Locator(".weave-button").First
    do! button.ClickAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``click again hides the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let button = wrapper.Locator(".weave-button").First
    do! button.ClickAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! button.ClickAsync()
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``hover does not show the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``focus does not show the click-only tooltip``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.Locator("[data-testid='tooltip-click']")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let focusable = wrapper.Locator(".weave-button").First
    do! focusable.FocusAsync()
    do! this.Expect(tooltip).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``default tooltip has top-center direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-default'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--top-center"))
  }

  [<Fact>]
  member this.``bottom tooltip has bottom-center direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-bottom'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--bottom-center"))
  }

  [<Fact>]
  member this.``left tooltip has center-left direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-left'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--center-left"))
  }

  [<Fact>]
  member this.``right tooltip has center-right direction class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-right'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--center-right"))
  }

  [<Fact>]
  member this.``default tooltip has arrow class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-default'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--arrow"))
  }

  [<Fact>]
  member this.``no-arrow tooltip lacks arrow class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-no-arrow'] [role='tooltip']")
    do! this.Expect(tooltip).Not.ToHaveClassAsync(Regex("weave-tooltip--arrow"))
  }

  [<Fact>]
  member this.``color tooltip has the specified palette class``() = task {
    do! this.NavigateTo("tooltip")
    let tooltip = this.Page.Locator("[data-testid='tooltip-primary'] [role='tooltip']")
    do! this.Expect(tooltip).ToHaveClassAsync(Regex("weave-tooltip--primary"))
  }

  member private this.ExpectTooltipInViewport(tooltip: ILocator) = task {
    let! id = tooltip.GetAttributeAsync("id")

    let! _ =
      this.Page.WaitForFunctionAsync(
        """id => {
          const panel = document.getElementById(id);
          if (!panel || getComputedStyle(panel).visibility !== 'visible') return false;
          const bounds = panel.getBoundingClientRect();
          return bounds.width > 0 && bounds.height > 0 && bounds.left >= 7.5 && bounds.top >= 7.5
            && bounds.right <= document.documentElement.clientWidth - 7.5
            && bounds.bottom <= document.documentElement.clientHeight - 7.5;
        }""",
        id,
        PageWaitForFunctionOptions(Timeout = 3000.0f)
      )

    ()
  }

  [<Fact>]
  member this.``hidden tooltips do not enlarge a narrow document``() = task {
    do! this.NavigateTo("tooltip-viewport")
    do! this.Page.SetViewportSizeAsync(320, 640)

    let! width =
      this.Page.EvaluateAsync<int>("() => document.documentElement.scrollWidth")

    let! client =
      this.Page.EvaluateAsync<int>("() => document.documentElement.clientWidth")

    Assert.True(width <= client, $"Hidden tooltip expanded the document from {client}px to {width}px.")
  }

  [<Theory>]
  [<InlineData("right")>]
  [<InlineData("left")>]
  [<InlineData("top")>]
  [<InlineData("bottom")>]
  member this.``tooltips flip from a constrained viewport edge``(side: string) = task {
    do! this.NavigateTo("tooltip-viewport")
    do! this.Page.SetViewportSizeAsync(320, 640)
    let wrapper = this.Page.GetByTestId("tooltip-edge-" + side)
    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.GetByRole(AriaRole.Tooltip)
    do! trigger.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! this.ExpectTooltipInViewport tooltip
    let! target = trigger.BoundingBoxAsync()
    let! popup = tooltip.BoundingBoxAsync()

    let placedOpposite =
      match side with
      | "right" -> popup.X + popup.Width <= target.X + 1.0f
      | "left" -> popup.X >= target.X + target.Width - 1.0f
      | "top" -> popup.Y >= target.Y + target.Height - 1.0f
      | "bottom" -> popup.Y + popup.Height <= target.Y + 1.0f
      | _ -> false

    Assert.True(placedOpposite, $"Tooltip requested at {side} did not fit on its opposite side.")
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``orthogonal clamping and scrolling keep the focused tooltip inside the viewport``() = task {
    do! this.NavigateTo("tooltip-viewport")
    do! this.Page.SetViewportSizeAsync(320, 640)
    let wrapper = this.Page.GetByTestId("tooltip-edge-top")

    let! _ =
      wrapper.EvaluateAsync("el => { el.style.left = '8px'; el.style.top = '120px'; }")

    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.GetByRole(AriaRole.Tooltip)
    do! trigger.FocusAsync()
    do! this.ExpectTooltipInViewport tooltip
    let! target = trigger.BoundingBoxAsync()
    let! popup = tooltip.BoundingBoxAsync()

    Assert.True(
      popup.Y + popup.Height <= target.Y + 1.0f,
      "Preferred top placement had room but was not retained."
    )

    let! _ = this.Page.EvaluateAsync("() => window.scrollTo(0, 100)")
    do! this.ExpectTooltipInViewport tooltip
    let! movedTarget = trigger.BoundingBoxAsync()
    let! movedPopup = tooltip.BoundingBoxAsync()

    Assert.True(
      movedPopup.Y >= movedTarget.Y + movedTarget.Height - 1.0f,
      "Scrolling did not flip the newly constrained top tooltip."
    )

    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``a fixed viewport-relative tooltip restores its requested side after resize``() = task {
    do! this.NavigateTo("tooltip-viewport")
    do! this.Page.SetViewportSizeAsync(320, 640)
    let wrapper = this.Page.GetByTestId("tooltip-edge-right")

    let! _ =
      wrapper.EvaluateAsync(
        "el => { el.style.position = 'fixed'; el.style.left = '50vw'; el.style.top = '120px'; }"
      )

    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.GetByRole(AriaRole.Tooltip)
    do! trigger.FocusAsync()
    do! this.ExpectTooltipInViewport tooltip
    do! this.Expect(tooltip).ToHaveAttributeAsync("data-popover-flip", "")
    let! originalOwner = wrapper.BoundingBoxAsync()
    do! this.Page.SetViewportSizeAsync(800, 640)

    let! _ =
      tooltip.EvaluateAsync(
        "el => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! this.ExpectTooltipInViewport tooltip
    do! this.Expect(tooltip).Not.ToHaveAttributeAsync("data-popover-flip", "")
    let! resizedOwner = wrapper.BoundingBoxAsync()

    Assert.True(
      abs (resizedOwner.Width - originalOwner.Width) <= 1.0f,
      "The trigger must keep a constant size."
    )

    let! target = trigger.BoundingBoxAsync()
    let! popup = tooltip.BoundingBoxAsync()

    Assert.True(
      popup.X >= target.X + target.Width,
      "Tooltip stayed flipped after its requested side had room."
    )

    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``long tooltip content wraps and remains keyboard scrollable after updates``() = task {
    do! this.NavigateTo("tooltip-viewport")
    do! this.Page.SetViewportSizeAsync(320, 640)
    let wrapper = this.Page.GetByTestId("tooltip-long")
    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.GetByRole(AriaRole.Tooltip)
    do! trigger.FocusAsync()
    do! this.ExpectTooltipInViewport tooltip
    let! _ = this.Page.GetByTestId("tooltip-extend").EvaluateAsync("el => el.click()")
    do! this.ExpectTooltipInViewport tooltip
    let scrollable = tooltip.Locator("[tabindex='0']")
    do! this.Expect(scrollable).ToHaveCountAsync(1)
    do! scrollable.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! scrollable.PressAsync("End")

    let! _ =
      scrollable.EvaluateAsync(
        "el => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    let! scrolled = scrollable.EvaluateAsync<bool>("el => el.scrollTop > 0")
    Assert.True(scrolled, "Overflowing tooltip text was not reachable with the keyboard.")
    do! scrollable.PressAsync("Escape")
    do! this.Expect(tooltip).ToBeHiddenAsync()
    do! this.Expect(trigger).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``positioning listeners stop when a tooltip closes or is removed``() = task {
    do! this.NavigateTo("tooltip-viewport")

    let! _ =
      this.Page.EvaluateAsync(
        """() => {
          const add = window.addEventListener, remove = window.removeEventListener;
          const registrations = new Map();
          const key = (type, options) => type + ':' + (typeof options === 'boolean' ? options : !!options?.capture);
          window.addEventListener = function(type, listener, options) {
            if (type === 'resize' || type === 'scroll') {
              const name = key(type, options);
              if (!registrations.has(name)) registrations.set(name, new Set());
              registrations.get(name).add(listener);
            }
            return add.call(this, type, listener, options);
          };
          window.removeEventListener = function(type, listener, options) {
            registrations.get(key(type, options))?.delete(listener);
            return remove.call(this, type, listener, options);
          };
          window.tooltipPositioningListeners = () => [...registrations.values()]
            .reduce((sum, listeners) => sum + listeners.size, 0);
        }"""
      )

    let wrapper = this.Page.GetByTestId("tooltip-long")
    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.GetByRole(AriaRole.Tooltip)
    do! trigger.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! trigger.PressAsync("Escape")

    let! _ =
      this.Page.WaitForFunctionAsync("() => window.tooltipPositioningListeners() === 0")

    do! this.Page.GetByTestId("tooltip-extend").FocusAsync()
    do! trigger.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    let! _ = this.Page.GetByTestId("tooltip-toggle").EvaluateAsync("el => el.click()")
    do! this.Expect(wrapper).ToHaveCountAsync(0)

    let! _ =
      this.Page.WaitForFunctionAsync("() => window.tooltipPositioningListeners() === 0")

    ()
  }

  [<Fact>]
  member this.``Escape closes the active tooltip before its containing dialog``() = task {
    do! this.NavigateTo("tooltip-in-dialog")
    let dialog = this.Page.GetByRole(AriaRole.Dialog)
    let wrapper = this.Page.GetByTestId("tooltip-in-dialog")
    let trigger = wrapper.Locator("button")
    let tooltip = wrapper.GetByRole(AriaRole.Tooltip)
    do! trigger.FocusAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()
    do! trigger.PressAsync("Escape")
    do! this.Expect(tooltip).ToBeHiddenAsync()
    do! this.Expect(dialog).ToBeVisibleAsync()
    do! this.Expect(trigger).ToBeFocusedAsync()
    do! trigger.PressAsync("Escape")
    do! this.Expect(dialog).ToHaveCountAsync(0)
  }

// Holding hover through a transition must not compete with other browser pages.
[<Collection("Browser input")>]
type TooltipTransitionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``tooltip fades out before it is removed from layout``() = task {
    do! this.NavigateTo("tooltip")
    let wrapper = this.Page.GetByTestId("tooltip-default")
    let tooltip = wrapper.Locator("[role='tooltip']")
    let! _ = tooltip.EvaluateAsync("el => { el.style.transitionDuration = '600ms'; }")
    do! wrapper.HoverAsync()
    do! this.Expect(tooltip).ToBeVisibleAsync()

    let! _ =
      tooltip.EvaluateAsync("el => Promise.all(el.getAnimations().map(animation => animation.finished))")

    // Observe in the browser before mouseleave; polling after the action can miss
    // the entire fade under CI load. Register before the component's hide handler.
    let! _ =
      tooltip.EvaluateAsync(
        """el => {
          const onEnd = event => {
            if (event.target !== el || event.propertyName !== 'opacity'
                || el.classList.contains('weave-tooltip--visible')) return;
            const style = getComputedStyle(el);
            el.setAttribute('data-fade-out-display', style.display);
            el.setAttribute('data-fade-out-opacity', style.opacity);
            el.removeEventListener('transitionend', onEnd);
          };
          el.addEventListener('transitionend', onEnd);
        }"""
      )

    do! this.Page.Mouse.MoveAsync(640.0f, 600.0f)
    do! this.Expect(tooltip).ToHaveAttributeAsync("data-fade-out-display", "block")
    do! this.Expect(tooltip).ToHaveAttributeAsync("data-fade-out-opacity", "0")
    do! this.Expect(tooltip).ToHaveCSSAsync("display", "none")
  }
