namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module DialogAppearance =

  let positions = [ "Center"; "Top center"; "Bottom center"; "Left"; "Right" ]
  let interactions = [ "Force"; "Optional" ]

  let positionName position =
    match position with
    | "Top center" -> "TopCenter"
    | "Bottom center" -> "BottomCenter"
    | "Left" -> "CenterLeft"
    | "Right" -> "CenterRight"
    | _ -> "Center"

  let source position interaction blur =
    let hasOptions = position <> "Center" || interaction = "Optional" || not blur

    [
      "let isOpen = Var.Create false"
      "let close () = Var.Set isOpen false"
      ""
      "Doc.Concat ["
      "  Button.primary ("
      "    text \"Open dialog\","
      "    onClick = (fun () -> Var.Set isOpen true),"
      "    attrs = [ Button.Variant.filled ]"
      "  )"
      "  isOpen.View"
      "  |> Doc.BindView (fun openNow ->"
      "    if openNow then"
      "      Dialog.create ("
      "        DialogTitle.create (text \"Dialog\"),"
      "        Doc.Concat ["
      "          DialogContent.create (text \"Review the details.\")"
      "          DialogActions.create ("
      "            Button.primary (text \"Close\", onClick = close)"
      "          )"
      "        ]" + (if hasOptions then "," else "")
      if position <> "Center" then
        "        dialogPosition = View.Const Dialog.DialogPosition."
        + positionName position
        + (if interaction = "Optional" || not blur then "," else "")
      if interaction = "Optional" then
        "        dialogInteraction = View.Const (Dialog.Interaction.Optional close)"
        + (if not blur then "," else "")
      if not blur then
        "        blurBackground = View.Const false"
      "      )"
      "    else"
      "      Doc.Empty)"
      "]"
    ]
    |> String.concat "\n"

  let cases () = [
    for position in positions do
      for interaction in interactions do
        for blur in [ false; true ] do
          sprintf "%s %s blur %b" position interaction blur, source position interaction blur
  ]
