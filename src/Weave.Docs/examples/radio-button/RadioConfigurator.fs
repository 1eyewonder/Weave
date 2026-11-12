namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module RadioConfigurator =

  let render () =
    let appearance = Var.Create ToggleAppearance.defaults
    let selected = Var.Create "Standard"

    let select label choices get set =
      let selectedChoice =
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
        selectedChoice,
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
        "Radio.Color.primary", Radio.Color.primary
        "Radio.Color.secondary", Radio.Color.secondary
        "Radio.Color.tertiary", Radio.Color.tertiary
        "Radio.Color.error", Radio.Color.error
        "Radio.Color.warning", Radio.Color.warning
        "Radio.Color.success", Radio.Color.success
        "Radio.Color.info", Radio.Color.info
        "Radio.Size.small", Radio.Size.small
        "Radio.Size.large", Radio.Size.large
        "Radio.ContentPlacement.left", Radio.ContentPlacement.left
        "Radio.ContentPlacement.top", Radio.ContentPlacement.top
        "Radio.ContentPlacement.bottom", Radio.ContentPlacement.bottom
        "Density.compact", Density.compact
        "Density.spacious", Density.spacious
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let selectedAttrs =
          ToggleAppearance.attributeNames "Radio" current
          |> List.map (fun key -> Map.find key attrs)
          |> fun attrs -> Attr.Class "docs-appearance__bounds" :: attrs

        let option label =
          Radio.create (
            selected,
            label,
            displayText = View.Const label,
            enabled = View.Const(not current.Disabled),
            inputAttrs = [ Attr.Create "name" "appearance-delivery" ],
            attrs = selectedAttrs
          )

        fieldset [] [
          legend [] [ text "Delivery" ]
          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g4 ] [
            option "Standard"
            option "Express"
          ]
        ])

    let source = (appearance.View, selected.View) ||> View.Map2 RadioAppearance.render

    let reset () =
      Var.Set appearance ToggleAppearance.defaults
      Var.Set selected "Standard"

    let description =
      Helpers.bodyText
        "Combine size, color, label placement, and density. Outlines show each control’s bounds as density changes. Select an option to update the code; the group shares one Var and native name. Disabled options cannot be changed."

    Helpers.appearanceSection
      [ "sizes"; "colors"; "content-placement"; "disabled"; "density" ]
      description
      controls
      (Some(Switch.create (disabled, text "Disabled")))
      preview
      reset
      source
