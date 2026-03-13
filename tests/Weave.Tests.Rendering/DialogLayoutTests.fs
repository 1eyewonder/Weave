module Weave.Tests.Rendering.DialogLayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type DialogLayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "dialog.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``dialog window has positive dimensions``() = task {
    do! this.LoadFixture()
    let! box = this.Page.Locator("#dialog-window").BoundingBoxAsync()

    Assert.True(box.Width > 0.0f, $"Dialog window width {box.Width}px should be > 0")
    Assert.True(box.Height > 0.0f, $"Dialog window height {box.Height}px should be > 0")
  }

  [<Fact>]
  member this.``title is above content``() = task {
    do! this.LoadFixture()
    let! title = this.Page.Locator("#dialog-title").BoundingBoxAsync()
    let! content = this.Page.Locator("#dialog-content").BoundingBoxAsync()

    Assert.True(title.Y < content.Y, $"Title (y={title.Y}) should be above content (y={content.Y})")
  }

  [<Fact>]
  member this.``content is above actions``() = task {
    do! this.LoadFixture()
    let! content = this.Page.Locator("#dialog-content").BoundingBoxAsync()
    let! actions = this.Page.Locator("#dialog-actions").BoundingBoxAsync()

    Assert.True(content.Y < actions.Y, $"Content (y={content.Y}) should be above actions (y={actions.Y})")
  }

  [<Fact>]
  member this.``dialog window is centered horizontally``() = task {
    do! this.LoadFixture()
    let viewport = this.Page.ViewportSize
    let! box = this.Page.Locator("#dialog-window").BoundingBoxAsync()

    let windowCenter = box.X + box.Width / 2.0f
    let viewportCenter = float32 viewport.Width / 2.0f

    Assert.True(
      abs (windowCenter - viewportCenter) <= 2.0f,
      $"Dialog center ({windowCenter}px) should be at viewport center ({viewportCenter}px)"
    )
  }

  [<Fact>]
  member this.``backdrop covers the full viewport``() = task {
    do! this.LoadFixture()
    let! backdrop = this.Page.Locator("#dialog-open .weave-dialog__backdrop").BoundingBoxAsync()
    let viewport = this.Page.ViewportSize

    Assert.True(
      backdrop.Width >= float32 viewport.Width - 1.0f,
      $"Backdrop width {backdrop.Width}px should cover viewport width {viewport.Width}px"
    )

    Assert.True(
      backdrop.Height >= float32 viewport.Height - 1.0f,
      $"Backdrop height {backdrop.Height}px should cover viewport height {viewport.Height}px"
    )
  }

  [<Fact>]
  member this.``topcenter dialog window is near top of container``() = task {
    do! this.LoadFixture()
    let! containerBox = this.Page.Locator("#dialog-topcenter").BoundingBoxAsync()
    let! windowBox = this.Page.Locator("#dialog-topcenter-window").BoundingBoxAsync()

    // Window should be near the top of the dialog container
    Assert.True(
      windowBox.Y - containerBox.Y < 100.0f,
      $"Top center dialog window (Y={windowBox.Y}) should be near top of container (Y={containerBox.Y})"
    )
  }

  [<Fact>]
  member this.``topcenter dialog is horizontally centered``() = task {
    do! this.LoadFixture()
    let! containerBox = this.Page.Locator("#dialog-topcenter").BoundingBoxAsync()
    let! windowBox = this.Page.Locator("#dialog-topcenter-window").BoundingBoxAsync()

    let windowCenter = windowBox.X + windowBox.Width / 2.0f
    let containerCenter = containerBox.X + containerBox.Width / 2.0f

    Assert.True(
      abs (windowCenter - containerCenter) <= 2.0f,
      $"Top center dialog ({windowCenter}px) should be horizontally centered ({containerCenter}px)"
    )
  }

  [<Fact>]
  member this.``bottomcenter dialog window is near bottom``() = task {
    do! this.LoadFixture()
    let! windowBox = this.Page.Locator("#dialog-window-bottomcenter").BoundingBoxAsync()

    // Bottom-center dialog's window bottom should be near the viewport bottom
    let windowBottom = windowBox.Y + windowBox.Height
    let viewportHeight = float32 this.Page.ViewportSize.Height

    Assert.True(
      viewportHeight - windowBottom < 100.0f,
      $"Bottom center dialog bottom ({windowBottom}px) should be near viewport bottom ({viewportHeight}px)"
    )
  }
