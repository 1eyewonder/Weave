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

  let private centered content =
    div [
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      AlignItems.center
      Gap.All.g3
    ] [ content ]

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

  let private basicExample () =
    let description =
      div [ Typography.body1 ] [
        text
          "Bind a Var<string> to a labelled input. Typing updates the value; the standard variant is the default. "
        text
          "The label floats on focus or when a value is present. To prefill a field, initialize the Var with existing text, such as "
        Helpers.inlineCode "Var.Create \"Ada\""
        text "."
      ]

    let content =
      let name = Var.Create ""

      TextField.singleLine (name, labelText = View.Const "Name")

    let code =
      """let name = Var.Create ""

TextField.singleLine (name, labelText = View.Const "Name")
"""

    div [ attr.id "with-content" ] [
      Helpers.codeSampleSection "Basic Usage" description (centered content) code
    ]

  let appearanceExample () =
    let configuration = Var.Create TextFieldAppearance.defaults
    let name = Var.Create ""

    let selectSetting label options getValue setValue disabled =
      let selected =
        configuration.Lens (fun current -> Some(getValue current)) (fun current selected ->
          match selected with
          | Some value -> current |> setValue value |> TextFieldAppearance.normalise
          | None -> current)

      let items =
        options
        |> List.map (fun (value, label) ->
          SelectItem.create (text label, label, value, disabled = disabled value))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let alwaysEnabled _ = View.Const false

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        selectSetting
          "Variant"
          TextFieldAppearance.variants
          (fun current -> current.Variant)
          (fun value current -> { current with Variant = value })
          alwaysEnabled

        selectSetting
          "Color"
          TextFieldAppearance.colors
          (fun current -> current.Color)
          (fun value current -> { current with Color = value })
          alwaysEnabled

        selectSetting
          "Width"
          TextFieldAppearance.widths
          (fun current -> current.Width)
          (fun value current -> { current with Width = value })
          alwaysEnabled

        selectSetting
          "Adornment"
          TextFieldAppearance.adornments
          (fun current -> current.Adornment)
          (fun value current -> { current with Adornment = value })
          alwaysEnabled

        selectSetting
          "Label position"
          TextFieldAppearance.labelPositions
          (fun current -> current.LabelPosition)
          (fun value current -> { current with LabelPosition = value })
          (fun position ->
            configuration.View
            |> View.Map(fun current ->
              position = TextFieldAppearance.LabelPosition.Above
              && current.Variant <> TextFieldAppearance.Variant.Outlined))
      ]

    let attribute setting =
      match setting with
      | TextFieldAppearance.Setting.Filled -> TextField.Variant.filled
      | TextFieldAppearance.Setting.Outlined -> TextField.Variant.outlined
      | TextFieldAppearance.Setting.FullWidth -> TextField.Width.full
      | TextFieldAppearance.Setting.LabelAbove -> TextField.Label.above
      | TextFieldAppearance.Setting.Accent color ->
        match color with
        | TextFieldAppearance.Color.Primary -> TextField.Color.primary
        | TextFieldAppearance.Color.Secondary -> TextField.Color.secondary
        | TextFieldAppearance.Color.Tertiary -> TextField.Color.tertiary
        | TextFieldAppearance.Color.Error -> TextField.Color.error
        | TextFieldAppearance.Color.Warning -> TextField.Color.warning
        | TextFieldAppearance.Color.Success -> TextField.Color.success
        | TextFieldAppearance.Color.Info -> TextField.Color.info

    let placeholder =
      Var.Lens configuration (fun current -> current.Placeholder) (fun current value -> {
        current with
            Placeholder = value
      })

    let preview =
      configuration.View
      |> Doc.BindView(fun current ->
        let attributes = current |> TextFieldAppearance.settings |> List.map attribute

        let startAdornment =
          match current.Adornment with
          | TextFieldAppearance.Adornment.Start
          | TextFieldAppearance.Adornment.Both -> Some(Icon.create (Icon.UiActions UiActions.Search))
          | _ -> None

        let endAdornment =
          match current.Adornment with
          | TextFieldAppearance.Adornment.End
          | TextFieldAppearance.Adornment.Both -> Some(Icon.create (Icon.UiActions UiActions.CheckCircle))
          | _ -> None

        TextField.singleLine (
          name,
          labelText = View.Const "Name",
          ?placeholder =
            (if current.Placeholder then
               Some(View.Const "Ada Lovelace")
             else
               None),
          ?startAdornment = startAdornment,
          ?endAdornment = endAdornment,
          attrs = attributes
        ))

    let reset () =
      configuration.Value <- TextFieldAppearance.defaults
      name.Value <- ""

    let description =
      Helpers.bodyText
        "Standard uses an underline, filled adds a surface, and outlined defines a clear boundary. Combine styles and adornments, then copy your selection. A placeholder keeps the floating label raised; outlined fields can instead place the label above. Focus the field to see its accent color. Adornments accept any Doc, including colored icons."

    let source =
      (configuration.View, name.View) ||> View.Map2 TextFieldAppearance.source

    Helpers.appearanceSection
      [ "variants"; "colors"; "adornments"; "placeholder"; "persistent-labels" ]
      description
      controls
      (Some(Switch.create (placeholder, text "Placeholder")))
      preview
      reset
      source

  let private shrinkLabelExample () =
    let description =
      Helpers.bodyText
        "Use shrinkLabel to force the label into its floated position regardless of focus or value."

    let content =
      let value = Var.Create ""

      TextField.singleLine (
        value,
        labelText = View.Const "ShrinkLabel Override",
        shrinkLabel = View.Const true,
        attrs = [ TextField.Variant.outlined ]
      )

    let code =
      """let value = Var.Create ""

TextField.singleLine (
  value,
  labelText = View.Const "ShrinkLabel Override",
  shrinkLabel = View.Const true,
  attrs = [ TextField.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Shrink Label Override" description (centered content) code

  let private helpTextValidationExample () =
    let description =
      Helpers.bodyText
        "Help text animates in and out based on a View<bool>. Here the help text appears when the value is non-empty but less than 3 characters."

    let content =
      let value = Var.Create ""

      let showHelpText =
        value.View
        |> View.MapCached(fun v -> not (System.String.IsNullOrEmpty(v)) && v.Length < 3)

      let helpTextContent =
        showHelpText
        |> View.MapCached(fun show -> if show then "Must be at least 3 characters" else "")

      let helpText =
        FieldHelpText.create (
          Doc.TextView helpTextContent,
          attrs = [ FieldHelpText.Color.error; attr.id "name-validation-help" ]
        )

      TextField.singleLine (
        value,
        showHelpText = showHelpText,
        labelText = View.Const "Name",
        helpText = helpText,
        inputAttrs = [
          Attr.Create "aria-describedby" "name-validation-help"
          showHelpText |> View.MapCached string<bool> |> Attr.Dynamic "aria-invalid"
        ],
        attrs = [ Attr.DynamicClassPred "weave-field--error" showHelpText ]
      )

    let code =
      """let value = Var.Create ""

