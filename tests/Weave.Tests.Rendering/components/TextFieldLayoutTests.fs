module Weave.Tests.Rendering.TextFieldLayoutTests

open Xunit

type TextFieldLayoutTests() =
  inherit LayoutTestBase("textfield")

  static member assertNonZeroPadding (msg: string) (padding: string) : unit =
    let condition = padding <> "0px"
    Assert.True(condition, msg)

  [<Fact>]
  member this.``text field has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#textfield-standard").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Text field width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Text field height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``floated label is positioned above non-floated label``() = task {
    do! this.LoadFixture()
    let! standardLabel = this.Page.Locator("#label-standard").BoundingBoxAsync()
    and! floatedLabel = this.Page.Locator("#label-floated").BoundingBoxAsync()

    and! standardControl =
      this.Page.Locator("#textfield-standard .weave-field__control").BoundingBoxAsync()

    and! floatedControl =
      this.Page.Locator("#textfield-floated .weave-field__control").BoundingBoxAsync()

    let standardRelY = float standardLabel.Y - float standardControl.Y
    let floatedRelY = float floatedLabel.Y - float floatedControl.Y

    Assert.True(
      floatedRelY < standardRelY,
      $"Floated label (relY={floatedRelY}) should be higher than non-floated label (relY={standardRelY})"
    )
  }

  [<Fact>]
  member this.``full-width field fills its container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#full-width-container").BoundingBoxAsync()
    and! field = this.Page.Locator("#textfield-full-width").BoundingBoxAsync()

    Assert.True(
      abs (field.Width - container.Width) <= 1.0f,
      $"Full-width field ({field.Width}px) should fill its container ({container.Width}px)"
    )
  }

  [<Fact>]
  member this.``multiline field renders a textarea element``() = task {
    do! this.LoadFixture()

    let! count =
      this.Page.Locator("#textfield-multiline textarea.weave-field__input").CountAsync()

    Assert.True(count > 0, "Multiline text field should render a textarea element")
  }

  [<Fact>]
  member this.``multiline textarea has min-height applied``() = task {
    do! this.LoadFixture()

    let! minHeight =
      this.ComputedStyle("#textfield-multiline textarea.weave-field__input", "minHeight")

    Assert.False(
      minHeight = "0px" || minHeight = "none",
      $"Multiline textarea min-height ({minHeight}) should be set"
    )
  }

  [<Fact>]
  member this.``character counter is visible``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#textfield-counter").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Counter width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Counter height {box.Height}px should be > 0")
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``text field layout is stable across themes``(theme: string) = task {
    do! this.LoadFixture()
    let! defaultBox = this.Page.Locator("#textfield-standard").BoundingBoxAsync()
    do! this.SetTheme(theme)
    let! themedBox = this.Page.Locator("#textfield-standard").BoundingBoxAsync()

    Assert.True(
      abs (defaultBox.Height - themedBox.Height) <= 1.0f,
      $"Text field height should not shift between themes (default={defaultBox.Height}px, {theme}={themedBox.Height}px)"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``outlined field border is visible in both themes``(theme: string) = task {
    do! this.LoadFixture()
    do! this.SetTheme(theme)

    let! borderColor =
      this.ComputedStyle("#textfield-outlined .weave-field__outline", "borderColor")

    Assert.False(
      borderColor = "transparent" || borderColor = "rgba(0, 0, 0, 0)",
      $"Outlined field border in {theme} theme ('{borderColor}') should not be transparent"
    )
  }

  [<Fact>]
  member this.``input font size is inherited from typography class``() = task {
    do! this.LoadFixture()

    let! fontSize =
      this.ComputedStyle("#textfield-standard .weave-field__input", "fontSize")

    Assert.False(
      fontSize = "" || fontSize = "0px",
      $"TextField input font-size ('{fontSize}') should be set via the weave-typography--body2 class"
    )
  }

  [<Fact>]
  member this.``standard input has non-zero vertical padding``() = task {
    do! this.LoadFixture()

    let! paddingTop =
      this.ComputedStyle("#textfield-standard .weave-field__input", "paddingTop")

    and! paddingBottom =
      this.ComputedStyle("#textfield-standard .weave-field__input", "paddingBottom")

    TextFieldLayoutTests.assertNonZeroPadding
      "Standard TextField input paddingTop should be non-zero"
      paddingTop

    TextFieldLayoutTests.assertNonZeroPadding
      "Standard TextField input paddingBottom should be non-zero"
      paddingBottom
  }

  [<Theory>]
  [<InlineData(320, "compact", "empty")>]
  [<InlineData(320, "compact", "populated")>]
  [<InlineData(320, "standard", "empty")>]
  [<InlineData(320, "standard", "populated")>]
  [<InlineData(320, "spacious", "empty")>]
  [<InlineData(320, "spacious", "populated")>]
  [<InlineData(1280, "compact", "empty")>]
  [<InlineData(1280, "compact", "populated")>]
  [<InlineData(1280, "standard", "empty")>]
  [<InlineData(1280, "standard", "populated")>]
  [<InlineData(1280, "spacious", "empty")>]
  [<InlineData(1280, "spacious", "populated")>]
  member this.``variants have equal visible control heights for empty and populated values``
    (width: int, density: string, state: string)
    = task {
    do! this.LoadFixture(width)
    let matrix = this.Page.Locator("#textfield-variant-sizing")

    let! _ =
      matrix.EvaluateAsync("(el, density) => el.className = 'weave-density--' + density", density)

    let mutable heights = []

    for variant in [ "standard"; "filled"; "outlined" ] do
      let field = this.Page.Locator($"#textfield-sizing-{variant}-{state}")
      let! control = field.Locator(".weave-field__control").BoundingBoxAsync()
      heights <- (variant, control.Height) :: heights

      if state = "populated" then
        let! label = field.Locator(".weave-field__label span").BoundingBoxAsync()
        let input = field.Locator(".weave-field__input")
        let! inputBox = input.BoundingBoxAsync()

        let! paddingTop =
          input.EvaluateAsync<float>("el => parseFloat(getComputedStyle(el).paddingTop)")

        Assert.True(
          float (label.Y + label.Height) <= float inputBox.Y + paddingTop + 1.0,
          $"{variant} floating label should remain above the entered text"
        )

    let controlHeights = heights |> List.map snd
    let difference = List.max controlHeights - List.min controlHeights

    Assert.True(
      difference <= 1.0f,
      $"{density} {state} controls should have equal heights; measured {heights}"
    )
  }

  [<Theory>]
  [<InlineData(320, "compact")>]
  [<InlineData(320, "standard")>]
  [<InlineData(320, "spacious")>]
  [<InlineData(1280, "compact")>]
  [<InlineData(1280, "standard")>]
  [<InlineData(1280, "spacious")>]
  member this.``empty labels share a baseline across variants``(width: int, density: string) = task {
    do! this.LoadFixture(width)
    let matrix = this.Page.Locator("#textfield-variant-sizing")

    let! _ =
      matrix.EvaluateAsync("(el, density) => el.className = 'weave-density--' + density", density)

    let mutable baselines = []

    for variant in [ "standard"; "filled"; "outlined" ] do
      let field = this.Page.Locator($"#textfield-sizing-{variant}-empty")
      let! control = field.Locator(".weave-field__control").BoundingBoxAsync()
      let! label = field.Locator(".weave-field__label span").BoundingBoxAsync()
      baselines <- (variant, label.Y + label.Height - control.Y) :: baselines

    let labelBaselines = baselines |> List.map snd
    let difference = List.max labelBaselines - List.min labelBaselines

    Assert.True(difference <= 1.0f, $"{density} empty labels should share a baseline; measured {baselines}")
  }

  [<Theory>]
  [<InlineData("primary")>]
  [<InlineData("secondary")>]
  [<InlineData("tertiary")>]
  [<InlineData("error")>]
  [<InlineData("warning")>]
  [<InlineData("success")>]
  [<InlineData("info")>]
  member this.``input caret follows the selected field palette``(color: string) = task {
    do! this.LoadFixture()

    for variant in [ "standard"; "filled"; "outlined" ] do
      let field = this.Page.Locator($"#textfield-sizing-{variant}-empty")

      let! matches =
        field.EvaluateAsync<bool>(
          "(field, color) => { field.classList.add('weave-field--' + color); const input = field.querySelector('.weave-field__input'); const sample = document.createElement('span'); sample.style.color = 'var(--palette-' + color + ')'; field.append(sample); const expected = getComputedStyle(sample).color; const actual = getComputedStyle(input).caretColor; sample.remove(); return actual === expected; }",
          color
        )

      Assert.True(matches, $"{variant} field caret should use the {color} palette")
  }
