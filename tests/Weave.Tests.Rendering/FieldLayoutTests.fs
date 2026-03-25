module Weave.Tests.Rendering.FieldLayoutTests

open Xunit

type FieldLayoutTests() =
  inherit LayoutTestBase("field")

  [<Fact>]
  member this.``floated label has scale transform applied``() = task {
    do! this.LoadFixture()

    let! transform = this.ComputedStyle("#label-floated", "transform")

    // scale(0.75) is serialized as a matrix: matrix(0.75, 0, 0, 0.75, tx, ty)
    Assert.Contains("0.75", transform)
  }

  [<Fact>]
  member this.``non-floated label does not have scale transform``() = task {
    do! this.LoadFixture()

    let! transform = this.ComputedStyle("#label-standard", "transform")

    // Non-floated label should use scale(1), serialized as matrix(1, 0, 0, 1, tx, ty)
    Assert.DoesNotContain("0.75", transform)
  }

  [<Fact>]
  member this.``floated label is positioned above non-floated label``() = task {
    do! this.LoadFixture()
    let! standard = this.Page.Locator("#label-standard").BoundingBoxAsync()
    and! floated = this.Page.Locator("#label-floated").BoundingBoxAsync()

    and! standardControlY =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#field-standard .weave-field__control').getBoundingClientRect().top"
      )

    and! floatedControlY =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#field-floated .weave-field__control').getBoundingClientRect().top"
      )

    let standardRelY = float standard.Y - standardControlY
    let floatedRelY = float floated.Y - floatedControlY

    Assert.True(
      floatedRelY < standardRelY,
      $"Floated label (relY={floatedRelY}) should be higher than non-floated label (relY={standardRelY})"
    )
  }

  [<Fact>]
  member this.``full-width field fills its container``() = task {
    do! this.LoadFixture()
    let! container = this.Page.Locator("#full-width-container").BoundingBoxAsync()
    and! field = this.Page.Locator("#field-full-width").BoundingBoxAsync()

    Assert.True(
      abs (field.Width - container.Width) <= 1.0f,
      $"Full-width field ({field.Width}px) should fill its container ({container.Width}px)"
    )
  }

  [<Fact>]
  member this.``help text is hidden by default``() = task {
    do! this.LoadFixture()

    let! maxHeight = this.ComputedStyle("#help-text", "maxHeight")

    Assert.Equal("0px", maxHeight)
  }

  [<Fact>]
  member this.``help text is visible when show-help-text modifier is applied``() = task {
    do! this.LoadFixture()

    let! maxHeight = this.ComputedStyle("#help-text-shown", "maxHeight")

    Assert.True(maxHeight <> "0px")
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``field border is visible in both themes``(theme: string) = task {
    do! this.LoadFixture()
    do! this.SetTheme(theme)

    let! borderColor = this.ComputedStyle("#field-outlined .weave-field__outline", "borderColor")

    Assert.False(
      borderColor = "transparent" || borderColor = "rgba(0, 0, 0, 0)",
      $"Field border in {theme} theme ('{borderColor}') should not be transparent"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``field layout is stable across themes``(theme: string) = task {
    do! this.LoadFixture()
    let! defaultBox = this.Page.Locator("#field-standard").BoundingBoxAsync()
    do! this.SetTheme(theme)
    let! themedBox = this.Page.Locator("#field-standard").BoundingBoxAsync()

    Assert.True(
      abs (defaultBox.Height - themedBox.Height) <= 1.0f,
      $"Field height should not shift between themes (default={defaultBox.Height}px, {theme}={themedBox.Height}px)"
    )
  }

  [<Fact>]
  member this.``outlined field renders a fieldset outline``() = task {
    do! this.LoadFixture()
    let! outline = this.Page.Locator("#field-outlined .weave-field__outline").BoundingBoxAsync()

    Assert.True(
      outline.Width > 0.0f,
      $"Outlined field should render a visible outline (width={outline.Width}px)"
    )

    Assert.True(
      outline.Height > 0.0f,
      $"Outlined field should render a visible outline (height={outline.Height}px)"
    )
  }
