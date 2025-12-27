namespace Weave

open WebSharper
open WebSharper.UI
open Weave.CssHelpers

[<JavaScript>]
module Container =

  [<RequireQualifiedAccess; Struct>]
  type MaxWidth =
    | ExtraSmall
    | Small
    | Medium
    | Large
    | ExtraLarge
    | ExtraExtraLarge

  module MaxWidth =

    let toClass maxWidth =
      match maxWidth with
      | MaxWidth.ExtraSmall -> Css.``weave-container--maxwidth-xs``
      | MaxWidth.Small -> Css.``weave-container--maxwidth-sm``
      | MaxWidth.Medium -> Css.``weave-container--maxwidth-md``
      | MaxWidth.Large -> Css.``weave-container--maxwidth-lg``
      | MaxWidth.ExtraLarge -> Css.``weave-container--maxwidth-xl``
      | MaxWidth.ExtraExtraLarge -> Css.``weave-container--maxwidth-xxl``

open Container

[<JavaScript>]
type Container =

  static member Create
    (content: Doc, ?fixedWidth: bool, ?maxWidth: MaxWidth, ?gutters: bool, ?attrs: Attr list)
    =
    let fixedWidth = defaultArg fixedWidth false
    let maxWidth = defaultArg maxWidth MaxWidth.ExtraExtraLarge
    let gutters = defaultArg gutters true
    let attrs = defaultArg attrs List.empty

    let containerClasses = [
      Css.``weave-container``
      Css.``weave-container--fill-height``

      if fixedWidth then
        Css.``weave-container--fixed``
      else
        MaxWidth.toClass maxWidth

      if gutters then
        Css.``weave-container--gutters``
    ]

    div [ yield! containerClasses |> List.map cl; yield! attrs ] [ content ]
