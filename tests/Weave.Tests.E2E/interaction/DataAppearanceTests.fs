namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type DataAppearanceTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Load(page: string, width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigatePage(this.Page, page)
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
  member this.``highlighter configuration updates matching and source``(width: int) = task {
    do! this.Load("highlighter-appearance", width)
    do! this.Choose("Search term", "Item")
    do! this.Choose("Color", "Primary")

    do!
      this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Case sensitive")).PressAsync("Space")

    let marks = this.Page.Locator(".docs-appearance__preview mark")
    do! this.Expect(marks).ToHaveCountAsync(1)
    do! this.Expect(marks).ToHaveTextAsync("Item")
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Highlighter.Color.primary")
    do! this.Expect(this.Source).ToContainTextAsync("caseSensitive = View.Const true")
    do! this.Expect(this.Source).ToContainTextAsync("View.Const \"Item\"")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``table appearance controls match source``(width: int) = task {
    do! this.Load("table-appearance", width)
    do! this.Choose("Density", "Compact")
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Striped")).PressAsync("Space")
    do! this.Page.GetByRole(AriaRole.Switch, PageGetByRoleOptions(Name = "Hoverable")).PressAsync("Space")
    let table = this.Page.Locator(".docs-appearance__preview .weave-table")
    do! this.Expect(table).ToHaveClassAsync(Regex("weave-table--compact"))
    do! this.Expect(table).ToHaveClassAsync(Regex("weave-table--striped"))
    do! this.Expect(table).ToHaveClassAsync(Regex("weave-table--hoverable"))
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("Table.Density.compact")
    do! this.Expect(this.Source).ToContainTextAsync("Table.striped")
    do! this.Expect(this.Source).ToContainTextAsync("Table.hoverable")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``list selection and options update source``(width: int) = task {
    do! this.Load("list-appearance", width)
    do! this.Choose("Selection", "Multiple")
    do! this.Choose("Density", "Compact")
    let rows = this.Page.Locator(".docs-appearance__preview .weave-list-item")
    do! rows.Nth(0).ClickAsync()
    do! rows.Nth(1).ClickAsync()
    do! rows.Nth(2).ClickAsync()
    do! this.OpenSource()
    do! this.Expect(this.Source).ToContainTextAsync("selectedValues = selected")
    do! this.Expect(this.Source).ToContainTextAsync("Set.ofList [")
    do! this.Expect(this.Source).ToContainTextAsync("\"Drafts\"")
    do! this.Expect(this.Source).ToContainTextAsync("\"Inbox\"")
    do! this.Expect(this.Source).ToContainTextAsync("\"Sent\"")
    do! this.Expect(this.Source).ToContainTextAsync("Density.compact")
  }

  [<Fact>]
  member this.``icon playground source follows chosen settings``() = task {
    do! this.Load("icons-documentation", 1280)

    let playground =
      this.Page
        .Locator(".docs-section")
        .Filter(
          LocatorFilterOptions(
            Has =
              this.Page.GetByRole(
                AriaRole.Heading,
                PageGetByRoleOptions(Name = "Icon Playground", Exact = true)
              )
          )
        )

    do!
      playground
        .GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "Filled", Exact = true))
        .PressAsync("Space")

    do!
      playground
        .GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "Rounded", Exact = true))
        .PressAsync("Space")

    do!
      playground
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    let source =
      playground.Locator("pre[aria-label='Selected icon F# source code'] code")

    do! this.Expect(source).ToContainTextAsync("Style.RoundedFilled")
    do! this.Expect(source).ToContainTextAsync("Icon.Action Action.Alarm")
    do! this.Expect(source).ToContainTextAsync("Attr.Create \"aria-hidden\" \"true\"")
    do! this.Expect(source).ToContainTextAsync("text \"Alarm\"")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``icon playground radio groups are named and keyboard independent``(width: int) = task {
    do! this.Load("icons-documentation", width)

    let playground =
      this.Page
        .Locator(".docs-section")
        .Filter(
          LocatorFilterOptions(
            Has =
              this.Page.GetByRole(
                AriaRole.Heading,
                PageGetByRoleOptions(Name = "Icon Playground", Exact = true)
              )
          )
        )

    for label in
      [
        "Select Fill"
        "Select Style"
        "Select Optical Size"
        "Select Weight"
        "Select Grade"
        "Select Color"
      ] do
      do!
        this
          .Expect(playground.GetByRole(AriaRole.Group, LocatorGetByRoleOptions(Name = label, Exact = true)))
          .ToHaveCountAsync(1)

    let weight =
      playground.GetByRole(AriaRole.Group, LocatorGetByRoleOptions(Name = "Select Weight", Exact = true))

    let! compactRows =
      weight.EvaluateAsync<bool>(
        "group => { const row = group.querySelector('div'); const labels = [...row.querySelectorAll('label.weave-radio')]; return parseFloat(getComputedStyle(row).rowGap) <= 12 && labels.every(label => parseFloat(getComputedStyle(label).marginTop) === 0 && parseFloat(getComputedStyle(label).marginBottom) === 0 && label.getBoundingClientRect().height >= 24); }"
      )

    Assert.True(compactRows, "Wrapped choices should remain close while preserving touch targets")

    let! fits =
      this.Page.EvaluateAsync<bool>("() => document.documentElement.scrollWidth <= innerWidth")

    Assert.True(fits)

    let fill =
      playground.GetByRole(AriaRole.Group, LocatorGetByRoleOptions(Name = "Select Fill", Exact = true))

    let style =
      playground.GetByRole(AriaRole.Group, LocatorGetByRoleOptions(Name = "Select Style", Exact = true))

    let unfilled =
      fill.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "Unfilled", Exact = true))

    let filled =
      fill.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "Filled", Exact = true))

    let outlined =
      style.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "Outlined", Exact = true))

    let rounded =
      style.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "Rounded", Exact = true))

    do! unfilled.FocusAsync()
    do! unfilled.PressAsync("ArrowRight")
    do! this.Expect(filled).ToBeCheckedAsync()
    do! this.Expect(outlined).ToBeCheckedAsync()
    do! outlined.FocusAsync()
    do! outlined.PressAsync("ArrowRight")
    do! this.Expect(rounded).ToBeCheckedAsync()
    do! this.Expect(filled).ToBeCheckedAsync()

    do!
      playground
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Show Code", Exact = true))
        .ClickAsync()

    let source =
      playground.Locator("pre[aria-label='Selected icon F# source code'] code")

    do! this.Expect(source).ToContainTextAsync("Style.RoundedFilled")
  }

  [<Fact>]
  member this.``list selection mode radios form a keyboard group``() = task {
    do! this.Load("list-documentation", 1280)

    let group =
      this.Page.GetByRole(AriaRole.Group, PageGetByRoleOptions(Name = "Selection mode", Exact = true))

    let single =
      group.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "SingleSelection", Exact = true))

    let toggle =
      group.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "ToggleSelection", Exact = true))

    do! single.FocusAsync()
    do! single.PressAsync("ArrowRight")
    do! this.Expect(toggle).ToBeCheckedAsync()

    let section =
      this.Page
        .Locator(".docs-section")
        .Filter(
          LocatorFilterOptions(
            Has =
              this.Page.GetByRole(
                AriaRole.Heading,
                PageGetByRoleOptions(Name = "Single-Selection", Exact = true)
              )
          )
        )

    let milk =
      section.Locator(".weave-list-item").Filter(LocatorFilterOptions(HasText = "Milk"))

    do! milk.ClickAsync()
    do! this.Expect(milk).ToHaveClassAsync(Regex("weave-list-item--selected"))
    do! milk.ClickAsync()
    do! this.Expect(milk).Not.ToHaveClassAsync(Regex("weave-list-item--selected"))

    do!
      section
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(NameRegex = Regex("^Show Code")))
        .ClickAsync()

    let source = section.Locator("pre code")
    do! this.Expect(source).ToContainTextAsync("legend [] [ text \"Selection mode\" ]")
    do! this.Expect(source).ToContainTextAsync("attr.name \"list-selection-mode\"")
  }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``reactive corner size radios have a keyboard group``(width: int) = task {
    do! this.Load("reactive-helpers-documentation", width)

    let section =
      this.Page
        .Locator(".docs-section")
        .Filter(
          LocatorFilterOptions(
            Has =
              this.Page.GetByRole(
                AriaRole.Heading,
                PageGetByRoleOptions(Name = "Attr.classSelection", Exact = true)
              )
          )
        )

    let group =
      section.GetByRole(AriaRole.Group, LocatorGetByRoleOptions(Name = "Corner size", Exact = true))

    let medium =
      group.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "medium", Exact = true))

    let large =
      group.GetByRole(AriaRole.Radio, LocatorGetByRoleOptions(Name = "large", Exact = true))

    do! medium.FocusAsync()
    do! medium.PressAsync("ArrowRight")
    do! this.Expect(large).ToBeCheckedAsync()
    do! this.Expect(section.Locator(".weave-rounded-lg")).ToHaveCountAsync(1)

    do!
      section
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(NameRegex = Regex("^Show Code")))
        .ClickAsync()

    let source = section.Locator("pre code")
    do! this.Expect(source).ToContainTextAsync("legend [] [ text \"Corner size\" ]")
    do! this.Expect(source).ToContainTextAsync("attr.name \"corner-size\"")
  }
