namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module SelectExamples =

  let private centered content = Grid.create [ GridItem.create content ]

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
      attrs = [ AlignItems.end' ]
    )

  let private basicExample () =
    let description =
      Helpers.bodyText
        "A basic single-select dropdown. Click the field to open, select an item to close. The label floats when a value is selected or the dropdown is open."

    let content =
      let selected = Var.Create<string option> None

      let items =
        [ "Apple"; "Banana"; "Cherry"; "Date"; "Elderberry" ]
        |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const "Fruit",
        placeholder = View.Const "Choose a fruit",
        attrs = [ Select.Color.primary ]
      )

    let code =
      """let selected = Var.Create<string option> None

let items =
  [ "Apple"; "Banana"; "Cherry"; "Date"; "Elderberry" ]
  |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
  |> View.Const

Select.create (
  items,
  selected,
  labelText = View.Const "Fruit",
  placeholder = View.Const "Choose a fruit",
  attrs = [ Select.Color.primary ]
)
"""

    Helpers.codeSampleSection "Basic Usage" description (centered content) code

  let private clearableExample () =
    let description =
      Helpers.bodyText
        "When clearable is enabled, a clear button appears when a value is selected. Click the X to reset the selection to None."

    let content =
      let selected = Var.Create<string option>(Some "Tokyo")

      let items =
        [ "Tokyo"; "London"; "New York"; "Paris"; "Sydney"; "Berlin" ]
        |> List.map (fun city -> SelectItem.create (text city, city, city))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const "City",
        clearable = View.Const true,
        attrs = [ Select.Variant.outlined ]
      )

    let code =
      """let selected = Var.Create<string option>(Some "Tokyo")

let items =
  [ "Tokyo"; "London"; "New York"; "Paris"; "Sydney"; "Berlin" ]
  |> List.map (fun city -> SelectItem.create (text city, city, city))
  |> View.Const

Select.create (
  items,
  selected,
  labelText = View.Const "City",
  clearable = View.Const true,
  attrs = [ Select.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Clearable" description (centered content) code

  let private searchableExample () =
    let description =
      Helpers.bodyText
        "Enable search to filter the dropdown items by typing. The search input auto-focuses when the dropdown opens. Filtering is case-insensitive on the item's Text field."

    let content =
      let selected = Var.Create<string option> None

      let items =
        [ "Argentina"; "Australia"; "Brazil"; "Canada"; "France"; "Germany"; "Japan" ]
        |> List.map (fun c -> SelectItem.create (text c, c, c))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const "Country",
        searchable = true,
        clearable = View.Const true,
        attrs = [ Select.Variant.outlined ]
      )

    let code =
      """let selected = Var.Create<string option> None

let items =
  [
    "Argentina"
    "Australia"
    "Brazil"
    "Canada"
    "France"
    "Germany"
    "Japan"
  ]
  |> List.map (fun c -> SelectItem.create (text c, c, c))
  |> View.Const

