namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module AppBarExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText
        "A minimal AppBar wrapping a title. The background and text colour are \
         driven by theme variables, but you can pass any BrandColor as an attr \
         to override them. The default position is Position.Fixed."

    let preview =
      AppBar.create (
        div [
          Flex.Flex.allSizes
          AlignItems.center
          Padding.Horizontal.medium
          Padding.Vertical.small
        ] [ div [ Typography.h6 ] [ text "My Application" ] ],
        attrs = [ BrandColor.BackgroundColor.primary ]
      )

    let code =
      """AppBar.create(
    div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Horizontal.medium
        Padding.Vertical.small
    ] [ div [ Typography.h6 ] [ text "My Application" ] ],
    attrs = [ BrandColor.BackgroundColor.primary ]
)
// Default position is Position.Fixed — sticks to the top of the viewport."""

    Helpers.codeSampleSection "Basic" description preview code

  let private positionExample () =
    let description =
      Helpers.bodyText
        "The `position` parameter controls how the AppBar interacts with scroll. \
         Static and Sticky previews below are scrollable — try it! Fixed and \
         Bottom are simulated inside clipped containers so they don't escape the page."

    let toolbarInner (title: string) =
      div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Horizontal.medium
        Padding.Vertical.small
      ] [ div [ Typography.h6 ] [ text title ] ]

    let filler () =
      div [ Padding.All.small ] [
        yield!
          [ 1..10 ]
          |> List.map (fun i -> div [ Typography.body2 ] [ text (sprintf "Content line %d" i) ])
      ]

    // Static: bar scrolls away with content inside a contained scroll box
    let staticPreview =
      div [ Attr.Style "height" "180px"; Attr.Style "overflow-y" "auto" ] [
        AppBar.create (toolbarInner "Position.Static", attrs = [ BrandColor.BackgroundColor.info ])
        filler ()
      ]

    // Sticky: bar sticks to the top of the scroll container
    let stickyPreview =
      div [ Attr.Style "height" "180px"; Attr.Style "overflow-y" "auto" ] [
        AppBar.create (
          toolbarInner "Position.Sticky",
          attrs = [ AppBar.Position.sticky; BrandColor.BackgroundColor.success ]
        )
        filler ()
      ]

    // Fixed: bar pinned to top, content scrolls below
    let fixedPreview =
      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Attr.Style "height" "180px"
      ] [
        AppBar.create (toolbarInner "Position.Fixed", attrs = [ BrandColor.BackgroundColor.primary ])
        div [ Attr.Style "overflow-y" "auto"; Attr.Style "flex" "1"; Padding.All.small ] [
          yield!
            [ 1..10 ]
            |> List.map (fun i -> div [ Typography.body2 ] [ text (sprintf "Content line %d" i) ])
        ]
      ]

    // Bottom: content scrolls above, bar pinned to bottom
    let bottomPreview =
      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Attr.Style "height" "180px"
      ] [
        div [ Attr.Style "overflow-y" "auto"; Attr.Style "flex" "1"; Padding.All.small ] [
          yield!
            [ 1..10 ]
            |> List.map (fun i -> div [ Typography.body2 ] [ text (sprintf "Content line %d" i) ])
        ]
        AppBar.create (toolbarInner "Position.Bottom", attrs = [ BrandColor.BackgroundColor.secondary ])
      ]

    let mkCard (title: string) (desc: string) (preview: Doc) =
      div [ Margin.Bottom.small; Attr.Style "width" "100%" ] [
        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text title ]
        div [ Margin.Bottom.extraSmall ] [ Helpers.bodyText desc ]
        preview
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            mkCard
              "Static"
              "Stays in document flow and scrolls away with content. Scroll the preview."
              staticPreview,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            mkCard
              "Sticky"
              "Scrolls with the page until it reaches the top, then sticks. Scroll the preview."
              stickyPreview,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            mkCard "Fixed" "Always pinned to the top of the viewport (simulated)." fixedPreview,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
          GridItem.create (
            mkCard "Bottom" "Always pinned to the bottom of the viewport (simulated)." bottomPreview,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """// Static — scrolls away with content
AppBar.create(content)

// Sticky — sticks to the top once the page scrolls past it
AppBar.create(content, position = AppBar.Position.sticky)

// Fixed (default) — always pinned to the top of the viewport
AppBar.create(content)

// Bottom — always pinned to the bottom of the viewport
AppBar.create(content, position = AppBar.Position.fixedBottom)"""

    Helpers.codeSampleSection "Position" description content code

  let private contentExample () =
    let description =
      Helpers.bodyText
        "AppBar accepts any Doc as content. Combine it with Spacer to push \
         action icons to the right edge while keeping branding anchored left."

    let preview =
      AppBar.create (
        div [
          Flex.Flex.allSizes
          AlignItems.center
          Padding.Horizontal.small
          Padding.Vertical.extraSmall
        ] [
          IconButton.create (
            Icon.create (Icon.UiActions UiActions.Menu),
            onClick = (fun () -> ()),
            attrs = [ Margin.Right.extraSmall ]
          )
          div [ Typography.h6 ] [ text "My Application" ]
          Spacer.create ()
          IconButton.create (Icon.create (Icon.UiActions UiActions.Search), onClick = (fun () -> ()))
          IconButton.create (Icon.create (Icon.Social Social.Person), onClick = (fun () -> ()))
        ],
        attrs = [ BrandColor.BackgroundColor.primary ]
      )

    let code =
      """AppBar.create(
    div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Horizontal.small
        Padding.Vertical.extraSmall
    ] [
        IconButton.create(
          Icon.create(Icon.UiActions UiActions.Menu),
          onClick = (fun () -> ()),
          attrs = [ Margin.Right.extraSmall ]
        )
        div [ Typography.h6 ] [ text "My Application" ]
        Spacer.create()
        IconButton.create(Icon.create(Icon.UiActions UiActions.Search), onClick = (fun () -> ()))
        IconButton.create(Icon.create(Icon.Social Social.Person), onClick = (fun () -> ()))
    ],
    attrs = [ BrandColor.BackgroundColor.primary ]
)"""

    Helpers.codeSampleSection "Content Composition" description preview code

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
        positionExample ()
        Helpers.divider ()
        contentExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
