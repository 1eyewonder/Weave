namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DropdownMenuExamples =

  let clickableItem (alertVar: Var<string option>) n =
    DropdownMenuItem.create (
      text (sprintf "Item %d" n),
      onClick = (fun () -> Var.Set alertVar (Some(sprintf "Clicked item %d" n)))
    )

  let private alertDialog (messageVar: Var<string option>) =
    messageVar.View
    |> Doc.BindView (function
      | Some message ->
        Dialog.create (
          DialogTitle.create (div [ Typography.h6 ] [ text "Alert" ]),
          DialogContent.create (
            div [] [
              div [ Typography.body1 ] [ text message ]
              div [ Margin.Top.small ] [
                Button.primary (
                  text "OK",
                  onClick = (fun () -> Var.Set messageVar None),
                  attrs = [ Button.Variant.filled ]
                )
              ]
            ]
          ),
          dialogInteraction = View.Const(Dialog.Interaction.Optional(fun () -> Var.Set messageVar None))
        )
      | None -> Doc.Empty)

  let private basicDropdownExample () =
    let alertVar = Var.Create None

    let items = [ 1..3 ] |> List.map (clickableItem alertVar)

    let description =
      Helpers.bodyText
        "A simple dropdown with a few items. Clicking on the button or anywhere else on screen while the dropdown is open will close the dropdown."

    let content =
      div [] [
        alertDialog alertVar
        DropdownMenu.create (
          triggerContent = text "Open Dropdown",
          items = View.Const items,
          attrs = [ Margin.Bottom.extraSmall ],
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
        )
      ]

    let code =
      """open Weave

DropdownMenu.create(
    triggerContent = text "Open Dropdown",
    items = View.Const [
        DropdownMenuItem.create(text "Edit", onClick = (fun () -> printfn "Edit"))
        DropdownMenuItem.create(text "Duplicate", onClick = (fun () -> printfn "Duplicate"))
        DropdownMenuItem.create(text "Delete", onClick = (fun () -> printfn "Delete"))
    ],
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private colorExample () =
    let alertVar = Var.Create None

    let colors = [
      "Primary", DropdownMenu.Color.primary, Button.Color.primary
      "Secondary", DropdownMenu.Color.secondary, Button.Color.secondary
      "Tertiary", DropdownMenu.Color.tertiary, Button.Color.tertiary
      "Error", DropdownMenu.Color.error, Button.Color.error
      "Warning", DropdownMenu.Color.warning, Button.Color.warning
      "Success", DropdownMenu.Color.success, Button.Color.success
      "Info", DropdownMenu.Color.info, Button.Color.info
    ]

    let description =
      Helpers.bodyText
        "Dropdown supports all brand colors. Pass DropdownMenu.Color.* via attrs to tint the menu (chevron, highlighted items, focus outline), and the matching Button.Color.* via triggerAttrs for the trigger button."

    let content =
      div [] [
        alertDialog alertVar
        Grid.create (
          colors
          |> List.map (fun (label, dropdownColor, buttonColor) ->
            let items =
              [ 1..3 ]
              |> List.map (fun n ->
                DropdownMenuItem.create (
                  text (sprintf "%s Item %d" label n),
                  onClick = (fun () -> Var.Set alertVar (Some(sprintf "Clicked %s item %d" label n)))
                ))

            GridItem.create (
              DropdownMenu.create (
                triggerContent = text label,
                items = View.Const items,
                attrs = [ dropdownColor ],
                triggerAttrs = [ Button.Variant.filled; buttonColor ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            ))
        )
      ]

    let code =
      """open Weave

// DropdownMenu.Color.* tints the menu; Button.Color.* styles the trigger
DropdownMenu.create(
    triggerContent = text "Primary",
    items = View.Const items,
    attrs = [ DropdownMenu.Color.primary ],
    triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
)

