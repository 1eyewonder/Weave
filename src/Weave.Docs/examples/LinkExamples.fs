namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module LinkExamples =

  let private whenToUseSection () =
    let description =
      div [ Typography.body1 ] [
        text "Both "
        Helpers.inlineCode "Link"
        text " and "
        Helpers.inlineCode "Button"
        text " are clickable elements — but they serve different purposes. Use this to pick the right one."
      ]

    let content =
      Helpers.guidanceColumns
        (Helpers.guidanceCard "Use Link when\u2026" [
          Helpers.guidanceBullet
            "The action navigates to a URL"
            "internal routes and external pages are both navigation, not side effects."
          Helpers.guidanceBullet
            "The element should be an anchor tag"
            "an <a> gives you right-click \u2018Open in new tab\u2019, SEO crawlability, and correct semantics."
          Helpers.guidanceBullet
            "The text is inline within a paragraph"
            "links flow naturally inside body text without breaking the reading rhythm."
          Helpers.guidanceBullet
            "You need underline or color-only styling"
            "Link supports onHover, always, and none underline modes for inline contexts."
        ])
        (Helpers.guidanceCard "Use Button when\u2026" [
          Helpers.guidanceBullet
            "The action triggers a side effect"
            "submitting a form, toggling state, or opening a dialog are actions, not navigation."
          Helpers.guidanceBullet
            "The element should be a button tag"
            "a <button> has correct ARIA role and keyboard activation semantics for actions."
          Helpers.guidanceBullet
            "There is no URL to navigate to"
            "if the action doesn\u2019t have an href, it\u2019s a Button, not a Link."
          Helpers.guidanceBullet
            "The action needs visual weight"
            "filled, outlined, and text variants give buttons prominence that links lack."
        ])

    Helpers.sectionPlain "When to Use" description content

  let private basicExample () =
    let description =
      Helpers.bodyText
        "A basic link navigates to an href. By default the underline appears on hover. Pass any Doc as the inner content — including Typography components — to control font styling."

    let content =
      div [] [
        div [] [
          div [ Typography.body1 ] [
            seq {
              yield text "Visit our "

              yield
                Link.create (
                  span [ Typography.body1 ] [ text "documentation" ],
                  href = "https://1eyewonder.github.io/Weave/"
                )

              yield text " for more details."
            }
            |> Doc.Concat
          ]
        ]

        div [] [
          div [ Typography.body1 ] [
            seq {
              yield text "Visit our "

              yield
                Link.create (
                  span [ Typography.h6 ] [ text "documentation" ],
                  href = "https://1eyewonder.github.io/Weave/"
                )

              yield text " for more details."
            }
            |> Doc.Concat
          ]
        ]
      ]

    let code =
      """open Weave
open WebSharper.UI.Html

div [ Typography.body1 ] [
    seq {
        yield text "Visit our "
        yield
            Link.create(
                span [ Typography.body1 ] [ text "documentation" ],   // brings its own font style
                href = "https://1eyewonder.github.io/Weave/"
            )
        yield text " for more details."
    }
    |> Doc.Concat
]

div [ Typography.body1 ] [
    seq {
        yield text "Visit our "
        yield
            Link.create(
                span [ Typography.h6 ] [ text "documentation" ],   // brings its own font style
                href = "https://1eyewonder.github.io/Weave/"
            )
        yield text " for more details."
    }
    |> Doc.Concat
]
"""

    Helpers.codeSampleSection "Basic Link" description content code

  let private underlineExamples () =
    let description =
      Helpers.bodyText
        "Control when the underline is shown: OnHover (default), Always, or None. The underline is applied only to the text — icon adornments are never underlined."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              Link.create (
                div [ Typography.h3 ] [ text "OnHover (default)" ],
                href = "https://1eyewonder.github.io/Weave/",
                attrs = [ Link.Underline.onHover ]
              )
            ]
          )
          GridItem.create (
            div [] [
              Link.create (
                div [ Typography.h3 ] [ text "Always" ],
                href = "https://1eyewonder.github.io/Weave/",
                attrs = [ Link.Underline.always ]
              )
            ]
          )
          GridItem.create (
            div [] [
              Link.create (
                div [ Typography.h3 ] [ text "None" ],
                href = "https://1eyewonder.github.io/Weave/",
                attrs = [ Link.Underline.none ]
              )
            ]
          )
        ]
      )

    let code =
      """open Weave


Link.create(
  div [ Typography.h3 ] [ text "OnHover (default)" ],
  href = "https://1eyewonder.github.io/Weave/",
  attrs = [ Link.Underline.onHover ]
)

Link.create(
  div [ Typography.h3 ] [ text "Always" ],
  href = "https://1eyewonder.github.io/Weave/",
  attrs = [ Link.Underline.always ]
)

Link.create(
  div [ Typography.h3 ] [ text "None" ],
  href = "https://1eyewonder.github.io/Weave/",
  attrs = [ Link.Underline.none ]
)"""

    Helpers.codeSampleSection "Underline" description content code

  let private colorExamples () =
    let colors = [
      "Primary", Link.Color.primary
      "Secondary", Link.Color.secondary
      "Tertiary", Link.Color.tertiary
      "Error", Link.Color.error
      "Warning", Link.Color.warning
      "Success", Link.Color.success
      "Info", Link.Color.info
    ]

    let description =
      Helpers.bodyText "Links can use any brand color from the theme palette."

    let content =
      Grid.create (
        colors
        |> List.map (fun (label, colorAttr) ->
          GridItem.create (
            Link.create (
              text label,
              href = "https://1eyewonder.github.io/Weave/",
              attrs = [ colorAttr; Link.Underline.always ]
            )
          ))
      )

    let code =
      """open Weave

Link.create(text "Primary", href = "#", attrs = [ Link.Color.primary; Link.Underline.always ])
Link.create(text "Secondary", href = "#", attrs = [ Link.Color.secondary; Link.Underline.always ])
Link.create(text "Tertiary", href = "#", attrs = [ Link.Color.tertiary; Link.Underline.always ])
Link.create(text "Error", href = "#", attrs = [ Link.Color.error; Link.Underline.always ])
Link.create(text "Warning", href = "#", attrs = [ Link.Color.warning; Link.Underline.always ])
Link.create(text "Success", href = "#", attrs = [ Link.Color.success; Link.Underline.always ])
Link.create(text "Info", href = "#", attrs = [ Link.Color.info; Link.Underline.always ])"""

    Helpers.codeSampleSection "Colors" description content code

  let private iconAdornmentExamples () =
    let description =
      Helpers.bodyText
        "Links can have a start icon, an end icon, or both. Icons are never underlined regardless of the underline setting."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              div [ Typography.caption ] [ text "Start icon" ]
              Link.create (
                text "Open in new tab",
                href = "https://1eyewonder.github.io/Weave/",
                startIcon = Icon.create (Icon.UiActions UiActions.OpenInNew),
                attrs = [ Link.Underline.always ]
              )
            ]
          )
          GridItem.create (
            div [] [
              div [ Typography.caption ] [ text "End icon" ]
              Link.create (
                text "Download",
                href = "https://1eyewonder.github.io/Weave/",
                endIcon = Icon.create (Icon.UiActions UiActions.Download),
                attrs = [ Link.Underline.always ]
              )
            ]
          )
          GridItem.create (
            div [] [
              div [ Typography.caption ] [ text "Both icons" ]
              Link.create (
                text "Send email",
                href = "https://1eyewonder.github.io/Weave/",
                startIcon = Icon.create (Icon.Communicate Communicate.Mail),
                endIcon = Icon.create (Icon.UiActions UiActions.ArrowForward),
                attrs = [ Link.Underline.always ]
              )
            ]
          )
        ]
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


