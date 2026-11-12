namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ChipAppearance =

  type Configuration = {
    Variant: string
    Color: string
    Density: string
    Corners: string
    Selected: bool
    Disabled: bool
  }

  let variants = [ "filled", "Filled"; "outlined", "Outlined"; "text", "Text" ]

  let colors = [
    "primary", "Primary"
    "secondary", "Secondary"
    "tertiary", "Tertiary"
    "error", "Error"
    "warning", "Warning"
    "success", "Success"
    "info", "Info"
  ]

  let densities = [ "compact", "Compact"; "standard", "Standard"; "spacious", "Spacious" ]

  let corners = [
    "pill", "Pill"
    "label", "Label"
    "none", "None"
    "small", "Small"
    "medium", "Medium"
    "large", "Large"
  ]

  let defaults = {
    Variant = "filled"
    Color = "primary"
    Density = "standard"
    Corners = "pill"
    Selected = false
    Disabled = false
  }

  let attributeNames current = [
    yield "Chip.Variant." + current.Variant
    yield "Chip.Color." + current.Color
    if current.Density <> "standard" then
      yield "Chip.Density." + current.Density
    match current.Corners with
    | "pill" -> ()
    | "label" -> yield "Chip.Shape.label"
    | size -> yield "BorderRadius.All." + size
  ]

  let render current =
    [
      sprintf "let selected = Var.Create %s" (if current.Selected then "true" else "false")
      ""
      "Chip.create ("
      "  text \"Ready\","
      "  onClick = (fun () -> Var.Update selected not),"
      "  selected = selected.View,"
      if current.Disabled then
        "  enabled = View.Const false,"
      "  attrs = ["
      "    Attr.Dynamic \"aria-pressed\" (selected.View |> View.Map string<bool>)"
      for attr in attributeNames current do
        "    " + attr
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant, _ in variants do
      for color, _ in colors do
        for density, _ in densities do
          for corners, _ in corners do
            for selected in [ false; true ] do
              for disabled in [ false; true ] do
                let current = {
                  Variant = variant
                  Color = color
                  Density = density
                  Corners = corners
                  Selected = selected
                  Disabled = disabled
                }

                sprintf "%s %s %s %s %b %b" variant color density corners selected disabled, render current
  ]
