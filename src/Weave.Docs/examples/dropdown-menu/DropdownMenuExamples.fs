namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DropdownMenuExamples =

  let private center content =
    div [ Flex.Flex.allSizes; JustifyContent.center ] [ content ]

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
      attrs = [ AlignItems.center ]
    )

  let private basicDropdownExample () =
    let description =
      Helpers.bodyText
        "Open the menu and choose an action. The menu closes and the text below reports the selection. These sample actions do not change data."

    let content =
      let lastAction = Var.Create "Choose an action"

      let createItem label =
        DropdownMenuItem.create (text label, onClick = (fun () -> Var.Set lastAction (label + " selected")))

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        DropdownMenu.create (
          triggerContent = text "Open dropdown",
          items = View.Const [ createItem "Edit"; createItem "Duplicate"; createItem "Delete" ],
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
        )

        textView lastAction.View
      ]

    let code =
      """let lastAction = Var.Create "Choose an action"

let createItem label =
  DropdownMenuItem.create (
    text label,
    onClick = (fun () -> Var.Set lastAction (label + " selected"))
  )

Doc.Concat [
  DropdownMenu.create (
    triggerContent = text "Open dropdown",
    items =
      View.Const [
        createItem "Edit"
        createItem "Duplicate"
        createItem "Delete"
      ],
    triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
  )

  textView lastAction.View
]"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private widthExample () =
    let description =
      Helpers.bodyText
        "Width.full fills the available width. Default popovers match their triggers; PopoverWidth.unbound expands for the longer action label. Open each menu to compare."

    let content =
      let items =
        [ "Edit"; "Duplicate"; "Export project as a template" ]
        |> List.map (fun label -> DropdownMenuItem.create (text label, onClick = (fun () -> ())))
        |> View.Const

      let createMenu menuAttrs label =
        DropdownMenu.create (
          triggerContent = text label,
          items = items,
          attrs = menuAttrs,
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
        )

      let previewMenu menuAttrs label =
        div [ Attr.Style "inline-size" "min(100%, 192px)" ] [ createMenu menuAttrs label ]

      comparisonGrid 2 [
        previewMenu [] "Default width"

        previewMenu [ DropdownMenu.Width.full ] "Full width"

        previewMenu [ DropdownMenu.PopoverWidth.unbound ] "Unbound popover"

        previewMenu [ DropdownMenu.Width.full; DropdownMenu.PopoverWidth.unbound ] "Full and unbound"
      ]

    let code =
      """let items =
  [ "Edit"; "Duplicate"; "Export project as a template" ]
  |> List.map (fun label ->
    DropdownMenuItem.create (text label, onClick = (fun () -> ())))
  |> View.Const

let createMenu menuAttrs label =
  DropdownMenu.create (
    triggerContent = text label,
    items = items,
    attrs = menuAttrs,
    triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
  )

createMenu [] "Default width"

createMenu [ DropdownMenu.Width.full ] "Full width"

createMenu [ DropdownMenu.PopoverWidth.unbound ] "Unbound popover"

createMenu
  [ DropdownMenu.Width.full; DropdownMenu.PopoverWidth.unbound ]
  "Full and unbound"
"""

    Helpers.codeSampleSection "Width & Popover Width" description content code

  let private placementExample () =
    let description =
      Helpers.bodyText
        "Both menus are wider than their triggers. AnchorOrigin selects the trigger edge; TransformOrigin aligns the matching menu edge. Open each to compare left and right alignment."

    let content =
      let items =
        [ "Edit"; "Duplicate"; "Export project template" ]
        |> List.map (fun label -> DropdownMenuItem.create (text label, onClick = (fun () -> ())))
        |> View.Const

      let createMenu anchor origin label =
        DropdownMenu.create (
          triggerContent = text label,
          items = items,
          attrs = [ anchor; origin; DropdownMenu.PopoverWidth.unbound ],
          triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
        )

      comparisonGrid 2 [
        createMenu DropdownMenu.AnchorOrigin.bottomLeft DropdownMenu.TransformOrigin.topLeft "Aligned left"

        createMenu DropdownMenu.AnchorOrigin.bottomRight DropdownMenu.TransformOrigin.topRight "Aligned right"
      ]

    let code =
      """let items =
  [ "Edit"; "Duplicate"; "Export project template" ]
  |> List.map (fun label ->
    DropdownMenuItem.create (text label, onClick = (fun () -> ())))
  |> View.Const

let createMenu anchor origin label =
  DropdownMenu.create (
    triggerContent = text label,
    items = items,
    attrs = [ anchor; origin; DropdownMenu.PopoverWidth.unbound ],
    triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
  )

createMenu
  DropdownMenu.AnchorOrigin.bottomLeft
  DropdownMenu.TransformOrigin.topLeft
  "Aligned left"

createMenu
  DropdownMenu.AnchorOrigin.bottomRight
  DropdownMenu.TransformOrigin.topRight
  "Aligned right"
