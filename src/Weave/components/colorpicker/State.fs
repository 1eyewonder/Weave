namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client

[<JavaScript; RequireQualifiedAccess>]
module internal ColorPickerState =

  type State = {
    Value: Var<PickerColor>
    Enabled: View<bool>
    ReadOnly: View<bool>
    Editable: View<bool>
    AllowsAlpha: bool
    Hsv: Var<HsvColor>
    Hex: Var<string>
    InvalidHex: Var<bool>
  }

  let canEdit state =
    state.Editable |> View.TryGet |> Option.defaultValue false

  let applyAlphaPolicy allowsAlpha color =
    if allowsAlpha then
      color
    else
      color |> PickerColor.withAlpha 255

  let create allowsAlpha enabled readOnly (value: Var<PickerColor>) = {
    Value = value
    Enabled = enabled
    ReadOnly = readOnly

    Editable =
      (enabled, readOnly)
      ||> View.Map2(fun enabled readOnly -> enabled && not readOnly)

    AllowsAlpha = allowsAlpha
    Hsv = Var.Create(PickerColor.toHsv value.Value)
    Hex = Var.Create(value.Value |> applyAlphaPolicy allowsAlpha |> PickerColor.toHex)
    InvalidHex = Var.Create false
  }

  // Keep hue/saturation through achromatic colors and byte rounding.
  let trackColor state (color: PickerColor) =
    let current = state.Hsv.Value

    if (current |> PickerColor.ofHsv color.Alpha) <> color then
      let next = PickerColor.toHsv color

      state.Hsv.Value <- {
        Hue = if next.Saturation = 0. then current.Hue else next.Hue

        Saturation =
          if next.Brightness = 0. then
            current.Saturation
          else
            next.Saturation

        Brightness = next.Brightness
      }

  let setColor state color =
    if canEdit state then
      state.Value.Value |> trackColor state
      let color = color |> applyAlphaPolicy state.AllowsAlpha
      color |> trackColor state
      state.Value.Value <- color

  let setHsv state color =
    if canEdit state then
      state.Hsv.Value <- color

      state.Value.Value <-
        color
        |> PickerColor.ofHsv state.Value.Value.Alpha
        |> applyAlphaPolicy state.AllowsAlpha

  let hue state =
    let setHue current hue =
      { current with Hue = hue } |> setHsv state
      state.Hsv.Value

    state.Hsv.Lens (fun color -> color.Hue) setHue

  let commitHex state =
    if canEdit state then
      match PickerColor.tryParseHex state.Hex.Value with
      | Some color ->
        color |> setColor state
        state.Hex.Value <- color |> applyAlphaPolicy state.AllowsAlpha |> PickerColor.toHex
        state.InvalidHex.Value <- false
      | None -> state.InvalidHex.Value <- true

  let synchronize state =
    let synchronizeHex color =
      state.Hex.Value <- color |> applyAlphaPolicy state.AllowsAlpha |> PickerColor.toHex
      state.InvalidHex.Value <- false

    Doc.Concat [
      state.Value.View |> Doc.sinkCached (trackColor state)
      state.Value.View |> Doc.sinkCached synchronizeHex
    ]
