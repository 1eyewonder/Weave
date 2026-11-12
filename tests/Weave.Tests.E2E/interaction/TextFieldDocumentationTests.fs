namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Xunit
open Microsoft.Playwright

[<Collection("Browser input")>]
type TextFieldDocumentationTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.Load(?width: int) = task {
    do! this.Page.SetViewportSizeAsync(defaultArg width 1280, 900)
    do! this.NavigatePage(this.Page, "text-field-documentation")

    let! _ =
      this.Page.WaitForFunctionAsync(
        "() => ['appearance-docs-style', 'appearance-highlighting-style'].every(id => document.getElementById(id)?.sheet)"
      )

    ()
  }

  [<Theory>]
  [<InlineData("with-content", "Basic Usage")>]
  [<InlineData("persistent-labels", "Appearance")>]
  [<InlineData("placeholder", "Appearance")>]
  [<InlineData("adornments", "Appearance")>]
  member this.``consolidated examples preserve a single legacy destination containing its heading``
    (id: string, title: string)
    = task {
    do! this.Load()
    let destination = this.Page.Locator("[id='" + id + "']")
    do! this.Expect(destination).ToHaveCountAsync(1)

    do!
      this
        .Expect(destination.GetByRole(AriaRole.Heading, LocatorGetByRoleOptions(Name = title, Exact = true)))
        .ToHaveCountAsync(1)

    do!
      this
        .Expect(
          this.Page.GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = "With Content", Exact = true))
        )
        .ToHaveCountAsync(0)

    do!
      this
        .Expect(
          this.Page.GetByRole(
            AriaRole.Heading,
            PageGetByRoleOptions(Name = "Persistent labels", Exact = true)
          )
        )
        .ToHaveCountAsync(0)
  }

  [<Fact>]
  member this.``basic usage retains prefilled guidance and demonstrates persistent floating on a value``() =
    task {
      do! this.Load()
      let basic = this.Page.Locator("#with-content")
      do! this.Expect(basic).ToContainTextAsync("Var.Create \"Ada\"")

      let input =
        basic.GetByRole(AriaRole.Textbox, LocatorGetByRoleOptions(Name = "Name", Exact = true))

      do! input.FillAsync("Ada")
      do! input.PressAsync("Tab")
      do! this.Expect(input).ToHaveValueAsync("Ada")
      do! this.Expect(basic.Locator(".weave-field__label--float")).ToContainTextAsync("Name")
    }

  [<Fact>]
  member this.``unique text field guidance and behaviors remain available``() = task {
    do! this.Load()

    for title in
      [
        "Shrink Label Override"
        "Help Text (Validation)"
        "Help Text Color Variations"
        "Disabled & Read Only"
        "Typography Configuration"
        "Max Length & Character Counter"
        "Debounce & onChange"
        "Multiline"
      ] do
      do!
        this
          .Expect(this.Page.GetByRole(AriaRole.Heading, PageGetByRoleOptions(Name = title, Exact = true)))
          .ToHaveCountAsync(1)
  }
