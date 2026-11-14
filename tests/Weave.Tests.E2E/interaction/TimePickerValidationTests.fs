namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type TimePickerValidationTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button name =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.Entry name =
    this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = name, Exact = true))

  [<Fact>]
  member this.``Given custom form rules When an unavailable time is typed Then it remains entered and submission is blocked``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Business time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "18"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! (this.Entry "Minutes").FillAsync "00"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! this.Expect(this.Page.Locator "#business-selected").ToHaveTextAsync "18:00"
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "not submitted"
    do! (this.Button "Business time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "09"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! (this.Entry "Minutes").FillAsync "15"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "09:15"
  }

  [<Theory>]
  [<InlineData("Hours", "99")>]
  [<InlineData("Minutes", "07")>]
  member this.``Given a valid stored time When its draft is invalid Then the form cannot submit the old value``
    (name: string, draft: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Business time").ClickAsync()
    do! (this.Entry name).FillAsync draft
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-selected").ToHaveTextAsync "09:00"
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "not submitted"
  }

  [<Theory>]
  [<InlineData("Set off-step time")>]
  [<InlineData("Set negative time")>]
  [<InlineData("Set next-day time")>]
  [<InlineData("Set seconds time")>]
  member this.``Given permissive custom rules When an external time violates defaults Then the defaults still block submission``
    (action: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Allow any business time").ClickAsync()
    do! (this.Button action).ClickAsync()
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "not submitted"
  }

  [<Fact>]
  member this.``Given a typed invalid draft When the form resets Then draft errors clear even when the stored value is unchanged``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Business time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "99"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Reset booking time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "valid"
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "09:00"
  }

  [<Fact>]
  member this.``Given reactive form rules When rules change Then the selected time is retained and revalidated``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Open at ten").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-selected").ToHaveTextAsync "09:00"
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
  }

  [<Fact>]
  member this.``Given a mounted picker with default errors When unmounted Then its errors are removed and remounting restores the constraints``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Set off-step time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Unmount business picker").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "valid"
    do! (this.Button "Remount business picker").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
  }

  [<Fact>]
  member this.``Given a parsing error When editing Then its message appears once beside the popup inputs``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Business time").ClickAsync()
      do! (this.Entry "Hours").FillAsync "99"
      do! (this.Entry "Hours").PressAsync "Enter"

      do!
        this
          .Expect(this.Page.GetByText("Enter hours from 0 to 23.", PageGetByTextOptions(Exact = true)))
          .ToHaveCountAsync
          1
    }

  [<Fact>]
  member this.``Given an invalid hour draft When valid minutes are entered Then the form remains invalid until the hour is corrected``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Business time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "99"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! (this.Entry "Minutes").FillAsync "15"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
    do! (this.Entry "Hours").FillAsync "10"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "10:15"
  }

  [<Theory>]
  [<InlineData("Set negative time", "00", "00:00")>]
  [<InlineData("Set next-day time", "00", "00:00")>]
  [<InlineData("Set seconds time", "13", "13:30")>]
  member this.``Given an invalid external time When its displayed hour is explicitly applied Then a valid whole minute can replace it``
    (action: string, hour: string, expected: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Allow any business time").ClickAsync()
    do! (this.Button action).ClickAsync()
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Business time").ClickAsync()
    do! (this.Entry "Hours").FillAsync hour
    do! (this.Entry "Hours").PressAsync "Enter"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync expected
  }

  [<Fact>]
  member this.``Given an external off step time When only the hour is typed Then minutes remain entered and invalid``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Allow any business time").ClickAsync()
    do! (this.Button "Set off-step time").ClickAsync()
    do! (this.Button "Business time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "14"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! this.Expect(this.Page.Locator "#business-selected").ToHaveTextAsync "14:07"
    do! this.Expect(this.Entry "Minutes").ToHaveValueAsync "07"
    do! this.Expect(this.Page.Locator "#business-valid").ToHaveTextAsync "invalid"
    do! (this.Entry "Minutes").FillAsync "15"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "14:15"
  }

