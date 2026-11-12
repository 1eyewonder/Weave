namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ButtonMenuExamples =

  let private comparisonGrid columns items =
    let span =
      match columns with
      | 2 -> GridItem.Span.Medium.six
      | 4 -> GridItem.Span.Medium.three
      | _ -> GridItem.Span.Medium.four

    Grid.create (
      items
      |> List.map (fun item ->
        GridItem.create (item, attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; span ])),
      attrs = [ AlignItems.start ]
    )

  let private basicExample () =
    let description =
      Helpers.bodyText
        "A ButtonMenu fans out a list of icon buttons when its trigger is clicked. Pass closedIcon for the default state. If no openIcon is provided, the icon rotates 45 degrees when opened."

    let content =
      let frame menu =
        div [
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "min-height" "360px"
        ] [ menu ]

      frame (
        IconButtonMenu.create (
          closedIcon = Icon.create (Icon.UiActions UiActions.Add),
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
            Tooltip.create (
              IconButton.success (
                Icon.create (Icon.Text Text.ContentCopy),
                onClick = (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "copy"; Button.Variant.filled; BorderRadius.circle ]
              ),
              tooltipContent = span [ Typography.body1 ] [ text "Copy" ],
              direction = Tooltip.Direction.Right
            )
          ],
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
        )
      )

    let code =
      """open Weave.Icons.MaterialSymbols

IconButtonMenu.create (
  closedIcon = Icon.create (Icon.UiActions UiActions.Add),
  items = [
    Tooltip.create (
      IconButton.info (
        Icon.create (Icon.Social Social.Share),
        onClick = (fun () -> ()),
        attrs = [
          Attr.Create "aria-label" "share"
          Button.Variant.filled
          BorderRadius.circle
        ]
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
    Tooltip.create (
      IconButton.success (
        Icon.create (Icon.Text Text.ContentCopy),
        onClick = (fun () -> ()),
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
)"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let appearanceExample () =
    let direction = Var.Create "Top"
    let variant = Var.Create "Filled"
    let color = Var.Create "Primary"
    let swapIcon = Var.Create false

    let choice label values (value: Var<string>) =
      let selected =
        Var.Lens value Some (fun current next -> Option.defaultValue current next)

      let items =
        values
        |> List.map (fun name -> SelectItem.create (text name, name, name))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice "Direction" ButtonMenuAppearance.directions direction
        choice "Variant" ButtonMenuAppearance.variants variant
        div [ Attr.Style "grid-column" "1 / -1" ] [ choice "Color" ButtonMenuAppearance.colors color ]
      ]

    let config =
      let look = (variant.View, color.View) ||> View.Map2(fun a b -> a, b)
      let behavior = (direction.View, swapIcon.View) ||> View.Map2(fun a b -> a, b)
      (look, behavior) ||> View.Map2(fun (a, b) (c, d) -> c, a, b, d)

    let preview =
      config
      |> Doc.BindView(fun (direction, variant, color, swap) ->
        let direction =
          match direction with
          | "Bottom" -> ButtonMenu.Direction.Bottom
          | "Left" -> ButtonMenu.Direction.Left
          | "Right" -> ButtonMenu.Direction.Right
          | _ -> ButtonMenu.Direction.Top

        let variant =
          match variant with
          | "Outlined" -> Button.Variant.outlined
          | "Text" -> Button.Variant.text
          | _ -> Button.Variant.filled

        let color =
          match color with
          | "Secondary" -> Button.Color.secondary
          | "Tertiary" -> Button.Color.tertiary
          | "Error" -> Button.Color.error
          | "Warning" -> Button.Color.warning
          | "Success" -> Button.Color.success
          | "Info" -> Button.Color.info
          | _ -> Button.Color.primary

        let openIcon =
          if swap then
            Some(Icon.create (Icon.UiActions UiActions.Close))
          else
            None

        div [
          Attr.Style "min-height" "300px"
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
        ] [
          IconButtonMenu.create (
            closedIcon = Icon.create (Icon.UiActions UiActions.Add),
            ?openIcon = openIcon,
            items = [
              IconButton.info (
                Icon.create (Icon.Action Action.Alarm),
                onClick = ignore,
                attrs = [ Attr.Create "aria-label" "Alarm" ]
              )
              IconButton.warning (
                Icon.create (Icon.UiActions UiActions.Star),
                onClick = ignore,
                attrs = [ Attr.Create "aria-label" "Star" ]
              )
            ],
            direction = direction,
            triggerAttrs = [ variant; color ]
          )
        ])

    let source =
      config |> View.Map(fun (d, v, c, s) -> ButtonMenuAppearance.source d v c s)

    let reset () =
      Var.Set direction "Top"
      Var.Set variant "Filled"
      Var.Set color "Primary"
      Var.Set swapIcon false

    Helpers.appearanceSection
      [ "directions"; "icon-swap-vs-rotation" ]
      (Helpers.bodyText
        "Choose a direction and trigger style, then open the menu. With an open icon the trigger swaps icons; otherwise the closed icon rotates.")
      controls
      (Some(Switch.create (swapIcon, text "Swap open icon")))
      preview
      reset
      source

  let private externalStateExample () =
    let description =
      Helpers.bodyText
        "You can control the menu's open state externally by passing a Var<bool> via the isOpen parameter. This enables programmatic control and integration with other UI elements."

    let content =
      let frame menu =
        div [
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "min-height" "280px"
        ] [ menu ]

      let isMenuOpen = Var.Create false

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        div [ Flex.Flex.allSizes; JustifyContent.center ] [
          Button.secondary (
            isMenuOpen.View
            |> View.Map(fun o -> if o then text "Close Menu" else text "Open Menu")
            |> Doc.EmbedView,
            onClick = (fun () -> isMenuOpen.Value <- not isMenuOpen.Value),
            attrs = [ Button.Variant.outlined ]
          )
        ]

        frame (
          IconButtonMenu.create (
            closedIcon = Icon.create (Icon.UiActions UiActions.Add),
            openIcon = Icon.create (Icon.UiActions UiActions.Close),
            items = [
              IconButton.info (
                Icon.create (Icon.Social Social.Share),
                onClick = (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
              )
              IconButton.error (
                Icon.create (Icon.UiActions UiActions.Favorite),
                onClick = (fun () -> ()),
                attrs = [
                  Attr.Create "aria-label" "favorite"
                  Button.Variant.filled
                  BorderRadius.circle
                ]
              )
            ],
            isOpen = isMenuOpen, // see here -- shared Var controls open state
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
          )
        )
      ]

    let code =
      """open Weave.Icons.MaterialSymbols

