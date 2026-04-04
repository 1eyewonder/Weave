namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TextFieldExamples =

  let private whenToUseSection () =
    let description =
      div [ Typography.body1 ] [
        text "Both "
        Helpers.inlineCode "TextField"
        text " and "
        Helpers.inlineCode "NumericField"
        text " handle user input — but they target different value types. Use this to pick the right one."
      ]

    let content =
      Helpers.guidanceColumns
        (Helpers.guidanceCard "Use TextField when\u2026" [
          Helpers.guidanceBullet
            "Input accepts freeform text"
            "names, emails, descriptions, and search queries are all string-typed inputs."
          Helpers.guidanceBullet
            "You need validation or character limits"
            "built-in maxLength counter, debounce callbacks, and reactive help text cover common validation patterns."
          Helpers.guidanceBullet
            "The value type is string"
            "TextField binds a Var<string> and handles focus, float, and placeholder logic for you."
          Helpers.guidanceBullet
            "Multiline input is needed"
            "use TextField.multiLine to render a resizable textarea."
        ])
        (Helpers.guidanceCard "Use NumericField when\u2026" [
          Helpers.guidanceBullet
            "The value is an int or float with a range"
            "quantities, prices, and ratings have well-defined numeric bounds."
          Helpers.guidanceBullet
            "Spin buttons or keyboard stepping is expected"
            "arrow keys, mouse wheel, and +/\u2212 buttons increment in configurable steps."
          Helpers.guidanceBullet
            "Built-in clamping and step-snapping are needed"
            "values are always valid numbers within [min, max], snapped to the step grid."
          Helpers.guidanceBullet
            "Use Field (core) to wrap custom input elements"
            "Field accepts any Doc as its input element for cases TextField does not cover."
        ])

    Helpers.sectionPlain "When to Use" description content

  let private variantsExample () =
    let stdVal = Var.Create ""
    let filledVal = Var.Create ""
    let outlinedVal = Var.Create ""

    let description =
      Helpers.bodyText
        "TextField supports three variants: Standard (bottom border), Filled (background + bottom border), and Outlined (full border)."

    let content =
      Grid.create (
        [
          GridItem.create (
            TextField.singleLine (stdVal, labelText = View.Const "Standard", attrs = [ TextField.Width.full ]),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            TextField.singleLine (
              filledVal,
              labelText = View.Const "Filled",
              attrs = [ TextField.Variant.filled; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            TextField.singleLine (
              outlinedVal,
              labelText = View.Const "Outlined",
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

TextField.singleLine(
    value,
    labelText = View.Const "Standard"
)

TextField.singleLine(
    value,
    labelText = View.Const "Filled",
    attrs = [ TextField.Variant.filled ]
)

TextField.singleLine(
    value,
    labelText = View.Const "Outlined",  // see here
    attrs = [ TextField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private withContentExample () =
    let stdVal = Var.Create "Some content here"
    let filledVal = Var.Create "Some content here"
    let outlinedVal = Var.Create "Some content here"

    let description =
      Helpers.bodyText "When the field has a value the label floats up and shrinks automatically."

    let content =
      Grid.create (
        [
          GridItem.create (
            TextField.singleLine (stdVal, labelText = View.Const "Standard", attrs = [ TextField.Width.full ]),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            TextField.singleLine (
              filledVal,
              labelText = View.Const "Filled",
              attrs = [ TextField.Variant.filled; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            TextField.singleLine (
              outlinedVal,
              labelText = View.Const "Outlined",
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

// When the Var has a value, the label automatically floats
let value = Var.Create "Some content here"

TextField.singleLine(
    value,
    labelText = View.Const "Outlined",
    attrs = [ TextField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "With Content" description content code

  let private adornmentsExample () =
    let startStdVal = Var.Create ""
    let startFilledVal = Var.Create ""
    let startOutlinedVal = Var.Create ""
    let endStdVal = Var.Create ""
    let endFilledVal = Var.Create ""
    let endOutlinedVal = Var.Create ""
    let bothStdVal = Var.Create ""
    let bothFilledVal = Var.Create ""
    let bothOutlinedVal = Var.Create ""

    let description =
      Helpers.bodyText
        "Icons or other content can be placed at the start, end, or both ends of the field. All three adornment placements are shown across every variant."

    let mkItem
      variantAttr
      label
      (startAdornment: unit -> Doc option)
      (endAdornment: unit -> Doc option)
      value
      =
      GridItem.create (
        TextField.singleLine (
          value,
          labelText = View.Const label,
          ?startAdornment = startAdornment (),
          ?endAdornment = endAdornment (),
          attrs = [ yield! variantAttr; TextField.Width.full ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
      )

    let searchIcon () =
      Some(Icon.create (Icon.UiActions UiActions.Search, attrs = [ BrandColor.TextColor.primary ]))

    let checkIcon () =
      Some(Icon.create (Icon.UiActions UiActions.CheckCircle, attrs = [ BrandColor.TextColor.success ]))

    let infoIcon () =
      Some(Icon.create (Icon.Action Action.Info, attrs = [ BrandColor.TextColor.info ]))

    let warningIcon () =
      Some(Icon.create (Icon.Action Action.Warning, attrs = [ BrandColor.TextColor.warning ]))

    let noIcon () = None

    let content =
      div [] [
        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Start Adornment" ]

        Grid.create (
          [
            mkItem [] "Standard" searchIcon noIcon startStdVal
            mkItem [ TextField.Variant.filled ] "Filled" searchIcon noIcon startFilledVal
            mkItem [ TextField.Variant.outlined ] "Outlined" searchIcon noIcon startOutlinedVal
          ],
          attrs = [ Grid.Spacing.large ]
        )

        div [ Typography.body1; Margin.Vertical.extraSmall ] [ text "End Adornment" ]

        Grid.create (
          [
            mkItem [] "Standard" noIcon checkIcon endStdVal
            mkItem [ TextField.Variant.filled ] "Filled" noIcon infoIcon endFilledVal
            mkItem [ TextField.Variant.outlined ] "Outlined" noIcon warningIcon endOutlinedVal
          ],
          attrs = [ Grid.Spacing.large ]
        )

        div [ Typography.body1; Margin.Vertical.extraSmall ] [ text "Both Adornments" ]

        Grid.create (
          [
            mkItem [] "Standard" searchIcon checkIcon bothStdVal
            mkItem [ TextField.Variant.filled ] "Filled" searchIcon infoIcon bothFilledVal
            mkItem [ TextField.Variant.outlined ] "Outlined" searchIcon warningIcon bothOutlinedVal
          ],
          attrs = [ Grid.Spacing.large ]
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

let value = Var.Create ""

// Start adornment only
TextField.singleLine(
    value,
    labelText = View.Const "Search",
    startAdornment = Icon.create(  // see here
        Icon.UiActions UiActions.Search,
        attrs = [ BrandColor.TextColor.primary ]
    ),
    attrs = [ TextField.Variant.outlined ]
)

// End adornment only
TextField.singleLine(
    value,
    labelText = View.Const "Status",
    endAdornment = Icon.create(
        Icon.UiActions UiActions.CheckCircle,
        attrs = [ BrandColor.TextColor.success ]
    ),
    attrs = [ TextField.Variant.outlined ]
)

// Both start and end adornments
TextField.singleLine(
    value,
    labelText = View.Const "Search",
    startAdornment = Icon.create(
        Icon.UiActions UiActions.Search,
        attrs = [ BrandColor.TextColor.primary ]
    ),
    endAdornment = Icon.create(
        Icon.UiActions UiActions.CheckCircle,
        attrs = [ BrandColor.TextColor.success ]
    ),
    attrs = [ TextField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Adornments" description content code

  let private placeholderExample () =
    let value = Var.Create ""

    let description =
      Helpers.bodyText
        "When an explicit placeholder is provided the label stays floated and the placeholder text is shown in the input area."

    let content =
      TextField.singleLine (
        value,
        labelText = View.Const "With Placeholder",
        placeholder = View.Const "Type something here\u2026",
        attrs = [ TextField.Variant.outlined; TextField.Width.full ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

TextField.singleLine(
    value,
    labelText = View.Const "With Placeholder",
    placeholder = View.Const "Type something here…",  // see here
    attrs = [ TextField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Placeholder" description content code

  let private shrinkLabelExample () =
    let value = Var.Create ""

    let description =
      Helpers.bodyText
        "Use shrinkLabel to force the label into its floated position regardless of focus or value."

    let content =
      TextField.singleLine (
        value,
        labelText = View.Const "ShrinkLabel Override",
        shrinkLabel = View.Const true,
        endAdornment = Icon.create (Icon.Action Action.Info, attrs = [ BrandColor.TextColor.info ]),
        attrs = [ TextField.Variant.outlined; TextField.Width.full ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

TextField.singleLine(
    value,
    labelText = View.Const "ShrinkLabel Override",
    shrinkLabel = View.Const true,  // see here
    attrs = [ TextField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Shrink Label Override" description content code

  let private helpTextValidationExample () =
    let value = Var.Create ""

    let showHelpText =
      value.View
      |> View.MapCached(fun v -> not (System.String.IsNullOrEmpty(v)) && v.Length < 3)

    let helpTextContent =
      showHelpText
      |> View.MapCached(fun show -> if show then "Must be at least 3 characters" else "")

    let helpText =
      FieldHelpText.create (Doc.TextView helpTextContent, attrs = [ FieldHelpText.Color.error ])

    let description =
      Helpers.bodyText
        "Help text animates in and out based on a View<bool>. Here the help text appears when the value is non-empty but less than 3 characters."

    let content =
      TextField.singleLine (
        value,
        labelText = View.Const "Name",
        showHelpText = showHelpText,
        helpText = helpText,
        attrs = [
          TextField.Variant.outlined
          TextField.Width.full
          Attr.DynamicClassPred "weave-field--error" showHelpText
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

// Derive visibility from validation logic
let showHelpText =
    value.View
    |> View.MapCached(fun v ->
        not (System.String.IsNullOrEmpty(v)) && v.Length < 3)

let helpTextContent =
    showHelpText
    |> View.MapCached(fun show ->
        if show then "Must be at least 3 characters" else "")

let helpText =
    FieldHelpText.create(
        Doc.TextView helpTextContent,
        attrs = [ FieldHelpText.Color.error ]  // see here
    )

TextField.singleLine(
    value,
    showHelpText = showHelpText,
    labelText = View.Const "Name",
    helpText = helpText,
    attrs = [
        Attr.DynamicClassPred "weave-field--error" showHelpText
    ]
)"""

    Helpers.codeSampleSection "Help Text (Validation)" description content code

  let private helpTextColorVariationsExample () =
    let successVal = Var.Create "Looks good!"
    let warningVal = Var.Create "password"
    let errorVal = Var.Create "not-an-email"

    let description =
      Helpers.bodyText
        "Help text can be styled with any brand color to convey success, warning, error, or informational states. Apply color classes to both the field and FieldHelpText independently."

    let content =
      Grid.create (
        [
          GridItem.create (
            TextField.singleLine (
              successVal,
              showHelpText = View.Const true,
              labelText = View.Const "Username",
              helpText =
                FieldHelpText.create (text "Username is available", attrs = [ FieldHelpText.Color.success ]),
              attrs = [ TextField.Variant.outlined; TextField.Width.full; TextField.Color.success ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            TextField.singleLine (
              warningVal,
              showHelpText = View.Const true,
              labelText = View.Const "Password",
              helpText =
                FieldHelpText.create (
                  text "Consider a stronger password",
                  attrs = [ FieldHelpText.Color.warning ]
                ),
              attrs = [ TextField.Variant.outlined; TextField.Width.full; TextField.Color.warning ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )

          GridItem.create (
            TextField.singleLine (
              errorVal,
              showHelpText = View.Const true,
              labelText = View.Const "Email",
              helpText =
                FieldHelpText.create (text "Invalid email address", attrs = [ FieldHelpText.Color.error ]),
              attrs = [ TextField.Variant.outlined; TextField.Width.full; TextField.Color.error ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create "Looks good!"

// Success help text
TextField.singleLine(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Username",
    helpText =
        FieldHelpText.create(
            text "Username is available",
            attrs = [ FieldHelpText.Color.success ]  // see here
        ),
    attrs = [ TextField.Color.success ]
)

// Warning help text
TextField.singleLine(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Password",
    helpText =
        FieldHelpText.create(
            text "Consider a stronger password",
            attrs = [ FieldHelpText.Color.warning ]
        ),
    attrs = [ TextField.Color.warning ]
)

// Error help text
TextField.singleLine(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Email",
    helpText =
        FieldHelpText.create(
            text "Invalid email address",
            attrs = [ FieldHelpText.Color.error ]
        ),
    attrs = [ TextField.Color.error ]
)"""

    Helpers.codeSampleSection "Help Text Color Variations" description content code

  let private disabledExample () =
    let disabledVal = Var.Create "Cannot edit"
    let readOnlyVal = Var.Create "Read-only value"

    let description =
      Helpers.bodyText "Fields can be disabled or set to read-only to prevent user input."

    let content =
      Grid.create (
        [
          GridItem.create (
            TextField.singleLine (
              disabledVal,
              labelText = View.Const "Disabled",
              enabled = View.Const false,
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            TextField.singleLine (
              readOnlyVal,
              labelText = View.Const "Read Only",
              readOnly = View.Const true,
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let disabledVal = Var.Create "Cannot edit"
let readOnlyVal = Var.Create "Read-only value"

// Disabled — non-interactive and visually dimmed
TextField.singleLine(
    disabledVal,
    labelText = View.Const "Disabled",
    enabled = View.Const false  // see here
)

// Read Only — displays value but prevents edits
TextField.singleLine(
    readOnlyVal,
    labelText = View.Const "Read Only",
    readOnly = View.Const true  // see here
)"""

    Helpers.codeSampleSection "Disabled & Read Only" description content code

  let private colorExample () =
    let mkField colorAttr label =
      let v = Var.Create ""

      GridItem.create (
        TextField.singleLine (
          v,
          labelText = View.Const label,
          attrs = [ TextField.Variant.outlined; TextField.Width.full; colorAttr ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
      )

    let description =
      Helpers.bodyText "The accent color used when the field is focused can be customised with color classes."

    let content =
      Grid.create (
        [
          mkField TextField.Color.primary "Primary"
          mkField TextField.Color.secondary "Secondary"
          mkField TextField.Color.tertiary "Tertiary"
          mkField TextField.Color.success "Success"
          mkField TextField.Color.warning "Warning"
          mkField TextField.Color.error "Error"
          mkField TextField.Color.info "Info"
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

TextField.singleLine(value, labelText = View.Const "Primary", attrs = [ TextField.Color.primary ])
TextField.singleLine(value, labelText = View.Const "Secondary", attrs = [ TextField.Color.secondary ])
TextField.singleLine(value, labelText = View.Const "Tertiary", attrs = [ TextField.Color.tertiary ])
TextField.singleLine(value, labelText = View.Const "Success", attrs = [ TextField.Color.success ])
TextField.singleLine(value, labelText = View.Const "Warning", attrs = [ TextField.Color.warning ])
TextField.singleLine(value, labelText = View.Const "Error", attrs = [ TextField.Color.error ])
TextField.singleLine(value, labelText = View.Const "Info", attrs = [ TextField.Color.info ])"""

    Helpers.codeSampleSection "Colors" description content code

  let private typographyExample () =
    let captionVal = Var.Create ""
    let body1Val = Var.Create ""
    let subtitle1Val = Var.Create ""
    let h6Val = Var.Create ""

    let description =
      Helpers.bodyText
        "Use the typoAttrs parameter to configure the field's font styling. Because the component uses em-based sizing, the entire field — label, input, and chrome — scales proportionally with the chosen typography class."

    let content =
      Grid.create (
        [
          GridItem.create (
            TextField.singleLine (
              captionVal,
              labelText = View.Const "Caption",
              typoAttrs = [ Typography.caption ],
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            TextField.singleLine (
              body1Val,
              labelText = View.Const "Body1",
              typoAttrs = [ Typography.body1 ],
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            TextField.singleLine (
              subtitle1Val,
              labelText = View.Const "Subtitle1",
              typoAttrs = [ Typography.subtitle1 ],
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            TextField.singleLine (
              h6Val,
              labelText = View.Const "H6",
              typoAttrs = [ Typography.h6 ],
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

// Scale down with Caption typography
TextField.singleLine(
    value,
    labelText = View.Const "Caption",
    typoAttrs = [ Typography.caption ]  // see here
)

// Scale up with Subtitle1 typography
TextField.singleLine(
    value,
    labelText = View.Const "Subtitle1",
    typoAttrs = [ Typography.subtitle1 ]
)"""

    Helpers.codeSampleSection "Typography Configuration" description content code

  let private maxLengthExample () =
    let value = Var.Create ""

    let description =
      Helpers.bodyText
        "Pass maxLength to enforce a character limit and render a counter below the field showing the current length against the maximum."

    let content =
      div [] [
        TextField.singleLine (
          value,
          labelText = View.Const "Bio",
          maxLength = 100,
          attrs = [ TextField.Variant.outlined; TextField.Width.full ]
        )

        value.View
        |> Doc.BindView(fun v ->
          div [ Typography.body2; Margin.Top.extraSmall ] [ text $"Current length: %i{v.Length}" ])
      ]

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

TextField.singleLine(
    value,
    labelText = View.Const "Bio",
    maxLength = 100,  // see here — renders "0/100" counter below
    attrs = [ TextField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Max Length & Character Counter" description content code

  let private debounceExample () =
    let liveValue = Var.Create ""
    let debouncedDisplay = Var.Create ""

    let description =
      Helpers.bodyText
        "Use debounce with onChange to delay the callback until the user stops typing. The live value updates immediately while the debounced value updates after the specified delay."

    let content =
      div [] [
        TextField.singleLine (
          liveValue,
          labelText = View.Const "Search",
          debounce = 500,
          onChange = (fun v -> Var.Set debouncedDisplay v),
          startAdornment =
            Icon.create (Icon.UiActions UiActions.Search, attrs = [ BrandColor.TextColor.primary ]),
          attrs = [ TextField.Variant.outlined; TextField.Width.full ]
        )

        Grid.create (
          [
            GridItem.create (
              div [ Typography.Align.center ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Live value" ]

                liveValue.View
                |> Doc.BindView(fun v ->
                  div [ Typography.body2; Typography.Family.mono ] [
                    text (if System.String.IsNullOrEmpty(v) then "(empty)" else v)
                  ])
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )

            GridItem.create (
              div [ Typography.Align.center ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Debounced value (500ms)" ]

                debouncedDisplay.View
                |> Doc.BindView(fun v ->
                  div [ Typography.body2; Typography.Family.mono ] [
                    text (if System.String.IsNullOrEmpty(v) then "(empty)" else v)
                  ])
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )
          ],
          attrs = [ Grid.Spacing.large; Margin.Top.extraSmall ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let liveValue = Var.Create ""
let debouncedDisplay = Var.Create ""

TextField.singleLine(
    liveValue,
    labelText = View.Const "Search",
    debounce = 500,  // see here — waits 500ms after last keystroke
    onChange = (fun v -> Var.Set debouncedDisplay v),
    attrs = [ TextField.Variant.outlined ]
)

// liveValue updates on every keystroke
// debouncedDisplay updates 500ms after the user stops typing"""

    Helpers.codeSampleSection "Debounce & onChange" description content code

  let private multilineExample () =
    let singleVal = Var.Create ""
    let multiVal = Var.Create ""

    let description =
      Helpers.bodyText
        "Use TextField.multiLine to render a textarea instead of a standard input. The textarea is vertically resizable by default."

    let content =
      Grid.create (
        [
          GridItem.create (
            TextField.singleLine (
              singleVal,
              labelText = View.Const "Single Line",
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )

          GridItem.create (
            TextField.multiLine (
              multiVal,
              labelText = View.Const "Multiline",
              attrs = [ TextField.Variant.outlined; TextField.Width.full ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
          )
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""

// Standard single-line input
TextField.singleLine(
    value,
    labelText = View.Const "Single Line",
    attrs = [ TextField.Variant.outlined ]
)

// Multiline textarea — vertically resizable
TextField.multiLine(
    value,
    labelText = View.Const "Multiline",
    attrs = [ TextField.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Multiline" description content code

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "Complete API reference for TextField and FieldHelpText. TextField provides two constructors — singleLine for standard text inputs and multiLine for textarea inputs. Both share the same parameter set and wrap Field's structural chrome.")
      [
        Helpers.apiTable "TextField.singleLine" [
          Helpers.apiParam "value" "Var<string>" "" "Two-way binding for the text input value"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Floating label displayed above the input"
          Helpers.apiParam
            "?placeholder"
            "View<string>"
            "\"\""
            "Placeholder text shown when the field is empty and label is floating"
          Helpers.apiParam "?showHelpText" "View<bool>" "" "Whether to display the help text area"
          Helpers.apiParam "?helpText" "Doc" "" "Content shown below the field when showHelpText is true"
          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the field is interactive"
          Helpers.apiParam
            "?readOnly"
            "View<bool>"
            "View.Const false"
            "Display the value without allowing changes"
          Helpers.apiParam
            "?shrinkLabel"
            "View<bool>"
            "View.Const false"
            "Force the label to always float above the input"
          Helpers.apiParam "?startAdornment" "Doc" "" "Content placed before the input (e.g. icon or prefix)"
          Helpers.apiParam "?endAdornment" "Doc" "" "Content placed after the input (e.g. icon or suffix)"
          Helpers.apiParam
            "?maxLength"
            "int"
            ""
            "Maximum character count — renders a counter below the field and sets the HTML maxlength attribute"
          Helpers.apiParam
            "?debounce"
            "int"
            ""
            "Delay in milliseconds before onChange fires after the last keystroke"
          Helpers.apiParam
            "?onChange"
            "string -> unit"
            ""
            "Callback invoked when the value changes (debounced if debounce is set)"
          Helpers.apiParam
            "?inputAttrs"
            "Attr list"
            "[]"
            "Additional attributes applied to the inner input element"
          Helpers.apiParam "?typoAttrs" "Attr list" "" "Typography attributes applied to the field wrapper"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
        ]

        Helpers.returnTypeNote
          "TextField.multiLine accepts the same parameters as TextField.singleLine and renders a vertically resizable textarea instead of a single-line input."

        Helpers.apiTable "FieldHelpText.create" [
          Helpers.apiParam "content" "Doc" "" "Help text content displayed below the field"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the help text element"
        ]

        Helpers.styleModuleTable "TextField.Variant (shared)" [
          ("standard", "Underline-only input style (default)")
          ("filled", "Filled background with underline")
          ("outlined", "Bordered outline with floating label notch")
        ]

        Helpers.styleModuleTable "TextField.Color (shared)" [
          ("primary", "Primary brand color accent")
          ("secondary", "Secondary brand color accent")
          ("tertiary", "Tertiary brand color accent")
          ("error", "Error/red accent — use for validation errors")
          ("warning", "Warning/orange accent")
          ("success", "Success/green accent")
          ("info", "Info/blue accent")
        ]

        Helpers.styleModuleTable "TextField.Width (shared)" [
          ("full", "Field stretches to fill its container width")
        ]

        Helpers.styleModuleTable "FieldHelpText.Color" [
          ("primary", "Primary color for help text")
          ("secondary", "Secondary color for help text")
          ("tertiary", "Tertiary color for help text")
          ("error", "Error/red color for help text — use for validation messages")
          ("warning", "Warning/orange color for help text")
          ("success", "Success/green color for help text")
          ("info", "Info/blue color for help text")
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Text Field"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "TextField creates standard text inputs with floating labels, validation, adornments, and optional character counting, debounce callbacks, and multiline (textarea) support. It wraps Field's structural chrome for consistent styling across the library."
        ]
        Helpers.divider ()
        whenToUseSection ()
        Helpers.divider ()
        variantsExample ()
        Helpers.divider ()
        withContentExample ()
        Helpers.divider ()
        adornmentsExample ()
        Helpers.divider ()
        placeholderExample ()
        Helpers.divider ()
        shrinkLabelExample ()
        Helpers.divider ()
        helpTextValidationExample ()
        Helpers.divider ()
        helpTextColorVariationsExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        colorExample ()
        Helpers.divider ()
        typographyExample ()
        Helpers.divider ()
        maxLengthExample ()
        Helpers.divider ()
        debounceExample ()
        Helpers.divider ()
        multilineExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