[<Collection("Browser input")>]
type TimePickerValidationPointerTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button name =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  [<Fact>]
  member this.``Given custom form rules When an unavailable time is chosen on the dial Then form submission is blocked``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Business time").ClickAsync()
    do! (this.Button "Hour 18").ClickAsync()
    do! (this.Button "Minute 00").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-selected").ToHaveTextAsync "18:00"
    do! (this.Button "Book time").ClickAsync()
    do! this.Expect(this.Page.Locator "#business-submitted").ToHaveTextAsync "not submitted"
  }

[<Collection("Browser input")>]
type TimeAvailabilityInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button name =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.Entry name =
    this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = name, Exact = true))

  [<Fact>]
  member this.``Given a partially available hour When selected Then it chooses an available stepped minute``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()
    do! this.Expect(this.Button "Hour 09").ToBeEnabledAsync()
    do! this.Expect(this.Button "Hour 08").ToBeDisabledAsync()
    do! this.Expect(this.Button "Hour 10").ToBeEnabledAsync()
    do! (this.Button "Hour 10").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "10:00"
    do! this.Expect(this.Button "Minute 00").ToBeEnabledAsync()
    do! this.Expect(this.Button "Minute 15").ToBeEnabledAsync()
    do! this.Expect(this.Button "Minute 30").ToBeDisabledAsync()
    do! this.Expect(this.Button "Minute 45").ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given an interval exclusion When choosing minutes by keyboard Then unavailable slots are skipped``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()
    do! (this.Entry "Minutes").ClickAsync()

    let slider =
      this.Page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = "Minutes", Exact = true))

    do! slider.FocusAsync()
    do! slider.PressAsync "Home"
    do! this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", "30")
    do! slider.PressAsync "ArrowRight"
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
    do! this.Expect(this.Button "Minute 45").ToBeDisabledAsync()
  }

  [<Fact>]
  member this.``Given an unavailable dial number When a synthetic activation occurs Then the stored time is unchanged``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()

    let! _ =
      (this.Button "Hour 08")
        .EvaluateAsync<obj>("el => el.dispatchEvent(new MouseEvent('click', {bubbles:true,detail:0}))")

    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
    do! this.Expect(this.Button "Hour 10").ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Given an unavailable typed time When submitting Then it remains selected and blocks submission``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "18"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! (this.Entry "Minutes").FillAsync "00"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "18:00"
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"

    do!
      this
        .Expect(this.Page.GetByText("Selected time is unavailable.", PageGetByTextOptions(Exact = true)))
        .ToHaveCountAsync
        1

    do! (this.Button "Submit scheduled time").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-submitted").ToHaveTextAsync "not submitted"
    do! (this.Button "Set available scheduled time").ClickAsync()
    do! (this.Button "Submit scheduled time").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-submitted").ToHaveTextAsync "15:00"
  }

  [<Fact>]
  member this.``Given a mounted open picker When availability changes Then choices and validation update without losing the draft``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()
    do! (this.Entry "Minutes").FillAsync "45"
    do! (this.Entry "Minutes").PressAsync "F4"
    do! this.Expect(this.Entry "Minutes").ToHaveValueAsync "45"
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"
    do! (this.Entry "Hours").ClickAsync()
    do! this.Expect(this.Button "Hour 09").ToBeDisabledAsync()
    do! this.Expect(this.Button "Hour 10").ToBeEnabledAsync()
  }

  [<Fact>]
  member this.``Given reactive exclusions When the only minute in an hour is removed Then the hour becomes disabled``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()
    let! _ = (this.Button "Exclude 09:30").EvaluateAsync<obj>("el => el.click()")
    do! this.Expect(this.Button "Hour 09").ToBeDisabledAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"
  }

  [<Fact>]
  member this.``Given no compatible stepped minute When the step changes Then hours become selectable at the new step``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Only 09:40").ClickAsync()
    do! (this.Button "Scheduled time").ClickAsync()
    do! this.Expect(this.Button "Hour 09").ToBeDisabledAsync()
    do! (this.Button "Close picker").ClickAsync()
    do! (this.Button "Use 10-minute steps").ClickAsync()
    do! (this.Button "Scheduled time").ClickAsync()
    do! this.Expect(this.Button "Hour 09").ToBeEnabledAsync()
    do! (this.Button "Hour 09").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:40"
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "valid"
  }

  [<Fact>]
  member this.``Given twelve hour mode When switching period Then another available hour remains reachable``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Use 12-hour").ClickAsync()
    do! (this.Button "Scheduled time").ClickAsync()
    do! (this.Button "PM").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "15:30"
    do! this.Expect(this.Button "Hour 03").ToBeEnabledAsync()
    do! this.Expect(this.Button "Hour 09").ToBeDisabledAsync()
    do! (this.Button "AM").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
  }

  [<Fact>]
  member this.``Given an overnight schedule When switching periods Then midnight windows remain selectable``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Use overnight schedule").ClickAsync()
    do! (this.Button "Use 12-hour").ClickAsync()
    do! (this.Button "Scheduled time").ClickAsync()
    do! this.Expect(this.Button "Hour 01").ToBeEnabledAsync()
    do! this.Expect(this.Button "Hour 09").ToBeDisabledAsync()
    do! (this.Button "PM").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "22:30"
    do! this.Expect(this.Button "Hour 11").ToBeEnabledAsync()
  }

  [<Fact>]
  member this.``Given all slots closed When the policy reopens Then the retained value becomes valid again``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Close all slots").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Scheduled time").ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-time-picker__number:enabled")).ToHaveCountAsync 0
    do! this.Expect(this.Page.GetByRole(AriaRole.Slider)).ToHaveAttributeAsync("aria-disabled", "true")
    do! (this.Button "Close picker").ClickAsync()
    do! (this.Button "Allow all slots").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "valid"
  }

  [<Fact>]
  member this.``Given an unavailable external value When unmounted and remounted Then contributed availability constraints follow the picker lifecycle``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Set unavailable scheduled time").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Unmount scheduled picker").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "valid"
    do! (this.Button "Remount scheduled picker").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Allow all slots").ClickAsync()
    do! (this.Button "Submit scheduled time").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-submitted").ToHaveTextAsync "18:00"
  }

  [<Fact>]
  member this.``Given an available time When application rules fail Then form submission remains blocked``() =
    task {
      do! this.NavigateTo "time-availability"
      do! (this.Button "Block application rule").ClickAsync()
      do! (this.Button "Submit scheduled time").ClickAsync()
      do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"
      do! this.Expect(this.Page.Locator "#availability-submitted").ToHaveTextAsync "not submitted"
    }

  [<Fact>]
  member this.``Given a restricted minute dial When its background is clicked at an unavailable minute Then the value is retained``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()
    do! (this.Entry "Minutes").ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = "Minutes", Exact = true)))
        .ToBeVisibleAsync()

    let! dial = this.Page.Locator(".weave-time-picker__clock").BoundingBoxAsync()
    do! this.Page.Mouse.ClickAsync(dial.X + dial.Width * 0.25f, dial.Y + dial.Height * 0.5f)
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
  }

  [<Fact>]
  member this.``Given only morning slots When showing twelve hour mode Then the unavailable period is disabled``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Only 09:40").ClickAsync()
    do! (this.Button "Use 10-minute steps").ClickAsync()
    do! (this.Button "Use 12-hour").ClickAsync()
    do! (this.Button "Scheduled time").ClickAsync()
    do! this.Expect(this.Button "AM").ToBeEnabledAsync()
    do! this.Expect(this.Button "PM").ToBeDisabledAsync()
    do! this.Expect(this.Button "Hour 09").ToBeEnabledAsync()
  }

  [<Fact>]
  member this.``Given an invalid hour draft When the current allowed hour is selected on the dial Then its draft is corrected``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Scheduled time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "99"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "invalid"
    do! (this.Button "Hour 09").ClickAsync()
    do! this.Expect(this.Entry "Hours").ToHaveValueAsync "09"
    do! this.Expect(this.Page.Locator "#availability-selected").ToHaveTextAsync "09:30"
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "valid"
  }

  [<Fact>]
  member this.``Given no available slots When an optional selection is cleared Then the empty value can be submitted``
    ()
    = task {
    do! this.NavigateTo "time-availability"
    do! (this.Button "Close all slots").ClickAsync()
    do! (this.Button "Make scheduled time optional").ClickAsync()
    do! (this.Button "Clear scheduled time").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-valid").ToHaveTextAsync "valid"
    do! (this.Button "Submit scheduled time").ClickAsync()
    do! this.Expect(this.Page.Locator "#availability-submitted").ToHaveTextAsync "empty"
  }

