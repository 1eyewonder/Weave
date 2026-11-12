namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Tooltip =

  [<RequireQualifiedAccess; Struct>]
  type Direction =
    | Top
    | Bottom
    | Left
    | Right

  module Direction =

    let top = cl Css.``weave-tooltip--top-center``
    let bottom = cl Css.``weave-tooltip--bottom-center``
    let left = cl Css.``weave-tooltip--center-left``
    let right = cl Css.``weave-tooltip--center-right``

  [<RequireQualifiedAccess; Struct>]
  type Transition =
    | Delayed of time: int
    | Duration of time: int

  module Color =

    let primary = cl Css.``weave-tooltip--primary``
    let secondary = cl Css.``weave-tooltip--secondary``
    let tertiary = cl Css.``weave-tooltip--tertiary``
    let error = cl Css.``weave-tooltip--error``
    let warning = cl Css.``weave-tooltip--warning``
    let success = cl Css.``weave-tooltip--success``
    let info = cl Css.``weave-tooltip--info``

  [<RequireQualifiedAccess; Struct>]
  type Activation =
    | Hover
    | Focus
    | Click

[<JavaScript>]
module private TooltipPlacement =

  [<Inline "$element.classList.toggle($className, $visible)">]
  let setVisible (element: Dom.Element) (className: string) (visible: bool) = X<unit>

  [<Inline "$element.style.setProperty($name, $value)">]
  let private setProperty (element: Dom.Element) (name: string) (value: string) = X<unit>

  type Bounds = {
    Left: float
    Top: float
    Right: float
    Bottom: float
    Width: float
    Height: float
  }

  let private bounds (element: Dom.Element) =
    let rect = element?getBoundingClientRect ()

    {
      Left = rect?left
      Top = rect?top
      Right = rect?right
      Bottom = rect?bottom
      Width = rect?width
      Height = rect?height
    }

  let private overflow direction width height rect =
    match direction with
    | Tooltip.Direction.Left
    | Tooltip.Direction.Right -> max 0. (8. - rect.Left) + max 0. (rect.Right - width + 8.)
    | Tooltip.Direction.Top
    | Tooltip.Direction.Bottom -> max 0. (8. - rect.Top) + max 0. (rect.Bottom - height + 8.)

  let private position direction (popup: Dom.Element) =
    let owner: Dom.Element = popup?parentElement
    let width: float = JS.Document?documentElement?clientWidth
    let height: float = JS.Document?documentElement?clientHeight

    let setLength name value =
      setProperty popup name (sprintf "%.2fpx" value)

    setLength "--tooltip-viewport-width" width
    setLength "--tooltip-viewport-height" height
    setLength "--tooltip-shift-x" 0.
    setLength "--tooltip-shift-y" 0.
    popup.RemoveAttribute("data-popover-flip")

    let preferred = bounds popup
    let preferredOverflow = overflow direction width height preferred

    if preferredOverflow > 0. then
      popup.SetAttribute("data-popover-flip", "")
      let opposite = bounds popup

      if overflow direction width height opposite >= preferredOverflow then
        popup.RemoveAttribute("data-popover-flip")

    let panel = bounds popup
    let anchor = bounds owner
    let left = panel.Left |> Bounded.clamp 8. (max 8. (width - panel.Width - 8.))
    let top = panel.Top |> Bounded.clamp 8. (max 8. (height - panel.Height - 8.))
    setLength "--tooltip-shift-x" (left - panel.Left)
    setLength "--tooltip-shift-y" (top - panel.Top)

    setLength
      "--tooltip-arrow-x"
      ((anchor.Left + anchor.Width / 2. - left)
       |> Bounded.clamp 8. (max 8. (panel.Width - 8.)))

    setLength
      "--tooltip-arrow-y"
      ((anchor.Top + anchor.Height / 2. - top)
       |> Bounded.clamp 8. (max 8. (panel.Height - 8.)))

    let content = popup.QuerySelector(".weave-tooltip__content")
    let contentHeight: float = content?clientHeight
    let contentWidth: float = content?clientWidth
    let scrollHeight: float = content?scrollHeight
    let scrollWidth: float = content?scrollWidth

    if scrollHeight > contentHeight + 0.5 || scrollWidth > contentWidth + 0.5 then
      content.SetAttribute("tabindex", "0")
      content.SetAttribute("role", "region")
      content.SetAttribute("aria-label", "Tooltip content")
    else
      content.RemoveAttribute("tabindex")
      content.RemoveAttribute("role")
      content.RemoveAttribute("aria-label")

  let watch direction (popup: Dom.Element) =
    let owner: Dom.Element = popup?parentElement
    let mutable active = true

    let update () =
      if active && As<bool>(popup?isConnected) then
        position direction popup

    update ()
    let stopOwnerResize = owner |> ResizeListener.watch update
    let stopPopupResize = popup |> ResizeListener.watch update
    let scroll = System.Action<Dom.Event>(fun _ -> update ())
    let removal = DomRemovalListener.create ()

    let dispose () =
      if active then
        active <- false
        JS.Window.RemoveEventListener("scroll", scroll, true)
        stopOwnerResize ()
        stopPopupResize ()
        removal.Disconnect()

    JS.Window.AddEventListener("scroll", scroll, true)
    removal.Watch(popup, dispose)
    dispose

