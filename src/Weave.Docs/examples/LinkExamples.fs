namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module LinkExamples =

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
        ],
        spacing = Grid.GutterSpacing.create 2
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
          )),
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


let colors = [
    "Primary", Link.Color.primary
    "Secondary", Link.Color.secondary
    // ...
]

colors |> List.map (fun (label, colorAttr) ->
    Link.create(
        text label,
        href = "#",
        attrs = [
            colorAttr
            Link.Underline.always
        ]
    )
)"""

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
        ],
        spacing = Grid.GutterSpacing.create 2
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
        ],
        spacing = Grid.GutterSpacing.create 2
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
        ],
        spacing = Grid.GutterSpacing.create 2
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

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Link"
        Helpers.bodyText
          "A component that wraps an anchor element with configurable underline behaviour, icon adornments, and reactive enable/disable state."
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
      ],
      attrs = [ Container.MaxWidth.large ]
    )
