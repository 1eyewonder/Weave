namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open WebSharper.JavaScript

[<JavaScript>]
module DropdownExamples =

  let clickableItem n =
    DropdownItem.Create(
      text (sprintf "Item %d" n),
      onClick = (fun () -> JS.Alert(sprintf "Clicked item %d" n))
    )

  let private basicDropdownExample () =

    let items = [ 1..3 ] |> List.map clickableItem

    Dropdown.Create(
      buttonContents = text "Open Dropdown",
      items = items,
      attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ],
      buttonAttrs = [
        cls [
          Button.Variant.toClass Button.Variant.Filled
          Button.Color.toClass BrandColor.Primary
        ]
      ]
    )
    |> Helpers.section
      "Basic Usage"
      "A simple dropdown with a few items. Clicking on the button or anywhere else on screen while the dropdown is open will close the dropdown."

  let private placementExample () =
    let anchorVar = Var.Create Dropdown.AnchorOrigin.BottomLeft
    let transformVar = Var.Create Dropdown.TransformOrigin.TopLeft

    let items = [ 1..3 ] |> List.map clickableItem

    let anchorOptions = [
      Dropdown.AnchorOrigin.TopLeft
      Dropdown.AnchorOrigin.TopCenter
      Dropdown.AnchorOrigin.TopRight
      Dropdown.AnchorOrigin.CenterLeft
      Dropdown.AnchorOrigin.CenterCenter
      Dropdown.AnchorOrigin.CenterRight
      Dropdown.AnchorOrigin.BottomLeft
      Dropdown.AnchorOrigin.BottomCenter
      Dropdown.AnchorOrigin.BottomRight
    ]

    let transformOptions = [
      Dropdown.TransformOrigin.TopLeft
      Dropdown.TransformOrigin.TopCenter
      Dropdown.TransformOrigin.TopRight
      Dropdown.TransformOrigin.CenterLeft
      Dropdown.TransformOrigin.CenterCenter
      Dropdown.TransformOrigin.CenterRight
      Dropdown.TransformOrigin.BottomLeft
      Dropdown.TransformOrigin.BottomCenter
      Dropdown.TransformOrigin.BottomRight
    ]

    let radioGroup options selected toString color =
      div [ cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] ] [
        yield!
          options
          |> List.map (fun opt ->
            Radio.Create(
              selected,
              opt,
              displayText = View.Const(toString opt),
              attrs = [ Radio.Color.toClass color |> cl ]
            ))
      ]

    Grid.Create(
      [
        GridItem.Create(
          div [] [
            H3.Create("Anchor Origin", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            radioGroup anchorOptions anchorVar Dropdown.AnchorOrigin.toString BrandColor.Secondary
          ],
          xs = Grid.Width.create 5
        )
        GridItem.Create(
          Grid.Create(
            [
              GridItem.Create(
                Subtitle1.Create(
                  "The dropdown below will open based on the selected anchor and transform origins. It is configured to stay open when you are changing the selections."
                )
              )
              GridItem.Create(
                Dropdown.Create(
                  buttonContents = text "Placement",
                  items = items,
                  anchorOrigin = anchorVar.View,
                  transformOrigin = transformVar.View,
                  closeOnOutsideClick = View.Const false,
                  attrs = [ cls [ yield! Margin.toClasses Margin.Top.large ] ],
                  buttonAttrs = [
                    cls [
                      Button.Variant.toClass Button.Variant.Filled
                      Button.Color.toClass BrandColor.Primary
                    ]
                  ]
                ),
                xs = Grid.Width.create 12
              )
            ]
          ),
          xs = Grid.Width.create 2
        )

        GridItem.Create(
          div [] [
            H3.Create("Transform Origin", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            radioGroup transformOptions transformVar Dropdown.TransformOrigin.toString BrandColor.Tertiary
          ],
          xs = Grid.Width.create 5
        )
      ],
      justify = JustifyContent.SpaceAround,
      spacing = Grid.GutterSpacing.create 2
    )
    |> Helpers.section
      "Placement"
      "Dropdowns can be positioned using both anchor origin and transform origin."

  let private nestedDropdownExample () =
    let nestedIsOpen = Var.Create false

    let nestedDropdownUnstyled =
      NestedDropdown.Create(
        buttonContents = text "Open Nested",
        items = [
          DropdownItem.Create(
            text "Nested Item 1",
            onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 1"))
          )

          DropdownItem.Create(
            text "Nested Item 2",
            onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 2"))
          )
        ]
      )

    let nestedDropdownStyled =
      NestedDropdown.Create(
        buttonContents = text "Open Nested",
        items = [
          DropdownItem.Create(
            text "Nested Item 1",
            onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 1"))
          )

          DropdownItem.Create(
            text "Nested Item 2",
            onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 2"))
          )
        ],
        isOpen = nestedIsOpen,
        buttonAttrs = [
          nestedIsOpen.View
          |> Attr.DynamicClassPred(Button.Color.toClass BrandColor.Tertiary)

          nestedIsOpen.View
          |> Attr.DynamicClassPred(Button.Variant.toClass Button.Variant.Outlined)
        ]
      )

    let items = [ clickableItem 1; clickableItem 2; nestedDropdownUnstyled ]
    let styledItems = [ clickableItem 1; clickableItem 2; nestedDropdownStyled ]

    Grid.Create(
      [
        GridItem.Create(
          Dropdown.Create(
            buttonContents = text "Dropdown w/o Styling",
            items = items,
            buttonAttrs = [
              cls [
                Button.Variant.toClass Button.Variant.Filled
                Button.Color.toClass BrandColor.Primary
              ]
            ]
          )
        )

        GridItem.Create(
          Dropdown.Create(
            buttonContents = text "Dropdown w/ Styling",
            items = styledItems,
            buttonAttrs = [
              cls [
                Button.Variant.toClass Button.Variant.Filled
                Button.Color.toClass BrandColor.Secondary
              ]
            ]
          )
        )
      ]
    )
    |> Helpers.section
      "Nested Dropdowns"
      "Dropdowns can contain nested dropdown menus. There are no default stylings for nested dropdowns, however utilizing the `isOpen` parameter, you can build reactive styling based on if the given menu is open."

  let private openOnExamples () =
    let clickableDropdown =
      let items = [ 1..3 ] |> List.map clickableItem

      Dropdown.Create(
        buttonContents = text "Click to Open",
        items = items,
        openOn = View.Const Dropdown.OpenOn.Click,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [
          cls [
            Button.Variant.toClass Button.Variant.Filled
            Button.Color.toClass BrandColor.Primary
            Button.Width.toClass Button.Width.Full |> Option.defaultValue ""
          ]
        ]
      )

    let hoverDropdown =
      let items = [ 1..3 ] |> List.map clickableItem

      Dropdown.Create(
        buttonContents = text "Hover to Open",
        items = items,
        openOn = View.Const Dropdown.OpenOn.Hover,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [
          cls [
            Button.Variant.toClass Button.Variant.Filled
            Button.Color.toClass BrandColor.Secondary
            Button.Width.toClass Button.Width.Full |> Option.defaultValue ""
          ]
        ]
      )

    let nestedClickableDropdown =
      let nestedIsOpen = Var.Create false

      let nestedDropdown =
        NestedDropdown.Create(
          buttonContents = text "Open Nested",
          items = [
            DropdownItem.Create(
              text "Nested Item 1",
              onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 1"))
            )
            DropdownItem.Create(
              text "Nested Item 2",
              onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 2"))
            )
          ],
          isOpen = nestedIsOpen,
          buttonAttrs = [
            nestedIsOpen.View
            |> Attr.DynamicClassPred(Button.Color.toClass BrandColor.Primary)
          ]
        )

      Dropdown.Create(
        buttonContents = text "Click to Open Nested",
        items = [ clickableItem 1; nestedDropdown ],
        openOn = View.Const Dropdown.OpenOn.Click,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [
          cls [
            Button.Variant.toClass Button.Variant.Filled
            Button.Color.toClass BrandColor.Tertiary
            Button.Width.toClass Button.Width.Full |> Option.defaultValue ""
          ]
        ]
      )

    let nestedHoverDropdown =
      let nestedIsOpen = Var.Create false

      let nestedDropdown =
        NestedDropdown.Create(
          buttonContents = text "Open Nested",
          items = [
            DropdownItem.Create(
              text "Nested Item 1",
              onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 1"))
            )
            DropdownItem.Create(
              text "Nested Item 2",
              onClick = (fun () -> JS.Alert(sprintf "Clicked Nested Item 2"))
            )
          ],
          isOpen = nestedIsOpen,
          buttonAttrs = [
            nestedIsOpen.View
            |> Attr.DynamicClassPred(Button.Color.toClass BrandColor.Primary)
          ]
        )

      Dropdown.Create(
        buttonContents = text "Hover to Open Nested",
        items = [ clickableItem 1; nestedDropdown ],
        openOn = View.Const Dropdown.OpenOn.Hover,
        attrs = [ Attr.Style "width" "100%" ],
        buttonAttrs = [
          cls [
            Button.Variant.toClass Button.Variant.Filled
            Button.Color.toClass BrandColor.Primary
            Button.Width.toClass Button.Width.Full |> Option.defaultValue ""
          ]
        ]
      )

    Grid.Create(
      [
        GridItem.Create(clickableDropdown, xs = Grid.Width.create 3)
        GridItem.Create(hoverDropdown, xs = Grid.Width.create 3)
        GridItem.Create(nestedClickableDropdown, xs = Grid.Width.create 3)
        GridItem.Create(nestedHoverDropdown, xs = Grid.Width.create 3)
      ],
      spacing = Grid.GutterSpacing.create 2
    )
    |> Helpers.section
      "OpenOn Property"
      "The OpenOn property allows you to specify whether the dropdown opens on click or hover. The default behavior for the Dropdown component is to open on click, while NestedDropdown defaults to opening on hover. You can override this by specifying the OpenOn property when creating either component."

  let private disabledExample () =
    let items = [
      DropdownItem.Create(
        text "Enabled Item",
        onClick = (fun () -> JS.Alert(sprintf "Clicked enabled item")),
        enabled = View.Const true
      )

      DropdownItem.Create(
        text "Disabled Item",
        onClick = (fun () -> JS.Alert(sprintf "Clicked disabled item")),
        enabled = View.Const false
      )
    ]

    Dropdown.Create(
      buttonContents = text "Dropdown",
      items = items,
      attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ],
      buttonAttrs = [
        cls [
          Button.Variant.toClass Button.Variant.Filled
          Button.Color.toClass BrandColor.Primary
        ]
      ]
    )
    |> Helpers.section "Disabled Items" "Dropdown items can be disabled using the `enabled` property."

  let render () =
    Container.Create(
      div [] [
        H1.Create("Dropdown Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(
          "Dropdowns allow users to select an option from a list. They can be customized with anchor origins, nested menus, and disabled items.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

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
      ],
      maxWidth = Container.MaxWidth.Large
    )
