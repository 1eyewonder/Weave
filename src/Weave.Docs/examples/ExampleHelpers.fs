namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.JavaScript
open Weave

open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module Helpers =

  let bodyText (str: string) = div [ Typography.body1 ] [ text str ]

  let divider () =
    Divider.create (attrs = [ Margin.Vertical.small; Attr.Class "docs-section-divider--alt" ])

  /// Generates a URL-friendly slug from a title string
  let private slugify (title: string) =
    title.ToLower().Replace(" ", "-").Replace("'", "").Replace("`", "").Replace("(", "").Replace(")", "")

  /// Section header with an anchor link for deep-linking
  let sectionHeader title =
    let slug = slugify title

    div [
      attr.id slug
      Margin.Bottom.extraSmall
      Attr.Class "section-header"
      Flex.Flex.allSizes
      AlignItems.center
      Attr.Style "gap" "8px"
    ] [
      div [ Typography.h4 ] [ textView (View.Const title) ]
      a [
        attr.href (sprintf "#%s" slug)
        Attr.Class "anchor-link"
        Attr.Style "text-decoration" "none"
        Attr.Style "color" "var(--palette-primary)"
        Attr.Style "font-size" "1.1em"
        Attr.Style "font-weight" "bold"
        Attr.Style "line-height" "1"
      ] [ text "#" ]
    ]

  /// Page-level title (H1) with an anchor link
  let pageTitle title =
    let slug = slugify title

    div [
      attr.id slug
      Margin.Bottom.extraSmall
      Attr.Class "section-header"
      Flex.Flex.allSizes
      AlignItems.center
      Attr.Style "gap" "12px"
    ] [
      div [ Typography.h1 ] [ text title ]
      a [
        attr.href (sprintf "#%s" slug)
        Attr.Class "anchor-link"
        Attr.Style "text-decoration" "none"
        Attr.Style "color" "var(--palette-primary)"
        Attr.Style "font-size" "1.25em"
        Attr.Style "font-weight" "bold"
        Attr.Style "line-height" "1"
      ] [ text "#" ]
    ]

  let section title description content =
    div [ Margin.Bottom.small ] [
      sectionHeader title

      div [ Margin.Bottom.extraSmall ] [ description ]

      div [
        Padding.All.small
        SurfaceColor.toBackgroundColor SurfaceColor.Surface
        BorderRadius.All.small
      ] [ content ]
    ]

  /// Section with header + description + content but no surface background wrapper.
  /// Use for content that provides its own visual containers (e.g. guidance cards, grids).
  let sectionPlain title description content =
    div [ Margin.Bottom.small ] [
      sectionHeader title
      div [ Margin.Bottom.extraSmall ] [ description ]
      content
    ]

  let textSection title (children: Doc list) =
    div [ Margin.Bottom.small ] [
      sectionHeader title

      div [
        Padding.All.small
        SurfaceColor.toBackgroundColor SurfaceColor.Surface
        BorderRadius.All.small
      ] [ yield! children ]
    ]

  [<Inline "window.highlightCodeElement($0)">]
  let highlightCodeElement (el: Dom.Element) = X<unit>

  [<Inline "navigator.clipboard.writeText($0)">]
  let private copyToClipboard (text: string) = X<Promise<unit>>

  [<Inline "setTimeout($0, $1)">]
  let private delay (callback: unit -> unit) (ms: int) = X<unit>

  /// Inline code badge — renders text in a monospace pill (e.g. `ChipSet`)
  let inlineCode (str: string) =
    span [ Attr.Class "docs-inline-code" ] [ text str ]

  /// A single bullet with a colored dot, bold lead text, and a description.
  let guidanceBullet (lead: string) (desc: string) =
    div [ Attr.Class "docs-guidance-bullet" ] [
      span [ Attr.Class "docs-guidance-bullet__dot" ] []
      div [] [
        span [ Attr.Class "docs-guidance-bullet__lead" ] [ text lead ]
        span [ Attr.Class "docs-guidance-bullet__desc" ] [ text (" — " + desc) ]
      ]
    ]

  /// A guidance card with a title and a list of bullets. Pair two of these
  /// inside a Grid for side-by-side comparison layouts.
  let guidanceCard (title: string) (bullets: Doc list) =
    div [ Attr.Class "docs-guidance-card" ] [
      div [ Attr.Class "docs-guidance-card__title" ] [ text title ]
      div [ Attr.Class "docs-guidance-card__list" ] [ yield! bullets ]
    ]

  /// Two guidance cards rendered side-by-side in a responsive grid row.
  let guidanceColumns (left: Doc) (right: Doc) =
    Grid.create (
      [
        GridItem.create (left, attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ])
        GridItem.create (right, attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ])
      ],
      attrs = [ AlignItems.stretch ]
    )

  let codeSampleSection title description (content: Doc) (linesOfCode: string) =
    div [ Margin.Bottom.small ] [
      sectionHeader title

      div [ Margin.Bottom.extraSmall ] [ description ]

      div [
        SurfaceColor.toBackgroundColor SurfaceColor.Surface
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Padding.All.small
        BorderRadius.All.small
      ] [
        content

        let codeIsExpanded = Var.Create false
        let justCopied = Var.Create false

        let icon =
          codeIsExpanded.View
          |> Doc.BindView(fun expanded ->
            let attrs = [ AlignItems.center ]

            if expanded then
              Icon.create (Icon.UiActions UiActions.CollapseAll, attrs = attrs)
            else
              Icon.create (Icon.UiActions UiActions.ExpandAll, attrs = attrs))

        let headerText =
          codeIsExpanded.View
          |> View.MapCached(fun expanded -> if expanded then "Hide Code" else "Show Code")

        let header =
          ExpansionPanelHeader.create (
            content = div [ Typography.subtitle2 ] [ textView headerText ],
            expanded = codeIsExpanded,
            icon = icon,
            attrs = [ ExpansionPanel.Color.primary ]
          )

        let copyIcon =
          justCopied.View
          |> Doc.BindView(fun copied ->
            if copied then
              Icon.create (Icon.UiActions UiActions.Check)
            else
              Icon.create (Icon.Text Text.ContentCopy))

        let copy () =
          copyToClipboard linesOfCode |> ignore
          Var.Set justCopied true

        let revertOnCollapse =
          codeIsExpanded.View
          |> Doc.sink (fun expanded ->
            if not expanded then
              Var.Set justCopied false)

        let copyButton =
          div [ Attr.Class "docs-copy-code" ] [
            IconButton.create (
              copyIcon,
              onClick = copy,
              attrs = [ Button.Variant.text; Button.Color.primary ]
            )
          ]

        let codeContent =
          div [ Attr.Class "docs-copy-code-wrapper" ] [
            copyButton
            revertOnCollapse

            pre [] [
              code [
                Attr.Class "language-fsharp"
                on.afterRender highlightCodeElement
                Typography.Family.mono
              ] [ text linesOfCode ]
            ]
          ]

        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = header,
              content = ExpansionPanelContent.create (codeContent),
              expanded = codeIsExpanded
            )
          ],
          attrs = [ Margin.Top.extraSmall ]
        )
      ]
    ]
