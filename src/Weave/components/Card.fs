namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module Card =

  module Variant =

    let elevated = cl Css.``weave-card--elevated``
    let filled = cl Css.``weave-card--filled``

[<JavaScript>]
type CardHeader =

  /// <summary>Creates a card header with a title and optional leading content, subtitle, and trailing actions.</summary>
  /// <param name="title">The Doc rendered as the header title. Pass a heading element, such as h3, to include the title in the page outline.</param>
  /// <param name="subtitle">Optional Doc rendered beneath the title in secondary text.</param>
  /// <param name="leading">Optional Doc rendered before the title, such as an avatar or icon.</param>
  /// <param name="actions">Optional Doc rendered at the trailing edge of the header, such as icon buttons.</param>
  /// <param name="attrs">Additional attributes merged onto the header element.</param>
  static member create(title: Doc, ?subtitle: Doc, ?leading: Doc, ?actions: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty

    let leadingDoc =
      match leading with
      | Some l -> div [ cl Css.``weave-card__header-leading`` ] [ l ]
      | None -> Doc.Empty

    let subtitleDoc =
      match subtitle with
      | Some s -> div [ Typography.body2; cl Css.``weave-card__subtitle`` ] [ s ]
      | None -> Doc.Empty

    let actionsDoc =
      match actions with
      | Some a -> div [ cl Css.``weave-card__header-actions`` ] [ a ]
      | None -> Doc.Empty

    div [ cl Css.``weave-card__header``; yield! attrs ] [
      leadingDoc
      div [ cl Css.``weave-card__header-text`` ] [
        div [ Typography.h6; cl Css.``weave-card__title`` ] [ title ]
        subtitleDoc
      ]
      actionsDoc
    ]

[<JavaScript>]
type CardMedia =

  /// <summary>Creates an edge-to-edge media region, such as an image or video, with no padding.</summary>
  /// <param name="content">The media element, which is stretched to the region's width.</param>
  /// <param name="attrs">Additional attributes merged onto the media element, such as a fixed height.</param>
  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty
    div [ cl Css.``weave-card__media``; yield! attrs ] [ content ]

[<JavaScript>]
type CardContent =

  /// <summary>Creates the main content region of a card.</summary>
  /// <param name="content">The Doc rendered inside the content region.</param>
  /// <param name="attrs">Additional attributes merged onto the content element.</param>
  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty
    div [ cl Css.``weave-card__content``; yield! attrs ] [ content ]

[<JavaScript>]
type CardActionArea =

  /// <summary>
  /// Makes the wrapped sections one clickable target for the card's primary action, rendered as a link when
  /// <c>href</c> is supplied and as a button otherwise. Keep other interactive controls, such as
  /// <c>CardActions</c>, outside the area.
  /// </summary>
  /// <param name="content">The card sections the area covers, such as CardMedia, CardHeader, and CardContent.</param>
  /// <param name="onClick">Callback invoked when the area is activated.</param>
  /// <param name="href">When supplied, the area is a link to this address.</param>
  /// <param name="attrs">Additional attributes merged onto the area element.</param>
  static member create(content: Doc, ?onClick: unit -> unit, ?href: string, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty
    let onClick = defaultArg onClick ignore

    match href with
    | Some url ->
      a [
        cl Css.``weave-card__action-area``
        attr.href url
        on.click (fun _ _ -> onClick ())
        yield! attrs
      ] [ content ]
    | None ->
      button [
        attr.``type`` "button"
        cl Css.``weave-card__action-area``
        on.click (fun _ _ -> onClick ())
        yield! attrs
      ] [ content ]

[<JavaScript>]
type CardActions =

  /// <summary>Creates a footer row of card actions, aligned to the trailing edge.</summary>
  /// <param name="content">The Doc rendered inside the actions row, typically buttons.</param>
  /// <param name="attrs">Additional attributes merged onto the actions element.</param>
  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty
    div [ cl Css.``weave-card__actions``; yield! attrs ] [ content ]

[<JavaScript; RequireQualifiedAccess>]
type Card =

  /// <summary>
  /// Creates a bordered card surface. Compose with <c>CardHeader</c>, <c>CardMedia</c>, <c>CardContent</c>,
  /// <c>CardActionArea</c>, and <c>CardActions</c>.
  /// </summary>
  /// <param name="content">The Doc rendered inside the card.</param>
  /// <param name="attrs">Additional attributes (variant, spacing, etc.) merged onto the root element.</param>
  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty
    div [ cl Css.``weave-card``; yield! attrs ] [ content ]
