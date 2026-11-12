namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module ToggleConfigurator =

  let render componentName =
    let appearance = Var.Create ToggleAppearance.defaults
    let isChecked = Var.Create false

    let select label choices get set =
      let selected =
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
        select "Size" ToggleAppearance.sizes (fun x -> x.Size) (fun value x -> { x with Size = value })
        select "Color" ToggleAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
        select "Label placement" ToggleAppearance.placements (fun x -> x.Placement) (fun value x -> {
          x with
              Placement = value
        })
        select "Density" ToggleAppearance.densities (fun x -> x.Density) (fun value x -> {
          x with
              Density = value
        })
      ]

    let disabled =
      Var.Lens appearance (fun current -> current.Disabled) (fun current value -> {
        current with
            Disabled = value
      })

    let attrs =
      Map.ofList [
        "Checkbox.Color.primary", Checkbox.Color.primary
        "Checkbox.Color.secondary", Checkbox.Color.secondary
        "Checkbox.Color.tertiary", Checkbox.Color.tertiary
        "Checkbox.Color.error", Checkbox.Color.error
        "Checkbox.Color.warning", Checkbox.Color.warning
        "Checkbox.Color.success", Checkbox.Color.success
        "Checkbox.Color.info", Checkbox.Color.info
        "Checkbox.Size.small", Checkbox.Size.small
        "Checkbox.Size.large", Checkbox.Size.large
        "Checkbox.ContentPlacement.left", Checkbox.ContentPlacement.left
        "Checkbox.ContentPlacement.top", Checkbox.ContentPlacement.top
        "Checkbox.ContentPlacement.bottom", Checkbox.ContentPlacement.bottom
        "Switch.Color.primary", Switch.Color.primary
        "Switch.Color.secondary", Switch.Color.secondary
        "Switch.Color.tertiary", Switch.Color.tertiary
        "Switch.Color.error", Switch.Color.error
        "Switch.Color.warning", Switch.Color.warning
        "Switch.Color.success", Switch.Color.success
        "Switch.Color.info", Switch.Color.info
        "Switch.Size.small", Switch.Size.small
        "Switch.Size.large", Switch.Size.large
        "Switch.ContentPlacement.left", Switch.ContentPlacement.left
        "Switch.ContentPlacement.top", Switch.ContentPlacement.top
        "Switch.ContentPlacement.bottom", Switch.ContentPlacement.bottom
        "Density.compact", Density.compact
        "Density.spacious", Density.spacious
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let selectedAttrs =
          ToggleAppearance.attributeNames componentName current
          |> List.map (fun key -> Map.find key attrs)
          |> fun attrs -> Attr.Class "docs-appearance__bounds" :: attrs

        if componentName = "Checkbox" then
          Checkbox.create (
            isChecked,
            View.Const "Email updates",
            enabled = View.Const(not current.Disabled),
            attrs = selectedAttrs
          )
        else
          Switch.create (
            isChecked,
            text "Email updates",
            enabled = View.Const(not current.Disabled),
            attrs = selectedAttrs
          ))

    let source =
      (appearance.View, isChecked.View)
      ||> View.Map2(ToggleAppearance.render componentName)

    let reset () =
      Var.Set appearance ToggleAppearance.defaults
      Var.Set isChecked false

    let description =
      Helpers.bodyText
        "Combine size, color, label placement, and density. The outline shows the control bounds as density changes. Toggle the control to see its selected state. A disabled control cannot be changed."

    Helpers.appearanceSection
      [ "sizes"; "colors"; "content-placement"; "disabled"; "density" ]
      description
      controls
      (Some(Switch.create (disabled, text "Disabled")))
      preview
      reset
      source
