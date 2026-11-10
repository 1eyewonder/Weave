namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

type ColorPickerPaletteInteractionTests(fixture: TestFixture) =
  inherit ColorPickerTestBase(fixture)

  member this.Palette =
    this.Picker.GetByRole(AriaRole.Group, LocatorGetByRoleOptions(Name = "Color palette", Exact = true))

  member this.RedSwatch =
    this.Palette.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "#FF0000", Exact = true))

  [<Fact>]
  member this.``Given a palette when a swatch is selected then its color is applied``() = task {
    do! this.NavigateTo("color-picker")

    let swatch =
      this.Palette.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "#6200EA", Exact = true))

    do! swatch.ClickAsync()
    do! this.Expect(this.Value).ToHaveTextAsync("#6200EA")
    do! this.Expect(swatch).ToHaveAttributeAsync("aria-pressed", "true")
  }

  [<Fact>]
  member this.``Given an unsaved color when saved then its swatch appears``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("save-swatch").ClickAsync()
    do! this.Expect(this.RedSwatch).ToHaveAttributeAsync("aria-pressed", "true")
  }

  [<Fact>]
  member this.``Given a saved color when saved again then no duplicate appears``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("save-swatch").ClickAsync()
    do! this.Expect(this.RedSwatch).ToBeVisibleAsync()
    do! this.Page.GetByTestId("save-swatch").ClickAsync()
    do! this.Expect(this.RedSwatch).ToHaveCountAsync(1)
  }

  [<Fact>]
  member this.``Given a selected saved color when removed then the selected color is retained``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("save-swatch").ClickAsync()
    do! this.Expect(this.RedSwatch).ToBeVisibleAsync()
    do! this.Page.GetByTestId("remove-swatch").ClickAsync()
    do! this.Expect(this.RedSwatch).ToHaveCountAsync(0)
    do! this.Expect(this.Value).ToHaveTextAsync("#FF0000")
  }

  [<Fact>]
  member this.``Given saved swatches when cleared then the palette is hidden``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("clear-swatches").ClickAsync()
    do! this.Expect(this.Palette).ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``Given an empty palette when a color is saved then the palette returns``() = task {
    do! this.NavigateTo("color-picker")
    do! this.Page.GetByTestId("clear-swatches").ClickAsync()
    do! this.Expect(this.Palette).ToHaveCountAsync(0)
    do! this.Page.GetByTestId("save-swatch").ClickAsync()
    do! this.Expect(this.RedSwatch).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``Given RGB mode when swatches change then the format is retained``() = task {
    do! this.NavigateTo("color-picker")
    do! this.FormatTab("RGB").ClickAsync()
    do! this.Page.GetByTestId("save-swatch").ClickAsync()
    do! this.Expect(this.RedSwatch).ToBeVisibleAsync()
    do! this.Expect(this.FormatTab("RGB")).ToHaveAttributeAsync("aria-selected", "true")
  }
