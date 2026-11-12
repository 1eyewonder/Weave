namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type ButtonAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  let minimal =
    "Button.primary (\n  text \"Save changes\",\n  onClick = (fun () -> ()),\n  attrs = [ Button.Variant.filled ]\n)"

  member private this.Preview =
    this.Page.Locator(".docs-appearance__preview").GetByRole(AriaRole.Button)

  member private this.Source =
    this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

  member private this.Control(label) =
    this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = label, Exact = true))

  member private this.FullWidth =
    this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Full width", Exact = true))

  member private this.Disabled =
    this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Disabled", Exact = true))

  member private this.Choose(label, choice) = task {
    do! this.Control(label).ClickAsync()
    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = choice, Exact = true)).ClickAsync()
  }

  member private this.Load(?width: int) = task {
    do! this.Page.SetViewportSizeAsync(defaultArg width 1280, 900)
    do! this.NavigatePage(this.Page, "button-appearance")

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

  [<Theory>]
  [<InlineData("variants")>]
  [<InlineData("colors")>]
  [<InlineData("full-width")>]
  [<InlineData("border-radius")>]
  [<InlineData("disabled")>]
  member this.``legacy appearance anchors retain a single destination with the configurator``(id: string) =
    task {
      do! this.Load()
      let destination = this.Page.Locator("[id='" + id + "']")
      do! this.Expect(destination).ToHaveCountAsync(1)
      do! this.Expect(destination.GetByRole(AriaRole.Heading)).ToContainTextAsync("Appearance")
      do! this.Expect(this.Control("Variant")).ToBeVisibleAsync()
      do! this.Expect(this.Control("Corners")).ToBeVisibleAsync()
      do! this.Expect(this.FullWidth).ToBeVisibleAsync()
    }

  [<Fact>]
  member this.``default appearance is filled with minimal copyable source``() = task {
    do! this.Load()
    do! this.OpenSource()
    do! this.Expect(this.Page.GetByRole(AriaRole.Combobox)).ToHaveCountAsync(4)
    do! this.Expect(this.Preview).ToHaveAccessibleNameAsync("Save changes")
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-button--filled"))
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-button--primary"))
    do! this.Expect(this.FullWidth).Not.ToBeCheckedAsync()
    do! this.Expect(this.Disabled).Not.ToBeCheckedAsync()
    do! this.Expect(this.Preview).ToBeEnabledAsync()
    do! this.Expect(this.Source).ToHaveTextAsync(minimal)
  }

  [<Theory>]
  [<InlineData("Primary", "primary")>]
  [<InlineData("Secondary", "secondary")>]
  [<InlineData("Tertiary", "tertiary")>]
  [<InlineData("Error", "error")>]
  [<InlineData("Warning", "warning")>]
  [<InlineData("Success", "success")>]
  [<InlineData("Info", "info")>]
  member this.``each named color updates the native button and source shorthand``
    (choice: string, color: string)
    = task {
    do! this.Load()
    do! this.OpenSource()
    do! this.Choose("Color", choice)
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-button--" + color))
    do! this.Expect(this.Source).ToContainTextAsync("Button." + color + " (")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Button.Color.")
  }

  [<Theory>]
  [<InlineData("None", "weave-rounded-none", "BorderRadius.All.none")>]
  [<InlineData("Small", "weave-rounded-sm", "BorderRadius.All.small")>]
  [<InlineData("Medium", "weave-rounded", "BorderRadius.All.medium")>]
  [<InlineData("Large", "weave-rounded-lg", "BorderRadius.All.large")>]
  [<InlineData("Pill", "weave-rounded-pill", "BorderRadius.pill")>]
  member this.``corner selection updates the native shape and source``
    (choice: string, cssClass: string, source: string)
    = task {
    do! this.Load()
    do! this.OpenSource()
    do! this.Choose("Corners", choice)
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex(@"(?:^|\s)" + Regex.Escape(cssClass) + @"(?:\s|$)"))
    do! this.Expect(this.Source).ToContainTextAsync(source)
  }

  [<Fact>]
  member this.``density changes the actual button height and generated attribute``() = task {
    do! this.Load()
    do! this.OpenSource()
    do! this.Choose("Density", "Compact")
    do! this.Expect(this.Source).ToContainTextAsync("Density.compact")
    let! compact = this.Preview.BoundingBoxAsync()
    do! this.Choose("Density", "Spacious")
    do! this.Expect(this.Source).ToContainTextAsync("Density.spacious")
    let! spacious = this.Preview.BoundingBoxAsync()
    Assert.True(spacious.Height > compact.Height + 8.0f, "Spacious must visibly increase button height")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``combined styles and full width fit the viewport with matching source``(width: int) = task {
    do! this.Load(width = width)
    let! natural = this.Preview.BoundingBoxAsync()
    do! this.Choose("Variant", "Text")
    do! this.Choose("Color", "Secondary")
    do! this.Choose("Density", "Compact")
    do! this.Choose("Corners", "Pill")
    do! this.FullWidth.ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-button--text"))
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-button--full-width"))
    do! this.Expect(this.Source).ToContainTextAsync("Button.secondary (")
    do! this.Expect(this.Source).ToContainTextAsync("Button.Variant.text")
    do! this.Expect(this.Source).ToContainTextAsync("Density.compact")
    do! this.Expect(this.Source).ToContainTextAsync("BorderRadius.pill")
    do! this.Expect(this.Source).ToContainTextAsync("Button.Width.full")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Select.create")
    let! full = this.Preview.BoundingBoxAsync()
    Assert.True(full.Width > natural.Width + 20.0f, "Full width must expand within the preview")

    let! fits =
      this.Page.EvaluateAsync<bool>(
        """() => document.documentElement.scrollWidth <= innerWidth &&
          [...document.querySelectorAll('[role="combobox"], [role="switch"], .docs-appearance__actions button')]
            .every(el => { const r = el.getBoundingClientRect(); return r.left >= 0 && r.right <= innerWidth; })"""
      )

    Assert.True(fits, "The preview and every control must fit; only the code block may scroll")
  }

  [<Fact>]
  member this.``reset restores all controls the native preview and minimal source``() = task {
    do! this.Load()
    do! this.OpenSource()
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Color", "Success")
    do! this.Choose("Density", "Spacious")
    do! this.Choose("Corners", "Large")
    do! this.FullWidth.ClickAsync()
    do! this.Disabled.ClickAsync()
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Control("Variant")).ToContainTextAsync("Filled")
    do! this.Expect(this.Control("Color")).ToContainTextAsync("Primary")
    do! this.Expect(this.Control("Density")).ToContainTextAsync("Standard")
    do! this.Expect(this.Control("Corners")).ToContainTextAsync("Default")
    do! this.Expect(this.FullWidth).Not.ToBeCheckedAsync()
    do! this.Expect(this.Disabled).Not.ToBeCheckedAsync()
    do! this.Expect(this.Preview).ToBeEnabledAsync()
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-button--filled"))
    do! this.Expect(this.Preview).Not.ToHaveClassAsync(Regex("weave-button--full-width"))
    do! this.Expect(this.Source).ToHaveTextAsync(minimal)
  }

  [<Fact>]
  member this.``disabled appearance updates the preview and copied source``() = task {
    let! _ =
      this.Page.AddInitScriptAsync(
        "Object.defineProperty(navigator, 'clipboard', { configurable: true, value: { writeText: async text => { window.copiedButtonSource = text; } } });"
      )

    do! this.Load()
    do! this.Choose("Variant", "Outlined")
    do! this.Disabled.ClickAsync()
    do! this.Expect(this.Preview).ToBeDisabledAsync()
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("enabled = View.Const false")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copy", Exact = true)).ClickAsync()
    let! copied = this.Page.EvaluateAsync<string>("() => window.copiedButtonSource")
    Assert.Contains("enabled = View.Const false", copied)
    Assert.Contains("Button.Variant.outlined", copied)
    do! this.Disabled.FocusAsync()
    do! this.Disabled.PressAsync("Space")
    do! this.Expect(this.Preview).ToBeEnabledAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("enabled = View.Const false")
  }

  [<Fact>]
  member this.``copy uses the latest selection after opening its code``() = task {
    let! _ =
      this.Page.AddInitScriptAsync(
        "Object.defineProperty(navigator, 'clipboard', { configurable: true, value: { writeText: async text => { window.copiedButtonSource = text; } } });"
      )

    do! this.Load()
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Color", "Info")
    do! this.Choose("Corners", "Small")

    let disclosure =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))

    do! this.Expect(disclosure).ToHaveAttributeAsync("aria-expanded", "false")
    do! this.OpenSource()
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copy", Exact = true)).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copied", Exact = true)))
        .ToBeVisibleAsync()

    let! copied = this.Page.EvaluateAsync<string>("() => window.copiedButtonSource")
    Assert.Contains("Button.info (", copied)
    Assert.Contains("Button.Variant.outlined", copied)
    Assert.Contains("BorderRadius.All.small", copied)
    let! displayed = this.Source.TextContentAsync()
    Assert.Equal(displayed, copied)
  }

  [<Fact>]
  member this.``keyboard selection updates appearance and retains control focus``() = task {
    do! this.Load()
    let variant = this.Control("Variant")
    do! variant.FocusAsync()
    do! variant.PressAsync("Enter")
    do! variant.PressAsync("ArrowDown")
    do! variant.PressAsync("ArrowDown")
    do! variant.PressAsync("Enter")
    do! this.Expect(variant).ToHaveAttributeAsync("aria-expanded", "false")
    do! this.Expect(variant).ToBeFocusedAsync()
    do! this.Expect(this.Preview).ToHaveClassAsync(Regex("weave-button--outlined"))
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Button.Variant.outlined")
  }

