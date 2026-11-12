namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ProgressAppearance =
  type Configuration = {
    Shape: string
    Color: string
    Size: string
    Style: string
  }

  let defaults = {
    Shape = "Linear"
    Color = "Primary"
    Size = "Medium"
    Style = "Plain"
  }

  let shapes = [ "Linear"; "Circular" ]
  let colors = [ "Primary"; "Secondary"; "Tertiary"; "Success"; "Error"; "Warning"; "Info" ]
  let sizes = [ "Small"; "Medium"; "Large" ]
  let styles = [ "Plain"; "Rounded"; "Striped"; "Rounded and striped" ]

  let attributes config = [
    if config.Color <> "Primary" then
      "Progress.Color." + config.Color.ToLower()
    if config.Size <> "Medium" then
      "Progress." + config.Shape + ".Size." + config.Size.ToLower()
    if config.Shape = "Linear" then
      if config.Style = "Rounded" || config.Style = "Rounded and striped" then
        "Progress.Linear.Variant.rounded"

      if config.Style = "Striped" || config.Style = "Rounded and striped" then
        "Progress.Linear.Variant.striped"
  ]

  let source config =
    let attrs = "Attr.Create \"aria-label\" \"Upload\"" :: attributes config

    [
      "Progress." + config.Shape + ".create ("
      "  value = View.Const 60.0,"
      "  attrs = ["
      yield! attrs |> List.map (fun attr -> "    " + attr)
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for shape in shapes do
      for color in colors do
        for size in sizes do
          for style in (if shape = "Linear" then styles else [ "Plain" ]) do
            let config = {
              Shape = shape
              Color = color
              Size = size
              Style = style
            }

            yield sprintf "%s %s %s %s" shape color size style, source config
  ]
