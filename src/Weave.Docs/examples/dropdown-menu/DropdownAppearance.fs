namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module DropdownAppearance =

  type Configuration = {
    Variant: string
    Color: string
    Density: string
    OpenOn: string
    FullWidth: bool
    Unbound: bool
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
  let openModes = [ "Click", "Click"; "Hover", "Hover" ]

  let defaults = {
    Variant = "filled"
    Color = "primary"
    Density = "standard"
    OpenOn = "Click"
    FullWidth = false
    Unbound = false
  }

  let attributes current = [
    yield "DropdownMenu.Color." + current.Color
    if current.Density <> "standard" then
      yield "Density." + current.Density
    if current.FullWidth then
      yield "DropdownMenu.Width.full"
    if current.Unbound then
      yield "DropdownMenu.PopoverWidth.unbound"
  ]

  let render current =
    [
      "let items ="
      "  [ \"Edit\"; \"Duplicate\"; \"Export project as a template\" ]"
      "  |> List.map (fun label ->"
      "    DropdownMenuItem.create (text label, onClick = (fun () -> ())))"
      "  |> View.Const"
      ""
      "DropdownMenu.create ("
      "  triggerContent = text \"Actions\","
      "  items = items,"
      if current.OpenOn <> "Click" then
        "  openOn = DropdownMenu.OpenOn.Hover,"
      "  triggerAttrs = ["
      "    Button.Variant." + current.Variant
      "    Button.Color." + current.Color
      "  ],"
      "  attrs = ["
      for attr in attributes current do
        "    " + attr
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant, _ in variants do
      for color, _ in colors do
        for density, _ in densities do
          for openOn, _ in openModes do
            for fullWidth in [ false; true ] do
              for unbound in [ false; true ] do
                let current = {
                  Variant = variant
                  Color = color
                  Density = density
                  OpenOn = openOn
                  FullWidth = fullWidth
                  Unbound = unbound
                }

                sprintf "%s %s %s %s %b %b" variant color density openOn fullWidth unbound, render current
  ]
