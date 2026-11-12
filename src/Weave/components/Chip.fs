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
module Chip =

  module Variant =

    let filled = cl Css.``weave-chip--filled``
    let text = cl Css.``weave-chip--text``
    let outlined = cl Css.``weave-chip--outlined``

  module Density =

    let compact = cl Css.``weave-chip--compact``
    let standard = cl Css.``weave-chip--standard``
    let spacious = cl Css.``weave-chip--spacious``

  module Shape =

    let label = cl Css.``weave-chip--label``

  module Color =

    let primary = cl Css.``weave-chip--primary``
    let secondary = cl Css.``weave-chip--secondary``
    let tertiary = cl Css.``weave-chip--tertiary``
    let error = cl Css.``weave-chip--error``
    let warning = cl Css.``weave-chip--warning``
    let success = cl Css.``weave-chip--success``
    let info = cl Css.``weave-chip--info``

[<JavaScript; RequireQualifiedAccess>]
type Chip =

  static member create
    (
      label: Doc,
      ?onClick: unit -> unit,
      ?onClose: unit -> unit,
      ?closeIcon: Doc,
      ?content: Doc,
      ?href: string,
      ?selected: View<bool>,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =

    let enabled = defaultArg enabled (View.Const true)
    let selected = defaultArg selected (View.Const false)
    let attrs = defaultArg attrs List.empty

    let interactiveAttrs () = [
      Attr.Dynamic "aria-disabled" (enabled |> View.Map(not >> string<bool>))
      enabled
      |> View.Map(fun value -> if value then "0" else "-1")
      |> Attr.DynamicCustom(fun el value -> el.SetAttribute("tabindex", value))
    ]

    let leadingDoc =
      match content with
      | Some c -> span [ cl Css.``weave-chip__content`` ] [ c ]
      | None -> Doc.Empty

    let labelDoc = span [ cl Css.``weave-chip__label`` ] [ label ]

    let closeDoc =
      match onClose with
      | Some callback ->
        let closeIconDoc = defaultArg closeIcon (span [] [ text "\u00D7" ])

        span [
          cl Css.``weave-chip__close``
          Attr.Create "role" "button"
          yield! interactiveAttrs ()
          Attr.Create "aria-label" "remove"
          on.clickView enabled (fun _ ev isEnabled ->
            ev.StopPropagation()
            ev.PreventDefault()

            if isEnabled then
              callback ())
          on.keyDownView enabled (fun _ ev isEnabled ->
            match ev with
            | Key.Activate ->
              ev.PreventDefault()
              ev.StopPropagation()

              if isEnabled then
                callback ()
            | _ -> ())
        ] [ closeIconDoc ]
      | None -> Doc.Empty

    let isClickable = Option.isSome onClick || Option.isSome href

    let commonAttrs = [
      cl Css.``weave-chip``
      if isClickable then
        cl Css.``weave-chip--clickable``
      Attr.DynamicClassPred Css.``weave-chip--selected`` selected
      Disabled.disabledClass Css.``weave-chip--disabled`` enabled
      yield! attrs
    ]

    let children = [ leadingDoc; labelDoc; closeDoc ]

    match href with
    | Some url when Option.isSome onClose ->
      let onClickHandler = defaultArg onClick (fun () -> ())

      span [ yield! commonAttrs ] [
        a [
          cl "weave-chip__action"
          attr.href url
          yield! interactiveAttrs ()
          on.clickView enabled (fun _ ev isEnabled ->
            if not isEnabled then
              ev.PreventDefault()
            else
              onClickHandler ())
        ] [ leadingDoc; labelDoc ]
        closeDoc
      ]
    | Some url ->
      let onClickHandler = defaultArg onClick (fun () -> ())

      a
        [
          attr.href url
          yield! commonAttrs
          yield! interactiveAttrs ()
          on.clickView enabled (fun _ ev isEnabled ->
            if not isEnabled then
              ev.PreventDefault()
            else
              onClickHandler ())
        ]
        children
    | None ->
      match onClick with
      | Some handler when Option.isSome onClose ->
        span [ yield! commonAttrs ] [
          span [
            cl "weave-chip__action"
            Attr.Create "role" "button"
            yield! interactiveAttrs ()
            on.clickViewGuarded enabled handler
            on.keyDownView enabled (fun _ ev isEnabled ->
              match ev with
              | Key.Activate ->
                ev.PreventDefault()

                if isEnabled then
                  handler ()
              | _ -> ())
          ] [ leadingDoc; labelDoc ]
          closeDoc
        ]
      | Some handler ->
        let enabledRef: bool ref = ref true

        span
          [
            yield! commonAttrs
            Attr.Create "role" "button"
            on.afterRender (fun _ -> enabled |> View.Sink(fun v -> enabledRef.Value <- v))

            yield! interactiveAttrs ()

            on.clickViewGuarded enabled handler

            on.keyDown (fun _ ev ->
              match ev with
              | Key.Activate ->
                ev.PreventDefault()

                if enabledRef.Value then
                  handler ()
              | _ -> ())
          ]
          children
      | None -> span [ yield! commonAttrs ] children