[<Collection("Browser input")>]
type IconButtonAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``icon configuration updates preview source and reset``(width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigatePage(this.Page, "icon-button-appearance")

    let preview =
      this.Page.Locator(".docs-appearance__preview").GetByRole(AriaRole.Button)

    let control label =
      this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = label, Exact = true))

    let choose label value = task {
      do! (control label).ClickAsync()
      do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = value, Exact = true)).ClickAsync()
    }

    do! this.Expect(preview).ToHaveAccessibleNameAsync("Favorite")
    do! choose "Icon variant" "Outlined"
    do! choose "Icon color" "Success"
    do! choose "Icon density" "Compact"
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Circle", Exact = true)).ClickAsync()

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Icon disabled", Exact = true))
        .ClickAsync()

    do! this.Expect(preview).ToBeDisabledAsync()
    do! this.Expect(preview).ToHaveClassAsync(Regex("weave-button--outlined"))
    do! this.Expect(preview).ToHaveClassAsync(Regex("weave-button--success"))

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    let source =
      this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

    do! this.Expect(source).ToContainTextAsync("IconButton.success (")
    do! this.Expect(source).ToContainTextAsync("Button.Variant.outlined")
    do! this.Expect(source).ToContainTextAsync("Density.compact")
    do! this.Expect(source).ToContainTextAsync("BorderRadius.circle")
    do! this.Expect(source).ToContainTextAsync("enabled = View.Const false")
    do! this.Expect(source).ToContainTextAsync("aria-label")

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits, "The configurator should fit the viewport")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(preview).ToBeEnabledAsync()
    do! this.Expect(source).ToContainTextAsync("IconButton.primary (")
    do! this.Expect(source).Not.ToContainTextAsync("Density.compact")
    do! this.Expect(source).Not.ToContainTextAsync("BorderRadius.circle")
  }
