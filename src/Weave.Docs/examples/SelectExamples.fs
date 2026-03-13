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
      |> List.map (fun fruit -> Select.SelectItemDef.create (text fruit) fruit fruit)
      |> View.Const

    let description =
      Helpers.bodyText
        "A basic single-select dropdown. Click the field to open, select an item to close. The label floats when a value is selected or the dropdown is open."

    let content =
      div [] [
        Grid.Create(
          [
            GridItem.Create(
              Select.Create(
                items,
                selected,
                labelText = View.Const "Fruit",
                placeholder = View.Const "Choose a fruit",
                attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
              ),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6
            )

            GridItem.Create(
              selected.View
              |> Doc.BindView(fun v ->
                Body2.Div(
                  sprintf "Selected: %s" (v |> Option.defaultValue "Nothing"),
                  attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls ]
                )),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              attrs = [ cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ] ]
            )
          ]
        )
      ]

    let code =
      """open Weave


let selected = Var.Create<string option> None

let items =
    [ "Apple"; "Banana"; "Cherry"; "Date"; "Elderberry" ]
    |> List.map (fun fruit ->
        Select.SelectItemDef.create (text fruit) fruit fruit)
    |> View.Const

Select.Create(
    items,
    selected,
    labelText = View.Const "Fruit",
    placeholder = View.Const "Choose a fruit",
    attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
)
"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private variantsExample () =
    let standardVal = Var.Create<string option> None
    let filledVal = Var.Create<string option> None
    let outlinedVal = Var.Create<string option> None

    let items =
      [ "Red"; "Green"; "Blue"; "Yellow"; "Purple" ]
      |> List.map (fun color -> Select.SelectItemDef.create (text color) color color)
      |> View.Const

    let description =
      Helpers.bodyText
        "Select supports all three Field variants: Standard (default), Filled, and Outlined. Pass the variant parameter to control the input chrome styling."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Select.Create(
              items,
              standardVal,
              variant = Field.Variant.Standard,
              labelText = View.Const "Standard",
              attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )

          GridItem.Create(
            Select.Create(
              items,
              filledVal,
              variant = Field.Variant.Filled,
              labelText = View.Const "Filled",
              attrs = [ Select.Color.toClass BrandColor.Secondary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )

          GridItem.Create(
            Select.Create(
              items,
              outlinedVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Outlined",
              attrs = [ Select.Color.toClass BrandColor.Tertiary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ]
      )

    let code =
      """open Weave


Select.Create(
    items, selected,
    variant = Field.Variant.Standard, // see here
    labelText = View.Const "Standard"
)

Select.Create(
    items, selected,
    variant = Field.Variant.Filled, // see here
    labelText = View.Const "Filled"
)

Select.Create(
    items, selected,
    variant = Field.Variant.Outlined, // see here
    labelText = View.Const "Outlined"
)
"""

    Helpers.codeSampleSection "Variants" description content code

  let private clearableExample () =
    let selected = Var.Create<string option>(Some "Tokyo")

    let items =
      [ "Tokyo"; "London"; "New York"; "Paris"; "Sydney"; "Berlin" ]
      |> List.map (fun city -> Select.SelectItemDef.create (text city) city city)
      |> View.Const

    let description =
      Helpers.bodyText
        "When clearable is enabled, a clear button appears when a value is selected. Click the X to reset the selection to None."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Select.Create(
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "City",
              clearable = View.Const true,
              attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )

          GridItem.Create(
            selected.View
            |> Doc.BindView(fun v ->
              Body2.Div(
                sprintf "Selected: %s" (v |> Option.defaultValue "Nothing"),
                attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls ]
              )),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            attrs = [ cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ] ]
          )
        ]
      )

    let code =
      """open Weave


let selected = Var.Create<string option> (Some "Tokyo")

Select.Create(
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "City",
    clearable = View.Const true // see here
)
"""

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
      countries
      |> List.map (fun c -> Select.SelectItemDef.create (text c) c c)
      |> View.Const

    let description =
      Helpers.bodyText
        "Enable search to filter the dropdown items by typing. The search input auto-focuses when the dropdown opens. Filtering is case-insensitive on the item's Text field."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Select.Create(
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Country",
              searchable = true,
              clearable = View.Const true,
              attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ]
      )

    let code =
      """open Weave


Select.Create(
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Country",
    searchable = true, // see here
    clearable = View.Const true
)
"""

    Helpers.codeSampleSection "Searchable" description content code

  let private multiSelectExample () =
    let selected = Var.Create<Set<string>> Set.empty

    let items =
      [ "Reading"; "Gaming"; "Cooking"; "Hiking"; "Photography"; "Music"; "Travel" ]
      |> List.map (fun hobby -> Select.SelectItemDef.create (text hobby) hobby hobby)
      |> View.Const

    let description =
      Helpers.bodyText
        "Multi-select mode allows selecting multiple values. Items show checkboxes and the dropdown stays open on selection. The display shows a comma-separated summary of selected items."

    let content =
      div [] [
        Grid.Create(
          [
            GridItem.Create(
              Select.CreateMulti(
                items,
                selected,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Hobbies",
                placeholder = View.Const "Select hobbies",
                clearable = View.Const true,
                attrs = [ Select.Color.toClass BrandColor.Secondary |> cl ]
              ),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6
            )

            GridItem.Create(
              selected.View
              |> Doc.BindView(fun sel ->
                let display =
                  if Set.isEmpty sel then
                    "Nothing"
                  else
                    sel |> Set.toList |> String.concat ", "

                Body2.Div(
                  sprintf "Selected: %s" display,
                  attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls ]
                )),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              attrs = [ cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ] ]
            )
          ]
        )
      ]

    let code =
      """open Weave


