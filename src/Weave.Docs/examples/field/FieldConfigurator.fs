namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module FieldConfigurator =

  let render () =
    let appearance = Var.Create FieldAppearance.defaults
    let value = Var.Create ""
    let isFocused = Var.Create false
    let hasValue = value.View |> View.MapCached(System.String.IsNullOrEmpty >> not)
    let shouldFloat = isFocused.View <||> hasValue

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
        select "Variant" FieldAppearance.variants (fun x -> x.Variant) (fun v x ->
          { x with Variant = v } |> FieldAppearance.normalise)
        select "Color" FieldAppearance.colors (fun x -> x.Color) (fun v x -> { x with Color = v })
        select "Width" FieldAppearance.widths (fun x -> x.FullWidth) (fun v x -> { x with FullWidth = v })
      ]

    let labelAbove =
      Var.Lens appearance (fun current -> current.LabelAbove) (fun current value -> {
        current with
            LabelAbove = value
      })

    let attrs =
      Map.ofList [
        "Field.Variant.filled", Field.Variant.filled
        "Field.Variant.outlined", Field.Variant.outlined
        "Field.Color.primary", Field.Color.primary
        "Field.Color.secondary", Field.Color.secondary
        "Field.Color.tertiary", Field.Color.tertiary
        "Field.Color.error", Field.Color.error
        "Field.Color.warning", Field.Color.warning
        "Field.Color.success", Field.Color.success
        "Field.Color.info", Field.Color.info
        "Field.Width.full", Field.Width.full
        "Field.Label.above", Field.Label.above
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let inputId = "custom-field"

        let inputElement =
          Doc.InputType.Text
            [
              Attr.Class "weave-field__input"
              attr.id inputId
              on.focus (fun _ _ -> isFocused.Value <- true)
              on.blur (fun _ _ -> isFocused.Value <- false)
            ]
            value

        let selectedAttrs =
          current
          |> FieldAppearance.attributes
          |> List.map (fun key -> Map.find key attrs)

        Field.create (
          inputElement,
          isFocused.View,
          shouldFloat,
          labelText = View.Const "Name",
          inputId = inputId,
          attrs = selectedAttrs
        ))

    let description =
      Helpers.bodyText
        "Wrap a custom input with field chrome. Track focus and whether the value is empty to control the floating label; an outlined field can instead keep its label above."

    Helpers.appearanceSection
      [ "custom-input" ]
      description
      controls
      (Some(
        Switch.create (
          labelAbove,
          text "Label above",
          enabled = (appearance.View |> View.Map(fun x -> x.Variant = "outlined"))
        )
      ))
      preview
      (fun () ->
        Var.Set appearance FieldAppearance.defaults
        Var.Set value ""
        Var.Set isFocused false)
      ((appearance.View, value.View) ||> View.Map2 FieldAppearance.render)
