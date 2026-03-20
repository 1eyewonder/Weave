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
        Switch.create (basicIsChecked, div [ Typography.body1 ] [ text "Default Switch" ])
      ]

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false // see here

Switch.create(isChecked, View.Const "Default Switch")
"""

    Helpers.codeSampleSection "Basic Switch" description content code

  let private disabledSwitchExample () =
    let description =
      Helpers.bodyText "A switch that is disabled and cannot be toggled."

    let content =
      let isChecked = Var.Create true

      Switch.create (
        isChecked,
        div [ Typography.body1 ] [ text "Disabled Switch" ],
        enabled = View.Const false
      )

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create true

Switch.create(
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
        |> Doc.BindView(fun t -> div [ Typography.body1 ] [ text t ])

      Switch.create (isChecked, label)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

let label =
    isChecked.View
    |> View.Map(fun v -> if v then "I am on!" else "Turn me on!") // see here

Switch.create(isChecked, label)
"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private switchSizesExample () =
    let description =
      Helpers.bodyText "Switches come in three sizes: Small, Medium, and Large."

    let content =
      let sizes = [
        Switch.Size.small, "Small", Var.Create false
        Switch.Size.medium, "Medium", Var.Create true
        Switch.Size.large, "Large", Var.Create false
      ]

      Grid.create (
        sizes
        |> List.map (fun (size, label, v) ->
          GridItem.create (
            Switch.create (v, div [ Typography.body1 ] [ text label ], attrs = [ size ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave

open WebSharper.UI

let isChecked = Var.Create false

Switch.create(
    isChecked,
    View.Const "Small",
    attrs = [ Switch.Size.small ] // see here
)

Switch.create(
    isChecked,
    View.Const "Medium",
    attrs = [ Switch.Size.medium ] // see here
)

Switch.create(
    isChecked,
    View.Const "Large",
    attrs = [ Switch.Size.large ] // see here
)
"""

    Helpers.codeSampleSection "Sizes" description content code

  let private switchColorsExample () =
    let description =
      Helpers.bodyText "Switches support all theme colors via the Switch.Color module."

    let content =
      let switches =
        [
          Switch.Color.primary, "Primary"
          Switch.Color.secondary, "Secondary"
          Switch.Color.tertiary, "Tertiary"
          Switch.Color.error, "Error"
          Switch.Color.warning, "Warning"
          Switch.Color.success, "Success"
          Switch.Color.info, "Info"
        ]
        |> List.map (fun (colorAttr, label) -> Var.Create false, colorAttr, label)

      Grid.create (
        switches
        |> List.map (fun (v, colorAttr, label) ->
          GridItem.create (
            Switch.create (v, div [ Typography.body1 ] [ text label ], attrs = [ colorAttr ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave

open WebSharper.UI

let isChecked = Var.Create false

Switch.create(
    isChecked,
    View.Const "Primary",
    attrs = [ Switch.Color.primary ] // see here
)

Switch.create(
    isChecked,
    View.Const "Error",
    attrs = [ Switch.Color.error ] // see here
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
          GridItem.create (
            Radio.create (placement, value, displayText = View.Const label),
            xs = Grid.Width.create 6,
            md = Grid.Width.create 3
          ))

      let demoChecked = Var.Create false

      let demoSwitch =
        Switch.create (
          demoChecked,
          placement.View
          |> View.MapCached(sprintf "%A")
          |> Doc.BindView(fun t -> div [ Typography.body1 ] [ text t ]),
          contentPlacement = placement.View,
          attrs = [ Switch.Size.large; Switch.Color.primary ]
        )

      Grid.create (radioButtons @ [ GridItem.create (demoSwitch, xs = Grid.Width.create 12) ])

    let code =
      """open Weave

open WebSharper.UI

let isChecked = Var.Create false

Switch.create(
    isChecked,
    View.Const "Left",
    contentPlacement = View.Const Switch.ContentPlacement.Left // see here
)

Switch.create(
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
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          Switch.create (
            Var.Create false,
            div [ Typography.body1 ] [ text "Off" ],
            attrs = [ Switch.Color.primary ]
          )
          Switch.create (
            Var.Create true,
            div [ Typography.body1 ] [ text "On" ],
            attrs = [ Switch.Color.primary ]
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


// Per-instance: pass the density class in attrs to set it on one component
Switch.create(
    isChecked,
    View.Const "Compact",
    attrs = [
        Density.compact // see here
        Switch.Color.primary
    ]
)

Switch.create(
    isChecked,
    View.Const "Spacious",
    attrs = [
        Density.spacious // see here
        Switch.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Switch"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Switch component allows users to toggle between two states. It supports different sizes, colors, and can be disabled."
        ]

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
      attrs = [ Container.MaxWidth.large ]
    )
