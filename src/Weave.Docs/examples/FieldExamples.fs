namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module FieldExamples =

  // ---------------------------------------------------------------------------
  // Variants
  // ---------------------------------------------------------------------------
  let private variantsExample () =
    let stdVal = Var.Create ""
    let filledVal = Var.Create ""
    let outlinedVal = Var.Create ""

    let description =
      Helpers.bodyText
        "Field supports three variants: Standard (bottom border), Filled (background + bottom border), and Outlined (full border)."

    let content =
      Grid.create (
        [
          GridItem.create (
            Field.create (
              stdVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Standard,
              labelText = View.Const "Standard",
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Field.create (
              filledVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Filled,
              labelText = View.Const "Filled",
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Field.create (
              outlinedVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Outlined",
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 10
      )

    let code =
      """open Weave

let value = Var.Create ""

Field.create(
    value,
    variant = Field.Variant.Standard, // see here
    labelText = View.Const "Standard"
)

Field.create(
    value,
    variant = Field.Variant.Filled, // see here
    labelText = View.Const "Filled"
)

Field.create(
    value,
    variant = Field.Variant.Outlined, // see here
    labelText = View.Const "Outlined"
)
"""

    Helpers.codeSampleSection "Variants" description content code

  // ---------------------------------------------------------------------------
  // With content
  // ---------------------------------------------------------------------------
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
            Field.create (
              stdVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Standard,
              labelText = View.Const "Standard",
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Field.create (
              filledVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Filled,
              labelText = View.Const "Filled",
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Field.create (
              outlinedVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Outlined",
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 10
      )

    let code =
      """open Weave

// When the Var has a value, the label automatically floats
let value = Var.Create "Some content here"

Field.create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined"
)
"""

    Helpers.codeSampleSection "With Content" description content code

  // ---------------------------------------------------------------------------
  // With adornments
  // ---------------------------------------------------------------------------
  let private adornmentsExample () =
    // Start adornment row
    let startStdVal = Var.Create ""
    let startFilledVal = Var.Create ""
    let startOutlinedVal = Var.Create ""
    // End adornment row
    let endStdVal = Var.Create ""
    let endFilledVal = Var.Create ""
    let endOutlinedVal = Var.Create ""
    // Both adornments row
    let bothStdVal = Var.Create ""
    let bothFilledVal = Var.Create ""
    let bothOutlinedVal = Var.Create ""

    let description =
      Helpers.bodyText
        "Icons or other content can be placed at the start, end, or both ends of the field. All three adornment placements are shown across every variant."

    let mkItem variant label (startAdornment: unit -> Doc option) (endAdornment: unit -> Doc option) value =
      GridItem.create (
        Field.create (
          value,
          showHelpText = View.Const false,
          variant = variant,
          labelText = View.Const label,
          ?startAdornment = startAdornment (),
          ?endAdornment = endAdornment (),
          attrs = [ Field.Width.full ]
        ),
        xs = Grid.Width.create 12,
        md = Grid.Width.create 4
      )

    let searchIcon () =
      Some(Icon.create (Icon.UiActions UiActions.Search, attrs = [ BrandColor.toColor BrandColor.Primary ]))

    let checkIcon () =
      Some(
        Icon.create (Icon.UiActions UiActions.CheckCircle, attrs = [ BrandColor.toColor BrandColor.Success ])
      )

    let infoIcon () =
      Some(Icon.create (Icon.Action Action.Info, attrs = [ BrandColor.toColor BrandColor.Info ]))

    let warningIcon () =
      Some(Icon.create (Icon.Action Action.Warning, attrs = [ BrandColor.toColor BrandColor.Warning ]))

    let noIcon () = None

    let content =
      div [] [
        div [ Typography.body1; Margin.Bottom.extraSmall ] [ text "Start Adornment" ]

        Grid.create (
          [
            mkItem Field.Variant.Standard "Standard" searchIcon noIcon startStdVal
            mkItem Field.Variant.Filled "Filled" searchIcon noIcon startFilledVal
            mkItem Field.Variant.Outlined "Outlined" searchIcon noIcon startOutlinedVal
          ],
          spacing = Grid.GutterSpacing.create 10
        )

        div [ Typography.body1; Margin.Vertical.extraSmall ] [ text "End Adornment" ]

        Grid.create (
          [
            mkItem Field.Variant.Standard "Standard" noIcon checkIcon endStdVal
            mkItem Field.Variant.Filled "Filled" noIcon infoIcon endFilledVal
            mkItem Field.Variant.Outlined "Outlined" noIcon warningIcon endOutlinedVal
          ],
          spacing = Grid.GutterSpacing.create 10
        )

        div [ Typography.body1; Margin.Vertical.extraSmall ] [ text "Both Adornments" ]

        Grid.create (
          [
            mkItem Field.Variant.Standard "Standard" searchIcon checkIcon bothStdVal
            mkItem Field.Variant.Filled "Filled" searchIcon infoIcon bothFilledVal
            mkItem Field.Variant.Outlined "Outlined" searchIcon warningIcon bothOutlinedVal
          ],
          spacing = Grid.GutterSpacing.create 10
        )
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

let value = Var.Create ""

// Start adornment only
Field.create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined",
    startAdornment = Icon.create( // see here
        Icon.UiActions UiActions.Search,
        attrs = [ BrandColor.toColor BrandColor.Primary ]
    )
)

// End adornment only
Field.create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined",
    endAdornment = Icon.create( // see here
        Icon.UiActions UiActions.CheckCircle,
        attrs = [ BrandColor.toColor BrandColor.Success ]
    )
)

// Both start and end adornments
Field.create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined",
    startAdornment = Icon.create( // see here
        Icon.UiActions UiActions.Search,
        attrs = [ BrandColor.toColor BrandColor.Primary ]
    ),
    endAdornment = Icon.create( // and here
        Icon.UiActions UiActions.CheckCircle,
        attrs = [ BrandColor.toColor BrandColor.Success ]
    )
)
"""

    Helpers.codeSampleSection "Adornments" description content code

  // ---------------------------------------------------------------------------
  // Shrink label override
  // ---------------------------------------------------------------------------
  let private shrinkLabelExample () =
    let value = Var.Create ""

    let description =
      Helpers.bodyText
        "Use shrinkLabel to force the label into its floated position regardless of focus or value."

    let content =
      Field.create (
        value,
        showHelpText = View.Const false,
        variant = Field.Variant.Outlined,
        labelText = View.Const "ShrinkLabel Override",
        shrinkLabel = View.Const true,
        endAdornment = Icon.create (Icon.Action Action.Info, attrs = [ BrandColor.toColor BrandColor.Info ]),
        attrs = [ Field.Width.full ]
      )

    let code =
      """open Weave

let value = Var.Create ""

Field.create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "ShrinkLabel Override",
    shrinkLabel = View.Const true // see here
)
"""

    Helpers.codeSampleSection "Shrink Label Override" description content code

  // ---------------------------------------------------------------------------
  // Placeholder text
  // ---------------------------------------------------------------------------
  let private placeholderExample () =
    let value = Var.Create ""

    let description =
      Helpers.bodyText
        "When an explicit placeholder is provided the label stays floated and the placeholder text is shown in the input area."

    let content =
      Field.create (
        value,
        showHelpText = View.Const false,
        variant = Field.Variant.Outlined,
        labelText = View.Const "With Placeholder",
        placeholder = View.Const "Type something here…",
        attrs = [ Field.Width.full ]
      )

    let code =
      """open Weave

let value = Var.Create ""

Field.create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "With Placeholder",
    placeholder = View.Const "Type something here…" // see here
)
"""

    Helpers.codeSampleSection "Placeholder" description content code

  // ---------------------------------------------------------------------------
  // Help text with validation
  // ---------------------------------------------------------------------------
  let private helpTextValidationExample () =
    let value = Var.Create ""

    let showHelpText =
      value.View
      |> View.MapCached(fun v -> not (System.String.IsNullOrEmpty(v)) && v.Length < 3)

    let helpTextContent =
      showHelpText
      |> View.MapCached(fun show -> if show then "Must be at least 3 characters" else "")

    let helpText =
      FieldHelpText.create (Doc.TextView helpTextContent, attrs = [ Field.HelpTextColor.error ])

    let description =
      Helpers.bodyText
        "Help text animates in and out based on a View<bool>. Here the help text appears when the value is non-empty but less than 3 characters."

    let content =
      Field.create (
        value,
        variant = Field.Variant.Outlined,
        labelText = View.Const "Name",
        showHelpText = showHelpText,
        helpText = helpText,
        attrs = [ Field.Width.full; Attr.DynamicClassPred "weave-field--error" showHelpText ]
      )

    let code =
      """open Weave


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
        attrs = [ Field.HelpTextColor.error ]
    )

Field.create(
    value,
    showHelpText = showHelpText, // see here — controls animated visibility
    labelText = View.Const "Name",
    helpText = helpText,
    attrs = [
        Attr.DynamicClassPred "weave-field--error" showHelpText
    ]
)
"""

    Helpers.codeSampleSection "Help Text (Validation)" description content code

  // ---------------------------------------------------------------------------
  // Help text color variations
  // ---------------------------------------------------------------------------
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
            Field.create (
              successVal,
              showHelpText = View.Const true,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Username",
              helpText =
                FieldHelpText.create (text "Username is available", attrs = [ Field.HelpTextColor.success ]),
              attrs = [ Field.Width.full; Field.Color.success ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Field.create (
              warningVal,
              showHelpText = View.Const true,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Password",
              helpText =
                FieldHelpText.create (
                  text "Consider a stronger password",
                  attrs = [ Field.HelpTextColor.warning ]
                ),
              attrs = [ Field.Width.full; Field.Color.warning ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.create (
            Field.create (
              errorVal,
              showHelpText = View.Const true,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Email",
              helpText =
                FieldHelpText.create (text "Invalid email address", attrs = [ Field.HelpTextColor.error ]),
              attrs = [ Field.Width.full; Field.Color.error ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 10
      )

    let code =
      """open Weave


let value = Var.Create "Looks good!"

// Success help text
Field.create(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Username",
    helpText =
        FieldHelpText.create(
            text "Username is available",
            attrs = [ Field.HelpTextColor.success ] // see here
        ),
    attrs = [ Field.Color.success ]
)

// Warning help text
Field.create(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Password",
    helpText =
        FieldHelpText.create(
            text "Consider a stronger password",
            attrs = [ Field.HelpTextColor.warning ]
        ),
    attrs = [ Field.Color.warning ]
)

// Error help text
Field.create(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Email",
    helpText =
        FieldHelpText.create(
            text "Invalid email address",
            attrs = [ Field.HelpTextColor.error ]
        ),
    attrs = [ Field.Color.error ]
)
"""

    Helpers.codeSampleSection "Help Text Color Variations" description content code

  // ---------------------------------------------------------------------------
  // Disabled & ReadOnly
  // ---------------------------------------------------------------------------
  let private disabledExample () =
    let disabledVal = Var.Create "Cannot edit"
    let readOnlyVal = Var.Create "Read-only value"

    let description =
      Helpers.bodyText "Fields can be disabled or set to read-only to prevent user input."

    let content =
      Grid.create (
        [
          GridItem.create (
            Field.create (
              disabledVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Disabled",
              enabled = View.Const false,
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.create (
            Field.create (
              readOnlyVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Read Only",
              readOnly = View.Const true,
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 10
      )

    let code =
      """open Weave

let disabledVal = Var.Create "Cannot edit"
let readOnlyVal = Var.Create "Read-only value"

Field.create(
    disabledVal,
    labelText = View.Const "Disabled",
    enabled = View.Const false // see here
)

Field.create(
    readOnlyVal,
    labelText = View.Const "Read Only",
    readOnly = View.Const true // see here
)
"""

    Helpers.codeSampleSection "Disabled & Read Only" description content code

  // ---------------------------------------------------------------------------
  // Color variants
  // ---------------------------------------------------------------------------
  let private colorExample () =
    let mkField colorAttr label =
      let v = Var.Create ""

      GridItem.create (
        Field.create (
          v,
          showHelpText = View.Const false,
          variant = Field.Variant.Outlined,
          labelText = View.Const label,
          attrs = [ Field.Width.full; colorAttr ]
        ),
        xs = Grid.Width.create 12,
        sm = Grid.Width.create 6,
        md = Grid.Width.create 4
      )

    let description =
      Helpers.bodyText "The accent color used when the field is focused can be customised with color classes."

    let content =
      Grid.create (
        [
          mkField Field.Color.primary "Primary"
          mkField Field.Color.secondary "Secondary"
          mkField Field.Color.tertiary "Tertiary"
          mkField Field.Color.success "Success"
          mkField Field.Color.warning "Warning"
          mkField Field.Color.error "Error"
          mkField Field.Color.info "Info"
        ]
      )

    let code =
      """open Weave


let value = Var.Create ""

Field.create(value, labelText = View.Const "Primary", attrs = [ Field.Color.primary ]) // see here
Field.create(value, labelText = View.Const "Secondary", attrs = [ Field.Color.secondary ])
Field.create(value, labelText = View.Const "Tertiary", attrs = [ Field.Color.tertiary ])
Field.create(value, labelText = View.Const "Success", attrs = [ Field.Color.success ])
Field.create(value, labelText = View.Const "Warning", attrs = [ Field.Color.warning ])
Field.create(value, labelText = View.Const "Error", attrs = [ Field.Color.error ])
Field.create(value, labelText = View.Const "Info", attrs = [ Field.Color.info ])
"""

    Helpers.codeSampleSection "Color Variants" description content code

  // ---------------------------------------------------------------------------
  // Typography Configuration
  // ---------------------------------------------------------------------------
  let private typographyExample () =
    let captionVal = Var.Create ""
    let body1Val = Var.Create ""
    let subtitle1Val = Var.Create ""
    let h6Val = Var.Create ""
    let colorVal = Var.Create ""

    let description =
      Helpers.bodyText
        "Use the typoAttrs parameter to configure the field's font styling. Because the component uses em-based sizing, the entire field — label, input, and chrome — scales proportionally with the chosen typography class. You can also mix in color classes."

    let content =
      Grid.create (
        [
          GridItem.create (
            Field.create (
              captionVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Caption",
              typoAttrs = [ Typography.caption ],
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.create (
            Field.create (
              body1Val,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Body1",
              typoAttrs = [ Typography.body1 ],
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.create (
            Field.create (
              subtitle1Val,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Subtitle1",
              typoAttrs = [ Typography.subtitle1 ],
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.create (
            Field.create (
              h6Val,
              variant = Field.Variant.Outlined,
              labelText = View.Const "H6",
              typoAttrs = [ Typography.h6 ],
              attrs = [ Field.Width.full ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 10
      )

    let code =
      """open Weave


let value = Var.Create ""

// Scale down with Caption typography
Field.create(
    value,
    labelText = View.Const "Caption",
    typoAttrs = [ Typography.caption ]
)

// Scale up with Subtitle1 typography
Field.create(
    value,
    labelText = View.Const "Subtitle1",
    typoAttrs = [ Typography.subtitle1 ]
)
"""

    Helpers.codeSampleSection "Typography Configuration" description content code

  // ---------------------------------------------------------------------------
  // Render
  // ---------------------------------------------------------------------------
  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Field"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Field is the generic base component for all text-based inputs. It supports Standard, Filled, and Outlined variants with floating labels, adornments, and help text."
        ]
        Helpers.divider ()
        variantsExample ()
        Helpers.divider ()
        colorExample ()
        Helpers.divider ()
        withContentExample ()
        Helpers.divider ()
        adornmentsExample ()
        Helpers.divider ()
        shrinkLabelExample ()
        Helpers.divider ()
        placeholderExample ()
        Helpers.divider ()
        helpTextValidationExample ()
        Helpers.divider ()
        helpTextColorVariationsExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        typographyExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
