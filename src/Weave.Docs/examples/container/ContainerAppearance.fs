namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ContainerAppearance =

  type Configuration = {
    MaxWidth: string
    Fixed: bool
    Gutters: bool
  }

  let widths = [
    "none", "None"
    "extraSmall", "Extra small"
    "small", "Small"
    "medium", "Medium"
    "large", "Large"
    "extraLarge", "Extra large"
    "extraExtraLarge", "Extra extra large"
  ]

  let defaults = {
    MaxWidth = "none"
    Fixed = false
    Gutters = false
  }

  let attributes current = [
    yield "SurfaceColor.BackgroundColor.paper"
    if current.MaxWidth <> "none" then
      yield "Container.MaxWidth." + current.MaxWidth
    if current.Fixed then
      yield "Container.fixedWidth"
    if current.Gutters then
      yield "Container.gutters"
  ]

  let render current =
    [
      "Container.create ("
      "  div [ BrandColor.Fill.primary; Padding.All.small ] [ text \"Content\" ],"
      "  attrs = ["
      for attr in attributes current do
        "    " + attr
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for width, _ in widths do
      for fixedWidth in [ false; true ] do
        for gutters in [ false; true ] do
          let current = {
            MaxWidth = width
            Fixed = fixedWidth
            Gutters = gutters
          }

          sprintf "%s %b %b" width fixedWidth gutters, render current
  ]
