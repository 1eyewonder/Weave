namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DropdownExamples =

  let clickableItem (alertVar: Var<string option>) n =
    DropdownItem.create (
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
        Dropdown.create (
          buttonContents = text "Open Dropdown",
          items = items,
          attrs = [ Margin.Bottom.extraSmall ],
          buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
        )
      ]

    let code =
      """open Weave

Dropdown.create(
    buttonContents = text "Open Dropdown",
    items = [
        DropdownItem.create(text "Edit", onClick = (fun () -> printfn "Edit"))
        DropdownItem.create(text "Duplicate", onClick = (fun () -> printfn "Duplicate"))
        DropdownItem.create(text "Delete", onClick = (fun () -> printfn "Delete"))
    ],
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private placementExample () =
    let alertVar = Var.Create None
    let anchorVar = Var.Create "Bottom Left"
    let transformVar = Var.Create "Top Left"

    let items = [ 1..3 ] |> List.map (clickableItem alertVar)

    let anchorClassMap =
      Map.ofList [
        "Top Left", "weave-dropdown--anchor-origin-top-left"
        "Top Center", "weave-dropdown--anchor-origin-top-center"
        "Top Right", "weave-dropdown--anchor-origin-top-right"
        "Center Left", "weave-dropdown--anchor-origin-center-left"
        "Center Center", "weave-dropdown--anchor-origin-center-center"
        "Center Right", "weave-dropdown--anchor-origin-center-right"
        "Bottom Left", "weave-dropdown--anchor-origin-bottom-left"
        "Bottom Center", "weave-dropdown--anchor-origin-bottom-center"
        "Bottom Right", "weave-dropdown--anchor-origin-bottom-right"
      ]

    let transformClassMap =
      Map.ofList [
        "Top Left", "weave-dropdown--transform-origin-top-left"
        "Top Center", "weave-dropdown--transform-origin-top-center"
        "Top Right", "weave-dropdown--transform-origin-top-right"
        "Center Left", "weave-dropdown--transform-origin-center-left"
        "Center Center", "weave-dropdown--transform-origin-center-center"
        "Center Right", "weave-dropdown--transform-origin-center-right"
        "Bottom Left", "weave-dropdown--transform-origin-bottom-left"
        "Bottom Center", "weave-dropdown--transform-origin-bottom-center"
        "Bottom Right", "weave-dropdown--transform-origin-bottom-right"
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
                    Dropdown.create (
                      buttonContents = text "Placement",
                      items = items,
                      closeOnOutsideClick = View.Const false,
                      attrs = [
                        anchorClassMap |> Attr.classSelection anchorVar.View
                        transformClassMap |> Attr.classSelection transformVar.View
                        Margin.Top.large
                      ],
                      buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
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
Dropdown.create(
    buttonContents = text "Below Right",
    items = items,
    attrs = [
        Dropdown.AnchorOrigin.bottomRight
        Dropdown.TransformOrigin.topRight
    ],
    buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
)

// Reactive placement — use Attr.classSelection with a Map<'T, string>
let anchorVar = Var.Create "Bottom Left"

let anchorClassMap =
    Map.ofList [
        "Top Left", "weave-dropdown--anchor-origin-top-left"
        "Bottom Left", "weave-dropdown--anchor-origin-bottom-left"
        // ...
    ]

Dropdown.create(
    buttonContents = text "Placement",
    items = items,
    attrs = [
        anchorClassMap |> Attr.classSelection anchorVar.View
    ],
    buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
)"""

    Helpers.codeSampleSection "Placement" description content code

  let private nestedDropdownExample () =
    let alertVar = Var.Create None
    let nestedIsOpen = Var.Create false

    let nestedDropdownUnstyled =
      NestedDropdown.create (
        buttonContents = text "Open Nested",
        items = [
          DropdownItem.create (
            text "Nested Item 1",
            onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 1"))
          )

          DropdownItem.create (
            text "Nested Item 2",
            onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 2"))
          )
        ]
      )

    let nestedDropdownStyled =
      NestedDropdown.create (
        buttonContents = text "Open Nested",
        items = [
          DropdownItem.create (
            text "Nested Item 1",
            onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 1"))
          )

          DropdownItem.create (
            text "Nested Item 2",
            onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 2"))
          )
        ],
        isOpen = nestedIsOpen,
        buttonAttrs = [
          nestedIsOpen.View |> Attr.DynamicClassPred "weave-button--tertiary"

          nestedIsOpen.View |> Attr.DynamicClassPred "weave-button--outlined"
        ]
      )

    let items = [ clickableItem alertVar 1; clickableItem alertVar 2; nestedDropdownUnstyled ]
    let styledItems = [ clickableItem alertVar 1; clickableItem alertVar 2; nestedDropdownStyled ]

    let description =
      Helpers.bodyText
        "Dropdowns can contain nested dropdown menus. There are no default stylings for nested dropdowns, however utilizing the `isOpen` parameter, you can build reactive styling based on if the given menu is open."

    let content =
      div [] [
        alertDialog alertVar
        Grid.create (
          [
            GridItem.create (
              Dropdown.create (
                buttonContents = text "Dropdown w/o Styling",
                items = items,
                buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
              )
            )

            GridItem.create (
              Dropdown.create (
                buttonContents = text "Dropdown w/ Styling",
                items = styledItems,
                buttonAttrs = [ Button.Variant.filled; Button.Color.secondary ]
              )
            )
          ]
        )
      ]

    let code =
      """open Weave


let nestedIsOpen = Var.Create false

let nestedDropdown =
    NestedDropdown.create(
        buttonContents = text "Open Nested",
        items = [
            DropdownItem.create(
                text "Nested Item 1",
                onClick = (fun () -> printfn "Clicked Nested Item 1")
            )
            DropdownItem.create(
                text "Nested Item 2",
                onClick = (fun () -> printfn "Clicked Nested Item 2")
            )
        ],
        isOpen = nestedIsOpen,
        buttonAttrs = [
            nestedIsOpen.View
            |> Attr.DynamicClassPred "weave-button--tertiary"

            nestedIsOpen.View
            |> Attr.DynamicClassPred "weave-button--outlined"
        ]
    )

Dropdown.create(
    buttonContents = text "Dropdown w/ Styling",
    items = [ clickableItem 1; clickableItem 2; nestedDropdown ],
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.secondary
    ]
)
"""

    Helpers.codeSampleSection "Nested Dropdowns" description content code

  let private openOnExamples () =
    let alertVar = Var.Create None

    let clickableDropdown =
      let items = [ 1..3 ] |> List.map (clickableItem alertVar)

      Dropdown.create (
        buttonContents = text "Click to Open",
        items = items,
        openOn = View.Const Dropdown.OpenOn.Click,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [ Button.Variant.filled; Button.Color.primary; Button.Width.full ]
      )

    let hoverDropdown =
      let items = [ 1..3 ] |> List.map (clickableItem alertVar)

      Dropdown.create (
        buttonContents = text "Hover to Open",
        items = items,
        openOn = View.Const Dropdown.OpenOn.Hover,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [ Button.Variant.filled; Button.Color.secondary; Button.Width.full ]
      )

    let nestedClickableDropdown =
      let nestedIsOpen = Var.Create false

      let nestedDropdown =
        NestedDropdown.create (
          buttonContents = text "Open Nested",
          items = [
            DropdownItem.create (
              text "Nested Item 1",
              onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 1"))
            )
            DropdownItem.create (
              text "Nested Item 2",
              onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 2"))
            )
          ],
          isOpen = nestedIsOpen,
          buttonAttrs = [ nestedIsOpen.View |> Attr.DynamicClassPred "weave-button--primary" ]
        )

      Dropdown.create (
        buttonContents = text "Click to Open Nested",
        items = [ clickableItem alertVar 1; nestedDropdown ],
        openOn = View.Const Dropdown.OpenOn.Click,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [ Button.Variant.filled; Button.Color.tertiary; Button.Width.full ]
      )

    let nestedHoverDropdown =
      let nestedIsOpen = Var.Create false

      let nestedDropdown =
        NestedDropdown.create (
          buttonContents = text "Open Nested",
          items = [
            DropdownItem.create (
              text "Nested Item 1",
              onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 1"))
            )
            DropdownItem.create (
              text "Nested Item 2",
              onClick = (fun () -> Var.Set alertVar (Some "Clicked Nested Item 2"))
            )
          ],
          isOpen = nestedIsOpen,
          openOn = View.Const Dropdown.OpenOn.Hover,
          buttonAttrs = [ nestedIsOpen.View |> Attr.DynamicClassPred "weave-button--primary" ]
        )

      Dropdown.create (
        buttonContents = text "Hover to Open Nested",
        items = [ clickableItem alertVar 1; nestedDropdown ],
        openOn = View.Const Dropdown.OpenOn.Hover,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [ Button.Variant.filled; Button.Color.primary; Button.Width.full ]
      )

    let description =
      Helpers.bodyText
        "The OpenOn property allows you to specify whether the dropdown opens on click or hover. The default behavior for the Dropdown and NestedDropdown component is to open on click. You can override this by specifying the OpenOn property when creating either component."

    let content =
      div [] [
        alertDialog alertVar
        Grid.create (
          [
            GridItem.create (
              clickableDropdown,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
            )
            GridItem.create (
              hoverDropdown,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
            )
            GridItem.create (
              nestedClickableDropdown,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
            )
            GridItem.create (
              nestedHoverDropdown,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
            )
          ]
        )
      ]

    let code =
      """open Weave


Dropdown.create(
    buttonContents = text "Click to Open",
    items = items,
    openOn = View.Const Dropdown.OpenOn.Click,
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)

Dropdown.create(
    buttonContents = text "Hover to Open",
    items = items,
    openOn = View.Const Dropdown.OpenOn.Hover,
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.secondary
    ]
)

NestedDropdown.create(
    buttonContents = text "Open Nested",
    items = nestedItems,
    openOn = View.Const Dropdown.OpenOn.Hover,
    buttonAttrs = [
        nestedIsOpen.View
        |> Attr.DynamicClassPred "weave-button--primary"
    ]
)
"""

    Helpers.codeSampleSection "OpenOn Property" description content code

  let private disabledExample () =
    let alertVar = Var.Create None

    let items = [
      DropdownItem.create (
        text "Enabled Item",
        onClick = (fun () -> Var.Set alertVar (Some "Clicked enabled item")),
        enabled = View.Const true
      )

      DropdownItem.create (
        text "Disabled Item",
        onClick = (fun () -> Var.Set alertVar (Some "Clicked disabled item")),
        enabled = View.Const false
      )
    ]

    let description =
      Helpers.bodyText "Dropdown items can be disabled using the `enabled` property."

    let content =
      div [] [
        alertDialog alertVar
        Dropdown.create (
          buttonContents = text "Dropdown",
          items = items,
          attrs = [ Margin.Bottom.extraSmall ],
          buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
        )
      ]

    let code =
      """open Weave


let items = [
    DropdownItem.create(
        text "Enabled Item",
        onClick = (fun () -> printfn "Clicked enabled item"),
        enabled = View.Const true
    )

    DropdownItem.create(
        text "Disabled Item",
        onClick = (fun () -> printfn "Clicked disabled item"),
        enabled = View.Const false
    )
]

Dropdown.create(
    buttonContents = text "Dropdown",
    items = items,
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Disabled Items" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding and minimum height of dropdown items on a three-step scale: Compact, Standard, and Spacious. Pass the density class on the dropdown's root element via attrs."

    let content =
      let col (label: string) densityAttr =
        let items =
          [ 1..3 ]
          |> List.map (fun n -> DropdownItem.create (text (sprintf "Item %d" n), onClick = (fun () -> ())))

        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          Dropdown.create (
            buttonContents = text "Open",
            items = items,
            buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
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
Dropdown.create(
    buttonContents = text "Compact",
    items = items,
    attrs = [ Density.compact ]
)

Dropdown.create(
    buttonContents = text "Standard",
    items = items,
    attrs = [ Density.standard ]
)

Dropdown.create(
    buttonContents = text "Spacious",
    items = items,
    attrs = [ Density.spacious ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let private whenToUseSection () =
    let description =
      div [ Typography.body1 ] [
        text "Both "
        Helpers.inlineCode "Dropdown"
        text " and "
        Helpers.inlineCode "ButtonMenu"
        text
          " reveal a set of actions from a trigger — but they differ in interaction style and layout. Use this to pick the right one."
      ]

    let content =
      Helpers.guidanceColumns
        (Helpers.guidanceCard "Use Dropdown when\u2026" [
          Helpers.guidanceBullet
            "Items are a flat list of actions"
            "edit, delete, share — standard menu actions triggered by onClick."
          Helpers.guidanceBullet
            "You need precise positioning control"
            "anchorOrigin and transformOrigin give 9-point placement."
          Helpers.guidanceBullet
            "Nested submenus are required"
            "NestedDropdown supports hierarchical menu structures."
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
    Helpers.apiSection
      (Helpers.bodyText "Complete API reference for Dropdown, DropdownItem, and NestedDropdown.")
      [
        Helpers.apiTable "Dropdown.create" [
          Helpers.apiParam "buttonContents" "Doc" "" "Content displayed inside the trigger button"
          Helpers.apiParam
            "items"
            "Doc seq"
            ""
            "Sequence of menu item Docs (use DropdownItem.create for styled items)"
          Helpers.apiParam "?isOpen" "Var<bool>" "" "External control over the dropdown open/closed state"
          Helpers.apiParam
            "?openOn"
            "View<OpenOn>"
            "View.Const Click"
            "Whether the menu opens on Click or Hover"
          Helpers.apiParam
            "?enabled"
            "View<bool>"
            "View.Const true"
            "Whether the dropdown trigger is interactive"
          Helpers.apiParam
            "?closeOnOutsideClick"
            "View<bool>"
            "View.Const true"
            "Close the menu when clicking outside"
          Helpers.apiParam
            "?buttonAttrs"
            "Attr list"
            "[]"
            "Additional attributes applied to the trigger button"
          Helpers.apiParam
            "?attrs"
            "Attr list"
            "[]"
            "Additional attributes (AnchorOrigin, TransformOrigin, etc.) applied to the root container"
        ]

        Helpers.apiTable "DropdownItem.create" [
          Helpers.apiParam "innerContents" "Doc" "" "Content displayed inside the menu item"
          Helpers.apiParam "onClick" "unit -> unit" "" "Callback invoked when the item is clicked"
          Helpers.apiParam "?enabled" "View<bool>" "" "Whether this item is interactive"
          Helpers.apiParam "?attrs" "Attr list" "" "Additional attributes applied to the item"
        ]

        Helpers.apiTable "NestedDropdown.create" [
          Helpers.apiParam "buttonContents" "Doc" "" "Content displayed inside the nested trigger button"
          Helpers.apiParam "items" "Doc seq" "" "Sequence of submenu item Docs"
          Helpers.apiParam "?isOpen" "Var<bool>" "" "External control over the submenu open/closed state"
          Helpers.apiParam
            "?openOn"
            "View<OpenOn>"
            "View.Const Click"
            "Whether the submenu opens on Click or Hover"
          Helpers.apiParam "?enabled" "View<bool>" "" "Whether the nested trigger is interactive"
          Helpers.apiParam
            "?buttonAttrs"
            "Attr list"
            ""
            "Additional attributes applied to the nested trigger button"
          Helpers.apiParam
            "?attrs"
            "Attr list"
            ""
            "Additional attributes applied to the nested container (includes AnchorOrigin.topRight by default)"
        ]

        Helpers.styleModuleTable "Dropdown.AnchorOrigin" [
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

        Helpers.styleModuleTable "Dropdown.TransformOrigin" [
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

        Helpers.styleModuleTable "Dropdown.OpenOn (DU)" [
          ("Click", "Menu opens/closes on button click (default)")
          ("Hover", "Menu opens on mouse enter, closes on mouse leave")
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Dropdown"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Dropdowns allow users to select an option from a list. They can be customized with anchor origins, nested menus, and disabled items."
        ]

        Helpers.divider ()
        whenToUseSection ()
        Helpers.divider ()
        basicDropdownExample ()
        Helpers.divider ()
        placementExample ()
        Helpers.divider ()
        nestedDropdownExample ()
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
