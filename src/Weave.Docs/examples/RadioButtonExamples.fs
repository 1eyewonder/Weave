namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave

[<JavaScript>]
module RadioButtonExamples =

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
    |> Helpers.section
      "Basic Radio"
      (Helpers.bodyText "A simple radio group with three options. Only one can be selected.")

  let private disabledRadioExample () =
    let selected = Var.Create "Disabled"

    Grid.Create(
      [
        Radio.Create(selected, "A", displayText = View.Const "Try Me", enabled = View.Const false)
        Radio.Create(selected, "B", displayText = View.Const "Or Me", enabled = View.Const false)
        Radio.Create(selected, "C", displayText = View.Const "Or Me", enabled = View.Const false)
      ]
    )
    |> Helpers.section
      "Disabled Radio"
      (Helpers.bodyText "A radio button that is disabled and cannot be toggled.")

  let private radioWithDynamicLabel () =
    let selected = Var.Create false

    let label =
      selected.View |> View.Map(fun v -> if v then "I am selected!" else "Select me!")

    Grid.Create([ Radio.Create(selected, true, displayText = label) ], justify = JustifyContent.Center)
    |> Helpers.section
      "Dynamic Label"
      (Helpers.bodyText "A radio button with a label that updates based on its state.")

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
    |> Helpers.section "Sizes" (Helpers.bodyText "Radio buttons in different sizes.")

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
    |> Helpers.section "Colors" (Helpers.bodyText "Radio buttons with different color themes.")

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
    |> Helpers.section
      "Content Placement"
      (Helpers.bodyText "Change the label position using the ContentPlacement option.")

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding on a three-step scale: Compact, Standard, and Spacious. Pass the density class in attrs to override a single instance."

    let content =
      let selected = Var.Create "A"

      let col density =
        let label = sprintf "%A" density

        div [ cl (Density.toClass density) ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          Radio.Create(
            selected,
            "A",
            displayText = View.Const "Option A",
            attrs = [ Radio.Color.toClass BrandColor.Primary |> cl ]
          )
          Radio.Create(
            selected,
            "B",
            displayText = View.Const "Option B",
            attrs = [ Radio.Color.toClass BrandColor.Primary |> cl ]
          )
        ]

      Grid.Create(
        [
          GridItem.Create(col Density.Compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Standard, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Spacious, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.toClass AlignItems.Start |> cl ]
      )

    let code =
      """open Weave
open Weave.CssHelpers

// Per-instance: pass the density class in attrs to set it on one component
Radio.Create(
    userSelection,
    value,
    displayText = View.Const "Compact",
    attrs = [
        cl (Density.toClass Density.Compact) // see here
        Radio.Color.toClass BrandColor.Primary |> cl
    ]
)

Radio.Create(
    userSelection,
    value,
    displayText = View.Const "Spacious",
    attrs = [
        cl (Density.toClass Density.Spacious) // see here
        Radio.Color.toClass BrandColor.Primary |> cl
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.Create(
      div [] [
        H1.Div("Radio Button Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "The Radio component allows users to select a single option from a set. It supports different sizes, colors, and can be disabled.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        basicRadioExample ()
        Helpers.divider ()
        disabledRadioExample ()
        Helpers.divider ()
        radioWithDynamicLabel ()
        Helpers.divider ()
        radioSizesExample ()
        Helpers.divider ()
        radioColorsExample ()
        Helpers.divider ()
        contentPlacementExample ()
        Helpers.divider ()
        densityExample ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
