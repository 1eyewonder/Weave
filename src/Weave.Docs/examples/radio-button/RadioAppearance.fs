namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module RadioAppearance =

  let render (current: ToggleAppearance.Configuration) selected =
    let attrs = ToggleAppearance.attributeNames "Radio" current

    [
      sprintf "let selected = Var.Create \"%s\"" selected
      ""
      "let option label ="
      "  Radio.create ("
      "    selected,"
      "    label,"
      "    displayText = View.Const label,"
      if current.Disabled then
        "    enabled = View.Const false,"
      "    inputAttrs = [ Attr.Create \"name\" \"appearance-delivery\" ]"
      + (if List.isEmpty attrs then "" else ",")
      match attrs with
      | [] -> ()
      | [ attr ] -> "    attrs = [ " + attr + " ]"
      | _ ->
        "    attrs = ["

        for attr in attrs do
          "      " + attr

        "    ]"
      "  )"
      ""
      "fieldset [] ["
      "  legend [] [ text \"Delivery\" ]"
      "  option \"Standard\""
      "  option \"Express\""
      "]"
    ]
    |> String.concat "\n"

  let cases () = [
    for size, _ in ToggleAppearance.sizes do
      for color, _ in ToggleAppearance.colors do
        for placement, _ in ToggleAppearance.placements do
          for density, _ in ToggleAppearance.densities do
            for disabled in [ false; true ] do
              let current: ToggleAppearance.Configuration = {
                Size = size
                Color = color
                Placement = placement
                Density = density
                Disabled = disabled
              }

              for selected in [ "Standard"; "Express" ] do
                sprintf "%s %s %s %s %b %s" size color placement density disabled selected,
                render current selected
  ]