let showHelpText =
  value.View
  |> View.MapCached(fun v ->
    not (System.String.IsNullOrEmpty(v)) && v.Length < 3
  )

let helpTextContent =
  showHelpText
  |> View.MapCached(fun show ->
    if show then "Must be at least 3 characters" else ""
  )

let helpText =
  FieldHelpText.create (
    Doc.TextView helpTextContent,
    attrs = [ FieldHelpText.Color.error; attr.id "name-validation-help" ]
  )

TextField.singleLine (
  value,
  showHelpText = showHelpText,
  labelText = View.Const "Name",
  helpText = helpText,
  inputAttrs = [
    Attr.Create "aria-describedby" "name-validation-help"
    showHelpText |> View.MapCached string<bool> |> Attr.Dynamic "aria-invalid"
  ],
  attrs = [ Attr.DynamicClassPred "weave-field--error" showHelpText ]
)
"""

    Helpers.codeSampleSection "Help Text (Validation)" description (centered content) code

  let private helpTextColorVariationsExample () =
    let description =
      Helpers.bodyText
        "Help text can be styled with any brand color to convey success, warning, error, or informational states. Apply color classes to both the field and FieldHelpText independently."

    let content =
      let createField fieldColor helpColor value label message =
        let help = FieldHelpText.create (text message, attrs = [ helpColor ])

        TextField.singleLine (
          Var.Create value,
          labelText = View.Const label,
          showHelpText = View.Const true,
          helpText = help,
          attrs = [ TextField.Variant.outlined; fieldColor ]
        )

      comparisonGrid 3 [
        createField
          TextField.Color.success
          FieldHelpText.Color.success
          "Looks good!"
          "Username"
          "Username is available"

        createField
          TextField.Color.warning
          FieldHelpText.Color.warning
          "password"
          "Password"
          "Consider a stronger password"

        createField
          TextField.Color.error
          FieldHelpText.Color.error
          "not-an-email"
          "Email"
          "Invalid email address"
      ]

    let code =
      """let createField fieldColor helpColor value label message =
  let help = FieldHelpText.create (text message, attrs = [ helpColor ])

  TextField.singleLine (
    Var.Create value,
    labelText = View.Const label,
    showHelpText = View.Const true,
    helpText = help,
    attrs = [ TextField.Variant.outlined; fieldColor ]
  )

