namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type ReferenceAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Load(page: string, width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigatePage(this.Page, page)
    do! this.Expect(this.Page.Locator(".docs-appearance")).ToBeVisibleAsync()
  }

  member private this.Choose(label: string, value: string) = task {
    do! this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = label, Exact = true)).ClickAsync()
    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = value, Exact = true)).ClickAsync()
  }

  member private this.Source =
    this.Page.Locator("pre[aria-label='Selected appearance F# source code'] code")

  member private this.OpenSource() = task {
    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Source).ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``link options update preview source and reset``(width: int) = task {
    do! this.Load("link-appearance", width)
    do! this.Choose("Underline", "Always")
    do! this.Choose("Color", "Secondary")
    do! this.Choose("Icons", "Both")
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Disabled")).PressAsync("Space")
    let link = this.Page.Locator(".docs-appearance__preview .weave-link")
    do! this.Expect(link).ToHaveClassAsync(Regex("weave-link--disabled"))
    do! this.Expect(link.Locator(".weave-link__start-icon")).ToHaveCountAsync(1)
    do! this.Expect(link.Locator(".weave-link__end-icon")).ToHaveCountAsync(1)
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Link.Underline.always")
    do! this.Expect(this.Source).ToContainTextAsync("Link.Color.secondary")
    do! this.Expect(this.Source).ToContainTextAsync("enabled = View.Const false")
    do! this.Expect(this.Source).ToContainTextAsync("startIcon =")
    do! this.Expect(this.Source).ToContainTextAsync("endIcon =")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(link).Not.ToHaveClassAsync(Regex("weave-link--disabled"))
    do! this.Expect(this.Source).Not.ToContainTextAsync("enabled = View.Const false")

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits)
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``tooltip settings update source and reset``(width: int) = task {
    do! this.Load("tooltip-appearance", width)
    do! this.Choose("Direction", "Bottom")
    do! this.Choose("Color", "Success")
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Show arrow")).PressAsync("Space")
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("direction = Tooltip.Direction.Bottom")
    do! this.Expect(this.Source).ToContainTextAsync("Tooltip.Color.success")
    do! this.Expect(this.Source).ToContainTextAsync("showArrow = false")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).Not.ToContainTextAsync("showArrow = false")

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits)
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``divider configuration updates preview and source``(width: int) = task {
    do! this.Load("divider-appearance", width)
    do! this.Choose("Variant", "Middle")
    do! this.Choose("Orientation", "Vertical")
    let divider = this.Page.Locator(".docs-appearance__preview .weave-divider")
    do! this.Expect(divider).ToHaveClassAsync(Regex("weave-divider--middle"))
    do! this.Expect(divider).ToHaveClassAsync(Regex("weave-divider--vertical"))
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Divider.Variant.middle")
    do! this.Expect(this.Source).ToContainTextAsync("Divider.Orientation.vertical")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(divider).Not.ToHaveClassAsync(Regex("weave-divider--vertical"))

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits)
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``typography settings update preview and source``(width: int) = task {
    do! this.Load("typography-appearance", width)
    do! this.Choose("Variant", "Heading 4")
    do! this.Choose("Weight", "Bold")
    do! this.Choose("Color", "Secondary")
    do! this.Choose("Alignment", "Center")

    let sample =
      this.Page.Locator(".docs-appearance__preview").GetByText("Make it clear.")

    do! this.Expect(sample).ToHaveClassAsync(Regex("weave-typography--h4"))
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Typography.h4")
    do! this.Expect(this.Source).ToContainTextAsync("Typography.Weight.bold")
    do! this.Expect(this.Source).ToContainTextAsync("Typography.Color.secondary")
    do! this.Expect(this.Source).ToContainTextAsync("Typography.Align.center")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(this.Source).ToContainTextAsync("Typography.body1")

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits)
  }
