namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type TabsAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Preview =
    this.Page.Locator(".docs-appearance__preview .weave-tabs")

  member private this.Source =
    this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

  member private this.Control(label) =
    this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = label, Exact = true))

  member private this.Tray =
    this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Colored tray", Exact = true))

  member private this.Choose(label, choice) = task {
    do! this.Control(label).ClickAsync()
    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = choice, Exact = true)).ClickAsync()
  }

  member private this.Load(?width: int) = task {
    do! this.Page.SetViewportSizeAsync(defaultArg width 1280, 900)
    do! this.NavigatePage(this.Page, "tabs-appearance")

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => ['appearance-docs-style', 'appearance-highlighting-style'].every(id => document.getElementById(id)?.sheet)"
      )

    do! this.Expect(this.Preview).ToBeVisibleAsync()
  }

  member private this.OpenSource() = task {
    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``default appearance keeps its intentional color and omits native defaults``() = task {
    do! this.Load()
    do! this.OpenSource()
    do! this.Expect(this.Page.GetByRole(AriaRole.Combobox)).ToHaveCountAsync(6)
    do! this.Expect(this.Source).ToContainTextAsync("attrs = [ Tabs.Color.primary ]")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Variant.text")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Density.standard")
    do! this.Expect(this.Source).Not.ToContainTextAsync("activeIndex =")
    do! this.Expect(this.Source).Not.ToContainTextAsync("position =")
    do! this.Expect(this.Source).Not.ToContainTextAsync("docs-appearance__bounds")
    do! this.Expect(this.Preview.GetByRole(AriaRole.Tabpanel).First).ToHaveTextAsync("Overview content")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``selected appearance and panel agree with source at desktop and mobile widths``(width: int) =
    task {
      do! this.Load(width = width)

      do!
        this.Preview
          .GetByRole(AriaRole.Tab, LocatorGetByRoleOptions(Name = "Activity", Exact = true))
          .ClickAsync()

      do! this.Choose("Variant", "Filled")
      do! this.Choose("Color", "Warning")
      do! this.Choose("Density", "Spacious")
      do! this.Choose("Position", "Left")
      do! this.OpenSource()
      do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--filled"))
      do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--warning"))
      do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-density--spacious"))
      do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--left"))
      do! this.Expect(this.Preview.Locator("[role='tabpanel']:visible")).ToHaveTextAsync("Activity content")
      do! this.Expect(this.Source).ToContainTextAsync("let selectedTab = Var.Create 1")
      do! this.Expect(this.Source).ToContainTextAsync("Tabs.Variant.filled")
      do! this.Expect(this.Source).ToContainTextAsync("Tabs.Color.warning")
      do! this.Expect(this.Source).ToContainTextAsync("Density.spacious")
      do! this.Expect(this.Source).ToContainTextAsync("position = Tabs.Position.Left")
      do! this.Expect(this.Source).Not.ToContainTextAsync("Select.create")

      let! hasBoundary =
        this.Preview.EvaluateAsync<bool>(
          "el => { const style = getComputedStyle(el); return style.outlineStyle === 'solid' && parseFloat(style.outlineWidth) >= 1; }"
        )

      Assert.True(hasBoundary, "The native Tabs root should show its actual bounds without a layout wrapper")

      let! fits =
        this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

      Assert.True(fits, "Only the code block may scroll horizontally, not the configurator page")
    }

  [<Fact>]
  member this.``styling changes preserve the selected tabs instance``() = task {
    do! this.Load()

    do!
      this.Preview
        .GetByRole(AriaRole.Tab, LocatorGetByRoleOptions(Name = "Settings", Exact = true))
        .ClickAsync()

    let! original = this.Preview.ElementHandleAsync()
    do! this.Choose("Variant", "Filled")
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--filled"))
    do! this.Choose("Color", "Warning")
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--warning"))
    do! this.Choose("Density", "Spacious")
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-density--spacious"))
    do! this.Choose("Alignment", "Center")
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--centered"))
    do! this.Tray.ClickAsync()
    do! this.Expect(this.Preview.Locator("[role='tabpanel']:visible")).ToHaveTextAsync("Settings content")
    let! connected = original.EvaluateAsync<bool>("el => el.isConnected")
    Assert.True(connected, "Styling changes should not remount the selected Tabs component")
  }

  [<Fact>]
  member this.``colored tray is available only for filled tabs and clears on variant change``() = task {
    do! this.Load()
    do! this.Expect(this.Tray).ToBeDisabledAsync()
    do! this.Choose("Variant", "Filled")
    do! this.Expect(this.Tray).ToBeEnabledAsync()
    do! this.Tray.ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Tabs.HeaderBackground.primary")
    do! this.Choose("Variant", "Outlined")
    do! this.Expect(this.Tray).ToBeDisabledAsync()
    do! this.Expect(this.Tray).Not.ToBeCheckedAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("HeaderBackground")
  }

  [<Fact>]
  member this.``reset restores all appearance controls and the first panel``() = task {
    do! this.Load()
    do! this.OpenSource()
    let! initial = this.Source.TextContentAsync()

    do!
      this.Preview
        .GetByRole(AriaRole.Tab, LocatorGetByRoleOptions(Name = "Settings", Exact = true))
        .ClickAsync()

    do! this.Choose("Variant", "Filled")
    do! this.Choose("Color", "Success")
    do! this.Choose("Density", "Compact")
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-density--compact"))
    do! this.Expect(this.Source).ToContainTextAsync("Density.compact")
    do! this.Choose("Position", "Bottom")
    do! this.Choose("Alignment", "Center")
    do! this.Choose("Icons", "End")
    do! this.Tray.ClickAsync()
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Control("Variant")).ToContainTextAsync("Text")
    do! this.Expect(this.Control("Color")).ToContainTextAsync("Primary")
    do! this.Expect(this.Control("Density")).ToContainTextAsync("Standard")
    do! this.Expect(this.Control("Position")).ToContainTextAsync("Top")
    do! this.Expect(this.Control("Alignment")).ToContainTextAsync("Start")
    do! this.Expect(this.Control("Icons")).ToContainTextAsync("None")
    do! this.Expect(this.Tray).Not.ToBeCheckedAsync()
    do! this.Expect(this.Tray).ToBeDisabledAsync()
    do! this.Expect(this.Preview.Locator("[role='tabpanel']:visible")).ToHaveTextAsync("Overview content")
    do! this.Expect(this.Source).ToHaveTextAsync(initial)
  }

  [<Fact>]
  member this.``expanded code copies the current appearance and selected panel``() = task {
    let! _ =
      this.Page.AddInitScriptAsync(
        "Object.defineProperty(navigator, 'clipboard', { configurable: true, value: { writeText: async text => { window.copiedTabsSource = text; } } });"
      )

    do! this.Load()

    let codeRegion =
      this.Page.Locator(".docs-example__code .weave-expansion-panel__content-wrapper")

    do! this.Expect(codeRegion).ToHaveAttributeAsync("inert", "")
    do! this.Expect(codeRegion.Locator(".docs-copy-button")).ToHaveCountAsync(1)

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => document.querySelector('.docs-example__code .weave-expansion-panel__content-wrapper').getBoundingClientRect().height <= 1",
        null,
        PageWaitForFunctionOptions(Timeout = 3000.0f)
      )

    let! collapsedHeight =
      codeRegion.EvaluateAsync<float>("el => el.getBoundingClientRect().height")

    Assert.True(collapsedHeight <= 1.0, "Collapsed code and its Copy action must be clipped out of view")

    if this.Page.Context.Browser.BrowserType.Name = "chromium" then
      let! session = this.Page.Context.NewCDPSessionAsync(this.Page)
      let! tree = session.SendAsync("Accessibility.getFullAXTree")

      let isAccessibleCopy (node: System.Text.Json.JsonElement) =
        let mutable name = Unchecked.defaultof<System.Text.Json.JsonElement>
        let mutable value = Unchecked.defaultof<System.Text.Json.JsonElement>

        node.TryGetProperty("name", &name)
        && name.TryGetProperty("value", &value)
        && value.ValueKind = System.Text.Json.JsonValueKind.String
        && value.GetString() = "Copy"
        && not (node.GetProperty("ignored").GetBoolean())

      let accessibleCopy =
        tree.Value.GetProperty("nodes").EnumerateArray() |> Seq.exists isAccessibleCopy

      Assert.False(accessibleCopy, "Collapsed Copy must be absent from the browser accessibility tree")
      do! session.DetachAsync()

    do! this.OpenSource()
    do! this.Choose("Variant", "Filled")
    do! this.Choose("Position", "Right")

    do!
      this.Preview
        .GetByRole(AriaRole.Tab, LocatorGetByRoleOptions(Name = "Settings", Exact = true))
        .ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Hide Code", Exact = true)))
        .ToBeVisibleAsync()

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copy", Exact = true)).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copied", Exact = true)))
        .ToBeVisibleAsync()

    let! copied = this.Page.EvaluateAsync<string>("() => window.copiedTabsSource")
    Assert.Contains("let selectedTab = Var.Create 2", copied)
    Assert.Contains("position = Tabs.Position.Right", copied)
    Assert.Contains("Tabs.Variant.filled", copied)
    let! displayed = this.Source.TextContentAsync()
    Assert.Equal(displayed, copied)
  }

  [<Fact>]
  member this.``keyboard appearance selection preserves control focus``() = task {
    do! this.Load()
    let variant = this.Control("Variant")
    do! variant.FocusAsync()
    do! variant.PressAsync("Enter")
    do! variant.PressAsync("ArrowDown")
    do! variant.PressAsync("ArrowDown")
    do! variant.PressAsync("Enter")
    do! this.Expect(variant).ToHaveAttributeAsync("aria-expanded", "false")
    do! this.Expect(variant).ToBeFocusedAsync()
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--outlined"))
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Tabs.Variant.outlined")
  }

  [<Fact>]
  member this.``legacy appearance links retain one discoverable heading``() = task {
    do! this.Load()

    do!
      this
        .Expect(
          this.Page.GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = "Appearance", Exact = true))
        )
        .ToHaveCountAsync(1)

    for alias in [ "variants"; "colors"; "tab-positions"; "tabs-with-icons"; "centered-tabs" ] do
      let target = this.Page.Locator("#" + alias)
      do! this.Expect(target).ToHaveCountAsync(1)

      do!
        this
          .Expect(
            target.GetByRole(AriaRole.Heading, LocatorGetByRoleOptions(Name = "Appearance", Exact = true))
          )
          .ToHaveCountAsync(1)

      let! targetBox = target.BoundingBoxAsync()
      let! headingBox = this.Page.Locator("#appearance").BoundingBoxAsync()

      Assert.True(
        abs (targetBox.Y - headingBox.Y) <= 1.0f,
        "Legacy links should scroll to Appearance's heading"
      )
  }

  [<Theory>]
  [<InlineData("Top", "top", "attrs = [ Tabs.Color.primary ]")>]
  [<InlineData("Bottom", "bottom", "position = Tabs.Position.Bottom")>]
  [<InlineData("Left", "left", "position = Tabs.Position.Left")>]
  [<InlineData("Right", "right", "position = Tabs.Position.Right")>]
  [<InlineData("Start (logical)", "start", "position = Tabs.Position.Start")>]
  [<InlineData("End (logical)", "end", "position = Tabs.Position.End")>]
  member this.``appearance covers every former position example``
    (choice: string, className: string, source: string)
    = task {
    do! this.Load()
    do! this.Choose("Position", choice)
    do! this.OpenSource()
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--" + className))
    do! this.Expect(this.Source).ToContainTextAsync(source)
    do! this.Expect(this.Preview.Locator("[role='tabpanel']:visible")).ToHaveTextAsync("Overview content")
  }

  [<Theory>]
  [<InlineData("Start (logical)", "start", true)>]
  [<InlineData("End (logical)", "end", false)>]
  member this.``logical tab positions follow right to left direction``
    (choice: string, className: string, headerOnRight: bool)
    = task {
    do! this.Load()
    do! this.Choose("Position", choice)
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--" + className))

    let! isOnRight =
      this.Preview.EvaluateAsync<bool>(
        "tabs => { tabs.dir = 'rtl'; const header = tabs.querySelector('.weave-tabs__header-wrapper').getBoundingClientRect(); const panel = tabs.querySelector('.weave-tabs__panels').getBoundingClientRect(); return header.left >= panel.right - 1; }"
      )

    Assert.Equal(headerOnRight, isOnRight)
  }

  [<Theory>]
  [<InlineData(320, "Top", "Start", "startIcon")>]
  [<InlineData(1280, "Left", "End", "endIcon")>]
  member this.``alignment and icon options stay in sync with preview and source``
    (width: int, position: string, iconPosition: string, iconParameter: string)
    = task {
    do! this.Load(width = width)
    do! this.Choose("Position", position)
    do! this.Choose("Alignment", "Center")
    do! this.Choose("Icons", iconPosition)
    do! this.OpenSource()
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-tabs--centered"))
    do! this.Expect(this.Preview.Locator(".weave-tabs__tab-icon")).ToHaveCountAsync(3)
    do! this.Expect(this.Source).ToContainTextAsync("Tabs.Alignment.center")
    do! this.Expect(this.Source).ToContainTextAsync(iconParameter + " = Icon.create")
    do! this.Expect(this.Source).ToContainTextAsync("open Weave.Icons.MaterialSymbols")

    let! placement =
      this.Preview
        .Locator("[role='tab']")
        .First.EvaluateAsync<bool>(
          "(tab, before) => before ? tab.firstElementChild.classList.contains('weave-tabs__tab-icon') : tab.lastElementChild.classList.contains('weave-tabs__tab-icon')",
          box (iconPosition = "Start")
        )

    Assert.True(placement, "The icon should appear on the selected side of the label")

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits, "The expanded controls must fit the viewport")
  }
