namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module SnackbarAppearance =

  type Configuration = {
    Variant: string
    Color: string
    Position: string
  }

  let variants = [ "standard", "Standard"; "outlined", "Outlined"; "filled", "Filled" ]

  let colors = [
    "primary", "Primary"
    "secondary", "Secondary"
    "tertiary", "Tertiary"
    "error", "Error"
    "warning", "Warning"
    "success", "Success"
    "info", "Info"
  ]

  let positions = [
    "topLeft", "Top left"
    "topCenter", "Top center"
    "topRight", "Top right"
    "bottomLeft", "Bottom left"
    "bottomCenter", "Bottom center"
    "bottomRight", "Bottom right"
  ]

  let defaults = {
    Variant = "filled"
    Color = "success"
    Position = "bottomRight"
  }

  let render current =
    [
      "let snackbars = SnackbarController.create ()"
      ""
      "Doc.Concat ["
      "  Button.primary ("
      "    text \"Show notification\","
      "    onClick = (fun () ->"
      "      snackbars.show ("
      "        \"Your changes have been saved.\","
      "        dismissal = Snackbar.Dismissal.After 4000,"
      "        attrs = ["
      "          Snackbar.Variant." + current.Variant
      "          Snackbar.Color." + current.Color
      "        ]"
      "      ) |> ignore),"
      "    attrs = [ Button.Variant.filled ]"
      "  )"
      "  Snackbar.create ("
      "    snackbars,"
      "    attrs = [ Snackbar.Position." + current.Position + " ]"
      "  )"
      "]"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant, _ in variants do
      for color, _ in colors do
        for position, _ in positions do
          let current = {
            Variant = variant
            Color = color
            Position = position
          }

          sprintf "%s %s %s" variant color position, render current
  ]
