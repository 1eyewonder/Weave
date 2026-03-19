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
          DialogTitle.create (H6.div ("Alert")),
          DialogContent.create (
            div [] [
              Body1.div (message)
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


let items =
    [ 1..3 ]
    |> List.map (fun n ->
        DropdownItem.create(
            text (sprintf "Item %d" n),
            onClick = (fun () -> printfn "Clicked item %d" n)
        )
    )

Dropdown.create(
    buttonContents = text "Open Dropdown",
    items = items, // see here
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private placementExample () =
    let alertVar = Var.Create None
    let anchorVar = Var.Create Dropdown.AnchorOrigin.BottomLeft
    let transformVar = Var.Create Dropdown.TransformOrigin.TopLeft

    let items = [ 1..3 ] |> List.map (clickableItem alertVar)

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

    let radioGroup options selected toString colorAttr =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ] [
        yield!
          options
          |> List.map (fun opt ->
            Radio.create (
              selected,
              opt,
              displayText = View.Const(toString opt),
              attrs = [ colorAttr; Margin.Bottom.extraSmall ]
            ))
      ]

    let description =
      Helpers.bodyText "Dropdowns can be positioned using both anchor origin and transform origin."

    let content =
      div [] [
        alertDialog alertVar
        Grid.create (
          [
            GridItem.create (
              div [] [
                H6.div ("Anchor Origin", attrs = [ Margin.Bottom.extraSmall ])
                radioGroup anchorOptions anchorVar Dropdown.AnchorOrigin.toString Radio.Color.secondary
              ],
              xs = Grid.Width.create 6
            )

            GridItem.create (
              div [] [
                H6.div ("Transform Origin", attrs = [ Margin.Bottom.extraSmall ])
                radioGroup
                  transformOptions
                  transformVar
                  Dropdown.TransformOrigin.toString
                  Radio.Color.tertiary
              ],
              xs = Grid.Width.create 6
            )

            GridItem.create (
              Grid.create (
                [
                  GridItem.create (
                    Body1.div (
                      "The dropdown below will open based on the selected anchor and transform origins. It is configured to stay open when you are changing the selections.",
                      attrs = [ Attr.Style "text-align" "center" ]
                    )
                  )
                  GridItem.create (
                    Dropdown.create (
                      buttonContents = text "Placement",
                      items = items,
                      anchorOrigin = anchorVar.View,
                      transformOrigin = transformVar.View,
                      closeOnOutsideClick = View.Const false,
                      attrs = [ Margin.Top.large ],
                      buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
                    ),
                    xs = Grid.Width.create 12
                  )
                ],
                justify = JustifyContent.center,
                attrs = [ AlignItems.center; AlignContent.center ]
              ),
              xs = Grid.Width.create 10,
              attrs = [ Margin.Top.small ]
            )

          ],
          justify = JustifyContent.spaceAround
        )
      ]

    let code =
      """open Weave


let items =
    [ 1..3 ]
    |> List.map (fun n ->
        DropdownItem.create(
            text (sprintf "Item %d" n),
            onClick = (fun () -> printfn "Clicked item %d" n)
        )
    )

Dropdown.create(
    buttonContents = text "Placement",
    items = items,
    anchorOrigin = anchorVar.View, // see here
    transformOrigin = transformVar.View, // see here
    closeOnOutsideClick = View.Const false,
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)
"""

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
    NestedDropdown.create( // see here
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
            |> Attr.DynamicClassPred Css.``weave-button--tertiary`` // see here

            nestedIsOpen.View
            |> Attr.DynamicClassPred Css.``weave-button--outlined``
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
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              md = Grid.Width.create 3
            )
            GridItem.create (
              hoverDropdown,
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              md = Grid.Width.create 3
            )
            GridItem.create (
              nestedClickableDropdown,
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              md = Grid.Width.create 3
            )
            GridItem.create (
              nestedHoverDropdown,
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              md = Grid.Width.create 3
            )
          ]
        )
      ]

    let code =
      """open Weave


Dropdown.create(
    buttonContents = text "Click to Open",
    items = items,
    openOn = View.Const Dropdown.OpenOn.Click, // see here
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)

Dropdown.create(
    buttonContents = text "Hover to Open",
    items = items,
    openOn = View.Const Dropdown.OpenOn.Hover, // see here
    buttonAttrs = [
        Button.Variant.filled
        Button.Color.secondary
    ]
)

NestedDropdown.create(
    buttonContents = text "Open Nested",
    items = nestedItems,
    openOn = View.Const Dropdown.OpenOn.Hover, // see here
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
        enabled = View.Const true // see here
    )

    DropdownItem.create(
        text "Disabled Item",
        onClick = (fun () -> printfn "Clicked disabled item"),
        enabled = View.Const false // see here
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
          Subtitle2.div (label, attrs = [ Margin.Bottom.extraSmall ])
          Dropdown.create (
            buttonContents = text "Open",
            items = items,
            buttonAttrs = [ Button.Variant.filled; Button.Color.primary ]
          )
        ]

      Grid.create (
        [
          GridItem.create (col "Compact" Density.compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.create (
            col "Standard" Density.standard,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave


// Per-instance: pass the density class in attrs to set it on the dropdown root
Dropdown.create(
    buttonContents = text "Compact",
    items = items,
    attrs = [ Density.compact ] // see here
)

Dropdown.create(
    buttonContents = text "Spacious",
    items = items,
    attrs = [ Density.spacious ] // see here
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Dropdown"
        Body1.div (
          "Dropdowns allow users to select an option from a list. They can be customized with anchor origins, nested menus, and disabled items.",
          attrs = [ Margin.Bottom.extraSmall ]
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
        Helpers.divider ()
        densityExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
