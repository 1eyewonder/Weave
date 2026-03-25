module Weave.Tests.Rendering.SpacingLayoutTests

open Xunit

type SpacingLayoutTests() =
  inherit LayoutTestBase("spacing")

  [<Fact>]
  member this.``mt-4 applies 16px margin-top``() = task {
    do! this.LoadFixture 1280

    let! value = this.ComputedStyle("#margin-mt-4", "marginTop")

    Assert.Equal("16px", value)
  }

  [<Fact>]
  member this.``mt-20 applies 80px margin-top``() = task {
    do! this.LoadFixture 1280

    let! value = this.ComputedStyle("#margin-mt-20", "marginTop")

    Assert.Equal("80px", value)
  }

  [<Fact>]
  member this.``mt mr mb ml apply correct values independently``() = task {
    do! this.LoadFixture 1280

    let! mt = this.ComputedStyle("#margin-all-sides", "marginTop")
    and! mr = this.ComputedStyle("#margin-all-sides", "marginRight")
    and! mb = this.ComputedStyle("#margin-all-sides", "marginBottom")
    and! ml = this.ComputedStyle("#margin-all-sides", "marginLeft")

    Assert.Equal("8px", mt) // mt-2: 2 * 0.25rem = 0.5rem = 8px
    Assert.Equal("16px", mr) // mr-4: 4 * 0.25rem = 1rem = 16px
    Assert.Equal("32px", mb) // mb-8: 8 * 0.25rem = 2rem = 32px
    Assert.Equal("48px", ml) // ml-12: 12 * 0.25rem = 3rem = 48px
  }

  [<Fact>]
  member this.``mx-4 sets left and right margin to 16px``() = task {
    do! this.LoadFixture 1280

    let! ml = this.ComputedStyle("#margin-mx-4", "marginLeft")
    and! mr = this.ComputedStyle("#margin-mx-4", "marginRight")

    Assert.Equal("16px", ml)
    Assert.Equal("16px", mr)
  }

  [<Fact>]
  member this.``my-8 sets top and bottom margin to 32px``() = task {
    do! this.LoadFixture 1280

    let! mt = this.ComputedStyle("#margin-my-8", "marginTop")
    and! mb = this.ComputedStyle("#margin-my-8", "marginBottom")

    Assert.Equal("32px", mt)
    Assert.Equal("32px", mb)
  }

  [<Fact>]
  member this.``ma-2 sets all margins to 8px``() = task {
    do! this.LoadFixture 1280

    let! mt = this.ComputedStyle("#margin-ma-2", "marginTop")
    and! mr = this.ComputedStyle("#margin-ma-2", "marginRight")
    and! mb = this.ComputedStyle("#margin-ma-2", "marginBottom")
    and! ml = this.ComputedStyle("#margin-ma-2", "marginLeft")

    Assert.Equal("8px", mt)
    Assert.Equal("8px", mr)
    Assert.Equal("8px", mb)
    Assert.Equal("8px", ml)
  }

  [<Fact>]
  member this.``pt-4 applies 16px padding-top``() = task {
    do! this.LoadFixture 1280

    let! value = this.ComputedStyle("#padding-pt-4", "paddingTop")

    Assert.Equal("16px", value)
  }

  [<Fact>]
  member this.``px-8 sets left and right padding to 32px``() = task {
    do! this.LoadFixture 1280

    let! pl = this.ComputedStyle("#padding-px-8", "paddingLeft")
    and! pr = this.ComputedStyle("#padding-px-8", "paddingRight")

    Assert.Equal("32px", pl)
    Assert.Equal("32px", pr)
  }

  [<Fact>]
  member this.``py-4 sets top and bottom padding to 16px``() = task {
    do! this.LoadFixture 1280

    let! pt = this.ComputedStyle("#padding-py-4", "paddingTop")
    and! pb = this.ComputedStyle("#padding-py-4", "paddingBottom")

    Assert.Equal("16px", pt)
    Assert.Equal("16px", pb)
  }

  [<Fact>]
  member this.``pa-2 sets all padding to 8px``() = task {
    do! this.LoadFixture 1280

    let! pt = this.ComputedStyle("#padding-pa-2", "paddingTop")
    and! pr = this.ComputedStyle("#padding-pa-2", "paddingRight")
    and! pb = this.ComputedStyle("#padding-pa-2", "paddingBottom")
    and! pl = this.ComputedStyle("#padding-pa-2", "paddingLeft")

    Assert.Equal("8px", pt)
    Assert.Equal("8px", pr)
    Assert.Equal("8px", pb)
    Assert.Equal("8px", pl)
  }

  [<Fact>]
  member this.``mx-auto centers element horizontally``() = task {
    do! this.LoadFixture 1280
    let! parent = this.Page.Locator("#margin-mx-auto >> xpath=..").BoundingBoxAsync()
    and! child = this.Page.Locator("#margin-mx-auto").BoundingBoxAsync()
    let expectedX = parent.X + (parent.Width - child.Width) / 2.0f

    Assert.True(
      abs (child.X - expectedX) <= 1.0f,
      $"Element X {child.X}px should be centered at {expectedX}px within parent"
    )
  }

  [<Fact>]
  member this.``mt-0 mt-sm-8 applies 0px margin below 600px``() = task {
    do! this.LoadFixture 599

    let! value = this.ComputedStyle("#resp-margin", "marginTop")

    Assert.Equal("0px", value)
  }

  [<Fact>]
  member this.``mt-0 mt-sm-8 applies 32px margin at 600px``() = task {
    do! this.LoadFixture 600

    let! value = this.ComputedStyle("#resp-margin", "marginTop")

    Assert.Equal("32px", value)
  }

  [<Fact>]
  member this.``pt-0 pt-md-4 applies 0px padding below 960px``() = task {
    do! this.LoadFixture 959

    let! value = this.ComputedStyle("#resp-padding", "paddingTop")

    Assert.Equal("0px", value)
  }

  [<Fact>]
  member this.``pt-0 pt-md-4 applies 16px padding at 960px``() = task {
    do! this.LoadFixture 960

    let! value = this.ComputedStyle("#resp-padding", "paddingTop")

    Assert.Equal("16px", value)
  }
