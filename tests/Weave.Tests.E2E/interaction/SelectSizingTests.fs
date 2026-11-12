namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type SelectSizingTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  let longLabel =
    "A longer option whose full text should remain available in the menu"

  member private this.Load(?width: int, ?height: int) = task {
    do! this.Page.SetViewportSizeAsync(defaultArg width 1280, defaultArg height 900)
    do! this.NavigatePage(this.Page, "select-sizing")
  }

  member private this.Settle() = task {
    let! _ =
      this.Page.EvaluateAsync(
        """async () => {
          await new Promise(requestAnimationFrame);
          await new Promise(requestAnimationFrame);
          document.body.getBoundingClientRect();
          await Promise.all(document.getAnimations({ subtree: true })
            .filter(animation => animation instanceof CSSTransition)
            .map(animation => animation.finished.catch(() => {})));
        }"""
      )

    ()
  }

  member private this.Open(root: ILocator) = task {
    do! root.GetByRole(AriaRole.Combobox).ClickAsync()
    do! this.Expect(root.Locator(".weave-select__popover")).ToBeVisibleAsync()
    do! this.Settle()
  }

  member private this.AssertInside(menu: ILocator) = task {
    let! bounds = menu.BoundingBoxAsync()
    let viewport = this.Page.ViewportSize
    Assert.True(bounds.X >= 7.0f && bounds.Y >= 7.0f, $"Menu starts outside viewport: {bounds.X},{bounds.Y}")

    Assert.True(
      bounds.X + bounds.Width <= float32 viewport.Width - 7.0f,
      "Menu extends past viewport right edge"
    )

    Assert.True(
      bounds.Y + bounds.Height <= float32 viewport.Height - 7.0f,
      "Menu extends past viewport bottom edge"
    )
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``intrinsic labels clear the chevron and width modes remain usable``(width: int) = task {
    do! this.Load(width = width)

    for mode in [ "stable"; "dynamic"; "full" ] do
      let host = this.Page.GetByTestId("measured-" + mode)
      let root = host.Locator(".weave-select")
      let label = root.Locator("label")
      let chevron = root.Locator(".weave-select__chevron")
      let! labelBox = label.BoundingBoxAsync()
      and! chevronBox = chevron.BoundingBoxAsync()
      and! rootBox = root.BoundingBoxAsync()
      and! hostBox = host.BoundingBoxAsync()
      Assert.True(labelBox.X + labelBox.Width <= chevronBox.X + 1.0f, mode + ": label clears the chevron")
      Assert.True(rootBox.X + rootBox.Width <= float32 width + 1.0f, mode + ": root fits the viewport")

      let! accessibleLabel =
        root.Locator("[role='combobox']").GetAttributeAsync("aria-label")

      let! labelText = label.TextContentAsync()
      Assert.Equal(labelText, accessibleLabel)

      if mode = "full" then
        Assert.True(abs (rootBox.Width - hostBox.Width) <= 1.0f, "Full width still fills the host")
      else
        let! untruncated =
          label.EvaluateAsync<bool>("el => el.scrollWidth <= el.clientWidth")

        Assert.True(untruncated, mode + ": short label stays fully readable")

    let dynamic = this.Page.GetByTestId("measured-dynamic").Locator(".weave-select")
    let! before = dynamic.BoundingBoxAsync()
    do! dynamic.Locator("[role='combobox']").PressAsync("Enter")
    do! dynamic.Locator("[role='option']").Last.ClickAsync()
    do! this.Expect(dynamic.Locator("[role='combobox']")).ToContainTextAsync("A considerably longer option")
    let! after = dynamic.BoundingBoxAsync()
    Assert.True(after.Width > before.Width + 10.0f, "Dynamic width should grow for longer selected content")
  }

  [<Fact>]
  member this.``default selected text stays on one line with its full label available``() = task {
    do! this.Load()
    let root = this.Page.GetByTestId("bound")
    let selected = root.GetByTitle(longLabel, LocatorGetByTitleOptions(Exact = true))
    do! this.Expect(selected).ToHaveTextAsync(longLabel)
    do! this.Expect(root.GetByRole(AriaRole.Combobox)).ToContainTextAsync(longLabel)

    let! truncated =
      selected.EvaluateAsync<bool>(
        "el => getComputedStyle(el).textOverflow === 'ellipsis' && getComputedStyle(el).whiteSpace === 'nowrap' && el.scrollWidth > el.clientWidth"
      )

    Assert.True(truncated, "Long selected text must truncate visibly without changing its full source label")
    let! textBounds = selected.BoundingBoxAsync()
    and! actionBounds = root.Locator(".weave-select__actions").BoundingBoxAsync()

    Assert.True(
      textBounds.X + textBounds.Width <= actionBounds.X + 1.0f,
      "Selected text must leave room for field actions"
    )
  }

  [<Fact>]
  member this.``custom selected content retains its own composition``() = task {
    do! this.Load()
    let rich = this.Page.GetByTestId("rich")
    do! this.Expect(rich.GetByTestId("rich-value")).ToContainTextAsync("Rich")
    do! this.Expect(rich.Locator(".weave-select__selected-text")).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``default full width menus remain bound to their trigger``() = task {
    do! this.Load()
    let root = this.Page.GetByTestId("bound")
    do! this.Open(root)
    let! trigger = root.Locator(".weave-select").BoundingBoxAsync()
    and! menu = root.Locator(".weave-select__popover").BoundingBoxAsync()
    Assert.True(abs (trigger.Width - menu.Width) <= 1.0f, "Default menus must retain bound width")
  }

  [<Fact>]
  member this.``unbound menus grow and expose full labels at their maximum width``() = task {
    do! this.Load(width = 320)
    let root = this.Page.GetByTestId("unbound")
    do! this.Open(root)
    let menu = root.Locator(".weave-select__popover")
    let! trigger = root.Locator(".weave-select").BoundingBoxAsync()
    and! popup = menu.BoundingBoxAsync()

    Assert.True(
      popup.Width > trigger.Width + 20.0f,
      "Unbound menus should be able to grow beyond their trigger"
    )

    do! this.AssertInside(menu)

    let! labelsFit =
      menu.EvaluateAsync<bool>(
        "el => [...el.querySelectorAll('.weave-select__item-content')].every(item => item.scrollWidth <= item.clientWidth + 1)"
      )

    Assert.True(labelsFit, "Long sentences and unbroken options must wrap instead of being clipped")
  }

  [<Theory>]
  [<InlineData("origin-right")>]
  [<InlineData("origin-above")>]
  member this.``unbound menus preserve authored origins when they fit``(name: string) = task {
    do! this.Load()
    let root = this.Page.GetByTestId(name)
    do! this.Open(root)
    let! trigger = root.Locator(".weave-select").BoundingBoxAsync()
    and! menu = root.Locator(".weave-select__popover").BoundingBoxAsync()

    if name = "origin-right" then
      Assert.True(
        abs (trigger.X + trigger.Width - menu.X - menu.Width) <= 1.0f,
        "Right-aligned menu must retain its authored edge"
      )

      Assert.True(
        abs (menu.Y - trigger.Y - trigger.Height - 2.0f) <= 1.0f,
        "Bottom-right anchor must stay below the trigger"
      )
    else
      Assert.True(abs (trigger.X - menu.X) <= 1.0f, "Top-left anchor must retain its authored edge")

      Assert.True(
        abs (trigger.Y - menu.Y - menu.Height) <= 1.0f,
        "Bottom-left transform origin must place the menu above"
      )
  }

  [<Fact>]
  member this.``bottom edge menus remain inside after open transitions and repeated viewport changes``() =
    task {
      do! this.Load()
      let root = this.Page.GetByTestId("edge")
      do! this.Open(root)
      let menu = root.Locator(".weave-select__popover")
      do! this.AssertInside(menu)

      for width, height in [ 320, 480; 860, 540; 320, 360; 1280, 900 ] do
        do! this.Page.SetViewportSizeAsync(width, height)
        do! this.Settle()
        do! this.AssertInside(menu)

      let! stable =
        menu.EvaluateAsync<bool>(
          """async el => {
          const positions = [];
          for (let frame = 0; frame < 6; frame++) {
            await new Promise(requestAnimationFrame);
            const rect = el.getBoundingClientRect();
            positions.push([rect.x, rect.y]);
          }
          return positions.every(([x,y]) => Math.abs(x - positions[0][0]) < 1 && Math.abs(y - positions[0][1]) < 1);
        }"""
        )

      Assert.True(stable, "Viewport correction must settle without accumulating shifts or jitter")
    }

  [<Fact>]
  member this.``window resize repositions a fixed trigger without changing element sizes``() = task {
    do! this.Load(width = 1280)
    let root = this.Page.GetByTestId("viewport-moving")
    do! this.Open(root)
    let trigger = root.Locator(".weave-select")
    let menu = root.Locator(".weave-select__popover")
    let! triggerBefore = trigger.BoundingBoxAsync()
    and! menuBefore = menu.BoundingBoxAsync()
    Assert.True(abs (menuBefore.X - triggerBefore.X) <= 1.0f, "Initial menu uses its natural anchor")

    do! this.Page.SetViewportSizeAsync(700, 900)
    do! this.Settle()
    let! triggerAfter = trigger.BoundingBoxAsync()
    and! menuAfter = menu.BoundingBoxAsync()
    Assert.True(abs (triggerAfter.Width - triggerBefore.Width) <= 1.0f, "Trigger width stays constant")
    Assert.True(abs (menuAfter.Width - menuBefore.Width) <= 1.0f, "Menu width stays constant")
    Assert.True(triggerAfter.X + menuAfter.Width > 692.0f, "Natural position would overflow after resize")
    do! this.AssertInside(menu)

    do! this.Page.SetViewportSizeAsync(1280, 900)
    do! this.Settle()
    let! restoredTrigger = trigger.BoundingBoxAsync()
    and! restoredMenu = menu.BoundingBoxAsync()
    Assert.True(abs (restoredMenu.X - restoredTrigger.X) <= 1.0f, "Natural anchor returns after resize")
  }

  [<Fact>]
  member this.``ancestor scrolling restores natural alignment when room becomes available``() = task {
    do! this.Load()
    let root = this.Page.GetByTestId("scrolling")
    do! this.Open(root)
    let menu = root.Locator(".weave-select__popover")

    let! _ =
      this.Page.GetByTestId("select-sizing-page").EvaluateAsync("el => { el.scrollTop += 300; }")

    let! _ =
      menu.EvaluateAsync(
        """async el => {
          await new Promise(requestAnimationFrame);
          await new Promise(requestAnimationFrame);
        }"""
      )

    do! this.Settle()
    do! this.AssertInside(menu)
    let! trigger = root.Locator(".weave-select").BoundingBoxAsync()
    and! popup = menu.BoundingBoxAsync()

    Assert.True(
      trigger.Y + trigger.Height + popup.Height + 2.0f
      <= float32 this.Page.ViewportSize.Height - 8.0f,
      "The scrolled fixture must provide enough room for the menu's natural placement"
    )

    Assert.True(
      abs (popup.Y - trigger.Y - trigger.Height - 2.0f) <= 1.0f,
      $"Scrolled menu top {popup.Y}px should follow the natural anchor at {trigger.Y + trigger.Height + 2.0f}px"
    )
  }

  [<Fact>]
  member this.``unbound resize and scroll listeners are removed on every close``() = task {
    do! this.Load()

    let! _ =
      this.Page.EvaluateAsync(
        """() => {
          window.selectListenerSets = { resize: new Set(), scroll: new Set() };
          const add = window.addEventListener.bind(window);
          const remove = window.removeEventListener.bind(window);
          window.addEventListener = (type, listener, options) => {
            window.selectListenerSets[type]?.add(listener);
            add(type, listener, options);
          };
          window.removeEventListener = (type, listener, options) => {
            window.selectListenerSets[type]?.delete(listener);
            remove(type, listener, options);
          };
        }"""
      )

    let root = this.Page.GetByTestId("edge")

    for _ in 1..3 do
      do! this.Open(root)
      do! root.GetByRole(AriaRole.Combobox).PressAsync("Escape")
      do! this.Expect(root.Locator(".weave-select__popover")).ToHaveCountAsync(0)

      let! _ =
        this.Page.WaitForFunctionAsync(
          "() => Object.values(window.selectListenerSets).every(listeners => listeners.size === 0)"
        )

      ()
  }
