namespace Weave.Tests.E2E.Site

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ColorPickerPage =

  let render () =
    let color = Var.Create(PickerColor.ofHex "#FF0000")
    let enabled = Var.Create true
    let readOnly = Var.Create false
    let opaque = Var.Create(PickerColor.ofHex "#12345680")
    let isOpen = Var.Create false
    let swatches = Var.Create ColorPicker.defaultPalette

    let saveSwatch () =
      if not (swatches.Value |> List.contains color.Value) then
        swatches.Value <- [ yield! swatches.Value; color.Value ]

    let removeSwatch () =
      swatches.Value <- swatches.Value |> List.filter ((<>) color.Value)

    div [] [
      ColorPicker.create (
        color,
        palette = swatches.View,
        enabled = enabled.View,
        readOnly = readOnly.View,
        attrs = [ Attr.Create "data-testid" "picker" ]
      )

      ColorPicker.field (
        color,
        labelText = View.Const "Accent",
        palette = swatches.View,
        enabled = enabled.View,
        readOnly = readOnly.View,
        isOpen = isOpen,
        attrs = [ Attr.Create "data-testid" "field" ]
      )

      ColorPicker.create (
        opaque,
        mode = ColorPicker.Mode.Hex,
        attrs = [ ColorPicker.Controls.hideFormatTabs; Attr.Create "data-testid" "opaque" ]
      )

      ColorPicker.field (
        color,
        mode = ColorPicker.Mode.RGBA,
        attrs = [ ColorPicker.Controls.hideFormatTabs; Attr.Create "data-testid" "rgb-only" ]
      )

      ColorPicker.create (
        opaque,
        mode = ColorPicker.Mode.RGB,
        attrs = [ Attr.Create "data-testid" "opaque-rgb" ]
      )

      p [ Attr.Create "data-testid" "value" ] [ Doc.TextView(color.View |> View.Map PickerColor.toHex) ]
      p [ Attr.Create "data-testid" "opaque-value" ] [
        Doc.TextView(opaque.View |> View.Map PickerColor.toHex)
      ]

      Button.create (text "Save swatch", saveSwatch, attrs = [ Attr.Create "data-testid" "save-swatch" ])

      Button.create (
        text "Remove swatch",
        removeSwatch,
        attrs = [ Attr.Create "data-testid" "remove-swatch" ]
      )

      Button.create (
        text "Clear swatches",
        (fun () -> swatches.Value <- []),
        attrs = [ Attr.Create "data-testid" "clear-swatches" ]
      )

      Button.create (
        text "Open picker",
        (fun () -> isOpen.Value <- true),
        attrs = [ Attr.Create "data-testid" "open" ]
      )

      p [ Attr.Create "data-testid" "open-state" ] [ Doc.TextView(isOpen.View |> View.Map string<bool>) ]
      Button.create (
        text "Set externally",
        (fun () -> color.Value <- PickerColor.ofHex "#00FF0080"),
        attrs = [ Attr.Create "data-testid" "external" ]
      )

      Button.create (
        text "Toggle enabled",
        (fun () -> enabled.Value <- not enabled.Value),
        attrs = [ Attr.Create "data-testid" "enabled" ]
      )

      Button.create (
        text "Toggle read only",
        (fun () -> readOnly.Value <- not readOnly.Value),
        attrs = [ Attr.Create "data-testid" "readonly" ]
      )
    ]