let selected = Var.Create<Set<string>> Set.empty

let items =
    [ "Reading"; "Gaming"; "Cooking"; "Hiking"; "Photography" ]
    |> List.map (fun hobby ->
        Select.SelectItemDef.create (text hobby) hobby hobby)
    |> View.Const

Select.CreateMulti( // see here
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Hobbies",
    placeholder = View.Const "Select hobbies",
    clearable = View.Const true
)
"""

    Helpers.codeSampleSection "Multi-Select" description content code

  let private selectAllExample () =
    let selected = Var.Create<Set<string>> Set.empty

    let items =
      [ "Admin"; "Editor"; "Viewer"; "Moderator"; "Contributor" ]
      |> List.map (fun role -> Select.SelectItemDef.create (text role) role role)
      |> View.Const

    let description =
      Helpers.bodyText
        "Multi-select with a Select All row. When all items are selected the checkbox shows a checkmark; when some are selected it shows an indeterminate dash. Custom selection text can summarize the selection count."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Select.CreateMulti(
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Roles",
              showSelectAll = true,
              selectAllText = "All Roles",
              searchable = true,
              clearable = View.Const true,
              selectionText = (fun sel -> sprintf "%d role(s) selected" (Set.count sel)),
              attrs = [ Select.Color.toClass BrandColor.Tertiary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ]
      )

    let code =
      """open Weave


Select.CreateMulti(
    items,
    selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Roles",
    showSelectAll = true, // see here
    selectAllText = "All Roles", // see here
    searchable = true,
    clearable = View.Const true,
    selectionText = (fun sel -> // see here
        sprintf "%d role(s) selected" (Set.count sel))
)
"""

    Helpers.codeSampleSection "Select All" description content code

  let private customRenderingExample () =
    let selected = Var.Create<string option> None

    let makeItem emoji name =
      Select.SelectItemDef.create
        (span [
          cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ]
          Attr.Style "gap" "8px"
        ] [ span [ Attr.Style "font-size" "1.4em" ] [ text emoji ]; text name ])
        name
        name
      |> Select.SelectItemDef.withSelectedContent (
        span [
          cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ]
          Attr.Style "gap" "6px"
        ] [ span [ Attr.Style "font-size" "1.2em" ] [ text emoji ]; text name ]
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
        "Items support custom rendering via the Content field. Use withSelectedContent to provide a different display when the item is selected in the field (e.g., a compact version with a flag)."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Select.Create(
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Country",
              searchable = true,
              clearable = View.Const true,
              attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ]
      )

    let code =
      """open Weave


let makeItem emoji name =
    Select.SelectItemDef.create
        (span [ Attr.Style "gap" "8px" ] [
            span [ Attr.Style "font-size" "1.4em" ] [ text emoji ]
            text name
        ])
        name
        name
    |> Select.SelectItemDef.withSelectedContent ( // see here
        span [ Attr.Style "gap" "6px" ] [
            span [ Attr.Style "font-size" "1.2em" ] [ text emoji ]
            text name
        ])

let items =
    [ makeItem "\U0001F1FA\U0001F1F8" "United States"
      makeItem "\U0001F1EC\U0001F1E7" "United Kingdom"
      makeItem "\U0001F1EB\U0001F1F7" "France" ]
    |> View.Const

Select.Create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Country",
    searchable = true,
    clearable = View.Const true
)
"""

    Helpers.codeSampleSection "Custom Item Rendering" description content code

  let private disabledReadonlyExample () =
    let disabledVal = Var.Create<string option>(Some "Locked")
    let readonlyVal = Var.Create<string option>(Some "Read Only")

    let items =
      [ "Locked"; "Open"; "Pending" ]
      |> List.map (fun s -> Select.SelectItemDef.create (text s) s s)
      |> View.Const

    let description =
      Helpers.bodyText
        "Select can be disabled or set to read-only. Disabled prevents all interaction and dims the component. Read-only shows the value but prevents changes."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Select.Create(
              items,
              disabledVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Disabled",
              enabled = View.Const false,
              attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )

          GridItem.Create(
            Select.Create(
              items,
              readonlyVal,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Read Only",
              readOnly = View.Const true,
              attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )
        ]
      )

    let code =
      """open Weave


// Disabled
Select.Create(
    items, selected,
    labelText = View.Const "Disabled",
    enabled = View.Const false // see here
)

