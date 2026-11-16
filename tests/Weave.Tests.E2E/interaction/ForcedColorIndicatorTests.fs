namespace Weave.Tests.E2E

open System
open Microsoft.Playwright
open Xunit

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
