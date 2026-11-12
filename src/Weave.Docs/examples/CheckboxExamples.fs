namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module CheckboxExamples =

  let private centered content =
    div [
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      AlignItems.center
      Gap.All.g3
    ] [ content ]

  let private basicCheckboxExample () =
    let description =
      Helpers.bodyText "Bind a Var<bool> to the checkbox. Interacting with the control updates that value."

    let content =
      let isChecked = Var.Create false

      Checkbox.create (isChecked, View.Const "Email updates")

    let code =
      """let isChecked = Var.Create false

Checkbox.create (isChecked, View.Const "Email updates")
"""

    Helpers.codeSampleSection "Basic Checkbox" description (centered content) code

  let private checkboxWithDynamicLabel () =
    let description =
      Helpers.bodyText "Map the same state to a reactive label. The control and its text stay in sync."

    let content =
      let isChecked = Var.Create false

      let label =
        isChecked.View
        |> View.Map(fun value -> if value then "Email updates on" else "Email updates off")

      Checkbox.create (isChecked, displayText = label)

    let code =
      """let isChecked = Var.Create false

let label =
  isChecked.View
  |> View.Map(fun value ->
    if value then "Email updates on" else "Email updates off"
  )

Checkbox.create (isChecked, displayText = label)
"""

    Helpers.codeSampleSection "Dynamic Label" description (centered content) code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Checkbox.") [
      Helpers.apiTable "Checkbox.create" [
        Helpers.apiParam "isChecked" "Var<bool>" "" "Two-way binding for the checked state"
        Helpers.apiParam "?displayText" "View<string>" "" "Reactive label text displayed next to the checkbox"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the checkbox is interactive"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (size, color, content placement, etc.)"
      ]

      Helpers.styleModuleTable "Checkbox.Size" [
        ("small", "Small checkbox")
        ("medium", "Medium checkbox (default)")
        ("large", "Large checkbox")
      ]

      Helpers.styleModuleTable "Checkbox.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]

      Helpers.styleModuleTable "Checkbox.ContentPlacement" [
        ("right", "Label to the right of the checkbox (default)")
        ("left", "Label to the left")
        ("top", "Label above the checkbox")
        ("bottom", "Label below the checkbox")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Checkbox"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Checkbox component allows users to select one or more options from a set. It supports different sizes, colors, and can be disabled."
        ]

        Helpers.divider ()
        basicCheckboxExample ()
        Helpers.divider ()
        checkboxWithDynamicLabel ()
        Helpers.divider ()
        ToggleConfigurator.render "Checkbox"
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
