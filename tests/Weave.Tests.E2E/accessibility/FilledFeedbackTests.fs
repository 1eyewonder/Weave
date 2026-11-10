namespace Weave.Tests.E2E

open System
open System.Globalization
open System.Text.RegularExpressions
open Microsoft.Playwright
open Xunit

module private FeedbackContrast =

  let private rgba (value: string) =
    let channels =
      Regex.Matches(value, @"[\d.]+")
      |> Seq.cast<Match>
      |> Seq.map (fun part -> Double.Parse(part.Value, CultureInfo.InvariantCulture))
      |> Seq.toArray

    match channels.Length with
    | 3 -> Array.append channels [| 1.0 |]
    | 4 -> channels
    | _ -> failwithf "Unexpected computed color: %s" value

  let private over (foreground: float[]) (background: float[]) = [|
    for channel in 0..2 ->
      foreground.[channel] * foreground.[3]
      + background.[channel] * (1.0 - foreground.[3])
  |]

  let private luminance (rgb: float[]) =
    let linear channel =
      let value = channel / 255.0

      if value <= 0.04045 then
        value / 12.92
      else
        ((value + 0.055) / 1.055) ** 2.4

    0.2126 * linear rgb.[0] + 0.7152 * linear rgb.[1] + 0.0722 * linear rgb.[2]

  let ratio first second =
    let first = luminance first
    let second = luminance second
    (max first second + 0.05) / (min first second + 0.05)

  let read (control: ILocator) = task {
    let! colors =
      control.EvaluateAsync<string[]>(
        """el => {
        const control = getComputedStyle(el);
        const surface = getComputedStyle(el.closest('.weave-snackbar,.weave-alert') || el);
        return [control.color, control.backgroundColor, surface.backgroundColor];
      }"""
      )

    let background = over (rgba colors.[1]) (rgba colors.[2])
    let foreground = over (rgba colors.[0]) background
    return foreground, background
  }

[<Collection("Browser input")>]
type FilledFeedbackTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("snackbar-close", false)>]
  [<InlineData("snackbar-close", true)>]
  [<InlineData("snackbar-action", false)>]
  [<InlineData("snackbar-action", true)>]
  [<InlineData("alert-close", false)>]
  [<InlineData("alert-close", true)>]
  [<InlineData("button", false)>]
  [<InlineData("button", true)>]
  [<InlineData("chip", false)>]
  [<InlineData("chip", true)>]
  [<InlineData("button-group", false)>]
  [<InlineData("button-group", true)>]
  member this.``filled controls have distinct readable hover and keyboard focus states``
    (kind: string, dark: bool)
    = task {
    let route, selector =
      match kind with
      | "snackbar-close" ->
        "snackbar-palette",
        fun color -> sprintf ".weave-snackbar--%s.weave-snackbar--filled .weave-snackbar__close" color
      | "snackbar-action" ->
        "snackbar-palette",
        fun color -> sprintf ".weave-snackbar--%s.weave-snackbar--filled .weave-snackbar__action" color
      | "alert-close" ->
        "alert", fun color -> sprintf "[data-testid='alert-%s-filled'] .weave-alert__close" color
      | "button" -> "button", fun color -> sprintf "[data-testid='button-contrast-%s-filled']" color
      | "chip" -> "chip", fun color -> sprintf "[data-testid='chip-color-%s-filled']" color
      | "button-group" ->
        "buttongroup-palette",
        fun color -> sprintf "[data-testid='group-color-%s-filled'] .weave-button" color
      | _ -> invalidArg "kind" kind

    do! this.NavigateTo(route)

    let! _ =
      this.Page.AddStyleTagAsync(
        PageAddStyleTagOptions(
          Content = "*,*::before,*::after {transition:none!important;animation:none!important;}"
        )
      )

    let! _ =
      this.Page.EvaluateAsync<obj>(
        "theme => document.body.setAttribute('data-theme',theme)",
        if dark then "dark" else "light"
      )

    let! hoverCapable =
      this.Page.EvaluateAsync<bool>("() => matchMedia('(hover: hover) and (pointer: fine)').matches")

    Assert.True(hoverCapable, "Hover feedback checks require a desktop pointer with hover support")

    for color in [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ] do
      let control = this.Page.Locator(selector color).First

      let waitForState state = task {
        let! _ =
          this.Page.WaitForFunctionAsync(
            "([selector,state]) => document.querySelector(selector).matches(state)",
            [| selector color; state |],
            PageWaitForFunctionOptions(Timeout = 5000.0f)
          )

        ()
      }

      let! _ =
        this.Page.Locator(":focus").EvaluateAllAsync<obj>("els => els.forEach(el => el.blur())")

      do! this.Page.Mouse.MoveAsync(-10.0f, -10.0f)
      do! control.ScrollIntoViewIfNeededAsync()
      do! waitForState ":not(:hover):not(:focus-visible)"

      let! normalBackground =
        control.EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor")

      let! foreground, background = FeedbackContrast.read control

      Assert.True(
        FeedbackContrast.ratio foreground background >= 4.5,
        sprintf "%s %s normal foreground must remain readable" kind color
      )

      let checkState state = task {
        do! this.Expect(control).Not.ToHaveCSSAsync("background-color", normalBackground)
        let! foreground, stateBackground = FeedbackContrast.read control
        let readability = FeedbackContrast.ratio foreground stateBackground
        let change = FeedbackContrast.ratio background stateBackground

        Assert.True(
          readability >= 4.5,
          sprintf "%s %s %s foreground contrast was %.2f:1" kind color state readability
        )
        // Guard the visually reviewed default palette against nearly identical states.
        // This is a regression heuristic, not an accessibility requirement.
        Assert.True(
          change >= 1.2,
          sprintf
            "%s %s %s background change was only %.2f:1 (normal %A, state %A)"
            kind
            color
            state
            change
            background
            stateBackground
        )
      }

      do! control.HoverAsync()
      do! waitForState ":hover"
      do! checkState "hover"
      do! this.Page.Mouse.MoveAsync(-10.0f, -10.0f)
      do! control.FocusAsync()
      // Return through native keyboard navigation rather than relying on programmatic focus styling.
      do! this.Page.Keyboard.PressAsync("Shift+Tab")
      do! this.Page.Keyboard.PressAsync("Tab")
      do! this.Expect(control).ToBeFocusedAsync()
      do! waitForState ":focus-visible:not(:hover)"
      do! checkState "focus"
  }
