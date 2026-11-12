namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module IconPlaygroundSource =
  type Configuration = {
    Style: string
    Filled: bool
    Weight: string
    Grade: string
    OpticalSize: string
    Color: string
  }

  let defaults = {
    Style = "Outlined"
    Filled = false
    Weight = "400"
    Grade = "0"
    OpticalSize = "24"
    Color = "Default"
  }

  let source config =
    let style = config.Style + (if config.Filled then "Filled" else "")

    [
      "open Weave.Icons"
      "open Weave.Icons.MaterialSymbols"
      ""
      "span [] ["
      "  Icon.create ("
      "    Icon.Action Action.Alarm,"
      "    style = View.Const Style." + style + ","
      "    weight = View.Const IconWeight.``" + config.Weight + "``,"
      "    grade = View.Const IconGrade.``" + config.Grade + "``,"
      "    opticalSize = View.Const OpticalSize.``" + config.OpticalSize + "``,"
      "    attrs = ["
      "      Attr.Style \"font-size\" \"3em\""
      "      Attr.Create \"aria-hidden\" \"true\""
      if config.Color <> "Default" then
        "      Typography.Color." + config.Color.ToLower()
      "    ]"
      "  )"
      "  text \"Alarm\""
      "]"
    ]
    |> String.concat "\n"

  let cases () = [
    for style in [ "Outlined"; "Rounded"; "Sharp" ] do
      for filled in [ false; true ] do
        let c = {
          defaults with
              Style = style
              Filled = filled
        }

        sprintf "Icon %s %b" style filled, source c
    for weight in [ "100"; "200"; "300"; "400"; "500"; "600"; "700" ] do
      let c = { defaults with Weight = weight }
      "Icon weight " + weight, source c
    for grade in [ "-25"; "0"; "200" ] do
      let c = { defaults with Grade = grade }
      "Icon grade " + grade, source c
    for size in [ "20"; "24"; "40"; "48" ] do
      let c = { defaults with OpticalSize = size }
      "Icon size " + size, source c
    for color in
      [
        "Default"
        "Primary"
        "Secondary"
        "Tertiary"
        "Info"
        "Success"
        "Warning"
        "Error"
      ] do
      let c = { defaults with Color = color }
      "Icon color " + color, source c
    let all = {
      Style = "Sharp"
      Filled = true
      Weight = "700"
      Grade = "-25"
      OpticalSize = "48"
      Color = "Tertiary"
    }

    "Icon combined options", source all
  ]