let isMenuOpen = Var.Create false

Doc.Concat [
  Button.secondary (
    isMenuOpen.View
    |> View.Map(fun o -> if o then text "Close Menu" else text "Open Menu")
    |> Doc.EmbedView,
    onClick = (fun () -> isMenuOpen.Value <- not isMenuOpen.Value),
    attrs = [ Button.Variant.outlined ]
  )

  IconButtonMenu.create (
    closedIcon = Icon.create (Icon.UiActions UiActions.Add),
    openIcon = Icon.create (Icon.UiActions UiActions.Close),
    items = [
      IconButton.info (
        Icon.create (Icon.Social Social.Share),
        onClick = (fun () -> ()),
        attrs = [
          Attr.Create "aria-label" "share"
          Button.Variant.filled
          BorderRadius.circle
        ]
      )
      IconButton.error (
        Icon.create (Icon.UiActions UiActions.Favorite),
        onClick = (fun () -> ()),
        attrs = [
          Attr.Create "aria-label" "favorite"
          Button.Variant.filled
          BorderRadius.circle
        ]
      )
    ],
    isOpen = isMenuOpen, // see here -- shared Var controls open state
    triggerAttrs = [
      Button.Variant.filled
      Button.Color.primary
      BorderRadius.circle
    ]
  )
]
"""

    Helpers.codeSampleSection "External State Control" description content code

  let private hoverExample () =
    let description =
      Helpers.bodyText
        "Set openOnHover to true so the menu opens when the user hovers over the trigger. Clicking the trigger still toggles the menu as usual."

    let content =
      let frame menu =
        div [
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "min-height" "200px"
        ] [ menu ]

      comparisonGrid 2 [
        frame (
          IconButtonMenu.create (
            closedIcon = Icon.create (Icon.UiActions UiActions.Add),
            openIcon = Icon.create (Icon.UiActions UiActions.Close),
            openOnHover = View.Const true, // see here
            items = [
              IconButton.info (
                Icon.create (Icon.Social Social.Share),
                onClick = (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
              )
            ],
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary; BorderRadius.circle ]
          )
        )

        frame (
          ButtonMenu.create (
            closedContent = text "Menu",
            openOnHover = View.Const true, // see here
            items = [
              IconButton.info (
                Icon.create (Icon.Social Social.Share),
                onClick = (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
              )
            ],
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        )
      ]

    let code =
      """open Weave.Icons.MaterialSymbols

