namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module NumericFieldExamples =

  let private centered content = Grid.create [ GridItem.create content ]

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
      attrs = [ AlignItems.end' ]
    )

  let private basicExample () =
    let description =
      Helpers.bodyText
        "Bind a Var<int> for whole numbers. Type a number or use the spin buttons to update the value."

    let content =
      let quantity = Var.Create 1

      NumericField.create (quantity, labelText = View.Const "Quantity")

    let code =
      """let quantity = Var.Create 1

NumericField.create (quantity, labelText = View.Const "Quantity")
"""

    Helpers.codeSampleSection "Basic Usage" description (centered content) code

  let private intVsFloatExample () =
    let description =
      Helpers.bodyText
        "NumericField has separate overloads for int and float, both named create (camelCase). F# disambiguates by the Var type: Var<int> selects the int overload, Var<float> selects the float overload."

    let content =
      let intVal = Var.Create 42

      let floatVal = Var.Create 3.14

      comparisonGrid 2 [
        NumericField.create (
          intVal,
          labelText = View.Const "Integer",
          attrs = [ NumericField.Variant.outlined ]
        )

        NumericField.create (
          floatVal,
          labelText = View.Const "Float",
          step = View.Const 0.1,
          attrs = [ NumericField.Variant.outlined ]
        )
      ]

    let code =
      """let intVal = Var.Create 42

let floatVal = Var.Create 3.14

NumericField.create (
  intVal,
  labelText = View.Const "Integer",
  attrs = [ NumericField.Variant.outlined ]
)

NumericField.create (
  floatVal,
  labelText = View.Const "Float",
  step = View.Const 0.1,
  attrs = [ NumericField.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Int vs Float" description content code

  let private minMaxStepExample () =
    let description =
      Helpers.bodyText
        "Use min, max, and step to constrain the allowed range and increment size. Values are clamped on blur and when using spin buttons, arrow keys, or the mouse wheel."

    let content =
      let quantity = Var.Create 5

      let opacity = Var.Create 0.5

      comparisonGrid 2 [
        NumericField.create (
          quantity,
          labelText = View.Const "Quantity (1-10, step 1)",
          min = 1,
          max = 10,
          step = View.Const 1,
          attrs = [ NumericField.Variant.outlined ]
        )

        NumericField.create (
          opacity,
          labelText = View.Const "Opacity (0.0-1.0, step 0.1)",
          min = 0.0,
          max = 1.0,
          step = View.Const 0.1,
          attrs = [ NumericField.Variant.outlined ]
        )
      ]

    let code =
      """let quantity = Var.Create 5

let opacity = Var.Create 0.5

NumericField.create (
  quantity,
  labelText = View.Const "Quantity (1-10, step 1)",
  min = 1,
  max = 10,
  step = View.Const 1,
  attrs = [ NumericField.Variant.outlined ]
)

