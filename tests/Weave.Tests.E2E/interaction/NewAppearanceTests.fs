namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type NewAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Preview = this.Page.Locator(".docs-appearance__preview")
  member private this.Settings = this.Page.Locator(".docs-appearance__settings")

  member private this.Source =
    this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

  member private this.Load(page: string, width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigatePage(this.Page, page)
    do! this.Expect(this.Preview).ToBeVisibleAsync()
  }

  member private this.Choose(label: string, choice: string) = task {
    do!
      this.Settings
        .GetByRole(AriaRole.Combobox, LocatorGetByRoleOptions(Name = label, Exact = true))
        .ClickAsync()

    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = choice, Exact = true)).ClickAsync()
  }

  member private this.OpenSource() = task {
    do!
      this.Page
        .Locator(".docs-appearance")
        .Locator("..")
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData("docs-alert", 320)>]
  [<InlineData("docs-checkbox", 320)>]
  [<InlineData("docs-switch", 320)>]
  [<InlineData("docs-radio", 320)>]
  [<InlineData("docs-chip", 320)>]
  [<InlineData("docs-chipset", 320)>]
  [<InlineData("docs-numeric", 320)>]
  [<InlineData("docs-select", 320)>]
  [<InlineData("docs-colorpicker", 320)>]
  [<InlineData("docs-alert", 1280)>]
  [<InlineData("docs-checkbox", 1280)>]
  [<InlineData("docs-switch", 1280)>]
  [<InlineData("docs-radio", 1280)>]
  [<InlineData("docs-chip", 1280)>]
  [<InlineData("docs-chipset", 1280)>]
  [<InlineData("docs-numeric", 1280)>]
  [<InlineData("docs-select", 1280)>]
  [<InlineData("docs-colorpicker", 1280)>]
  member this.``configurator fits available viewport``(page: string, width: int) = task {
    do! this.Load(page, width)

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits, page + " must fit the viewport")
  }

  [<Fact>]
  member this.``alert style and icon are reflected in the live source``() = task {
    do! this.Load("docs-alert", 1280)
    do! this.Choose("Variant", "Filled")
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Leading icon")).ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Preview.Locator(".weave-alert")).ToHaveClassAsync(Regex("weave-alert--filled"))
    do! this.Expect(this.Source).ToContainTextAsync("Alert.Variant.filled")
    do! this.Expect(this.Source).ToContainTextAsync("UiActions.CheckCircle")
  }

  [<Theory>]
  [<InlineData("docs-checkbox", "Checkbox")>]
  [<InlineData("docs-switch", "Switch")>]
  member this.``toggle state and color agree with source and reset``(page: string, name: string) = task {
    do! this.Load(page, 1280)
    do! this.Preview.GetByText("Email updates", LocatorGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Choose("Color", "Secondary")
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create true")
    do! this.Expect(this.Source).ToContainTextAsync(name + ".Color.secondary")

    do!
      this.Settings
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Reset", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToContainTextAsync("Var.Create false")
    do! this.Expect(this.Source).Not.ToContainTextAsync(name + ".Color.secondary")
  }

  [<Theory>]
  [<InlineData("docs-checkbox", ".weave-checkbox", 320)>]
  [<InlineData("docs-switch", ".weave-switch", 320)>]
  [<InlineData("docs-radio", ".weave-radio", 320)>]
  [<InlineData("docs-checkbox", ".weave-checkbox", 1280)>]
  [<InlineData("docs-switch", ".weave-switch", 1280)>]
  [<InlineData("docs-radio", ".weave-radio", 1280)>]
  member this.``density preview outlines the changing control without adding source styling``
    (page: string, selector: string, width: int)
    = task {
    do! this.Load(page, width)
    let control = this.Preview.Locator(selector).First
    do! this.Expect(control).ToHaveClassAsync(Regex("docs-appearance__bounds"))
    do! this.Choose("Density", "Compact")
    do! this.Expect(control).ToHaveClassAsync(Regex("weave-density--compact"))
    let! compact = control.BoundingBoxAsync()
    do! this.Choose("Density", "Spacious")
    do! this.Expect(control).ToHaveClassAsync(Regex("weave-density--spacious"))
    let! spacious = control.BoundingBoxAsync()
    Assert.True(spacious.Height > compact.Height, page + " density should change the outlined height")
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Density.spacious")
    do! this.Expect(this.Source).Not.ToContainTextAsync("docs-appearance__bounds")
    do! this.Expect(this.Page.Locator("#density")).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``radio selection and settings appear in source``() = task {
    do! this.Load("docs-radio", 1280)
    do! this.Preview.GetByText("Express", LocatorGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Choose("Label placement", "Top")
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create \"Express\"")
    do! this.Expect(this.Source).ToContainTextAsync("Radio.ContentPlacement.top")
    do! this.Expect(this.Source).ToContainTextAsync("appearance-delivery")
    do! this.Expect(this.Source).ToContainTextAsync("fieldset [] [")
    do! this.Expect(this.Source).ToContainTextAsync("legend [] [ text \"Delivery\" ]")
    do! this.Expect(this.Source).ToContainTextAsync("option \"Standard\"")
    do! this.Expect(this.Source).ToContainTextAsync("option \"Express\"")
  }

  [<Fact>]
  member this.``chip corner choice appears in preview and source``() = task {
    do! this.Load("docs-chip", 1280)
    do! this.Choose("Corners", "Label")
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Selected")).ClickAsync()
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Disabled")).ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Preview.Locator(".weave-chip")).ToHaveClassAsync(Regex("weave-chip--label"))
    do! this.Expect(this.Preview.Locator(".weave-chip")).ToHaveClassAsync(Regex("weave-chip--selected"))
    do! this.Expect(this.Preview.Locator(".weave-chip")).ToHaveAttributeAsync("aria-pressed", "true")
    do! this.Expect(this.Preview.Locator(".weave-chip")).ToHaveClassAsync(Regex("weave-chip--disabled"))
    do! this.Expect(this.Source).ToContainTextAsync("Chip.Shape.label")
    do! this.Expect(this.Source).ToContainTextAsync("let selected = Var.Create true")
    do! this.Expect(this.Source).ToContainTextAsync("enabled = View.Const false")
  }

  [<Fact>]
  member this.``chip activation updates selected state and disabled blocks it``() = task {
    do! this.Load("docs-chip", 1280)

    let chip =
      this.Preview.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Ready"))

    do! this.Expect(chip).ToHaveAttributeAsync("aria-pressed", "false")
    do! chip.ClickAsync()
    do! this.Expect(chip).ToHaveAttributeAsync("aria-pressed", "true")
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("let selected = Var.Create true")
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Disabled")).ClickAsync()
    do! this.Expect(chip).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Expect(chip).ToHaveAttributeAsync("tabindex", "-1")
    let! _ = chip.EvaluateAsync("el => el.click()")
    do! this.Expect(chip).ToHaveAttributeAsync("aria-pressed", "true")
    do! this.Expect(this.Source).ToContainTextAsync("enabled = View.Const false")
  }

  [<Fact>]
  member this.``chip set selection is captured in source``() = task {
    do! this.Load("docs-chipset", 1280)
    do! this.Preview.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Option B")).ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Some \"b\"")
    do! this.Expect(this.Source).ToContainTextAsync("ChipSet.SelectionMode.Toggle")
  }

  [<Fact>]
  member this.``numeric spin control is reflected in source``() = task {
    do! this.Load("docs-numeric", 1280)

    do!
      this.Settings
        .GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Spin buttons"))
        .PressAsync("Space")

    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("showSpinButtons = View.Const false")
  }

  [<Fact>]
  member this.``select appearance respects dependent label and menu settings``() = task {
    do! this.Load("docs-select", 1280)
    do! this.Choose("Variant", "Outlined")
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Label above")).ClickAsync()
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Unbound menu")).ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Select.Label.above")
    do! this.Expect(this.Source).ToContainTextAsync("Select.PopoverWidth.unbound")
  }

  [<Fact>]
  member this.``picker format and controls are reflected in source``() = task {
    do! this.Load("docs-colorpicker", 1280)
    do! this.Choose("Format", "RGB")

    do!
      this.Settings
        .GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Hide format tabs"))
        .ClickAsync()

    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("ColorPicker.Mode.RGB")
    do! this.Expect(this.Source).ToContainTextAsync("ColorPicker.Controls.hideFormatTabs")
  }

  [<Theory>]
  [<InlineData("docs-container", 320)>]
  [<InlineData("docs-field", 320)>]
  [<InlineData("docs-dropdown", 320)>]
  [<InlineData("docs-scrolltop", 320)>]
  [<InlineData("docs-spacer", 320)>]
  [<InlineData("docs-snackbar", 320)>]
  [<InlineData("docs-container", 1280)>]
  [<InlineData("docs-field", 1280)>]
  [<InlineData("docs-dropdown", 1280)>]
  [<InlineData("docs-scrolltop", 1280)>]
  [<InlineData("docs-spacer", 1280)>]
  [<InlineData("docs-snackbar", 1280)>]
  member this.``additional configurators fit the viewport``(page: string, width: int) = task {
    do! this.Load(page, width)

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits, page + " must fit the viewport")
  }

  [<Fact>]
  member this.``container gutters update preview and source``() = task {
    do! this.Load("docs-container", 1280)
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Gutters")).ClickAsync()
    do! this.OpenSource()

    do!
      this
        .Expect(this.Preview.Locator(".weave-container"))
        .ToHaveClassAsync(Regex("weave-container--gutters"))

    do! this.Expect(this.Source).ToContainTextAsync("Container.gutters")
  }

  [<Fact>]
  member this.``field custom input and outlined label appear in source``() = task {
    do! this.Load("docs-field", 1280)
    do! this.Choose("Variant", "Outlined")
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Label above")).ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Field.Label.above")
    do! this.Expect(this.Source).ToContainTextAsync("on.focus")
    do! this.Expect(this.Source).ToContainTextAsync("shouldFloat")
  }

  [<Fact>]
  member this.``dropdown unbound menu expands beyond its trigger``() = task {
    do! this.Load("docs-dropdown", 1280)
    do! this.Settings.GetByRole(AriaRole.Switch, LocatorGetByRoleOptions(Name = "Unbound menu")).ClickAsync()

    let trigger =
      this.Preview.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Actions"))

    do! trigger.ClickAsync()
    let menu = this.Preview.Locator(".weave-dropdownmenu__popover")
    do! this.Expect(menu).ToBeVisibleAsync()
    let! triggerBounds = trigger.BoundingBoxAsync()
    let! menuBounds = menu.BoundingBoxAsync()
    Assert.True(menuBounds.Width > triggerBounds.Width + 30.0f)
    do! trigger.ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("DropdownMenu.PopoverWidth.unbound")
  }

  [<Fact>]
  member this.``scroll threshold is reflected in source``() = task {
    do! this.Load("docs-scrolltop", 1280)
    do! this.Choose("Reveal after", "100 px")
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("topOffset = 100")
    do! this.Expect(this.Source).ToContainTextAsync("selector =")
  }

  [<Fact>]
  member this.``spacer count and direction change layout and code``() = task {
    do! this.Load("docs-spacer", 1280)
    do! this.Choose("Direction", "Column")
    do! this.Choose("Spacers", "One")
    do! this.Expect(this.Preview.Locator(".weave-spacer")).ToHaveCountAsync(1)
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("FlexDirection.Column.allSizes")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Three")
  }

  [<Fact>]
  member this.``snackbar controls update a visible notification and source``() = task {
    do! this.Load("docs-snackbar", 1280)
    do! this.Choose("Color", "Warning")

    do!
      this.Preview
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Show notification"))
        .ClickAsync()

    do!
      this
        .Expect(this.Page.GetByText("Your changes have been saved.", PageGetByTextOptions(Exact = true)))
        .ToBeVisibleAsync()

    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Snackbar.Color.warning")
    do! this.Expect(this.Source).ToContainTextAsync("Doc.Concat [")
    do! this.Expect(this.Source).ToContainTextAsync("Button.primary")
    do! this.Expect(this.Source).ToContainTextAsync("Snackbar.create")
  }

  [<Theory>]
  [<InlineData("docs-numeric", "show-/-hide-spin-buttons")>]
  [<InlineData("docs-chipset", "toggle-selection")>]
  [<InlineData("docs-chip", "disabled")>]
  member this.``consolidated section links still point to appearance``(page: string, id: string) = task {
    do! this.Load(page, 1280)
    let section = this.Page.Locator("[id='" + id + "']")
    do! this.Expect(section).ToHaveCountAsync(1)
    do! this.Expect(section.GetByRole(AriaRole.Heading)).ToContainTextAsync("Appearance")
  }
