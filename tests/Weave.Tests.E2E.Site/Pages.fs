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
      Radio.create (selected, "A", displayText = View.Const "Option A")
      Radio.create (selected, "B", displayText = View.Const "Option B")
      Radio.create (selected, "C", displayText = View.Const "Disabled Option", enabled = View.Const false)
    ]

  let private switchPage () =
    div [] [
      Switch.create (Var.Create false, content = text "Off Switch")
      Switch.create (Var.Create true, content = text "On Switch")
      Switch.create (Var.Create false, content = text "Disabled Switch", enabled = View.Const false)
    ]

  let private fieldPage () =
    div [] [
      Field.create (Var.Create "", variant = Field.Variant.Standard, labelText = View.Const "Standard Field")
      Field.create (
        Var.Create "some value",
        variant = Field.Variant.Outlined,
        labelText = View.Const "Outlined Field"
      )
      Field.create (
        Var.Create "",
        variant = Field.Variant.Standard,
        labelText = View.Const "Disabled Field",
        enabled = View.Const false
      )
      Field.create (
        Var.Create "",
        variant = Field.Variant.Standard,
        labelText = View.Const "With Help",
        showHelpText = View.Const true,
        helpText = text "Help text content"
      )
      Field.create (
        Var.Create "not-an-email",
        variant = Field.Variant.Outlined,
        labelText = View.Const "Email",
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Invalid email address",
            attrs = [ Attr.Create "id" "email-error"; Field.HelpTextColor.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "email-error"
        ],
        attrs = [ Field.Color.error ]
      )
    ]

  let private numericFieldPage () =
    div [] [
      NumericField.create (
        Var.Create 5,
        variant = Field.Variant.Standard,
        labelText = View.Const "Standard Numeric",
        showSpinButtons = View.Const true
      )
      NumericField.create (
        Var.Create 10,
        variant = Field.Variant.Outlined,
        labelText = View.Const "Outlined Numeric",
        showSpinButtons = View.Const true
      )
      NumericField.create (
        Var.Create 150,
        variant = Field.Variant.Outlined,
        labelText = View.Const "Max 100",
        max = 100,
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Value exceeds maximum",
            attrs = [ Attr.Create "id" "numeric-error"; Field.HelpTextColor.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "numeric-error"
        ],
        attrs = [ Field.Color.error ]
      )
    ]

  let private selectPage () =
    let items =
      [ "Apple"; "Banana"; "Cherry" ]
      |> List.map (fun fruit -> Select.SelectItemDef.create (text fruit) fruit fruit)
      |> View.Const

    let multiItems =
      [ "Red"; "Green"; "Blue" ]
      |> List.map (fun c -> Select.SelectItemDef.create (text c) c c)
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
    ]

  let private buttonPage () =
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
              Body1.div ("Dialog content goes here.")
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
          Body1.div ("You must complete this action.")
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
                Body1.div ("Dialog opened by button.")
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
        {
          Tabs.TabDef.Header = text "Tab 1"
          Tabs.TabDef.Disabled = View.Const false
          Tabs.TabDef.Panel = div [] [ text "Panel 1 content" ]
        }
        {
          Tabs.TabDef.Header = text "Tab 2"
          Tabs.TabDef.Disabled = View.Const false
          Tabs.TabDef.Panel = div [] [ text "Panel 2 content" ]
        }
        {
          Tabs.TabDef.Header = text "Disabled Tab"
          Tabs.TabDef.Disabled = View.Const true
          Tabs.TabDef.Panel = div [] [ text "Disabled panel" ]
        }
      ]
      |> View.Const

    Tabs.create (tabs)

  let private dropdownPage () =
    div [] [
      Dropdown.create (
        text "Open Menu",
        [
          DropdownItem.create (text "Item 1", fun () -> ())
          DropdownItem.create (text "Item 2", fun () -> ())
          DropdownItem.create (text "Disabled Item", (fun () -> ()), enabled = View.Const false)
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
          ExpansionPanelContent.create (Body1.div ("Expanded content")),
          expanded = expanded1
        )
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Collapsed Panel",
            expanded2,
            icon = ExpansionPanelHeader.defaultIcon expanded2
          ),
          ExpansionPanelContent.create (Body1.div ("Collapsed content")),
          expanded = expanded2
        )
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Focus Color Panel",
            expanded3,
            icon = ExpansionPanelHeader.defaultIcon expanded3,
            attrs = [ ExpansionPanel.FocusColor.error ]
          ),
          ExpansionPanelContent.create (Body1.div ("Focus color content")),
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

  let private appbarPage () =
    div [] [
      AppBar.create (div [] [ text "App Bar Title" ], attrs = [ Attr.Create "aria-label" "Static app bar" ])
    ]

  let private drawerPage () =
    let isOpen = Var.Create true

    DrawerContainer.create (
      mainContent = div [] [ Body1.div ("Main content area") ],
      leftDrawer =
        Drawer.create (
          div [] [
            WeaveList.create (
              [
                ListItem.create (text "Nav Item 1")
                ListItem.create (text "Nav Item 2")
                ListItem.create (text "Nav Item 3")
              ]
            )
          ],
          isOpen.View,
          variant = Drawer.Variant.Persistent,
          attrs = [ Attr.Create "aria-label" "Navigation drawer" ]
        )
    )

  let private linkPage () =
    div [] [
      Link.create (text "Default Link", href = "#")
      Link.create (text "Disabled Link", href = "#", enabled = View.Const false)
      Link.create (text "Always Underline", href = "#", attrs = [ Link.Underline.always ])
    ]

  let private listPage () =
    WeaveList.create (
      [
        ListItem.create (text "Item 1", value = "1")
        ListItem.create (text "Item 2", value = "2")
        ListItem.create (text "Disabled Item", value = "3", disabled = View.Const true)
      ]
    )

  let private chipPage () =
    div [] [
      Chip.create (text "Default Chip", attrs = [ Chip.Variant.filled ])
      Chip.create (text "Deletable Chip", onClose = (fun () -> ()), attrs = [ Chip.Variant.outlined ])
    ]

  let private chipsetPage () =
    ChipSet.create (
      [
        {
          ChipSet.ChipDef.Label = text "Chip A"
          ChipSet.ChipDef.Value = "a"
          ChipSet.ChipDef.Content = None
          ChipSet.ChipDef.Closable = false
          ChipSet.ChipDef.Disabled = View.Const false
          ChipSet.ChipDef.Attrs = []
        }
        {
          ChipSet.ChipDef.Label = text "Chip B"
          ChipSet.ChipDef.Value = "b"
          ChipSet.ChipDef.Content = None
          ChipSet.ChipDef.Closable = true
          ChipSet.ChipDef.Disabled = View.Const false
          ChipSet.ChipDef.Attrs = []
        }
      ]
    )

  let private buttonmenuPage () =
    div [] [
      ButtonMenu.create (
        text "Menu",
        [
          DropdownItem.create (text "Action 1", fun () -> ())
          DropdownItem.create (text "Action 2", fun () -> ())
        ]
      )
      ButtonMenu.create (
        text "Horizontal Menu",
        [
          DropdownItem.create (text "Left 1", fun () -> ())
          DropdownItem.create (text "Left 2", fun () -> ())
        ],
        direction = ButtonMenu.Direction.Right,
        attrs = [ Attr.Create "data-testid" "horizontal-menu" ]
      )
      Button.create (text "After", (fun () -> ()), attrs = [ Attr.Create "data-testid" "focus-target" ])
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
        text "Tooltip text"
      )
    ]

  let private renderPage (hash: string) =
    match hash with
    | "checkbox" -> checkboxPage ()
    | "radio" -> radioPage ()
    | "switch" -> switchPage ()
    | "field" -> fieldPage ()
    | "numericfield" -> numericFieldPage ()
    | "select" -> selectPage ()
    | "animation" -> animationPage ()
    | "button" -> buttonPage ()
    | "dialog" -> dialogPage ()
    | "dialog-force" -> dialogForcePage ()
    | "dialog-triggered" -> dialogTriggeredPage ()
    | "tabs" -> tabsPage ()
    | "dropdown" -> dropdownPage ()
    | "expansion-panel" -> expansionPanelPage ()
    | "alert" -> alertPage ()
    | "appbar" -> appbarPage ()
    | "drawer" -> drawerPage ()
    | "link" -> linkPage ()
    | "list" -> listPage ()
    | "chip" -> chipPage ()
    | "chipset" -> chipsetPage ()
    | "buttonmenu" -> buttonmenuPage ()
    | "buttongroup" -> buttongroupPage ()
    | "tooltip" -> tooltipPage ()
    | _ -> div [] [ text (sprintf "Unknown page: %s" hash) ]

  [<Inline "window.location.hash.replace('#', '')">]
  let private getHash () = X<string>

  [<SPAEntryPoint>]
  let Main () =
    let hash = getHash ()
    let page = renderPage hash
    let wrapper = div [ Attr.Create "data-e2e-ready" "true" ] [ page ]
    wrapper |> Doc.RunById "main"
