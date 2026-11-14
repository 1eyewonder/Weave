namespace Weave.Tests.E2E

open System
open System.Threading.Tasks
open Microsoft.Playwright
open Xunit

type RangePickerModelTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button(name: string) =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  [<Theory>]
  [<InlineData("Try closed range", "accepted")>]
  [<InlineData("Try singleton range", "accepted")>]
  [<InlineData("Try exclusive lower", "rejected")>]
  [<InlineData("Try exclusive upper", "rejected")>]
  [<InlineData("Try unbounded lower", "rejected")>]
  [<InlineData("Try unbounded upper", "rejected")>]
  [<InlineData("Try fully unbounded", "rejected")>]
  [<InlineData("Try reversed range", "rejected")>]
  member this.``Given an external selection When constructing a picker Then only ordered finite inclusive ranges are accepted``
    (button: string, expected: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button button).ClickAsync()
    do! this.Expect(this.Page.Locator "#range-construction-result").ToHaveTextAsync expected
  }

  [<Fact>]
  member this.``Given a range binding When updated externally Then inclusive endpoints appear in the field``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Set external range").ClickAsync()
    do! this.Expect(this.Button "Range").ToHaveTextAsync "2026-11-21 – 2026-11-22"
  }

  [<Fact>]
  member this.``Given a rejected range When a valid external selection replaces it Then the stale warning clears without closing``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Range").ClickAsync()
    do! (this.Button "2026-11-17").ClickAsync()
    do! (this.Button "2026-11-20").ClickAsync()
    let popup = this.Page.GetByRole(AriaRole.Dialog)
    do! this.Expect(popup.GetByRole(AriaRole.Status)).ToContainTextAsync "unavailable date"

    // Simulate an application update without an outside click dismissing the popup.
    let! _ =
      (this.Button "Set external range").EvaluateAsync<obj>("element => element.click()")

    do! this.Expect(this.Button "Range").ToHaveTextAsync "2026-11-21 – 2026-11-22"
    do! this.Expect(popup).ToBeVisibleAsync()
    do! this.Expect(popup.GetByRole(AriaRole.Status)).ToHaveTextAsync ""
  }

  [<Fact>]
  member this.``Given a selected range When converted to availability bounds Then only its dates are enabled``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Set external range").ClickAsync()
    do! (this.Button "Use range as bounds").ClickAsync()
    do! (this.Button "Date").ClickAsync()

    do!
      this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveTextAsync [|
        "21"
        "22"
      |]
  }

  [<Fact>]
  member this.``Given a singleton range When converted to availability bounds Then only its one date is enabled``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Set singleton range").ClickAsync()
    do! (this.Button "Use range as bounds").ClickAsync()
    do! (this.Button "Date").ClickAsync()

    do!
      this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveTextAsync [| "21" |]
  }

  [<Fact>]
  member this.``Given a mounted picker When an unsupported selection is assigned Then an argument error identifies the contract``
    ()
    = task {
    do! this.NavigateTo "pickers"

    let error =
      TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously)

    this.Page.PageError.Add(fun message -> error.TrySetResult message |> ignore)
    do! (this.Button "Set exclusive selection").ClickAsync()
    let! message = error.Task.WaitAsync(TimeSpan.FromSeconds 10.0)
    Assert.Contains("ordered, finite, inclusive bounds", message)
  }

  [<Theory>]
  [<InlineData("#range-time-analysis", "excluded")>]
  [<InlineData("#range-number-analysis", "included")>]
  member this.``Given shared ranges When analyzing browser values Then generic membership is preserved``
    (selector: string, expected: string)
    = task {
    do! this.NavigateTo "pickers"
    do! this.Expect(this.Page.Locator selector).ToHaveTextAsync expected
  }

  [<Fact>]
  member this.``Given supplied date-time endpoints When formatting a range Then original time precision is retained``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! this.Expect(this.Button "Timed range").ToHaveTextAsync "09:30 – 17:45"
  }

  [<Fact>]
  member this.``Given a range picker When choosing dates Then committed endpoints have no time portion``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Timed range").ClickAsync()
      do! (this.Button "2026-11-21").ClickAsync()
      do! (this.Button "2026-11-22").ClickAsync()
      do! this.Expect(this.Button "Timed range").ToHaveTextAsync "00:00 – 00:00"
    }

  [<Fact>]
  member this.``Given date bounds When projected years coincide Then the browser retains the bounded case``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! this.Expect(this.Page.Locator "#range-mapped-case").ToHaveTextAsync "bounded"
  }

  [<Theory>]
  [<InlineData("2026-11-21")>]
  [<InlineData("2026-11-22")>]
  member this.``Given inclusive bounds with times When showing calendar dates Then both endpoint dates remain selectable``
    (date: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Timed calendar bounds").ClickAsync()
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Button date).ToBeEnabledAsync()
  }

  [<Theory>]
  [<InlineData("Date")>]
  [<InlineData("Range")>]
  member this.``Given invalid calendar bounds When opening a picker Then no dates are selectable``
    (label: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Invalid calendar bounds").ClickAsync()
    do! (this.Button label).ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveCountAsync 0
  }

  [<Theory>]
  [<InlineData("equal excluded range")>]
  [<InlineData("equal excluded interval")>]
  [<InlineData("collapsed excluded range")>]
  [<InlineData("collapsed excluded interval")>]
  [<InlineData("equal inclusive range")>]
  [<InlineData("equal inclusive interval")>]
  member this.``Given checked bounds When constructing and projecting in the browser Then contradictory bounds are rejected and inclusive singletons remain valid``
    (name: string)
    = task {
    do! this.NavigateTo "pickers"
    do! this.Expect(this.Page.GetByTestId name).ToHaveTextAsync "correct"
  }

[<Collection("Browser input")>]
type DateRangePickerDocumentationTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("America/New_York", "March", "2027-03-13", "March", "2027-03-15", "2 nights")>]
  [<InlineData("America/New_York", "March", "2027-03-14", "March", "2027-03-15", "1 night")>]
  [<InlineData("America/New_York", "November", "2027-11-06", "November", "2027-11-08", "2 nights")>]
  [<InlineData("Australia/Lord_Howe", "October", "2027-10-02", "October", "2027-10-04", "2 nights")>]
  [<InlineData("America/New_York", "February", "2028-02-28", "March", "2028-03-01", "2 nights")>]
  [<InlineData("America/New_York", "December", "2000-12-31", "January", "2001-01-01", "1 night")>]
  member this.``Given the booking documentation When dates cross clock or calendar boundaries Then the summary counts calendar nights``
    (zone: string, firstMonth: string, first: string, lastMonth: string, last: string, expected: string)
    = task {
    use! context = fixture.NewTimezoneContextAsync(zone)
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "date-range-docs")

    let button name =
      page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

    let chooseDate month (date: string) = task {
      let year =
        page.GetByRole(AriaRole.Spinbutton, PageGetByRoleOptions(Name = "Year", Exact = true))

      do! year.FillAsync(date.Substring(0, 4))
      do! year.PressAsync "Tab"
      do! page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Month", Exact = true)).ClickAsync()
      do! page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = month, Exact = true)).ClickAsync()
      do! (button date).ClickAsync()
    }

    do! (button "Check-in / check-out").ClickAsync()
    do! chooseDate firstMonth first
    do! chooseDate lastMonth last
    do! (button "Close picker").ClickAsync()
    let summary = page.Locator(".docs-picker-summary [role='status']").First
    do! this.Expect(summary).ToHaveTextAsync(first + " to " + last + " / " + expected)
  }