createField
  TextField.Color.success
  FieldHelpText.Color.success
  "Looks good!"
  "Username"
  "Username is available"

createField
  TextField.Color.warning
  FieldHelpText.Color.warning
  "password"
  "Password"
  "Consider a stronger password"

createField
  TextField.Color.error
  FieldHelpText.Color.error
  "not-an-email"
  "Email"
  "Invalid email address"
"""

    Helpers.codeSampleSection "Help Text Color Variations" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText "Fields can be disabled or set to read-only to prevent user input."

    let content =
      let disabledVal = Var.Create "Cannot edit"

      let readOnlyVal = Var.Create "Read-only value"

      comparisonGrid 2 [
        TextField.singleLine (disabledVal, labelText = View.Const "Disabled", enabled = View.Const false)

        TextField.singleLine (readOnlyVal, labelText = View.Const "Read Only", readOnly = View.Const true)
      ]

    let code =
      """let disabledVal = Var.Create "Cannot edit"

let readOnlyVal = Var.Create "Read-only value"

TextField.singleLine (
  disabledVal,
  labelText = View.Const "Disabled",
  enabled = View.Const false
)

TextField.singleLine (
  readOnlyVal,
  labelText = View.Const "Read Only",
  readOnly = View.Const true
)
"""

    Helpers.codeSampleSection "Disabled & Read Only" description content code

  let private typographyExample () =
    let description =
      Helpers.bodyText
        "Use the typoAttrs parameter to configure the field's font styling. Because the component uses em-based sizing, the entire field — label, input, and chrome — scales proportionally with the chosen typography class."

    let content =
      let createField typography label =
        TextField.singleLine (
          Var.Create "",
          labelText = View.Const label,
          typoAttrs = [ typography ],
          attrs = [ TextField.Variant.outlined ]
        )

      comparisonGrid 2 [
        createField Typography.caption "Caption"

        createField Typography.body1 "Body1"

        createField Typography.subtitle1 "Subtitle1"

        createField Typography.h6 "H6"
      ]

    let code =
      """let createField typography label =
  TextField.singleLine (
    Var.Create "",
    labelText = View.Const label,
    typoAttrs = [ typography ],
    attrs = [ TextField.Variant.outlined ]
  )

createField Typography.caption "Caption"

createField Typography.body1 "Body1"

createField Typography.subtitle1 "Subtitle1"

createField Typography.h6 "H6"
"""

    Helpers.codeSampleSection "Typography Configuration" description content code

  let private maxLengthExample () =
    let description =
      Helpers.bodyText
        "Pass maxLength to enforce a character limit and render a counter below the field showing the current length against the maximum."

    let content =
      let value = Var.Create ""

      TextField.singleLine (
        value,
        labelText = View.Const "Bio",
        maxLength = 100,
        attrs = [ TextField.Variant.outlined ]
      )

    let code =
      """let value = Var.Create ""

