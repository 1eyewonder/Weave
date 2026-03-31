namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ButtonGroupExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText "Buttons can be grouped together in Filled, Outlined, and Text variants."

    let content =
      Grid.create (
        [
          let makeGroup variant =
            GridItem.create (
              ButtonGroup.create (
                [
                  Button.create (text "One", onClick = (fun () -> ()))
                  Button.create (text "Two", onClick = (fun () -> ()))
                  Button.create (text "Three", onClick = (fun () -> ()))
                ],
                attrs = [ variant; ButtonGroup.Color.primary ]
              )
            )

          makeGroup ButtonGroup.Variant.filled
          makeGroup ButtonGroup.Variant.text
          makeGroup ButtonGroup.Variant.outlined
        ],
        attrs = [ AlignItems.center ]
      )

    let code =
      """open Weave

ButtonGroup.create(
    [
        Button.create(text "One", onClick = (fun () -> ()))
        Button.create(text "Two", onClick = (fun () -> ()))
        Button.create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.filled
        ButtonGroup.Color.primary
    ]
)

ButtonGroup.create(
    [
        Button.create(text "One", onClick = (fun () -> ()))
        Button.create(text "Two", onClick = (fun () -> ()))
        Button.create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.text
        ButtonGroup.Color.primary
    ]
)

ButtonGroup.create(
    [
        Button.create(text "One", onClick = (fun () -> ()))
        Button.create(text "Two", onClick = (fun () -> ()))
        Button.create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.outlined
        ButtonGroup.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Basic Button Group" description content code

  let private verticalExample () =
    let description =
      Helpers.bodyText "With Vertical orientation the buttons are displayed vertically."

    let content =
      Grid.create (
        [
          let makeGroup variant =
            GridItem.create (
              ButtonGroup.create (
                [
                  Button.create (text "One", onClick = (fun () -> ()))
                  Button.create (text "Two", onClick = (fun () -> ()))
                  Button.create (text "Three", onClick = (fun () -> ()))
                ],
                attrs = [ variant; ButtonGroup.Color.primary; ButtonGroup.Orientation.vertical ]
              )
            )

          makeGroup ButtonGroup.Variant.filled
          makeGroup ButtonGroup.Variant.text
          makeGroup ButtonGroup.Variant.outlined
        ],
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave

ButtonGroup.create(
    [
        Button.create(text "One", onClick = (fun () -> ()))
        Button.create(text "Two", onClick = (fun () -> ()))
        Button.create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.filled
        ButtonGroup.Color.primary
        ButtonGroup.Orientation.vertical // see here
    ]
)"""

    Helpers.codeSampleSection "Vertical Orientation" description content code

  let private densityAndColorsExample () =
    let description =
      Helpers.bodyText
        "Button groups support all theme colors and three density levels: Compact, Standard, and Spacious."

    let content =
      Grid.create (
        [
          let makeGroup densityAttr colorAttr =
            GridItem.create (
              ButtonGroup.create (
                [
                  Button.create (text "One", onClick = (fun () -> ()))
                  Button.create (text "Two", onClick = (fun () -> ()))
                  Button.create (text "Three", onClick = (fun () -> ()))
                ],
                attrs = [ ButtonGroup.Variant.filled; densityAttr; colorAttr ]
              )
            )

          makeGroup ButtonGroup.Density.compact ButtonGroup.Color.primary
          makeGroup ButtonGroup.Density.standard ButtonGroup.Color.error
          makeGroup ButtonGroup.Density.spacious ButtonGroup.Color.success
        ],
        attrs = [ AlignItems.center ]
      )

    let code =
      """open Weave

ButtonGroup.create(
    [
        Button.create(text "One", onClick = (fun () -> ()))
        Button.create(text "Two", onClick = (fun () -> ()))
        Button.create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.filled
        ButtonGroup.Density.compact
        ButtonGroup.Color.primary
    ]
)

ButtonGroup.create(
    [
        Button.create(text "One", onClick = (fun () -> ()))
        Button.create(text "Two", onClick = (fun () -> ()))
        Button.create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.filled
        ButtonGroup.Density.standard
        ButtonGroup.Color.error
    ]
)

ButtonGroup.create(
    [
        Button.create(text "One", onClick = (fun () -> ()))
        Button.create(text "Two", onClick = (fun () -> ()))
        Button.create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.filled
        ButtonGroup.Density.spacious
        ButtonGroup.Color.success
    ]
)"""

    Helpers.codeSampleSection "Density and Colors" description content code

  let private splitButtonExample () =
    let description =
      Helpers.bodyText
        "A ButtonMenu can be placed inside a button group to create a split button. The group wraps around both the action button and the dropdown trigger."

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.center; AlignItems.center ] [
        let isOpen = Var.Create false

        ButtonGroup.create (
          [
            Button.create (text "Reply", onClick = (fun () -> printfn "Reply clicked"))
            IconButtonMenu.create (
              closedIcon = Icon.create (Icon.UiActions UiActions.ArrowDropDown),
              items = [
                Button.create (
                  text "Reply",
                  onClick = (fun () -> printfn "Reply"),
                  attrs = [ Button.Variant.filled; Button.Width.full ]
                )
                Button.create (
                  text "Reply All",
                  onClick = (fun () -> printfn "Reply All"),
                  attrs = [ Button.Variant.filled; Button.Width.full ]
                )
                Button.create (
                  text "Forward",
                  onClick = (fun () -> printfn "Forward"),
                  attrs = [ Button.Variant.filled; Button.Width.full ]
                )
              ],
              direction = ButtonMenu.Direction.Bottom,
              isOpen = isOpen,
              triggerAttrs = []
            )
          ],
          attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols
open WebSharper.UI

let isOpen = Var.Create false

ButtonGroup.create(
    [
        Button.create(text "Reply", onClick = (fun () -> printfn "Reply clicked"))
        IconButtonMenu.create(
            closedIcon = Icon.create(Icon.UiActions UiActions.ArrowDropDown),
            items = [
                Button.create(
                    text "Reply",
                    onClick = (fun () -> printfn "Reply"),
                    attrs = [ Button.Variant.filled; Button.Width.full ]
                )
                Button.create(
                    text "Reply All",
                    onClick = (fun () -> printfn "Reply All"),
                    attrs = [ Button.Variant.filled; Button.Width.full ]
                )
                Button.create(
                    text "Forward",
                    onClick = (fun () -> printfn "Forward"),
                    attrs = [ Button.Variant.filled; Button.Width.full ]
                )
            ],
            direction = ButtonMenu.Direction.Bottom,
            isOpen = isOpen,
            triggerAttrs = []
        )
    ],
    attrs = [
        ButtonGroup.Variant.outlined
        ButtonGroup.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Split Button" description content code

  let private iconButtonExample () =
    let description = Helpers.bodyText "Icon buttons can also be grouped together."

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.center; AlignItems.center ] [
        ButtonGroup.create (
          [
            IconButton.create (Icon.create (Icon.Text Text.FormatBold), onClick = (fun () -> ()))
            IconButton.create (Icon.create (Icon.Text Text.FormatItalic), onClick = (fun () -> ()))
            IconButton.create (Icon.create (Icon.Text Text.FormatUnderlined), onClick = (fun () -> ()))
          ],
          attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

ButtonGroup.create(
    [
        IconButton.create(Icon.create(Icon.Text Text.FormatBold), onClick = (fun () -> ()))
        IconButton.create(Icon.create(Icon.Text Text.FormatItalic), onClick = (fun () -> ()))
        IconButton.create(Icon.create(Icon.Text Text.FormatUnderlined), onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.outlined
        ButtonGroup.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Icon Buttons" description content code

  let private dropdownExample () =
    let description =
      Helpers.bodyText
        "A Dropdown can be placed inside a button group to create a split button with a dropdown menu."

    let content =
      div [ Flex.Flex.allSizes; JustifyContent.center; AlignItems.center ] [
        ButtonGroup.create (
          [
            Button.create (text "Save", onClick = (fun () -> printfn "Save clicked"))
            Dropdown.create (
              text "Save Options",
              [
                DropdownItem.create (text "Save as Draft", onClick = (fun () -> printfn "Draft"))
                DropdownItem.create (text "Save and Publish", onClick = (fun () -> printfn "Publish"))
                DropdownItem.create (text "Save and Close", onClick = (fun () -> printfn "Close"))
              ],
              anchorOrigin = View.Const Dropdown.AnchorOrigin.BottomLeft,
              transformOrigin = View.Const Dropdown.TransformOrigin.TopLeft
            )
          ],
          attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

ButtonGroup.create(
    [
        Button.create(text "Save", onClick = (fun () -> printfn "Save clicked"))
        Dropdown.create(
            text "Save Options",
            [
                DropdownItem.create(text "Save as Draft", onClick = (fun () -> printfn "Draft"))
                DropdownItem.create(text "Save and Publish", onClick = (fun () -> printfn "Publish"))
                DropdownItem.create(text "Save and Close", onClick = (fun () -> printfn "Close"))
            ],
            anchorOrigin = View.Const Dropdown.AnchorOrigin.BottomLeft,
            transformOrigin = View.Const Dropdown.TransformOrigin.TopLeft
        )
    ],
    attrs = [
        ButtonGroup.Variant.outlined
        ButtonGroup.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Dropdown" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for ButtonGroup.") [
      Helpers.apiTable "ButtonGroup.create" [
        Helpers.apiParam
          "items"
          "Doc list"
          ""
          "List of child elements (buttons, icon buttons, dropdowns, etc.)"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (variant, orientation, density, color, etc.)"
      ]

      Helpers.styleModuleTable "ButtonGroup.Variant" [
        ("filled", "Solid background buttons")
        ("outlined", "Bordered buttons with transparent background")
        ("text", "Text-only buttons")
      ]

      Helpers.styleModuleTable "ButtonGroup.Orientation" [
        ("vertical", "Stack buttons vertically instead of horizontally")
      ]

      Helpers.styleModuleTable "ButtonGroup.Density" [
        ("compact", "Compact button sizing")
        ("standard", "Standard button sizing (default)")
        ("spacious", "Spacious button sizing")
      ]

      Helpers.styleModuleTable "ButtonGroup.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Button Group"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "ButtonGroup groups buttons, icon buttons, button menus, and dropdowns into a single visual unit with shared variant, color, and size styling."
        ]

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        verticalExample ()
        Helpers.divider ()
        densityAndColorsExample ()
        Helpers.divider ()
        splitButtonExample ()
        Helpers.divider ()
        iconButtonExample ()
        Helpers.divider ()
        dropdownExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