"""

    Helpers.codeSampleSection "Placement" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Individual items can be disabled with the disabled parameter. Use DropdownMenuItem.divider() to insert visual separators between groups."

    let content =
      let items = [
        DropdownMenuItem.create (text "Edit", onClick = (fun () -> ()))

        DropdownMenuItem.create (text "Duplicate", onClick = (fun () -> ()))

        DropdownMenuItem.divider ()

        DropdownMenuItem.create (
          text "Delete (disabled)",
          onClick = (fun () -> ()),
          disabled = View.Const true
        )
      ]

      DropdownMenu.create (
        triggerContent = text "Dropdown",
        items = View.Const items,
        triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
      )

    let code =
      """let items = [
  DropdownMenuItem.create (text "Edit", onClick = (fun () -> ()))

  DropdownMenuItem.create (text "Duplicate", onClick = (fun () -> ()))

  DropdownMenuItem.divider ()

  DropdownMenuItem.create (
    text "Delete (disabled)",
    onClick = (fun () -> ()),
    disabled = View.Const true
  )
]

DropdownMenu.create (
  triggerContent = text "Dropdown",
  items = View.Const items,
  triggerAttrs = [ Button.Variant.filled; Button.Color.primary ]
)"""

    Helpers.codeSampleSection "Disabled Items and Dividers" description (center content) code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for DropdownMenu and DropdownMenuItem.") [
      Helpers.apiTable "DropdownMenu.create" [
        Helpers.apiParam "triggerContent" "Doc" "" "Content displayed inside the trigger button"
        Helpers.apiParam
          "items"
          "View<DropdownMenuItemKind list>"
          ""
          "Reactive list of menu items. Use View.Const for static lists"
        Helpers.apiParam "?isOpen" "Var<bool>" "" "External two-way binding for the open/closed state"
        Helpers.apiParam "?openOn" "OpenOn" "Click" "Whether the menu opens on Click or Hover"
        Helpers.apiParam
          "?enabled"
          "View<bool>"
          "View.Const true"
          "Whether the dropdown menu trigger is interactive"
        Helpers.apiParam "?closeOnOutsideClick" "bool" "true" "Close the menu when clicking outside"
        Helpers.apiParam
          "?triggerAttrs"
          "Attr list"
          "[]"
          "Additional attributes applied to the trigger button"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (Color, Width, AnchorOrigin, TransformOrigin, etc.) applied to the root container"
      ]

      Helpers.returnTypeNote
        "DropdownMenuItem.create returns a DropdownMenu.DropdownMenuItemKind, not a Doc. Build items with DropdownMenuItem.create and pass them as a reactive list to DropdownMenu.create."

      Helpers.apiTable "DropdownMenuItem.create" [
        Helpers.apiParam "content" "Doc" "" "Visual content displayed inside the menu item"
        Helpers.apiParam "onClick" "unit -> unit" "" "Callback invoked when the item is activated"
        Helpers.apiParam "?text" "string" "\"\"" "Accessible text for the item, used for type-ahead search"
        Helpers.apiParam "?icon" "Doc" "" "Optional leading icon Doc"
        Helpers.apiParam
          "?disabled"
          "View<bool>"
          "View.Const false"
          "Reactive view controlling the disabled state"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the item element"
      ]

      Helpers.styleModuleTable "DropdownMenuItem.divider (static method)" [
        ("divider()", "Creates a visual separator between groups of menu items")
      ]

      Helpers.styleModuleTable "DropdownMenu.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/danger color")
        ("warning", "Warning color")
        ("success", "Success color")
        ("info", "Informational color")
      ]

      Helpers.styleModuleTable "DropdownMenu.Width" [
        ("full", "Component root stretches to 100% of its container")
      ]

      Helpers.styleModuleTable "DropdownMenu.PopoverWidth" [
        ("unbound",
         "Popover panel grows freely to content width, capped at min(400px, 90vw); default is root width")
      ]

      Helpers.styleModuleTable "DropdownMenu.AnchorOrigin" [
        ("topLeft", "Anchor at top-left of trigger")
        ("topCenter", "Anchor at top-center of trigger")
        ("topRight", "Anchor at top-right of trigger")
        ("centerLeft", "Anchor at center-left of trigger")
        ("center", "Anchor at center of trigger")
        ("centerRight", "Anchor at center-right of trigger")
        ("bottomLeft", "Anchor at bottom-left of trigger")
        ("bottomCenter", "Anchor at bottom-center of trigger")
        ("bottomRight", "Anchor at bottom-right of trigger")
      ]

      Helpers.styleModuleTable "DropdownMenu.TransformOrigin" [
        ("topLeft", "Menu aligns its top-left to the anchor")
        ("topCenter", "Menu aligns its top-center to the anchor")
        ("topRight", "Menu aligns its top-right to the anchor")
        ("centerLeft", "Menu aligns its center-left to the anchor")
        ("center", "Menu aligns its center to the anchor")
        ("centerRight", "Menu aligns its center-right to the anchor")
        ("bottomLeft", "Menu aligns its bottom-left to the anchor")
        ("bottomCenter", "Menu aligns its bottom-center to the anchor")
        ("bottomRight", "Menu aligns its bottom-right to the anchor")
      ]

      Helpers.styleModuleTable "DropdownMenu.OpenOn (DU)" [
        ("Click", "Menu opens/closes on button click (default)")
        ("Hover", "Menu opens on mouse enter, closes on mouse leave")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Dropdown Menu"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Dropdown menus allow users to select an action from a list. They can be customized with colors, width modes, anchor origins, and disabled items."
        ]

        Helpers.divider ()
        basicDropdownExample ()
        Helpers.divider ()
        DropdownConfigurator.render ()
        Helpers.divider ()
        widthExample ()
        Helpers.divider ()
        placementExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
