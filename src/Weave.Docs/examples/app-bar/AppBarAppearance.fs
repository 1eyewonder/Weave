namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module AppBarAppearance =

  let positions = [ "Static"; "Sticky"; "Fixed top"; "Fixed bottom" ]

  let attributes position accent = [
    match position with
    | "Sticky" -> "AppBar.Position.sticky"
    | "Fixed top" -> "AppBar.Position.fixedTop"
    | "Fixed bottom" -> "AppBar.Position.fixedBottom"
    | _ -> ()
    if accent then
      "Attr.Style \"--palette-appbar-background\" \"var(--palette-primary-fill)\""
      "Attr.Style \"--palette-appbar-text\" \"var(--palette-primary-fill-text)\""
  ]

  let source position accent =
    let attrs = attributes position accent

    [
      "AppBar.create ("
      "  div [ Padding.All.small ] [ text \"My Application\" ]"
      + (if List.isEmpty attrs then "" else ",")
      if not (List.isEmpty attrs) then
        "  attrs = ["
        yield! attrs |> List.map (fun attr -> "    " + attr)
        "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for position in positions do
      for accent in [ false; true ] do
        sprintf "%s accent %b" position accent, source position accent
  ]
