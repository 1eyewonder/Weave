namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ButtonMenuExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText
        "A ButtonMenu fans out a list of icon buttons when its trigger is clicked. Pass closedIcon for the default state. If no openIcon is provided, the icon rotates 45 degrees when opened."

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.center; AlignItems.center ] [
        IconButtonMenu.create (
          closedIcon = Icon.create (Icon.UiActions UiActions.Add),
          items = [
            Tooltip.create (
              IconButton.info (
                Icon.create (Icon.Social Social.Share),
                onClick = (fun () -> printfn "share clicked"),
                attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Share" ],
              direction = Tooltip.Direction.Right
            )
            Tooltip.create (
              IconButton.error (
                Icon.create (Icon.UiActions UiActions.Favorite),
                onClick = (fun () -> printfn "favorite clicked"),
                attrs = [
                  Attr.Create "aria-label" "favorite"
                  Button.Variant.filled
                  BorderRadius.circle
                ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
              direction = Tooltip.Direction.Right
            )
            Tooltip.create (
              IconButton.success (
                Icon.create (Icon.Text Text.ContentCopy),
                onClick = (fun () -> printfn "copy clicked"),
                attrs = [ Attr.Create "aria-label" "copy"; Button.Variant.filled; BorderRadius.circle ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Copy" ],
              direction = Tooltip.Direction.Right
            )
          ],
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add),
    items = [
        Tooltip.create(
            IconButton.info(
                Icon.create(Icon.Social Social.Share),
                onClick = (fun () -> printfn "share clicked"),
                attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    BorderRadius.circle
                ]
            ),
            tooltipContent = span [ Typography.body1 ] [ text "Share" ],
            direction = Tooltip.Direction.Right
        )
        Tooltip.create(
            IconButton.error(
                Icon.create(Icon.UiActions UiActions.Favorite),
                onClick = (fun () -> printfn "favorite clicked"),
                attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    BorderRadius.circle
                ]
            ),
            tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
            direction = Tooltip.Direction.Right
        )
        Tooltip.create(
            IconButton.success(
                Icon.create(Icon.Text Text.ContentCopy),
                onClick = (fun () -> printfn "copy clicked"),
                attrs = [
                    Attr.Create "aria-label" "copy"
                    Button.Variant.filled
                    BorderRadius.circle
                ]
            ),
            tooltipContent = span [ Typography.body1 ] [ text "Copy" ],
            direction = Tooltip.Direction.Right
        )
    ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.circle
    ]
)
"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private iconSwapExample () =
    let description =
      Helpers.bodyText
        "When an openIcon is provided, the trigger swaps between the closed and open icons. Without openIcon, the closed icon rotates 45 degrees instead."

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.spaceEvenly; AlignItems.center ] [
        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
          div [ Typography.caption; Margin.Bottom.small ] [ text "With openIcon" ]
          IconButtonMenu.create (
            closedIcon = Icon.create (Icon.Communicate Communicate.Send),
            openIcon = Icon.create (Icon.UiActions UiActions.Close),
            items = [
              Tooltip.create (
                IconButton.secondary (
                  Icon.create (Icon.Social Social.Share),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Share" ],
                direction = Tooltip.Direction.Right
              )
              Tooltip.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    BorderRadius.circle
                  ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
          )
        ]
        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
          div [ Typography.caption; Margin.Bottom.small ] [ text "Without openIcon (rotates)" ]
          IconButtonMenu.create (
            closedIcon = Icon.create (Icon.UiActions UiActions.Add),
            items = [
              Tooltip.create (
                IconButton.secondary (
                  Icon.create (Icon.Social Social.Share),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Share" ],
                direction = Tooltip.Direction.Right
              )
              Tooltip.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    BorderRadius.circle
                  ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
          )
        ]
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


// With openIcon: swaps between send and close icons
IconButtonMenu.create(
    closedIcon = Icon.create(Icon.Communicate Communicate.Send),
    openIcon = Icon.create(Icon.UiActions UiActions.Close), // see here
    items = [ ... ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.circle
    ]
)

// Without openIcon: the Add icon rotates 45 degrees (becomes an X)
IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add), // rotates when open
    items = [ ... ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.circle
    ]
)
"""

    Helpers.codeSampleSection "Icon Swap vs Rotation" description content code

  let private directionExamples () =
    let description =
      Helpers.bodyText "The menu can fan out in four directions: Top (default), Bottom, Left, and Right."

    let makeMenu (label: string) direction =
      let tooltipDir =
        match direction with
        | ButtonMenu.Direction.Top -> Tooltip.Direction.Right
        | ButtonMenu.Direction.Bottom -> Tooltip.Direction.Right
        | ButtonMenu.Direction.Left -> Tooltip.Direction.Top
        | ButtonMenu.Direction.Right -> Tooltip.Direction.Top

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
        div [ Typography.caption; Margin.Bottom.small ] [ text label ]
        IconButtonMenu.create (
          closedIcon = Icon.create (Icon.UiActions UiActions.Add),
          items = [
            Tooltip.create (
              IconButton.info (
                Icon.create (Icon.Action Action.Alarm),
                onClick = (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "alarm"; Button.Variant.filled; BorderRadius.circle ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Alarm" ],
              direction = tooltipDir
            )
            Tooltip.create (
              IconButton.warning (
                Icon.create (Icon.UiActions UiActions.Star),
                onClick = (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "star"; Button.Variant.filled; BorderRadius.circle ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Star" ],
              direction = tooltipDir
            )
          ],
          direction = direction,
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
        )
      ]

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.spaceEvenly; AlignItems.center ] [
        makeMenu "Top" ButtonMenu.Direction.Top
        makeMenu "Bottom" ButtonMenu.Direction.Bottom
        makeMenu "Left" ButtonMenu.Direction.Left
        makeMenu "Right" ButtonMenu.Direction.Right
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


let items = [
    IconButton.info(
        Icon.create(Icon.Action Action.Alarm),
        onClick = (fun () -> ()),
        attrs = [
            Attr.Create "aria-label" "alarm"
            Button.Variant.filled
            BorderRadius.circle
        ]
    )
    IconButton.warning(
        Icon.create(Icon.UiActions UiActions.Star),
        onClick = (fun () -> ()),
        attrs = [
            Attr.Create "aria-label" "star"
            Button.Variant.filled
            BorderRadius.circle
        ]
    )
]

let triggerAttrs = [
    Button.Variant.filled
    Button.Color.primary
    BorderRadius.circle
]

IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add),
    items = items,
    direction = ButtonMenu.Direction.Top, // see here
    triggerAttrs = triggerAttrs
)

IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add),
    items = items,
    direction = ButtonMenu.Direction.Bottom, // see here
    triggerAttrs = triggerAttrs
)

IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add),
    items = items,
    direction = ButtonMenu.Direction.Left, // see here
    triggerAttrs = triggerAttrs
)

IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add),
    items = items,
    direction = ButtonMenu.Direction.Right, // see here
    triggerAttrs = triggerAttrs
)
"""

    Helpers.codeSampleSection "Directions" description content code

  let private externalStateExample () =
    let description =
      Helpers.bodyText
        "You can control the menu's open state externally by passing a Var<bool> via the isOpen parameter. This enables programmatic control and integration with other UI elements."

    let isMenuOpen = Var.Create false

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
        div [ Attr.Style "margin-bottom" "16px" ] [
          Button.secondary (
            isMenuOpen.View
            |> View.Map(fun o -> if o then text "Close Menu" else text "Open Menu")
            |> Doc.EmbedView,
            onClick = (fun () -> isMenuOpen.Value <- not isMenuOpen.Value),
            attrs = [ Button.Variant.outlined ]
          )
        ]
        IconButtonMenu.create (
          closedIcon = Icon.create (Icon.UiActions UiActions.Add),
          openIcon = Icon.create (Icon.UiActions UiActions.Close),
          items = [
            Tooltip.create (
              IconButton.info (
                Icon.create (Icon.Social Social.Share),
                onClick = (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Share" ],
              direction = Tooltip.Direction.Right
            )
            Tooltip.create (
              IconButton.error (
                Icon.create (Icon.UiActions UiActions.Favorite),
                onClick = (fun () -> ()),
                attrs = [
                  Attr.Create "aria-label" "favorite"
                  Button.Variant.filled
                  BorderRadius.circle
                ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
              direction = Tooltip.Direction.Right
            )
          ],
          isOpen = isMenuOpen,
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

open WebSharper.UI

let isMenuOpen = Var.Create false

// External button to toggle the menu
Button.secondary(
    isMenuOpen.View
    |> View.Map (fun o -> if o then text "Close Menu" else text "Open Menu")
    |> Doc.EmbedView,
    onClick = (fun () -> isMenuOpen.Value <- not isMenuOpen.Value),
    attrs = [
        Button.Variant.outlined
    ]
)

IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add),
    openIcon = Icon.create(Icon.UiActions UiActions.Close),
    items = [ ... ],
    isOpen = isMenuOpen, // see here - shared state
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.circle
    ]
)
"""

    Helpers.codeSampleSection "External State Control" description content code

  let private hoverExample () =
    let description =
      Helpers.bodyText
        "Set openOnHover to true so the menu opens when the user hovers over the trigger. Clicking the trigger still toggles the menu as usual."

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.spaceEvenly; AlignItems.center ] [
        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
          div [ Typography.caption; Margin.Bottom.small ] [ text "Icon button" ]
          IconButtonMenu.create (
            closedIcon = Icon.create (Icon.UiActions UiActions.Add),
            openIcon = Icon.create (Icon.UiActions UiActions.Close),
            openOnHover = View.Const true,
            items = [
              Tooltip.create (
                IconButton.info (
                  Icon.create (Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Share" ],
                direction = Tooltip.Direction.Right
              )
              Tooltip.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    BorderRadius.circle
                  ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
          )
        ]
        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
          div [ Typography.caption; Margin.Bottom.small ] [ text "Text button" ]
          ButtonMenu.create (
            closedContent = text "Menu",
            openOnHover = View.Const true,
            items = [
              Tooltip.create (
                IconButton.info (
                  Icon.create (Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Share" ],
                direction = Tooltip.Direction.Right
              )
              Tooltip.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    BorderRadius.circle
                  ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


// Icon button with hover
IconButtonMenu.create(
    closedIcon = Icon.create(Icon.UiActions UiActions.Add),
    openIcon = Icon.create(Icon.UiActions UiActions.Close),
    openOnHover = View.Const true,
    items = [
        IconButton.info(
            Icon.create(Icon.Social Social.Share),
            onClick = (fun () -> printfn "share clicked"),
            attrs = [
                Attr.Create "aria-label" "share"
                Button.Variant.filled
                BorderRadius.circle
            ]
        )
    ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.circle
    ]
)

// Text button with hover
ButtonMenu.create(
    closedContent = text "Menu",
    openOnHover = View.Const true,
    items = [
        IconButton.info(
            Icon.create(Icon.Social Social.Share),
            onClick = (fun () -> printfn "share clicked"),
            attrs = [
                Attr.Create "aria-label" "share"
                Button.Variant.filled
                BorderRadius.circle
            ]
        )
    ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Open on Hover" description content code

  let private textButtonExample () =
    let description =
      Helpers.bodyText
        "ButtonMenu.create uses a standard text button as the trigger instead of an icon button. This is useful for labeled menus. It supports optional openContent to change the label when open. Since the button's inner content is fully composable, you can include icons alongside text by composing them in closedContent/openContent."

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.spaceEvenly; AlignItems.center ] [
        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
          div [ Typography.caption; Margin.Bottom.small ] [ text "Text only" ]
          ButtonMenu.create (
            closedContent = text "Menu",
            items = [
              Tooltip.create (
                IconButton.info (
                  Icon.create (Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Share" ],
                direction = Tooltip.Direction.Right
              )
              Tooltip.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    BorderRadius.circle
                  ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [ Button.Variant.text; Button.Color.primary ]
          )
        ]
        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
          div [ Typography.caption; Margin.Bottom.small ] [ text "With icon and openContent" ]
          ButtonMenu.create (
            closedContent = Doc.Concat [ Icon.create (Icon.UiActions UiActions.Add); text "Actions" ],
            openContent = Doc.Concat [ Icon.create (Icon.UiActions UiActions.Add); text "Close" ],
            items = [
              Tooltip.create (
                IconButton.info (
                  Icon.create (Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Share" ],
                direction = Tooltip.Direction.Right
              )
              Tooltip.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    BorderRadius.circle
                  ]
                ),
                tooltipContent = span [ Typography.body1 ] [ text "Favorite" ],
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [ Button.Variant.filled; Button.Color.tertiary ]
          )
        ]
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


let menuItems = [
    IconButton.info(
        Icon.create(Icon.Social Social.Share),
        onClick = (fun () -> printfn "share clicked"),
        attrs = [
            Attr.Create "aria-label" "share"
            Button.Variant.filled
            BorderRadius.circle
        ]
    )
    IconButton.error(
        Icon.create(Icon.UiActions UiActions.Favorite),
        onClick = (fun () -> printfn "favorite clicked"),
        attrs = [
            Attr.Create "aria-label" "favorite"
            Button.Variant.filled
            BorderRadius.circle
        ]
    )
]

// Text-only trigger
ButtonMenu.create(
    closedContent = text "Menu",
    items = menuItems,
    triggerAttrs = [
        Button.Variant.text
        Button.Color.primary
    ]
)

// With icon and openContent: compose icon + text in closedContent
ButtonMenu.create(
    closedContent = Doc.Concat [ Icon.create(Icon.UiActions UiActions.Add); text "Actions" ],
    openContent = Doc.Concat [ Icon.create(Icon.UiActions UiActions.Add); text "Close" ],
    items = menuItems,
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.tertiary
    ]
)
"""

    Helpers.codeSampleSection "Text Button Trigger" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Button Menu"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "A floating action button menu that fans out a list of items when clicked. Supports icon button triggers (IconButtonMenu) and standard text button triggers (ButtonMenu), four fan-out directions, and icon swap or rotation animations."
        ]

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        iconSwapExample ()
        Helpers.divider ()
        directionExamples ()
        Helpers.divider ()
        externalStateExample ()
        Helpers.divider ()
        hoverExample ()
        Helpers.divider ()
        textButtonExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
