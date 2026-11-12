namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module SelectAppearance =

  type Configuration = {
    Variant: string
    Color: string
    Width: string
    LabelAbove: bool
    Unbound: bool
  }

  let variants = [ "standard", "Standard"; "filled", "Filled"; "outlined", "Outlined" ]

  let colors = [
    "default", "Default"
    "primary", "Primary"
    "secondary", "Secondary"
    "tertiary", "Tertiary"
    "error", "Error"
    "warning", "Warning"
    "success", "Success"
    "info", "Info"
  ]

  let widths = [ "stable", "Stable"; "dynamic", "Dynamic"; "full", "Full" ]

  let defaults = {
    Variant = "standard"
    Color = "default"
    Width = "stable"
    LabelAbove = false
    Unbound = false
  }

  let normalise current =
    if current.Variant = "outlined" then
      current
    else
      { current with LabelAbove = false }

  let attributeNames configuration =
    let current = normalise configuration

    [
      if current.Variant <> "standard" then
        yield "Select.Variant." + current.Variant
      if current.Color <> "default" then
        yield "Select.Color." + current.Color
      if current.Width <> "stable" then
        yield "Select.Width." + current.Width
      if current.LabelAbove then
        yield "Select.Label.above"
      if current.Unbound then
        yield "Select.PopoverWidth.unbound"
    ]

  let render current selected =
    let attrs = attributeNames current

    let value =
      match selected with
      | Some fruit -> sprintf "(Some \"%s\")" fruit
      | None -> "None"

    [
      "let selected ="
      sprintf "  Var.Create<string option> %s" value
      ""
      "let items ="
      "  [ \"Apple\"; \"Banana\""
      "    \"Passion fruit from the southern orchard\" ]"
      "  |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))"
      "  |> View.Const"
      ""
      "Select.create ("
      "  items,"
      "  selected,"
      "  labelText = View.Const \"Fruit\"" + (if List.isEmpty attrs then "" else ",")
      match attrs with
      | [] -> ()
      | [ attr ] -> "  attrs = [ " + attr + " ]"
      | _ ->
        "  attrs = ["

        for attr in attrs do
          "    " + attr

        "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant, _ in variants do
      for color, _ in colors do
        for width, _ in widths do
          for labelAbove in [ false; true ] do
            for unbound in [ false; true ] do
              let current = {
                Variant = variant
                Color = color
                Width = width
                LabelAbove = labelAbove
                Unbound = unbound
              }

              if not labelAbove || variant = "outlined" then
                for selected in
                  [
                    None
                    Some "Apple"
                    Some "Banana"
                    Some "Passion fruit from the southern orchard"
                  ] do
                  sprintf "%s %s %s %b %b %A" variant color width labelAbove unbound selected,
                  render current selected
  ]
