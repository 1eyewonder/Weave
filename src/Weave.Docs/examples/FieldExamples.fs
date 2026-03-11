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
      Grid.Create(
        [
          GridItem.Create(
            Field.Create(
              stdVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Standard,
              labelText = View.Const "Standard",
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Field.Create(
              filledVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Filled,
              labelText = View.Const "Filled",
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Field.Create(
              outlinedVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Outlined",
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
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

Field.Create(
    value,
    variant = Field.Variant.Standard, // see here
    labelText = View.Const "Standard"
)

Field.Create(
    value,
    variant = Field.Variant.Filled, // see here
    labelText = View.Const "Filled"
)

Field.Create(
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
      Grid.Create(
        [
          GridItem.Create(
            Field.Create(
              stdVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Standard,
              labelText = View.Const "Standard",
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Field.Create(
              filledVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Filled,
              labelText = View.Const "Filled",
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Field.Create(
              outlinedVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Outlined",
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
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
let value = Var.Create "Some Content 💛 follows here"

Field.Create(
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
      GridItem.Create(
        Field.Create(
          value,
          showHelpText = View.Const false,
          variant = variant,
          labelText = View.Const label,
          ?startAdornment = startAdornment (),
          ?endAdornment = endAdornment (),
          attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
        ),
        xs = Grid.Width.create 12,
        md = Grid.Width.create 4
      )

    let searchIcon () =
      Some(Icon.Create(Icon.UiActions UiActions.Search, attrs = [ BrandColor.toColor BrandColor.Primary ]))

    let checkIcon () =
      Some(
        Icon.Create(Icon.UiActions UiActions.CheckCircle, attrs = [ BrandColor.toColor BrandColor.Success ])
      )

    let infoIcon () =
      Some(Icon.Create(Icon.Action Action.Info, attrs = [ BrandColor.toColor BrandColor.Info ]))

    let warningIcon () =
      Some(Icon.Create(Icon.Action Action.Warning, attrs = [ BrandColor.toColor BrandColor.Warning ]))

    let noIcon () = None

    let content =
      div [] [
        Body1.Div("Start Adornment", attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ])

        Grid.Create(
          [
            mkItem Field.Variant.Standard "Standard" searchIcon noIcon startStdVal
            mkItem Field.Variant.Filled "Filled" searchIcon noIcon startFilledVal
            mkItem Field.Variant.Outlined "Outlined" searchIcon noIcon startOutlinedVal
          ],
          spacing = Grid.GutterSpacing.create 10
        )

        Body1.Div("End Adornment", attrs = [ cls [ yield! Margin.toClasses Margin.Vertical.extraSmall ] ])

        Grid.Create(
          [
            mkItem Field.Variant.Standard "Standard" noIcon checkIcon endStdVal
            mkItem Field.Variant.Filled "Filled" noIcon infoIcon endFilledVal
            mkItem Field.Variant.Outlined "Outlined" noIcon warningIcon endOutlinedVal
          ],
          spacing = Grid.GutterSpacing.create 10
        )

        Body1.Div("Both Adornments", attrs = [ cls [ yield! Margin.toClasses Margin.Vertical.extraSmall ] ])

        Grid.Create(
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
Field.Create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined",
    startAdornment = Icon.Create( // see here
        Icon.UiActions UiActions.Search,
        attrs = [ BrandColor.toColor BrandColor.Primary ]
    )
)

// End adornment only
Field.Create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined",
    endAdornment = Icon.Create( // see here
        Icon.UiActions UiActions.CheckCircle,
        attrs = [ BrandColor.toColor BrandColor.Success ]
    )
)

// Both start and end adornments
Field.Create(
    value,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined",
    startAdornment = Icon.Create( // see here
        Icon.UiActions UiActions.Search,
        attrs = [ BrandColor.toColor BrandColor.Primary ]
    ),
    endAdornment = Icon.Create( // and here
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
      Field.Create(
        value,
        showHelpText = View.Const false,
        variant = Field.Variant.Outlined,
        labelText = View.Const "ShrinkLabel Override",
        shrinkLabel = View.Const true,
        endAdornment = Icon.Create(Icon.Action Action.Info, attrs = [ BrandColor.toColor BrandColor.Info ]),
        attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
      )

    let code =
      """open Weave

let value = Var.Create ""

Field.Create(
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
      Field.Create(
        value,
        showHelpText = View.Const false,
        variant = Field.Variant.Outlined,
        labelText = View.Const "With Placeholder",
        placeholder = View.Const "Type something here…",
        attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
      )

    let code =
      """open Weave

let value = Var.Create ""

Field.Create(
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
      FieldHelpText.Create(
        Doc.TextView helpTextContent,
        attrs = [ Field.HelpTextColor.toClass BrandColor.Error |> cl ]
      )

    let description =
      Helpers.bodyText
        "Help text animates in and out based on a View<bool>. Here the help text appears when the value is non-empty but less than 3 characters."

    let content =
      Field.Create(
        value,
        variant = Field.Variant.Outlined,
        labelText = View.Const "Name",
        showHelpText = showHelpText,
        helpText = helpText,
        attrs = [
          Field.Width.toClass Field.Width.Full |> Attr.bindOption cl
          Attr.DynamicClassPred (Field.Color.toClass BrandColor.Error) showHelpText
        ]
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
    FieldHelpText.Create(
        Doc.TextView helpTextContent,
        attrs = [ Field.HelpTextColor.toClass BrandColor.Error |> cl ]
    )

Field.Create(
    value,
    showHelpText = showHelpText, // see here — controls animated visibility
    labelText = View.Const "Name",
    helpText = helpText,
    attrs = [
        Attr.DynamicClassPred (Field.Color.toClass BrandColor.Error) showHelpText
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
      Grid.Create(
        [
          GridItem.Create(
            Field.Create(
              successVal,
              showHelpText = View.Const true,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Username",
              helpText =
                FieldHelpText.Create(
                  text "Username is available",
                  attrs = [ Field.HelpTextColor.toClass BrandColor.Success |> cl ]
                ),
              attrs = [
                Field.Width.toClass Field.Width.Full |> Attr.bindOption cl
                Field.Color.toClass BrandColor.Success |> cl
              ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Field.Create(
              warningVal,
              showHelpText = View.Const true,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Password",
              helpText =
                FieldHelpText.Create(
                  text "Consider a stronger password",
                  attrs = [ Field.HelpTextColor.toClass BrandColor.Warning |> cl ]
                ),
              attrs = [
                Field.Width.toClass Field.Width.Full |> Attr.bindOption cl
                Field.Color.toClass BrandColor.Warning |> cl
              ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )

          GridItem.Create(
            Field.Create(
              errorVal,
              showHelpText = View.Const true,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Email",
              helpText =
                FieldHelpText.Create(
                  text "Invalid email address",
                  attrs = [ Field.HelpTextColor.toClass BrandColor.Error |> cl ]
                ),
              attrs = [
                Field.Width.toClass Field.Width.Full |> Attr.bindOption cl
                Field.Color.toClass BrandColor.Error |> cl
              ]
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
Field.Create(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Username",
    helpText =
        FieldHelpText.Create(
            text "Username is available",
            attrs = [ Field.HelpTextColor.toClass BrandColor.Success |> cl ] // see here
        ),
    attrs = [ Field.Color.toClass BrandColor.Success |> cl ]
)

// Warning help text
Field.Create(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Password",
    helpText =
        FieldHelpText.Create(
            text "Consider a stronger password",
            attrs = [ Field.HelpTextColor.toClass BrandColor.Warning |> cl ]
        ),
    attrs = [ Field.Color.toClass BrandColor.Warning |> cl ]
)

// Error help text
Field.Create(
    value,
    showHelpText = View.Const true,
    labelText = View.Const "Email",
    helpText =
        FieldHelpText.Create(
            text "Invalid email address",
            attrs = [ Field.HelpTextColor.toClass BrandColor.Error |> cl ]
        ),
    attrs = [ Field.Color.toClass BrandColor.Error |> cl ]
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
      Grid.Create(
        [
          GridItem.Create(
            Field.Create(
              disabledVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Disabled",
              enabled = View.Const false,
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.Create(
            Field.Create(
              readOnlyVal,
              showHelpText = View.Const false,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Read Only",
              readOnly = View.Const true,
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
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

Field.Create(
    disabledVal,
    labelText = View.Const "Disabled",
    enabled = View.Const false // see here
)

Field.Create(
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
    let mkField color label =
      let v = Var.Create ""

      GridItem.Create(
        Field.Create(
          v,
          showHelpText = View.Const false,
          variant = Field.Variant.Outlined,
          labelText = View.Const label,
          attrs = [
            Field.Width.toClass Field.Width.Full |> Attr.bindOption cl
            Field.Color.toClass color |> cl
          ]
        ),
        xs = Grid.Width.create 12,
        sm = Grid.Width.create 6,
        md = Grid.Width.create 4
      )

    let description =
      Helpers.bodyText "The accent color used when the field is focused can be customised with color classes."

    let content =
      Grid.Create(
        [
          mkField BrandColor.Primary "Primary"
          mkField BrandColor.Secondary "Secondary"
          mkField BrandColor.Tertiary "Tertiary"
          mkField BrandColor.Success "Success"
          mkField BrandColor.Warning "Warning"
          mkField BrandColor.Error "Error"
          mkField BrandColor.Info "Info"
        ]
      )

    let code =
      """open Weave


let value = Var.Create ""

let mkField color label =
    Field.Create(
        value,
        labelText = View.Const label,
        attrs = [ Field.Color.toClass color |> cl ] // see here
    )

mkField BrandColor.Primary "Primary"
mkField BrandColor.Secondary "Secondary"
mkField BrandColor.Tertiary "Tertiary"
mkField BrandColor.Success "Success"
mkField BrandColor.Warning "Warning"
mkField BrandColor.Error "Error"
mkField BrandColor.Info "Info"
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
      Grid.Create(
        [
          GridItem.Create(
            Field.Create(
              captionVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Caption",
              typoAttrs = [ Typography.Typo.toClass Typography.Typo.Caption |> cl ],
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.Create(
            Field.Create(
              body1Val,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Body1",
              typoAttrs = [ Typography.Typo.toClass Typography.Typo.Body1 |> cl ],
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.Create(
            Field.Create(
              subtitle1Val,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Subtitle1",
              typoAttrs = [ Typography.Typo.toClass Typography.Typo.Subtitle1 |> cl ],
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )

          GridItem.Create(
            Field.Create(
              h6Val,
              variant = Field.Variant.Outlined,
              labelText = View.Const "H6",
              typoAttrs = [ Typography.Typo.toClass Typography.Typo.H6 |> cl ],
              attrs = [ Field.Width.toClass Field.Width.Full |> Attr.bindOption cl ]
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
Field.Create(
    value,
    labelText = View.Const "Caption",
    typoAttrs = [ Typography.Typo.toClass Typography.Typo.Caption |> cl ]
)

// Scale up with Subtitle1 typography
Field.Create(
    value,
    labelText = View.Const "Subtitle1",
    typoAttrs = [ Typography.Typo.toClass Typography.Typo.Subtitle1 |> cl ]
)
"""

    Helpers.codeSampleSection "Typography Configuration" description content code

  // ---------------------------------------------------------------------------
  // Render
  // ---------------------------------------------------------------------------
  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Field"
        Body1.Div(
          "Field is the generic base component for all text-based inputs. It supports Standard, Filled, and Outlined variants with floating labels, adornments, and help text.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
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
      maxWidth = Container.MaxWidth.Large
    )
