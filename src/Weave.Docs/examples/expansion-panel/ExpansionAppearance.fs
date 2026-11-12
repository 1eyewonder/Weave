namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ExpansionAppearance =

  let colors = [ "Primary"; "Secondary"; "Tertiary"; "Error"; "Warning"; "Success"; "Info" ]
  let variants = [ "Highlight"; "Filled"; "None" ]
  let densities = [ "Compact"; "Standard"; "Spacious" ]

  let source (color: string) (focus: string) variant (density: string) expanded =
    [
      "let expanded = Var.Create " + (if expanded then "true" else "false")
      "let header ="
      "  ExpansionPanelHeader.create ("
      "    text \"Details\","
      "    expanded,"
      "    icon = ExpansionPanelHeader.defaultIcon expanded,"
      if variant <> "Highlight" then
        "    highlightVariant = View.Const ExpansionPanel.HeaderVariant."
        + variant
        + ","
      "    attrs = ["
      "      ExpansionPanel.Color." + color.ToLower()
      "      ExpansionPanel.FocusColor." + focus.ToLower()
      "    ]"
      "  )"
      "let panel ="
      "  ExpansionPanel.create ("
      "    header,"
      "    content = ExpansionPanelContent.create (text \"Panel content.\"),"
      "    expanded = expanded"
      "  )"
      if density = "Standard" then
        "ExpansionPanelContainer.create ([ panel ])"
      else
        "ExpansionPanelContainer.create ("
        "  [ panel ],"
        "  attrs = [ Density." + density.ToLower() + " ]"
        ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant in variants do
      for color in colors do
        for density in densities do
          for expanded in [ false; true ] do
            sprintf "%s %s %s %b" variant color density expanded,
            source color "Primary" variant density expanded
    for focus in colors do
      sprintf "Focus %s" focus, source "Primary" focus "Highlight" "Standard" false
  ]
