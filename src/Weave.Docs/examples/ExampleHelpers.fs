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
    Divider.create (attrs = [ Margin.Vertical.small ])

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

        let codeContent =
          pre [] [
            code [
              SurfaceColor.toBackgroundColor SurfaceColor.Background
              Attr.Class "language-fsharp"
              on.afterRender highlightCodeElement
            ] [ text linesOfCode ]
          ]

        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = header,
              content = ExpansionPanelContent.create (codeContent, gutters = View.Const false),
              expanded = codeIsExpanded
            )
          ],
          attrs = [ Margin.Top.extraSmall ]
        )
      ]
    ]
