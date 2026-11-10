namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module ColorPicker =

  [<RequireQualifiedAccess; Struct>]
  type Mode =
    /// Start in Hex with opaque edits.
    | Hex
    /// Start in Hex and allow transparency.
    | HexWithAlpha
    /// Start in RGB with opaque edits.
    | RGB
    /// Start in RGB and allow transparency.
    | RGBA

  module Mode =
    let internal allowsAlpha =
      function
      | Mode.Hex
      | Mode.RGB -> false
      | Mode.HexWithAlpha
      | Mode.RGBA -> true

  module Controls =
    /// Hide format tabs, including from keyboard navigation, and keep the initial mode.
    let hideFormatTabs = cl Css.``weave-colorpicker--hide-format-tabs``

  let defaultPalette =
    [
      "#6200EA"
      "#D81B60"
      "#D32F2F"
      "#ED6C02"
      "#F9A825"
      "#2E7D32"
      "#00897B"
      "#0288D1"
      "#1565C0"
      "#FFFFFF"
      "#757575"
      "#000000"
    ]
    |> List.map PickerColor.ofHex

[<JavaScript; RequireQualifiedAccess>]
type ColorPicker =

  /// An embedded color picker. Hex and RGB modes produce opaque edits;
  /// caller-supplied values remain unchanged until the user edits them.
  static member create
    (
      value: Var<PickerColor>,
      ?labelText: View<string>,
      ?mode: ColorPicker.Mode,
      ?palette: View<PickerColor list>,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?attrs: Attr list
    ) =
    let labelText = defaultArg labelText (View.Const "Color")
    let mode = defaultArg mode ColorPicker.Mode.HexWithAlpha
    let allowsAlpha = mode |> ColorPicker.Mode.allowsAlpha
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)
    let state = value |> ColorPickerState.create allowsAlpha enabled readOnly

    let activeFormat =
      match mode with
      | ColorPicker.Mode.Hex
      | ColorPicker.Mode.HexWithAlpha -> Var.Create 0
      | ColorPicker.Mode.RGB
      | ColorPicker.Mode.RGBA -> Var.Create 1

    let swatches = defaultArg palette (View.Const ColorPicker.defaultPalette)

    let color = value.View |> View.Map(ColorPickerState.applyAlphaPolicy allowsAlpha)

    div [
      cl Css.``weave-colorpicker``
      Typography.body2
      Attr.Create "role" "group"
      Attr.DynamicCustom (fun element label -> element.SetAttribute("aria-label", label)) labelText
      Disabled.disabledClass Css.``weave-colorpicker--disabled`` enabled
      yield! defaultArg attrs []
    ] [
      ColorPickerState.synchronize state

      div [ cl Css.``weave-colorpicker__header`` ] [
        span [] [ Doc.TextView labelText ]
        ColorPickerRender.preview color
      ]

      ColorPickerRender.spectrum state
      ColorPickerRender.hueSlider state
      ColorPickerRender.alphaSlider state

      state |> ColorPickerRender.inputs activeFormat
      state |> ColorPickerRender.palette swatches
    ]

  /// A field-style button that opens a nonmodal picker. Changes apply immediately.
  /// Escape, the Done button, focus leaving the field, or an outside pointer closes it.
  static member field
    (
      value: Var<PickerColor>,
      ?labelText: View<string>,
      ?mode: ColorPicker.Mode,
      ?palette: View<PickerColor list>,
      ?enabled: View<bool>,
      ?readOnly: View<bool>,
      ?isOpen: Var<bool>,
      ?attrs: Attr list
    ) =
    let enabled = defaultArg enabled (View.Const true)
    let readOnly = defaultArg readOnly (View.Const false)

    let editable =
      (enabled, readOnly)
      ||> View.Map2(fun enabled readOnly -> enabled && not readOnly)

    let labelText = defaultArg labelText (View.Const "Color")
    let mode = defaultArg mode ColorPicker.Mode.HexWithAlpha
    let allowsAlpha = mode |> ColorPicker.Mode.allowsAlpha
    let openVar = defaultArg isOpen (Var.Create false)
    let root = ref (Unchecked.defaultof<Dom.Element>)
    let trigger = Var.Create<Dom.Element option> None
    let id = WeaveId.create "weave-colorpicker"

    let color = value.View |> View.Map(ColorPickerState.applyAlphaPolicy allowsAlpha)

    let openView = (editable, openVar.View) ||> View.Map2 (&&)

    let close () = openVar.Value <- false

    let closeAndRestoreFocus () =
      close ()

      trigger.Value |> Option.iter (fun element -> element?focus ())

    let closeOnEscape (_: Dom.Element) (event: Dom.KeyboardEvent) =
      if event.Key = "Escape" && openVar.Value then
        event.PreventDefault()
        event.StopPropagation()
        closeAndRestoreFocus ()

    let closeWhenFocusLeaves (element: Dom.Element) (event: Dom.FocusEvent) =
      let target: Dom.Node = event?relatedTarget

      if isNull target || not (element.Contains target) then
        close ()

    let closeWhenUnavailable (canEdit, requested) =
      if not canEdit && requested then
        close ()

    let renderPopover isOpen =
      if not isOpen then
        Doc.Empty
      else
        let picker =
          ColorPicker.create (
            value,
            labelText = labelText,
            mode = mode,
            ?palette = palette,
            enabled = enabled,
            readOnly = readOnly
          )

        picker |> ColorPickerPopover.render id labelText trigger closeAndRestoreFocus

    div [
      cl Css.``weave-colorpicker-field``
      on.afterRender (fun element -> root.Value <- element)
      on.keyDown closeOnEscape
      on.focusOut closeWhenFocusLeaves
      yield! defaultArg attrs []
    ] [
      View.zip editable openVar.View |> Doc.sinkCached closeWhenUnavailable
      openView |> DocumentEventListener.watch "pointerdown" [ root ] close

      Button.create (
        div [ cl Css.``weave-colorpicker-field__label`` ] [
          span [] [ Doc.TextView labelText ]
          ColorPickerRender.preview color
        ],
        (fun () -> openVar.Value <- not openVar.Value),
        enabled = editable,
        attrs = [
          Button.Variant.outlined
          Attr.Create "aria-haspopup" "dialog"
          Attr.Create "aria-controls" id

          Attr.DynamicCustom
            (fun element isOpen -> element.SetAttribute("aria-expanded", if isOpen then "true" else "false"))
            openView

          on.afterRender (fun element -> trigger.Value <- Some element)
        ]
      )

      openView |> Doc.BindView renderPopover
    ]
