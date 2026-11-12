namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module SliderAppearance =
  type Configuration = {
    Color: string
    Ticks: bool
    Disabled: bool
  }

  let defaults = {
    Color = "primary"
    Ticks = false
    Disabled = false
  }

  let colors = [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ]

  let source config value =
    [
      sprintf "let value = Var.Create %d" value
      ""
      "Slider." + config.Color + " ("
      "  value,"
      if config.Ticks then
        "  step = 20,"
        "  showTickMarks = true,"
      if config.Disabled then
        "  enabled = View.Const false,"
      "  labelText = View.Const \"Volume\""
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for color in colors do
      for ticks in [ false; true ] do
        for disabled in [ false; true ] do
          for value in [ 0; 40; 100 ] do
            let config = {
              Color = color
              Ticks = ticks
              Disabled = disabled
            }

            yield sprintf "%s %b %b %d" color ticks disabled value, source config value
  ]