DropdownMenu.create(
    triggerContent = text "Error",
    items = View.Const items,
    attrs = [ DropdownMenu.Color.error ],
    triggerAttrs = [ Button.Variant.filled; Button.Color.error ]
)"""

    Helpers.codeSampleSection "Colors" description content code

  let private widthExample () =
    let alertVar = Var.Create None

    let description =
      Helpers.bodyText
        "Dropdown supports two width modes: Full (100% of container) and Fit Content (sizes to widest item). Pass width classes via attrs using DropdownMenu.Width.*."

    let content =
      div [] [
        alertDialog alertVar
        Grid.create (
          [
            GridItem.create (
              div [] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Full Width" ]
                DropdownMenu.create (
                  triggerContent = text "Full Width",
                  items =
                    View.Const [
                      DropdownMenuItem.create (
                        text "Edit",
                        onClick = (fun () -> Var.Set alertVar (Some "Edit"))
                      )
                      DropdownMenuItem.create (
                        text "Duplicate",
                        onClick = (fun () -> Var.Set alertVar (Some "Duplicate"))
                      )
                    ],
                  attrs = [ DropdownMenu.Width.full ],
                  triggerAttrs = [ Button.Variant.filled; Button.Color.primary; Button.Width.full ]
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )

            GridItem.create (
              div [] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Fit Content" ]
                DropdownMenu.create (
                  triggerContent = text "Fit Content",
                  items =
                    View.Const [
                      DropdownMenuItem.create (
                        text "A longer menu item",
                        onClick = (fun () -> Var.Set alertVar (Some "Long item"))
                      )
                      DropdownMenuItem.create (
                        text "Short",
                        onClick = (fun () -> Var.Set alertVar (Some "Short"))
                      )
                    ],
                  attrs = [ DropdownMenu.Width.fitContent ],
                  triggerAttrs = [ Button.Variant.filled; Button.Color.secondary ]
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )
          ]
        )
      ]

    let code =
      """open Weave

// Full Width — menu stretches to 100% of the container
DropdownMenu.create(
    triggerContent = text "Full Width",
    items = View.Const items,
    attrs = [ DropdownMenu.Width.full ],  // see here
    triggerAttrs = [ Button.Variant.filled; Button.Color.primary; Button.Width.full ]
)

// Fit Content — menu sizes to the widest item
DropdownMenu.create(
    triggerContent = text "Fit Content",
    items = View.Const items,
    attrs = [ DropdownMenu.Width.fitContent ],  // see here
    triggerAttrs = [ Button.Variant.filled; Button.Color.secondary ]
)"""

    Helpers.codeSampleSection "Width Modes" description content code

  let private placementExample () =
    let alertVar = Var.Create None
    let anchorVar = Var.Create "Bottom Left"
    let transformVar = Var.Create "Top Left"

    let items = [ 1..3 ] |> List.map (clickableItem alertVar)

    let anchorClassMap =
      Map.ofList [
        "Top Left", "weave-dropdownmenu--anchor-origin-top-left"
        "Top Center", "weave-dropdownmenu--anchor-origin-top-center"
        "Top Right", "weave-dropdownmenu--anchor-origin-top-right"
        "Center Left", "weave-dropdownmenu--anchor-origin-center-left"
        "Center Center", "weave-dropdownmenu--anchor-origin-center-center"
        "Center Right", "weave-dropdownmenu--anchor-origin-center-right"
        "Bottom Left", "weave-dropdownmenu--anchor-origin-bottom-left"
        "Bottom Center", "weave-dropdownmenu--anchor-origin-bottom-center"
        "Bottom Right", "weave-dropdownmenu--anchor-origin-bottom-right"
      ]

    let transformClassMap =
      Map.ofList [
        "Top Left", "weave-dropdownmenu--transform-origin-top-left"
        "Top Center", "weave-dropdownmenu--transform-origin-top-center"
        "Top Right", "weave-dropdownmenu--transform-origin-top-right"
        "Center Left", "weave-dropdownmenu--transform-origin-center-left"
        "Center Center", "weave-dropdownmenu--transform-origin-center-center"
        "Center Right", "weave-dropdownmenu--transform-origin-center-right"
        "Bottom Left", "weave-dropdownmenu--transform-origin-bottom-left"
        "Bottom Center", "weave-dropdownmenu--transform-origin-bottom-center"
        "Bottom Right", "weave-dropdownmenu--transform-origin-bottom-right"
      ]

    let radioGroup (classMap: Map<string, string>) selected colorAttr =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] [
        yield!
          classMap
          |> Map.keys
          |> Seq.toList
          |> List.map (fun label ->
            Radio.create (
              selected,
              label,
              displayText = View.Const label,
              attrs = [ colorAttr; Margin.Bottom.extraSmall ]
            ))
      ]

    let description =
      Helpers.bodyText
        "Dropdowns can be positioned using both anchor origin and transform origin. Pass the style modules via attrs for static placement, or use Attr.classSelection for reactive selection."

    let content =
      div [] [
        alertDialog alertVar
        Grid.create (
          [
            GridItem.create (
              div [] [
                div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Anchor Origin" ]
                radioGroup anchorClassMap anchorVar Radio.Color.secondary
              ],
              attrs = [ GridItem.Span.six ]
            )

            GridItem.create (
              div [] [
                div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Transform Origin" ]
                radioGroup transformClassMap transformVar Radio.Color.tertiary
              ],
              attrs = [ GridItem.Span.six ]
            )

            GridItem.create (
              Grid.create (
                [
                  GridItem.create (
                    DropdownMenu.create (
                      triggerContent = text "Placement",
                      items = View.Const items,
                      closeOnOutsideClick = false,
                      attrs = [
                        anchorClassMap |> Attr.classSelection anchorVar.View
                        transformClassMap |> Attr.classSelection transformVar.View
                        Margin.Top.large
                      ],
                      triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
                    ),
                    attrs = [ GridItem.Span.twelve ]
                  )
                ],
                attrs = [ JustifyContent.center; AlignItems.center; AlignContent.center ]
              ),
              attrs = [ GridItem.Span.ten; Margin.Top.small ]
            )

          ]
        )
      ]

    let code =
      """open Weave

