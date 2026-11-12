namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module TypographyAppearance =
  module Source = TypographyAppearanceSource

  let private attrs =
    Map.ofList [
      "Typography.h1", Typography.h1
      "Typography.h2", Typography.h2
      "Typography.h3", Typography.h3
      "Typography.h4", Typography.h4
      "Typography.h5", Typography.h5
      "Typography.h6", Typography.h6
      "Typography.subtitle1", Typography.subtitle1
      "Typography.subtitle2", Typography.subtitle2
      "Typography.body1", Typography.body1
      "Typography.body2", Typography.body2
      "Typography.button", Typography.button
      "Typography.caption", Typography.caption
      "Typography.overline", Typography.overline
      "Typography.Weight.light", Typography.Weight.light
      "Typography.Weight.medium", Typography.Weight.medium
      "Typography.Weight.bold", Typography.Weight.bold
      "Typography.Color.primary", Typography.Color.primary
      "Typography.Color.secondary", Typography.Color.secondary
      "Typography.Color.tertiary", Typography.Color.tertiary
      "Typography.Color.error", Typography.Color.error
      "Typography.Color.warning", Typography.Color.warning
      "Typography.Color.success", Typography.Color.success
      "Typography.Color.info", Typography.Color.info
      "Typography.Align.left", Typography.Align.left
      "Typography.Align.center", Typography.Align.center
      "Typography.Align.right", Typography.Align.right
    ]

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

  let render () =
    let state: Var<Source.Configuration> = Var.Create Source.defaults

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice
          "Variant"
          Source.variants
          (fun (x: Source.Configuration) -> x.Variant)
          (fun (x: Source.Configuration) value -> { x with Variant = value })
          state
        choice
          "Weight"
          Source.weights
          (fun (x: Source.Configuration) -> x.Weight)
          (fun (x: Source.Configuration) value -> { x with Weight = value })
          state
        choice
          "Color"
          Source.colors
          (fun (x: Source.Configuration) -> x.Color)
          (fun (x: Source.Configuration) value -> { x with Color = value })
          state
        choice
          "Alignment"
          Source.alignments
          (fun (x: Source.Configuration) -> x.Alignment)
          (fun (x: Source.Configuration) value -> { x with Alignment = value })
          state
      ]

    let preview =
      state.View
      |> Doc.BindView(fun current ->
        div [ Attr.Style "width" "100%" ] [
          div (current |> Source.attributes |> List.map (fun key -> Map.find key attrs)) [
            text "Make it clear."
          ]
        ])

    Helpers.appearanceSection
      [ "colors"; "text-alignment" ]
      (Helpers.bodyText
        "Combine a visual variant, weight, color, and alignment. Choose semantic HTML elements for your document structure independently of the visual style.")
      controls
      None
      preview
      (fun () -> state.Value <- Source.defaults)
      (state.View |> View.Map Source.source)
