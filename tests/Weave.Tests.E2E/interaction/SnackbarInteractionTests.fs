namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type SnackbarInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  let mutable controlledTime = false

  member private this.Host = this.Page.GetByTestId("primary-host")
  member private this.Message = this.Host.Locator(".weave-snackbar")

  member private this.Click(label: string) = task {
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = label, Exact = true)).ClickAsync()
    // WebSharper synchronizes reactive DOM changes on the next animation frame.
    if controlledTime then
      do! this.Page.Clock.RunForAsync(50L)
  }

  member private this.ControlTime() = task {
    do! this.NavigateTo("snackbar")
    do! this.Page.Clock.InstallAsync(ClockInstallOptions(TimeString = "2026-11-09T12:00:00Z"))
    do! this.Page.Clock.PauseAtAsync("2026-11-09T12:01:00Z")
    controlledTime <- true
  }

  [<Fact>]
  member this.``duplicates reuse IDs and dismissed messages may be shown again``() = task {
    do! this.NavigateTo("snackbar")
    do! this.Click("Persistent")
    do! this.Expect(this.Message).ToContainTextAsync("Persistent message")
    let! id = this.Page.GetByTestId("last-id").TextContentAsync()
    do! this.Click("Duplicate")
    do! this.Expect(this.Page.GetByTestId("last-id")).ToHaveTextAsync(id)
    do! this.Expect(this.Message).ToHaveCountAsync(1)
    do! this.Message.GetByRole(AriaRole.Button).ClickAsync()
    do! this.Expect(this.Message).ToHaveCountAsync(0)
    do! this.Click("Persistent")
    do! this.Expect(this.Message).ToContainTextAsync("Persistent message")
    let! nextId = this.Page.GetByTestId("last-id").TextContentAsync()
    Assert.NotEqual<string>(id, nextId)
  }

  [<Fact>]
  member this.``queued messages get a full duration after promotion``() = task {
    do! this.ControlTime()
    do! this.Click("Timed")
    do! this.Click("Queue timed")
    do! this.Expect(this.Message).ToContainTextAsync("Timed message")
    do! this.Page.Clock.RunForAsync(1100L)
    do! this.Expect(this.Message).ToContainTextAsync("Queued message")
    do! this.Page.Clock.RunForAsync(500L)
    do! this.Expect(this.Message).ToContainTextAsync("Queued message")
    do! this.Page.Clock.RunForAsync(600L)
    do! this.Expect(this.Message).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``hover and keyboard focus independently pause the remaining duration``() = task {
    do! this.ControlTime()
    do! this.Click("Timed")
    do! this.Page.Clock.RunForAsync(400L)
    do! this.Message.HoverAsync()
    do! this.Message.GetByRole(AriaRole.Button).FocusAsync()
    do! this.Page.Clock.RunForAsync(2000L)
    do! this.Expect(this.Message).ToHaveCountAsync(1)
    do! this.Page.Mouse.MoveAsync(0.0f, 0.0f)
    do! this.Page.Clock.RunForAsync(2000L)
    do! this.Expect(this.Message).ToHaveCountAsync(1)

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Persistent", Exact = true))
        .FocusAsync()

    do! this.Page.Clock.RunForAsync(300L)
    do! this.Expect(this.Message).ToHaveCountAsync(1)
    do! this.Page.Clock.RunForAsync(400L)
    do! this.Expect(this.Message).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``actions remain visible and keyboard activation dismisses before invoking once``() = task {
    do! this.ControlTime()
    do! this.Click("Action")
    do! this.Page.Clock.RunForAsync(6000L)
    do! this.Expect(this.Message).ToContainTextAsync("Item deleted")

    let action =
      this.Message.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Undo"))

    do! action.FocusAsync()
    do! action.PressAsync("Enter")
    do! this.Page.Clock.RunForAsync(50L)
    do! this.Expect(this.Message).ToHaveCountAsync(0)
    do! this.Expect(this.Page.GetByTestId("action-result")).ToHaveTextAsync("1")
    do! this.Click("Timed action")
    do! this.Page.Clock.RunForAsync(1100L)
    do! this.Expect(this.Message).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``remove and clear include queued entries and preserve other controllers``() = task {
    do! this.NavigateTo("snackbar")
    do! this.Click("Persistent")
    do! this.Click("Queue timed")
    do! this.Click("Remove last")
    do! this.Click("Custom")
    do! this.Click("Custom")
    do! this.Message.GetByRole(AriaRole.Button).ClickAsync()
    do! this.Expect(this.Message).ToContainTextAsync("Custom content")
    do! this.Message.GetByRole(AriaRole.Button).ClickAsync()
    do! this.Expect(this.Message).ToHaveCountAsync(0)
    do! this.Click("Custom")
    do! this.Click("Other")
    do! this.Click("Queue timed")
    do! this.Click("Clear")
    do! this.Expect(this.Message).ToHaveCountAsync(0)

    do!
      this
        .Expect(this.Page.GetByTestId("other-host").Locator(".weave-snackbar"))
        .ToContainTextAsync("Other message")

    do! this.Click("Persistent")
    do! this.Message.GetByRole(AriaRole.Button).ClickAsync()
    do! this.Expect(this.Message).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``unmount cancels timers and remount retains messages with fresh time``() = task {
    do! this.ControlTime()
    do! this.Click("Timed")
    do! this.Page.Clock.RunForAsync(400L)
    do! this.Click("Toggle host")
    do! this.Expect(this.Host).ToHaveCountAsync(0)
    do! this.Page.Clock.RunForAsync(5000L)
    do! this.Click("Toggle host")
    do! this.Expect(this.Message).ToContainTextAsync("Timed message")
    do! this.Page.Clock.RunForAsync(500L)
    do! this.Expect(this.Message).ToHaveCountAsync(1)
    do! this.Page.Clock.RunForAsync(600L)
    do! this.Expect(this.Message).ToHaveCountAsync(0)
  }

  [<Theory>]
  [<InlineData(390)>]
  [<InlineData(1280)>]
  member this.``host positions and message actions fit the viewport``(width: int) = task {
    do! this.NavigateTo("snackbar")
    do! this.Page.SetViewportSizeAsync(width, 800)
    do! this.Click("Action")
    let! bottom = this.Message.BoundingBoxAsync()
    Assert.True(bottom.X >= 0.0f && bottom.X + bottom.Width <= float32 width)
    Assert.True(bottom.Y + bottom.Height <= 800.0f)
    do! this.Click("Toggle position")

    do!
      this
        .Expect(this.Host)
        .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-snackbar-host--top-center"))

    let! top = this.Message.BoundingBoxAsync()
    Assert.True(top.Y < 40.0f)
    Assert.True(abs (top.X + top.Width / 2.0f - float32 width / 2.0f) < 1.0f)

    do!
      this
        .Expect(this.Message.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Undo")))
        .ToBeVisibleAsync()

    do! this.Expect(this.Host).ToHaveAttributeAsync("aria-live", "polite")
  }

  [<Fact>]
  member this.``explicit dismissal overrides the controller duration and manual messages stay visible``() =
    task {
      do! this.ControlTime()
      do! this.Click("Timed override")
      do! this.Expect(this.Message).ToContainTextAsync("Short message")
      do! this.Page.Clock.RunForAsync(300L)
      do! this.Expect(this.Message).ToHaveCountAsync(0)
      do! this.Click("Persistent")
      do! this.Page.Clock.RunForAsync(6000L)
      do! this.Expect(this.Message).ToContainTextAsync("Persistent message")
      do! this.Click("Clear")
      do! this.Expect(this.Message).ToHaveCountAsync(0)
    }

  [<Theory>]
  [<InlineData(390)>]
  [<InlineData(1280)>]
  member this.``docs position picker previews all six locations and replaces queued messages``(width: int) =
    task {
      // The isolated test host does not load the docs syntax-highlighting script.
      use! _highlighting =
        this.Page.AddInitScriptAsync("window.highlightCodeElement = () => {}")

      do! this.NavigateTo("snackbar-example")
      do! this.Page.SetViewportSizeAsync(width, 800)
      let host = this.Page.Locator(".weave-snackbar-host")
      let messages = host.Locator(".weave-snackbar")
      do! this.Expect(messages).ToHaveCountAsync(0)
      do! this.Click("Run queue demo")
      do! this.Expect(messages).ToHaveCountAsync(3)

      for label, position, isTop, horizontal in
        [
          "Top left", "top-left", true, -1
          "Top center", "top-center", true, 0
          "Top right", "top-right", true, 1
          "Bottom left", "bottom-left", false, -1
          "Bottom center", "bottom-center", false, 0
          "Bottom right", "bottom-right", false, 1
        ] do
        let choice =
          this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Position", Exact = true))

        do! choice.ClickAsync()

        do!
          this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = label, Exact = true)).ClickAsync()

        do! this.Expect(choice).ToContainTextAsync(label)
        do! this.Expect(messages).ToHaveCountAsync(0)
        do! this.Click("Show notification")

        do!
          this
            .Expect(host)
            .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-snackbar-host--" + position))

        do! this.Expect(messages).ToHaveCountAsync(1)
        do! this.Expect(messages).ToContainTextAsync("Your changes have been saved.")
        let! bounds = messages.BoundingBoxAsync()

        if isTop then
          Assert.True(abs (bounds.Y - 16.0f) < 1.0f)
        else
          Assert.True(abs (bounds.Y + bounds.Height - 784.0f) < 1.0f)

        match horizontal with
        | -1 -> Assert.True(abs (bounds.X - 16.0f) < 1.0f)
        | 0 -> Assert.True(abs (bounds.X + bounds.Width / 2.0f - float32 width / 2.0f) < 1.0f)
        | _ -> Assert.True(abs (bounds.X + bounds.Width - float32 width + 16.0f) < 1.0f)

      do! this.Click("Clear all")
      do! this.Expect(messages).ToHaveCountAsync(0)

      let positionChoice =
        this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Position", Exact = true))

      do! positionChoice.FocusAsync()
      do! positionChoice.PressAsync("ArrowDown")
      do! positionChoice.PressAsync("ArrowDown")
      do! positionChoice.PressAsync("Enter")
      do! this.Expect(positionChoice).ToContainTextAsync("Top center")

      let show =
        this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show notification", Exact = true))

      do! show.FocusAsync()
      do! show.PressAsync("Space")

      do!
        this
          .Expect(host)
          .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-snackbar-host--top-center"))

      do! this.Expect(messages).ToContainTextAsync("Your changes have been saved.")
    }
