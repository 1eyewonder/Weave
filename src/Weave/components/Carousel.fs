namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript>]
module Carousel =

  module Controls =
    let hideArrows = cl Css.``weave-carousel--hide-arrows``
    let hideIndicators = cl Css.``weave-carousel--hide-indicators``

  module Transition =
    /// Disable the default crossfade.
    let none = cl Css.``weave-carousel--no-transition``

  /// Configuration for one slide. Content stays mounted when selection changes.
  type Item = {
    Content: Doc
    Label: string
    Attrs: Attr list
  }

[<JavaScript>]
type CarouselItem =

  /// A descriptive label identifies the slide to assistive technology.
  static member create(content: Doc, ?label: string, ?attrs: Attr list) : Carousel.Item = {
    Content = content
    Label = defaultArg label ""
    Attrs = defaultArg attrs []
  }

[<JavaScript>]
type Carousel =

  /// Creates a carousel. Omit autoCycleInterval to disable autoplay.
  /// Autoplay stops when slides or navigation receive focus. The interval is in
  /// milliseconds, with a minimum of 1000. Selection is clamped for display.
  static member create
    (
      items: View<Carousel.Item list>,
      ?activeIndex: Var<int>,
      ?autoCycleInterval: int,
      ?label: string,
      ?attrs: Attr list
    ) =
    let activeIndex = defaultArg activeIndex (Var.Create 0)
    let attrs = defaultArg attrs []
    let playing = Var.Create autoCycleInterval.IsSome
    let hovered = ref false
    let groupId = WeaveId.create "weave-carousel"
    let count = items |> View.Map List.length
    let selected = View.Map2 (fun n i -> max 0 (min (n - 1) i)) count activeIndex.View
    let state = View.zip count selected
    let rotating = View.Map2 (fun play n -> play && n > 1) playing.View count
    let stopOnFocus = on.focusIn (fun _ _ -> playing.Set false)

    let select n index =
      if n > 0 then
        activeIndex.Set((index + n) % n)

    let move delta =
      state |> View.Get(fun (n, i) -> select n (i + delta))

    let arrow forward =
      IconButton.create (
        Doc.SvgElement "svg" [
          Attr.Create "viewBox" "0 0 24 24"
          Attr.Create "aria-hidden" "true"
          Attr.Create "focusable" "false"
        ] [
          Doc.SvgElement "path" [
            Attr.Create "d" (if forward then "m9 5 7 7-7 7" else "m15 5-7 7 7 7")
            Attr.Create "fill" "none"
            Attr.Create "stroke" "currentColor"
            Attr.Create "stroke-width" "1.75"
            Attr.Create "stroke-linecap" "round"
            Attr.Create "stroke-linejoin" "round"
          ] []
        ],
        (fun () -> move (if forward then 1 else -1)),
        attrs = [
          cl Css.``weave-carousel__arrow``
          Attr.Create "aria-label" (if forward then "Next slide" else "Previous slide")
        ]
      )

    div [
      cl Css.``weave-carousel``
      Attr.Create "role" "region"
      Attr.Create "aria-roledescription" "carousel"
      Attr.Create "aria-label" (defaultArg label "Slides")
      match autoCycleInterval with
      | Some interval ->
        on.mouseEnter (fun _ _ -> hovered.Value <- true)
        on.mouseLeave (fun _ _ -> hovered.Value <- false)

        on.afterRender (fun el ->
          if JS.Window.MatchMedia("(prefers-reduced-motion: reduce)").Matches then
            playing.Set false

          let timer =
            JS.SetInterval
              (fun () ->
                state
                |> View.Get(fun (n, i) ->
                  if
                    playing.Value
                    && n > 1
                    && not hovered.Value
                    && not (As<bool> JS.Document?hidden)
                    && As<bool> el?isConnected
                  then
                    select n (i + 1)))
              (max 1000 interval)

          let removal = DomRemovalListener.create ()
          removal.Watch(el, fun () -> JS.ClearInterval timer))
      | None -> ()
      yield! attrs
    ] [
      div [
        cl Css.``weave-carousel__viewport``
        Attr.Create "id" groupId
        rotating
        |> View.Map(fun v -> if v then "off" else "polite")
        |> Attr.Dynamic "aria-live"
        Attr.Create "aria-atomic" "false"
        Attr.Create "tabindex" "0"
        stopOnFocus
        on.keyDownView state (fun el ev (n, i) ->
          // Preserve arrow-key behavior inside inputs and other slide controls.
          if obj.ReferenceEquals(ev.Target, el) then
            match ev.Key with
            | "ArrowLeft" ->
              ev.PreventDefault()
              select n (i - 1)
            | "ArrowRight" ->
              ev.PreventDefault()
              select n (i + 1)
            | "Home" ->
              ev.PreventDefault()
              select n 0
            | "End" ->
              ev.PreventDefault()
              select n (n - 1)
            | _ -> ())
      ] [
        items
        |> Doc.BindView(fun slides ->
          slides
          |> List.mapi (fun i slide ->
            let isActive = selected |> View.Map((=) i)

            div [
              cl Css.``weave-carousel__item``
              isActive |> Attr.DynamicClassPred Css.``weave-carousel__item--active``
              Attr.Create "role" "group"
              Attr.Create "aria-roledescription" "slide"

              if slide.Label = "" then
                sprintf "%d of %d" (i + 1) slides.Length |> Attr.Create "aria-label"
              else
                sprintf "%d of %d: %s" (i + 1) slides.Length slide.Label
                |> Attr.Create "aria-label"

              isActive |> View.Map(not >> string<bool>) |> Attr.Dynamic "aria-hidden"
              Attr.DynamicPred "inert" (View.not isActive) (View.Const "")
              yield! slide.Attrs
            ] [ slide.Content ])
          |> Doc.Concat)
      ]
      count
      |> Doc.BindView(fun n ->
        if n < 2 then
          Doc.Empty
        else
          div [ cl Css.``weave-carousel__controls`` ] [
            if autoCycleInterval.IsSome then
              Button.create (
                playing.View
                |> Doc.BindView(fun p -> text (if p then "Pause slides" else "Play slides")),
                (fun () -> playing.Set(not playing.Value)),
                attrs = [ Button.Variant.text; cl Css.``weave-carousel__rotation`` ]
              )

            div [ cl Css.``weave-carousel__navigation``; stopOnFocus ] [
              arrow false
              div [
                cl Css.``weave-carousel__indicators``
                Attr.Create "role" "group"
                Attr.Create "aria-label" "Choose slide"
              ] [
                for i in 0 .. n - 1 do
                  IconButton.create (
                    Doc.Empty,
                    (fun () -> count |> View.Get(fun currentCount -> select currentCount i)),
                    attrs = [
                      cl Css.``weave-carousel__indicator``
                      Attr.Create "aria-label" (sprintf "Go to slide %d" (i + 1))
                      Attr.Create "aria-controls" groupId
                      selected
                      |> View.Map(fun s -> string<bool>(s = i))
                      |> Attr.Dynamic "aria-current"
                    ]
                  )
              ]
              arrow true
            ]
          ])
    ]
