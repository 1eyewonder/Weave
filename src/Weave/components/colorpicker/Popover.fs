namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module internal ColorPickerPopover =

  type Bounds = {
    Left: float
    Top: float
    Bottom: float
  }

  type Size = {
    Width: float
    Height: float
  }

  type Placement = {
    Left: float
    Top: float
  }

  let calculatePlacement (anchor: Bounds) (panel: Size) (viewport: Size) =
    let left =
      anchor.Left |> Bounded.clamp 16. (max 16. (viewport.Width - panel.Width - 16.))

    let top =
      if viewport.Width <= 600. then
        max 16. (viewport.Height - panel.Height - 16.)
      elif anchor.Bottom + panel.Height + 8. <= viewport.Height - 16. then
        anchor.Bottom + 8.
      elif anchor.Top - panel.Height - 8. >= 16. then
        anchor.Top - panel.Height - 8.
      else
        anchor.Bottom + 8.
        |> Bounded.clamp 16. (max 16. (viewport.Height - panel.Height - 16.))

    {
      Left = left
      Top = top
    }

  let private position (trigger: Dom.Element) (panel: Dom.Element) =
    let bounds = trigger?getBoundingClientRect ()

    let anchor = {
      Left = bounds?left
      Top = bounds?top
      Bottom = bounds?bottom
    }

    let size = {
      Width = panel?offsetWidth
      Height = panel?offsetHeight
    }

    let viewport = {
      Width = JS.Window?innerWidth
      Height = JS.Window?innerHeight
    }

    let placement = calculatePlacement anchor size viewport
    panel?style?left <- sprintf "%.2fpx" placement.Left
    panel?style?top <- sprintf "%.2fpx" placement.Top

  let initialize (trigger: Var<Dom.Element option>) (panel: Dom.Element) =
    let updatePosition () =
      trigger.Value |> Option.iter (fun element -> position element panel)

    let reposition = System.Action<Dom.Event>(fun _ -> updatePosition ())
    let stopResize = panel |> ResizeListener.watch updatePosition
    let removal = DomRemovalListener.create ()

    let dispose () =
      JS.Window.RemoveEventListener("scroll", reposition, true)
      stopResize ()

    updatePosition ()
    JS.Window.AddEventListener("scroll", reposition, true)
    removal.Watch(panel, dispose)
    let focusable = panel.QuerySelector("[tabindex='0']")

    if not (isNull focusable) then
      focusable?focus ()

  let render id labelText trigger close picker =
    div [
      cl Css.``weave-colorpicker-field__popover``
      attr.id id
      Attr.Create "role" "dialog"
      Attr.DynamicCustom (fun element label -> element.SetAttribute("aria-label", label)) labelText
      on.afterRender (initialize trigger)
    ] [
      div [ cl Css.``weave-colorpicker-field__body`` ] [ picker ]

      div [ cl Css.``weave-colorpicker-field__footer`` ] [
        Button.primary (text "Done", close, attrs = [ Button.Variant.filled ])
      ]
    ]
