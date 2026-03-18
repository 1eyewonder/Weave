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
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.Center
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        ButtonMenu.CreateIcon(
          closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
          items = [
            Tooltip.Create(
              Button.CreateIcon(
                Icon.Create(Icon.Social Social.Share),
                onClick = (fun () -> printfn "share clicked"),
                attrs = [
                  Attr.Create "aria-label" "share"
                  Button.Variant.filled
                  Button.Color.info
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              ),
              tooltipContent = Body1.Span("Share"),
              direction = Tooltip.Direction.Right
            )
            Tooltip.Create(
              Button.CreateIcon(
                Icon.Create(Icon.UiActions UiActions.Favorite),
                onClick = (fun () -> printfn "favorite clicked"),
                attrs = [
                  Attr.Create "aria-label" "favorite"
                  Button.Variant.filled
                  Button.Color.error
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              ),
              tooltipContent = Body1.Span("Favorite"),
              direction = Tooltip.Direction.Right
            )
            Tooltip.Create(
              Button.CreateIcon(
                Icon.Create(Icon.Text Text.ContentCopy),
                onClick = (fun () -> printfn "copy clicked"),
                attrs = [
                  Attr.Create "aria-label" "copy"
                  Button.Variant.filled
                  Button.Color.success
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              ),
              tooltipContent = Body1.Span("Copy"),
              direction = Tooltip.Direction.Right
            )
          ],
          triggerAttrs = [
            Button.Variant.filled
            Button.Color.primary
            BorderRadius.toClass BorderRadius.Circle |> cl
          ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
    items = [
        Tooltip.Create(
            Button.CreateIcon(
                Icon.Create(Icon.Social Social.Share),
                onClick = (fun () -> printfn "share clicked"),
                attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    Button.Color.info
                    BorderRadius.toClass BorderRadius.Circle |> cl
                ]
            ),
            tooltipContent = Body1.Span("Share"),
            direction = Tooltip.Direction.Right
        )
        Tooltip.Create(
            Button.CreateIcon(
                Icon.Create(Icon.UiActions UiActions.Favorite),
                onClick = (fun () -> printfn "favorite clicked"),
                attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.error
                    BorderRadius.toClass BorderRadius.Circle |> cl
                ]
            ),
            tooltipContent = Body1.Span("Favorite"),
            direction = Tooltip.Direction.Right
        )
        Tooltip.Create(
            Button.CreateIcon(
                Icon.Create(Icon.Text Text.ContentCopy),
                onClick = (fun () -> printfn "copy clicked"),
                attrs = [
                    Attr.Create "aria-label" "copy"
                    Button.Variant.filled
                    Button.Color.success
                    BorderRadius.toClass BorderRadius.Circle |> cl
                ]
            ),
            tooltipContent = Body1.Span("Copy"),
            direction = Tooltip.Direction.Right
        )
    ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.toClass BorderRadius.Circle |> cl
    ]
)
"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private iconSwapExample () =
    let description =
      Helpers.bodyText
        "When an openIcon is provided, the trigger swaps between the closed and open icons. Without openIcon, the closed icon rotates 45 degrees instead."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.SpaceEvenly
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Center
          ]
        ] [
          Caption.Div(content = "With openIcon", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
          ButtonMenu.CreateIcon(
            closedIcon = Icon.Create(Icon.Communicate Communicate.Send),
            openIcon = Icon.Create(Icon.UiActions UiActions.Close),
            items = [
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.Social Social.Share),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    Button.Color.secondary
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Share"),
                direction = Tooltip.Direction.Right
              )
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.error
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Favorite"),
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [
              Button.Variant.filled
              Button.Color.primary
              BorderRadius.toClass BorderRadius.Circle |> cl
            ]
          )
        ]
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Center
          ]
        ] [
          Caption.Div(
            content = "Without openIcon (rotates)",
            attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
          )
          ButtonMenu.CreateIcon(
            closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
            items = [
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.Social Social.Share),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    Button.Color.secondary
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Share"),
                direction = Tooltip.Direction.Right
              )
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.error
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Favorite"),
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [
              Button.Variant.filled
              Button.Color.primary
              BorderRadius.toClass BorderRadius.Circle |> cl
            ]
          )
        ]
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


// With openIcon: swaps between send and close icons
ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.Communicate Communicate.Send),
    openIcon = Icon.Create(Icon.UiActions UiActions.Close), // see here
    items = [ ... ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.toClass BorderRadius.Circle |> cl
    ]
)

