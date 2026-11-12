namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright
open Deque.AxeCore.Commons
open Deque.AxeCore.Playwright

[<Collection("Browser input")>]
type TextFieldAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  let minimal =
    "let name = Var.Create \"\"\n\nTextField.singleLine (name, labelText = View.Const \"Name\")"

  member private this.Input =
    this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = "Name", Exact = true))

  member private this.Field = this.Page.Locator(".docs-appearance__preview .weave-field")

  member private this.Source =
    this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

  member private this.Control(label) =
    this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = label, Exact = true))

  member private this.Choose(label, choice) = task {
    do! this.Control(label).ClickAsync()
    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = choice, Exact = true)).ClickAsync()
  }

  member private this.Load(?width: int) = task {
    do! this.Page.SetViewportSizeAsync(defaultArg width 1280, 900)
    do! this.NavigatePage(this.Page, "text-field-appearance")

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => ['appearance-docs-style', 'appearance-highlighting-style'].every(id => document.getElementById(id)?.sheet)"
      )

    do! this.Expect(this.Input).ToBeVisibleAsync()

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``default appearance has minimal source and usable labelled controls``() = task {
    do! this.Load()
    do! this.Expect(this.Page.GetByRole(AriaRole.Combobox)).ToHaveCountAsync(5)
    do! this.Expect(this.Source).ToHaveTextAsync(minimal)
    do! this.Input.FillAsync("Ada")
    do! this.Expect(this.Input).ToHaveValueAsync("Ada")
  }

  [<Theory>]
  [<InlineData("Filled", "filled")>]
  [<InlineData("Outlined", "outlined")>]
  member this.``variant changes preserve typed content and update the component source``
    (choice: string, variant: string)
    = task {
    do! this.Load()
    do! this.Input.FillAsync("Ada")
    do! this.Choose("Variant", choice)
    do! this.Expect(this.Input).ToHaveValueAsync("Ada")
    do! this.Expect(this.Field).ToHaveClassAsync(Regex("weave-field--" + variant))
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Variant." + variant)
    do! this.Expect(this.Source).Not.ToContainTextAsync("TextField.Color.primary")
  }

  [<Fact>]
  member this.``source preserves and escapes the entered value``() = task {
    do! this.Load()
    do! this.Input.FillAsync("Ada \"Q\" \\ café 👍")
    do! this.Expect(this.Source).ToContainTextAsync("Ada \\\"Q\\\" \\\\ café")
    do! this.Expect(this.Source).ToContainTextAsync("\\uD83D\\uDC4D")
    do! this.Input.FillAsync(System.String('a', 100))
    do! this.Expect(this.Source).ToContainTextAsync("let name =\n  Var.Create (")
  }

  [<Fact>]
  member this.``keyboard selection updates the preview while retaining control focus``() = task {
    do! this.Load()
    let variant = this.Control("Variant")
    do! variant.FocusAsync()
    do! variant.PressAsync("Enter")
    do! variant.PressAsync("ArrowDown")
    do! variant.PressAsync("ArrowDown")
    do! variant.PressAsync("Enter")
    do! this.Expect(variant).ToHaveAttributeAsync("aria-expanded", "false")
    do! this.Expect(variant).ToBeFocusedAsync()
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Variant.filled")
  }

  [<Fact>]
  member this.``color selection changes the native accent without adding default settings``() = task {
    do! this.Load()
    do! this.Choose("Color", "Secondary")
    do! this.Expect(this.Field).ToHaveClassAsync(Regex("weave-field--secondary"))
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Color.secondary")
    do! this.Expect(this.Source).Not.ToContainTextAsync("TextField.Variant.standard")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Select.create")
  }

  [<Theory>]
  [<InlineData("Standard")>]
  [<InlineData("Filled")>]
  [<InlineData("Outlined")>]
  member this.``caret follows the field accent for each color``(variant: string) = task {
    do! this.Load()

    if variant <> "Standard" then
      do! this.Choose("Variant", variant)

    for label, color in
      [
        "Primary", "primary"
        "Secondary", "secondary"
        "Tertiary", "tertiary"
        "Error", "error"
        "Warning", "warning"
        "Success", "success"
        "Info", "info"
      ] do
      do! this.Choose("Color", label)
      do! this.Input.FocusAsync()

      if color <> "primary" then
        do! this.Expect(this.Field).ToHaveClassAsync(Regex("weave-field--" + color))

      let! _ =
        this.Page.WaitForFunctionAsync(
          "color => { const input = document.querySelector('.docs-appearance__preview .weave-field__input'); if (!input) return false; const palette = getComputedStyle(input).getPropertyValue('--palette-' + color).trim(); if (!palette) return false; const canvas = document.createElement('canvas').getContext('2d'); canvas.fillStyle = palette; const expected = canvas.fillStyle; canvas.fillStyle = getComputedStyle(input).caretColor; return canvas.fillStyle === expected; }",
          color
        )

      ()

  }

  [<Fact>]
  member this.``full width changes the preview and generated public attribute``() = task {
    do! this.Load()
    let! initial = this.Field.BoundingBoxAsync()
    do! this.Choose("Width", "Full")
    do! this.Expect(this.Field).ToHaveClassAsync(Regex("weave-field--full-width"))
    let! full = this.Field.BoundingBoxAsync()
    Assert.True(full.Width > initial.Width + 20.0f, "Full width should expand within the preview")
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Width.full")
  }

  [<Fact>]
  member this.``above labels are offered only for outlined fields and reset on variant change``() = task {
    do! this.Load()
    do! this.Control("Label position").ClickAsync()

    let above =
      this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = "Above (outlined)", Exact = true))

    do! this.Expect(above).ToHaveAttributeAsync("aria-disabled", "true")
    do! this.Control("Label position").PressAsync("Escape")
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Label position", "Above (outlined)")
    do! this.Expect(this.Field).ToHaveClassAsync(Regex("weave-field--label-above"))
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Label.above")
    do! this.Choose("Variant", "Filled")
    do! this.Expect(this.Control("Label position")).ToContainTextAsync("Floating")
    do! this.Expect(this.Source).Not.ToContainTextAsync("TextField.Label.above")
  }

  [<Theory>]
  [<InlineData("Start", true, false)>]
  [<InlineData("End", false, true)>]
  [<InlineData("Both", true, true)>]
  member this.``adornments compose with appearance and appear in the source``
    (choice: string, hasStart: bool, hasEnd: bool)
    = task {
    do! this.Load()
    do! this.Input.FillAsync("Ada")
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Adornment", choice)
    do! this.Expect(this.Input).ToHaveValueAsync("Ada")

    if hasStart then
      do! this.Expect(this.Source).ToContainTextAsync("startAdornment =")
    else
      do! this.Expect(this.Source).Not.ToContainTextAsync("startAdornment =")

    if hasEnd then
      do! this.Expect(this.Source).ToContainTextAsync("endAdornment =")
    else
      do! this.Expect(this.Source).Not.ToContainTextAsync("endAdornment =")

    do!
      this
        .Expect(this.Field.Locator("[class*=material-symbols]"))
        .ToHaveCountAsync((if hasStart then 1 else 0) + (if hasEnd then 1 else 0))

    do! this.Expect(this.Source).ToContainTextAsync("TextField.Variant.outlined")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``placeholder composes with above label and updates source``(width: int) = task {
    do! this.Load(width = width)
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Label position", "Above (outlined)")

    do!
      this.Page
        .GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Placeholder", Exact = true))
        .PressAsync("Space")

    do! this.Expect(this.Input).ToHaveAttributeAsync("placeholder", "Ada Lovelace")
    do! this.Expect(this.Field).ToHaveClassAsync(Regex("weave-field--label-above"))
    do! this.Expect(this.Source).ToContainTextAsync("placeholder = View.Const \"Ada Lovelace\"")
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Label.above")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToHaveTextAsync(minimal)

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits)
  }

  [<Fact>]
  member this.``reset restores the default appearance value and minimal source``() = task {
    do! this.Load()
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Color", "Success")
    do! this.Choose("Width", "Full")
    do! this.Choose("Adornment", "Both")
    do! this.Choose("Label position", "Above (outlined)")
    do! this.Input.FillAsync("Local value")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Input).ToHaveValueAsync("")
    do! this.Expect(this.Source).ToHaveTextAsync(minimal)
    do! this.Expect(this.Control("Variant")).ToContainTextAsync("Standard")
    do! this.Expect(this.Control("Color")).ToContainTextAsync("Primary")
    do! this.Expect(this.Control("Width")).ToContainTextAsync("Auto")
    do! this.Expect(this.Control("Adornment")).ToContainTextAsync("None")
    do! this.Expect(this.Control("Label position")).ToContainTextAsync("Floating")
  }

  [<Fact>]
  member this.``copy uses the currently selected source``() = task {
    let! _ =
      this.Page.AddInitScriptAsync(
        "Object.defineProperty(navigator, 'clipboard', { configurable: true, value: { writeText: async text => { window.copiedAppearanceSource = text; } } });"
      )

    do! this.Load()
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Color", "Info")
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Color.info")
    let! source = this.Source.TextContentAsync()
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copy", Exact = true)).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copied", Exact = true)))
        .ToBeVisibleAsync()

    let! copied = this.Page.EvaluateAsync<string>("() => window.copiedAppearanceSource")
    Assert.Equal(source, copied)
  }

  [<Fact>]
  member this.``expanded source copies the selected F sharp to the real Chromium clipboard``() = task {
    if this.Page.Context.Browser.BrowserType.Name <> "chromium" then
      Assert.Skip("Playwright clipboard-read permissions are supported by Chromium only")

    do!
      this.Page.Context.GrantPermissionsAsync(
        [| "clipboard-read"; "clipboard-write" |],
        BrowserContextGrantPermissionsOptions(Origin = fixture.BaseUrl)
      )

    do! this.Load()

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Hide Code", Exact = true))
        .ClickAsync()

    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Color", "Info")
    do! this.Expect(this.Source).ToContainTextAsync("TextField.Color.info")

    do!
      this
        .Expect(this.Page.Locator(".weave-expansion-panel__content-wrapper"))
        .ToHaveAttributeAsync("inert", "")

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copy", Exact = true)).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Copied", Exact = true)))
        .ToBeVisibleAsync()

    let! copied = this.Page.EvaluateAsync<string>("() => navigator.clipboard.readText()")

    let! source = this.Source.TextContentAsync()
    Assert.Equal(source, copied.Replace("\r\n", "\n"))
  }

  [<Fact>]
  member this.``appearance fits a 320 pixel viewport with reachable controls``() = task {
    do! this.Load(width = 320)
    do! this.Choose("Width", "Full")
    do! this.Choose("Variant", "Outlined")
    do! this.Choose("Label position", "Above (outlined)")

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits, "The configurator must fit the page; only its code block may scroll")
    do! this.Expect(this.Input).ToBeVisibleAsync()
    do! this.Expect(this.Control("Label position")).ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``appearance controls and source pass accessibility checks in both themes``(dark: bool) = task {
    do! this.Load()

    if dark then
      let! _ =
        this.Page.EvaluateAsync("() => document.body.setAttribute('data-theme', 'dark')")

      ()

    let! _ =
      this.Page.EvaluateAsync(
        """async () => {
          getComputedStyle(document.body).backgroundColor;
          await Promise.all(document.getAnimations({ subtree: true })
            .filter(animation => animation instanceof CSSTransition)
            .map(animation => animation.finished.catch(() => {})));
        }"""
      )

    if dark then
      do!
        this.Expect(this.Page.Locator(".docs-example")).ToHaveCSSAsync("background-color", "rgb(32, 39, 51)")

    let options = AxeRunOptions()

    options.Rules <-
      dict [
        "region", RuleOptions(Enabled = false)
        "landmark-one-main", RuleOptions(Enabled = false)
        "page-has-heading-one", RuleOptions(Enabled = false)
      ]
      |> System.Collections.Generic.Dictionary

    let! result = this.Page.RunAxe(options)

    let details =
      result.Violations
      |> Array.map (fun violation ->
        let nodes =
          violation.Nodes
          |> Array.map (fun node ->
            let checks =
              [ node.Any; node.All; node.None ]
              |> Array.concat
              |> Array.map (fun check -> check.Message + " " + string check.Data)
              |> String.concat "\n"

            node.Html + "\n" + checks)
          |> String.concat "\n"

        violation.Id + ": " + violation.Description + "\n" + nodes)
      |> String.concat "\n"

    Assert.True(result.Violations.Length = 0, details)
  }
