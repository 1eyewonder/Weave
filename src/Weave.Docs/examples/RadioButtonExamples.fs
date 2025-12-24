namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave

[<JavaScript>]
module RadioButtonExamples =

  let private section title description content =
    div [ Margin.toClasses Margin.Bottom.extraLarge |> cls ] [
      H3.Create(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
      Body1.Create(View.Const description, attrs = [ Margin.toClasses Margin.Bottom.medium |> cls ])
      div [
        Padding.toClasses Padding.All.medium |> cls
        SurfaceColor.toAttr SurfaceColor.Paper
        BorderRadius.toClass BorderRadius.All.small |> cl
      ] [ content ]
    ]

  let private basicRadioExample () =
    let selected = Var.Create "Option 1"
    let options = [ "Option 1"; "Option 2"; "Option 3" ]

    Grid.Create(
      [
        options
        |> List.map (fun opt -> GridItem.Create(Radio.Create(selected, opt, displayText = View.Const opt)))
        |> Doc.Concat
      ]
    )
    |> section "Basic Radio" "A simple radio group with three options. Only one can be selected."

  let private disabledRadioExample () =
    let selected = Var.Create "Disabled"

    Grid.Create(
      [
        Radio.Create(selected, "A", displayText = View.Const "Try Me", enabled = View.Const false)
        Radio.Create(selected, "B", displayText = View.Const "Or Me", enabled = View.Const false)
        Radio.Create(selected, "C", displayText = View.Const "Or Me", enabled = View.Const false)
      ]
    )
    |> section "Disabled Radio" "A radio button that is disabled and cannot be toggled."

  let private radioWithDynamicLabel () =
    let selected = Var.Create false

    let label =
      selected.View |> View.Map(fun v -> if v then "I am selected!" else "Select me!")

    Grid.Create([ Radio.Create(selected, true, displayText = label) ], justify = JustifyContent.Center)
    |> section "Dynamic Label" "A radio button with a label that updates based on its state."

  let private radioSizesExample () =
    let selected = Var.Create Radio.Size.Medium

    let sizes = [ Radio.Size.Small; Radio.Size.Medium; Radio.Size.Large ]

    Grid.Create(
      sizes
      |> List.map (fun size ->
        GridItem.Create(
          Radio.Create(
            selected,
            size,
            displayText = View.Const(sprintf "%A" size),
            attrs = [ Radio.Size.toClass size |> cl ]
          ),
          xs = Grid.Width.create 12,
          sm = Grid.Width.create 6,
          md = Grid.Width.create 4
        ))
    )
    |> section "Sizes" "Radio buttons in different sizes."

  let private radioColorsExample () =
    let selected = Var.Create BrandColor.Primary

    let colors = [
      BrandColor.Primary
      BrandColor.Secondary
      BrandColor.Tertiary
      BrandColor.Error
      BrandColor.Warning
      BrandColor.Success
      BrandColor.Info
    ]

    Grid.Create(
      colors
      |> List.map (fun color ->
        GridItem.Create(
          Radio.Create(
            selected,
            color,
            displayText = (sprintf "%A" color |> View.Const),
            attrs = [ Radio.Color.toClass color |> cl ]
          ),
          xs = Grid.Width.create 12,
          sm = Grid.Width.create 6,
          md = Grid.Width.create 1
        ))
    )
    |> section "Colors" "Radio buttons with different color themes."

  let private contentPlacementExample () =
    let placement = Var.Create Radio.ContentPlacement.Right

    let radioOptions = [
      Radio.ContentPlacement.Left, "Left"
      Radio.ContentPlacement.Right, "Right"
      Radio.ContentPlacement.Top, "Top"
      Radio.ContentPlacement.Bottom, "Bottom"
    ]

    let radioButtons =
      radioOptions
      |> List.map (fun (value, label) ->
        GridItem.Create(
          Radio.Create(placement, value, displayText = View.Const label),
          xs = Grid.Width.create 6,
          md = Grid.Width.create 3
        ))

    let demoSelected = Var.Create false

    let demoRadio =
      Radio.Create(
        demoSelected,
        true,
        displayText = (placement.View |> View.MapCached(sprintf "%A")),
        contentPlacement = placement.View,
        attrs = [
          Radio.Size.toClass Radio.Size.Large |> cl
          Radio.Color.toClass BrandColor.Primary |> cl
        ]
      )

    Grid.Create(radioButtons @ [ GridItem.Create(demoRadio, xs = Grid.Width.create 12) ])
    |> section "Content Placement" "Change the label position using the ContentPlacement option."

  let render () =
    Container.Create(
      div [] [
        H1.Create("Radio Button Component", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
        Body1.Create(
          "The Radio component allows users to select a single option from a set. It supports different sizes, colors, and can be disabled.",
          attrs = [ Margin.toClasses Margin.Bottom.extraLarge |> cls ]
        )
        basicRadioExample ()
        disabledRadioExample ()
        radioWithDynamicLabel ()
        radioSizesExample ()
        radioColorsExample ()
        contentPlacementExample ()
      ]
    )