// Start icon only
Link.create(
    text "Open in new tab",
    href = "#",
    startIcon = Icon.create(Icon.UiActions UiActions.OpenInNew),
    attrs = [ Link.Underline.always ]
)

// End icon only
Link.create(
    text "Download",
    href = "#",
    endIcon = Icon.create(Icon.UiActions UiActions.Download),
    attrs = [ Link.Underline.always ]
)"""

    Helpers.codeSampleSection "Icon Adornments" description content code

  let private iconOnlyExample () =
    let description =
      Helpers.bodyText "Use IconLink.create when the link should contain only an icon with no text label."

    let content =
      Grid.create (
        [
          GridItem.create (
            IconLink.create (
              Icon.create (Icon.UiActions UiActions.Home),
              href = "https://1eyewonder.github.io/Weave/"
            )
          )
          GridItem.create (
            IconLink.create (
              Icon.create (Icon.UiActions UiActions.Settings),
              href = "https://1eyewonder.github.io/Weave/",
              attrs = [ Link.Color.secondary ]
            )
          )
          GridItem.create (
            IconLink.create (
              Icon.create (Icon.UiActions UiActions.Delete),
              href = "https://1eyewonder.github.io/Weave/",
              attrs = [ Link.Color.error ]
            )
          )
        ]
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


// Icon-only link (no text)
IconLink.create(
    Icon.create(Icon.UiActions UiActions.Home),
    href = "#"
)

// With color
IconLink.create(
    Icon.create(Icon.UiActions UiActions.Delete),
    href = "#",
    attrs = [ Link.Color.error ]
)"""

    Helpers.codeSampleSection "Icon-Only" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText "A disabled link prevents navigation and suppresses the onClick callback."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              div [ Typography.caption ] [ text "Enabled" ]
              Link.create (
                text "Active link",
                href = "https://1eyewonder.github.io/Weave/",
                enabled = View.Const true,
                attrs = [ Link.Underline.always ]
              )
            ]
          )
          GridItem.create (
            div [] [
              div [ Typography.caption ] [ text "Disabled" ]
              Link.create (
                text "Disabled link",
                href = "https://1eyewonder.github.io/Weave/",
                enabled = View.Const false,
                attrs = [ Link.Underline.always ]
              )
            ]
          )
        ]
      )

    let code =
      """open Weave

open WebSharper.UI

// Enabled (default)
Link.create(
    text "Active link",
    href = "#",
    enabled = View.Const true,
    attrs = [ Link.Underline.always ]
)

// Disabled — no navigation, onClick not fired
Link.create(
    text "Disabled link",
    href = "#",
    enabled = View.Const false,
    attrs = [ Link.Underline.always ]
)"""

    Helpers.codeSampleSection "Disabled" description content code

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
        whenToUseSection ()
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        underlineExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        iconAdornmentExamples ()
        Helpers.divider ()
        iconOnlyExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
