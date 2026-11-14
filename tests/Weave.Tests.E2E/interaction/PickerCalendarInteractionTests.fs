namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type PickerCalendarInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button(name: string) =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.Month =
    this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Month", Exact = true))

  [<Fact>]
  member this.``Given an unrestricted calendar When choosing a month from the Weave select Then the calendar changes month``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "No bounds").ClickAsync()
    do! (this.Button "Date").ClickAsync()
    do! this.Month.ClickAsync()

    do!
      this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = "January", Exact = true)).ClickAsync()

    do! this.Expect(this.Month).ToHaveTextAsync "January"
    do! this.Expect(this.Button "2026-01-01").ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Given an open month menu When Escape is pressed Then only the innermost popup closes``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Date").ClickAsync()
      do! this.Month.PressAsync "Enter"
      do! this.Month.PressAsync "Escape"
      do! this.Expect(this.Month).ToHaveAttributeAsync("aria-expanded", "false")
      do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
      do! this.Month.PressAsync "Escape"
      do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync 0
    }

  [<Fact>]
  member this.``Given an unrestricted month selector When choosing with arrow keys Then the displayed month updates``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "No bounds").ClickAsync()
    do! (this.Button "Date").ClickAsync()
    do! this.Month.PressAsync "Enter"
    do! this.Month.PressAsync "ArrowDown"
    do! this.Month.PressAsync "ArrowDown"
    do! this.Month.PressAsync "Enter"
    do! this.Expect(this.Month).ToHaveTextAsync "February"
  }

  [<Fact>]
  member this.``Given overlapping availability windows When showing dates Then their union is selectable once``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Window date").ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveCountAsync 14
  }

  [<Fact>]
  member this.``Given disjoint windows When choosing a range across the gap Then the range is rejected``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Window range").ClickAsync()
      do! (this.Button "2026-11-05").ClickAsync()
      do! (this.Button "2026-11-15").ClickAsync()
      do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToContainTextAsync "unavailable date"
      do! this.Expect(this.Page.Locator "#window-range-value").ToHaveTextAsync "empty"
    }

  [<Fact>]
  member this.``Given disjoint windows When choosing within one window Then the range is committed``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Window range").ClickAsync()
    do! (this.Button "2026-11-05").ClickAsync()
    do! (this.Button "2026-11-07").ClickAsync()
    do! this.Expect(this.Page.Locator "#window-range-value").ToHaveTextAsync "5-7"
  }

  [<Theory>]
  [<InlineData("2026-11-01", "ArrowRight", "2026-11-02")>]
  [<InlineData("2026-11-02", "ArrowLeft", "2026-11-01")>]
  member this.``Given a daylight saving transition When moving one calendar day Then focus reaches the adjacent date``
    (start: string, key: string, expected: string)
    = task {
    use! context = fixture.NewTimezoneContextAsync("America/Chicago")
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "pickers")
    do! page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Window date", Exact = true)).ClickAsync()

    let date =
      page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = start, Exact = true))

    do! date.FocusAsync()
    do! date.PressAsync key

    do!
      this
        .Expect(page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = expected, Exact = true)))
        .ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("2026-11-03")>]
  [<InlineData("2026-11-04")>]
  [<InlineData("2026-11-17")>]
  member this.``Given an open calendar When interval exclusions change Then dates in every closure become unavailable``
    (date: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Window date").ClickAsync()
    do! (this.Button date).FocusAsync()
    do! (this.Button date).PressAsync "F4"
    do! this.Expect(this.Button date).ToHaveAttributeAsync("aria-disabled", "true")
  }

  [<Fact>]
  member this.``Given interval exclusions When a range crosses a closure Then it is rejected``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Window range").ClickAsync()
    do! (this.Button "2026-11-02").PressAsync "F4"
    do! (this.Button "2026-11-02").ClickAsync()
    do! (this.Button "2026-11-05").ClickAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToContainTextAsync "unavailable date"
    do! this.Expect(this.Page.Locator "#window-range-value").ToHaveTextAsync "empty"
  }

  [<Fact>]
  member this.``Given multiple allowed months When opening the calendar Then navigation starts at the earliest window``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Multi-month date").ClickAsync()
    do! this.Expect(this.Month).ToHaveTextAsync "December"
    do! this.Expect(this.Button "Previous month").ToBeDisabledAsync()
    do! this.Expect(this.Button "Next month").ToBeEnabledAsync()
  }

  [<Fact>]
  member this.``Given multiple allowed months When navigating through their gap Then every date in the gap is unavailable``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Multi-month date").ClickAsync()
    do! (this.Button "Next month").ClickAsync()
    do! this.Expect(this.Month).ToHaveTextAsync "January"
    do! this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveCountAsync 0
  }

  [<Fact>]
  member this.``Given rejected contradictory bounds outside allowed months When reaching the final window Then they do not extend navigation``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Multi-month date").ClickAsync()
    do! (this.Button "Next month").ClickAsync()
    do! (this.Button "Next month").ClickAsync()
    do! this.Expect(this.Month).ToHaveTextAsync "February"
    do! this.Expect(this.Button "Next month").ToBeDisabledAsync()
    do! this.Expect(this.Button "2027-02-21").ToHaveAttributeAsync("aria-disabled", "false")
  }

  [<Theory>]
  [<InlineData("Window date", "October")>]
  [<InlineData("Window date", "December")>]
  [<InlineData("Window range", "October")>]
  [<InlineData("Window range", "December")>]
  member this.``Given a bounded calendar When opening the month selector Then months outside its envelope are disabled``
    (name: string, month: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button name).ClickAsync()
    do! this.Month.ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = month, Exact = true)))
        .ToHaveAttributeAsync("aria-disabled", "true")

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = "November", Exact = true)))
        .ToHaveAttributeAsync("aria-disabled", "false")
  }

  [<Theory>]
  [<InlineData("Window date", "2025")>]
  [<InlineData("Window date", "2027")>]
  [<InlineData("Window range", "2025")>]
  [<InlineData("Window range", "2027")>]
  member this.``Given a bounded calendar When an out of bounds year is entered Then navigation stays within the envelope``
    (name: string, year: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button name).ClickAsync()

    let input =
      this.Page.GetByRole(AriaRole.Spinbutton, PageGetByRoleOptions(Name = "Year", Exact = true))

    do! input.FillAsync year
    do! input.PressAsync "Tab"
    do! this.Expect(input).ToHaveValueAsync "2026"
    do! this.Expect(input).ToHaveAttributeAsync("min", "2026")
    do! this.Expect(input).ToHaveAttributeAsync("max", "2026")
    do! this.Expect(this.Month).ToHaveTextAsync "November"
    do! this.Expect(this.Button "Previous month").ToBeDisabledAsync()
    do! this.Expect(this.Button "Next month").ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given windows across a year boundary When the year changes Then the month clamps to the closest boundary``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Multi-month date").ClickAsync()

    let input =
      this.Page.GetByRole(AriaRole.Spinbutton, PageGetByRoleOptions(Name = "Year", Exact = true))

    do! input.FillAsync "2027"
    do! input.PressAsync "Tab"
    do! this.Expect(input).ToHaveValueAsync "2027"
    do! this.Expect(this.Month).ToHaveTextAsync "February"
    do! this.Expect(this.Button "Next month").ToBeDisabledAsync()
    do! input.FillAsync "2026"
    do! input.PressAsync "Tab"
    do! this.Expect(input).ToHaveValueAsync "2026"
    do! this.Expect(this.Month).ToHaveTextAsync "December"
    do! this.Expect(this.Button "Previous month").ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given disjoint windows When selecting a month inside their envelope Then the gap remains navigable``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Multi-month date").ClickAsync()

    let year =
      this.Page.GetByRole(AriaRole.Spinbutton, PageGetByRoleOptions(Name = "Year", Exact = true))

    do! year.FillAsync "2027"
    do! year.PressAsync "Tab"
    do! this.Month.ClickAsync()

    let january =
      this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = "January", Exact = true))

    do! this.Expect(january).ToHaveAttributeAsync("aria-disabled", "false")
    do! january.ClickAsync()
    do! this.Expect(this.Month).ToHaveTextAsync "January"
    do! this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveCountAsync 0
    do! this.Expect(this.Button "Previous month").ToBeEnabledAsync()
    do! this.Expect(this.Button "Next month").ToBeEnabledAsync()
  }

  [<Theory>]
  [<InlineData("date-picker-docs", "Localize the calendar and format the field", 1280)>]
  [<InlineData("date-range-docs", "Localize and format a range", 1280)>]
  [<InlineData("date-picker-docs", "Localize the calendar and format the field", 390)>]
  [<InlineData("date-range-docs", "Localize and format a range", 390)>]
  member this.``Given a localization example When displaying culture controls Then they align with the fields and update culture``
    (route: string, title: string, width: int)
    = task {
    do! this.Page.SetViewportSizeAsync(width, 1000)
    do! this.NavigateTo route

    let section =
      this.Page
        .GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = title, Exact = true))
        .Locator(
          "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
        )

    let! aligned =
      section.EvaluateAsync<bool>(
        """el => {
      const fields = el.querySelectorAll('.docs-appearance__preview .weave-field__control');
      const buttons = el.querySelector('.docs-picker-culture-controls button').getBoundingClientRect();
      const first = fields[0].getBoundingClientRect();
      const last = fields[fields.length - 1].getBoundingClientRect();
      return innerWidth >= 768 ? Math.abs(first.top - buttons.top) <= 1 : buttons.top > last.bottom;
    }"""
      )

    Assert.True(
      aligned,
      "Culture controls align with the first field on desktop and follow both fields on mobile."
    )

    do! section.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "UK", Exact = true)).ClickAsync()

    do!
      this.Expect(section.Locator(".docs-picker-culture-controls")).ToContainTextAsync
        "Application culture: en-GB"
  }
