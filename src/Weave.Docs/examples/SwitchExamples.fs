namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave

[<JavaScript>]
module SwitchExamples =

  let private centered content = Grid.create [ GridItem.create content ]

  let private basicSwitchExample () =
    let description =
      Helpers.bodyText "Bind a Var<bool> to the switch. Interacting with the control updates that value."

    let content =
      let isChecked = Var.Create false

      Switch.create (isChecked, text "Email updates")

    let code =
      """let isChecked = Var.Create false

Switch.create (isChecked, text "Email updates")
"""

    Helpers.codeSampleSection "Basic Switch" description (centered content) code

  let private switchWithDynamicLabel () =
    let description =
      Helpers.bodyText "Map the same state to a reactive label. The control and its text stay in sync."

    let content =
      let isChecked = Var.Create false

      let label =
        isChecked.View
        |> View.Map(fun value -> if value then "Email updates on" else "Email updates off")
        |> Doc.TextView

      Switch.create (isChecked, label)

    let code =
      """let isChecked = Var.Create false

let label =
  isChecked.View
  |> View.Map(fun value ->
    if value then "Email updates on" else "Email updates off")
  |> Doc.TextView

Switch.create (isChecked, label)
"""

    Helpers.codeSampleSection "Dynamic Label" description (centered content) code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Switch.") [
      Helpers.apiTable "Switch.create" [
        Helpers.apiParam "isChecked" "Var<bool>" "" "Two-way binding for the on/off state"
        Helpers.apiParam "?content" "Doc" "" "Optional label content displayed next to the switch"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the switch is interactive"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (size, color, content placement, etc.)"
      ]

      Helpers.styleModuleTable "Switch.Size" [
        ("small", "Small switch")
        ("medium", "Medium switch (default)")
        ("large", "Large switch")
      ]

      Helpers.styleModuleTable "Switch.Color" [
        ("primary", "Primary brand color when on")
        ("secondary", "Secondary brand color when on")
        ("tertiary", "Tertiary brand color when on")
        ("error", "Error/red color when on")
        ("warning", "Warning/orange color when on")
        ("success", "Success/green color when on")
        ("info", "Info/blue color when on")
      ]

      Helpers.styleModuleTable "Switch.ContentPlacement" [
        ("right", "Label to the right of the switch (default)")
        ("left", "Label to the left")
        ("top", "Label above the switch")
        ("bottom", "Label below the switch")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Switch"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Switch component allows users to toggle between two states. It supports different sizes, colors, and can be disabled."
        ]

        Helpers.divider ()
        basicSwitchExample ()
        Helpers.divider ()
        switchWithDynamicLabel ()
        Helpers.divider ()
        ToggleConfigurator.render "Switch"
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
