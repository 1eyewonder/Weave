namespace Weave.Tests.E2E

open Xunit

type ContainerLayoutTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("container-block-sections", 320)>]
  [<InlineData("container-block-sections", 1280)>]
  [<InlineData("container-column-sections", 320)>]
  member this.``content-sized sections fit their children and the default shell fills its parent``
    (route: string, width: int)
    = task {
    do! this.Page.SetViewportSizeAsync(width, 800)
    do! this.NavigateTo(route)

    let bounds (id: string) =
      this.Page.GetByTestId(id).BoundingBoxAsync()

    let! first = bounds "account-section"
    let! second = bounds "notification-section"
    let! following = bounds "following-content"
    let! shell = bounds "default-shell"
    Assert.Equal(40.0f, first.Height)
    Assert.Equal(40.0f, second.Height)
    Assert.Equal(first.Y + first.Height, second.Y)
    Assert.True(second.Y + second.Height <= following.Y)
    Assert.Equal(120.0f, shell.Height)
  }
