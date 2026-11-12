namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ButtonExamples =

  let private comparison components =
    Grid.create (
      components |> List.map (fun content -> GridItem.create content),
      attrs = [ AlignItems.center ]
    )

  let private basicButtonExample () =
    let description =
      Helpers.bodyText
        "A Var holds the count. Activate the button with a pointer, Enter, or Space to update its label."

    let content =
      let count = Var.Create 0
      let label = count.View |> View.Map(sprintf "Clicks: %d") |> Doc.TextView

      Button.primary (
        label,
        onClick = (fun () -> Var.Update count ((+) 1)),
        attrs = [ Button.Variant.filled ]
      )
      |> fun button -> comparison [ button ]

    let code =
      """let count = Var.Create 0
let label = count.View |> View.Map(sprintf "Clicks: %d") |> Doc.TextView

Button.primary (
  label,
  onClick = (fun () -> Var.Update count ((+) 1)),
  attrs = [ Button.Variant.filled ]
)"""

    Helpers.codeSampleSection "Basic Button" description content code

  let appearanceExample () =
    let appearance = Var.Create ButtonSource.defaults

    let select label choices get set =
      let selected =
        Var.Lens appearance (get >> Some) (fun current value ->
          match value with
          | Some value -> set value current
          | None -> current)

      let items =
        choices
        |> List.map (fun (value, label) -> SelectItem.create (text label, label, value))
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

    let fullWidth =
      Var.Lens appearance (fun current -> current.FullWidth) (fun current value -> {
        current with
            FullWidth = value
      })

    let disabled =
      Var.Lens appearance (fun current -> current.Disabled) (fun current value -> {
        current with
            Disabled = value
      })

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        select "Variant" ButtonSource.variants (fun current -> current.Variant) (fun value current -> {
          current with
              Variant = value
        })

        select "Color" ButtonSource.colors (fun current -> current.Color) (fun value current -> {
          current with
              Color = value
        })

        select "Density" ButtonSource.densities (fun current -> current.Density) (fun value current -> {
          current with
              Density = value
        })

        select "Corners" ButtonSource.corners (fun current -> current.Corners) (fun value current -> {
          current with
              Corners = value
        })
      ]

    let accessory =
      Some(
        div [ Attr.Class "docs-appearance__actions" ] [
          Switch.create (fullWidth, text "Full width")
          Switch.create (disabled, text "Disabled")
        ]
      )

    let renderButton (current: ButtonSource.Appearance) =
      let variant =
        match current.Variant with
        | ButtonSource.Variant.Filled -> Button.Variant.filled
        | ButtonSource.Variant.Outlined -> Button.Variant.outlined
        | ButtonSource.Variant.Text -> Button.Variant.text

      let color =
        match current.Color with
        | ButtonSource.Color.Primary -> Button.Color.primary
        | ButtonSource.Color.Secondary -> Button.Color.secondary
        | ButtonSource.Color.Tertiary -> Button.Color.tertiary
        | ButtonSource.Color.Error -> Button.Color.error
        | ButtonSource.Color.Warning -> Button.Color.warning
        | ButtonSource.Color.Success -> Button.Color.success
        | ButtonSource.Color.Info -> Button.Color.info

      let attrs = [
        variant
        color

        match current.Density with
        | ButtonSource.Density.Compact -> Density.compact
        | ButtonSource.Density.Standard -> ()
        | ButtonSource.Density.Spacious -> Density.spacious

        match current.Corners with
        | ButtonSource.Corners.Default -> ()
        | ButtonSource.Corners.None -> BorderRadius.All.none
        | ButtonSource.Corners.Small -> BorderRadius.All.small
        | ButtonSource.Corners.Medium -> BorderRadius.All.medium
        | ButtonSource.Corners.Large -> BorderRadius.All.large
        | ButtonSource.Corners.Pill -> BorderRadius.pill

        if current.FullWidth then
          Button.Width.full
      ]

      div [
        Attr.Class "docs-appearance__bounds"
        Attr.Style "padding" "12px"
        if current.FullWidth then
          Attr.Style "width" "100%"
          Attr.Style "box-sizing" "border-box"
      ] [
        Button.create (
          text "Save changes",
          onClick = (fun () -> ()),
          enabled = View.Const(not current.Disabled),
          attrs = attrs
        )
      ]

    let preview = appearance.View |> Doc.BindView renderButton
    let source = appearance.View |> View.Map ButtonSource.render

    let reset () =
      Var.Set appearance ButtonSource.defaults

    let description =
      Helpers.bodyText
        "Use filled for the main action, outlined for a secondary action, and text for a quieter action. Full width fills the parent. Disable the button to prevent activation. Combine styles and copy the selected button."

    Helpers.appearanceSection
      [
        "variants"
        "colors"
        "full-width"
        "border-radius"
        "disabled"
        "density"
        "when-to-use"
      ]
      description
      controls
      accessory
      preview
      reset
      source

  let iconButtonAppearance () =
    let appearance = Var.Create ButtonSource.defaults
    let circle = Var.Create false

    let select label choices get set =
      let selected =
        Var.Lens appearance (get >> Some) (fun current value ->
          match value with
          | Some value -> set value current
          | None -> current)

      let items =
        choices
        |> List.map (fun (value, itemLabel) -> SelectItem.create (text itemLabel, itemLabel, value))
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

    let disabled =
      Var.Lens appearance (fun current -> current.Disabled) (fun current value -> {
        current with
            Disabled = value
      })

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        select "Icon variant" ButtonSource.variants (fun current -> current.Variant) (fun value current -> {
          current with
              Variant = value
        })
        select "Icon color" ButtonSource.colors (fun current -> current.Color) (fun value current -> {
          current with
              Color = value
        })
        select "Icon density" ButtonSource.densities (fun current -> current.Density) (fun value current -> {
          current with
              Density = value
        })
      ]

    let preview =
      View.Map2
        (fun (current: ButtonSource.Appearance) isCircle -> current, isCircle)
        appearance.View
        circle.View
      |> Doc.BindView(fun (current, isCircle) ->
        let variant =
          match current.Variant with
          | ButtonSource.Variant.Filled -> Button.Variant.filled
          | ButtonSource.Variant.Outlined -> Button.Variant.outlined
          | ButtonSource.Variant.Text -> Button.Variant.text

        let color =
          match current.Color with
          | ButtonSource.Color.Primary -> Button.Color.primary
          | ButtonSource.Color.Secondary -> Button.Color.secondary
          | ButtonSource.Color.Tertiary -> Button.Color.tertiary
          | ButtonSource.Color.Error -> Button.Color.error
          | ButtonSource.Color.Warning -> Button.Color.warning
          | ButtonSource.Color.Success -> Button.Color.success
          | ButtonSource.Color.Info -> Button.Color.info

        let attrs = [
          Attr.Create "aria-label" "Favorite"
          variant
          color
          match current.Density with
          | ButtonSource.Density.Compact -> Density.compact
          | ButtonSource.Density.Standard -> ()
          | ButtonSource.Density.Spacious -> Density.spacious
          if isCircle then
            BorderRadius.circle
        ]

        div [ Attr.Class "docs-appearance__bounds"; Attr.Style "padding" "12px" ] [
          IconButton.create (
            Icon.create (Icon.UiActions UiActions.Favorite),
            onClick = (fun () -> ()),
            enabled = View.Const(not current.Disabled),
            attrs = attrs
          )
        ])

    let source = View.Map2 ButtonSource.renderIcon appearance.View circle.View

    let reset () =
      Var.Set appearance ButtonSource.defaults
      Var.Set circle false

    let description =
      Helpers.bodyText
        "Give each icon button an accessible name with aria-label. Change its appearance here; the outline shows its size."

    Helpers.appearanceSectionNamed
      "Icon Buttons"
      [ "icon-button-density"; "disabled-icon-buttons" ]
      description
      controls
      (Some(
        div [ Attr.Class "docs-appearance__actions" ] [
          Switch.create (circle, text "Circle")
          Switch.create (disabled, text "Icon disabled")
        ]
      ))
      preview
      reset
      source

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Button and IconButton.") [
      Helpers.apiTable "Button.create" [
        Helpers.apiParam "innerContents" "Doc" "" "Content rendered inside the button label"
        Helpers.apiParam "onClick" "unit -> unit" "" "Callback invoked on click, tap, or keyboard activation"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Reactive enabled/disabled state"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (variant, color, width, etc.)"
      ]

      Helpers.apiTable "IconButton.create" [
        Helpers.apiParam "icon" "Doc" "" "Icon content rendered inside the button"
        Helpers.apiParam "onClick" "unit -> unit" "" "Callback invoked on click, tap, or keyboard activation"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Reactive enabled/disabled state"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (variant, color, etc.)"
      ]

      Helpers.styleModuleTable "Button.Variant" [
        ("filled", "Solid background button")
        ("outlined", "Bordered button with transparent background")
        ("text", "Text-only button with no background or border")
      ]

      Helpers.styleModuleTable "Button.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]

      Helpers.styleModuleTable "Button.Width" [ ("full", "Button stretches to fill its container width") ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Button"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Use Button for an action. Use Link when activation should navigate to another page."
        ]

        Helpers.divider ()
        basicButtonExample ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        iconButtonAppearance ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
