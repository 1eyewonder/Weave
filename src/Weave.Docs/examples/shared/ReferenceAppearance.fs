namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ReferenceAppearance =

  let private choice label options get set state =
    let selected =
      Var.Lens state (get >> Some) (fun current choice ->
        match choice with
        | Some value -> set current value
        | None -> current)

    let items =
      options
      |> List.map (fun value -> SelectItem.create (text value, value, value))
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

  let private linkColor color =
    match color with
    | "Primary" -> Link.Color.primary
    | "Secondary" -> Link.Color.secondary
    | "Tertiary" -> Link.Color.tertiary
    | "Error" -> Link.Color.error
    | "Warning" -> Link.Color.warning
    | "Success" -> Link.Color.success
    | "Info" -> Link.Color.info
    | _ -> Attr.Empty

  let link () =
    let state: Var<ReferenceAppearanceSource.Link.Configuration> =
      Var.Create ReferenceAppearanceSource.Link.defaults

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice
          "Underline"
          ReferenceAppearanceSource.Link.underlines
          (fun (x: ReferenceAppearanceSource.Link.Configuration) -> x.Underline)
          (fun (x: ReferenceAppearanceSource.Link.Configuration) value -> { x with Underline = value })
          state
        choice
          "Color"
          ReferenceAppearanceSource.colors
          (fun (x: ReferenceAppearanceSource.Link.Configuration) -> x.Color)
          (fun (x: ReferenceAppearanceSource.Link.Configuration) value -> { x with Color = value })
          state
        choice
          "Icons"
          ReferenceAppearanceSource.Link.icons
          (fun (x: ReferenceAppearanceSource.Link.Configuration) -> x.Icons)
          (fun (x: ReferenceAppearanceSource.Link.Configuration) value -> { x with Icons = value })
          state
      ]

    let disabled =
      Var.Lens
        state
        (fun (x: ReferenceAppearanceSource.Link.Configuration) -> x.Disabled)
        (fun (x: ReferenceAppearanceSource.Link.Configuration) value -> { x with Disabled = value })

    let preview =
      state.View
      |> Doc.BindView(fun x ->
        let start =
          if x.Icons = "Start" || x.Icons = "Both" then
            Some(Icon.create (Icon.UiActions UiActions.Home))
          else
            None

        let finish =
          if x.Icons = "End" || x.Icons = "Both" then
            Some(Icon.create (Icon.UiActions UiActions.ArrowForward))
          else
            None

        let underline =
          match x.Underline with
          | "Always" -> Link.Underline.always
          | "None" -> Link.Underline.none
          | _ -> Attr.Empty

        Link.create (
          text "Documentation",
          href = "https://1eyewonder.github.io/Weave/",
          enabled = View.Const(not x.Disabled),
          ?startIcon = start,
          ?endIcon = finish,
          attrs = [ underline; linkColor x.Color ]
        ))

    Helpers.appearanceSection
      [ "underline"; "colors"; "icon-adornments"; "disabled" ]
      (Helpers.bodyText
        "Choose an underline, color, and optional icons. Keep an underline when color alone would make the link hard to distinguish. Disabled links prevent navigation and callbacks.")
      controls
      (Some(Switch.create (disabled, text "Disabled")))
      preview
      (fun () -> state.Value <- ReferenceAppearanceSource.Link.defaults)
      (state.View |> View.Map ReferenceAppearanceSource.Link.source)

  let private tooltipColor color =
    match color with
    | "Primary" -> Tooltip.Color.primary
    | "Secondary" -> Tooltip.Color.secondary
    | "Tertiary" -> Tooltip.Color.tertiary
    | "Error" -> Tooltip.Color.error
    | "Warning" -> Tooltip.Color.warning
    | "Success" -> Tooltip.Color.success
    | "Info" -> Tooltip.Color.info
    | _ -> Attr.Empty

  let tooltip () =
    let state: Var<ReferenceAppearanceSource.Tooltip.Configuration> =
      Var.Create ReferenceAppearanceSource.Tooltip.defaults

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice
          "Direction"
          ReferenceAppearanceSource.Tooltip.directions
          (fun (x: ReferenceAppearanceSource.Tooltip.Configuration) -> x.Direction)
          (fun (x: ReferenceAppearanceSource.Tooltip.Configuration) value -> { x with Direction = value })
          state
        choice
          "Color"
          ReferenceAppearanceSource.colors
          (fun (x: ReferenceAppearanceSource.Tooltip.Configuration) -> x.Color)
          (fun (x: ReferenceAppearanceSource.Tooltip.Configuration) value -> { x with Color = value })
          state
      ]

    let arrow =
      Var.Lens
        state
        (fun (x: ReferenceAppearanceSource.Tooltip.Configuration) -> x.Arrow)
        (fun (x: ReferenceAppearanceSource.Tooltip.Configuration) value -> { x with Arrow = value })

    let preview =
      state.View
      |> Doc.BindView(fun x ->
        let direction =
          match x.Direction with
          | "Bottom" -> Tooltip.Direction.Bottom
          | "Left" -> Tooltip.Direction.Left
          | "Right" -> Tooltip.Direction.Right
          | _ -> Tooltip.Direction.Top

        Tooltip.create (
          Button.primary (text "Details", onClick = ignore),
          text "More information",
          direction = direction,
          showArrow = x.Arrow,
          tooltipAttrs = [ tooltipColor x.Color ]
        ))

    Helpers.appearanceSection
      [ "directions"; "colors"; "arrow-visibility" ]
      (Helpers.bodyText
        "Hover or focus the button to reveal the tooltip. Its preferred direction may flip or shift to fit the viewport; choose a color and whether to show its arrow.")
      controls
      (Some(Switch.create (arrow, text "Show arrow")))
      preview
      (fun () -> state.Value <- ReferenceAppearanceSource.Tooltip.defaults)
      (state.View |> View.Map ReferenceAppearanceSource.Tooltip.source)

  let divider () =
    let state: Var<ReferenceAppearanceSource.Divider.Configuration> =
      Var.Create ReferenceAppearanceSource.Divider.defaults

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice
          "Variant"
          ReferenceAppearanceSource.Divider.variants
          (fun (x: ReferenceAppearanceSource.Divider.Configuration) -> x.Variant)
          (fun (x: ReferenceAppearanceSource.Divider.Configuration) value -> { x with Variant = value })
          state
        choice
          "Orientation"
          ReferenceAppearanceSource.Divider.orientations
          (fun (x: ReferenceAppearanceSource.Divider.Configuration) -> x.Orientation)
          (fun (x: ReferenceAppearanceSource.Divider.Configuration) value -> { x with Orientation = value })
          state
      ]

    let preview =
      state.View
      |> Doc.BindView(fun x ->
        let variant =
          match x.Variant with
          | "Inset" -> Divider.Variant.inset
          | "Middle" -> Divider.Variant.middle
          | _ -> Attr.Empty

        let orientation =
          if x.Orientation = "Vertical" then
            Divider.Orientation.vertical
          else
            Attr.Empty

        if x.Orientation = "Vertical" then
          div [ Attr.Style "width" "100%"; Attr.Style "height" "80px"; Flex.Flex.allSizes ] [
            Divider.create (attrs = [ variant; orientation ])
          ]
        else
          div [ Attr.Style "width" "100%" ] [ Divider.create (attrs = [ variant ]) ])

    Helpers.appearanceSection
      [ "variants" ]
      (Helpers.bodyText
        "Full width is the default. Inset adds space at the start and Middle at both ends. A vertical divider needs a flex row with a defined height.")
      controls
      None
      preview
      (fun () -> state.Value <- ReferenceAppearanceSource.Divider.defaults)
      (state.View |> View.Map ReferenceAppearanceSource.Divider.source)
