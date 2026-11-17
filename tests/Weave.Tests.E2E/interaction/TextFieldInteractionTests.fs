namespace Weave.Tests.E2E

open Xunit

[<Collection("Browser input")>]
type TextFieldInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``multiline length uses normalized newlines and native UTF-16 units``() = task {
    do! this.NavigateTo("multiline-editing")
    let notes = this.Page.GetByLabel("Notes")
    let counter = this.Page.Locator(".weave-field__counter").First
    let initial = String.replicate 18 "a\n" + "\n🙂z"
    do! this.Expect(notes).ToHaveValueAsync(initial)
    do! this.Expect(counter).ToHaveTextAsync("40/50")
    do! notes.PressAsync("Control+End")
    do! notes.PressSequentiallyAsync("12345678901")
    do! this.Expect(notes).ToHaveValueAsync(initial + "1234567890")
    do! this.Expect(counter).ToHaveTextAsync("50/50")
    let! model = this.Page.Locator("output").TextContentAsync()
    Assert.Equal(initial + "1234567890", model)
    do! this.Page.GetByText("Restore notes").ClickAsync()
    do! this.Expect(notes).ToHaveValueAsync(initial)
    do! this.Expect(counter).ToHaveTextAsync("40/50")
  }

  [<Fact>]
  member this.``multiline typing preserves edits made during a queued render``() = task {
    do! this.NavigateTo("multiline-editing")
    let notes = this.Page.GetByLabel("Notes")
    do! notes.FillAsync("")
    do! this.Expect(this.Page.Locator(".weave-field__counter").First).ToHaveTextAsync("0/50")

    let! _ =
      notes.EvaluateAsync(
        """async el => {
      const frame = () => new Promise(requestAnimationFrame);
      const type = ch => {
        el.setRangeText(ch, el.selectionStart, el.selectionEnd, 'end');
        el.dispatchEvent(new InputEvent('input', { bubbles: true, data: ch }));
      };
      await frame();
      // Queue the second edit ahead of the first edit's render, then type against the resulting DOM.
      const second = frame().then(() => type('2'));
      type('1');
      await second;
      await frame();
      type('3');
    }"""
      )

    do! this.Expect(notes).ToHaveValueAsync("123")
    do! this.Expect(this.Page.Locator("output")).ToHaveTextAsync("123")
  }

  [<Fact>]
  member this.``multiline typing retains a newer external value during a queued render``() = task {
    do! this.NavigateTo("multiline-editing")
    let notes = this.Page.GetByLabel("Notes")
    do! notes.FillAsync("")
    do! this.Expect(this.Page.Locator(".weave-field__counter").First).ToHaveTextAsync("0/50")

    let! _ =
      notes.EvaluateAsync(
        """async el => {
          const frame = () => new Promise(requestAnimationFrame);
          const type = ch => {
            el.setRangeText(ch, el.selectionStart, el.selectionEnd, 'end');
            el.dispatchEvent(new InputEvent('input', { bubbles: true, data: ch }));
          };
          await frame();
          const button = [...document.querySelectorAll('button')].find(el => el.textContent === 'Restore notes');
          const restore = frame().then(() => button.click());
          type('1');
          await restore;
          await frame();
          type('3');
        }"""
      )

    let expected = String.replicate 18 "a\n" + "\n🙂z3"
    do! this.Expect(notes).ToHaveValueAsync(expected)
    do! this.Expect(this.Page.Locator("output")).ToHaveTextAsync(expected)
  }

  [<Fact>]
  member this.``multiline rendering preserves the current selection after an edit``() = task {
    do! this.NavigateTo("multiline-editing")
    let notes = this.Page.GetByLabel("Notes")
    do! notes.FillAsync("abcdef")
    do! this.Expect(this.Page.Locator("output")).ToHaveTextAsync("abcdef")

    let! selection =
      notes.EvaluateAsync<string>(
        """async el => {
          const frame = () => new Promise(requestAnimationFrame);
          el.focus();
          el.setSelectionRange(2, 4, 'backward');
          el.setRangeText('xy', 2, 4, 'end');
          el.setSelectionRange(2, 4, 'backward');
          el.dispatchEvent(new InputEvent('input', {bubbles: true, data: 'xy'}));
          await frame();
          await frame();
          return [el.value, el.selectionStart, el.selectionEnd, el.selectionDirection].join('|');
        }"""
      )

    Assert.Equal("abxyef|2|4|backward", selection)
    do! this.Expect(this.Page.Locator("output")).ToHaveTextAsync("abxyef")
  }

  [<Fact>]
  member this.``multiline rendering preserves native minlength validation after typing``() = task {
    do! this.NavigateTo("multiline-editing")
    let notes = this.Page.GetByLabel("Notes")
    do! notes.FillAsync("")
    do! notes.PressSequentiallyAsync("abc")
    do! this.Expect(this.Page.Locator("output")).ToHaveTextAsync("abc")

    let! _ =
      notes.EvaluateAsync(
        "el => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    let! tooShort = notes.EvaluateAsync<bool>("el => el.validity.tooShort")
    Assert.True(tooShort, "Typing fewer than the minimum length must remain invalid")
  }

  [<Fact>]
  member this.``single-line rendering preserves native minlength validation after typing``() = task {
    do! this.NavigateTo("multiline-editing")
    let summary = this.Page.GetByLabel("Summary")
    do! summary.PressSequentiallyAsync("abc")
    do! this.Expect(this.Page.Locator(".weave-field__counter").Last).ToHaveTextAsync("3/50")

    let! _ =
      summary.EvaluateAsync(
        "el => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)))"
      )

    let! tooShort = summary.EvaluateAsync<bool>("el => el.validity.tooShort")
    Assert.True(tooShort, "Typing fewer than the minimum length must remain invalid")
  }

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

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``character counters stay within their fields while typing``(width: int) = task {
    do! this.Page.SetViewportSizeAsync(width, 900)
    do! this.NavigateTo("textfield")

    for label in [ "Max Length"; "Full width counter"; "Multiline Text Field" ] do
      let input = this.Page.GetByLabel(label)
      let field = input.Locator("..").Locator("..")
      let counter = field.Locator(".weave-field__counter")
      do! this.Expect(counter).ToHaveTextAsync("0/50")
      do! input.FillAsync("Hello")
      do! this.Expect(counter).ToHaveTextAsync("5/50")

      let! controlBox = field.Locator(".weave-field__control").BoundingBoxAsync()
      let! counterBox = counter.BoundingBoxAsync()
      let! fieldBox = field.BoundingBoxAsync()
      Assert.True(counterBox.Y >= controlBox.Y + controlBox.Height - 1.0f, label + ": below the control")
      Assert.True(counterBox.X >= fieldBox.X - 1.0f, label + ": within the field start")

      Assert.True(
        counterBox.X + counterBox.Width <= fieldBox.X + fieldBox.Width + 1.0f,
        label + ": within the field end"
      )
  }
