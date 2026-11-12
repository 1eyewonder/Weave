namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ToggleAppearance =

  type Configuration = {
    Size: string
    Color: string
    Placement: string
    Density: string
    Disabled: bool
  }

  let sizes = [ "small", "Small"; "medium", "Medium"; "large", "Large" ]

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

  let placements = [ "right", "Right"; "left", "Left"; "top", "Top"; "bottom", "Bottom" ]
  let densities = [ "compact", "Compact"; "standard", "Standard"; "spacious", "Spacious" ]

  let defaults = {
    Size = "medium"
    Color = "default"
    Placement = "right"
    Density = "standard"
    Disabled = false
  }

  let attributeNames componentName current = [
    if current.Size <> "medium" then
      yield componentName + ".Size." + current.Size
    if current.Color <> "default" then
      yield componentName + ".Color." + current.Color
    if current.Placement <> "right" then
      yield componentName + ".ContentPlacement." + current.Placement
    if current.Density <> "standard" then
      yield "Density." + current.Density
  ]

  let render componentName current isChecked =
    let attrs = attributeNames componentName current

    let content =
      if componentName = "Checkbox" then
        "View.Const \"Email updates\""
      else
        "text \"Email updates\""

    [
      sprintf "let isChecked = Var.Create %s" (if isChecked then "true" else "false")
      ""
      if not current.Disabled && List.isEmpty attrs then
        sprintf "%s.create (isChecked, %s)" componentName content
      else
        sprintf "%s.create (" componentName
        "  isChecked,"
        "  " + content + ","

        if current.Disabled then
          "  enabled = View.Const false" + (if List.isEmpty attrs then "" else ",")

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

  let cases componentName () = [
    for size, _ in sizes do
      for color, _ in colors do
        for placement, _ in placements do
          for density, _ in densities do
            for disabled in [ false; true ] do
              let current = {
                Size = size
                Color = color
                Placement = placement
                Density = density
                Disabled = disabled
              }

              for isChecked in [ false; true ] do
                sprintf "%s %s %s %s %b %b" size color placement density disabled isChecked,
                render componentName current isChecked
  ]
