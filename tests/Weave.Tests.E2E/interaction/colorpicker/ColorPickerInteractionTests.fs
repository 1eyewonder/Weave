namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

type ColorPickerInteractionTests(fixture: TestFixture) =
  inherit ColorPickerTestBase(fixture)

  [<Fact>]
  member this.``Given valid hex when Enter is pressed then the bound color changes``() = task {
    do! this.NavigateTo("color-picker")
    do! this.EnterHex("#08f8")
    do! this.Expect(this.Value).ToHaveTextAsync("#0088FF88")
  }

  [<Fact>]
  member this.``Given invalid hex when committed then the selected color is retained``() = task {
    do! this.NavigateTo("color-picker")
    do! this.EnterHex("#invalid")
    do! this.Expect(this.Hex).ToHaveAttributeAsync("aria-invalid", "true")
    do! this.Expect(this.Value).ToHaveTextAsync("#FF0000")
  }

  [<Fact>]
  member this.``Given valid hex when focus leaves then the bound color changes``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Hex.FillAsync("#123456")
    do! this.Hex.PressAsync("Tab")
    do! this.Expect(this.Value).ToHaveTextAsync("#123456")
  }

  [<Fact>]
  member this.``Given invalid hex when corrected then validation clears``() = task {
    do! this.NavigateTo("color-picker")
    do! this.EnterHex("invalid")
    do! this.Expect(this.Hex).ToHaveAttributeAsync("aria-invalid", "true")
    do! this.EnterHex("#123456")
    do! this.Expect(this.Hex).ToHaveAttributeAsync("aria-invalid", "false")
  }

  [<Theory>]
  [<InlineData("Shift+ArrowLeft", "#FF1A1A")>]
  [<InlineData("Shift+ArrowDown", "#E60000")>]
  member this.``Given red when the spectrum receives an arrow then its color changes``
    (key: string, expected: string)
    = task {
    do! this.NavigateTo("color-picker")
    do! this.Spectrum.PressAsync(key)
    do! this.Expect(this.Value).ToHaveTextAsync(expected)
  }

  [<Fact>]
  member this.``Given red when hue increases one degree then RGB changes``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Slider("Hue").PressAsync("ArrowRight")
    do! this.Expect(this.Value).ToHaveTextAsync("#FF0400")
  }

  [<Theory>]
  [<InlineData("Home", "#00FF0000")>]
  [<InlineData("End", "#00FF00")>]
  member this.``Given translucent green when opacity moves to a boundary then alpha matches``
    (key: string, expected: string)
    = task {
    do! this.NavigateTo("color-picker")
    do! this.UseExternalColor()
    do! this.Slider("Opacity").PressAsync(key)
    do! this.Expect(this.Value).ToHaveTextAsync(expected)
  }

  [<Fact>]
  member this.``Given red when the spectrum midpoint is clicked then its color matches``() = task {
    do! this.NavigateTo("color-picker")
    let! bounds = this.Spectrum.BoundingBoxAsync()

    do!
      this.Spectrum.ClickAsync(
        LocatorClickOptions(Position = Position(X = bounds.Width / 2.f, Y = bounds.Height / 2.f))
      )

    do! this.Expect(this.Value).ToHaveTextAsync("#804040")
  }

  [<Fact>]
  member this.``Given a captured spectrum pointer when dragged outside then selection is clamped``() = task {
    do! this.NavigateTo("color-picker")
    let! bounds = this.Spectrum.BoundingBoxAsync()
    do! this.Page.Mouse.MoveAsync(bounds.X + bounds.Width / 2.f, bounds.Y + bounds.Height / 2.f)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(bounds.X + bounds.Width + 30.f, bounds.Y - 20.f)
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Value).ToHaveTextAsync("#FF0000")
  }

  [<Fact>]
  member this.``Given an external color change then hex reflects it``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("external").ClickAsync()
    do! this.Expect(this.Hex).ToHaveValueAsync("#00FF0080")
  }

  [<Fact>]
  member this.``Given an external color change then hue reflects it``() = task {
    do! this.NavigateTo("color-picker")
    do! this.UseExternalColor()
    do! this.Expect(this.Slider("Hue")).ToHaveAttributeAsync("aria-valuenow", "120.00")
  }

  [<Fact>]
  member this.``Given an RGB channel edit then the other channels are retained``() = task {
    do! this.NavigateTo("color-picker")
    do! this.UseExternalColor()
    do! this.FormatTab("RGB").ClickAsync()
    do! this.Picker.GetByLabel("R", LocatorGetByLabelOptions(Exact = true)).FillAsync("255")
    do! this.Expect(this.Value).ToHaveTextAsync("#FFFF0080")
  }

  [<Theory>]
  [<InlineData("#000000", "Shift+ArrowUp", "#001A00")>]
  [<InlineData("#808080", "Shift+ArrowRight", "#738073")>]
  member this.``Given green when changed to an achromatic color then further edits preserve hue``
    (color: string, key: string, expected: string)
    = task {
    do! this.NavigateTo("color-picker")
    do! this.UseExternalColor()
    do! this.EnterHex(color)
    do! this.Expect(this.Value).ToHaveTextAsync(color)
    do! this.Spectrum.PressAsync(key)
    do! this.Expect(this.Value).ToHaveTextAsync(expected)
  }

  [<Fact>]
  member this.``Given opaque mode when mounted then caller alpha is preserved``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Expect(this.Page.GetByTestId("opaque-value")).ToHaveTextAsync("#12345680")
  }

  [<Fact>]
  member this.``Given opaque mode when transparent hex is committed then alpha becomes opaque``() = task {
    do! this.NavigateTo("color-picker")

    let hex =
      this.Page
        .GetByTestId("opaque")
        .GetByRole(AriaRole.Textbox, LocatorGetByRoleOptions(Name = "Hex", Exact = true))

    do! hex.FillAsync("#abcdef12")
    do! hex.PressAsync("Enter")
    do! this.Expect(this.Page.GetByTestId("opaque-value")).ToHaveTextAsync("#ABCDEF")
  }

  [<Theory>]
  [<InlineData("opaque")>]
  [<InlineData("opaque-rgb")>]
  member this.``Given an opaque mode then opacity controls are absent``(picker: string) = task {
    do! this.NavigateTo("color-picker")

    do!
      this
        .Expect(
          this.Page.GetByTestId(picker).GetByRole(AriaRole.Slider, LocatorGetByRoleOptions(Name = "Opacity"))
        )
        .ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given opaque RGB mode when a channel changes then alpha becomes opaque``() = task {
    do! this.NavigateTo("color-picker")

    do!
      this.Page
        .GetByTestId("opaque-rgb")
        .GetByLabel("G", LocatorGetByLabelOptions(Exact = true))
        .FillAsync("17")

    do! this.Expect(this.Page.GetByTestId("opaque-value")).ToHaveTextAsync("#121156")
  }

  [<Theory>]
  [<InlineData("enabled")>]
  [<InlineData("readonly")>]
  member this.``Given unavailable editing when a spectrum key is dispatched then color is unchanged``
    (toggle: string)
    = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId(toggle).ClickAsync()
    do! this.Expect(this.Spectrum).ToHaveAttributeAsync("tabindex", "-1")

    let! _ =
      this.Spectrum.EvaluateAsync(
        "el => el.dispatchEvent(new KeyboardEvent('keydown', {key:'ArrowLeft', bubbles:true}))"
      )

    do! this.Expect(this.Value).ToHaveTextAsync("#FF0000")
  }

  [<Theory>]
  [<InlineData("enabled")>]
  [<InlineData("readonly")>]
  member this.``Given unavailable editing when the caller changes color then display updates``
    (toggle: string)
    = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId(toggle).ClickAsync()
    do! this.UseExternalColor()
  }

  [<Theory>]
  [<InlineData("enabled")>]
  [<InlineData("readonly")>]
  member this.``Given editing is restored when the spectrum changes then color updates``(toggle: string) =
    task {
      do! this.NavigateTo("color-picker")
      do! this.Page.GetByTestId(toggle).ClickAsync()
      do! this.Expect(this.Spectrum).ToHaveAttributeAsync("tabindex", "-1")
      do! this.Page.GetByTestId(toggle).ClickAsync()
      do! this.Spectrum.PressAsync("Shift+ArrowLeft")
      do! this.Expect(this.Value).ToHaveTextAsync("#FF1A1A")
    }

  [<Fact>]
  member this.``Given the picker then it passes accessibility checks``() = this.RunAxeScan("color-picker")

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``Given either theme then picker text has sufficient contrast``(dark: bool) =
    this.RunAxeScan("color-picker", contrast = true, dark = dark)

  [<Fact>]
  member this.``Given Hex mode when Right is pressed then RGB is selected without changing color``() = task {
    do! this.NavigateTo("color-picker")
    do! this.FormatTab("Hex").PressAsync("ArrowRight")
    do! this.Expect(this.FormatTab("RGB")).ToHaveAttributeAsync("aria-selected", "true")
    do! this.Expect(this.Value).ToHaveTextAsync("#FF0000")
  }

  [<Fact>]
  member this.``Given RGB mode when Left is pressed then Hex is selected``() = task {
    do! this.NavigateTo("color-picker")
    do! this.FormatTab("RGB").ClickAsync()
    do! this.FormatTab("RGB").PressAsync("ArrowLeft")
    do! this.Expect(this.Hex).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Given a fixed input format then format tabs are absent``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Expect(this.Page.GetByTestId("opaque").GetByRole(AriaRole.Tab)).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given hidden format tabs when moving backward from Hex then focus skips the tabs``() = task {
    do! this.NavigateTo("color-picker")
    let picker = this.Page.GetByTestId("opaque")

    let hex =
      picker.GetByRole(AriaRole.Textbox, LocatorGetByRoleOptions(Name = "Hex", Exact = true))

    let hiddenTab =
      picker.GetByRole(
        AriaRole.Tab,
        LocatorGetByRoleOptions(Name = "Hex", Exact = true, IncludeHidden = true)
      )

    do! hex.PressAsync("Shift+Tab")
    do! this.Expect(hiddenTab).Not.ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData(320, "R")>]
  [<InlineData(320, "G")>]
  [<InlineData(320, "B")>]
  [<InlineData(320, "A")>]
  [<InlineData(390, "R")>]
  [<InlineData(390, "G")>]
  [<InlineData(390, "B")>]
  [<InlineData(390, "A")>]
  member this.``Given a narrow viewport then each RGB channel fits``(width: int, name: string) = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.SetViewportSizeAsync(width, 844)
    do! this.FormatTab("RGB").ClickAsync()
    let channel = this.Picker.GetByLabel(name, LocatorGetByLabelOptions(Exact = true))
    do! this.Expect(channel).ToBeVisibleAsync()
    let! bounds = channel.BoundingBoxAsync()

    Assert.True(
      bounds.Width >= 32.f
      && bounds.X >= 0.f
      && bounds.X + bounds.Width <= float32 width
    )
  }

  [<Fact; Trait("Capability", "MobileEmulation")>]
  member this.``Given touch input when the spectrum midpoint is tapped then color updates``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "color-picker")

    let! bounds =
      page.GetByTestId("picker").Locator(".weave-colorpicker__spectrum").BoundingBoxAsync()

    do! page.Touchscreen.TapAsync(bounds.X + bounds.Width / 2.f, bounds.Y + bounds.Height / 2.f)
    do! Assertions.Expect(page.GetByTestId("value")).ToHaveTextAsync("#804040")
  }

  [<Fact; Trait("Capability", "MobileEmulation")>]
  member this.``Given touch input when RGB is tapped then the format changes``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "color-picker")

    let tab =
      page.GetByTestId("picker").GetByRole(AriaRole.Tab, LocatorGetByRoleOptions(Name = "RGB", Exact = true))

    do! tab.TapAsync()
    do! Assertions.Expect(tab).ToHaveAttributeAsync("aria-selected", "true")
  }
