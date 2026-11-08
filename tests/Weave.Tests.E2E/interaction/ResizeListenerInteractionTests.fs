namespace Weave.Tests.E2E

open Xunit

type ResizeListenerInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

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
