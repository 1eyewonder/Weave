namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module GroupBAppearance =

  let cardVariants = [ "Default"; "Elevated"; "Filled" ]
  let groupVariants = [ "Filled"; "Outlined"; "Text" ]
  let colors = [ "Primary"; "Secondary"; "Tertiary"; "Error"; "Warning"; "Success"; "Info" ]
  let densities = [ "Compact"; "Standard"; "Spacious" ]
  let orientations = [ "Horizontal"; "Vertical" ]

  let lower (value: string) = value.ToLower()

  let cardSource variant actions =
    [
      "Card.create ("
      "  Doc.Concat ["
      "    CardHeader.create (text \"Weekly summary\")"
      "    CardContent.create (text \"Your week at a glance.\")"
      if actions then
        "    CardActions.create ("
        "      Button.primary (text \"Open\", onClick = ignore)"
        "    )"
      "  ]" + (if variant = "Default" then "" else ",")
      if variant <> "Default" then
        "  attrs = [ Card.Variant." + lower variant + " ]"
      ")"
    ]
    |> String.concat "\n"

  let cardCases () = [
    for variant in cardVariants do
      for actions in [ false; true ] do
        sprintf "%s actions %b" variant actions, cardSource variant actions
  ]

  let groupAttributes variant color density orientation = [
    "ButtonGroup.Variant." + lower variant
    "ButtonGroup.Color." + lower color
    if density <> "Standard" then
      "ButtonGroup.Density." + lower density
    if orientation = "Vertical" then
      "ButtonGroup.Orientation.vertical"
  ]

  let groupSource variant color density orientation =
    [
      "ButtonGroup.create ("
      "  ["
      "    Button.create (text \"One\", onClick = ignore)"
      "    Button.create (text \"Two\", onClick = ignore)"
      "    Button.create (text \"Three\", onClick = ignore)"
      "  ],"
      "  attrs = ["
      yield!
        groupAttributes variant color density orientation
        |> List.map (fun attr -> "    " + attr)
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let groupCases () = [
    for variant in groupVariants do
      for color in colors do
        for density in densities do
          for orientation in orientations do
            sprintf "%s %s %s %s" variant color density orientation,
            groupSource variant color density orientation
  ]
