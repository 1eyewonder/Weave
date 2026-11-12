namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module ChipSetConfigurator =

  let render () =
    let appearance = Var.Create ChipSetAppearance.defaults
    let selected = Var.Create<string option> None

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
        select "Variant" ChipSetAppearance.variants (fun x -> x.Variant) (fun value x -> {
          x with
              Variant = value
        })
        select "Color" ChipSetAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
        select "Density" ChipSetAppearance.densities (fun x -> x.Density) (fun value x -> {
          x with
              Density = value
        })
      ]

    let showIcon =
      Var.Lens appearance (fun current -> current.SelectedIcon) (fun current value -> {
        current with
            SelectedIcon = value
      })

    let attrs =
      Map.ofList [
        "Chip.Variant.filled", Chip.Variant.filled
        "Chip.Variant.outlined", Chip.Variant.outlined
        "Chip.Variant.text", Chip.Variant.text
        "Chip.Color.primary", Chip.Color.primary
        "Chip.Color.secondary", Chip.Color.secondary
        "Chip.Color.tertiary", Chip.Color.tertiary
        "Chip.Color.error", Chip.Color.error
        "Chip.Color.warning", Chip.Color.warning
        "Chip.Color.success", Chip.Color.success
        "Chip.Color.info", Chip.Color.info
        "Chip.Density.compact", Chip.Density.compact
        "Chip.Density.spacious", Chip.Density.spacious
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let selectedAttrs =
          current
          |> ChipSetAppearance.attributes
          |> List.map (fun key -> Map.find key attrs)

        let option label value =
          ChipItem.create (text label, value, attrs = selectedAttrs)

        let chips = [ option "Option A" "a"; option "Option B" "b"; option "Option C" "c" ]

        ChipSet.create (
          chips,
          selectedValue = selected,
          selectionMode = ChipSet.SelectionMode.Toggle,
          showSelectedIcon = current.SelectedIcon
        ))

    let source = (appearance.View, selected.View) ||> View.Map2 ChipSetAppearance.render

    let reset () =
      Var.Set appearance ChipSetAppearance.defaults
      Var.Set selected None

    let description =
      Helpers.bodyText
        "Combine chip variants, colors, and density. Choose a chip to select it; select it again to clear the selection. Hide the selected icon to rely on the chip's styling instead."

    Helpers.appearanceSection
      [ "variants"; "no-selected-icon"; "toggle-selection" ]
      description
      controls
      (Some(Switch.create (showIcon, text "Selected icon")))
      preview
      reset
      source
