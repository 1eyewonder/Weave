namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module DataAppearance =
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

  let highlighter () =
    let state: Var<DataAppearanceSource.Highlighter.Configuration> =
      Var.Create DataAppearanceSource.Highlighter.defaults

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice
          "Search term"
          DataAppearanceSource.Highlighter.terms
          (fun (x: DataAppearanceSource.Highlighter.Configuration) -> x.Term)
          (fun x value -> { x with Term = value })
          state
        choice
          "Color"
          DataAppearanceSource.Highlighter.colors
          (fun (x: DataAppearanceSource.Highlighter.Configuration) -> x.Color)
          (fun x value -> { x with Color = value })
          state
      ]

    let sensitive =
      Var.Lens state (fun x -> x.Sensitive) (fun x value -> { x with Sensitive = value })

    let preview =
      state.View
      |> Doc.BindView(fun x ->
        let color =
          match x.Color with
          | "Primary" -> Highlighter.Color.primary
          | "Secondary" -> Highlighter.Color.secondary
          | "Tertiary" -> Highlighter.Color.tertiary
          | "Success" -> Highlighter.Color.success
          | "Error" -> Highlighter.Color.error
          | "Info" -> Highlighter.Color.info
          | _ -> Highlighter.Color.warning

        Highlighter.create (
          View.Const "This Item is an item.",
          View.Const x.Term,
          caseSensitive = View.Const x.Sensitive,
          attrs = [ color ]
        ))

    Helpers.appearanceSection
      [ "appearance-and-plain-text" ]
      (Helpers.bodyText
        "Choose a term, color, and case sensitivity. Content stays plain text, including HTML-looking strings. For custom colors, override --weave-highlighter-background and --weave-highlighter-color on the wrapper.")
      controls
      (Some(Switch.create (sensitive, text "Case sensitive")))
      preview
      (fun () -> state.Value <- DataAppearanceSource.Highlighter.defaults)
      (state.View |> View.Map DataAppearanceSource.Highlighter.source)

  let table () =
    let state: Var<DataAppearanceSource.Table.Configuration> =
      Var.Create DataAppearanceSource.Table.defaults

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice
          "Density"
          DataAppearanceSource.Table.densities
          (fun (x: DataAppearanceSource.Table.Configuration) -> x.Density)
          (fun x value -> { x with Density = value })
          state
      ]

    let striped =
      Var.Lens state (fun x -> x.Striped) (fun x value -> { x with Striped = value })

    let hoverable =
      Var.Lens state (fun x -> x.Hoverable) (fun x value -> { x with Hoverable = value })

    let options =
      Some(
        div [ Attr.Class "docs-appearance__actions" ] [
          Switch.create (striped, text "Striped")
          Switch.create (hoverable, text "Hoverable")
        ]
      )

    let preview =
      state.View
      |> Doc.BindView(fun x ->
        let rows = [ "Apple", 42; "Banana", 17; "Cherry", 120 ]

        let columns = [
          TableColumn.create ("fruit", text "Fruit", (fun (name, _) -> text name))
          TableColumn.create ("stock", text "Stock", (fun (_, stock) -> text (string<int> stock)))
        ]

        let attrs = [
          match x.Density with
          | "Compact" -> Table.Density.compact
          | "Spacious" -> Table.Density.spacious
          | _ -> ()
          if x.Striped then
            Table.striped
          if x.Hoverable then
            Table.hoverable
        ]

        Table.create (columns, View.Const rows, fst, caption = text "Fruit stock", attrs = attrs))

    Helpers.appearanceSection
      [ "density-and-styling"; "density" ]
      (Helpers.bodyText
        "Combine density, alternating row stripes, and hover highlighting. For custom tint colors, see the styling example below.")
      controls
      options
      preview
      (fun () -> state.Value <- DataAppearanceSource.Table.defaults)
      (state.View |> View.Map DataAppearanceSource.Table.source)
