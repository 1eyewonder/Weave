namespace Weave.Tests.E2E

open Xunit

type CheckboxTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("checkbox")

  [<Theory>]
  [<InlineData("long-label-checkbox", "Keep every word of this checkbox label available in a narrow form.")>]
  [<InlineData("unbroken-label-checkbox", "AnUnbrokenTaskIdentifierThatMustRemainFullyReadableInANarrowForm")>]
  member this.``narrow checkbox preserves its full label and square control``(id: string, labelText: string) =
    task {
      do! this.NavigateTo("checkbox")
      do! this.Page.SetViewportSizeAsync(320, 640)
      let root = this.Page.GetByTestId(id)
      let label = root.Locator(".weave-checkbox__label")
      let input = root.Locator("input")
      do! this.Expect(label).ToHaveTextAsync(labelText)
      do! this.Expect(input).ToHaveAccessibleNameAsync(labelText)
      let! bounds = root.BoundingBoxAsync()
      let! box = root.Locator(".weave-checkbox__span").BoundingBoxAsync()
      let! labelBounds = label.BoundingBoxAsync()
      Assert.True(bounds.Width <= 221.0f, $"Checkbox exceeded its 220px container: {bounds.Width}px")

      Assert.True(
        abs (box.Width - box.Height) <= 1.0f,
        $"Checkbox box was distorted: {box.Width} × {box.Height}"
      )

      Assert.True(box.Width >= 20.0f, $"Checkbox box shrank to {box.Width}px")
      Assert.True(labelBounds.Height > box.Height, "The constrained long label must wrap onto multiple lines")

      let! fullLabelVisible =
        label.EvaluateAsync<bool>(
          "el => el.scrollWidth <= el.clientWidth + 1 && el.scrollHeight <= el.clientHeight + 1"
        )

      Assert.True(fullLabelVisible, "The full checkbox label must remain visible without clipping")
      do! label.ClickAsync()
      do! this.Expect(input).ToBeCheckedAsync()
      do! input.FocusAsync()
      do! input.PressAsync("Space")
      do! this.Expect(input).Not.ToBeCheckedAsync()
    }

  [<Fact>]
  member this.``callers can explicitly request a single-line checkbox label``() = task {
    do! this.NavigateTo("checkbox")

    let label =
      this.Page.GetByTestId("nowrap-label-checkbox").Locator(".weave-checkbox__label")

    let! whiteSpace =
      label.EvaluateAsync<string>("el => getComputedStyle(el).whiteSpace")

    Assert.Equal("nowrap", whiteSpace)
  }
