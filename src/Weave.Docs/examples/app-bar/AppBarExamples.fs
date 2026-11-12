namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module AppBarExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText
        "AppBar accepts any Doc and uses the current theme. With no position attribute it stays in normal document flow."

    let content =
      AppBar.create (
        div [ Flex.Flex.allSizes; AlignItems.center; Padding.All.small ] [
          h3 [ Typography.h6 ] [ text "My Application" ]
        ]
      )

    let code =
      """AppBar.create (
  div [ Flex.Flex.allSizes; AlignItems.center; Padding.All.small ] [
    h3 [ Typography.h6 ] [ text "My Application" ]
  ]
)"""

    Helpers.codeSampleSection "Basic" description content code

  let appearanceExample () =
    let position = Var.Create "Static"
    let accent = Var.Create false

    let selected =
      Var.Lens position Some (fun current next -> Option.defaultValue current next)

    let items =
      AppBarAppearance.positions
      |> List.map (fun name -> SelectItem.create (text name, name, name))
      |> View.Const

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        Select.create (
          items,
          selected,
          labelText = View.Const "Position",
          attrs = [
            Select.Variant.outlined
            Select.Label.above
            Select.Width.full
            Select.PopoverWidth.unbound
            Select.Color.primary
            Attr.Style "grid-column" "1 / -1"
          ]
        )
      ]

    let config = (position.View, accent.View) ||> View.Map2(fun a b -> a, b)

    let preview =
      config
      |> Doc.BindView(fun (position, accent) ->
        let attrs = [
          match position with
          | "Sticky" -> AppBar.Position.sticky
          | "Fixed top" -> AppBar.Position.fixedTop
          | "Fixed bottom" -> AppBar.Position.fixedBottom
          | _ -> ()
          if accent then
            Attr.Style "--palette-appbar-background" "var(--palette-primary-fill)"
            Attr.Style "--palette-appbar-text" "var(--palette-primary-fill-text)"
        ]

        let contentAttrs =
          if position = "Fixed top" || position = "Fixed bottom" then
            [ Attr.Style "padding-block" "var(--appbar-height)" ]
          else
            []

        div [
          Attr.Style "position" "relative"
          Attr.Style "width" "100%"
          Attr.Style "height" "220px"
          Attr.Style "overflow" "hidden"
          Attr.Style "transform" "translateZ(0)"
        ] [
          div [
            Attr.Style "height" "100%"
            Attr.Style "overflow-y" "auto"
            Attr.Style "scrollbar-width" "thin"
            Attr.Style "scrollbar-color" "var(--palette-text-secondary) transparent"
          ] [
            AppBar.create (div [ Padding.All.small ] [ text "My Application" ], attrs = attrs)
            div contentAttrs [
              for index in 1..12 do
                p [] [ text (sprintf "Content line %d" index) ]
            ]
          ]
        ])

    let source =
      config
      |> View.Map(fun (position, accent) -> AppBarAppearance.source position accent)

    let reset () =
      Var.Set position "Static"
      Var.Set accent false

    Helpers.appearanceSection
      [ "position" ]
      (Helpers.bodyText
        "Scroll the frame to compare positions. Static scrolls away; Sticky remains at the scroll edge; Fixed pins to an edge. The frame confines fixed positioning here; in an app it is relative to the viewport.")
      controls
      (Some(Switch.create (accent, text "Primary color")))
      preview
      reset
      source

  let private contentExample () =
    let description =
      Helpers.bodyText
        "Group branding and actions so a narrow bar wraps between groups. Keep related controls together and name every icon-only action."

    let content =
      AppBar.create (
        div [
          Flex.Flex.allSizes
          AlignItems.center
          FlexWrap.Wrap.allSizes
          Gap.All.g2
          Padding.All.extraSmall
        ] [
          div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g1 ] [
            IconButton.create (
              Icon.create (Icon.UiActions UiActions.Menu),
              onClick = ignore,
              attrs = [ Attr.Create "aria-label" "Navigation" ]
            )
            div [ Typography.h6 ] [ text "My Application" ]
          ]
          div [
            Flex.Flex.allSizes
            AlignItems.center
            Attr.Style "margin-inline-start" "auto"
          ] [
            IconButton.create (
              Icon.create (Icon.UiActions UiActions.Search),
              onClick = ignore,
              attrs = [ Attr.Create "aria-label" "Search" ]
            )
            IconButton.create (
              Icon.create (Icon.Social Social.Person),
              onClick = ignore,
              attrs = [ Attr.Create "aria-label" "Profile" ]
            )
          ]
        ],
        attrs = [
          Attr.Style "--palette-appbar-background" "var(--palette-primary-fill)"
          Attr.Style "--palette-appbar-text" "var(--palette-primary-fill-text)"
        ]
      )

    let code =
      """open Weave.Icons.MaterialSymbols

AppBar.create (
  div [
    Flex.Flex.allSizes
    AlignItems.center
    FlexWrap.Wrap.allSizes
    Gap.All.g2
    Padding.All.extraSmall
  ] [
    div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g1 ] [
      IconButton.create (
        Icon.create (Icon.UiActions UiActions.Menu),
        onClick = ignore,
        attrs = [ Attr.Create "aria-label" "Navigation" ]
      )
      div [ Typography.h6 ] [ text "My Application" ]
    ]
    div [
      Flex.Flex.allSizes
      AlignItems.center
      Attr.Style "margin-inline-start" "auto"
    ] [
      IconButton.create (
        Icon.create (Icon.UiActions UiActions.Search),
        onClick = ignore,
        attrs = [ Attr.Create "aria-label" "Search" ]
      )
      IconButton.create (
        Icon.create (Icon.Social Social.Person),
        onClick = ignore,
        attrs = [ Attr.Create "aria-label" "Profile" ]
      )
    ]
  ],
  attrs = [
    Attr.Style "--palette-appbar-background" "var(--palette-primary-fill)"
    Attr.Style "--palette-appbar-text" "var(--palette-primary-fill-text)"
  ]
)"""

    Helpers.codeSampleSection "Content Composition" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for AppBar.") [
      Helpers.apiTable "AppBar.create" [
        Helpers.apiParam "content" "Doc" "" "Content rendered inside the bar (branding, navigation, actions)"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (position, etc.)"
      ]

      Helpers.styleModuleTable "AppBar.Position" [
        ("fixedTop", "Fixes the bar to the top of the viewport")
        ("fixedBottom", "Fixes the bar to the bottom of the viewport")
        ("sticky", "Sticks to the top once the page scrolls past it")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "App Bar"
        Helpers.bodyText
          "AppBar provides a top-level horizontal bar for branding, navigation, \
           and global actions. It themes automatically with the rest of the application."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        contentExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