Select.create (
  items,
  selected,
  labelText = View.Const "Country",
  searchable = true,
  clearable = View.Const true,
  attrs = [ Select.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Searchable" description (centered content) code

  let private multiSelectExample () =
    let description =
      Helpers.bodyText
        "Multi-select mode allows selecting multiple values. Items show checkboxes and the dropdown stays open on selection. The display shows a comma-separated summary of selected items."

    let content =
      let selected = Var.Create<Set<string>> Set.empty

      let items =
        [ "Reading"; "Gaming"; "Cooking"; "Hiking"; "Photography" ]
        |> List.map (fun hobby -> SelectItem.create (text hobby, hobby, hobby))
        |> View.Const

      MultiSelect.create (
        items,
        selected,
        labelText = View.Const "Hobbies",
        placeholder = View.Const "Select hobbies",
        clearable = View.Const true,
        attrs = [ Select.Variant.outlined ]
      )

    let code =
      """let selected = Var.Create<Set<string>> Set.empty

let items =
  [ "Reading"; "Gaming"; "Cooking"; "Hiking"; "Photography" ]
  |> List.map (fun hobby -> SelectItem.create (text hobby, hobby, hobby))
  |> View.Const

MultiSelect.create (
  items,
  selected,
  labelText = View.Const "Hobbies",
  placeholder = View.Const "Select hobbies",
  clearable = View.Const true,
  attrs = [ Select.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Multi-Select" description (centered content) code

  let private selectAllExample () =
    let description =
      Helpers.bodyText
        "Multi-select with a Select All row. Use the arrow keys to move between search, Select All, and the options; Space or Enter toggles Select All. The checkbox shows a checkmark when all items are selected and a dash when some are selected. Custom selection text can summarize the selection count."

    let content =
      let selected = Var.Create<Set<string>> Set.empty

      let items =
        [ "Admin"; "Editor"; "Viewer"; "Moderator"; "Contributor" ]
        |> List.map (fun role -> SelectItem.create (text role, role, role))
        |> View.Const

      MultiSelect.create (
        items,
        selected,
        labelText = View.Const "Roles",
        showSelectAll = true,
        selectAllText = "All Roles",
        searchable = true,
        clearable = View.Const true,
        selectionText = (fun sel -> sprintf "%d role(s) selected" (Set.count sel)),
        attrs = [ Select.Variant.outlined ]
      )

    let code =
      """let selected = Var.Create<Set<string>> Set.empty

let items =
  [ "Admin"; "Editor"; "Viewer"; "Moderator"; "Contributor" ]
  |> List.map (fun role -> SelectItem.create (text role, role, role))
  |> View.Const

MultiSelect.create (
  items,
  selected,
  labelText = View.Const "Roles",
  showSelectAll = true,
  selectAllText = "All Roles",
  searchable = true,
  clearable = View.Const true,
  selectionText = (fun sel -> sprintf "%d role(s) selected" (Set.count sel)),
  attrs = [ Select.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Select All" description (centered content) code

  let private customRenderingExample () =
    let description =
      Helpers.bodyText
        "Items support custom rendering via the Content field. Use the selectedContent parameter to provide a different display when the item is selected in the field (e.g., a compact version with a flag)."

    let content =
      let selected = Var.Create<string option> None

      let makeItem emoji name =
        SelectItem.create (
          span [ Attr.Style "gap" "8px" ] [
            span [ Attr.Style "font-size" "1.4em" ] [ text emoji ]
            text name
          ],
          name,
          name,
          selectedContent =
            span [ Attr.Style "gap" "6px" ] [
              span [ Attr.Style "font-size" "1.2em" ] [ text emoji ]
              text name
            ]
        )

      let items =
        [
          makeItem "\U0001F1FA\U0001F1F8" "United States"
          makeItem "\U0001F1EC\U0001F1E7" "United Kingdom"
          makeItem "\U0001F1EB\U0001F1F7" "France"
        ]
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const "Country",
        searchable = true,
        clearable = View.Const true,
        attrs = [ Select.Variant.outlined ]
      )

    let code =
      """let selected = Var.Create<string option> None

let makeItem emoji name =
  SelectItem.create (
    span [ Attr.Style "gap" "8px" ] [
      span [ Attr.Style "font-size" "1.4em" ] [ text emoji ]
      text name
    ],
    name,
    name,
    selectedContent =
      span [ Attr.Style "gap" "6px" ] [
        span [ Attr.Style "font-size" "1.2em" ] [ text emoji ]
        text name
      ]
  )

let items =
  [
    makeItem "\U0001F1FA\U0001F1F8" "United States"
    makeItem "\U0001F1EC\U0001F1E7" "United Kingdom"
    makeItem "\U0001F1EB\U0001F1F7" "France"
  ]
  |> View.Const

Select.create (
  items,
  selected,
  labelText = View.Const "Country",
  searchable = true,
  clearable = View.Const true,
  attrs = [ Select.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Custom Item Rendering" description (centered content) code

  let private disabledReadonlyExample () =
    let description =
      Helpers.bodyText
        "Select can be disabled or set to read-only. Disabled prevents all interaction and dims the component. Read-only shows the value but prevents changes."

    let content =
      let items =
        [ "Locked"; "Open"; "Pending" ]
        |> List.map (fun s -> SelectItem.create (text s, s, s))
        |> View.Const

      let disabledVal = Var.Create<string option>(Some "Locked")

      let readOnlyVal = Var.Create<string option>(Some "Locked")

      comparisonGrid 2 [
        Select.create (
          items,
          disabledVal,
          labelText = View.Const "Disabled",
          enabled = View.Const false,
          attrs = [ Select.Variant.outlined ]
        )

        Select.create (
          items,
          readOnlyVal,
          labelText = View.Const "Read Only",
          readOnly = View.Const true,
          attrs = [ Select.Variant.outlined ]
        )
      ]

    let code =
      """let items =
  [ "Locked"; "Open"; "Pending" ]
  |> List.map (fun s -> SelectItem.create (text s, s, s))
  |> View.Const

let disabledVal = Var.Create<string option>(Some "Locked")

let readOnlyVal = Var.Create<string option>(Some "Locked")

Select.create (
  items,
  disabledVal,
  labelText = View.Const "Disabled",
  enabled = View.Const false,
  attrs = [ Select.Variant.outlined ]
)

Select.create (
  items,
  readOnlyVal,
  labelText = View.Const "Read Only",
  readOnly = View.Const true,
  attrs = [ Select.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Disabled & Read Only" description content code

  let private placementExample () =
    let description =
      Helpers.bodyText
        "AnchorOrigin chooses a point on the trigger. TransformOrigin chooses the point on the popover that aligns with it. Open each field to compare left and right alignment."

    let content =
      let items =
        [ "Apple"; "Banana"; "Cherry" ]
        |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
        |> View.Const

      let createField anchor origin label =
        Select.create (
          items,
          Var.Create<string option> None,
          labelText = View.Const label,
          attrs = [ Select.Variant.outlined; anchor; origin ]
        )

      comparisonGrid 2 [
        createField Select.AnchorOrigin.bottomLeft Select.TransformOrigin.topLeft "Aligned left"

        createField Select.AnchorOrigin.bottomRight Select.TransformOrigin.topRight "Aligned right"
      ]

    let code =
      """let items =
  [ "Apple"; "Banana"; "Cherry" ]
  |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
  |> View.Const

let createField anchor origin label =
  Select.create (
    items,
    Var.Create<string option> None,
    labelText = View.Const label,
    attrs = [ Select.Variant.outlined; anchor; origin ]
  )

createField
  Select.AnchorOrigin.bottomLeft
  Select.TransformOrigin.topLeft
  "Aligned left"

createField
  Select.AnchorOrigin.bottomRight
  Select.TransformOrigin.topRight
  "Aligned right"
"""

    Helpers.codeSampleSection "Placement" description content code

  type private Language = {
    Code: string
    Name: string
    Native: string
  }

  let private genericTypeExample () =
    let description =
      Helpers.bodyText
        "Select is generic over the value type. Here we use a custom record type as the item value. The Text field is used for search filtering while Value holds the full domain object."

    let content =
      let selected = Var.Create<Language option> None

      let languages = [
        {
          Code = "en"
          Name = "English"
          Native = "English"
        }
        {
          Code = "es"
          Name = "Spanish"
          Native = "Español"
        }
        {
          Code = "fr"
          Name = "French"
          Native = "Français"
        }
      ]

      let items =
        languages
        |> List.map (fun lang ->
          SelectItem.create (text (sprintf "%s (%s)" lang.Name lang.Native), lang.Name, lang))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const "Language",
        searchable = true,
        clearable = View.Const true,
        attrs = [ Select.Variant.outlined ]
      )

    let code =
      """type Language = {
  Code: string
  Name: string
  Native: string
}

let selected = Var.Create<Language option> None

let languages = [
  {
    Code = "en"
    Name = "English"
    Native = "English"
  }
  {
    Code = "es"
    Name = "Spanish"
    Native = "Español"
  }
  {
    Code = "fr"
    Name = "French"
    Native = "Français"
  }
]

let items =
  languages
  |> List.map (fun lang ->
    SelectItem.create (
      text (sprintf "%s (%s)" lang.Name lang.Native),
      lang.Name,
      lang
    ))
  |> View.Const

Select.create (
  items,
  selected,
  labelText = View.Const "Language",
  searchable = true,
  clearable = View.Const true,
  attrs = [ Select.Variant.outlined ]
)
"""

    Helpers.codeSampleSection "Generic Types" description (centered content) code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Select, MultiSelect, and SelectItem.") [
      Helpers.apiTable "Select.create" [
        Helpers.apiParam "items" "View<SelectItemDef<'T> list>" "" "Reactive list of selectable items"
        Helpers.apiParam
          "selectedValue"
          "Var<'T option> or FormField<'T option>"
          ""
          "Selected value; a FormField automatically wires its validation"
        Helpers.apiParam "?labelText" "View<string>" "" "Floating label displayed above the input"
        Helpers.apiParam "?placeholder" "View<string>" "" "Placeholder text shown when no value is selected"
        Helpers.apiParam
          "?showHelpText"
          "View<bool>"
          "View.Const false"
          "Whether to display the help text area"
        Helpers.apiParam
          "?validation"
          "FieldValidation"
          ""
          "FormField.Validation: errors, styling, blur tracking, and ARIA (on the combobox)"
        Helpers.apiParam
          "?helpText"
          "Doc"
          "Doc.Empty"
          "Content shown below the select when showHelpText is true"
        Helpers.apiParam
          "?clearable"
          "View<bool>"
          "View.Const false"
          "Show a clear button when a value is selected"
        Helpers.apiParam "?searchable" "bool" "false" "Enable type-to-filter on the item list"
        Helpers.apiParam "?isOpen" "Var<bool>" "" "External control over the dropdown open state"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the select is interactive"
        Helpers.apiParam
          "?readOnly"
          "View<bool>"
          "View.Const false"
          "Display the selected value without allowing changes"
        Helpers.apiParam "?noItemsContent" "Doc" "" "Content shown when the filtered item list is empty"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
      ]

      Helpers.apiTable "MultiSelect.create" [
        Helpers.apiParam "items" "View<SelectItemDef<'T> list>" "" "Reactive list of selectable items"
        Helpers.apiParam
          "selectedValues"
          "Var<Set<'T>> or FormField<Set<'T>>"
          ""
          "Two-way binding for the set of selected values"
        Helpers.apiParam "?labelText" "View<string>" "" "Floating label displayed above the input"
        Helpers.apiParam "?placeholder" "View<string>" "" "Placeholder text shown when no values are selected"
        Helpers.apiParam
          "?showHelpText"
          "View<bool>"
          "View.Const false"
          "Whether to display the help text area"
        Helpers.apiParam
          "?validation"
          "FieldValidation"
          ""
          "FormField.Validation: errors, styling, blur tracking, and ARIA (on the combobox)"
        Helpers.apiParam
          "?helpText"
          "Doc"
          "Doc.Empty"
          "Content shown below the select when showHelpText is true"
        Helpers.apiParam
          "?selectionText"
          "Set<'T> -> string"
          ""
          "Custom function to format the display text from the selected set"
        Helpers.apiParam
          "?clearable"
          "View<bool>"
          "View.Const false"
          "Show a clear button when values are selected"
        Helpers.apiParam "?searchable" "bool" "false" "Enable type-to-filter on the item list"
        Helpers.apiParam "?showSelectAll" "bool" "false" "Show a Select All checkbox at the top of the list"
        Helpers.apiParam "?selectAllText" "string" "\"Select All\"" "Label for the Select All checkbox"
        Helpers.apiParam "?isOpen" "Var<bool>" "" "External control over the dropdown open state"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the select is interactive"
        Helpers.apiParam
          "?readOnly"
          "View<bool>"
          "View.Const false"
          "Display the selected values without allowing changes"
        Helpers.apiParam "?noItemsContent" "Doc" "" "Content shown when the filtered item list is empty"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
      ]

      Helpers.returnTypeNote
        "SelectItem.create returns a SelectItemDef<'T> record, not a Doc. Build items with SelectItem.create and pass them as a reactive list to Select.create or MultiSelect.create."

      Helpers.apiTable "SelectItem.create" [
        Helpers.apiParam "content" "Doc" "" "Visual content displayed in the dropdown list"
        Helpers.apiParam "text" "string" "" "Plain-text label used for search filtering and accessibility"
        Helpers.apiParam "value" "'T" "" "The typed value associated with this item"
        Helpers.apiParam
          "?selectedContent"
          "Doc"
          ""
          "Alternative content shown in the input when this item is selected"
        Helpers.apiParam "?disabled" "View<bool>" "View.Const false" "Whether this item is non-selectable"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the item element"
      ]

      Helpers.styleModuleTable "Select.Color" [
        ("primary", "Primary brand accent (default)")
        ("secondary", "Secondary brand accent")
        ("tertiary", "Tertiary brand accent")
        ("error", "Red accent — use for validation errors")
        ("warning", "Orange accent color")
        ("success", "Green accent color")
        ("info", "Informational accent")
      ]

      Helpers.styleModuleTable "Select.Label" [ ("above", "Keep the label above an outlined select") ]

      Helpers.styleModuleTable "Select.Width" [
        ("full", "Component root stretches to fill its container width")
        ("dynamic", "Trigger resizes to the currently selected value (opt-out of stable default)")
      ]

      Helpers.styleModuleTable "Select.PopoverWidth" [
        ("unbound",
         "Popover panel grows freely to content width, capped at min(400px, 90vw); default is root width")
      ]

      Helpers.styleModuleTable "Select.AnchorOrigin" [
        ("topLeft", "Anchor at top-left of trigger")
        ("topCenter", "Anchor at top-center of trigger")
        ("topRight", "Anchor at top-right of trigger")
        ("centerLeft", "Anchor at center-left of trigger")
        ("center", "Anchor at center of trigger")
        ("centerRight", "Anchor at center-right of trigger")
        ("bottomLeft", "Anchor at bottom-left of trigger (default)")
        ("bottomCenter", "Anchor at bottom-center of trigger")
        ("bottomRight", "Anchor at bottom-right of trigger")
      ]

      Helpers.styleModuleTable "Select.TransformOrigin" [
        ("topLeft", "Popover aligns its top-left to the anchor (default)")
        ("topCenter", "Popover aligns its top-center to the anchor")
        ("topRight", "Popover aligns its top-right to the anchor")
        ("centerLeft", "Popover aligns its center-left to the anchor")
        ("center", "Popover aligns its center to the anchor")
        ("centerRight", "Popover aligns its center-right to the anchor")
        ("bottomLeft", "Popover aligns its bottom-left to the anchor")
        ("bottomCenter", "Popover aligns its bottom-center to the anchor")
        ("bottomRight", "Popover aligns its bottom-right to the anchor")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Select"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Select components allow users to choose from a list of options in a dropdown. Supports single and multi-select, search filtering, clear, select-all, custom item rendering, and all Field variants."
        ]

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        SelectConfigurator.render ()
        Helpers.divider ()
        placementExample ()
        Helpers.divider ()
        clearableExample ()
        Helpers.divider ()
        searchableExample ()
        Helpers.divider ()
        multiSelectExample ()
        Helpers.divider ()
        selectAllExample ()
        Helpers.divider ()
        customRenderingExample ()
        Helpers.divider ()
        disabledReadonlyExample ()
        Helpers.divider ()
        genericTypeExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