[<Collection("Browser input")>]
type TimePickerDocsValidationTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData(1280)>]
  [<InlineData(390)>]
  member this.``Given the reminder example When opening hours change Then controls dial and submission reflect the active rule``
    (width: int)
    = task {
    do! this.Page.SetViewportSizeAsync(width, 1000)
    do! this.NavigateTo "time-picker-docs"

    let section =
      this.Page
        .GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = "Validate a reminder", Exact = true))
        .Locator(
          "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
        )

    let button name =
      section.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = name, Exact = true))

    let dialButton name =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

    let status = section.Locator("[role=status]")

    do! this.Expect(section).ToContainTextAsync "Opens at 09:00"
    do! this.Expect(button "Restore 09:00 opening").ToBeDisabledAsync()
    do! (button "Open at 10:00").ClickAsync()
    do! this.Expect(section).ToContainTextAsync "Opens at 10:00"
    do! this.Expect(button "Open at 10:00").ToBeDisabledAsync()
    do! (button "Daily reminder").ClickAsync()
    do! this.Expect(dialButton "Hour 09").ToBeDisabledAsync()
    do! this.Expect(dialButton "Hour 10").ToBeEnabledAsync()
    do! (dialButton "Hour 10").ClickAsync()
    do! (dialButton "Close picker").ClickAsync()
    do! (button "Restore 09:00 opening").ClickAsync()
    do! this.Expect(section).ToContainTextAsync "Opens at 09:00"
    do! (button "Daily reminder").ClickAsync()
    do! this.Expect(dialButton "Hour 09").ToBeEnabledAsync()
    do! (dialButton "Hour 09").ClickAsync()
    do! (dialButton "Close picker").ClickAsync()
    do! (button "Open at 10:00").ClickAsync()
    do! this.Expect(button "Daily reminder").ToContainTextAsync "09:00"
    do! this.Expect(section).ToContainTextAsync "Selected time is unavailable."
    do! (button "Save reminder").ClickAsync()
    do! this.Expect(status).ToHaveTextAsync "Choose a time, then save your reminder."
    do! (button "Restore 09:00 opening").ClickAsync()

    do!
      this
        .Expect(section.GetByText("Selected time is unavailable.", LocatorGetByTextOptions(Exact = true)))
        .ToHaveCountAsync
        0

    do! (button "Save reminder").ClickAsync()
    do! this.Expect(status).ToHaveTextAsync "Reminder set for 09:00"
  }

  [<Theory>]
  [<InlineData("time-picker-docs", "Format and review a schedule")>]
  [<InlineData("date-range-docs", "Review an existing booking")>]
  member this.``Given picker example controls When displayed side by side Then field edges align``
    (route: string, title: string)
    = task {
    do! this.Page.SetViewportSizeAsync(1280, 1000)
    do! this.NavigateTo route

    let section =
      this.Page
        .GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = title, Exact = true))
        .Locator(
          "xpath=ancestor::div[contains(concat(' ', normalize-space(@class), ' '), ' docs-section ')][1]"
        )

    let! difference =
      section.EvaluateAsync<float[]>(
        """el => {
      const picker = el.querySelector('.docs-appearance__preview .weave-field__control').getBoundingClientRect();
      const select = el.querySelector('.docs-appearance__settings .weave-field__control').getBoundingClientRect();
      return [Math.abs(picker.top - select.top), Math.abs(picker.width - select.width)];
    }"""
      )

    Assert.InRange(difference[0], 0.0, 1.0)
    Assert.InRange(difference[1], 0.0, 1.0)
  }
