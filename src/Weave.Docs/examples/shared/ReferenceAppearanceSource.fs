namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ReferenceAppearanceSource =

  let colors = [
    "Default"
    "Primary"
    "Secondary"
    "Tertiary"
    "Error"
    "Warning"
    "Success"
    "Info"
  ]

  let lower (value: string) = value.ToLower()

  module Link =
    type Configuration = {
      Underline: string
      Color: string
      Icons: string
      Disabled: bool
    }

    let defaults = {
      Underline = "On hover"
      Color = "Default"
      Icons = "None"
      Disabled = false
    }

    let underlines = [ "On hover"; "Always"; "None" ]
    let icons = [ "None"; "Start"; "End"; "Both" ]

    let source config =
      let hasStart = config.Icons = "Start" || config.Icons = "Both"
      let hasEnd = config.Icons = "End" || config.Icons = "Both"

      let attrs = [
        if config.Underline = "Always" then
          "Link.Underline.always"
        if config.Underline = "None" then
          "Link.Underline.none"
        if config.Color <> "Default" then
          "Link.Color." + lower config.Color
      ]

      [
        if hasStart || hasEnd then
          "open Weave.Icons.MaterialSymbols"
          ""
        "Link.create ("
        "  text \"Documentation\","
        "  href = \"https://1eyewonder.github.io/Weave/\","
        if hasStart then
          "  startIcon = Icon.create (Icon.UiActions UiActions.Home),"
        if hasEnd then
          "  endIcon = Icon.create (Icon.UiActions UiActions.ArrowForward),"
        if config.Disabled then
          "  enabled = View.Const false,"
        if not attrs.IsEmpty then
          match attrs with
          | [ attr ] -> "  attrs = [ " + attr + " ]"
          | _ ->
            "  attrs = ["
            yield! attrs |> List.map (fun attr -> "    " + attr)
            "  ]"
        else
          // The final argument needs no trailing comma.
          ()
        ")"
      ]
      |> fun lines ->
        let last = lines.Length - 2

        lines
        |> List.mapi (fun i line -> if i = last then line.TrimEnd(',') else line)
        |> String.concat "\n"

    let cases () = [
      for underline in underlines do
        for color in colors do
          for icons in icons do
            for disabled in [ false; true ] do
              let c = {
                Underline = underline
                Color = color
                Icons = icons
                Disabled = disabled
              }

              sprintf "Link %s %s %s %b" underline color icons disabled, source c
    ]

  module Tooltip =
    type Configuration = {
      Direction: string
      Color: string
      Arrow: bool
    }

    let defaults = {
      Direction = "Top"
      Color = "Default"
      Arrow = true
    }

    let directions = [ "Top"; "Bottom"; "Left"; "Right" ]

    let source config =
      let args = [
        if config.Direction <> "Top" then
          "direction = Tooltip.Direction." + config.Direction
        if not config.Arrow then
          "showArrow = false"
        if config.Color <> "Default" then
          "tooltipAttrs = [ Tooltip.Color." + lower config.Color + " ]"
      ]

      [
        "Tooltip.create ("
        "  Button.primary (text \"Details\", onClick = ignore),"
        "  text \"More information\"" + (if args.IsEmpty then "" else ",")
        for i, arg in args |> List.indexed do
          "  " + arg + (if i < args.Length - 1 then "," else "")
        ")"
      ]
      |> String.concat "\n"

    let cases () = [
      for direction in directions do
        for color in colors do
          for arrow in [ false; true ] do
            let c = {
              Direction = direction
              Color = color
              Arrow = arrow
            }

            sprintf "Tooltip %s %s %b" direction color arrow, source c
    ]

  module Divider =
    type Configuration = {
      Variant: string
      Orientation: string
    }

    let defaults = {
      Variant = "Full width"
      Orientation = "Horizontal"
    }

    let variants = [ "Full width"; "Inset"; "Middle" ]
    let orientations = [ "Horizontal"; "Vertical" ]

    let source config =
      let attrs = [
        if config.Variant = "Inset" then
          "Divider.Variant.inset"
        if config.Variant = "Middle" then
          "Divider.Variant.middle"
        if config.Orientation = "Vertical" then
          "Divider.Orientation.vertical"
      ]

      let lines =
        match attrs with
        | [] -> [ "Divider.create ()" ]
        | [ attr ] -> [ "Divider.create (attrs = [ " + attr + " ])" ]
        | _ -> [
            "Divider.create (attrs = ["
            yield! attrs |> List.map (fun attr -> "  " + attr)
            "])"
          ]

      if config.Orientation = "Vertical" then
        [
          "div [ Flex.Flex.allSizes; Attr.Style \"height\" \"80px\" ] ["
          yield! lines |> List.map (fun line -> "  " + line)
          "]"
        ]
        |> String.concat "\n"
      else
        String.concat "\n" lines

    let cases () = [
      for variant in variants do
        for orientation in orientations do
          let c = {
            Variant = variant
            Orientation = orientation
          }

          sprintf "Divider %s %s" variant orientation, source c
    ]
