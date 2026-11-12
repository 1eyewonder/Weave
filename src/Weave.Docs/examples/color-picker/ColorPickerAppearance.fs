namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ColorPickerAppearance =

  type Configuration = {
    Presentation: string
    Mode: string
    HideTabs: bool
    NoPalette: bool
  }

  let presentations = [ "field", "Field"; "create", "Embedded" ]
  let modes = [ "HexWithAlpha", "Hex + alpha"; "Hex", "Hex"; "RGB", "RGB"; "RGBA", "RGBA" ]

  let defaults = {
    Presentation = "field"
    Mode = "HexWithAlpha"
    HideTabs = false
    NoPalette = false
  }

  let render current color =
    let args = [
      if current.Mode <> "HexWithAlpha" then
        yield "mode = ColorPicker.Mode." + current.Mode
      if current.NoPalette then
        yield "palette = View.Const []"
      if current.HideTabs then
        yield "attrs = [ ColorPicker.Controls.hideFormatTabs ]"
    ]

    [
      sprintf "let color = Var.Create(PickerColor.ofHex \"%s\")" color
      ""
      sprintf "ColorPicker.%s (" current.Presentation
      "  color,"
      "  labelText = View.Const \"Accent color\""
      + (if List.isEmpty args then "" else ",")
      for index, arg in List.indexed args do
        "  " + arg + (if index < List.length args - 1 then "," else "")
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for presentation, _ in presentations do
      for mode, _ in modes do
        for hideTabs in [ false; true ] do
          for noPalette in [ false; true ] do
            for color in [ "#6200EA"; "#6200EA80" ] do
              let current = {
                Presentation = presentation
                Mode = mode
                HideTabs = hideTabs
                NoPalette = noPalette
              }

              sprintf "%s %s %b %b %s" presentation mode hideTabs noPalette color, render current color
  ]
