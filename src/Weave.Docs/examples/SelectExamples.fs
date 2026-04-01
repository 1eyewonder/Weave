namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module SelectExamples =

  let private basicExample () =
    let selected = Var.Create<string option> None

    let items =
      [ "Apple"; "Banana"; "Cherry"; "Date"; "Elderberry" ]
      |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
      |> View.Const

    let description =
      Helpers.bodyText
        "A basic single-select dropdown. Click the field to open, select an item to close. The label floats when a value is selected or the dropdown is open."

    let content =
      div [] [
        Grid.create (
          [
            GridItem.create (
              Select.create (
                items,
                selected,
                labelText = View.Const "Fruit",
                placeholder = View.Const "Choose a fruit",
                attrs = [ Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )

            GridItem.create (
              selected.View
              |> Doc.BindView(fun v ->
                div [ Typography.body2; Margin.Top.extraSmall ] [
                  text (sprintf "Selected: %s" (v |> Option.defaultValue "Nothing"))
                ]),
              attrs = [
                GridItem.Span.twelve
                GridItem.Span.Small.six
                Flex.Flex.allSizes
                AlignItems.center
              ]
            )
          ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let items =
    [ "Apple"; "Banana"; "Cherry"; "Date"; "Elderberry" ]
    |> List.map (fun fruit ->
        SelectItem.create (text fruit, fruit, fruit))
    |> View.Const

Select.create(
    items,
    selected,
    labelText = View.Const "Fruit",
    placeholder = View.Const "Choose a fruit",
    attrs = [ Select.Color.primary ]
)"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private variantsExample () =
    let standardVal = Var.Create<string option> None
    let filledVal = Var.Create<string option> None
    let outlinedVal = Var.Create<string option> None

    let items =
      [ "Red"; "Green"; "Blue"; "Yellow"; "Purple" ]
      |> List.map (fun color -> SelectItem.create (text color, color, color))
      |> View.Const

    let description =
      Helpers.bodyText
        "Select supports all three Field variants: Standard (default), Filled, and Outlined. Pass the variant parameter to control the input chrome styling."

    let content =
      Grid.create (
        [
          GridItem.create (
            Select.create (
              items,
              standardVal,
              variant = Field.Variant.Standard,
              labelText = View.Const "Standard",
              attrs = [ Select.Color.primary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )

          GridItem.create (
            Select.create (
              items,
              filledVal,
              variant = Field.Variant.Filled,
              labelText = View.Const "Filled",
              attrs = [ Select.Color.secondary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )

          GridItem.create (
            Select.create (
              items,
              outlinedVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Outlined",
              attrs = [ Select.Color.tertiary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let items =
    [ "Red"; "Green"; "Blue"; "Yellow"; "Purple" ]
    |> List.map (fun color ->
        SelectItem.create (text color, color, color))
    |> View.Const

Select.create(
    items, selected,
    variant = Field.Variant.Standard,  // see here
    labelText = View.Const "Standard"
)

Select.create(
    items, selected,
    variant = Field.Variant.Filled,
    labelText = View.Const "Filled"
)

Select.create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Outlined"
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private clearableExample () =
    let selected = Var.Create<string option>(Some "Tokyo")

    let items =
      [ "Tokyo"; "London"; "New York"; "Paris"; "Sydney"; "Berlin" ]
      |> List.map (fun city -> SelectItem.create (text city, city, city))
      |> View.Const

    let description =
      Helpers.bodyText
        "When clearable is enabled, a clear button appears when a value is selected. Click the X to reset the selection to None."

    let content =
      Grid.create (
        [
          GridItem.create (
            Select.create (
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "City",
              clearable = View.Const true,
              attrs = [ Select.Color.primary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )

          GridItem.create (
            selected.View
            |> Doc.BindView(fun v ->
              div [ Typography.body2; Margin.Top.extraSmall ] [
                text (sprintf "Selected: %s" (v |> Option.defaultValue "Nothing"))
              ]),
            attrs = [
              GridItem.Span.twelve
              GridItem.Span.Small.six
              Flex.Flex.allSizes
              AlignItems.center
            ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> (Some "Tokyo")

let items =
    [ "Tokyo"; "London"; "New York"; "Paris"; "Sydney"; "Berlin" ]
    |> List.map (fun city ->
        SelectItem.create (text city, city, city))
    |> View.Const

Select.create(
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "City",
    clearable = View.Const true  // see here
)"""

    Helpers.codeSampleSection "Clearable" description content code

  let private searchableExample () =
    let selected = Var.Create<string option> None

    let countries = [
      "Argentina"
      "Australia"
      "Brazil"
      "Canada"
      "China"
      "Denmark"
      "Egypt"
      "France"
      "Germany"
      "India"
      "Japan"
      "Mexico"
      "Norway"
      "Portugal"
      "Spain"
      "Sweden"
      "Switzerland"
      "United Kingdom"
      "United States"
    ]

    let items =
      countries |> List.map (fun c -> SelectItem.create (text c, c, c)) |> View.Const

    let description =
      Helpers.bodyText
        "Enable search to filter the dropdown items by typing. The search input auto-focuses when the dropdown opens. Filtering is case-insensitive on the item's Text field."

    let content =
      Grid.create (
        [
          GridItem.create (
            Select.create (
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Country",
              searchable = true,
              clearable = View.Const true,
              attrs = [ Select.Color.primary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let items =
    [ "Argentina"; "Australia"; "Brazil"; "Canada"; "France"; "Germany"; "Japan" ]
    |> List.map (fun c ->
        SelectItem.create (text c, c, c))
    |> View.Const

Select.create(
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Country",
    searchable = true,  // see here
    clearable = View.Const true
)"""

    Helpers.codeSampleSection "Searchable" description content code

  let private multiSelectExample () =
    let selected = Var.Create<Set<string>> Set.empty

    let items =
      [ "Reading"; "Gaming"; "Cooking"; "Hiking"; "Photography"; "Music"; "Travel" ]
      |> List.map (fun hobby -> SelectItem.create (text hobby, hobby, hobby))
      |> View.Const

    let description =
      Helpers.bodyText
        "Multi-select mode allows selecting multiple values. Items show checkboxes and the dropdown stays open on selection. The display shows a comma-separated summary of selected items."

    let content =
      div [] [
        Grid.create (
          [
            GridItem.create (
              MultiSelect.create (
                items,
                selected,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Hobbies",
                placeholder = View.Const "Select hobbies",
                clearable = View.Const true,
                attrs = [ Select.Color.secondary ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )

            GridItem.create (
              selected.View
              |> Doc.BindView(fun sel ->
                let display =
                  if Set.isEmpty sel then
                    "Nothing"
                  else
                    sel |> Set.toList |> String.concat ", "

                div [ Typography.body2; Margin.Top.extraSmall ] [ text (sprintf "Selected: %s" display) ]),
              attrs = [
                GridItem.Span.twelve
                GridItem.Span.Small.six
                Flex.Flex.allSizes
                AlignItems.center
              ]
            )
          ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let items =
    [ "Reading"; "Gaming"; "Cooking"; "Hiking"; "Photography" ]
    |> List.map (fun hobby ->
        SelectItem.create (text hobby, hobby, hobby))
    |> View.Const

MultiSelect.create(
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Hobbies",
    placeholder = View.Const "Select hobbies",
    clearable = View.Const true
)"""

    Helpers.codeSampleSection "Multi-Select" description content code

  let private selectAllExample () =
    let selected = Var.Create<Set<string>> Set.empty

    let items =
      [ "Admin"; "Editor"; "Viewer"; "Moderator"; "Contributor" ]
      |> List.map (fun role -> SelectItem.create (text role, role, role))
      |> View.Const

    let description =
      Helpers.bodyText
        "Multi-select with a Select All row. When all items are selected the checkbox shows a checkmark; when some are selected it shows an indeterminate dash. Custom selection text can summarize the selection count."

    let content =
      Grid.create (
        [
          GridItem.create (
            MultiSelect.create (
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Roles",
              showSelectAll = true,
              selectAllText = "All Roles",
              searchable = true,
              clearable = View.Const true,
              selectionText = (fun sel -> sprintf "%d role(s) selected" (Set.count sel)),
              attrs = [ Select.Color.tertiary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let items =
    [ "Admin"; "Editor"; "Viewer"; "Moderator"; "Contributor" ]
    |> List.map (fun role ->
        SelectItem.create (text role, role, role))
    |> View.Const

MultiSelect.create(
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Roles",
    showSelectAll = true,  // see here
    selectAllText = "All Roles",
    searchable = true,
    clearable = View.Const true,
    selectionText = (fun sel ->
        sprintf "%d role(s) selected" (Set.count sel))
)"""

    Helpers.codeSampleSection "Select All" description content code

  let private customRenderingExample () =
    let selected = Var.Create<string option> None

    let makeItem emoji name =
      SelectItem.create (
        span [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [
          span [ Attr.Style "font-size" "1.4em" ] [ text emoji ]
          text name
        ],
        name,
        name,
        selectedContent =
          span [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "6px" ] [
            span [ Attr.Style "font-size" "1.2em" ] [ text emoji ]
            text name
          ]
      )

    let items =
      [
        makeItem "\U0001F1FA\U0001F1F8" "United States"
        makeItem "\U0001F1EC\U0001F1E7" "United Kingdom"
        makeItem "\U0001F1EB\U0001F1F7" "France"
        makeItem "\U0001F1E9\U0001F1EA" "Germany"
        makeItem "\U0001F1EF\U0001F1F5" "Japan"
        makeItem "\U0001F1E7\U0001F1F7" "Brazil"
        makeItem "\U0001F1E6\U0001F1FA" "Australia"
      ]
      |> View.Const

    let description =
      Helpers.bodyText
        "Items support custom rendering via the Content field. Use the selectedContent parameter to provide a different display when the item is selected in the field (e.g., a compact version with a flag)."

    let content =
      Grid.create (
        [
          GridItem.create (
            Select.create (
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Country",
              searchable = true,
              clearable = View.Const true,
              attrs = [ Select.Color.primary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let makeItem emoji name =
    SelectItem.create (
        span [ Attr.Style "gap" "8px" ] [
            span [ Attr.Style "font-size" "1.4em" ] [ text emoji ]
            text name
        ],
        name,
        name,
        selectedContent =  // see here
            span [ Attr.Style "gap" "6px" ] [
                span [ Attr.Style "font-size" "1.2em" ] [ text emoji ]
                text name
            ]
    )

let items =
    [ makeItem "\U0001F1FA\U0001F1F8" "United States"
      makeItem "\U0001F1EC\U0001F1E7" "United Kingdom"
      makeItem "\U0001F1EB\U0001F1F7" "France" ]
    |> View.Const

Select.create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Country",
    searchable = true,
    clearable = View.Const true
)"""

    Helpers.codeSampleSection "Custom Item Rendering" description content code

  let private disabledReadonlyExample () =
    let disabledVal = Var.Create<string option>(Some "Locked")
    let readonlyVal = Var.Create<string option>(Some "Read Only")

    let items =
      [ "Locked"; "Open"; "Pending" ]
      |> List.map (fun s -> SelectItem.create (text s, s, s))
      |> View.Const

    let description =
      Helpers.bodyText
        "Select can be disabled or set to read-only. Disabled prevents all interaction and dims the component. Read-only shows the value but prevents changes."

    let content =
      Grid.create (
        [
          GridItem.create (
            Select.create (
              items,
              disabledVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Disabled",
              enabled = View.Const false,
              attrs = [ Select.Color.primary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )

          GridItem.create (
            Select.create (
              items,
              readonlyVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Read Only",
              readOnly = View.Const true,
              attrs = [ Select.Color.primary ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let items =
    [ "Locked"; "Open"; "Pending" ]
    |> List.map (fun s ->
        SelectItem.create (text s, s, s))
    |> View.Const

// Disabled — prevents all interaction and dims the component
let disabledVal = Var.Create<string option> (Some "Locked")

Select.create(
    items, disabledVal,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Disabled",
    enabled = View.Const false  // see here
)

// Read Only — shows value but prevents changes
let readOnlyVal = Var.Create<string option> (Some "Locked")

Select.create(
    items, readOnlyVal,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Read Only",
    readOnly = View.Const true  // see here
)"""

    Helpers.codeSampleSection "Disabled & Read Only" description content code

  let private colorsExample () =
    let colors = [
      "Primary", Select.Color.primary
      "Secondary", Select.Color.secondary
      "Tertiary", Select.Color.tertiary
      "Error", Select.Color.error
      "Warning", Select.Color.warning
      "Success", Select.Color.success
      "Info", Select.Color.info
    ]

    let description =
      Helpers.bodyText "Select supports all brand colors, passed via attrs using Select.Color.*."

    let content =
      let items =
        [ "Option A"; "Option B"; "Option C" ]
        |> List.map (fun o -> SelectItem.create (text o, o, o))
        |> View.Const

      Grid.create (
        colors
        |> List.map (fun (label, colorAttr) ->
          let v = Var.Create<string option>(Some "Option A")

          GridItem.create (
            Select.create (
              items,
              v,
              variant = Field.Variant.Outlined,
              labelText = View.Const label,
              attrs = [ colorAttr ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> (Some "Option A")

let items =
    [ "Option A"; "Option B"; "Option C" ]
    |> List.map (fun o ->
        SelectItem.create (text o, o, o))
    |> View.Const

Select.create(items, selected, variant = Field.Variant.Outlined, labelText = View.Const "Primary", attrs = [ Select.Color.primary ])
Select.create(items, selected, variant = Field.Variant.Outlined, labelText = View.Const "Secondary", attrs = [ Select.Color.secondary ])
Select.create(items, selected, variant = Field.Variant.Outlined, labelText = View.Const "Tertiary", attrs = [ Select.Color.tertiary ])
Select.create(items, selected, variant = Field.Variant.Outlined, labelText = View.Const "Error", attrs = [ Select.Color.error ])
Select.create(items, selected, variant = Field.Variant.Outlined, labelText = View.Const "Warning", attrs = [ Select.Color.warning ])
Select.create(items, selected, variant = Field.Variant.Outlined, labelText = View.Const "Success", attrs = [ Select.Color.success ])
Select.create(items, selected, variant = Field.Variant.Outlined, labelText = View.Const "Info", attrs = [ Select.Color.info ])"""

    Helpers.codeSampleSection "Colors" description content code

  let private widthExample () =
    let autoVal = Var.Create<string option> None
    let fullVal = Var.Create<string option> None
    let fitVal = Var.Create<string option> None

    let items =
      [ "Short"; "Medium Text"; "A Longer Option Here" ]
      |> List.map (fun o -> SelectItem.create (text o, o, o))
      |> View.Const

    let description =
      Helpers.bodyText
        "Select supports three width modes: Auto (default, inline sizing), Full (100% of container), and Fit Content (sizes to content). Pass width classes via attrs using Select.Width.full or Select.Width.fitContent."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              div [ Typography.body2; Margin.Bottom.extraSmall ] [ text "Auto (default)" ]

              Select.create (
                items,
                autoVal,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Auto",
                attrs = [ Select.Color.primary ]
              )
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )

          GridItem.create (
            div [ Attr.Style "width" "100%" ] [
              div [ Typography.body2; Margin.Bottom.extraSmall ] [ text "Full Width" ]

              Select.create (
                items,
                fullVal,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Full",
                attrs = [ Select.Color.secondary; Select.Width.full ]
              )
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )

          GridItem.create (
            div [] [
              div [ Typography.body2; Margin.Bottom.extraSmall ] [ text "Fit Content" ]

              Select.create (
                items,
                fitVal,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Fit Content",
                attrs = [ Select.Color.tertiary; Select.Width.fitContent ]
              )
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let items =
    [ "Short"; "Medium Text"; "A Longer Option Here" ]
    |> List.map (fun o ->
        SelectItem.create (text o, o, o))
    |> View.Const

// Auto (default) — inline, sizes to min-width
Select.create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Auto"
)

// Full Width — 100% of container
Select.create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Full",
    attrs = [ Select.Width.full ]  // see here
)

// Fit Content — sizes to content
Select.create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Fit Content",
    attrs = [ Select.Width.fitContent ]  // see here
)"""

    Helpers.codeSampleSection "Width Modes" description content code

  type private Language = {
    Code: string
    Name: string
    Native: string
  }

  let private genericTypeExample () =
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
        Native = "Espa\u00f1ol"
      }
      {
        Code = "fr"
        Name = "French"
        Native = "Fran\u00e7ais"
      }
      {
        Code = "de"
        Name = "German"
        Native = "Deutsch"
      }
      {
        Code = "ja"
        Name = "Japanese"
        Native = "\u65e5\u672c\u8a9e"
      }
      {
        Code = "zh"
        Name = "Chinese"
        Native = "\u4e2d\u6587"
      }
    ]

    let items =
      languages
      |> List.map (fun lang ->
        SelectItem.create (span [] [ text (sprintf "%s (%s)" lang.Name lang.Native) ], lang.Name, lang))
      |> View.Const

    let description =
      Helpers.bodyText
        "Select is generic over the value type. Here we use a custom record type as the item value. The Text field is used for search filtering while Value holds the full domain object."

    let content =
      Grid.create (
        [
          GridItem.create (
            Select.create (
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Language",
              searchable = true,
              clearable = View.Const true,
              attrs = [ Select.Color.info ]
            ),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
          )

          GridItem.create (
            selected.View
            |> Doc.BindView(fun v ->
              match v with
              | Some lang ->
                div [ Typography.body2; Margin.Top.extraSmall ] [
                  text (sprintf "Code: %s | Name: %s | Native: %s" lang.Code lang.Name lang.Native)
                ]
              | None -> div [ Typography.body2; Margin.Top.extraSmall ] [ text "No language selected" ]),
            attrs = [
              GridItem.Span.twelve
              GridItem.Span.Small.six
              Flex.Flex.allSizes
              AlignItems.center
            ]
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

type Language = { Code: string; Name: string; Native: string }

let selected = Var.Create<Language option> None

let languages = [
    { Code = "en"; Name = "English"; Native = "English" }
    { Code = "es"; Name = "Spanish"; Native = "Español" }
    { Code = "fr"; Name = "French"; Native = "Français" }
]

let items =
    languages
    |> List.map (fun lang ->
        SelectItem.create (
            text (sprintf "%s (%s)" lang.Name lang.Native),
            lang.Name,  // Text for search filtering
            lang))      // Value is the full record
    |> View.Const

Select.create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Language",
    searchable = true,
    clearable = View.Const true
)"""

    Helpers.codeSampleSection "Generic Types" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Select, MultiSelect, and SelectItem.") [
      Helpers.apiTable "Select.create" [
        Helpers.apiParam "items" "View<SelectItemDef<'T> list>" "" "Reactive list of selectable items"
        Helpers.apiParam
          "selectedValue"
          "Var<'T option>"
          ""
          "Two-way binding for the selected value; None when nothing is selected"
        Helpers.apiParam "?variant" "Variant" "Standard" "Visual style — Standard, Filled, or Outlined"
        Helpers.apiParam "?labelText" "View<string>" "" "Floating label displayed above the input"
        Helpers.apiParam "?placeholder" "View<string>" "" "Placeholder text shown when no value is selected"
        Helpers.apiParam
          "?showHelpText"
          "View<bool>"
          "View.Const false"
          "Whether to display the help text area"
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
        Helpers.apiParam "selectedValues" "Var<Set<'T>>" "" "Two-way binding for the set of selected values"
        Helpers.apiParam "?variant" "Variant" "Standard" "Visual style — Standard, Filled, or Outlined"
        Helpers.apiParam "?labelText" "View<string>" "" "Floating label displayed above the input"
        Helpers.apiParam "?placeholder" "View<string>" "" "Placeholder text shown when no values are selected"
        Helpers.apiParam
          "?showHelpText"
          "View<bool>"
          "View.Const false"
          "Whether to display the help text area"
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
        ("primary", "Default blue accent color")
        ("secondary", "Purple accent color")
        ("tertiary", "Teal accent color")
        ("error", "Red accent — use for validation errors")
        ("warning", "Orange accent color")
        ("success", "Green accent color")
        ("info", "Light blue accent color")
      ]

      Helpers.styleModuleTable "Select.Width" [
        ("full", "Select stretches to fill its container width")
        ("fitContent", "Select shrinks to fit the selected content")
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
        variantsExample ()
        Helpers.divider ()
        widthExample ()
        Helpers.divider ()
        colorsExample ()
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
