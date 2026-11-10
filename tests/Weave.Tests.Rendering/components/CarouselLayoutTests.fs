module Weave.Tests.Rendering.CarouselLayoutTests

open Xunit
open Microsoft.Playwright

type CarouselLayoutTests() =
  inherit LayoutTestBase("carousel")

  [<Theory>]
  [<InlineData(320)>]
  [<InlineData(1280)>]
  member this.``slides overlap and controls fit within narrow and wide containers``(width: int) = task {
    do! this.LoadFixture width
    let! first = this.Page.Locator("#first").BoundingBoxAsync()
    let! second = this.Page.Locator("#second").BoundingBoxAsync()
    let! carousel = this.Page.Locator("#carousel").BoundingBoxAsync()
    let! controls = this.Page.Locator(".weave-carousel__controls").BoundingBoxAsync()
    Assert.Equal(first.Y, second.Y)
    Assert.Equal(first.Width, second.Width)
    Assert.True(controls.Y >= first.Y + first.Height)
    Assert.True(controls.X + controls.Width <= carousel.X + carousel.Width)
    Assert.True(carousel.X + carousel.Width <= float32 width)
  }

  [<Fact>]
  member this.``changing selection does not shift controls below unequal slides``() = task {
    do! this.LoadFixture()
    let controls = this.Page.Locator(".weave-carousel__controls")
    let! before = controls.BoundingBoxAsync()

    let! _ =
      this.Page.EvaluateAsync(
        """() => {
      document.querySelector('#first').classList.remove('weave-carousel__item--active');
      document.querySelector('#second').classList.add('weave-carousel__item--active');
    }"""
      )

    let! after = controls.BoundingBoxAsync()
    Assert.Equal(before.Y, after.Y)
  }

  [<Fact>]
  member this.``outgoing slide stays painted throughout the crossfade``() = task {
    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ReducedMotion = ReducedMotion.NoPreference))
    do! this.LoadFixture()

    let! overlaps =
      this.Page.EvaluateAsync<bool>(
        """async () => {
      const first = document.querySelector('#first');
      const second = document.querySelector('#second');
      await new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)));
      first.classList.remove('weave-carousel__item--active');
      second.classList.add('weave-carousel__item--active');
      await new Promise(resolve => setTimeout(resolve, 100));
      const outgoing = getComputedStyle(first);
      const incoming = getComputedStyle(second);
      return outgoing.visibility === 'visible' && incoming.visibility === 'visible'
        && Number(outgoing.opacity) > 0 && Number(outgoing.opacity) < 1
        && Number(incoming.opacity) > 0 && Number(incoming.opacity) < 1;
    }"""
      )

    Assert.True(overlaps, "Both slides should remain painted during the crossfade, without a blank frame")
    do! this.Expect(this.Page.Locator("#first")).ToBeHiddenAsync()
  }

  [<Fact>]
  member this.``no transition attribute switches slides immediately``() = task {
    do! this.LoadFixture()

    let! _ =
      this.Page.EvaluateAsync(
        """() => {
      document.querySelector('#carousel').classList.add('weave-carousel--no-transition');
      document.querySelector('#first').classList.remove('weave-carousel__item--active');
      document.querySelector('#second').classList.add('weave-carousel__item--active');
    }"""
      )

    let! duration = this.ComputedStyle("#second", "transitionDuration")
    Assert.Equal("0s", duration)
    do! this.Expect(this.Page.Locator("#first")).ToBeHiddenAsync()
    do! this.Expect(this.Page.Locator("#second")).ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``reduced motion disables slide transitions``() = task {
    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ReducedMotion = ReducedMotion.Reduce))
    do! this.LoadFixture()
    let! duration = this.ComputedStyle("#first", "transitionDuration")
    Assert.Equal("0s", duration)
  }
