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

  /// Preview-only captions and alignment for density comparisons.
  let densityComparison (examples: (string * Doc) list) =
    let cells =
      examples
      |> List.map (fun (label, content) ->
        GridItem.create (
          div [
            Attr.Style "inline-size" "100%"
            Attr.Style "min-inline-size" "0"
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.center
            Gap.All.g2
          ] [ p [ Typography.subtitle2 ] [ text label ]; content ],
          attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
        ))

    Grid.create (cells, attrs = [ Grid.Spacing.large; AlignItems.start ])

  let divider () =
    Divider.create (attrs = [ Margin.Vertical.small; Attr.Class "docs-section-divider--alt" ])

  /// Generates a URL-friendly slug from a title string
  let private slugify (title: string) =
    title.ToLower().Replace(" ", "-").Replace("'", "").Replace("`", "").Replace("(", "").Replace(")", "")

  [<Inline "(window.location.hash.split('/')[0] || '#home') + '/' + $slug">]
  let private sectionHref (slug: string) = X<string>

  /// Section header with an anchor link for deep-linking
  let sectionHeader title =
    let slug = slugify title

    div [
      attr.id slug
      Margin.Bottom.extraSmall
      Attr.Class "section-header docs-section-heading"
      Flex.Flex.allSizes
      AlignItems.center
      Attr.Style "gap" "8px"
    ] [
      h2 [ Typography.h5; Attr.Style "margin" "0" ] [ textView (View.Const title) ]
      a [
        attr.href (sectionHref slug)
        Attr.Create "aria-label" (sprintf "Link to %s" title)
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
      Attr.Class "section-header docs-page-title"
      Flex.Flex.allSizes
      AlignItems.center
      Attr.Style "gap" "12px"
    ] [
      h1 [ Typography.h3; Attr.Style "margin" "0" ] [ text title ]
      a [
        attr.href (sectionHref slug)
        Attr.Create "aria-label" (sprintf "Link to %s" title)
        Attr.Class "anchor-link"
        Attr.Style "text-decoration" "none"
        Attr.Style "color" "var(--palette-primary)"
        Attr.Style "font-size" "1.25em"
        Attr.Style "font-weight" "bold"
        Attr.Style "line-height" "1"
      ] [ text "#" ]
    ]

  let section title description content =
    div [ Margin.Bottom.small; Attr.Class "docs-section" ] [
      sectionHeader title

      div [ Margin.Bottom.extraSmall; Attr.Class "docs-description" ] [ description ]

      div [ Attr.Class "docs-surface" ] [ content ]
    ]

  /// Section with header + description + content but no surface background wrapper.
  /// Use for content that provides its own visual containers (e.g. guidance cards, grids).
  let sectionPlain title description content =
    div [ Margin.Bottom.small; Attr.Class "docs-section" ] [
      sectionHeader title
      div [ Margin.Bottom.extraSmall; Attr.Class "docs-description" ] [ description ]
      content
    ]

  let textSection title (children: Doc list) =
    div [ Margin.Bottom.small; Attr.Class "docs-section" ] [
      sectionHeader title

      div [ Attr.Class "docs-surface docs-text-surface" ] [ yield! children ]
    ]

  [<Inline "window.highlightCodeElement($0)">]
  let highlightCodeElement (el: Dom.Element) = X<unit>

  [<Inline "navigator.clipboard.writeText($text).then(function(){ $onSuccess(); }).catch(function(){ $onError(); })">]
  let private copyToClipboard (text: string) (onSuccess: unit -> unit) (onError: unit -> unit) = X<unit>

  let copyButton (content: string) =
    let status = Var.Create "Copy"

    button [
      Attr.Class "docs-copy-button"
      attr.``type`` "button"
      Attr.Create "aria-live" "polite"
      on.click (fun _ _ ->
        copyToClipboard content (fun () -> Var.Set status "Copied") (fun () -> Var.Set status "Copy failed"))
    ] [ textView status.View ]

  /// Inline code badge — renders text in a monospace pill (e.g. `ChipSet`)
  let inlineCode (str: string) =
    code [ Attr.Class "docs-inline-code" ] [ text str ]

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

  type ApiParam = ApiReference.Parameter

  let apiParam name typ def desc : ApiParam = {
    Name = name
    Type = typ
    Default = def
    Description = desc
  }

  let apiTable title rows =
    ApiReference.Parameters(title, rows, None)

  let apiMember title returnType rows =
    ApiReference.Parameters(title, rows, Some returnType)

  let styleModuleTable title values = ApiReference.Options(title, values)

  let returnTypeNote noteText = ApiReference.Note noteText

  let private apiType signature =
    code [
      Attr.Class "docs-api-type language-fsharp-type"
      on.afterRender highlightCodeElement
    ] [ text signature ]

  let private apiNotes notes =
    notes
    |> List.map (fun note -> p [ Typography.body2; Attr.Class "docs-return-type" ] [ text note ])
    |> Doc.Concat

  let private apiGroup (terms: View<string list>) (group: ApiReference.Group) =
    let shown = terms |> View.Map(fun query -> ApiReference.filterRows query group)

    let visibility =
      terms
      |> View.Map(fun query -> if ApiReference.matches query group then "" else "none")

    if System.String.IsNullOrEmpty group.Title then
      div [ Attr.DynamicStyle "display" visibility ] [ apiNotes group.IntroNotes; apiNotes group.Notes ]
    else
      let column key label width render =
        TableColumn.create (
          key,
          text label,
          render,
          attrs = [ Attr.Create "data-label" label; Attr.Style "width" width ]
        )

      let nameColumn label width =
        column "name" label width (fun (row: ApiReference.Row) ->
          code [ Attr.Class "docs-api-name" ] [ text row.Name ])

      let descriptionColumn width =
        column "description" "Description" width (fun (row: ApiReference.Row) -> text row.Description)

      let columns =
        if group.IsParameterTable then
          [
            nameColumn "Parameter" "22%"
            column "type" "Type" "22%" (fun row -> apiType row.Type)
            column "default" "Default" "18%" (fun row ->
              if System.String.IsNullOrEmpty row.Default then
                span [ Attr.Class "docs-api-empty-default" ] [ text "\u2014" ]
              else
                code [ Attr.Class "docs-api-default" ] [ text row.Default ])
            descriptionColumn "38%"
          ]
        else
          [ nameColumn "Value" "28%"; descriptionColumn "72%" ]

      let caption =
        group.Title
        + (if group.IsParameterTable then
             " API reference"
           else
             " style reference")

      let emptyMessage =
        if List.isEmpty group.Rows then
          if group.IsParameterTable then
            "No parameters."
          else
            "No options."
        else
          "No matching entries."

      div [ Attr.Class "docs-api-group"; Attr.DynamicStyle "display" visibility ] [
        apiNotes group.IntroNotes
        div [ Attr.Class "docs-api-heading" ] [
          h3 [ Typography.h6 ] [ text group.Title ]
          match group.ReturnType with
          | Some returnType -> span [ Attr.Class "docs-api-returns" ] [ text "Returns "; apiType returnType ]
          | None -> ()
        ]
        Table.create (
          columns,
          shown,
          (fun row -> row.Key),
          rowHeader = "name",
          caption = text caption,
          emptyContent = text emptyMessage,
          attrs = [
            Attr.Class "docs-api-table"
            Table.hiddenCaption
            Table.hoverable
            if not group.IsParameterTable then
              Attr.Class "docs-api-table--options"
              Table.Density.compact
          ]
        )
        apiNotes group.Notes
      ]

  let apiSection (description: Doc) (definitions: ApiReference.Definition list) : Doc =
    let groups = ApiReference.group definitions
    let query = Var.Create ""
    let terms = query.View |> View.Map ApiReference.normalise
    let total = groups |> List.sumBy ApiReference.totalCount
    let resultsId = WeaveId.create "docs-api-results"
    let countId = resultsId + "-count"
    let inputElement = ref (None: Dom.Element option)

    let clear () =
      Var.Set query ""
      inputElement.Value |> Option.iter (fun element -> element?focus ())

    let countText =
      terms
      |> View.Map(fun current ->
        let count = groups |> List.sumBy (ApiReference.visibleCount current)

        if List.isEmpty current then
          sprintf "%d API entries" total
        else
          sprintf "%d of %d entries" count total)

    let emptyVisibility =
      terms
      |> View.Map(fun current ->
        if groups |> List.exists (ApiReference.matches current) then
          "none"
        else
          "")

    div [
      Margin.Bottom.small
      Attr.Class "docs-section docs-api-reference"
      Attr.DynamicClassPred "docs-api-reference--filtering" (terms |> View.Map(List.isEmpty >> not))
    ] [
      sectionHeader "API"
      div [ Margin.Bottom.extraSmall; Attr.Class "docs-description" ] [ description ]
      if total > 0 then
        div [ Attr.Class "docs-api-toolbar" ] [
          div [ Attr.Class "docs-api-filter" ] [
            TextField.singleLine (
              query,
              labelText = View.Const "Filter API",
              placeholder = View.Const "Name, type, default, or option",
              inputAttrs = [
                Attr.Create "aria-controls" resultsId
                Attr.Create "aria-describedby" countId
                on.afterRender (fun element -> inputElement.Value <- Some element)
                on.keyDown (fun _ ev ->
                  if As<string>(ev?key) = "Escape" && query.Value <> "" then
                    ev.PreventDefault()
                    clear ())
              ],
              attrs = [ TextField.Variant.outlined; TextField.Label.above; TextField.Width.full ]
            )
          ]
          p [ attr.id countId; Attr.Class "docs-api-count"; Attr.Create "role" "status" ] [
            textView countText
          ]
          Button.create (
            text "Clear filter",
            onClick = clear,
            enabled = (query.View |> View.Map(fun value -> value <> "")),
            attrs = [ Button.Variant.outlined ]
          )
        ]
      div [ attr.id resultsId; Attr.Class "docs-surface docs-api-surface" ] [
        p [
          Attr.Class "docs-api-no-results"
          Attr.DynamicStyle "display" emptyVisibility
        ] [
          text "No API entries match this filter. Try another name, type, default, or option."
        ]
        for group in groups do
          apiGroup terms group
      ]
    ]

  let private codeSampleSectionWithLanguage
    language
    title
    description
    (content: Doc option)
    (linesOfCode: View<string>)
    =
    let languageLabel =
      match language with
      | "css" -> "CSS"
      | "html" -> "HTML"
      | _ -> "F#"

    div [ Margin.Bottom.small; Attr.Class "docs-section" ] [
      sectionHeader title

      div [ Margin.Bottom.extraSmall; Attr.Class "docs-description" ] [ description ]

      div [
        SurfaceColor.BackgroundColor.surface
        Attr.Class "docs-example"
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        BorderRadius.All.small
      ] [
        match content with
        | Some preview -> div [ Attr.Class "docs-example__preview" ] [ preview ]
        | None -> ()

        let codeIsExpanded = Var.Create false

        let icon =
          codeIsExpanded.View
          |> Doc.BindView(fun expanded ->
            let attrs = [ AlignItems.center; Attr.Create "aria-hidden" "true" ]

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
          linesOfCode
          |> Doc.BindView(fun current ->
            div [ Attr.Class "docs-copy-code-wrapper" ] [
              span [ Attr.Class "docs-code-language" ] [ text languageLabel ]
              copyButton current

              pre [
                attr.tabindex "0"
                Attr.Create "aria-label" (title + " " + languageLabel + " source code")
              ] [
                code [
                  Attr.Class("language-" + language)
                  on.afterRender highlightCodeElement
                  Typography.Family.mono
                ] [ text current ]
              ]
            ])

        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = header,
              content = ExpansionPanelContent.create (codeContent),
              expanded = codeIsExpanded
            )
          ],
          attrs = [ Attr.Class "docs-example__code" ]
        )
      ]
    ]

  /// A configurable component preview with source for the selected settings.
  let appearanceSectionNamed
    title
    aliases
    description
    controls
    (accessory: Doc option)
    preview
    reset
    (source: View<string>)
    =
    let expanded = Var.Create false

    let header =
      ExpansionPanelHeader.create (
        content =
          div [ Typography.subtitle2 ] [
            textView (
              expanded.View
              |> View.Map(fun value -> if value then "Hide Code" else "Show Code")
            )
          ],
        expanded = expanded,
        icon =
          (expanded.View
           |> Doc.BindView(fun value ->
             Icon.create (
               Icon.UiActions(if value then UiActions.CollapseAll else UiActions.ExpandAll),
               attrs = [ Attr.Create "aria-hidden" "true" ]
             ))),
        attrs = [ ExpansionPanel.Color.primary ]
      )

    let codeContent =
      source
      |> Doc.BindView(fun current ->
        div [ Attr.Class "docs-copy-code-wrapper" ] [
          span [ Attr.Class "docs-code-language" ] [ text "F#" ]
          copyButton current
          pre [
            attr.tabindex "0"
            Attr.Create "aria-label" "Selected appearance F# source code"
          ] [
            code [
              Attr.Class "language-fsharp"
              on.afterRender highlightCodeElement
              Typography.Family.mono
            ] [ text current ]
          ]
        ])

    let heading =
      List.foldBack (fun id content -> div [ attr.id id ] [ content ]) aliases (sectionHeader title)

    div [ Margin.Bottom.small; Attr.Class "docs-section" ] [
      heading
      div [ Margin.Bottom.extraSmall; Attr.Class "docs-description" ] [ description ]
      div [
        SurfaceColor.BackgroundColor.surface
        Attr.Class "docs-example"
        BorderRadius.All.small
      ] [
        div [ Attr.Class "docs-example__preview docs-appearance" ] [
          div [ Attr.Class "docs-appearance__preview" ] [ preview ]
          div [ Attr.Class "docs-appearance__settings" ] [
            controls
            div [ Attr.Class "docs-appearance__actions" ] [
              match accessory with
              | Some content -> yield content
              | None -> ()
              yield
                Button.primary (
                  text "Reset",
                  onClick = reset,
                  attrs = [ Button.Variant.outlined; Attr.Class "docs-appearance__reset" ]
                )
            ]
          ]
        ]
        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = header,
              content = ExpansionPanelContent.create codeContent,
              expanded = expanded
            )
          ],
          attrs = [ Attr.Class "docs-example__code" ]
        )
      ]
    ]

  let appearanceSection aliases description controls accessory preview reset source =
    appearanceSectionNamed "Appearance" aliases description controls accessory preview reset source

  /// A live example paired with source in the selected language.
  let codeSampleSectionFor language title description content linesOfCode =
    codeSampleSectionWithLanguage language title description (Some content) (View.Const linesOfCode)

  let codeSampleSection title description content linesOfCode =
    codeSampleSectionFor "fsharp" title description content linesOfCode

  /// Source-only guidance avoids an empty preview above the code disclosure.
  let codeSection language title description linesOfCode =
    codeSampleSectionWithLanguage language title description None (View.Const linesOfCode)

  /// A live example whose copied source follows its state without resetting the disclosure.
  let codeSampleSectionView title description content source =
    codeSampleSectionWithLanguage "fsharp" title description (Some content) source
