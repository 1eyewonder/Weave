namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module SelectConfigurator =

  let render () =
    let appearance = Var.Create SelectAppearance.defaults
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
        select "Variant" SelectAppearance.variants (fun x -> x.Variant) (fun value x ->
          { x with Variant = value } |> SelectAppearance.normalise)
        select "Color" SelectAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
        select "Width" SelectAppearance.widths (fun x -> x.Width) (fun value x -> { x with Width = value })
      ]

    let labelAbove =
      Var.Lens appearance (fun current -> current.LabelAbove) (fun current value -> {
        current with
            LabelAbove = value
      })

    let unbound =
      Var.Lens appearance (fun current -> current.Unbound) (fun current value -> {
        current with
            Unbound = value
      })

    let attrs =
      Map.ofList [
        "Select.Variant.filled", Select.Variant.filled
        "Select.Variant.outlined", Select.Variant.outlined
        "Select.Color.primary", Select.Color.primary
        "Select.Color.secondary", Select.Color.secondary
        "Select.Color.tertiary", Select.Color.tertiary
        "Select.Color.error", Select.Color.error
        "Select.Color.warning", Select.Color.warning
        "Select.Color.success", Select.Color.success
        "Select.Color.info", Select.Color.info
        "Select.Width.dynamic", Select.Width.dynamic
        "Select.Width.full", Select.Width.full
        "Select.Label.above", Select.Label.above
        "Select.PopoverWidth.unbound", Select.PopoverWidth.unbound
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let selectedAttrs =
          current
          |> SelectAppearance.attributeNames
          |> List.map (fun key -> Map.find key attrs)

        let items =
          [ "Apple"; "Banana"; "Passion fruit from the southern orchard" ]
          |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
          |> View.Const

        Select.create (items, selected, labelText = View.Const "Fruit", attrs = selectedAttrs))

    let source = (appearance.View, selected.View) ||> View.Map2 SelectAppearance.render

    let reset () =
      Var.Set appearance SelectAppearance.defaults
      Var.Set selected None

    let accessory =
      div [ Attr.Class "docs-appearance__actions" ] [
        Switch.create (
          labelAbove,
          text "Label above",
          enabled = (appearance.View |> View.Map(fun current -> current.Variant = "outlined"))
        )
        Switch.create (unbound, text "Unbound menu")
      ]

    let description =
      Helpers.bodyText
        "Choose a style, accent, and trigger width. The label can stay above an outlined field. Choose Dynamic and Unbound menu to let longer options grow beyond the trigger."

    Helpers.appearanceSection
      [ "variants"; "colors"; "persistent-label"; "width-&-popover-width" ]
      description
      controls
      (Some accessory)
      preview
      reset
      source
