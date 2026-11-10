namespace Weave

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module Highlighter =

  module Color =
    let primary = cl Css.``weave-highlighter--primary``
    let secondary = cl Css.``weave-highlighter--secondary``
    let tertiary = cl Css.``weave-highlighter--tertiary``
    let error = cl Css.``weave-highlighter--error``
    let warning = cl Css.``weave-highlighter--warning``
    let success = cl Css.``weave-highlighter--success``
    let info = cl Css.``weave-highlighter--info``

[<JavaScript; RequireQualifiedAccess>]
type Highlighter =

  static member private fragments(content: string, terms: string list, caseSensitive: bool) =
    let content = if isNull content then "" else content

    let terms =
      terms
      |> List.filter (String.IsNullOrEmpty >> not)
      |> List.distinct
      |> List.sortByDescending String.length

    if content = "" || List.isEmpty terms then
      text content
    else
      // Escape each term: punctuation is literal, never a caller-supplied regex.
      // Longest first makes matches at the same position independent of term order.
      let punctuation = RegExp(@"[.*+?^${}()|[\]\\]", "g")

      let pattern =
        terms
        |> List.map (fun term -> punctuation.Replace(term, @"\$&"))
        |> String.concat "|"

      // Unicode mode enables simple case folding for non-ASCII and astral letters.
      // lastIndex and string slices still use UTF-16 offsets, preserving the source text.
      let matcher = RegExp(pattern, if caseSensitive then "gu" else "giu")
      let fragments = ResizeArray<Doc>()
      let mutable position = 0
      let mutable matched = matcher.Exec(content)

      while not (isNull matched) do
        let value = matched.[0]
        let matchEnd = matcher.LastIndex
        let matchStart = matchEnd - value.Length

        if matchStart > position then
          fragments.Add(text (content.Substring(position, matchStart - position)))

        fragments.Add(mark [ cl Css.``weave-highlighter__match`` ] [ text value ])
        position <- matchEnd
        matched <- matcher.Exec(content)

      if position < content.Length then
        fragments.Add(text (content.Substring(position)))

      Doc.Concat fragments

  static member private render
    (content: View<string>, terms: View<string list>, caseSensitive: View<bool>, attrs: Attr list)
    =
    span [ cl Css.``weave-highlighter``; yield! attrs ] [
      View.Map3
        (fun content terms sensitive -> Highlighter.fragments (content, terms, sensitive))
        content
        terms
        caseSensitive
      |> Doc.EmbedView
    ]

  /// Highlights literal substrings using locale-independent Unicode simple case folding by default.
  /// Accents, normalization forms, punctuation, and whitespace remain distinct; terms are not trimmed.
  /// Matches may occur inside words or grapheme clusters (such as combined emoji).
  /// Text is always escaped. Attributes apply to the inline wrapper; Color attributes style its marks.
  static member create
    (text: View<string>, highlightedText: View<string>, ?caseSensitive: View<bool>, ?attrs: Attr list)
    =
    Highlighter.render (
      text,
      highlightedText |> View.Map(fun term -> [ term ]),
      defaultArg caseSensitive (View.Const false),
      defaultArg attrs []
    )

  /// Highlights multiple literal substrings with the same matching rules as create; terms are not split or trimmed.
  /// Null and empty terms are ignored. Matches proceed left to right without overlapping;
  /// at a shared start, the longest term wins. The original text and casing are preserved.
  static member createMany
    (text: View<string>, highlightedTexts: View<string list>, ?caseSensitive: View<bool>, ?attrs: Attr list)
    =
    Highlighter.render (
      text,
      highlightedTexts,
      defaultArg caseSensitive (View.Const false),
      defaultArg attrs []
    )
