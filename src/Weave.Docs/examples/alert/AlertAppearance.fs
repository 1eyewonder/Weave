namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module AlertAppearance =

  type Configuration = {
    Variant: string
    Color: string
    Density: string
    Icon: bool
  }

  let variants = [ "standard", "Standard"; "outlined", "Outlined"; "filled", "Filled" ]

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

  let densities = [ "compact", "Compact"; "standard", "Standard"; "spacious", "Spacious" ]

  let defaults = {
    Variant = "standard"
    Color = "info"
    Density = "standard"
    Icon = false
  }

  let attributeNames current = [
    "Alert.Variant." + current.Variant
    if current.Color <> "default" then
      "Alert.Color." + current.Color
    if current.Density <> "standard" then
      "Density." + current.Density
  ]

  let render current =
    let attrs = attributeNames current

    [
      if current.Icon then
        "open Weave.Icons.MaterialSymbols"
        ""
      "Alert.create ("
      "  text \"Your changes have been saved.\","
      if current.Icon then
        "  icon = Icon.create (Icon.UiActions UiActions.CheckCircle),"
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
        for density, _ in densities do
          for icon in [ false; true ] do
            let current = {
              Variant = variant
              Color = color
              Density = density
              Icon = icon
            }

            sprintf "%s %s %s %b" variant color density icon, render current
  ]