// Static placement — pass style modules directly via attrs
DropdownMenu.create(
    triggerContent = text "Below Right",
    items = View.Const items,
    attrs = [
        DropdownMenu.AnchorOrigin.bottomRight
        DropdownMenu.TransformOrigin.topRight
    ],
    triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
)

// Reactive placement — use Attr.classSelection with a Map<'T, string>
let anchorVar = Var.Create "Bottom Left"

let anchorClassMap =
    Map.ofList [
        "Top Left", "weave-dropdownmenu--anchor-origin-top-left"
        "Bottom Left", "weave-dropdownmenu--anchor-origin-bottom-left"
        // ...
    ]

DropdownMenu.create(
    triggerContent = text "Placement",
    items = View.Const items,
    attrs = [
        anchorClassMap |> Attr.classSelection anchorVar.View
    ],
    triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
)"""

    Helpers.codeSampleSection "Placement" description content code

  let private openOnExamples () =
    let alertVar = Var.Create None

    let clickableDropdown =
      let items = [ 1..3 ] |> List.map (clickableItem alertVar)

      DropdownMenu.create (
        triggerContent = text "Click to Open",
        items = View.Const items,
        openOn = DropdownMenu.OpenOn.Click,
        attrs = [ Attr.Style "width" "100%" ],
        triggerAttrs = [ Button.Variant.filled; Button.Color.primary; Button.Width.full ]
      )

    let hoverDropdown =
      let items = [ 1..3 ] |> List.map (clickableItem alertVar)

      DropdownMenu.create (
        triggerContent = text "Hover to Open",
        items = View.Const items,
        openOn = DropdownMenu.OpenOn.Hover,
        attrs = [ Attr.Style "width" "100%" ],
        triggerAttrs = [ Button.Variant.filled; Button.Color.secondary; Button.Width.full ]
      )

    let description =
      Helpers.bodyText
        "The openOn parameter controls whether the dropdown opens on click or hover. The default is Click."

    let content =
      div [] [
        alertDialog alertVar
        Grid.create (
          [
            GridItem.create (clickableDropdown, attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ])
            GridItem.create (hoverDropdown, attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ])
          ]
        )
      ]

    let code =
      """open Weave

DropdownMenu.create(
    triggerContent = text "Click to Open",
    items = View.Const items,
    openOn = DropdownMenu.OpenOn.Click,  // see here (this is the default)
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)

