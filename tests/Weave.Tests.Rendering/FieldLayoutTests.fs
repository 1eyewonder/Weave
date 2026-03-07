module Weave.Tests.Rendering.FieldLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type FieldLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "field.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``floated label has scale transform applied``() = task {
    do! this.LoadFixture()

    let! transform =
      this.Page.EvaluateAsync<string> "() => getComputedStyle(document.querySelector('#label-floated')).transform"

    // scale(0.75) is serialized as a matrix: matrix(0.75, 0, 0, 0.75, tx, ty)
    Assert.Contains("0.75", transform)
  }

  [<Fact>]
  member this.``non-floated label does not have scale transform``() = task {
    do! this.LoadFixture()

    let! transform =
      this.Page.EvaluateAsync<string> "() => getComputedStyle(document.querySelector('#label-standard')).transform"

    // Non-floated label should use scale(1), serialized as matrix(1, 0, 0, 1, tx, ty)
    Assert.DoesNotContain("0.75", transform)
  }

  [<Fact>]
  member this.``floated label is positioned above non-floated label``() = task {
    do! this.LoadFixture()
    let! standard = this.Page.Locator("#label-standard").BoundingBoxAsync()
    let! floated = this.Page.Locator("#label-floated").BoundingBoxAsync()

    // Floated label renders higher (lower Y) relative to its own field control
    // Compare Y relative to each field control
    let! standardControlY =
      this.Page.EvaluateAsync<float>(
        "() => document.querySelector('#field-standard .weave-field__control').getBoundingClientRect().top"
      )

    let! floatedControlY =
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
    let! field = this.Page.Locator("#field-full-width").BoundingBoxAsync()

    Assert.True(
      abs (field.Width - container.Width) <= 1.0f,
      $"Full-width field ({field.Width}px) should fill its container ({container.Width}px)"
    )
  }

  [<Fact>]
  member this.``help text is hidden by default``() = task {
    do! this.LoadFixture()

    let! maxHeight =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#help-text')).maxHeight"
      )

    Assert.Equal("0px", maxHeight)
  }

  [<Fact>]
  member this.``help text is visible when show-help-text modifier is applied``() = task {
    do! this.LoadFixture()

    let! maxHeight =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#help-text-shown')).maxHeight"
      )

    Assert.True(maxHeight <> "0px")
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
