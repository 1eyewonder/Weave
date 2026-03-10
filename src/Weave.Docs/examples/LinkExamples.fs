namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.CssHelpers

[<JavaScript>]
module LinkExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText
        "A basic link navigates to an href. By default the underline appears on hover. Pass any Doc as the inner content — including Typography components — to control font styling."

    let content =
      div [] [
        div [] [
          Body1.Div(
            seq {
              yield text "Visit our "
              yield Link.Create(Body1.Span("documentation"), href = "https://1eyewonder.github.io/Weave/")
              yield text " for more details."
            }
            |> Doc.Concat
          )
        ]

        div [] [
          Body1.Div(
            seq {
              yield text "Visit our "
              yield Link.Create(H6.Span("documentation"), href = "https://1eyewonder.github.io/Weave/")
              yield text " for more details."
            }
            |> Doc.Concat
          )
        ]
      ]

    let code =
      """open Weave
open WebSharper.UI.Html

Body1.Div(
    seq {
        yield text "Visit our "
        yield
            Link.Create(
                Body1.Span("documentation"),   // brings its own font style
                href = "https://1eyewonder.github.io/Weave/"
            )
        yield text " for more details."
    }
    |> Doc.Concat
)

Body1.Div(
    seq {
        yield text "Visit our "
        yield
            Link.Create(
                H6.Span("documentation"),   // brings its own font style
                href = "https://1eyewonder.github.io/Weave/"
            )
        yield text " for more details."
    }
    |> Doc.Concat
)
"""

    Helpers.codeSampleSection "Basic Link" description content code

  let private underlineExamples () =
    let description =
      Helpers.bodyText
        "Control when the underline is shown: OnHover (default), Always, or None. The underline is applied only to the text — icon adornments are never underlined."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [] [
              Link.Create(
                H3.Div("OnHover (default)"),
                href = "https://1eyewonder.github.io/Weave/",
                attrs = [ Link.Underline.toClass Link.Underline.OnHover |> cl ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Link.Create(
                H3.Div("Always"),
                href = "https://1eyewonder.github.io/Weave/",
                attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Link.Create(
                H3.Div("None"),
                href = "https://1eyewonder.github.io/Weave/",
                attrs = [ Link.Underline.toClass Link.Underline.None |> cl ]
              )
            ]
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

Link.Create(
  H3.Div("OnHover (default)"),
  href = "https://1eyewonder.github.io/Weave/",
  attrs = [ Link.Underline.toClass Link.Underline.OnHover |> cl ]
)

Link.Create(
  H3.Div("Always"),
  href = "https://1eyewonder.github.io/Weave/",
  attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
)

Link.Create(
  H3.Div("None"),
  href = "https://1eyewonder.github.io/Weave/",
  attrs = [ Link.Underline.toClass Link.Underline.None |> cl ]
)"""

    Helpers.codeSampleSection "Underline" description content code

  let private colorExamples () =
    let colors = [
      BrandColor.Primary
      BrandColor.Secondary
      BrandColor.Tertiary
      BrandColor.Error
      BrandColor.Warning
      BrandColor.Success
      BrandColor.Info
    ]

    let description =
      Helpers.bodyText "Links can use any BrandColor from the theme palette."

    let content =
      Grid.Create(
        colors
        |> List.map (fun color ->
          GridItem.Create(
            Link.Create(
              text (sprintf "%A" color),
              href = "https://1eyewonder.github.io/Weave/",
              attrs = [
                Link.Color.toClass color |> cl
                Link.Underline.toClass Link.Underline.Always |> cl
              ]
            )
          )),
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

let colors = [ BrandColor.Primary; BrandColor.Secondary; (* ... *) ]

colors |> List.map (fun color ->
    Link.Create(
        text (sprintf "%A" color),
        href = "#",
        attrs = [
            Link.Color.toClass color |> cl         // see here
            Link.Underline.toClass Link.Underline.Always |> cl
        ]
    )
)"""

    Helpers.codeSampleSection "Colors" description content code

  let private iconAdornmentExamples () =
    let description =
      Helpers.bodyText
        "Links can have a start icon, an end icon, or both. Icons are never underlined regardless of the underline setting."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [] [
              Caption.Div("Start icon")
              Link.Create(
                text "Open in new tab",
                href = "https://1eyewonder.github.io/Weave/",
                startIcon = Icon.Create(Icon.UiActions UiActions.OpenInNew),
                attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Caption.Div("End icon")
              Link.Create(
                text "Download",
                href = "https://1eyewonder.github.io/Weave/",
                endIcon = Icon.Create(Icon.UiActions UiActions.Download),
                attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Caption.Div("Both icons")
              Link.Create(
                text "Send email",
                href = "https://1eyewonder.github.io/Weave/",
                startIcon = Icon.Create(Icon.Communicate Communicate.Mail),
                endIcon = Icon.Create(Icon.UiActions UiActions.ArrowForward),
                attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
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
open Weave.CssHelpers

// Start icon only
Link.Create(
    text "Open in new tab",
    href = "#",
    startIcon = Icon.Create(Icon.UiActions UiActions.OpenInNew), // see here
    attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
)

// End icon only
Link.Create(
    text "Download",
    href = "#",
    endIcon = Icon.Create(Icon.UiActions UiActions.Download), // see here
    attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
)"""

    Helpers.codeSampleSection "Icon Adornments" description content code

  let private iconOnlyExample () =
    let description =
      Helpers.bodyText "Use Link.CreateIcon when the link should contain only an icon with no text label."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Link.CreateIcon(
              Icon.Create(Icon.UiActions UiActions.Home),
              href = "https://1eyewonder.github.io/Weave/"
            )
          )
          GridItem.Create(
            Link.CreateIcon(
              Icon.Create(Icon.UiActions UiActions.Settings),
              href = "https://1eyewonder.github.io/Weave/",
              attrs = [ Link.Color.toClass BrandColor.Secondary |> cl ]
            )
          )
          GridItem.Create(
            Link.CreateIcon(
              Icon.Create(Icon.UiActions UiActions.Delete),
              href = "https://1eyewonder.github.io/Weave/",
              attrs = [ Link.Color.toClass BrandColor.Error |> cl ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.CssHelpers

// Icon-only link (no text)
Link.CreateIcon(
    Icon.Create(Icon.UiActions UiActions.Home),
    href = "#"
)

// With color
Link.CreateIcon(
    Icon.Create(Icon.UiActions UiActions.Delete),
    href = "#",
    attrs = [ Link.Color.toClass BrandColor.Error |> cl ]
)"""

    Helpers.codeSampleSection "Icon-Only" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText "A disabled link prevents navigation and suppresses the onClick callback."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [] [
              Caption.Div("Enabled")
              Link.Create(
                text "Active link",
                href = "https://1eyewonder.github.io/Weave/",
                enabled = View.Const true,
                attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Caption.Div("Disabled")
              Link.Create(
                text "Disabled link",
                href = "https://1eyewonder.github.io/Weave/",
                enabled = View.Const false,
                attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
              )
            ]
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

// Enabled (default)
Link.Create(
    text "Active link",
    href = "#",
    enabled = View.Const true,
    attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
)

// Disabled — no navigation, onClick not fired
Link.Create(
    text "Disabled link",
    href = "#",
    enabled = View.Const false, // see here
    attrs = [ Link.Underline.toClass Link.Underline.Always |> cl ]
)"""

    Helpers.codeSampleSection "Disabled" description content code

  let render () =
    Container.Create(
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
      maxWidth = Container.MaxWidth.Large
    )
