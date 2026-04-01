namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave

[<JavaScript>]
module SliderExamples =

  let private basicSliderExample () =
    let description =
      Helpers.bodyText
        "A basic slider with default settings (0–100, step 1). The current value is displayed reactively."

    let content =
      let value = Var.Create 50

      div [] [
        Slider.create (value, labelText = (value.View |> View.MapCached(sprintf "Volume: %d")))
      ]

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create 50

Slider.create(
    value,
    labelText = (value.View |> View.MapCached(sprintf "Volume: %d"))
)"""

    Helpers.codeSampleSection "Basic Slider" description content code

  let private colorsExample () =
    let description =
      Helpers.bodyText
        "Apply brand colors using shorthand constructors like Slider.primary, Slider.secondary, etc."

    let content =
      Grid.create (
        [
          GridItem.create (
            Slider.primary (Var.Create 20, labelText = View.Const "Primary"),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
          GridItem.create (
            Slider.secondary (Var.Create 30, labelText = View.Const "Secondary"),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
          GridItem.create (
            Slider.tertiary (Var.Create 40, labelText = View.Const "Tertiary"),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
          GridItem.create (
            Slider.error (Var.Create 50, labelText = View.Const "Error"),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
          GridItem.create (
            Slider.warning (Var.Create 60, labelText = View.Const "Warning"),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
          GridItem.create (
            Slider.success (Var.Create 70, labelText = View.Const "Success"),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
          GridItem.create (
            Slider.info (Var.Create 80, labelText = View.Const "Info"),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

Slider.primary(Var.Create 20, labelText = View.Const "Primary")

Slider.secondary(Var.Create 30, labelText = View.Const "Secondary")

Slider.tertiary(Var.Create 40, labelText = View.Const "Tertiary")

Slider.error(Var.Create 50, labelText = View.Const "Error")

Slider.warning(Var.Create 60, labelText = View.Const "Warning")

Slider.success(Var.Create 70, labelText = View.Const "Success")

Slider.info(Var.Create 80, labelText = View.Const "Info")"""

    Helpers.codeSampleSection "Colors" description content code

  let private stepAndRangeExample () =
    let description =
      Helpers.bodyText
        "Customize the min, max, and step values. The slider snaps to the nearest step when dragged or when using keyboard arrows."

    let content =
      let tempValue = Var.Create 22
      let percentValue = Var.Create 50

      Grid.create (
        [
          GridItem.create (
            Slider.warning (
              tempValue,
              min = 16,
              max = 30,
              step = 2,
              labelText = (tempValue.View |> View.MapCached(sprintf "Temperature: %d°C"))
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
          GridItem.create (
            Slider.info (
              percentValue,
              min = 0,
              max = 100,
              step = 10,
              labelText = (percentValue.View |> View.MapCached(sprintf "Opacity: %d%%"))
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let tempValue = Var.Create 22

Slider.warning(
    tempValue,
    min = 16,
    max = 30,
    step = 2,
    labelText = (tempValue.View |> View.MapCached(sprintf "Temperature: %d°C"))
)

let percentValue = Var.Create 50

Slider.info(
    percentValue,
    min = 0,
    max = 100,
    step = 10,
    labelText = (percentValue.View |> View.MapCached(sprintf "Opacity: %d%%"))
)"""

    Helpers.codeSampleSection "Step & Range" description content code

  let private tickMarksExample () =
    let description =
      Helpers.bodyText
        "Enable tick marks to show discrete step positions along the track. Ticks before the current value are highlighted with the brand color."

    let content =
      let value = Var.Create 40

      div [] [
        value.View |> View.MapCached(sprintf "Value: %d") |> View.printfn

        Slider.primary (
          value,
          min = 0,
          max = 100,
          step = 20,
          showTickMarks = true,
          labelText = View.Const "With Tick Marks"
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create 40

Slider.primary(
    value,
    min = 0,
    max = 100,
    step = 20,
    showTickMarks = true,
    labelText = View.Const "With Tick Marks"
)"""

    Helpers.codeSampleSection "Tick Marks" description content code

  let private tickLabelsExample () =
    let description =
      Helpers.bodyText
        "Add labels beneath the tick positions to give users context for each stop. Labels are evenly spaced across the track width."

    let content =
      let value = Var.Create 2

      div [] [
        value.View |> View.MapCached(sprintf "Selected: %d") |> View.printfn

        Slider.secondary (
          value,
          min = 0,
          max = 4,
          step = 1,
          showTickMarks = true,
          tickMarkLabels = [ "XS"; "S"; "M"; "L"; "XL" ],
          labelText = View.Const "Size"
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create 2

Slider.secondary(
    value,
    min = 0,
    max = 4,
    step = 1,
    showTickMarks = true,
    tickMarkLabels = [ "XS"; "S"; "M"; "L"; "XL" ],
    labelText = View.Const "Size"
)"""

    Helpers.codeSampleSection "Tick Labels" description content code

  let private floatSliderExample () =
    let description =
      Helpers.bodyText
        "The slider also supports floating-point values with fine-grained steps. Pass a Var<float> to use the float overload."

    let content =
      let value = Var.Create 0.5

      div [] [
        Slider.tertiary (
          value,
          min = 0.0,
          max = 1.0,
          step = 0.05,
          labelText = (value.View |> View.MapCached(sprintf "Opacity: %.2f"))
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create 0.5

Slider.tertiary(
    value,
    min = 0.0,
    max = 1.0,
    step = 0.05,
    labelText = (value.View |> View.MapCached(sprintf "Opacity: %.2f"))
)"""

    Helpers.codeSampleSection "Float Values" description content code

  let private disabledSliderExample () =
    let description =
      Helpers.bodyText
        "A disabled slider prevents interaction. The thumb and track appear at reduced opacity."

    let content =
      Slider.primary (Var.Create 35, labelText = View.Const "Disabled", enabled = View.Const false)

    let code =
      """open Weave
open WebSharper.UI

Slider.primary(
    Var.Create 35,
    labelText = View.Const "Disabled",
    enabled = View.Const false
)"""

    Helpers.codeSampleSection "Disabled" description content code

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "Complete API reference for Slider. Two overloads exist: one for Var<int> and one for Var<float>.")
      [
        Helpers.apiTable "Slider.create (int)" [
          Helpers.apiParam "value" "Var<int>" "" "Two-way binding for the integer slider value"
          Helpers.apiParam "?min" "int" "0" "Minimum value of the slider range"
          Helpers.apiParam "?max" "int" "100" "Maximum value of the slider range"
          Helpers.apiParam "?step" "int" "1" "Step increment for keyboard and tick marks"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Label displayed above the slider"
          Helpers.apiParam "?showTickMarks" "bool" "false" "Show tick marks at each step position"
          Helpers.apiParam "?tickMarkLabels" "string list" "[]" "Labels displayed below tick mark positions"
          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the slider is interactive"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (color, etc.)"
        ]

        Helpers.apiTable "Slider.create (float)" [
          Helpers.apiParam "value" "Var<float>" "" "Two-way binding for the floating-point slider value"
          Helpers.apiParam "?min" "float" "0.0" "Minimum value of the slider range"
          Helpers.apiParam "?max" "float" "100.0" "Maximum value of the slider range"
          Helpers.apiParam "?step" "float" "1.0" "Step increment for keyboard and tick marks"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Label displayed above the slider"
          Helpers.apiParam "?showTickMarks" "bool" "false" "Show tick marks at each step position"
          Helpers.apiParam "?tickMarkLabels" "string list" "[]" "Labels displayed below tick mark positions"
          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the slider is interactive"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (color, etc.)"
        ]

        Helpers.styleModuleTable "Slider.Color" [
          ("primary", "Primary brand color for the slider fill and thumb")
          ("secondary", "Secondary brand color")
          ("tertiary", "Tertiary brand color")
          ("error", "Error/red color")
          ("warning", "Warning/orange color")
          ("success", "Success/green color")
          ("info", "Info/blue color")
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Slider"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Slider component lets users select a value from a continuous or discrete range by dragging a thumb along a track. It supports colors, step snapping, tick marks, labels, and keyboard navigation."
        ]

        Helpers.divider ()
        basicSliderExample ()
        Helpers.divider ()
        colorsExample ()
        Helpers.divider ()
        stepAndRangeExample ()
        Helpers.divider ()
        tickMarksExample ()
        Helpers.divider ()
        tickLabelsExample ()
        Helpers.divider ()
        floatSliderExample ()
        Helpers.divider ()
        disabledSliderExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
