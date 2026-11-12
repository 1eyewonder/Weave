namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module NumericFieldConfigurator =

  let render () =
    let appearance = Var.Create NumericFieldAppearance.defaults
    let quantity = Var.Create 1

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
        select "Variant" NumericFieldAppearance.variants (fun x -> x.Variant) (fun value x -> {
          x with
              Variant = value
        })
        select "Color" NumericFieldAppearance.colors (fun x -> x.Color) (fun value x -> {
          x with
              Color = value
        })
        select "Width" NumericFieldAppearance.widths (fun x -> x.FullWidth) (fun value x -> {
          x with
              FullWidth = value
        })
      ]

    let showSpin =
      Var.Lens appearance (fun current -> current.SpinButtons) (fun current value -> {
        current with
            SpinButtons = value
      })

    let attrs =
      Map.ofList [
        "NumericField.Variant.filled", NumericField.Variant.filled
        "NumericField.Variant.outlined", NumericField.Variant.outlined
        "NumericField.Color.primary", NumericField.Color.primary
        "NumericField.Color.secondary", NumericField.Color.secondary
        "NumericField.Color.tertiary", NumericField.Color.tertiary
        "NumericField.Color.error", NumericField.Color.error
        "NumericField.Color.warning", NumericField.Color.warning
        "NumericField.Color.success", NumericField.Color.success
        "NumericField.Color.info", NumericField.Color.info
        "NumericField.Width.full", NumericField.Width.full
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let selectedAttrs =
          current
          |> NumericFieldAppearance.attributeNames
          |> List.map (fun key -> Map.find key attrs)

        NumericField.create (
          quantity,
          labelText = View.Const "Quantity",
          showSpinButtons = View.Const current.SpinButtons,
          attrs = selectedAttrs
        ))

    let source =
      (appearance.View, quantity.View) ||> View.Map2 NumericFieldAppearance.render

    let reset () =
      Var.Set appearance NumericFieldAppearance.defaults
      Var.Set quantity 1

    let description =
      Helpers.bodyText
        "Combine field styles, accent colors, width, and spin buttons. Arrow keys and the mouse wheel still work when the spin buttons are hidden."

    Helpers.appearanceSection
      [ "variants"; "show-/-hide-spin-buttons"; "show-hide-spin-buttons" ]
      description
      controls
      (Some(Switch.create (showSpin, text "Spin buttons")))
      preview
      reset
      source
