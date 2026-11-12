namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module ColorPickerConfigurator =

  let render () =
    let appearance = Var.Create ColorPickerAppearance.defaults
    let color = Var.Create(PickerColor.ofHex "#6200EA")

    let select label choices get set =
      let choice =
        Var.Lens appearance (get >> Some) (fun current value ->
          match value with
          | Some value -> set value current
          | None -> current)

      let items =
        choices
        |> List.map (fun (value, name) -> SelectItem.create (text name, name, value))
        |> View.Const

      Select.create (
        items,
        choice,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        select "Presentation" ColorPickerAppearance.presentations (fun x -> x.Presentation) (fun value x -> {
          x with
              Presentation = value
        })
        select "Format" ColorPickerAppearance.modes (fun x -> x.Mode) (fun value x -> { x with Mode = value })
      ]

    let hideTabs =
      Var.Lens appearance (fun current -> current.HideTabs) (fun current value -> {
        current with
            HideTabs = value
      })

    let noPalette =
      Var.Lens appearance (fun current -> current.NoPalette) (fun current value -> {
        current with
            NoPalette = value
      })

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let mode =
          match current.Mode with
          | "Hex" -> ColorPicker.Mode.Hex
          | "RGB" -> ColorPicker.Mode.RGB
          | "RGBA" -> ColorPicker.Mode.RGBA
          | _ -> ColorPicker.Mode.HexWithAlpha

        let palette = if current.NoPalette then Some(View.Const []) else None

        let attrs =
          if current.HideTabs then
            [ ColorPicker.Controls.hideFormatTabs ]
          else
            []

        if current.Presentation = "field" then
          ColorPicker.field (
            color,
            labelText = View.Const "Accent color",
            mode = mode,
            ?palette = palette,
            attrs = attrs
          )
        else
          ColorPicker.create (
            color,
            labelText = View.Const "Accent color",
            mode = mode,
            ?palette = palette,
            attrs = attrs
          ))

    let source =
      (appearance.View, color.View)
      ||> View.Map2(fun current value -> ColorPickerAppearance.render current (PickerColor.toHex value))

    let reset () =
      Var.Set appearance ColorPickerAppearance.defaults
      Var.Set color (PickerColor.ofHex "#6200EA")

    let accessory =
      div [ Attr.Class "docs-appearance__actions" ] [
        Switch.create (hideTabs, text "Hide format tabs")
        Switch.create (noPalette, text "Hide swatches")
      ]

    let description =
      Helpers.bodyText
        "Choose an embedded picker or a field, and try Hex, RGB, or alpha-enabled formats. Format sets the initial tab; the picker tabs can then be changed independently. The generated code uses the chosen initial format. Hex and RGB produce opaque edits; alpha-enabled modes retain transparency. Hide format tabs for a fixed input format. RGB channels use bytes from 0 to 255; Hex accepts 3, 4, 6, or 8 digits and commits on Enter or blur. Invalid text leaves the color unchanged. Edits update the selected color and code."

    Helpers.appearanceSection [ "rgb-inputs" ] description controls (Some accessory) preview reset source
