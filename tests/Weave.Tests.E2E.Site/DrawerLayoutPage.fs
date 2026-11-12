namespace Weave.Tests.E2E.Site

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module DrawerLayoutPage =
  let private drawerSizingPage variant clipMode itemCount =
    let navigation =
      div [] [
        for index in 1..itemCount do
          Button.create (
            text (sprintf "Page %d" index),
            onClick = ignore,
            attrs = [
              Button.Width.full
              Attr.Create "data-testid" (sprintf "sizing-item-%d" index)
            ]
          )
      ]

    div [
      Attr.Create "data-testid" "sizing-host"
      Attr.Style "inline-size" "min(100%, 480px)"
      Attr.Style "height" "320px"
      Attr.Style "position" "relative"
      Attr.Style "overflow" "hidden"
    ] [
      DrawerContainer.create (
        mainContent =
          Doc.Concat [
            AppBar.create (text "Workspace")
            div [ cl "weave-main-content" ] [ text "Main content" ]
          ],
        leftDrawer =
          Drawer.create (
            navigation,
            View.Const true,
            variant = variant,
            clipMode = clipMode,
            header = DrawerHeader.create (text "Navigation"),
            isFixed = false,
            width = 208,
            breakpoint = Drawer.DrawerBreakpoint.Always,
            attrs = [ Attr.Create "data-testid" "sizing-drawer" ]
          )
      )
    ]

  let private standaloneMiniWidthPage position =
    let drawer =
      Drawer.create (
        text "Navigation",
        View.Const true,
        variant = Drawer.Variant.Mini,
        position = position,
        isFixed = false,
        width = 208,
        attrs = [ Attr.Create "data-testid" "standalone-width-drawer" ]
      )

    div [ Attr.Style "position" "relative"; Attr.Style "height" "320px" ] [ Drawer.Render drawer ]

  let private drawerWidthPage variant position width hover =
    let isOpen = Var.Create(not hover)

    let drawer =
      Drawer.create (
        Button.create (text "Close navigation", onClick = (fun () -> Var.Set isOpen false)),
        isOpen.View,
        variant = variant,
        position = position,
        breakpoint =
          (if variant = Drawer.Variant.Mini then
             Drawer.DrawerBreakpoint.Always
           else
             Drawer.DrawerBreakpoint.At Breakpoint.Medium),
        expandOnHover = View.Const hover,
        header =
          DrawerHeader.create (
            Doc.Concat [
              Icon.create (
                Icon.UiActions UiActions.Home,
                attrs = [
                  Attr.Create "data-testid" "width-header-icon"
                  Attr.Style "width" "24px"
                  Attr.Style "height" "24px"
                ]
              )
              span [ Attr.Create "data-testid" "width-header-title" ] [ text "Workspace" ]
            ],
            attrs = [ Attr.Create "data-testid" "width-header"; Gap.All.g2 ]
          ),
        isFixed = false,
        ?width = width,
        attrs = [ Attr.Create "data-testid" "width-drawer" ]
      )

    div [
      Attr.Create "data-testid" "width-host"
      Attr.Style "inline-size" "min(100%, 480px)"
      Attr.Style "height" "320px"
      Attr.Style "position" "relative"
      Attr.Style "overflow" "hidden"
    ] [
      DrawerContainer.create (
        mainContent =
          Doc.Concat [
            AppBar.create (text "Workspace")
            div [ cl "weave-main-content" ] [ text "Main content" ]
          ],
        ?leftDrawer =
          (if position = Drawer.Position.Left then
             Some drawer
           else
             None),
        ?rightDrawer =
          (if position = Drawer.Position.Right then
             Some drawer
           else
             None)
      )
    ]

  let render route =
    match route with
    | "drawer-sizing-short" -> drawerSizingPage Drawer.Variant.Temporary Drawer.ClipMode.FullHeight 3
    | "drawer-sizing-long" -> drawerSizingPage Drawer.Variant.Temporary Drawer.ClipMode.FullHeight 20
    | "drawer-clip-temporary" -> drawerSizingPage Drawer.Variant.Temporary Drawer.ClipMode.AppBar 3
    | "drawer-clip-persistent" -> drawerSizingPage Drawer.Variant.Persistent Drawer.ClipMode.AppBar 3
    | "drawer-clip-responsive" -> drawerSizingPage Drawer.Variant.Responsive Drawer.ClipMode.AppBar 3
    | "drawer-clip-mini" -> drawerSizingPage Drawer.Variant.Mini Drawer.ClipMode.AppBar 3
    | "drawer-width-standalone-mini-left" -> standaloneMiniWidthPage Drawer.Position.Left
    | "drawer-width-standalone-mini-right" -> standaloneMiniWidthPage Drawer.Position.Right
    | "drawer-width-persistent-left" ->
      drawerWidthPage Drawer.Variant.Persistent Drawer.Position.Left (Some 208) false
    | "drawer-width-persistent-right" ->
      drawerWidthPage Drawer.Variant.Persistent Drawer.Position.Right (Some 208) false
    | "drawer-width-responsive-left" ->
      drawerWidthPage Drawer.Variant.Responsive Drawer.Position.Left (Some 208) false
    | "drawer-width-responsive-right" ->
      drawerWidthPage Drawer.Variant.Responsive Drawer.Position.Right (Some 208) false
    | "drawer-width-mini-left" -> drawerWidthPage Drawer.Variant.Mini Drawer.Position.Left (Some 208) false
    | "drawer-width-mini-right" -> drawerWidthPage Drawer.Variant.Mini Drawer.Position.Right (Some 208) false
    | "drawer-width-hover-left" -> drawerWidthPage Drawer.Variant.Mini Drawer.Position.Left (Some 208) true
    | "drawer-width-hover-right" -> drawerWidthPage Drawer.Variant.Mini Drawer.Position.Right (Some 208) true
    | "drawer-width-default-left" -> drawerWidthPage Drawer.Variant.Persistent Drawer.Position.Left None false
    | "drawer-width-default-right" ->
      drawerWidthPage Drawer.Variant.Persistent Drawer.Position.Right None false
    | _ -> div [] [ text "Unknown drawer layout fixture" ]
