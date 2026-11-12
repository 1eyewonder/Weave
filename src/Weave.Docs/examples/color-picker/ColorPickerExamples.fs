namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ColorPickerExamples =

  let private centered content =
    div [
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      AlignItems.center
      Gap.All.g4
    ] [ content ]

  let private basicExample () =
    let color = Var.Create(PickerColor.ofHex "#6200EA")

    let content =
      centered (
        div [ Typography.Align.center ] [
          ColorPicker.field (color, labelText = View.Const "Accent color")

          p [ Typography.body2; Margin.Top.small ] [
            text "Selected: "
            Doc.TextView(color.View |> View.Map PickerColor.toHex)
          ]
        ]
      )

    Helpers.codeSampleSection
      "Color field"
      (Helpers.bodyText
        "The field shows your label and a color swatch. Bind labelText to the selected color if you want to include its value. Changes apply immediately. Close with Done, Escape, an outside click, or Tab out of the picker.")
      content
      """let color = Var.Create(PickerColor.ofHex "#6200EA")

Doc.Concat [
  ColorPicker.field (color, labelText = View.Const "Accent color")
  p [] [
    text "Selected: "
    Doc.TextView(color.View |> View.Map PickerColor.toHex)
  ]
]"""

  let private embeddedExample () =
    let color = Var.Create(PickerColor.ofHex "#0288D1")

    let content =
      div [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        Gap.All.g4
        JustifyContent.center
        AlignItems.center
      ] [
        ColorPicker.create (color, labelText = View.Const "Project color")

        Card.create (
          Doc.Concat [
            CardMedia.create (
              div [
                Attr.Style "height" "128px"
                Attr.DynamicStyle "background-color" (color.View |> View.Map PickerColor.toCss)
                Attr.Create "aria-hidden" "true"
              ] []
            )

            CardContent.create (
              div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g2 ] [
                h3 [ Typography.h6 ] [ text "Your next project" ]

                p [ Typography.body2; Typography.Color.textSecondary ] [
                  text "A shared color, from the picker to your interface."
                ]
              ]
            )
          ],
          attrs = [
            Attr.Style "width" "260px"
            Attr.Style "max-width" "100%"
            Attr.Create "data-testid" "project-color-preview"
          ]
        )
      ]

    Helpers.codeSampleSection
      "Embedded picker"
      (Helpers.bodyText
        "Use create for an always-visible picker. Drag in the spectrum or focus it and use Left/Right for saturation and Up/Down for brightness. Hold Shift for larger steps. Hue and opacity also support arrow keys, Home, and End.")
      content
      """let color = Var.Create(PickerColor.ofHex "#0288D1")

Doc.Concat [
  ColorPicker.create (color, labelText = View.Const "Project color")
  Card.create (
    Doc.Concat [
      CardMedia.create (
        div [
          Attr.Style "height" "128px"
          Attr.DynamicStyle
            "background-color"
            (color.View |> View.Map PickerColor.toCss)
          Attr.Create "aria-hidden" "true"
        ] []
      )
      CardContent.create (
        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g2 ] [
          h3 [ Typography.h6 ] [ text "Your next project" ]
          p [ Typography.body2; Typography.Color.textSecondary ] [
            text "A shared color, from the picker to your interface."
          ]
        ]
      )
    ],
    attrs = [ Attr.Style "width" "260px"; Attr.Style "max-width" "100%" ]
  )
]"""

  let private paletteExample () =
    let color = Var.Create(PickerColor.ofHex "#2E7D32")

    let palette =
      [ "#1565C0"; "#2E7D32"; "#ED6C02"; "#D32F2F"; "#757575"; "#000000" ]
      |> List.map PickerColor.ofHex

    let content =
      centered (ColorPicker.create (color, mode = ColorPicker.Mode.Hex, palette = View.Const palette))

    Helpers.codeSampleSection
      "Palette and opaque colors"
      (Helpers.bodyText
        "Supply your own swatches and use Hex or RGB mode when your application needs solid colors. Edits then produce opaque values. Programmatic values are never rewritten just by mounting a picker. Pass palette = View.Const [] to omit swatches.")
      content
      """let color = Var.Create(PickerColor.ofHex "#2E7D32")

let palette =
  [ "#1565C0"; "#2E7D32"; "#ED6C02"; "#D32F2F"; "#757575"; "#000000" ]
  |> List.map PickerColor.ofHex

ColorPicker.create (
  color,
  mode = ColorPicker.Mode.Hex,
  palette = View.Const palette
)"""

  let private editableSwatchesExample () =
    let color = Var.Create(PickerColor.ofHex "#D81B60")

    let swatches =
      Var.Create([ "#1565C0"; "#2E7D32"; "#ED6C02" ] |> List.map PickerColor.ofHex)

    let saved =
      (swatches.View, color.View)
      ||> View.Map2(fun colors color -> colors |> List.contains color)

    let add () =
      if not (swatches.Value |> List.contains color.Value) then
        swatches.Value <- [ yield! swatches.Value; color.Value ]

    let remove () =
      swatches.Value <- swatches.Value |> List.filter ((<>) color.Value)

    let content =
      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
        Attr.Create "data-testid" "editable-swatches-example"
      ] [
        ColorPicker.create (color, labelText = View.Const "Your palette", palette = swatches.View)

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          JustifyContent.center
          Gap.All.g2
        ] [
          Button.primary (
            text "Add current color",
            add,
            enabled = View.not saved,
            attrs = [ Button.Variant.filled ]
          )

          Button.create (
            text "Remove current color",
            remove,
            enabled = saved,
            attrs = [ Button.Variant.outlined ]
          )
        ]

        p [
          Typography.body2
          Typography.Color.textSecondary
          Attr.Create "role" "status"
        ] [
          Doc.TextView(swatches.View |> View.Map(fun colors -> sprintf "%d saved colors" colors.Length))
        ]
      ]

    Helpers.codeSampleSection
      "Editable swatches"
      (Helpers.bodyText
        "Save the current color or select a saved swatch to remove it. The application owns the list, so it can also persist or share it. Updating palette preserves the current color and input format; an empty list hides the swatches.")
      content
      """let color = Var.Create(PickerColor.ofHex "#D81B60")

let swatches =
  Var.Create(
    [ "#1565C0"; "#2E7D32"; "#ED6C02" ] |> List.map PickerColor.ofHex
  )

let saved =
  (swatches.View, color.View)
  ||> View.Map2(fun colors color -> colors |> List.contains color)

let add () =
  if not (swatches.Value |> List.contains color.Value) then
    swatches.Value <- [ yield! swatches.Value; color.Value ]

let remove () =
  swatches.Value <- swatches.Value |> List.filter ((<>) color.Value)

Doc.Concat [
  ColorPicker.create (
    color,
    labelText = View.Const "Your palette",
    palette = swatches.View
  )
  Button.primary (
    text "Add current color",
    add,
    enabled = View.not saved,
    attrs = [ Button.Variant.filled ]
  )
  Button.create (
    text "Remove current color",
    remove,
    enabled = saved,
    attrs = [ Button.Variant.outlined ]
  )
  p [ Attr.Create "role" "status" ] [
    Doc.TextView (
      swatches.View
      |> View.Map(fun colors -> sprintf "%d saved colors" colors.Length)
    )
  ]
]"""

  let private stateExample () =
    let enabled = Var.Create true
    let readOnly = Var.Create false
    let color = Var.Create(PickerColor.ofHex "#D81B60")

    let content =
      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        div [
          Flex.Flex.allSizes
          JustifyContent.center
          FlexWrap.Wrap.allSizes
          Gap.All.g4
          Margin.Bottom.small
        ] [
          Switch.create (enabled, content = text "Enabled")
          Switch.create (readOnly, content = text "Read only")

          Button.create (
            text "Reset color",
            (fun () -> color.Value <- PickerColor.ofHex "#D81B60"),
            attrs = [ Button.Variant.outlined ]
          )
        ]

        ColorPicker.create (color, enabled = enabled.View, readOnly = readOnly.View)
      ]

    Helpers.codeSampleSection
      "Reactive state"
      (Helpers.bodyText
        "Bind enabled and readOnly to Views. All editing paths respect these states, while programmatic updates still appear in every control.")
      content
      """let enabled = Var.Create true
let readOnly = Var.Create false
let color = Var.Create(PickerColor.ofHex "#D81B60")

Doc.Concat [
  Doc.Concat [
    Switch.create (enabled, content = text "Enabled")
    Switch.create (readOnly, content = text "Read only")

    Button.create (
      text "Reset color",
      (fun () -> color.Value <- PickerColor.ofHex "#D81B60"),
      attrs = [ Button.Variant.outlined ]
    )
  ]

  ColorPicker.create (color, enabled = enabled.View, readOnly = readOnly.View)
]"""

  let private apiReference () =
    Helpers.apiSection
      (Helpers.bodyText
        "Both picker variants share the same color model and editing options. The field adds a caller-owned open state.")
      [
        Helpers.apiTable "ColorPicker.create / ColorPicker.field" [
          Helpers.apiParam "value" "Var<PickerColor>" "Required" "Two-way sRGB color binding"
          Helpers.apiParam "?labelText" "View<string>" "Color" "Visible and accessible label"

          Helpers.apiParam
            "?mode"
            "ColorPicker.Mode"
            "HexWithAlpha"
            "Initial format and opacity support: Hex, HexWithAlpha, RGB, or RGBA"

          Helpers.apiParam
            "?palette"
            "View<PickerColor list>"
            "View.Const defaultPalette"
            "Reactive swatches; View.Const [] hides them"

          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Reactive enabled state"
          Helpers.apiParam "?readOnly" "View<bool>" "View.Const false" "Prevent editing"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Attributes on the root element"

          Helpers.apiParam
            "?isOpen (field only)"
            "Var<bool>"
            "Internal"
            "Two-way open state; unavailable fields remain closed"
        ]

        Helpers.apiTable "ColorPicker.Controls (attrs)" [
          Helpers.apiParam
            "hideFormatTabs"
            "Attr"
            "Tabs visible"
            "Hide the Hex/RGB tabs and keep the initial mode; works on both variants"
        ]

        Helpers.apiTable "PickerColor" [

          Helpers.apiParam
            "rgb / rgba"
            "int -> int -> int [-> int] -> PickerColor"
            ""
            "Construct clamped byte channels (RGBA includes alpha)"

          Helpers.apiParam
            "ofHex / tryParseHex"
            "string -> PickerColor / PickerColor option"
            ""
            "Parse #RGB, #RGBA, #RRGGBB, #RRGGBBAA; # is optional"

          Helpers.apiParam
            "toHex / toCss"
            "PickerColor -> string"
            ""
            "#RRGGBB, or #RRGGBBAA for transparent values"

          Helpers.apiParam "withAlpha" "int -> PickerColor -> PickerColor" "" "Replace the alpha byte"
          Helpers.apiParam "Red / Green / Blue / Alpha" "int" "" "Read the color channels"
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Color Picker"

        Helpers.bodyText
          "Choose colors with a spectrum, hexadecimal or RGB inputs, and a palette. Built with Weave fields, sliders, buttons, and reactive values."

        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        embeddedExample ()
        Helpers.divider ()
        paletteExample ()
        Helpers.divider ()
        editableSwatchesExample ()
        Helpers.divider ()
        ColorPickerConfigurator.render ()
        Helpers.divider ()
        stateExample ()
        Helpers.divider ()
        apiReference ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
