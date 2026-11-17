namespace Weave.Tests.E2E

open System
open Microsoft.Playwright
open Xunit

[<Collection("Browser input")>]
type ForcedColorIndicatorTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  let contrastsWith background samples =
    samples
    |> Array.exists (fun pixel -> (max background pixel + 0.05) / (min background pixel + 0.05) >= 3.0)

  member private this.Open(route, dark) = task {
    do!
      this.Page.EmulateMediaAsync(
        PageEmulateMediaOptions(
          ForcedColors = ForcedColors.Active,
          ColorScheme = (if dark then ColorScheme.Dark else ColorScheme.Light)
        )
      )

    do! this.NavigateTo(route)

    let! forced =
      this.Page.EvaluateAsync<bool>("matchMedia('(forced-colors: active)').matches")

    Assert.True(forced, "The forced-colors stylesheet must be active.")
  }

  // Sample the painted pixels because opacity, overlapping layers and UA color substitution
  // can hide an indicator even when its elements have different computed styles.
  member private this.LuminanceRow(element: ILocator, heightFraction: float) = task {
    let! bytes =
      element.ScreenshotAsync(LocatorScreenshotOptions(Scale = ScreenshotScale.Css))

    return!
      this.Page.EvaluateAsync<float array>(
        """async ([png, heightFraction]) => {
        const image = new Image();
        image.src = "data:image/png;base64," + png;
        await image.decode();
        const canvas = document.createElement("canvas");
        canvas.width = image.width;
        canvas.height = image.height;
        const ctx = canvas.getContext("2d");
        ctx.drawImage(image, 0, 0);
        const y = Math.floor(heightFraction * image.height);
        const {data} = ctx.getImageData(0, y, image.width, 1);
        const linear = byte => { const v = byte / 255; return v <= 0.04045 ? v / 12.92 : ((v + 0.055) / 1.055) ** 2.4; };
        return Array.from({length: image.width}, (_, x) =>
          0.2126 * linear(data[4*x]) + 0.7152 * linear(data[4*x+1]) + 0.0722 * linear(data[4*x+2]));
      }""",
        [| box (Convert.ToBase64String bytes); box heightFraction |]
      )
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``forced colors distinguish selected dates``(dark: bool) = task {
    do! this.Open("calendar-validation", dark)

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Allow all Booking date", Exact = true))
        .ClickAsync()

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Booking date", Exact = true))
        .ClickAsync()

    let selected = this.Page.Locator(".weave-calendar__day--selected")

    let ordinary =
      this.Page.Locator(".weave-calendar__day:not(.weave-calendar__day--selected):not(:disabled)").First

    do! this.Expect(selected).ToHaveCountAsync(1)
    let! selectedRow = this.LuminanceRow(selected, 0.3)
    let! ordinaryRow = this.LuminanceRow(ordinary, 0.3)
    let selectedPixel = selectedRow[selectedRow.Length / 3]
    let ordinaryPixel = ordinaryRow[ordinaryRow.Length / 3]
    Assert.True([| selectedPixel |] |> contrastsWith ordinaryPixel, "Selected date must remain visible")
    let! _ = ordinary.EvaluateAsync("el => el.setAttribute('aria-current', 'date')")

    let! todayBorder =
      ordinary.EvaluateAsync<float>("el => parseFloat(getComputedStyle(el).borderTopWidth)")

    Assert.True(todayBorder >= 1.0, "Today's date must retain a border")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``forced colors distinguish selected clock values``(dark: bool) = task {
    do! this.Open("time-availability", dark)

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Scheduled time", Exact = true))
        .ClickAsync()

    let hour = this.Page.Locator(".weave-time-picker__number--selected")
    do! this.Expect(hour).ToHaveCountAsync(1)

    let! colors =
      hour.EvaluateAsync<string array>(
        """el => [getComputedStyle(el).color,
          getComputedStyle(document.querySelector('.weave-time-picker__hand')).fill,
          getComputedStyle(el).forcedColorAdjust]"""
      )

    Assert.NotEqual<string>(colors[0], colors[1])

    if this.Page.Context.Browser.BrowserType.Name <> "webkit" then
      Assert.Equal("none", colors[2])

    let! contrast =
      hour.EvaluateAsync<float>(
        """el => {
          const color = value => value.match(/\d+/g).slice(0, 3).map(Number).map(v => {
            const c = v / 255;
            return c <= 0.04045 ? c / 12.92 : ((c + 0.055) / 1.055) ** 2.4;
          });
          const luminance = rgb => 0.2126 * rgb[0] + 0.7152 * rgb[1] + 0.0722 * rgb[2];
          const text = luminance(color(getComputedStyle(el).color));
          const fill = luminance(color(getComputedStyle(document.querySelector('.weave-time-picker__hand')).fill));
          return (Math.max(text, fill) + 0.05) / (Math.min(text, fill) + 0.05);
        }"""
      )

    Assert.True(contrast >= 4.5, $"Clock number contrast: %f{contrast}")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``Given forced colors then ticks on both sides of the thumb extend visibly beyond the track``
    (dark: bool)
    = task {
    do! this.Open("slider", dark)

    let slider =
      this.Page.GetByRole(AriaRole.Slider, PageGetByRoleOptions(Name = "With Ticks"))

    for _ in 1..3 do
      do! slider.PressAsync("ArrowRight")

    do!
      this.Expect(slider).ToHaveAttributeAsync("aria-valuenow", Text.RegularExpressions.Regex(@"^3(\.0+)?$"))

    let! row = this.LuminanceRow(slider, 0.25)

    for percent in [ 20; 80 ] do
      let x = row.Length * percent / 100

      Assert.True(row.[x - 2 .. x + 2] |> contrastsWith row.[x - 8], $"Tick at %d{percent}%%")
  }

  [<Theory>]
  [<InlineData(false)>]
  [<InlineData(true)>]
  member this.``Given forced colors then completed buffered and remaining work are distinguishable``
    (dark: bool)
    = task {
    do! this.Open("progress", dark)
    let! row = this.LuminanceRow(this.Page.GetByTestId("linear"), 0.5)
    // This fixture is 25% complete and 75% buffered. Stay clear of region boundaries.
    let completed = row.[row.Length / 8]
    let buffered = row.[row.Length / 3 .. row.Length * 2 / 3]
    let remaining = row.[row.Length * 7 / 8]
    Assert.True([| remaining |] |> contrastsWith completed, "Completed vs remaining")
    Assert.True(buffered |> contrastsWith completed, "Buffered vs completed")
    Assert.True(buffered |> contrastsWith remaining, "Buffered vs remaining")
  }
