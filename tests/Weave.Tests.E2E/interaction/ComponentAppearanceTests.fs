namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type ComponentAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Source =
    this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

  member private this.Choose(label: string, choice: string) = task {
    do! this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = label, Exact = true)).ClickAsync()
    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = choice, Exact = true)).ClickAsync()
  }

  member private this.Load(route: string, width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigatePage(this.Page, route)
    do! this.Expect(this.Page.Locator(".docs-appearance__preview")).ToBeVisibleAsync()

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToBeVisibleAsync()
  }

  member private this.Alias(id: string) = task {
    let target = this.Page.Locator("[id='" + id + "']")
    do! this.Expect(target).ToHaveCountAsync(1)
    do! this.Expect(target.GetByRole(AriaRole.Heading)).ToContainTextAsync("Appearance")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Card variant and actions match source and reset``(width: int) = task {
    do! this.Load("card-appearance", width)
    do! this.Alias("variants")
    do! this.Choose("Variant", "Elevated")

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Actions", Exact = true))
        .PressAsync("Space")

    do! this.Expect(this.Source).ToContainTextAsync("Card.Variant.elevated")
    do! this.Expect(this.Source).Not.ToContainTextAsync("CardActions.create")

    do!
      this
        .Expect(this.Page.Locator(".docs-appearance__preview").GetByRole(AriaRole.Button))
        .ToHaveCountAsync(0)

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("CardActions.create")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Card.Variant.elevated")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Button Group orientation and color match source and reset``(width: int) = task {
    do! this.Load("button-group-appearance", width)
    do! this.Alias("vertical-orientation")
    do! this.Alias("density-and-colors")
    do! this.Alias("colors")
    do! this.Choose("Orientation", "Vertical")
    do! this.Choose("Color", "Error")
    do! this.Expect(this.Source).ToContainTextAsync("ButtonGroup.Orientation.vertical")
    do! this.Expect(this.Source).ToContainTextAsync("ButtonGroup.Color.error")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("ButtonGroup.Orientation.vertical")
    do! this.Expect(this.Source).ToContainTextAsync("ButtonGroup.Color.primary")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Expansion Panel settings and state match source and reset``(width: int) = task {
    do! this.Load("expansion-panel-appearance", width)
    do! this.Alias("colors")
    do! this.Alias("focus-color")
    do! this.Alias("header-highlight-variants")
    do! this.Choose("Header", "Filled")
    do! this.Choose("Focus color", "Error")

    do! this.Page.Locator(".docs-appearance__preview .weave-expansion-panel__header").ClickAsync()

    do! this.Expect(this.Source).ToContainTextAsync("ExpansionPanel.HeaderVariant.Filled")
    do! this.Expect(this.Source).ToContainTextAsync("ExpansionPanel.FocusColor.error")
    do! this.Expect(this.Source).ToContainTextAsync("let expanded = Var.Create true")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("let expanded = Var.Create false")
    do! this.Expect(this.Source).Not.ToContainTextAsync("ExpansionPanel.HeaderVariant.Filled")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Button Menu direction and icon swap match source and reset``(width: int) = task {
    do! this.Load("button-menu-appearance", width)
    do! this.Alias("directions")
    do! this.Alias("icon-swap-vs-rotation")
    do! this.Choose("Direction", "Bottom")
    do! this.Choose("Variant", "Outlined")

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Swap open icon", Exact = true))
        .PressAsync("Space")

    do! this.Expect(this.Source).ToContainTextAsync("ButtonMenu.Direction.Bottom")
    do! this.Expect(this.Source).ToContainTextAsync("Button.Variant.outlined")
    do! this.Expect(this.Source).ToContainTextAsync("openIcon =")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Menu", Exact = true)).ClickAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Menu)).ToBeVisibleAsync()
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("ButtonMenu.Direction.Bottom")
    do! this.Expect(this.Source).Not.ToContainTextAsync("openIcon =")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``App Bar positioning and accent match source and reset``(width: int) = task {
    do! this.Load("app-bar-appearance", width)
    do! this.Alias("position")
    do! this.Choose("Position", "Fixed bottom")

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Primary color", Exact = true))
        .PressAsync("Space")

    do! this.Expect(this.Source).ToContainTextAsync("AppBar.Position.fixedBottom")
    do! this.Expect(this.Source).ToContainTextAsync("--palette-appbar-background")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("AppBar.Position.fixedBottom")
    do! this.Expect(this.Source).Not.ToContainTextAsync("--palette-appbar-background")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Dialog configuration supports optional escape and matching source``(width: int) = task {
    do! this.Load("dialog-appearance", width)
    do! this.Alias("optional-dialog")
    do! this.Alias("dialog-positions")
    do! this.Choose("Position", "Top center")
    do! this.Choose("Interaction", "Optional")

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Blur background", Exact = true))
        .PressAsync("Space")

    do! this.Expect(this.Source).ToContainTextAsync("Dialog.DialogPosition.TopCenter")
    do! this.Expect(this.Source).ToContainTextAsync("Dialog.Interaction.Optional close")
    do! this.Expect(this.Source).ToContainTextAsync("blurBackground = View.Const false")

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Open dialog", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync(0)
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("Dialog.Interaction.Optional")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Drawer mode side and clipping match generated source``(width: int) = task {
    do! this.Load("drawer-appearance", width)
    do! this.Alias("temporary")
    do! this.Alias("persistent")
    do! this.Alias("responsive")
    do! this.Alias("clip-mode")
    do! this.Choose("Variant", "Persistent")
    do! this.Choose("Position", "Right")
    do! this.Choose("Clip mode", "Below app bar")
    do! this.Expect(this.Source).ToContainTextAsync("variant = Drawer.Variant.Persistent")
    do! this.Expect(this.Source).ToContainTextAsync("rightDrawer =")
    do! this.Expect(this.Source).ToContainTextAsync("clipMode = Drawer.ClipMode.AppBar")
    do! this.Expect(this.Source).Not.ToContainTextAsync("overlayClose =")
    let preview = this.Page.Locator(".docs-appearance__preview")
    do! preview.Locator("[aria-label='Open drawer'][aria-expanded='false']").ClickAsync()

    do! this.Expect(this.Source).ToContainTextAsync("let isOpen = Var.Create true")

    do!
      preview
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Activity", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToContainTextAsync("removed for brevity")

    do!
      this
        .Expect(
          preview
            .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Close drawer", Exact = true))
            .First
        )
        .ToBeVisibleAsync()

    do!
      preview
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Close drawer", Exact = true))
        .First.ClickAsync()

    do! this.Expect(this.Source).ToContainTextAsync("let isOpen = Var.Create false")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("removed for brevity")
    do! this.Expect(this.Source).Not.ToContainTextAsync("variant = Drawer.Variant.Temporary")
    do! this.Expect(this.Source).ToContainTextAsync("overlayClose =")
    do! this.Expect(this.Source).ToContainTextAsync("leftDrawer =")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Carousel controls and selection match source and reset``(width: int) = task {
    do! this.Load("carousel-appearance", width)
    do! this.Alias("automatic-cycling")

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next slide", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToContainTextAsync("let selected = Var.Create 1")

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Show arrows", Exact = true))
        .PressAsync("Space")

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Automatic cycling", Exact = true))
        .PressAsync("Space")

    do! this.Expect(this.Source).ToContainTextAsync("Carousel.Controls.hideArrows")
    do! this.Expect(this.Source).ToContainTextAsync("autoCycleInterval = 5000")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("Carousel.Controls.hideArrows")
    do! this.Expect(this.Source).Not.ToContainTextAsync("autoCycleInterval = 5000")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Grid builder spacing matches source and reset``(width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigatePage(this.Page, "grid-builder")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show layout source")).ClickAsync()

    let source =
      this.Page.Locator("pre[aria-label='Selected grid layout F# source code'] code")

    do! this.Expect(source).ToBeVisibleAsync()
    do! this.Choose("Spacing", "Extra large")
    do! this.Expect(source).ToContainTextAsync("Grid.Spacing.extraLarge")
    do! this.Expect(this.Page.Locator(".docs-example__preview .weave-grid--spacing-20")).ToHaveCountAsync(1)

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset layout", Exact = true))
        .ClickAsync()

    do! this.Expect(source).ToContainTextAsync("Grid.Spacing.medium")
    do! this.Expect(this.Page.Locator(".docs-example__preview .weave-grid--spacing-8")).ToHaveCountAsync(1)
  }

  [<Theory>]
  [<InlineData("drawer-documentation", "choosing-a-variant", "Choosing a Variant", "Responsive")>]
  member this.``restored decision guidance retains its heading and link``
    (route: string, id: string, heading: string, guidance: string)
    = task {
    do! this.Page.SetViewportSizeAsync(1280, 900)
    do! this.NavigatePage(this.Page, route)
    let target = this.Page.Locator("[id='" + id + "']")
    do! this.Expect(target).ToHaveCountAsync(1)

    do!
      this
        .Expect(target.GetByRole(AriaRole.Heading, LocatorGetByRoleOptions(Name = heading, Exact = true)))
        .ToHaveCountAsync(1)

    do!
      this.Expect(this.Page.GetByText(guidance, PageGetByTextOptions(Exact = true)).First).ToBeVisibleAsync()
  }
