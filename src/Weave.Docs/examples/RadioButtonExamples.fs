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
      Helpers.bodyText
        "Create a radio group by passing the same Var to multiple Radio.create calls. Only one option can be selected at a time."

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

let selected = Var.Create "Option 1"

Radio.create(selected, "Option 1", displayText = View.Const "Option 1")
Radio.create(selected, "Option 2", displayText = View.Const "Option 2")
Radio.create(selected, "Option 3", displayText = View.Const "Option 3")"""

    Helpers.codeSampleSection "Basic Radio" description content code

  let private disabledRadioExample () =
    let description =
      Helpers.bodyText
        "Set enabled to View.Const false to prevent user interaction and visually dim the radio button."

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
    enabled = View.Const false
)

Radio.create(
    selected,
    "B",
    displayText = View.Const "Or Me",
    enabled = View.Const false  // see here
)"""

    Helpers.codeSampleSection "Disabled Radio" description content code

  let private radioWithDynamicLabel () =
    let description =
      Helpers.bodyText "A radio button with a label that updates based on its state."

    let content =
      let selected = Var.Create false

      let label =
        selected.View |> View.Map(fun v -> if v then "I am selected!" else "Select me!")

      Grid.create ([ Radio.create (selected, true, displayText = label) ], attrs = [ JustifyContent.center ])

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create false

let label =
    selected.View
    |> View.Map(fun v -> if v then "I am selected!" else "Select me!")

Radio.create(selected, true, displayText = label)"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private radioSizesExample () =
    let description =
      Helpers.bodyText
        "Control the radio indicator size with Radio.Size.small, Radio.Size.medium, or Radio.Size.large passed via attrs."

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
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create "Medium"

Radio.create(selected, "Small", displayText = View.Const "Small", attrs = [ Radio.Size.small ])
Radio.create(selected, "Medium", displayText = View.Const "Medium", attrs = [ Radio.Size.medium ])
Radio.create(selected, "Large", displayText = View.Const "Large", attrs = [ Radio.Size.large ])"""

    Helpers.codeSampleSection "Sizes" description content code

  let private radioColorsExample () =
    let description =
      Helpers.bodyText
        "Apply a brand color via attrs using Radio.Color.* to theme the radio indicator and ripple."

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
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create BrandColor.Primary

Radio.create(selected, BrandColor.Primary, displayText = View.Const "Primary", attrs = [ Radio.Color.primary ])
Radio.create(selected, BrandColor.Secondary, displayText = View.Const "Secondary", attrs = [ Radio.Color.secondary ])
Radio.create(selected, BrandColor.Tertiary, displayText = View.Const "Tertiary", attrs = [ Radio.Color.tertiary ])
Radio.create(selected, BrandColor.Error, displayText = View.Const "Error", attrs = [ Radio.Color.error ])
Radio.create(selected, BrandColor.Warning, displayText = View.Const "Warning", attrs = [ Radio.Color.warning ])
Radio.create(selected, BrandColor.Success, displayText = View.Const "Success", attrs = [ Radio.Color.success ])
Radio.create(selected, BrandColor.Info, displayText = View.Const "Info", attrs = [ Radio.Color.info ])"""

    Helpers.codeSampleSection "Colors" description content code

  let private contentPlacementExample () =
    let description =
      Helpers.bodyText
        "Position the label relative to the radio indicator using the ContentPlacement module. Available placements are right (default), left, top, and bottom."

    let content =
      let placements = [
        Radio.ContentPlacement.left, "Left"
        Radio.ContentPlacement.right, "Right"
        Radio.ContentPlacement.top, "Top"
        Radio.ContentPlacement.bottom, "Bottom"
      ]

      Grid.create (
        placements
        |> List.map (fun (placementAttr, label) ->
          GridItem.create (
            Radio.create (
              Var.Create false,
              true,
              displayText = View.Const label,
              attrs = [ Radio.Size.large; Radio.Color.primary; placementAttr ]
            ),
            attrs = [ GridItem.Span.six; GridItem.Span.Medium.three ]
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create false

// Label to the right of the radio (default)
Radio.create(
    selected, true,
    displayText = View.Const "Right",
    attrs = [ Radio.ContentPlacement.right ] // see here
)

// Label to the left
Radio.create(
    selected, true,
    displayText = View.Const "Left",
    attrs = [ Radio.ContentPlacement.left ]
)

// Label above
Radio.create(
    selected, true,
    displayText = View.Const "Top",
    attrs = [ Radio.ContentPlacement.top ]
)

// Label below
Radio.create(
    selected, true,
    displayText = View.Const "Bottom",
    attrs = [ Radio.ContentPlacement.bottom ]
)"""

    Helpers.codeSampleSection "Content Placement" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding on a three-step scale: Compact, Standard, and Spacious. Wrap radio buttons in a container with the density attribute to apply it."

    let content =
      let selected = Var.Create "A"

      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          Radio.create (selected, "A", displayText = View.Const "Option A", attrs = [ Radio.Color.primary ])
          Radio.create (selected, "B", displayText = View.Const "Option B", attrs = [ Radio.Color.primary ])
        ]

      Grid.create (
        [
          GridItem.create (
            col "Compact" Density.compact,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Standard" Density.standard,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
        ],
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create "A"

// Compact
div [ Density.compact ] [
    Radio.create(selected, "A", displayText = View.Const "Option A", attrs = [ Radio.Color.primary ])
    Radio.create(selected, "B", displayText = View.Const "Option B", attrs = [ Radio.Color.primary ])
]

// Standard
div [ Density.standard ] [
    Radio.create(selected, "A", displayText = View.Const "Option A", attrs = [ Radio.Color.primary ])
    Radio.create(selected, "B", displayText = View.Const "Option B", attrs = [ Radio.Color.primary ])
]

// Spacious
div [ Density.spacious ] [
    Radio.create(selected, "A", displayText = View.Const "Option A", attrs = [ Radio.Color.primary ])
    Radio.create(selected, "B", displayText = View.Const "Option B", attrs = [ Radio.Color.primary ])
]"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Radio Button"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Radio component allows users to select a single option from a set. It supports different sizes, colors, and can be disabled."
        ]

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
