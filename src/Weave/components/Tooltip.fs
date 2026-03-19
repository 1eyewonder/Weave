namespace Weave

open WebSharper
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
    let tooltipId = WeaveId.create "weave-tooltip"

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
      Attr.Create "aria-describedby" tooltipId
      yield! rootClasses |> List.map cl
      yield! attrs

      yield!
        activationEvents
        |> List.collect (function
          | Tooltip.Activation.Hover -> [
              on.mouseEnter (fun _ _ -> Var.Set isVisible true)
              on.mouseLeave (fun _ _ -> Var.Set isVisible false)
            ]
          | Tooltip.Activation.Focus -> [
              on.focus (fun _ _ -> Var.Set isVisible true)
              on.blur (fun _ _ -> Var.Set isVisible false)
            ]
          | Tooltip.Activation.Click -> [ on.clickTap (fun _ _ -> Var.Set isVisible (not isVisible.Value)) ])
    ] [
      innerContent

      div [
        Attr.Create "id" tooltipId
        Attr.Create "role" "tooltip"
        yield! tooltipClasses |> List.map cl
        directionAttr
        yield! tooltipAttrs

        Attr.DynamicStyle
          "display"
          (isVisible.View |> View.Map(fun visible -> if visible then "block" else "none"))
      ] [ tooltipContent ]
    ]
