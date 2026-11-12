namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module FieldExamples =

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
            "?validation"
            "FieldValidation"
            ""
            "Error presentation; apply ValidationInput.attrs to the custom input for ARIA and blur tracking"
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

        Helpers.styleModuleTable "Field.Label" [ ("above", "Keep the label above an outlined field") ]

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
        span [ attr.id "when-to-use" ] []
        FieldConfigurator.render ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
