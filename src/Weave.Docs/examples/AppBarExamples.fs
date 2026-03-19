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
      AppBar.Create(
        div [
          cls [
            Flex.Flex.allSizes
            AlignItems.toClass AlignItems.Center
            yield! Padding.toClasses Padding.Horizontal.medium
            yield! Padding.toClasses Padding.Vertical.small
          ]
        ] [ H6.Div("My Application") ],
        position = AppBar.Position.Static,
        attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
      )

    let code =
      """AppBar.Create(
    div [
        cls [
            Flex.Flex.allSizes
            AlignItems.toClass AlignItems.Center
            yield! Padding.toClasses Padding.Horizontal.medium
            yield! Padding.toClasses Padding.Vertical.small
        ]
    ] [ H6.Div("My Application") ],
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
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          yield! Padding.toClasses Padding.Horizontal.medium
          yield! Padding.toClasses Padding.Vertical.small
        ]
      ] [ H6.Div(title) ]

    let filler () =
      div [ Padding.toClasses Padding.All.small |> cls ] [
        yield! [ 1..10 ] |> List.map (fun i -> Body2.Div(sprintf "Content line %d" i))
      ]

    // Static: bar scrolls away with content inside a contained scroll box
    let staticPreview =
      div [ Attr.Style "height" "180px"; Attr.Style "overflow-y" "auto" ] [
        AppBar.Create(
          toolbarInner "Position.Static",
          position = AppBar.Position.Static,
          attrs = [ BrandColor.toBackgroundColor BrandColor.Info ]
        )
        filler ()
      ]

    // Sticky: bar sticks to the top of the scroll container
    let stickyPreview =
      div [ Attr.Style "height" "180px"; Attr.Style "overflow-y" "auto" ] [
        AppBar.Create(
          toolbarInner "Position.Sticky",
          position = AppBar.Position.Sticky,
          attrs = [ BrandColor.toBackgroundColor BrandColor.Success ]
        )
        filler ()
      ]

    // Fixed: bar pinned to top, content scrolls below
    let fixedPreview =
      div [
        cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ]
        Attr.Style "height" "180px"
      ] [
        AppBar.Create(
          toolbarInner "Position.Fixed",
          position = AppBar.Position.Static,
          attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
        )
        div [
          Attr.Style "overflow-y" "auto"
          Attr.Style "flex" "1"
          Padding.toClasses Padding.All.small |> cls
        ] [
          yield! [ 1..10 ] |> List.map (fun i -> Body2.Div(sprintf "Content line %d" i))
        ]
      ]

    // Bottom: content scrolls above, bar pinned to bottom
    let bottomPreview =
      div [
        cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ]
        Attr.Style "height" "180px"
      ] [
        div [
          Attr.Style "overflow-y" "auto"
          Attr.Style "flex" "1"
          Padding.toClasses Padding.All.small |> cls
        ] [
          yield! [ 1..10 ] |> List.map (fun i -> Body2.Div(sprintf "Content line %d" i))
        ]
        AppBar.Create(
          toolbarInner "Position.Bottom",
          position = AppBar.Position.Static,
          attrs = [ BrandColor.toBackgroundColor BrandColor.Secondary ]
        )
      ]

    let mkCard (title: string) (desc: string) (preview: Doc) =
      div [ Margin.toClasses Margin.Bottom.small |> cls; Attr.Style "width" "100%" ] [
        H6.Div(title, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        div [ Margin.toClasses Margin.Bottom.extraSmall |> cls ] [ Helpers.bodyText desc ]
        preview
      ]

    let content =
      Grid.Create(
        [
          GridItem.Create(
            mkCard
              "Static"
              "Stays in document flow and scrolls away with content. Scroll the preview."
              staticPreview,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            mkCard
              "Sticky"
              "Scrolls with the page until it reaches the top, then sticks. Scroll the preview."
              stickyPreview,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            mkCard "Fixed" "Always pinned to the top of the viewport (simulated)." fixedPreview,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
          GridItem.Create(
            mkCard "Bottom" "Always pinned to the bottom of the viewport (simulated)." bottomPreview,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """// Static — scrolls away with content
AppBar.Create(content, position = AppBar.Position.Static)

// Sticky — sticks to the top once the page scrolls past it
AppBar.Create(content, position = AppBar.Position.Sticky)

// Fixed (default) — always pinned to the top of the viewport
AppBar.Create(content)

// Bottom — always pinned to the bottom of the viewport
AppBar.Create(content, position = AppBar.Position.Bottom)"""

    Helpers.codeSampleSection "Position" description content code

  let private contentExample () =
    let description =
      Helpers.bodyText
        "AppBar accepts any Doc as content. Combine it with Spacer to push \
         action icons to the right edge while keeping branding anchored left."

    let preview =
      AppBar.Create(
        div [
          cls [
            Flex.Flex.allSizes
            AlignItems.toClass AlignItems.Center
            yield! Padding.toClasses Padding.Horizontal.small
            yield! Padding.toClasses Padding.Vertical.extraSmall
          ]
        ] [
          IconButton.create (
            Icon.Create(Icon.UiActions UiActions.Menu),
            onClick = (fun () -> ()),
            attrs = [ Margin.toClasses Margin.Right.extraSmall |> cls ]
          )
          H6.Div("My Application")
          Spacer.Create()
          IconButton.create (Icon.Create(Icon.UiActions UiActions.Search), onClick = (fun () -> ()))
          IconButton.create (Icon.Create(Icon.Social Social.Person), onClick = (fun () -> ()))
        ],
        position = AppBar.Position.Static,
        attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
      )

    let code =
      """AppBar.Create(
    div [
        cls [
            Flex.Flex.allSizes
            AlignItems.toClass AlignItems.Center
            yield! Padding.toClasses Padding.Horizontal.small
            yield! Padding.toClasses Padding.Vertical.extraSmall
        ]
    ] [
        IconButton.create(
          Icon.Create(Icon.UiActions UiActions.Menu),
          onClick = (fun () -> ()),
          attrs = [ Margin.toClasses Margin.Right.extraSmall |> cls ]
        )
        H6.Div("My Application")
        Spacer.Create()
        IconButton.create(Icon.Create(Icon.UiActions UiActions.Search), onClick = (fun () -> ()))
        IconButton.create(Icon.Create(Icon.Social Social.Person), onClick = (fun () -> ()))
    ],
    attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
)"""

    Helpers.codeSampleSection "Content Composition" description preview code

  let render () =
    Container.Create(
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
      maxWidth = Container.MaxWidth.Large
    )