// Without openIcon: the Add icon rotates 45 degrees (becomes an X)
ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add), // rotates when open
    items = [ ... ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.toClass BorderRadius.Circle |> cl
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

      div [
        cls [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        Caption.Div(label, attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
        ButtonMenu.CreateIcon(
          closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
          items = [
            Tooltip.Create(
              Button.CreateIcon(
                Icon.Create(Icon.Action Action.Alarm),
                onClick = (fun () -> ()),
                attrs = [
                  Attr.Create "aria-label" "alarm"
                  Button.Variant.filled
                  Button.Color.info
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              ),
              tooltipContent = Body1.Span("Alarm"),
              direction = tooltipDir
            )
            Tooltip.Create(
              Button.CreateIcon(
                Icon.Create(Icon.UiActions UiActions.Star),
                onClick = (fun () -> ()),
                attrs = [
                  Attr.Create "aria-label" "star"
                  Button.Variant.filled
                  Button.Color.warning
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              ),
              tooltipContent = Body1.Span("Star"),
              direction = tooltipDir
            )
          ],
          direction = direction,
          triggerAttrs = [
            Button.Variant.filled
            Button.Color.primary
            BorderRadius.toClass BorderRadius.Circle |> cl
          ]
        )
      ]

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.SpaceEvenly
          AlignItems.toClass AlignItems.Center
        ]
      ] [
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
    Button.CreateIcon(
        Icon.Create(Icon.Action Action.Alarm),
        onClick = (fun () -> ()),
        attrs = [
            Attr.Create "aria-label" "alarm"
            Button.Variant.filled
            Button.Color.info
            BorderRadius.toClass BorderRadius.Circle |> cl
        ]
    )
    Button.CreateIcon(
        Icon.Create(Icon.UiActions UiActions.Star),
        onClick = (fun () -> ()),
        attrs = [
            Attr.Create "aria-label" "star"
            Button.Variant.filled
            Button.Color.warning
            BorderRadius.toClass BorderRadius.Circle |> cl
        ]
    )
]

let triggerAttrs = [
    Button.Variant.filled
    Button.Color.primary
    BorderRadius.toClass BorderRadius.Circle |> cl
]

ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
    items = items,
    direction = ButtonMenu.Direction.Top, // see here
    triggerAttrs = triggerAttrs
)

ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
    items = items,
    direction = ButtonMenu.Direction.Bottom, // see here
    triggerAttrs = triggerAttrs
)

ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
    items = items,
    direction = ButtonMenu.Direction.Left, // see here
    triggerAttrs = triggerAttrs
)

ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
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
      div [
        cls [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        div [ Attr.Style "margin-bottom" "16px" ] [
          Button.Create(
            isMenuOpen.View
            |> View.Map(fun o -> if o then text "Close Menu" else text "Open Menu")
            |> Doc.EmbedView,
            onClick = (fun () -> isMenuOpen.Value <- not isMenuOpen.Value),
            attrs = [ Button.Variant.outlined; Button.Color.secondary ]
          )
        ]
        ButtonMenu.CreateIcon(
          closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
          openIcon = Icon.Create(Icon.UiActions UiActions.Close),
          items = [
            Tooltip.Create(
              Button.CreateIcon(
                Icon.Create(Icon.Social Social.Share),
                onClick = (fun () -> ()),
                attrs = [
                  Attr.Create "aria-label" "share"
                  Button.Variant.filled
                  Button.Color.info
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              ),
              tooltipContent = Body1.Span("Share"),
              direction = Tooltip.Direction.Right
            )
            Tooltip.Create(
              Button.CreateIcon(
                Icon.Create(Icon.UiActions UiActions.Favorite),
                onClick = (fun () -> ()),
                attrs = [
                  Attr.Create "aria-label" "favorite"
                  Button.Variant.filled
                  Button.Color.error
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              ),
              tooltipContent = Body1.Span("Favorite"),
              direction = Tooltip.Direction.Right
            )
          ],
          isOpen = isMenuOpen,
          triggerAttrs = [
            Button.Variant.filled
            Button.Color.primary
            BorderRadius.toClass BorderRadius.Circle |> cl
          ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

open WebSharper.UI

let isMenuOpen = Var.Create false

// External button to toggle the menu
Button.Create(
    isMenuOpen.View
    |> View.Map (fun o -> if o then text "Close Menu" else text "Open Menu")
    |> Doc.EmbedView,
    onClick = (fun () -> isMenuOpen.Value <- not isMenuOpen.Value),
    attrs = [
        Button.Variant.outlined
        Button.Color.secondary
    ]
)

ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
    openIcon = Icon.Create(Icon.UiActions UiActions.Close),
    items = [ ... ],
    isOpen = isMenuOpen, // see here - shared state
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.toClass BorderRadius.Circle |> cl
    ]
)
"""

    Helpers.codeSampleSection "External State Control" description content code

  let private hoverExample () =
    let description =
      Helpers.bodyText
        "Set openOnHover to true so the menu opens when the user hovers over the trigger. Clicking the trigger still toggles the menu as usual."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.SpaceEvenly
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Center
          ]
        ] [
          Caption.Div(content = "Icon button", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
          ButtonMenu.CreateIcon(
            closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
            openIcon = Icon.Create(Icon.UiActions UiActions.Close),
            openOnHover = View.Const true,
            items = [
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    Button.Color.info
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Share"),
                direction = Tooltip.Direction.Right
              )
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.error
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Favorite"),
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [
              Button.Variant.filled
              Button.Color.primary
              BorderRadius.toClass BorderRadius.Circle |> cl
            ]
          )
        ]
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Center
          ]
        ] [
          Caption.Div(content = "Text button", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
          ButtonMenu.Create(
            closedContent = text "Menu",
            openOnHover = View.Const true,
            items = [
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    Button.Color.info
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Share"),
                direction = Tooltip.Direction.Right
              )
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.error
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Favorite"),
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
ButtonMenu.CreateIcon(
    closedIcon = Icon.Create(Icon.UiActions UiActions.Add),
    openIcon = Icon.Create(Icon.UiActions UiActions.Close),
    openOnHover = View.Const true,
    items = [
        Button.CreateIcon(
            Icon.Create(Icon.Social Social.Share),
            onClick = (fun () -> printfn "share clicked"),
            attrs = [
                Attr.Create "aria-label" "share"
                Button.Variant.filled
                Button.Color.info
                BorderRadius.toClass BorderRadius.Circle |> cl
            ]
        )
    ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
        BorderRadius.toClass BorderRadius.Circle |> cl
    ]
)

// Text button with hover
ButtonMenu.Create(
    closedContent = text "Menu",
    openOnHover = View.Const true,
    items = [
        Button.CreateIcon(
            Icon.Create(Icon.Social Social.Share),
            onClick = (fun () -> printfn "share clicked"),
            attrs = [
                Attr.Create "aria-label" "share"
                Button.Variant.filled
                Button.Color.info
                BorderRadius.toClass BorderRadius.Circle |> cl
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
        "ButtonMenu.Create uses a standard text button as the trigger instead of an icon button. This is useful for labeled menus. It supports optional openContent to change the label when open. Since the button's inner content is fully composable, you can include icons alongside text by composing them in closedContent/openContent."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.SpaceEvenly
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Center
          ]
        ] [
          Caption.Div(content = "Text only", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
          ButtonMenu.Create(
            closedContent = text "Menu",
            items = [
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    Button.Color.info
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Share"),
                direction = Tooltip.Direction.Right
              )
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.error
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Favorite"),
                direction = Tooltip.Direction.Right
              )
            ],
            triggerAttrs = [ Button.Variant.text; Button.Color.primary ]
          )
        ]
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Center
          ]
        ] [
          Caption.Div(
            content = "With icon and openContent",
            attrs = [ Margin.toClasses Margin.Bottom.small |> cls ]
          )
          ButtonMenu.Create(
            closedContent = Doc.Concat [ Icon.Create(Icon.UiActions UiActions.Add); text "Actions" ],
            openContent = Doc.Concat [ Icon.Create(Icon.UiActions UiActions.Add); text "Close" ],
            items = [
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.Social Social.Share),
                  onClick = (fun () -> printfn "share clicked"),
                  attrs = [
                    Attr.Create "aria-label" "share"
                    Button.Variant.filled
                    Button.Color.info
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Share"),
                direction = Tooltip.Direction.Right
              )
              Tooltip.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> printfn "favorite clicked"),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.error
                    BorderRadius.toClass BorderRadius.Circle |> cl
                  ]
                ),
                tooltipContent = Body1.Span("Favorite"),
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
    Button.CreateIcon(
        Icon.Create(Icon.Social Social.Share),
        onClick = (fun () -> printfn "share clicked"),
        attrs = [
            Attr.Create "aria-label" "share"
            Button.Variant.filled
            Button.Color.info
            BorderRadius.toClass BorderRadius.Circle |> cl
        ]
    )
    Button.CreateIcon(
        Icon.Create(Icon.UiActions UiActions.Favorite),
        onClick = (fun () -> printfn "favorite clicked"),
        attrs = [
            Attr.Create "aria-label" "favorite"
            Button.Variant.filled
            Button.Color.error
            BorderRadius.toClass BorderRadius.Circle |> cl
        ]
    )
]

// Text-only trigger
ButtonMenu.Create(
    closedContent = text "Menu",
    items = menuItems,
    triggerAttrs = [
        Button.Variant.text
        Button.Color.primary
    ]
)

// With icon and openContent: compose icon + text in closedContent
ButtonMenu.Create(
    closedContent = Doc.Concat [ Icon.Create(Icon.UiActions UiActions.Add); text "Actions" ],
    openContent = Doc.Concat [ Icon.Create(Icon.UiActions UiActions.Add); text "Close" ],
    items = menuItems,
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.tertiary
    ]
)
"""

    Helpers.codeSampleSection "Text Button Trigger" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Button Menu"
        Body1.Div(
          "A floating action button menu that fans out a list of items when clicked. Supports icon button triggers (CreateIcon) and standard text button triggers (Create), four fan-out directions, and icon swap or rotation animations.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

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
      maxWidth = Container.MaxWidth.Large
    )
