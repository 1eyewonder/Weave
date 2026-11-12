namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open WeaveList

[<JavaScript>]
module ListAppearance =
  module Source = ListAppearanceSource

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
    let selected = Var.Create<string option> None
    let selectedMany = Var.Create<Set<string>> Set.empty

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice
          "Selection"
          Source.modes
          (fun (x: Source.Configuration) -> x.Mode)
          (fun x value -> { x with Mode = value })
          state
        choice
          "Density"
          Source.densities
          (fun (x: Source.Configuration) -> x.Density)
          (fun x value -> { x with Density = value })
          state
      ]

    let bordered =
      Var.Lens state (fun x -> x.Bordered) (fun x value -> { x with Bordered = value })

    let readOnly =
      Var.Lens state (fun x -> x.ReadOnly) (fun x value -> { x with ReadOnly = value })

    let accessory =
      Some(
        div [ Attr.Class "docs-appearance__actions" ] [
          Switch.create (bordered, text "Bordered")
          Switch.create (readOnly, text "Read only")
        ]
      )

    let preview =
      state.View
      |> Doc.BindView(fun current ->
        let attrs = [
          match current.Density with
          | "Compact" -> Density.compact
          | "Spacious" -> Density.spacious
          | _ -> ()
        ]

        let items = [
          for name in [ "Inbox"; "Sent"; "Drafts" ] do
            ListItem.create (text name, value = name)
        ]

        let mode =
          if current.Mode = "Toggle" then
            WeaveList.SelectionMode.ToggleSelection
          else
            WeaveList.SelectionMode.SingleSelection

        if current.Mode = "Multiple" then
          WeaveList.create (
            items,
            selectedValues = selectedMany,
            bordered = View.Const current.Bordered,
            readOnly = View.Const current.ReadOnly,
            attrs = attrs
          )
        else
          WeaveList.create (
            items,
            selectedValue = selected,
            selectionMode = mode,
            bordered = View.Const current.Bordered,
            readOnly = View.Const current.ReadOnly,
            attrs = attrs
          ))

    let selections =
      (selected.View, selectedMany.View)
      ||> View.Map2(fun single multiple -> single, Set.toList multiple)

    let source =
      (state.View, selections)
      ||> View.Map2(fun current (single, multiple) -> Source.source current single multiple)

    let reset () =
      state.Value <- Source.defaults
      selected.Value <- None
      selectedMany.Value <- Set.empty

    Helpers.appearanceSection
      [ "density" ]
      (Helpers.bodyText
        "Combine density, border, and selection modes. Toggle selection can clear the selected item; multiple selection permits several items. Read only prevents changes.")
      controls
      accessory
      preview
      reset
      source
