namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type ProgressSliderAppearanceTests(fixture: TestFixture) =
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

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Progress shape and style update preview source and reset``(width: int) = task {
    do! this.Load("progress-appearance", width)
    do! this.Choose("Shape", "Circular")
    do! this.Choose("Color", "Secondary")
    do! this.Choose("Size", "Large")
    do! this.Expect(this.Source).ToContainTextAsync("Progress.Circular.create")
    do! this.Expect(this.Source).ToContainTextAsync("Progress.Circular.Size.large")
    do! this.Expect(this.Source).ToContainTextAsync("Progress.Color.secondary")

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Linear style")))
        .ToBeDisabledAsync()

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("Progress.Linear.create")
    do! this.Choose("Linear style", "Rounded and striped")
    do! this.Expect(this.Source).ToContainTextAsync("Progress.Linear.Variant.rounded")
    do! this.Expect(this.Source).ToContainTextAsync("Progress.Linear.Variant.striped")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Slider ticks disabled state and value update generated source``(width: int) = task {
    do! this.Load("slider-appearance", width)
    do! this.Choose("Color", "Secondary")
    do! this.Expect(this.Source).ToContainTextAsync("Slider.secondary")

    let slider =
      this.Page.Locator(".docs-appearance__preview").GetByRole(AriaRole.Slider)

    do! slider.PressAsync("End")
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 100")
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Tick marks")).PressAsync("Space")
    do! this.Expect(this.Source).ToContainTextAsync("Slider.secondary")
    do! this.Expect(this.Source).ToContainTextAsync("showTickMarks = true")
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 100")
    do! slider.PressAsync("ArrowLeft")
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 80")
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Disabled")).PressAsync("Space")
    do! this.Expect(slider).ToBeDisabledAsync()
    do! this.Expect(this.Source).ToContainTextAsync("enabled = View.Const false")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 40")
    do! this.Expect(this.Source).Not.ToContainTextAsync("showTickMarks = true")
    do! slider.PressAsync("ArrowRight")
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 41")
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Tick marks")).PressAsync("Space")
    do! this.Expect(this.Source).ToContainTextAsync("showTickMarks = true")
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 40")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``Pagination settings and current page update generated source``(width: int) = task {
    do! this.Load("pagination-appearance", width)
    do! this.Choose("Variant", "Filled")
    do! this.Choose("Color", "Secondary")

    do!
      this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "First and last")).PressAsync("Space")

    let! fits =
      this.Page.EvaluateAsync<bool>(
        """() => document.documentElement.scrollWidth <= innerWidth &&
          [...document.querySelectorAll('.docs-appearance__preview button')]
            .every(el => { const r = el.getBoundingClientRect(); return r.left >= 0 && r.right <= innerWidth; })"""
      )

    Assert.True(fits, "Pagination controls must remain within the viewport.")
    let preview = this.Page.Locator(".docs-appearance__preview")
    do! preview.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Go to page 3")).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 3")
    do! this.Expect(this.Source).ToContainTextAsync("Pagination.Variant.filled")
    do! this.Expect(this.Source).ToContainTextAsync("Pagination.Color.secondary")
    do! this.Expect(this.Source).ToContainTextAsync("Pagination.Controls.showFirstLast")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("Var.Create 2")
    do! this.Expect(this.Source).Not.ToContainTextAsync("Pagination.Controls.showFirstLast")
  }