[<JavaScript; RequireQualifiedAccess>]
type Tooltip =

  static member create
    (
      innerContent: Doc,
      tooltipContent: Doc,
      ?activationEvents: Tooltip.Activation list,
      ?direction: Tooltip.Direction,
      ?showArrow: bool,
      ?tooltipAttrs: Attr list,
      ?wrapperAttrs: Attr list
    ) =
    let direction = defaultArg direction Tooltip.Direction.Top
    let showArrow = defaultArg showArrow true
    let attrs = defaultArg wrapperAttrs List.empty

    let activationEvents =
      defaultArg activationEvents [ Tooltip.Activation.Hover; Tooltip.Activation.Focus ]

    let tooltipAttrs = tooltipAttrs |> Option.defaultValue []

    let isVisible = Var.Create false
    let popupRef = ref (None: Dom.Element option)
    let mutable stopPositioning = ignore

    let updateVisibility (popup: Dom.Element) visible =
      stopPositioning ()
      stopPositioning <- ignore
      TooltipPlacement.setVisible popup Css.``weave-tooltip--visible`` visible

      if visible then
        stopPositioning <- TooltipPlacement.watch direction popup

    let tooltipId = WeaveId.create "weave-tooltip"

    let effectiveId (popup: Dom.Element) =
      let id = popup.GetAttribute("id")

      if System.String.IsNullOrWhiteSpace id then
        popup.SetAttribute("id", tooltipId)
        tooltipId
      else
        id

    let describe descriptionId (element: Dom.Element) =
      let raw = element.GetAttribute("aria-describedby")
      let existing = if isNull raw then "" else raw

      let tokens =
        existing.Replace("\t", " ").Replace("\r", " ").Replace("\n", " ").Replace("\u000C", " ")

      if not ((" " + tokens + " ").Contains(" " + descriptionId + " ")) then
        let value =
          if existing = "" then
            descriptionId
          else
            existing + " " + descriptionId

        element.SetAttribute("aria-describedby", value)

    let describeTriggers (popup: Dom.Element) =
      let owner = As<Dom.Element>(popup?parentElement)
      let descriptionId = effectiveId popup
      describe descriptionId owner

      let targets =
        owner.QuerySelectorAll("a[href], button, input, select, textarea, summary, [tabindex]")

      for index in 0 .. targets.Length - 1 do
        let target = As<Dom.Element>(targets.Item index)

        if not (popup.Contains target) then
          describe descriptionId target

    let directionAttr =
      match direction with
      | Tooltip.Direction.Top -> Tooltip.Direction.top
      | Tooltip.Direction.Bottom -> Tooltip.Direction.bottom
      | Tooltip.Direction.Left -> Tooltip.Direction.left
      | Tooltip.Direction.Right -> Tooltip.Direction.right

    let tooltipClasses = [
      Css.``weave-tooltip``
      Css.``weave-tooltip--default``
      if showArrow then
        Css.``weave-tooltip--arrow``
    ]

    let rootClasses = [ Css.``weave-tooltip-root`` ]

    div [
      AlignItems.center
      AlignContent.center
      yield! rootClasses |> List.map cl
      yield! attrs

      on.focusIn (fun owner ev ->
        let target = As<Dom.Element>(ev.Target)

        match popupRef.Value with
        | Some popup when not (popup.Contains target) -> describe (effectiveId popup) target
        | _ -> ())

      on.keyDown (fun owner ev ->
        match ev with
        | Key.Escape when isVisible.Value ->
          ev.PreventDefault()
          ev.StopPropagation()
          let active = As<Dom.Element>(JS.Document?activeElement)

          match popupRef.Value with
          | Some popup when popup.Contains active ->
            FocusNavigation.visibleElements owner
            |> Array.tryFind (fun element -> not (popup.Contains element))
            |> Option.iter (fun element -> element?focus ())
          | _ -> ()

          Var.Set isVisible false
        | _ -> ())

      yield!
        activationEvents
        |> List.collect (function
          | Tooltip.Activation.Hover -> [
              on.mouseEnter (fun _ _ -> Var.Set isVisible true)
              on.mouseLeave (fun _ _ -> Var.Set isVisible false)
            ]
          | Tooltip.Activation.Focus -> [
              on.focusIn (fun _ _ ->
                if not isVisible.Value then
                  Var.Set isVisible true)
              on.focusOut (fun owner ev ->
                let next = As<Dom.Element>(ev?relatedTarget)

                if isNull next || not (owner.Contains next) then
                  Var.Set isVisible false)
            ]
          | Tooltip.Activation.Click -> [ on.click (fun _ _ -> Var.Set isVisible (not isVisible.Value)) ])
    ] [
      innerContent

      div [
        Attr.Create "id" tooltipId
        Attr.Create "role" "tooltip"
        yield! tooltipClasses |> List.map cl
        directionAttr
        yield! tooltipAttrs
        on.afterRender (fun popup ->
          popupRef.Value <- Some popup
          describeTriggers popup)

        isVisible.View |> Attr.DynamicCustom updateVisibility
      ] [ div [ cl Css.``weave-tooltip__content`` ] [ tooltipContent ] ]
    ]
