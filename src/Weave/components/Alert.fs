namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Alert =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Standard
    | Outlined
    | Filled

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Standard -> Css.``weave-alert--standard``
      | Variant.Outlined -> Css.``weave-alert--outlined``
      | Variant.Filled -> Css.``weave-alert--filled``

  [<RequireQualifiedAccess; Struct>]
  type AlertColor =
    | Default
    | BrandColor of color: BrandColor

  module AlertColor =

    let toClass color =
      match color with
      | AlertColor.Default -> None
      | AlertColor.BrandColor bc ->
        match bc with
        | BrandColor.Primary -> Some Css.``weave-alert--primary``
        | BrandColor.Secondary -> Some Css.``weave-alert--secondary``
        | BrandColor.Tertiary -> Some Css.``weave-alert--tertiary``
        | BrandColor.Error -> Some Css.``weave-alert--error``
        | BrandColor.Warning -> Some Css.``weave-alert--warning``
        | BrandColor.Success -> Some Css.``weave-alert--success``
        | BrandColor.Info -> Some Css.``weave-alert--info``

[<JavaScript>]
type Alert =

  static member Create(content: Doc, ?icon: Doc, ?onClose: unit -> unit, ?closeIcon: Doc, ?attrs: Attr list) =

    let attrs = defaultArg attrs List.empty

    let iconDoc =
      match icon with
      | Some i -> div [ cl Css.``weave-alert__icon`` ] [ i ]
      | None -> Doc.Empty

    let closeDoc =
      match onClose with
      | Some callback ->
        let closeIconDoc = defaultArg closeIcon (H6.Span("\u00D7"))

        Button.CreateIcon(
          closeIconDoc,
          onClick = callback,
          attrs = [ Attr.Create "aria-label" "close alert"; cl Css.``weave-alert__close`` ]
        )
      | None -> Doc.Empty

    div [ cl Css.``weave-alert``; yield! attrs ] [
      iconDoc
      div [ cl Css.``weave-alert__content`` ] [ content ]
      closeDoc
    ]
