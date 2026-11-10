namespace Weave.Tests.E2E

open System.Threading.Tasks
open Microsoft.Playwright
open Xunit

type CarouselInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  member private this.VisibleSlide =
    this.Page.Locator(".weave-carousel__item[aria-hidden='false']")

  [<Theory>]
  [<InlineData(390)>]
  [<InlineData(1280)>]
  member this.``dots stay circular and arrow glyphs share their vertical center``(width: int) = task {
    do! this.NavigateTo "carousel"
    do! this.Page.SetViewportSizeAsync(width, 800)

    let! aligned =
      this.Page
        .Locator(".weave-carousel")
        .EvaluateAsync<bool>(
          """el => {
      const center = element => {
        const rect = element.getBoundingClientRect();
        return rect.y + rect.height / 2;
      };
      const dots = [...el.querySelectorAll('.weave-carousel__indicator')];
      const arrows = [...el.querySelectorAll('.weave-carousel__arrow svg')];
      const baseline = center(dots[0]);
      return dots.every(dot => {
        const style = getComputedStyle(dot, '::before');
        return style.width === style.height && style.borderRadius === '50%'
          && Math.abs(center(dot) - baseline) < 1;
      }) && arrows.every(arrow => Math.abs(center(arrow) - baseline) < 1);
    }"""
        )

    Assert.True(
      aligned,
      "Active and inactive dots should be circular, with arrow glyphs centered on the same row"
    )
  }

  [<Fact>]
  member this.``arrows wrap and update the caller selection``() = task {
    do! this.NavigateTo "carousel"

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Previous slide", Exact = true))
        .ClickAsync()

    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Third slide"
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync "2"

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next slide", Exact = true))
        .ClickAsync()

    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
  }

  [<Fact>]
  member this.``indicators and external selection stay synchronized``() = task {
    do! this.NavigateTo "carousel"
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Go to slide 2")).ClickAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync "1"
    do! this.Page.GetByText("Select last", PageGetByTextOptions(Exact = true)).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Go to slide 3")))
        .ToHaveAttributeAsync("aria-current", "true")
  }

  [<Fact>]
  member this.``keyboard navigation preserves keys in slide inputs``() = task {
    do! this.NavigateTo "carousel"
    let viewport = this.Page.Locator(".weave-carousel__viewport")
    do! viewport.FocusAsync()
    do! viewport.PressAsync "End"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Third slide"
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync "2"
    do! this.Expect(viewport).ToBeFocusedAsync()
    do! viewport.PressAsync "ArrowRight"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
    let input = this.Page.GetByPlaceholder "Slide input"
    do! input.FillAsync "Preserved"
    do! input.PressAsync "ArrowLeft"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
    do! viewport.FocusAsync()
    do! viewport.PressAsync "ArrowLeft"
    do! viewport.PressAsync "Home"
    do! this.Expect(input).ToHaveValueAsync "Preserved"
  }

  [<Fact>]
  member this.``inactive slide actions are inert and excluded from accessibility tree``() = task {
    do! this.NavigateTo "carousel"

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Slide action")))
        .ToHaveCountAsync
        0

    do! this.Expect(this.Page.Locator(".weave-carousel__item").Nth(1)).ToHaveAttributeAsync("inert", "")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Go to slide 2")).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Slide action")))
        .ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``reactive collections handle invalid selection single and empty lists``() = task {
    do! this.NavigateTo "carousel"
    do! this.Page.GetByText("Invalid selection", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Third slide"
    do! this.Page.GetByText("Single slide", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
    do! this.Expect(this.Page.Locator(".weave-carousel__controls")).ToHaveCountAsync 0
    do! this.Page.GetByText("Empty slides", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-carousel__item")).ToHaveCountAsync 0
    do! this.Page.GetByText("Restore slides", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Third slide"

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next slide", Exact = true))
        .ClickAsync()

    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
  }

  [<Fact>]
  member this.``negative selection is clamped for display without rewriting caller state``() = task {
    do! this.NavigateTo "carousel"
    do! this.Page.GetByText("Negative selection", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync "-5"

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next slide", Exact = true))
        .ClickAsync()

    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``navigation visibility follows reactive styling attributes``() = task {
    do! this.NavigateTo "carousel"
    do! this.Page.GetByText("Hide arrows", PageGetByTextOptions(Exact = true)).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next slide", Exact = true)))
        .ToBeHiddenAsync()

    do! this.Page.GetByText("Hide indicators", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-carousel__indicators")).ToBeHiddenAsync()
    do! this.Expect(this.Page.Locator(".weave-carousel__controls")).ToBeHiddenAsync()
    do! this.Page.Locator(".weave-carousel__viewport").PressAsync "ArrowRight"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
  }

  [<Fact>]
  member this.``omitting the interval keeps autoplay disabled``() = task {
    do! this.NavigateTo "carousel"
    do! this.Expect(this.Page.Locator(".weave-carousel__rotation")).ToHaveCountAsync 0
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)
    do! Task.Delay 1200
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"

    do!
      this.Expect(this.Page.Locator(".weave-carousel__viewport")).ToHaveAttributeAsync("aria-live", "polite")
  }

  [<Fact>]
  member this.``cycling advances then stops on focus and resumes with play``() = task {
    do! this.NavigateTo "carousel-autoplay"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
    let viewport = this.Page.Locator(".weave-carousel__viewport")
    do! viewport.FocusAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")))
        .ToBeVisibleAsync()

    let! before = this.Page.Locator("#carousel-index").TextContentAsync()
    do! Task.Delay 1200
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync before
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")).ClickAsync()
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)
    do! this.Expect(viewport).ToHaveAttributeAsync("aria-live", "off")
    do! this.Expect(this.Page.Locator("#carousel-index")).Not.ToHaveTextAsync before
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Pause slides")).ClickAsync()
    do! this.Expect(viewport).ToHaveAttributeAsync("aria-live", "polite")
  }

  [<Fact>]
  member this.``pause control stops rotation and hover temporarily suspends it``() = task {
    do! this.NavigateTo "carousel-autoplay"
    do! this.Page.Locator(".weave-carousel__viewport").HoverAsync()
    do! Task.Delay 1200
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Pause slides")).ClickAsync()
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")))
        .ToBeVisibleAsync()

    let! before = this.Page.Locator("#carousel-index").TextContentAsync()
    do! Task.Delay 1200
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync before
  }

  [<Fact>]
  member this.``reduced motion starts cycling paused``() = task {
    do! this.Page.EmulateMediaAsync(PageEmulateMediaOptions(ReducedMotion = ReducedMotion.Reduce))
    do! this.NavigateTo "carousel-autoplay"

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")))
        .ToBeVisibleAsync()

    do! Task.Delay 1200
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")).ClickAsync()
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
  }

  [<Fact>]
  member this.``removing carousel stops timer updates``() = task {
    use! _script =
      this.Page.AddInitScriptAsync(
        """(() => {
      const live = window.carouselIntervals = new Set();
      const start = window.setInterval.bind(window);
      const stop = window.clearInterval.bind(window);
      window.setInterval = (...args) => { const id = start(...args); live.add(id); return id; };
      window.clearInterval = id => { live.delete(id); stop(id); };
    })()"""
      )

    do! this.NavigateTo "carousel-autoplay"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"

    let! activeTimers =
      this.Page.EvaluateAsync<int>("() => window.carouselIntervals.size")

    Assert.Equal(1, activeTimers)
    do! this.Page.GetByText("Remove carousel", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-carousel")).ToHaveCountAsync 0
    let! _ = this.Page.WaitForFunctionAsync("() => window.carouselIntervals.size === 0")
    let! before = this.Page.Locator("#carousel-index").TextContentAsync()
    do! Task.Delay 1200
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync before
    do! this.Page.GetByText("Restore carousel", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)
    let! _ = this.Page.WaitForFunctionAsync("() => window.carouselIntervals.size === 1")
    do! this.Expect(this.Page.Locator("#carousel-index")).Not.ToHaveTextAsync before
  }

  [<Fact>]
  member this.``autoplay waits for multiple slides and resumes after empty and single lists``() = task {
    do! this.NavigateTo "carousel-autoplay"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
    do! this.Page.GetByText("Single slide", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"

    do!
      this.Expect(this.Page.Locator(".weave-carousel__viewport")).ToHaveAttributeAsync("aria-live", "polite")

    do! Task.Delay 1200
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync "1"
    do! this.Page.GetByText("Empty slides", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.Page.Locator(".weave-carousel__item")).ToHaveCountAsync 0
    do! Task.Delay 1200
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync "1"
    do! this.Page.GetByText("Restore slides", PageGetByTextOptions(Exact = true)).ClickAsync()
    // Restoring the taller content can put it under the pointer; move away to allow cycling.
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Third slide"
  }

  [<Fact>]
  member this.``hidden documents suspend autoplay and visible documents resume it``() = task {
    // Headless browsers do not reliably background tabs; control only the visibility input.
    use! _script =
      this.Page.AddInitScriptAsync(
        "Object.defineProperty(document, 'hidden', { configurable: true, get: () => true })"
      )

    do! this.NavigateTo "carousel-autoplay"
    do! Task.Delay 1200
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"

    let! _ =
      this.Page.EvaluateAsync(
        "() => Object.defineProperty(document, 'hidden', { configurable: true, get: () => false })"
      )

    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
  }

  [<Fact>]
  member this.``hiding navigation retains the autoplay pause control``() = task {
    do! this.NavigateTo "carousel-autoplay-custom-controls"
    do! this.Expect(this.Page.Locator(".weave-carousel__navigation")).ToBeHiddenAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Second slide"
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Pause slides")).ClickAsync()
    let! before = this.Page.Locator("#carousel-index").TextContentAsync()
    do! this.Page.Mouse.MoveAsync(1279.f, 799.f)
    do! Task.Delay 1200
    do! this.Expect(this.Page.Locator("#carousel-index")).ToHaveTextAsync before
  }

  [<Fact>]
  member this.``multiple carousels keep controls selection and timers independent``() = task {
    do! this.NavigateTo "carousel-pair"

    let first =
      this.Page.GetByRole(AriaRole.Region, PageGetByRoleOptions(Name = "First carousel"))

    let second =
      this.Page.GetByRole(AriaRole.Region, PageGetByRoleOptions(Name = "Second carousel"))

    let! firstTarget =
      first
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Go to slide 2"))
        .GetAttributeAsync("aria-controls")

    let! secondTarget =
      second
        .GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Go to slide 2"))
        .GetAttributeAsync("aria-controls")

    Assert.NotEqual<string>(firstTarget, secondTarget)
    do! this.Expect(first.Locator(".weave-carousel__viewport")).ToHaveAttributeAsync("id", firstTarget)
    do! this.Expect(second.Locator(".weave-carousel__viewport")).ToHaveAttributeAsync("id", secondTarget)
    do! this.Expect(first.Locator(".weave-carousel__item").First).ToHaveAttributeAsync("aria-label", "1 of 2")
    do! first.GetByRole(AriaRole.Button, LocatorGetByRoleOptions(Name = "Go to slide 2")).ClickAsync()
    do! this.Expect(first.Locator("[aria-hidden='false']")).ToHaveTextAsync "Beta"
    do! this.Page.GetByText("Remove first carousel", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(first).ToHaveCountAsync 0
    do! this.Expect(second.Locator("[aria-hidden='false']")).ToHaveTextAsync "Beta"
    do! this.Expect(second.Locator("[aria-hidden='false']")).ToHaveTextAsync "Alpha"
    do! this.Expect(this.Page.Locator("#first-index")).ToHaveTextAsync "1"
  }

  [<Fact>]
  member this.``navigation focus stops autoplay without changing the play button action``() = task {
    do! this.NavigateTo "carousel-autoplay"

    // WebKit does not necessarily focus buttons on mouse click.
    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next slide", Exact = true))
        .FocusAsync()

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Next slide", Exact = true))
        .ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")))
        .ToBeVisibleAsync()

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Pause slides")))
        .ToBeVisibleAsync()

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Go to slide 3")).FocusAsync()
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Go to slide 3")).ClickAsync()

    do!
      this
        .Expect(this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Play slides")))
        .ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``typed control attributes allow external navigation without a footer``() = task {
    do! this.NavigateTo "carousel-custom-controls"
    do! this.Expect(this.Page.Locator(".weave-carousel__controls")).ToBeHiddenAsync()
    do! this.Page.GetByText("Select last", PageGetByTextOptions(Exact = true)).ClickAsync()
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "Third slide"
    do! this.Page.Locator(".weave-carousel__viewport").PressAsync "Home"
    do! this.Expect(this.VisibleSlide).ToContainTextAsync "First slide"
  }

  [<Fact>]
  member this.``carousel passes accessibility scans``() = task {
    do! this.RunAxeScan "carousel"
    do! this.RunAxeScan("carousel", dark = true, contrast = true)
    do! this.RunAxeScan "carousel-autoplay"
    do! this.RunAxeScan "carousel-custom-controls"
  }