// Read Only
Select.Create(
    items, selected,
    labelText = View.Const "Read Only",
    readOnly = View.Const true // see here
)
"""

    Helpers.codeSampleSection "Disabled and Read Only" description content code

  let private colorsExample () =
    let colors = [
      "Primary", BrandColor.Primary
      "Secondary", BrandColor.Secondary
      "Tertiary", BrandColor.Tertiary
      "Error", BrandColor.Error
      "Warning", BrandColor.Warning
      "Success", BrandColor.Success
      "Info", BrandColor.Info
    ]

    let description =
      Helpers.bodyText "Select supports all brand colors, passed via attrs using Select.Color.toClass."

    let content =
      let items =
        [ "Option A"; "Option B"; "Option C" ]
        |> List.map (fun o -> Select.SelectItemDef.create (text o) o o)
        |> View.Const

      Grid.Create(
        colors
        |> List.map (fun (label, color) ->
          let v = Var.Create<string option>(Some "Option A")

          GridItem.Create(
            Select.Create(
              items,
              v,
              variant = Field.Variant.Outlined,
              labelText = View.Const label,
              attrs = [ Select.Color.toClass color |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4
          ))
      )

    let code =
      """open Weave


Select.Create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Primary",
    attrs = [ Select.Color.toClass BrandColor.Primary |> cl ] // see here
)

Select.Create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Error",
    attrs = [ Select.Color.toClass BrandColor.Error |> cl ] // see here
)
"""

    Helpers.codeSampleSection "Colors" description content code

  let private widthExample () =
    let autoVal = Var.Create<string option> None
    let fullVal = Var.Create<string option> None
    let fitVal = Var.Create<string option> None

    let items =
      [ "Short"; "Medium Text"; "A Longer Option Here" ]
      |> List.map (fun o -> Select.SelectItemDef.create (text o) o o)
      |> View.Const

    let description =
      Helpers.bodyText
        "Select supports three width modes: Auto (default, inline sizing), Full (100% of container), and Fit Content (sizes to content). Pass width classes via attrs using Select.Width.toClass."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [] [
              Body2.Div("Auto (default)", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

              Select.Create(
                items,
                autoVal,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Auto",
                attrs = [ Select.Color.toClass BrandColor.Primary |> cl ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )

          GridItem.Create(
            div [ Attr.Style "width" "100%" ] [
              Body2.Div("Full Width", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

              Select.Create(
                items,
                fullVal,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Full",
                attrs = [
                  Select.Color.toClass BrandColor.Secondary |> cl
                  yield! (Select.Width.toClass Select.Width.Full |> Option.map cl |> Option.toList)
                ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )

          GridItem.Create(
            div [] [
              Body2.Div("Fit Content", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

              Select.Create(
                items,
                fitVal,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Fit Content",
                attrs = [
                  Select.Color.toClass BrandColor.Tertiary |> cl
                  yield! (Select.Width.toClass Select.Width.FitContent |> Option.map cl |> Option.toList)
                ]
              )
            ],
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ]
      )

    let code =
      """open Weave


// Auto (default) — inline, sizes to min-width
Select.Create(items, selected, ...)

// Full Width — 100% of container
Select.Create(
    items, selected,
    attrs = [ Css.``weave-select--full-width`` |> cl ] // see here
)

// Fit Content — sizes to content
Select.Create(
    items, selected,
    attrs = [ Css.``weave-select--fit-content`` |> cl ] // see here
)
"""

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
        Select.SelectItemDef.create
          (span [] [ text (sprintf "%s (%s)" lang.Name lang.Native) ])
          lang.Name
          lang)
      |> View.Const

    let description =
      Helpers.bodyText
        "Select is generic over the value type. Here we use a custom record type as the item value. The Text field is used for search filtering while Value holds the full domain object."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Select.Create(
              items,
              selected,
              variant = Field.Variant.Outlined,
              labelText = View.Const "Language",
              searchable = true,
              clearable = View.Const true,
              attrs = [ Select.Color.toClass BrandColor.Info |> cl ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6
          )

          GridItem.Create(
            selected.View
            |> Doc.BindView(fun v ->
              match v with
              | Some lang ->
                Body2.Div(
                  sprintf "Code: %s | Name: %s | Native: %s" lang.Code lang.Name lang.Native,
                  attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls ]
                )
              | None ->
                Body2.Div("No language selected", attrs = [ Margin.toClasses Margin.Top.extraSmall |> cls ])),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            attrs = [ cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ] ]
          )
        ]
      )

    let code =
      """open Weave


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
        Select.SelectItemDef.create
            (text (sprintf "%s (%s)" lang.Name lang.Native))
            lang.Name  // Text for search filtering
            lang)      // Value is the full record // see here
    |> View.Const

Select.Create(
    items, selected,
    variant = Field.Variant.Outlined,
    labelText = View.Const "Language",
    searchable = true,
    clearable = View.Const true
)
"""

    Helpers.codeSampleSection "Generic Types" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Select"
        Body1.Div(
          "Select components allow users to choose from a list of options in a dropdown. Supports single and multi-select, search filtering, clear, select-all, custom item rendering, and all Field variants.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

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
      ],
      maxWidth = Container.MaxWidth.Large
    )
