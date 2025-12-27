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
    |> Helpers.section "Basic Usage" "A simple dropdown with a few items."

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
          Dropdown.Create(
            buttonContents = text "Placement",
            items = items,
            anchorOrigin = anchorVar.View,
            transformOrigin = transformVar.View,
            attrs = [ cls [ yield! Margin.toClasses Margin.Top.large ] ],
            buttonAttrs = [
              cls [
                Button.Variant.toClass Button.Variant.Filled
                Button.Color.toClass BrandColor.Primary
              ]
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
      justify = JustifyContent.SpaceBetween,
      spacing = Grid.GutterSpacing.create 2
    )
    |> Helpers.section
      "Placement"
      "Dropdowns can be positioned using both anchor origin and transform origin."

  let private nestedDropdownExample () =
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

    let items = [ clickableItem 1; clickableItem 2; nestedDropdown ]

    Dropdown.Create(
      buttonContents = text "Dropdown with Nested",
      items = items,
      attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ],
      buttonAttrs = [
        cls [
          Button.Variant.toClass Button.Variant.Filled
          Button.Color.toClass BrandColor.Primary
        ]
      ]
    )
    |> Helpers.section "Nested Dropdowns" "Dropdowns can contain nested dropdown menus."

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
    |> Helpers.section "Disabled Items" "Dropdown items can be disabled using the Disabled property."

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
        disabledExample ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