NumericField.create (
  opacity,
  labelText = View.Const "Opacity (0.0-1.0, step 0.1)",
  min = 0.0,
  max = 1.0,
  step = View.Const 0.1,
  attrs = [ NumericField.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Min, Max & Step" description content code

  let private keyboardWheelExample () =
    let description =
      Helpers.bodyText
        "When focused, use Arrow Up / Arrow Down keys or the mouse wheel to increment and decrement. Both interactions are enabled by default but can be individually toggled off via enableArrowKeys and enableMouseWheel."

    let content =
      let createField arrows wheel label =
        NumericField.create (
          Var.Create 50,
          labelText = View.Const label,
          min = 0,
          max = 100,
          step = View.Const 5,
          enableArrowKeys = View.Const arrows,
          enableMouseWheel = View.Const wheel,
          attrs = [ NumericField.Variant.outlined ]
        )

      comparisonGrid 2 [
        createField true true "Both enabled (default)"

        createField false true "Arrow keys disabled"

        createField true false "Mouse wheel disabled"

        createField false false "Both disabled"
      ]

    let code =
      """let createField arrows wheel label =
  NumericField.create (
    Var.Create 50,
    labelText = View.Const label,
    min = 0,
    max = 100,
    step = View.Const 5,
    enableArrowKeys = View.Const arrows,
    enableMouseWheel = View.Const wheel,
    attrs = [ NumericField.Variant.outlined ]
  )

createField true true "Both enabled (default)"

createField false true "Arrow keys disabled"

createField true false "Mouse wheel disabled"

createField false false "Both disabled"
"""

    Helpers.codeSampleSection "Keyboard & Mouse Wheel" description content code

  let private customIconsExample () =
    let description =
      Helpers.bodyText
        "Override the default ▲/▼ icons via upIcon and downIcon. Any Doc can be used — here we use Material Symbols."

    let content =
      let value = Var.Create 0

      NumericField.create (
        value,
        labelText = View.Const "Custom icons",
        upIcon =
          Icon.create (Icon.Hardware Hardware.KeyboardArrowUp, attrs = [ Attr.Style "font-size" "1rem" ]),
        downIcon =
          Icon.create (Icon.Hardware Hardware.KeyboardArrowDown, attrs = [ Attr.Style "font-size" "1rem" ]),
        attrs = [ NumericField.Variant.outlined ]
      )

    let code =
      """open Weave.Icons.MaterialSymbols

let value = Var.Create 0

NumericField.create (
  value,
  labelText = View.Const "Custom icons",
  upIcon =
    Icon.create (
      Icon.Hardware Hardware.KeyboardArrowUp,
      attrs = [ Attr.Style "font-size" "1rem" ]
    ),
  downIcon =
    Icon.create (
      Icon.Hardware Hardware.KeyboardArrowDown,
      attrs = [ Attr.Style "font-size" "1rem" ]
    ),
  attrs = [ NumericField.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Custom Spin Icons" description (centered content) code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Disabled fields are non-interactive and visually dimmed. Read-only fields display the value but prevent edits; spin buttons and keyboard/wheel interactions are suppressed."

    let content =
      let disabledVal = Var.Create 42

      let readOnlyVal = Var.Create 99

      comparisonGrid 2 [
        NumericField.create (
          disabledVal,
          labelText = View.Const "Disabled",
          enabled = View.Const false,
          attrs = [ NumericField.Variant.outlined ]
        )

        NumericField.create (
          readOnlyVal,
          labelText = View.Const "Read Only",
          readOnly = View.Const true,
          attrs = [ NumericField.Variant.outlined ]
        )
      ]

    let code =
      """let disabledVal = Var.Create 42

let readOnlyVal = Var.Create 99

NumericField.create (
  disabledVal,
  labelText = View.Const "Disabled",
  enabled = View.Const false,
  attrs = [ NumericField.Variant.outlined ]
)

NumericField.create (
  readOnlyVal,
  labelText = View.Const "Read Only",
  readOnly = View.Const true,
  attrs = [ NumericField.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Disabled & Read Only" description content code

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "Complete API reference for NumericField. The create overloads accept integer or floating-point values, bound through Var or FormField.")
      [
        Helpers.returnTypeNote
          "NumericField.create also accepts FormField directly in place of the bound Var, with the same optional parameters except validation, which is wired automatically. The explicit validation parameter below applies to Var bindings."
        Helpers.apiTable "NumericField.create (int)" [
          Helpers.apiParam "value" "Var<int> or FormField<int>" "" "Bind a Var or a validated form field"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Floating label displayed above the input"
          Helpers.apiParam "?placeholder" "View<string>" "\"\"" "Placeholder text when the field is empty"
          Helpers.apiParam "?showHelpText" "View<bool>" "" "Whether to display the help text area"
          Helpers.apiParam
            "?validation"
            "FieldValidation"
            ""
            "FormField.Validation: show errors after blur or submit; overrides help text while invalid"
          Helpers.apiParam "?helpText" "Doc" "" "Content shown below the field when showHelpText is true"
          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the field is interactive"
          Helpers.apiParam
            "?readOnly"
            "View<bool>"
            "View.Const false"
            "Display the value without allowing changes"
          Helpers.apiParam "?startAdornment" "Doc" "" "Content placed before the input (e.g. currency symbol)"
          Helpers.apiParam "?min" "int" "Int32.MinValue" "Minimum allowed value (clamped on blur and spin)"
          Helpers.apiParam "?max" "int" "Int32.MaxValue" "Maximum allowed value (clamped on blur and spin)"
          Helpers.apiParam
            "?step"
            "View<int>"
            "View.Const 1"
            "Increment/decrement step for spin buttons and arrow keys"
          Helpers.apiParam "?showSpinButtons" "View<bool>" "View.Const true" "Show the up/down spin buttons"
          Helpers.apiParam
            "?enableArrowKeys"
            "View<bool>"
            "View.Const true"
            "Allow ArrowUp/ArrowDown to increment/decrement"
          Helpers.apiParam
            "?enableMouseWheel"
            "View<bool>"
            "View.Const true"
            "Allow mouse wheel to increment/decrement when focused"
          Helpers.apiParam "?upIcon" "Doc" "text \"▲\"" "Custom icon for the increment spin button"
          Helpers.apiParam "?downIcon" "Doc" "text \"▼\"" "Custom icon for the decrement spin button"
          Helpers.apiParam
            "?inputAttrs"
            "Attr list"
            "[]"
            "Additional attributes applied to the inner input element"
          Helpers.apiParam "?typoAttrs" "Attr list" "" "Typography attributes applied to the field wrapper"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
        ]

        Helpers.apiTable "NumericField.create (float)" [
          Helpers.apiParam "value" "Var<float>" "" "Two-way binding for the floating-point value"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Floating label displayed above the input"
          Helpers.apiParam "?placeholder" "View<string>" "\"\"" "Placeholder text when the field is empty"
          Helpers.apiParam "?showHelpText" "View<bool>" "" "Whether to display the help text area"
          Helpers.apiParam
            "?validation"
            "FieldValidation"
            ""
            "FormField.Validation: show errors after blur or submit; overrides help text while invalid"
          Helpers.apiParam "?helpText" "Doc" "" "Content shown below the field when showHelpText is true"
          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the field is interactive"
          Helpers.apiParam
            "?readOnly"
            "View<bool>"
            "View.Const false"
            "Display the value without allowing changes"
          Helpers.apiParam "?startAdornment" "Doc" "" "Content placed before the input (e.g. currency symbol)"
          Helpers.apiParam "?min" "float" "-infinity" "Minimum allowed value (clamped on blur and spin)"
          Helpers.apiParam "?max" "float" "infinity" "Maximum allowed value (clamped on blur and spin)"
          Helpers.apiParam
            "?step"
            "View<float>"
            "View.Const 1.0"
            "Increment/decrement step for spin buttons and arrow keys"
          Helpers.apiParam "?showSpinButtons" "View<bool>" "View.Const true" "Show the up/down spin buttons"
          Helpers.apiParam
            "?enableArrowKeys"
            "View<bool>"
            "View.Const true"
            "Allow ArrowUp/ArrowDown to increment/decrement"
          Helpers.apiParam
            "?enableMouseWheel"
            "View<bool>"
            "View.Const true"
            "Allow mouse wheel to increment/decrement when focused"
          Helpers.apiParam "?upIcon" "Doc" "text \"▲\"" "Custom icon for the increment spin button"
          Helpers.apiParam "?downIcon" "Doc" "text \"▼\"" "Custom icon for the decrement spin button"
          Helpers.apiParam
            "?inputAttrs"
            "Attr list"
            "[]"
            "Additional attributes applied to the inner input element"
          Helpers.apiParam "?typoAttrs" "Attr list" "" "Typography attributes applied to the field wrapper"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
        ]

        Helpers.styleModuleTable "NumericField.Variant (shared)" [
          ("standard", "Underline-only input style (default)")
          ("filled", "Filled background with underline")
          ("outlined", "Bordered outline with floating label notch")
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Numeric Field"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "NumericField wraps Field to provide a typed numeric input with spin buttons, keyboard arrow keys, mouse wheel support, and optional min/max/step constraints. Separate overloads exist for int and float values."
        ]
        Helpers.divider ()
        basicExample ()
        NumericFieldConfigurator.render ()
        Helpers.divider ()
        intVsFloatExample ()
        Helpers.divider ()
        minMaxStepExample ()
        Helpers.divider ()
        keyboardWheelExample ()
        Helpers.divider ()
        customIconsExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
