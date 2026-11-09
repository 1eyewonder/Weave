namespace Weave.Tests.E2E

open Xunit

type TextFieldInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``invalid field has aria-invalid attribute``() = task {
    do! this.NavigateTo("textfield")
    let input = this.Page.Locator("input[aria-invalid='true']").First
    do! this.Expect(input).ToBeVisibleAsync()
    do! this.Expect(input).ToHaveAttributeAsync("aria-describedby", "text-email-error")
  }

  [<Fact>]
  member this.``aria-describedby points to visible help text``() = task {
    do! this.NavigateTo("textfield")
    let helpText = this.Page.Locator("#text-email-error")
    do! this.Expect(helpText).ToBeVisibleAsync()
    do! this.Expect(helpText).ToHaveTextAsync("Invalid email address")
  }

  [<Fact>]
  member this.``multiline field renders a textarea element``() = task {
    do! this.NavigateTo("textfield")
    let textarea = this.Page.Locator("textarea.weave-field__input").Last
    do! this.Expect(textarea).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``persistent label stays above its control while typing``() = task {
    do! this.NavigateTo("textfield")
    let field = this.Page.Locator(".weave-field--label-above")
    let input = field.Locator("input")
    let label = field.Locator("label")
    do! this.Expect(label).ToBeVisibleAsync()
    do! input.FillAsync("Hello")
    do! this.Expect(input).ToHaveValueAsync("Hello")
    let! labelBox = label.BoundingBoxAsync()
    let! inputBox = input.BoundingBoxAsync()
    Assert.NotNull(labelBox)
    Assert.NotNull(inputBox)

    Assert.True(
      labelBox.Y + labelBox.Height <= inputBox.Y,
      "The label should remain above the input after typing"
    )
  }
