namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Tooltip =

  [<RequireQualifiedAccess; Struct>]
  type Direction =
    | Top
    | Bottom
    | Left
    | Right

  module Direction =

    let toClass direction =
      match direction with
      | Direction.Top -> Css.``weave-tooltip--top-center``
      | Direction.Bottom -> Css.``weave-tooltip--bottom-center``
      | Direction.Left -> Css.``weave-tooltip--center-left``
      | Direction.Right -> Css.``weave-tooltip--center-right``

  [<RequireQualifiedAccess; Struct>]
  type Transition =
    | Delayed of time: int
    | Duration of time: int

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-tooltip--primary``
      | BrandColor.Secondary -> Css.``weave-tooltip--secondary``
      | BrandColor.Tertiary -> Css.``weave-tooltip--tertiary``
      | BrandColor.Error -> Css.``weave-tooltip--error``
      | BrandColor.Warning -> Css.``weave-tooltip--warning``
      | BrandColor.Success -> Css.``weave-tooltip--success``
      | BrandColor.Info -> Css.``weave-tooltip--info``

  [<Struct>]
  type Activation =
    | Hover
    | Focus
    | Click

open Tooltip

[<JavaScript>]
type Tooltip =

  static member Create
    (
      innerContent: Doc,
      tooltipContent: Doc,
      ?activationEvents: Activation list,
      ?direction: Direction,
      ?showArrow: bool,
      ?tooltipAttrs: Attr list,
      ?wrapperAttrs: Attr list
    ) =
    let direction = defaultArg direction Direction.Top
    let showArrow = defaultArg showArrow true
    let attrs = defaultArg wrapperAttrs List.empty
    let activationEvents = defaultArg activationEvents [ Hover; Focus ]

    let tooltipAttrs = tooltipAttrs |> Option.defaultValue []

    let isVisible = Var.Create false

    let tooltipClasses = [
      Css.``weave-tooltip``
      Css.``weave-tooltip--default``

      Direction.toClass direction

      if showArrow then
        Css.``weave-tooltip--arrow``
    ]

    let rootClasses = [ Css.``weave-tooltip-root`` ]

    div [
      AlignItems.toClass AlignItems.Center |> cl
      AlignContent.toClass AlignContent.Center |> cl
      yield! rootClasses |> List.map cl
      yield! attrs

      yield!
        activationEvents
        |> List.collect (function
          | Hover -> [
              on.mouseEnter (fun _ _ -> Var.Set isVisible true)
              on.mouseLeave (fun _ _ -> Var.Set isVisible false)
            ]
          | Focus -> [
              on.focus (fun _ _ -> Var.Set isVisible true)
              on.blur (fun _ _ -> Var.Set isVisible false)
            ]
          | Click -> [ on.click (fun _ _ -> Var.Set isVisible (not isVisible.Value)) ])
    ] [
      innerContent

      div [
        yield! tooltipClasses |> List.map cl
        yield! tooltipAttrs

        Attr.DynamicStyle
          "display"
          (isVisible.View |> View.Map(fun visible -> if visible then "block" else "none"))
      ] [ tooltipContent ]
    ]