DropdownMenu.create(
    triggerContent = text "Hover to Open",
    items = View.Const items,
    openOn = DropdownMenu.OpenOn.Hover,  // see here
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.secondary
    ]
)"""

    Helpers.codeSampleSection "OpenOn Property" description content code

  let private disabledExample () =
    let alertVar = Var.Create None

    let items = [
      DropdownMenuItem.create (text "Edit", onClick = (fun () -> Var.Set alertVar (Some "Clicked Edit")))

      DropdownMenuItem.create (
        text "Duplicate",
        onClick = (fun () -> Var.Set alertVar (Some "Clicked Duplicate"))
      )

      DropdownMenuItem.divider () // see here

      DropdownMenuItem.create (
        text "Delete (disabled)",
        onClick = (fun () -> Var.Set alertVar (Some "Clicked Delete")),
        disabled = View.Const true // see here
      )
    ]

    let description =
      Helpers.bodyText
        "Individual items can be disabled with the disabled parameter. Use DropdownMenuItem.divider() to insert visual separators between groups."

    let content =
      div [] [
        alertDialog alertVar
        DropdownMenu.create (
          triggerContent = text "Dropdown",
          items = View.Const items,
          attrs = [ Margin.Bottom.extraSmall ],
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
        )
      ]

    let code =
      """open Weave

let items = [
    DropdownMenuItem.create(
        text "Edit",
        onClick = (fun () -> printfn "Edit")
    )

    DropdownMenuItem.create(
        text "Duplicate",
        onClick = (fun () -> printfn "Duplicate")
    )

    DropdownMenuItem.divider()  // see here — visual separator

    DropdownMenuItem.create(
        text "Delete (disabled)",
        onClick = (fun () -> printfn "Delete"),
        disabled = View.Const true  // see here
    )
]

