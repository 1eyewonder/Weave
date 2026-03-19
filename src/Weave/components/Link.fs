namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Link =

  module Underline =

    let onHover = cl Css.``weave-link--underline-hover``
    let always = cl Css.``weave-link--underline-always``
    let none = cl Css.``weave-link--underline-none``

  module Color =

    let primary = cl Css.``weave-link--primary``
    let secondary = cl Css.``weave-link--secondary``
    let tertiary = cl Css.``weave-link--tertiary``
    let error = cl Css.``weave-link--error``
    let warning = cl Css.``weave-link--warning``
    let success = cl Css.``weave-link--success``
    let info = cl Css.``weave-link--info``

[<JavaScript; RequireQualifiedAccess>]
type Link =

  /// <summary>
  /// Creates a link with optional text label and start/end icon adornments.
  /// The underline decoration is applied only to the text portion, never to icons.
  /// Pass <c>Link.Underline.*</c> via <c>?attrs</c> to control decoration.
  /// </summary>
  static member create
    (
      innerContents: Doc,
      ?href: string,
      ?enabled: View<bool>,
      ?onClick: unit -> unit,
      ?startIcon: Doc,
      ?endIcon: Doc,
      ?attrs: Attr list
    ) =

    let href = defaultArg href "#"
    let enabled = defaultArg enabled (View.Const true)
    let onClick = defaultArg onClick (fun () -> ())
    let attrs = defaultArg attrs List.empty

    let startIconDoc =
      match startIcon with
      | Some icon -> Html.span [ cl Css.``weave-link__start-icon`` ] [ icon ]
      | None -> Doc.Empty

    let endIconDoc =
      match endIcon with
      | Some icon -> Html.span [ cl Css.``weave-link__end-icon`` ] [ icon ]
      | None -> Doc.Empty

    let textLabel = Html.span [ cl Css.``weave-link__text`` ] [ innerContents ]

    a [
      cl Css.``weave-link``

      attr.href href

      Disabled.disabledClass Css.``weave-link--disabled`` enabled

      // Prevent default navigation when disabled; fire onClick only when enabled
      on.clickTapView enabled (fun _ ev isEnabled ->
        if not isEnabled then ev.PreventDefault() else onClick ())

      yield! attrs
    ] [ startIconDoc; textLabel; endIconDoc ]

[<JavaScript; RequireQualifiedAccess>]
type IconLink =

  /// <summary>
  /// Creates a link containing only an icon — no text label and no underline.
  /// </summary>
  static member create
    (icon: Doc, ?href: string, ?enabled: View<bool>, ?onClick: unit -> unit, ?attrs: Attr list)
    =

    let href = defaultArg href "#"
    let enabled = defaultArg enabled (View.Const true)
    let onClick = defaultArg onClick (fun () -> ())
    let attrs = defaultArg attrs List.empty

    a [
      cls [ Css.``weave-link``; Css.``weave-link--icon`` ]

      attr.href href

      Disabled.disabledClass Css.``weave-link--disabled`` enabled

      on.clickTapView enabled (fun _ ev isEnabled ->
        if not isEnabled then ev.PreventDefault() else onClick ())

      yield! attrs
    ] [ icon ]
