namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module DropdownConfigurator =

  let render () =
    let appearance = Var.Create DropdownAppearance.defaults

    let select label choices get set =
      let choice =
        Var.Lens appearance (get >> Some) (fun current value ->
          match value with
          | Some value -> set value current
          | None -> current)

      let items =
        choices
        |> List.map (fun (value, name) -> SelectItem.create (text name, name, value))
        |> View.Const

      Select.create (
        items,
        choice,
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
        select "Trigger variant" DropdownAppearance.variants (fun x -> x.Variant) (fun value x -> {
          x with
              Variant = value
        })
        select "Color" DropdownAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
        select "Density" DropdownAppearance.densities (fun x -> x.Density) (fun value x -> {
          x with
              Density = value
        })
        select "Open on" DropdownAppearance.openModes (fun x -> x.OpenOn) (fun value x -> {
          x with
              OpenOn = value
        })
      ]

    let fullWidth =
      Var.Lens appearance (fun current -> current.FullWidth) (fun current value -> {
        current with
            FullWidth = value
      })

    let unbound =
      Var.Lens appearance (fun current -> current.Unbound) (fun current value -> {
        current with
            Unbound = value
      })

    let attrs =
      Map.ofList [
        "Button.Variant.filled", Button.Variant.filled
        "Button.Variant.outlined", Button.Variant.outlined
        "Button.Variant.text", Button.Variant.text
        "Button.Color.primary", Button.Color.primary
        "Button.Color.secondary", Button.Color.secondary
        "Button.Color.tertiary", Button.Color.tertiary
        "Button.Color.error", Button.Color.error
        "Button.Color.warning", Button.Color.warning
        "Button.Color.success", Button.Color.success
        "Button.Color.info", Button.Color.info
        "DropdownMenu.Color.primary", DropdownMenu.Color.primary
        "DropdownMenu.Color.secondary", DropdownMenu.Color.secondary
        "DropdownMenu.Color.tertiary", DropdownMenu.Color.tertiary
        "DropdownMenu.Color.error", DropdownMenu.Color.error
        "DropdownMenu.Color.warning", DropdownMenu.Color.warning
        "DropdownMenu.Color.success", DropdownMenu.Color.success
        "DropdownMenu.Color.info", DropdownMenu.Color.info
        "Density.compact", Density.compact
        "Density.spacious", Density.spacious
        "DropdownMenu.Width.full", DropdownMenu.Width.full
        "DropdownMenu.PopoverWidth.unbound", DropdownMenu.PopoverWidth.unbound
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let menuAttrs =
          current
          |> DropdownAppearance.attributes
          |> List.map (fun key -> Map.find key attrs)

        let triggerAttrs = [
          Map.find ("Button.Variant." + current.Variant) attrs
          Map.find ("Button.Color." + current.Color) attrs
        ]

        let items =
          [ "Edit"; "Duplicate"; "Export project as a template" ]
          |> List.map (fun label -> DropdownMenuItem.create (text label, onClick = (fun () -> ())))
          |> View.Const

        let openOn =
          if current.OpenOn = "Hover" then
            DropdownMenu.OpenOn.Hover
          else
            DropdownMenu.OpenOn.Click

        DropdownMenu.create (
          triggerContent = text "Actions",
          items = items,
          openOn = openOn,
          triggerAttrs = triggerAttrs,
          attrs = menuAttrs
        ))

    let accessory =
      div [ Attr.Class "docs-appearance__actions" ] [
        Switch.create (fullWidth, text "Full width")
        Switch.create (unbound, text "Unbound menu")
      ]

    let description =
      Helpers.bodyText
        "Combine trigger style, color, density, and opening behavior. The trigger and menu use matching colors; an unbound menu can grow wider than its trigger."

    Helpers.appearanceSection
      [ "colors"; "openon-property"; "density"; "when-to-use" ]
      description
      controls
      (Some accessory)
      preview
      (fun () -> Var.Set appearance DropdownAppearance.defaults)
      (appearance.View |> View.Map DropdownAppearance.render)
