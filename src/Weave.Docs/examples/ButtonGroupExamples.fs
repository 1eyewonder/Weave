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
      Grid.Create(
        [
          let makeGroup variant =
            GridItem.Create(
              ButtonGroup.Create(
                [
                  Button.Create(text "One", onClick = (fun () -> ()))
                  Button.Create(text "Two", onClick = (fun () -> ()))
                  Button.Create(text "Three", onClick = (fun () -> ()))
                ],
                attrs = [
                  ButtonGroup.Variant.toClass variant |> cl
                  ButtonGroup.Color.toClass BrandColor.Primary |> cl
                ]
              )
            )

          makeGroup ButtonGroup.Variant.Filled
          makeGroup ButtonGroup.Variant.Text
          makeGroup ButtonGroup.Variant.Outlined
        ],
        spacing = Grid.GutterSpacing.create 2,
        justify = JustifyContent.SpaceAround,
        attrs = [ cls [ AlignItems.toClass AlignItems.Center ] ]
      )

    let code =
      """open Weave


ButtonGroup.Create(
    [
        Button.Create(text "One", onClick = (fun () -> ()))
        Button.Create(text "Two", onClick = (fun () -> ()))
        Button.Create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Filled |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl
    ]
)

ButtonGroup.Create(
    [
        Button.Create(text "One", onClick = (fun () -> ()))
        Button.Create(text "Two", onClick = (fun () -> ()))
        Button.Create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Text |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl
    ]
)

ButtonGroup.Create(
    [
        Button.Create(text "One", onClick = (fun () -> ()))
        Button.Create(text "Two", onClick = (fun () -> ()))
        Button.Create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Outlined |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl
    ]
)
    """

    Helpers.codeSampleSection "Basic Button Group" description content code

  let private verticalExample () =
    let description =
      Helpers.bodyText "With Vertical orientation the buttons are displayed vertically."

    let content =
      Grid.Create(
        [
          let makeGroup variant =
            GridItem.Create(
              ButtonGroup.Create(
                [
                  Button.Create(text "One", onClick = (fun () -> ()))
                  Button.Create(text "Two", onClick = (fun () -> ()))
                  Button.Create(text "Three", onClick = (fun () -> ()))
                ],
                attrs = [
                  ButtonGroup.Variant.toClass variant |> cl
                  ButtonGroup.Color.toClass BrandColor.Primary |> cl

                  ButtonGroup.Orientation.toClass ButtonGroup.Orientation.Vertical
                  |> Attr.bindOption cl
                ]
              )
            )

          makeGroup ButtonGroup.Variant.Filled
          makeGroup ButtonGroup.Variant.Text
          makeGroup ButtonGroup.Variant.Outlined
        ],
        spacing = Grid.GutterSpacing.create 2,
        justify = JustifyContent.SpaceAround,
        attrs = [ cls [ AlignItems.toClass AlignItems.Start ] ]
      )

    let code =
      """open Weave


ButtonGroup.Create(
    [
        Button.Create(text "One", onClick = (fun () -> ()))
        Button.Create(text "Two", onClick = (fun () -> ()))
        Button.Create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.toClass variant |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl

        ButtonGroup.Orientation.toClass ButtonGroup.Orientation.Vertical
        |> Attr.bindOption cl
    ]
)
    """

    Helpers.codeSampleSection "Vertical Orientation" description content code

  let private densityAndColorsExample () =
    let description =
      Helpers.bodyText
        "Button groups support all theme colors and three density levels: Compact, Standard, and Spacious."

    let content =
      Grid.Create(
        [
          let makeGroup density color =
            GridItem.Create(
              ButtonGroup.Create(
                [
                  Button.Create(text "One", onClick = (fun () -> ()))
                  Button.Create(text "Two", onClick = (fun () -> ()))
                  Button.Create(text "Three", onClick = (fun () -> ()))
                ],
                attrs = [
                  ButtonGroup.Variant.toClass ButtonGroup.Variant.Filled |> cl
                  ButtonGroup.Density.toClass density |> cl
                  ButtonGroup.Color.toClass color |> cl
                ]
              )
            )

          makeGroup Density.Compact BrandColor.Primary
          makeGroup Density.Standard BrandColor.Error
          makeGroup Density.Spacious BrandColor.Success
        ],
        spacing = Grid.GutterSpacing.create 2,
        justify = JustifyContent.SpaceAround,
        attrs = [ cls [ AlignItems.toClass AlignItems.Center ] ]
      )

    let code =
      """open Weave


ButtonGroup.Create(
    [
        Button.Create(text "One", onClick = (fun () -> ()))
        Button.Create(text "Two", onClick = (fun () -> ()))
        Button.Create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Filled |> cl
        ButtonGroup.Density.toClass Density.Compact |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl
    ]
)

ButtonGroup.Create(
    [
        Button.Create(text "One", onClick = (fun () -> ()))
        Button.Create(text "Two", onClick = (fun () -> ()))
        Button.Create(text "Three", onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Filled |> cl
        ButtonGroup.Density.toClass Density.Spacious |> cl
        ButtonGroup.Color.toClass BrandColor.Success |> cl
    ]
)
    """

    Helpers.codeSampleSection "Density and Colors" description content code

  let private splitButtonExample () =
    let description =
      Helpers.bodyText
        "A ButtonMenu can be placed inside a button group to create a split button. The group wraps around both the action button and the dropdown trigger."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.Center
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        let isOpen = Var.Create false

        ButtonGroup.Create(
          [
            Button.Create(text "Reply", onClick = (fun () -> printfn "Reply clicked"))
            ButtonMenu.CreateIcon(
              closedIcon = Icon.Create(Icon.UiActions UiActions.ArrowDropDown),
              items = [
                Button.Create(
                  text "Reply",
                  onClick = (fun () -> printfn "Reply"),
                  attrs = [
                    Button.Variant.toClass Button.Variant.Filled |> cl
                    Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                  ]
                )
                Button.Create(
                  text "Reply All",
                  onClick = (fun () -> printfn "Reply All"),
                  attrs = [
                    Button.Variant.toClass Button.Variant.Filled |> cl
                    Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                  ]
                )
                Button.Create(
                  text "Forward",
                  onClick = (fun () -> printfn "Forward"),
                  attrs = [
                    Button.Variant.toClass Button.Variant.Filled |> cl
                    Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                  ]
                )
              ],
              direction = ButtonMenu.Direction.Bottom,
              isOpen = isOpen,
              triggerAttrs = []
            )
          ],
          attrs = [
            ButtonGroup.Variant.toClass ButtonGroup.Variant.Outlined |> cl
            ButtonGroup.Color.toClass BrandColor.Primary |> cl
          ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


let isOpen = Var.Create false

ButtonGroup.Create(
    [
        Button.Create(text "Reply", onClick = (fun () -> printfn "Reply clicked"))
        ButtonMenu.CreateIcon(
            closedIcon = Icon.Create(Icon.UiActions UiActions.ArrowDropDown),
            items = [
                Button.Create(
                    text "Reply",
                    onClick = (fun () -> printfn "Reply"),
                    attrs = [
                        Button.Variant.toClass Button.Variant.Filled |> cl
                        Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                    ]
                )
                Button.Create(
                    text "Reply All",
                    onClick = (fun () -> printfn "Reply All"),
                    attrs = [
                        Button.Variant.toClass Button.Variant.Filled |> cl
                        Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                    ]
                )
                Button.Create(
                    text "Forward",
                    onClick = (fun () -> printfn "Forward"),
                    attrs = [
                        Button.Variant.toClass Button.Variant.Filled |> cl
                        Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                    ]
                )
            ],
            direction = ButtonMenu.Direction.Bottom,
            isOpen = isOpen,
            triggerAttrs = []
        )
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Outlined |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl
    ]
)
    """

    Helpers.codeSampleSection "Split Button" description content code

  let private iconButtonExample () =
    let description = Helpers.bodyText "Icon buttons can also be grouped together."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.Center
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        ButtonGroup.Create(
          [
            Button.CreateIcon(Icon.Create(Icon.Text Text.FormatBold), onClick = (fun () -> ()))
            Button.CreateIcon(Icon.Create(Icon.Text Text.FormatItalic), onClick = (fun () -> ()))
            Button.CreateIcon(Icon.Create(Icon.Text Text.FormatUnderlined), onClick = (fun () -> ()))
          ],
          attrs = [
            ButtonGroup.Variant.toClass ButtonGroup.Variant.Outlined |> cl
            ButtonGroup.Color.toClass BrandColor.Primary |> cl
          ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


ButtonGroup.Create(
    [
        Button.CreateIcon(Icon.Create(Icon.Text Text.FormatBold), onClick = (fun () -> ()))
        Button.CreateIcon(Icon.Create(Icon.Text Text.FormatItalic), onClick = (fun () -> ()))
        Button.CreateIcon(Icon.Create(Icon.Text Text.FormatUnderlined), onClick = (fun () -> ()))
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Outlined |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl
    ]
)
    """

    Helpers.codeSampleSection "Icon Buttons" description content code

  let private dropdownExample () =
    let description =
      Helpers.bodyText
        "A Dropdown can be placed inside a button group to create a split button with a dropdown menu."

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          JustifyContent.toClass JustifyContent.Center
          AlignItems.toClass AlignItems.Center
        ]
      ] [
        ButtonGroup.Create(
          [
            Button.Create(text "Save", onClick = (fun () -> printfn "Save clicked"))
            Dropdown.Create(
              text "Save Options",
              [
                DropdownItem.Create(text "Save as Draft", onClick = (fun () -> printfn "Draft"))
                DropdownItem.Create(text "Save and Publish", onClick = (fun () -> printfn "Publish"))
                DropdownItem.Create(text "Save and Close", onClick = (fun () -> printfn "Close"))
              ],
              anchorOrigin = View.Const Dropdown.AnchorOrigin.BottomLeft,
              transformOrigin = View.Const Dropdown.TransformOrigin.TopLeft
            )
          ],
          attrs = [
            ButtonGroup.Variant.toClass ButtonGroup.Variant.Outlined |> cl
            ButtonGroup.Color.toClass BrandColor.Primary |> cl
          ]
        )
      ]

    let code =
      """open Weave


ButtonGroup.Create(
    [
        Button.Create(text "Save", onClick = (fun () -> printfn "Save clicked"))
        Dropdown.Create(
            text "Save Options",
            [
                DropdownItem.Create(text "Save as Draft", onClick = (fun () -> printfn "Draft"))
                DropdownItem.Create(text "Save and Publish", onClick = (fun () -> printfn "Publish"))
                DropdownItem.Create(text "Save and Close", onClick = (fun () -> printfn "Close"))
            ],
            anchorOrigin = View.Const Dropdown.AnchorOrigin.BottomLeft,
            transformOrigin = View.Const Dropdown.TransformOrigin.TopLeft
        )
    ],
    attrs = [
        ButtonGroup.Variant.toClass ButtonGroup.Variant.Outlined |> cl
        ButtonGroup.Color.toClass BrandColor.Primary |> cl
    ]
)
    """

    Helpers.codeSampleSection "Dropdown" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Button Group"
        Body1.Div(
          "ButtonGroup groups buttons, icon buttons, button menus, and dropdowns into a single visual unit with shared variant, color, and size styling.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

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
      ],
      maxWidth = Container.MaxWidth.Large
    )
