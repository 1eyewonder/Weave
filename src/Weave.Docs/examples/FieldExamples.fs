namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module FieldExamples =

  let private whenToUseSection () =
    let description =
      div [ Typography.body1 ] [
        text
          "Field is the low-level primitive that provides field chrome. Most of the time you should reach for a higher-level wrapper instead."
      ]

    let content =
      Helpers.guidanceColumns
        (Helpers.guidanceCard "Use a wrapper component when\u2026" [
          Helpers.guidanceBullet
            "TextField for string inputs"
            "names, emails, search queries, and multiline text areas."
          Helpers.guidanceBullet
            "NumericField for int or float inputs"
            "quantities, prices, and sliders with spin buttons and min/max clamping."
          Helpers.guidanceBullet
            "Select for dropdown selection"
            "pick one value from a predefined list with search and keyboard navigation."
        ])
        (Helpers.guidanceCard "Use Field directly when\u2026" [
          Helpers.guidanceBullet
            "You are building a custom input component"
            "date pickers, color pickers, or third-party inputs that need field chrome."
          Helpers.guidanceBullet
            "You need full control over focus and float logic"
            "Field requires you to wire isFocused and shouldFloat yourself."
          Helpers.guidanceBullet
            "No existing wrapper fits your use case"
            "Field gives you the label, adornments, outline, help text, and variant styling with no opinions about the input element."
        ])

    Helpers.sectionPlain "When to Use" description content

  let private customInputExample () =
    let description =
      Helpers.bodyText
        "Wrap a custom input element by providing it along with isFocused and shouldFloat views. You are responsible for tracking focus state and deciding when the label should float."

    let mkField variantAttr label =
      let value = Var.Create ""
      let isFocused = Var.Create false

      let hasValue =
        value.View |> View.MapCached(fun v -> not (System.String.IsNullOrEmpty v))

      let shouldFloat = isFocused.View <||> hasValue

      let inputElement =
        Doc.InputType.Text
          [
            Attr.Class "weave-field__input"
            on.focus (fun _ _ -> isFocused.Value <- true)
            on.blur (fun _ _ -> isFocused.Value <- false)
          ]
          value

      GridItem.create (
        Field.create (
          inputElement,
          isFocused.View,
          shouldFloat,
          labelText = View.Const label,
          attrs = [ yield! variantAttr; Field.Width.full ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
      )

    let content =
      Grid.create (
        [
          mkField [] "Standard"
          mkField [ Field.Variant.filled ] "Filled"
          mkField [ Field.Variant.outlined ] "Outlined"
        ],
        attrs = [ Grid.Spacing.large ]
      )

    let code =
      """open Weave
open WebSharper.UI

let value = Var.Create ""
let isFocused = Var.Create false
let hasValue =
    value.View |> View.MapCached(fun v ->
        not (System.String.IsNullOrEmpty v))
let shouldFloat = isFocused.View <||> hasValue  // see here

let inputElement =
    Doc.InputType.Text
        [
            Attr.Class "weave-field__input"
            on.focus (fun _ _ -> isFocused.Value <- true)
            on.blur (fun _ _ -> isFocused.Value <- false)
        ]
        value

Field.create(
    inputElement,
    isFocused.View,
    shouldFloat,
    labelText = View.Const "Custom Input",
    attrs = [ Field.Variant.outlined ]
)"""

    Helpers.codeSampleSection "Custom Input" description content code

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "Complete API reference for Field and FieldHelpText. Field exposes a single create overload for wrapping custom input elements with field chrome.")
      [
        Helpers.apiTable "Field.create" [
          Helpers.apiParam "inputElement" "Doc" "" "Pre-built input element to wrap with field chrome"
          Helpers.apiParam "isFocused" "View<bool>" "" "View tracking whether the input has focus"
          Helpers.apiParam
            "shouldFloat"
            "View<bool>"
            ""
            "View controlling when the label floats above the input"
          Helpers.apiParam "?labelText" "View<string>" "\"\"" "Floating label displayed above the input"
          Helpers.apiParam
            "?showHelpText"
            "View<bool>"
            "View.Const false"
            "Whether to display the help text area"
          Helpers.apiParam
            "?helpText"
            "Doc"
            "Doc.Empty"
            "Content shown below the field when showHelpText is true"
          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the field is interactive"
          Helpers.apiParam "?startAdornment" "Doc" "" "Content placed before the input"
          Helpers.apiParam "?endAdornment" "Doc" "" "Content placed after the input"
          Helpers.apiParam "?typoAttrs" "Attr list" "" "Typography attributes applied to the field wrapper"
          Helpers.apiParam "?inputId" "string" "" "HTML id linking the label to the input via for/id"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
        ]

        Helpers.apiTable "FieldHelpText.create" [
          Helpers.apiParam "content" "Doc" "" "Help text content displayed below the field"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the help text element"
        ]

        Helpers.styleModuleTable "Field.Variant" [
          ("standard", "Underline-only input style (default)")
          ("filled", "Filled background with underline")
          ("outlined", "Bordered outline with floating label notch")
        ]

        Helpers.styleModuleTable "Field.Color" [
          ("primary", "Primary brand color accent")
          ("secondary", "Secondary brand color accent")
          ("tertiary", "Tertiary brand color accent")
          ("error", "Error/red accent — use for validation errors")
          ("warning", "Warning/orange accent")
          ("success", "Success/green accent")
          ("info", "Info/blue accent")
        ]

        Helpers.styleModuleTable "Field.Width" [ ("full", "Field stretches to fill its container width") ]

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
        Helpers.pageTitle "Field"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Field is the base component for wrapping custom input elements with field chrome — floating labels, adornments, help text, and variant styling. For standard text inputs use TextField; for numbers use NumericField."
        ]
        Helpers.divider ()
        whenToUseSection ()
        Helpers.divider ()
        customInputExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
