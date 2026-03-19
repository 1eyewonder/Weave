namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Alert =

  module Variant =

    let standard = cl Css.``weave-alert--standard``
    let outlined = cl Css.``weave-alert--outlined``
    let filled = cl Css.``weave-alert--filled``

  module Color =

    let primary = cl Css.``weave-alert--primary``
    let secondary = cl Css.``weave-alert--secondary``
    let tertiary = cl Css.``weave-alert--tertiary``
    let error = cl Css.``weave-alert--error``
    let warning = cl Css.``weave-alert--warning``
    let success = cl Css.``weave-alert--success``
    let info = cl Css.``weave-alert--info``

[<JavaScript; RequireQualifiedAccess>]
type Alert =

  static member create(content: Doc, ?icon: Doc, ?onClose: unit -> unit, ?closeIcon: Doc, ?attrs: Attr list) =

    let attrs = defaultArg attrs List.empty

    let iconDoc =
      match icon with
      | Some i -> div [ cl Css.``weave-alert__icon`` ] [ i ]
      | None -> Doc.Empty

    let closeDoc =
      match onClose with
      | Some callback ->
        let closeIconDoc = defaultArg closeIcon (H6.span ("\u00D7"))

        IconButton.create (
          closeIconDoc,
          onClick = callback,
          attrs = [ Attr.Create "aria-label" "close alert"; cl Css.``weave-alert__close`` ]
        )
      | None -> Doc.Empty

    div [
      cl Css.``weave-alert``
      Attr.Create "role" "alert"
      Attr.Create "aria-live" "polite"
      yield! attrs
    ] [ iconDoc; div [ cl Css.``weave-alert__content`` ] [ content ]; closeDoc ]
