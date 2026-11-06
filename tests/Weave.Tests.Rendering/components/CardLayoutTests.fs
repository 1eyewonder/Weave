module Weave.Tests.Rendering.CardLayoutTests

open Xunit
open Weave.Tests.Rendering.ContainmentAssertions

type CardLayoutTests() =
  inherit LayoutTestBase("card")

  [<Fact>]
  member this.``header, content and actions stack vertically``() = task {
    do! this.LoadFixture()
    let! header = this.Page.Locator("#card-full .weave-card__header").BoundingBoxAsync()

    and! content =
      this.Page.Locator("#card-full .weave-card__content").BoundingBoxAsync()

    and! actions =
      this.Page.Locator("#card-full .weave-card__actions").BoundingBoxAsync()

    Assert.True(header.Y < content.Y, $"Header (y={header.Y}) should be above content (y={content.Y})")
    Assert.True(content.Y < actions.Y, $"Content (y={content.Y}) should be above actions (y={actions.Y})")
  }

  [<Fact>]
  member this.``long title does not overlap the header action``() = task {
    do! this.LoadFixture()
    let! title = this.Page.Locator("#card-full .weave-card__title").BoundingBoxAsync()
    and! action = this.Page.Locator("#card-action").BoundingBoxAsync()

    Assert.True(
      title.X + title.Width <= action.X + tolerance,
      $"Title right edge ({title.X + title.Width}) should not overlap the action (x={action.X})"
    )
  }

  [<Fact>]
  member this.``header action stays inside the card``() = task {
    do! this.LoadFixture()
    let! card = this.Page.Locator("#card-full").BoundingBoxAsync()
    and! action = this.Page.Locator("#card-action").BoundingBoxAsync()

    assertContainedWithin "card" "header action" card action
  }

  [<Fact>]
  member this.``footer button aligns to the trailing edge``() = task {
    do! this.LoadFixture()

    let! actions =
      this.Page.Locator("#card-full .weave-card__actions").BoundingBoxAsync()

    and! button = this.Page.Locator("#card-footer-button").BoundingBoxAsync()

    Assert.True(
      button.X > actions.X + actions.Width / 2.0f,
      $"Footer button (x={button.X}) should sit in the trailing half of the actions row"
    )
  }

  [<Theory>]
  [<InlineData("light")>]
  [<InlineData("dark")>]
  member this.``default card has a visible border in both themes``(theme: string) = task {
    do! this.LoadFixture()
    do! this.SetTheme(theme)
    let! width = this.ComputedStyle("#card-full", "borderTopWidth")

    Assert.Equal("1px", width)
  }

  [<Fact>]
  member this.``header leading content sits before the title``() = task {
    do! this.LoadFixture()

    let! leading = this.Page.Locator("#card-leading").BoundingBoxAsync()
    and! title = this.Page.Locator("#card-full .weave-card__title").BoundingBoxAsync()

    Assert.True(
      leading.X + leading.Width <= title.X + tolerance,
      $"Leading right edge ({leading.X + leading.Width}) should be left of the title (x={title.X})"
    )
  }

  [<Fact>]
  member this.``media fills the card edge to edge``() = task {
    do! this.LoadFixture()

    let! card = this.Page.Locator("#card-media").BoundingBoxAsync()
    and! image = this.Page.Locator("#card-media-image").BoundingBoxAsync()

    Assert.True(
      abs (image.Width - (card.Width - 2.0f)) <= tolerance,
      $"Image ({image.Width}px) should span the card ({card.Width}px) inside its border"
    )

    Assert.True(
      abs (image.Height - 80.0f) <= tolerance,
      $"Image ({image.Height}px) should fill the media height"
    )
  }
