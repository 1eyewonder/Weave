namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright

// Exercise the WebSharper-generated JavaScript, including browser literal matching.
type HighlighterInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  static member PunctuationCases: obj[] seq = seq {
    for punctuation in ".*+?^${}()|[]\\/-#,@:;!&<>'\"`%=~" do
      yield [| box (string punctuation) |]
  }

  [<Fact>]
  member this.``text search and case sensitivity update reactively without rendering markup``() = task {
    do! this.NavigateTo("highlighter")
    let output = this.Page.GetByTestId("single-highlight")
    do! this.Expect(output).ToHaveTextAsync("Weave weave WEAVE; C++ [a]. <b>weave</b>")
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| "Weave"; "weave"; "WEAVE"; "weave" |])
    do! this.Expect(output.Locator("b")).ToHaveCountAsync(0)
    do! this.Page.GetByRole(AriaRole.Switch).PressAsync("Space")
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| "weave"; "weave" |])
    do! this.Page.GetByTestId("highlight-search").FillAsync("C++")
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| "C++" |])
    do! this.Page.GetByTestId("highlight-source").FillAsync("C++ C++ <img src=x onerror=alert(1)>")
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| "C++"; "C++" |])
    do! this.Expect(output.Locator("img")).ToHaveCountAsync(0)
    do! this.Page.GetByTestId("highlight-search").FillAsync("")
    do! this.Expect(output.Locator("mark")).ToHaveCountAsync(0)
    do! this.Expect(output).ToHaveTextAsync("C++ C++ <img src=x onerror=alert(1)>")
    do! this.Page.GetByTestId("highlight-source").FillAsync("")
    do! this.Expect(output).ToHaveTextAsync("")
    do! this.Expect(this.Page.GetByTestId("null-highlight")).ToHaveTextAsync("")
  }

  [<Fact>]
  member this.``multiple terms prefer the longest match and treat punctuation literally``() = task {
    do! this.NavigateTo("highlighter")
    let output = this.Page.GetByTestId("multi-highlight")

    do!
      this
        .Expect(output.Locator("mark"))
        .ToHaveTextAsync([| "Weave"; "weave"; "WEAVE"; "C++"; "[a]."; "weave" |])

    do! this.Page.GetByTestId("change-terms").ClickAsync()
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| "Weave"; "weave"; "WEAVE"; "<b>"; "weave" |])
    do! this.Page.GetByTestId("highlight-source").FillAsync("weaveweave weaver")
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| "weave"; "weave"; "weave" |])
    do! this.Expect(output).ToHaveTextAsync("weaveweave weaver")
    do! this.Page.GetByTestId("highlight-source").FillAsync("unrelated")
    do! this.Expect(output.Locator("mark")).ToHaveCountAsync(0)
    do! this.Expect(output).ToHaveTextAsync("unrelated")
  }

  [<Theory>]
  [<MemberData(nameof HighlighterInteractionTests.PunctuationCases)>]
  [<InlineData("a.b+tag@example.com")>]
  [<InlineData("https://example.com/a-b?q=x&n=2#top")>]
  [<InlineData("C:\\Users\\Pat")>]
  [<InlineData("(555) 123-4567")>]
  [<InlineData("$19.99")>]
  [<InlineData("50%")>]
  [<InlineData("O'Reilly")>]
  [<InlineData("foo_bar")>]
  [<InlineData("Smith, Jane")>]
  [<InlineData(" spaced ")>]
  [<InlineData("\\b")>]
  [<InlineData("(a+)+$")>]
  [<InlineData(".*")>]
  [<InlineData(".*+?^${}()|[]\\")>]
  [<InlineData("\u00e9\U0001f600")>]
  member this.``literal terms preserve punctuation and Unicode offsets``(term: string) = task {
    do! this.NavigateTo("highlighter")
    let source = "prefix " + term + " middle " + term + " suffix"
    do! this.Page.GetByTestId("highlight-source").FillAsync(source)
    do! this.Page.GetByTestId("highlight-search").FillAsync(term)
    do! this.Page.GetByTestId("highlight-terms").FillAsync(term + "\nnever-a-match")

    for id in [ "single-highlight"; "multi-highlight" ] do
      let output = this.Page.GetByTestId(id)
      do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| term; term |])
      let! matches = output.Locator("mark").AllTextContentsAsync()
      Assert.Equal<string>([| term; term |], matches)
      let! preserved = output.TextContentAsync()
      Assert.Equal(source, preserved)
  }

  [<Theory>]
  [<InlineData("\U00010400", "\U00010428")>]
  [<InlineData("\u212a", "k")>]
  [<InlineData("S", "\u017f")>]
  member this.``Unicode case folding preserves original text in both APIs``(upper: string, lower: string) =
    task {
      do! this.NavigateTo("highlighter")
      let source = upper + " / " + lower
      do! this.Page.GetByTestId("highlight-source").FillAsync(source)
      do! this.Page.GetByTestId("highlight-search").FillAsync(lower)
      do! this.Page.GetByTestId("highlight-terms").FillAsync("unmatched\n" + lower)

      for id in [ "single-highlight"; "multi-highlight" ] do
        let output = this.Page.GetByTestId(id)
        do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| upper; lower |])
        let! preserved = output.TextContentAsync()
        Assert.Equal(source, preserved)

      do! this.Page.GetByRole(AriaRole.Switch).PressAsync("Space")

      for id in [ "single-highlight"; "multi-highlight" ] do
        do! this.Expect(this.Page.GetByTestId(id).Locator("mark")).ToHaveTextAsync([| lower |])

      do! this.Page.GetByRole(AriaRole.Switch).PressAsync("Space")

      for id in [ "single-highlight"; "multi-highlight" ] do
        do! this.Expect(this.Page.GetByTestId(id).Locator("mark")).ToHaveTextAsync([| upper; lower |])
    }

  [<Theory>]
  [<InlineData("caf\u00e9", "cafe")>]
  [<InlineData("cafe\u0301", "caf\u00e9")>]
  [<InlineData("\u0130stanbul", "istanbul")>]
  [<InlineData("Stra\u00dfe", "STRASSE")>]
  [<InlineData("O\u2019Reilly", "O'Reilly")>]
  [<InlineData("a\u2014b", "a-b")>]
  member this.``matching keeps accents normalization locale and typographic punctuation distinct``
    (source: string, term: string)
    = task {
    do! this.NavigateTo("highlighter")
    do! this.Page.GetByTestId("highlight-source").FillAsync(source)
    do! this.Page.GetByTestId("highlight-search").FillAsync(source)
    let output = this.Page.GetByTestId("single-highlight")
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| source |])
    do! this.Page.GetByTestId("highlight-search").FillAsync(term)
    do! this.Expect(output.Locator("mark")).ToHaveCountAsync(0)
    let! preserved = output.TextContentAsync()
    Assert.Equal(source, preserved)
  }

  [<Theory>]
  [<InlineData("a  b", " ", 2)>]
  [<InlineData("a\u00a0b", " ", 0)>]
  [<InlineData("a\u00a0b", "\u00a0", 1)>]
  [<InlineData("a\tb", "\t", 1)>]
  [<InlineData("a\nb", "\n", 1)>]
  [<InlineData("hello  world / hello world", "hello world", 1)>]
  member this.``whitespace is literal and source text is preserved exactly``
    (source: string, term: string, count: int)
    = task {
    do! this.NavigateTo("highlighter")
    do! this.Page.GetByTestId("highlight-source").FillAsync(source)
    do! this.Page.GetByTestId("highlight-search").FillAsync(source)
    let output = this.Page.GetByTestId("single-highlight")
    do! this.Expect(output.Locator("mark")).ToHaveCountAsync(1)
    do! this.Page.GetByTestId("highlight-search").FillAsync(term)
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync(Array.create count term)
    let! matches = output.Locator("mark").AllTextContentsAsync()
    Assert.Equal<string>(Array.create count term, matches)
    let! preserved = output.TextContentAsync()
    Assert.Equal(source, preserved)
  }

  [<Fact>]
  member this.``empty and null terms produce no matches while mixed lists retain valid terms``() = task {
    do! this.NavigateTo("highlighter")
    let source = "Weave weave"
    do! this.Page.GetByTestId("highlight-source").FillAsync(source)

    for id in [ "empty-terms"; "null-terms" ] do
      let output = this.Page.GetByTestId(id)
      do! this.Expect(output).ToHaveTextAsync(source)
      do! this.Expect(output.Locator("mark")).ToHaveCountAsync(0)

    do!
      this
        .Expect(this.Page.GetByTestId("mixed-terms").Locator("mark"))
        .ToHaveTextAsync([| "Weave"; "weave" |])

    let multiple = this.Page.GetByTestId("multi-highlight")
    do! this.Expect(multiple.Locator("mark")).ToHaveTextAsync([| "Weave"; "weave" |])
    do! this.Page.GetByTestId("highlight-terms").FillAsync("\n\n")
    do! this.Expect(multiple.Locator("mark")).ToHaveCountAsync(0)
    do! this.Expect(multiple).ToHaveTextAsync(source)
  }

  [<Fact>]
  member this.``earlier matches take precedence over longer terms starting later``() = task {
    do! this.NavigateTo("highlighter")
    do! this.Page.GetByTestId("highlight-source").FillAsync("abcde abcde")
    let output = this.Page.GetByTestId("multi-highlight")

    for terms in [ "bcde\nab"; "ab\nbcde" ] do
      do! this.Page.GetByTestId("highlight-terms").FillAsync(terms)
      do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| "ab"; "ab" |])
      do! this.Expect(output).ToHaveTextAsync("abcde abcde")
  }

  [<Theory>]
  [<InlineData("cat concatenate", "cat", "cat", 2)>]
  [<InlineData("\U0001f44d\U0001f3fd", "\U0001f44d", "\U0001f44d", 1)>]
  member this.``matches may occur inside words or combined emoji``
    (source: string, term: string, expected: string, count: int)
    = task {
    do! this.NavigateTo("highlighter")
    do! this.Page.GetByTestId("highlight-source").FillAsync(source)
    do! this.Page.GetByTestId("highlight-search").FillAsync(term)
    let output = this.Page.GetByTestId("single-highlight")
    do! this.Expect(output.Locator("mark")).ToHaveTextAsync(Array.create count expected)
    let! preserved = output.TextContentAsync()
    Assert.Equal(source, preserved)
  }

  [<Fact>]
  member this.``matched HTML-looking content stays text in both APIs``() = task {
    do! this.NavigateTo("highlighter")
    let source = "<img src=x onerror=alert(1)> &amp; <script>alert(1)</script>"
    do! this.Page.GetByTestId("highlight-source").FillAsync(source)
    do! this.Page.GetByTestId("highlight-search").FillAsync(source)
    do! this.Page.GetByTestId("highlight-terms").FillAsync(source)

    for id in [ "single-highlight"; "multi-highlight" ] do
      let output = this.Page.GetByTestId(id)
      do! this.Expect(output.Locator("mark")).ToHaveTextAsync([| source |])
      do! this.Expect(output.Locator("img, script")).ToHaveCountAsync(0)
      let! preserved = output.TextContentAsync()
      Assert.Equal(source, preserved)
  }

  [<Fact>]
  member this.``bounded dense matches complete with many search terms``() = task {
    do! this.NavigateTo("highlighter")
    let source = String.replicate 2048 "x"
    let terms = [ for i in 1..64 -> "absent-" + string i ] @ [ "x" ]
    do! this.Page.GetByTestId("highlight-source").FillAsync(source)
    do! this.Page.GetByTestId("highlight-search").FillAsync("x")
    do! this.Page.GetByTestId("highlight-terms").FillAsync(String.concat "\n" terms)

    for id in [ "single-highlight"; "multi-highlight" ] do
      let output = this.Page.GetByTestId(id)
      do! this.Expect(output.Locator("mark")).ToHaveCountAsync(2048)
      let! preserved = output.TextContentAsync()
      Assert.Equal(source, preserved)
  }

  [<Fact>]
  member this.``marks inherit typography and use customizable theme tokens``() = task {
    do! this.NavigateTo("highlighter")

    let! _ =
      this.Page.AddStyleTagAsync(
        PageAddStyleTagOptions(
          Content =
            """
      body { --palette-primary-fill: #123456; --palette-primary-fill-text: #ffffff; }
      [data-testid=single-highlight] { font-size: 23px; font-weight: 700; }
    """
        )
      )

    let colored = this.Page.GetByTestId("highlight-primary").Locator("mark")
    do! this.Expect(colored).ToHaveCSSAsync("background-color", "rgb(18, 52, 86)")
    do! this.Expect(colored).ToHaveCSSAsync("color", "rgb(255, 255, 255)")
    let first = this.Page.GetByTestId("single-highlight").Locator("mark").First
    do! this.Expect(first).ToHaveCSSAsync("font-size", "23px")
    do! this.Expect(first).ToHaveCSSAsync("font-weight", "700")
    do! this.Expect(this.Page.GetByTestId("single-highlight")).ToHaveCSSAsync("display", "inline")
  }

  [<Fact>]
  member this.``highlighter passes accessibility and contrast checks in light mode``() =
    this.RunAxeScan("highlighter", contrast = true)

  [<Fact>]
  member this.``highlighter passes accessibility and contrast checks in dark mode``() =
    this.RunAxeScan("highlighter", contrast = true, dark = true)
