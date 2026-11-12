namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module TypographyAppearanceSource =
  type Configuration = {
    Variant: string
    Weight: string
    Color: string
    Alignment: string
  }

  let defaults = {
    Variant = "Body 1"
    Weight = "Default"
    Color = "Default"
    Alignment = "Default"
  }

  let variants = [
    "Heading 1"
    "Heading 2"
    "Heading 3"
    "Heading 4"
    "Heading 5"
    "Heading 6"
    "Subtitle 1"
    "Subtitle 2"
    "Body 1"
    "Body 2"
    "Button"
    "Caption"
    "Overline"
  ]

  let weights = [ "Default"; "Light"; "Medium"; "Bold" ]

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

  let alignments = [ "Default"; "Left"; "Center"; "Right" ]

  let variantName variant =
    match variant with
    | "Heading 1" -> "h1"
    | "Heading 2" -> "h2"
    | "Heading 3" -> "h3"
    | "Heading 4" -> "h4"
    | "Heading 5" -> "h5"
    | "Heading 6" -> "h6"
    | "Subtitle 1" -> "subtitle1"
    | "Subtitle 2" -> "subtitle2"
    | "Body 1" -> "body1"
    | "Body 2" -> "body2"
    | "Button" -> "button"
    | "Caption" -> "caption"
    | _ -> "overline"

  let attributes config = [
    "Typography." + variantName config.Variant
    if config.Weight <> "Default" then
      "Typography.Weight." + config.Weight.ToLower()
    if config.Color <> "Default" then
      "Typography.Color." + config.Color.ToLower()
    if config.Alignment <> "Default" then
      "Typography.Align." + config.Alignment.ToLower()
  ]

  let source config =
    let attrs = attributes config
    let prefix = "div [ " + String.concat "; " attrs + " ]"

    if prefix.Length <= 52 then
      prefix + " [ text \"Make it clear.\" ]"
    else
      [
        "div ["
        yield! attrs |> List.map (fun attr -> "  " + attr)
        "] [ text \"Make it clear.\" ]"
      ]
      |> String.concat "\n"

  let cases () = [
    for variant in variants do
      for weight in weights do
        for color in colors do
          for alignment in alignments do
            let config = {
              Variant = variant
              Weight = weight
              Color = color
              Alignment = alignment
            }

            sprintf "Typography %s %s %s %s" variant weight color alignment, source config
  ]
