namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ButtonGroupExamples =

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
      attrs = [ AlignItems.start ]
    )

  let private basicExample () =
    let group =
      ButtonGroup.create (
        [
          Button.create (text "One", onClick = ignore)
          Button.create (text "Two", onClick = ignore)
          Button.create (text "Three", onClick = ignore)
        ],
        attrs = [ ButtonGroup.Variant.filled; ButtonGroup.Color.primary ]
      )

    let code =
      """ButtonGroup.create (
  [
    Button.create (text "One", onClick = ignore)
    Button.create (text "Two", onClick = ignore)
    Button.create (text "Three", onClick = ignore)
  ],
  attrs = [ ButtonGroup.Variant.filled; ButtonGroup.Color.primary ]
)"""

    Helpers.codeSampleSection
      "Basic Button Group"
      (Helpers.bodyText "Group related actions into one visual unit.")
      (center group)
      code

  let appearanceExample () =
    let variant = Var.Create "Filled"
    let color = Var.Create "Primary"
    let density = Var.Create "Standard"
    let orientation = Var.Create "Horizontal"

    let choice label values (value: Var<string>) =
      let selected =
        Var.Lens value Some (fun current next -> Option.defaultValue current next)

      let items =
        values
        |> List.map (fun name -> SelectItem.create (text name, name, name))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice "Variant" GroupBAppearance.groupVariants variant
        choice "Color" GroupBAppearance.colors color
        choice "Density" GroupBAppearance.densities density
        choice "Orientation" GroupBAppearance.orientations orientation
      ]

    let attributes =
      Map.ofList [
        "ButtonGroup.Variant.filled", ButtonGroup.Variant.filled
        "ButtonGroup.Variant.outlined", ButtonGroup.Variant.outlined
        "ButtonGroup.Variant.text", ButtonGroup.Variant.text
        "ButtonGroup.Color.primary", ButtonGroup.Color.primary
        "ButtonGroup.Color.secondary", ButtonGroup.Color.secondary
        "ButtonGroup.Color.tertiary", ButtonGroup.Color.tertiary
        "ButtonGroup.Color.error", ButtonGroup.Color.error
        "ButtonGroup.Color.warning", ButtonGroup.Color.warning
        "ButtonGroup.Color.success", ButtonGroup.Color.success
        "ButtonGroup.Color.info", ButtonGroup.Color.info
        "ButtonGroup.Density.compact", ButtonGroup.Density.compact
        "ButtonGroup.Density.spacious", ButtonGroup.Density.spacious
        "ButtonGroup.Orientation.vertical", ButtonGroup.Orientation.vertical
      ]

    let config =
      let visual = (variant.View, color.View) ||> View.Map2(fun a b -> a, b)
      let layout = (density.View, orientation.View) ||> View.Map2(fun a b -> a, b)
      (visual, layout) ||> View.Map2(fun (a, b) (c, d) -> a, b, c, d)

    let preview =
      config
      |> Doc.BindView(fun (variant, color, density, orientation) ->
        let attrs =
          GroupBAppearance.groupAttributes variant color density orientation
          |> List.map (fun name -> Map.find name attributes)

        ButtonGroup.create (
          [
            Button.create (text "One", onClick = ignore)
            Button.create (text "Two", onClick = ignore)
            Button.create (text "Three", onClick = ignore)
          ],
          attrs = attrs
        ))

    let source =
      config |> View.Map(fun (a, b, c, d) -> GroupBAppearance.groupSource a b c d)

    let reset () =
      Var.Set variant "Filled"
      Var.Set color "Primary"
      Var.Set density "Standard"
      Var.Set orientation "Horizontal"

    Helpers.appearanceSection
      [ "colors"; "vertical-orientation"; "density-and-colors"; "density" ]
      (Helpers.bodyText "Combine a variant, color, density, and orientation for the whole group.")
      controls
      None
      preview
      reset
      source

  let private splitButtonExample () =
    let description =
      Helpers.bodyText
        "A ButtonMenu can be placed inside a button group to create a split button. The group wraps around both the action button and the dropdown trigger."

    let content =
      let isOpen = Var.Create false

      ButtonGroup.create (
        [
          Button.create (text "Reply", onClick = (fun () -> ()))
          IconButtonMenu.create (
            closedIcon = Icon.create (Icon.UiActions UiActions.ArrowDropDown),
            items = [
              Button.create (
                text "Reply",
                onClick = (fun () -> ()),
                attrs = [ Button.Variant.filled; Button.Width.full ]
              )
              Button.create (
                text "Reply All",
                onClick = (fun () -> ()),
                attrs = [ Button.Variant.filled; Button.Width.full ]
              )
              Button.create (
                text "Forward",
                onClick = (fun () -> ()),
                attrs = [ Button.Variant.filled; Button.Width.full ]
              )
            ],
            direction = ButtonMenu.Direction.Bottom,
            isOpen = isOpen,
            triggerAttrs = []
          )
        ],
        attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
      )

    let code =
      """open Weave.Icons.MaterialSymbols

let isOpen = Var.Create false

ButtonGroup.create (
  [
    Button.create (text "Reply", onClick = (fun () -> ()))
    IconButtonMenu.create (
      closedIcon = Icon.create (Icon.UiActions UiActions.ArrowDropDown),
      items = [
        Button.create (
          text "Reply",
          onClick = (fun () -> ()),
          attrs = [ Button.Variant.filled; Button.Width.full ]
        )
        Button.create (
          text "Reply All",
          onClick = (fun () -> ()),
          attrs = [ Button.Variant.filled; Button.Width.full ]
        )
        Button.create (
          text "Forward",
          onClick = (fun () -> ()),
          attrs = [ Button.Variant.filled; Button.Width.full ]
        )
      ],
      direction = ButtonMenu.Direction.Bottom,
      isOpen = isOpen,
      triggerAttrs = []
    )
  ],
  attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
)"""

    Helpers.codeSampleSection "Split Button" description (center content) code

  let private iconButtonExample () =
    let description = Helpers.bodyText "Icon buttons can also be grouped together."

    let content =
      ButtonGroup.create (
        [
          IconButton.create (
            Icon.create (Icon.Text Text.FormatBold),
            onClick = (fun () -> ()),
            attrs = [ Attr.Create "aria-label" "Bold" ]
          )
          IconButton.create (
            Icon.create (Icon.Text Text.FormatItalic),
            onClick = (fun () -> ()),
            attrs = [ Attr.Create "aria-label" "Italic" ]
          )
          IconButton.create (
            Icon.create (Icon.Text Text.FormatUnderlined),
            onClick = (fun () -> ()),
            attrs = [ Attr.Create "aria-label" "Underline" ]
          )
        ],
        attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
      )

    let code =
      """open Weave.Icons.MaterialSymbols

ButtonGroup.create (
  [
    IconButton.create (
      Icon.create (Icon.Text Text.FormatBold),
      onClick = (fun () -> ()),
      attrs = [ Attr.Create "aria-label" "Bold" ]
    )
    IconButton.create (
      Icon.create (Icon.Text Text.FormatItalic),
      onClick = (fun () -> ()),
      attrs = [ Attr.Create "aria-label" "Italic" ]
    )
    IconButton.create (
      Icon.create (Icon.Text Text.FormatUnderlined),
      onClick = (fun () -> ()),
      attrs = [ Attr.Create "aria-label" "Underline" ]
    )
  ],
  attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
)"""

    Helpers.codeSampleSection "Icon Buttons" description (center content) code

  let private dropdownExample () =
    let description =
      Helpers.bodyText
        "A DropdownMenu can be placed inside a button group to create a split button with a dropdown menu."

    let content =
      ButtonGroup.create (
        [
          Button.create (text "Save", onClick = (fun () -> ()))
          DropdownMenu.create (
            text "Save Options",
            View.Const [
              DropdownMenuItem.create (text "Save as Draft", (fun () -> ()))
              DropdownMenuItem.create (text "Save and Publish", (fun () -> ()))
              DropdownMenuItem.create (text "Save and Close", (fun () -> ()))
            ]
          )
        ],
        attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
      )

    let code =
      """ButtonGroup.create (
  [
    Button.create (text "Save", onClick = (fun () -> ()))
    DropdownMenu.create (
      text "Save Options",
      View.Const [
        DropdownMenuItem.create (text "Save as Draft", (fun () -> ()))
        DropdownMenuItem.create (text "Save and Publish", (fun () -> ()))
        DropdownMenuItem.create (text "Save and Close", (fun () -> ()))
      ]
    )
  ],
  attrs = [ ButtonGroup.Variant.outlined; ButtonGroup.Color.primary ]
)"""

    Helpers.codeSampleSection "Dropdown Menu" description (center content) code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for ButtonGroup.") [
      Helpers.apiTable "ButtonGroup.create" [
        Helpers.apiParam
          "items"
          "Doc list"
          ""
          "List of child elements (buttons, icon buttons, dropdowns, etc.)"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (variant, orientation, density, color, etc.)"
      ]

      Helpers.styleModuleTable "ButtonGroup.Variant" [
        ("filled", "Solid background buttons")
        ("outlined", "Bordered buttons with transparent background")
        ("text", "Text-only buttons")
      ]

      Helpers.styleModuleTable "ButtonGroup.Orientation" [
        ("vertical", "Stack buttons vertically instead of horizontally")
      ]

      Helpers.styleModuleTable "ButtonGroup.Density" [
        ("compact", "Compact button sizing")
        ("standard", "Standard button sizing (default)")
        ("spacious", "Spacious button sizing")
      ]

      Helpers.styleModuleTable "ButtonGroup.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Button Group"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "ButtonGroup groups buttons, icon buttons, button menus, and dropdowns into a single visual unit with shared variant, color, and size styling."
        ]

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        splitButtonExample ()
        Helpers.divider ()
        iconButtonExample ()
        Helpers.divider ()
        dropdownExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
