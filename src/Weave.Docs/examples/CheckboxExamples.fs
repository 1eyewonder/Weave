namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
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

        Checkbox.Create(basicIsChecked, View.Const "Default Checkbox")
      ]

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.Create(isChecked, View.Const "Default Checkbox") // see here
"""

    Helpers.codeSampleSection "Basic Checkbox" description content code

  let private disabledCheckboxExample () =
    let description =
      Helpers.bodyText "A checkbox that is disabled and cannot be toggled."

    let content =
      let isChecked = Var.Create true
      Checkbox.Create(isChecked, View.Const "Disabled Checkbox", enabled = View.Const false)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create true

Checkbox.Create(
    isChecked,
    View.Const "Disabled Checkbox",
    enabled = View.Const false // see here
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

      Checkbox.Create(isChecked, label)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

let label =
    isChecked.View
    |> View.Map(fun v -> if v then "I am checked!" else "Check me!") // see here

Checkbox.Create(isChecked, label)
"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private checkboxSizesExample () =
    let description = Helpers.bodyText "Checkboxes in different sizes."

    let content =
      let sizes = [
        Checkbox.Size.Small, "Small", Var.Create false
        Checkbox.Size.Medium, "Medium", Var.Create true
        Checkbox.Size.Large, "Large", Var.Create false
      ]

      Grid.Create(
        sizes
        |> List.map (fun (size, label, v) ->
          GridItem.Create(
            Checkbox.Create(v, View.Const label, attrs = [ Checkbox.Size.toClass size |> cl ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.Create(
    isChecked,
    View.Const "Small",
    attrs = [ Checkbox.Size.toClass Checkbox.Size.Small |> cl ] // see here
)

Checkbox.Create(
    isChecked,
    View.Const "Medium",
    attrs = [ Checkbox.Size.toClass Checkbox.Size.Medium |> cl ]
)

Checkbox.Create(
    isChecked,
    View.Const "Large",
    attrs = [ Checkbox.Size.toClass Checkbox.Size.Large |> cl ]
)
"""

    Helpers.codeSampleSection "Sizes" description content code

  let private checkboxColorsExample () =
    let description = Helpers.bodyText "Checkboxes with different color themes."

    let content =
      let checkboxes =
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
        checkboxes
        |> List.map (fun (v, color, label) ->
          GridItem.Create(
            Checkbox.Create(v, View.Const label, attrs = [ Checkbox.Color.toClass color |> cl ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 1
          ))
      )

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.Create(
    isChecked,
    View.Const "Primary",
    attrs = [ Checkbox.Color.toClass BrandColor.Primary |> cl ] // see here
)

Checkbox.Create(
    isChecked,
    View.Const "Error",
    attrs = [ Checkbox.Color.toClass BrandColor.Error |> cl ]
)
"""

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
          GridItem.Create(
            Radio.Create(placement, value, displayText = View.Const label),
            xs = Grid.Width.create 6,
            md = Grid.Width.create 3
          ))

      let demoChecked = Var.Create false

      let demoCheckBox =
        Checkbox.Create(
          demoChecked,
          placement.View |> View.MapCached(sprintf "%A"),
          contentPlacement = placement.View,
          attrs = [
            Checkbox.Size.toClass Checkbox.Size.Large |> cl
            Checkbox.Color.toClass BrandColor.Primary |> cl
          ]
        )

      Grid.Create(radioButtons @ [ GridItem.Create(demoCheckBox, xs = Grid.Width.create 12) ])

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

let isChecked = Var.Create false

Checkbox.Create(
    isChecked,
    View.Const "Left",
    contentPlacement = View.Const Checkbox.ContentPlacement.Left, // see here
    attrs = [
        Checkbox.Size.toClass Checkbox.Size.Large |> cl
        Checkbox.Color.toClass BrandColor.Primary |> cl
    ]
)

Checkbox.Create(
    isChecked,
    View.Const "Top",
    contentPlacement = View.Const Checkbox.ContentPlacement.Top, // see here
    attrs = [
        Checkbox.Size.toClass Checkbox.Size.Large |> cl
        Checkbox.Color.toClass BrandColor.Primary |> cl
    ]
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
          Checkbox.Create(
            Var.Create false,
            View.Const "Unchecked",
            attrs = [ Checkbox.Color.toClass BrandColor.Primary |> cl ]
          )
          Checkbox.Create(
            Var.Create true,
            View.Const "Checked",
            attrs = [ Checkbox.Color.toClass BrandColor.Primary |> cl ]
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
Checkbox.Create(
    isChecked,
    View.Const "Compact",
    attrs = [
        cl (Density.toClass Density.Compact) // see here
        Checkbox.Color.toClass BrandColor.Primary |> cl
    ]
)

Checkbox.Create(
    isChecked,
    View.Const "Spacious",
    attrs = [
        cl (Density.toClass Density.Spacious) // see here
        Checkbox.Color.toClass BrandColor.Primary |> cl
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Checkbox"
        Body1.Div(
          "The Checkbox component allows users to select one or more options from a set. It supports different sizes, colors, and can be disabled.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

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
      maxWidth = Container.MaxWidth.Large
    )
