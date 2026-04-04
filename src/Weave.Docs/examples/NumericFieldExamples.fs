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

  let private variantsExample () =
    let stdVal = Var.Create 0
    let filledVal = Var.Create 0
    let outlinedVal = Var.Create 0

    let description =
      Helpers.bodyText
        "NumericField supports the same three variants as Field: Standard, Filled, and Outlined. Spin buttons are shown by default as a right adornment."

    let content =
      Grid.create (
        [
          GridItem.create (
            NumericField.create (
              stdVal,
              labelText = View.Const "Standard",
              attrs = [ NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            NumericField.create (
              filledVal,
              labelText = View.Const "Filled",
              attrs = [ NumericField.Variant.filled; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            NumericField.create (
              outlinedVal,
              labelText = View.Const "Outlined",
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let stdVal = Var.Create 0
let filledVal = Var.Create 0
let outlinedVal = Var.Create 0

NumericField.create(
    stdVal,
    labelText = View.Const "Standard"
)

NumericField.create(
    filledVal,
    labelText = View.Const "Filled",
    attrs = [ NumericField.Variant.filled ]
)

NumericField.create(
    outlinedVal,
    labelText = View.Const "Outlined",
    attrs = [ NumericField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private intVsFloatExample () =
    let intVal = Var.Create 42
    let floatVal = Var.Create 3.14

    let description =
      Helpers.bodyText
        "NumericField has separate overloads for int and float, both named create (camelCase). F# disambiguates by the Var type: Var<int> selects the int overload, Var<float> selects the float overload."

    let content =
      Grid.create (
        [
          GridItem.create (
            NumericField.create (
              intVal,
              labelText = View.Const "Integer",
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            NumericField.create (
              floatVal,
              labelText = View.Const "Float",
              step = View.Const 0.1,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

// Integer field — step defaults to 1
let intVal = Var.Create 42

NumericField.create(
    intVal,
    labelText = View.Const "Integer",
    attrs = [ NumericField.Variant.outlined ]
)

// Float field — same create name, F# resolves by Var<float>
let floatVal = Var.Create 3.14

NumericField.create(  // see here — Var<float> selects the float overload
    floatVal,
    labelText = View.Const "Float",
    step = View.Const 0.1,
    attrs = [ NumericField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Int vs Float" description content code

  let private minMaxStepExample () =
    let value = Var.Create 5

    let description =
      Helpers.bodyText
        "Use min, max, and step to constrain the allowed range and increment size. Values are clamped on blur and when using spin buttons, arrow keys, or the mouse wheel."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              NumericField.create (
                value,
                labelText = View.Const "Quantity (1–10, step 1)",
                min = 1,
                max = 10,
                step = View.Const 1,
                attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
              )

              value.View
              |> Doc.BindView(fun v ->
                div [ Typography.body2; Margin.Top.extraSmall ] [ text $"Current value: %i{v}" ])
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            (let floatVal = Var.Create 0.5

             div [] [
               NumericField.create (
                 floatVal,
                 labelText = View.Const "Opacity (0.0\u20131.0, step 0.1)",
                 min = 0.0,
                 max = 1.0,
                 step = View.Const 0.1,
                 attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
               )

               floatVal.View
               |> Doc.BindView(fun v ->
                 div [ Typography.body2; Margin.Top.extraSmall ] [ text $"Current value: %f{v}" ])
             ]),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

// Integer with min/max
let quantity = Var.Create 5

NumericField.create(
    quantity,
    labelText = View.Const "Quantity (1-10, step 1)",
    min = 1,
    max = 10,
    step = View.Const 1,
    attrs = [ NumericField.Variant.outlined ]
)

// Float with min/max
let opacity = Var.Create 0.5

NumericField.create(
    opacity,
    labelText = View.Const "Opacity (0.0-1.0, step 0.1)",
    min = 0.0,
    max = 1.0,
    step = View.Const 0.1,
    attrs = [ NumericField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Min, Max & Step" description content code

  let private keyboardWheelExample () =
    let enabledVal = Var.Create 50
    let noArrowsVal = Var.Create 50
    let noWheelVal = Var.Create 50
    let neitherVal = Var.Create 50

    let description =
      Helpers.bodyText
        "When focused, use Arrow Up / Arrow Down keys or the mouse wheel to increment and decrement. Both interactions are enabled by default but can be individually toggled off via enableArrowKeys and enableMouseWheel."

    let content =
      Grid.create (
        [
          GridItem.create (
            NumericField.create (
              enabledVal,
              labelText = View.Const "Both enabled (default)",
              min = 0,
              max = 100,
              step = View.Const 5,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            NumericField.create (
              noArrowsVal,
              labelText = View.Const "Arrow keys disabled",
              min = 0,
              max = 100,
              step = View.Const 5,
              enableArrowKeys = View.Const false,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            NumericField.create (
              noWheelVal,
              labelText = View.Const "Mouse wheel disabled",
              min = 0,
              max = 100,
              step = View.Const 5,
              enableMouseWheel = View.Const false,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            NumericField.create (
              neitherVal,
              labelText = View.Const "Both disabled",
              min = 0,
              max = 100,
              step = View.Const 5,
              enableArrowKeys = View.Const false,
              enableMouseWheel = View.Const false,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create 50

// Both enabled (default)
NumericField.create(
    value,
    labelText = View.Const "Both enabled (default)",
    min = 0,
    max = 100,
    step = View.Const 5,
    attrs = [ NumericField.Variant.outlined ]
)

// Disable arrow key stepping
NumericField.create(
    value,
    labelText = View.Const "Arrow keys disabled",
    enableArrowKeys = View.Const false,  // see here
    attrs = [ NumericField.Variant.outlined ]
)

// Disable mouse wheel stepping
NumericField.create(
    value,
    labelText = View.Const "Mouse wheel disabled",
    enableMouseWheel = View.Const false,  // see here
    attrs = [ NumericField.Variant.outlined ]
)

// Disable both
NumericField.create(
    value,
    labelText = View.Const "Both disabled",
    enableArrowKeys = View.Const false,
    enableMouseWheel = View.Const false,
    attrs = [ NumericField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Keyboard & Mouse Wheel" description content code

  let private hideSpinButtonsExample () =
    let withSpin = Var.Create 10
    let withoutSpin = Var.Create 10

    let description =
      Helpers.bodyText
        "Spin buttons are visible by default. Set showSpinButtons to false to hide them. Arrow key and mouse wheel interaction still works."

    let content =
      Grid.create (
        [
          GridItem.create (
            NumericField.create (
              withSpin,
              labelText = View.Const "With spin buttons",
              showSpinButtons = View.Const true,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            NumericField.create (
              withoutSpin,
              labelText = View.Const "Without spin buttons",
              showSpinButtons = View.Const false,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let withSpin = Var.Create 10
let withoutSpin = Var.Create 10

// Default — spin buttons visible
NumericField.create(
    withSpin,
    labelText = View.Const "With spin buttons",
    showSpinButtons = View.Const true,
    attrs = [ NumericField.Variant.outlined ]
)

// Hidden spin buttons
NumericField.create(
    withoutSpin,
    labelText = View.Const "Without spin buttons",
    showSpinButtons = View.Const false,  // see here
    attrs = [ NumericField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Show / Hide Spin Buttons" description content code

  let private customIconsExample () =
    let value = Var.Create 0

    let description =
      Helpers.bodyText
        "Override the default ▲/▼ icons via upIcon and downIcon. Any Doc can be used — here we use Material Symbols."

    let content =
      NumericField.create (
        value,
        labelText = View.Const "Custom icons",
        upIcon =
          Icon.create (Icon.Hardware Hardware.KeyboardArrowUp, attrs = [ Attr.Style "font-size" "1rem" ]),
        downIcon =
          Icon.create (Icon.Hardware Hardware.KeyboardArrowDown, attrs = [ Attr.Style "font-size" "1rem" ]),
        attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
      )

    let code =
      """open Weave
open WebSharper.UI
open Weave.Icons
open Weave.Icons.MaterialSymbols

let value = Var.Create 0

NumericField.create(
    value,
    labelText = View.Const "Custom icons",
    upIcon = Icon.create(  // see here
        Icon.Hardware Hardware.KeyboardArrowUp,
        attrs = [ Attr.Style "font-size" "1rem" ]
    ),
    downIcon = Icon.create(
        Icon.Hardware Hardware.KeyboardArrowDown,
        attrs = [ Attr.Style "font-size" "1rem" ]
    ),
    attrs = [ NumericField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Custom Spin Icons" description content code

  let private disabledExample () =
    let disabledVal = Var.Create 42
    let readOnlyVal = Var.Create 99

    let description =
      Helpers.bodyText
        "Disabled fields are non-interactive and visually dimmed. Read-only fields display the value but prevent edits; spin buttons and keyboard/wheel interactions are suppressed."

    let content =
      Grid.create (
        [
          GridItem.create (
            NumericField.create (
              disabledVal,
              labelText = View.Const "Disabled",
              enabled = View.Const false,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            NumericField.create (
              readOnlyVal,
              labelText = View.Const "Read Only",
              readOnly = View.Const true,
              attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let disabledVal = Var.Create 42
let readOnlyVal = Var.Create 99

// Disabled — non-interactive and visually dimmed
NumericField.create(
    disabledVal,
    labelText = View.Const "Disabled",
    enabled = View.Const false,  // see here
    attrs = [ NumericField.Variant.outlined ]
)

// Read Only — displays value but prevents edits
NumericField.create(
    readOnlyVal,
    labelText = View.Const "Read Only",
    readOnly = View.Const true,  // see here
    attrs = [ NumericField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Disabled & Read Only" description content code

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "Complete API reference for NumericField. Two overloads of create exist, disambiguated by parameter type: Var<int> for integers and Var<float> for floating-point values.")
      [
        Helpers.apiTable "NumericField.create (int)" [
          Helpers.apiParam "value" "Var<int>" "" "Two-way binding for the integer value"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Floating label displayed above the input"
          Helpers.apiParam "?placeholder" "View<string>" "\"\"" "Placeholder text when the field is empty"
          Helpers.apiParam "?showHelpText" "View<bool>" "" "Whether to display the help text area"
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
        variantsExample ()
        Helpers.divider ()
        intVsFloatExample ()
        Helpers.divider ()
        minMaxStepExample ()
        Helpers.divider ()
        keyboardWheelExample ()
        Helpers.divider ()
        hideSpinButtonsExample ()
        Helpers.divider ()
        customIconsExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
