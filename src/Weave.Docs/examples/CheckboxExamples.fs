namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module CheckboxExamples =

  let private basicCheckboxExample () =
    let description = Helpers.bodyText "A simple checkbox with a label."

    let content =
      let basicIsChecked = Var.Create false

      div [] [
        basicIsChecked.View
        |> View.MapCached(sprintf "Basic is Checked: %b")
        |> View.printfn

        Checkbox.create (basicIsChecked, View.Const "Default Checkbox")
      ]

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.create(isChecked, View.Const "Default Checkbox")
"""

    Helpers.codeSampleSection "Basic Checkbox" description content code

  let private disabledCheckboxExample () =
    let description =
      Helpers.bodyText "A checkbox that is disabled and cannot be toggled."

    let content =
      let isChecked = Var.Create true
      Checkbox.create (isChecked, View.Const "Disabled Checkbox", enabled = View.Const false)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create true

Checkbox.create(
    isChecked,
    View.Const "Disabled Checkbox",
    enabled = View.Const false
)
"""

    Helpers.codeSampleSection "Disabled Checkbox" description content code

  let private checkboxWithDynamicLabel () =
    let description =
      Helpers.bodyText "A checkbox with a label that updates based on its state."

    let content =
      let isChecked = Var.Create false

      let label =
        isChecked.View |> View.Map(fun v -> if v then "I am checked!" else "Check me!")

      Checkbox.create (isChecked, label)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

let label =
    isChecked.View
    |> View.Map(fun v -> if v then "I am checked!" else "Check me!")

Checkbox.create(isChecked, label)
"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private checkboxSizesExample () =
    let description =
      Helpers.bodyText
        "Control the checkbox size using the Size module. Available sizes are small, medium, and large."

    let content =
      let sizes = [
        Checkbox.Size.small, "Small", Var.Create false
        Checkbox.Size.medium, "Medium", Var.Create true
        Checkbox.Size.large, "Large", Var.Create false
      ]

      Grid.create (
        sizes
        |> List.map (fun (size, label, v) ->
          GridItem.create (
            Checkbox.create (v, View.Const label, attrs = [ size ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let smallChecked = Var.Create false
let mediumChecked = Var.Create false
let largeChecked = Var.Create false

Checkbox.create(
    smallChecked,
    View.Const "Small",
    attrs = [ Checkbox.Size.small ] // see here
)

Checkbox.create(
    mediumChecked,
    View.Const "Medium",
    attrs = [ Checkbox.Size.medium ]
)

Checkbox.create(
    largeChecked,
    View.Const "Large",
    attrs = [ Checkbox.Size.large ]
)"""

    Helpers.codeSampleSection "Sizes" description content code

  let private checkboxColorsExample () =
    let description =
      Helpers.bodyText
        "Apply brand colors to checkboxes using the Color module. Colors affect the checked state indicator."

    let content =
      let checkboxes =
        [
          Checkbox.Color.primary, "Primary"
          Checkbox.Color.secondary, "Secondary"
          Checkbox.Color.tertiary, "Tertiary"
          Checkbox.Color.error, "Error"
          Checkbox.Color.warning, "Warning"
          Checkbox.Color.success, "Success"
          Checkbox.Color.info, "Info"
        ]
        |> List.map (fun (colorAttr, label) -> Var.Create false, colorAttr, label)

      Grid.create (
        checkboxes
        |> List.map (fun (v, colorAttr, label) ->
          GridItem.create (
            Checkbox.create (v, View.Const label, attrs = [ colorAttr ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.create(isChecked, View.Const "Primary", attrs = [ Checkbox.Color.primary ])
Checkbox.create(isChecked, View.Const "Secondary", attrs = [ Checkbox.Color.secondary ])
Checkbox.create(isChecked, View.Const "Tertiary", attrs = [ Checkbox.Color.tertiary ])
Checkbox.create(isChecked, View.Const "Error", attrs = [ Checkbox.Color.error ])
Checkbox.create(isChecked, View.Const "Warning", attrs = [ Checkbox.Color.warning ])
Checkbox.create(isChecked, View.Const "Success", attrs = [ Checkbox.Color.success ])
Checkbox.create(isChecked, View.Const "Info", attrs = [ Checkbox.Color.info ])"""

    Helpers.codeSampleSection "Colors" description content code

  let private contentPlacementExample () =
    let description =
      Helpers.bodyText "Change the label position using the ContentPlacement option."

    let content =
      let placement = Var.Create Checkbox.ContentPlacement.Right

      let radioOptions = [
        Checkbox.ContentPlacement.Left, "Left"
        Checkbox.ContentPlacement.Right, "Right"
        Checkbox.ContentPlacement.Top, "Top"
        Checkbox.ContentPlacement.Bottom, "Bottom"
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

      let demoCheckBox =
        Checkbox.create (
          demoChecked,
          placement.View |> View.MapCached(sprintf "%A"),
          contentPlacement = placement.View,
          attrs = [ Checkbox.Size.large; Checkbox.Color.primary ]
        )

      Grid.create (radioButtons @ [ GridItem.create (demoCheckBox, xs = Grid.Width.create 12) ])

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.create(
    isChecked,
    View.Const "Left",
    contentPlacement = View.Const Checkbox.ContentPlacement.Left, // see here
    attrs = [ Checkbox.Size.large; Checkbox.Color.primary ]
)

Checkbox.create(
    isChecked,
    View.Const "Right",
    contentPlacement = View.Const Checkbox.ContentPlacement.Right,
    attrs = [ Checkbox.Size.large; Checkbox.Color.primary ]
)

Checkbox.create(
    isChecked,
    View.Const "Top",
    contentPlacement = View.Const Checkbox.ContentPlacement.Top,
    attrs = [ Checkbox.Size.large; Checkbox.Color.primary ]
)

Checkbox.create(
    isChecked,
    View.Const "Bottom",
    contentPlacement = View.Const Checkbox.ContentPlacement.Bottom,
    attrs = [ Checkbox.Size.large; Checkbox.Color.primary ]
)"""

    Helpers.codeSampleSection "Content Placement" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding on a three-step scale: Compact, Standard, and Spacious. Pass the density class in attrs to override a single instance."

    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          Checkbox.create (Var.Create false, View.Const "Unchecked", attrs = [ Checkbox.Color.primary ])
          Checkbox.create (Var.Create true, View.Const "Checked", attrs = [ Checkbox.Color.primary ])
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
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.create(
    isChecked,
    View.Const "Compact",
    attrs = [
        Density.compact // see here
        Checkbox.Color.primary
    ]
)

Checkbox.create(
    isChecked,
    View.Const "Standard",
    attrs = [
        Density.standard
        Checkbox.Color.primary
    ]
)

Checkbox.create(
    isChecked,
    View.Const "Spacious",
    attrs = [
        Density.spacious
        Checkbox.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Checkbox"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Checkbox component allows users to select one or more options from a set. It supports different sizes, colors, and can be disabled."
        ]

        Helpers.divider ()
        basicCheckboxExample ()
        Helpers.divider ()
        disabledCheckboxExample ()
        Helpers.divider ()
        checkboxWithDynamicLabel ()
        Helpers.divider ()
        checkboxSizesExample ()
        Helpers.divider ()
        checkboxColorsExample ()
        Helpers.divider ()
        contentPlacementExample ()
        Helpers.divider ()
        densityExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
