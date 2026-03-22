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
        attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
      )

    let code =
      """AppBar.create(
    div [
        Flex.Flex.allSizes
        AlignItems.center
        Padding.Horizontal.medium
        Padding.Vertical.small
    ] [ div [ Typography.h6 ] [ text "My Application" ] ],
    attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
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
        AppBar.create (
          toolbarInner "Position.Static",
          attrs = [ BrandColor.toBackgroundColor BrandColor.Info ]
        )
        filler ()
      ]

    // Sticky: bar sticks to the top of the scroll container
    let stickyPreview =
      div [ Attr.Style "height" "180px"; Attr.Style "overflow-y" "auto" ] [
        AppBar.create (
          toolbarInner "Position.Sticky",
          attrs = [ AppBar.Position.sticky; BrandColor.toBackgroundColor BrandColor.Success ]
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
        AppBar.create (
          toolbarInner "Position.Fixed",
          attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
        )
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
        AppBar.create (
          toolbarInner "Position.Bottom",
          attrs = [ BrandColor.toBackgroundColor BrandColor.Secondary ]
        )
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
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            mkCard
              "Sticky"
              "Scrolls with the page until it reaches the top, then sticks. Scroll the preview."
              stickyPreview,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            mkCard "Fixed" "Always pinned to the top of the viewport (simulated)." fixedPreview,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.create (
            mkCard "Bottom" "Always pinned to the bottom of the viewport (simulated)." bottomPreview,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
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
        attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
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
    attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
)"""

    Helpers.codeSampleSection "Content Composition" description preview code

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
      ],
      attrs = [ Container.MaxWidth.large ]
    )