DropdownMenu.create(
    triggerContent = text "Dropdown",
    items = View.Const items,
    triggerAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Disabled Items and Dividers" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding and minimum height of dropdown items on a three-step scale: Compact, Standard, and Spacious. Pass the density class on the dropdown's root element via attrs."

    let content =
      let col (label: string) densityAttr =
        let items =
          [ 1..3 ]
          |> List.map (fun n ->
            DropdownMenuItem.create (text (sprintf "Item %d" n), onClick = (fun () -> ())))

        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          DropdownMenu.create (
            triggerContent = text "Open",
            items = View.Const items,
            triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]

      Grid.create (
        [
          GridItem.create (
            col "Compact" Density.compact,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Standard" Density.standard,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
        ],
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave

// Per-instance: pass the density class in attrs to set it on the dropdown root
DropdownMenu.create(
    triggerContent = text "Compact",
    items = View.Const items,
    attrs = [ Density.compact ]
)

DropdownMenu.create(
    triggerContent = text "Standard",
    items = View.Const items,
    attrs = [ Density.standard ]
)

DropdownMenu.create(
    triggerContent = text "Spacious",
    items = View.Const items,
    attrs = [ Density.spacious ]
)"""

    Helpers.codeSampleSection "Density" description content code

  let private whenToUseSection () =
    let description =
      div [ Typography.body1 ] [
        text "Both "
        Helpers.inlineCode "DropdownMenu"
        text " and "
        Helpers.inlineCode "ButtonMenu"
        text
          " reveal a set of actions from a trigger — but they differ in interaction style and layout. Use this to pick the right one."
      ]

    let content =
      Helpers.guidanceColumns
        (Helpers.guidanceCard "Use DropdownMenu when\u2026" [
          Helpers.guidanceBullet
            "Items are a flat list of actions"
            "edit, delete, share — standard menu actions triggered by onClick."
          Helpers.guidanceBullet
            "You need precise positioning control"
            "anchorOrigin and transformOrigin give 9-point placement."
          Helpers.guidanceBullet
            "Standard click-to-open behavior"
            "familiar menu pattern that works on all devices."
        ])
        (Helpers.guidanceCard "Use ButtonMenu when\u2026" [
          Helpers.guidanceBullet
            "The trigger animates between states"
            "the button content morphs between open and closed icons."
          Helpers.guidanceBullet
            "Items fan out directionally"
            "top, bottom, left, or right from the trigger button."
          Helpers.guidanceBullet
            "Hover-open behavior is needed"
            "openOnHover enables menus that appear without clicking."
          Helpers.guidanceBullet
            "The trigger is a FAB-like action button"
            "circular icon buttons that reveal a radial menu."
        ])

    Helpers.sectionPlain "When to Use" description content

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for DropdownMenu and DropdownMenuItem.") [
      Helpers.apiTable "DropdownMenu.create" [
        Helpers.apiParam "triggerContent" "Doc" "" "Content displayed inside the trigger button"
        Helpers.apiParam
          "items"
          "View<DropdownMenuItemKind list>"
          ""
          "Reactive list of menu items. Use View.Const for static lists"
        Helpers.apiParam "?isOpen" "Var<bool>" "" "External two-way binding for the open/closed state"
        Helpers.apiParam "?openOn" "OpenOn" "Click" "Whether the menu opens on Click or Hover"
        Helpers.apiParam
          "?enabled"
          "View<bool>"
          "View.Const true"
          "Whether the dropdown menu trigger is interactive"
        Helpers.apiParam "?closeOnOutsideClick" "bool" "true" "Close the menu when clicking outside"
        Helpers.apiParam
          "?triggerAttrs"
          "Attr list"
          "[]"
          "Additional attributes applied to the trigger button"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (Color, Width, AnchorOrigin, TransformOrigin, etc.) applied to the root container"
      ]

      Helpers.returnTypeNote
        "DropdownMenuItem.create returns a DropdownMenu.DropdownMenuItemKind, not a Doc. Build items with DropdownMenuItem.create and pass them as a reactive list to DropdownMenu.create."

      Helpers.apiTable "DropdownMenuItem.create" [
        Helpers.apiParam "content" "Doc" "" "Visual content displayed inside the menu item"
        Helpers.apiParam "onClick" "unit -> unit" "" "Callback invoked when the item is activated"
        Helpers.apiParam "?text" "string" "\"\"" "Accessible text for the item, used for type-ahead search"
        Helpers.apiParam "?icon" "Doc" "" "Optional leading icon Doc"
        Helpers.apiParam
          "?disabled"
          "View<bool>"
          "View.Const false"
          "Reactive view controlling the disabled state"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the item element"
      ]

      Helpers.styleModuleTable "DropdownMenuItem.divider (static method)" [
        ("divider()", "Creates a visual separator between groups of menu items")
      ]

      Helpers.styleModuleTable "DropdownMenu.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/danger color")
        ("warning", "Warning color")
        ("success", "Success color")
        ("info", "Informational color")
      ]

      Helpers.styleModuleTable "DropdownMenu.Width" [
        ("full", "Menu stretches to 100% of the container")
        ("fitContent", "Menu sizes to the widest item")
      ]

      Helpers.styleModuleTable "DropdownMenu.AnchorOrigin" [
        ("topLeft", "Anchor at top-left of trigger")
        ("topCenter", "Anchor at top-center of trigger")
        ("topRight", "Anchor at top-right of trigger")
        ("centerLeft", "Anchor at center-left of trigger")
        ("center", "Anchor at center of trigger")
        ("centerRight", "Anchor at center-right of trigger")
        ("bottomLeft", "Anchor at bottom-left of trigger")
        ("bottomCenter", "Anchor at bottom-center of trigger")
        ("bottomRight", "Anchor at bottom-right of trigger")
      ]

      Helpers.styleModuleTable "DropdownMenu.TransformOrigin" [
        ("topLeft", "Menu aligns its top-left to the anchor")
        ("topCenter", "Menu aligns its top-center to the anchor")
        ("topRight", "Menu aligns its top-right to the anchor")
        ("centerLeft", "Menu aligns its center-left to the anchor")
        ("center", "Menu aligns its center to the anchor")
        ("centerRight", "Menu aligns its center-right to the anchor")
        ("bottomLeft", "Menu aligns its bottom-left to the anchor")
        ("bottomCenter", "Menu aligns its bottom-center to the anchor")
        ("bottomRight", "Menu aligns its bottom-right to the anchor")
      ]

      Helpers.styleModuleTable "DropdownMenu.OpenOn (DU)" [
        ("Click", "Menu opens/closes on button click (default)")
        ("Hover", "Menu opens on mouse enter, closes on mouse leave")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Dropdown Menu"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Dropdown menus allow users to select an action from a list. They can be customized with colors, width modes, anchor origins, and disabled items."
        ]

        Helpers.divider ()
        whenToUseSection ()
        Helpers.divider ()
        basicDropdownExample ()
        Helpers.divider ()
        colorExample ()
        Helpers.divider ()
        widthExample ()
        Helpers.divider ()
        placementExample ()
        Helpers.divider ()
        openOnExamples ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        densityExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
