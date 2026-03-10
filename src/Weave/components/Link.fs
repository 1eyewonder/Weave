namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Link =

  [<RequireQualifiedAccess; Struct>]
  type Underline =
    | OnHover
    | Always
    | None

  module Underline =

    let toClass underline =
      match underline with
      | Underline.OnHover -> Css.``weave-link--underline-hover``
      | Underline.Always -> Css.``weave-link--underline-always``
      | Underline.None -> Css.``weave-link--underline-none``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-link--primary``
      | BrandColor.Secondary -> Css.``weave-link--secondary``
      | BrandColor.Tertiary -> Css.``weave-link--tertiary``
      | BrandColor.Error -> Css.``weave-link--error``
      | BrandColor.Warning -> Css.``weave-link--warning``
      | BrandColor.Success -> Css.``weave-link--success``
      | BrandColor.Info -> Css.``weave-link--info``

open Link

[<JavaScript>]
type Link =

  /// <summary>
  /// Creates a link with optional text label and start/end icon adornments.
  /// The underline decoration is applied only to the text portion, never to icons.
  /// </summary>
  static member Create
    (
      innerContents: Doc,
      ?href: string,
      ?underline: Underline,
      ?enabled: View<bool>,
      ?onClick: unit -> unit,
      ?startIcon: Doc,
      ?endIcon: Doc,
      ?attrs: Attr list
    ) =

    let href = defaultArg href "#"
    let underline = defaultArg underline Underline.OnHover
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
      cls [ Css.``weave-link``; Underline.toClass underline ]

      attr.href href

      Disabled.disabledClass Css.``weave-link--disabled`` enabled

      // Prevent default navigation when disabled; fire onClick only when enabled
      on.clickTapView enabled (fun _ ev isEnabled ->
        if not isEnabled then ev.PreventDefault() else onClick ())

      yield! attrs
    ] [ startIconDoc; textLabel; endIconDoc ]

  /// <summary>
  /// Creates a link containing only an icon — no text label and no underline.
  /// </summary>
  static member CreateIcon
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
