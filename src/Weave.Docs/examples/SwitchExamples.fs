namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave

[<JavaScript>]
module SwitchExamples =

  let private basicSwitchExample () =
    let description =
      Helpers.bodyText "A simple switch with a label and reactive state output."

    let content =
      let basicIsChecked = Var.Create false

      div [] [
        basicIsChecked.View
        |> View.MapCached(sprintf "Basic is Checked: %b")
        |> View.printfn
        Switch.Create(basicIsChecked, Body1.Div("Default Switch"))
      ]

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false // see here

Switch.Create(isChecked, View.Const "Default Switch")
"""

    Helpers.codeSampleSection "Basic Switch" description content code

  let private disabledSwitchExample () =
    let description =
      Helpers.bodyText "A switch that is disabled and cannot be toggled."

    let content =
      let isChecked = Var.Create true
      Switch.Create(isChecked, Body1.Div("Disabled Switch"), enabled = View.Const false)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create true

Switch.Create(
    isChecked,
    View.Const "Disabled Switch",
    enabled = View.Const false // see here
)
"""

    Helpers.codeSampleSection "Disabled Switch" description content code

  let private switchWithDynamicLabel () =
    let description =
      Helpers.bodyText "A switch with a label that updates based on its state."

    let content =
      let isChecked = Var.Create false

      let label =
        isChecked.View
        |> View.Map(fun v -> if v then "I am on!" else "Turn me on!")
        |> Doc.BindView(fun text -> Body1.Div(text))

      Switch.Create(isChecked, label)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

let label =
    isChecked.View
    |> View.Map(fun v -> if v then "I am on!" else "Turn me on!") // see here

Switch.Create(isChecked, label)
"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private switchSizesExample () =
    let description =
      Helpers.bodyText "Switches come in three sizes: Small, Medium, and Large."

    let content =
      let sizes = [
        Switch.Size.Small, "Small", Var.Create false
        Switch.Size.Medium, "Medium", Var.Create true
        Switch.Size.Large, "Large", Var.Create false
      ]

      Grid.Create(
        sizes
        |> List.map (fun (size, label, v) ->
          GridItem.Create(
            Switch.Create(v, Body1.Div(label), attrs = [ Switch.Size.toClass size |> cl ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave

open WebSharper.UI

let isChecked = Var.Create false

Switch.Create(
    isChecked,
    View.Const "Small",
    attrs = [ Switch.Size.toClass Switch.Size.Small |> cl ] // see here
)

Switch.Create(
    isChecked,
    View.Const "Medium",
    attrs = [ Switch.Size.toClass Switch.Size.Medium |> cl ] // see here
)

Switch.Create(
    isChecked,
    View.Const "Large",
    attrs = [ Switch.Size.toClass Switch.Size.Large |> cl ] // see here
)
"""

    Helpers.codeSampleSection "Sizes" description content code

  let private switchColorsExample () =
    let description =
      Helpers.bodyText "Switches support all theme colors via the Switch.Color module."

    let content =
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
            Switch.Create(v, Body1.Div(label), attrs = [ Switch.Color.toClass color |> cl ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave

open WebSharper.UI

let isChecked = Var.Create false

Switch.Create(
    isChecked,
    View.Const "Primary",
    attrs = [ Switch.Color.toClass BrandColor.Primary |> cl ] // see here
)

Switch.Create(
    isChecked,
    View.Const "Error",
    attrs = [ Switch.Color.toClass BrandColor.Error |> cl ] // see here
)
"""

    Helpers.codeSampleSection "Colors" description content code

  let private contentPlacementExample () =
    let description =
      Helpers.bodyText
        "Change the label position using the contentPlacement parameter. Use the radio buttons below to see each placement in action."

    let content =
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
          placement.View
          |> View.MapCached(sprintf "%A")
          |> Doc.BindView(fun text -> Body1.Div(text)),
          contentPlacement = placement.View,
          attrs = [
            Switch.Size.toClass Switch.Size.Large |> cl
            Switch.Color.toClass BrandColor.Primary |> cl
          ]
        )

      Grid.Create(radioButtons @ [ GridItem.Create(demoSwitch, xs = Grid.Width.create 12) ])

    let code =
      """open Weave

open WebSharper.UI

let isChecked = Var.Create false

Switch.Create(
    isChecked,
    View.Const "Left",
    contentPlacement = View.Const Switch.ContentPlacement.Left // see here
)

Switch.Create(
    isChecked,
    View.Const "Top",
    contentPlacement = View.Const Switch.ContentPlacement.Top // see here
)
"""

    Helpers.codeSampleSection "Content Placement" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding on a three-step scale: Compact, Standard, and Spacious. Pass the density class in attrs to override a single instance."

    let content =
      let col density =
        let label = sprintf "%A" density

        div [ cl (Density.toClass density) ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          Switch.Create(
            Var.Create false,
            Body1.Div("Off"),
            attrs = [ Switch.Color.toClass BrandColor.Primary |> cl ]
          )
          Switch.Create(
            Var.Create true,
            Body1.Div("On"),
            attrs = [ Switch.Color.toClass BrandColor.Primary |> cl ]
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


// Per-instance: pass the density class in attrs to set it on one component
Switch.Create(
    isChecked,
    View.Const "Compact",
    attrs = [
        cl (Density.toClass Density.Compact) // see here
        Switch.Color.toClass BrandColor.Primary |> cl
    ]
)

Switch.Create(
    isChecked,
    View.Const "Spacious",
    attrs = [
        cl (Density.toClass Density.Spacious) // see here
        Switch.Color.toClass BrandColor.Primary |> cl
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Switch"
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
        Helpers.divider ()
        densityExample ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
