namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module internal ColorPickerRender =

  let preview color =
    span [ cl Css.``weave-colorpicker__preview``; Attr.Create "aria-hidden" "true" ] [
      span [ Attr.DynamicStyle "background-color" (color |> View.Map PickerColor.toCss) ] []
    ]

  let spectrum (state: ColorPickerState.State) =
    let dragging = Var.Create false

    let updateFromPointer (element: Dom.Element) clientX clientY =
      let bounds = element?getBoundingClientRect ()
      let width: float = bounds?width
      let height: float = bounds?height

      if width > 0. && height > 0. then
        {
          state.Hsv.Value with
              Saturation = (clientX - (bounds?left: float)) / width |> Bounded.clamp 0. 1.
              Brightness = 1. - ((clientY - (bounds?top: float)) / height |> Bounded.clamp 0. 1.)
        }
        |> ColorPickerState.setHsv state

    let adjustWithKeyboard (_: Dom.Element) (event: Dom.KeyboardEvent) canInteract =
      if canInteract then
        let current = state.Hsv.Value
        let step = if event.ShiftKey then 0.1 else 0.01

        let next =
          match event with
          | Key.ArrowRight -> Some { current with Saturation = current.Saturation + step }
          | Key.ArrowLeft -> Some { current with Saturation = current.Saturation - step }
          | Key.ArrowUp -> Some { current with Brightness = current.Brightness + step }
          | Key.ArrowDown -> Some { current with Brightness = current.Brightness - step }
          | _ -> None

        let apply color =
          event.PreventDefault()

          {
            color with
                Saturation = color.Saturation |> Bounded.clamp 0. 1.
                Brightness = color.Brightness |> Bounded.clamp 0. 1.
          }
          |> ColorPickerState.setHsv state

        next |> Option.iter apply

    let describe color =
      sprintf
        "Saturation %.0f%%, brightness %.0f%%. Use arrow keys to adjust; Shift for larger steps."
        (color.Saturation * 100.)
        (color.Brightness * 100.)

    let background color =
      {
        color with
            Saturation = 1.
            Brightness = 1.
      }
      |> PickerColor.ofHsv 255
      |> PickerColor.toCss

    let beginDrag (element: Dom.Element) (event: Dom.MouseEvent) =
      if ColorPickerState.canEdit state && event.Button = 0 then
        event.PreventDefault()
        element?focus ()
        element?setPointerCapture (event?pointerId)
        dragging.Value <- true
        updateFromPointer element (float event.ClientX) (float event.ClientY)

    let continueDrag (element: Dom.Element) (event: Dom.MouseEvent) =
      if ColorPickerState.canEdit state && dragging.Value then
        updateFromPointer element (float event.ClientX) (float event.ClientY)

    div [
      cl Css.``weave-colorpicker__spectrum``
      Attr.Create "role" "group"

      Attr.DynamicCustom
        (fun element enabled -> element.SetAttribute("tabindex", if enabled then "0" else "-1"))
        state.Editable

      Attr.DynamicCustom
        (fun element enabled -> element.SetAttribute("aria-disabled", if enabled then "false" else "true"))
        state.Editable

      Attr.DynamicCustom
        (fun element label -> element.SetAttribute("aria-label", label))
        (state.Hsv.View |> View.Map describe)

      Attr.DynamicStyle "background-color" (state.Hsv.View |> View.Map background)

      on.pointerDown beginDrag
      on.pointerMove continueDrag

      on.pointerUp (fun _ _ -> dragging.Value <- false)
      on.pointerCancel (fun _ _ -> dragging.Value <- false)
      on.keyDownView state.Editable adjustWithKeyboard
    ] [
      span [
        cl Css.``weave-colorpicker__cursor``

        Attr.DynamicStyle
          "left"
          (state.Hsv.View
           |> View.Map(fun color -> sprintf "%.4f%%" (color.Saturation * 100.)))

        Attr.DynamicStyle
          "top"
          (state.Hsv.View
           |> View.Map(fun color -> sprintf "%.4f%%" ((1. - color.Brightness) * 100.)))
      ] []
    ]

  let private channel label getter setter (state: ColorPickerState.State) =
    let update color value =
      if ColorPickerState.canEdit state then
        color |> setter value |> ColorPickerState.applyAlphaPolicy state.AllowsAlpha
      else
        color

    NumericField.create (
      state.Value.Lens getter update,
      labelText = View.Const label,
      min = 0,
      max = 255,
      enabled = state.Enabled,
      readOnly = state.ReadOnly,
      showSpinButtons = View.Const false,
      attrs = [ NumericField.Variant.outlined; NumericField.Width.full ]
    )

  let private hexInput (state: ColorPickerState.State) =
    let commitOnEnter (_: Dom.Element) (event: Dom.KeyboardEvent) =
      if event.Key = "Enter" then
        event.PreventDefault()
        ColorPickerState.commitHex state

    let validationMessage invalid =
      if invalid then
        p [ Typography.caption; Typography.Color.error; Attr.Create "role" "status" ] [
          text "Enter a hex color, such as #6200EA."
        ]
      else
        Doc.Empty

    div [ cl Css.``weave-colorpicker__inputs`` ] [
      TextField.singleLine (
        state.Hex,
        labelText = View.Const "Hex",
        enabled = state.Enabled,
        readOnly = state.ReadOnly,
        inputAttrs = [
          Attr.Create "spellcheck" "false"

          Attr.DynamicCustom
            (fun element invalid -> element.SetAttribute("aria-invalid", if invalid then "true" else "false"))
            state.InvalidHex.View

          on.change (fun _ _ -> ColorPickerState.commitHex state)
          on.keyDown commitOnEnter
        ],
        attrs = [ TextField.Variant.outlined; TextField.Width.full ]
      )

      state.InvalidHex.View |> Doc.BindView validationMessage
    ]

  let private rgbInputs (state: ColorPickerState.State) =
    div [ cl Css.``weave-colorpicker__channels`` ] [
      state |> channel "R" (fun color -> color.Red) PickerColor.withRed
      state |> channel "G" (fun color -> color.Green) PickerColor.withGreen
      state |> channel "B" (fun color -> color.Blue) PickerColor.withBlue

      if state.AllowsAlpha then
        state |> channel "A" (fun color -> color.Alpha) PickerColor.withAlpha
    ]

  let inputs activeFormat state =
    Tabs.create (
      View.Const [
        TabItem.create ("Hex", hexInput state, disabled = View.not state.Enabled)
        TabItem.create ("RGB", rgbInputs state, disabled = View.not state.Enabled)
      ],
      activeIndex = activeFormat,
      attrs = [ cl Css.``weave-colorpicker__formats`` ]
    )

  let hueSlider state =
    Slider.create (
      ColorPickerState.hue state,
      min = 0.,
      max = 360.,
      step = 1.,
      labelText = View.Const "Hue",
      enabled = state.Editable,
      attrs = [ cl Css.``weave-colorpicker__hue`` ]
    )

  let alphaSlider (state: ColorPickerState.State) =
    if not state.AllowsAlpha then
      Doc.Empty
    else
      let setAlpha color alpha =
        if ColorPickerState.canEdit state then
          color |> PickerColor.withAlpha alpha
        else
          color

      Slider.create (
        state.Value.Lens (fun color -> color.Alpha) setAlpha,
        min = 0,
        max = 255,
        labelText = View.Const "Opacity",
        enabled = state.Editable,
        attrs = [
          cl Css.``weave-colorpicker__alpha``

          Attr.DynamicStyle
            "--colorpicker-opaque"
            (state.Value.View |> View.Map(PickerColor.withAlpha 255 >> PickerColor.toCss))
        ]
      )

  let private swatch (state: ColorPickerState.State) color =
    let color = color |> ColorPickerState.applyAlphaPolicy state.AllowsAlpha

    let selected =
      state.Value.View
      |> View.Map(ColorPickerState.applyAlphaPolicy state.AllowsAlpha >> (=) color)

    button [
      attr.``type`` "button"
      cl Css.``weave-colorpicker__swatch``
      Attr.Create "aria-label" (PickerColor.toHex color)
      Attr.Create "title" (PickerColor.toHex color)

      Attr.DynamicCustom
        (fun element selected -> element.SetAttribute("aria-pressed", if selected then "true" else "false"))
        selected

      Attr.enabled state.Editable
      on.clickViewGuarded state.Editable (fun () -> color |> ColorPickerState.setColor state)
    ] [ span [ Attr.Style "background-color" (PickerColor.toCss color) ] [] ]

  let palette colors state =
    let render colors =
      if List.isEmpty colors then
        Doc.Empty
      else
        div [
          cl Css.``weave-colorpicker__palette``
          Attr.Create "role" "group"
          Attr.Create "aria-label" "Color palette"
        ] [ for color in colors -> swatch state color ]

    colors |> Doc.BindView render
