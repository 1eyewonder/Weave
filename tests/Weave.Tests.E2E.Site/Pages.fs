namespace Weave.Tests.E2E.Site

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module Pages =

  let private animationPage () =
    let isVisible = Var.Create false
    let isActive = Var.Create false

    div [] [
      div [ Attr.Create "data-testid" "fade-in-static"; AnimationEntrance.fadeIn ] [ text "Fade in" ]
      div [ Attr.Create "data-testid" "bounce-static"; AnimationEmphasis.bounce ] [ text "Bounce" ]
      div [
        Attr.Create "data-testid" "pulse-hover"
        AnimationEmphasis.pulse
        AnimationOn.hover
      ] [ text "Pulse on hover" ]
      div [ Attr.Create "data-testid" "suppress-container"; AnimationKind.suppress ] [
        div [ Attr.Create "data-testid" "suppress-child"; AnimationEntrance.fadeIn ] [
          text "Suppressed child"
        ]
      ]
      Button.create (
        text "Toggle Show",
        (fun () -> isVisible.Value <- not isVisible.Value),
        attrs = [ Attr.Create "data-testid" "show-toggle-btn" ]
      )
      Animate.showWith
        AnimationPair.fadeInOut
        isVisible.View
        (fun () -> div [ Attr.Create "data-testid" "show-content" ] [ text "Animated show content" ])
        [ Attr.Create "data-testid" "show-wrapper" ]
        None
      Button.create (
        text "Toggle Class",
        (fun () -> isActive.Value <- not isActive.Value),
        attrs = [ Attr.Create "data-testid" "toggle-btn" ]
      )
      div [
        Attr.Create "data-testid" "toggle-target"
        Animate.toggleClass AnimationPair.fadeInOut isActive.View
      ] [ text "Toggle target" ]
      div [] [
        div [
          Attr.Create "data-testid" "stagger-1"
          AnimationEntrance.fadeIn
          AnimationDelay.stagger 1
        ] [ text "Stagger 1" ]
        div [
          Attr.Create "data-testid" "stagger-2"
          AnimationEntrance.fadeIn
          AnimationDelay.stagger 2
        ] [ text "Stagger 2" ]
        div [
          Attr.Create "data-testid" "stagger-3"
          AnimationEntrance.fadeIn
          AnimationDelay.stagger 3
        ] [ text "Stagger 3" ]
      ]
    ]

  let private checkboxPage () =
    div [] [
      Checkbox.create (Var.Create false, View.Const "Default Checkbox")
      Checkbox.create (Var.Create true, View.Const "Checked Checkbox")
      Checkbox.create (Var.Create false, View.Const "Disabled Checkbox", enabled = View.Const false)
      Checkbox.create (Var.Create false, View.Const "Small", attrs = [ Checkbox.Size.small ])
      Checkbox.create (Var.Create false, View.Const "Large", attrs = [ Checkbox.Size.large ])
    ]

  let private radioPage () =
    let selected = Var.Create "A"

    div [] [
      Radio.create (
        selected,
        "A",
        displayText = View.Const "Option A",
        inputAttrs = [ Attr.Create "name" "test-options" ]
      )
      Radio.create (
        selected,
        "B",
        displayText = View.Const "Option B",
        inputAttrs = [ Attr.Create "name" "test-options" ]
      )
      Radio.create (
        selected,
        "C",
        displayText = View.Const "Disabled Option",
        enabled = View.Const false,
        inputAttrs = [ Attr.Create "name" "test-options" ]
      )
    ]

  let private radioFormGroupsPage () =
    let first = Var.Create "A"
    let second = Var.Create "A"

    div [] [
      form [ Attr.Create "data-testid" "first-radio-form" ] [
        Radio.create (
          first,
          "A",
          displayText = View.Const "First A",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
        Radio.create (
          first,
          "B",
          displayText = View.Const "First B",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
      ]
      form [ Attr.Create "data-testid" "second-radio-form" ] [
        Radio.create (
          second,
          "A",
          displayText = View.Const "Second A",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
        Radio.create (
          second,
          "B",
          displayText = View.Const "Second B",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
      ]
    ]

  let private switchPage () =
    div [] [
      Switch.create (Var.Create false, content = text "Off Switch")
      Switch.create (Var.Create true, content = text "On Switch")
      Switch.create (Var.Create false, content = text "Disabled Switch", enabled = View.Const false)
    ]

  let private fieldPage () =
    div [] [
      TextField.singleLine (Var.Create "", labelText = View.Const "Standard Field")
      TextField.singleLine (
        Var.Create "some value",
        labelText = View.Const "Outlined Field",
        attrs = [ TextField.Variant.outlined ]
      )
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "Disabled Field",
        enabled = View.Const false
      )
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "With Help",
        showHelpText = View.Const true,
        helpText = text "Help text content"
      )
      TextField.singleLine (
        Var.Create "not-an-email",
        labelText = View.Const "Email",
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Invalid email address",
            attrs = [ Attr.Create "id" "email-error"; FieldHelpText.Color.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "email-error"
        ],
        attrs = [ TextField.Color.error ]
      )
    ]

  let private numericFieldPage () =
    div [] [
      div [ Attr.Create "data-testid" "numeric-int" ] [
        NumericField.create (Var.Create 5, labelText = View.Const "Integer spin")
      ]
      div [ Attr.Create "data-testid" "numeric-float" ] [
        NumericField.create (Var.Create 5.0, labelText = View.Const "Float spin")
      ]
      NumericField.create (
        Var.Create 5,
        labelText = View.Const "Standard Numeric",
        showSpinButtons = View.Const true
      )
      NumericField.create (
        Var.Create 10,
        labelText = View.Const "Outlined Numeric",
        showSpinButtons = View.Const true,
        attrs = [ NumericField.Variant.outlined ]
      )
      NumericField.create (
        Var.Create 150,
        labelText = View.Const "Max 100",
        max = 100,
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Value exceeds maximum",
            attrs = [ Attr.Create "id" "numeric-error"; FieldHelpText.Color.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "numeric-error"
        ],
        attrs = [ NumericField.Color.error ]
      )
    ]

  let private textFieldPage () =
    div [] [
      TextField.singleLine (Var.Create "", labelText = View.Const "Standard Text Field")
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "Outlined Text Field",
        attrs = [ TextField.Variant.outlined ]
      )
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "Disabled Text Field",
        enabled = View.Const false
      )
      TextField.singleLine (
        Var.Create "not-an-email",
        labelText = View.Const "Email",
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Invalid email address",
            attrs = [ Attr.Create "id" "text-email-error"; FieldHelpText.Color.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "text-email-error"
        ],
        attrs = [ TextField.Color.error ]
      )
      TextField.singleLine (Var.Create "", labelText = View.Const "Max Length", maxLength = 50)
      TextField.multiLine (Var.Create "", labelText = View.Const "Multiline Text Field")
    ]

  let private selectPage () =
    let dynamicDisabled = Var.Create false
    let selectEnabled = Var.Create true
    let selectReadOnly = Var.Create false

    let keyboardItems =
      [
        SelectItem.create (text "Blocked", "Blocked", "Blocked", disabled = View.Const true)
        SelectItem.create (text "Alpha", "Alpha", "Alpha")
        SelectItem.create (text "Dynamic", "Dynamic", "Dynamic", disabled = dynamicDisabled.View)
        SelectItem.create (text "Omega", "Omega", "Omega")
      ]
      |> Var.Create

    let items =
      [ "Apple"; "Banana"; "Cherry" ]
      |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
      |> View.Const

    let multiItems =
      [ "Red"; "Green"; "Blue" ]
      |> List.map (fun c -> SelectItem.create (text c, c, c))
      |> View.Const

    div [] [
      Select.create (
        items,
        Var.Create<string option> None,
        labelText = View.Const "Single Select",
        placeholder = View.Const "Choose a fruit"
      )
      Select.create (
        items,
        Var.Create<string option>(Some "Apple"),
        labelText = View.Const "With Value",
        clearable = View.Const true
      )
      Select.create (
        items,
        Var.Create<string option> None,
        labelText = View.Const "Disabled Select",
        enabled = View.Const false
      )
      Select.create (
        items,
        Var.Create<string option> None,
        labelText = View.Const "Searchable",
        searchable = true
      )
      MultiSelect.create (
        multiItems,
        Var.Create(set [ "Red"; "Blue" ]),
        labelText = View.Const "Multi Select"
      )
      div [ Attr.Create "data-testid" "keyboard-single" ] [
        Select.create (
          keyboardItems.View,
          Var.Create<string option> None,
          searchable = true,
          clearable = View.Const true,
          labelText = View.Const "Keyboard single"
        )
      ]
      div [ Attr.Create "data-testid" "keyboard-multi" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          searchable = true,
          clearable = View.Const true,
          labelText = View.Const "Keyboard multi"
        )
      ]
      div [ Attr.Create "data-testid" "restricted-single" ] [
        Select.create (
          keyboardItems.View,
          Var.Create<string option> None,
          searchable = true,
          clearable = View.Const true,
          enabled = selectEnabled.View,
          readOnly = selectReadOnly.View,
          labelText = View.Const "Restricted single"
        )
      ]
      div [ Attr.Create "data-testid" "restricted-multi" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          searchable = true,
          clearable = View.Const true,
          enabled = selectEnabled.View,
          readOnly = selectReadOnly.View,
          labelText = View.Const "Restricted multi"
        )
      ]
      div [ Attr.Create "data-testid" "select-all-multi" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          searchable = true,
          showSelectAll = true,
          labelText = View.Const "Select all options"
        )
      ]
      div [ Attr.Create "data-testid" "select-all-no-search" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          showSelectAll = true,
          labelText = View.Const "Select all without search"
        )
      ]
      Button.create (
        text "Toggle enabled",
        (fun () -> Var.Set selectEnabled (not selectEnabled.Value)),
        attrs = [ Attr.Create "data-testid" "toggle-select-enabled" ]
      )
      Button.create (
        text "Toggle read only",
        (fun () -> Var.Set selectReadOnly (not selectReadOnly.Value)),
        attrs = [ Attr.Create "data-testid" "toggle-select-readonly" ]
      )
      Button.create (
        text "Disable dynamic option",
        (fun () -> Var.Set dynamicDisabled true),
        attrs = [ Attr.Create "data-testid" "disable-option" ]
      )
      Button.create (
        text "Reorder options",
        (fun () -> Var.Set keyboardItems (List.rev keyboardItems.Value)),
        attrs = [ Attr.Create "data-testid" "reorder-options" ]
      )
      Button.create (
        text "Disable all options",
        (fun () ->
          keyboardItems.Value
          |> List.map (fun item -> { item with Disabled = View.Const true })
          |> Var.Set keyboardItems),
        attrs = [ Attr.Create "data-testid" "disable-all-options" ]
      )
    ]

  let private buttonPage () =
    let activations = Var.Create 0

    div [] [
      Button.create (text "Filled Button", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.create (text "Outlined Button", (fun () -> ()), attrs = [ Button.Variant.outlined ])
      Button.create (
        text "Disabled Button",
        (fun () -> ()),
        enabled = View.Const false,
        attrs = [ Button.Variant.filled ]
      )
      IconButton.primary (
        Icon.create (Icon.UiActions UiActions.Home),
        (fun () -> ()),
        attrs = [ Attr.Create "aria-label" "home"; Button.Variant.filled ]
      )
      div [ Attr.Create "data-testid" "activation-count" ] [
        Doc.TextView(activations.View |> View.Map string)
      ]
      Button.create (
        text "Count activation",
        (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [ Attr.Create "data-testid" "activation-button" ]
      )
      IconButton.create (
        text "Icon",
        (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [
          Attr.Create "data-testid" "activation-icon"
          Attr.Create "aria-label" "Count icon"
        ]
      )
      Button.create (
        text "Disabled count",
        (fun () -> Var.Set activations (activations.Value + 1)),
        enabled = View.Const false,
        attrs = [ Attr.Create "data-testid" "activation-disabled" ]
      )
      IconButton.create (
        text "Disabled icon",
        (fun () -> Var.Set activations (activations.Value + 1)),
        enabled = View.Const false,
        attrs = [
          Attr.Create "data-testid" "activation-disabled-icon"
          Attr.Create "aria-label" "Disabled count icon"
        ]
      )
      Button.primary (text "Primary", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.error (text "Error", (fun () -> ()), attrs = [ Button.Variant.filled ])
    ]

  let private dialogPage () =
    let isOpen = Var.Create true

    isOpen.View
    |> Doc.BindView(fun visible ->
      if visible then
        Dialog.create (
          DialogTitle.create (text "Dialog Title"),
          DialogContent.create (
            div [] [
              div [ Typography.body1 ] [ text "Dialog content goes here." ]
              div [] [
                Button.create (text "Action", (fun () -> ()), attrs = [ Button.Variant.filled ])
              ]
            ]
          ),
          dialogInteraction = View.Const(Dialog.Interaction.Optional(fun () -> isOpen.Value <- false))
        )
      else
        Doc.Empty)

  let private dialogForcePage () =
    Dialog.create (
      DialogTitle.create (text "Force Dialog Title"),
      DialogContent.create (
        div [] [
          div [ Typography.body1 ] [ text "You must complete this action." ]
          div [] [
            Button.create (text "Confirm", (fun () -> ()), attrs = [ Button.Variant.filled ])
          ]
        ]
      ),
      dialogInteraction = View.Const Dialog.Interaction.Force
    )

  let private dialogTriggeredPage () =
    let isOpen = Var.Create false

    div [] [
      Button.create (
        text "Open Dialog",
        (fun () -> isOpen.Value <- true),
        attrs = [ Attr.Create "id" "open-dialog-btn"; Button.Variant.filled ]
      )
      isOpen.View
      |> Doc.BindView(fun visible ->
        if visible then
          Dialog.create (
            DialogTitle.create (text "Triggered Dialog"),
            DialogContent.create (
              div [] [
                div [ Typography.body1 ] [ text "Dialog opened by button." ]
                div [] [
                  Button.create (text "Action", (fun () -> ()), attrs = [ Button.Variant.filled ])
                ]
              ]
            ),
            dialogInteraction = View.Const(Dialog.Interaction.Optional(fun () -> isOpen.Value <- false))
          )
        else
          Doc.Empty)
    ]

  let private tabsPage () =
    let tabs =
      [
        TabItem.createCustom (text "Tab 1", div [] [ text "Panel 1 content" ])
        TabItem.createCustom (text "Tab 2", div [] [ text "Panel 2 content" ])
        TabItem.createCustom (
          text "Disabled Tab",
          div [] [ text "Disabled panel" ],
          disabled = View.Const true
        )
      ]
      |> View.Const

    Tabs.create (tabs)

  let private dropdownPage () =
    let selectedAction = Var.Create "none"

    div [] [
      div [ Attr.Create "data-testid" "dropdown-action" ] [ Doc.TextView selectedAction.View ]
      DropdownMenu.create (
        text "Open Menu",
        View.Const [
          DropdownMenuItem.create (text "Item 1", (fun () -> Var.Set selectedAction "item 1"))
          DropdownMenuItem.create (text "Item 2", (fun () -> ()))
          DropdownMenuItem.create (text "Disabled Item", (fun () -> ()), disabled = View.Const true)
        ]
      )
      Button.create (text "After", (fun () -> ()), attrs = [ Attr.Create "data-testid" "focus-target" ])
    ]

  let private expansionPanelPage () =
    let expanded1 = Var.Create true
    let expanded2 = Var.Create false
    let expanded3 = Var.Create false

    ExpansionPanelContainer.create (
      [
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Expanded Panel",
            expanded1,
            icon = ExpansionPanelHeader.defaultIcon expanded1
          ),
          ExpansionPanelContent.create (div [ Typography.body1 ] [ text "Expanded content" ]),
          expanded = expanded1
        )
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Collapsed Panel",
            expanded2,
            icon = ExpansionPanelHeader.defaultIcon expanded2
          ),
          ExpansionPanelContent.create (div [ Typography.body1 ] [ text "Collapsed content" ]),
          expanded = expanded2
        )
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Focus Color Panel",
            expanded3,
            icon = ExpansionPanelHeader.defaultIcon expanded3,
            attrs = [ ExpansionPanel.FocusColor.error ]
          ),
          ExpansionPanelContent.create (div [ Typography.body1 ] [ text "Focus color content" ]),
          expanded = expanded3
        )
      ]
    )

  let private alertPage () =
    div [] [
      Alert.create (text "Success alert message", attrs = [ Alert.Color.success ])
      Alert.create (text "Error alert message", attrs = [ Alert.Color.error ])
      Alert.create (text "Warning alert message", attrs = [ Alert.Color.warning ])
      Alert.create (text "Info alert message", attrs = [ Alert.Color.info ])
    ]

  let private cardPage () =
    let opened = Var.Create "none"

    div [] [
      Card.create (
        Doc.Concat [
          CardHeader.create (
            h2 [] [ text "Bordered card" ],
            subtitle = text "Card subtitle",
            leading = Icon.create (Icon.UiActions UiActions.Home),
            actions =
              IconButton.create (
                Icon.create (Icon.UiActions UiActions.Home),
                (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "home" ]
              )
          )
          CardContent.create (div [ Typography.body1 ] [ text "Card body content" ])
          CardActions.create (Button.create (text "Action", (fun () -> ()), attrs = [ Button.Variant.text ]))
        ],
        attrs = [ Attr.Create "data-testid" "bordered-card" ]
      )
      Card.create (
        Doc.Concat [
          CardHeader.create (text "Elevated card")
          CardContent.create (div [ Typography.body1 ] [ text "Elevated content" ])
        ],
        attrs = [ Card.Variant.elevated ]
      )
      Card.create (
        CardContent.create (div [ Typography.body1 ] [ text "Filled content" ]),
        attrs = [ Card.Variant.filled; Attr.Create "data-testid" "filled-card" ]
      )
      Card.create (
        Doc.Concat [
          CardActionArea.create (
            Doc.Concat [
              CardMedia.create (div [ Attr.Style "height" "80px" ] [])
              CardHeader.create (h2 [] [ text "Clickable card" ])
              CardContent.create (div [ Typography.body1 ] [ text "Opens the card" ])
            ],
            onClick = (fun () -> Var.Set opened "clickable")
          )
          CardActions.create (
            Button.create (
              text "Secondary",
              (fun () -> Var.Set opened "secondary"),
              attrs = [ Button.Variant.text ]
            )
          )
        ],
        attrs = [ Attr.Create "data-testid" "action-card" ]
      )
      Card.create (
        CardActionArea.create (CardContent.create (text "Linked card"), href = "#card"),
        attrs = [ Attr.Create "data-testid" "link-card" ]
      )
      div [ Attr.Create "data-testid" "opened-card" ] [ textView opened.View ]
    ]

  let private tablePage () =
    let clicked = Var.Create "none"
    let selected = Var.Create(None: string option)
    let checkedRows = Var.Create(Set.ofList [ "Alpha" ])

    let externalSort =
      Var.Create(
        Some {
          Column = "count"
          Direction = Table.SortDirection.Descending
        }
        : Table.SortState option
      )

    let rows = [ "Beta", 2; "Alpha", 3; "Gamma", 1 ]

    let columns () = [
      TableColumn.create (
        "name",
        text "Name",
        (fun (name: string, _: int) -> text name),
        sort = Table.ColumnSort.by fst
      )
      TableColumn.create ("count", text "Count", (fun (_: string, count: int) -> text (string<int> count)))
    ]

    let externalColumns = [
      TableColumn.create ("name", text "Name", (fun (name: string, _: int) -> text name))
      TableColumn.create (
        "count",
        text "Count",
        (fun (_: string, count: int) -> text (string<int> count)),
        sort = Table.ColumnSort.External
      )
    ]

    let externallySorted =
      externalSort.View
      |> View.Map(fun state ->
        match state with
        | Some { Direction = Table.SortDirection.Ascending } -> List.sortBy snd rows
        | Some { Direction = Table.SortDirection.Descending } -> List.sortByDescending snd rows
        | None -> rows)

    let describeSort (state: Table.SortState option) =
      match state with
      | Some {
               Column = column
               Direction = Table.SortDirection.Ascending
             } -> column + " ascending"
      | Some {
               Column = column
               Direction = Table.SortDirection.Descending
             } -> column + " descending"
      | None -> "unsorted"

    div [] [
      Table.create (
        columns (),
        View.Const rows,
        fst,
        onRowClick = (fun (name, _) -> Var.Set clicked name),
        selectedKey = selected,
        caption = text "Clickable rows",
        attrs = [
          Attr.Create "data-testid" "clickable-table"
          Table.striped
          Table.StripeColor.custom "rgb(255, 0, 0)"
          Table.HoverColor.custom "rgb(0, 0, 255)"
        ]
      )
      div [ Attr.Create "data-testid" "clicked-row" ] [ textView clicked.View ]
      Table.create (
        columns (),
        View.Const [],
        fst,
        emptyContent = text "No rows",
        caption = text "Empty rows",
        attrs = [
          Attr.Create "data-testid" "empty-table"
          Table.Density.compact
          Table.hiddenCaption
        ]
      )
      Table.create (
        columns (),
        View.Const rows,
        fst,
        caption = text "Sticky rows",
        attrs = [
          Attr.Create "data-testid" "sticky-table"
          Table.stickyHeader
          Table.Density.spacious
          Table.hiddenCaption
          Attr.Style "max-height" "120px"
        ]
      )
      Table.create (
        columns (),
        View.Const rows,
        fst,
        selectedKeys = checkedRows,
        rowHeader = "name",
        caption = text "Selectable rows",
        attrs = [ Attr.Create "data-testid" "select-table"; Table.hiddenCaption ]
      )
      Table.create (
        columns (),
        View.Const rows,
        fst,
        selectedKey = Var.Create(None: string option),
        selectedKeys = Var.Create(Set.empty: Set<string>),
        caption = text "Current and checked rows",
        attrs = [ Attr.Create "data-testid" "current-and-checked-table"; Table.hiddenCaption ]
      )
      div [ Attr.Create "data-testid" "checked-rows" ] [
        textView (checkedRows.View |> View.Map(Set.toList >> String.concat ","))
      ]
      Table.create (
        externalColumns,
        externallySorted,
        fst,
        sort = externalSort,
        caption = text "Externally sorted rows",
        attrs = [ Attr.Create "data-testid" "external-table"; Table.hiddenCaption ]
      )
      div [ Attr.Create "data-testid" "external-sort" ] [
        textView (externalSort.View |> View.Map describeSort)
      ]
    ]

  let private themePage useSystemPreference =
    let initialMode =
      if useSystemPreference then
        Theming.detectSystemPreference ()
      else
        Theming.Light

    Theming.setMode initialMode

    let recordMode mode =
      let value = if mode = Theming.Dark then "dark" else "light"
      JS.Document.Body.SetAttribute("data-observed-mode", value)

    div [] [
      Button.create (
        text "Primary",
        ignore,
        attrs = [
          Attr.Create "data-testid" "theme-swatch"
          Button.Variant.filled
          Button.Color.primary
        ]
      )
      div [ Attr.Class "weave-card"; Attr.Create "data-testid" "theme-card" ] [ text "Card" ]
      div [ Typography.body1; Attr.Create "data-testid" "theme-body" ] [ text "Body" ]
      Button.create (
        text "Toggle",
        (fun () -> Theming.toggleMode () |> recordMode),
        attrs = [ Attr.Create "data-testid" "theme-toggle" ]
      )
      Button.create (
        text "Light",
        (fun () ->
          Theming.setMode Theming.Light
          Theming.getMode () |> recordMode),
        attrs = [ Attr.Create "data-testid" "theme-light" ]
      )
      Button.create (
        text "Dark",
        (fun () ->
          Theming.setMode Theming.Dark
          Theming.getMode () |> recordMode),
        attrs = [ Attr.Create "data-testid" "theme-dark" ]
      )
    ]

  let private plainDialogPage () =
    Dialog.create (text "Plain dialog title", text "Dialog content")

  let private appbarPage () =
    div [] [
      AppBar.create (div [] [ text "App Bar Title" ], attrs = [ Attr.Create "aria-label" "Static app bar" ])
    ]

  let private listenerRemovalPage () =
    let mounted = Var.Create true
    let active = Var.Create true
    let calls = Var.Create 0
    let target = ref (JS.Document.CreateElement "div")

    div [] [
      Button.create (
        text "Toggle listener",
        (fun () -> Var.Set mounted (not mounted.Value)),
        attrs = [ Attr.Create "data-testid" "remove-listener" ]
      )
      Button.create (text "Outside", ignore, attrs = [ Attr.Create "data-testid" "listener-outside" ])
      div [ Attr.Create "data-testid" "listener-calls" ] [ textView (calls.View |> View.Map string) ]
      mounted.View
      |> Doc.BindView(fun visible ->
        if visible then
          div [
            Attr.Create "data-testid" "listener-owner"
            on.afterRender (fun el -> target.Value <- el)
          ] [
            DocumentEventListener.onMouseDown
              [ target ]
              (fun () -> Var.Set calls (calls.Value + 1))
              active.View
            text "Owner"
          ]
        else
          Doc.Empty)
    ]

  let private drawerPage variant initiallyOpen =
    let isOpen = Var.Create initiallyOpen

    div [] [
      Button.create (
        text "Toggle Drawer",
        (fun () -> Var.Set isOpen (not isOpen.Value)),
        attrs = [ Attr.Create "data-testid" "drawer-toggle"; Button.Variant.filled ]
      )

      DrawerContainer.create (
        mainContent =
          div [] [
            Button.create (
              text "Main Focus Target",
              (fun () -> ()),
              attrs = [ Attr.Create "data-testid" "main-focus-target" ]
            )
          ],
        leftDrawer =
          Drawer.create (
            div [] [
              Button.create (
                text "Drawer Button",
                (fun () -> ()),
                attrs = [ Attr.Create "data-testid" "drawer-button" ]
              )
            ],
            isOpen.View,
            variant = variant,
            overlayClose = (fun () -> Var.Set isOpen false),
            attrs = [ Attr.Create "aria-label" "Navigation drawer" ]
          )
      )
    ]

  let private linkPage () =
    let activations = Var.Create 0

    div [] [
      div [ Attr.Create "data-testid" "link-count" ] [ Doc.TextView(activations.View |> View.Map string) ]
      Link.create (
        text "Count link",
        href = "#link",
        onClick = (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [ Attr.Create "data-testid" "count-link" ]
      )
      IconLink.create (
        text "Icon",
        href = "#link",
        onClick = (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [
          Attr.Create "data-testid" "count-icon-link"
          Attr.Create "aria-label" "Count icon link"
        ]
      )
      Link.create (
        text "Disabled count link",
        href = "#other",
        enabled = View.Const false,
        onClick = (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [ Attr.Create "data-testid" "disabled-count-link" ]
      )
      Link.create (text "Default Link", href = "#")
      Link.create (text "Disabled Link", href = "#", enabled = View.Const false)
      Link.create (text "Always Underline", href = "#", attrs = [ Link.Underline.always ])
    ]

  let private listPage () =
    let selected = Var.Create<string option> None
    let nestedSelected = Var.Create(Set.empty<string>)
    let nestedExpanded = Var.Create false

    div [] [
      selected.View
      |> Doc.BindView(fun s ->
        div [ Attr.Create "data-testid" "list-selection" ] [
          text (
            match s with
            | Some v -> v
            | None -> "none"
          )
        ])

      WeaveList.create (
        [
          ListItem.create (text "Item 1", value = "1")
          ListItem.create (text "Item 2", value = "2")
          ListItem.create (text "Item 3", value = "3")
          ListItem.create (text "Disabled Item", value = "4", disabled = View.Const true)
        ],
        selectedValue = selected,
        attrs = [ Attr.Create "aria-label" "Test items" ]
      )
      div [ Attr.Create "data-testid" "nested-selected" ] [
        Doc.TextView(
          nestedSelected.View
          |> View.Map(fun values -> values |> Set.toList |> String.concat ",")
        )
      ]
      div [ Attr.Create "data-testid" "nested-list" ] [
        WeaveList.create (
          [
            ListItem.create (
              text "Group",
              expanded = nestedExpanded,
              nestedChildren = [
                ListItem.create (text "A", value = "a")
                ListItem.create (text "B", value = "b")
              ]
            )
          ],
          selectedValues = nestedSelected,
          selectionMode = WeaveList.SelectionMode.MultiSelection
        )
      ]
    ]

  let private chipPage () =
    let clicked = Var.Create "none"
    let chipActions = Var.Create 0
    let chipCloses = Var.Create 0

    div [] [
      clicked.View
      |> Doc.BindView(fun v -> div [ Attr.Create "data-testid" "chip-clicked" ] [ text v ])

      Chip.create (
        text "Clickable Chip",
        onClick = (fun () -> Var.Set clicked "clicked"),
        attrs = [ Chip.Variant.filled; Attr.Create "data-testid" "clickable-chip" ]
      )
      Chip.create (text "Default Chip", attrs = [ Chip.Variant.filled ])
      Chip.create (
        text "Disabled Chip",
        onClick = (fun () -> ()),
        enabled = View.Const false,
        attrs = [ Chip.Variant.filled; Attr.Create "data-testid" "disabled-chip" ]
      )
      Chip.create (text "Deletable Chip", onClose = (fun () -> ()), attrs = [ Chip.Variant.outlined ])
      div [ Attr.Create "data-testid" "chip-actions" ] [ Doc.TextView(chipActions.View |> View.Map string) ]
      div [ Attr.Create "data-testid" "chip-closes" ] [ Doc.TextView(chipCloses.View |> View.Map string) ]
      Chip.create (
        text "Both",
        onClick = (fun () -> Var.Set chipActions (chipActions.Value + 1)),
        onClose = (fun () -> Var.Set chipCloses (chipCloses.Value + 1)),
        attrs = [ Attr.Create "data-testid" "chip-both" ]
      )
      Chip.create (
        text "Disabled close",
        onClose = (fun () -> Var.Set chipCloses (chipCloses.Value + 1)),
        enabled = View.Const false,
        attrs = [ Attr.Create "data-testid" "chip-disabled-close" ]
      )
      Chip.create (
        text "Chip link",
        href = "#chip",
        onClick = (fun () -> Var.Set chipActions (chipActions.Value + 1)),
        attrs = [ Attr.Create "data-testid" "chip-link" ]
      )
      Chip.create (
        text "Closable link",
        href = "#other",
        onClick = (fun () -> Var.Set chipActions (chipActions.Value + 1)),
        onClose = (fun () -> Var.Set chipCloses (chipCloses.Value + 1)),
        attrs = [ Attr.Create "data-testid" "closable-chip-link" ]
      )
    ]

  let private chipsetPage () =
    let selected = Var.Create<string option> None

    div [] [
      selected.View
      |> Doc.BindView(fun s ->
        div [ Attr.Create "data-testid" "chipset-selection" ] [
          text (
            match s with
            | Some v -> v
            | None -> "none"
          )
        ])

      ChipSet.create (
        [
          ChipItem.create (text "Chip A", "a", attrs = [ Chip.Variant.outlined ])
          ChipItem.create (text "Chip B", "b", attrs = [ Chip.Variant.outlined ])
          ChipItem.create (text "Chip C", "c", attrs = [ Chip.Variant.outlined ])
          ChipItem.create (
            text "Disabled Chip",
            "d",
            disabled = View.Const true,
            attrs = [ Chip.Variant.outlined ]
          )
        ],
        selectedValue = selected,
        selectionMode = ChipSet.SelectionMode.Toggle
      )
      div [ Attr.Create "data-testid" "closable-chipset" ] [
        ChipSet.create (
          [
            ChipItem.create (text "One", "one", closable = true)
            ChipItem.create (text "Disabled", "disabled", closable = true, disabled = View.Const true)
            ChipItem.create (text "Three", "three", closable = true)
          ],
          selectedValue = Var.Create None,
          onClose = (fun _ -> ())
        )
      ]
    ]

  let private buttonmenuPage () =
    let action = Var.Create "none"

    let menuItem label =
      Button.create (
        text label,
        (fun () -> action.Set label),
        attrs = [ Button.Variant.text; Button.Width.full; BorderRadius.All.none ]
      )

    div [] [
      ButtonMenu.create (text "Menu", [ menuItem "Action 1"; menuItem "Action 2" ])
      ButtonMenu.create (
        text "Horizontal Menu",
        [ menuItem "Left 1"; menuItem "Left 2" ],
        direction = ButtonMenu.Direction.Right,
        attrs = [ Attr.Create "data-testid" "horizontal-menu" ]
      )
      ButtonMenu.create (
        text "Edge cases",
        [
          Button.create (text "Unavailable", (fun () -> action.Set "disabled"), enabled = View.Const false)
          div [] [
            Link.create (
              text "Wrapped link",
              href = "#menu-link-target",
              onClick = (fun () -> action.Set "link")
            )
          ]
        ],
        attrs = [ Attr.Create "data-testid" "edge-menu" ]
      )
      div [ Attr.Create "data-testid" "menu-action" ] [ textView action.View ]
      Button.create (
        text "After",
        (fun () -> ()),
        attrs = [
          Attr.Create "data-testid" "focus-target"
          Attr.Create "id" "menu-link-target"
        ]
      )
    ]

  let private buttongroupPage () =
    ButtonGroup.create (
      [
        Button.create (text "Left", (fun () -> ()), attrs = [ Button.Variant.outlined ])
        Button.create (text "Center", (fun () -> ()), attrs = [ Button.Variant.outlined ])
        Button.create (text "Right", (fun () -> ()), attrs = [ Button.Variant.outlined ])
      ]
    )

  let private tooltipPage () =
    div [] [
      Tooltip.create (
        Button.create (text "Hover me", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Tooltip text",
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-default" ]
      )
      Tooltip.create (
        Button.create (text "Click me", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Click tooltip",
        activationEvents = [ Tooltip.Activation.Click ],
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-click" ]
      )
      Tooltip.create (
        Button.create (text "Bottom", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Bottom tooltip",
        direction = Tooltip.Direction.Bottom,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-bottom" ]
      )
      Tooltip.create (
        Button.create (text "Left", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Left tooltip",
        direction = Tooltip.Direction.Left,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-left" ]
      )
      Tooltip.create (
        Button.create (text "Right", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Right tooltip",
        direction = Tooltip.Direction.Right,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-right" ]
      )
      Tooltip.create (
        Button.create (text "No arrow", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "No arrow tooltip",
        showArrow = false,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-no-arrow" ]
      )
      Tooltip.create (
        Button.create (text "Primary", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Primary tooltip",
        tooltipAttrs = [ Tooltip.Color.primary ],
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-primary" ]
      )
    ]

  let private sliderPage () =
    div [] [
      Slider.create (Var.Create 50, labelText = View.Const "Default Slider")
      Slider.create (
        Var.Create 0,
        min = 0,
        max = 10,
        step = 1,
        showTickMarks = true,
        tickMarkLabels = [ "0"; "5"; "10" ],
        labelText = View.Const "With Ticks"
      )
      Slider.create (Var.Create 30, labelText = View.Const "Disabled Slider", enabled = View.Const false)
      Slider.primary (Var.Create 75, labelText = View.Const "Primary Slider")
    ]

  let private resizeListenerPage () =
    let mounted = Var.Create true
    let size = Var.Create<ResizeListener.ElementSize option> None

    div [] [
      Button.create (
        text "Toggle tracked element",
        (fun () -> Var.Set mounted (not mounted.Value)),
        attrs = [ Attr.Create "data-testid" "resize-toggle" ]
      )
      size.View
      |> Doc.BindView(fun current ->
        div [ Attr.Create "data-testid" "tracked-width" ] [
          match current with
          | Some measured -> Doc.TextView(measured.WidthView |> View.Map(sprintf "%.0f"))
          | None -> text "not mounted"
        ])
      mounted.View
      |> Doc.BindView(fun visible ->
        if visible then
          div [
            Attr.Create "data-testid" "resize-target"
            Attr.Style "width" "120px"
            Attr.Style "height" "20px"
            ResizeListener.track size
          ] []
        else
          Doc.Empty)
    ]

  let private renderPage (hash: string) =
    match hash with
    | "checkbox" -> checkboxPage ()
    | "radio-groups" -> radioFormGroupsPage ()
    | "radio" -> radioPage ()
    | "switch" -> switchPage ()
    | "field" -> fieldPage ()
    | "numericfield" -> numericFieldPage ()
    | "textfield" -> textFieldPage ()
    | "select" -> selectPage ()
    | "resize-listener" -> resizeListenerPage ()
    | "animation" -> animationPage ()
    | "button" -> buttonPage ()
    | "dialog-plain" -> plainDialogPage ()
    | "dialog" -> dialogPage ()
    | "dialog-force" -> dialogForcePage ()
    | "dialog-triggered" -> dialogTriggeredPage ()
    | "tabs" -> tabsPage ()
    | "dropdown-menu" -> dropdownPage ()
    | "expansion-panel" -> expansionPanelPage ()
    | "alert" -> alertPage ()
    | "card" -> cardPage ()
    | "table" -> tablePage ()
    | "theme" -> themePage false
    | "theme-system" -> themePage true
    | "appbar" -> appbarPage ()
    | "listener-removal" -> listenerRemovalPage ()
    | "drawer-closed" -> drawerPage Drawer.Variant.Temporary false
    | "drawer-responsive" -> drawerPage Drawer.Variant.Responsive true
    | "drawer" -> drawerPage Drawer.Variant.Temporary true
    | "link" -> linkPage ()
    | "list" -> listPage ()
    | "chip" -> chipPage ()
    | "chipset" -> chipsetPage ()
    | "buttonmenu" -> buttonmenuPage ()
    | "buttongroup" -> buttongroupPage ()
    | "tooltip" -> tooltipPage ()
    | "slider" -> sliderPage ()
    | _ -> div [] [ text (sprintf "Unknown page: %s" hash) ]

  [<Inline "window.location.hash.replace('#', '')">]
  let private getHash () = X<string>

  [<SPAEntryPoint>]
  let Main () =
    let hash = getHash ()
    let page = renderPage hash
    let wrapper = div [ Attr.Create "data-e2e-ready" "true" ] [ page ]
    wrapper |> Doc.RunById "main"
