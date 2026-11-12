namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ChipSetAppearance =

  type Configuration = {
    Variant: string
    Color: string
    Density: string
    SelectedIcon: bool
  }

  let variants = ChipAppearance.variants
  let colors = ChipAppearance.colors
  let densities = ChipAppearance.densities

  let defaults = {
    Variant = "outlined"
    Color = "primary"
    Density = "standard"
    SelectedIcon = true
  }

  let attributes current =
    ChipAppearance.attributeNames {
      Variant = current.Variant
      Color = current.Color
      Density = current.Density
      Corners = "pill"
      Selected = false
      Disabled = false
    }

  let render current selected =
    let selectedSource =
      match selected with
      | Some value -> sprintf "(Some \"%s\")" value
      | None -> "None"

    [
      sprintf "let selected = Var.Create<string option> %s" selectedSource
      ""
      "let option label value ="
      "  ChipItem.create ("
      "    text label,"
      "    value,"
      "    attrs = ["
      for attr in attributes current do
        "      " + attr
      "    ]"
      "  )"
      ""
      "let chips = ["
      "  option \"Option A\" \"a\""
      "  option \"Option B\" \"b\""
      "  option \"Option C\" \"c\""
      "]"
      ""
      "ChipSet.create ("
      "  chips,"
      "  selectedValue = selected,"
      "  selectionMode = ChipSet.SelectionMode.Toggle"
      + (if current.SelectedIcon then "" else ",")
      if not current.SelectedIcon then
        "  showSelectedIcon = false"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant, _ in variants do
      for color, _ in colors do
        for density, _ in densities do
          for icon in [ false; true ] do
            let current = {
              Variant = variant
              Color = color
              Density = density
              SelectedIcon = icon
            }

            for selected in [ None; Some "a"; Some "b"; Some "c" ] do
              sprintf "%s %s %s %b %A" variant color density icon selected, render current selected
  ]
