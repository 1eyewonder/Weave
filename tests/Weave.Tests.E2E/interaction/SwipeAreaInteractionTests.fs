namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type SwipeAreaInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Start() = task {
    let! bounds = this.Page.Locator("#swipe-area").BoundingBoxAsync()
    let x, y = bounds.X + 180.0f, bounds.Y + 150.0f
    do! this.Page.Mouse.MoveAsync(x, y)
    do! this.Page.Mouse.DownAsync()
    return x, y
  }

  member private this.Swipe(dx: float32, dy: float32) = task {
    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + dx, y + dy, MouseMoveOptions(Steps = 4))
    do! this.Page.Mouse.UpAsync()
  }

  member private this.FlushUpdates() = task {
    let! _ =
      this.Page.EvaluateAsync<obj>(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    ()
  }

  [<Theory>]
  [<InlineData(120, 0, "Right:120:0")>]
  [<InlineData(-120, 0, "Left:-120:0")>]
  [<InlineData(0, -120, "Up:0:-120")>]
  [<InlineData(0, 120, "Down:0:120")>]
  [<InlineData(110, 40, "Right:110:40")>]
  [<InlineData(100, 100, "Down:100:100")>]
  member this.``release reports dominant direction and signed displacement``
    (dx: int, dy: int, expected: string)
    = task {
    do! this.NavigateTo "swipe-area"
    do! this.Swipe(float32 dx, float32 dy)
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync expected
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``movement is cumulative and does not bypass the threshold``() = task {
    do! this.NavigateTo "swipe-area"
    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + 10.0f, y + 5.0f)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "10:5"
    do! this.Page.Mouse.MoveAsync(x + 40.0f, y + 15.0f)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "40:15"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.Swipe(100.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``drag reports movement before one completed swipe on release``() = task {
    do! this.NavigateTo "swipe-area"
    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + 120.0f, y)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "120:0"
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync "Right:120:0"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
    do! this.Page.Mouse.MoveAsync(x + 160.0f, y)
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "120:0"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "0"
  }

  [<Fact>]
  member this.``threshold and enabled state update reactively``() = task {
    do! this.NavigateTo "swipe-area"
    do! this.Page.GetByText("Lower threshold", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Swipe(30.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
    do! this.Page.GetByText("Toggle enabled", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Swipe(120.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
    do! this.Page.GetByText("Toggle enabled", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Swipe(-30.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "2"
  }

  [<Fact>]
  member this.``zero threshold still ignores taps``() = task {
    do! this.NavigateTo "swipe-area"
    do! this.Page.GetByText("Zero threshold", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Swipe(0.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Swipe(1.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``negative threshold clamps to zero without turning taps into swipes``() = task {
    do! this.NavigateTo "swipe-area"
    do! this.Page.GetByText("Negative threshold", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Swipe(0.0f, 0.0f)
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Swipe(1.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync "Right:1:0"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``release displacement controls threshold after reversing direction``() = task {
    do! this.NavigateTo "swipe-area"
    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + 150.0f, y)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "150:0"
    do! this.Page.Mouse.MoveAsync(x + 20.0f, y)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "20:0"
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Swipe(120.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``threshold changes while held determine release outcome``() = task {
    do! this.NavigateTo "swipe-area"
    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + 40.0f, y)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "40:0"
    do! this.Page.GetByText("Lower threshold", PageGetByTextOptions(Exact = true)).DispatchEventAsync "click"
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync "Right:40:0"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"

    let! x2, y2 = this.Start()
    do! this.Page.Mouse.MoveAsync(x2 - 50.0f, y2)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "-50:0"
    do! this.Page.GetByText("Raise threshold", PageGetByTextOptions(Exact = true)).DispatchEventAsync "click"
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``disable immediately followed by release cancels exactly once``() = task {
    do! this.NavigateTo "swipe-area"
    let area = this.Page.Locator("#swipe-area")

    let! _ =
      area.EvaluateAsync(
        "el => el.addEventListener('pointerdown', e => el.dataset.pointerId = e.pointerId, {once: true})"
      )

    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + 120.0f, y)
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "120:0"
    // Queue both transitions in one browser task before WebSharper can render the disabled state.
    let! _ =
      area.EvaluateAsync(
        "el => { document.querySelector('#toggle-swipe-enabled').click(); el.dispatchEvent(new PointerEvent('pointerup', {pointerId: Number(el.dataset.pointerId), clientX: 300, clientY: 150, bubbles: true})); }"
      )

    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.Page.GetByText("Toggle enabled", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Swipe(120.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``removing a captured area leaves the replacement usable``() = task {
    do! this.NavigateTo "swipe-area-removal"
    let area = this.Page.Locator("#removable-area")
    let! bounds = area.BoundingBoxAsync()
    let x, y = bounds.X + 120.0f, bounds.Y + 80.0f
    do! this.Page.Mouse.MoveAsync(x, y)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(x + 120.0f, y)
    do! this.Expect(this.Page.Locator("#removal-movement")).ToHaveTextAsync "120:0"
    do! this.Page.GetByText("Toggle area", PageGetByTextOptions(Exact = true)).DispatchEventAsync "click"
    do! this.Expect(area).ToHaveCountAsync 0
    do! this.Page.Mouse.UpAsync()
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#removal-count")).ToHaveTextAsync "0"
    do! this.Page.GetByText("Toggle area", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(area).ToHaveCountAsync 1
    let! freshBounds = area.BoundingBoxAsync()
    let x2, y2 = freshBounds.X + 120.0f, freshBounds.Y + 80.0f
    do! this.Page.Mouse.MoveAsync(x2, y2)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(x2 + 120.0f, y2)
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#removal-count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``a nonprimary pointer cannot start an idle gesture``() = task {
    do! this.NavigateTo "swipe-area"
    let area = this.Page.Locator("#swipe-area")

    let! captured =
      area.EvaluateAsync<bool>(
        "el => { for (const [type, x] of [['pointerdown', 100], ['pointermove', 250], ['pointerup', 250]]) el.dispatchEvent(new PointerEvent(type, {pointerId: 99, isPrimary: false, button: 0, buttons: 1, clientX: x, clientY: 100, bubbles: true})); return el.hasPointerCapture(99); }"
      )

    Assert.False(captured)
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "0"
    do! this.Expect(this.Page.Locator("#movement")).ToHaveTextAsync "None"
    do! this.Swipe(120.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync "Right:120:0"
  }

  [<Fact>]
  member this.``pointer capture completes outside the area and resets for another swipe``() = task {
    do! this.NavigateTo "swipe-area"
    do! this.Swipe(450.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync "Right:450:0"
    do! this.Swipe(-120.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "2"
  }

  [<Theory>]
  [<InlineData("pointercancel")>]
  [<InlineData("lostpointercapture")>]
  member this.``cancellation clears the gesture``(eventName: string) = task {
    do! this.NavigateTo "swipe-area"

    let! _ =
      this.Page
        .Locator("#swipe-area")
        .EvaluateAsync(
          "el => el.addEventListener('pointerdown', e => el.dataset.pointerId = e.pointerId, {once: true})"
        )

    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + 120.0f, y)

    let! _ =
      this.Page
        .Locator("#swipe-area")
        .EvaluateAsync(
          "(el, type) => { const pointerId = Number(el.dataset.pointerId); if (type === 'lostpointercapture') el.releasePointerCapture(pointerId); else el.dispatchEvent(new PointerEvent(type, {pointerId, bubbles: true})); }",
          eventName
        )

    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Swipe(120.0f, 0.0f)
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``disabling in mid gesture cancels once``() = task {
    do! this.NavigateTo "swipe-area"
    let! x, y = this.Start()
    do! this.Page.Mouse.MoveAsync(x + 120.0f, y)
    // Change external state while the pointer remains captured by the area.
    do! this.Page.GetByText("Toggle enabled", PageGetByTextOptions(Exact = true)).DispatchEventAsync "click"
    do! this.Expect(this.Page.Locator("#enabled")).ToHaveTextAsync "false"
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
  }

  [<Fact>]
  member this.``embedded controls and ignored content retain their interaction``() = task {
    do! this.NavigateTo "swipe-area"
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Embedded button")).ClickAsync()
    do! this.Expect(this.Page.Locator("#clicks")).ToHaveTextAsync "1"
    do! this.Page.GetByRole(AriaRole.Textbox).FillAsync "Editable"

    for selector in [ "#swipe-area button"; "#swipe-area input"; "#ignored" ] do
      let! bounds = this.Page.Locator(selector).BoundingBoxAsync()
      let x, y = bounds.X + 10.0f, bounds.Y + 10.0f
      do! this.Page.Mouse.MoveAsync(x, y)
      do! this.Page.Mouse.DownAsync()
      do! this.Page.Mouse.MoveAsync(x + 120.0f, y)
      do! this.Page.Mouse.UpAsync()

    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "0"
  }

  [<Fact>]
  member this.``embedded native controls remain operable and ignored content does not start a swipe``() = task {
    do! this.NavigateTo "swipe-area-controls"
    let input = this.Page.Locator("#editable-input")
    let textarea = this.Page.Locator("#editable-textarea")

    for control in [ input; textarea ] do
      do! control.ClickAsync()
      do! this.Expect(control).ToBeFocusedAsync()
      do! control.PressAsync "Control+A"
      do! control.PressSequentiallyAsync "edited text"
      do! this.Expect(control).ToHaveValueAsync "edited text"
      let! rect = control.BoundingBoxAsync()
      do! this.Page.Mouse.ClickAsync(rect.X + 25.0f, rect.Y + 10.0f, MouseClickOptions(ClickCount = 2))

      let! selected =
        control.EvaluateAsync<bool>("el => el.selectionEnd > el.selectionStart")

      Assert.True(selected, "Text should remain selectable inside a swipe area")

    let select = this.Page.Locator("#editable-select")
    do! select.FocusAsync()
    do! this.Expect(select).ToBeFocusedAsync()
    let! _ = select.SelectOptionAsync "two"
    do! this.Expect(select).ToHaveValueAsync "two"

    let editable = this.Page.Locator("#editable-content")
    do! editable.ClickAsync()
    do! this.Expect(editable).ToBeFocusedAsync()
    do! editable.PressAsync "End"
    do! editable.PressSequentiallyAsync "!"
    do! this.Expect(editable).ToHaveTextAsync "alpha beta!"
    let! editableBounds = editable.BoundingBoxAsync()

    do!
      this.Page.Mouse.ClickAsync(
        editableBounds.X + 20.0f,
        editableBounds.Y + 10.0f,
        MouseClickOptions(ClickCount = 2)
      )

    let! hasSelection =
      editable.EvaluateAsync<bool>("() => !window.getSelection().isCollapsed")

    Assert.True(hasSelection, "Editable text should remain selectable inside a swipe area")
    do! this.Page.Locator("#ignored-control").ClickAsync()
    do! this.Expect(this.Page.Locator("#controls-clicks")).ToHaveTextAsync "1"
    do! this.Page.Locator("#control-link").ClickAsync()
    do! this.Page.WaitForURLAsync("**/#control-target")

    let ignored = this.Page.Locator("#custom-ignore")

    let! _ =
      ignored.EvaluateAsync(
        "el => el.addEventListener('pointerdown', () => el.dataset.reached = 'yes', {once: true})"
      )

    let! bounds = ignored.BoundingBoxAsync()
    do! this.Page.Mouse.MoveAsync(bounds.X + 10.0f, bounds.Y + 8.0f)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(bounds.X + 150.0f, bounds.Y + 8.0f)
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(ignored).ToHaveAttributeAsync("data-reached", "yes")
    do! this.FlushUpdates()
    do! this.Expect(this.Page.Locator("#controls-count")).ToHaveTextAsync "0"

    let! area = this.Page.Locator("#controls-area").BoundingBoxAsync()
    do! this.Page.Mouse.MoveAsync(area.X + 100.0f, area.Y + 220.0f)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(area.X + 250.0f, area.Y + 220.0f)
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#controls-count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``native touch tap cancels without a swipe``() = task {
    use! context =
      this.Page.Context.Browser.NewContextAsync(
        BrowserNewContextOptions(HasTouch = true, ViewportSize = ViewportSize(Width = 800, Height = 844))
      )

    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "swipe-area")
    let area = page.Locator("#swipe-area")

    let! _ =
      area.EvaluateAsync(
        "el => el.addEventListener('pointerdown', e => el.dataset.pointerType = e.pointerType, {once: true})"
      )

    let! bounds = area.BoundingBoxAsync()
    do! page.Touchscreen.TapAsync(bounds.X + 180.0f, bounds.Y + 150.0f)
    do! this.Expect(area).ToHaveAttributeAsync("data-pointer-type", "touch")
    do! this.Expect(page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.Expect(page.Locator("#count")).ToHaveTextAsync "0"
    do! page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Embedded button")).TapAsync()
    do! this.Expect(page.Locator("#clicks")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``nested areas do not trigger the parent``() = task {
    do! this.NavigateTo "swipe-area"
    let! bounds = this.Page.Locator("#nested-area").BoundingBoxAsync()
    do! this.Page.Mouse.MoveAsync(bounds.X + 20.0f, bounds.Y + 30.0f)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(bounds.X + 80.0f, bounds.Y + 30.0f)
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#inner")).ToHaveTextAsync "1"
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
  }

  [<Fact>]
  member this.``right mouse drags and unrelated pointers are ignored``() = task {
    do! this.NavigateTo "swipe-area"
    let! bounds = this.Page.Locator("#swipe-area").BoundingBoxAsync()
    do! this.Page.Mouse.MoveAsync(bounds.X + 180.0f, bounds.Y + 150.0f)
    do! this.Page.Mouse.DownAsync(MouseDownOptions(Button = MouseButton.Right))
    do! this.Page.Mouse.MoveAsync(bounds.X + 300.0f, bounds.Y + 150.0f)
    do! this.Page.Mouse.UpAsync(MouseUpOptions(Button = MouseButton.Right))
    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "0"
    let! x, y = this.Start()

    let! _ =
      this.Page
        .Locator("#swipe-area")
        .EvaluateAsync(
          "el => { for (const type of ['pointerdown', 'pointermove', 'pointerup', 'pointercancel']) el.dispatchEvent(new PointerEvent(type, {pointerId: 99, isPrimary: false, clientX: 500, clientY: 500, bubbles: true})); }"
        )

    do! this.Page.Mouse.MoveAsync(x + 120.0f, y)
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync "Right:120:0"
    do! this.Expect(this.Page.Locator("#cancels")).ToHaveTextAsync "0"
  }

  [<Fact>]
  member this.``touch policy preserves browser gestures by default``() = task {
    do! this.NavigateTo "swipe-area"
    do! this.Expect(this.Page.Locator("#native-area")).ToHaveCSSAsync("touch-action", "auto")
    do! this.Expect(this.Page.Locator("#horizontal-area")).ToHaveCSSAsync("touch-action", "pan-y pinch-zoom")
    do! this.Expect(this.Page.Locator("#vertical-area")).ToHaveCSSAsync("touch-action", "pan-x pinch-zoom")
    do! this.Expect(this.Page.Locator("#swipe-area")).ToHaveCSSAsync("touch-action", "none")

    do!
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next", Exact = true)).PressAsync
        "Enter"

    do! this.Expect(this.Page.Locator("#count")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``real touch swipes complete while vertical browser scrolling cancels``() = task {
    Assert.SkipWhen(
      this.Page.Context.Browser.BrowserType.Name <> "chromium",
      "Touch injection requires Chromium CDP."
    )

    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    // Keep the fixed-width fixture at scale 1 for CDP's viewport coordinates.
    do! page.SetViewportSizeAsync(800, 844)
    do! this.NavigatePage(page, "swipe-area")
    let! cdp = context.NewCDPSessionAsync(page)

    let send eventType (points: obj array) = task {
      let parameters = System.Collections.Generic.Dictionary<string, obj>()
      parameters.Add("type", box eventType)
      parameters.Add("touchPoints", box points)
      let! _ = cdp.SendAsync("Input.dispatchTouchEvent", parameters)
      let! _ = page.EvaluateAsync<obj>("() => new Promise(requestAnimationFrame)")

      ()
    }

    let point x y = [|
      box {|
        x = x
        y = y
      |}
    |]

    let! bounds = page.Locator("#swipe-area").BoundingBoxAsync()
    let x, y = float bounds.X + 180.0, float bounds.Y + 150.0
    do! send "touchStart" (point x y)
    do! send "touchMove" (point (x - 60.0) y)
    do! send "touchMove" (point (x - 120.0) y)
    do! send "touchEnd" [||]
    do! this.Expect(page.Locator("#result")).ToHaveTextAsync "Left:-120:0"
    do! this.Expect(page.Locator("#count")).ToHaveTextAsync "1"

    // The fixture reads the hash once at startup, so force a new document.
    let! _ = page.GotoAsync("about:blank")
    do! this.NavigatePage(page, "swipe-area-scroll")
    let! scrollBounds = page.Locator("#swipe-area").BoundingBoxAsync()
    let sx, sy = float scrollBounds.X + 180.0, float scrollBounds.Y + 150.0
    do! send "touchStart" (point sx sy)

    for distance in [ 30.0; 60.0; 90.0; 120.0 ] do
      do! send "touchMove" (point sx (sy - distance))

    do! send "touchEnd" [||]
    do! this.Expect(page.Locator("#cancels")).ToHaveTextAsync "1"
    do! this.Expect(page.Locator("#count")).ToHaveTextAsync "0"
    let! _ = page.WaitForFunctionAsync("() => window.scrollY > 0")
    do! cdp.DetachAsync()
  }

  [<Fact>]
  member this.``a second touch cannot take over the primary gesture``() = task {
    Assert.SkipWhen(
      this.Page.Context.Browser.BrowserType.Name <> "chromium",
      "Multi-touch injection requires Chromium CDP."
    )

    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! page.SetViewportSizeAsync(800, 844)
    do! this.NavigatePage(page, "swipe-area")
    let area = page.Locator("#swipe-area")

    let! _ =
      area.EvaluateAsync(
        "el => el.addEventListener('pointerdown', e => el.dataset[e.isPrimary ? 'primarySeen' : 'secondarySeen'] = e.pointerType)"
      )

    let! bounds = area.BoundingBoxAsync()
    let x, y = float bounds.X + 180.0, float bounds.Y + 150.0
    let! cdp = context.NewCDPSessionAsync(page)

    let point id x =
      box {|
        id = id
        x = x
        y = y
      |}

    let send eventType (points: obj array) = task {
      let parameters = System.Collections.Generic.Dictionary<string, obj>()
      parameters.Add("type", box eventType)
      parameters.Add("touchPoints", box points)
      let! _ = cdp.SendAsync("Input.dispatchTouchEvent", parameters)
      let! _ = page.EvaluateAsync<obj>("() => new Promise(requestAnimationFrame)")
      ()
    }

    do! send "touchStart" [| point 1 x |]
    do! send "touchStart" [| point 1 x; point 2 (x + 100.0) |]
    do! this.Expect(area).ToHaveAttributeAsync("data-primary-seen", "touch")
    do! this.Expect(area).ToHaveAttributeAsync("data-secondary-seen", "touch")
    do! send "touchMove" [| point 1 x; point 2 (x + 150.0) |]
    // CDP touchEnd names the contacts to release, not the contacts that remain.
    do! send "touchEnd" [| point 2 (x + 150.0) |]
    do! this.Expect(page.Locator("#count")).ToHaveTextAsync "0"
    do! this.Expect(page.Locator("#cancels")).ToHaveTextAsync "0"
    do! send "touchMove" [| point 1 (x + 120.0) |]
    do! this.Expect(page.Locator("#movement")).ToHaveTextAsync "120:0"
    do! send "touchEnd" [||]
    do! this.Expect(page.Locator("#result")).ToHaveTextAsync "Right:120:0"
    do! this.Expect(page.Locator("#count")).ToHaveTextAsync "1"
    do! this.Expect(page.Locator("#cancels")).ToHaveTextAsync "0"
    do! cdp.DetachAsync()
  }

  [<Fact>]
  member this.``swipe area passes accessibility scans``() = task {
    do! this.RunAxeScan "swipe-area"
    do! this.RunAxeScan("swipe-area", dark = true, contrast = true)
    do! this.RunAxeScan "swipe-area-documentation"
  }

[<Collection("Browser input")>]
type SwipeAreaDocumentationTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Navigation =
    this.Page
      .Locator("#swipe-navigation")
      .Locator(
        "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
      )

  [<Fact>]
  member this.``drawer edge and app gestures stay in their own regions``() = task {
    do! this.NavigateTo "swipe-area-documentation"

    let section =
      this.Page
        .Locator("#swipe-to-open-a-drawer")
        .Locator(
          "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
        )

    let drawer = section.Locator(".weave-drawer")
    let edge = section.Locator(".weave-swipearea[aria-hidden='true']")
    let appArea = section.Locator(".weave-main-content > .weave-swipearea")
    let status = section.GetByRole(AriaRole.Status)

    let toggle =
      section.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Enable drawer edge swipe"))

    do! section.ScrollIntoViewIfNeededAsync()

    let swipeFrom (area: ILocator) startOffset delta = task {
      let! bounds = area.BoundingBoxAsync()
      let x, y = bounds.X + startOffset, bounds.Y + min bounds.Height 220.0f - 30.0f
      do! this.Page.Mouse.MoveAsync(x, y)
      do! this.Page.Mouse.DownAsync()
      do! this.Page.Mouse.MoveAsync(x + delta, y, MouseMoveOptions(Steps = 5))
      do! this.Page.Mouse.UpAsync()
    }

    do! this.Expect(edge).ToHaveCountAsync 1
    do! swipeFrom appArea 270.0f -120.0f
    do! this.Expect(status).ToHaveTextAsync "App swipe: Left"
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))
    do! swipeFrom edge 10.0f 50.0f
    do! this.Expect(status).ToHaveTextAsync "App swipe: Left"
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))
    do! swipeFrom edge 10.0f 120.0f
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--open"))
    do! this.Expect(edge).ToHaveCountAsync 0

    // Wait for the actual slide-in animation before measuring the header.
    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => { const el = document.querySelector('#swipe-to-open-a-drawer')?.closest('.docs-section')?.querySelector('.weave-drawer'); return el && el.getAnimations().every(a => a.playState !== 'running'); }"
      )

    let header = drawer.Locator(".weave-swipearea")
    let! headerBounds = header.BoundingBoxAsync()

    let x, y =
      headerBounds.X + headerBounds.Width - 15.0f, headerBounds.Y + headerBounds.Height / 2.0f

    do! this.Page.Mouse.MoveAsync(x, y)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(x - 120.0f, y, MouseMoveOptions(Steps = 5))
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))
    do! this.Expect(edge).ToHaveCountAsync 1

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => { const el = document.querySelector('#swipe-to-open-a-drawer')?.closest('.docs-section')?.querySelector('.weave-drawer'); return el && el.getAnimations().every(a => a.playState !== 'running'); }"
      )

    do! toggle.PressAsync "Space"
    do! this.Expect(edge).ToHaveCountAsync 0
    do! swipeFrom appArea 150.0f -120.0f
    do! this.Expect(status).ToHaveTextAsync "App swipe: Left"
    do! swipeFrom appArea 10.0f 120.0f
    do! this.Expect(status).ToHaveTextAsync "App swipe: Right"
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))

    do!
      section
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Open navigation", Exact = true))
        .ClickAsync()

    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--open"))

    do!
      section
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Close navigation", Exact = true))
        .ClickAsync()

    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))
  }

  [<Fact>]
  member this.``drawer edge swipe and app swipe work with touch``() = task {
    Assert.SkipWhen(
      this.Page.Context.Browser.BrowserType.Name <> "chromium",
      "Touch-swipe injection requires Chromium CDP."
    )

    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "swipe-area-documentation")

    let section =
      page
        .Locator("#swipe-to-open-a-drawer")
        .Locator(
          "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
        )

    do! section.ScrollIntoViewIfNeededAsync()
    let edge = section.Locator(".weave-swipearea[aria-hidden='true']")
    let drawer = section.Locator(".weave-drawer")
    let appArea = section.Locator(".weave-main-content > .weave-swipearea")
    let status = section.GetByRole(AriaRole.Status)
    let! cdp = context.NewCDPSessionAsync(page)

    let send eventType (points: obj array) = task {
      let parameters = System.Collections.Generic.Dictionary<string, obj>()
      parameters.Add("type", box eventType)
      parameters.Add("touchPoints", box points)
      let! _ = cdp.SendAsync("Input.dispatchTouchEvent", parameters)
      let! _ = page.EvaluateAsync<obj>("() => new Promise(requestAnimationFrame)")
      ()
    }

    let swipe x y delta = task {
      let point px = [|
        box {|
          x = px
          y = y
        |}
      |]

      do! send "touchStart" (point x)

      for step in 1..6 do
        do! send "touchMove" (point (x + delta * float step / 6.0))

      do! send "touchEnd" [||]
    }

    let! appBounds = appArea.BoundingBoxAsync()
    do! swipe (float appBounds.X + 220.0) (float appBounds.Y + 180.0) -120.0
    do! this.Expect(status).ToHaveTextAsync "App swipe: Left"
    let! edgeBounds = edge.BoundingBoxAsync()
    do! swipe (float edgeBounds.X + 10.0) (float edgeBounds.Y + 180.0) 120.0
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--open"))
    do! this.Expect(edge).ToHaveCountAsync 0

    let! _ =
      page.WaitForFunctionAsync(
        "() => { const el = document.querySelector('#swipe-to-open-a-drawer')?.closest('.docs-section')?.querySelector('.weave-drawer'); return el && el.getAnimations().every(a => a.playState !== 'running'); }"
      )

    let! headerBounds = drawer.Locator(".weave-swipearea").BoundingBoxAsync()

    do!
      swipe
        (float (headerBounds.X + headerBounds.Width) - 15.0)
        (float (headerBounds.Y + headerBounds.Height / 2.0f))
        -120.0

    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))

    let! _ =
      page.WaitForFunctionAsync(
        "() => { const el = document.querySelector('#swipe-to-open-a-drawer')?.closest('.docs-section')?.querySelector('.weave-drawer'); return el && el.getAnimations().every(a => a.playState !== 'running'); }"
      )

    do!
      section
        .GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Enable drawer edge swipe"))
        .PressAsync
        "Space"

    do! this.Expect(edge).ToHaveCountAsync 0
    let! newAppBounds = appArea.BoundingBoxAsync()
    do! swipe (float newAppBounds.X + 10.0) (float newAppBounds.Y + 180.0) 120.0
    do! this.Expect(drawer).ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-drawer--closed"))
    do! this.Expect(status).ToHaveTextAsync "App swipe: Right"
    do! cdp.DetachAsync()
  }

  [<Fact>]
  member this.``configurator updates the displayed source and reset restores it``() = task {
    do! this.NavigateTo "swipe-area-documentation"

    let section =
      this.Page
        .Locator("#swipe-directions")
        .Locator(
          "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
        )

    let code =
      section.Locator("pre[aria-label='Selected appearance F# source code'] code")

    do!
      section
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Expect(code).ToContainTextAsync "let threshold = Var.Create 60"

    let slider =
      section.GetByRole(AriaRole.Slider, LocatorGetByRoleOptions(Name = "Swipe threshold (pixels)"))

    do! slider.PressAsync "Home"
    do! this.Expect(code).ToContainTextAsync "let threshold = Var.Create 10"

    let enabled =
      section.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Enable swipe gestures"))

    do! enabled.PressAsync "Space"
    do! this.Expect(code).ToContainTextAsync "let enabled = Var.Create false"
    let preview = section.Locator(".weave-swipearea")
    do! preview.ScrollIntoViewIfNeededAsync()

    let swipe () = task {
      let! bounds = preview.BoundingBoxAsync()
      let x, y = bounds.X + bounds.Width * 0.2f, bounds.Y + bounds.Height / 2.0f
      do! this.Page.Mouse.MoveAsync(x, y)
      do! this.Page.Mouse.DownAsync()
      do! this.Page.Mouse.MoveAsync(x + bounds.Width * 0.6f, y, MouseMoveOptions(Steps = 4))
      do! this.Page.Mouse.UpAsync()
    }

    do! swipe ()

    let! _ =
      this.Page.EvaluateAsync<obj>(
        "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    do! this.Expect(preview).ToContainTextAsync "Swipe in any direction"
    do! section.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(code).ToContainTextAsync "let threshold = Var.Create 60"
    do! this.Expect(code).ToContainTextAsync "let enabled = Var.Create true"
    do! swipe ()
    do! this.Expect(preview).ToContainTextAsync "Right"
  }

  [<Fact>]
  member this.``navigation buttons reach each chapter in both directions``() = task {
    do! this.NavigateTo "swipe-area-documentation"
    let area = this.Navigation
    let card = area.Locator(".weave-swipearea")

    let next =
      area.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Next", Exact = true))

    let previous =
      area.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Previous", Exact = true))

    do! this.Expect(card).ToContainTextAsync "1 / 3"
    do! next.ClickAsync()
    do! this.Expect(card).ToContainTextAsync "Connect the pieces"
    do! this.Expect(card).ToContainTextAsync "2 / 3"
    do! next.PressAsync "Enter"
    do! this.Expect(card).ToContainTextAsync "3 / 3"
    do! previous.ClickAsync()
    do! this.Expect(card).ToContainTextAsync "2 / 3"
    do! previous.PressAsync "Enter"
    do! this.Expect(card).ToContainTextAsync "1 / 3"
    do! previous.ClickAsync()
    do! this.Expect(card).ToContainTextAsync "3 / 3"
  }

  [<Fact>]
  member this.``swiping reaches each chapter and wraps``() = task {
    do! this.NavigateTo "swipe-area-documentation"
    let card = this.Navigation.Locator(".weave-swipearea")
    do! card.ScrollIntoViewIfNeededAsync()

    let swipe left = task {
      let! bounds = card.BoundingBoxAsync()
      let startX = bounds.X + bounds.Width * (if left then 0.75f else 0.25f)
      let endX = bounds.X + bounds.Width * (if left then 0.25f else 0.75f)
      let y = bounds.Y + bounds.Height / 2.0f
      do! this.Page.Mouse.MoveAsync(startX, y)
      do! this.Page.Mouse.DownAsync()
      do! this.Page.Mouse.MoveAsync(endX, y, MouseMoveOptions(Steps = 5))
      do! this.Page.Mouse.UpAsync()
    }

    do! swipe true
    do! this.Expect(card).ToContainTextAsync "2 / 3"
    do! swipe true
    do! this.Expect(card).ToContainTextAsync "3 / 3"
    do! swipe true
    do! this.Expect(card).ToContainTextAsync "1 / 3"
    do! swipe false
    do! this.Expect(card).ToContainTextAsync "3 / 3"
    do! swipe false
    do! this.Expect(card).ToContainTextAsync "2 / 3"
  }

  [<Fact>]
  member this.``a long swipe ending outside the card advances once``() = task {
    do! this.NavigateTo "swipe-area-documentation"
    let card = this.Navigation.Locator(".weave-swipearea")
    do! card.ScrollIntoViewIfNeededAsync()
    let! bounds = card.BoundingBoxAsync()
    let y = bounds.Y + bounds.Height / 2.0f
    do! this.Page.Mouse.MoveAsync(bounds.X + bounds.Width * 0.8f, y)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(max 1.0f (bounds.X - 10.0f), y, MouseMoveOptions(Steps = 8))
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(card).ToContainTextAsync "Connect the pieces"
    do! this.Expect(card).ToContainTextAsync "2 / 3"
  }

  [<Fact>]
  member this.``a long touch swipe ending outside the card advances once``() = task {
    Assert.SkipWhen(
      this.Page.Context.Browser.BrowserType.Name <> "chromium",
      "Touch injection requires Chromium CDP."
    )

    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "swipe-area-documentation")

    let card =
      page
        .Locator("#swipe-navigation")
        .Locator(
          "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
        )
        .Locator(".weave-swipearea")

    do! card.ScrollIntoViewIfNeededAsync()
    let! bounds = card.BoundingBoxAsync()
    let startX = float (bounds.X + bounds.Width * 0.8f)
    let endX = float (max 1.0f (bounds.X - 10.0f))
    let y = float (bounds.Y + bounds.Height / 2.0f)
    let! cdp = context.NewCDPSessionAsync(page)

    let send eventType (points: obj array) = task {
      let parameters = System.Collections.Generic.Dictionary<string, obj>()
      parameters.Add("type", box eventType)
      parameters.Add("touchPoints", box points)
      let! _ = cdp.SendAsync("Input.dispatchTouchEvent", parameters)
      let! _ = page.EvaluateAsync<obj>("() => new Promise(requestAnimationFrame)")
      ()
    }

    let point x = [|
      box {|
        x = x
        y = y
      |}
    |]

    do! send "touchStart" (point startX)

    for step in 1..6 do
      do! send "touchMove" (point (startX + (endX - startX) * float step / 6.0))

    do! send "touchEnd" [||]
    do! this.Expect(card).ToContainTextAsync "Connect the pieces"
    do! this.Expect(card).ToContainTextAsync "2 / 3"
    do! cdp.DetachAsync()
  }
