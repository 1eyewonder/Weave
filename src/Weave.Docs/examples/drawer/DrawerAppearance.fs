namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module DrawerAppearance =

  let variants = [ "Temporary"; "Persistent"; "Responsive"; "Mini" ]
  let positions = [ "Left"; "Right" ]
  let clips = [ "Full height"; "Below app bar" ]

  let private workspaceSource =
    """open Weave.Icons.MaterialSymbols

let isOpen = Var.Create false
let toggle () = Var.Update isOpen not

let navigation =
  nav [ Attr.Create "aria-label" "Workspace navigation" ] [
    Icon.create (Icon.Text Text.FolderOpen)
    span [ cl "weave-drawer-mini-label" ] [ text "Projects" ]
    // Other navigation items removed for brevity.
  ]

let toolbar =
  AppBar.create (
    Button.create (text "Toggle drawer", onClick = toggle)
  )

// Workspace content and inline preview host removed for brevity.
let page = div [ cl "weave-main-content" ] [ text "Workspace" ]
"""

  let source variant position clip isOpen =
    let slot =
      if position = "Right" then
        "  rightDrawer ="
      else
        "  leftDrawer ="

    [
      yield
        workspaceSource
          .TrimEnd()
          .Replace(
            "let isOpen = Var.Create false",
            "let isOpen = Var.Create " + (if isOpen then "true" else "false")
          )
      yield ""
      yield "DrawerContainer.create ("
      yield "  mainContent = Doc.Concat [ toolbar; page ],"
      yield slot
      yield "    Drawer.create ("
      yield "      navigation,"
      yield "      isOpen.View,"
      if variant <> "Temporary" then
        yield "      variant = Drawer.Variant." + variant + ","
      if position = "Right" then
        yield "      position = Drawer.Position.Right,"
      if clip = "Below app bar" then
        yield "      clipMode = Drawer.ClipMode.AppBar,"
      if variant <> "Persistent" then
        yield "      overlayClose = (fun () -> Var.Set isOpen false),"
      yield "      width = 208,"
      yield "      isFixed = false"
      yield "    ),"
      yield "  attrs = [ SurfaceColor.BackgroundColor.background ]"
      yield ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant in variants do
      for position in positions do
        for clip in clips do
          for isOpen in [ false; true ] do
            sprintf "%s %s %s %b" variant position clip isOpen, source variant position clip isOpen
  ]
