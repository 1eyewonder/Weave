namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Container =

  module MaxWidth =

    let extraSmall = cl Css.``weave-container--maxwidth-xs``
    let small = cl Css.``weave-container--maxwidth-sm``
    let medium = cl Css.``weave-container--maxwidth-md``
    let large = cl Css.``weave-container--maxwidth-lg``
    let extraLarge = cl Css.``weave-container--maxwidth-xl``
    let extraExtraLarge = cl Css.``weave-container--maxwidth-xxl``

  let fixedWidth = cl Css.``weave-container--fixed``
  let gutters = cl Css.``weave-container--gutters``

[<JavaScript; RequireQualifiedAccess>]
type Container =

  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty

    div [
      cl Css.``weave-container``
      cl Css.``weave-container--fill-height``
      yield! attrs
    ] [ content ]
