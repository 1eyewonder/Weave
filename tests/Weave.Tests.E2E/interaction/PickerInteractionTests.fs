namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type PickerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button(name: string) =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.Date(day: int) = this.Button(sprintf "2026-11-%02d" day)
  member private this.Popup = this.Page.GetByRole(AriaRole.Dialog)

  member private this.Slider(name: string) =
    this.Page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.Capture(name: string) = task {
    let directory = System.Environment.GetEnvironmentVariable "WEAVE_PICKER_SCREENSHOTS"

    if not (System.String.IsNullOrEmpty directory) then
      System.IO.Directory.CreateDirectory directory |> ignore

      let! _ =
        this.Popup.ScreenshotAsync(
          LocatorScreenshotOptions(Path = System.IO.Path.Combine(directory, name + ".png"))
        )

      ()
  }

  member private this.OpenCalendar(year: string, month: string) = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Required date").PressAsync "ArrowDown"

    let yearInput =
      this.Page.GetByRole(AriaRole.Spinbutton, PageGetByRoleOptions(Name = "Year"))

    do! yearInput.FillAsync year
    do! yearInput.PressAsync "Tab"

    do! this.Page.GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Month")).ClickAsync()
    do! this.Page.Locator(sprintf "[data-calendar-month='%s']" month).ClickAsync()

    ()
  }

  member private this.SetTheme(theme: string) = task {
    let! _ =
      this.Page.EvaluateAsync("theme => document.body.setAttribute('data-theme', theme)", theme)

    ()
  }

  [<Theory>]
  [<InlineData(9)>]
  [<InlineData(18)>]
  [<InlineData(26)>]
  member this.``Given calendar restrictions When an unavailable date is shown Then it is disabled``
    (day: int)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Date day).ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given a date picker When a date is selected Then its binding updates``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! (this.Date 17).ClickAsync()
    do! this.Expect(this.Page.Locator "#date-value").ToHaveTextAsync "2026-11-17"
  }

  [<Fact>]
  member this.``Given an open picker When a date is selected Then it stays open until Done returns focus``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Date").ClickAsync()
      do! (this.Date 17).ClickAsync()
      do! this.Expect(this.Popup).ToBeVisibleAsync()
      do! this.Expect(this.Date 17).ToBeFocusedAsync()
      do! (this.Button "Close picker").ClickAsync()
      do! this.Expect(this.Popup).ToHaveCountAsync 0
      do! this.Expect(this.Button "Date").ToBeFocusedAsync()
    }

  [<Fact>]
  member this.``Given a selected date When cleared Then its binding is empty``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! (this.Button "Clear selection").ClickAsync()
    do! this.Expect(this.Page.Locator "#date-value").ToHaveTextAsync "empty"
  }

  [<Fact>]
  member this.``Given leap day When moving right Then focus reaches March first``() = task {
    do! this.OpenCalendar("2024", "2")
    do! (this.Button "2024-02-29").FocusAsync()
    do! this.Page.Keyboard.PressAsync "ArrowRight"
    do! this.Expect(this.Button "2024-03-01").ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Given a calendar date When shift page up is pressed Then focus moves back a year``() = task {
    do! this.OpenCalendar("2024", "3")
    do! (this.Button "2024-03-01").FocusAsync()
    do! this.Page.Keyboard.PressAsync "Shift+PageUp"
    do! this.Expect(this.Button "2023-03-01").ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Given an open calendar When escape is pressed Then focus returns to its trigger``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Date 15).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync "Escape"
    do! this.Expect(this.Button "Date").ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Given an empty range When only the start is selected Then no range is committed``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Range").ClickAsync()
    do! (this.Date 17).ClickAsync()
    do! this.Expect(this.Page.Locator "#range-value").ToHaveTextAsync "empty"
  }

  [<Fact>]
  member this.``Given a range When endpoints are selected backwards Then the committed range is ordered``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Range").ClickAsync()
      do! (this.Date 17).ClickAsync()
      do! (this.Date 15).ClickAsync()
      do! this.Expect(this.Page.Locator "#range-value").ToHaveTextAsync "15-17"
    }

  [<Fact>]
  member this.``Given an unavailable interior date When choosing a range across it Then the range is rejected``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Range").ClickAsync()
    do! (this.Date 17).ClickAsync()
    do! (this.Date 20).ClickAsync()
    do! this.Expect(this.Page.Locator "#range-value").ToHaveTextAsync "empty"
    do! this.Expect(this.Page.GetByRole(AriaRole.Status)).ToContainTextAsync "unavailable date"
  }

  [<Fact>]
  member this.``Given an unfinished range When closed and reopened Then a new start can be chosen``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Range").ClickAsync()
    do! (this.Date 17).ClickAsync()
    do! (this.Button "Close picker").ClickAsync()
    do! (this.Button "Range").ClickAsync()
    do! (this.Date 21).ClickAsync()
    do! (this.Date 22).ClickAsync()
    do! this.Expect(this.Page.Locator "#range-value").ToHaveTextAsync "21-22"
  }

  [<Theory>]
  [<InlineData("AM", "00:45")>]
  [<InlineData("PM", "12:45")>]
  member this.``Given twelve hour mode When twelve is selected Then the period determines midnight or noon``
    (period: string, expected: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    do! (this.Button period).ClickAsync()
    do! (this.Button "Hour 12").ClickAsync()
    do! (this.Button "Minute 45").ClickAsync()
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync expected
  }

  [<Fact>]
  member this.``Given fifteen minute intervals When the minute dial advances Then it moves by fifteen``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Time").ClickAsync()

      do!
        (this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = "Minutes", Exact = true)))
          .ClickAsync()

      do! (this.Slider "Minutes").PressAsync "Home"
      do! this.Page.Keyboard.PressAsync "ArrowRight"
      do! this.Expect(this.Slider "Minutes").ToHaveAttributeAsync("aria-valuenow", "15")
    }

  [<Fact>]
  member this.``Given twenty four hour mode When the last minute is confirmed Then late evening is selected``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! (this.Button "Hour 23").ClickAsync()
    do! (this.Slider "Minutes").PressAsync "End"
    do! this.Page.Keyboard.PressAsync "Enter"
    do! this.Expect(this.Button "24-hour time").ToContainTextAsync "23:59"
  }

  [<Fact>]
  member this.``Given a selected time When cleared Then its binding is empty``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    do! (this.Button "Clear selection").ClickAsync()
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "empty"
  }

  [<Fact>]
  member this.``Given a date binding When changed externally Then the date trigger updates``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "External update").ClickAsync()
    do! this.Expect(this.Button "Date").ToContainTextAsync "2026-11-21"
  }

  [<Fact>]
  member this.``Given a time binding When changed externally Then the clock selects the new hour``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "External update").ClickAsync()
    do! (this.Button "Time").ClickAsync()
    do! this.Expect(this.Button "Hour 12").ToHaveAttributeAsync("aria-pressed", "true")
  }

  [<Fact>]
  member this.``Given a read only picker When its trigger is clicked Then it stays closed``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Toggle read only").ClickAsync()
    do! this.Expect(this.Button "Date").ToHaveAttributeAsync("aria-disabled", "true")
    do! (this.Button "Date").ClickAsync(LocatorClickOptions(Force = true))
    do! this.Expect(this.Popup).ToHaveCountAsync 0
  }

  [<Fact>]
  member this.``Given an enabled picker When disabled externally Then its trigger is disabled``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Toggle enabled").ClickAsync()
    do! this.Expect(this.Button "Time").ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given reactive bounds When the upper bound shrinks Then later dates become unavailable``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Tighten bounds").ClickAsync()
      do! (this.Button "Date").ClickAsync()
      do! this.Expect(this.Date 17).ToBeDisabledAsync()
    }

  [<Fact>]
  member this.``Given a required empty picker When validated Then its error is associated with the trigger``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Validate").ClickAsync()
    do! this.Expect(this.Button "Required date").ToHaveAttributeAsync("aria-invalid", "true")

    do!
      this.Expect(this.Page.GetByText("Choose a date", PageGetByTextOptions(Exact = true))).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Given an open picker When clicking outside Then it closes``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! this.Page.Mouse.ClickAsync(1100.0f, 700.0f)
    do! this.Expect(this.Popup).ToHaveCountAsync 0
  }

  [<Fact>]
  member this.``Given the last popup action When tabbing forward Then focus leaves and the picker closes``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Date").ClickAsync()
      do! (this.Button "Close picker").FocusAsync()
      do! this.Page.Keyboard.PressAsync "Tab"
      do! this.Expect(this.Popup).ToHaveCountAsync 0
    }

  [<Fact>]
  member this.``Given closed pickers When scanned Then accessibility checks pass``() =
    this.RunAxeScan "pickers"

  [<Theory>]
  [<InlineData("Date")>]
  [<InlineData("Range")>]
  [<InlineData("Time")>]
  member this.``Given an open picker When scanned Then accessibility checks pass``(name: string) = task {
    do! this.RunAxeScan("pickers-" + name)
    do! this.Expect(this.Popup).ToBeVisibleAsync()
    do! this.Capture("open-" + name)
  }

  [<Theory>]
  [<InlineData(390)>]
  [<InlineData(1280)>]
  member this.``Given a viewport When opening the rightmost calendar Then the popup stays inside it``
    (width: int)
    = task {
    do! this.NavigateTo "pickers"
    do! this.Page.SetViewportSizeAsync(width, 800)
    do! (this.Button "Required date").ClickAsync()
    let! box = this.Popup.BoundingBoxAsync()
    Assert.True(box.X >= 0.0f && box.X + box.Width <= float32 width)
  }

  [<Fact>]
  member this.``Given an unavailable date When reached by arrow key Then it remains focusable``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! (this.Date 17).FocusAsync()
    do! this.Page.Keyboard.PressAsync "ArrowRight"
    do! this.Expect(this.Date 18).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``Given a focused unavailable date When enter is pressed Then the binding does not change``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Date").ClickAsync()
      do! (this.Date 18).FocusAsync()
      do! this.Page.Keyboard.PressAsync "Enter"
      do! this.Expect(this.Page.Locator "#date-value").ToHaveTextAsync "2026-11-15"
    }

  [<Fact>]
  [<Trait("Capability", "MobileEmulation")>]
  member this.``Given a touch screen When clock numbers are tapped Then the selected time updates``() = task {
    use! context = fixture.NewTouchContextAsync()
    let! page = context.NewPageAsync()
    do! this.NavigatePage(page, "pickers")
    do! page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Time", Exact = true)).TapAsync()
    do! page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Hour 03", Exact = true)).TapAsync()
    do! page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Minute 30", Exact = true)).TapAsync()
    do! this.Expect(page.Locator "#time-value").ToHaveTextAsync "15:30"
  }

  [<Fact>]
  member this.``Given twenty four hour mode When the dial opens Then morning hours use the inner ring``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "24-hour time").ClickAsync()
      do! this.Expect(this.Button "Hour 01").ToBeVisibleAsync()
      let! inner = (this.Button "Hour 01").BoundingBoxAsync()
      let! outer = (this.Button "Hour 13").BoundingBoxAsync()
      let! dial = this.Page.Locator(".weave-time-picker__clock").BoundingBoxAsync()
      Assert.True(inner.Y > outer.Y && inner.X < outer.X && dial.Width = dial.Height)
      do! this.Capture "clock-24-hours"
    }

  [<Theory>]
  [<InlineData(390, "light")>]
  [<InlineData(1280, "dark")>]
  member this.``Given a themed viewport When the time picker opens Then its clock stays circular``
    (width: int, theme: string)
    = task {
    do! this.NavigateTo "pickers"
    do! this.Page.SetViewportSizeAsync(width, 800)
    do! this.SetTheme theme
    do! (this.Button "Time").ClickAsync()
    let clock = this.Page.Locator(".weave-time-picker__clock")
    do! this.Expect(clock).ToBeVisibleAsync()
    let! dial = clock.BoundingBoxAsync()
    Assert.True(abs (dial.Width - dial.Height) < 1.0f)
    do! this.Capture(sprintf "clock-%d-%s" width theme)
  }

  [<Fact>]
  member this.``Given a minute dial When tapping between printed marks Then individual minutes are selectable``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! (this.Button "Hour 23").ClickAsync()
    do! this.Expect(this.Slider "Minutes").ToBeVisibleAsync()
    let! dial = this.Page.Locator(".weave-time-picker__clock").BoundingBoxAsync()
    // The point immediately counterclockwise from midnight represents minute 59.
    do! this.Page.Mouse.ClickAsync(dial.X + dial.Width * 0.4582f, dial.Y + dial.Height * 0.1022f)
    do! this.Expect(this.Button "24-hour time").ToContainTextAsync "23:59"
  }

  static member MinuteDialLabels: obj array array = [|
    [| "Time"; [| "00"; "15"; "30"; "45" |] |]
    [|
      "24-hour time"
      [| "00"; "05"; "10"; "15"; "20"; "25"; "30"; "35"; "40"; "45"; "50"; "55" |]
    |]
    [|
      "7-minute time"
      [| "00"; "07"; "14"; "21"; "28"; "35"; "42"; "49"; "56" |]
    |]
  |]

  [<Theory>]
  [<MemberData(nameof PickerInteractionTests.MinuteDialLabels)>]
  member this.``Given minute steps When opening the minute dial Then only selectable values are printed``
    (label: string, expected: string array)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button label).ClickAsync()

    do!
      (this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = "Minutes", Exact = true)))
        .ClickAsync()

    do! this.Expect(this.Page.Locator(".weave-time-picker__number")).ToHaveTextAsync expected
    do! this.Capture("clock-minutes-" + label)
  }

  [<Fact>]
  member this.``Given degenerate date bounds When the calendar opens Then exactly one date is selectable``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Single date").ClickAsync()
      do! (this.Button "Date").ClickAsync()
      do! this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveCountAsync 1
      do! this.Expect(this.Date 15).ToBeEnabledAsync()
    }

  [<Fact>]
  member this.``Given degenerate date bounds When selecting its date twice Then a same day range is committed``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Single date").ClickAsync()
    do! (this.Button "Range").ClickAsync()
    do! (this.Date 15).ClickAsync()
    do! (this.Date 15).ClickAsync()
    do! this.Expect(this.Page.Locator "#range-value").ToHaveTextAsync "15-15"
  }

  [<Fact>]
  member this.``Given exclusive calendar bounds When the calendar opens Then its endpoints are unavailable``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Exclusive bounds").ClickAsync()
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Date 14).ToBeDisabledAsync()
    do! this.Expect(this.Date 17).ToBeDisabledAsync()
    do! this.Expect(this.Page.Locator(".weave-calendar__day[aria-disabled='false']")).ToHaveCountAsync 2
  }

  [<Fact>]
  member this.``Given an unbounded calendar When opening another month Then date bounds do not restrict it``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "No bounds").ClickAsync()
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Button "Previous month").ToBeEnabledAsync()
    do! this.Expect(this.Button "Next month").ToBeEnabledAsync()
  }

  [<Fact>]
  member this.``Given a clearable attribute When its view becomes false Then the clear action is hidden``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Toggle clear").ClickAsync()
      do! (this.Button "Date").ClickAsync()
      do! this.Expect(this.Button "Clear selection").ToHaveCountAsync 0
    }

  [<Fact>]
  member this.``Given help content When a picker renders Then the help is displayed automatically``() = task {
    do! this.NavigateTo "pickers"

    do!
      this
        .Expect(this.Page.GetByText("Calendar help is visible", PageGetByTextOptions(Exact = true)))
        .ToBeVisibleAsync()
  }

  [<Theory>]
  [<InlineData("en-US", "Sun", "November")>]
  [<InlineData("en-GB", "Mon", "November")>]
  [<InlineData("fr-FR", "lun.", "novembre")>]
  member this.``Given an application culture When opening a calendar Then labels and week order follow it``
    (culture: string, first: string, month: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button culture).ClickAsync()
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-calendar__weekdays span").First).ToHaveTextAsync first
    do! this.Expect(this.Page.Locator(".weave-calendar__title")).ToContainTextAsync month
    do! this.Capture("culture-" + culture)
  }

  [<Fact>]
  member this.``Given a Monday first culture When a picker explicitly requests Sunday Then the override wins``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "en-GB").ClickAsync()
    do! (this.Button "Sunday override").ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-calendar__weekdays span").First).ToHaveTextAsync "Sun"
  }

  [<Fact>]
  member this.``Given a Saturday first culture When the calendar opens Then Saturday starts the week``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "ar-EG").ClickAsync()
      do! (this.Button "Date").ClickAsync()
      do! this.Expect(this.Page.Locator(".weave-calendar__weekdays span").First).ToHaveTextAsync "السبت"
    }

  [<Fact>]
  member this.``Given footer and navigation actions When the calendar opens Then standard Weave buttons are used``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()

    do!
      this
        .Expect(this.Button "Previous month")
        .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-button--icon"))

    do!
      this
        .Expect(this.Button "Close picker")
        .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-button"))
  }

  [<Fact>]
  member this.``Given weekday restrictions When using a Sunday first culture Then excluding weekends does not reorder the week``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Weekdays").ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-calendar__weekdays span").First).ToHaveTextAsync "Sun"
    do! this.Expect(this.Date 15).ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given a mounted calendar When application culture changes Then the labels update immediately``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Date 15).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync "F2"
    do! this.Expect(this.Page.Locator(".weave-calendar__weekdays span").First).ToHaveTextAsync "lun."
  }

  [<Fact>]
  member this.``Given a focused date When application culture changes Then focus stays on that date``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Date 15).ToBeFocusedAsync()
    do! this.Page.Keyboard.PressAsync "F2"
    do! this.Expect(this.Page.Locator(".weave-calendar__weekdays span").First).ToHaveTextAsync "lun."
    do! this.Expect(this.Date 15).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("en-US", "Sun")>]
  [<InlineData("en-GB", "Mon")>]
  [<InlineData("ar-EG", "السبت")>]
  member this.``Given an engine without week information APIs When choosing a culture Then the CLDR fallback determines week order``
    (culture: string, first: string)
    = task {
    use! _script =
      this.Page.AddInitScriptAsync(
        "delete Intl.Locale.prototype.getWeekInfo; delete Intl.Locale.prototype.weekInfo;"
      )

    do! this.NavigateTo "pickers"
    do! (this.Button culture).ClickAsync()
    do! (this.Button "Date").ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-calendar__weekdays span").First).ToHaveTextAsync first
  }

  // Keep pointer capture exclusive, following the shared Color Picker test pattern.
  [<Fact>]
  member this.``Given a completed range When selecting the second date Then the calendar remains open until Done``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Range").ClickAsync()
    do! (this.Date 17).ClickAsync()
    do! (this.Date 15).ClickAsync()
    do! this.Expect(this.Page.Locator "#range-value").ToHaveTextAsync "15-17"
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    do! (this.Button "Close picker").ClickAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync 0
  }

  [<Theory>]
  [<InlineData("Date")>]
  [<InlineData("Range")>]
  member this.``Given an open calendar When clicking its background Then it remains open``(label: string) =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button label).ClickAsync()
      let popup = this.Page.GetByRole(AriaRole.Dialog)
      do! popup.ClickAsync(LocatorClickOptions(Position = Position(X = 3.0f, Y = 3.0f)))
      do! this.Expect(popup).ToBeVisibleAsync()
    }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1440)>]
  member this.``Given a calendar When rendered at different widths Then day highlights remain circular``
    (width: int)
    = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigateTo "pickers"
    do! (this.Button "Date").ClickAsync()

    let! difference =
      (this.Date 15)
        .EvaluateAsync<float>(
          "e => { const r=e.getBoundingClientRect(); return Math.abs(r.width-r.height); }"
        )

    Assert.True(difference < 0.1, $"Day width and height differ by {difference}px")
  }