TextField.singleLine (
  value,
  labelText = View.Const "Bio",
  maxLength = 100,
  attrs = [ TextField.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Max Length & Character Counter" description (centered content) code

  let private debounceExample () =
    let description =
      Helpers.bodyText
        "Use debounce with onChange to delay the callback until the user stops typing. The live value updates immediately while the debounced value updates after the specified delay."

    let content =
      let liveValue = Var.Create ""
      let debouncedValue = Var.Create ""
      let liveText = liveValue.View |> View.Map(sprintf "Live value: %s")
      let delayedText = debouncedValue.View |> View.Map(sprintf "After 500 ms: %s")

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        Gap.All.g4
        Attr.Style "max-inline-size" "32rem"
        Attr.Style "margin-inline" "auto"
      ] [
        TextField.singleLine (
          liveValue,
          labelText = View.Const "Search",
          debounce = 500,
          onChange = (fun value -> Var.Set debouncedValue value),
          attrs = [ TextField.Variant.outlined ]
        )

        div [] [ textView liveText ]

        div [] [ textView delayedText ]
      ]

    let code =
      """let liveValue = Var.Create ""
let debouncedValue = Var.Create ""
let liveText = liveValue.View |> View.Map(sprintf "Live value: %s")
let delayedText = debouncedValue.View |> View.Map(sprintf "After 500 ms: %s")

Doc.Concat [
  TextField.singleLine (
    liveValue,
    labelText = View.Const "Search",
    debounce = 500,
    onChange = (fun value -> Var.Set debouncedValue value),
    attrs = [ TextField.Variant.outlined ]
  )

  textView liveText

  textView delayedText
]
"""

    Helpers.codeSampleSection "Debounce & onChange" description content code

  let private multilineExample () =
    let description =
      Helpers.bodyText
        "Use TextField.multiLine to render a textarea instead of a standard input. The textarea is vertically resizable by default."

    let content =
      comparisonGrid 2 [
        TextField.singleLine (
          Var.Create "",
          labelText = View.Const "Single Line",
          attrs = [ TextField.Variant.outlined ]
        )

        TextField.multiLine (
          Var.Create "",
          labelText = View.Const "Multiline",
          attrs = [ TextField.Variant.outlined ]
        )
      ]

    let code =
      """TextField.singleLine (
  Var.Create "",
  labelText = View.Const "Single Line",
  attrs = [ TextField.Variant.outlined ]
)

TextField.multiLine (
  Var.Create "",
  labelText = View.Const "Multiline",
  attrs = [ TextField.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Multiline" description content code

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "Complete API reference for TextField and FieldHelpText. TextField provides two constructors — singleLine for standard text inputs and multiLine for textarea inputs. Both share the same parameter set and wrap Field's structural chrome.")
      [
        Helpers.returnTypeNote
          "TextField.singleLine / multiLine also accept FormField directly in place of the bound Var, with the same optional parameters except validation, which is wired automatically. The explicit validation parameter below applies to Var bindings."
        Helpers.apiTable "TextField.singleLine" [
          Helpers.apiParam
            "value"
            "Var<string> or FormField<string>"
            ""
            "Bind a Var, or pass a form field to wire its value and validation automatically"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Floating label displayed above the input"
          Helpers.apiParam
            "?placeholder"
            "View<string>"
            "\"\""
            "Placeholder text shown when the field is empty and label is floating"
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

        Helpers.styleModuleTable "TextField.Label" [ ("above", "Keep the label above an outlined field") ]

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
        basicExample ()
        appearanceExample ()
        Helpers.divider ()
        span [ attr.id "when-to-use" ] []
        shrinkLabelExample ()
        Helpers.divider ()
        helpTextValidationExample ()
        Helpers.divider ()
        helpTextColorVariationsExample ()
        Helpers.divider ()
        disabledExample ()
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
