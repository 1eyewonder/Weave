namespace Weave

open WebSharper
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

    div [ cl Css.``weave-chipset``; yield! attrs ] (chips |> List.map renderChip)