[<Collection("Browser input")>]
type PickerPointerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``Given an hour dial When dragged to three Then the selected hour follows the pointer``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Time", Exact = true))).ClickAsync()
    let! dial = this.Page.Locator(".weave-time-picker__clock").BoundingBoxAsync()
    let centerX = dial.X + dial.Width / 2.0f
    let centerY = dial.Y + dial.Height / 2.0f
    do! this.Page.Mouse.MoveAsync(centerX, centerY - dial.Height * 0.4f)
    do! this.Page.Mouse.DownAsync()
    do! this.Page.Mouse.MoveAsync(centerX + dial.Width * 0.4f, centerY, MouseMoveOptions(Steps = 5))
    do! this.Page.Mouse.UpAsync()
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "15:30"
  }

[<Collection("Browser input")>]
type PickerThemeInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``Given a theme When a date is selected Then it uses the theme accent``(theme: string) = task {
    do! this.NavigateTo "pickers"

    let! _ =
      this.Page.EvaluateAsync("theme => document.body.setAttribute('data-theme', theme)", theme)

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Date", Exact = true)).ClickAsync()

    let selected =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "2026-11-15", Exact = true))

    do! this.Expect(selected).ToBeVisibleAsync()

    let! selectedColor =
      selected.EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

    let! primaryColor =
      this.Page.EvaluateAsync<string>(
        "() => { const el = document.createElement('span'); el.style.backgroundColor = 'var(--palette-primary)'; document.body.append(el); const color = getComputedStyle(el).backgroundColor; el.remove(); return color; }"
      )

    Assert.Equal(primaryColor, selectedColor)
  }
