namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave

[<JavaScript>]
module RadioButtonExamples =

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

  let private basicRadioExample () =
    let description =
      Helpers.bodyText
        "Share a selection Var and native input name within a group. Arrow keys move between options; use a different name for every independent group."

    let content =
      let selected = Var.Create "Standard"

      let createOption label =
        Radio.create (
          selected,
          label,
          displayText = View.Const label,
          inputAttrs = [ Attr.Create "name" "delivery" ]
        )

      fieldset [] [
        legend [] [ text "Delivery" ]
        comparisonGrid 3 [ createOption "Standard"; createOption "Express"; createOption "Collection" ]
      ]

    let code =
      """let selected = Var.Create "Standard"

let createOption label =
  Radio.create (
    selected,
    label,
    displayText = View.Const label,
    inputAttrs = [ Attr.Create "name" "delivery" ]
  )

fieldset [] [
  legend [] [ text "Delivery" ]
  createOption "Standard"
  createOption "Express"
  createOption "Collection"
]
"""

    Helpers.codeSampleSection "Basic Radio" description content code

  let private radioWithDynamicLabel () =
    let description =
      Helpers.bodyText "Map the selection to label text. Selecting either option updates the same Var."

    let content =
      let selected = Var.Create "Standard"

      let expressLabel =
        selected.View
        |> View.Map(fun value ->
          if value = "Express" then
            "Express selected"
          else
            "Choose express")

      fieldset [] [
        legend [] [ text "Delivery" ]
        comparisonGrid 2 [
          Radio.create (
            selected,
            "Standard",
            displayText = View.Const "Standard",
            inputAttrs = [ Attr.Create "name" "dynamic-delivery" ]
          )
          Radio.create (
            selected,
            "Express",
            displayText = expressLabel,
            inputAttrs = [ Attr.Create "name" "dynamic-delivery" ]
          )
        ]
      ]

    let code =
      """let selected = Var.Create "Standard"

let expressLabel =
  selected.View
  |> View.Map(fun value ->
    if value = "Express" then
      "Express selected"
    else
      "Choose express"
  )

fieldset [] [
  legend [] [ text "Delivery" ]
  Radio.create (
    selected,
    "Standard",
    displayText = View.Const "Standard",
    inputAttrs = [ Attr.Create "name" "dynamic-delivery" ]
  )

  Radio.create (
    selected,
    "Express",
    displayText = expressLabel,
    inputAttrs = [ Attr.Create "name" "dynamic-delivery" ]
  )
]
"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Radio.") [
      Helpers.apiTable "Radio.create" [
        Helpers.apiParam
          "userSelection"
          "Var<'T>"
          ""
          "Two-way binding for the selected value shared across a group"
        Helpers.apiParam "value" "'T" "" "The value this radio button represents (must support equality)"
        Helpers.apiParam
          "?displayText"
          "View<string>"
          ""
          "Reactive label text displayed next to the radio button"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether this radio button is interactive"
        Helpers.apiParam
          "?inputAttrs"
          "Attr list"
          "[]"
          "Attributes for the native input, including its radio group name"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (size, color, content placement, etc.)"
      ]

      Helpers.styleModuleTable "Radio.Size" [
        ("small", "Small radio button")
        ("medium", "Medium radio button (default)")
        ("large", "Large radio button")
      ]

      Helpers.styleModuleTable "Radio.Color" [
        ("primary", "Primary brand color when selected")
        ("secondary", "Secondary brand color when selected")
        ("tertiary", "Tertiary brand color when selected")
        ("error", "Error/red color when selected")
        ("warning", "Warning/orange color when selected")
        ("success", "Success/green color when selected")
        ("info", "Info/blue color when selected")
      ]

      Helpers.styleModuleTable "Radio.ContentPlacement" [
        ("right", "Label to the right of the radio button (default)")
        ("left", "Label to the left")
        ("top", "Label above the radio button")
        ("bottom", "Label below the radio button")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Radio Button"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Radio component allows users to select a single option from a set. It supports different sizes, colors, and can be disabled."
        ]

        Helpers.divider ()
        basicRadioExample ()
        Helpers.divider ()
        radioWithDynamicLabel ()
        Helpers.divider ()
        RadioConfigurator.render ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
