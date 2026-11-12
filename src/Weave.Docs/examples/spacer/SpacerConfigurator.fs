namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module SpacerConfigurator =

  let render () =
    let appearance = Var.Create SpacerAppearance.defaults

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
        select "Direction" SpacerAppearance.directions (fun x -> x.Direction) (fun value x -> {
          x with
              Direction = value
        })
        select "Spacers" SpacerAppearance.counts (fun x -> x.Count) (fun value x -> { x with Count = value })
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let action label =
          Button.primary (text label, onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])

        let attrs = [
          Flex.Flex.allSizes
          if current.Direction = "column" then
            FlexDirection.Column.allSizes
            Attr.Style "height" "180px"
          else
            AlignItems.center
            Attr.Style "width" "100%"
        ]

        div attrs [
          action "One"
          Spacer.create ()
          action "Two"
          if current.Count = 2 then
            Spacer.create ()
            action "Three"
        ])

    let description =
      Helpers.bodyText
        "See how one or two spacers distribute remaining space in a row or column. A column needs available height for a vertical spacer to expand."

    Helpers.appearanceSection
      [ "multiple-spacers" ]
      description
      controls
      None
      preview
      (fun () -> Var.Set appearance SpacerAppearance.defaults)
      (appearance.View |> View.Map SpacerAppearance.render)