IconButtonMenu.create (
  closedIcon = Icon.create (Icon.UiActions UiActions.Add),
  openIcon = Icon.create (Icon.UiActions UiActions.Close),
  openOnHover = View.Const true, // see here
  items = [
    IconButton.info (
      Icon.create (Icon.Social Social.Share),
      onClick = (fun () -> ()),
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

ButtonMenu.create (
  closedContent = text "Menu",
  openOnHover = View.Const true, // see here
  items = [
    IconButton.info (
      Icon.create (Icon.Social Social.Share),
      onClick = (fun () -> ()),
      attrs = [
        Attr.Create "aria-label" "share"
        Button.Variant.filled
        BorderRadius.circle
      ]
    )
  ],
  triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
)"""

    Helpers.codeSampleSection "Open on Hover" description content code

  let private textButtonExample () =
    let description =
      Helpers.bodyText
        "ButtonMenu.create uses a standard text button as the trigger instead of an icon button. This is useful for labeled menus. It supports optional openContent to change the label when open. Since the button's inner content is fully composable, you can include icons alongside text by composing them in closedContent/openContent."

    let content =
      let frame menu =
        div [
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "min-height" "280px"
        ] [ menu ]

      let menuItems = [
        IconButton.info (
          Icon.create (Icon.Social Social.Share),
          onClick = (fun () -> ()),
          attrs = [ Attr.Create "aria-label" "share"; Button.Variant.filled; BorderRadius.circle ]
        )
        IconButton.error (
          Icon.create (Icon.UiActions UiActions.Favorite),
          onClick = (fun () -> ()),
          attrs = [
            Attr.Create "aria-label" "favorite"
            Button.Variant.filled
            BorderRadius.circle
          ]
        )
      ]

      comparisonGrid 2 [
        frame (
          ButtonMenu.create (
            closedContent = text "Menu",
            items = menuItems,
            triggerAttrs = [ Button.Variant.text; Button.Color.primary ]
          )
        )

        frame (
          ButtonMenu.create (
            closedContent = Doc.Concat [ Icon.create (Icon.UiActions UiActions.Add); text "Actions" ],
            openContent = Doc.Concat [ Icon.create (Icon.UiActions UiActions.Add); text "Close" ],
            items = menuItems,
            triggerAttrs = [ Button.Variant.filled; Button.Color.tertiary ]
          )
        )
      ]

    let code =
      """open Weave.Icons.MaterialSymbols

let menuItems = [
  IconButton.info (
    Icon.create (Icon.Social Social.Share),
    onClick = (fun () -> ()),
    attrs = [
      Attr.Create "aria-label" "share"
      Button.Variant.filled
      BorderRadius.circle
    ]
  )
  IconButton.error (
    Icon.create (Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
      Attr.Create "aria-label" "favorite"
      Button.Variant.filled
      BorderRadius.circle
    ]
  )
]

ButtonMenu.create (
  closedContent = text "Menu",
  items = menuItems,
  triggerAttrs = [ Button.Variant.text; Button.Color.primary ]
)

ButtonMenu.create (
  closedContent =
    Doc.Concat [ Icon.create (Icon.UiActions UiActions.Add); text "Actions" ],
  openContent =
    Doc.Concat [ Icon.create (Icon.UiActions UiActions.Add); text "Close" ],
  items = menuItems,
  triggerAttrs = [ Button.Variant.filled; Button.Color.tertiary ]
)"""

    Helpers.codeSampleSection "Text Button Trigger" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for ButtonMenu and IconButtonMenu.") [
      Helpers.apiTable "ButtonMenu.create" [
        Helpers.apiParam "closedContent" "Doc" "" "Button content shown when the menu is closed"
        Helpers.apiParam "items" "Doc list" "" "List of menu item Docs"
        Helpers.apiParam "?openContent" "Doc" "" "Alternative button content shown when the menu is open"
        Helpers.apiParam
          "?direction"
          "Direction"
          "Top"
          "Direction items fan out — Top, Bottom, Left, or Right"
        Helpers.apiParam "?isOpen" "Var<bool>" "" "External control over the menu open/closed state"
        Helpers.apiParam
          "?openOnHover"
          "View<bool>"
          "View.Const false"
          "Open the menu on mouse hover instead of click"
        Helpers.apiParam
          "?triggerAttrs"
          "Attr list"
          "[]"
          "Additional attributes applied to the trigger button"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root container"
      ]

      Helpers.apiTable "IconButtonMenu.create" [
        Helpers.apiParam "closedIcon" "Doc" "" "Icon shown when the menu is closed"
        Helpers.apiParam "items" "Doc list" "" "List of menu item Docs"
        Helpers.apiParam
          "?openIcon"
          "Doc"
          ""
          "Alternative icon shown when open; if omitted, closedIcon rotates 45°"
        Helpers.apiParam
          "?direction"
          "Direction"
          "Top"
          "Direction items fan out — Top, Bottom, Left, or Right"
        Helpers.apiParam "?isOpen" "Var<bool>" "" "External control over the menu open/closed state"
        Helpers.apiParam
          "?openOnHover"
          "View<bool>"
          "View.Const false"
          "Open the menu on mouse hover instead of click"
        Helpers.apiParam
          "?triggerAttrs"
          "Attr list"
          "[]"
          "Additional attributes applied to the trigger icon button"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root container"
      ]

      Helpers.styleModuleTable "ButtonMenu.Direction (DU)" [
        ("Top", "Items fan out upward from the trigger (default)")
        ("Bottom", "Items fan out downward from the trigger")
        ("Left", "Items fan out to the left of the trigger")
        ("Right", "Items fan out to the right of the trigger")
      ]
    ]

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
        appearanceExample ()
        Helpers.divider ()
        externalStateExample ()
        Helpers.divider ()
        hoverExample ()
        Helpers.divider ()
        textButtonExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
