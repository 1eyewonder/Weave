namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module HighlighterExamples =

  let private searchExample () =
    let search = Var.Create "item"
    let sensitive = Var.Create false

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        TextField.singleLine (search, labelText = View.Const "Search text")
        Switch.create (sensitive, content = text "Case sensitive")
        WeaveList.create (
          [
            for sentence in
              [
                "This is the first item"
                "This Item has a capital I"
                "Another item in the list"
              ] do
              ListItem.create (
                Highlighter.create (View.Const sentence, search.View, caseSensitive = sensitive.View)
              )
          ]
        )
      ]

    Helpers.codeSampleSection
      "Live Search"
      (Helpers.bodyText
        "Compose a highlighter with lists, table cells, links, or typography. Text and search terms are Views; matching updates as they change. Matching ignores case by default.")
      content
      """let search = Var.Create "item"
let sensitive = Var.Create false

Doc.Concat [
  TextField.singleLine (search, labelText = View.Const "Search text")
  Switch.create (sensitive, content = text "Case sensitive")
  WeaveList.create (
    [
      for sentence in
        [
          "This is the first item"
          "This Item has a capital I"
          "Another item in the list"
        ] do
        ListItem.create (
          Highlighter.create (
            View.Const sentence,
            search.View,
            caseSensitive = sensitive.View
          )
        )
    ]
  )
]
"""

  let private multipleExample () =
    let search = Var.Create "Weave, F#"

    let parseTerms (value: string) =
      value.Split(',') |> Array.map (fun term -> term.Trim()) |> Array.toList

    let terms = search.View |> View.Map parseTerms

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        TextField.singleLine (search, labelText = View.Const "Terms separated by commas")
        p [ Typography.body1 ] [
          Highlighter.createMany (View.Const "Weave helps you build reactive interfaces in F#.", terms)
        ]
      ]

    Helpers.codeSampleSection
      "Multiple Terms"
      (Helpers.bodyText
        "Use createMany for several terms. Empty terms are ignored. Matches proceed left to right without overlapping; at the same start position, the longest term wins. Punctuation such as F# or C++ is literal. This demo splits on commas and trims each term; createMany itself accepts the supplied strings unchanged, including commas and spaces.")
      content
      """let search = Var.Create "Weave, F#"

let parseTerms (value: string) =
  value.Split(',') |> Array.map (fun term -> term.Trim()) |> Array.toList

let terms = search.View |> View.Map parseTerms

Doc.Concat [
  TextField.singleLine (
    search,
    labelText = View.Const "Terms separated by commas"
  )

  p [ Typography.body1 ] [
    Highlighter.createMany (
      View.Const "Weave helps you build reactive interfaces in F#.",
      terms
    )
  ]
]
"""

  let private matchingBehavior () =
    Helpers.sectionPlain
      "Matching Behavior"
      (Helpers.bodyText
        "Highlighter performs literal substring matching and preserves the original text and casing. Both create and createMany follow these rules.")
      (div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        Helpers.guidanceBullet
          "Punctuation is literal"
          "Search for email addresses, URLs, paths, prices, brackets, or regex-looking text such as .* directly. Terms are never interpreted as regular expressions. Straight and curly apostrophes, and hyphens and typographic dashes, remain distinct. HTML-looking content is displayed as text."
        Helpers.guidanceBullet
          "Case matching uses Unicode simple case folding"
          "By default, upper and lower case letters match, including letters outside the Basic Multilingual Plane. Set caseSensitive to true for exact case. Matching is locale-independent: Turkish dotted İ does not match i, and German ß does not expand to ss."
        Helpers.guidanceBullet
          "Accents and Unicode normalization are preserved"
          "cafe does not match café. A precomposed é and an e followed by a combining acute accent (e + U+0301) are also distinct. Normalize your inputs consistently before passing them in if your application needs canonical equivalence. Highlighter does not remove accents or rewrite the displayed text."
        Helpers.guidanceBullet
          "Whitespace is exact"
          "Terms are not trimmed. A space, a nonbreaking space, a tab, and a line break are different characters; repeated spaces are not collapsed for matching. Whitespace-only terms can match. Normal HTML whitespace layout still applies; set white-space: pre-wrap via attrs when line breaks and repeated spaces should remain visible."
        Helpers.guidanceBullet
          "Matches proceed left to right without overlapping"
          "Matching is not limited to whole words: cat also matches within concatenate. The earliest match wins; at the same start position, the longest term wins regardless of term order. For abcde with terms ab and bcde, only ab is highlighted. Empty and null terms are ignored, and duplicate terms do not create duplicate marks."
        Helpers.guidanceBullet
          "Combined characters can be partly highlighted"
          "Matching respects Unicode code points but does not expand to a complete visible character (grapheme). Searching for 👍 within 👍🏽 highlights the base emoji only; combining accents and joined emoji can likewise be split across a mark boundary."
        Helpers.guidanceBullet
          "Multiple terms are supplied as a list"
          "createMany does not split commas, spaces, or other delimiters. A term such as Smith, Jane matches that entire phrase. Any splitting or trimming in a search field belongs to the application, as shown in the comma-separated demo above."
        Helpers.guidanceBullet
          "Keep large result sets bounded"
          "Each match creates a mark element and updates run synchronously. For long documents, many terms, or dense matches, debounce the search input or limit the displayed results. Highlighter does not impose a match limit or virtualize its content."
      ])

  let private apiReference () =
    Helpers.apiSection
      (Helpers.bodyText
        "Literal search highlighting. Returns an inline span containing text nodes and semantic mark elements.")
      [
        Helpers.apiTable "Highlighter.create / Highlighter.createMany" [
          Helpers.apiParam "text" "View<string>" "" "Source text; null or empty text renders empty"
          Helpers.apiParam
            "highlightedText (create)"
            "View<string>"
            ""
            "Literal search term; null or empty terms highlight nothing"
          Helpers.apiParam
            "highlightedTexts (createMany)"
            "View<string list>"
            ""
            "Literal search terms; null and empty entries are ignored; strings are not split or trimmed"
          Helpers.apiParam
            "?caseSensitive"
            "View<bool>"
            "View.Const false"
            "Exact case when true; locale-independent Unicode simple case folding when false"
          Helpers.apiParam
            "?attrs"
            "Attr list"
            "[]"
            "Attributes on the wrapper, including Highlighter.Color styles"
        ]
        Helpers.styleModuleTable "Highlighter.Color" [
          "primary", "Primary fill and text colors"
          "secondary", "Secondary fill and text colors"
          "tertiary", "Tertiary fill and text colors"
          "warning", "Warning fill and text colors (default)"
          "success", "Success fill and text colors"
          "error", "Error fill and text colors"
          "info", "Info fill and text colors"
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Highlighter"
        Helpers.bodyText
          "Highlight search terms within plain text while preserving the surrounding component's typography."
        Helpers.divider ()
        searchExample ()
        Helpers.divider ()
        multipleExample ()
        Helpers.divider ()
        DataAppearance.highlighter ()
        Helpers.divider ()
        matchingBehavior ()
        Helpers.divider ()
        apiReference ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
