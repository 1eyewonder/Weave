namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave

[<JavaScript>]
module SwitchExamples =

  let private basicSwitchExample () =
    let basicIsChecked = Var.Create false

    div [] [
      basicIsChecked.View
      |> View.MapCached(sprintf "Basic is Checked: %b")
      |> View.printfn
      Switch.Create(basicIsChecked, View.Const "Default Switch")
    ]
    |> Helpers.section "Basic Switch" (Helpers.bodyText "A simple switch with a label.")

  let private disabledSwitchExample () =
    let isChecked = Var.Create true

    Switch.Create(isChecked, View.Const "Disabled Switch", enabled = View.Const false)
    |> Helpers.section "Disabled Switch" (Helpers.bodyText "A switch that is disabled and cannot be toggled.")

  let private switchWithDynamicLabel () =
    let isChecked = Var.Create false

    let label =
      isChecked.View |> View.Map(fun v -> if v then "I am on!" else "Turn me on!")

    Switch.Create(isChecked, label)
    |> Helpers.section
      "Dynamic Label"
      (Helpers.bodyText "A switch with a label that updates based on its state.")

  let private switchSizesExample () =
    let sizes = [
      Switch.Size.Small, "Small", Var.Create false
      Switch.Size.Medium, "Medium", Var.Create true
      Switch.Size.Large, "Large", Var.Create false
    ]

    Grid.Create(
      sizes
      |> List.map (fun (size, label, v) ->
        GridItem.Create(
          Switch.Create(v, View.Const label, attrs = [ Switch.Size.toClass size |> cl ]),
          xs = Grid.Width.create 12,
          sm = Grid.Width.create 6,
          md = Grid.Width.create 4
        ))
    )
    |> Helpers.section "Sizes" (Helpers.bodyText "Switches in different sizes.")

  let private switchColorsExample () =
    let switches =
      [
        BrandColor.Primary, "Primary"
        BrandColor.Secondary, "Secondary"
        BrandColor.Tertiary, "Tertiary"
        BrandColor.Error, "Error"
        BrandColor.Warning, "Warning"
        BrandColor.Success, "Success"
        BrandColor.Info, "Info"
      ]
      |> List.map (fun (color, label) -> Var.Create false, color, label)

    Grid.Create(
      switches
      |> List.map (fun (v, color, label) ->
        GridItem.Create(
          Switch.Create(v, View.Const label, attrs = [ Switch.Color.toClass color |> cl ]),
          xs = Grid.Width.create 12,
          sm = Grid.Width.create 6,
          md = Grid.Width.create 1
        ))
    )
    |> Helpers.section "Colors" (Helpers.bodyText "Switches with different color themes.")

  let private contentPlacementExample () =
    let placement = Var.Create Switch.ContentPlacement.Right

    let radioOptions = [
      Switch.ContentPlacement.Left, "Left"
      Switch.ContentPlacement.Right, "Right"
      Switch.ContentPlacement.Top, "Top"
      Switch.ContentPlacement.Bottom, "Bottom"
    ]

    let radioButtons =
      radioOptions
      |> List.map (fun (value, label) ->
        GridItem.Create(
          Radio.Create(placement, value, displayText = View.Const label),
          xs = Grid.Width.create 6,
          md = Grid.Width.create 3
        ))

    let demoChecked = Var.Create false

    let demoSwitch =
      Switch.Create(
        demoChecked,
        placement.View |> View.MapCached(sprintf "%A"),
        contentPlacement = placement.View,
        attrs = [
          Switch.Size.toClass Switch.Size.Large |> cl
          Switch.Color.toClass BrandColor.Primary |> cl
        ]
      )

    Grid.Create(radioButtons @ [ GridItem.Create(demoSwitch, xs = Grid.Width.create 12) ])
    |> Helpers.section
      "Content Placement"
      (Helpers.bodyText "Change the label position using the ContentPlacement option.")

  let render () =
    Container.Create(
      div [] [
        H1.Div("Switch Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "The Switch component allows users to toggle between two states. It supports different sizes, colors, and can be disabled.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        basicSwitchExample ()
        Helpers.divider ()
        disabledSwitchExample ()
        Helpers.divider ()
        switchWithDynamicLabel ()
        Helpers.divider ()
        switchSizesExample ()
        Helpers.divider ()
        switchColorsExample ()
        Helpers.divider ()
        contentPlacementExample ()
      ]
    )
