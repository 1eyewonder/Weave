namespace Weave.Tests.E2E

open Microsoft.Playwright
open Xunit

type TimePickerEntryTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Button(name: string) =
    this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = name, Exact = true))

  member private this.Entry(name: string) =
    this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = name, Exact = true))

  [<Fact>]
  member this.``Given a twenty four hour picker When entering hours and minutes Then Enter applies the time and Done closes``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "23"
    do! this.Expect(this.Button "24-hour time").ToHaveTextAsync ""
    do! (this.Entry "Hours").PressAsync "Enter"
    do! this.Expect(this.Entry "Minutes").ToBeFocusedAsync()
    do! (this.Entry "Minutes").FillAsync "59"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! this.Expect(this.Button "24-hour time").ToHaveTextAsync "23:59"
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    do! this.Expect(this.Entry "Minutes").ToBeFocusedAsync()
    do! (this.Button "Close picker").ClickAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync 0
  }

  [<Theory>]
  [<InlineData("Time", "Hour 02", "Minute 15")>]
  [<InlineData("24-hour time", "Hour 23", "Minute 15")>]
  member this.``Given a clock dial When selecting a minute Then it stays open for revision until Done``
    (trigger: string, hour: string, minute: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button trigger).ClickAsync()
    do! (this.Button hour).ClickAsync()
    do! (this.Button minute).ClickAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    do! this.Expect(this.Entry "Minutes").ToHaveValueAsync "15"
    do! (this.Button "Minute 30").ClickAsync()
    do! this.Expect(this.Entry "Minutes").ToHaveValueAsync "30"
    do! (this.Button "Close picker").ClickAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync 0
    do! this.Expect(this.Button trigger).ToBeFocusedAsync()
  }

  [<Theory>]
  [<InlineData("AM", "00:45")>]
  [<InlineData("PM", "12:45")>]
  member this.``Given twelve hour input When entering twelve Then the selected period controls midnight or noon``
    (period: string, expected: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    do! (this.Button period).ClickAsync()
    do! (this.Entry "Hours").FillAsync "12"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! (this.Entry "Minutes").FillAsync "45"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync expected
  }

  [<Theory>]
  [<InlineData("Hours", "0")>]
  [<InlineData("Hours", "13")>]
  [<InlineData("Hours", "")>]
  [<InlineData("Minutes", "14")>]
  [<InlineData("Minutes", "60")>]
  [<InlineData("Minutes", "")>]
  member this.``Given invalid or incomplete input When applying Then the previous value remains and feedback is associated``
    (name: string, draft: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    let input = this.Entry name
    do! input.FillAsync draft
    do! input.PressAsync "Enter"
    do! this.Expect(input).ToHaveValueAsync draft
    do! this.Expect(input).ToHaveAttributeAsync("aria-invalid", "true")
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "13:30"
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    let! errorId = input.GetAttributeAsync "aria-describedby"
    do! this.Expect(this.Page.Locator("#" + errorId)).ToContainTextAsync "Enter"
    do! this.Expect(this.Page.Locator("#" + errorId)).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Given invalid minutes When corrected and tabbed away Then validation clears and the dial follows``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    do! (this.Entry "Minutes").FillAsync "14"
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! (this.Entry "Minutes").FillAsync "45"
    do! (this.Entry "Minutes").PressAsync "Tab"
    do! this.Expect(this.Entry "Minutes").ToHaveAttributeAsync("aria-invalid", "false")
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "13:45"

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = "Minutes", Exact = true)))
        .ToHaveAttributeAsync("aria-valuenow", "45")
  }

  [<Theory>]
  [<InlineData("07", "00:07")>]
  [<InlineData("56", "00:56")>]
  member this.``Given a step that does not divide sixty When entering an allowed minute Then it is accepted``
    (minute: string, expected: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "7-minute time").ClickAsync()
    do! (this.Entry "Minutes").FillAsync minute
    do! (this.Entry "Minutes").PressAsync "Enter"
    do! this.Expect(this.Button "7-minute time").ToHaveTextAsync expected
  }

  [<Fact>]
  member this.``Given typed hours When changing period and choosing the dial Then both header fields stay synchronized``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "02"
    do! (this.Button "AM").ClickAsync()
    do! this.Expect(this.Entry "Hours").ToHaveValueAsync "02"
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "02:30"
    do! (this.Entry "Minutes").ClickAsync()
    do! (this.Button "Minute 15").ClickAsync()
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
    do! (this.Button "Close picker").ClickAsync()
    do! (this.Button "Time").ClickAsync()
    do! this.Expect(this.Entry "Hours").ToHaveValueAsync "02"
    do! this.Expect(this.Entry "Minutes").ToHaveValueAsync "15"
  }

  [<Fact>]
  member this.``Given an invalid draft When an external time arrives Then the header and validation reset``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    do! (this.Entry "Minutes").FillAsync "14"
    do! (this.Entry "Minutes").PressAsync "Enter"
    let! _ = (this.Button "External update").EvaluateAsync<obj>("e=>e.click()")
    do! this.Expect(this.Entry "Hours").ToHaveValueAsync "12"
    do! this.Expect(this.Entry "Minutes").ToHaveValueAsync "45"
    do! this.Expect(this.Entry "Minutes").ToHaveAttributeAsync("aria-invalid", "false")
  }

  [<Fact>]
  member this.``Given an invalid draft When dismissed and reopened Then the last committed time is shown``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Time").ClickAsync()
      do! (this.Entry "Hours").FillAsync "99"
      do! (this.Entry "Hours").PressAsync "Escape"
      do! (this.Button "Time").ClickAsync()
      do! this.Expect(this.Entry "Hours").ToHaveValueAsync "01"
      do! this.Expect(this.Entry "Minutes").ToHaveValueAsync "30"
    }

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1440)>]
  member this.``Given editable time fields When rendered Then numeric input retains the large toolbar typography``
    (width: int)
    = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    let input = this.Entry "Hours"
    do! this.Expect(input).ToHaveAttributeAsync("inputmode", "numeric")

    let! fontSize =
      input.EvaluateAsync<float>("e=>parseFloat(getComputedStyle(e).fontSize)")

    Assert.True(fontSize >= 40.0, $"Time entry font is too small: {fontSize}px")
  }

  [<Fact>]
  member this.``Given an empty time When opening Then zero hints do not initialize the bound value``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! this.Expect(this.Entry "Hours").ToHaveValueAsync ""
    do! this.Expect(this.Entry "Minutes").ToHaveValueAsync ""
    do! this.Expect(this.Entry "Hours").ToHaveAttributeAsync("placeholder", "00")
    do! this.Expect(this.Entry "Minutes").ToHaveAttributeAsync("placeholder", "00")
    do! this.Expect(this.Button "24-hour time").ToHaveTextAsync ""
  }

  [<Theory>]
  [<InlineData("Toggle enabled")>]
  [<InlineData("Toggle read only")>]
  member this.``Given a typed draft When interaction is revoked Then the draft cannot update the bound time``
    (toggle: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "02"
    let! _ = (this.Button toggle).EvaluateAsync<obj>("e=>e.click()")
    do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync 0
    do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "13:30"
  }

  [<Fact>]
  member this.``Given twenty four hour input When entering twenty four Then the draft is rejected``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "24"
    do! (this.Entry "Hours").PressAsync "Enter"
    do! this.Expect(this.Entry "Hours").ToHaveAttributeAsync("aria-invalid", "true")
    do! this.Expect(this.Button "24-hour time").ToHaveTextAsync ""
  }

  [<Fact>]
  member this.``Given empty fields When moving between them without typing Then no validation error appears``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! (this.Entry "Hours").ClickAsync()
    do! (this.Entry "Minutes").ClickAsync()
    do! this.Expect(this.Entry "Hours").ToHaveAttributeAsync("aria-invalid", "false")
    do! this.Expect(this.Button "24-hour time").ToHaveTextAsync ""
  }

  [<Fact>]
  member this.``Given a typed minute draft When Space is pressed Then it does not activate the field``() =
    task {
      do! this.NavigateTo "pickers"
      do! (this.Button "Time").ClickAsync()
      do! (this.Entry "Minutes").FillAsync "45"
      do! (this.Entry "Minutes").PressAsync "Space"
      do! this.Expect(this.Page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync()
      do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "13:30"
      do! (this.Entry "Minutes").PressAsync "Enter"
      do! this.Expect(this.Page.Locator "#time-value").ToHaveTextAsync "13:45"
    }

  [<Theory>]
  [<InlineData("Standard", "Hours")>]
  [<InlineData("Standard", "Minutes")>]
  [<InlineData("Filled", "Hours")>]
  [<InlineData("Filled", "Minutes")>]
  [<InlineData("Outlined", "Hours")>]
  [<InlineData("Outlined", "Minutes")>]
  member this.``Given a themed picker variant When editing the time Then fields remain unboxed with a visible focus cue``
    (variant: string, name: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button(variant + " time appearance")).ClickAsync()
    let input = this.Entry name

    let! expected =
      input.EvaluateAsync<string>("e=>getComputedStyle(e.closest('.weave-time-picker__toolbar')).color")

    let! resting =
      input.EvaluateAsync<string>(
        "e=>getComputedStyle(e.closest('.weave-field').querySelector('fieldset')).display"
      )

    Assert.Equal("none", resting)
    do! input.FocusAsync()

    let! focused = input.EvaluateAsync<string>("e=>getComputedStyle(e).outlineColor")

    Assert.Equal(expected, focused)
    do! input.PressAsync "Tab"
    let! blurred = input.EvaluateAsync<string>("e=>getComputedStyle(e).outlineStyle")
    Assert.Equal("none", blurred)
  }

  [<Theory>]
  [<InlineData("Hours", "a1-2x3", "12")>]
  [<InlineData("Minutes", "x3.0yz9", "30")>]
  member this.``Given a time input When typing mixed characters Then only the first two ASCII digits are accepted``
    (name: string, typed: string, expected: string)
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! (this.Entry name).FillAsync ""
    do! (this.Entry name).PressSequentiallyAsync typed
    do! this.Expect(this.Entry name).ToHaveValueAsync expected
    do! this.Expect(this.Entry name).ToHaveAttributeAsync("maxlength", "2")
  }

  [<Fact>]
  member this.``Given a full time input When replacing selected digits or deleting Then editing keys still work``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    do! (this.Entry "Hours").FillAsync "12"
    do! (this.Entry "Hours").PressAsync "ControlOrMeta+A"
    do! (this.Entry "Hours").PressSequentiallyAsync "09"
    do! this.Expect(this.Entry "Hours").ToHaveValueAsync "09"
    do! (this.Entry "Hours").PressAsync "Backspace"
    do! this.Expect(this.Entry "Hours").ToHaveValueAsync "0"
  }

  [<Fact>]
  member this.``Given a time input When pasting nonnumeric text Then the paste is ignored``() = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "Time").ClickAsync()
    let input = this.Entry "Hours"

    let! rejected =
      input.EvaluateAsync<bool>(
        "e=>{const event=new Event('paste',{bubbles:true,cancelable:true});Object.defineProperty(event,'clipboardData',{value:{getData:()=>'-1'}});return !e.dispatchEvent(event)}"
      )

    Assert.True rejected
    do! this.Expect(input).ToHaveValueAsync "01"
  }

  [<Fact>]
  member this.``Given a noncancelable input update When it contains disallowed characters Then the fallback retains only two ASCII digits``
    ()
    = task {
    do! this.NavigateTo "pickers"
    do! (this.Button "24-hour time").ClickAsync()
    let input = this.Entry "Hours"

    let! _ =
      input.EvaluateAsync<obj>("e=>{e.value='a1b923';e.dispatchEvent(new Event('input',{bubbles:true}))}")

    do! this.Expect(input).ToHaveValueAsync "19"
  }

  [<Fact>]
  member this.``Given no clock format When opening a time picker Then twelve hour display and period controls are the default``
    ()
    = task {
    do! this.NavigateTo "pickers"

    let trigger =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Default time", Exact = true))

    do! this.Expect(trigger).ToHaveTextAsync "1:30 PM"
    do! trigger.ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "AM", Exact = true)))
        .ToBeVisibleAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "PM", Exact = true)))
        .ToBeVisibleAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Hour 13", Exact = true)))
        .ToHaveCountAsync
        0
  }

[<Collection("Browser input")>]
type TimePickerEntryPointerTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``Given a time entry When hovered and focused Then only a single focus outline appears``() =
    task {
      do! this.NavigateTo "pickers"
      do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Time", Exact = true)).ClickAsync()

      let input =
        this.Page.GetByRole(AriaRole.Textbox, PageGetByRoleOptions(Name = "Hours", Exact = true))

      do! input.HoverAsync()

      let! hoverFill =
        input.EvaluateAsync<string>("e=>getComputedStyle(e).backgroundColor")

      Assert.Equal("rgba(0, 0, 0, 0)", hoverFill)
      do! input.FocusAsync()

      let! focusOutline =
        input.EvaluateAsync<string>("e=>getComputedStyle(e).outlineStyle")

      Assert.Equal("solid", focusOutline)

      let! outlineWidth =
        input.EvaluateAsync<string>("e=>getComputedStyle(e).outlineWidth")

      Assert.Equal("2px", outlineWidth)
      do! input.FillAsync "99"
      do! input.PressAsync "Enter"

      let! decoration =
        input.EvaluateAsync<string>("e=>getComputedStyle(e).textDecorationLine")

      Assert.Equal("none", decoration)
    }
