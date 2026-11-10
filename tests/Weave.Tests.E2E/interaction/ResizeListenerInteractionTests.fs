namespace Weave.Tests.E2E

open Xunit

type ResizeListenerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("padding", "10px")>]
  [<InlineData("border", "10px solid transparent")>]
  member this.``Given a tracked element when padding or borders change then its measured width updates``
    (property: string, value: string)
    = task {
    do! this.NavigateTo("resize-listener")
    let target = this.Page.GetByTestId("resize-target")
    let width = this.Page.GetByTestId("tracked-width")

    let! _ =
      target.EvaluateAsync(
        "element => { element.style.boxSizing = 'content-box'; return new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve))); }"
      )

    let! _ =
      target.EvaluateAsync(
        "(element, change) => { element.style.setProperty(change.property, change.value); }",
        {|
          property = property
          value = value
        |}
      )

    do! this.Expect(width).ToHaveTextAsync("140")
  }

  [<Fact>]
  member this.``Given a tracked element when its width changes without a window resize then its measurement updates``
    ()
    = task {
    do! this.NavigateTo("resize-listener")
    let target = this.Page.GetByTestId("resize-target")
    let width = this.Page.GetByTestId("tracked-width")
    do! this.Expect(width).ToHaveTextAsync("120")
    let! _ = target.EvaluateAsync("element => { element.style.width = '173px'; }")
    do! this.Expect(width).ToHaveTextAsync("173")
  }

  [<Fact>]
  member this.``tracking responds while mounted and stops after repeated removals``() = task {
    do! this.NavigateTo("resize-listener")
    let target = this.Page.GetByTestId("resize-target")
    let width = this.Page.GetByTestId("tracked-width")
    let toggle = this.Page.GetByTestId("resize-toggle")

    for _ in 1..3 do
      do! this.Expect(target).ToBeVisibleAsync()
      do! this.Expect(width).ToHaveTextAsync("120")

      let! _ =
        target.EvaluateAsync(
          "el => { el.style.width = '173px'; window.dispatchEvent(new Event('resize')); window.removedResizeTarget = el; }"
        )

      do! this.Expect(width).ToHaveTextAsync("173")
      do! toggle.ClickAsync()
      do! this.Expect(target).ToHaveCountAsync(0)

      let! _ =
        this.Page.EvaluateAsync(
          "() => new Promise(resolve => requestAnimationFrame(() => { window.removedResizeTarget.style.width = '301px'; window.dispatchEvent(new Event('resize')); requestAnimationFrame(() => requestAnimationFrame(resolve)); }))"
        )

      do! this.Expect(width).ToHaveTextAsync("173")
      do! toggle.ClickAsync()
  }
