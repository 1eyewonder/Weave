namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave

[<JavaScript>]
module RadioButtonExamples =

  let private basicRadioExample () =
    let description =
      Helpers.bodyText "A simple radio group with three options. Only one can be selected."

    let content =
      let selected = Var.Create "Option 1"
      let options = [ "Option 1"; "Option 2"; "Option 3" ]

      Grid.create (
        [
          options
          |> List.map (fun opt ->
            GridItem.create (Radio.create (selected, opt, displayText = View.Const opt)))
          |> Doc.Concat
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create "Option 1" // see here
let options = [ "Option 1"; "Option 2"; "Option 3" ]

options
|> List.map (fun opt ->
    Radio.create(selected, opt, displayText = View.Const opt)
)
"""

    Helpers.codeSampleSection "Basic Radio" description content code

  let private disabledRadioExample () =
    let description =
      Helpers.bodyText "A radio button that is disabled and cannot be toggled."

    let content =
      let selected = Var.Create "Disabled"

      Grid.create (
        [
          Radio.create (selected, "A", displayText = View.Const "Try Me", enabled = View.Const false)
          Radio.create (selected, "B", displayText = View.Const "Or Me", enabled = View.Const false)
          Radio.create (selected, "C", displayText = View.Const "Or Me", enabled = View.Const false)
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create "Disabled"

Radio.create(
    selected,
    "A",
    displayText = View.Const "Try Me",
    enabled = View.Const false // see here
)

Radio.create(
    selected,
    "B",
    displayText = View.Const "Or Me",
    enabled = View.Const false // see here
)
"""

    Helpers.codeSampleSection "Disabled Radio" description content code

  let private radioWithDynamicLabel () =
    let description =
      Helpers.bodyText "A radio button with a label that updates based on its state."

    let content =
      let selected = Var.Create false

      let label =
        selected.View |> View.Map(fun v -> if v then "I am selected!" else "Select me!")

      Grid.create ([ Radio.create (selected, true, displayText = label) ], justify = JustifyContent.center)

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create false

let label =
    selected.View
    |> View.Map(fun v -> if v then "I am selected!" else "Select me!") // see here

Radio.create(selected, true, displayText = label)
"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private radioSizesExample () =
    let description = Helpers.bodyText "Radio buttons in different sizes."

    let content =
      let selected = Var.Create "Medium"

      let sizes = [
        "Small", Radio.Size.small
        "Medium", Radio.Size.medium
        "Large", Radio.Size.large
      ]

      Grid.create (
        sizes
        |> List.map (fun (label, sizeAttr) ->
          GridItem.create (
            Radio.create (selected, label, displayText = View.Const label, attrs = [ sizeAttr ]),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave

open WebSharper.UI

let selected = Var.Create "Medium"

Radio.create(selected, "Small", displayText = View.Const "Small", attrs = [ Radio.Size.small ])
Radio.create(selected, "Medium", displayText = View.Const "Medium", attrs = [ Radio.Size.medium ])
Radio.create(selected, "Large", displayText = View.Const "Large", attrs = [ Radio.Size.large ])
"""

    Helpers.codeSampleSection "Sizes" description content code

  let private radioColorsExample () =
    let description = Helpers.bodyText "Radio buttons with different color themes."

    let content =
      let selected = Var.Create BrandColor.Primary

      let colors = [
        BrandColor.Primary, Radio.Color.primary
        BrandColor.Secondary, Radio.Color.secondary
        BrandColor.Tertiary, Radio.Color.tertiary
        BrandColor.Error, Radio.Color.error
        BrandColor.Warning, Radio.Color.warning
        BrandColor.Success, Radio.Color.success
        BrandColor.Info, Radio.Color.info
      ]

      Grid.create (
        colors
        |> List.map (fun (color, colorAttr) ->
          GridItem.create (
            Radio.create (
              selected,
              color,
              displayText = (sprintf "%A" color |> View.Const),
              attrs = [ colorAttr ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave

open WebSharper.UI

let selected = Var.Create BrandColor.Primary

let colors = [
    BrandColor.Primary, Radio.Color.primary
    BrandColor.Secondary, Radio.Color.secondary
    BrandColor.Tertiary, Radio.Color.tertiary
    BrandColor.Error, Radio.Color.error
    BrandColor.Warning, Radio.Color.warning
    BrandColor.Success, Radio.Color.success
    BrandColor.Info, Radio.Color.info
]

colors
|> List.map (fun (color, colorAttr) ->
    Radio.create(
        selected,
        color,
        displayText = (sprintf "%A" color |> View.Const),
        attrs = [ colorAttr ] // see here
    )
)
"""

    Helpers.codeSampleSection "Colors" description content code

  let private contentPlacementExample () =
    let description =
      Helpers.bodyText "Change the label position using the ContentPlacement option."

    let content =
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
          GridItem.create (
            Radio.create (placement, value, displayText = View.Const label),
            xs = Grid.Width.create 6,
            md = Grid.Width.create 3
          ))

      let demoSelected = Var.Create false

      let demoRadio =
        Radio.create (
          demoSelected,
          true,
          displayText = (placement.View |> View.MapCached(sprintf "%A")),
          contentPlacement = placement.View,
          attrs = [ Radio.Size.large; Radio.Color.primary ]
        )

      Grid.create (radioButtons @ [ GridItem.create (demoRadio, xs = Grid.Width.create 12) ])

    let code =
      """open Weave

open WebSharper.UI

let placement = Var.Create Radio.ContentPlacement.Right

Radio.create(
    demoSelected,
    true,
    displayText = (placement.View |> View.MapCached(sprintf "%A")),
    contentPlacement = placement.View, // see here
    attrs = [
        Radio.Size.large
        Radio.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Content Placement" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding on a three-step scale: Compact, Standard, and Spacious. Pass the density class in attrs to override a single instance."

    let content =
      let selected = Var.Create "A"

      let col (label: string) densityAttr =
        div [ densityAttr ] [
          Subtitle2.div (label, attrs = [ Margin.Bottom.extraSmall ])
          Radio.create (selected, "A", displayText = View.Const "Option A", attrs = [ Radio.Color.primary ])
          Radio.create (selected, "B", displayText = View.Const "Option B", attrs = [ Radio.Color.primary ])
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
Radio.create(
    userSelection,
    value,
    displayText = View.Const "Compact",
    attrs = [
        Density.compact // see here
        Radio.Color.primary
    ]
)

Radio.create(
    userSelection,
    value,
    displayText = View.Const "Spacious",
    attrs = [
        Density.spacious // see here
        Radio.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Radio Button"
        Body1.div (
          "The Radio component allows users to select a single option from a set. It supports different sizes, colors, and can be disabled.",
          attrs = [ Margin.Bottom.extraSmall ]
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
      attrs = [ Container.MaxWidth.large ]
    )
