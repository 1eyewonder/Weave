namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.Docs

[<JavaScript>]
module SliderExamples =

  let private comparisonGrid columns items =
    let span =
      match columns with
      | 2 -> GridItem.Span.Medium.six
      | 4 -> GridItem.Span.Medium.three
      | _ -> GridItem.Span.Medium.four

    Grid.create (
      items
      |> List.map (fun item ->
        GridItem.create (item, attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; span ])),
      attrs = [ AlignItems.start ]
    )

  let private basicSliderExample () =
    let description =
      Helpers.bodyText
        "A basic slider with default settings (0–100, step 1). The current value is displayed reactively."

    let content =
      let value = Var.Create 50

      Slider.create (value, labelText = (value.View |> View.MapCached(sprintf "Volume: %d")))

    let code =
      """let value = Var.Create 50

Slider.create (
  value,
  labelText = (value.View |> View.MapCached(sprintf "Volume: %d"))
)
"""

    Helpers.codeSampleSection "Basic Slider" description content code

  let appearance () =
    let settings = Var.Create SliderAppearance.defaults
    let value = Var.Create 40

    let selected =
      Var.Lens settings (fun current -> Some current.Color) (fun current selection ->
        match selection with
        | Some color -> { current with Color = color }
        | None -> current)

    let colors =
      SliderAppearance.colors
      |> List.map (fun color ->
        let label = color.Substring(0, 1).ToUpper() + color.Substring(1)
        SelectItem.create (text label, label, color))
      |> View.Const

    let colorControl =
      Select.create (
        colors,
        selected,
        labelText = View.Const "Color",
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let ticks =
      Var.Lens settings (fun current -> current.Ticks) (fun current enabled ->
        if enabled then
          let nearestStep = ((value.Value + 10) / 20) * 20
          Var.Set value (max 0 (min 100 nearestStep))

        { current with Ticks = enabled })

    let disabled =
      Var.Lens settings (fun current -> current.Disabled) (fun current enabled -> {
        current with
            Disabled = enabled
      })

    let attrs =
      Map.ofList [
        "primary", Slider.Color.primary
        "secondary", Slider.Color.secondary
        "tertiary", Slider.Color.tertiary
        "error", Slider.Color.error
        "warning", Slider.Color.warning
        "success", Slider.Color.success
        "info", Slider.Color.info
      ]

    let preview =
      settings.View
      |> Doc.BindView(fun current ->
        Slider.create (
          value,
          step = (if current.Ticks then 20 else 1),
          showTickMarks = current.Ticks,
          enabled = View.Const(not current.Disabled),
          labelText = View.Const "Volume",
          attrs = [ Map.find current.Color attrs ]
        ))

    let description =
      Helpers.bodyText
        "Choose a color, add tick marks at 20-point steps, or disable interaction. Drag the thumb or use arrow keys; the code reflects its current value."

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        div [ Attr.Style "grid-column" "1 / -1" ] [ colorControl ]
      ]

    let switches =
      div [ Attr.Class "docs-appearance__actions" ] [
        Switch.create (ticks, text "Tick marks")
        Switch.create (disabled, text "Disabled")
      ]

    let reset () =
      Var.Set settings SliderAppearance.defaults
      Var.Set value 40

    Helpers.appearanceSection
      [ "colors"; "tick-marks"; "disabled" ]
      description
      controls
      (Some switches)
      preview
      reset
      ((settings.View, value.View) ||> View.Map2 SliderAppearance.source)

  let private stepAndRangeExample () =
    let description =
      Helpers.bodyText
        "Customize the min, max, and step values. The slider snaps to the nearest step when dragged or when using keyboard arrows."

    let content =
      let tempValue = Var.Create 22

      let percentValue = Var.Create 50

      comparisonGrid 2 [
        Slider.warning (
          tempValue,
          min = 16,
          max = 30,
          step = 2,
          labelText = (tempValue.View |> View.MapCached(sprintf "Temperature: %d°C"))
        )

        Slider.info (
          percentValue,
          min = 0,
          max = 100,
          step = 10,
          labelText = (percentValue.View |> View.MapCached(sprintf "Opacity: %d%%"))
        )
      ]

    let code =
      """let tempValue = Var.Create 22

let percentValue = Var.Create 50

Slider.warning (
  tempValue,
  min = 16,
  max = 30,
  step = 2,
  labelText = (tempValue.View |> View.MapCached(sprintf "Temperature: %d°C"))
)

Slider.info (
  percentValue,
  min = 0,
  max = 100,
  step = 10,
  labelText = (percentValue.View |> View.MapCached(sprintf "Opacity: %d%%"))
)
"""

    Helpers.codeSampleSection "Step & Range" description content code

  let private tickLabelsExample () =
    let description =
      Helpers.bodyText
        "Add labels beneath the tick positions to give users context for each stop. Labels are evenly spaced across the track width."

    let content =
      let value = Var.Create 2

      Slider.secondary (
        value,
        min = 0,
        max = 4,
        step = 1,
        showTickMarks = true,
        tickMarkLabels = [ "XS"; "S"; "M"; "L"; "XL" ],
        labelText = View.Const "Size"
      )

    let code =
      """let value = Var.Create 2

Slider.secondary (
  value,
  min = 0,
  max = 4,
  step = 1,
  showTickMarks = true,
  tickMarkLabels = [ "XS"; "S"; "M"; "L"; "XL" ],
  labelText = View.Const "Size"
)
"""

    Helpers.codeSampleSection "Tick Labels" description content code

  let private floatSliderExample () =
    let description =
      Helpers.bodyText
        "The slider also supports floating-point values with fine-grained steps. Pass a Var<float> to use the float overload."

    let content =
      let value = Var.Create 0.5

      Slider.tertiary (
        value,
        min = 0.0,
        max = 1.0,
        step = 0.05,
        labelText = (value.View |> View.MapCached(sprintf "Opacity: %.2f"))
      )

    let code =
      """let value = Var.Create 0.5

Slider.tertiary (
  value,
  min = 0.0,
  max = 1.0,
  step = 0.05,
  labelText = (value.View |> View.MapCached(sprintf "Opacity: %.2f"))
)
"""

    Helpers.codeSampleSection "Float Values" description content code

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
        appearance ()
        Helpers.divider ()
        stepAndRangeExample ()
        Helpers.divider ()
        tickLabelsExample ()
        Helpers.divider ()
        floatSliderExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
