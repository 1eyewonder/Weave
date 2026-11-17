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

  static member private fragments
    (document: Dom.Document, content: string, terms: string list, caseSensitive: bool)
    =
    let fragment = document.CreateDocumentFragment()
    let append node = fragment.AppendChild(node) |> ignore
    let appendText value = append (document.CreateTextNode(value))

    let terms =
      terms
      |> List.filter (String.IsNullOrEmpty >> not)
      |> List.distinct
      |> List.sortByDescending String.length

    if content = "" || List.isEmpty terms then
      appendText content
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
      let mutable position = 0
      let mutable matched = matcher.Exec(content)

      while not (isNull matched) do
        let value = matched.[0]
        let matchEnd = matcher.LastIndex
        let matchStart = matchEnd - value.Length

        if matchStart > position then
          appendText (content.Substring(position, matchStart - position))

        let mark = document.CreateElement("mark")
        mark.ClassList.Add(Css.``weave-highlighter__match``)
        mark.TextContent <- value
        append mark
        position <- matchEnd
        matched <- matcher.Exec(content)

      if position < content.Length then
        appendText (content.Substring(position))

    fragment

  // A selection can start outside the highlighter or run backwards across several marks.
  // Keep its two endpoints separately. An endpoint inside is measured in source-text offsets.
  static member private captureEndpoint(root: Dom.Element, node: Dom.Node, offset: int) =
    if not (root.Contains(node)) then
      fun () -> node, offset
    else
      let range = root.OwnerDocument.CreateRange()
      range.SelectNodeContents(root)
      range.SetEnd(node, offset)
      let position = range.ToString().Length

      fun () ->
        let walker = root.OwnerDocument.CreateTreeWalker(root, Dom.NodeFilter.SHOW_TEXT)
        let mutable textNode = walker.NextNode()
        let mutable remaining = position

        while not (isNull textNode) && remaining > textNode.NodeValue.Length do
          remaining <- remaining - textNode.NodeValue.Length
          textNode <- walker.NextNode()

        if isNull textNode then
          (root :> Dom.Node), 0
        else
          textNode, remaining

  static member private update(root: Dom.Element, content: string, terms: string list, sensitive: bool) =
    let content = if isNull content then "" else content
    let selection = root.OwnerDocument.GetSelection()

    let rangeCount: int = if isNull selection then 0 else selection?rangeCount

    let capture (range: Dom.Range) =
      let start =
        Highlighter.captureEndpoint (root, range.StartContainer, range.StartOffset)

      let finish = Highlighter.captureEndpoint (root, range.EndContainer, range.EndOffset)

      let backward =
        rangeCount = 1
        && not range.Collapsed
        && obj.ReferenceEquals(selection?anchorNode, range.EndContainer)
        && selection?anchorOffset = range.EndOffset

      fun () ->
        let startNode, startOffset = start ()
        let endNode, endOffset = finish ()

        if backward then
          JS.Inline(
            "$0.setBaseAndExtent($1, $2, $3, $4)",
            selection,
            endNode,
            endOffset,
            startNode,
            startOffset
          )
        else
          range.SetStart(startNode, startOffset)
          range.SetEnd(endNode, endOffset)

    let restorations = [|
      if rangeCount > 0 && root.TextContent = content then
        for index in 0 .. rangeCount - 1 do
          let range: Dom.Range = selection?getRangeAt (index)

          if root.Contains(range.StartContainer) || root.Contains(range.EndContainer) then
            capture range
    |]

    root.ReplaceChildren(Highlighter.fragments (root.OwnerDocument, content, terms, sensitive))

    for restore in restorations do
      restore ()

  static member private render
    (content: View<string>, terms: View<string list>, caseSensitive: View<bool>, attrs: Attr list)
    =
    let values =
      View.Map3 (fun text terms sensitive -> text, terms, sensitive) content terms caseSensitive

    span [
      cl Css.``weave-highlighter``
      values
      |> Attr.DynamicCustom(fun el (text, terms, sensitive) ->
        Highlighter.update (el, text, terms, sensitive))
      yield! attrs
    ] []

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
