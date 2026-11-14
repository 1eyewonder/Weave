namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

[<Collection("Browser input")>]
type CalendarPickerValidationTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button name =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.State name state =
    this.Page.GetByTestId(name + " " + state)

  [<Theory>]
  [<InlineData("Booking date", "2026-11-18")>]
  [<InlineData("Booking range", "2026-11-17 – 2026-11-20")>]
  member this.``Given an unavailable initial selection When submitting Then the value is retained and availability blocks submission``
    (name: string, selection: string)
    = task {
    do! this.NavigateTo "calendar-validation"
    do! this.Expect(this.State name "valid").ToHaveTextAsync "invalid"
    do! (this.Button("Submit " + name)).ClickAsync()
    do! this.Expect(this.State name "submitted").ToHaveTextAsync "not submitted"
    do! this.Expect(this.State name "selected").ToHaveTextAsync selection
    do! this.Expect(this.Button name).ToHaveAttributeAsync("aria-invalid", "true")
  }

  [<Theory>]
  [<InlineData("Booking date", "2026-11-18")>]
  [<InlineData("Booking range", "2026-11-17 – 2026-11-20")>]
  member this.``Given a stored selection When availability changes Then it is revalidated without replacing the value``
    (name: string, selection: string)
    = task {
    do! this.NavigateTo "calendar-validation"
    do! (this.Button("Allow all " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "valid"
    do! (this.Button("Exclude " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "invalid"
    do! (this.Button("Submit " + name)).ClickAsync()
    do! this.Expect(this.State name "submitted").ToHaveTextAsync "not submitted"
    do! this.Expect(this.State name "selected").ToHaveTextAsync selection
    do! (this.Button("Allow all " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "valid"
    do! (this.Button("Submit " + name)).ClickAsync()
    do! this.Expect(this.State name "submitted").ToHaveTextAsync selection
  }

  [<Theory>]
  [<InlineData("Booking date", "2026-11-17")>]
  [<InlineData("Booking range", "2026-11-16 – 2026-11-17")>]
  member this.``Given reactive selection values When an unavailable value is assigned externally Then correcting it clears the availability error``
    (name: string, selection: string)
    = task {
    do! this.NavigateTo "calendar-validation"
    do! (this.Button("Set available " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "valid"
    do! (this.Button("Set unavailable " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "invalid"
    do! (this.Button("Submit " + name)).ClickAsync()
    do! this.Expect(this.State name "submitted").ToHaveTextAsync "not submitted"
    do! (this.Button("Set available " + name)).ClickAsync()
    do! this.Expect(this.State name "errors").ToHaveTextAsync "0"
    do! (this.Button("Submit " + name)).ClickAsync()
    do! this.Expect(this.State name "submitted").ToHaveTextAsync selection
  }

  [<Theory>]
  [<InlineData("Booking date")>]
  [<InlineData("Booking range")>]
  member this.``Given an optional calendar field When cleared Then only application rules can require a selection``
    (name: string)
    = task {
    do! this.NavigateTo "calendar-validation"
    do! (this.Button("Clear " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "valid"
    do! (this.Button("Submit " + name)).ClickAsync()
    do! this.Expect(this.State name "submitted").ToHaveTextAsync "empty"
    do! (this.Button("Require " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "invalid"
    do! this.Expect(this.State name "errors").ToHaveTextAsync "1"
  }

  [<Theory>]
  [<InlineData("Booking date")>]
  [<InlineData("Booking range")>]
  member this.``Given an unavailable selection When the picker is removed and remounted Then its constraint is removed and registered once``
    (name: string)
    = task {
    do! this.NavigateTo "calendar-validation"
    do! this.Expect(this.State name "valid").ToHaveTextAsync "invalid"
    do! (this.Button("Unmount " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "valid"
    do! (this.Button("Remount " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "invalid"
    do! this.Expect(this.State name "errors").ToHaveTextAsync "1"
  }

  [<Theory>]
  [<InlineData("Booking date")>]
  [<InlineData("Booking range")>]
  member this.``Given a valid selection When no dates remain available Then submission is blocked until availability returns``
    (name: string)
    = task {
    do! this.NavigateTo "calendar-validation"
    do! (this.Button("Set available " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "valid"
    do! (this.Button("Close " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "invalid"
    do! (this.Button("Submit " + name)).ClickAsync()
    do! this.Expect(this.State name "submitted").ToHaveTextAsync "not submitted"
    do! (this.Button("Allow all " + name)).ClickAsync()
    do! this.Expect(this.State name "valid").ToHaveTextAsync "valid"
  }

  [<Fact>]
  member this.``Given an unavailable date When an allowed day is selected in the calendar Then submission succeeds``
    ()
    = task {
    do! this.NavigateTo "calendar-validation"
    do! (this.Button "Booking date").ClickAsync()
    do! (this.Button "2026-11-17").ClickAsync()
    do! (this.Button "Close picker").ClickAsync()
    do! this.Expect(this.State "Booking date" "valid").ToHaveTextAsync "valid"
    do! (this.Button "Submit Booking date").ClickAsync()
    do! this.Expect(this.State "Booking date" "submitted").ToHaveTextAsync "2026-11-17"
  }

  [<Fact>]
  member this.``Given an unavailable range When an allowed range is selected in the calendar Then submission succeeds``
    ()
    = task {
    do! this.NavigateTo "calendar-validation"
    do! (this.Button "Booking range").ClickAsync()
    do! (this.Button "2026-11-16").ClickAsync()
    do! (this.Button "2026-11-17").ClickAsync()
    do! (this.Button "Close picker").ClickAsync()
    do! this.Expect(this.State "Booking range" "valid").ToHaveTextAsync "valid"
    do! (this.Button "Submit Booking range").ClickAsync()
    do! this.Expect(this.State "Booking range" "submitted").ToHaveTextAsync "2026-11-16 – 2026-11-17"
  }
