namespace Weave.Tests.E2E

open Microsoft.Playwright

[<AbstractClass>]
type ColorPickerTestBase(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member this.Picker = this.Page.GetByTestId("picker")
  member this.Value = this.Page.GetByTestId("value")

  member this.Hex =
    this.Picker.GetByRole(AriaRole.Textbox, LocatorGetByRoleOptions(Name = "Hex", Exact = true))

  member this.Spectrum = this.Picker.Locator(".weave-colorpicker__spectrum")
  member this.Trigger = this.Page.GetByTestId("field").GetByRole(AriaRole.Button).First
  member this.Dialog = this.Page.GetByRole(AriaRole.Dialog)

  member this.Done =
    this.Dialog.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Done", Exact = true))

  member this.FormatTab(name) =
    this.Picker.GetByRole(AriaRole.Tab, LocatorGetByRoleOptions(Name = name, Exact = true))

  member this.Slider(name) =
    this.Picker.GetByRole(AriaRole.Slider, LocatorGetByRoleOptions(Name = name, Exact = true))

  member this.EnterHex(value: string) = task {
    do! this.Hex.FillAsync(value)
    do! this.Hex.PressAsync("Enter")
  }

  member this.UseExternalColor() = task {
    do! this.Page.GetByTestId("external").ClickAsync()
    do! this.Expect(this.Hex).ToHaveValueAsync("#00FF0080")
  }

  member this.OpenField() = task {
    do! this.Trigger.ClickAsync()
    do! this.Expect(this.Dialog.Locator(".weave-colorpicker__spectrum")).ToBeFocusedAsync()
  }
