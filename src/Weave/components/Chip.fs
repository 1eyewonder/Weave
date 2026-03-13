namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Chip =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Filled
    | Text
    | Outlined

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Filled -> Css.``weave-chip--filled``
      | Variant.Text -> Css.``weave-chip--text``
      | Variant.Outlined -> Css.``weave-chip--outlined``

  module Density =

    let toClass density =
      match density with
      | Density.Compact -> Css.``weave-chip--compact``
      | Density.Standard -> Css.``weave-chip--standard``
      | Density.Spacious -> Css.``weave-chip--spacious``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-chip--primary``
      | BrandColor.Secondary -> Css.``weave-chip--secondary``
      | BrandColor.Tertiary -> Css.``weave-chip--tertiary``
      | BrandColor.Error -> Css.``weave-chip--error``
      | BrandColor.Warning -> Css.``weave-chip--warning``
      | BrandColor.Success -> Css.``weave-chip--success``
      | BrandColor.Info -> Css.``weave-chip--info``

open Chip

[<JavaScript>]
type Chip =

  static member Create
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
        let closeIconDoc = defaultArg closeIcon (span [] [ text "\u00D7" ])

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
