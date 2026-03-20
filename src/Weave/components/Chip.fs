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

    let leadingDoc =
      match content with
      | Some c -> span [ cl Css.``weave-chip__content`` ] [ c ]
      | None -> Doc.Empty

    let labelDoc = span [ cl Css.``weave-chip__label`` ] [ label ]

    let closeDoc =
      match onClose with
      | Some callback ->
        let closeIconDoc = defaultArg closeIcon (span [ Typography.h4 ] [ text "\u00D7" ])

        span [
          cl Css.``weave-chip__close``
          Attr.Create "tabindex" "0"
          Attr.Create "aria-label" "remove"
          on.clickTapViewGuarded enabled callback
          on.keyDown (fun _ ev ->
            if ev?key = "Enter" || ev?key = " " then
              ev.PreventDefault()
              callback ())
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
    | Some url ->
      let onClickHandler = defaultArg onClick (fun () -> ())

      a
        [
          attr.href url
          yield! commonAttrs
          on.clickTapView enabled (fun _ ev isEnabled ->
            if not isEnabled then
              ev.PreventDefault()
            else
              onClickHandler ())
        ]
        children
    | None ->
      match onClick with
      | Some handler -> span [ yield! commonAttrs; on.clickTapViewGuarded enabled handler ] children
      | None -> span [ yield! commonAttrs ] children
