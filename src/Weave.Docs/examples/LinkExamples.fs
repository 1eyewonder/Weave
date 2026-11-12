namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module LinkExamples =

  let private comparisonGrid columns items =
    let cells =
      items
      |> List.map (fun item ->
        GridItem.create (
          div [ Flex.Flex.allSizes; JustifyContent.center ] [ item ],
          attrs = [
            GridItem.Span.twelve
            GridItem.Span.Small.six
            (if columns = 4 then GridItem.Span.Medium.three
             elif columns = 3 then GridItem.Span.Medium.four
             else GridItem.Span.Medium.six)
          ]
        ))

    Grid.create (cells, attrs = [ Grid.Spacing.large ])

  let private basicExample () =
    let description =
      Helpers.bodyText
        "A Link navigates to its href. Its content inherits typography, so inline text can use the surrounding body style or an explicit heading style."

    let content =
      let paragraph style =
        div [ Typography.body1 ] [
          text "Visit our "
          Link.create (span [ style ] [ text "documentation" ], href = "https://1eyewonder.github.io/Weave/")
          text " for more details."
        ]

      div [] [ paragraph Typography.body1; paragraph Typography.h6 ]

    let code =
      """let paragraph style =
  div [ Typography.body1 ] [
    text "Visit our "
    Link.create (
      span [ style ] [ text "documentation" ],
      href = "https://1eyewonder.github.io/Weave/"
    )
    text " for more details."
  ]

paragraph Typography.body1

paragraph Typography.h6"""

    Helpers.codeSampleSection "Basic Link" description content code

  let private iconOnlyExample () =
    let description =
      Helpers.bodyText
        "Use IconLink for an icon-only destination. Give each link an accessible name that identifies where it goes."

    let content =
      comparisonGrid 3 [
        for color in [ Attr.Empty; Link.Color.secondary; Link.Color.error ] do
          IconLink.create (
            Icon.create (Icon.UiActions UiActions.Home),
            href = "https://1eyewonder.github.io/Weave/",
            attrs = [ color; Attr.Create "aria-label" "Documentation home" ]
          )
      ]

    let code =
      """open Weave.Icons.MaterialSymbols

[
  for color in [ Attr.Empty; Link.Color.secondary; Link.Color.error ] do
    IconLink.create (
      Icon.create (Icon.UiActions UiActions.Home),
      href = "https://1eyewonder.github.io/Weave/",
      attrs = [ color; Attr.Create "aria-label" "Documentation home" ]
    )
]"""

    Helpers.codeSampleSection "Icon-Only" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Link and IconLink.") [
      Helpers.apiTable "Link.create" [
        Helpers.apiParam "innerContents" "Doc" "" "Text or content displayed as the link label"
        Helpers.apiParam "?href" "string" "\"#\"" "URL to navigate to"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the link is interactive"
        Helpers.apiParam "?onClick" "unit -> unit" "" "Additional click handler fired alongside navigation"
        Helpers.apiParam "?startIcon" "Doc" "" "Icon rendered before the link text"
        Helpers.apiParam "?endIcon" "Doc" "" "Icon rendered after the link text"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (underline, color, etc.)"
      ]

      Helpers.apiTable "IconLink.create" [
        Helpers.apiParam "icon" "Doc" "" "Icon content (no text label, no underline)"
        Helpers.apiParam "?href" "string" "\"#\"" "URL to navigate to"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the link is interactive"
        Helpers.apiParam "?onClick" "unit -> unit" "" "Additional click handler fired alongside navigation"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (color, etc.)"
      ]

      Helpers.styleModuleTable "Link.Underline" [
        ("onHover", "Underline appears only on hover")
        ("always", "Underline is always visible")
        ("none", "No underline decoration")
      ]

      Helpers.styleModuleTable "Link.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Link"
        Helpers.bodyText
          "A component that wraps an anchor element with configurable underline behaviour, icon adornments, and reactive enable/disable state."
        Helpers.divider ()
        span [ attr.id "when-to-use" ] []
        basicExample ()
        Helpers.divider ()
        ReferenceAppearance.link ()
        Helpers.divider ()
        iconOnlyExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
