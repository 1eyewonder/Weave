namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module ChipSet =

  [<RequireQualifiedAccess; Struct>]
  type SelectionMode =
    | Single
    | Toggle
    | Multi

  type ChipDef = {
    Label: Doc
    Value: string
    Content: Doc option
    Closable: bool
    Disabled: View<bool>
    Attrs: Attr list
  }

open ChipSet

[<JavaScript>]
type ChipItem =

  static member create
    (label: Doc, value: string, ?content: Doc, ?closable: bool, ?disabled: View<bool>, ?attrs: Attr list)
    : ChipSet.ChipDef =
    {
      Label = label
      Value = value
      Content = content
      Closable = defaultArg closable false
      Disabled = defaultArg disabled (View.Const false)
      Attrs = defaultArg attrs []
    }

[<JavaScript>]
type ChipSet =

  static member create
    (
      chips: ChipDef list,
      ?selectedValue: Var<string option>,
      ?selectedValues: Var<Set<string>>,
      ?selectionMode: SelectionMode,
      ?selectedIcon: (unit -> Doc),
      ?showSelectedIcon: bool,
      ?onClose: string -> unit,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =

    let selectionMode = defaultArg selectionMode SelectionMode.Single
    let showSelectedIcon = defaultArg showSelectedIcon true
    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    let defaultSelectedIcon () =
      Doc.Verbatim
        """<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"></polyline></svg>"""

    let resolvedSelectedIcon =
      if showSelectedIcon then
        Some(defaultArg selectedIcon defaultSelectedIcon)
      else
        None

    let renderChip (def: ChipDef) =
      let isSelected =
        match selectedValues, selectedValue with
        | Some svs, _ -> svs.View |> View.Map(fun sel -> Set.contains def.Value sel)
        | _, Some sv -> sv.View |> View.Map(fun sel -> sel = Some def.Value)
        | _ -> View.Const false

      let chipEnabled = View.Map2 (fun e d -> e && not d) enabled def.Disabled

      let handleClick =
        match selectedValues, selectedValue with
        | Some svs, _ ->
          Some(fun () ->
            Var.Update svs (fun current ->
              if Set.contains def.Value current then
                Set.remove def.Value current
              else
                Set.add def.Value current))
        | _, Some sv ->
          Some(fun () ->
            match selectionMode with
            | SelectionMode.Toggle ->
              Var.Update sv (fun current -> if current = Some def.Value then None else Some def.Value)
            | SelectionMode.Single -> Var.Set sv (Some def.Value)
            | SelectionMode.Multi -> Var.Set sv (Some def.Value))
        | _ -> None

      let handleClose =
        match onClose with
        | Some handler when def.Closable -> Some(fun () -> handler def.Value)
        | _ -> None

      let chipContent =
        match resolvedSelectedIcon, def.Content with
        | Some icon, Some c -> Some(isSelected.Doc(fun isSel -> if isSel then icon () else c))
        | Some icon, None -> Some(isSelected.Doc(fun isSel -> if isSel then icon () else Doc.Empty))
        | None, Some c -> Some c
        | None, None -> None

      Chip.create (
        label = def.Label,
        ?onClick = handleClick,
        ?onClose = handleClose,
        ?content = chipContent,
        selected = isSelected,
        enabled = chipEnabled,
        attrs = def.Attrs
      )

    let chipSelector = ".weave-chip[role='button']"

    let setRovingTabindex (container: Dom.Element) (focusedIndex: int) =
      let items = container.QuerySelectorAll(chipSelector)

      for i in 0 .. items.Length - 1 do
        As<Dom.Element>(items.Item(i)).SetAttribute("tabindex", if i = focusedIndex then "0" else "-1")

    let currentIndex (container: Dom.Element) =
      let items = container.QuerySelectorAll(chipSelector)
      let active = JS.Document?activeElement
      let mutable idx = 0
      let mutable found = -1

      while idx < items.Length && found < 0 do
        if JS.Inline("$0 === $1", items.Item(idx), active) then
          found <- idx

        idx <- idx + 1

      found

    let focusChipAt (container: Dom.Element) (index: int) =
      let items = container.QuerySelectorAll(chipSelector)

      if index >= 0 && index < items.Length then
        setRovingTabindex container index
        As<Dom.Element>(items.Item(index))?focus()

    let findNextEnabled (container: Dom.Element) (fromIndex: int) (direction: int) =
      let items = container.QuerySelectorAll(chipSelector)
      let count = items.Length

      if count = 0 then
        None
      else
        let mutable i = (fromIndex + direction + count) % count
        let mutable iterations = 0

        while iterations < count
              && As<Dom.Element>(items.Item(i)).ClassList.Contains(Css.``weave-chip--disabled``) do
          i <- (i + direction + count) % count
          iterations <- iterations + 1

        if iterations < count then Some i else None

    div
      [
        cl Css.``weave-chipset``
        Attr.Create "role" "group"

        on.afterRender (fun el ->
          // Deferred to ensure individual Chip DynamicCustom tabindex bindings
          // have already fired before we override them with roving tabindex.
          JS.SetTimeout (fun () -> setRovingTabindex el 0) 0 |> ignore)

        // Keep roving tabindex in sync when a chip receives focus via click
        Attr.Handler "focusin" (fun el _ ->
          let idx = currentIndex el

          if idx >= 0 then
            setRovingTabindex el idx)

        on.keyDown (fun el ev ->
          let idx = currentIndex el

          match ev with
          | Key.ArrowRight
          | Key.ArrowDown ->
            ev.PreventDefault()

            if idx >= 0 then
              match findNextEnabled el idx 1 with
              | Some target -> focusChipAt el target
              | None -> ()
            else
              focusChipAt el 0
          | Key.ArrowLeft
          | Key.ArrowUp ->
            ev.PreventDefault()

            if idx >= 0 then
              match findNextEnabled el idx -1 with
              | Some target -> focusChipAt el target
              | None -> ()
            else
              focusChipAt el 0
          | Key.Home ->
            ev.PreventDefault()
            let items = el.QuerySelectorAll(chipSelector)

            if items.Length > 0 then
              match findNextEnabled el (items.Length - 1) 1 with
              | Some target -> focusChipAt el target
              | None -> ()
          | Key.End ->
            ev.PreventDefault()

            match findNextEnabled el 0 -1 with
            | Some target -> focusChipAt el target
            | None -> ()
          | _ -> ())

        yield! attrs
      ]
      (chips |> List.map renderChip)
