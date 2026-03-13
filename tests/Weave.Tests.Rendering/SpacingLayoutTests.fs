module Weave.Tests.Rendering.SpacingLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type SpacingLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "spacing.html")

  member this.LoadFixture(viewportWidth: int) = task {
    do! this.Page.SetViewportSizeAsync(viewportWidth, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``mt-4 applies 16px margin-top``() = task {
    do! this.LoadFixture 1280

    let! value =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#margin-mt-4')).marginTop"
      )

    Assert.Equal("16px", value)
  }

  [<Fact>]
  member this.``mt-20 applies 80px margin-top``() = task {
    do! this.LoadFixture 1280

    let! value =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#margin-mt-20')).marginTop"
      )

    Assert.Equal("80px", value)
  }

  [<Fact>]
  member this.``mt mr mb ml apply correct values independently``() = task {
    do! this.LoadFixture 1280
    let el = "#margin-all-sides"

    let! mt =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginTop")

    let! mr =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginRight")

    let! mb =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginBottom")

    let! ml =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginLeft")

    Assert.Equal("8px", mt) // mt-2: 2 * 0.25rem = 0.5rem = 8px
    Assert.Equal("16px", mr) // mr-4: 4 * 0.25rem = 1rem = 16px
    Assert.Equal("32px", mb) // mb-8: 8 * 0.25rem = 2rem = 32px
    Assert.Equal("48px", ml) // ml-12: 12 * 0.25rem = 3rem = 48px
  }

  [<Fact>]
  member this.``mx-4 sets left and right margin to 16px``() = task {
    do! this.LoadFixture 1280
    let el = "#margin-mx-4"

    let! ml =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginLeft")

    let! mr =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginRight")

    Assert.Equal("16px", ml)
    Assert.Equal("16px", mr)
  }

  [<Fact>]
  member this.``my-8 sets top and bottom margin to 32px``() = task {
    do! this.LoadFixture 1280
    let el = "#margin-my-8"

    let! mt =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginTop")

    let! mb =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginBottom")

    Assert.Equal("32px", mt)
    Assert.Equal("32px", mb)
  }

  [<Fact>]
  member this.``ma-2 sets all margins to 8px``() = task {
    do! this.LoadFixture 1280
    let el = "#margin-ma-2"

    let! mt =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginTop")

    let! mr =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginRight")

    let! mb =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginBottom")

    let! ml =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).marginLeft")

    Assert.Equal("8px", mt)
    Assert.Equal("8px", mr)
    Assert.Equal("8px", mb)
    Assert.Equal("8px", ml)
  }

  [<Fact>]
  member this.``pt-4 applies 16px padding-top``() = task {
    do! this.LoadFixture 1280

    let! value =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#padding-pt-4')).paddingTop"
      )

    Assert.Equal("16px", value)
  }

  [<Fact>]
  member this.``px-8 sets left and right padding to 32px``() = task {
    do! this.LoadFixture 1280
    let el = "#padding-px-8"

    let! pl =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingLeft")

    let! pr =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingRight")

    Assert.Equal("32px", pl)
    Assert.Equal("32px", pr)
  }

  [<Fact>]
  member this.``py-4 sets top and bottom padding to 16px``() = task {
    do! this.LoadFixture 1280
    let el = "#padding-py-4"

    let! pt =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingTop")

    let! pb =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingBottom")

    Assert.Equal("16px", pt)
    Assert.Equal("16px", pb)
  }

  [<Fact>]
  member this.``pa-2 sets all padding to 8px``() = task {
    do! this.LoadFixture 1280
    let el = "#padding-pa-2"

    let! pt =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingTop")

    let! pr =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingRight")

    let! pb =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingBottom")

    let! pl =
      this.Page.EvaluateAsync<string>($"() => getComputedStyle(document.querySelector('{el}')).paddingLeft")

    Assert.Equal("8px", pt)
    Assert.Equal("8px", pr)
    Assert.Equal("8px", pb)
    Assert.Equal("8px", pl)
  }

  [<Fact>]
  member this.``mx-auto centers element horizontally``() = task {
    do! this.LoadFixture 1280
    let! parent = this.Page.Locator("#margin-mx-auto >> xpath=..").BoundingBoxAsync()
    let! child = this.Page.Locator("#margin-mx-auto").BoundingBoxAsync()
    let expectedX = parent.X + (parent.Width - child.Width) / 2.0f

    Assert.True(
      abs (child.X - expectedX) <= 1.0f,
      $"Element X {child.X}px should be centered at {expectedX}px within parent"
    )
  }

  [<Fact>]
  member this.``mt-0 mt-sm-8 applies 0px margin below 600px``() = task {
    do! this.LoadFixture 599

    let! value =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#resp-margin')).marginTop"
      )

    Assert.Equal("0px", value)
  }

  [<Fact>]
  member this.``mt-0 mt-sm-8 applies 32px margin at 600px``() = task {
    do! this.LoadFixture 600

    let! value =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#resp-margin')).marginTop"
      )

    Assert.Equal("32px", value)
  }

  [<Fact>]
  member this.``pt-0 pt-md-4 applies 0px padding below 960px``() = task {
    do! this.LoadFixture 959

    let! value =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#resp-padding')).paddingTop"
      )

    Assert.Equal("0px", value)
  }

  [<Fact>]
  member this.``pt-0 pt-md-4 applies 16px padding at 960px``() = task {
    do! this.LoadFixture 960

    let! value =
      this.Page.EvaluateAsync<string>(
        "() => getComputedStyle(document.querySelector('#resp-padding')).paddingTop"
      )

    Assert.Equal("16px", value)
  }
