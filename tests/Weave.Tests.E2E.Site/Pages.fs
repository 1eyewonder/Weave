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
      div [
        Attr.Create "data-testid" "fade-in-static"
        cl (AnimationEntrance.toClass AnimationEntrance.FadeIn)
      ] [ text "Fade in" ]
      div [
        Attr.Create "data-testid" "bounce-static"
        cl (AnimationEmphasis.toClass AnimationEmphasis.Bounce)
      ] [ text "Bounce" ]
      div [
        Attr.Create "data-testid" "pulse-hover"
        cls [
          AnimationEmphasis.toClass AnimationEmphasis.Pulse
          AnimationOn.toClass AnimationOn.Hover
        ]
      ] [ text "Pulse on hover" ]
      div [
        Attr.Create "data-testid" "suppress-container"
        cl (AnimationKind.toClass AnimationKind.Suppress)
      ] [
        div [
          Attr.Create "data-testid" "suppress-child"
          cl (AnimationEntrance.toClass AnimationEntrance.FadeIn)
        ] [ text "Suppressed child" ]
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
          cl (AnimationEntrance.toClass AnimationEntrance.FadeIn)
          Attr.Class(AnimationDelay.stagger 1)
        ] [ text "Stagger 1" ]
        div [
          Attr.Create "data-testid" "stagger-2"
          cl (AnimationEntrance.toClass AnimationEntrance.FadeIn)
          Attr.Class(AnimationDelay.stagger 2)
        ] [ text "Stagger 2" ]
        div [
          Attr.Create "data-testid" "stagger-3"
          cl (AnimationEntrance.toClass AnimationEntrance.FadeIn)
          Attr.Class(AnimationDelay.stagger 3)
        ] [ text "Stagger 3" ]
      ]
    ]

  let private checkboxPage () =
    div [] [
      Checkbox.Create(Var.Create false, View.Const "Default Checkbox")
      Checkbox.Create(Var.Create true, View.Const "Checked Checkbox")
      Checkbox.Create(Var.Create false, View.Const "Disabled Checkbox", enabled = View.Const false)
      Checkbox.Create(
        Var.Create false,
        View.Const "Small",
        attrs = [ Checkbox.Size.toClass Checkbox.Size.Small |> cl ]
      )
      Checkbox.Create(
        Var.Create false,
        View.Const "Large",
        attrs = [ Checkbox.Size.toClass Checkbox.Size.Large |> cl ]
      )
    ]

  let private radioPage () =
    let selected = Var.Create "A"

    div [] [
      Radio.Create(selected, "A", displayText = View.Const "Option A")
      Radio.Create(selected, "B", displayText = View.Const "Option B")
      Radio.Create(selected, "C", displayText = View.Const "Disabled Option", enabled = View.Const false)
    ]

  let private switchPage () =
    div [] [
      Switch.Create(Var.Create false, content = text "Off Switch")
      Switch.Create(Var.Create true, content = text "On Switch")
      Switch.Create(Var.Create false, content = text "Disabled Switch", enabled = View.Const false)
    ]

  let private fieldPage () =
    div [] [
      Field.Create(Var.Create "", variant = Field.Variant.Standard, labelText = View.Const "Standard Field")
      Field.Create(
        Var.Create "some value",
        variant = Field.Variant.Outlined,
        labelText = View.Const "Outlined Field"
      )
      Field.Create(
        Var.Create "",
        variant = Field.Variant.Standard,
        labelText = View.Const "Disabled Field",
        enabled = View.Const false
      )
      Field.Create(
        Var.Create "",
        variant = Field.Variant.Standard,
        labelText = View.Const "With Help",
        showHelpText = View.Const true,
        helpText = text "Help text content"
      )
      Field.Create(
        Var.Create "not-an-email",
        variant = Field.Variant.Outlined,
        labelText = View.Const "Email",
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.Create(
            text "Invalid email address",
            attrs = [
              Attr.Create "id" "email-error"
              Field.HelpTextColor.toClass BrandColor.Error |> cl
            ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "email-error"
        ],
        attrs = [ Field.Color.toClass BrandColor.Error |> cl ]
      )
    ]

  let private numericFieldPage () =
    div [] [
      NumericField.Create(
        Var.Create 5,
        variant = Field.Variant.Standard,
        labelText = View.Const "Standard Numeric",
        showSpinButtons = View.Const true
      )
      NumericField.Create(
        Var.Create 10,
        variant = Field.Variant.Outlined,
        labelText = View.Const "Outlined Numeric",
        showSpinButtons = View.Const true
      )
      NumericField.Create(
        Var.Create 150,
        variant = Field.Variant.Outlined,
        labelText = View.Const "Max 100",
        max = 100,
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.Create(
            text "Value exceeds maximum",
            attrs = [
              Attr.Create "id" "numeric-error"
              Field.HelpTextColor.toClass BrandColor.Error |> cl
            ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "numeric-error"
        ],
        attrs = [ Field.Color.toClass BrandColor.Error |> cl ]
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
      Select.Create(
        items,
        Var.Create<string option> None,
        labelText = View.Const "Single Select",
        placeholder = View.Const "Choose a fruit"
      )
      Select.Create(
        items,
        Var.Create<string option>(Some "Apple"),
        labelText = View.Const "With Value",
        clearable = View.Const true
      )
      Select.Create(
        items,
        Var.Create<string option> None,
        labelText = View.Const "Disabled Select",
        enabled = View.Const false
      )
      Select.Create(
        items,
        Var.Create<string option> None,
        labelText = View.Const "Searchable",
        searchable = true
      )
      Select.CreateMulti(multiItems, Var.Create(set [ "Red"; "Blue" ]), labelText = View.Const "Multi Select")
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
        Icon.Create(Icon.UiActions UiActions.Home),
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
        Dialog.Create(
          DialogTitle.Create(text "Dialog Title"),
          DialogContent.Create(
            div [] [
              Body1.Div("Dialog content goes here.")
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
    Dialog.Create(
      DialogTitle.Create(text "Force Dialog Title"),
      DialogContent.Create(
        div [] [
          Body1.Div("You must complete this action.")
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
          Dialog.Create(
            DialogTitle.Create(text "Triggered Dialog"),
            DialogContent.Create(
              div [] [
                Body1.Div("Dialog opened by button.")
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

    Tabs.Create(tabs)

  let private dropdownPage () =
    div [] [
      Dropdown.Create(
        text "Open Menu",
        [
          DropdownItem.Create(text "Item 1", fun () -> ())
          DropdownItem.Create(text "Item 2", fun () -> ())
          DropdownItem.Create(text "Disabled Item", (fun () -> ()), enabled = View.Const false)
        ]
      )
      Button.create (text "After", (fun () -> ()), attrs = [ Attr.Create "data-testid" "focus-target" ])
    ]

  let private expansionPanelPage () =
    let expanded1 = Var.Create true
    let expanded2 = Var.Create false
    let expanded3 = Var.Create false

    ExpansionPanelContainer.Create(
      [
        ExpansionPanel.Create(
          ExpansionPanelHeader.CreateWithDefaultIcons(text "Expanded Panel", expanded1),
          ExpansionPanelContent.Create(Body1.Div("Expanded content")),
          expanded = expanded1
        )
        ExpansionPanel.Create(
          ExpansionPanelHeader.CreateWithDefaultIcons(text "Collapsed Panel", expanded2),
          ExpansionPanelContent.Create(Body1.Div("Collapsed content")),
          expanded = expanded2
        )
        ExpansionPanel.Create(
          ExpansionPanelHeader.CreateWithDefaultIcons(
            text "Focus Color Panel",
            expanded3,
            attrs = [ ExpansionPanel.FocusColor.toClass BrandColor.Error |> cl ]
          ),
          ExpansionPanelContent.Create(Body1.Div("Focus color content")),
          expanded = expanded3
        )
      ]
    )

  let private alertPage () =
    div [] [
      Alert.Create(
        text "Success alert message",
        attrs = [
          Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Success)
          |> Attr.bindOption cl
        ]
      )
      Alert.Create(
        text "Error alert message",
        attrs = [
          Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Error)
          |> Attr.bindOption cl
        ]
      )
      Alert.Create(
        text "Warning alert message",
        attrs = [
          Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Warning)
          |> Attr.bindOption cl
        ]
      )
      Alert.Create(
        text "Info alert message",
        attrs = [
          Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
          |> Attr.bindOption cl
        ]
      )
    ]

  let private appbarPage () =
    div [] [
      AppBar.Create(
        div [] [ text "App Bar Title" ],
        position = AppBar.Position.Static,
        attrs = [ Attr.Create "aria-label" "Static app bar" ]
      )
    ]

  let private drawerPage () =
    let isOpen = Var.Create true

    DrawerContainer.Create(
      mainContent = div [] [ Body1.Div("Main content area") ],
      leftDrawer =
        Drawer.CreatePersistent(
          div [] [
            WeaveList.Create(
              [
                ListItem.Create(text "Nav Item 1")
                ListItem.Create(text "Nav Item 2")
                ListItem.Create(text "Nav Item 3")
              ]
            )
          ],
          isOpen.View,
          attrs = [ Attr.Create "aria-label" "Navigation drawer" ]
        )
    )

  let private linkPage () =
    div [] [
      Link.Create(text "Default Link", href = "#")
      Link.Create(text "Disabled Link", href = "#", enabled = View.Const false)
      Link.Create(text "Always Underline", href = "#", underline = Link.Underline.Always)
    ]

  let private listPage () =
    WeaveList.Create(
      [
        ListItem.Create(text "Item 1", value = "1")
        ListItem.Create(text "Item 2", value = "2")
        ListItem.Create(text "Disabled Item", value = "3", disabled = View.Const true)
      ]
    )

  let private chipPage () =
    div [] [
      Chip.Create(text "Default Chip", attrs = [ Chip.Variant.toClass Chip.Variant.Filled |> cl ])
      Chip.Create(
        text "Deletable Chip",
        onClose = (fun () -> ()),
        attrs = [ Chip.Variant.toClass Chip.Variant.Outlined |> cl ]
      )
    ]

  let private chipsetPage () =
    ChipSet.Create(
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
      ButtonMenu.Create(
        text "Menu",
        [
          DropdownItem.Create(text "Action 1", fun () -> ())
          DropdownItem.Create(text "Action 2", fun () -> ())
        ]
      )
      ButtonMenu.Create(
        text "Horizontal Menu",
        [
          DropdownItem.Create(text "Left 1", fun () -> ())
          DropdownItem.Create(text "Left 2", fun () -> ())
        ],
        direction = ButtonMenu.Direction.Right,
        attrs = [ Attr.Create "data-testid" "horizontal-menu" ]
      )
      Button.create (text "After", (fun () -> ()), attrs = [ Attr.Create "data-testid" "focus-target" ])
    ]

  let private buttongroupPage () =
    ButtonGroup.Create(
      [
        Button.create (text "Left", (fun () -> ()), attrs = [ Button.Variant.outlined ])
        Button.create (text "Center", (fun () -> ()), attrs = [ Button.Variant.outlined ])
        Button.create (text "Right", (fun () -> ()), attrs = [ Button.Variant.outlined ])
      ]
    )

  let private tooltipPage () =
    div [] [
      Tooltip.Create(
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
